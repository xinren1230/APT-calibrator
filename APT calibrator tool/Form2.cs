using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace APT_calibrator_tool
{
    public partial class Form2 : Form
    {
        public static Image im;
        public Form2()
        {
            InitializeComponent();
        }


        private void button53_Click(object sender, EventArgs e)
        {
            im = Properties.Resources.Al;
            Form3 poleform = new Form3();
            poleform.Show();

        }

        private void button39_Click(object sender, EventArgs e)
        {
            im = Properties.Resources.Ni;
            Form3 poleform = new Form3();
            poleform.Show();
        }

        private void button35_Click(object sender, EventArgs e)
        {
            im = Properties.Resources.W;
            Form3 poleform = new Form3();
            poleform.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            im = Properties.Resources.cubic001;
            Form3 poleform = new Form3();
            poleform.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            im = Properties.Resources.fcc002;
            Form3 poleform = new Form3();
            poleform.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            im = Properties.Resources.fcc111;
            Form3 poleform = new Form3();
            poleform.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            im = Properties.Resources.bcc110;
            Form3 poleform = new Form3();
            poleform.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            im = Properties.Resources.bcc002;
            Form3 poleform = new Form3();
            poleform.Show();
        }

        private void button114_Click(object sender, EventArgs e)
        {
            im = Properties.Resources.hcp01_10;
            Form3 poleform = new Form3();
            poleform.Show();
        }

        private void button113_Click(object sender, EventArgs e)
        {
            im = Properties.Resources.hcp0001;
            Form3 poleform = new Form3();
            poleform.Show();
        }

        private void button116_Click(object sender, EventArgs e)
        {
            im = Properties.Resources.dc111;
            Form3 poleform = new Form3();
            poleform.Show();
        }

        private void button115_Click(object sender, EventArgs e)
        {
            im = Properties.Resources.PERt;
            Form3 poleform = new Form3();
            poleform.Show();
        }

        private void button28_Click(object sender, EventArgs e)
        {
            im = Properties.Resources.Fe;
            Form3 poleform = new Form3();
            poleform.Show();
        }

        private void button58_Click(object sender, EventArgs e)
        {
            im = Properties.Resources.Si;
            Form3 poleform = new Form3();
            poleform.Show();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            im = Properties.Resources.Mg;
            Form3 poleform = new Form3();
            poleform.Show();
        }

        private void button_evpfield_Click(object sender, EventArgs e)
        {
            Form_Photo Form_PhotoIso = new Form_Photo();
            Form_PhotoIso.Show();
        }

    }
}
