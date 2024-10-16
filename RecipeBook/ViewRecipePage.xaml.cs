using RecipeBook.Recipes;
namespace RecipeBook;

public partial class ViewRecipePage : ContentPage
{
    public RecipeList RecipeBook;
    public Recipe Recipe;

    public ViewRecipePage(Recipe selectedRecipe, RecipeList recipeBook)
    {
        InitializeComponent();

        // Set the selected recipe as the BindingContext for this page
        BindingContext = selectedRecipe;

        RecipeBook = recipeBook;
        Recipe = selectedRecipe;
    }

    private async void btnEditClicked(object sender, EventArgs e)
    {
        // Create an instance of EditRecipePage with the current Recipe and RecipeBook
        var editRecipePage = (EditRecipePage)Activator.CreateInstance(typeof(EditRecipePage), Recipe, RecipeBook, "Edit Recipe");

        // Navigate to the EditRecipePage
        await Navigation.PushAsync(editRecipePage);
    }
}