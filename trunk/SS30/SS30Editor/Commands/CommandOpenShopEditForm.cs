using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SS30Conf;
using SS30Conf.Actions;
using SS30Conf.Actions.Conditions;
using SS30Conf.Actions.Commands;
using SS30Conf.Items;

namespace SS30Editor
{
    public partial class CommandOpenShopEditForm : Form
    {
        private CCommandOpenShop m_pCommand;

        public CommandOpenShopEditForm(CCommandOpenShop pCommand)
        {
            InitializeComponent();

            m_pCommand = pCommand;

            textBox1.Text = pCommand.Value;

            CommoditiesListView.Items.Clear();
            foreach (CCommodity commodity in pCommand.Commodities)
            {
                ListViewItem pItem = new ListViewItem(commodity.Value);
                pItem.SubItems.Add(commodity.ToString());
                pItem.Tag = commodity;

                CommoditiesListView.Items.Add(pItem);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_pCommand.Value = textBox1.Text;

            m_pCommand.Commodities.Clear();
            foreach (ListViewItem item in CommoditiesListView.Items)
            {
                m_pCommand.Commodities.Add(item.Tag as CCommodity);
            }

            DialogResult = DialogResult.OK;
        }

        private void addNewCommodityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CCommodity pCommodity = new CCommodity(m_pCommand);
            m_pCommand.Commodities.Add(pCommodity);

            ListViewItem pItem = AddCommodity(pCommodity);
            CommoditiesListView.FocusedItem = pItem;
            EditCommodity(pItem);
        }

        private ListViewItem AddCommodity(CCommodity pCommodity)
        {
            ListViewItem pItem = new ListViewItem(pCommodity.Value);
            pItem.SubItems.Add(pCommodity.ToString());
            pItem.Tag = pCommodity;

            CommoditiesListView.Items.Add(pItem);
            UpdateStringColor(pItem);

            return pItem;
        }

        private void EditCommodity(ListViewItem item)
        {
            CCommodity pSelectedCommodity = item.Tag as CCommodity;
            Form form = new CommodityEditForm(pSelectedCommodity);

            if (form != null && form.ShowDialog() == DialogResult.OK)
            {
                item.Text = pSelectedCommodity.Value;
                item.SubItems[1].Text = pSelectedCommodity.ToString();
                UpdateStringColor(item);
            }

        }

        private void UpdateStringColor(ListViewItem pItem)
        {
            CConfigObject pString = pItem.Tag as CConfigObject;

            switch (pString.State)
            {
                case ModifyState.Unmodified:
                    pItem.ForeColor = Color.Black;
                    break;
                case ModifyState.Added:
                    pItem.ForeColor = Color.DarkGreen;
                    break;
                case ModifyState.Modified:
                    pItem.ForeColor = Color.Blue;
                    break;
                case ModifyState.Erased:
                    pItem.ForeColor = Color.DarkRed;
                    break;
            }
        }

        private void CommodityEditMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in CommoditiesListView.SelectedItems)
            {
                EditCommodity(item);
            }
        }

        private void deleteCommodityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in CommoditiesListView.SelectedItems)
            {
                CCommodity pSelectedCommodity = item.Tag as CCommodity;

                if (pSelectedCommodity.Delete())
                {
                    m_pCommand.Commodities.Remove(pSelectedCommodity);
                    CommoditiesListView.Items.Remove(item);
                }
                else
                    UpdateStringColor(item);
            }
        }

    }
}
