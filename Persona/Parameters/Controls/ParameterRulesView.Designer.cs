namespace Persona.Core.Controls
{
    partial class ParameterRulesView
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
            this.contextMenuStrip_Rules = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem17 = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewRuleFormula_toolstripItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transformToFormula_toolstripItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem18 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem19 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem20 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem21 = new System.Windows.Forms.ToolStripMenuItem();
            this.RulesListView = new System.Windows.Forms.ListView();
            this.columnHeader22 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader23 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip_Rules.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip_Rules
            // 
            this.contextMenuStrip_Rules.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem17,
            this.addNewRuleFormula_toolstripItem,
            this.transformToFormula_toolstripItem,
            this.toolStripMenuItem18,
            this.toolStripMenuItem19,
            this.toolStripMenuItem20,
            this.toolStripSeparator4,
            this.toolStripMenuItem21});
            this.contextMenuStrip_Rules.Name = "contextMenuStrip3";
            this.contextMenuStrip_Rules.Size = new System.Drawing.Size(288, 164);
            this.contextMenuStrip_Rules.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Rules_Opening);
            // 
            // toolStripMenuItem17
            // 
            this.toolStripMenuItem17.Name = "toolStripMenuItem17";
            this.toolStripMenuItem17.Size = new System.Drawing.Size(287, 22);
            this.toolStripMenuItem17.Text = "Добавить новое правило (константа)...";
            this.toolStripMenuItem17.Click += new System.EventHandler(this.AddRuleToolStripMenuItem_Click);
            // 
            // addNewRuleFormula_toolstripItem
            // 
            this.addNewRuleFormula_toolstripItem.Name = "addNewRuleFormula_toolstripItem";
            this.addNewRuleFormula_toolstripItem.Size = new System.Drawing.Size(287, 22);
            this.addNewRuleFormula_toolstripItem.Text = "Добавить новое правило (формула)...";
            this.addNewRuleFormula_toolstripItem.Click += new System.EventHandler(this.AddRuleFormulaToolStripMenuItem_Click);
            // 
            // transformToFormula_toolstripItem
            // 
            this.transformToFormula_toolstripItem.Name = "transformToFormula_toolstripItem";
            this.transformToFormula_toolstripItem.Size = new System.Drawing.Size(287, 22);
            this.transformToFormula_toolstripItem.Text = "Преобразовать в формулу...";
            this.transformToFormula_toolstripItem.Click += new System.EventHandler(this.transformToFormula_toolstripItem_Click);
            // 
            // toolStripMenuItem18
            // 
            this.toolStripMenuItem18.Name = "toolStripMenuItem18";
            this.toolStripMenuItem18.Size = new System.Drawing.Size(287, 22);
            this.toolStripMenuItem18.Text = "Редактировать...";
            this.toolStripMenuItem18.Click += new System.EventHandler(this.EditRuleToolStripMenuItem_Click);
            // 
            // toolStripMenuItem19
            // 
            this.toolStripMenuItem19.Name = "toolStripMenuItem19";
            this.toolStripMenuItem19.Size = new System.Drawing.Size(287, 22);
            this.toolStripMenuItem19.Text = "Клонировать...";
            this.toolStripMenuItem19.Click += new System.EventHandler(this.CopyRuleToolStripMenuItem_Click);
            // 
            // toolStripMenuItem20
            // 
            this.toolStripMenuItem20.Name = "toolStripMenuItem20";
            this.toolStripMenuItem20.Size = new System.Drawing.Size(287, 22);
            this.toolStripMenuItem20.Text = "Удалить правило";
            this.toolStripMenuItem20.Click += new System.EventHandler(this.DeleteRuleToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(284, 6);
            // 
            // toolStripMenuItem21
            // 
            this.toolStripMenuItem21.Name = "toolStripMenuItem21";
            this.toolStripMenuItem21.Size = new System.Drawing.Size(287, 22);
            this.toolStripMenuItem21.Text = "Отмена";
            // 
            // RulesListView
            // 
            this.RulesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader22,
            this.columnHeader23});
            this.RulesListView.ContextMenuStrip = this.contextMenuStrip_Rules;
            this.RulesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RulesListView.FullRowSelect = true;
            this.RulesListView.GridLines = true;
            this.RulesListView.HideSelection = false;
            this.RulesListView.Location = new System.Drawing.Point(0, 0);
            this.RulesListView.Name = "RulesListView";
            this.RulesListView.Size = new System.Drawing.Size(828, 582);
            this.RulesListView.TabIndex = 10;
            this.RulesListView.UseCompatibleStateImageBehavior = false;
            this.RulesListView.View = System.Windows.Forms.View.Details;
            this.RulesListView.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.RulesListView_ColumnWidthChanged);
            this.RulesListView.ItemActivate += new System.EventHandler(this.EditRuleToolStripMenuItem_Click);
            this.RulesListView.SizeChanged += new System.EventHandler(this.RulesListView_SizeChanged);
            // 
            // columnHeader22
            // 
            this.columnHeader22.Text = "Условия";
            this.columnHeader22.Width = 413;
            // 
            // columnHeader23
            // 
            this.columnHeader23.Text = "Значение";
            this.columnHeader23.Width = 391;
            // 
            // ParameterRulesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.RulesListView);
            this.Name = "ParameterRulesView";
            this.Size = new System.Drawing.Size(828, 582);
            this.contextMenuStrip_Rules.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Rules;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem17;
        private System.Windows.Forms.ToolStripMenuItem addNewRuleFormula_toolstripItem;
        private System.Windows.Forms.ToolStripMenuItem transformToFormula_toolstripItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem18;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem19;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem20;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem21;
        private System.Windows.Forms.ListView RulesListView;
        private System.Windows.Forms.ColumnHeader columnHeader22;
        private System.Windows.Forms.ColumnHeader columnHeader23;
    }
}
