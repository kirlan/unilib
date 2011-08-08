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
using SS30Conf.Actions.Commands;
using SS30Conf.Actions.Conditions;

namespace SS30Editor
{
    public partial class ReactionEditForm : Form
    {
        private CReaction m_pReaction;

        public ReactionEditForm(CReaction pReaction)
        {
            InitializeComponent();

            m_pReaction = pReaction;

            textBox1.Text = pReaction.Value;
            textBox3.Text = pReaction.Description;

            if (pReaction.Continue)
                ContinueOn.Checked = true;
            else
                ContinueOff.Checked = true;

            numericUpDown1.Value = pReaction.Priority;

            ConditionsListView.Items.Clear();
            foreach (CCondition condition in pReaction.Conditions)
            {
                AddCondition(condition);
            }

            CommandsListView.Items.Clear();
            foreach (CCommand command in pReaction.Commands)
            {
                AddCommand(command);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_pReaction.Value = textBox1.Text;
            m_pReaction.Description = textBox3.Text;

            m_pReaction.Continue = ContinueOn.Checked;

            m_pReaction.Priority = (int)numericUpDown1.Value;

            DialogResult = DialogResult.OK;
        }

        private void ConditionRandomNewMenuItem_Click(object sender, EventArgs e)
        {
            CConditionRnd pConditionRnd = new CConditionRnd(m_pReaction);
            m_pReaction.Conditions.Add(pConditionRnd);

            ListViewItem pItem = AddCondition(pConditionRnd);
            ConditionsListView.FocusedItem = pItem;
            EditCondition(pItem);
        }

        private void EditCondition(ListViewItem item)
        {
            CCondition pSelectedCondition = item.Tag as CCondition;
            Form form = null;
            
            if (pSelectedCondition is CConditionRnd)
                form = new ConditionRndEditForm(pSelectedCondition as CConditionRnd);

            if (pSelectedCondition is CConditionSkillLevel)
                form = new ConditionSkillEditForm(pSelectedCondition as CConditionSkillLevel);

            if (pSelectedCondition is CConditionStatLevel)
                form = new ConditionStatEditForm(pSelectedCondition as CConditionStatLevel);

            if (pSelectedCondition is CConditionItem)
                form = new ConditionItemEditForm(pSelectedCondition as CConditionItem);

            if (pSelectedCondition is CConditionPersonalAppeal)
                form = new ConditionPersonalAppealEditForm(pSelectedCondition as CConditionPersonalAppeal);

            if (pSelectedCondition is CConditionSkillComparsion)
                form = new ConditionSkillComparsionEditForm(pSelectedCondition as CConditionSkillComparsion);

            if (form != null && form.ShowDialog() == DialogResult.OK)
            {
                item.Text = pSelectedCondition.Value;
                item.SubItems[1].Text = pSelectedCondition.ToString();
                UpdateStringColor(item);
            }

        }

        private ListViewItem AddCondition(CCondition pCondition)
        {
            ListViewItem pItem = new ListViewItem(pCondition.Value);
            pItem.SubItems.Add(pCondition.ToString());
            pItem.Tag = pCondition;

            ConditionsListView.Items.Add(pItem);
            UpdateStringColor(pItem);

            return pItem;
        }

        private void UpdateStringColor(ListViewItem pItem)
        {
            CConfigObject pString = pItem.Tag as CConfigObject;

            switch (pString.State)
            {
                case ModifyState.Unmodified:
                    pItem.ForeColor = Color.Black;
                    pItem.Font = new Font(pItem.Font, FontStyle.Regular);
                    break;
                case ModifyState.Added:
                    pItem.ForeColor = Color.DarkGreen;
                    pItem.Font = new Font(pItem.Font, FontStyle.Regular);
                    break;
                case ModifyState.Modified:
                    pItem.ForeColor = Color.Blue;
                    pItem.Font = new Font(pItem.Font, FontStyle.Italic);
                    break;
                case ModifyState.Erased:
                    {
                        pItem.ForeColor = Color.DarkRed;
                        pItem.Font = new Font(pItem.Font, FontStyle.Strikeout);
                    }
                    break;
            }
        }

        private void ConditionSkillLevelNewMenuItem_Click(object sender, EventArgs e)
        {
            CConditionSkillLevel pConditionSkill = new CConditionSkillLevel(m_pReaction);
            m_pReaction.Conditions.Add(pConditionSkill);

            ListViewItem pItem = AddCondition(pConditionSkill);
            ConditionsListView.FocusedItem = pItem;
            EditCondition(pItem);
        }

        private void ConditionItemNewMenuItem_Click(object sender, EventArgs e)
        {
            CConditionItem pConditionItem = new CConditionItem(m_pReaction);
            m_pReaction.Conditions.Add(pConditionItem);

            ListViewItem pItem = AddCondition(pConditionItem);
            ConditionsListView.FocusedItem = pItem;
            EditCondition(pItem);
        }

        private void ConditionPersonalAppealNewMenuItem_Click(object sender, EventArgs e)
        {
            CConditionPersonalAppeal pConditionPersonalAppeal = new CConditionPersonalAppeal(m_pReaction);
            m_pReaction.Conditions.Add(pConditionPersonalAppeal);

            ListViewItem pItem = AddCondition(pConditionPersonalAppeal);
            ConditionsListView.FocusedItem = pItem;
            EditCondition(pItem);
        }

        private void ConditionSkillComparsionNewMenuItem_Click(object sender, EventArgs e)
        {
            CConditionSkillComparsion pConditionSkillComparsion = new CConditionSkillComparsion(m_pReaction);
            m_pReaction.Conditions.Add(pConditionSkillComparsion);

            ListViewItem pItem = AddCondition(pConditionSkillComparsion);
            ConditionsListView.FocusedItem = pItem;
            EditCondition(pItem);
        }

        private void ConditionEditMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in ConditionsListView.SelectedItems)
            {
                EditCondition(item);
            }
        }

        private void deleteConditionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in ConditionsListView.SelectedItems)
            {
                CCondition pSelectedCondition = item.Tag as CCondition;

                if (pSelectedCondition.Delete())
                {
                    m_pReaction.Conditions.Remove(pSelectedCondition);
                    ConditionsListView.Items.Remove(item);
                }
                else
                    UpdateStringColor(item);
            }
        }

        private void CommandBuildRoomNewMenuItem_Click(object sender, EventArgs e)
        {
            CCommandBuildRoom pCommandBuildRoom = new CCommandBuildRoom(m_pReaction);
            m_pReaction.Commands.Add(pCommandBuildRoom);

            ListViewItem pItem = AddCommand(pCommandBuildRoom);
            CommandsListView.FocusedItem = pItem;
            EditCommand(pItem);
        }

        private ListViewItem AddCommand(CCommand pCommand)
        {
            ListViewItem pItem = new ListViewItem(pCommand.Value);
            pItem.SubItems.Add(pCommand.ToString());
            pItem.Tag = pCommand;

            CommandsListView.Items.Add(pItem);
            UpdateStringColor(pItem);

            return pItem;
        }

        private void EditCommand(ListViewItem item)
        {
            CCommand pSelectedCommand = item.Tag as CCommand;
            Form form = null;

            if (pSelectedCommand is CCommandBuildRoom)
                form = new CommandBuildRoomEditForm(pSelectedCommand as CCommandBuildRoom);

            if (pSelectedCommand is CCommandChangeSkill)
                form = new CommandChangeSkillEditForm(pSelectedCommand as CCommandChangeSkill);

            if (pSelectedCommand is CCommandChangeStat)
                form = new CommandChangeStatEditForm(pSelectedCommand as CCommandChangeStat);

            if (pSelectedCommand is CCommandItem)
                form = new CommandItemEditForm(pSelectedCommand as CCommandItem);

            if (pSelectedCommand is CCommandOpenShop)
                form = new CommandOpenShopEditForm(pSelectedCommand as CCommandOpenShop);

            if (pSelectedCommand is CCommandStatus)
                form = new CommandStatusEditForm(pSelectedCommand as CCommandStatus);

            if (pSelectedCommand is CCommandRest)
            {
                item.Text = pSelectedCommand.Value;
                item.SubItems[1].Text = pSelectedCommand.ToString();
                UpdateStringColor(item);
                return;
            }

            if (form != null && form.ShowDialog() == DialogResult.OK)
            {
                item.Text = pSelectedCommand.Value;
                item.SubItems[1].Text = pSelectedCommand.ToString();
                UpdateStringColor(item);
            }
        }

        private void CommandChangeSkillLevelMenuItem_Click(object sender, EventArgs e)
        {
            CCommandChangeSkill pCommandChangeSkill = new CCommandChangeSkill(m_pReaction);
            m_pReaction.Commands.Add(pCommandChangeSkill);

            ListViewItem pItem = AddCommand(pCommandChangeSkill);
            CommandsListView.FocusedItem = pItem;
            EditCommand(pItem);
        }

        private void CommandChangeStatLevelMenuItem_Click(object sender, EventArgs e)
        {
            CCommandChangeStat pCommandChangeStat = new CCommandChangeStat(m_pReaction);
            m_pReaction.Commands.Add(pCommandChangeStat);

            ListViewItem pItem = AddCommand(pCommandChangeStat);
            CommandsListView.FocusedItem = pItem;
            EditCommand(pItem);
        }

        private void CommandChangeItemsCountMenuItem_Click(object sender, EventArgs e)
        {
            CCommandItem pCommandItem = new CCommandItem(m_pReaction);
            m_pReaction.Commands.Add(pCommandItem);

            ListViewItem pItem = AddCommand(pCommandItem);
            CommandsListView.FocusedItem = pItem;
            EditCommand(pItem);
        }

        private void CommandOpenShopMenuItem_Click(object sender, EventArgs e)
        {
            CCommandOpenShop pCommandOpenShop = new CCommandOpenShop(m_pReaction);
            m_pReaction.Commands.Add(pCommandOpenShop);

            ListViewItem pItem = AddCommand(pCommandOpenShop);
            CommandsListView.FocusedItem = pItem;
            EditCommand(pItem);
        }

        private void CommandChangeStatusMenuItem_Click(object sender, EventArgs e)
        {
            CCommandStatus pCommandStatus = new CCommandStatus(m_pReaction);
            m_pReaction.Commands.Add(pCommandStatus);

            ListViewItem pItem = AddCommand(pCommandStatus);
            CommandsListView.FocusedItem = pItem;
            EditCommand(pItem);
        }

        private void CommandEditMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in CommandsListView.SelectedItems)
            {
                EditCommand(item);
            }
        }

        private void CommandDeleteMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in CommandsListView.SelectedItems)
            {
                CCommand pSelectedCommand = item.Tag as CCommand;

                if (pSelectedCommand.Delete())
                {
                    m_pReaction.Commands.Remove(pSelectedCommand);
                    CommandsListView.Items.Remove(item);
                }
                else
                    UpdateStringColor(item);
            }
        }

        private void ConditionStatLevelCheckMenuItem_Click(object sender, EventArgs e)
        {
            CConditionStatLevel pConditionStat = new CConditionStatLevel(m_pReaction);
            m_pReaction.Conditions.Add(pConditionStat);

            ListViewItem pItem = AddCondition(pConditionStat);
            ConditionsListView.FocusedItem = pItem;
            EditCondition(pItem);
        }

        private void restRelaxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CCommandRest pCommandRest = new CCommandRest(m_pReaction);
            m_pReaction.Commands.Add(pCommandRest);

            ListViewItem pItem = AddCommand(pCommandRest);
            CommandsListView.FocusedItem = pItem;
            EditCommand(pItem);
        }
    }
}
