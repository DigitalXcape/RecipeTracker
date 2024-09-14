using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RecipeBook.Recipes
{
    public static class SaveHelper
    {
        /// <summary>
        /// Save a recipe list to Json
        /// </summary>
        /// <param name="recipeList"></param>
        public static void SaveRecipeListToJson(RecipeList recipeList)
        {

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RecipeData.json");
            Console.Write(filePath);

            string json = JsonSerializer.Serialize(recipeList, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Load a recipe list from json
        /// </summary>
        /// <returns></returns>
        public static RecipeList LoadRecipeListFromJson()
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RecipeData.json");

            if (!File.Exists(filePath))
            {
                // If the file does not exist, return a new RecipeList
                return new RecipeList();
            }

            // Read JSON content from the file
            string json = File.ReadAllText(filePath);

            RecipeList recipeList;

            try
            {
                recipeList = JsonSerializer.Deserialize<RecipeList>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });

                if (recipeList == null)
                {
                    return new RecipeList();
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing JSON: {ex.Message}");
                recipeList = new RecipeList();
            }

            return recipeList;
        }
    }
}

