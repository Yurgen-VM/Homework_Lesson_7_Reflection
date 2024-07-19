using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_1
{

   
    class TestClass
    {
        [CustomName("INT")]
        public int I { get; set; }
        [CustomName("STRING")]
        public string? S { get; set; }
        [CustomName("DECIMAL")]
        public decimal D { get; set; }
        [CustomName("CHAR")]
        public char[]? C { get; set; }

        public TestClass()
        {
        }

        private TestClass(int i)
        {
            this.I = i;
        }

        public TestClass(int i, string s, decimal d, char[] c) : this(i)
        {
            this.S = s;
            this.D = d;
            this.C = c;
        }
    }
}
