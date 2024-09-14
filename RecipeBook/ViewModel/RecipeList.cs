using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace RecipeBook.Recipes
{
    [System.Serializable]
    public class RecipeList : INotifyPropertyChanged
    {
        private ObservableCollection<Recipe> _recipes;

        public List<Recipe> StoredRecipes {  get; private set; }

        public ObservableCollection<Recipe> Recipes
        {
            get => _recipes;
            set
            {
                _recipes = value;
                OnPropertyChanged(nameof(Recipes));
            }
        }

        public ICommand SelectRecipeCommand { get; private set; }

        public RecipeList()
        {
            Recipes = new ObservableCollection<Recipe>
            {
                //input hard coded elements here
            };

            SelectRecipeCommand = new Command<Recipe>(OnRecipeSelected);
        }

        /// <summary>
        /// Navigate to a ViewRecipe page with the selected recipe
        /// </summary>
        /// <param name="recipe"></param>
        private void OnRecipeSelected(Recipe recipe)
        {
            Application.Current.MainPage.Navigation.PushAsync(new ViewRecipe(recipe, this));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
