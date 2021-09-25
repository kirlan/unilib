namespace Persona.Collections.Controls
{
    partial class CollectionsList
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip_Collections = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem13 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem14 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem15 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem16 = new System.Windows.Forms.ToolStripMenuItem();
            this.CollectionsListBox = new System.Windows.Forms.ListBox();
            this.contextMenuStrip_Collections.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip_Collections
            // 
            this.contextMenuStrip_Collections.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem13,
            this.toolStripMenuItem14,
            this.toolStripMenuItem15,
            this.toolStripSeparator3,
            this.toolStripMenuItem16});
            this.contextMenuStrip_Collections.Name = "contextMenuStrip1";
            this.contextMenuStrip_Collections.Size = new System.Drawing.Size(216, 120);
            // 
            // toolStripMenuItem13
            // 
            this.toolStripMenuItem13.Name = "toolStripMenuItem13";
            this.toolStripMenuItem13.Size = new System.Drawing.Size(215, 22);
            this.toolStripMenuItem13.Text = "Добавить новый объект...";
            this.toolStripMenuItem13.Click += new System.EventHandler(this.CollectionAdd_Click);
            // 
            // toolStripMenuItem14
            // 
            this.toolStripMenuItem14.Name = "toolStripMenuItem14";
            this.toolStripMenuItem14.Size = new System.Drawing.Size(215, 22);
            this.toolStripMenuItem14.Text = "Редактировать";
            this.toolStripMenuItem14.Click += new System.EventHandler(this.CollectionEdit_Click);
            // 
            // toolStripMenuItem15
            // 
            this.toolStripMenuItem15.Name = "toolStripMenuItem15";
            this.toolStripMenuItem15.Size = new System.Drawing.Size(215, 22);
            this.toolStripMenuItem15.Text = "Удалить объект";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(212, 6);
            // 
            // toolStripMenuItem16
            // 
            this.toolStripMenuItem16.Name = "toolStripMenuItem16";
            this.toolStripMenuItem16.Size = new System.Drawing.Size(215, 22);
            this.toolStripMenuItem16.Text = "Отмена";
            // 
            // CollectionsListBox
            // 
            this.CollectionsListBox.ContextMenuStrip = this.contextMenuStrip_Collections;
            this.CollectionsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CollectionsListBox.FormattingEnabled = true;
            this.CollectionsListBox.IntegralHeight = false;
            this.CollectionsListBox.Location = new System.Drawing.Point(0, 0);
            this.CollectionsListBox.Name = "CollectionsListBox";
            this.CollectionsListBox.Size = new System.Drawing.Size(150, 348);
            this.CollectionsListBox.TabIndex = 3;
            this.CollectionsListBox.SelectedIndexChanged += new System.EventHandler(this.CollectionsListBox_SelectedIndexChanged);
            this.CollectionsListBox.DoubleClick += new System.EventHandler(this.CollectionEdit_Click);
            // 
            // CollectionsList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CollectionsListBox);
            this.Name = "CollectionsList";
            this.Size = new System.Drawing.Size(150, 348);
            this.contextMenuStrip_Collections.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Collections;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem13;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem14;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem15;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem16;
        private System.Windows.Forms.ListBox CollectionsListBox;
    }
}
