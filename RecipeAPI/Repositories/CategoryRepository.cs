using Newtonsoft.Json;
using RecipeAPI.Models;

namespace RecipeAPI.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string fileName = "Data/categories.json";

        public CategoryRepository()
        {
            if (!File.Exists(fileName))
            {
                File.WriteAllText(fileName, "[]");
            }
        }

        // Get Category by Id
        public async Task<Category> GetCategoryByIdAsync(Guid id)
        {
            try
            {
                var categories = await GetCategoriesAsync();
                var category = categories.FirstOrDefault(c => c.Id == id);

                if (category == null)
                {
                    throw new Exception($"Category with id {id} not found.");
                }

                return category;
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading category file", ex);
            }
        }

        // Add a new category
        public async Task AddCategoryAsync(Category category)
        {
            try
            {
                var categories = (await GetCategoriesAsync()).ToList();
                categories.Add(category);
                await SaveCategoriesAsync(categories);
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding category", ex);
            }
        }

        // Remove a category
        public async Task RemoveCategoryAsync(Guid id)
        {
            try
            {
                var categories = (await GetCategoriesAsync()).ToList();
                var category = categories.FirstOrDefault(c => c.Id == id);

                if (category == null)
                {
                    throw new Exception($"Category with id {id} not found.");
                }

                categories.Remove(category);
                await SaveCategoriesAsync(categories);
            }
            catch (Exception ex)
            {
                throw new Exception("Error removing category", ex);
            }
        }

        // Retrieve all categories from the file
        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            try
            {
                var categoryJson = await File.ReadAllTextAsync(fileName);
                var categories = JsonConvert.DeserializeObject<List<Category>>(categoryJson);
                return categories;
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading categories file", ex);
            }
        }

        // Save categories to the file
        public async Task SaveCategoriesAsync(List<Category> categories)
        {
            try
            {
                var categoryJson = JsonConvert.SerializeObject(categories, Formatting.Indented);
                await File.WriteAllTextAsync(fileName, categoryJson);
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving categories", ex);
            }
        }
    }
}
