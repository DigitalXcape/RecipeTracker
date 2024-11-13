using RecipeAPI.Models;

namespace RecipeAPI.Repositories
{
    public interface IRecipeRepository
    {
        Task<IEnumerable<Recipe>> GetRecipesAsync();

        Task<Recipe> GetRecipeByIdAsync(Guid id);

        Task AddRecipeAsync(Recipe recipe);

        Task DeleteRecipeAsync(Guid id);

        Task UpdateRecipeAsync(Recipe recipe);
    }
}
