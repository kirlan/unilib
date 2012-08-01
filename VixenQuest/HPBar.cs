using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VixenQuest
{
    public partial class HPBar : UserControl
    {
        public HPBar()
        {
            InitializeComponent();
        }

        public bool Inverse
        {
            get { return glassProgressBar1.Inverse; }
            set
            {
                glassProgressBar1.Inverse = value;
                if (value)
                {
                    borderLabel1.TextAlign = ContentAlignment.MiddleRight;
                    tableLayoutPanel1.SetColumn(glassProgressBar1, 1);
                }
                else
                {
                    borderLabel1.TextAlign = ContentAlignment.MiddleLeft;
                    tableLayoutPanel1.SetColumn(glassProgressBar1, 0);
                }
            }
        }

        private bool m_bCritical = false;

        public bool Critical
        {
            get { return m_bCritical; }
            set
            {
                if (m_bCritical != value)
                {
                    m_bCritical = value;
                    if (m_bCritical)
                        timer2.Start();
                    else
                        timer2.Stop();
                }
            }
        }

        private int m_iMaxHP = 100;

        public void SetFighter(string sName, int iMaxHP)
        {
            timer1.Stop();
            Critical = false;
            timer3.Stop();

            borderLabel1.Text = sName;
            m_iMaxHP = iMaxHP;
            m_iShouldBeValue = glassProgressBar1.Maximum;
            glassProgressBar1.Value = glassProgressBar1.Maximum;
        }

        private void SetShouldBeValue(int iValue)
        {
            m_iShouldBeValue = iValue * glassProgressBar1.Maximum / m_iMaxHP;
        }

        private int m_iShouldBeValue = 0;
        private int m_iCounter = 0;
        private int m_iCounterMax = 0;

        public void LoseHP(int iNewHP)
        {
            SetShouldBeValue(iNewHP);

            timer1.Start();
        }

        private bool m_bRestorationMode = false;
        private int m_iRestorationStartHP = 0;

        public void RestoreHP(int iTime)
        {
            m_iRestorationStartHP = glassProgressBar1.Value;
            m_iShouldBeValue = glassProgressBar1.Maximum;
            m_iCounter = 0;
            m_iCounterMax = iTime;

            m_bRestorationMode = true;

            timer1.Start();
        }

        private int m_iLoseCounter = 0;

        public void ShowLose()
        {
            Critical = false;
            m_iLoseCounter = 0;
            timer3.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (m_bRestorationMode)
            {
                if (m_iCounter++ >= m_iCounterMax)
                {
                    timer1.Stop();
                    return;
                }

                glassProgressBar1.Value = m_iRestorationStartHP + (m_iShouldBeValue - m_iRestorationStartHP) * m_iCounter / m_iCounterMax;
            }
            else
            {
                if (glassProgressBar1.Value == m_iShouldBeValue)
                {
                    timer1.Stop();
                    return;
                }

                glassProgressBar1.Value = (glassProgressBar1.Value * 3 + m_iShouldBeValue) / 4;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            glassProgressBar1.Visible = !glassProgressBar1.Visible;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (m_iLoseCounter++ >= 40)
            {
                timer3.Stop();
                LoseShowEndsEvent();
                return;
            }

            glassProgressBar1.Visible = !glassProgressBar1.Visible;
        }

        public event EventHandler<EventArgs> LoseShowEnds;

        private void LoseShowEndsEvent()
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<EventArgs> temp = LoseShowEnds;
            if (temp != null)
                temp(this, new EventArgs());
        }
    }
}
