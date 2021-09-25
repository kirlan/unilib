using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Runtime.InteropServices;

namespace RandomStory
{
    public partial class ExportForm : Form
    {
        public ExportForm(Story m_pStory)
        {
            InitializeComponent();

            printPreviewControl1.Dock = DockStyle.Fill;

            richTextBox1.Clear();

            RichTextBoxAppend("Сеттинг:\r\n", true, false, false);
            RichTextBoxAppend(string.Format("{0}\r\n\r\n", m_pStory.m_pSetting));

            RichTextBoxAppend("Главный герой:\r\n", true, false, false);
            RichTextBoxAppend(string.Format("{0}\r\n\r\n", m_pStory.m_pHero));

            if (m_pStory.m_pTutor != null)
            {
                RichTextBoxAppend("Покровитель/наставник героя:\r\n", true, false, false);
                RichTextBoxAppend(string.Format("{0}\r\n\r\n", m_pStory.m_pTutor));
            }

            if (m_pStory.m_pHelper != null)
            {
                RichTextBoxAppend("Спутник героя:\r\n", true, false, false);
                RichTextBoxAppend(string.Format("{0}\r\n\r\n", m_pStory.m_pHelper));
            }

            RichTextBoxAppend("Проблема:\r\n", true, false, false);
            RichTextBoxAppend(string.Format("{0}\r\n\r\n", m_pStory.m_sProblem));

            RichTextBoxAppend("Антагонист:\r\n", true, false, false);
            RichTextBoxAppend(string.Format("{0}\r\n\r\n", m_pStory.m_pVillain));

            if (m_pStory.m_pMinion != null)
            {
                RichTextBoxAppend("Помощник антагониста:\r\n", true, false, false);
                RichTextBoxAppend(string.Format("{0}\r\n\r\n", m_pStory.m_pMinion));
            }

            RichTextBoxAppend("Решение:\r\n", true, false, false);
            RichTextBoxAppend(string.Format("{0}\r\n\r\n", m_pStory.m_sSolution));

            RichTextBoxAppend("Ключевые предметы:\r\n", true, false, false);
            RichTextBoxAppend(string.Format("{0}\r\n\r\n", m_pStory.m_cKeyItems.ToString()));

            RichTextBoxAppend("Места основных событий:\r\n", true, false, false);
            RichTextBoxAppend(string.Format("{0}\r\n\r\n", m_pStory.m_cLocations.ToString()));

            RichTextBoxAppend("Ключевые события:\r\n", true, false, false);
            RichTextBoxAppend(string.Format("{0}\r\n\r\n", m_pStory.m_cEvents.ToString()));
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
            if (bBold)
                eStyle |= FontStyle.Bold;
            if (bItalic)
                eStyle |= FontStyle.Italic;
            if (bUnderline)
                eStyle |= FontStyle.Underline;
            richTextBox1.SelectionFont = new Font(pFont.Name, pFont.Size, eStyle);
            richTextBox1.SelectionColor = eColor;
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;

            richTextBox1.Select(iSelectionStart + sText.Length, 0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Clipboard.SetText(richTextBox1.Text, TextDataFormat.UnicodeText);
            //Clipboard.SetText(richTextBox1.Rtf, TextDataFormat.Rtf);

            DataObject dto = new DataObject();
            dto.SetText(richTextBox1.Rtf, TextDataFormat.Rtf);
            dto.SetText(richTextBox1.Text, TextDataFormat.UnicodeText);
            Clipboard.Clear();
            Clipboard.SetDataObject(dto);
        }
        
        private int checkPrint;
        
        private void button1_Click(object sender, EventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            PrintPreviewDialog ppd = new PrintPreviewDialog();
            ppd.Document = doc;
            pd.Document = doc;
            if (ppd.ShowDialog() == DialogResult.OK)
                if (pd.ShowDialog() == DialogResult.OK)
                    doc.Print();

            //if (printDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    printDocument1.Print();
            //}
        }

        void doc_BeginPrint(object sender, PrintEventArgs e)
        {
            checkPrint = 0;
        }

        void doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            checkPrint = richTextBox1.Print(checkPrint, richTextBox1.TextLength, e);

            if (checkPrint < richTextBox1.TextLength)
                e.HasMorePages = true;
            else
                e.HasMorePages = false;
        }

    }
}
