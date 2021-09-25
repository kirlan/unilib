namespace Persona.Core.Controls
{
    partial class MilesonesView
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
            this.contextMenuStrip_Milestones = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem30 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem31 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem32 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem33 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem34 = new System.Windows.Forms.ToolStripMenuItem();
            this.MilestonesListView = new System.Windows.Forms.ListView();
            this.columnHeader29 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader30 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader31 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader32 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader33 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip_Milestones.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip_Milestones
            // 
            this.contextMenuStrip_Milestones.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem30,
            this.toolStripMenuItem32,
            this.toolStripMenuItem31,
            this.toolStripMenuItem33,
            this.toolStripSeparator7,
            this.toolStripMenuItem34});
            this.contextMenuStrip_Milestones.Name = "contextMenuStrip1";
            this.contextMenuStrip_Milestones.Size = new System.Drawing.Size(201, 142);
            // 
            // toolStripMenuItem30
            // 
            this.toolStripMenuItem30.Name = "toolStripMenuItem30";
            this.toolStripMenuItem30.Size = new System.Drawing.Size(200, 22);
            this.toolStripMenuItem30.Text = "Добавить новую веху...";
            this.toolStripMenuItem30.Click += new System.EventHandler(this.AddMilestone_Click);
            // 
            // toolStripMenuItem31
            // 
            this.toolStripMenuItem31.Name = "toolStripMenuItem31";
            this.toolStripMenuItem31.Size = new System.Drawing.Size(200, 22);
            this.toolStripMenuItem31.Text = "Клонировать";
            this.toolStripMenuItem31.Click += new System.EventHandler(this.CloneMilestone_Click);
            // 
            // toolStripMenuItem32
            // 
            this.toolStripMenuItem32.Name = "toolStripMenuItem32";
            this.toolStripMenuItem32.Size = new System.Drawing.Size(200, 22);
            this.toolStripMenuItem32.Text = "Редактировать";
            this.toolStripMenuItem32.Click += new System.EventHandler(this.EditMilestone_Click);
            // 
            // toolStripMenuItem33
            // 
            this.toolStripMenuItem33.Name = "toolStripMenuItem33";
            this.toolStripMenuItem33.Size = new System.Drawing.Size(200, 22);
            this.toolStripMenuItem33.Text = "Удалить веху";
            this.toolStripMenuItem33.Click += new System.EventHandler(this.DeleteMilestone_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(197, 6);
            // 
            // toolStripMenuItem34
            // 
            this.toolStripMenuItem34.Name = "toolStripMenuItem34";
            this.toolStripMenuItem34.Size = new System.Drawing.Size(200, 22);
            this.toolStripMenuItem34.Text = "Отмена";
            // 
            // MilestonesListView
            // 
            this.MilestonesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader29,
            this.columnHeader30,
            this.columnHeader31,
            this.columnHeader32,
            this.columnHeader33});
            this.MilestonesListView.ContextMenuStrip = this.contextMenuStrip_Milestones;
            this.MilestonesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MilestonesListView.FullRowSelect = true;
            this.MilestonesListView.GridLines = true;
            this.MilestonesListView.Location = new System.Drawing.Point(0, 0);
            this.MilestonesListView.Name = "MilestonesListView";
            this.MilestonesListView.Size = new System.Drawing.Size(808, 509);
            this.MilestonesListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.MilestonesListView.TabIndex = 8;
            this.MilestonesListView.UseCompatibleStateImageBehavior = false;
            this.MilestonesListView.View = System.Windows.Forms.View.Details;
            this.MilestonesListView.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.MilestonesListView_ColumnWidthChanged);
            this.MilestonesListView.ItemActivate += new System.EventHandler(this.EditMilestone_Click);
            this.MilestonesListView.SizeChanged += new System.EventHandler(this.MilestonesListView_SizeChanged);
            // 
            // columnHeader29
            // 
            this.columnHeader29.Text = "Позиция";
            // 
            // columnHeader30
            // 
            this.columnHeader30.Text = "Имя";
            // 
            // columnHeader31
            // 
            this.columnHeader31.Text = "Повторяемость";
            this.columnHeader31.Width = 23;
            // 
            // columnHeader32
            // 
            this.columnHeader32.Text = "Условия";
            this.columnHeader32.Width = 230;
            // 
            // columnHeader33
            // 
            this.columnHeader33.Text = "Последствия";
            this.columnHeader33.Width = 411;
            // 
            // MilesonesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MilestonesListView);
            this.Name = "MilesonesView";
            this.Size = new System.Drawing.Size(808, 509);
            this.contextMenuStrip_Milestones.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Milestones;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem30;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem31;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem32;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem33;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem34;
        private System.Windows.Forms.ListView MilestonesListView;
        private System.Windows.Forms.ColumnHeader columnHeader29;
        private System.Windows.Forms.ColumnHeader columnHeader30;
        private System.Windows.Forms.ColumnHeader columnHeader31;
        private System.Windows.Forms.ColumnHeader columnHeader32;
        private System.Windows.Forms.ColumnHeader columnHeader33;
    }
}
