using Newtonsoft.Json;
using RecipeBook.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace RecipeBook.Recipes
{
    [System.Serializable]
    public class RecipeList : INotifyPropertyChanged
    {
        public string Theme;

        private ObservableCollection<Recipe> _recipes;

        public List<Recipe> StoredRecipes {  get; private set; }

        public RecipeService recipeService;

        private readonly string baseUrl = "https://your-api-url.com";

        public ObservableCollection<Recipe> Recipes
        {
            get => _recipes;
            set
            {
                _recipes = value;
                OnPropertyChanged(nameof(Recipes));
            }
        }

        public ObservableCollection<Recipe> FavoriteRecipes
        {
            get
            {
                ObservableCollection<Recipe> result = new ObservableCollection<Recipe>();

                foreach (Recipe item in _recipes)
                {
                    if (item.Favorite)
                    {
                        result.Add(item);
                    }
                }

                OnPropertyChanged(nameof(FavoriteRecipes));

                return result;
            }
        }

        public ICommand SelectRecipeCommand { get; private set; }

        public RecipeList()
        {
            Recipes = new ObservableCollection<Recipe>
            {
                //input hard coded elements here
            };

            SelectRecipeCommand = new Command<Recipe>(OnRecipeSelected);
            Theme = "Light";
        }

        /// <summary>
        /// Navigate to a ViewRecipe page with the selected recipe
        /// </summary>
        /// <param name="recipe"></param>
        private void OnRecipeSelected(Recipe recipe)
        {
            Application.Current.MainPage.Navigation.PushAsync(new ViewRecipePage(recipe, this));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void GetRecipesFromDatabase()
        {
            try
            {
                string recipesJson = await recipeService.GetAllRecipesAsync();

                var jsonData = JsonConvert.DeserializeObject<List<dynamic>>(recipesJson);

                this.Recipes.Clear();

                foreach (var recipeData in jsonData)
                {
                    var recipe = new Recipe((string)recipeData.name)
                    {
                        Description = (string)recipeData.description,
                        Id = (string)recipeData.id,
                    };

                    foreach (var ingredient in recipeData.ingredients)
                    {
                        recipe.Ingredients.Add(new Ingredient((string)ingredient));
                    }

                    foreach (var instruction in recipeData.instructions)
                    {
                        recipe.Instructions.Add(new Instruction((string)instruction));
                    }

                    this.Recipes.Add(recipe);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("There was an error getting all recipes: " + ex.Message, ex);
            }
        }

        public async void DeleteRecipeFromDatabase(string id)
        {
            try
            {
                await recipeService.DeleteRecipe(id);

                var recipeToRemove = this.Recipes.FirstOrDefault(r => r.Id == id);
                if (recipeToRemove != null)
                {
                    this.Recipes.Remove(recipeToRemove);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("There was an error deleting the recipe: " + ex.Message, ex);
            }
        }

        public async void AddRecipeToDatabase(Recipe recipe)
        {
            try
            {
                var recipeJson = JsonConvert.SerializeObject(new
                {
                    Id = recipe.Id,
                    Name = recipe.Name,
                    Description = recipe.Description,
                    TimeToMake = 30.0,
                    ImageURL = "https://example.com/image-placeholder.jpg",
                    Ingredients = recipe.Ingredients.Select(i => i.Name).ToList(),
                    Instructions = recipe.Instructions.Select(i => i.Direction).ToList()
                });

                await recipeService.AddRecipe(recipeJson);

                this.Recipes.Add(recipe);
            }
            catch (Exception ex)
            {
                throw new Exception("There was an error adding the recipe: " + ex.Message, ex);
            }
        }

        public async void UpdateRecipeInDatabase(Recipe recipe)
        {
            try
            {
                var recipeJson = JsonConvert.SerializeObject(new
                {
                    Id = recipe.Id,
                    Name = recipe.Name,
                    Description = recipe.Description,
                    TimeToMake = 30.0,
                    ImageURL = "https://example.com/image-placeholder.jpg",
                    Ingredients = recipe.Ingredients.Select(i => i.Name).ToList(),
                    Instructions = recipe.Instructions.Select(i => i.Direction).ToList()
                });

                // Send the JSON to the API
                await recipeService.UpdateRecipe(recipeJson, recipe.Id);

                // Update the local collection
                var existingRecipe = this.Recipes.FirstOrDefault(r => r.Id == recipe.Id);
                if (existingRecipe != null)
                {
                    existingRecipe.Name = recipe.Name;
                    existingRecipe.Description = recipe.Description;
                    existingRecipe.Ingredients = recipe.Ingredients;
                    existingRecipe.Instructions = recipe.Instructions;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("There was an error updating the recipe: " + ex.Message, ex);
            }
        }
    }
}
