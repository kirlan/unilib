namespace Persona.Core.Controls
{
    partial class CollectedItemsView
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
            this.contextMenuStrip_CollectedObjects = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem22 = new System.Windows.Forms.ToolStripMenuItem();
            this.клонироватьToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem23 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem24 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem25 = new System.Windows.Forms.ToolStripMenuItem();
            this.CollectedItemsListView = new System.Windows.Forms.ListView();
            this.columnHeader24 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader28 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader25 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader27 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader26 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip_CollectedObjects.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip_CollectedObjects
            // 
            this.contextMenuStrip_CollectedObjects.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem22,
            this.клонироватьToolStripMenuItem3,
            this.toolStripMenuItem23,
            this.toolStripMenuItem24,
            this.toolStripSeparator5,
            this.toolStripMenuItem25});
            this.contextMenuStrip_CollectedObjects.Name = "contextMenuStrip1";
            this.contextMenuStrip_CollectedObjects.Size = new System.Drawing.Size(234, 120);
            // 
            // toolStripMenuItem22
            // 
            this.toolStripMenuItem22.Name = "toolStripMenuItem22";
            this.toolStripMenuItem22.Size = new System.Drawing.Size(233, 22);
            this.toolStripMenuItem22.Text = "Добавить новую вариацию...";
            this.toolStripMenuItem22.Click += new System.EventHandler(this.AddCollectedObject_Click);
            // 
            // клонироватьToolStripMenuItem3
            // 
            this.клонироватьToolStripMenuItem3.Name = "клонироватьToolStripMenuItem3";
            this.клонироватьToolStripMenuItem3.Size = new System.Drawing.Size(233, 22);
            this.клонироватьToolStripMenuItem3.Text = "Клонировать";
            this.клонироватьToolStripMenuItem3.Click += new System.EventHandler(this.CloneCollectedObject_Click);
            // 
            // toolStripMenuItem23
            // 
            this.toolStripMenuItem23.Name = "toolStripMenuItem23";
            this.toolStripMenuItem23.Size = new System.Drawing.Size(233, 22);
            this.toolStripMenuItem23.Text = "Редактировать";
            this.toolStripMenuItem23.Click += new System.EventHandler(this.EditCollectedObject_Click);
            // 
            // toolStripMenuItem24
            // 
            this.toolStripMenuItem24.Name = "toolStripMenuItem24";
            this.toolStripMenuItem24.Size = new System.Drawing.Size(233, 22);
            this.toolStripMenuItem24.Text = "Удалить вариацию";
            this.toolStripMenuItem24.Click += new System.EventHandler(this.DeleteCollectedObject_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(230, 6);
            // 
            // toolStripMenuItem25
            // 
            this.toolStripMenuItem25.Name = "toolStripMenuItem25";
            this.toolStripMenuItem25.Size = new System.Drawing.Size(233, 22);
            this.toolStripMenuItem25.Text = "Отмена";
            // 
            // CollectedItemsListView
            // 
            this.CollectedItemsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader24,
            this.columnHeader28,
            this.columnHeader25,
            this.columnHeader27,
            this.columnHeader26});
            this.CollectedItemsListView.ContextMenuStrip = this.contextMenuStrip_CollectedObjects;
            this.CollectedItemsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CollectedItemsListView.FullRowSelect = true;
            this.CollectedItemsListView.GridLines = true;
            this.CollectedItemsListView.Location = new System.Drawing.Point(0, 0);
            this.CollectedItemsListView.Name = "CollectedItemsListView";
            this.CollectedItemsListView.Size = new System.Drawing.Size(850, 494);
            this.CollectedItemsListView.TabIndex = 4;
            this.CollectedItemsListView.UseCompatibleStateImageBehavior = false;
            this.CollectedItemsListView.View = System.Windows.Forms.View.Details;
            this.CollectedItemsListView.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.CollectedItemsListView_ColumnWidthChanged);
            this.CollectedItemsListView.ItemActivate += new System.EventHandler(this.EditCollectedObject_Click);
            this.CollectedItemsListView.SizeChanged += new System.EventHandler(this.CollectedItemsListView_SizeChanged);
            // 
            // columnHeader24
            // 
            this.columnHeader24.Text = "ID";
            // 
            // columnHeader28
            // 
            this.columnHeader28.Text = "Имя";
            // 
            // columnHeader25
            // 
            this.columnHeader25.Text = "Частота";
            // 
            // columnHeader27
            // 
            this.columnHeader27.Text = "Условия";
            this.columnHeader27.Width = 230;
            // 
            // columnHeader26
            // 
            this.columnHeader26.Text = "Свойства";
            this.columnHeader26.Width = 230;
            // 
            // CollectedItemsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CollectedItemsListView);
            this.Name = "CollectedItemsView";
            this.Size = new System.Drawing.Size(850, 494);
            this.contextMenuStrip_CollectedObjects.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_CollectedObjects;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem22;
        private System.Windows.Forms.ToolStripMenuItem клонироватьToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem23;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem24;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem25;
        private System.Windows.Forms.ListView CollectedItemsListView;
        private System.Windows.Forms.ColumnHeader columnHeader24;
        private System.Windows.Forms.ColumnHeader columnHeader28;
        private System.Windows.Forms.ColumnHeader columnHeader25;
        private System.Windows.Forms.ColumnHeader columnHeader27;
        private System.Windows.Forms.ColumnHeader columnHeader26;
    }
}
