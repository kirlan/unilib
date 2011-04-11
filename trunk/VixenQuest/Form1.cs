using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Random;
using VixenQuest.World;

namespace VixenQuest
{
    public partial class Form1 : Form
    {
        Vixen m_pVixen;
        StoryLine m_pStoryLine;
        Universe m_pWorld;

        public Form1()
        {
            InitializeComponent();

            StartAdventure();
        }

        private void StartAdventure()
        {
            m_pVixen = new Vixen();

            ShowTraits(true);
            ShowStats(true);
            ShowSkills(true);

            SpellsListView.Items.Clear();
            ShowSpells();

            ShowClothes(true);
            ShowJevelry(true);

            ToysListBox.Items.Clear();
            ShowToys();

            InventoryListView.Items.Clear();
            ShowInventory("");

            m_pWorld = new Universe();

            m_pStoryLine = new StoryLine(m_pVixen, m_pWorld);

            ShowPlot(true);

            ShowQuests(true);

            ActionBar.Maximum = ActionBar.ClientRectangle.Width;
            ActionBar.Value = 0;

            VixenPotencyBar.Value = 0;
            VixenPotencyBar.Maximum = VixenPotencyBar.ClientRectangle.Width;
            VixenPotencyBar.Value = VixenPotencyBar.Maximum;
            m_iShouldBeValueVP = VixenPotencyBar.Maximum;

            TargetPotencyBar.Maximum = VixenPotencyBar.Maximum;

            worldMap1.DrawMap(m_pWorld.m_cWorldsByTiers[0][0]);

            timer1.Enabled = true;
        }

        private void ShowQuests(bool bRecreate)
        {
            if (m_pStoryLine == null)
                return;

            if (bRecreate || QuestsListBox.Items.Count > m_pStoryLine.CurrentAdventure.m_iCurrentQuestIndex)
            {
                QuestsListBox.Items.Clear();
            }

            for(int i= 0; i< m_pStoryLine.CurrentAdventure.m_cQuests.Count; i++)
            {
                Quest qst = m_pStoryLine.CurrentAdventure.m_cQuests[i];

                if (!qst.m_bFinished && qst != m_pStoryLine.CurrentQuest)
                    break;

                if(QuestsListBox.Items.Count <= i)
                    QuestsListBox.Items.Add(qst.m_sName);

                if (QuestsListBox.Items[i] as string != qst.m_sName)
                    QuestsListBox.Items[i] = qst.m_sName;

                QuestsListBox.SetItemChecked(i, qst.m_bFinished);
            }
            int iVisibleCount = QuestsListBox.ClientRectangle.Height / QuestsListBox.GetItemHeight(0);
            if (QuestsListBox.Items.Count > iVisibleCount)
                QuestsListBox.TopIndex = QuestsListBox.Items.Count - iVisibleCount;

            if (m_pStoryLine.CurrentQuest.m_iProgressMax > QuestBar.Value)
            {
                QuestBar.Maximum = m_pStoryLine.CurrentQuest.m_iProgressMax;
                QuestBar.Value = m_pStoryLine.CurrentQuest.m_iProgress;
            }
            else
            {
                QuestBar.Value = m_pStoryLine.CurrentQuest.m_iProgress;
                QuestBar.Maximum = m_pStoryLine.CurrentQuest.m_iProgressMax;
            }
        }

        private void ShowPlot(bool bRecreate)
        {
            if (m_pStoryLine == null)
                return;

            if (bRecreate)
            {
                PlotListBox.Items.Clear();
            }

            foreach (Adventure adv in m_pStoryLine.m_cAdventures)
            { 
                if(!PlotListBox.Items.Contains(adv.m_sName))
                    PlotListBox.Items.Add(adv.m_sName);

                int index = PlotListBox.Items.IndexOf(adv.m_sName);
                PlotListBox.SetItemChecked(index, adv.m_bFinished);
            }

            if (m_pStoryLine.CurrentAdventure.m_cQuests.Count > PlotBar.Value)
            {
                PlotBar.Maximum = m_pStoryLine.CurrentAdventure.m_cQuests.Count;
                PlotBar.Value = m_pStoryLine.CurrentAdventure.m_iCurrentQuestIndex;
            }
            else
            {
                PlotBar.Value = m_pStoryLine.CurrentAdventure.m_iCurrentQuestIndex;
                PlotBar.Maximum = m_pStoryLine.CurrentAdventure.m_cQuests.Count;
            }
        }

        private void ShowInventory(string sLoot)
        {
            if (m_pVixen == null)
                return;

            //Удаляем предметы, которые есть в экранном списке, но отсутствуют в реальном
            List<ListViewItem> pVictims = new List<ListViewItem>();
            foreach (ListViewItem pItem in InventoryListView.Items)
            {
                bool shouldBeDeleted = true;
                foreach (Item pLoot in m_pVixen.Loot.Keys)
                {
                    if ((pItem.SubItems[2].Text == pLoot.m_sName ||
                         pItem.SubItems[2].Text == pLoot.m_sNames) && 
                        m_pVixen.Loot[pLoot] > 0)
                    {
                        shouldBeDeleted = false;
                        break;
                    }
                }
                if (shouldBeDeleted)
                {
                    pVictims.Add(pItem);
                }
            }
            foreach(ListViewItem pItem in pVictims)
            {
                InventoryListView.Items.Remove(pItem);
            }

            //Обновляем экранный список
            foreach (Item pLoot in m_pVixen.Loot.Keys)
            {
                if (m_pVixen.Loot[pLoot] <= 0)
                    continue;

                int index = -1;
                foreach (ListViewItem pItem in InventoryListView.Items)
                {
                    if (pItem.SubItems[2].Text == pLoot.m_sName ||
                        pItem.SubItems[2].Text == pLoot.m_sNames)
                    {
                        pItem.SubItems[1].Text = m_pVixen.Loot[pLoot].ToString();
                        if (m_pVixen.Loot[pLoot] > 1)
                            pItem.SubItems[2].Text = pLoot.m_sNames;
                        else
                            pItem.SubItems[2].Text = pLoot.m_sName;

                        index = InventoryListView.Items.IndexOf(pItem);
                        break;
                    }
                }
                if (index < 0)
                {
                    ListViewItem pNewItem = new ListViewItem();
                    pNewItem.SubItems.Add(m_pVixen.Loot[pLoot].ToString());
                    if (m_pVixen.Loot[pLoot] > 1)
                        pNewItem.SubItems.Add(pLoot.m_sNames);
                    else
                        pNewItem.SubItems.Add(pLoot.m_sName);

                    InventoryListView.Items.Add(pNewItem);
                    index = InventoryListView.Items.Count - 1;
                }

                if (pLoot.m_sName == sLoot)
                {
                    InventoryListView.EnsureVisible(index);
                }
            }
            //EncumbranceBar.Value = 0;
            EncumbranceBar.Maximum = m_pVixen.MaxEncumbrance;
            EncumbranceBar.Value = m_pVixen.m_iEncumbrance;
        }

        private void ShowToys()
        {
            if (m_pVixen == null)
                return;
        }

        private void ShowJevelry(bool bRecreate)
        {
            if (m_pVixen == null)
                return;

            if (bRecreate)
            {
                JevelryListView.Items.Clear();

                ListViewItem newItem = new ListViewItem("Neck");
                newItem.SubItems.Add("");
                JevelryListView.Items.Add(newItem);

                newItem = new ListViewItem("Ears");
                newItem.SubItems.Add("");
                JevelryListView.Items.Add(newItem);

                newItem = new ListViewItem("Finger");
                newItem.SubItems.Add("");
                JevelryListView.Items.Add(newItem);

                newItem = new ListViewItem("Wrist");
                newItem.SubItems.Add("");
                JevelryListView.Items.Add(newItem);

                newItem = new ListViewItem("Ankle");
                newItem.SubItems.Add("");
                JevelryListView.Items.Add(newItem);

                newItem = new ListViewItem("Nipples");
                newItem.SubItems.Add("");
                JevelryListView.Items.Add(newItem);

                newItem = new ListViewItem("Belly");
                newItem.SubItems.Add("");
                JevelryListView.Items.Add(newItem);

                if(m_pVixen.Gender == Gender.Male)
                    newItem = new ListViewItem("Cock");
                else
                    newItem = new ListViewItem("Clit");
                newItem.SubItems.Add("");
                JevelryListView.Items.Add(newItem);
            }

            if (m_pVixen.Jevelry.ContainsKey(JevelryBodyPart.Neck))
                JevelryListView.Items[0].SubItems[1].Text = m_pVixen.Jevelry[JevelryBodyPart.Neck].m_sName;
            else
                JevelryListView.Items[0].SubItems[1].Text = "";
            if (m_pVixen.Jevelry.ContainsKey(JevelryBodyPart.Ears))
                JevelryListView.Items[1].SubItems[1].Text = m_pVixen.Jevelry[JevelryBodyPart.Ears].m_sNames;
            else
                JevelryListView.Items[1].SubItems[1].Text = "";
            if (m_pVixen.Jevelry.ContainsKey(JevelryBodyPart.Finger))
                JevelryListView.Items[2].SubItems[1].Text = m_pVixen.Jevelry[JevelryBodyPart.Finger].m_sName;
            else
                JevelryListView.Items[2].SubItems[1].Text = "";
            if (m_pVixen.Jevelry.ContainsKey(JevelryBodyPart.Wrist))
                JevelryListView.Items[3].SubItems[1].Text = m_pVixen.Jevelry[JevelryBodyPart.Wrist].m_sName;
            else
                JevelryListView.Items[3].SubItems[1].Text = "";
            if (m_pVixen.Jevelry.ContainsKey(JevelryBodyPart.Ankle))
                JevelryListView.Items[4].SubItems[1].Text = m_pVixen.Jevelry[JevelryBodyPart.Ankle].m_sName;
            else
                JevelryListView.Items[4].SubItems[1].Text = "";
            if (m_pVixen.Jevelry.ContainsKey(JevelryBodyPart.Nipples))
                JevelryListView.Items[5].SubItems[1].Text = m_pVixen.Jevelry[JevelryBodyPart.Nipples].m_sNames;
            else
                JevelryListView.Items[5].SubItems[1].Text = "";
            if (m_pVixen.Jevelry.ContainsKey(JevelryBodyPart.Belly))
                JevelryListView.Items[6].SubItems[1].Text = m_pVixen.Jevelry[JevelryBodyPart.Belly].m_sName;
            else
                JevelryListView.Items[6].SubItems[1].Text = "";
            if (m_pVixen.Jevelry.ContainsKey(JevelryBodyPart.ClitCock))
                JevelryListView.Items[7].SubItems[1].Text = m_pVixen.Jevelry[JevelryBodyPart.ClitCock].m_sName;
            else
                JevelryListView.Items[7].SubItems[1].Text = "";
        }

        private void ShowSpells()
        {
            if (m_pVixen == null)
                return;
        }

        private void ShowSkills(bool bRecreate)
        {
            if (m_pVixen == null)
                return;

            if (bRecreate)
            {
                SkillsListView.Items.Clear();

                ListViewItem newItem = new ListViewItem("Traditional");
                newItem.SubItems.Add("");
                SkillsListView.Items.Add(newItem);

                newItem = new ListViewItem("Anal");
                newItem.SubItems.Add("");
                SkillsListView.Items.Add(newItem);

                newItem = new ListViewItem("Oral");
                newItem.SubItems.Add("");
                SkillsListView.Items.Add(newItem);

                newItem = new ListViewItem("BDSM");
                newItem.SubItems.Add("");
                SkillsListView.Items.Add(newItem);
            }

            SkillsListView.Items[0].SubItems[1].Text = m_pVixen.Skills[VixenSkill.Traditional].ToString();
            SkillsListView.Items[1].SubItems[1].Text = m_pVixen.Skills[VixenSkill.Anal].ToString();
            SkillsListView.Items[2].SubItems[1].Text = m_pVixen.Skills[VixenSkill.Oral].ToString();
            SkillsListView.Items[3].SubItems[1].Text = m_pVixen.Skills[VixenSkill.SM].ToString();
        }

        private void ShowStats(bool bRecreate)
        {
            if (m_pVixen == null)
                return;

            if (bRecreate)
            {
                StatsListView.Items.Clear();

                ListViewItem newItem = new ListViewItem("Force");
                newItem.SubItems.Add("");
                StatsListView.Items.Add(newItem);

                newItem = new ListViewItem("Beauty");
                newItem.SubItems.Add("");
                StatsListView.Items.Add(newItem);

                newItem = new ListViewItem("Luck");
                newItem.SubItems.Add("");
                StatsListView.Items.Add(newItem);

                newItem = new ListViewItem("Potency");
                newItem.SubItems.Add("");
                StatsListView.Items.Add(newItem);
            }

            if (m_pVixen.StatsBonuses[Stat.Force] > 0)
                StatsListView.Items[0].SubItems[1].Text = m_pVixen.EffectiveStats[Stat.Force].ToString() + " (" + m_pVixen.Stats[Stat.Force].ToString() + " + " + m_pVixen.StatsBonuses[Stat.Force].ToString() + ")";
            else
                StatsListView.Items[0].SubItems[1].Text = m_pVixen.EffectiveStats[Stat.Force].ToString();

            if (m_pVixen.StatsBonuses[Stat.Beauty] > 0)
                StatsListView.Items[1].SubItems[1].Text = m_pVixen.EffectiveStats[Stat.Beauty].ToString() + " (" + m_pVixen.Stats[Stat.Beauty].ToString() + " + " + m_pVixen.StatsBonuses[Stat.Beauty].ToString() + ")";
            else
                StatsListView.Items[1].SubItems[1].Text = m_pVixen.EffectiveStats[Stat.Beauty].ToString();

            if (m_pVixen.StatsBonuses[Stat.Luck] > 0)
                StatsListView.Items[2].SubItems[1].Text = m_pVixen.EffectiveStats[Stat.Luck].ToString() + " (" + m_pVixen.Stats[Stat.Luck].ToString() + " + " + m_pVixen.StatsBonuses[Stat.Luck].ToString() + ")";
            else
                StatsListView.Items[2].SubItems[1].Text = m_pVixen.EffectiveStats[Stat.Luck].ToString();

            if (m_pVixen.StatsBonuses[Stat.Potency] > 0)
                StatsListView.Items[3].SubItems[1].Text = m_pVixen.EffectiveStats[Stat.Potency].ToString() + " (" + m_pVixen.Stats[Stat.Potency].ToString() + " + " + m_pVixen.StatsBonuses[Stat.Potency].ToString() + ")";
            else
                StatsListView.Items[3].SubItems[1].Text = m_pVixen.EffectiveStats[Stat.Potency].ToString();
        }

        private void UpdateTraits()
        {
            ShowTraits(false);
        }

        private void ShowTraits(bool bRecreate)
        {
            if (m_pVixen == null)
                return;

            if (bRecreate)
            {
                TraitsListView.Items.Clear();

                ListViewItem newItem = new ListViewItem("Name");
                newItem.SubItems.Add("");
                TraitsListView.Items.Add(newItem);

                newItem = new ListViewItem("Gender");
                newItem.SubItems.Add("");
                TraitsListView.Items.Add(newItem);

                newItem = new ListViewItem("Race");
                newItem.SubItems.Add("");
                TraitsListView.Items.Add(newItem);

                newItem = new ListViewItem("Orientation");
                newItem.SubItems.Add("");
                TraitsListView.Items.Add(newItem);

                newItem = new ListViewItem("Class");
                newItem.SubItems.Add("");
                TraitsListView.Items.Add(newItem);

                newItem = new ListViewItem("Level");
                newItem.SubItems.Add("");
                TraitsListView.Items.Add(newItem);
            }

            TraitsListView.Items[0].SubItems[1].Text = m_pVixen.Name;
            TraitsListView.Items[1].SubItems[1].Text = m_pVixen.GenderString;
            TraitsListView.Items[2].SubItems[1].Text = m_pVixen.RaceString;
            TraitsListView.Items[3].SubItems[1].Text = m_pVixen.OrientationString;
            if(m_pVixen.Gender == Gender.Male)
                TraitsListView.Items[4].SubItems[1].Text = m_pVixen.Class.m_sNameM;
            else
                TraitsListView.Items[4].SubItems[1].Text = m_pVixen.Class.m_sNameF;
            TraitsListView.Items[5].SubItems[1].Text = m_pVixen.Level.ToString();

            //ExperienceBar.Value = 0;
            ExperienceBar.Maximum = m_pVixen.Experience2LevelUp;
            ExperienceBar.Value = m_pVixen.m_iExperience;
        }

        private void ShowClothes(bool bRecreate)
        {
            if (m_pVixen == null)
                return;

            if (bRecreate)
            {
                ClothesListView.Items.Clear();

                ListViewItem newItem = new ListViewItem("Top");
                newItem.SubItems.Add("");
                ClothesListView.Items.Add(newItem);

                newItem = new ListViewItem("Bottom");
                newItem.SubItems.Add("");
                ClothesListView.Items.Add(newItem);

                newItem = new ListViewItem("Foot");
                newItem.SubItems.Add("");
                ClothesListView.Items.Add(newItem);
            }

            if (m_pVixen.Clothes.ContainsKey(ClothesBodyPart.Top))
                ClothesListView.Items[0].SubItems[1].Text = m_pVixen.Clothes[ClothesBodyPart.Top].m_sName;
            else
                ClothesListView.Items[0].SubItems[1].Text = "";
            if (m_pVixen.Clothes.ContainsKey(ClothesBodyPart.Bottom))
                ClothesListView.Items[1].SubItems[1].Text = m_pVixen.Clothes[ClothesBodyPart.Bottom].m_sName;
            else
                ClothesListView.Items[1].SubItems[1].Text = "";
            if (m_pVixen.Clothes.ContainsKey(ClothesBodyPart.Foot))
                ClothesListView.Items[2].SubItems[1].Text = m_pVixen.Clothes[ClothesBodyPart.Foot].m_sName;
            else
                ClothesListView.Items[2].SubItems[1].Text = "";
        }

        private void TraitsListView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        int m_iOldValueVP = 0;
        int m_iShouldBeValueVP;
        int m_iShouldBeValueTP = 100;
        int m_iOldValue = 0;
        int m_iShouldBeValue = 100;
        int m_iCurrentStep = 0;
        int m_iTotalSteps = 40;

        bool m_bBlink = false;
        int m_iBlinkCounter = 0;

        bool m_bShowViktoryMode = false;

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (m_pVixen == null || m_pStoryLine == null)
                return;

            //if (m_pStoryLine.CurrentAction.m_eType == ActionType.Move)
            //    LocationLabel.Text = "- ? -";
            //else
                LocationLabel.Text = m_pStoryLine.CurrentAction.m_pLocation.m_sName;

            VixenPotency.Text = m_pVixen.Name;
            if (m_pStoryLine.CurrentAction.m_pTarget != null)
            {
                TargetPotency.Visible = true;
                string sEncounterName = m_pStoryLine.CurrentAction.m_pTarget.LongEncounterName;
                sEncounterName = sEncounterName.Substring(0, 1).ToUpper() + sEncounterName.Substring(1);
                TargetPotency.Text = sEncounterName;
            }
            else
            {
                TargetPotency.Visible = false;
                TargetPotencyBar.Visible = false;
            }

            if (m_pStoryLine.CurrentAction.m_pTarget != null)
            {
                VixenPotencyBar.Value = (VixenPotencyBar.Value * 3 + m_iShouldBeValueVP) / 4;
                TargetPotencyBar.Value = (TargetPotencyBar.Value * 3 + m_iShouldBeValueTP) / 4;
            }

            if (!m_bShowViktoryMode)
            {
                ActionLabel.Text = "   " + m_pStoryLine.CurrentAction.m_sName;

                if (VixenPotencyBar.Value > 0)
                    VixenPotencyBar.Visible = true;
                else
                    VixenPotencyBar.Visible = m_bBlink;
                if (m_pStoryLine.CurrentAction.m_pTarget != null)
                {
                    if (TargetPotencyBar.Value > 0)
                        TargetPotencyBar.Visible = true;
                    else
                        TargetPotencyBar.Visible = m_bBlink;
                }

                if (m_iBlinkCounter++ > 10)
                {
                    m_iBlinkCounter = 0;
                    m_bBlink = !m_bBlink;
                }

                m_iShouldBeValue = ActionBar.Maximum;
                //m_iShouldBeValue = m_pStoryLine.CurrentAction.m_iProgressPercent * ActionBar.Maximum / 100 + delta;
                //if (m_iShouldBeValue < 0)
                //    m_iShouldBeValue = 0;
                //if (m_iShouldBeValue > ActionBar.Maximum)
                //    m_iShouldBeValue = ActionBar.Maximum;

                //delta += deltaInc;
                //if (delta > Math.Sqrt(m_iShouldBeValue))
                //    deltaInc = -1;
                //if (delta < -Math.Sqrt(m_iShouldBeValue))
                //    deltaInc = 1;

                ActionBar.Value = m_iOldValue + (m_iShouldBeValue - m_iOldValue) * m_iCurrentStep / m_iTotalSteps;
                if (m_pStoryLine.CurrentAction.m_eType == ActionType.Rest)
                    VixenPotencyBar.Value = m_iOldValueVP + (m_iShouldBeValueVP - m_iOldValueVP) * m_iCurrentStep / m_iTotalSteps;

                if (m_iCurrentStep >= m_iTotalSteps)
                //            if (ActionBar.Value == m_iShouldBeValue)
                {
                    string sLoot = m_pVixen.Complete(m_pStoryLine.CurrentAction);

                    if (m_pStoryLine.CurrentAction.m_iVixenPotency != -1)
                    {
                        //VixenPotencyBar.Value = 0;
                        int iVixenPotencyMaximum = m_pVixen.EffectiveStats[Stat.Potency];
                        m_iShouldBeValueVP = m_pStoryLine.CurrentAction.m_iVixenPotency * VixenPotencyBar.Maximum / iVixenPotencyMaximum;
                    }

                    if (m_pStoryLine.CurrentAction.m_pTarget != null)
                    {
                        //TargetPotencyBar.Value = 0;
                        int iTargetPotencyMaximum = m_pStoryLine.CurrentAction.m_pTarget.Stats[Stat.Potency];
                        m_iShouldBeValueTP = m_pStoryLine.CurrentAction.m_iTargetPotency * TargetPotencyBar.Maximum / iTargetPotencyMaximum;
                    }
                    ShowInventory(sLoot);
                    UpdateTraits();
                    ShowSkills(false);
                    ShowStats(false);
                    ShowClothes(false);
                    ShowJevelry(false);

                    if (m_pStoryLine.CurrentAction.m_pTarget != null && m_pStoryLine.CurrentEncounter.LastAction)
                        m_iTotalSteps = 40;
                    else
                        m_iTotalSteps = 0;
                    m_iCurrentStep = 0;

                    m_bShowViktoryMode = true;
                }
                else
                {
                    m_iCurrentStep++;
                }
            }
            else
            {
                if (m_iCurrentStep >= m_iTotalSteps)
                {
                    ActionBar.Value = 0;
                    m_iOldValueVP = VixenPotencyBar.Value;
                    string sLoot = "";
                    if (m_pStoryLine.CurrentEncounter.m_pReward != null)
                        sLoot = m_pStoryLine.CurrentEncounter.m_pReward.m_sName;
                    
                    //Переходим к следующему действию.
                    m_pStoryLine.Advance();

                    if (m_pStoryLine.CurrentEncounter.m_pTarget != null && m_pStoryLine.CurrentEncounter.FirstAction)
                    {
                        TargetPotencyBar.Value = TargetPotencyBar.Maximum;
                        m_iShouldBeValueTP = TargetPotencyBar.Value;
                    }

                    if (m_pStoryLine.CurrentAction.m_eType == ActionType.Rest)
                    {
                        m_iShouldBeValueVP = VixenPotencyBar.Maximum;
                    }

                    if (m_pStoryLine.CurrentEncounter.m_pTarget != null)
                        ActionBar.RightToLeft = m_pStoryLine.CurrentAction.Passive ? RightToLeft.Yes : RightToLeft.No;
                    else
                        ActionBar.RightToLeft = RightToLeft.No;

                    m_iOldValue = ActionBar.Value;
                    m_iTotalSteps = m_pVixen.ActionDifficulty(m_pStoryLine.CurrentAction);
                    m_iCurrentStep = 0;
                    ShowInventory(sLoot);
                    UpdateTraits();
                    ShowStats(false);
                    ShowSkills(false);
                    ShowClothes(false);
                    ShowJevelry(false);
                    ShowPlot(false);
                    ShowQuests(false);

                    m_bShowViktoryMode = false;
                }
                else
                {
                    if (VixenPotencyBar.Value > TargetPotencyBar.Value)
                        VixenPotencyBar.Visible = true;
                    else
                    {
                        string sEncounterName = m_pStoryLine.CurrentAction.m_pTarget.ShortEncounterName;
                        //sEncounterName = sEncounterName.Substring(0, 1).ToUpper() + sEncounterName.Substring(1);
                        ActionLabel.Text = "   Left lying completely drained...";

                        VixenPotencyBar.Visible = m_bBlink;
                    }

                    if (TargetPotencyBar.Value > VixenPotencyBar.Value)
                        TargetPotencyBar.Visible = true;
                    else
                    {
                        string sEncounterName = m_pStoryLine.CurrentAction.m_pTarget.ShortEncounterName;
                        //sEncounterName = sEncounterName.Substring(0, 1).ToUpper() + sEncounterName.Substring(1);
                        if(m_pStoryLine.CurrentAction.m_eType == ActionType.Evade)
                            ActionLabel.Text = "   Successfully escaped...";
                        else
                            ActionLabel.Text = "   Departing - tired, but triumphant...";

                        TargetPotencyBar.Visible = m_bBlink;
                    }

                    if (m_iBlinkCounter++ > 0)
                    {
                        m_iBlinkCounter = 0;
                        m_bBlink = !m_bBlink;
                    }
                    
                    //VixenPotencyBar.Value = (VixenPotencyBar.Value + m_iShouldBeValueVP) / 2;
                    //TargetPotencyBar.Value = (TargetPotencyBar.Value + m_iShouldBeValueTP) / 2;

                    m_iCurrentStep++;
                }
            }
        }

        private void QuestsListBox_MouseMove(object sender, MouseEventArgs e)
        {
            int index = QuestsListBox.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                Graphics g = QuestsListBox.CreateGraphics();
                string s = QuestsListBox.Items[index] as string;
                SizeF stringSize = g.MeasureString(s, QuestsListBox.Font);
                g.Dispose();

                if (stringSize.Width > QuestsListBox.ClientRectangle.Width)
                {
                    toolTip1.SetToolTip(QuestsListBox, QuestsListBox.Items[index] as string);
                    return;
                }
            }
            toolTip1.SetToolTip(QuestsListBox, "");
        }

        private void ActionBar_Click(object sender, EventArgs e)
        {

        }

        private void PlotBar_Click(object sender, EventArgs e)
        {

        }

        private void vixenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveVixenFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_pVixen.SaveXML(saveVixenFileDialog.FileName);
            }
        }
    }
}
