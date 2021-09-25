namespace Persona.Flows.Controls
{
    partial class FlowsList
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
            this.contextMenuStrip_Flows = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem26 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem27 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem28 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem29 = new System.Windows.Forms.ToolStripMenuItem();
            this.FlowsListBox = new System.Windows.Forms.ListBox();
            this.contextMenuStrip_Flows.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip_Flows
            // 
            this.contextMenuStrip_Flows.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem26,
            this.toolStripMenuItem27,
            this.toolStripMenuItem28,
            this.toolStripSeparator6,
            this.toolStripMenuItem29});
            this.contextMenuStrip_Flows.Name = "contextMenuStrip1";
            this.contextMenuStrip_Flows.Size = new System.Drawing.Size(210, 98);
            // 
            // toolStripMenuItem26
            // 
            this.toolStripMenuItem26.Name = "toolStripMenuItem26";
            this.toolStripMenuItem26.Size = new System.Drawing.Size(209, 22);
            this.toolStripMenuItem26.Text = "Добавить новый поток...";
            this.toolStripMenuItem26.Click += new System.EventHandler(this.FlowAdd_Click);
            // 
            // toolStripMenuItem27
            // 
            this.toolStripMenuItem27.Name = "toolStripMenuItem27";
            this.toolStripMenuItem27.Size = new System.Drawing.Size(209, 22);
            this.toolStripMenuItem27.Text = "Редактировать";
            this.toolStripMenuItem27.Click += new System.EventHandler(this.FlowEdit_Click);
            // 
            // toolStripMenuItem28
            // 
            this.toolStripMenuItem28.Name = "toolStripMenuItem28";
            this.toolStripMenuItem28.Size = new System.Drawing.Size(209, 22);
            this.toolStripMenuItem28.Text = "Удалить поток";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(206, 6);
            // 
            // toolStripMenuItem29
            // 
            this.toolStripMenuItem29.Name = "toolStripMenuItem29";
            this.toolStripMenuItem29.Size = new System.Drawing.Size(209, 22);
            this.toolStripMenuItem29.Text = "Отмена";
            // 
            // FlowsListBox
            // 
            this.FlowsListBox.ContextMenuStrip = this.contextMenuStrip_Flows;
            this.FlowsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FlowsListBox.FormattingEnabled = true;
            this.FlowsListBox.IntegralHeight = false;
            this.FlowsListBox.Location = new System.Drawing.Point(0, 0);
            this.FlowsListBox.Name = "FlowsListBox";
            this.FlowsListBox.Size = new System.Drawing.Size(150, 393);
            this.FlowsListBox.TabIndex = 7;
            this.FlowsListBox.SelectedIndexChanged += new System.EventHandler(this.FlowsListBox_SelectedIndexChanged);
            this.FlowsListBox.DoubleClick += new System.EventHandler(this.FlowEdit_Click);
            // 
            // FlowsList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.FlowsListBox);
            this.Name = "FlowsList";
            this.Size = new System.Drawing.Size(150, 393);
            this.contextMenuStrip_Flows.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Flows;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem26;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem27;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem28;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem29;
        private System.Windows.Forms.ListBox FlowsListBox;
    }
}
