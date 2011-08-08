using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace MiscControls
{
    public partial class TrackBarCustom : UserControl
    {
        public TrackBarCustom()
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

        private int m_iValue = 5;
        public int Value
        {
            get { return m_iValue; }
            set 
            { 
                m_iValue = value;
                MyRepaint();
            }
        }

        private bool m_bSingleColor = false;
        public bool SingleColor
        {
            get { return m_bSingleColor; }
            set 
            { 
                m_bSingleColor = value;
                MyRepaint();
            }
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
                toolTip1.SetToolTip(panel3, m_sToolTip);
                toolTip1.SetToolTip(panel4, m_sToolTip);
                toolTip1.SetToolTip(panel5, m_sToolTip);
                toolTip1.SetToolTip(panel6, m_sToolTip);
                toolTip1.SetToolTip(panel7, m_sToolTip);
                toolTip1.SetToolTip(panel8, m_sToolTip);
                toolTip1.SetToolTip(panel9, m_sToolTip);
                toolTip1.SetToolTip(panel10, m_sToolTip);
                toolTip1.SetToolTip(tableLayoutPanel1, m_sToolTip);
            }
        }


        private void MyRepaint()
        {
            if (m_bSingleColor)
            {
                Color currentColor = Color.Black;
                switch (m_iValue)
                {
                    case 1:
                        currentColor = m_eColor01;
                        break;
                    case 2:
                        currentColor = m_eColor02;
                        break;
                    case 3:
                        currentColor = m_eColor03;
                        break;
                    case 4:
                        currentColor = m_eColor04;
                        break;
                    case 5:
                        currentColor = m_eColor05;
                        break;
                    case 6:
                        currentColor = m_eColor06;
                        break;
                    case 7:
                        currentColor = m_eColor07;
                        break;
                    case 8:
                        currentColor = m_eColor08;
                        break;
                    case 9:
                        currentColor = m_eColor09;
                        break;
                    case 10:
                        currentColor = m_eColor10;
                        break;
                }

                panel1.BackColor = (m_iValue > 0) ? currentColor : m_eColor00;
                panel2.BackColor = (m_iValue > 1) ? currentColor : m_eColor00;
                panel3.BackColor = (m_iValue > 2) ? currentColor : m_eColor00;
                panel4.BackColor = (m_iValue > 3) ? currentColor : m_eColor00;
                panel5.BackColor = (m_iValue > 4) ? currentColor : m_eColor00;
                panel6.BackColor = (m_iValue > 5) ? currentColor : m_eColor00;
                panel7.BackColor = (m_iValue > 6) ? currentColor : m_eColor00;
                panel8.BackColor = (m_iValue > 7) ? currentColor : m_eColor00;
                panel9.BackColor = (m_iValue > 8) ? currentColor : m_eColor00;
                panel10.BackColor = (m_iValue > 9) ? currentColor : m_eColor00;
            }
            else
            {
                panel1.BackColor = (m_iValue > 0) ? m_eColor01 : m_eColor00;
                panel2.BackColor = (m_iValue > 1) ? m_eColor02 : m_eColor00;
                panel3.BackColor = (m_iValue > 2) ? m_eColor03 : m_eColor00;
                panel4.BackColor = (m_iValue > 3) ? m_eColor04 : m_eColor00;
                panel5.BackColor = (m_iValue > 4) ? m_eColor05 : m_eColor00;
                panel6.BackColor = (m_iValue > 5) ? m_eColor06 : m_eColor00;
                panel7.BackColor = (m_iValue > 6) ? m_eColor07 : m_eColor00;
                panel8.BackColor = (m_iValue > 7) ? m_eColor08 : m_eColor00;
                panel9.BackColor = (m_iValue > 8) ? m_eColor09 : m_eColor00;
                panel10.BackColor = (m_iValue > 9) ? m_eColor10 : m_eColor00;
            }
        }
    }
}
