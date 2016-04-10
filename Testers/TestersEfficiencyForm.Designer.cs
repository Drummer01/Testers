namespace Testers
{
    partial class TestersEfficiencyForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.testersEfficiencyChar = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.testersEfficiencyChar)).BeginInit();
            this.SuspendLayout();
            // 
            // testersEfficiencyChar
            // 
            chartArea1.Name = "ChartArea1";
            this.testersEfficiencyChar.ChartAreas.Add(chartArea1);
            this.testersEfficiencyChar.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.testersEfficiencyChar.Legends.Add(legend1);
            this.testersEfficiencyChar.Location = new System.Drawing.Point(0, 0);
            this.testersEfficiencyChar.Name = "testersEfficiencyChar";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.testersEfficiencyChar.Series.Add(series1);
            this.testersEfficiencyChar.Size = new System.Drawing.Size(1250, 434);
            this.testersEfficiencyChar.TabIndex = 0;
            this.testersEfficiencyChar.Text = "chart1";
            // 
            // TestersEfficiencyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1250, 434);
            this.Controls.Add(this.testersEfficiencyChar);
            this.Name = "TestersEfficiencyForm";
            this.Text = "TestersEfficiencyForm";
            this.Load += new System.EventHandler(this.TestersEfficiencyForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.testersEfficiencyChar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart testersEfficiencyChar;
    }
}