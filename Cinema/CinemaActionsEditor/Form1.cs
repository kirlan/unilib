using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CinemaEngine;
using System.Collections;
using GlacialComponents.Controls;

namespace CinemaActionsEditor
{
    public partial class Form1 : Form
    {
        GenreTag.Genre m_eSelectedGenre = GenreTag.Genre.Action;

        public Form1()
        {
            InitializeComponent();
            glacialList1.SortColumn(3);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Universe.Instance.Clear();
            Clear();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Universe.Instance.Read(openFileDialog1.FileName);
                ShowTags();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                Universe.Instance.Write(saveFileDialog1.FileName);
        }

        private void Clear()
        {
            ActionsListView.Items.Clear();
            glacialList1.Items.Clear();
            radioAction.Enabled = true;
        }

        private void radio_CheckedChanged(object sender, EventArgs e)
        {
            if (radioAction.Checked)
                m_eSelectedGenre = GenreTag.Genre.Action;
            if (radioFun.Checked)
                m_eSelectedGenre = GenreTag.Genre.Fun;
            if (radioRomance.Checked)
                m_eSelectedGenre = GenreTag.Genre.Romance;
            if (radioHorror.Checked)
                m_eSelectedGenre = GenreTag.Genre.Horror;
            if (radioXXX.Checked)
                m_eSelectedGenre = GenreTag.Genre.XXX;
            if (radioGore.Checked)
                m_eSelectedGenre = GenreTag.Genre.Gore;
            if (radioMistery.Checked)
                m_eSelectedGenre = GenreTag.Genre.Mistery;
            if (radioUnrated.Checked)
                m_eSelectedGenre = GenreTag.Genre.Unrated;

            ShowTags();

            ActionsListView.ListViewItemSorter = new ActionsComparer(m_eSelectedGenre);
        }

        private void ShowActions()
        {
            ActionsListView.BeginUpdate();
            ActionsListView.Items.Clear();

            List<string> cShow = new List<string>();
            List<string> cHide = new List<string>();
            List<string> cAll = new List<string>();

            foreach (GLItem pItem in glacialList1.Items)
            {
                if (pItem.SubItems[1].Checked)
                    cShow.Add(pItem.Tag as string);
                if (pItem.SubItems[2].Checked)
                    cHide.Add(pItem.Tag as string);
                cAll.Add(pItem.Tag as string);
            }
            
            foreach (CinemaEngine.Action pAction in Universe.Instance.Actions)
            {
                bool bPermitted = true;
                //if (SubGenresListBox.CheckedItems.Count > 0)
                //{
                //    bPermitted = true;

                //    foreach (string sTag in SubGenresListBox.CheckedItems)
                //    {
                //        if (!pAction.Tags.Contains(Repository.Instance.Tags[sTag]))
                //        {
                //            bPermitted = false;
                //            break;
                //        }
                //    }
                //}
                //else
                //{
                //    bPermitted = false;
                //    foreach (GenreTag pTag in pAction.Tags)
                //    {
                //        if (SubGenresListBox.Items.Contains(pTag.Name))
                //        {
                //            bPermitted = true;
                //            break;
                //        }
                //    }
                //}
                if (cShow.Count > 0)
                {
                    bPermitted = true;

                    foreach (string sTag in cShow)
                    {
                        if (!pAction.Tags.Contains(Universe.Instance.Tags[sTag]))
                        {
                            bPermitted = false;
                            break;
                        }
                    }
                }
                else
                {
                    bPermitted = false;
                    foreach (GenreTag pTag in pAction.Tags)
                    {
                        if (cAll.Contains(pTag.Name))
                        {
                            bPermitted = true;
                            break;
                        }
                    }
                }

                foreach (GenreTag pTag in pAction.Tags)
                {
                    if (cHide.Contains(pTag.Name))
                    {
                        bPermitted = false;
                        break;
                    }
                }

                if(bPermitted)
                {
                    ListViewItem pNewItem = new ListViewItem(pAction.Name);
                    pNewItem.SubItems.Add(pAction.Roles.Count.ToString());

                    string sSubGenreString = "";
                    foreach (GenreTag pTag in pAction.Tags)
                    {
                        if (string.IsNullOrEmpty(sSubGenreString))
                            sSubGenreString += pTag.Name;
                        else
                            sSubGenreString += ", " + pTag.Name;
                    }
                    pNewItem.SubItems.Add(sSubGenreString);

                    pNewItem.Tag = pAction;

                    ActionsListView.Items.Add(pNewItem);
                }
            }

            ActionsListView.Sort();
            ActionsListView.EndUpdate();
            ResortTags();
        }

        private void ShowTags()
        {
            //List<string> cChecked = new List<string>();

            //foreach (string sChecked in SubGenresListBox.CheckedItems)
            //    cChecked.Add(sChecked);

            //SubGenresListBox.Items.Clear();

            //foreach (string sGenreTag in Repository.Instance.Tags.Keys)
            //{
            //    if (!SubGenresListBox.Items.Contains(sGenreTag) &&
            //        (Repository.Instance.Tags[sGenreTag].Rating[m_eSelectedGenre] > 0 ||
            //         (m_eSelectedGenre == Genre.Unrated && Repository.Instance.Tags[sGenreTag].FullRating == 0)))
            //    {
            //        int index = SubGenresListBox.Items.Add(sGenreTag);
            //        if (cChecked.Contains(sGenreTag))
            //            SubGenresListBox.SetItemChecked(index, true);
            //    }
            //}

            List<string> cShow = new List<string>();
            List<string> cHide = new List<string>();

            foreach (GLItem pItem in glacialList1.Items)
            {
                if (pItem.SubItems[1].Checked)
                    cShow.Add(pItem.Text);
                if (pItem.SubItems[2].Checked)
                    cHide.Add(pItem.Text);
            }

            glacialList1.Items.Clear();
            foreach (string sGenreTag in Universe.Instance.Tags.Keys)
            {
                if (Universe.Instance.Tags[sGenreTag].Rating[m_eSelectedGenre] > 0 ||
                     (m_eSelectedGenre == GenreTag.Genre.Unrated && Universe.Instance.Tags[sGenreTag].FullRating == 0))
                {
                    GLItem pNewItem = glacialList1.Items.Add(sGenreTag);
                    if (cShow.Contains(sGenreTag))
                        pNewItem.SubItems[1].Checked = true;
                    if (cHide.Contains(sGenreTag))
                        pNewItem.SubItems[2].Checked = true;

                    pNewItem.Tag = sGenreTag;
                }
            }

            glacialList1.Refresh();
            ShowActions();
        }

        private void ResortTags()
        {
            Dictionary<string, int> cTagsCount = new Dictionary<string, int>();

            foreach (ListViewItem pActionItem in ActionsListView.Items)
            {
                CinemaEngine.Action pAction = pActionItem.Tag as CinemaEngine.Action;
                foreach (GenreTag pTag in pAction.Tags)
                {
                    if (cTagsCount.ContainsKey(pTag.Name))
                        cTagsCount[pTag.Name]++;
                    else
                        cTagsCount[pTag.Name] = 1;

                }
            }

            glacialList1.SortType = SortTypes.None;
            foreach (GLItem pItem in glacialList1.Items)
            {
                string sGenreTag = pItem.Tag as string;

                pItem.ForeColor = Color.Black;
                if (cTagsCount.ContainsKey(sGenreTag))
                {
                    pItem.Text = sGenreTag + " (" + cTagsCount[sGenreTag].ToString() + ")";
                    pItem.SubItems[3].Text = cTagsCount[sGenreTag].ToString();
                }
                else
                {
                    if (!pItem.SubItems[2].Checked)
                    {
                        pItem.Text = sGenreTag + " (0)";
                        pItem.SubItems[3].Text = "0";
                        pItem.ForeColor = Color.Gray;
                    }
                }
            }
            glacialList1.SortType = SortTypes.InsertionSort;
            glacialList1.SortColumn(3);
            glacialList1.SortColumn(3);

            glacialList1.Refresh();
        }

        private void newActionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> cShow = new List<string>();

            foreach (GLItem pItem in glacialList1.Items)
            {
                if (pItem.SubItems[1].Checked)
                    cShow.Add(pItem.Text);
            }

            ActionEditForm pForm = new ActionEditForm(cShow.ToArray());

            if (pForm.ShowDialog() == DialogResult.OK)
            {
                Universe.Instance.Actions.Add(pForm.Action);

                ShowTags();
            }
        }

        private void editActionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActionsListView.SelectedIndices.Count == 0)
                return;

            CinemaEngine.Action pSelected = ActionsListView.SelectedItems[0].Tag as CinemaEngine.Action;

            if (pSelected == null)
                return;

            ActionEditForm pForm = new ActionEditForm(pSelected);

            if (pForm.ShowDialog() == DialogResult.OK)
            {
                pSelected.Assign(pForm.Action);

                ShowTags();
            }
        }

        private void editTagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TagSelectForm pForm = new TagSelectForm();

            pForm.ShowDialog();
            ShowTags();
        }

        private void glacialList1_ItemChangedEvent(object source, ChangedEventArgs e)
        {
            GLItem pItem = e.Item;
            if (e.ChangedType == ChangedTypes.SubItemChanged &&
                (pItem.SubItems[1] == e.SubItem || pItem.SubItems[2] == e.SubItem))
            {
                if (pItem.SubItems[1].Checked && pItem.SubItems[1] == e.SubItem)
                {
                    pItem.BackColor = Color.Lime;
                    pItem.SubItems[2].Checked = false;
                }
                if (pItem.SubItems[2].Checked && pItem.SubItems[2] == e.SubItem)
                {
                    pItem.BackColor = Color.Red;
                    pItem.SubItems[1].Checked = false;
                }
                if (!pItem.SubItems[1].Checked && !pItem.SubItems[2].Checked)
                    pItem.BackColor = Color.White;
                    
                ShowActions();
            }
        }

        private void glacialList1_HoverEvent(object source, HoverEventArgs e)
        {
            if (e.HoverType == HoverTypes.HoverEnd)
                return;

            if (e.ColumnIndex == 1)
            {
                toolTip1.SetToolTip(glacialList1, "Show");
                return;
            }
            if (e.ColumnIndex == 2)
            {
                toolTip1.SetToolTip(glacialList1, "Hide");
                return;
            }
            toolTip1.SetToolTip(glacialList1, "");
        }
    }

    class ActionsComparer : IComparer
    {
        private GenreTag.Genre m_eGenre;
        public ActionsComparer()
        {
            m_eGenre = GenreTag.Genre.Action;
        }
        public ActionsComparer(GenreTag.Genre eGenre)
        {
            m_eGenre = eGenre;
        }
        public int Compare(object x, object y)
        {
            CinemaEngine.Action pAction1 = (CinemaEngine.Action)(((ListViewItem)x).Tag);
            CinemaEngine.Action pAction2 = (CinemaEngine.Action)(((ListViewItem)y).Tag);

            if (pAction1.GetRating(m_eGenre) < pAction2.GetRating(m_eGenre))
                return 1;

            if (pAction1.GetRating(m_eGenre) > pAction2.GetRating(m_eGenre))
                return -1;

            if (pAction1.GetFullRating() < pAction2.GetFullRating())
                return 1;

            if (pAction1.GetFullRating() > pAction2.GetFullRating())
                return -1;

            return 0;
        }
    }
}
