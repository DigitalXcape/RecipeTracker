using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBook.Recipes
{
    public static class ConvertHelper
    {
        //Converts values into one string
        public static string ConvertUnitToString(int amount, MeasurementUnit measurementUnit, FractionalAmount fractionalAmount)
        {
            string unitLable = string.Empty;

            switch (fractionalAmount)
            {
                case FractionalAmount.OneQuarter:
                    unitLable = "1/4";
                    break;
                case FractionalAmount.OneThird:
                    unitLable = "1/3";
                    break;
                case FractionalAmount.OneHalf:
                    unitLable = "1/2";
                    break;
                case FractionalAmount.ThreeQuarter:
                    unitLable = "3/4";
                    break;
                case FractionalAmount.TwoThird:
                    unitLable = "2/3";
                    break;
                default:
                    unitLable = string.Empty;
                    break;
            }

            if (unitLable != string.Empty)
            {
                return amount + " " + measurementUnit.ToString();
            }
            else
            {
                return unitLable + " "  + measurementUnit.ToString();
            }
        }
    }
}
