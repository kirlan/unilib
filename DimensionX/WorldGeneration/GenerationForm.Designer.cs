﻿namespace WorldGeneration
{
    partial class GenerationForm
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
            this.components = new System.ComponentModel.Container();
            this.StartGenerationButton = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.AgesView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.ChunkSizeUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.ChunksCountBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.WorkingAreaBox = new System.Windows.Forms.ComboBox();
            this.mapProperties1 = new WorldGeneration.MapProperties();
            this.epochProperties1 = new WorldGeneration.EpochProperties();
            this.groupBox5.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChunkSizeUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // StartGenerationButton
            // 
            this.StartGenerationButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.StartGenerationButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.StartGenerationButton.Location = new System.Drawing.Point(732, 362);
            this.StartGenerationButton.Name = "StartGenerationButton";
            this.StartGenerationButton.Size = new System.Drawing.Size(162, 31);
            this.StartGenerationButton.TabIndex = 8;
            this.StartGenerationButton.Text = "Start generation!";
            this.StartGenerationButton.UseVisualStyleBackColor = true;
            this.StartGenerationButton.Click += new System.EventHandler(this.StartGeneration_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 1;
            this.toolTip1.AutoPopDelay = 100000000;
            this.toolTip1.InitialDelay = 0;
            this.toolTip1.ReshowDelay = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.AgesView);
            this.groupBox5.Controls.Add(this.tableLayoutPanel1);
            this.groupBox5.Controls.Add(this.epochProperties1);
            this.groupBox5.Location = new System.Drawing.Point(456, 4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(438, 352);
            this.groupBox5.TabIndex = 10;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "History";
            // 
            // AgesView
            // 
            this.AgesView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AgesView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.AgesView.FullRowSelect = true;
            this.AgesView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.AgesView.HideSelection = false;
            this.AgesView.Location = new System.Drawing.Point(6, 16);
            this.AgesView.MultiSelect = false;
            this.AgesView.Name = "AgesView";
            this.AgesView.ShowItemToolTips = true;
            this.AgesView.Size = new System.Drawing.Size(425, 80);
            this.AgesView.TabIndex = 3;
            this.AgesView.UseCompatibleStateImageBehavior = false;
            this.AgesView.View = System.Windows.Forms.View.Details;
            this.AgesView.SelectedIndexChanged += new System.EventHandler(this.AgesView_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Age";
            this.columnHeader1.Width = 89;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Progress";
            this.columnHeader2.Width = 118;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Races";
            this.columnHeader3.Width = 148;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Length";
            this.columnHeader4.Width = 46;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.button1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.button2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.button3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.button4, 3, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 95);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(426, 30);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Location = new System.Drawing.Point(3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 24);
            this.button1.TabIndex = 0;
            this.button1.Text = "Add new age";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.AddNewAge_Click);
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(109, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 24);
            this.button2.TabIndex = 1;
            this.button2.Text = "Remove age";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(215, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 24);
            this.button3.TabIndex = 2;
            this.button3.Text = "Move earlier";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button4.Enabled = false;
            this.button4.Location = new System.Drawing.Point(321, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(100, 24);
            this.button4.TabIndex = 3;
            this.button4.Text = "Move later";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.WorkingAreaBox);
            this.groupBox6.Controls.Add(this.label3);
            this.groupBox6.Controls.Add(this.ChunksCountBox);
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Controls.Add(this.ChunkSizeUpDown);
            this.groupBox6.Controls.Add(this.label1);
            this.groupBox6.Location = new System.Drawing.Point(12, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(438, 68);
            this.groupBox6.TabIndex = 1;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Planet";
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.Location = new System.Drawing.Point(16, 370);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(118, 17);
            this.checkBox1.TabIndex = 17;
            this.checkBox1.Text = "Advanced Mode";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button5
            // 
            this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button5.Location = new System.Drawing.Point(456, 362);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(162, 31);
            this.button5.TabIndex = 18;
            this.button5.Text = "Load world preset";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.LoadWorldPreset_Click);
            // 
            // button6
            // 
            this.button6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button6.Location = new System.Drawing.Point(288, 362);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(162, 31);
            this.button6.TabIndex = 19;
            this.button6.Text = "Save world preset";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.SaveWorldPreset_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "xml";
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "XML files|*.xml|All files|*.*";
            this.openFileDialog1.Title = "Load Dimension X world preset file";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "xml";
            this.saveFileDialog1.Filter = "XML files|*.xml|All files|*.*";
            this.saveFileDialog1.Title = "Save Dimension X world preset file";
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "Please, select a folder for temporary app data (about 10-50Mb total):";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Chunk size:";
            // 
            // ChunkSizeUpDown
            // 
            this.ChunkSizeUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ChunkSizeUpDown.Location = new System.Drawing.Point(74, 15);
            this.ChunkSizeUpDown.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.ChunkSizeUpDown.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.ChunkSizeUpDown.Name = "ChunkSizeUpDown";
            this.ChunkSizeUpDown.Size = new System.Drawing.Size(142, 20);
            this.ChunkSizeUpDown.TabIndex = 2;
            this.ChunkSizeUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ChunkSizeUpDown.Value = new decimal(new int[] {
            800,
            0,
            0,
            0});
            this.ChunkSizeUpDown.ValueChanged += new System.EventHandler(this.ChunkSizeUpDown_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(222, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Chunks count:";
            // 
            // ChunksCountBox
            // 
            this.ChunksCountBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ChunksCountBox.FormattingEnabled = true;
            this.ChunksCountBox.Items.AddRange(new object[] {
            "6x1x1",
            "6x3x3",
            "6x5x5",
            "6x7x7",
            "6x9x9",
            "6x17x17",
            "6x25x25"});
            this.ChunksCountBox.Location = new System.Drawing.Point(304, 14);
            this.ChunksCountBox.Name = "ChunksCountBox";
            this.ChunksCountBox.Size = new System.Drawing.Size(128, 21);
            this.ChunksCountBox.TabIndex = 4;
            this.ChunksCountBox.SelectedIndexChanged += new System.EventHandler(this.ChunksCountBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Available territory:";
            // 
            // WorkingAreaBox
            // 
            this.WorkingAreaBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WorkingAreaBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.WorkingAreaBox.FormattingEnabled = true;
            this.WorkingAreaBox.Items.AddRange(new object[] {
            "Segment 1/6",
            "Equatorial hemisphere",
            "Polar hemisphere",
            "Whole sphere"});
            this.WorkingAreaBox.Location = new System.Drawing.Point(102, 40);
            this.WorkingAreaBox.Name = "WorkingAreaBox";
            this.WorkingAreaBox.Size = new System.Drawing.Size(114, 21);
            this.WorkingAreaBox.TabIndex = 11;
            this.WorkingAreaBox.SelectedIndexChanged += new System.EventHandler(this.WorkingAreaBox_SelectedIndexChanged);
            // 
            // mapProperties1
            // 
            this.mapProperties1.AdvancedMode = true;
            this.mapProperties1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.mapProperties1.BackColor = System.Drawing.Color.White;
            this.mapProperties1.ChunksCount = 5;
            this.mapProperties1.ChunkSize = 800;
            this.mapProperties1.ContinentsCount = 1;
            this.mapProperties1.Enabled = false;
            this.mapProperties1.LandMassesDiversity = 14;
            this.mapProperties1.LandsDiversity = 40;
            this.mapProperties1.Location = new System.Drawing.Point(7, 90);
            this.mapProperties1.Name = "mapProperties1";
            this.mapProperties1.Size = new System.Drawing.Size(443, 229);
            this.mapProperties1.StatesCount = 60;
            this.mapProperties1.TabIndex = 16;
            this.mapProperties1.WaterPercent = 66;
            this.mapProperties1.WorkingArea = LandscapeGeneration.PlanetBuilder.WorkingArea.WholeSphere;
            // 
            // epochProperties1
            // 
            this.epochProperties1.AdvancedMode = false;
            this.epochProperties1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.epochProperties1.BackColor = System.Drawing.Color.White;
            this.epochProperties1.Enabled = false;
            this.epochProperties1.Epoch = null;
            this.epochProperties1.Location = new System.Drawing.Point(5, 131);
            this.epochProperties1.Name = "epochProperties1";
            this.epochProperties1.Size = new System.Drawing.Size(425, 215);
            this.epochProperties1.TabIndex = 4;
            this.epochProperties1.UpdateEvent += new WorldGeneration.EpochProperties.UpdateEventHandler(this.epochProperties1_UpdateEvent);
            // 
            // GenerationForm
            // 
            this.AcceptButton = this.StartGenerationButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(906, 401);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.mapProperties1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.StartGenerationButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GenerationForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Building World";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GenerationForm_FormClosing);
            this.groupBox5.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChunkSizeUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartGenerationButton;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ListView AgesView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private EpochProperties epochProperties1;
        private MapProperties mapProperties1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown ChunkSizeUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ChunksCountBox;
        private System.Windows.Forms.ComboBox WorkingAreaBox;
    }
}