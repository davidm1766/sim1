using SimLog;
using SimShared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SimGUI
{
    public partial class Form1 : Form
    {
        private MainLog _log;

        public Form1()
        {
            InitializeComponent();
            
            chart1.Series["Series2"].XValueType = ChartValueType.Int32;
            chart1.Series["Series1"].XValueType = ChartValueType.Int32;
            chart1.Series["Series1"].ChartType = SeriesChartType.Line;
            chart1.Series["Series2"].ChartType = SeriesChartType.Line;

            chart1.Series["Series2"].BorderWidth = 3;
            chart1.Series["Series1"].BorderWidth = 3;

            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Minimum = 0;
            
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int doors = int.Parse(txtDoors.Text);
                int rep = int.Parse(txtRep.Text);
                
                _log = new MainLog(doors,rep);
                _log.OnChangedDecision += this._log_OnChangedDecision;
                _log.OnDontChangedDecision += _log_OnDontChangedDecision;
                _log.Start();

            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void _log_OnDontChangedDecision(object sender, CoreArgs e)
        {

            this.BeginInvoke(new MethodInvoker(delegate
            {
                chart1.Series["Series2"].Points.AddXY(e.Iteration, e.PercentOfSuccessfull);
            }));            
        }

        private void _log_OnChangedDecision(object sender, CoreArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                chart1.Series["Series1"].Points.AddXY(e.Iteration, e.PercentOfSuccessfull);
            }));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _log.Stop();
        }
    }
}
