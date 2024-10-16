using RecipeBook.Recipes;
using System.Collections.ObjectModel;

namespace RecipeBook
{
    public partial class RecipePage : ContentPage
    {
        int count = 0;

        public RecipeList RecipeList;

        public RecipePage(RecipeList recipeList)
        {
            InitializeComponent();

            if (RecipeList == null)
            {
                RecipeList = recipeList;
            }

            BindingContext = RecipeList;
        }

        private async void OnRecipeTapped(object sender, EventArgs e)
        {
            // Get the selected recipe via the CommandParameter
            var tappedItem = (sender as Element)?.BindingContext as Recipe;

            if (tappedItem != null)
            {
                // Create an instance of ViewRecipePage, passing the tapped recipe and RecipeBook
                var viewRecipePage = (ViewRecipePage)Activator.CreateInstance(typeof(ViewRecipePage), tappedItem, RecipeList);

                // Navigate to the ViewRecipePage
                await Navigation.PushAsync(viewRecipePage);
            }
        }

        private void OnSwipeEnded(object sender, SwipeEndedEventArgs e)
        {
            // Check if the swipe direction was right
            if (e.SwipeDirection == SwipeDirection.Left)
            {
                var swipeView = (SwipeView)sender;
                var recipe = (Recipe)swipeView.BindingContext;

                if (!recipe.Favorite)
                {
                    recipe.Favorite = true;
                }
                else
                {
                    recipe.Favorite = false;
                }

                SaveHelper.SaveRecipeListToJson(RecipeList);
                swipeView.Close();
            }
        }

        private async void btnAddClicked(object sender, EventArgs e)
        {
            // Create a new recipe
            Recipe newRecipe = new Recipe("New Recipe");

            // Add the new recipe to the RecipeList
            RecipeList.Recipes.Add(newRecipe);

            // Resolve the EditRecipePage from the service provider
            var editRecipePage = new EditRecipePage(newRecipe, RecipeList, "Add New Recipe");

            // Navigate to the EditRecipePage
            await Navigation.PushModalAsync(editRecipePage);

            // Save the updated RecipeList to JSON
            SaveHelper.SaveRecipeListToJson(RecipeList);
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


