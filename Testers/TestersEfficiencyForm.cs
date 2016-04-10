using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Testers
{
    public partial class TestersEfficiencyForm : Form
    {
        private EfficiencyChartItems chartItems;

        public TestersEfficiencyForm(EfficiencyChartItems items)
        {
            InitializeComponent();
            chartItems = items;
        }

        private void TestersEfficiencyForm_Load(object sender, EventArgs e)
        {

            testersEfficiencyChar.DataSource = chartItems.toList();


            testersEfficiencyChar.Series.Clear();
            var goodprog = testersEfficiencyChar.Series.Add("Правельних програм");
            goodprog.XValueMember = "Name";
            goodprog.YValueMembers = "Good";
            goodprog.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

            var badprog = testersEfficiencyChar.Series.Add("Неправельних програм");
            badprog.XValueMember = "Name";
            badprog.YValueMembers = "Bad";
            badprog.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

        }
    }
}
