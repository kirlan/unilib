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
using Random;

namespace RB
{
    public partial class Form2 : Form
    {
        private class CPersonItem : ListViewItem
        {
            private CPerson m_pPerson = null;
            public CPerson Person
            {
                get { return m_pPerson; }
            }

            public CPersonItem(CPerson man)
                : base(man.m_sName)
            {
                m_pPerson = man;

                SubItems.Add(man.m_sFamily);
                SubItems.Add(string.Format("{0}, {1}", man.m_pHomeLocation.FullName, man.m_pBuilding.ToString()));
                SubItems.Add(man.Gender.ToString());
                SubItems.Add(man.Age.ToString());
                SubItems.Add(string.Format("{1} ({0})", man.m_pEstate.m_ePosition, man.m_pEstate.m_sName));
                SubItems.Add(man.Gender == CPerson._Gender.Male ? man.m_pProfession.m_sNameM : man.m_pProfession.m_sNameF);
                SubItems.Add(man.m_cRelations.Count.ToString());
                int iInf1 = man.GetInfluence(false);
                SubItems.Add(iInf1.ToString());
                int iInf2 = man.GetInfluence(true);
                SubItems.Add(iInf2.ToString());
                double fConflict = 0;
                double fBackConflict = 0;
                foreach (var pRelative in m_pPerson.m_cRelations)
                {
                    //double fWeight1 = Math.Sqrt(pRelative.Key.GetInfluence(true) * m_pPerson.GetInfluence(true));
                    //double fWeight2 = fWeight1 / Math.Max(pRelative.Key.GetInfluence(true), m_pPerson.GetInfluence(true));
                    //int iProximity = CPerson.GetProximity(m_pPerson, pRelative.Key);
                    //int iRelationWeight = (int)(fWeight2 * iProximity * 100);
                    
                    //int iPersonHostility = -m_pPerson.CalcHostility(pRelative.Key, false);
                    //int iRelativeHostility = -pRelative.Key.CalcHostility(m_pPerson, false);

                    //int iPersonConflict = iPersonHostility * iProximity * m_pPerson.GetInfluence(true) / 100;
                    //int iRelativeConflict = iRelativeHostility * iProximity * pRelative.Key.GetInfluence(true) / 100;
                    fConflict += Math.Pow(m_pPerson.GetAttitudeTo(pRelative.Key), 3);//iRelationWeight * (iAtt1 + iAtt2) / 100;
                    fBackConflict += Math.Pow(pRelative.Key.GetAttitudeTo(m_pPerson), 3);//iRelationWeight * (iAtt1 + iAtt2) / 100;
                }
                if (m_pPerson.m_cRelations.Count > 0)
                {
                    fConflict /= m_pPerson.m_cRelations.Count;
                    fBackConflict /= m_pPerson.m_cRelations.Count;
                }

                double iConflict;
                if (fConflict >= 0)
                    iConflict = Math.Pow(fConflict, 1.0 / 3);
                else
                    iConflict = -Math.Pow(-fConflict, 1.0 / 3);

                SubItems.Add(((int)iConflict).ToString());

                double iBackConflict;
                if (fBackConflict >= 0)
                    iBackConflict = Math.Pow(fBackConflict, 1.0 / 3);
                else
                    iBackConflict = -Math.Pow(-fBackConflict, 1.0 / 3);

                SubItems.Add(((int)iBackConflict).ToString());

                int iDepth;
                CPerson pFaction = m_pPerson.GetFaction(out iDepth);
                if (pFaction != null)
                    SubItems.Add(string.Format("{1} ({0})", iDepth, pFaction.ShortName));
                else
                    SubItems.Add("");
                //int iEnvy = man.GetEnvy();
                //SubItems.Add(iEnvy.ToString());
                //int iArrogance = man.GetArrogance();
                //SubItems.Add(iArrogance.ToString());
                //int iDiscontent = iEnvy > iArrogance ? iEnvy - iArrogance : 0;
                //SubItems.Add(iDiscontent.ToString());
                //SubItems.Add(man.GetPowerHunger().ToString());
            }
        }

        // Implements the manual sorting of items by columns.
        private class ListViewItemComparer : IComparer
        {
            private int m_iColumn;
            public int Column
            {
                get { return m_iColumn; }
            }
            private bool m_bStright = false;
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
                string sText1 = ((ListViewItem)item1).SubItems[m_iColumn].Text;
                string sText2 = ((ListViewItem)item2).SubItems[m_iColumn].Text;

                if (string.IsNullOrEmpty(sText1))
                    sText1 = "zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz";

                if (string.IsNullOrEmpty(sText2))
                    sText2 = "zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz";

                if (m_bStright)
                    return String.Compare(sText1, sText2);
                else
                    return String.Compare(sText2, sText1);
            }

            public int Compare(object item1, object item2)
            {
                //сортировка по возрасту
                if (m_iColumn == 4)
                {
                    if (((CPersonItem)item1).Person.Age == ((CPersonItem)item2).Person.Age)
                        return 0;
                    if (((CPersonItem)item1).Person.Age < ((CPersonItem)item2).Person.Age && m_bStright)
                        return -1;
                    else
                        return 1;
                }
                //сортировка по сословтю
                if (m_iColumn == 5)
                {
                    //принадлежат ли они оба к одному сословию?
                    if (((CPersonItem)item1).Person.m_pEstate.m_ePosition == ((CPersonItem)item2).Person.m_pEstate.m_ePosition)
                        return CompareAsStrings(item1, item2);

                    if (((CPersonItem)item1).Person.m_pEstate.m_ePosition > ((CPersonItem)item2).Person.m_pEstate.m_ePosition && m_bStright)
                        return -1;
                    else
                        return 1;
                }
                //сортировка по профессии
                if (m_iColumn == 6)
                {
                    //принадлежат ли они оба к одному сословию?
                    if (((CPersonItem)item1).Person.m_pEstate.m_ePosition == ((CPersonItem)item2).Person.m_pEstate.m_ePosition)
                    {
                        //в рамках одного сословия правитель государства стоит выше
                        if (((CPersonItem)item1).Person.IsRuler(false) == ((CPersonItem)item2).Person.IsRuler(false))
                        {
                            //на второй ступеньке - наследники и члены правительства
                            if (((CPersonItem)item1).Person.IsInGovernment() == ((CPersonItem)item2).Person.IsInGovernment())
                            {
                                //затем - региональные правители
                                if (((CPersonItem)item1).Person.IsRuler() == ((CPersonItem)item2).Person.IsRuler())
                                {
                                    //затем - главы отдельных домов и предприятий
                                    if (((CPersonItem)item1).Person.m_pProfession.m_bMaster == ((CPersonItem)item2).Person.m_pProfession.m_bMaster)
                                    {
                                        return CompareAsStrings(item1, item2);
                                    }
                                    if (((CPersonItem)item1).Person.m_pProfession.m_bMaster && m_bStright)
                                        return -1;
                                    else
                                        return 1;
                                }
                                if (((CPersonItem)item1).Person.IsRuler() && m_bStright)
                                    return -1;
                                else
                                    return 1;
                            }
                            if (((CPersonItem)item1).Person.IsInGovernment() && m_bStright)
                                return -1;
                            else
                                return 1;
                        }
                        if (((CPersonItem)item1).Person.IsRuler(false) && m_bStright)
                            return -1;
                        else
                            return 1;
                    }

                    if (((CPersonItem)item1).Person.m_pEstate.m_ePosition > ((CPersonItem)item2).Person.m_pEstate.m_ePosition && m_bStright)
                        return -1;
                    else
                        return 1;
                }

                int iResult;
                if (CompareAsNumbers(item1, item2, out iResult))
                    return iResult;

                return CompareAsStrings(item1, item2);
            }
        }

        // Implements the manual sorting of items by columns.
        private class ListViewItemComparer2 : IComparer
        {
            private CPerson m_pPerson0 = null;
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
            public ListViewItemComparer2()
            {
                m_iColumn = 0;
            }
            public ListViewItemComparer2(int column, CPerson pPerson)
            {
                m_iColumn = column;
                m_pPerson0 = pPerson;
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
                    iResult = - 1;
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
                if (m_iColumn == 0)
                {
                    CPerson pPerson1 = (CPerson)((ListViewItem)item1).Tag;
                    CPerson pPerson2 = (CPerson)((ListViewItem)item2).Tag;

                    if (m_pPerson0.m_cRelations.ContainsKey(pPerson1) &&
                        m_pPerson0.m_cRelations.ContainsKey(pPerson2))
                    {
                        CPerson.Relation eRelation1 = m_pPerson0.m_cRelations[pPerson1];
                        CPerson.Relation eRelation2 = m_pPerson0.m_cRelations[pPerson2];

                        if (eRelation1 == eRelation2)
                        {
                            bool bHome1 = m_pPerson0.m_pHomeLocation == pPerson1.m_pHomeLocation;
                            bool bHome2 = m_pPerson0.m_pHomeLocation == pPerson2.m_pHomeLocation;
                            if (bHome1 == bHome2)
                            {
                                if (m_bStright)
                                    return String.Compare(pPerson1.ToString(), pPerson2.ToString());
                                else
                                    return String.Compare(pPerson2.ToString(), pPerson1.ToString());
                            }
                            else if (bHome1 && m_bStright)
                                return -1;
                            else
                                return 1; 
                        }
                        else
                        {
                            if (eRelation1 > eRelation2 && m_bStright)
                                return -1;
                            else
                                return 1;
                        }
                    }
                    else 
                    {
                        if (m_pPerson0.m_cRelations.ContainsKey(pPerson1))
                            return m_bStright ? -1 : 1;
                        else if (m_pPerson0.m_cRelations.ContainsKey(pPerson2))
                            return m_bStright ? 1 : -1;
                        else if (m_bStright)
                            return String.Compare(pPerson1.ToString(), pPerson2.ToString());
                        else
                            return String.Compare(pPerson2.ToString(), pPerson1.ToString());
                    }
                }

                int iResult;
                if (CompareAsNumbers(item1, item2, out iResult))
                    return iResult;

                return CompareAsStrings(item1, item2);
            }
        }

        CWorld m_pWorld = null;

        public Form2(CWorld pWorld)
        {
            InitializeComponent();

            m_pWorld = pWorld;
            
            //float fMinTension = float.MaxValue;
            //KeyValuePair<CPerson, CPerson> pBestPair;
            foreach (CPerson pMan in m_pWorld.m_cPersons)
            {
                CPersonItem pItem = new CPersonItem(pMan);

                //foreach(var pRelation in pMan.m_cRelations)
                //{
                //    float fTension = CPerson.GetTension(pMan, pRelation.Key);
                //    if (fTension < fMinTension)
                //    {
                //        fMinTension = fTension;
                //        pBestPair = new KeyValuePair<CPerson, CPerson>(pMan, pRelation.Key);
                //    }
                //}

                populationList.Items.Add(pItem);
            }
            populationList.ListViewItemSorter = new ListViewItemComparer(9);
            populationList.Sort();
        }

        CPerson m_pSelected = null;

        public CPerson SelectedPerson
        {
            get { return m_pSelected; }
        }

        private void Form2_Shown(object sender, EventArgs e)
        {
            if (populationList.SelectedIndices.Count == 0)
                populationList.SelectedIndices.Add(0);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection items = populationList.SelectedItems;

            if (items.Count > 0)
            {
                m_pSelected = ((CPersonItem)items[0]).Person;

                label1.Text = m_pSelected.ToString();

                relativesList.Items.Clear();
                int iColumn = 0;
                bool bReverce = false;
                if (relativesList.ListViewItemSorter != null)
                {
                    iColumn = ((ListViewItemComparer2)relativesList.ListViewItemSorter).Column;
                    bReverce = ((ListViewItemComparer2)relativesList.ListViewItemSorter).Reverced;
                }

                relativesList.ListViewItemSorter = new ListViewItemComparer2(iColumn, m_pSelected);
                if (bReverce)
                {
                    ((ListViewItemComparer2)relativesList.ListViewItemSorter).Reverce();
                    relativesList.Sort();
                }

                CPerson pNemezis = null;
                CPerson pSoulmate = null;
                float fMinAttraction = 0;
                float fMaxAttraction = 0;
                foreach (var pRelative in m_pSelected.m_cRelations)
                {
                    string sRelation = pRelative.Key.WhoAmITo(m_pSelected);

                    ListViewItem pItem;

                    if (pRelative.Key.m_pHomeLocation == m_pSelected.m_pHomeLocation)
                        pItem = new ListViewItem(string.Format("{0}", sRelation));
                    else
                        pItem = new ListViewItem(string.Format("({0})", sRelation));

                    pItem.SubItems.Add(pRelative.Key.ToString());
                    pItem.SubItems.Add(CPerson.GetProximity(m_pSelected, pRelative.Key).ToString("P0"));

                    int iMutual = 0;
                    foreach (var pFarRelative in pRelative.Key.m_cRelations)
                        if (m_pSelected.m_cRelations.ContainsKey(pFarRelative.Key))
                            iMutual++;
                    pItem.SubItems.Add(iMutual.ToString());
                    //int iEnvy = m_pSelected.GetEnvy(pRelative.Key);
                    //pItem.SubItems.Add(iEnvy.ToString());
                    pItem.SubItems.Add(pRelative.Key.GetInfluence(true).ToString());
                    //double fWeight1 = Math.Sqrt(pRelative.Key.GetInfluence(true) * m_pSelected.GetInfluence(true));
                    //double fWeight2 = fWeight1 / Math.Max(pRelative.Key.GetInfluence(true), m_pSelected.GetInfluence(true));
                    //float fProximity = CPerson.GetProximity(m_pSelected, pRelative.Key);
                    //int iRelationWeight = (int)(fWeight2 * iProximity * 100);
                    //pItem.SubItems.Add(iRelationWeight.ToString());
                    //pItem.SubItems.Add(m_pSelected.GetAdmiration(pRelative.Key).ToString());
                    //pItem.SubItems.Add(m_pSelected.GetArrogance(pRelative.Key).ToString());
                    //pItem.SubItems.Add(m_pSelected.GetCompassion(pRelative.Key).ToString());
                    float fSelectedHostility = -m_pSelected.CalcNormalizedHostility(pRelative.Key, false);
                    float fRelativeHostility = -pRelative.Key.CalcNormalizedHostility(m_pSelected, false);
                    pItem.SubItems.Add(fSelectedHostility.ToString("F2"));
                    pItem.SubItems.Add(fRelativeHostility.ToString("F2"));
                    //pItem.SubItems.Add((iSelectedHostility + iRelativeHostility).ToString());
                    //pItem.SubItems.Add((iRelationWeight*(iAtt1 + iAtt2)/100).ToString());
                    int iWRel1 = (int)m_pSelected.GetAttitudeTo(pRelative.Key);
                    pItem.SubItems.Add(iWRel1.ToString());
                    int iWRel2 = (int)pRelative.Key.GetAttitudeTo(m_pSelected);
                    pItem.SubItems.Add(iWRel2.ToString());
                    float fAttraction = CPerson.GetAttraction(m_pSelected, pRelative.Key);
                    pItem.SubItems.Add(((int)fAttraction).ToString());

                    if (fAttraction < fMinAttraction)
                    {
                        fMinAttraction = fAttraction;
                        pNemezis = pRelative.Key;
                    }
                    if (fAttraction > fMaxAttraction)
                    {
                        fMaxAttraction = fAttraction;
                        pSoulmate = pRelative.Key;
                    }
                    pItem.Tag = pRelative.Key;

                    relativesList.Items.Add(pItem);
                }
                relativesList.Sort();

                if (pNemezis == null)
                {
                    label3.Text = "- none\n";
                }
                else
                {
                    string sRelation = pNemezis.WhoAmITo(m_pSelected);
                    if (pNemezis.m_pHomeLocation == m_pSelected.m_pHomeLocation)
                        label3.Text = string.Format("{0} \t\t: {1}\n", sRelation, pNemezis.ToString());
                    else
                        label3.Text = string.Format("({0}) \t\t: {1}\n", sRelation, pNemezis.ToString());
                }

                if (pSoulmate == null)
                {
                    label5.Text = "- none\n";
                }
                else
                {
                    string sRelation = pSoulmate.WhoAmITo(m_pSelected);
                    if (pSoulmate.m_pHomeLocation == m_pSelected.m_pHomeLocation)
                        label5.Text = string.Format("{0} \t\t: {1}\n", sRelation, pSoulmate.ToString());
                    else
                        label5.Text = string.Format("({0}) \t\t: {1}\n", sRelation, pSoulmate.ToString());
                }

                string sDescription;
                m_pSelected.CalcHostility(m_pSelected, out sDescription, false);
                selectedPersonDescription.Text = m_pSelected.GetDescription();
                selectedPersonDescription.Text += "\nSelf-esteem:\n" + sDescription;

                //DNAPanel.Refresh();
            }
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Set the ListViewItemSorter property to a new ListViewItemComparer 
            // object. Setting this property immediately sorts the 
            // ListView using the ListViewItemComparer object.
            if (populationList.ListViewItemSorter != null)
            {
                if (((ListViewItemComparer)populationList.ListViewItemSorter).Column == e.Column)
                {
                    ((ListViewItemComparer)populationList.ListViewItemSorter).Reverce();
                    populationList.Sort();
                    return;
                }
            }
            populationList.ListViewItemSorter = new ListViewItemComparer(e.Column);
            populationList.Sort();
        }

        private void listView2_ItemActivate(object sender, EventArgs e)
        {
            foreach (CPersonItem pItem in populationList.Items)
            {
                if (pItem.Person == m_pSelectedRelative)
                {
                    pItem.Selected = true;
                    populationList.EnsureVisible(pItem.Index);
                    return;
                }
            }
        }

        private CPerson m_pSelectedRelative = null;

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection items = relativesList.SelectedItems;

            if (items.Count > 0 && m_pSelected != null)
            {
                m_pSelectedRelative = (CPerson)items[0].Tag;
                string sDescription;
                m_pSelected.CalcHostility(m_pSelectedRelative, out sDescription, false);
                selectedRelativeDescription.Text = "Attitude:\n" + sDescription;
            }
        }

        private void listView2_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
            CPerson pNewSelection = (CPerson)e.Item.Tag;
            if(pNewSelection != m_pSelectedRelative)
            {
                m_pSelectedRelative = pNewSelection;
            }
        }

        private void listView2_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Set the ListViewItemSorter property to a new ListViewItemComparer 
            // object. Setting this property immediately sorts the 
            // ListView using the ListViewItemComparer object.
            if (relativesList.ListViewItemSorter != null)
            {
                if (((ListViewItemComparer2)relativesList.ListViewItemSorter).Column == e.Column)
                {
                    ((ListViewItemComparer2)relativesList.ListViewItemSorter).Reverce();
                    relativesList.Sort();
                    return;
                }
            }
            relativesList.ListViewItemSorter = new ListViewItemComparer2(e.Column, m_pSelected);
            relativesList.Sort();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (m_pSelected == null)
            {
                goToToolStripMenuItem.Text = "Go to...";
                goToToolStripMenuItem.Enabled = false;

                markAsTargetToolStripMenuItem.Text = "Mark as target...";
                markAsTargetToolStripMenuItem.Enabled = false;
            }
            else 
            {
                goToToolStripMenuItem.Text = "Go to " + m_pSelected.m_pHomeLocation.FullName;
                goToToolStripMenuItem.Enabled = true;

                markAsTargetToolStripMenuItem.Text = "Mark " + m_pSelected.ShortName + " as target...";
                markAsTargetToolStripMenuItem.Enabled = true;
            }
        }

        private void goToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Retry;
        }

        private void markAsTargetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
        }

        private void generatePlotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int iMaxInf = 0;
            CPerson pTreasureKeeper = null;
            foreach (CPerson man in m_pWorld.m_cPersons)
            {
                int iInf = man.GetInfluence(true);
                if (iInf > iMaxInf)
                {
                    iMaxInf = iInf;
                    pTreasureKeeper = man;
                }
            }

            CPerson pLord = pTreasureKeeper;
            CPerson pVictim = null;

            float fMaxProximity = 0;
            int iMinInf = int.MaxValue;
            foreach (var man in pLord.m_cRelations)
            {
                float fProx = CPerson.GetProximity(pLord, man.Key);
                if (fProx > fMaxProximity)
                {
                    fMaxProximity = fProx;
                    pVictim = man.Key;
                    iMinInf = pVictim.GetInfluence(true);
                }
                else if (fProx == fMaxProximity)
                {
                    int iInf = pVictim.GetInfluence(true);
                    if (iInf < iMinInf)
                    {
                        pVictim = man.Key;
                        iMinInf = iInf;
                    }
                }
            }

            CPerson pVillain = null;

            Dictionary<CPerson, float> cPossibleVillains = new Dictionary<CPerson, float>();

            foreach (var man in pLord.m_cRelations)
            {
                if (man.Key == pVictim)
                    continue;

                int iInf = man.Key.GetInfluence(true);
                if (man.Key.m_cRelations.ContainsValue(CPerson.Relation.AlterEgo))
                    iInf *= 2;

                if (man.Key.m_cRelations.ContainsKey(pVictim))
                    iInf *= 2;

                cPossibleVillains[man.Key] = iInf;
            }

            pVillain = cPossibleVillains.ElementAt(Rnd.ChooseOne(cPossibleVillains.Values, 2)).Key;
        }

    }
}
