namespace RandomStory
{
    partial class WorldsEdit
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
            this.worldsListBox = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.добавитьМирToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьМирToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.отменаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.professionsEvilTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.itemsTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.locationsTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.professionsTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.perksTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.racesTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // worldsListBox
            // 
            this.worldsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.worldsListBox.ContextMenuStrip = this.contextMenuStrip1;
            this.worldsListBox.FormattingEnabled = true;
            this.worldsListBox.Location = new System.Drawing.Point(12, 12);
            this.worldsListBox.Name = "worldsListBox";
            this.worldsListBox.Size = new System.Drawing.Size(145, 472);
            this.worldsListBox.TabIndex = 0;
            this.worldsListBox.SelectedIndexChanged += new System.EventHandler(this.worldsListBox_SelectedIndexChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьМирToolStripMenuItem,
            this.удалитьМирToolStripMenuItem,
            this.toolStripMenuItem1,
            this.отменаToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(171, 76);
            // 
            // добавитьМирToolStripMenuItem
            // 
            this.добавитьМирToolStripMenuItem.Name = "добавитьМирToolStripMenuItem";
            this.добавитьМирToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.добавитьМирToolStripMenuItem.Text = "Добавить сеттинг";
            this.добавитьМирToolStripMenuItem.Click += new System.EventHandler(this.добавитьМирToolStripMenuItem_Click);
            // 
            // удалитьМирToolStripMenuItem
            // 
            this.удалитьМирToolStripMenuItem.Name = "удалитьМирToolStripMenuItem";
            this.удалитьМирToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.удалитьМирToolStripMenuItem.Text = "Удалить сеттинг";
            this.удалитьМирToolStripMenuItem.Click += new System.EventHandler(this.удалитьМирToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
            // 
            // отменаToolStripMenuItem
            // 
            this.отменаToolStripMenuItem.Name = "отменаToolStripMenuItem";
            this.отменаToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.отменаToolStripMenuItem.Text = "Отмена";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(163, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Название:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel6, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel5, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(166, 54);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(565, 430);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.professionsEvilTextBox);
            this.panel6.Controls.Add(this.label7);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 289);
            this.panel6.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(279, 141);
            this.panel6.TabIndex = 5;
            // 
            // professionsEvilTextBox
            // 
            this.professionsEvilTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.professionsEvilTextBox.Location = new System.Drawing.Point(0, 16);
            this.professionsEvilTextBox.Multiline = true;
            this.professionsEvilTextBox.Name = "professionsEvilTextBox";
            this.professionsEvilTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.professionsEvilTextBox.Size = new System.Drawing.Size(279, 125);
            this.professionsEvilTextBox.TabIndex = 1;
            this.professionsEvilTextBox.TextChanged += new System.EventHandler(this.professionsEvilTextBox_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(-3, 0);
            this.label7.Margin = new System.Windows.Forms.Padding(0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(113, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Элитные профессии:";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.itemsTextBox);
            this.panel5.Controls.Add(this.label6);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(282, 289);
            this.panel5.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(280, 141);
            this.panel5.TabIndex = 4;
            // 
            // itemsTextBox
            // 
            this.itemsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.itemsTextBox.Location = new System.Drawing.Point(0, 16);
            this.itemsTextBox.Multiline = true;
            this.itemsTextBox.Name = "itemsTextBox";
            this.itemsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.itemsTextBox.Size = new System.Drawing.Size(280, 125);
            this.itemsTextBox.TabIndex = 1;
            this.itemsTextBox.TextChanged += new System.EventHandler(this.itemsTextBox_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(-3, 0);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(116, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Ключевые предметы:";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.locationsTextBox);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(285, 146);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(280, 140);
            this.panel4.TabIndex = 3;
            // 
            // locationsTextBox
            // 
            this.locationsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.locationsTextBox.Location = new System.Drawing.Point(0, 16);
            this.locationsTextBox.Multiline = true;
            this.locationsTextBox.Name = "locationsTextBox";
            this.locationsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.locationsTextBox.Size = new System.Drawing.Size(280, 124);
            this.locationsTextBox.TabIndex = 1;
            this.locationsTextBox.TextChanged += new System.EventHandler(this.locationsTextBox_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(-3, 0);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Локации:";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.professionsTextBox);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 146);
            this.panel3.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(279, 140);
            this.panel3.TabIndex = 2;
            // 
            // professionsTextBox
            // 
            this.professionsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.professionsTextBox.Location = new System.Drawing.Point(0, 16);
            this.professionsTextBox.Multiline = true;
            this.professionsTextBox.Name = "professionsTextBox";
            this.professionsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.professionsTextBox.Size = new System.Drawing.Size(279, 124);
            this.professionsTextBox.TabIndex = 1;
            this.professionsTextBox.TextChanged += new System.EventHandler(this.professionsTextBox_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(-3, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Простые профессии:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.perksTextBox);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(285, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 0, 0, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(280, 140);
            this.panel2.TabIndex = 1;
            // 
            // perksTextBox
            // 
            this.perksTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.perksTextBox.Location = new System.Drawing.Point(0, 16);
            this.perksTextBox.Multiline = true;
            this.perksTextBox.Name = "perksTextBox";
            this.perksTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.perksTextBox.Size = new System.Drawing.Size(280, 124);
            this.perksTextBox.TabIndex = 1;
            this.perksTextBox.TextChanged += new System.EventHandler(this.perksTextBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(-3, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(142, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Особенности персонажей:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.racesTextBox);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(279, 140);
            this.panel1.TabIndex = 0;
            // 
            // racesTextBox
            // 
            this.racesTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.racesTextBox.Location = new System.Drawing.Point(0, 16);
            this.racesTextBox.Multiline = true;
            this.racesTextBox.Name = "racesTextBox";
            this.racesTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.racesTextBox.Size = new System.Drawing.Size(279, 124);
            this.racesTextBox.TabIndex = 1;
            this.racesTextBox.TextChanged += new System.EventHandler(this.racesTextBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-3, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Расы:";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTextBox.Location = new System.Drawing.Point(166, 28);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(565, 20);
            this.nameTextBox.TabIndex = 3;
            this.nameTextBox.TextChanged += new System.EventHandler(this.nameTextBox_TextChanged);
            // 
            // WorldsEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 500);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.worldsListBox);
            this.Name = "WorldsEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Сеттинги";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.WorldsEdit_FormClosed);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox worldsListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox locationsTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox professionsTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox perksTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox racesTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem добавитьМирToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem удалитьМирToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem отменаToolStripMenuItem;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TextBox itemsTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.TextBox professionsEvilTextBox;
        private System.Windows.Forms.Label label7;
    }
}