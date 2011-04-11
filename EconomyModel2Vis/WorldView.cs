using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EconomyModel2;
using EconomyModel2.Trading;
using EconomyModel2.Buildings;

namespace EconomyModel2Vis
{
    public partial class WorldView : UserControl
    {
        public WorldView()
        {
            InitializeComponent();
        }

        public void ShowWorld(CWorld pSelected)
        {
            if (pSelected != null)
            {
                groupBox1.Text = pSelected.Name;
                pop.Text = string.Format("Pop: {0}/{1}", pSelected.Population, pSelected.Size);
                //label2.Text = string.Format("Hap: {0}%", (int)(pSelected.m_fHappiness * 100));
                //label3.Text = string.Format("$$$: {0}", (int)(pSelected.Wealth * 100));

                production.Items.Clear();
                market.Items.Clear();
                export.Items.Clear();
                import.Items.Clear();
                foreach (CBuilding building in pSelected.Buildings)
                {
                    production.Items.Add(string.Format("{0}", building.ToString()));
                    foreach (Commodity commodity in building.Production.Keys)
                    {
                        if (building.Production[commodity].Amount >= 0)
                            production.Items.Add(string.Format("  {0} : {2}(+{1})", Enum.GetName(typeof(Commodity), commodity), building.Production[commodity].Amount, pSelected.Stock.Stored(commodity)));
                        else
                            production.Items.Add(string.Format("  {0} : {2}({1})", Enum.GetName(typeof(Commodity), commodity), building.Production[commodity].Amount, pSelected.Stock.Stored(commodity)));
                    }
                }
                foreach (CTradingRecord record in pSelected.Spaceport.Offers.Values)
                {
                    market.Items.Add(string.Format("-> {0}", record.ToString()));
                }
                market.Items.Add("");
                foreach (CTradingRecord record in pSelected.Spaceport.Demands.Values)
                {
                    market.Items.Add(string.Format("<- {0}", record.ToString()));
                }
                foreach (CTradingRoute route in pSelected.Spaceport.Routes)
                {
                    if (route.Seller == pSelected)
                    {
                        export.Items.Add(string.Format("{0}", route.ToString()));
                    }
                    else
                    {
                        import.Items.Add(string.Format("{0}", route.ToString()));
                    }
                }
            }
        }
    }
}
