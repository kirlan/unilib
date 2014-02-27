using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RandomStory
{
    public partial class Form1 : Form
    {
        Repository m_pRepository = new Repository();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            m_pRepository.LoadXML();
            checkedListBox1.Items.Clear();
            checkedListBox1.Items.AddRange(m_pRepository.m_cWorlds.ToArray());

            for (int i = 0; i < checkedListBox1.Items.Count; i++)
                checkedListBox1.SetItemChecked(i, true);

            button1_Click(sender, e);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(e.CloseReason == CloseReason.UserClosing || e.CloseReason == CloseReason.ApplicationExitCall)
                m_pRepository.SaveXML();
        }

        private void выходToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void мирыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WorldsEdit pForm = new WorldsEdit(m_pRepository);
            pForm.ShowDialog();

            bool bAllChecked = (checkedListBox1.CheckedItems.Count == checkedListBox1.Items.Count);

            List<World> cAllowed = new List<World>();
            foreach (var pItem in checkedListBox1.CheckedItems)
                cAllowed.Add(pItem as World);
            
            checkedListBox1.Items.Clear();
            checkedListBox1.Items.AddRange(m_pRepository.m_cWorlds.ToArray());

            for (int i = 0; i < checkedListBox1.Items.Count; i++)
                if (bAllChecked || cAllowed.Contains(checkedListBox1.Items[i]))
                    checkedListBox1.SetItemChecked(i, true);
        }

        private void RichTextBoxAppend(string sText)
        {
            RichTextBoxAppend(sText, false, false, false, richTextBox1.ForeColor);
        }

        private void RichTextBoxAppend(string sText, bool bBold, bool bItalic, bool bUnderline)
        {
            RichTextBoxAppend(sText, bBold, bItalic, bUnderline, richTextBox1.ForeColor);
        }

        private void RichTextBoxAppend(string sText, bool bBold, bool bItalic, bool bUnderline, Color eColor)
        {
            int iSelectionStart = richTextBox1.SelectionStart;
            richTextBox1.Select(iSelectionStart, 0);
            richTextBox1.SelectedText = sText;
            richTextBox1.Select(iSelectionStart, sText.Length);

            Font pFont = richTextBox1.Font;
            FontStyle eStyle = FontStyle.Regular;
            if(bBold)
                eStyle |= FontStyle.Bold;
            if(bItalic)
                eStyle |= FontStyle.Italic;
            if(bUnderline)
                eStyle |= FontStyle.Underline;
            richTextBox1.SelectionFont = new Font(pFont.Name, pFont.Size, eStyle);
            richTextBox1.SelectionColor = eColor;
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;

            richTextBox1.Select(iSelectionStart + sText.Length, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<World> cAllowed = new List<World>();
            foreach (var pItem in checkedListBox1.CheckedItems)
                cAllowed.Add(pItem as World);

            m_pRepository.MarkPossibleWorlds(cAllowed);

            Story pStory = new Story(m_pRepository, попаданцыToolStripMenuItem.Checked);

            richTextBox1.Clear();

            RichTextBoxAppend("Сеттинг:\r\n", true, false, false);
            RichTextBoxAppend(string.Format("{0}\r\n\r\n", pStory.m_pWorld));

            RichTextBoxAppend("Главный герой:\r\n", true, false, false);
            RichTextBoxAppend(string.Format("{0}\r\n\r\n", pStory.m_pHero));

            RichTextBoxAppend("Проблема:\r\n", true, false, false);
            RichTextBoxAppend(string.Format("{0}\r\n\r\n", pStory.m_sProblem));

            RichTextBoxAppend("Покровитель/наставник героя:\r\n", true, false, false);
            RichTextBoxAppend(string.Format("{0}\r\n\r\n", pStory.m_pTutor == null ? "-" : pStory.m_pTutor.ToString()));

            RichTextBoxAppend("Спутник героя:\r\n", true, false, false);
            RichTextBoxAppend(string.Format("{0}\r\n\r\n", pStory.m_pHelper == null ? "-" : pStory.m_pHelper.ToString()));

            RichTextBoxAppend("Главный злодей:\r\n", true, false, false);
            RichTextBoxAppend(string.Format("{0}\r\n\r\n", pStory.m_pVillain));

            RichTextBoxAppend("Помощник злодея:\r\n", true, false, false);
            RichTextBoxAppend(string.Format("{0}\r\n\r\n", pStory.m_pMinion == null ? "-" : pStory.m_pMinion.ToString()));

            RichTextBoxAppend("Решение:\r\n", true, false, false);
            RichTextBoxAppend(string.Format("{0}\r\n\r\n", pStory.m_sSolution));

            RichTextBoxAppend("Ключевой предмет:\r\n", true, false, false);
            RichTextBoxAppend(string.Format("{0}\r\n\r\n", pStory.m_sKeyItem));

            StringBuilder sLocations = new StringBuilder();
            foreach (string sLoc in pStory.m_cLocations)
            {
                if (sLocations.Length == 0)
                    sLocations.Append(sLoc);
                else
                    sLocations.AppendFormat(", {0}",sLoc);
            }
            RichTextBoxAppend("Места основных событий:\r\n", true, false, false);
            RichTextBoxAppend(string.Format("{0}\r\n\r\n", sLocations.ToString()));
        }

        private void проблемыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProblemsEdit pForm = new ProblemsEdit(m_pRepository);
            pForm.ShowDialog();
        }

        private void считатьБазуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                m_pRepository.LoadXML(openFileDialog1.FileName);
        }

        private void сохранитьБазуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                m_pRepository.SaveXML(saveFileDialog1.FileName);
        }

        private void пометитьВсеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
                checkedListBox1.SetItemChecked(i, true);
        }

        private void снятьПометкиСоВсехToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
                checkedListBox1.SetItemChecked(i, false);
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
                button1.Enabled = true;

            if (e.NewValue == CheckState.Unchecked && checkedListBox1.CheckedItems.Count == 1)
                button1.Enabled = false;
        }
    }
}
