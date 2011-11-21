using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace WorldGeneration
{
    public partial class WaitForm : Form
    {
        private static object m_pWaitFormLock = new object();
        private static WaitForm m_pWaitForm = null;

        /// <summary>
        /// Показывает диалог ожидания.
        /// </summary>
        public static void StartWait()
        {
            Monitor.Enter(m_pWaitFormLock);

            if (m_pWaitForm != null)
                return;

            m_pWaitForm = new WaitForm();

            m_pWaitForm.Show();
            m_pWaitForm.Refresh();
        }

        public static void BeginStep(string sDescription, int iLength)
        {
            if (m_pWaitForm == null)
                return;

            m_pWaitForm.richTextBox1.AppendText(sDescription + "\r\n");
            m_pWaitForm.panel2.Width = 0;
            m_pWaitForm.m_iScale = iLength;
            m_pWaitForm.m_iRealProgress = 0;
            //m_pWaitForm.progressBar1.Maximum = iLength;
            m_pWaitForm.Refresh();
        }

        public static void ProgressStep()
        {
            if (m_pWaitForm == null)
                return;

            m_pWaitForm.IncProgress();
        }

        public static void CloseWait()
        {
            if (m_pWaitForm == null)
                return;

            m_pWaitForm.Close();
            m_pWaitForm = null;

            Monitor.Exit(m_pWaitFormLock);
        }

        private int m_iScale = 100;
        private int m_iRealProgress = 0;

        public WaitForm()
        {
            InitializeComponent();

            richTextBox1.Clear();
        }

        private void IncProgress()
        {
            m_iRealProgress++;

            while (m_iRealProgress > m_iScale)
                m_iRealProgress -= m_iScale;

            int iScaledProgress = m_iRealProgress * panel1.ClientRectangle.Width / m_iScale;

            if (iScaledProgress != panel2.Width)
            {
                panel2.Width = iScaledProgress;
                Refresh();
            }
        }
    }
}
