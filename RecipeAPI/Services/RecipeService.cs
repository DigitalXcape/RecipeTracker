using RecipeAPI.Models;
using RecipeAPI.Repositories;

namespace RecipeAPI.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly string recipesFilePath = "Data/recipes.json";
        private readonly string favoritesFilePath = "Data/favoriteRecipes.json";

        private readonly IRecipeRepository recipeRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ILogger<RecipeService> logger;

        public RecipeService(IRecipeRepository recipeRepository, ILogger<RecipeService> logger)
        {
            this.recipeRepository = recipeRepository;
            this.categoryRepository = new CategoryRepository();
            this.logger = logger;
        }

        public async Task<IEnumerable<Recipe>> GetRecipesAsync()
        {
            try
            {
                return await recipeRepository.GetRecipesAsync(recipesFilePath);
            }
            catch (Exception ex)
            {
                logger.LogError("Error retriving recipe", ex);
                throw;
            }
        }

        public async Task<IEnumerable<Recipe>> GetFavoriteRecipesAsync()
        {
            try
            {
                return await recipeRepository.GetRecipesAsync(favoritesFilePath);
            }
            catch (Exception ex)
            {
                logger.LogError("Error retriving recipe", ex);
                throw;
            }
        }

        public async Task AddRecipeAsync(Recipe recipe)
        {
            if (recipe == null)
            {
                logger.LogInformation("Adding recipe failed");
                throw new ArgumentNullException(nameof(recipe), "Recipe cannot be null");
            }
            logger.LogInformation("Adding recipe with id");
            await recipeRepository.AddRecipeAsync(recipe, recipesFilePath);
        }

        public async Task AddFavoriteRecipeAsync(Recipe recipe)
        {
            if (recipe == null)
            {
                logger.LogInformation("Adding recipe failed");
                throw new ArgumentNullException(nameof(recipe), "Recipe cannot be null");
            }
            logger.LogInformation("Adding recipe with id");
            await recipeRepository.AddRecipeAsync(recipe, favoritesFilePath);
        }

        public async Task DeleteRecipeAsync(Guid id)
        {
            try
            {
                // Check id to make sure it's not empty
                if (id == Guid.Empty)
                {
                    logger.LogInformation("Invalid id {0}", id);
                    throw new ArgumentNullException(nameof(id), "Id cannot be empty");
                }

                // Call the repository to delete the coffee and await the task
                await recipeRepository.DeleteRecipeAsync(id, recipesFilePath);
                logger.LogInformation("Deleted recipe with id {0}", id);
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogError(ex, "Recipe with id {0} not found", id);
                throw new Exception(ex.Message, ex);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while deleting recipe with id {0}", id);
                throw new Exception("An error occurred while deleting the recipe", ex);
            }
        }

        public async Task DeleteFavoriteRecipeAsync(Guid id)
        {
            try
            {
                // Check id to make sure it's not empty
                if (id == Guid.Empty)
                {
                    logger.LogInformation("Invalid id {0}", id);
                    throw new ArgumentNullException(nameof(id), "Id cannot be empty");
                }

                // Call the repository to delete the coffee and await the task
                await recipeRepository.DeleteRecipeAsync(id, favoritesFilePath);
                logger.LogInformation("Deleted recipe with id {0}", id);
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogError(ex, "Recipe with id {0} not found", id);
                throw new Exception(ex.Message, ex);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while deleting recipe with id {0}", id);
                throw new Exception("An error occurred while deleting the recipe", ex);
            }
        }

        public async Task<Recipe> GetRecipeByIdAsync(Guid id)
        {
            try
            {
                return await recipeRepository.GetRecipeByIdAsync(id, recipesFilePath);
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogError("Recipe with id {0} not found", id, ex);
                throw new Exception(ex.Message, ex);
            }

            catch (Exception ex)
            {
                logger.LogError("Error while retrieving recipe with id {0}", id, ex);
                throw new Exception("An error occurred while retrieving the recipe by id async", ex);
            }
        }

        public async Task<Recipe> GetFavoriteRecipeByIdAsync(Guid id)
        {
            try
            {
                return await recipeRepository.GetRecipeByIdAsync(id, favoritesFilePath);
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogError("Recipe with id {0} not found", id, ex);
                throw new Exception(ex.Message, ex);
            }

            catch (Exception ex)
            {
                logger.LogError("Error while retrieving recipe with id {0}", id, ex);
                throw new Exception("An error occurred while retrieving the recipe by id async", ex);
            }
        }

        public async Task UpdateRecipeAsync(Recipe recipe)
        {
            if (recipe == null)
            {
                logger.LogWarning("Recipe object is null in RecipeService.UpdateRecipeAsync.");
                throw new ArgumentNullException(nameof(recipe), "Recipe object cannot be null.");
            }

            if (recipe.Id == default)
            {
                logger.LogWarning("Recipe object has an invalid Id in RecipeService.UpdateRecipeAsync.");
                throw new ArgumentException("Recipe object must have a valid Id.", nameof(recipe));
            }

            try
            {
                // Verify existence
                var existingRecipe = await recipeRepository.GetRecipeByIdAsync(recipe.Id, recipesFilePath);
                if (existingRecipe == null)
                {
                    logger.LogError("Recipe with Id {RecipeId} not found.", recipe.Id);
                    throw new KeyNotFoundException($"Recipe with Id {recipe.Id} not found.");
                }

                await recipeRepository.UpdateRecipeAsync(recipe, recipesFilePath);
                logger.LogInformation("Successfully updated recipe with Id {RecipeId}.", recipe.Id);
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogError(ex, "Failed to update recipe because it was not found. Id: {RecipeId}", recipe.Id);
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while updating recipe with Id {RecipeId}.", recipe.Id);
                throw;
            }
        }

        public async Task FavoriteRecipeByIdAsync(Guid id)
        {
            try
            {
                var existingRecipe = await recipeRepository.GetRecipeByIdAsync(id, recipesFilePath);

                if (existingRecipe == null)
                {
                    logger.LogError("The recipe you were looking for was not found");
                    throw new KeyNotFoundException($"Recipe with Id {id} not found.");
                }

                await recipeRepository.AddRecipeAsync(existingRecipe, favoritesFilePath);
            }
            catch (KeyNotFoundException ex)
            {
                throw;
            }
        }

        public async Task RemoveFavoriteRecipeByIdAsync(Guid id)
        {
            try
            {
                await recipeRepository.DeleteRecipeAsync(id, favoritesFilePath);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        // Get all categories
        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            try
            {
                return await categoryRepository.GetCategoriesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError("Error retrieving categories", ex);
                throw;
            }
        }

        // Get category by ID
        public async Task<Category> GetCategoryByIdAsync(Guid id)
        {
            try
            {
                return await categoryRepository.GetCategoryByIdAsync(id);
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogError("Category with id {0} not found", id, ex);
                throw new Exception(ex.Message, ex);
            }
            catch (Exception ex)
            {
                logger.LogError("Error while retrieving category with id {0}", id, ex);
                throw new Exception("An error occurred while retrieving the category by id async", ex);
            }
        }

        // Add a new category
        public async Task AddCategoryAsync(Category category)
        {
            if (category == null)
            {
                logger.LogInformation("Adding category failed");
                throw new ArgumentNullException(nameof(category), "Category cannot be null");
            }
            logger.LogInformation("Adding category with id {0}", category.Id);
            await categoryRepository.AddCategoryAsync(category);
        }

        // Remove a category by ID
        public async Task RemoveCategoryAsync(Guid id)
        {
            try
            {
                // Check id to make sure it's not empty
                if (id == Guid.Empty)
                {
                    logger.LogInformation("Invalid id {0}", id);
                    throw new ArgumentNullException(nameof(id), "Id cannot be empty");
                }

                // Call the repository to delete the category and await the task
                await categoryRepository.RemoveCategoryAsync(id);
                logger.LogInformation("Deleted category with id {0}", id);
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogError(ex, "Category with id {0} not found", id);
                throw new Exception(ex.Message, ex);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while deleting category with id {0}", id);
                throw new Exception("An error occurred while deleting the category", ex);
            }
        }

        // Update a category
        public async Task UpdateCategoryAsync(Category category)
        {
            if (category == null)
            {
                logger.LogWarning("Category object is null in RecipeService.UpdateCategoryAsync.");
                throw new ArgumentNullException(nameof(category), "Category object cannot be null.");
            }

            if (category.Id == default)
            {
                logger.LogWarning("Category object has an invalid Id in RecipeService.UpdateCategoryAsync.");
                throw new ArgumentException("Category object must have a valid Id.", nameof(category));
            }

            try
            {
                // Verify existence
                var existingCategory = await categoryRepository.GetCategoryByIdAsync(category.Id);
                if (existingCategory == null)
                {
                    logger.LogError("Category with Id {CategoryId} not found.", category.Id);
                    throw new KeyNotFoundException($"Category with Id {category.Id} not found.");
                }

                await categoryRepository.SaveCategoriesAsync(new List<Category> { category }); // Update logic may vary
                logger.LogInformation("Successfully updated category with Id {CategoryId}.", category.Id);
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogError(ex, "Failed to update category because it was not found. Id: {CategoryId}", category.Id);
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while updating category with Id {CategoryId}.", category.Id);
                throw;
            }
        }
    }
}
