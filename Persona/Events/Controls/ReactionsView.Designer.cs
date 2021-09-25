namespace Persona.Core.Controls
{
    partial class ReactionsView
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
            this.contextMenuStrip_Reactions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.клонироватьToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.ReactionsListView = new System.Windows.Forms.ListView();
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip_Reactions.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip_Reactions
            // 
            this.contextMenuStrip_Reactions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.клонироватьToolStripMenuItem1,
            this.toolStripMenuItem6,
            this.toolStripSeparator1,
            this.toolStripMenuItem7});
            this.contextMenuStrip_Reactions.Name = "contextMenuStrip2";
            this.contextMenuStrip_Reactions.Size = new System.Drawing.Size(227, 120);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(226, 22);
            this.toolStripMenuItem4.Text = "Добавить новую реакцию...";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.AddReactionToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(226, 22);
            this.toolStripMenuItem5.Text = "Редактировать...";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.EditReactionToolStripMenuItem_Click);
            // 
            // клонироватьToolStripMenuItem1
            // 
            this.клонироватьToolStripMenuItem1.Name = "клонироватьToolStripMenuItem1";
            this.клонироватьToolStripMenuItem1.Size = new System.Drawing.Size(226, 22);
            this.клонироватьToolStripMenuItem1.Text = "Клонировать...";
            this.клонироватьToolStripMenuItem1.Click += new System.EventHandler(this.CopyReactionToolStripMenuItem1_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(226, 22);
            this.toolStripMenuItem6.Text = "Удалить реакцию";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(223, 6);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(226, 22);
            this.toolStripMenuItem7.Text = "Отмена";
            // 
            // ReactionsListView
            // 
            this.ReactionsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader13,
            this.columnHeader14});
            this.ReactionsListView.ContextMenuStrip = this.contextMenuStrip_Reactions;
            this.ReactionsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReactionsListView.FullRowSelect = true;
            this.ReactionsListView.GridLines = true;
            this.ReactionsListView.HideSelection = false;
            this.ReactionsListView.Location = new System.Drawing.Point(0, 0);
            this.ReactionsListView.Name = "ReactionsListView";
            this.ReactionsListView.Size = new System.Drawing.Size(842, 524);
            this.ReactionsListView.TabIndex = 7;
            this.ReactionsListView.UseCompatibleStateImageBehavior = false;
            this.ReactionsListView.View = System.Windows.Forms.View.Details;
            this.ReactionsListView.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.ReactionsListView_ColumnWidthChanged);
            this.ReactionsListView.ItemActivate += new System.EventHandler(this.EditReactionToolStripMenuItem_Click);
            this.ReactionsListView.SizeChanged += new System.EventHandler(this.ReactionsListView_SizeChanged);
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Имя";
            this.columnHeader11.Width = 117;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "Видимость";
            this.columnHeader12.Width = 23;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "Условия";
            this.columnHeader13.Width = 196;
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "Последствия";
            this.columnHeader14.Width = 481;
            // 
            // ReactionsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ReactionsListView);
            this.Name = "ReactionsView";
            this.Size = new System.Drawing.Size(842, 524);
            this.contextMenuStrip_Reactions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Reactions;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem клонироватьToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ListView ReactionsListView;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.ColumnHeader columnHeader14;
    }
}
