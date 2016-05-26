using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//https://github.com/Self747/Testers/blob/master/Testers/EfficiencyChartItems.cs
namespace Testers
{
    public class EfficiencyChartItems : IEnumerable<EfficiencyChartItem>
    {
        private Dictionary<string, EfficiencyChartItem> items = new Dictionary<string, EfficiencyChartItem>();

        public EfficiencyChartItem this[string name]
        {
            get
            {
                try
                {
                    return this.items[name];
                }
                catch (KeyNotFoundException)
                {
                    items.Add(name, new EfficiencyChartItem { Name = name, Good = 0, Bad = 0 });
                    return this[name];
                }
                
            }
            set
            {
                items.Add(name, new EfficiencyChartItem { Name = name, Good = 0, Bad = 0 });
            }
        }

        public List<EfficiencyChartItem> toList()
        {
            return items.Values.ToList();
        }

        public IEnumerator<EfficiencyChartItem> GetEnumerator()
        {
            return items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.Values.GetEnumerator();
        }
    }
}
