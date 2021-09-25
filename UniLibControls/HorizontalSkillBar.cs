using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using nsUniLibControls;
using Random;

namespace MiscControls
{
    public partial class HorizontalSkillBar : UserControl
    {
        public HorizontalSkillBar()
        {
            InitializeComponent();
        }
        private Color m_eColor00 = SystemColors.Control;
        /// <summary>
        /// Backcolor
        /// </summary>
        public Color Color00
        {
            get { return m_eColor00; }
            set
            {
                m_eColor00 = value;
                MyRepaint();
            }
        }

        private Color m_eColor01 = Color.Crimson;
        /// <summary>
        /// Forecolor1
        /// </summary>
        public Color Color01
        {
            get { return m_eColor01; }
            set
            {
                m_eColor01 = value;
                MyRepaint();
            }
        }

        private Color m_eColor02 = Color.Red;
        /// <summary>
        /// Forecolor2
        /// </summary>
        public Color Color02
        {
            get { return m_eColor02; }
            set
            {
                m_eColor02 = value;
                MyRepaint();
            }
        }

        private Color m_eColor03 = Color.OrangeRed;
        /// <summary>
        /// Forecolor3
        /// </summary>
        public Color Color03
        {
            get { return m_eColor03; }
            set
            {
                m_eColor03 = value;
                MyRepaint();
            }
        }

        private Color m_eColor04 = Color.Orange;
        /// <summary>
        /// Forecolor4
        /// </summary>
        public Color Color04
        {
            get { return m_eColor04; }
            set
            {
                m_eColor04 = value;
                MyRepaint();
            }
        }

        private Color m_eColor05 = Color.Gold;
        /// <summary>
        /// Forecolor5
        /// </summary>
        public Color Color05
        {
            get { return m_eColor05; }
            set
            {
                m_eColor05 = value;
                MyRepaint();
            }
        }

        private Color m_eColor06 = Color.Yellow;
        /// <summary>
        /// Forecolor6
        /// </summary>
        public Color Color06
        {
            get { return m_eColor06; }
            set
            {
                m_eColor06 = value;
                MyRepaint();
            }
        }

        private Color m_eColor07 = Color.GreenYellow;
        /// <summary>
        /// Forecolor7
        /// </summary>
        public Color Color07
        {
            get { return m_eColor07; }
            set
            {
                m_eColor07 = value;
                MyRepaint();
            }
        }

        private Color m_eColor08 = Color.Chartreuse;
        /// <summary>
        /// Forecolor8
        /// </summary>
        public Color Color08
        {
            get { return m_eColor08; }
            set
            {
                m_eColor08 = value;
                MyRepaint();
            }
        }

        private Color m_eColor09 = Color.LawnGreen;
        /// <summary>
        /// Forecolor9
        /// </summary>
        public Color Color09
        {
            get { return m_eColor09; }
            set
            {
                m_eColor09 = value;
                MyRepaint();
            }
        }

        private Color m_eColor10 = Color.Lime;
        /// <summary>
        /// Forecolor10
        /// </summary>
        public Color Color10
        {
            get { return m_eColor10; }
            set
            {
                m_eColor10 = value;
                MyRepaint();
            }
        }

        public bool Bouncing
        {
            get { return timer1.Enabled; }
            set { timer1.Enabled = value; }
        }

        private int m_iMaxValue = 10;
        public int MaxValue
        {
            get { return m_iMaxValue; }
            set { m_iMaxValue = value; SetValue(m_iValue, 0); }
        }

        private int m_iMinValue = 0;
        public int MinValue
        {
            get { return m_iMinValue; }
            set { m_iMinValue = value; SetValue(m_iValue, 0); }
        }

        private int m_iValue = 5;
        public int Value
        {
            get { return m_iValue; }
            set { SetValue(value, 0); }
        }

        private string m_sToolTip = "";
        public string ToolTip
        {
            get { return m_sToolTip; }
            set
            {
                m_sToolTip = value;
                toolTip1.SetToolTip(panel1, m_sToolTip);
                toolTip1.SetToolTip(panel2, m_sToolTip);
                toolTip1.SetToolTip(this, m_sToolTip);
            }
        }

        public void SetValue(int iValue, int iTime)
        {
            m_iValue = iValue;

            if (m_iValue > m_iMaxValue)
                m_iValue = m_iMaxValue;

            if (m_iValue < m_iMinValue)
                m_iValue = m_iMinValue;

            m_iShouldBeWidth = (int)((float)(panel1.ClientRectangle.Width) / (float)(m_iMaxValue - m_iMinValue) * (float)m_iValue);

            MyRepaint();
        }

        private void HorizontalSkillBar_Resize(object sender, EventArgs e)
        {
            SetValue(m_iValue, 0); 
        }

        private int m_iShouldBeWidth = 0;

        private void MyRepaint()
        {
            panel1.BackColor = m_eColor00;

            double fShouldBeWidth = m_iShouldBeWidth;

            if (Bouncing)
            {
                int iWidth = (int)(fShouldBeWidth + (fShouldBeWidth / 2 - Rnd.Get(fShouldBeWidth)) / 6);

                if (fShouldBeWidth > iWidth)
                    fShouldBeWidth -= Rnd.Get((fShouldBeWidth - iWidth) / 2);
                else
                    fShouldBeWidth += Rnd.Get((iWidth - fShouldBeWidth) / 2);
            }

            double fIntervalWidth = (double)panel1.ClientRectangle.Width / 9.0;
            int iInterval = (int)(fShouldBeWidth / fIntervalWidth);
            int iDelta = (int)(fShouldBeWidth - (iInterval * fIntervalWidth));

            KColor color1 = new KColor();
            KColor color2 = new KColor();
            switch (iInterval)
            {
                case 0:
                    color1.RGB = m_eColor01;
                    color2.RGB = m_eColor02;
                    break;
                case 1:
                    color1.RGB = m_eColor02;
                    color2.RGB = m_eColor03;
                    break;
                case 2:
                    color1.RGB = m_eColor03;
                    color2.RGB = m_eColor04;
                    break;
                case 3:
                    color1.RGB = m_eColor04;
                    color2.RGB = m_eColor05;
                    break;
                case 4:
                    color1.RGB = m_eColor05;
                    color2.RGB = m_eColor06;
                    break;
                case 5:
                    color1.RGB = m_eColor06;
                    color2.RGB = m_eColor07;
                    break;
                case 6:
                    color1.RGB = m_eColor07;
                    color2.RGB = m_eColor08;
                    break;
                case 7:
                    color1.RGB = m_eColor08;
                    color2.RGB = m_eColor09;
                    break;
                case 8:
                    color1.RGB = m_eColor09;
                    color2.RGB = m_eColor10;
                    break;
                case 9:
                    color1.RGB = m_eColor10;
                    color2.RGB = m_eColor10;
                    break;
            }
            
            KColor color3 = new KColor();
            if (color2.Hue - color1.Hue > 180)
                color3.Hue = color1.Hue + (int)((float)(color2.Hue - color1.Hue - 360) * (float)iDelta / fIntervalWidth);
            else if (color2.Hue - color1.Hue < -180)
                color3.Hue = color1.Hue + (int)((float)(color2.Hue - color1.Hue + 360) * (float)iDelta / fIntervalWidth);
            else
                color3.Hue = color1.Hue + (int)((float)(color2.Hue - color1.Hue) * (float)iDelta / fIntervalWidth);
            color3.Lightness = color1.Lightness + (int)((float)(color2.Lightness - color1.Lightness) * (float)iDelta / fIntervalWidth);
            color3.Saturation = color1.Saturation + (int)((float)(color2.Saturation - color1.Saturation) * (float)iDelta / fIntervalWidth);

            panel2.BackColor = color3.RGB;
            panel2.Width = (int)fShouldBeWidth;

            Invalidate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            MyRepaint();
        }
    }
}
