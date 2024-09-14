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
        private async void btnRecipeClicked(object sender, EventArgs e)
        {
            // Pass the loaded or newly created RecipeList to the RecipePage
            await Navigation.PushModalAsync(new RecipePage(RecipeList));
        }
    }

}
