using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace APT_calibrator_tool
{
    public partial class Form1 : Form
    {
        public static string elename = "";
        public static int itercount= 0;
        public static string references = "";
        public int rightcindex;
        public static Image im;
        public Form1()
        {
            InitializeComponent();
        }
        private void comboBox_pole2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "FCC" || comboBox1.Text == "BCC")
            {
            int i;
            double a = 0;
            string[] hkl;

            hkl=comboBox_pole2.Text.Split(' ');
            if (comboBox_pole2.Text == "1 1 0" || comboBox_pole2.Text == "1 0 0" || comboBox_pole2.Text == "0 0 1")
            { MessageBox.Show("Please check whether crystal planes with half spacing exist and use half-spacing crystal plane index if it exists, e.g., (110)→(220), (100)→(200), (001)→(002)."); }
            for (i=0; i < 3; i = i + 1)
            {
                a= a+ Math.Pow(Convert.ToDouble(hkl[i]),2);
            }
            double TeroD = Convert.ToDouble(textBox_latticec.Text)/Math.Sqrt(a);
            textBox_theoD.Text = TeroD.ToString();
            }

            else if (comboBox1.Text == "HCP")
            {

                string[] hkl;
                hkl = comboBox_pole2.Text.Split(' ');
                double TeroD = Math.Sqrt(1/(4 * (Math.Pow(Convert.ToDouble(hkl[0]), 2) + Math.Pow(Convert.ToDouble(hkl[1]), 2) + Convert.ToDouble(hkl[1]) * Convert.ToDouble(hkl[0])) / 3 / Math.Pow(Convert.ToDouble(textBox_latticec.Text), 2) + Math.Pow(Convert.ToDouble(hkl[2]), 2) / Math.Pow(Convert.ToDouble(textBox_c.Text), 2)));
                textBox_theoD.Text = TeroD.ToString();
            }
            else { MessageBox.Show("Undefined crystal structure"); }
        }

        private void button1_Click(object sender, EventArgs e)
        {

                try
                {
                    itercount = itercount + 1;
                    label8.Text = "Radius calibration (Iterration time: " + itercount.ToString() + ")";
                    label17.Text = "Tip Radius";
                    textBox_tipR.Text = (Convert.ToDouble(textBox_tipR.Text) / (Math.Sqrt(Convert.ToDouble(textBox_theoD.Text) / Convert.ToDouble(textBox_ObsD.Text)))).ToString();
                }
                catch (Exception)
                {
                    MessageBox.Show("Unexpected error, pls check all parameters are inputted");
                }

         }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "FCC" || comboBox1.Text == "BCC")
            {
                try
                {
                    if (textBoxmx3.Text != "" & textBoxmy3.Text != "")
                    {
                        double h1 = Convert.ToDouble(textBoxmh1.Text);
                        double k1 = Convert.ToDouble(textBoxmk1.Text);
                        double l1 = Convert.ToDouble(textBoxml1.Text);
                        double h2 = Convert.ToDouble(textBoxmh2.Text);
                        double k2 = Convert.ToDouble(textBoxmk2.Text);
                        double l2 = Convert.ToDouble(textBoxml2.Text);
                        double h3 = Convert.ToDouble(textBoxmh3.Text);
                        double k3 = Convert.ToDouble(textBoxmk3.Text);
                        double l3 = Convert.ToDouble(textBoxml3.Text);
                        double la = Convert.ToDouble(textBox_latticec.Text);

                        double angab = (180 * Math.Acos((h1*h2+k1*k2+l1*l2) / (Math.Sqrt(h1*h1+k1*k1+l1*l1) * Math.Sqrt(h2*h2+k2*k2+l2*l2))) / Math.PI);
                        double angbc = (180 * Math.Acos((h2 * h3 + k3 * k2 + l3 * l2) / (Math.Sqrt(h3 * h3 + k3 * k3 + l3 * l3) * Math.Sqrt(h2 * h2 + k2 * k2 + l2 * l2))) / Math.PI);
                        double angac = (180 * Math.Acos((h1 * h3 + k1 * k3 + l1 * l3) / (Math.Sqrt(h1 * h1 + k1 * k1 + l1 * l1) * Math.Sqrt(h3 * h3 + k3 * k3 + l3 * l3))) / Math.PI);
                        double Dab = Math.Sqrt(Math.Pow((Convert.ToDouble(textBoxmx1.Text) - Convert.ToDouble(textBoxmx2.Text)), 2) + Math.Pow((Convert.ToDouble(textBoxmy1.Text) - Convert.ToDouble(textBoxmy2.Text)), 2));
                        double Dbc = Math.Sqrt(Math.Pow((Convert.ToDouble(textBoxmx2.Text) - Convert.ToDouble(textBoxmx3.Text)), 2) + Math.Pow((Convert.ToDouble(textBoxmy2.Text) - Convert.ToDouble(textBoxmy3.Text)), 2));
                        double Dac = Math.Sqrt(Math.Pow((Convert.ToDouble(textBoxmx1.Text) - Convert.ToDouble(textBoxmx3.Text)), 2) + Math.Pow((Convert.ToDouble(textBoxmy1.Text) - Convert.ToDouble(textBoxmy3.Text)), 2));
                        double obs_angab = (180 * Math.Atan(Dab / Convert.ToDouble(textBox_Flightpath.Text)) / Math.PI);
                        double obs_angbc = (180 * Math.Atan(Dbc / Convert.ToDouble(textBox_Flightpath.Text)) / Math.PI);
                        double obs_angac = (180 * Math.Atan(Dac / Convert.ToDouble(textBox_Flightpath.Text)) / Math.PI);
                        double etaHKab = angab / obs_angab;
                        double etaHKbc = angbc / obs_angbc;
                        double etaHKac = angac / obs_angac;
                        //textBox_obstheta.Text = ((obs_angab + obs_angbc + obs_angac) / 3).ToString();
                        textBox_etaresult.Text = ((etaHKab + etaHKbc + etaHKac) / 3).ToString("0.000");
                    }
                    else if (textBoxmx2.Text != "" & textBoxmy2.Text != "")
                    {
                        double h1 = Convert.ToDouble(textBoxmh1.Text);
                        double k1 = Convert.ToDouble(textBoxmk1.Text);
                        double l1 = Convert.ToDouble(textBoxml1.Text);
                        double h2 = Convert.ToDouble(textBoxmh2.Text);
                        double k2 = Convert.ToDouble(textBoxmk2.Text);
                        double l2 = Convert.ToDouble(textBoxml2.Text);
                        double angab = (180 * Math.Acos((h1 * h2 + k1 * k2 + l1 * l2) / (Math.Sqrt(h1 * h1 + k1 * k1 + l1 * l1) * Math.Sqrt(h2 * h2 + k2 * k2 + l2 * l2))) / Math.PI);
                        double Dab = Math.Sqrt(Math.Pow((Convert.ToDouble(textBoxmx1.Text) - Convert.ToDouble(textBoxmx2.Text)), 2) + Math.Pow((Convert.ToDouble(textBoxmy1.Text) - Convert.ToDouble(textBoxmy2.Text)), 2));
                        double obs_angab = (180 * Math.Atan(Dab / Convert.ToDouble(textBox_Flightpath.Text)) / Math.PI);
                        double etaHKab = angab / obs_angab;
                        //textBox_obstheta.Text = ((obs_angab + obs_angbc + obs_angac) / 3).ToString();
                        textBox_etaresult.Text = etaHKab.ToString();

                    }
                    else
                        MessageBox.Show("At least two poles");
                }
                catch (Exception)
                {
                    MessageBox.Show("Unexpected error, pls check all parameters are inputted");
                }
            }
            else if (comboBox1.Text == "HCP")
            {
                //try
                //{
                    if (textBoxmx3.Text != "" & textBoxmy3.Text != "")
                    {
                        double h1 = Convert.ToDouble(textBoxmh1.Text);
                        double k1 = Convert.ToDouble(textBoxmk1.Text);
                        double l1 = Convert.ToDouble(textBoxml1.Text);
                        double h2 = Convert.ToDouble(textBoxmh2.Text);
                        double k2 = Convert.ToDouble(textBoxmk2.Text);
                        double l2 = Convert.ToDouble(textBoxml2.Text);
                        double h3 = Convert.ToDouble(textBoxmh3.Text);
                        double k3 = Convert.ToDouble(textBoxmk3.Text);
                        double l3 = Convert.ToDouble(textBoxml3.Text);
                        double la = Convert.ToDouble(textBox_latticec.Text);
                        double lc = Convert.ToDouble(textBox_c.Text);
                        double angab = 180 * Math.Acos((h1 * h2 + 0.5 * k1 * k2 * (h1 * k2 + h2 * k1) + 3 * la * la * l1 * l2 / 4 / lc / lc) / Math.Sqrt((h1 * h1 + k1 * k1 + h1 * k1 * 3 * la * la * l1 * l1 / 4 / lc / lc) * (h2 * h2 + k2 * k2 + h2 * k2 * 3 * la * la * l2 * l2 / 4 / lc / lc))) / Math.PI;
                        double angbc = 180 * Math.Acos((h2 * h3 + 0.5 * k2 * k3 * (h2 * k3 + h3 * k2) + 3 * la * la * l2 * l3 / 4 / lc / lc) / Math.Sqrt((h2 * h2 + k2 * k2 + h2 * k2 * 3 * la * la * l2 * l2 / 4 / lc / lc) * (h3 * h3 + k3 * k3 + h3 * k3 * 3 * la * la * l3 * l3 / 4 / lc / lc))) / Math.PI;
                        double angac = 180 * Math.Acos((h1 * h3 + 0.5 * k1 * k3 * (h1 * k3 + h3 * k1) + 3 * la * la * l1 * l3 / 4 / lc / lc) / Math.Sqrt((h1 * h1 + k1 * k1 + h1 * k1 * 3 * la * la * l1 * l1 / 4 / lc / lc) * (h3 * h3 + k3 * k3 + h3 * k3 * 3 * la * la * l3 * l3 / 4 / lc / lc))) / Math.PI;
                        double Dab = Math.Sqrt(Math.Pow((Convert.ToDouble(textBoxmx1.Text) - Convert.ToDouble(textBoxmx2.Text)), 2) + Math.Pow((Convert.ToDouble(textBoxmy1.Text) - Convert.ToDouble(textBoxmy2.Text)), 2));
                        double Dbc = Math.Sqrt(Math.Pow((Convert.ToDouble(textBoxmx2.Text) - Convert.ToDouble(textBoxmx3.Text)), 2) + Math.Pow((Convert.ToDouble(textBoxmy2.Text) - Convert.ToDouble(textBoxmy3.Text)), 2));
                        double Dac = Math.Sqrt(Math.Pow((Convert.ToDouble(textBoxmx1.Text) - Convert.ToDouble(textBoxmx3.Text)), 2) + Math.Pow((Convert.ToDouble(textBoxmy1.Text) - Convert.ToDouble(textBoxmy3.Text)), 2));
                        double obs_angab = (180 * Math.Atan(Dab / Convert.ToDouble(textBox_Flightpath.Text)) / Math.PI);
                        double obs_angbc = (180 * Math.Atan(Dbc / Convert.ToDouble(textBox_Flightpath.Text)) / Math.PI);
                        double obs_angac = (180 * Math.Atan(Dac / Convert.ToDouble(textBox_Flightpath.Text)) / Math.PI);
                        double etaHKab = angab / obs_angab;
                        double etaHKbc = angbc / obs_angbc;
                        double etaHKac = angac / obs_angac;
                        textBox_etaresult.Text = ((etaHKab + etaHKbc + etaHKac) / 3).ToString();
                    }
                    else if (textBoxmx2.Text != "" & textBoxmy2.Text != "")
                    {
                        double h1=Convert.ToDouble(textBoxmh1.Text); 
                        double k1=Convert.ToDouble(textBoxmk1.Text); 
                        double l1=Convert.ToDouble(textBoxml1.Text);
                        double h2=Convert.ToDouble(textBoxmh2.Text);
                        double k2=Convert.ToDouble(textBoxmk2.Text);
                        double l2 = Convert.ToDouble(textBoxml2.Text);
                        double la=Convert.ToDouble(textBox_latticec.Text);
                        double lc=Convert.ToDouble(textBox_c.Text);
                        double angab = 180 * Math.Acos((h1 * h2 + 0.5 * k1 * k2 * (h1 * k2 + h2 * k1) + 3 * la * la * l1 * l2 / 4 / lc / lc) / Math.Sqrt((h1 * h1 + k1 * k1 + h1 * k1 * 3 * la * la * l1 * l1 / 4 / lc / lc) * (h2 * h2 + k2 * k2 + h2 * k2 * 3 * la * la * l2 * l2 / 4 / lc / lc))) / Math.PI;
                        double Dab = Math.Sqrt(Math.Pow((Convert.ToDouble(textBoxmx1.Text) - Convert.ToDouble(textBoxmx2.Text)), 2) + Math.Pow((Convert.ToDouble(textBoxmy1.Text) - Convert.ToDouble(textBoxmy2.Text)), 2));
                        double obs_angab = (180 * Math.Atan(Dab / Convert.ToDouble(textBox_Flightpath.Text)) / Math.PI);
                        double etaHKab = angab / obs_angab;
                        textBox_etaresult.Text = etaHKab.ToString();

                        textBox_etaresult.Text = angab.ToString();


                    }
                    else
                        MessageBox.Show("At least two poles");
                //}
                //catch (Exception)
                //{
                //    MessageBox.Show("Unexpected error, pls check all parameters are inputted");
                //}
            }
            else { MessageBox.Show("Undefined crystal structure"); }
        }
        private void closeAutosave(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.combox1t= comboBox1.Text;//crystal structure
            Properties.Settings.Default.lc = textBox_c.Text;//a/c c
            Properties.Settings.Default.initialflightPath = textBox_Flightpath.Text;//将这次输入的数据储存
            Properties.Settings.Default.mx1 = textBoxmx1.Text;//将这次输入的数据储存
            Properties.Settings.Default.my1 = textBoxmy1.Text;//将这次输入的数据储存
            Properties.Settings.Default.mx2 = textBoxmx2.Text;//将这次输入的数据储存
            Properties.Settings.Default.my2 = textBoxmy2.Text;//将这次输入的数据储存
            Properties.Settings.Default.mx3 = textBoxmx3.Text;//将这次输入的数据储存
            Properties.Settings.Default.my3 = textBoxmy3.Text;//将这次输入的数据储存
            Properties.Settings.Default.h1 = textBoxmh1.Text;
            Properties.Settings.Default.k1 = textBoxmk1.Text;
            Properties.Settings.Default.l1 = textBoxml1.Text;
            Properties.Settings.Default.h2 = textBoxmh2.Text;
            Properties.Settings.Default.k2 = textBoxmk2.Text;
            Properties.Settings.Default.l2 = textBoxml2.Text;
            Properties.Settings.Default.h3 = textBoxmh3.Text;
            Properties.Settings.Default.k3 = textBoxmk3.Text;
            Properties.Settings.Default.l3 = textBoxml3.Text;
            Properties.Settings.Default.inkf = textBox1.Text;
            Properties.Settings.Default.inR = textBox2.Text;
            Properties.Settings.Default.latticec = textBox_latticec.Text;
            Properties.Settings.Default.obsd=textBox_ObsD.Text;
            Properties.Settings.Default.evaporfield = textBox_Eva_field.Text;
            string[] ss = listBox1.Items.Cast<string>().ToArray();
            Properties.Settings.Default.LISTB=string.Join(",",ss);//字符串数组转字符串
            Properties.Settings.Default.Save();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox_Flightpath.Text=Properties.Settings.Default.initialflightPath;//将这次输入的数据储存
            comboBox1.Text=Properties.Settings.Default.combox1t;//crystal structure
            textBox_c.Text=Properties.Settings.Default.lc;//a/c c
            textBoxmx1.Text=Properties.Settings.Default.mx1;//将这次输入的数据储存
             textBoxmy1.Text=Properties.Settings.Default.my1 ;//将这次输入的数据储存
            textBoxmx2.Text=Properties.Settings.Default.mx2 ;//将这次输入的数据储存
            textBoxmy2.Text=Properties.Settings.Default.my2 ;//将这次输入的数据储存
            textBoxmx3.Text=Properties.Settings.Default.mx3 ;//将这次输入的数据储存
            textBoxmy3.Text=Properties.Settings.Default.my3 ;//将这次输入的数据储存
            textBoxmh1.Text=Properties.Settings.Default.h1;
            textBoxmk1.Text=Properties.Settings.Default.k1;
            textBoxml1.Text=Properties.Settings.Default.l1;
            textBoxmh2.Text=Properties.Settings.Default.h2;
            textBoxmk2.Text=Properties.Settings.Default.k2;
            textBoxml2.Text=Properties.Settings.Default.l2;
            textBoxmh3.Text=Properties.Settings.Default.h3;
            textBoxmk3.Text=Properties.Settings.Default.k3;
            textBoxml3.Text=Properties.Settings.Default.l3;
            textBox1.Text=Properties.Settings.Default.inkf;
            textBox2.Text=Properties.Settings.Default.inR;
            textBox_ObsD.Text=Properties.Settings.Default.obsd;
            textBox_Eva_field.Text=Properties.Settings.Default.evaporfield;
            textBox_latticec.Text=Properties.Settings.Default.latticec;
            string [] aa = Properties.Settings.Default.LISTB.Split(',');
            for (int i = 0; i < aa.Length; i++)
            {
                listBox1.Items.Add(aa[i]);
            }


            bool isInGermany = RegionLanguageHelper.CheckIfRegionIsDE();
            if (isInGermany)
            {
                MessageBox.Show("Warning: The language is German (de-DE), please switch the regional language setting to en-US to avoid the problem of math format.");
            }
            else
            {
            }
        }

        private void sETToolStripMenuItem_Click(object sender, EventArgs e)
        {
   
        }

        private void poleFigureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 eminfo = new Form2();
            eminfo.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_save saform = new Form_save();
            saform.Show();
            saform.from1 = this;
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
          int index = listBox1.IndexFromPoint(e.X, e.Y);
          listBox1.SelectedIndex = index;
          if(listBox1.SelectedIndex!=-1)
        {
           string[] plist = listBox1.SelectedItem.ToString().Split(new string[] { " Crystalst:" }, StringSplitOptions.RemoveEmptyEntries);
          string[] crystalst = plist[1].Split(new string[] { " Hcpc:" }, StringSplitOptions.RemoveEmptyEntries);
          comboBox1.Text = crystalst[0];
          string[] hcpc = crystalst[1].Split(new string[] { " Flightpath:" }, StringSplitOptions.RemoveEmptyEntries);
          textBox_c.Text = hcpc[0];
          string[] flightpath = hcpc[1].Split(new string[] { " x1:" }, StringSplitOptions.RemoveEmptyEntries);
          textBox_Flightpath.Text = flightpath[0];
          string[] x1 = flightpath[1].Split(new string[] { " y1:" }, StringSplitOptions.RemoveEmptyEntries);
          textBoxmx1.Text = x1[0];
          string[] y1 = x1[1].Split(new string[] { " x2:" }, StringSplitOptions.RemoveEmptyEntries);
          textBoxmy1.Text = y1[0];
          string[] x2 = y1[1].Split(new string[] { " y2:" }, StringSplitOptions.RemoveEmptyEntries);
          textBoxmx2.Text = x2[0];
          string[] y2 = x2[1].Split(new string[] { " x3:" }, StringSplitOptions.RemoveEmptyEntries);
          textBoxmy2.Text=y2[0];
          string[] x3 = y2[1].Split(new string[] { " y3:" }, StringSplitOptions.RemoveEmptyEntries);
          if (x3[0]=="\n")
          {
              textBoxmx3.Text = "";
          }
          string[] y3 = x3[1].Split(new string[] { " h1:" }, StringSplitOptions.RemoveEmptyEntries);
          if (y3[0]== "\n")
          {
              textBoxmy3.Text = "";
          }
          string[] h1 = y3[1].Split(new string[] { " k1:" }, StringSplitOptions.RemoveEmptyEntries);
          textBoxmh1.Text=h1[0];
          string[] k1 = h1[1].Split(new string[] { " l1:" }, StringSplitOptions.RemoveEmptyEntries);
          textBoxmk1.Text=k1[0];
          string[] l1 = k1[1].Split(new string[] { " h2:" }, StringSplitOptions.RemoveEmptyEntries);
          textBoxml1.Text=l1[0];
          string[] h2 = l1[1].Split(new string[] { " k2:" }, StringSplitOptions.RemoveEmptyEntries);
           textBoxmh2.Text=h2[0];
           string[] k2 = h2[1].Split(new string[] { " l2:" }, StringSplitOptions.RemoveEmptyEntries);
           textBoxmk2.Text=k2[0];
           string[] l2 = k2[1].Split(new string[] { " h3:" }, StringSplitOptions.RemoveEmptyEntries);
           textBoxml2.Text=l2[0];
           string[] h3 = l2[1].Split(new string[] { " k3:" }, StringSplitOptions.RemoveEmptyEntries);
           textBoxmh3.Text=h3[0];
           string[] k3 = h3[1].Split(new string[] { " l3:" }, StringSplitOptions.RemoveEmptyEntries);
           textBoxmk3.Text=k3[0];
           string[] l3 = k3[1].Split(new string[] { " kf:" }, StringSplitOptions.RemoveEmptyEntries);
           textBoxml3.Text=l3[0];
           string[] kf = l3[1].Split(new string[] { " tipR:" }, StringSplitOptions.RemoveEmptyEntries);
           textBox1.Text=kf[0];
           string[] tipR = kf[1].Split(new string[] { " latticeC:" }, StringSplitOptions.RemoveEmptyEntries);
           textBox2.Text=tipR[0];
           string[] latticeC = tipR[1].Split(new string[] { " ObsD:" }, StringSplitOptions.RemoveEmptyEntries);
            textBox_latticec.Text=latticeC[0];
            textBox_ObsD.Text = latticeC[1];
        }
        }

        private void listBox1_MouseUp(object sender, MouseEventArgs e)
        {
            rightcindex = listBox1.IndexFromPoint(e.X, e.Y);
            if (e.Button == MouseButtons.Right)
                contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.SelectedIndex = rightcindex;
            if (listBox1.SelectedIndex != -1)
            {
                listBox1.Items.RemoveAt(rightcindex);
            }
        }

        private void kfCalibrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form5 kfmethod = new Form5();
            kfmethod.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox_tipR.Text = textBox2.Text;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "HCP")
            {
                label_cnm.Visible = true;
                textBox_c.Visible = true;
                label_c.Visible = true;
            }
            else
            {
                label_cnm.Visible = false;
                textBox_c.Visible = false;
                label_c.Visible = false;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
             if (comboBox2.Text == "Crystal Plane")
            {
                label21.Visible = true;
                comboBox_pole2.Visible = true;
                label15.Text = "Theoretical D";
                label16.Text = "Observed D";
            }

            else if (comboBox2.Text == "Precipitate Size")
            {
                label21.Visible=false;
                comboBox_pole2.Visible=false;
                label15.Text = "Theoretical size";
                label16.Text = "Observed size";
            }
            else if(comboBox2.Text == "Shank Angle") 
             { MessageBox.Show("Does not work now"); }
        

        }

        private void referenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            references = "B. Gault et al., Advances in the calibration of atom probe tomographic reconstruction, Journal of Applied Physics, vol. 105, no. 3, p. 034913 \r\n \r\n Ceguerra et al., Assessing the spatial accuracy of the reconstruction in atom probe tomography and a new calibratable adaptive reconstruction., Microscopy and Microanalysis, submitted 2018 \r\n \r\n De Geuser, F. & Gault, B. Reflections on the Projection of Ions in Atom Probe Tomography. Microscopy and Microanalysis 23, 238–246 (2017).";
            Form_Reference myfrm3 = new Form_Reference();
            myfrm3.Show();
        }

        private void authorshipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_about myfrm2 = new Form_about();
            myfrm2.Show();
        }

        private void poleFigureSimulatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pole_Simulator polesimulator = new Pole_Simulator();
            polesimulator.Show();
        }

        private void button_KFCA_Click(object sender, EventArgs e)
        {

             try
                {
                    if (textBox4_KF.Text != "" & textBox3_R.Text != "")
                    { textBox4_KF.Text = (Convert.ToDouble(textBox4_KF.Text) * Convert.ToDouble(textBox3_R.Text) / Convert.ToDouble(textBox_tipR.Text)).ToString();
                    textBox3_R.Text = textBox_tipR.Text;
                    }
                      
                    else
                    { MessageBox.Show("Please input Kf and tip R !");}
                }
                catch (Exception)
                {
                    MessageBox.Show("Unexpected error, pls check all parameters are inputted");
                }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkLabel1.LinkVisited = true;

            // Navigate to a URL.
            System.Diagnostics.Process.Start("https://www.xinrenchen.com/home/apt-calibrator");

        }

        private void runTimeCalculatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Time_calculator myfrm_runtime = new Time_calculator();
            myfrm_runtime.Show();
        }

        private void btn_kfprop_Click(object sender, EventArgs e)
        {
            Form_kfprop kfmethod = new Form_kfprop();
            kfmethod.Show();
        }

        private void evaporationFieldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Photo Form_PhotoIso = new Form_Photo();
            Form_PhotoIso.Show();
        }

        private void periodicTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            im = Properties.Resources.PERt;
            Form_pero poleform = new Form_pero();
            poleform.Show();
        }

        private void button_kfcalc_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox_Eva_field.Text != "" & textBox_initv.Text != "")
                {
                    textBox_kfrest.Text = (Convert.ToDouble(textBox_initv.Text) / Convert.ToDouble(textBox_tipR.Text) / Convert.ToDouble(textBox_Eva_field.Text)).ToString();
                }

                else
                { MessageBox.Show("Please input Evaporation field and Init. Volt. +Vp !"); }
            }
            catch (Exception)
            {
                MessageBox.Show("Unexpected error, pls check all parameters are inputted");
            }
        }

        private void label37_Click(object sender, EventArgs e)
        {

        }

        private void textBox_Eva_field_TextChanged(object sender, EventArgs e)
        {

        }

        private void label36_Click(object sender, EventArgs e)
        {

        }

        private void textBox_initv_TextChanged(object sender, EventArgs e)
        {

        }

        private void label38_Click(object sender, EventArgs e)
        {

        }

        private void textBox_kfrest_TextChanged(object sender, EventArgs e)
        {

        }

        private void dynamicAPTReconstructionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_DynRec dynform = new Form_DynRec();
            dynform.Show();
        }

        private void textBox_etaresult_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
