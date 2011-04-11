using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EconomyModel;
using EconomyModelVis;

namespace COM_test3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        CUniverse m_pUniverse = null;

        private void button1_Click(object sender, EventArgs e)
        {
            m_pUniverse = new CUniverse(40, 40, 15);

            economyModelView1.DrawMap(m_pUniverse);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_pUniverse.Advance();
            m_pUniverse.AddRoute();
            economyModelView1.DrawMap();
        }

        private void economyModelView1_WorldSelectedEvent(object sender, EconomyModelView.WorldSelectedEventArgs e)
        {
            CWorld pSelected = e.World;

            if (pSelected != null)
            {
                label1.Text = string.Format("Pop: {0}/{1}", pSelected.m_iPopulation, pSelected.m_iSize);
                label2.Text = string.Format("Hap: {0}%", (int)(pSelected.m_fHappiness*100));
                label3.Text = string.Format("$$$: {0}", (int)(pSelected.Wealth*100));

                listBox1.Items.Clear();
                listBox2.Items.Clear();
                listBox3.Items.Clear();
                listBox4.Items.Clear();
                foreach (Goods goods in Enum.GetValues(typeof(Goods)))
                {
                    if (pSelected.m_pProduction[goods] > 0)
                    {
                        listBox1.Items.Add(string.Format("{0} +{1} ({3})   :  {2}$", Enum.GetName(typeof(Goods), goods), (int)(pSelected.m_pProduction[goods] * pSelected.m_pProduction[goods] * pSelected.m_fHappiness), (int)(pSelected.m_pStock[goods].m_fPrice * 100), pSelected.m_pStock[goods].m_iStore));
                    }
                    if (pSelected.m_pConsumption[goods] > 0)
                    {
                        listBox2.Items.Add(string.Format("{0} -{1} ({3})   :  {2}$", Enum.GetName(typeof(Goods), goods), pSelected.m_pConsumption[goods], (int)(pSelected.m_pStock[goods].m_fPrice*100), pSelected.m_pStock[goods].m_iStore));
                    }
                }

                foreach (CTradingRoute pRoute in pSelected.m_cRoutes)
                {
                    if (pRoute.m_pSeller == pSelected)
                    {
                        listBox4.Items.Add(string.Format("{0} -{1} ({2}$)   :  {3}$", Enum.GetName(typeof(Goods), pRoute.m_eGoods), pRoute.Volume, (int)(pSelected.m_pStock[pRoute.m_eGoods].m_fPrice*100*pRoute.Volume), (int)(pRoute.Profit*100)));
                    }
                    else
                    {
                        listBox3.Items.Add(string.Format("{0} +{1} ({2}$)   :  {3}$", Enum.GetName(typeof(Goods), pRoute.m_eGoods), pRoute.Volume, (int)(pSelected.m_pStock[pRoute.m_eGoods].m_fPrice * 100 * pRoute.Volume), (int)(pRoute.Profit * 100)));
                    }
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            m_pUniverse.AddRoute();
            economyModelView1.DrawMap();
        }
    }
}
