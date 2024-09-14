using RecipeBook.Recipes;

namespace RecipeBook
{
    public partial class RecipePage : ContentPage
    {
        int count = 0;

        public RecipeList RecipeBook;

        public RecipePage(RecipeList recipeList)
        {
            InitializeComponent();

            if (RecipeBook == null)
            {
                RecipeBook = recipeList;
            }

            BindingContext = RecipeBook;
        }

        // This method is called when a recipe is selected from the CollectionView
        // This method is called when an item is tapped
        private async void OnRecipeTapped(object sender, EventArgs e)
        {
            // Get the selected recipe via the CommandParameter
            var tappedItem = (sender as Element)?.BindingContext as Recipe;

            if (tappedItem != null)
            {
                // Navigate to the RecipeDetailPage, passing the selected recipe
                Navigation.PushModalAsync(new ViewRecipe(tappedItem, RecipeBook));
                SaveHelper.SaveRecipeListToJson(RecipeBook);
            }
        }

        private async void btnAddClicked(object sender, EventArgs e)
        {
            Recipe newRecipe = new Recipe("New Recipe");
            RecipeBook.Recipes.Add(newRecipe);
            Navigation.PushModalAsync(new EditRecipePage(newRecipe, RecipeBook, "Add Recipe"));
            SaveHelper.SaveRecipeListToJson(RecipeBook);
        }

        private async void OnDeleteRecipeClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Recipe recipe)
            {
                bool isConfirmed = await DisplayAlert(
                    "Confirm Deletion",
                    $"Are you sure you want to delete this recipe? '{recipe.Name}' will be lost forever! (A long time!)",
                    "Yes",
                    "No");

                if (isConfirmed)
                {
                    // Remove the recipe if confirmed
                    (BindingContext as RecipeList)?.Recipes.Remove(recipe);
                    SaveHelper.SaveRecipeListToJson((BindingContext as RecipeList));
                }
            }
        }
    }
}


