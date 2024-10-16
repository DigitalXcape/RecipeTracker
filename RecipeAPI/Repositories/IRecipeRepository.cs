using RecipeAPI.Models;

namespace RecipeAPI.Repositories
{
    public interface IRecipeRepository
    {
        Task<IEnumerable<Recipe>> GetRecipesAsync(string filepath);

        Task<Recipe> GetRecipeByIdAsync(Guid id, string filepath);

        Task AddRecipeAsync(Recipe recipe, string filepath);

        Task DeleteRecipeAsync(Guid id, string filepath);

        Task UpdateRecipeAsync(Recipe recipe, string filepath);
    }
}
