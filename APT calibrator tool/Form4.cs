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
    public partial class Form_save : Form
    {
        public Form_save()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (from1.textBoxmx3.Text != "" & from1.textBoxmy3.Text != "")
            {
                from1.listBox1.Items.Add(textBox1.Text + " " + DateTime.Now.ToString() + " Crystalst:" + from1.comboBox1.Text + " Hcpc:" + from1.textBox_c.Text + " Flightpath:" + from1.textBox_Flightpath.Text + " x1:" + from1.textBoxmx1.Text + " y1:" + from1.textBoxmy1.Text + " x2:" + from1.textBoxmx2.Text + " y2:" + from1.textBoxmy2.Text + " x3:" + from1.textBoxmx3.Text + " y3:" + from1.textBoxmy3.Text + " h1:" + from1.textBoxmh1.Text + " k1:" + from1.textBoxmk1.Text
                    + " l1:" + from1.textBoxml1.Text + " h2:" + from1.textBoxmh2.Text + " k2:" + from1.textBoxmk2.Text + " l2:" + from1.textBoxml2.Text + " h3:" + from1.textBoxmh3.Text + " k3:" + from1.textBoxmk3.Text + " l3:" + from1.textBoxml3.Text + " kf:" + from1.textBox1.Text + " tipR:" + from1.textBox2.Text + " latticeC:" + from1.textBox_latticec.Text + " ObsD:" + from1.textBox_ObsD.Text);
                this.Close();
            }
            else if (from1.textBoxmx3.Text == "")
            {
                from1.listBox1.Items.Add(textBox1.Text + " " + DateTime.Now.ToString() + " Crystalst:" + from1.comboBox1.Text + " Hcpc:" + from1.textBox_c.Text + " Flightpath:" + from1.textBox_Flightpath.Text + " x1:" + from1.textBoxmx1.Text + " y1:" + from1.textBoxmy1.Text + " x2:" + from1.textBoxmx2.Text + " y2:" + from1.textBoxmy2.Text + " x3:" + "\n" + " y3:" + "\n" + " h1:" + from1.textBoxmh1.Text + " k1:" + from1.textBoxmk1.Text
                    + " l1:" + from1.textBoxml1.Text + " h2:" + from1.textBoxmh2.Text + " k2:" + from1.textBoxmk2.Text + " l2:" + from1.textBoxml2.Text + " h3:" + from1.textBoxmh3.Text + " k3:" + from1.textBoxmk3.Text + " l3:" + from1.textBoxml3.Text + " kf:" + from1.textBox1.Text + " tipR:" + from1.textBox2.Text + " latticeC:" + from1.textBox_latticec.Text + " ObsD:" + from1.textBox_ObsD.Text);
                this.Close();
            }
        }

        public Form1 from1 { get; set; }
    }
}
