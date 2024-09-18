using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBook.Recipes
{
    [System.Serializable]
    public class Recipe : INotifyPropertyChanged
    {
        private string _name;
        private string _description;

        private bool _favorite;

        private ObservableCollection<Ingredient> _ingredients;

        private ObservableCollection<Instruction> _instructions;

        public bool Favorite
        {
            get => _favorite;
            set
            {
                _favorite = value;
                OnPropertyChanged(nameof(Favorite));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public ObservableCollection<Ingredient> Ingredients
        {
            get => _ingredients;
            set
            {
                _ingredients = value;
                OnPropertyChanged(nameof(Ingredients));
            }
        }

        public ObservableCollection<Instruction> Instructions
        {
            get => _instructions;
            set
            {
                _instructions = value;
                OnPropertyChanged(nameof(Instructions));
            }
        }

        public Recipe(string name)
        {
            Name = name;
            Ingredients = new ObservableCollection<Ingredient>();
            Instructions = new ObservableCollection<Instruction>();
        }

        public void AddIngredient(Ingredient ingredient)
        {
            Ingredients.Add(ingredient);
            OnPropertyChanged(nameof(Ingredients));
        }

        public void RemoveIngredient(Ingredient ingredient)
        {
            Ingredients.Remove(ingredient);
            OnPropertyChanged(nameof(Ingredients));
        }

        public void AddInstruction(Instruction instruction)
        {
            Instructions.Add(instruction);
            OnPropertyChanged(nameof(Instructions));
        }

        public void RemoveInstruction(Instruction instruction)
        {
            Instructions.Remove(instruction);
            OnPropertyChanged(nameof(Instructions));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
