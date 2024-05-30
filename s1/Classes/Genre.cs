using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace s1.Classes
{
    public class Genre
    {
        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                if (value != null && value.Length > 0)
                {
                    name = value;
                }
                else
                {
                    throw new ArgumentException("Name must not be empty.");
                }
            }
        }
        public bool IsSelected { get; set; }
    }
}
