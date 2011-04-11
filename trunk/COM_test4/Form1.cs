using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EconomyModel2;
using EconomyModel2.Buildings;
using EconomyModel2.Trading;

namespace COM_test4
{

    public partial class Form1 : Form
    {
        private CUniverse m_pUniverse = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_pUniverse = new CUniverse(40, 40, 15);
            //m_pUniverse = new CUniverse();

            economyModel2View1.DrawMap(m_pUniverse);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(m_pUniverse == null)
                return;

            m_pUniverse.Update();
            economyModel2View1.DrawMap();
        }

        private void economyModel2View1_WorldSelectedEvent(object sender, EconomyModel2Vis.EconomyModel2View.WorldSelectedEventArgs e)
        {
            CWorld pSelected = e.World;

            if (pSelected != null)
            {
                groupBox1.Text = pSelected.Name;
                label1.Text = string.Format("Pop: {0}/{1}", pSelected.Population, pSelected.Size);
                //label2.Text = string.Format("Hap: {0}%", (int)(pSelected.m_fHappiness * 100));
                //label3.Text = string.Format("$$$: {0}", (int)(pSelected.Wealth * 100));

                listBox1.Items.Clear();
                listBox2.Items.Clear();
                listBox3.Items.Clear();
                listBox4.Items.Clear();
                foreach (CBuilding building in pSelected.Buildings)
                {
                    listBox1.Items.Add(string.Format("{0}", building.ToString()));
                    foreach (Goods goods in building.Production.Keys)
                    {
                        if (building.Production[goods].Amount >= 0)
                            listBox1.Items.Add(string.Format("  {0} : {2}(+{1})", Enum.GetName(typeof(Goods), goods), building.Production[goods].Amount, pSelected.Stock.Stored(goods)));
                        else
                            listBox1.Items.Add(string.Format("  {0} : {2}({1})", Enum.GetName(typeof(Goods), goods), building.Production[goods].Amount, pSelected.Stock.Stored(goods)));
                    }
                }
                foreach (CTradingRecord record in pSelected.Spaceport.Offers.Values)
                {
                    listBox2.Items.Add(string.Format("-> {0} : {1} / {2}$", Enum.GetName(typeof(Goods), record.Goods), record.Count, (int)record.Price));
                }
                listBox2.Items.Add("");
                foreach (CTradingRecord record in pSelected.Spaceport.Demands.Values)
                {
                    listBox2.Items.Add(string.Format("<- {0} : {1} / {2}$", Enum.GetName(typeof(Goods), record.Goods), record.Count, (int)record.Price));
                }
                foreach (CTradingRoute route in pSelected.Spaceport.Routes)
                {
                    if (route.Seller == pSelected)
                    {
                        listBox3.Items.Add(string.Format("{0}", route.ToString()));
                    }
                    else
                    {
                        listBox4.Items.Add(string.Format("{0}", route.ToString()));
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            m_pUniverse = new CUniverse();

            economyModel2View1.DrawMap(m_pUniverse);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (m_pUniverse == null)
                return;

            m_pUniverse.Update();
            economyModel2View1.DrawMap();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (m_pUniverse == null)
                return;

            if (timer1.Enabled)
            {
                timer1.Enabled = false;
                button4.Text = "Start";
            }
            else
            {
                timer1.Enabled = true;
                button4.Text = "Stop";
            }
        }
    }
}
