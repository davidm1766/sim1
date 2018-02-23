﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SimGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            chart1.Series["Series2"].XValueType = ChartValueType.Int32;
            chart1.Series["Series1"].XValueType = ChartValueType.Int32;
            chart1.Series["Series1"].ChartType = SeriesChartType.Line;
            chart1.Series["Series2"].ChartType = SeriesChartType.Line;

            chart1.Series["Series2"].BorderWidth = 3;
            //chart1.ChartAreas["0"].AxisX.Interval = 1;

            chart1.Series["Series1"].BorderWidth = 3;
            //chart1.ChartAreas["0"].AxisX.Interval = 1;

        }
        int last = 0;
        private void button1_Click(object sender, EventArgs e)
        {            
            chart1.Series["Series1"].Points.AddXY(last,new Random().Next(199));
            chart1.Series["Series2"].Points.AddXY(last++, new Random().Next(199));
        }
    }
}
