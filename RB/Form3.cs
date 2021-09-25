using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RB.Socium;
using System.Collections;

namespace RB
{
    public partial class Form3 : Form
    {
        private CWorld m_pWorld = null;

        // Implements the manual sorting of items by columns.
        private class ListViewItemComparer : IComparer
        {
            private int m_iColumn;
            public int Column
            {
                get { return m_iColumn; }
            }
            private bool m_bStright = true;
            public bool Reverced
            {
                get { return !m_bStright; }
            }
            public void Reverce()
            {
                m_bStright = !m_bStright;
            }
            public ListViewItemComparer()
            {
                m_iColumn = 0;
            }
            public ListViewItemComparer(int column)
            {
                m_iColumn = column;
            }

            private bool CompareAsNumbers(object item1, object item2, out int iResult)
            {
                iResult = 0;

                int iX = 0;
                int iY = 0;
                if (!int.TryParse(((ListViewItem)item1).SubItems[m_iColumn].Text, out iX) ||
                    !int.TryParse(((ListViewItem)item2).SubItems[m_iColumn].Text, out iY))
                    return false;

                if (iX == iY)
                    iResult = 0;
                if (iX > iY && m_bStright)
                    iResult = -1;
                else
                    iResult = 1;

                return true;
            }

            private int CompareAsStrings(object item1, object item2)
            {
                if (m_bStright)
                    return String.Compare(((ListViewItem)item1).SubItems[m_iColumn].Text, ((ListViewItem)item2).SubItems[m_iColumn].Text);
                else
                    return String.Compare(((ListViewItem)item2).SubItems[m_iColumn].Text, ((ListViewItem)item1).SubItems[m_iColumn].Text);
            }

            public int Compare(object item1, object item2)
            {
                int iResult;
                if (CompareAsNumbers(item1, item2, out iResult))
                    return iResult;

                return CompareAsStrings(item1, item2);
            }
        }

        public Form3(CWorld pWorld)
        {
            InitializeComponent();

            m_pWorld = pWorld;

            listView1.Items.Clear();
            listView1.Groups.Clear();

            ListViewGroup pNoGroup = null;
            int iCoalitionNumber = 1;
            Dictionary<CPerson, ListViewGroup> cGroups = new Dictionary<CPerson, ListViewGroup>();
            foreach (var pCoalition in m_pWorld.m_cCoalitions)
            {
                if (pCoalition.Count <= 1)
                    continue;

                ListViewGroup pGroup = new ListViewGroup("Coalition " + iCoalitionNumber.ToString());
                iCoalitionNumber++;
                foreach (CPerson pMember in pCoalition)
                    cGroups[pMember] = pGroup;

                listView1.Groups.Add(pGroup);
            }
            if (cGroups.Count > 0)
            {
                pNoGroup = new ListViewGroup("Independent");
                listView1.Groups.Add(pNoGroup);
            }

            foreach (CPerson pPerson in m_pWorld.m_cFactions)
            {
                ListViewItem pItem = new ListViewItem(pPerson.ShortName);
                pItem.Tag = pPerson;

                pItem.SubItems.Add(pPerson.GetFactionSize().ToString());
                pItem.SubItems.Add(pPerson.GetFactionInfluence(true).ToString());

                if(cGroups.ContainsKey(pPerson))
                    pItem.Group = cGroups[pPerson];
                else
                    pItem.Group = pNoGroup;

                listView1.Items.Add(pItem);
            }

            listView1.ListViewItemSorter = new ListViewItemComparer(2);
            listView1.Sort();

            listView2.ListViewItemSorter = new ListViewItemComparer(3);
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Set the ListViewItemSorter property to a new ListViewItemComparer 
            // object. Setting this property immediately sorts the 
            // ListView using the ListViewItemComparer object.
            if (listView1.ListViewItemSorter != null)
            {
                if (((ListViewItemComparer)listView1.ListViewItemSorter).Column == e.Column)
                {
                    ((ListViewItemComparer)listView1.ListViewItemSorter).Reverce();
                    listView1.Sort();
                    return;
                }
            }
            listView1.ListViewItemSorter = new ListViewItemComparer(e.Column);
            listView1.Sort();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count == 0)
                return;

            CPerson pSelectedLeader = listView1.SelectedItems[0].Tag as CPerson;

            listView2.Items.Clear();
            //int iColumn = 0;
            //bool bReverce = false;
            //if (listView2.ListViewItemSorter != null)
            //{
            //    iColumn = ((ListViewItemComparer)listView2.ListViewItemSorter).Column;
            //    bReverce = ((ListViewItemComparer)listView2.ListViewItemSorter).Reverced;
            //}

            //listView2.ListViewItemSorter = new ListViewItemComparer(iColumn);
            //if (bReverce)
            //{
            //    ((ListViewItemComparer)listView2.ListViewItemSorter).Reverce();
            //}
            foreach (CPerson pLeader in m_pWorld.m_cFactions)
            {
                if (pLeader == pSelectedLeader)
                    continue;

                float fAttraction = CPerson.GetFactionAttraction(pSelectedLeader, pLeader);

                ListViewItem pItem = new ListViewItem(pLeader.ShortName);
                pItem.Tag = pLeader;
                pItem.SubItems.Add(pLeader.GetFactionSize().ToString());
                pItem.SubItems.Add(pLeader.GetFactionInfluence(true).ToString());
                pItem.SubItems.Add(fAttraction.ToString("F0"));

                listView2.Items.Add(pItem);
            }
            if(listView2.ListViewItemSorter != null)
                listView2.Sort();

            treeView1.Nodes.Clear();
            TreeNode pRoot = treeView1.Nodes.Add(pSelectedLeader.ToString());
            foreach (CPerson pMinion in pSelectedLeader.m_cMinions)
                AddFactionMember(pMinion, pRoot);
            treeView1.ExpandAll();
        }

        private void AddFactionMember(CPerson pMember, TreeNode pRoot)
        {
            TreeNode pSubRoot = pRoot.Nodes.Add(pMember.ToString());
            foreach (CPerson pMinion in pMember.m_cMinions)
                AddFactionMember(pMinion, pSubRoot);
        }

        private void listView2_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (listView2.ListViewItemSorter != null)
            {
                if (((ListViewItemComparer)listView2.ListViewItemSorter).Column == e.Column)
                {
                    ((ListViewItemComparer)listView2.ListViewItemSorter).Reverce();
                    listView2.Sort();
                    return;
                }
            }
            listView2.ListViewItemSorter = new ListViewItemComparer(e.Column);
            listView2.Sort();
        }

        private CPerson m_pSelectedRelative = null;

        private void listView2_ItemActivate(object sender, EventArgs e)
        {
            foreach (ListViewItem pItem in listView1.Items)
            {
                if (pItem.Tag == m_pSelectedRelative)
                {
                    pItem.Selected = true;
                    listView1.EnsureVisible(pItem.Index);
                    return;
                }
            }
        }

        private void listView2_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
            CPerson pNewSelection = (CPerson)e.Item.Tag;
            if (pNewSelection != m_pSelectedRelative)
            {
                m_pSelectedRelative = pNewSelection;
            }
        }
    }
}
