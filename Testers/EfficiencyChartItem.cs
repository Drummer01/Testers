using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testers
{
    public class EfficiencyChartItem
    {
        public string Name { get; set; }
        public int Good { get; set; }
        public int Bad { get; set; }

        public void update(bool correct)
        {
            Good += Convert.ToInt32(correct);
            Bad += Convert.ToInt32(!correct);
        }
    }
}
