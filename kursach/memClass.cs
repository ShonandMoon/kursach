using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursach
{
    public class memClass: Object
    {
        public string ImagePath { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }

        public memClass()
        {

        }

        public memClass(string path, string category, string name)
        {
            ImagePath = path;
            Category = category;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
