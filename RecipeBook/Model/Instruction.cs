using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBook.Recipes
{
    [System.Serializable]
    public class Instruction
    {
        public string Direction { get; set; }

        public Instruction(string strStep)
        {
            Direction = strStep;
        }

        // Parameterless constructor for deserialization
        public Instruction() { }
    }
}
