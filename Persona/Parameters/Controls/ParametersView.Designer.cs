namespace Persona.Core.Controls
{
    partial class ParametersView
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
            System.Windows.Forms.ColumnHeader columnHeader3;
            this.contextMenuStrip_Parameters = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.добавитьНовыйПараметрToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.числовойToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.логическийToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.строковыйToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.редактироватьToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.клонироватьToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.отменаToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.ParametersListView = new System.Windows.Forms.ListView();
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader21 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader20 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip_Parameters.SuspendLayout();
            this.SuspendLayout();
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Имя";
            columnHeader3.Width = 115;
            // 
            // contextMenuStrip_Parameters
            // 
            this.contextMenuStrip_Parameters.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьНовыйПараметрToolStripMenuItem,
            this.редактироватьToolStripMenuItem1,
            this.клонироватьToolStripMenuItem2,
            this.удалитьToolStripMenuItem,
            this.toolStripMenuItem3,
            this.отменаToolStripMenuItem2});
            this.contextMenuStrip_Parameters.Name = "contextMenuStrip3";
            this.contextMenuStrip_Parameters.Size = new System.Drawing.Size(222, 120);
            // 
            // добавитьНовыйПараметрToolStripMenuItem
            // 
            this.добавитьНовыйПараметрToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.числовойToolStripMenuItem,
            this.логическийToolStripMenuItem,
            this.строковыйToolStripMenuItem});
            this.добавитьНовыйПараметрToolStripMenuItem.Name = "добавитьНовыйПараметрToolStripMenuItem";
            this.добавитьНовыйПараметрToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.добавитьНовыйПараметрToolStripMenuItem.Text = "Добавить новый параметр";
            // 
            // числовойToolStripMenuItem
            // 
            this.числовойToolStripMenuItem.Name = "числовойToolStripMenuItem";
            this.числовойToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.числовойToolStripMenuItem.Text = "Числовой...";
            this.числовойToolStripMenuItem.Click += new System.EventHandler(this.числовойToolStripMenuItem_Click);
            // 
            // логическийToolStripMenuItem
            // 
            this.логическийToolStripMenuItem.Name = "логическийToolStripMenuItem";
            this.логическийToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.логическийToolStripMenuItem.Text = "Логический...";
            this.логическийToolStripMenuItem.Click += new System.EventHandler(this.логическийToolStripMenuItem_Click);
            // 
            // строковыйToolStripMenuItem
            // 
            this.строковыйToolStripMenuItem.Name = "строковыйToolStripMenuItem";
            this.строковыйToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.строковыйToolStripMenuItem.Text = "Строковый...";
            this.строковыйToolStripMenuItem.Click += new System.EventHandler(this.строковыйToolStripMenuItem_Click);
            // 
            // редактироватьToolStripMenuItem1
            // 
            this.редактироватьToolStripMenuItem1.Name = "редактироватьToolStripMenuItem1";
            this.редактироватьToolStripMenuItem1.Size = new System.Drawing.Size(221, 22);
            this.редактироватьToolStripMenuItem1.Text = "Редактировать...";
            this.редактироватьToolStripMenuItem1.Click += new System.EventHandler(this.EditParameterToolStripMenuItem1_Click);
            // 
            // клонироватьToolStripMenuItem2
            // 
            this.клонироватьToolStripMenuItem2.Name = "клонироватьToolStripMenuItem2";
            this.клонироватьToolStripMenuItem2.Size = new System.Drawing.Size(221, 22);
            this.клонироватьToolStripMenuItem2.Text = "Клонировать...";
            this.клонироватьToolStripMenuItem2.Click += new System.EventHandler(this.клонироватьToolStripMenuItem2_Click);
            // 
            // удалитьToolStripMenuItem
            // 
            this.удалитьToolStripMenuItem.Name = "удалитьToolStripMenuItem";
            this.удалитьToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.удалитьToolStripMenuItem.Text = "Удалить параметр";
            this.удалитьToolStripMenuItem.Click += new System.EventHandler(this.DeleteParameterToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(218, 6);
            // 
            // отменаToolStripMenuItem2
            // 
            this.отменаToolStripMenuItem2.Name = "отменаToolStripMenuItem2";
            this.отменаToolStripMenuItem2.Size = new System.Drawing.Size(221, 22);
            this.отменаToolStripMenuItem2.Text = "Отмена";
            // 
            // ParametersListView
            // 
            this.ParametersListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeader3,
            this.columnHeader15,
            this.columnHeader21,
            this.columnHeader4,
            this.columnHeader20,
            this.columnHeader5});
            this.ParametersListView.ContextMenuStrip = this.contextMenuStrip_Parameters;
            this.ParametersListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ParametersListView.FullRowSelect = true;
            this.ParametersListView.GridLines = true;
            this.ParametersListView.HideSelection = false;
            this.ParametersListView.Location = new System.Drawing.Point(0, 0);
            this.ParametersListView.Name = "ParametersListView";
            this.ParametersListView.Size = new System.Drawing.Size(936, 546);
            this.ParametersListView.TabIndex = 1;
            this.ParametersListView.UseCompatibleStateImageBehavior = false;
            this.ParametersListView.View = System.Windows.Forms.View.Details;
            this.ParametersListView.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.ParametersListView_ColumnWidthChanged);
            this.ParametersListView.ItemActivate += new System.EventHandler(this.EditParameterToolStripMenuItem1_Click);
            this.ParametersListView.SelectedIndexChanged += new System.EventHandler(this.ParametersListView_SelectedIndexChanged);
            this.ParametersListView.SizeChanged += new System.EventHandler(this.ParametersListView_SizeChanged);
            // 
            // columnHeader15
            // 
            this.columnHeader15.Text = "Группа";
            this.columnHeader15.Width = 107;
            // 
            // columnHeader21
            // 
            this.columnHeader21.Text = "Тип";
            this.columnHeader21.Width = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Скрытый";
            this.columnHeader4.Width = 25;
            // 
            // columnHeader20
            // 
            this.columnHeader20.Text = "Автоматически вычислять";
            this.columnHeader20.Width = 25;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Описание";
            this.columnHeader5.Width = 537;
            // 
            // ParametersView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ParametersListView);
            this.Name = "ParametersView";
            this.Size = new System.Drawing.Size(936, 546);
            this.contextMenuStrip_Parameters.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Parameters;
        private System.Windows.Forms.ToolStripMenuItem добавитьНовыйПараметрToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem числовойToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem логическийToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem строковыйToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem редактироватьToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem клонироватьToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem удалитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem отменаToolStripMenuItem2;
        private System.Windows.Forms.ListView ParametersListView;
        private System.Windows.Forms.ColumnHeader columnHeader15;
        private System.Windows.Forms.ColumnHeader columnHeader21;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader20;
        private System.Windows.Forms.ColumnHeader columnHeader5;
    }
}
