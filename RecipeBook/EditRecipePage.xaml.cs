using RecipeBook.Recipes;

namespace RecipeBook;

public partial class EditRecipePage : ContentPage
{
	public RecipeList RecipeList;
	public Recipe CurrentRecipe;
    public String mode;


    public EditRecipePage(Recipe recipe, RecipeList recipeBook, string modeName)
    {
        InitializeComponent();

        // Set the BindingContext and other properties
        CurrentRecipe = recipe;
        RecipeList = recipeBook;
        mode = modeName;

        BindingContext = CurrentRecipe;

        // Update labels with the mode and recipe name
        lableMode.Text = mode; // Display the mode (e.g., "Add Recipe" or "Edit Recipe")
        lableRecipeName.Text = "Recipe Name: " + CurrentRecipe.Name;
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
        SaveHelper.SaveRecipeListToJson(RecipeList);
    }

    private async void OnInstructionTapped(object sender, TappedEventArgs e)
    {
        // Get the selected recipe via the CommandParameter
        var tappedItem = (sender as Element)?.BindingContext as Instruction;

        if (tappedItem != null)
        {
            string result = await DisplayPromptAsync("Edit Instruction", tappedItem.Direction,
                                                    placeholder: "", maxLength: 100);

            tappedItem.Direction = result;
            SaveRecipeList();
        }
    }

    private async void OnIngredientTapped(object sender, TappedEventArgs e)
    {
        // Get the selected recipe via the CommandParameter
        var tappedItem = (sender as Element)?.BindingContext as Ingredient;

        if (tappedItem != null)
        {
            string result = await DisplayPromptAsync("Edit Ingredient", tappedItem.Name,
                                                    placeholder: "", maxLength: 100);

            tappedItem.Name = result;
            SaveRecipeList();
        }
    }
}