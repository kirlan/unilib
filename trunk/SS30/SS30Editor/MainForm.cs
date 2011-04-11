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
using SS30Conf.Items;
using SS30Conf.Actions.Conditions;
using SS30Conf.Actions.Commands;
using System.Collections;

namespace SS30Editor
{
    public partial class MainForm : Form
    {
        static internal CWorld m_pWorld = new CWorld();

        public MainForm()
        {
            InitializeComponent();
            ReactionsListView.ListViewItemSorter = new ListViewReactionComparer();

            UpdateAll();
        }

        private void MainNewMenuItem_Click(object sender, EventArgs e)
        {
            CConfigRepository.Instance.Strings.Clear();
            m_pWorld = new CWorld();
            UpdateAll();
        }

        private void UpdateAll()
        {
            StatsListView.Items.Clear();
            foreach (CPersonStat stat in m_pWorld.Stats.Values)
            {
                AddStat(stat);
            }

            SkillsListView.Items.Clear();
            foreach (CSkill skill in m_pWorld.Skills)
            {
                AddSkill(skill);
            }

            FetishesListView.Items.Clear();
            foreach (CFetish fetish in m_pWorld.Fetishes)
            {
                AddFetish(fetish);
            }

            LeisuresListView.Items.Clear();
            foreach (CLeisure leisure in m_pWorld.Leisures)
            {
                AddLeisure(leisure);
            }

            RoomsListView.Items.Clear();
            foreach (CRoom room in m_pWorld.Rooms)
            {
                AddRoom(room);
            }

            ItemsListView.Items.Clear();
            foreach (CItem item in m_pWorld.Items)
            {
                if (item is CHouseholdMachinery)
                    AddItem(item as CHouseholdMachinery);
                if (item is CGift)
                    AddItem(item as CGift);
                if (item is CUniform)
                    AddItem(item as CUniform);
                if (item is CSexToy)
                    AddItem(item as CSexToy);
            }

            ActionsListView.Items.Clear();
            foreach (CAction action in m_pWorld.Actions)
            {
                AddAction(action);
            }
        }

        private ListViewItem AddAction(CAction pAction)
        {
            ListViewItem pItem = new ListViewItem(pAction.Value);
            pItem.UseItemStyleForSubItems = false;
            string statuses = "";
            foreach (string status in pAction.ActorStatusList.Keys)
            {
                if (statuses.Length > 0)
                    statuses += ", ";
                statuses += status;
                if (!pAction.ActorStatusList[status])
                    statuses += "-";
                else
                    statuses += "+";
            }
            if (statuses.Length > 0)
                statuses = string.Format(" ({0})", statuses);
            switch (pAction.ActorFilter)
            {
                case PersonFilter.ANY_PERSON:
                    pItem.SubItems.Add(string.Format("anyone{0}", statuses));
                    break;
                case PersonFilter.ANY_MAID:
                    pItem.SubItems.Add(string.Format("any maid{0}", statuses));
                    break;
                case PersonFilter.ANY_GUEST:
                    pItem.SubItems.Add(string.Format("any guest{0}", statuses));
                    break;
                case PersonFilter.HERO:
                    pItem.SubItems.Add(string.Format("hero{0}", statuses));
                    break;
                case PersonFilter.SPECIFIED_PERSON:
                    pItem.SubItems.Add(string.Format("someone...{0}", statuses));
                    break;
            }
            statuses = "";
            foreach (string status in pAction.TargetStatusList.Keys)
            {
                if (statuses.Length > 0)
                    statuses += ", ";
                statuses += status;
                if (!pAction.TargetStatusList[status])
                    statuses += "-";
                else
                    statuses += "+";
            }
            if (statuses.Length > 0)
                statuses = string.Format(" ({0})", statuses);
            switch (pAction.TargetFilter)
            {
                case PersonFilter.ANY_PERSON:
                    pItem.SubItems.Add(string.Format("anyone{0}", statuses));
                    break;
                case PersonFilter.ANY_MAID:
                    pItem.SubItems.Add(string.Format("any maid{0}", statuses));
                    break;
                case PersonFilter.ANY_GUEST:
                    pItem.SubItems.Add(string.Format("any guest{0}", statuses));
                    break;
                case PersonFilter.HERO:
                    pItem.SubItems.Add(string.Format("hero{0}", statuses));
                    break;
                case PersonFilter.SOLO:
                    pItem.SubItems.Add(string.Format("none{0}", statuses));
                    break;
                case PersonFilter.SPECIFIED_PERSON:
                    pItem.SubItems.Add(string.Format("someone...{0}", statuses));
                    break;
            }
            switch (pAction.RoomFilter)
            {
                case RoomFilter.ANY_ROOM:
                    pItem.SubItems.Add("anywhere");
                    break;
                case RoomFilter.ANY_SERVICE_ROOM:
                    pItem.SubItems.Add("any service room");
                    break;
                case RoomFilter.ANY_GUEST_ROOM:
                    pItem.SubItems.Add("any guest's room");
                    break;
                case RoomFilter.ANY_LEISURE_ROOM:
                    pItem.SubItems.Add("any leisure room");
                    break;
                case RoomFilter.SPECIFIED_ROOM:
                    pItem.SubItems.Add(pAction.Room.Value);
                    break;
            }
            switch (pAction.DayTimeFilter)
            {
                case DayTimeFilter.ANY_TIME:
                    pItem.SubItems.Add("any time");
                    break;
                case DayTimeFilter.MORNING_DAY:
                    pItem.SubItems.Add("at morning or afternoon");
                    break;
                case DayTimeFilter.MORNING:
                    pItem.SubItems.Add("at morning");
                    break;
                case DayTimeFilter.DAY:
                    pItem.SubItems.Add("at afternoon");
                    break;
                case DayTimeFilter.EVENING:
                    pItem.SubItems.Add("at evening");
                    break;
            }
            pItem.SubItems.Add(pAction.Duration.ToString());
            pItem.SubItems.Add(pAction.Name);
            pItem.SubItems.Add(pAction.Description);
            pItem.Tag = pAction;

            ActionsListView.Items.Add(pItem);

            if (pAction.Priority == PriorityFilter.TARGET)
                pItem.SubItems[2].Font = new Font(pItem.SubItems[2].Font, FontStyle.Bold);
            else
                pItem.SubItems[2].Font = new Font(pItem.SubItems[2].Font, FontStyle.Regular);

            if (pAction.Priority == PriorityFilter.ROOM)
                pItem.SubItems[3].Font = new Font(pItem.SubItems[3].Font, FontStyle.Bold);
            else
                pItem.SubItems[3].Font = new Font(pItem.SubItems[3].Font, FontStyle.Regular);

            UpdateStringColor(pItem);

            return pItem;
        }

        private ListViewItem AddStat(CPersonStat pStat)
        {
            ListViewItem pItem = new ListViewItem(pStat.Value);
            pItem.SubItems.Add(pStat.Name);
            string sAvailability = "";
            if (pStat.Availability[StringSubCategory.PERSON_MAID])
                sAvailability += "maids";
            if (pStat.Availability[StringSubCategory.PERSON_GUEST])
            {
                if (sAvailability.Length > 0)
                    sAvailability += ", ";
                sAvailability += "guests";
            }
            if (pStat.Availability[StringSubCategory.PERSON_SPECIAL])
            {
                if (sAvailability.Length > 0)
                    sAvailability += ", ";
                sAvailability += "special persons";
            }
            pItem.SubItems.Add(sAvailability);
            pItem.SubItems.Add(pStat.Description);
            pItem.Tag = pStat;

            StatsListView.Items.Add(pItem);
            UpdateStringColor(pItem);

            return pItem;
        }

        private void SkillNewMenuItem_Click(object sender, EventArgs e)
        {
            CSkill pSkill = new CSkill();
            m_pWorld.Skills.Add(pSkill);

            ListViewItem pItem = AddSkill(pSkill);
            SkillsListView.FocusedItem = pItem;
            if (!EditSkill(pItem))
            {
                pSkill.Delete();
                m_pWorld.Skills.Remove(pSkill);
                SkillsListView.Items.Remove(pItem);
            }
        }

        private ListViewItem AddSkill(CSkill pSkill)
        {
            ListViewItem pItem = new ListViewItem(pSkill.Value);
            pItem.SubItems.Add(pSkill.Name);
            pItem.SubItems.Add(pSkill.Description);
            pItem.Tag = pSkill;

            SkillsListView.Items.Add(pItem);
            UpdateStringColor(pItem);

            return pItem;
        }

        private void SkillEditMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in SkillsListView.SelectedItems)
            {
                EditSkill(item);
            }
        }

        private bool EditSkill(ListViewItem item)
        {
            CSkill pSelectedSkill = item.Tag as CSkill;
            SkillEditForm form = new SkillEditForm(pSelectedSkill);
            if (form.ShowDialog() == DialogResult.OK)
            {
                item.Text = pSelectedSkill.Value;
                item.SubItems[1].Text = pSelectedSkill.Name;
                item.SubItems[2].Text = pSelectedSkill.Description;
                UpdateStringColor(item);
                return true;
            }
            return false;
        }

        private void SkillDeleteMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in SkillsListView.SelectedItems)
            {
                CSkill pSelectedSkill = item.Tag as CSkill;

                if (pSelectedSkill.Delete())
                {
                    m_pWorld.Skills.Remove(pSelectedSkill);
                    SkillsListView.Items.Remove(item);
                }
                else
                    UpdateStringColor(item);
            }
        }

        private void FetishNewMenuItem_Click(object sender, EventArgs e)
        {
            CFetish pFetish = new CFetish();
            m_pWorld.Fetishes.Add(pFetish);

            ListViewItem pItem = AddFetish(pFetish);
            FetishesListView.FocusedItem = pItem;
            if (!EditFetish(pItem))
            {
                pFetish.Delete();
                m_pWorld.Fetishes.Remove(pFetish);
                FetishesListView.Items.Remove(pItem);
            }
        }

        private ListViewItem AddFetish(CFetish pFetish)
        {
            ListViewItem pItem = new ListViewItem(pFetish.Value);
            pItem.SubItems.Add(pFetish.Name);
            pItem.SubItems.Add(pFetish.Description);
            pItem.Tag = pFetish;

            FetishesListView.Items.Add(pItem);
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
                    pItem.ForeColor = Color.DarkRed;
                    pItem.Font = new Font(pItem.Font, FontStyle.Strikeout);
                    break;
            }
        }

        private bool EditFetish(ListViewItem item)
        {
            CFetish pSelectedFetish = item.Tag as CFetish;
            FetishEditForm form = new FetishEditForm(pSelectedFetish);
            if (form.ShowDialog() == DialogResult.OK)
            {
                item.Text = pSelectedFetish.Value;
                item.SubItems[1].Text = pSelectedFetish.Name;
                item.SubItems[2].Text = pSelectedFetish.Description;
                UpdateStringColor(item);
                return true;
            }
            return false;
        }

        private void FetishEditMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in FetishesListView.SelectedItems)
            {
                EditFetish(item);
            }
        }

        private void FetishDeleteMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in FetishesListView.SelectedItems)
            {
                CFetish pSelectedFetish = item.Tag as CFetish;

                if (pSelectedFetish.Delete())
                {
                    m_pWorld.Fetishes.Remove(pSelectedFetish);
                    FetishesListView.Items.Remove(item);
                }
                else
                    UpdateStringColor(item);
            }
        }

        private void LeisureNewMenuItem_Click(object sender, EventArgs e)
        {
            CLeisure pLeisure = new CLeisure();
            m_pWorld.Leisures.Add(pLeisure);

            ListViewItem pItem = AddLeisure(pLeisure);
            LeisuresListView.FocusedItem = pItem;
            if (!EditLeisure(pItem))
            {
                pLeisure.Delete();
                m_pWorld.Leisures.Remove(pLeisure);
                LeisuresListView.Items.Remove(pItem);
            }
        }

        private ListViewItem AddLeisure(CLeisure pLeisure)
        {
            ListViewItem pItem = new ListViewItem(pLeisure.Value);
            pItem.SubItems.Add(pLeisure.Name);
            if (pLeisure.Action == null)
                pItem.SubItems.Add("none");
            else
                pItem.SubItems.Add(pLeisure.Action.Value);
            pItem.SubItems.Add(pLeisure.Description);
            pItem.Tag = pLeisure;

            LeisuresListView.Items.Add(pItem);
            UpdateStringColor(pItem);

            return pItem;
        }

        private bool EditLeisure(ListViewItem item)
        {
            CLeisure pSelectedLeisure = item.Tag as CLeisure;
            LeisureEditForm form = new LeisureEditForm(pSelectedLeisure);
            if (form.ShowDialog() == DialogResult.OK)
            {
                item.Text = pSelectedLeisure.Value;
                item.SubItems[1].Text = pSelectedLeisure.Name;
                if (pSelectedLeisure.Action == null)
                    item.SubItems[2].Text = "none";
                else
                    item.SubItems[2].Text = pSelectedLeisure.Action.Value;
                item.SubItems[3].Text = pSelectedLeisure.Description;
                UpdateStringColor(item);
                return true;
            }
            return false;
        }

        private void LeisureEditMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in LeisuresListView.SelectedItems)
            {
                EditLeisure(item);
            }
        }

        private void LeisureDeleteMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in LeisuresListView.SelectedItems)
            {
                CLeisure pSelectedLeisure = item.Tag as CLeisure;

                if (pSelectedLeisure.Delete())
                {
                    m_pWorld.Leisures.Remove(pSelectedLeisure);
                    LeisuresListView.Items.Remove(item);
                }
                else
                    UpdateStringColor(item);
            }
        }

        private void RoomNewMenuItem_Click(object sender, EventArgs e)
        {
            CRoom pRoom = new CRoom();
            m_pWorld.Rooms.Add(pRoom);

            ListViewItem pItem = AddRoom(pRoom);
            RoomsListView.FocusedItem = pItem;
            if (!EditRoom(pItem))
            {
                pRoom.Delete();
                m_pWorld.Rooms.Remove(pRoom);
                RoomsListView.Items.Remove(pItem);
            }
        }

        private ListViewItem AddRoom(CRoom pRoom)
        {
            ListViewItem pItem = new ListViewItem(pRoom.Value);
            switch(pRoom.RoomType)
            {
                case RoomType.GUEST:
                    pItem.SubItems.Add("guestroom");
                    break;
                case RoomType.LEISURE:
                    pItem.SubItems.Add("leisure room");
                    break;
                case RoomType.SERVICE:
                    pItem.SubItems.Add("service room");
                    break;
            }
            pItem.SubItems.Add(pRoom.Name);
            pItem.SubItems.Add(pRoom.CostToBuild.ToString());
            pItem.SubItems.Add(pRoom.DaysToBuild.ToString());
            pItem.SubItems.Add(pRoom.Description);
            pItem.Tag = pRoom;

            RoomsListView.Items.Add(pItem);
            UpdateStringColor(pItem);

            return pItem;
        }

        private bool EditRoom(ListViewItem item)
        {
            CRoom pSelectedRoom = item.Tag as CRoom;
            RoomEditForm form = new RoomEditForm(pSelectedRoom);
            if (form.ShowDialog() == DialogResult.OK)
            {
                item.Text = pSelectedRoom.Value;
                switch (pSelectedRoom.RoomType)
                {
                    case RoomType.GUEST:
                        item.SubItems[1].Text = "guestroom";
                        break;
                    case RoomType.LEISURE:
                        item.SubItems[1].Text = "leisure room";
                        break;
                    case RoomType.SERVICE:
                        item.SubItems[1].Text = "service room";
                        break;
                }
                item.SubItems[2].Text = pSelectedRoom.Name;
                item.SubItems[3].Text = pSelectedRoom.CostToBuild.ToString();
                item.SubItems[4].Text = pSelectedRoom.DaysToBuild.ToString();
                item.SubItems[5].Text = pSelectedRoom.Description;
                UpdateStringColor(item);
                return true;
            }
            return false;
        }

        private void RoomEditMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in RoomsListView.SelectedItems)
            {
                EditRoom(item);
            }
        }

        private void RoomDeleteMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in RoomsListView.SelectedItems)
            {
                CRoom pSelectedRoom = item.Tag as CRoom;

                if (pSelectedRoom.Delete())
                {
                    m_pWorld.Rooms.Remove(pSelectedRoom);
                    RoomsListView.Items.Remove(item);
                }
                else
                    UpdateStringColor(item);
            }
        }

        private void ItemNewHouseholdMenuItem_Click(object sender, EventArgs e)
        {
            CHouseholdMachinery pMachinery = new CHouseholdMachinery();
            m_pWorld.Items.Add(pMachinery);

            ListViewItem pItem = AddItem(pMachinery);
            ItemsListView.FocusedItem = pItem;
            if (!EditItem(pItem))
            {
                pMachinery.Delete();
                m_pWorld.Items.Remove(pMachinery);
                ItemsListView.Items.Remove(pItem);
            }
        }

        private ListViewItem AddItem(CHouseholdMachinery pMachinery)
        {
            ListViewItem pItem = new ListViewItem(pMachinery.Value);
            pItem.SubItems.Add(pMachinery.Name);
            pItem.SubItems.Add("Household device");
            pItem.SubItems.Add(pMachinery.Description);
            pItem.Tag = pMachinery;

            ItemsListView.Items.Add(pItem);
            UpdateStringColor(pItem);

            return pItem;
        }

        private void ItemNewGiftMenuItem_Click(object sender, EventArgs e)
        {
            CGift pGift = new CGift();
            m_pWorld.Items.Add(pGift);

            ListViewItem pItem = AddItem(pGift);
            ItemsListView.FocusedItem = pItem;
            if (!EditItem(pItem))
            {
                pGift.Delete();
                m_pWorld.Items.Remove(pGift);
                ItemsListView.Items.Remove(pItem);
            }
        }

        private ListViewItem AddItem(CGift pGift)
        {
            ListViewItem pItem = new ListViewItem(pGift.Value);
            pItem.SubItems.Add(pGift.Name);
            pItem.SubItems.Add("Gift");
            pItem.SubItems.Add(pGift.Description);
            pItem.Tag = pGift;

            ItemsListView.Items.Add(pItem);
            UpdateStringColor(pItem);

            return pItem;
        }

        private void ItemNewUniformMenuItem_Click(object sender, EventArgs e)
        {
            CUniform pUniform = new CUniform();
            m_pWorld.Items.Add(pUniform);

            ListViewItem pItem = AddItem(pUniform);
            ItemsListView.FocusedItem = pItem;
            if (!EditItem(pItem))
            {
                pUniform.Delete();
                m_pWorld.Items.Remove(pUniform);
                ItemsListView.Items.Remove(pItem);
            }
        }

        private ListViewItem AddItem(CUniform pUniform)
        {
            ListViewItem pItem = new ListViewItem(pUniform.Value);
            pItem.SubItems.Add(pUniform.Name);
            pItem.SubItems.Add("Uniform");
            pItem.SubItems.Add(pUniform.Description);
            pItem.Tag = pUniform;

            ItemsListView.Items.Add(pItem);
            UpdateStringColor(pItem);

            return pItem;
        }

        private void ItemNewSexToyMenuItem_Click(object sender, EventArgs e)
        {
            CSexToy pSexToy = new CSexToy();
            m_pWorld.Items.Add(pSexToy);

            ListViewItem pItem = AddItem(pSexToy);
            ItemsListView.FocusedItem = pItem;
            if (!EditItem(pItem))
            {
                pSexToy.Delete();
                m_pWorld.Items.Remove(pSexToy);
                ItemsListView.Items.Remove(pItem);
            }
        }

        private ListViewItem AddItem(CSexToy pSexToy)
        {
            ListViewItem pItem = new ListViewItem(pSexToy.Value);
            pItem.SubItems.Add(pSexToy.Name);
            pItem.SubItems.Add("Sex toy");
            pItem.SubItems.Add(pSexToy.Description);
            pItem.Tag = pSexToy;

            ItemsListView.Items.Add(pItem);
            UpdateStringColor(pItem);

            return pItem;
        }

        private bool EditItem(ListViewItem item)
        {
            if(item.SubItems[2].Text == "Household device")
            {
                CHouseholdMachinery pDevice = item.Tag as CHouseholdMachinery;
                DeviceEditForm form = new DeviceEditForm(pDevice);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    item.Text = pDevice.Value;
                    item.SubItems[1].Text = pDevice.Name;
                    item.SubItems[3].Text = pDevice.Description;
                    UpdateStringColor(item);
                    return true;
                }
            }
            if(item.SubItems[2].Text == "Gift")
            {
                CGift pGift = item.Tag as CGift;
                GiftEditForm form = new GiftEditForm(pGift);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    item.Text = pGift.Value;
                    item.SubItems[1].Text = pGift.Name;
                    item.SubItems[3].Text = pGift.Description;
                    UpdateStringColor(item);
                    return true;
                }
            }
            if(item.SubItems[2].Text == "Uniform")
            {
                CUniform pUniform = item.Tag as CUniform;
                UniformEditForm form = new UniformEditForm(pUniform);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    item.Text = pUniform.Value;
                    item.SubItems[1].Text = pUniform.Name;
                    item.SubItems[3].Text = pUniform.Description;
                    UpdateStringColor(item);
                    return true;
                }
            }
            if(item.SubItems[2].Text == "Sex toy")
            {
                CSexToy pSexToy = item.Tag as CSexToy;
                SexToyEditForm form = new SexToyEditForm(pSexToy);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    item.Text = pSexToy.Value;
                    item.SubItems[1].Text = pSexToy.Name;
                    item.SubItems[3].Text = pSexToy.Description;
                    UpdateStringColor(item);
                    return true;
                }
            }
            return false;
        }

        private void ItemEditMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in ItemsListView.SelectedItems)
            {
                EditItem(item);
            }
        }

        private void ItemDeleteMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in ItemsListView.SelectedItems)
            {
                CItem pSelectedItem = item.Tag as CItem;

                if (pSelectedItem.Delete())
                {
                    m_pWorld.Items.Remove(pSelectedItem);
                    ItemsListView.Items.Remove(item);
                }
                else
                    UpdateStringColor(item);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
                m_pWorld.Save(saveFileDialog1.FileName);
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                m_pWorld.Reset();
                m_pWorld.Load(openFileDialog1.FileName);
                UpdateAll();
            }
        }

        private void ActionNewMenuItem_Click(object sender, EventArgs e)
        {
            CAction pAction = new CAction();
            m_pWorld.Actions.Add(pAction);

            ListViewItem pItem = AddAction(pAction);
            ActionsListView.FocusedItem = pItem;
            if (!EditAction(pItem))
            {
                pAction.Delete();
                m_pWorld.Actions.Remove(pAction);
                ActionsListView.Items.Remove(pItem);
            }
        }

        private bool EditAction(ListViewItem item)
        {
            CAction pSelectedAction = item.Tag as CAction;
            ActionEditForm form = new ActionEditForm(pSelectedAction);
            if (form.ShowDialog() == DialogResult.OK)
            {
                item.Text = pSelectedAction.Value;
                string statuses = "";
                foreach (string status in pSelectedAction.ActorStatusList.Keys)
                {
                    if (statuses.Length > 0)
                        statuses += ", ";
                    statuses += status;
                    if (!pSelectedAction.ActorStatusList[status])
                        statuses += "-";
                    else
                        statuses += "+";
                }
                if (statuses.Length > 0)
                    statuses = string.Format(" ({0})", statuses);
                switch (pSelectedAction.ActorFilter)
                {
                    case PersonFilter.ANY_PERSON:
                        item.SubItems[1].Text = string.Format("anyone{0}", statuses);
                        break;
                    case PersonFilter.ANY_MAID:
                        item.SubItems[1].Text = string.Format("any maid{0}", statuses);
                        break;
                    case PersonFilter.ANY_GUEST:
                        item.SubItems[1].Text = string.Format("any guest{0}", statuses);
                        break;
                    case PersonFilter.HERO:
                        item.SubItems[1].Text = string.Format("hero{0}", statuses);
                        break;
                    case PersonFilter.SPECIFIED_PERSON:
                        item.SubItems[1].Text = string.Format("someone...{0}", statuses);
                        break;
                }
                statuses = "";
                foreach (string status in pSelectedAction.TargetStatusList.Keys)
                {
                    if (statuses.Length > 0)
                        statuses += ", ";
                    statuses += status;
                    if (!pSelectedAction.TargetStatusList[status])
                        statuses += "-";
                    else
                        statuses += "+";
                }
                if (statuses.Length > 0)
                    statuses = string.Format(" ({0})", statuses);
                switch (pSelectedAction.TargetFilter)
                {
                    case PersonFilter.ANY_PERSON:
                        item.SubItems[2].Text = string.Format("anyone{0}", statuses);
                        break;
                    case PersonFilter.ANY_MAID:
                        item.SubItems[2].Text = string.Format("any maid{0}", statuses);
                        break;
                    case PersonFilter.ANY_GUEST:
                        item.SubItems[2].Text = string.Format("any guest{0}", statuses);
                        break;
                    case PersonFilter.HERO:
                        item.SubItems[2].Text = string.Format("hero{0}", statuses);
                        break;
                    case PersonFilter.SOLO:
                        item.SubItems[2].Text = "none";
                        break;
                    case PersonFilter.SPECIFIED_PERSON:
                        item.SubItems[2].Text = string.Format("someone...{0}", statuses);
                        break;
                }
                if(pSelectedAction.Priority == PriorityFilter.TARGET)
                    item.SubItems[2].Font = new Font(item.SubItems[2].Font, FontStyle.Bold);
                else
                    item.SubItems[2].Font = new Font(item.SubItems[2].Font, FontStyle.Regular);

                switch (pSelectedAction.RoomFilter)
                {
                    case RoomFilter.ANY_ROOM:
                        item.SubItems[3].Text = "anywhere";
                        break;
                    case RoomFilter.ANY_SERVICE_ROOM:
                        item.SubItems[3].Text = "any service room";
                        break;
                    case RoomFilter.ANY_GUEST_ROOM:
                        item.SubItems[3].Text = "any guest's room";
                        break;
                    case RoomFilter.ANY_LEISURE_ROOM:
                        item.SubItems[3].Text = "any leisure room";
                        break;
                    case RoomFilter.SPECIFIED_ROOM:
                        item.SubItems[3].Text = pSelectedAction.Room.Value;
                        break;
                }
                if (pSelectedAction.Priority == PriorityFilter.ROOM)
                    item.SubItems[3].Font = new Font(item.SubItems[3].Font, FontStyle.Bold);
                else
                    item.SubItems[3].Font = new Font(item.SubItems[3].Font, FontStyle.Regular);
                switch (pSelectedAction.DayTimeFilter)
                {
                    case DayTimeFilter.ANY_TIME:
                        item.SubItems[4].Text = "any time";
                        break;
                    case DayTimeFilter.MORNING_DAY:
                        item.SubItems[4].Text = "at morning or afternoon";
                        break;
                    case DayTimeFilter.MORNING:
                        item.SubItems[4].Text = "at morning";
                        break;
                    case DayTimeFilter.DAY:
                        item.SubItems[4].Text = "at afternoon";
                        break;
                    case DayTimeFilter.EVENING:
                        item.SubItems[4].Text = "at evening";
                        break;
                }
                item.SubItems[5].Text = pSelectedAction.Duration.ToString();
                item.SubItems[6].Text = pSelectedAction.Name;
                item.SubItems[7].Text = pSelectedAction.Description;
                UpdateStringColor(item);

                return true;
            }
            return false;
        }

        private void ActionEditMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in ActionsListView.SelectedItems)
            {
                EditAction(item);
            }
        }

        private void ActionDeleteMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in ActionsListView.SelectedItems)
            {
                CAction pSelectedAction = item.Tag as CAction;

                if (pSelectedAction.Delete())
                {
                    m_pWorld.Actions.Remove(pSelectedAction);
                    ActionsListView.Items.Remove(item);
                }
                else
                    UpdateStringColor(item);
            }
        }

        private void ActionsListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                CAction pSelectedAction = e.Item.Tag as CAction;
                ReactionsListView.Items.Clear();
                foreach (CReaction reaction in pSelectedAction.Reactions)
                {
                    AddReaction(reaction);
                }
            }
        }

        private ListViewItem AddReaction(CReaction pReaction)
        {
            ListViewItem pItem = new ListViewItem(pReaction.Value);
            pItem.SubItems.Add(pReaction.Priority.ToString());
            if (pReaction.Continue)
                pItem.SubItems.Add("x");
            else
                pItem.SubItems.Add("");

            string conditions = "";
            foreach (CCondition pCondition in pReaction.Conditions)
            {
                if (pCondition.State != ModifyState.Erased)
                {
                    if (conditions.Length > 0)
                        conditions += ", ";
                    conditions += pCondition.ToString();
                }
            }
            pItem.SubItems.Add(conditions);

            string commands = "";
            foreach (CCommand pCommand in pReaction.Commands)
            {
                if (pCommand.State != ModifyState.Erased)
                {
                    if (commands.Length > 0)
                        commands += ", ";
                    commands += pCommand.ToString();
                }
            }
            pItem.SubItems.Add(commands);

            pItem.SubItems.Add(pReaction.Description);
            pItem.Tag = pReaction;

            ReactionsListView.Items.Add(pItem);
            UpdateStringColor(pItem);

            return pItem;
        }

        private void ReactionNewMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in ActionsListView.SelectedItems)
            {
                CAction pSelectedAction = item.Tag as CAction;

                CReaction pReaction = new CReaction(pSelectedAction);

                ListViewItem pItem = AddReaction(pReaction);
                ReactionsListView.FocusedItem = pItem;
                if (!EditReaction(pItem))
                {
                    pReaction.Delete();
                    pSelectedAction.RemoveReaction(pReaction);
                    ReactionsListView.Items.Remove(pItem);
                }
                else
                    ReactionsListView.Sort();
            }
        }

        private bool EditReaction(ListViewItem pItem)
        {
            CReaction pSelectedReaction = pItem.Tag as CReaction;
            ReactionEditForm form = new ReactionEditForm(pSelectedReaction);
            if (form.ShowDialog() == DialogResult.OK)
            {
                pItem.Text = pSelectedReaction.Value;
                pItem.SubItems[1].Text = pSelectedReaction.Priority.ToString();
                if (pSelectedReaction.Continue)
                    pItem.SubItems[2].Text = "x";
                else
                    pItem.SubItems[2].Text = "";

                string conditions = "";
                foreach (CCondition pCondition in pSelectedReaction.Conditions)
                {
                    if (pCondition.State != ModifyState.Erased)
                    {
                        if (conditions.Length > 0)
                            conditions += ", ";
                        conditions += pCondition.ToString();
                    }
                }
                pItem.SubItems[3].Text = conditions;

                string commands = "";
                foreach (CCommand pCommand in pSelectedReaction.Commands)
                {
                    if (pCommand.State != ModifyState.Erased)
                    {
                        if (commands.Length > 0)
                            commands += ", ";
                        commands += pCommand.ToString();
                    }
                }
                pItem.SubItems[4].Text = commands;

                pItem.SubItems[5].Text = pSelectedReaction.Description;

                UpdateStringColor(pItem);
                
                return true;
            }
            return false;
        }

        // Implements the manual sorting of items by columns.
        class ListViewReactionComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                return int.Parse(((ListViewItem)x).SubItems[1].Text) - int.Parse(((ListViewItem)y).SubItems[1].Text);
            }
        }

        private void ReactionDeleteMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in ReactionsListView.SelectedItems)
            {
                CReaction pSelectedReaction = item.Tag as CReaction;
                CAction pSelectedAction = pSelectedReaction.Parent;

                if (pSelectedReaction.Delete())
                {
                    pSelectedAction.RemoveReaction(pSelectedReaction);
                    ReactionsListView.Items.Remove(item);
                    ReactionsListView.Sort();
                }
                else
                    UpdateStringColor(item);
            }
        }

        private void ReactionEditMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in ReactionsListView.SelectedItems)
            {
                if (EditReaction(item))
                    ReactionsListView.Sort();
            }
        }


    }
}
