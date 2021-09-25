namespace Persona.Core.Controls
{
    partial class EventsView
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
            this.contextMenuStrip_Events = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.добавитьНовуюКартуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.редактироватьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.клонироватьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьКартуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.отменаToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.EventsListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip_Events.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip_Events
            // 
            this.contextMenuStrip_Events.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьНовуюКартуToolStripMenuItem,
            this.редактироватьToolStripMenuItem,
            this.клонироватьToolStripMenuItem,
            this.удалитьКартуToolStripMenuItem,
            this.toolStripMenuItem2,
            this.отменаToolStripMenuItem1});
            this.contextMenuStrip_Events.Name = "contextMenuStrip2";
            this.contextMenuStrip_Events.Size = new System.Drawing.Size(222, 120);
            // 
            // добавитьНовуюКартуToolStripMenuItem
            // 
            this.добавитьНовуюКартуToolStripMenuItem.Name = "добавитьНовуюКартуToolStripMenuItem";
            this.добавитьНовуюКартуToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.добавитьНовуюКартуToolStripMenuItem.Text = "Добавить новое событие...";
            this.добавитьНовуюКартуToolStripMenuItem.Click += new System.EventHandler(this.AddEventToolStripMenuItem_Click);
            // 
            // редактироватьToolStripMenuItem
            // 
            this.редактироватьToolStripMenuItem.Name = "редактироватьToolStripMenuItem";
            this.редактироватьToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.редактироватьToolStripMenuItem.Text = "Редактировать...";
            this.редактироватьToolStripMenuItem.Click += new System.EventHandler(this.EditEventToolStripMenuItem_Click);
            // 
            // клонироватьToolStripMenuItem
            // 
            this.клонироватьToolStripMenuItem.Name = "клонироватьToolStripMenuItem";
            this.клонироватьToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.клонироватьToolStripMenuItem.Text = "Клонировать...";
            this.клонироватьToolStripMenuItem.Click += new System.EventHandler(this.CopyEventToolStripMenuItem_Click);
            // 
            // удалитьКартуToolStripMenuItem
            // 
            this.удалитьКартуToolStripMenuItem.Name = "удалитьКартуToolStripMenuItem";
            this.удалитьКартуToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.удалитьКартуToolStripMenuItem.Text = "Удалить событие";
            this.удалитьКартуToolStripMenuItem.Click += new System.EventHandler(this.RemoveEventToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(218, 6);
            // 
            // отменаToolStripMenuItem1
            // 
            this.отменаToolStripMenuItem1.Name = "отменаToolStripMenuItem1";
            this.отменаToolStripMenuItem1.Size = new System.Drawing.Size(221, 22);
            this.отменаToolStripMenuItem1.Text = "Отмена";
            // 
            // EventsListView
            // 
            this.EventsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader6,
            this.columnHeader10});
            this.EventsListView.ContextMenuStrip = this.contextMenuStrip_Events;
            this.EventsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EventsListView.FullRowSelect = true;
            this.EventsListView.GridLines = true;
            this.EventsListView.HideSelection = false;
            this.EventsListView.Location = new System.Drawing.Point(0, 0);
            this.EventsListView.Name = "EventsListView";
            this.EventsListView.Size = new System.Drawing.Size(781, 582);
            this.EventsListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.EventsListView.TabIndex = 4;
            this.EventsListView.UseCompatibleStateImageBehavior = false;
            this.EventsListView.View = System.Windows.Forms.View.Details;
            this.EventsListView.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.EventsListView_ColumnWidthChanged);
            this.EventsListView.ItemActivate += new System.EventHandler(this.EditEventToolStripMenuItem_Click);
            this.EventsListView.SelectedIndexChanged += new System.EventHandler(this.EventsListView_SelectedIndexChanged);
            this.EventsListView.SizeChanged += new System.EventHandler(this.EventsListView_SizeChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID";
            this.columnHeader1.Width = 70;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Действие";
            this.columnHeader2.Width = 65;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Приоритет";
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Частота";
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Повторяемость";
            this.columnHeader9.Width = 22;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Условия";
            this.columnHeader6.Width = 163;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Текст";
            this.columnHeader10.Width = 157;
            // 
            // EventsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.EventsListView);
            this.Name = "EventsView";
            this.Size = new System.Drawing.Size(781, 582);
            this.contextMenuStrip_Events.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Events;
        private System.Windows.Forms.ToolStripMenuItem добавитьНовуюКартуToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem редактироватьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem клонироватьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem удалитьКартуToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem отменаToolStripMenuItem1;
        private System.Windows.Forms.ListView EventsListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader10;
    }
}
