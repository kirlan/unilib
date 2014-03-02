using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Random;

namespace RandomStory
{
    public partial class Form1 : Form
    {
        Repository m_pRepository = new Repository();

        public Form1()
        {
            InitializeComponent();

            string sStr = "[F]male or female[M]";
            char[] aFlags = null;
            aFlags = Strings.GetFlags(ref sStr);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            m_pRepository.LoadXML();
            checkedListBox1.Items.Clear();
            checkedListBox1.Items.AddRange(m_pRepository.m_cAllSettings.ToArray());

            button2_Click(sender, e);
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
            m_pRepository.SaveXML();

            bool bAllChecked = (checkedListBox1.CheckedItems.Count == checkedListBox1.Items.Count);

            List<Setting> cAllowed = new List<Setting>();
            List<Setting> cPrimed = new List<Setting>();
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                CheckState eState = checkedListBox1.GetItemCheckState(i);

                if (eState == CheckState.Unchecked)
                    continue;

                Setting pItem = checkedListBox1.Items[i] as Setting;
                if (eState == CheckState.Checked)
                    cPrimed.Add(pItem);
                else
                    cAllowed.Add(pItem);
            }
            
            checkedListBox1.Items.Clear();
            checkedListBox1.Items.AddRange(m_pRepository.m_cAllSettings.ToArray());

            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (bAllChecked || cPrimed.Contains(checkedListBox1.Items[i]))
                    checkedListBox1.SetItemCheckState(i, CheckState.Checked);
                else if (cAllowed.Contains(checkedListBox1.Items[i]))
                    checkedListBox1.SetItemCheckState(i, CheckState.Indeterminate);
            }
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
            List<Setting> cAllowed = new List<Setting>();
            List<Setting> cPrimed = new List<Setting>();
            for (int i=0; i<checkedListBox1.Items.Count; i++)
            {
                CheckState eState = checkedListBox1.GetItemCheckState(i);

                if (eState == CheckState.Unchecked)
                    continue;

                Setting pItem = checkedListBox1.Items[i] as Setting;
                if (eState == CheckState.Checked)
                    cPrimed.Add(pItem);
                else
                    cAllowed.Add(pItem);
            }

            m_pRepository.MarkPossibleWorlds(cAllowed, cPrimed);

            Story pStory = new Story(m_pRepository, checkBox1.Checked);

            richTextBox1.Clear();

            RichTextBoxAppend("Сеттинг:\r\n", true, false, false);
            RichTextBoxAppend(string.Format("{0}\r\n\r\n", pStory.m_pSetting));

            RichTextBoxAppend("Главный герой:\r\n", true, false, false);
            RichTextBoxAppend(string.Format("{0}\r\n\r\n", pStory.m_pHero));

            if (pStory.m_pTutor != null)
            {
                RichTextBoxAppend("Покровитель/наставник героя:\r\n", true, false, false);
                RichTextBoxAppend(string.Format("{0}\r\n\r\n", pStory.m_pTutor));
            }

            if (pStory.m_pHelper != null)
            {
                RichTextBoxAppend("Спутник героя:\r\n", true, false, false);
                RichTextBoxAppend(string.Format("{0}\r\n\r\n", pStory.m_pHelper));
            }

            RichTextBoxAppend("Проблема:\r\n", true, false, false);
            RichTextBoxAppend(string.Format("{0}\r\n\r\n", pStory.m_sProblem));

            RichTextBoxAppend("Антагонист:\r\n", true, false, false);
            RichTextBoxAppend(string.Format("{0}\r\n\r\n", pStory.m_pVillain));

            if (pStory.m_pMinion != null)
            {
                RichTextBoxAppend("Помощник антагониста:\r\n", true, false, false);
                RichTextBoxAppend(string.Format("{0}\r\n\r\n",  pStory.m_pMinion));
            }

            RichTextBoxAppend("Решение:\r\n", true, false, false);
            RichTextBoxAppend(string.Format("{0}\r\n\r\n", pStory.m_sSolution));

            RichTextBoxAppend("Ключевой предмет:\r\n", true, false, false);
            RichTextBoxAppend(string.Format("{0}\r\n\r\n", pStory.m_sKeyItem));

            RichTextBoxAppend("Места основных событий:\r\n", true, false, false);
            RichTextBoxAppend(string.Format("{0}\r\n\r\n", pStory.m_cLocations.ToString()));
        }

        private void проблемыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProblemsEdit pForm = new ProblemsEdit(m_pRepository);
            pForm.ShowDialog();
            m_pRepository.SaveXML();
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
            checkedListBox1.ItemCheck -= new ItemCheckEventHandler(checkedListBox1_ItemCheck);
            
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
                checkedListBox1.SetItemCheckState(i, CheckState.Checked);

            checkedListBox1.ItemCheck += new ItemCheckEventHandler(checkedListBox1_ItemCheck);
            button1.Enabled = checkedListBox1.CheckedItems.Count > 0;
        }

        private void снятьПометкиСоВсехToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkedListBox1.ItemCheck -= new ItemCheckEventHandler(checkedListBox1_ItemCheck); 
            
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
                checkedListBox1.SetItemCheckState(i, CheckState.Unchecked);
        
            checkedListBox1.ItemCheck += new ItemCheckEventHandler(checkedListBox1_ItemCheck);
            button1.Enabled = false;
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            switch (e.CurrentValue)
            {
                case CheckState.Checked:
                    e.NewValue = CheckState.Indeterminate;
                    break;

                case CheckState.Indeterminate:
                    e.NewValue = CheckState.Unchecked;
                    break;

                case CheckState.Unchecked:
                    e.NewValue = CheckState.Checked;
                    break;
            }

            if (e.NewValue == CheckState.Checked)
                button1.Enabled = true;

            if (e.NewValue != CheckState.Checked && checkedListBox1.CheckedItems.Count == 1)
                button1.Enabled = false;
        }

        private void отношенияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CharactersEdit pForm = new CharactersEdit(m_pRepository);
            pForm.ShowDialog();
            m_pRepository.SaveXML();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = Rnd.OneChanceFrom(2);

            if (checkedListBox1.Items.Count > 0)
            {
                checkedListBox1.ItemCheck -= new ItemCheckEventHandler(checkedListBox1_ItemCheck);

                for(int i=0; i<checkedListBox1.Items.Count; i++)
                    checkedListBox1.SetItemCheckState(i, CheckState.Unchecked);

                int iSetting1 = Rnd.Get(checkedListBox1.Items.Count);
                int iSetting2 = -1;
                checkedListBox1.SetItemCheckState(iSetting1, CheckState.Checked);

                if (checkedListBox1.Items.Count > 1 && Rnd.OneChanceFrom(2))
                {
                    do
                    {
                        iSetting2 = Rnd.Get(checkedListBox1.Items.Count);
                    }
                    while (iSetting2 == iSetting1);

                    checkedListBox1.SetItemCheckState(iSetting2, CheckState.Checked);
                }

                if (checkedListBox1.Items.Count > 2)
                {
                    int iSetting3;
                    do
                    {
                        iSetting3 = Rnd.Get(checkedListBox1.Items.Count);
                    }
                    while (iSetting3 == iSetting1 || iSetting3 == iSetting2);

                    checkedListBox1.SetItemCheckState(iSetting3, CheckState.Indeterminate);
                }

                checkedListBox1.ItemCheck += new ItemCheckEventHandler(checkedListBox1_ItemCheck);
            }
        }
    }
}
