namespace APT_calibrator_tool
{
    partial class FormImageViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImageViewer));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnMeasureDistance = new System.Windows.Forms.Button();
            this.btnMeasureAngle = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblDistance = new System.Windows.Forms.Label();
            this.lblAngle = new System.Windows.Forms.Label();
            this.btnScaleBar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCollectSeeds = new System.Windows.Forms.Button();
            this.textBoxSeeds = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 68);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1137, 560);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btnMeasureDistance
            // 
            this.btnMeasureDistance.Location = new System.Drawing.Point(12, 634);
            this.btnMeasureDistance.Name = "btnMeasureDistance";
            this.btnMeasureDistance.Size = new System.Drawing.Size(292, 23);
            this.btnMeasureDistance.TabIndex = 1;
            this.btnMeasureDistance.Text = "Measure Distance（Click start and end points）";
            this.btnMeasureDistance.UseVisualStyleBackColor = true;
            // 
            // btnMeasureAngle
            // 
            this.btnMeasureAngle.Location = new System.Drawing.Point(319, 634);
            this.btnMeasureAngle.Name = "btnMeasureAngle";
            this.btnMeasureAngle.Size = new System.Drawing.Size(326, 23);
            this.btnMeasureAngle.TabIndex = 2;
            this.btnMeasureAngle.Text = "Measure angle";
            this.btnMeasureAngle.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(1007, 634);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(142, 23);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clean all";
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // lblDistance
            // 
            this.lblDistance.AutoSize = true;
            this.lblDistance.Location = new System.Drawing.Point(756, 639);
            this.lblDistance.Name = "lblDistance";
            this.lblDistance.Size = new System.Drawing.Size(35, 13);
            this.lblDistance.TabIndex = 4;
            this.lblDistance.Text = "label1";
            // 
            // lblAngle
            // 
            this.lblAngle.AutoSize = true;
            this.lblAngle.Location = new System.Drawing.Point(966, 639);
            this.lblAngle.Name = "lblAngle";
            this.lblAngle.Size = new System.Drawing.Size(35, 13);
            this.lblAngle.TabIndex = 5;
            this.lblAngle.Text = "label1";
            // 
            // btnScaleBar
            // 
            this.btnScaleBar.Location = new System.Drawing.Point(12, 22);
            this.btnScaleBar.Name = "btnScaleBar";
            this.btnScaleBar.Size = new System.Drawing.Size(359, 23);
            this.btnScaleBar.TabIndex = 7;
            this.btnScaleBar.Text = "Mark Scale Bar （Click start and end points）";
            this.btnScaleBar.UseVisualStyleBackColor = true;
            this.btnScaleBar.Click += new System.EventHandler(this.btnScaleBar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(667, 639);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Distance:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(887, 639);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Angle:";
            // 
            // btnCollectSeeds
            // 
            this.btnCollectSeeds.Location = new System.Drawing.Point(414, 22);
            this.btnCollectSeeds.Name = "btnCollectSeeds";
            this.btnCollectSeeds.Size = new System.Drawing.Size(145, 23);
            this.btnCollectSeeds.TabIndex = 9;
            this.btnCollectSeeds.Text = "Pick Golden Seeds";
            this.btnCollectSeeds.UseVisualStyleBackColor = true;
            // 
            // textBoxSeeds
            // 
            this.textBoxSeeds.Location = new System.Drawing.Point(1036, 77);
            this.textBoxSeeds.Multiline = true;
            this.textBoxSeeds.Name = "textBoxSeeds";
            this.textBoxSeeds.Size = new System.Drawing.Size(104, 106);
            this.textBoxSeeds.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1033, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Seeds:";
            // 
            // FormImageViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1161, 664);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxSeeds);
            this.Controls.Add(this.btnCollectSeeds);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnScaleBar);
            this.Controls.Add(this.lblAngle);
            this.Controls.Add(this.lblDistance);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnMeasureAngle);
            this.Controls.Add(this.btnMeasureDistance);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormImageViewer";
            this.Text = "Pick Golden Seeds";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnMeasureDistance;
        private System.Windows.Forms.Button btnMeasureAngle;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lblDistance;
        private System.Windows.Forms.Label lblAngle;
        private System.Windows.Forms.Button btnScaleBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCollectSeeds;
        private System.Windows.Forms.TextBox textBoxSeeds;
        private System.Windows.Forms.Label label3;
    }
}