using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CinemaEngine;
using GlacialComponents.Controls;
using CinemaEngine.RoleConditions;

namespace CinemaActionsEditor
{
    public partial class ActionEditForm : Form
    {
        private CinemaEngine.Action m_pAction;

        public CinemaEngine.Action Action
        {
            get { return m_pAction; }
        }

        public ActionEditForm(string[] cTags):
            this(new CinemaEngine.Action(cTags))
        { }

        public ActionEditForm(CinemaEngine.Action pAction)
        {
            if(pAction == null)
                throw new NoNullAllowedException();

            m_pAction = new CinemaEngine.Action(pAction);

            InitializeComponent();

            ComboBox pComboBox0 = new ComboBox();

            pComboBox0.Items.Add("actor1");
            pComboBox0.Items.Add("actor2");
            pComboBox0.Items.Add("actor3");
            pComboBox0.Items.Add("actor4");
            pComboBox0.DropDownStyle = ComboBoxStyle.DropDownList;

            ComboBox pComboBox1 = new ComboBox();

            pComboBox1.Items.Add("link1");
            pComboBox1.Items.Add("link2");
            pComboBox1.Items.Add("link3");
            pComboBox1.Items.Add("link4");
            pComboBox1.DropDownStyle = ComboBoxStyle.DropDown;

            GLItem pNewItem = AllowedBindingsList.Items.Add("actor1");
            pNewItem.SubItems[1].Text = "link1";
            pNewItem.SubItems[0].Control = pComboBox0;
            pNewItem.SubItems[1].Control = pComboBox1;

            ActionName.Text = m_pAction.Name;
            ActionDescription.Text = m_pAction.Description;

            foreach (GenreTag pSubGenre in m_pAction.Tags)
                TagsList.Items.Add(pSubGenre.Name);

            RecalcGenres();


            foreach (CharacterState.Pose eType in Enum.GetValues(typeof(CharacterState.Pose)))
            {
                AnchorBefore.Items.Add(CharacterState.GetPoseString(eType, false));
                AnchorAfter.Items.Add(CharacterState.GetPoseString(eType, true));
            }

            foreach (Actor.Gender eGender in Enum.GetValues(typeof(Actor.Gender)))
                ActorsGender.Items.Add(Actor.GetGenderString(eGender));

            foreach (Role pRole in m_pAction.Roles)
            {
                RolesList.Items.Add(pRole.Name);
            }

            //foreach (Role pRole in m_pAction.Roles)
            //{
            //    ListViewItem pNewItem = new ListViewItem(pRole.Name);
            //    string sStatusString = "";
            //    foreach (string sAllowed in pRole.Status.AnyOf)
            //    {
            //        if (sStatusString == "")
            //            sStatusString = sAllowed;
            //        else
            //            sStatusString += ", " + sAllowed;
            //    }
            //    bool bFirst = true;
            //    if (pRole.Status.Except.Count() > 0)
            //    {
            //        if (sStatusString != "")
            //            sStatusString += ", but ";

            //        foreach(string sForbidden in pRole.Status.Except)
            //        {
            //            if (bFirst)
            //                sStatusString += "not " + sForbidden;
            //            else
            //                sStatusString += ", " + sForbidden;
            //        }
            //    }
            //    pNewItem.SubItems.Add(sStatusString);
            //    pNewItem.SubItems.Add("-");

            //    pNewItem.Tag = pRole;

            //    listView1.Items.Add(pNewItem);
            //}
        }

        private void RecalcGenres()
        {
            progressBarAction.Value = 0;
            progressBarAction.Maximum = GenreTag.MaxRating;
            progressBarAction.Value = m_pAction.GetRating(GenreTag.Genre.Action);
            labelAction.Text = "(" + progressBarAction.Value.ToString() + ")";

            progressBarGore.Value = 0;
            progressBarGore.Maximum = GenreTag.MaxRating;
            progressBarGore.Value = m_pAction.GetRating(GenreTag.Genre.Gore);
            labelGore.Text = "(" + progressBarGore.Value.ToString() + ")";

            progressBarFun.Value = 0;
            progressBarFun.Maximum = GenreTag.MaxRating;
            progressBarFun.Value = m_pAction.GetRating(GenreTag.Genre.Fun);
            labelFun.Text = "(" + progressBarFun.Value.ToString() + ")";

            progressBarMistery.Value = 0;
            progressBarMistery.Maximum = GenreTag.MaxRating;
            progressBarMistery.Value = m_pAction.GetRating(GenreTag.Genre.Mistery);
            labelMistery.Text = "(" + progressBarMistery.Value.ToString() + ")";

            progressBarRomance.Value = 0;
            progressBarRomance.Maximum = GenreTag.MaxRating;
            progressBarRomance.Value = m_pAction.GetRating(GenreTag.Genre.Romance);
            labelRomance.Text = "(" + progressBarRomance.Value.ToString() + ")";

            progressBarHorror.Value = 0;
            progressBarHorror.Maximum = GenreTag.MaxRating;
            progressBarHorror.Value = m_pAction.GetRating(GenreTag.Genre.Horror);
            labelHorror.Text = "(" + progressBarHorror.Value.ToString() + ")";

            progressBarXXX.Value = 0;
            progressBarXXX.Maximum = GenreTag.MaxRating;
            progressBarXXX.Value = m_pAction.GetRating(GenreTag.Genre.XXX);
            labelXXX.Text = "(" + progressBarXXX.Value.ToString() + ")";
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
        }

        private void buttonAddSubGenre_Click(object sender, EventArgs e)
        {
            TagSelectForm pForm = new TagSelectForm(m_pAction);

            if (pForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_pAction.Tags.Add(pForm.SelectedTag);
                TagsList.Items.Add(pForm.SelectedTag.Name);
            }
            RecalcGenres();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonRemoveSubGenre.Enabled = TagsList.SelectedIndex != -1;
        }

        private void buttonRemoveSubGenre_Click(object sender, EventArgs e)
        {
            if (TagsList.SelectedIndex == -1)
                return;

            GenreTag pTag = Universe.Instance.Tags[TagsList.SelectedItem as string];

            m_pAction.Tags.Remove(pTag);
            TagsList.Items.Remove(TagsList.SelectedItem);

            RecalcGenres();
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            m_pAction.Name = ActionName.Text;
        }

        private void textBoxDescription_TextChanged(object sender, EventArgs e)
        {
            m_pAction.Description = ActionDescription.Text;
        }

        private Role m_pSelectedRole = null;

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RolesList.SelectedIndex == -1)
                return;

            foreach(Role pRole in m_pAction.Roles)
            {
                if(pRole.Name == RolesList.SelectedItem as string)
                {
                    m_pSelectedRole = pRole;
                    break;
                }
            }

            ShowRoleInfo();
        }

        private void ShowRoleInfo()
        {
            if (m_pSelectedRole == null)
                return;

            RoleAlias.Text = m_pSelectedRole.Name;
            AnchorBefore.Text = CharacterState.GetPoseString((m_pSelectedRole.Conditions[RoleCondition.ConditionType.Pose] as PoseCondition).Pose, false);
            AnchorAfter.Text = CharacterState.GetPoseString((m_pSelectedRole.Consequences[RoleCondition.ConditionType.Pose] as PoseCondition).Pose, true);

            int iCounter = 1;
            foreach (Actor.Gender eGender in Enum.GetValues(typeof(Actor.Gender)))
                ActorsGender.CheckBoxItems[iCounter++].Checked = (m_pSelectedRole.Conditions[RoleCondition.ConditionType.Gender] as GenderCondition).Genders.Contains(eGender);
        }
    }
}
