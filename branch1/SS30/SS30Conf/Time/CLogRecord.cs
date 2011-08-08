using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS30Conf.Time
{
    public class CLogRecord
    {
        private int m_iDay;

        public int Day
        {
            get { return m_iDay; }
        }

        private int m_iHour;

        public int Hour
        {
            get { return m_iHour; }
        }

        private int m_iMinute;

        public int Minute
        {
            get { return m_iMinute; }
        }

        public int FullMinutes
        {
            get { return (m_iDay * 24 + m_iHour) * 60 + m_iMinute; }
        }

        private string m_sMessage;

        public string Message
        {
            get { return m_sMessage; }
        }

        public CLogRecord(string sMessage)
        {
            m_iDay = CTimeKeeper.Instance.DaysPassed;
            m_iHour = CTimeKeeper.Instance.Hours;
            m_iMinute = CTimeKeeper.Instance.Minutes;

            m_sMessage = sMessage;
        }

        public override string ToString()
        {
            return string.Format("[{1:D2}:{2:D2}] {0}\n", Message, Hour, Minute);
        }
    }
}
