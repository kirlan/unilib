using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace MiscControls
{
    public partial class Clock : UserControl
    {
        public Clock()
        {
            InitializeComponent();
        }
        
        private int m_iDay = 1;
        public int Day
        {
            get { return m_iDay; }
            set
            {
                m_iDay = Math.Max(1, value);
                DayLabel.Text = string.Format("{0}", m_iDay);
            }
        }

        private int m_iHours = 0;
        public int Hours
        {
            get { return m_iHours; }
            set
            {
                m_iHours = value;
                while (m_iHours >= 24)
                {
                    m_iHours -= 24;
                    Day++;
                }
                while (m_iHours < 0)
                {
                    m_iHours += 24;
                    Day--;
                }
                TimeHoursLabel.Text = string.Format("{0:00}", m_iHours);
            }
        }
        private int m_iMinutes = 0;
        public int Minutes
        {
            get { return m_iMinutes; }
            set
            {
                m_iMinutes = value;
                while (m_iMinutes >= 60)
                {
                    m_iMinutes -= 60;
                    Hours++;
                }
                while (m_iMinutes < 0)
                {
                    m_iMinutes += 60;
                    Hours--;
                }

                TimeMinutesLabel.Text = string.Format("{0:00}", m_iMinutes);
            }
        }
        private bool m_bTick = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSecondsLabel.Text = m_bTick ? "" : ":";

            m_bTick = !m_bTick;
        }

        private int m_iAlarmDay = 1;
        public int AlarmDay
        {
            get { return m_iAlarmDay; }
            set { m_iAlarmDay = Math.Max(1, value); }
        }

        private int m_iAlarmHour = 20;
        public int AlarmHour
        {
            get { return m_iAlarmHour; }
            set { m_iAlarmHour = Math.Max(0, Math.Min(23, value)); }
        }

        private int m_iAlarmMinute = 0;
        public int AlarmMinute
        {
            get { return m_iAlarmMinute; }
            set { m_iAlarmMinute = Math.Max(0, Math.Min(59, value)); }
        }

        private int m_iInc = 1;

        private void timer2_Tick(object sender, EventArgs e)
        {
            Minutes += m_iInc;

            if (m_iDay >= m_iAlarmDay && m_iHours >= m_iAlarmHour && m_iMinutes >= m_iAlarmMinute)
            {
                timer2.Enabled = false;
                RiseSinchroEndedEvent();
            }
        }

        /// <summary>
        /// Включить анимацию хода времени
        /// </summary>
        /// <param name="iHour">час, до которого перевести время</param>
        public void SinchronizeTime(int iHour)
        {
            SinchronizeTime(iHour, 0, false);
        }

        /// <summary>
        /// Включить анимацию хода времени
        /// </summary>
        /// <param name="iHour">час, до которого перевести время</param>
        /// <param name="bFast">ускорение времени. Если true, то в 5 раз быстрее чем обычно</param>
        public void SinchronizeTime(int iHour, bool bFast)
        {
            SinchronizeTime(iHour, 0, bFast);
        }

        /// <summary>
        /// Включить анимацию хода времени
        /// </summary>
        /// <param name="iHour">час, до которого перевести время</param>
        /// <param name="iMinute">минута, до которого перевести время</param>
        public void SinchronizeTime(int iHour, int iMinute)
        {
            SinchronizeTime(iHour, iMinute, false);
        }

        /// <summary>
        /// Включить анимацию хода времени
        /// </summary>
        /// <param name="iHour">час, до которого перевести время</param>
        /// <param name="iMinute">минута, до которого перевести время</param>
        /// <param name="bFast">ускорение времени. Если true, то в 5 раз быстрее чем обычно</param>
        public void SinchronizeTime(int iHour, int iMinute, bool bFast)
        {
            if(iHour > m_iHours)
                SinchronizeTime(m_iDay, iHour, iMinute, false);
            else
                SinchronizeTime(m_iDay+1, iHour, iMinute, false);
        }

        /// <summary>
        /// Включить анимацию хода времени
        /// </summary>
        /// <param name="iDay">день, до которого перевести время</param>
        /// <param name="iHour">час, до которого перевести время</param>
        /// <param name="iMinute">минута, до которого перевести время</param>
        /// <param name="bFast">ускорение времени. Если true, то в 5 раз быстрее чем обычно</param>
        public void SinchronizeTime(int iDay, int iHour, int iMinute, bool bFast)
        {
            m_iAlarmDay = iDay;
            m_iAlarmHour = iHour;
            m_iAlarmMinute = iMinute;
            
            m_iInc = bFast ? 5 : 1;
            
            timer2.Enabled = true;
        }

        public event EventHandler<EventArgs> SinchroEnded;

        private void RiseSinchroEndedEvent()
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<EventArgs> temp = SinchroEnded;
            if (temp != null)
                temp(this, new EventArgs());
        }
    }
}
