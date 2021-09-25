namespace Persona.Core.Controls
{
    partial class ConditionsList
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.contextMenuStrip_Conditions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.добавитьНовоеУсловиеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сравнение2хПараметровToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.попаданиеПараметраВДиапазонToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.проверкаИстинностиФлагаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.проверкаОбъектаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.редактироватьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьУсловиеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.отменаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip_Conditions.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBox1.ContextMenuStrip = this.contextMenuStrip_Conditions;
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.IntegralHeight = false;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(150, 150);
            this.listBox1.TabIndex = 6;
            this.listBox1.DoubleClick += new System.EventHandler(this.EditConditionToolStripMenuItem_Click);
            this.listBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDown);
            // 
            // contextMenuStrip_Conditions
            // 
            this.contextMenuStrip_Conditions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьНовоеУсловиеToolStripMenuItem,
            this.редактироватьToolStripMenuItem,
            this.удалитьУсловиеToolStripMenuItem,
            this.toolStripMenuItem2,
            this.отменаToolStripMenuItem});
            this.contextMenuStrip_Conditions.Name = "contextMenuStrip_Conditions";
            this.contextMenuStrip_Conditions.Size = new System.Drawing.Size(211, 98);
            // 
            // добавитьНовоеУсловиеToolStripMenuItem
            // 
            this.добавитьНовоеУсловиеToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.сравнение2хПараметровToolStripMenuItem,
            this.попаданиеПараметраВДиапазонToolStripMenuItem,
            this.проверкаИстинностиФлагаToolStripMenuItem,
            this.проверкаОбъектаToolStripMenuItem});
            this.добавитьНовоеУсловиеToolStripMenuItem.Name = "добавитьНовоеУсловиеToolStripMenuItem";
            this.добавитьНовоеУсловиеToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.добавитьНовоеУсловиеToolStripMenuItem.Text = "Добавить новое условие";
            // 
            // сравнение2хПараметровToolStripMenuItem
            // 
            this.сравнение2хПараметровToolStripMenuItem.Name = "сравнение2хПараметровToolStripMenuItem";
            this.сравнение2хПараметровToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
            this.сравнение2хПараметровToolStripMenuItem.Text = "Сравнение 2х параметров...";
            this.сравнение2хПараметровToolStripMenuItem.Click += new System.EventHandler(this.AddConditionComparsionToolStripMenuItem_Click);
            // 
            // попаданиеПараметраВДиапазонToolStripMenuItem
            // 
            this.попаданиеПараметраВДиапазонToolStripMenuItem.Name = "попаданиеПараметраВДиапазонToolStripMenuItem";
            this.попаданиеПараметраВДиапазонToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
            this.попаданиеПараметраВДиапазонToolStripMenuItem.Text = "Попадание параметра в диапазон...";
            this.попаданиеПараметраВДиапазонToolStripMenuItem.Click += new System.EventHandler(this.AddConditionRangeToolStripMenuItem_Click);
            // 
            // проверкаИстинностиФлагаToolStripMenuItem
            // 
            this.проверкаИстинностиФлагаToolStripMenuItem.Name = "проверкаИстинностиФлагаToolStripMenuItem";
            this.проверкаИстинностиФлагаToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
            this.проверкаИстинностиФлагаToolStripMenuItem.Text = "Проверка истинности флага...";
            this.проверкаИстинностиФлагаToolStripMenuItem.Click += new System.EventHandler(this.AddConditionStatusToolStripMenuItem_Click);
            // 
            // проверкаОбъектаToolStripMenuItem
            // 
            this.проверкаОбъектаToolStripMenuItem.Name = "проверкаОбъектаToolStripMenuItem";
            this.проверкаОбъектаToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
            this.проверкаОбъектаToolStripMenuItem.Text = "Проверка объекта...";
            this.проверкаОбъектаToolStripMenuItem.Click += new System.EventHandler(this.AddConditionObjectSelectedToolStripMenuItem_Click);
            // 
            // редактироватьToolStripMenuItem
            // 
            this.редактироватьToolStripMenuItem.Name = "редактироватьToolStripMenuItem";
            this.редактироватьToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.редактироватьToolStripMenuItem.Text = "Редактировать";
            this.редактироватьToolStripMenuItem.Click += new System.EventHandler(this.EditConditionToolStripMenuItem_Click);
            // 
            // удалитьУсловиеToolStripMenuItem
            // 
            this.удалитьУсловиеToolStripMenuItem.Name = "удалитьУсловиеToolStripMenuItem";
            this.удалитьУсловиеToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.удалитьУсловиеToolStripMenuItem.Text = "Удалить условие";
            this.удалитьУсловиеToolStripMenuItem.Click += new System.EventHandler(this.RemoveConditionToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(207, 6);
            // 
            // отменаToolStripMenuItem
            // 
            this.отменаToolStripMenuItem.Name = "отменаToolStripMenuItem";
            this.отменаToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.отменаToolStripMenuItem.Text = "Отмена";
            // 
            // ConditionsList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listBox1);
            this.Name = "ConditionsList";
            this.contextMenuStrip_Conditions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Conditions;
        private System.Windows.Forms.ToolStripMenuItem добавитьНовоеУсловиеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сравнение2хПараметровToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem попаданиеПараметраВДиапазонToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem проверкаИстинностиФлагаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem проверкаОбъектаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem редактироватьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem удалитьУсловиеToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem отменаToolStripMenuItem;
    }
}
