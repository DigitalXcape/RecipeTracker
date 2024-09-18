using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBook.Recipes
{
    [System.Serializable]
    public class Ingredient : INotifyPropertyChanged
    {
        public int Amount;

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public Ingredient(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            //return ConvertHelper.ConvertUnitToString(Amount, MeasurementUnit, FractionalAmount) + " " + Name; 
            return Name;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
