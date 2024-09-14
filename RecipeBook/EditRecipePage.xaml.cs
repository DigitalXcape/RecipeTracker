using RecipeBook.Recipes;

namespace RecipeBook;

public partial class EditRecipePage : ContentPage
{
	public RecipeList RecipeBook;
	public Recipe CurrentRecipe;
    public String mode;

	public EditRecipePage(Recipe recipe, RecipeList recipeBook, String modeName)
	{
		InitializeComponent();

        BindingContext = recipe;

        RecipeBook = recipeBook;
		CurrentRecipe = recipe;
        mode = modeName;

        lableMode.Text = mode;
        lableRecipeName.Text = "Recipe Name: " + recipe.Name;
	}

    private async void OnAddIngredientClicked(object sender, EventArgs e)
    {
        // Display a prompt for adding an ingredient
        string result = await DisplayPromptAsync("Add Ingredient", "Enter the name of the ingredient:",
                                                    placeholder: "e.g., 1 cup sugar", maxLength: 100);

        if (!string.IsNullOrWhiteSpace(result))
        {
            // Add the ingredient to the list (you can also bind this list to a UI element)
            CurrentRecipe.Ingredients.Add(new Ingredient(result));
        }

        SaveRecipeList();
    }

    private async void OnAddInstructionClicked(object sender, EventArgs e)
    {
        // Display a prompt for adding an ingredient
        string result = await DisplayPromptAsync("Add Instruction", "Enter the name of the ingredient:",
                                                    placeholder: "e.g., Add flour to bowl", maxLength: 100);

        if (!string.IsNullOrWhiteSpace(result))
        {
            // Add the ingredient to the list (you can also bind this list to a UI element)
            CurrentRecipe.Instructions.Add(new Instruction(result));
        }

        SaveRecipeList();
    }

    private async void btnEditNameClicked(object sender, EventArgs e)
    {
        // Display a prompt for adding an ingredient
        string result = await DisplayPromptAsync("Change Name", "Enter the name of the recipe",
                                                    placeholder: CurrentRecipe.Name, maxLength: 100);

        if (!string.IsNullOrWhiteSpace(result))
        {
            // Add the ingredient to the list (you can also bind this list to a UI element)
            CurrentRecipe.Name = result;

            lableRecipeName.Text = result;
        }

        SaveRecipeList();
    }

    private void OnDeleteIngredientClicked(object sender, EventArgs e)
    {
        // Get the button that was clicked
        var button = (Button)sender;

        // Get the ingredient associated with the button (command parameter)
        var ingredient = (Ingredient)button.CommandParameter;

        if (ingredient != null)
        {
            // Remove the ingredient from the collection
            CurrentRecipe.Ingredients.Remove(ingredient);
        }

        SaveRecipeList();
    }

    private void OnDeleteInstructionClicked(object sender, EventArgs e)
    {
        // Get the button that was clicked
        var button = (Button)sender;

        // Get the instruction associated with the button (command parameter)
        var instruction = (Instruction)button.CommandParameter;

        if (instruction != null)
        {
            // Remove the instruction from the collection
            CurrentRecipe.Instructions.Remove(instruction);
        }

        SaveRecipeList();
    }

    private void SaveRecipeList()
    {
        SaveHelper.SaveRecipeListToJson(RecipeBook);
    }
}