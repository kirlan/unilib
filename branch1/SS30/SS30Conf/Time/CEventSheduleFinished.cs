using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS30Conf.Time
{
    class CEventSheduleFinished: CTimelineRecord
    {
        private DayTime m_eDayTime;

        public DayTime DayTime
        {
            get { return m_eDayTime; }
        }

        public CEventSheduleFinished(DayTime eDayTime, int iDuration)
            : base(RecordType.DAYTIME_END)
        {
            m_eDayTime = eDayTime;
            m_iTimeLabel = iDuration;
        }
    }
}
