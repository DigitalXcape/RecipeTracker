using RecipeBook.Recipes;
namespace RecipeBook;

public partial class ViewRecipe : ContentPage
{
    public RecipeList RecipeBook;
    public Recipe Recipe;

    public ViewRecipe(Recipe selectedRecipe, RecipeList recipeBook)
    {
        InitializeComponent();

        // Set the selected recipe as the BindingContext for this page
        BindingContext = selectedRecipe;

        RecipeBook = recipeBook;
        Recipe = selectedRecipe;
    }

    private void btnEditClicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new EditRecipePage(Recipe, RecipeBook, "Edit Recipe"));
    }
}