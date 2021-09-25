namespace Persona.Core.Controls
{
    partial class TriggersView
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
            this.contextMenuStrip_Triggers = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            this.преобразоватьВФункциюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripMenuItem();
            this.TriggersListView = new System.Windows.Forms.ListView();
            this.columnHeader16 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader17 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader18 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader19 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip_Triggers.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip_Triggers
            // 
            this.contextMenuStrip_Triggers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem8,
            this.toolStripMenuItem9,
            this.toolStripMenuItem10,
            this.преобразоватьВФункциюToolStripMenuItem,
            this.toolStripMenuItem11,
            this.toolStripSeparator2,
            this.toolStripMenuItem12});
            this.contextMenuStrip_Triggers.Name = "contextMenuStrip2";
            this.contextMenuStrip_Triggers.Size = new System.Drawing.Size(223, 142);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(222, 22);
            this.toolStripMenuItem8.Text = "Добавить новый триггер...";
            this.toolStripMenuItem8.Click += new System.EventHandler(this.toolStripMenuItem8_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(222, 22);
            this.toolStripMenuItem9.Text = "Редактировать...";
            this.toolStripMenuItem9.Click += new System.EventHandler(this.EditTriggerToolStripMenuItem_Click);
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(222, 22);
            this.toolStripMenuItem10.Text = "Клонировать...";
            this.toolStripMenuItem10.Click += new System.EventHandler(this.toolStripMenuItem10_Click);
            // 
            // преобразоватьВФункциюToolStripMenuItem
            // 
            this.преобразоватьВФункциюToolStripMenuItem.Name = "преобразоватьВФункциюToolStripMenuItem";
            this.преобразоватьВФункциюToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.преобразоватьВФункциюToolStripMenuItem.Text = "Преобразовать в функцию";
            this.преобразоватьВФункциюToolStripMenuItem.Click += new System.EventHandler(this.Trigger2FunctionToolStripMenuItem_Click);
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(222, 22);
            this.toolStripMenuItem11.Text = "Удалить триггер";
            this.toolStripMenuItem11.Click += new System.EventHandler(this.toolStripMenuItem11_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(219, 6);
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new System.Drawing.Size(222, 22);
            this.toolStripMenuItem12.Text = "Отмена";
            // 
            // TriggersListView
            // 
            this.TriggersListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader16,
            this.columnHeader17,
            this.columnHeader18,
            this.columnHeader19});
            this.TriggersListView.ContextMenuStrip = this.contextMenuStrip_Triggers;
            this.TriggersListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TriggersListView.FullRowSelect = true;
            this.TriggersListView.GridLines = true;
            this.TriggersListView.HideSelection = false;
            this.TriggersListView.Location = new System.Drawing.Point(0, 0);
            this.TriggersListView.Name = "TriggersListView";
            this.TriggersListView.Size = new System.Drawing.Size(843, 513);
            this.TriggersListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.TriggersListView.TabIndex = 8;
            this.TriggersListView.UseCompatibleStateImageBehavior = false;
            this.TriggersListView.View = System.Windows.Forms.View.Details;
            this.TriggersListView.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.TriggersListView_ColumnWidthChanged);
            this.TriggersListView.ItemActivate += new System.EventHandler(this.EditTriggerToolStripMenuItem_Click);
            this.TriggersListView.SizeChanged += new System.EventHandler(this.TriggersListView_SizeChanged);
            // 
            // columnHeader16
            // 
            this.columnHeader16.Text = "ID";
            this.columnHeader16.Width = 117;
            // 
            // columnHeader17
            // 
            this.columnHeader17.Text = "Повторяемость";
            this.columnHeader17.Width = 23;
            // 
            // columnHeader18
            // 
            this.columnHeader18.Text = "Условия";
            this.columnHeader18.Width = 196;
            // 
            // columnHeader19
            // 
            this.columnHeader19.Text = "Последствия";
            this.columnHeader19.Width = 385;
            // 
            // TriggersView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TriggersListView);
            this.Name = "TriggersView";
            this.Size = new System.Drawing.Size(843, 513);
            this.contextMenuStrip_Triggers.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Triggers;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem10;
        private System.Windows.Forms.ToolStripMenuItem преобразоватьВФункциюToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem11;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem12;
        private System.Windows.Forms.ListView TriggersListView;
        private System.Windows.Forms.ColumnHeader columnHeader16;
        private System.Windows.Forms.ColumnHeader columnHeader17;
        private System.Windows.Forms.ColumnHeader columnHeader18;
        private System.Windows.Forms.ColumnHeader columnHeader19;
    }
}
