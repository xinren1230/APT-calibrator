﻿using System;
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
    public partial class Form_kfprop : Form
    {
        public Form_kfprop()
        {
            InitializeComponent();
        }

        private void Form_kfprop_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.Kfcalibration;
        }
    }
}