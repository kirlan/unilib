namespace RandomStory
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.редактироватьБазуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.считатьБазуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьБазуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.выходToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.мирыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.проблемыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.попаданцыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.пометитьВсеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.снятьПометкиСоВсехToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.отменаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.отношенияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.редактироватьБазуToolStripMenuItem,
            this.выходToolStripMenuItem,
            this.настройкиToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(668, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // редактироватьБазуToolStripMenuItem
            // 
            this.редактироватьБазуToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.считатьБазуToolStripMenuItem,
            this.сохранитьБазуToolStripMenuItem,
            this.toolStripMenuItem1,
            this.выходToolStripMenuItem1});
            this.редактироватьБазуToolStripMenuItem.Name = "редактироватьБазуToolStripMenuItem";
            this.редактироватьБазуToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.редактироватьБазуToolStripMenuItem.Text = "Файл";
            // 
            // считатьБазуToolStripMenuItem
            // 
            this.считатьБазуToolStripMenuItem.Name = "считатьБазуToolStripMenuItem";
            this.считатьБазуToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.считатьБазуToolStripMenuItem.Text = "Считать базу...";
            this.считатьБазуToolStripMenuItem.Click += new System.EventHandler(this.считатьБазуToolStripMenuItem_Click);
            // 
            // сохранитьБазуToolStripMenuItem
            // 
            this.сохранитьБазуToolStripMenuItem.Name = "сохранитьБазуToolStripMenuItem";
            this.сохранитьБазуToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.сохранитьБазуToolStripMenuItem.Text = "Сохранить базу...";
            this.сохранитьБазуToolStripMenuItem.Click += new System.EventHandler(this.сохранитьБазуToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(165, 6);
            // 
            // выходToolStripMenuItem1
            // 
            this.выходToolStripMenuItem1.Name = "выходToolStripMenuItem1";
            this.выходToolStripMenuItem1.Size = new System.Drawing.Size(168, 22);
            this.выходToolStripMenuItem1.Text = "Выход";
            this.выходToolStripMenuItem1.Click += new System.EventHandler(this.выходToolStripMenuItem1_Click);
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.мирыToolStripMenuItem,
            this.проблемыToolStripMenuItem,
            this.отношенияToolStripMenuItem});
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.выходToolStripMenuItem.Text = "Редактор";
            // 
            // мирыToolStripMenuItem
            // 
            this.мирыToolStripMenuItem.Name = "мирыToolStripMenuItem";
            this.мирыToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.мирыToolStripMenuItem.Text = "Сеттинги...";
            this.мирыToolStripMenuItem.Click += new System.EventHandler(this.мирыToolStripMenuItem_Click);
            // 
            // проблемыToolStripMenuItem
            // 
            this.проблемыToolStripMenuItem.Name = "проблемыToolStripMenuItem";
            this.проблемыToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.проблемыToolStripMenuItem.Text = "Проблемы и решения...";
            this.проблемыToolStripMenuItem.Click += new System.EventHandler(this.проблемыToolStripMenuItem_Click);
            // 
            // настройкиToolStripMenuItem
            // 
            this.настройкиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.попаданцыToolStripMenuItem});
            this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.настройкиToolStripMenuItem.Text = "Настройки";
            // 
            // попаданцыToolStripMenuItem
            // 
            this.попаданцыToolStripMenuItem.CheckOnClick = true;
            this.попаданцыToolStripMenuItem.Name = "попаданцыToolStripMenuItem";
            this.попаданцыToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.попаданцыToolStripMenuItem.Text = "Попаданцы";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(141, 488);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(387, 31);
            this.button1.TabIndex = 2;
            this.button1.Text = "Новый сюжет!";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "xml";
            this.openFileDialog1.FileName = "repository";
            this.openFileDialog1.Filter = "XML файлы|*.xml|Все файлы|*.*";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "xml";
            this.saveFileDialog1.FileName = "repository";
            this.saveFileDialog1.Filter = "XML файлы|*.xml|Все файлы|*.*";
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.checkedListBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.IntegralHeight = false;
            this.checkedListBox1.Location = new System.Drawing.Point(0, 44);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(215, 436);
            this.checkedListBox1.TabIndex = 3;
            this.checkedListBox1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox1_ItemCheck);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.пометитьВсеToolStripMenuItem,
            this.снятьПометкиСоВсехToolStripMenuItem,
            this.toolStripMenuItem2,
            this.отменаToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(199, 76);
            // 
            // пометитьВсеToolStripMenuItem
            // 
            this.пометитьВсеToolStripMenuItem.Name = "пометитьВсеToolStripMenuItem";
            this.пометитьВсеToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.пометитьВсеToolStripMenuItem.Text = "Пометить все";
            this.пометитьВсеToolStripMenuItem.Click += new System.EventHandler(this.пометитьВсеToolStripMenuItem_Click);
            // 
            // снятьПометкиСоВсехToolStripMenuItem
            // 
            this.снятьПометкиСоВсехToolStripMenuItem.Name = "снятьПометкиСоВсехToolStripMenuItem";
            this.снятьПометкиСоВсехToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.снятьПометкиСоВсехToolStripMenuItem.Text = "Снять пометки со всех";
            this.снятьПометкиСоВсехToolStripMenuItem.Click += new System.EventHandler(this.снятьПометкиСоВсехToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(195, 6);
            // 
            // отменаToolStripMenuItem
            // 
            this.отменаToolStripMenuItem.Name = "отменаToolStripMenuItem";
            this.отменаToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.отменаToolStripMenuItem.Text = "Отмена";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Location = new System.Drawing.Point(222, 28);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(10);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(446, 452);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Используемые сеттинги:";
            // 
            // отношенияToolStripMenuItem
            // 
            this.отношенияToolStripMenuItem.Name = "отношенияToolStripMenuItem";
            this.отношенияToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.отношенияToolStripMenuItem.Text = "Отношения...";
            this.отношенияToolStripMenuItem.Click += new System.EventHandler(this.отношенияToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 525);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Генератор сюжетов";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem редактироватьБазуToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem считатьБазуToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьБазуToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem мирыToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripMenuItem проблемыToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem пометитьВсеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem снятьПометкиСоВсехToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem отменаToolStripMenuItem;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem попаданцыToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem отношенияToolStripMenuItem;
    }
}

