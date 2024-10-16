using RecipeAPI.Models;

namespace RecipeAPI.Services
{
    public interface IRecipeService
    {
        // Recipe Methods
        Task<IEnumerable<Recipe>> GetRecipesAsync();
        Task<Recipe> GetRecipeByIdAsync(Guid id);
        Task AddRecipeAsync(Recipe recipe);
        Task DeleteRecipeAsync(Guid id);
        Task UpdateRecipeAsync(Recipe recipe);
        Task FavoriteRecipeByIdAsync(Guid id);
        Task RemoveFavoriteRecipeByIdAsync(Guid id);

        // Category Methods
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(Guid id);
        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task RemoveCategoryAsync(Guid id);
    }
}
