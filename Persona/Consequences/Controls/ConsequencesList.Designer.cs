namespace Persona.Core.Controls
{
    partial class ConsequencesList
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
            this.contextMenuStrip_Consequences = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.добавитьНовоеПоследствиеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.заданиеЗначенияПараметраToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.изменениеЗначенияПараметраToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.изменениеЗначенияПараметраНаПеременнуюВеличинуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.прогрессПотокаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.прогрессПотокапараметрToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выбратьОбъектToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.обновитьОбъектToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.системнаяКомандаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.редактироватьToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьПоследствиеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.отменаToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.contextMenuStrip_Consequences.SuspendLayout();
            this.SuspendLayout();
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
            // добавитьНовоеПоследствиеToolStripMenuItem
            // 
            this.добавитьНовоеПоследствиеToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.заданиеЗначенияПараметраToolStripMenuItem,
            this.изменениеЗначенияПараметраToolStripMenuItem,
            this.изменениеЗначенияПараметраНаПеременнуюВеличинуToolStripMenuItem,
            this.прогрессПотокаToolStripMenuItem,
            this.прогрессПотокапараметрToolStripMenuItem,
            this.выбратьОбъектToolStripMenuItem,
            this.обновитьОбъектToolStripMenuItem,
            this.системнаяКомандаToolStripMenuItem});
            this.добавитьНовоеПоследствиеToolStripMenuItem.Name = "добавитьНовоеПоследствиеToolStripMenuItem";
            this.добавитьНовоеПоследствиеToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.добавитьНовоеПоследствиеToolStripMenuItem.Text = "Добавить новое последствие";
            // 
            // заданиеЗначенияПараметраToolStripMenuItem
            // 
            this.заданиеЗначенияПараметраToolStripMenuItem.Name = "заданиеЗначенияПараметраToolStripMenuItem";
            this.заданиеЗначенияПараметраToolStripMenuItem.Size = new System.Drawing.Size(323, 22);
            this.заданиеЗначенияПараметраToolStripMenuItem.Text = "Задание значения параметра...";
            this.заданиеЗначенияПараметраToolStripMenuItem.Click += new System.EventHandler(this.AddConsequenceSetToolStripMenuItem_Click);
            // 
            // изменениеЗначенияПараметраToolStripMenuItem
            // 
            this.изменениеЗначенияПараметраToolStripMenuItem.Name = "изменениеЗначенияПараметраToolStripMenuItem";
            this.изменениеЗначенияПараметраToolStripMenuItem.Size = new System.Drawing.Size(323, 22);
            this.изменениеЗначенияПараметраToolStripMenuItem.Text = "Изменение значения параметра...";
            this.изменениеЗначенияПараметраToolStripMenuItem.Click += new System.EventHandler(this.AddConsequenceChangeToolStripMenuItem_Click);
            // 
            // изменениеЗначенияПараметраНаПеременнуюВеличинуToolStripMenuItem
            // 
            this.изменениеЗначенияПараметраНаПеременнуюВеличинуToolStripMenuItem.Name = "изменениеЗначенияПараметраНаПеременнуюВеличинуToolStripMenuItem";
            this.изменениеЗначенияПараметраНаПеременнуюВеличинуToolStripMenuItem.Size = new System.Drawing.Size(323, 22);
            this.изменениеЗначенияПараметраНаПеременнуюВеличинуToolStripMenuItem.Text = "Изменение значения параметра (формула)...";
            this.изменениеЗначенияПараметраНаПеременнуюВеличинуToolStripMenuItem.Click += new System.EventHandler(this.AddConsequenceChangeVariableToolStripMenuItem_Click);
            // 
            // прогрессПотокаToolStripMenuItem
            // 
            this.прогрессПотокаToolStripMenuItem.Name = "прогрессПотокаToolStripMenuItem";
            this.прогрессПотокаToolStripMenuItem.Size = new System.Drawing.Size(323, 22);
            this.прогрессПотокаToolStripMenuItem.Text = "Прогресс потока (константа)...";
            this.прогрессПотокаToolStripMenuItem.Click += new System.EventHandler(this.AddConsequenceFlowProgressToolStripMenuItem_Click);
            // 
            // прогрессПотокапараметрToolStripMenuItem
            // 
            this.прогрессПотокапараметрToolStripMenuItem.Name = "прогрессПотокапараметрToolStripMenuItem";
            this.прогрессПотокапараметрToolStripMenuItem.Size = new System.Drawing.Size(323, 22);
            this.прогрессПотокапараметрToolStripMenuItem.Text = "Прогресс потока (параметр)...";
            this.прогрессПотокапараметрToolStripMenuItem.Click += new System.EventHandler(this.AddConsequenceFlowProgressVariableToolStripMenuItem_Click);
            // 
            // выбратьОбъектToolStripMenuItem
            // 
            this.выбратьОбъектToolStripMenuItem.Name = "выбратьОбъектToolStripMenuItem";
            this.выбратьОбъектToolStripMenuItem.Size = new System.Drawing.Size(323, 22);
            this.выбратьОбъектToolStripMenuItem.Text = "Выбрать объект...";
            this.выбратьОбъектToolStripMenuItem.Click += new System.EventHandler(this.AddConsequenceSelectToolStripMenuItem_Click);
            // 
            // обновитьОбъектToolStripMenuItem
            // 
            this.обновитьОбъектToolStripMenuItem.Name = "обновитьОбъектToolStripMenuItem";
            this.обновитьОбъектToolStripMenuItem.Size = new System.Drawing.Size(323, 22);
            this.обновитьОбъектToolStripMenuItem.Text = "Выбрать случайный объект...";
            this.обновитьОбъектToolStripMenuItem.Click += new System.EventHandler(this.AddConsequenceShuffleToolStripMenuItem_Click);
            // 
            // системнаяКомандаToolStripMenuItem
            // 
            this.системнаяКомандаToolStripMenuItem.Name = "системнаяКомандаToolStripMenuItem";
            this.системнаяКомандаToolStripMenuItem.Size = new System.Drawing.Size(323, 22);
            this.системнаяКомандаToolStripMenuItem.Text = "Системная команда...";
            this.системнаяКомандаToolStripMenuItem.Click += new System.EventHandler(this.AddConsequenceCommandToolStripMenuItem_Click);
            // 
            // редактироватьToolStripMenuItem1
            // 
            this.редактироватьToolStripMenuItem1.Name = "редактироватьToolStripMenuItem1";
            this.редактироватьToolStripMenuItem1.Size = new System.Drawing.Size(234, 22);
            this.редактироватьToolStripMenuItem1.Text = "Редактировать";
            this.редактироватьToolStripMenuItem1.Click += new System.EventHandler(this.EditConsequenceToolStripMenuItem1_Click);
            // 
            // удалитьПоследствиеToolStripMenuItem
            // 
            this.удалитьПоследствиеToolStripMenuItem.Name = "удалитьПоследствиеToolStripMenuItem";
            this.удалитьПоследствиеToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.удалитьПоследствиеToolStripMenuItem.Text = "Удалить последствие";
            this.удалитьПоследствиеToolStripMenuItem.Click += new System.EventHandler(this.DeleteConsequenceToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(231, 6);
            // 
            // отменаToolStripMenuItem1
            // 
            this.отменаToolStripMenuItem1.Name = "отменаToolStripMenuItem1";
            this.отменаToolStripMenuItem1.Size = new System.Drawing.Size(234, 22);
            this.отменаToolStripMenuItem1.Text = "Отмена";
            // 
            // listBox2
            // 
            this.listBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBox2.ContextMenuStrip = this.contextMenuStrip_Consequences;
            this.listBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox2.FormattingEnabled = true;
            this.listBox2.IntegralHeight = false;
            this.listBox2.Location = new System.Drawing.Point(0, 0);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(150, 150);
            this.listBox2.TabIndex = 12;
            this.listBox2.DoubleClick += new System.EventHandler(this.EditConsequenceToolStripMenuItem1_Click);
            this.listBox2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBox2_MouseDown);
            // 
            // ConsequencesList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listBox2);
            this.Name = "ConsequencesList";
            this.contextMenuStrip_Consequences.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Consequences;
        private System.Windows.Forms.ToolStripMenuItem добавитьНовоеПоследствиеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem заданиеЗначенияПараметраToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem изменениеЗначенияПараметраToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem изменениеЗначенияПараметраНаПеременнуюВеличинуToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem прогрессПотокаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem прогрессПотокапараметрToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выбратьОбъектToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem обновитьОбъектToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem системнаяКомандаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem редактироватьToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem удалитьПоследствиеToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem отменаToolStripMenuItem1;
        private System.Windows.Forms.ListBox listBox2;
    }
}
