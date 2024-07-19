using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_1
{
    internal class CustomNameAttribute : Attribute
    {
        public string name { get; set;}

        public CustomNameAttribute() { }
        public CustomNameAttribute(string CustomName)
        {
            name = CustomName;
        }
    }      
}
