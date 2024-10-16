using RecipeAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeAPI.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> GetCategoryByIdAsync(Guid id);

        Task AddCategoryAsync(Category category);

        Task RemoveCategoryAsync(Guid id);

        Task<IEnumerable<Category>> GetCategoriesAsync();

        Task SaveCategoriesAsync(List<Category> categories);
    }
}
