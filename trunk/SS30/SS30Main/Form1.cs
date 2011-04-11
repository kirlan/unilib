using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SS30Conf;
using SS30Conf.Persons;
using SS30Conf.Actions;
using SS30Conf.Time;

namespace SS30Main
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            tabControl1.ItemSize = new Size(0,1);
        }

        private CWorld m_pWorld;

        private CPerson m_pHero;
        private CMaid m_pMaid;

        private CRoom m_pRoom;

        private void LoadButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                m_pWorld = new CWorld();
                m_pWorld.Load(openFileDialog1.FileName);
                richTextBox1.AppendText(string.Format("{0} загружен.\n", openFileDialog1.FileName));

                analogClock1.Hours = 9;
                analogClock1.Minutes = 0;
                gameCalendar1.DaysPassed = 0;

                m_pHero = m_pWorld.Hero;
                m_pHero.Name = "Вы";

                m_pMaid = new CMaid();
                m_pMaid.Name = "Яна";
                m_pWorld.Persons.Add(m_pMaid);

                m_pRoom = CConfigRepository.Instance.Strings[StringCategory.ROOM]["roo_hero"] as CRoom;

                m_pHero.GoTo(m_pRoom);
                m_pMaid.GoTo(m_pRoom);

                roomCam1.Room = CConfigRepository.Instance.Strings[StringCategory.ROOM]["roo_kitchen"] as CRoom;
                roomCam2.Room = CConfigRepository.Instance.Strings[StringCategory.ROOM]["roo_hero"] as CRoom;
                roomCam3.Room = CConfigRepository.Instance.Strings[StringCategory.ROOM]["roo_maids_1"] as CRoom;

                UpdateStats(true);
            }
        }

        private void UpdateActions()
        {
            if (button2.Enabled)
                return;

            List<CAction> cActions = m_pWorld.GetAvailableActions(m_pHero);

            listBox1.Items.Clear();
            foreach (CAction action in cActions)
            {
                listBox1.Items.Add(action.Name);
            }
        }

        void newItem_Click(object sender, EventArgs e)
        {
            DayTime eDayTime = (DayTime)((sender as ToolStripItem).Owner.Tag);

            CAction action = (sender as ToolStripItem).Tag as CAction;
            m_pMaid.SheduledActions[eDayTime] = action;
            switch (eDayTime)
            {
                case DayTime.MORNING:
                    button3.Text = action.Name;
                    button3.Tag = action;
                    break;
                case DayTime.AFTERNOON:
                    button4.Text = action.Name;
                    button4.Tag = action;
                    break;
                case DayTime.EVENING:
                    button5.Text = action.Name;
                    button5.Tag = action;
                    break;
            }
            CheckSheduleReady();
        }

        private void GoButton_Click(object sender, EventArgs e)
        {
            UpdateStats(true);
            tabControl1.SelectedIndex = 0;

            m_pHero.GoTo(m_pRoom);

            m_pWorld.StartMaidsShedule();

            button2.Enabled = false;

            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;

            PlayHeroLog();
        }

        private void ActionsList_DoubleClick(object sender, EventArgs e)
        {
            List<CAction> cActions = m_pWorld.GetActionsByName(m_pHero, listBox1.SelectedItem as string);
            
            foreach (CAction pAction in cActions)
            {
                m_pWorld.ExecuteHeroAction(pAction);
            }
            PlayHeroLog();
        }

        /// <summary>
        /// отображает в richTextBox1 лог событий отдельно взятой комнаты
        /// </summary>
        /// <param name="pRoom"></param>
        private void PlayHeroLog()
        {
            int currentMinutes = analogClock1.Hours * 60 + analogClock1.Minutes;
            int sheduledMinutes = PlayLog(m_pHero.Room, richTextBox1, currentMinutes);

            if (sheduledMinutes == -1)
            {
                UpdateActions();
                return;
            }

            analogClock1.AddMinutesAnimated(sheduledMinutes - currentMinutes);
        }

        private void PlayLogs()
        {
            int currentMinutes = analogClock1.Hours * 60 + analogClock1.Minutes;
            int sheduledMinutes1 = roomCam1.PlayLog(currentMinutes, gameCalendar1.DaysPassed);
            int sheduledMinutes2 = roomCam2.PlayLog(currentMinutes, gameCalendar1.DaysPassed);
            int sheduledMinutes3 = roomCam3.PlayLog(currentMinutes, gameCalendar1.DaysPassed);

            int sheduledMinutes = MinPos(sheduledMinutes1, MinPos(sheduledMinutes2, sheduledMinutes3));
            if (sheduledMinutes != -1)
            {
                analogClock1.AddMinutesAnimated(sheduledMinutes - currentMinutes);
            }
        }

        private int MinPos(int x1, int x2)
        {
            if (x1 == -1)
                return x2;

            if (x2 == -1)
                return x1;

            return Math.Min(x1, x2);
        }

        private int PlayLog(CRoom pRoom, RichTextBox textBox, int currentMinutes)
        {
            int sheduledMinutes;
            do
            {
                CLogRecord pFirstRecord = pRoom.GetFirstRecord();
                if (pFirstRecord == null)
                {
                    return -1;
                }

                sheduledMinutes = pFirstRecord.Hour * 60 + pFirstRecord.Minute;
                sheduledMinutes += (pFirstRecord.Day - gameCalendar1.DaysPassed) * 24 * 60;

                if (sheduledMinutes <= currentMinutes)
                {
                    if (pFirstRecord.Message != "")
                    {
                        textBox.AppendText(string.Format("[{1:D2}:{2:D2}] {0}\n", pFirstRecord.Message, pFirstRecord.Hour, pFirstRecord.Minute));
                        textBox.ScrollToCaret();
                    }
                    pRoom.Log.Remove(pFirstRecord);
                }
            }
            while (sheduledMinutes <= currentMinutes);

            return sheduledMinutes;
        }

        private void SheduleFinished(object sender, EventArgs e)
        {
            //button2.Enabled = true;
            listBox1.Items.Clear();

            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;

            CheckSheduleReady();
        }

        private void CheckSheduleReady()
        {
            button2.Enabled = button3.Tag != null && button4.Tag != null && button5.Tag != null;
            button9.Enabled = button2.Enabled;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Items.Clear();
            contextMenuStrip1.Tag = DayTime.MORNING;

            List<CAction> actions = m_pWorld.GetAvailableActions(m_pMaid, DayTime.MORNING);
            foreach (CAction act in actions)
            {
                ToolStripItem newItem = contextMenuStrip1.Items.Add(act.Name);
                newItem.Click += new EventHandler(newItem_Click);
                newItem.Tag = act;
            }
            contextMenuStrip1.Show(button3, 0, 0);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Items.Clear();
            contextMenuStrip1.Tag = DayTime.AFTERNOON;

            List<CAction> actions = m_pWorld.GetAvailableActions(m_pMaid, DayTime.AFTERNOON);
            foreach (CAction act in actions)
            {
                ToolStripItem newItem = contextMenuStrip1.Items.Add(act.Name);
                newItem.Click += new EventHandler(newItem_Click);
                newItem.Tag = act;
            }
            contextMenuStrip1.Show(button4, 0, 0);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Items.Clear();
            contextMenuStrip1.Tag = DayTime.EVENING;

            List<CAction> actions = m_pWorld.GetAvailableActions(m_pMaid, DayTime.EVENING);
            foreach (CAction act in actions)
            {
                ToolStripItem newItem = contextMenuStrip1.Items.Add(act.Name);
                newItem.Click += new EventHandler(newItem_Click);
                newItem.Tag = act;
            }
            contextMenuStrip1.Show(button5, 0, 0);
        }

        private void Midnight(object sender, EventArgs e)
        {
            gameCalendar1.DaysPassed++;
        }

        private void analogClock1_AnimationFinished(object sender, EventArgs e)
        {
            //if (m_pFirstRecord == null)
            //    return;

            //if (m_pFirstRecord.Message != "")
            //{
            //    richTextBox1.AppendText(string.Format("[{1:D2}:{2:D2}] {0}\n", m_pFirstRecord.Message, m_pFirstRecord.Hour, m_pFirstRecord.Minute));
            //    richTextBox1.ScrollToCaret();
            //}

            UpdateStats(false);

            //m_cLog.Remove(m_pFirstRecord);
            if(tabControl1.SelectedIndex == 0)
                PlayHeroLog();
            else
                PlayLogs();
        }

        private void UpdateStats(bool bReset)
        {
            int newValue = m_pMaid.GetStat(PersonStats.health);
            if (HealthStat.Value != newValue)
                HealthStat.Value = newValue;
            newValue = m_pMaid.GetStat(PersonStats.fatique);
            if (FatiqueStat.Value != newValue)
                FatiqueStat.Value = newValue;
            newValue = m_pMaid.GetStat(PersonStats.endurance);
            if (EnduranceStat.Value != newValue)
                EnduranceStat.Value = newValue;
            newValue = m_pMaid.GetStat(PersonStats.happiness);
            if (HappinessStat.Value != newValue)
                HappinessStat.Value = newValue;
            newValue = m_pMaid.GetStat(PersonStats.stress);
            if (StressStat.Value != newValue)
                StressStat.Value = newValue;
            newValue = m_pMaid.GetStat(PersonStats.obedience);
            if (ObedienceStat.Value != newValue)
                ObedienceStat.Value = newValue;
            newValue = m_pMaid.GetStat(PersonStats.sexuality);
            if (SexualityStat.Value != newValue)
                SexualityStat.Value = newValue;
            newValue = m_pMaid.GetStat(PersonStats.arousal);
            if (ArousalStat.Value != newValue)
                ArousalStat.Value = newValue;
            newValue = m_pMaid.GetStat(PersonStats.sensuality);
            if (SensualityStat.Value != newValue)
                SensualityStat.Value = newValue;

            if (bReset)
            {
                HealthStat.Value = m_pMaid.GetStat(PersonStats.health);
                FatiqueStat.Value = m_pMaid.GetStat(PersonStats.fatique);
                EnduranceStat.Value = m_pMaid.GetStat(PersonStats.endurance);
                HappinessStat.Value = m_pMaid.GetStat(PersonStats.happiness);
                StressStat.Value = m_pMaid.GetStat(PersonStats.stress);
                ObedienceStat.Value = m_pMaid.GetStat(PersonStats.obedience);
                SexualityStat.Value = m_pMaid.GetStat(PersonStats.sexuality);
                ArousalStat.Value = m_pMaid.GetStat(PersonStats.arousal);
                SensualityStat.Value = m_pMaid.GetStat(PersonStats.sensuality);
            }
        }

        private void LookButton_Click(object sender, EventArgs e)
        {
            UpdateStats(true);
            tabControl1.SelectedIndex = 1;

            m_pHero.GoTo(m_pRoom);

            m_pWorld.StartMaidsShedule();
            m_pWorld.ExecuteHeroAction(null);

            button2.Enabled = false;

            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;

            analogClock1.Stop();
            PlayLogs();
        }
    }
}
