﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace nsUniLibControls
{
    public partial class StatView : UserControl
    {
        public StatView()
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
                toolTip1.SetToolTip(label1, m_sToolTip);
                toolTip1.SetToolTip(label2, m_sToolTip);
                toolTip1.SetToolTip(this, m_sToolTip);
            }
        }

        public void SetValue(int iValue, int iTime)
        {
            int iDelta = iValue - m_iValue;

            m_iValue = iValue;

            if (m_iValue > m_iMaxValue)
                m_iValue = m_iMaxValue;

            if (m_iValue < m_iMinValue)
                m_iValue = m_iMinValue;

            m_iShouldBeWidth = (int)((float)(panel1.ClientRectangle.Width) / (float)(m_iMaxValue - m_iMinValue) * (float)iValue);

            label2.Text = m_iValue.ToString();
            label1.Text = iDelta == 0 ? "":string.Format("({0}{1})", iDelta > 0 ? "+":"", iDelta);
            label1.ForeColor = iDelta > 0 ? Color.Lime : Color.Red;

            MyRepaint();
        }

        private void StatView_Resize(object sender, EventArgs e)
        {
            SetValue(m_iValue, 0);
        }

        private int m_iShouldBeWidth = 0;

        private void MyRepaint()
        {
            panel1.BackColor = m_eColor00;

            double fIntervalWidth = (double)panel1.ClientRectangle.Width / 9.0;
            int iInterval = (int)((float)m_iShouldBeWidth / fIntervalWidth);
            int iDelta = m_iShouldBeWidth - (int)(iInterval * fIntervalWidth);

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
            color3.Hue = color1.Hue + (int)((float)(color2.Hue - color1.Hue) * (float)iDelta / fIntervalWidth);
            color3.Lightness = color1.Lightness + (int)((float)(color2.Lightness - color1.Lightness) * (float)iDelta / fIntervalWidth);
            color3.Saturation = color1.Saturation + (int)((float)(color2.Saturation - color1.Saturation) * (float)iDelta / fIntervalWidth);

            panel2.BackColor = color3.RGB;
            panel2.Width = m_iShouldBeWidth;

            Invalidate();
        }
    }
}
