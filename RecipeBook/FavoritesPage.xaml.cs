using RecipeBook.Recipes;

namespace RecipeBook;

public partial class FavoritesPage : ContentPage
{
    public RecipeList RecipeBook;

    public FavoritesPage(RecipeList recipeList)
    {
        InitializeComponent();

        RecipeBook = recipeList;

        BindingContext = recipeList.FavoriteRecipes;
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
}