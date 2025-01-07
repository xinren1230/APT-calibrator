namespace APT_calibrator_tool
{
    partial class Form_DynRec
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_DynRec));
            this.btn_selec = new System.Windows.Forms.Button();
            this.textBox_message = new System.Windows.Forms.TextBox();
            this.openGLControl = new SharpGL.OpenGLControl();
            this.pole_calc = new System.Windows.Forms.Button();
            this.listViewData = new System.Windows.Forms.ListView();
            this.btn_xy = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_ICFscaling = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_Kfscaling = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.tb_slicex = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_slicey = new System.Windows.Forms.TextBox();
            this.tb_slicez = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.tb_slicewidth = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.nm = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.bt_showall = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_selec
            // 
            this.btn_selec.Location = new System.Drawing.Point(28, 24);
            this.btn_selec.Name = "btn_selec";
            this.btn_selec.Size = new System.Drawing.Size(170, 23);
            this.btn_selec.TabIndex = 0;
            this.btn_selec.Text = "Select POS file";
            this.btn_selec.UseVisualStyleBackColor = true;
            this.btn_selec.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // textBox_message
            // 
            this.textBox_message.Location = new System.Drawing.Point(41, 98);
            this.textBox_message.Multiline = true;
            this.textBox_message.Name = "textBox_message";
            this.textBox_message.ReadOnly = true;
            this.textBox_message.Size = new System.Drawing.Size(321, 214);
            this.textBox_message.TabIndex = 1;
            // 
            // openGLControl
            // 
            this.openGLControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.openGLControl.DrawFPS = false;
            this.openGLControl.Location = new System.Drawing.Point(28, 72);
            this.openGLControl.Name = "openGLControl";
            this.openGLControl.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
            this.openGLControl.RenderContextType = SharpGL.RenderContextType.DIBSection;
            this.openGLControl.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.openGLControl.Size = new System.Drawing.Size(1034, 625);
            this.openGLControl.TabIndex = 3;
            this.openGLControl.OpenGLInitialized += new System.EventHandler(this.openGLControl_OpenGLInitialized);
            this.openGLControl.OpenGLDraw += new SharpGL.RenderEventHandler(this.openGLControl_OpenGLDraw);
            this.openGLControl.Resized += new System.EventHandler(this.openGLControl_Resized);
            this.openGLControl.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseWheel);
            // 
            // pole_calc
            // 
            this.pole_calc.Location = new System.Drawing.Point(987, 28);
            this.pole_calc.Name = "pole_calc";
            this.pole_calc.Size = new System.Drawing.Size(75, 23);
            this.pole_calc.TabIndex = 4;
            this.pole_calc.Text = "Save Pos";
            this.pole_calc.UseVisualStyleBackColor = true;
            this.pole_calc.Click += new System.EventHandler(this.Save_New_file);
            // 
            // listViewData
            // 
            this.listViewData.HideSelection = false;
            this.listViewData.Location = new System.Drawing.Point(355, 205);
            this.listViewData.Name = "listViewData";
            this.listViewData.Size = new System.Drawing.Size(401, 218);
            this.listViewData.TabIndex = 2;
            this.listViewData.UseCompatibleStateImageBehavior = false;
            // 
            // btn_xy
            // 
            this.btn_xy.Location = new System.Drawing.Point(950, 93);
            this.btn_xy.Name = "btn_xy";
            this.btn_xy.Size = new System.Drawing.Size(75, 23);
            this.btn_xy.TabIndex = 6;
            this.btn_xy.Text = "XZ";
            this.btn_xy.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_xy.UseVisualStyleBackColor = true;
            this.btn_xy.Click += new System.EventHandler(this.btn_xy_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(823, 137);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Scale Bar: 50 nm";
            // 
            // tb_ICFscaling
            // 
            this.tb_ICFscaling.Location = new System.Drawing.Point(286, 27);
            this.tb_ICFscaling.Name = "tb_ICFscaling";
            this.tb_ICFscaling.Size = new System.Drawing.Size(100, 20);
            this.tb_ICFscaling.TabIndex = 9;
            this.tb_ICFscaling.Text = "1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(218, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "ICF scaling:";
            // 
            // tb_Kfscaling
            // 
            this.tb_Kfscaling.Location = new System.Drawing.Point(453, 26);
            this.tb_Kfscaling.Name = "tb_Kfscaling";
            this.tb_Kfscaling.Size = new System.Drawing.Size(100, 20);
            this.tb_Kfscaling.TabIndex = 9;
            this.tb_Kfscaling.Text = "1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(391, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Kf scaling:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(559, 25);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(108, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "Re-reconstruction";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Re_recon);
            // 
            // tb_slicex
            // 
            this.tb_slicex.Location = new System.Drawing.Point(735, 17);
            this.tb_slicex.Name = "tb_slicex";
            this.tb_slicex.Size = new System.Drawing.Size(50, 20);
            this.tb_slicex.TabIndex = 9;
            this.tb_slicex.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(696, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Slice:";
            // 
            // tb_slicey
            // 
            this.tb_slicey.Location = new System.Drawing.Point(791, 17);
            this.tb_slicey.Name = "tb_slicey";
            this.tb_slicey.Size = new System.Drawing.Size(50, 20);
            this.tb_slicey.TabIndex = 9;
            this.tb_slicey.Text = "0";
            // 
            // tb_slicez
            // 
            this.tb_slicez.Location = new System.Drawing.Point(847, 17);
            this.tb_slicez.Name = "tb_slicez";
            this.tb_slicez.Size = new System.Drawing.Size(50, 20);
            this.tb_slicez.TabIndex = 9;
            this.tb_slicez.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(738, 1);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(15, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "x:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(797, 1);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(15, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "y:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(853, 1);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(15, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "z:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(735, 43);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(45, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "Set";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.slicex);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(791, 43);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(45, 23);
            this.button3.TabIndex = 11;
            this.button3.Text = "Set";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.slicey);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(847, 43);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(45, 23);
            this.button4.TabIndex = 11;
            this.button4.Text = "Set";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.slicez);
            // 
            // tb_slicewidth
            // 
            this.tb_slicewidth.Location = new System.Drawing.Point(903, 17);
            this.tb_slicewidth.Name = "tb_slicewidth";
            this.tb_slicewidth.Size = new System.Drawing.Size(46, 20);
            this.tb_slicewidth.TabIndex = 12;
            this.tb_slicewidth.Text = "5";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(900, -3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Width:";
            // 
            // nm
            // 
            this.nm.AutoSize = true;
            this.nm.Location = new System.Drawing.Point(949, 22);
            this.nm.Name = "nm";
            this.nm.Size = new System.Drawing.Size(21, 13);
            this.nm.TabIndex = 10;
            this.nm.Text = "nm";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(950, 122);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 6;
            this.button5.Text = "YZ";
            this.button5.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.btn_yz_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(950, 151);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 6;
            this.button6.Text = "XY";
            this.button6.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.btn_xz_Click);
            // 
            // bt_showall
            // 
            this.bt_showall.Location = new System.Drawing.Point(903, 43);
            this.bt_showall.Name = "bt_showall";
            this.bt_showall.Size = new System.Drawing.Size(70, 23);
            this.bt_showall.TabIndex = 11;
            this.bt_showall.Text = "Show All";
            this.bt_showall.UseVisualStyleBackColor = true;
            this.bt_showall.Click += new System.EventHandler(this.ShowAll);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(940, 196);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "Move: Shift+Left";
            // 
            // Form_DynRec
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1074, 723);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tb_slicewidth);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.bt_showall);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nm);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_slicez);
            this.Controls.Add(this.tb_slicey);
            this.Controls.Add(this.tb_slicex);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_Kfscaling);
            this.Controls.Add(this.tb_ICFscaling);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.btn_xy);
            this.Controls.Add(this.pole_calc);
            this.Controls.Add(this.openGLControl);
            this.Controls.Add(this.listViewData);
            this.Controls.Add(this.textBox_message);
            this.Controls.Add(this.btn_selec);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_DynRec";
            this.Text = "Dynamic APT reconstruction";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_selec;
        private System.Windows.Forms.TextBox textBox_message;
        private SharpGL.OpenGLControl openGLControl;
        private System.Windows.Forms.Button pole_calc;
        private System.Windows.Forms.ListView listViewData;
        private System.Windows.Forms.Button btn_xy;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_ICFscaling;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_Kfscaling;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tb_slicex;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_slicey;
        private System.Windows.Forms.TextBox tb_slicez;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox tb_slicewidth;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label nm;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button bt_showall;
        private System.Windows.Forms.Label label9;
    }
}