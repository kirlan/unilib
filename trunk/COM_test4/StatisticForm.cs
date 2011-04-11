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
    public partial class StatisticForm : Form
    {
        CUniverse m_pUniverse = null;

        public StatisticForm(CUniverse pUniverse)
        {
            InitializeComponent();

            m_pUniverse = pUniverse;

            PopulateFilter();

            PopulateTable();
        }

        private Commodity m_eSelectedCommodity = Commodity.None;

        private void PopulateFilter()
        {
            comboBox1.Items.Clear();
            foreach (Commodity commodity in CommodityCategorizer.All)
            {
                comboBox1.Items.Add(commodity);
            }
        }

        private void PopulateTable()
        {
            if (m_pUniverse == null)
                return;

            listView1.Items.Clear();

            int iTotalProd = 0;
            int iTotalSell = 0;
            int iTotalBuy = 0;
            foreach (CWorld pWorld in m_pUniverse.Worlds)
            { 
                ListViewItem newItem = new ListViewItem(pWorld.Name);
                newItem.Tag = pWorld;
                newItem.SubItems.Add(string.Format("{0,2}:{1,2}", pWorld.X, pWorld.Y));

                int iProduced = 0;
                bool bShow = false;
                foreach (CBuilding pBuilding in pWorld.Buildings)
                {
                    if (pBuilding != pWorld.Spaceport && (pBuilding.Production.ContainsKey(m_eSelectedCommodity) || m_eSelectedCommodity == Commodity.None))
                    {
                        if (m_eSelectedCommodity != Commodity.None)
                            iProduced += pBuilding.Production[m_eSelectedCommodity].Amount;
                        else
                        {
                            foreach (Storage stor in pBuilding.Production.Values)
                                iProduced += stor.Amount;
                        }
                        bShow = true;
                    }
                }

                iTotalProd += iProduced;

                if(bShow)
                    newItem.SubItems.Add(string.Format("{0}", iProduced));
                else
                    newItem.SubItems.Add("");

                if (m_eSelectedCommodity != Commodity.None)
                    newItem.SubItems.Add(string.Format("{0}", pWorld.Stock.Stored(m_eSelectedCommodity)));
                else
                {
                    int amount = 0;
                    foreach (Commodity comm in pWorld.Stock.Commodity)
                    {
                        amount += pWorld.Stock.Stored(comm);
                    }
                    newItem.SubItems.Add(string.Format("{0}", amount));
                }

                if (m_eSelectedCommodity != Commodity.None)
                {
                    if (pWorld.Spaceport.Offers.ContainsKey(m_eSelectedCommodity))
                    {
                        //newItem.SubItems.Add(string.Format("{0}", pWorld.Spaceport.Offers[m_eSelectedCommodity].ToString()));
                        newItem.SubItems.Add(string.Format("{0} x {1}$", pWorld.Spaceport.Offers[m_eSelectedCommodity].Count, (int)pWorld.Spaceport.Offers[m_eSelectedCommodity].Price));
                        iTotalSell += pWorld.Spaceport.Offers[m_eSelectedCommodity].Count;
                    }
                    else
                        newItem.SubItems.Add("");
                }
                else
                {
                    int count = 0;
                    double cost = 0;
                    foreach (CTradingRecord rec in pWorld.Spaceport.Offers.Values)
                    {
                        count += rec.Count;
                        cost += rec.Price;
                    }
                    newItem.SubItems.Add(string.Format("{0} x {1}$", count, (int)cost));
                }

                if (m_eSelectedCommodity != Commodity.None)
                {
                    if (pWorld.Spaceport.Demands.ContainsKey(m_eSelectedCommodity))
                    {
                        //newItem.SubItems.Add(string.Format("{0}", pWorld.Spaceport.Demands[m_eSelectedCommodity].ToString()));
                        newItem.SubItems.Add(string.Format("{0} x {1}$", pWorld.Spaceport.Demands[m_eSelectedCommodity].Count, (int)pWorld.Spaceport.Demands[m_eSelectedCommodity].Price));
                        iTotalBuy += pWorld.Spaceport.Demands[m_eSelectedCommodity].Count;
                    }
                    else
                        newItem.SubItems.Add("");
                }
                else
                {
                    int count = 0;
                    double cost = 0;
                    foreach (CTradingRecord rec in pWorld.Spaceport.Demands.Values)
                    {
                        count += rec.Count;
                        cost += rec.Price;
                    }
                    newItem.SubItems.Add(string.Format("{0} x {1}$", count, (int)cost));
                }


                if (m_eSelectedCommodity != Commodity.None)
                {
                    if (pWorld.Spaceport.Production.ContainsKey(m_eSelectedCommodity))
                        newItem.SubItems.Add(string.Format("{0}", pWorld.Spaceport.Production[m_eSelectedCommodity].ToString()));
                    else
                        newItem.SubItems.Add("");
                }
                else
                {
                    int amount = 0;
                    foreach (Commodity comm in pWorld.Spaceport.Production.Keys)
                    {
                        if(comm != Commodity.Credits)
                            amount += pWorld.Spaceport.Production[comm].Amount;
                    }
                    newItem.SubItems.Add(string.Format("{0}", amount));
                }

                //if (pWorld.Spaceport.Production.ContainsKey(m_eSelectedCommodity))
                //    newItem.SubItems.Add(string.Format("{0}", pWorld.Spaceport.Production[m_eSelectedCommodity].ToString()));
                //else
                    newItem.SubItems.Add("");

                listView1.Items.Add(newItem);
            }

            label2.Text = string.Format("Total Production: {0}", iTotalProd);
            label3.Text = string.Format("Total Offer: {0}", iTotalSell);
            label4.Text = string.Format("Total Demand: {0}", iTotalBuy);
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            m_eSelectedCommodity = (Commodity)comboBox1.SelectedItem;

            PopulateTable();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
                worldView1.ShowWorld((CWorld)item.Tag);
        }
    }
}
