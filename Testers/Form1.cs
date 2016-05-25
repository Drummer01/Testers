using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

//https://github.com/Self747/Testers/blob/master/Testers/Form1.cs
namespace Testers
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Log testersLogs = new Log();

        private int testersCount;

        private string[] names = new string[] { "Jhon", "Alex", "Bob", "Carl", "Monika", "Philip", "Andrew", "George", "Josefina", "Danielle", "Tommy", "Arnold", "Carolyn", "Herman", "Raymond", "Ricky", "Allan", "Kerry", "Miguel", "Ken" };

        private Thread[] threads;

        delegate void OnProgPoolSizeCallback(object text);

        private void Form1_Load(object sender, EventArgs e)
        {
            Log.logSource = logsListBox;
            Log.mainControlForm = this;

            testersCountTrackBar.Maximum = names.Length;

            this.testersCount = testersCountTrackBar.Value;

            Tester.OnTextStatusUpdate += TesterTextStatusUpdateHandler;
            ProgPool.OnProgPoolSizeUpdate += ProgPoolSizeUpdateHandler;

        }

        private void TesterTextStatusUpdateHandler(object t, string msg, string prefix)
        {
            try
            {
                var tester = (Tester)t;
                testersLogs.push($"{DateTime.Now.ToLongTimeString()} {prefix} {tester.name} : {msg}", true);
                testersInfoGrid.Rows[tester.index].Cells[1].Value = msg;
                testersInfoGrid.Rows[tester.index].Cells[0].Style = new DataGridViewCellStyle { ForeColor = Tester.textStatusColors[tester.status] };
            }
            catch (Exception)
            {
            }
        }

        private void ProgPoolSizeUpdateHandler(object size)
        {
            if (programCount.InvokeRequired)
            {
                OnProgPoolSizeCallback cb = new OnProgPoolSizeCallback(ProgPoolSizeUpdateHandler);
                this.Invoke(cb, new object[] { size });
            }
            else
            {
                programCount.Text = $"Кількість програм у черзі {size}";
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "xml files (*.xlm)|*.xml|All files (*.*)|*.*";
            sfd.FilterIndex = 2;
            sfd.RestoreDirectory = true;
            if(sfd.ShowDialog() == DialogResult.OK)
            {
                testersLogs.Save(sfd.OpenFile());
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "xml files (*.xlm)|*.xml|All files (*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                testersLogs = Log.Open(ofd.OpenFile());
                var form = new TestersEfficiencyForm(testersLogs.CalculateEfficiency());
                form.Show();
            }
        }

        private void testersCountTrackBar_ValueChanged(object sender, EventArgs e)
        {
            testersCountLabel.Text = $"Кількість тестерів: {testersCountTrackBar.Value}";
            testersCount = testersCountTrackBar.Value;
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.testersInfoGrid.Rows.Clear();
            this.logsListBox.Items.Clear();

            testersLogs = new Log();
            this.threads = new Thread[this.testersCount];

            for (int i = 0; i < testersCount; i++)
            {
                string name = names[i];
                int temp = i;
                Thread t = new Thread(() =>
                {
                    new Tester(name, temp);
                });
                testersInfoGrid.Rows.Add(new string[] { name, " " });
                threads[temp] = t;
                t.Start();
            }
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < (this.threads?.Length == null ? 0 : this.threads.Length); i++)
                this.threads[i]?.Abort();
            
        }

        private void calculateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new TestersEfficiencyForm(testersLogs.CalculateEfficiency());
            form.Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopToolStripMenuItem_Click(null, null);
        }
    }
}
