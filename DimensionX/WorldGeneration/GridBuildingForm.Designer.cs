namespace WorldGeneration
{
    partial class GridBuildingForm
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
            this.NoGridPanel = new System.Windows.Forms.Panel();
            this.PointsCount = new System.Windows.Forms.ComboBox();
            this.NoGridHeight = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.NoGridWidth = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.GridPanel = new System.Windows.Forms.Panel();
            this.GridSquare = new System.Windows.Forms.CheckBox();
            this.GridHeight = new System.Windows.Forms.ComboBox();
            this.GridWidth = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Looped = new System.Windows.Forms.CheckBox();
            this.NoGridPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NoGridHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NoGridWidth)).BeginInit();
            this.GridPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // NoGridPanel
            // 
            this.NoGridPanel.Controls.Add(this.PointsCount);
            this.NoGridPanel.Controls.Add(this.NoGridHeight);
            this.NoGridPanel.Controls.Add(this.label11);
            this.NoGridPanel.Controls.Add(this.NoGridWidth);
            this.NoGridPanel.Controls.Add(this.label10);
            this.NoGridPanel.Controls.Add(this.label4);
            this.NoGridPanel.Location = new System.Drawing.Point(7, 74);
            this.NoGridPanel.Name = "NoGridPanel";
            this.NoGridPanel.Size = new System.Drawing.Size(336, 32);
            this.NoGridPanel.TabIndex = 5;
            // 
            // PointsCount
            // 
            this.PointsCount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PointsCount.FormattingEnabled = true;
            this.PointsCount.Items.AddRange(new object[] {
            "5K",
            "10K",
            "25K",
            "50K"});
            this.PointsCount.Location = new System.Drawing.Point(78, 6);
            this.PointsCount.Name = "PointsCount";
            this.PointsCount.Size = new System.Drawing.Size(66, 21);
            this.PointsCount.TabIndex = 7;
            this.PointsCount.SelectedIndexChanged += new System.EventHandler(this.PointsCount_SelectedIndexChanged);
            // 
            // NoGridHeight
            // 
            this.NoGridHeight.Location = new System.Drawing.Point(295, 7);
            this.NoGridHeight.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.NoGridHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NoGridHeight.Name = "NoGridHeight";
            this.NoGridHeight.Size = new System.Drawing.Size(34, 20);
            this.NoGridHeight.TabIndex = 6;
            this.NoGridHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NoGridHeight.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.NoGridHeight.ValueChanged += new System.EventHandler(this.NoGridHeight_ValueChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(277, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(12, 13);
            this.label11.TabIndex = 5;
            this.label11.Text = "x";
            // 
            // NoGridWidth
            // 
            this.NoGridWidth.Location = new System.Drawing.Point(237, 7);
            this.NoGridWidth.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.NoGridWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NoGridWidth.Name = "NoGridWidth";
            this.NoGridWidth.Size = new System.Drawing.Size(34, 20);
            this.NoGridWidth.TabIndex = 4;
            this.NoGridWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NoGridWidth.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.NoGridWidth.ValueChanged += new System.EventHandler(this.NoGridWidth_ValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(150, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(81, 13);
            this.label10.TabIndex = 2;
            this.label10.Text = "Map proportion:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Points count:";
            // 
            // GridPanel
            // 
            this.GridPanel.Controls.Add(this.GridSquare);
            this.GridPanel.Controls.Add(this.GridHeight);
            this.GridPanel.Controls.Add(this.GridWidth);
            this.GridPanel.Controls.Add(this.label1);
            this.GridPanel.Location = new System.Drawing.Point(85, 74);
            this.GridPanel.Name = "GridPanel";
            this.GridPanel.Size = new System.Drawing.Size(180, 32);
            this.GridPanel.TabIndex = 4;
            // 
            // GridSquare
            // 
            this.GridSquare.AutoSize = true;
            this.GridSquare.Checked = true;
            this.GridSquare.CheckState = System.Windows.Forms.CheckState.Checked;
            this.GridSquare.Location = new System.Drawing.Point(112, 11);
            this.GridSquare.Name = "GridSquare";
            this.GridSquare.Size = new System.Drawing.Size(15, 14);
            this.GridSquare.TabIndex = 6;
            this.GridSquare.UseVisualStyleBackColor = true;
            this.GridSquare.CheckedChanged += new System.EventHandler(this.GridSquare_CheckedChanged);
            // 
            // GridHeight
            // 
            this.GridHeight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GridHeight.Enabled = false;
            this.GridHeight.FormattingEnabled = true;
            this.GridHeight.Items.AddRange(new object[] {
            "24",
            "32",
            "48",
            "64",
            "80",
            "96",
            "128",
            "144",
            "160",
            "200",
            "240",
            "256"});
            this.GridHeight.Location = new System.Drawing.Point(130, 6);
            this.GridHeight.Name = "GridHeight";
            this.GridHeight.Size = new System.Drawing.Size(47, 21);
            this.GridHeight.TabIndex = 5;
            this.GridHeight.SelectedIndexChanged += new System.EventHandler(this.GridHeight_SelectedIndexChanged);
            // 
            // GridWidth
            // 
            this.GridWidth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GridWidth.FormattingEnabled = true;
            this.GridWidth.Items.AddRange(new object[] {
            "24",
            "32",
            "48",
            "64",
            "80",
            "96",
            "128",
            "144",
            "160",
            "200",
            "240",
            "256"});
            this.GridWidth.Location = new System.Drawing.Point(59, 6);
            this.GridWidth.Name = "GridWidth";
            this.GridWidth.Size = new System.Drawing.Size(47, 21);
            this.GridWidth.TabIndex = 4;
            this.GridWidth.SelectedIndexChanged += new System.EventHandler(this.GridWidth_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Grid size:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.radioButton1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.radioButton3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.radioButton2, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 25);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(327, 43);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButton1.Enabled = false;
            this.radioButton1.Location = new System.Drawing.Point(3, 3);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(103, 37);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.Text = "Square\r\n(Not yet)";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // radioButton3
            // 
            this.radioButton3.Checked = true;
            this.radioButton3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButton3.Location = new System.Drawing.Point(221, 3);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(103, 37);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "No grid \r\n(random points)";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButton2.Location = new System.Drawing.Point(112, 3);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(103, 37);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "Hexagonal\r\n(Atlantis type)";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Grid Type:";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(113, 172);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(125, 34);
            this.button1.TabIndex = 7;
            this.button1.Text = "Build grid";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "dxz";
            this.saveFileDialog1.Filter = "Grid files|*.dxz|All files|*.*";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Description:";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Location = new System.Drawing.Point(20, 133);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(311, 31);
            this.panel1.TabIndex = 11;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(72, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(232, 20);
            this.textBox1.TabIndex = 11;
            // 
            // Looped
            // 
            this.Looped.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Looped.Location = new System.Drawing.Point(117, 112);
            this.Looped.Name = "Looped";
            this.Looped.Size = new System.Drawing.Size(117, 17);
            this.Looped.TabIndex = 12;
            this.Looped.Text = "Looped horisontally";
            this.Looped.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Looped.UseVisualStyleBackColor = true;
            this.Looped.CheckedChanged += new System.EventHandler(this.Looped_CheckedChanged);
            // 
            // GridBuildingForm
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 216);
            this.Controls.Add(this.Looped);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.NoGridPanel);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.GridPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GridBuildingForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Building Grid";
            this.NoGridPanel.ResumeLayout(false);
            this.NoGridPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NoGridHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NoGridWidth)).EndInit();
            this.GridPanel.ResumeLayout(false);
            this.GridPanel.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel NoGridPanel;
        private System.Windows.Forms.ComboBox PointsCount;
        private System.Windows.Forms.NumericUpDown NoGridHeight;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown NoGridWidth;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel GridPanel;
        private System.Windows.Forms.CheckBox GridSquare;
        private System.Windows.Forms.ComboBox GridHeight;
        private System.Windows.Forms.ComboBox GridWidth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox Looped;
    }
}