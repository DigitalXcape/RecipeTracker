using Newtonsoft.Json;
using RecipeBook.Recipes;
using RecipeBook.Services;
using System.Collections.ObjectModel;

namespace RecipeBook
{
    public partial class MainPage : ContentPage
    {
        public RecipeList RecipeList { get; private set; }

        private readonly RecipeService recipeService;

        public MainPage(RecipeList recipeList, RecipeService recipeService)
        {
            InitializeComponent();

            this.recipeService = recipeService;

            RecipeList = recipeList;

            LoadRecipeList();
        }

        /// <summary>
        /// Load the recipe list if available
        /// </summary>
        private void LoadRecipeList()
        {
            // Try to load the RecipeList from JSON
            var loadedRecipeList = SaveHelper.LoadRecipeListFromJson();

            // If loading returns null or an empty list, create a new one
            if (loadedRecipeList == null || loadedRecipeList.Recipes.Count == 0)
            {
                loadedRecipeList = new RecipeList();
            }

            // Set the loaded RecipeList to the singleton
            RecipeList = loadedRecipeList;
            RecipeList.recipeService = recipeService;

            // App.Current.Services.GetRequiredService<RecipeList>() = loadedRecipeList;
        }

        /// <summary>
        /// Click event for the recipe list button
        /// </summary>
        private async void btnYourRecipesClicked(object sender, EventArgs e)
        {
            var recipePage = new RecipePage(RecipeList);
            await Navigation.PushAsync(recipePage);
        }

        private async void btnYourFavoriteRecipesClicked(object sender, EventArgs e)
        {
            var favoritePage = new FavoritesPage(RecipeList);
            await Navigation.PushAsync(favoritePage);
        }

        private async void btnGetRecipesOnlineClicked(object sender, EventArgs e)
        {
            RecipeList.GetRecipesFromDatabase();
        }

        private void OnThemeSelected(object sender, EventArgs e)
        {
            // Get selected theme from Picker
            var selectedTheme = ThemePicker.SelectedItem.ToString();

            // Call SetTheme method from App.xaml.cs to apply the selected theme globally
            (Application.Current as App).SetTheme(selectedTheme);
        }
    }
}
