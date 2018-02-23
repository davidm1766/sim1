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
            btnPause.Enabled = false;
            btnStop.Enabled = false;
            BtnCont.Enabled = false;

            chart1.Series["Series2"].LegendText = "So zmenou";
            chart1.Series["Series1"].LegendText = "Bez zmeny";

            chart1.Series["Series2"].XValueType = ChartValueType.Int32;
            chart1.Series["Series1"].XValueType = ChartValueType.Int32;
            chart1.Series["Series1"].ChartType = SeriesChartType.Line;
            chart1.Series["Series2"].ChartType = SeriesChartType.Line;

            chart1.Series["Series2"].BorderWidth = 3;
            chart1.Series["Series1"].BorderWidth = 3;

            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Minimum = 0;

            chart1.ChartAreas[0].AxisY.Maximum = 1;
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                chart1.Series["Series1"].Points.Clear();
                chart1.Series["Series2"].Points.Clear();

                int doors = int.Parse(txtDoors.Text);
                int rep = int.Parse(txtRep.Text);
                int paus = int.Parse(txtPause.Text);

                _log = new MainLog(doors,rep,paus);
                _log.OnChangedDecision += this._log_OnChangedDecision;
                _log.OnDontChangedDecision += _log_OnDontChangedDecision;
                _log.Start();

                btnStart.Enabled = false;
                btnPause.Enabled = true;
                btnStop.Enabled = true;
                BtnCont.Enabled = true;

            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void StopSim()
        {
            
            if (_log != null)
            {
                _log.Stop();
                _log.OnDontChangedDecision -= _log_OnDontChangedDecision;
                _log.OnChangedDecision -= _log_OnChangedDecision;
                _log = null;
            }
            
        }

        private void _log_OnDontChangedDecision(object sender, CoreArgs e)
        {

            this.BeginInvoke(new MethodInvoker(delegate
            {
                if (e.PercentOfSuccessfull > 1) {
                    throw new Exception();
                }
                chart1.Series["Series2"].Points.AddXY(e.Iteration, e.PercentOfSuccessfull);
                lblNoChange.Text = e.PercentOfSuccessfull.ToString("N4");
            }));            
        }

        private void _log_OnChangedDecision(object sender, CoreArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                if (e.PercentOfSuccessfull > 1)
                {
                    throw new Exception();
                }
                chart1.Series["Series1"].Points.AddXY(e.Iteration, e.PercentOfSuccessfull);
                lblChange.Text = e.PercentOfSuccessfull.ToString("N4");
            }));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _log?.Stop();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopSim();
            btnStart.Enabled = true;

            btnPause.Enabled = false;
            btnStop.Enabled = false;
            BtnCont.Enabled = false;

        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            btnPause.Enabled = false;
            _log?.Pause();
        }

        private void BtnCont_Click(object sender, EventArgs e)
        {
            btnPause.Enabled = true;
            _log?.Continue();
        }
    }
}
