using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS30Conf.Actions;
using SS30Conf.Persons;

namespace SS30Conf.Time
{
    public enum RecordType
    { 
        DAYTIME_END,
        ACTION_END,
        BUILDING_END
    }

    /// <summary>
    /// Будущее событие
    /// </summary>
    abstract class CTimelineRecord
    {
        private RecordType m_eType = RecordType.ACTION_END;
        /// <summary>
        /// Тип события - начало или окончание действия
        /// </summary>
        public RecordType Type
        {
            get { return m_eType; }
        }

        public CTimelineRecord(RecordType eType)
        {
            m_eType = eType;
        }

        protected int m_iTimeLabel = 0;
        /// <summary>
        /// Временная метка - количество минут, оставшихся до события
        /// </summary>
        public int TimeLabel
        {
            get { return m_iTimeLabel; }
        }

        /// <summary>
        /// Пропустить время.
        /// Уменьшает временную метку события (т.е. время оставшееся до события)
        /// на указанную величину.
        /// </summary>
        /// <param name="iTimePassed">сколько минут пропустить</param>
        public void PassTime(int iTimePassed)
        {
            m_iTimeLabel -= iTimePassed;
        }
    }
}
