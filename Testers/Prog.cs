using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//https://github.com/Self747/Testers/blob/master/Testers/Prog.cs
namespace Testers
{
    class Prog
    {
        public Tester writer { get; set; }

        public Tester tester { get; set; }

        public bool correct { get; set; } = true;

        public bool beenFixed { get; set; } = false;
    }
}
