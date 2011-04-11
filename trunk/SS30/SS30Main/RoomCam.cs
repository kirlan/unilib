using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SS30Conf;
using SS30Conf.Time;

namespace SS30Main
{
    public partial class RoomCam : UserControl
    {
        private CRoom m_pRoom = null;

        public CRoom Room
        {
            get { return m_pRoom; }
            set 
            { 
                m_pRoom = value;
                if (m_pRoom != null)
                    groupBox1.Text = m_pRoom.Name;
                richTextBox1.Text = "";
            }
        }

        public RoomCam()
        {
            InitializeComponent();
        }

        private List<CLogRecord> m_cContinuousEvents = new List<CLogRecord>();

        public int PlayLog(int currentMinutes, int currentDay)
        {
            richTextBox1.Text = "";

            foreach (CLogRecord pRec in m_cContinuousEvents)
            { 
                if(pRec.FullMinutes)
            }

            int sheduledMinutes1 = -1;
            do
            {
                CLogRecord pFirstRecord = m_pRoom.GetFirstRecord();
                if (pFirstRecord == null)
                {
                    break;
                }

                sheduledMinutes1 = pFirstRecord.Hour * 60 + pFirstRecord.Minute;
                sheduledMinutes1 += (pFirstRecord.Day - currentDay) * 24 * 60;

                if (sheduledMinutes1 <= currentMinutes)
                {
                    if (pFirstRecord.Message != "")
                    {
                        richTextBox1.AppendText(pFirstRecord.ToString());
                        richTextBox1.ScrollToCaret();
                    }
                    m_pRoom.Log.Remove(pFirstRecord);
                    m_cContinuousEvents.Add(pFirstRecord);
                }
            }
            while (sheduledMinutes1 <= currentMinutes);

            return sheduledMinutes;
        }
    }
}
