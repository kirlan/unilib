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

namespace SS30Editor
{
    public partial class ActionEditForm : Form
    {
        private CAction m_pAction;

        public ActionEditForm(CAction pAction)
        {
            InitializeComponent();

            m_pAction = pAction;

            textBox1.Text = pAction.Value;
            textBox2.Text = pAction.Name;
            textBox3.Text = pAction.Description;

            switch (pAction.ActorFilter)
            {
                case PersonFilter.ANY_PERSON:
                    actor_any.Checked = true;
                    break;
                case PersonFilter.ANY_MAID:
                    actor_maid.Checked = true;
                    break;
                case PersonFilter.ANY_GUEST:
                    actor_guest.Checked = true;
                    break;
                case PersonFilter.HERO:
                    actor_hero.Checked = true;
                    break;
                case PersonFilter.SPECIFIED_PERSON:
                    actor_spec.Checked = true;
                    break;
            }
            switch (pAction.Priority)
            {
                case PriorityFilter.TARGET:
                    priority_target.Checked = true;
                    break;
                case PriorityFilter.ROOM:
                    priority_room.Checked = true;
                    break;
                case PriorityFilter.BACKGROUND:
                    priority_none.Checked = true;
                    break;
            }
            switch (pAction.TargetFilter)
            {
                case PersonFilter.ANY_PERSON:
                    target_any.Checked = true;
                    break;
                case PersonFilter.ANY_MAID:
                    target_maid.Checked = true;
                    break;
                case PersonFilter.ANY_GUEST:
                    target_guest.Checked = true;
                    break;
                case PersonFilter.HERO:
                    target_hero.Checked = true;
                    break;
                case PersonFilter.SOLO:
                    target_none.Checked = true;
                    break;
                case PersonFilter.SPECIFIED_PERSON:
                    target_spec.Checked = true;
                    break;
            }

            comboBox3.Items.Clear();
            foreach (CRoom room in MainForm.m_pWorld.Rooms)
            {
                comboBox3.Items.Add(room.Value);
            }

            switch (pAction.RoomFilter)
            {
                case RoomFilter.ANY_ROOM:
                    room_any.Checked = true;
                    break;
                case RoomFilter.ANY_SERVICE_ROOM:
                    room_service.Checked = true;
                    break;
                case RoomFilter.ANY_GUEST_ROOM:
                    room_guest.Checked = true;
                    break;
                case RoomFilter.ANY_LEISURE_ROOM:
                    room_leisure.Checked = true;
                    break;
                case RoomFilter.SPECIFIED_ROOM:
                    room_spec.Checked = true;
                    comboBox3.SelectedItem = pAction.Room.Value;
                    break;
            }
            switch (pAction.DayTimeFilter)
            {
                case DayTimeFilter.ANY_TIME:
                    time_any.Checked = true;
                    break;
                case DayTimeFilter.MORNING_DAY:
                    time_morday.Checked = true;
                    break;
                case DayTimeFilter.MORNING:
                    time_mor.Checked = true;
                    break;
                case DayTimeFilter.DAY:
                    time_day.Checked = true;
                    break;
                case DayTimeFilter.EVENING:
                    time_even.Checked = true;
                    break;
            }

            switch (pAction.ActionType)
            {
                case ActionType.WORK:
                    typeWork.Enabled = true;
                    break;
                case ActionType.PUNISHMENT:
                    typePunish.Enabled = true;
                    break;
                case ActionType.SEX:
                    typeSex.Enabled = true;
                    break;
                case ActionType.REST:
                    typeRelax.Enabled = true;
                    break;
            }

            SelectedTargetStatusListBox.Items.Clear();
            foreach (string status in pAction.TargetStatusList.Keys)
            {
                int index = SelectedTargetStatusListBox.Items.Add(status);
                SelectedTargetStatusListBox.SetItemChecked(index, pAction.TargetStatusList[status]);
            }
            SelectedActorStatusListBox.Items.Clear();
            foreach (string status in pAction.ActorStatusList.Keys)
            {
                int index = SelectedActorStatusListBox.Items.Add(status);
                SelectedActorStatusListBox.SetItemChecked(index, pAction.ActorStatusList[status]);
            }

            AvailableTargetStatusListBox.Items.Clear();
            AvailableActorStatusListBox.Items.Clear();
            foreach (string status in CConfigRepository.Instance.Statuses)
            {
                if (!SelectedTargetStatusListBox.Items.Contains(status))
                    AvailableTargetStatusListBox.Items.Add(status);
                if (!SelectedActorStatusListBox.Items.Contains(status))
                    AvailableActorStatusListBox.Items.Add(status);
            }

            numericUpDown1.Value = pAction.Duration;

            if (pAction.Hidden)
                hiddenYes.Checked = true;
            else
                hiddenNo.Checked = true;

            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_pAction.Value = textBox1.Text;
            m_pAction.Name = textBox2.Text;
            m_pAction.Description = textBox3.Text;

            if (actor_any.Checked)
                m_pAction.ActorFilter = PersonFilter.ANY_PERSON;
            if (actor_maid.Checked)
                m_pAction.ActorFilter = PersonFilter.ANY_MAID;
            if (actor_guest.Checked)
                m_pAction.ActorFilter = PersonFilter.ANY_GUEST;
            if (actor_hero.Checked)
                m_pAction.ActorFilter = PersonFilter.HERO;
            if (actor_spec.Checked)
                m_pAction.ActorFilter = PersonFilter.SPECIFIED_PERSON;

            if (priority_target.Checked)
                m_pAction.Priority = PriorityFilter.TARGET;
            if (priority_room.Checked)
                m_pAction.Priority = PriorityFilter.ROOM;
            if (priority_none.Checked)
                m_pAction.Priority = PriorityFilter.BACKGROUND;

            if (target_any.Checked)
                m_pAction.TargetFilter = PersonFilter.ANY_PERSON;
            if (target_maid.Checked)
                m_pAction.TargetFilter = PersonFilter.ANY_MAID;
            if (target_guest.Checked)
                m_pAction.TargetFilter = PersonFilter.ANY_GUEST;
            if (target_hero.Checked)
                m_pAction.TargetFilter = PersonFilter.HERO;
            if (target_none.Checked)
                m_pAction.TargetFilter = PersonFilter.SOLO;
            if (target_spec.Checked)
                m_pAction.TargetFilter = PersonFilter.SPECIFIED_PERSON;

            if (room_any.Checked)
                m_pAction.RoomFilter = RoomFilter.ANY_ROOM;
            if (room_service.Checked)
                m_pAction.RoomFilter = RoomFilter.ANY_SERVICE_ROOM;
            if (room_guest.Checked)
                m_pAction.RoomFilter = RoomFilter.ANY_GUEST_ROOM;
            if (room_leisure.Checked)
                m_pAction.RoomFilter = RoomFilter.ANY_LEISURE_ROOM;
            if (room_spec.Checked)
            {
                m_pAction.RoomFilter = RoomFilter.SPECIFIED_ROOM;
                foreach (CRoom room in MainForm.m_pWorld.Rooms)
                {
                    if (comboBox3.SelectedItem as string == room.Value)
                    {
                        m_pAction.Room = room;
                        break;
                    }
                }
            }

            if (time_any.Checked)
                m_pAction.DayTimeFilter = DayTimeFilter.ANY_TIME;
            if (time_mor.Checked)
                m_pAction.DayTimeFilter = DayTimeFilter.MORNING;
            if (time_day.Checked)
                m_pAction.DayTimeFilter = DayTimeFilter.DAY;
            if (time_morday.Checked)
                m_pAction.DayTimeFilter = DayTimeFilter.MORNING_DAY;
            if (time_even.Checked)
                m_pAction.DayTimeFilter = DayTimeFilter.EVENING;

            if (typeWork.Checked)
                m_pAction.ActionType = ActionType.WORK;
            if (typePunish.Checked)
                m_pAction.ActionType = ActionType.PUNISHMENT;
            if (typeSex.Checked)
                m_pAction.ActionType = ActionType.SEX;
            if (typeRelax.Checked)
                m_pAction.ActionType = ActionType.REST;

            m_pAction.Duration = (int)numericUpDown1.Value;

            Dictionary<string, bool> newList = new Dictionary<string, bool>();
            for (int i = 0; i < SelectedTargetStatusListBox.Items.Count; i++)
            {
                string status = SelectedTargetStatusListBox.Items[i] as string;
                status = status.Replace(' ', '_');
                newList[status] = SelectedTargetStatusListBox.GetItemChecked(i);
            }
            m_pAction.UpdateTargetStatusList(newList);
            
            newList.Clear();
            for (int i = 0; i < SelectedActorStatusListBox.Items.Count; i++)
            {
                string status = SelectedActorStatusListBox.Items[i] as string;
                status = status.Replace(' ', '_');
                newList[status] = SelectedActorStatusListBox.GetItemChecked(i);
            }
            m_pAction.UpdateActorStatusList(newList);

            m_pAction.Hidden = hiddenYes.Checked;

            DialogResult = DialogResult.OK;
        }

        private void room_spec_CheckedChanged(object sender, EventArgs e)
        {
            comboBox3.Enabled = room_spec.Checked;
        }

        private void AddStatusButton_Click(object sender, EventArgs e)
        {
            if (AvailableTargetStatusListBox.SelectedIndex == -1)
                return;

            string status = AvailableTargetStatusListBox.SelectedItem as string;
            if (SelectedTargetStatusListBox.Items.Contains(status))
                return;

            int index = SelectedTargetStatusListBox.Items.Add(status);
            SelectedTargetStatusListBox.SetItemChecked(index, true);
            SelectedTargetStatusListBox.SelectedIndex = index;

            AvailableTargetStatusListBox.Items.Remove(status);
        }

        private void RemoveStatusButton_Click(object sender, EventArgs e)
        {
            if (SelectedTargetStatusListBox.SelectedIndex == -1)
                return;

            string status = SelectedTargetStatusListBox.SelectedItem as string;
            SelectedTargetStatusListBox.Items.Remove(status);
            AvailableTargetStatusListBox.Items.Add(status);
        }

        private void AddActorStatusButton_Click(object sender, EventArgs e)
        {
            if (AvailableActorStatusListBox.SelectedIndex == -1)
                return;

            string status = AvailableActorStatusListBox.SelectedItem as string;
            if (SelectedActorStatusListBox.Items.Contains(status))
                return;

            int index = SelectedActorStatusListBox.Items.Add(status);
            SelectedActorStatusListBox.SetItemChecked(index, true);
            SelectedActorStatusListBox.SelectedIndex = index;

            AvailableActorStatusListBox.Items.Remove(status);
        }

        private void RemoveActorStatusButton_Click(object sender, EventArgs e)
        {
            if (SelectedActorStatusListBox.SelectedIndex == -1)
                return;

            string status = SelectedActorStatusListBox.SelectedItem as string;
            SelectedActorStatusListBox.Items.Remove(status);
            AvailableActorStatusListBox.Items.Add(status);
        }

        private void target_none_CheckedChanged(object sender, EventArgs e)
        {
            SelectedTargetStatusListBox.Enabled = !target_none.Checked;
            AvailableTargetStatusListBox.Enabled = !target_none.Checked;
            AddTargetStatusButton.Enabled = !target_none.Checked;
            RemoveTargetStatusButton.Enabled = !target_none.Checked;
        }

        private void priority_none_CheckedChanged(object sender, EventArgs e)
        {
            if (priority_none.Checked)
            {
                numericUpDown1.Value = 0;
                numericUpDown1.Enabled = false;
                hiddenYes.Checked = true;
                hiddenYes.Enabled = false;
                hiddenNo.Enabled = false;
            }
            else
            {
                numericUpDown1.Value = m_pAction.Duration;
                numericUpDown1.Enabled = true;
                hiddenNo.Checked = !m_pAction.Hidden;
                hiddenYes.Enabled = true;
                hiddenNo.Enabled = true;
            }
        }
    }
}
