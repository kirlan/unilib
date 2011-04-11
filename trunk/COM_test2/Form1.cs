using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StarMap;
using StarMapVis;

namespace COM_test2
{
    public partial class Form1 : Form
    {
        CUniverse m_pUniverse = null;

        public Form1()
        {
            InitializeComponent();

            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_pUniverse = new CUniverse();

            universeMap1.DrawMap(m_pUniverse);
            label1.Text = string.Format("Universe age: {0}", m_pUniverse.m_iAge);

            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (m_pUniverse == null)
                return;

            m_pUniverse.Advance();
            universeMap1.DrawMap();
            label1.Text = string.Format("Universe age: {0}", m_pUniverse.m_iAge);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            universeMap1.MapDraw = UniverseMap.MapDrawType.mdtRaces;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            universeMap1.MapDraw = UniverseMap.MapDrawType.mdtTechLevel;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            universeMap1.MapDraw = UniverseMap.MapDrawType.mdtSocium;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            universeMap1.MapDraw = UniverseMap.MapDrawType.mdtWorldType;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            universeMap1.MapDraw = UniverseMap.MapDrawType.mdtWealth;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = true;

            timer1.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = false;

            timer1.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            button2_Click(sender, e);
        }

        private void universeMap1_WorldSelectedEvent(object sender, UniverseMap.WorldSelectedEventArgs e)
        {
            CWorld pSelected = e.World;

            if (pSelected != null)
            {
                label2.Text = string.Format("Name: {0}", pSelected.ToString());
                label3.Text = string.Format("Race: {0} Size {2} Wealth {1}", pSelected.m_pMajorRace != null ? pSelected.m_pMajorRace.ToString() : "-", pSelected.m_pMajorRace != null ? pSelected.m_pMajorRace.m_iTotalWealth : 0, pSelected.m_pMajorRace != null ? pSelected.m_pMajorRace.m_cOwnedWorlds.Count : 0);
                //label4.Text = string.Format("Belongs to: {0}", pSelected.m_pTradingRoute != null ? pSelected.m_pTradingRoute.m_pMajorRace.ToString() : "-");
                label5.Text = string.Format("Wealth: {0}", pSelected.m_fWealth);
            }
        }
    }
}
