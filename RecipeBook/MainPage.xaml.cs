using Newtonsoft.Json;
using RecipeBook.Recipes;
using RecipeBook.Services;

namespace RecipeBook
{
    public partial class MainPage : ContentPage
    {
        public RecipeList RecipeList { get; private set; }

        private readonly RecipeService recipeService;

        public MainPage(RecipeList recipeList, RecipeService recipeService)
        {
            InitializeComponent();

            RecipeList = recipeList;

            this.recipeService = recipeService;

            LoadRecipeList();
        }

        /// <summary>
        /// Load the recipe list if available
        /// </summary>
        private void LoadRecipeList()
        {
            // Try to load the RecipeList from JSON
            RecipeList = SaveHelper.LoadRecipeListFromJson();

            // If loading returns null or an empty list, create a new one
            if (RecipeList == null || RecipeList.Recipes.Count == 0)
            {
                RecipeList = new RecipeList();
            }
        }

        /// <summary>
        /// Click event for the recipe list button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnYourRecipesClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//RecipePage");
        }

        private async void btnYourFavoriteRecipesClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//FavoritesPage");
        }

        private async void btnGetRecipesOnlineClicked(object sender, EventArgs e)
        {
            try
            {
                string recipesJson = await recipeService.GetAllRecipesAsync();

                List<Recipe> recipes = JsonConvert.DeserializeObject<List<Recipe>>(recipesJson);

                RecipeList.Recipes.Clear();
                foreach (var recipe in recipes)
                {
                    RecipeList.Recipes.Add(recipe);
                }

                await DisplayAlert("Success", "Recipes loaded successfully!", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load recipes: {ex.Message}", "OK");
            }
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
