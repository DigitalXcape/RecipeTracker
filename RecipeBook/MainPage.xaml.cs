using RecipeBook.Recipes;

namespace RecipeBook
{
    public partial class MainPage : ContentPage
    {
        public RecipeList RecipeList { get; private set; }

        public MainPage()
        {
            InitializeComponent();
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
            // Pass the loaded or newly created RecipeList to the RecipePage
            await Navigation.PushModalAsync(new RecipePage(RecipeList));
        }

        private async void btnYourFavoriteRecipesClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new FavoritesPage(RecipeList));
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
