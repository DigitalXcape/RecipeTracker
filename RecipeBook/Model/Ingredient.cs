using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBook.Recipes
{
    [System.Serializable]
    public class Ingredient
    {
        public int Amount;
        public string Name { get; set; }

        public Ingredient(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            //return ConvertHelper.ConvertUnitToString(Amount, MeasurementUnit, FractionalAmount) + " " + Name; 
            return Name;
        }
    }
}
