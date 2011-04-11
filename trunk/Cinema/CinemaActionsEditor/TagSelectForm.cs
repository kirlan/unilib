using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CinemaEngine;

namespace CinemaActionsEditor
{
    public partial class TagSelectForm : Form
    {
        public TagSelectForm(CinemaEngine.Action pAction)
        {
            InitializeComponent();

            m_bJustEditMode = false;

            foreach (GenreTag pTag in Universe.Instance.Tags.Values)
            {
                if (!pAction.Tags.Contains(pTag))
                    AddTag(pTag);
            }

            buttonOK.Enabled = m_bJustEditMode;
            buttonCancel.Enabled = !m_bJustEditMode;
        }

        public TagSelectForm()
        {
            InitializeComponent();

            m_bJustEditMode = true;

            foreach (GenreTag pTag in Universe.Instance.Tags.Values)
            {
                AddTag(pTag);
            }

            buttonOK.Enabled = m_bJustEditMode;
            buttonCancel.Enabled = !m_bJustEditMode;
        }

        private bool m_bJustEditMode = false;

        public GenreTag SelectedTag
        {
            get
            {
                if (listView1.SelectedIndices.Count == 0)
                    return null;
                else
                    return listView1.SelectedItems[0].Tag as GenreTag;
            }
        }

        private void AddTag(GenreTag pTag)
        {
            ListViewItem pNewItem = new ListViewItem(pTag.Name);
            pNewItem.SubItems.Add(pTag.Rating[GenreTag.Genre.Action].ToString());
            pNewItem.SubItems.Add(pTag.Rating[GenreTag.Genre.Gore].ToString());
            pNewItem.SubItems.Add(pTag.Rating[GenreTag.Genre.Fun].ToString());
            pNewItem.SubItems.Add(pTag.Rating[GenreTag.Genre.Mistery].ToString());
            pNewItem.SubItems.Add(pTag.Rating[GenreTag.Genre.Romance].ToString());
            pNewItem.SubItems.Add(pTag.Rating[GenreTag.Genre.Horror].ToString());
            pNewItem.SubItems.Add(pTag.Rating[GenreTag.Genre.XXX].ToString());

            pNewItem.Tag = pTag;

            listView1.Items.Add(pNewItem);
        }

        private void SetTag(ListViewItem pItem, GenreTag pTag)
        {
            pItem.Text = pTag.Name;
            pItem.SubItems[1].Text = pTag.Rating[GenreTag.Genre.Action].ToString();
            pItem.SubItems[2].Text = pTag.Rating[GenreTag.Genre.Gore].ToString();
            pItem.SubItems[3].Text = pTag.Rating[GenreTag.Genre.Fun].ToString();
            pItem.SubItems[4].Text = pTag.Rating[GenreTag.Genre.Mistery].ToString();
            pItem.SubItems[5].Text = pTag.Rating[GenreTag.Genre.Romance].ToString();
            pItem.SubItems[6].Text = pTag.Rating[GenreTag.Genre.Horror].ToString();
            pItem.SubItems[7].Text = pTag.Rating[GenreTag.Genre.XXX].ToString();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonOK.Enabled = m_bJustEditMode || listView1.SelectedIndices.Count != 0;
            buttonCancel.Enabled = !m_bJustEditMode;
        }

        private void addTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TagEditForm pForm = new TagEditForm(null);

            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
                !Universe.Instance.Tags.ContainsKey(pForm.GenreTag.Name))
            {
                Universe.Instance.AddTag(pForm.GenreTag);
                AddTag(pForm.GenreTag);
            }
        }

        private void editTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
                return;

            GenreTag pSelected = listView1.SelectedItems[0].Tag as GenreTag;

            if (pSelected == null)
                return;

            TagEditForm pForm = new TagEditForm(pSelected);

            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
                (!Universe.Instance.Tags.ContainsKey(pForm.GenreTag.Name) ||
                 pSelected.Name == pForm.GenreTag.Name))
            {
                SetTag(listView1.SelectedItems[0], pForm.GenreTag);
            }
        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            if (m_bJustEditMode)
                editTagToolStripMenuItem_Click(sender, e);
            else
                DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
