using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APT_calibrator_tool
{
    public partial class Time_calculator : Form
    {
        public Time_calculator()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double RUN_time = Convert.ToDouble(textBox_IN.Text) *1000000/ 1000 / Convert.ToDouble(textBox_PF.Text) / Convert.ToDouble(textBox_DR.Text) / 0.01 / 3600;
            textBox_RT.Text = RUN_time.ToString("G17");

        }
    }
}
