using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Testers
{
    static class Util
    {
       

        private static Random rand = new Random(); 

        /*
        [min; max)
        */
        public static int randInt(double min, double max)
        {
            return (int) (rand.NextDouble() * (max - min) + min);
        }

        public static bool randBool()
        {
            return rand.Next(0, 3) == 0 ? false : true;
        }
        
    }
}
