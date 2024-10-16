using Newtonsoft.Json;
using RecipeAPI.Models;

namespace RecipeAPI.Repositories
{
    public class JsonRecipeRepository : IRecipeRepository
    {

        public JsonRecipeRepository()
        {

        }

        public async Task<IEnumerable<Recipe>> GetRecipesAsync(string filePath)
        {
            try
            {
                var recipeJson = await File.ReadAllTextAsync(filePath);
                var recipes = JsonConvert.DeserializeObject<List<Recipe>>(recipeJson);
                return recipes;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to read from {filePath}", ex);
            }
        }

        public async Task AddRecipeAsync(Recipe recipe, string filePath)
        {
            try
            {
                var recipes = (await GetRecipesAsync(filePath)).ToList();
                recipes.Add(recipe);
                await SaveRecipesAsync(recipes, filePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding recipe to {filePath}", ex);
            }
        }

        public async Task DeleteRecipeAsync(Guid id, string filePath)
        {
            try
            {
                var recipes = (await GetRecipesAsync(filePath)).ToList();
                var recipe = recipes.FirstOrDefault(c => c.Id == id);

                if (recipe == null)
                {
                    throw new Exception($"Could not find recipe with id {id} in {filePath}");
                }

                recipes.Remove(recipe);
                await SaveRecipesAsync(recipes, filePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not delete recipe from {filePath}", ex);
            }
        }

        public async Task<Recipe> GetRecipeByIdAsync(Guid id, string filePath)
        {
            try
            {
                var recipes = await GetRecipesAsync(filePath);
                var recipe = recipes.FirstOrDefault(c => c.Id == id);

                if (recipe == null)
                {
                    throw new Exception($"Could not find recipe with id {id} in {filePath}");
                }

                return recipe;
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not read file at {filePath}", ex);
            }
        }

        public async Task UpdateRecipeAsync(Recipe recipe, string filePath)
        {
            try
            {
                var recipes = (await GetRecipesAsync(filePath)).ToList();
                var existingRecipe = recipes.FirstOrDefault(c => c.Id == recipe.Id);
                if (existingRecipe == null)
                {
                    throw new Exception($"Could not find recipe with id {recipe.Id} in {filePath}");
                }
                else
                {
                    recipes.Remove(existingRecipe);
                    recipes.Add(recipe);
                    await SaveRecipesAsync(recipes, filePath);
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Could not read file at {filePath}", ex);
            }
        }

        private async Task SaveRecipesAsync(List<Recipe> recipes, string filePath)
        {
            try
            {
                var recipeJson = JsonConvert.SerializeObject(recipes, Formatting.Indented);
                await File.WriteAllTextAsync(filePath, recipeJson);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to save to {filePath}", ex);
            }
        }
    }
}
