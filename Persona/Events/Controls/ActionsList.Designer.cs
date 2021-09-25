namespace Persona.Events.Controls
{
    partial class ActionsList
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
            this.contextMenuStrip_Actions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.добавитьНовуюМастьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.переименоватьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьМастьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.отменаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ActionsListBox = new System.Windows.Forms.CheckedListBox();
            this.contextMenuStrip_Actions.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip_Actions
            // 
            this.contextMenuStrip_Actions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьНовуюМастьToolStripMenuItem,
            this.переименоватьToolStripMenuItem,
            this.удалитьМастьToolStripMenuItem,
            this.toolStripMenuItem1,
            this.отменаToolStripMenuItem});
            this.contextMenuStrip_Actions.Name = "contextMenuStrip1";
            this.contextMenuStrip_Actions.Size = new System.Drawing.Size(224, 120);
            // 
            // добавитьНовуюМастьToolStripMenuItem
            // 
            this.добавитьНовуюМастьToolStripMenuItem.Name = "добавитьНовуюМастьToolStripMenuItem";
            this.добавитьНовуюМастьToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.добавитьНовуюМастьToolStripMenuItem.Text = "Добавить новое действие...";
            this.добавитьНовуюМастьToolStripMenuItem.Click += new System.EventHandler(this.AddCategoryToolStripMenuItem_Click);
            // 
            // переименоватьToolStripMenuItem
            // 
            this.переименоватьToolStripMenuItem.Name = "переименоватьToolStripMenuItem";
            this.переименоватьToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.переименоватьToolStripMenuItem.Text = "Редактировать";
            this.переименоватьToolStripMenuItem.Click += new System.EventHandler(this.EditCategoryToolStripMenuItem_Click);
            // 
            // удалитьМастьToolStripMenuItem
            // 
            this.удалитьМастьToolStripMenuItem.Name = "удалитьМастьToolStripMenuItem";
            this.удалитьМастьToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.удалитьМастьToolStripMenuItem.Text = "Удалить действие";
            this.удалитьМастьToolStripMenuItem.Click += new System.EventHandler(this.RemoveCategoryToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(220, 6);
            // 
            // отменаToolStripMenuItem
            // 
            this.отменаToolStripMenuItem.Name = "отменаToolStripMenuItem";
            this.отменаToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.отменаToolStripMenuItem.Text = "Отмена";
            // 
            // ActionsListBox
            // 
            this.ActionsListBox.ContextMenuStrip = this.contextMenuStrip_Actions;
            this.ActionsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ActionsListBox.FormattingEnabled = true;
            this.ActionsListBox.IntegralHeight = false;
            this.ActionsListBox.Location = new System.Drawing.Point(0, 0);
            this.ActionsListBox.Name = "ActionsListBox";
            this.ActionsListBox.Size = new System.Drawing.Size(150, 371);
            this.ActionsListBox.TabIndex = 6;
            this.ActionsListBox.DoubleClick += new System.EventHandler(this.EditCategoryToolStripMenuItem_Click);
            this.ActionsListBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ActionsListBox_MouseDown);
            // 
            // ActionsList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ActionsListBox);
            this.Name = "ActionsList";
            this.Size = new System.Drawing.Size(150, 371);
            this.contextMenuStrip_Actions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Actions;
        private System.Windows.Forms.ToolStripMenuItem добавитьНовуюМастьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem переименоватьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem удалитьМастьToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem отменаToolStripMenuItem;
        private System.Windows.Forms.CheckedListBox ActionsListBox;
    }
}
