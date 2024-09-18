using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RecipeBook.Recipes
{
    [System.Serializable]
    public class Instruction : INotifyPropertyChanged
    {
        private string direction;
        public string Direction
        {
            get
            {
                return this.direction;
            }
            set
            {
                this.direction = value;
                OnPropertyChanged(nameof(Direction));
            }
        }

        public Instruction(string strStep)
        {
            Direction = strStep;
        }

        // Parameterless constructor for deserialization
        public Instruction() { }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
