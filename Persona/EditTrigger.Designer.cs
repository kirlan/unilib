namespace Persona
{
    partial class EditTrigger
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.редактироватьToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.системнаяКомандаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьПоследствиеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.изменениеЗначенияПараметраToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавитьНовоеПоследствиеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.заданиеЗначенияПараметраToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.label1 = new System.Windows.Forms.Label();
            this.отменаToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.contextMenuStrip_Conditions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.добавитьНовоеУсловиеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сравнение2хПараметровToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.попаданиеПараметраВДиапазонToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.проверкаИстинностиФлагаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.редактироватьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьУсловиеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.отменаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip_Consequences = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2.SuspendLayout();
            this.contextMenuStrip_Conditions.SuspendLayout();
            this.contextMenuStrip_Consequences.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // редактироватьToolStripMenuItem1
            // 
            this.редактироватьToolStripMenuItem1.Name = "редактироватьToolStripMenuItem1";
            this.редактироватьToolStripMenuItem1.Size = new System.Drawing.Size(234, 22);
            this.редактироватьToolStripMenuItem1.Text = "Редактировать";
            this.редактироватьToolStripMenuItem1.Click += new System.EventHandler(this.EditConsequenceToolStripMenuItem1_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.button3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.button2, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 388);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(470, 36);
            this.tableLayoutPanel2.TabIndex = 14;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button3.Location = new System.Drawing.Point(157, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 6;
            this.button3.Text = "Отмена";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(238, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "ОК";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // системнаяКомандаToolStripMenuItem
            // 
            this.системнаяКомандаToolStripMenuItem.Name = "системнаяКомандаToolStripMenuItem";
            this.системнаяКомандаToolStripMenuItem.Size = new System.Drawing.Size(261, 22);
            this.системнаяКомандаToolStripMenuItem.Text = "Системная команда...";
            this.системнаяКомандаToolStripMenuItem.Click += new System.EventHandler(this.AddConsequenceCommandToolStripMenuItem_Click);
            // 
            // удалитьПоследствиеToolStripMenuItem
            // 
            this.удалитьПоследствиеToolStripMenuItem.Name = "удалитьПоследствиеToolStripMenuItem";
            this.удалитьПоследствиеToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.удалитьПоследствиеToolStripMenuItem.Text = "Удалить последствие";
            this.удалитьПоследствиеToolStripMenuItem.Click += new System.EventHandler(this.DeleteConsequenceToolStripMenuItem_Click);
            // 
            // изменениеЗначенияПараметраToolStripMenuItem
            // 
            this.изменениеЗначенияПараметраToolStripMenuItem.Name = "изменениеЗначенияПараметраToolStripMenuItem";
            this.изменениеЗначенияПараметраToolStripMenuItem.Size = new System.Drawing.Size(261, 22);
            this.изменениеЗначенияПараметраToolStripMenuItem.Text = "Изменение значения параметра...";
            this.изменениеЗначенияПараметраToolStripMenuItem.Click += new System.EventHandler(this.AddConsequenceChangeToolStripMenuItem_Click);
            // 
            // добавитьНовоеПоследствиеToolStripMenuItem
            // 
            this.добавитьНовоеПоследствиеToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.заданиеЗначенияПараметраToolStripMenuItem,
            this.изменениеЗначенияПараметраToolStripMenuItem,
            this.системнаяКомандаToolStripMenuItem});
            this.добавитьНовоеПоследствиеToolStripMenuItem.Name = "добавитьНовоеПоследствиеToolStripMenuItem";
            this.добавитьНовоеПоследствиеToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.добавитьНовоеПоследствиеToolStripMenuItem.Text = "Добавить новое последствие";
            // 
            // заданиеЗначенияПараметраToolStripMenuItem
            // 
            this.заданиеЗначенияПараметраToolStripMenuItem.Name = "заданиеЗначенияПараметраToolStripMenuItem";
            this.заданиеЗначенияПараметраToolStripMenuItem.Size = new System.Drawing.Size(261, 22);
            this.заданиеЗначенияПараметраToolStripMenuItem.Text = "Задание значения параметра...";
            this.заданиеЗначенияПараметраToolStripMenuItem.Click += new System.EventHandler(this.AddConsequenceSetToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(231, 6);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // отменаToolStripMenuItem1
            // 
            this.отменаToolStripMenuItem1.Name = "отменаToolStripMenuItem1";
            this.отменаToolStripMenuItem1.Size = new System.Drawing.Size(234, 22);
            this.отменаToolStripMenuItem1.Text = "Отмена";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(140, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(327, 20);
            this.textBox1.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 24);
            this.label3.TabIndex = 4;
            this.label3.Text = "Условие:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButton1.Location = new System.Drawing.Point(3, 3);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(160, 18);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.Text = "может повторяться";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBox1.ContextMenuStrip = this.contextMenuStrip_Conditions;
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.IntegralHeight = false;
            this.listBox1.Location = new System.Drawing.Point(140, 27);
            this.listBox1.Name = "listBox1";
            this.tableLayoutPanel1.SetRowSpan(this.listBox1, 7);
            this.listBox1.Size = new System.Drawing.Size(327, 162);
            this.listBox1.TabIndex = 5;
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
            this.проверкаИстинностиФлагаToolStripMenuItem});
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
            // contextMenuStrip_Consequences
            // 
            this.contextMenuStrip_Consequences.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьНовоеПоследствиеToolStripMenuItem,
            this.редактироватьToolStripMenuItem1,
            this.удалитьПоследствиеToolStripMenuItem,
            this.toolStripMenuItem1,
            this.отменаToolStripMenuItem1});
            this.contextMenuStrip_Consequences.Name = "contextMenuStrip_Consequences";
            this.contextMenuStrip_Consequences.Size = new System.Drawing.Size(235, 98);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButton2.Location = new System.Drawing.Point(169, 3);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(161, 18);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "случается однажды";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // listBox2
            // 
            this.listBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBox2.ContextMenuStrip = this.contextMenuStrip_Consequences;
            this.listBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.IntegralHeight = false;
            this.listBox2.Location = new System.Drawing.Point(140, 195);
            this.listBox2.Name = "listBox2";
            this.tableLayoutPanel1.SetRowSpan(this.listBox2, 7);
            this.listBox2.Size = new System.Drawing.Size(327, 162);
            this.listBox2.TabIndex = 11;
            this.listBox2.DoubleClick += new System.EventHandler(this.EditConsequenceToolStripMenuItem1_Click);
            this.listBox2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBox2_MouseDown);
            // 
            // label9
            // 
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(3, 360);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(131, 24);
            this.label9.TabIndex = 16;
            this.label9.Text = "Повторяемость:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.radioButton1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.radioButton2, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(137, 360);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(333, 24);
            this.tableLayoutPanel3.TabIndex = 17;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.17548F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.82452F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBox1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.listBox1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.listBox2, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 15);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 15);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 17;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(470, 388);
            this.tableLayoutPanel1.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(3, 192);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(131, 24);
            this.label6.TabIndex = 10;
            this.label6.Text = "Последствия:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // EditTrigger
            // 
            this.AcceptButton = this.button2;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button3;
            this.ClientSize = new System.Drawing.Size(470, 424);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.tableLayoutPanel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "EditTrigger";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Редактирование триггера";
            this.tableLayoutPanel2.ResumeLayout(false);
            this.contextMenuStrip_Conditions.ResumeLayout(false);
            this.contextMenuStrip_Consequences.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem редактироватьToolStripMenuItem1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolStripMenuItem системнаяКомандаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem удалитьПоследствиеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem изменениеЗначенияПараметраToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem добавитьНовоеПоследствиеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem заданиеЗначенияПараметраToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem отменаToolStripMenuItem1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Conditions;
        private System.Windows.Forms.ToolStripMenuItem добавитьНовоеУсловиеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сравнение2хПараметровToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem попаданиеПараметраВДиапазонToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem проверкаИстинностиФлагаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem редактироватьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem удалитьУсловиеToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem отменаToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Consequences;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.RadioButton radioButton2;
    }
}