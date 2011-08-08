namespace COM_test
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.InitialSize = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.FinalSize = new System.Windows.Forms.ComboBox();
            this.ContinentsCount = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.IslandsCount = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.WaterPercent = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label6 = new System.Windows.Forms.Label();
            this.debug = new System.Windows.Forms.CheckBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.fractalMapView1 = new MapVis.FractalMapView2D();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ContinentsCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IslandsCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WaterPercent)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(719, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(176, 67);
            this.button1.TabIndex = 1;
            this.button1.Text = "BUILD WORLD";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(719, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Initial size:";
            // 
            // InitialSize
            // 
            this.InitialSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.InitialSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.InitialSize.FormattingEnabled = true;
            this.InitialSize.Items.AddRange(new object[] {
            "1 - 20x6",
            "2 - 40x12",
            "3 - 80x24",
            "4 - 160x48",
            "5 - 320x96",
            "6 - 640x192",
            "7 - 1280x384"});
            this.InitialSize.Location = new System.Drawing.Point(786, 92);
            this.InitialSize.Name = "InitialSize";
            this.InitialSize.Size = new System.Drawing.Size(109, 21);
            this.InitialSize.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(719, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Final size:";
            // 
            // FinalSize
            // 
            this.FinalSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FinalSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FinalSize.FormattingEnabled = true;
            this.FinalSize.Items.AddRange(new object[] {
            "1 - 20x6",
            "2 - 40x12",
            "3 - 80x24",
            "4 - 160x48",
            "5 - 320x96",
            "6 - 640x192",
            "7 - 1280x384"});
            this.FinalSize.Location = new System.Drawing.Point(786, 120);
            this.FinalSize.Name = "FinalSize";
            this.FinalSize.Size = new System.Drawing.Size(109, 21);
            this.FinalSize.TabIndex = 5;
            // 
            // ContinentsCount
            // 
            this.ContinentsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ContinentsCount.Location = new System.Drawing.Point(786, 147);
            this.ContinentsCount.Name = "ContinentsCount";
            this.ContinentsCount.Size = new System.Drawing.Size(109, 20);
            this.ContinentsCount.TabIndex = 6;
            this.ContinentsCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ContinentsCount.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(719, 149);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Continents:";
            // 
            // IslandsCount
            // 
            this.IslandsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.IslandsCount.Location = new System.Drawing.Point(786, 175);
            this.IslandsCount.Name = "IslandsCount";
            this.IslandsCount.Size = new System.Drawing.Size(109, 20);
            this.IslandsCount.TabIndex = 8;
            this.IslandsCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.IslandsCount.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(719, 177);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Islands:";
            // 
            // WaterPercent
            // 
            this.WaterPercent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.WaterPercent.Location = new System.Drawing.Point(786, 204);
            this.WaterPercent.Name = "WaterPercent";
            this.WaterPercent.Size = new System.Drawing.Size(109, 20);
            this.WaterPercent.TabIndex = 10;
            this.WaterPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.WaterPercent.Value = new decimal(new int[] {
            70,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(719, 206);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Water %";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(25, 204);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(669, 23);
            this.progressBar1.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.BackColor = System.Drawing.Color.Black;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(22, 158);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(672, 19);
            this.label6.TabIndex = 13;
            this.label6.Text = "label6";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // debug
            // 
            this.debug.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.debug.Location = new System.Drawing.Point(719, 230);
            this.debug.Name = "debug";
            this.debug.Size = new System.Drawing.Size(176, 24);
            this.debug.TabIndex = 14;
            this.debug.Text = "Debug Mode";
            this.debug.UseVisualStyleBackColor = true;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "jpg";
            this.saveFileDialog1.Filter = "JPEG files|*.jpg";
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(719, 318);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(176, 38);
            this.button2.TabIndex = 15;
            this.button2.Text = "Save map";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(267, 403);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 16;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(12, 379);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 17;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // fractalMapView1
            // 
            this.fractalMapView1.FullProection = true;
            this.fractalMapView1.Location = new System.Drawing.Point(12, 12);
            this.fractalMapView1.Mode = MapVis.FractalMapView2D.DisplayMode.Height;
            this.fractalMapView1.Name = "fractalMapView1";
            this.fractalMapView1.Size = new System.Drawing.Size(692, 346);
            this.fractalMapView1.TabIndex = 0;
            this.fractalMapView1.RegionSelectedEvent += new System.EventHandler<MapVis.FractalMapView2D.RegionSelectedEventArgs>(this.fractalMapView1_RegionSelectedEvent);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(12, 403);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 19;
            this.button5.Text = "button5";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(12, 428);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 20;
            this.button6.Text = "button6";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(492, 379);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "label7";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 463);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.debug);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.WaterPercent);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.IslandsCount);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ContinentsCount);
            this.Controls.Add(this.FinalSize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.InitialSize);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.fractalMapView1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.ContinentsCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IslandsCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WaterPercent)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MapVis.FractalMapView2D fractalMapView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox InitialSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox FinalSize;
        private System.Windows.Forms.NumericUpDown ContinentsCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown IslandsCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown WaterPercent;
        private System.Windows.Forms.Label label5;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox debug;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label7;
    }
}

