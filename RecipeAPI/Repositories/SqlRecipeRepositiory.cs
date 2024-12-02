using Microsoft.Data.SqlClient;
using RecipeAPI.Models;
using System.Text.Json;

namespace RecipeAPI.Repositories
{
    public class SqlRecipeRepository : IRecipeRepository
    {
        private readonly string connectionString;
        private readonly ILogger<SqlRecipeRepository> logger;

        public SqlRecipeRepository(IConfiguration configuration, ILogger<SqlRecipeRepository> logger)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
            this.logger = logger;
        }

        public async Task SeedDataAsync()
        {
            try
            {
                await EnsureDatabaseAsync();

                // Ensure the Recipes table exists
                await EnsureTableExistsAsync();

                var jsonData = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Data", "recipes.json"));
                var recipes = JsonSerializer.Deserialize<List<Recipe>>(jsonData);

                if (recipes == null || recipes.Count == 0)
                {
                    logger.LogWarning("No recipe data found");
                    return;
                }

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    foreach (var recipe in recipes)
                    {
                        var checkQuery = "SELECT COUNT(1) FROM Recipes WHERE Id = @Id";

                        using (var checkCommand = new SqlCommand(checkQuery, connection))
                        {
                            checkCommand.Parameters.AddWithValue("@Id", recipe.Id);

                            var exist = (int)await checkCommand.ExecuteScalarAsync() > 0;

                            if (exist)
                            {
                                logger.LogInformation($"Recipe with ID {recipe.Id} already exists..skipping");
                                continue;
                            }
                        }

                        var insertQuery = @"INSERT INTO Recipes (Id, Name, Description, TimeToMake, Ingredients, Instructions)
                                    VALUES (@Id, @Name, @Description, @TimeToMake, @Ingredients, @Instructions)";

                        using (var insertCommand = new SqlCommand(insertQuery, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@Id", recipe.Id);
                            insertCommand.Parameters.AddWithValue("@Name", recipe.Name);
                            insertCommand.Parameters.AddWithValue("@Description", recipe.Description);
                            insertCommand.Parameters.AddWithValue("@TimeToMake", recipe.TimeToMake);


                            insertCommand.Parameters.AddWithValue("@Ingredients", JsonSerializer.Serialize(recipe.Ingredients));
                            insertCommand.Parameters.AddWithValue("@Instructions", JsonSerializer.Serialize(recipe.Instructions));

                            await insertCommand.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error: " + ex.Message);
                throw;
            }
        }

        private async Task EnsureTableExistsAsync()
        {
            var createTableQuery = @"
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Recipes')
    BEGIN
        CREATE TABLE Recipes (
            Id UNIQUEIDENTIFIER PRIMARY KEY,
            Name NVARCHAR(100),
            Description NVARCHAR(255),
            TimeToMake DECIMAL(10, 2),
            Ingredients NVARCHAR(MAX), -- Storing as a JSON string
            Instructions NVARCHAR(MAX) -- Storing as a JSON string
        );
    END";

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(createTableQuery, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task EnsureDatabaseAsync()
        {
            var masterConnectionString = new SqlConnectionStringBuilder(connectionString)
            {
                InitialCatalog = "master"
            }.ConnectionString;

            using (var connection = new SqlConnection(masterConnectionString))
            {
                try
                {
                    await connection.OpenAsync();
                }
                catch (Exception e)
                {
                    logger.LogError(e.Message);
                }

                var checkDbQuery = "SELECT COUNT(*) FROM sys.databases WHERE name = 'RecipeDB'";
                using (var command = new SqlCommand(checkDbQuery, connection))
                {
                    var dbExists = (int)await command.ExecuteScalarAsync() > 0;
                    if (!dbExists)
                    {
                        logger.LogInformation("Database 'RecipeDB' does not exist. Creating database.");
                        var createDbQuery = "CREATE DATABASE RecipeDB";
                        using (var createDbCommand = new SqlCommand(createDbQuery, connection))
                        {
                            await createDbCommand.ExecuteNonQueryAsync();
                        }
                    }
                    else
                    {
                        logger.LogInformation("Database 'RecipeDB' already exists.");
                    }
                }
            }
        }

        public async Task DeleteRecipeAsync(Guid id)
        {
            var query = "DELETE FROM Recipes WHERE Id = @Id";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                try
                {
                    await connection.OpenAsync();
                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                    {
                        throw new KeyNotFoundException($"Recipe not found with id {id}");
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error deleting recipe");
                    throw new Exception("Error deleting recipe");
                }
            }
        }

        public async Task<Recipe> GetRecipeByIdAsync(Guid id)
        {
            var query = "SELECT * FROM Recipes WHERE Id = @Id";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                try
                {
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Recipe
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                Name = reader["Name"] as string,
                                Description = reader["Description"] as string,
                                TimeToMake = (decimal)reader["TimeToMake"],
                                Ingredients = JsonSerializer.Deserialize<List<string>>(reader["Ingredients"].ToString()),
                                Instructions = JsonSerializer.Deserialize<List<string>>(reader["Instructions"].ToString())
                            };
                        }
                        else
                        {
                            throw new KeyNotFoundException("Recipe not found");
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error getting recipe");
                    throw new Exception("Error getting recipe", ex);
                }
            }
        }

        public async Task<IEnumerable<Recipe>> GetRecipesAsync()
        {
            List<Recipe> recipes = new List<Recipe>();
            var query = "SELECT * FROM Recipes";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                try
                {
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            recipes.Add(new Recipe
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                Name = reader["Name"] as string,
                                Description = reader["Description"] as string,
                                TimeToMake = (decimal)reader["TimeToMake"],
                                Ingredients = JsonSerializer.Deserialize<List<string>>(reader["Ingredients"].ToString()),
                                Instructions = JsonSerializer.Deserialize<List<string>>(reader["Instructions"].ToString())
                            });
                        }
                    }
                    return recipes;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error getting all recipes");
                    throw new Exception("Error getting all recipes");
                }
            }
        }

        public async Task AddRecipeAsync(Recipe recipe)
        {
            var query = @"INSERT INTO Recipes (Id, Name, Description, TimeToMake, Ingredients, Instructions)
                  VALUES (@Id, @Name, @Description, @TimeToMake, @Ingredients, @Instructions)";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", recipe.Id);
                command.Parameters.AddWithValue("@Name", recipe.Name);
                command.Parameters.AddWithValue("@Description", recipe.Description);
                command.Parameters.AddWithValue("@TimeToMake", recipe.TimeToMake);
                command.Parameters.AddWithValue("@Ingredients", JsonSerializer.Serialize(recipe.Ingredients));
                command.Parameters.AddWithValue("@Instructions", JsonSerializer.Serialize(recipe.Instructions));

                try
                {
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                    logger.LogInformation($"Recipe with ID {recipe.Id} added successfully.");
                }
                catch (SqlException ex) when (ex.Number == 2627) // Unique constraint violation
                {
                    logger.LogWarning($"Recipe with ID {recipe.Id} already exists.");
                    throw new InvalidOperationException($"A recipe with the same ID ({recipe.Id}) already exists.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error adding recipe");
                    throw new Exception("Error adding recipe", ex);
                }
            }
        }

        public async Task UpdateRecipeAsync(Recipe recipe)
        {
            var query = @"UPDATE Recipes
                  SET Name = @Name,
                      Description = @Description,
                      TimeToMake = @TimeToMake,
                      Ingredients = @Ingredients,
                      Instructions = @Instructions
                  WHERE Id = @Id";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", recipe.Id);
                command.Parameters.AddWithValue("@Name", recipe.Name);
                command.Parameters.AddWithValue("@Description", recipe.Description);
                command.Parameters.AddWithValue("@TimeToMake", recipe.TimeToMake);
                command.Parameters.AddWithValue("@Ingredients", JsonSerializer.Serialize(recipe.Ingredients));
                command.Parameters.AddWithValue("@Instructions", JsonSerializer.Serialize(recipe.Instructions));

                try
                {
                    await connection.OpenAsync();
                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                    {
                        throw new KeyNotFoundException($"Recipe with ID {recipe.Id} not found.");
                    }
                    logger.LogInformation($"Recipe with ID {recipe.Id} updated successfully.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error updating recipe");
                    throw new Exception("Error updating recipe", ex);
                }
            }
        }
    }
}
