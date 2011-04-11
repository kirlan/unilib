using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS30Conf.Actions;
using SS30Conf.Persons;

namespace SS30Conf.Time
{
    public enum DayTime
    {
        MORNING,
        AFTERNOON,
        EVENING,
        NIGHT
    }

    class CTimeKeeper
    {
        private static CTimeKeeper m_pThis = new CTimeKeeper();

        internal static CTimeKeeper Instance
        {
            get { return CTimeKeeper.m_pThis; }
        }

        private int m_iDaysPassed = 0;

        public int DaysPassed
        {
            get { return m_iDaysPassed; }
        }

        private int m_iHours = 9;

        public int Hours
        {
            get { return m_iHours; }
        }

        private int m_iMinutes = 0;

        public int Minutes
        {
            get { return m_iMinutes; }
        }

        public int FullMinutes
        {
            get { return (m_iDaysPassed * 24 + m_iHours) * 60 + m_iMinutes; }
        }


        public DayTime DayTime
        {
            get
            {
                int dayMinutes = m_iHours * 60 + m_iMinutes;
                if (dayMinutes < 9 * 60)
                    return DayTime.NIGHT;
                if (dayMinutes < 14 * 60)
                    return DayTime.MORNING;
                if (dayMinutes < 19 * 60)
                    return DayTime.AFTERNOON;

                return DayTime.EVENING;
            }
        }

        private CTimeline m_pTimeline = new CTimeline();

        /// <summary>
        /// Добавляет в очередь событий событие окончания указанного действия.
        /// Считаем, что действие выполняется в момент, когда оно добавляется, т.е.
        /// событию окончания действия присваивается временная метка равная длительности
        /// действия.
        /// </summary>
        /// <param name="pAction">Действие, окончание которого добавляем в очередь</param>
        /// <param name="pActor">Исполнитель</param>
        /// <param name="pTarget">Цель</param>
        public void ExecuteAction(CAction pAction, CPerson pActor, CPerson pTarget)
        {
            m_pTimeline.AddRecord(new CEventActionFinished(pAction, pActor, pTarget));
        }

        /// <summary>
        /// Начать новое время суток
        /// </summary>
        public void StartShedule()
        {
            //Сбрасываем все неоконченные действия из прошлого времени суток
            m_pTimeline.ClearActions();

            int dayMinutes = m_iHours * 60 + m_iMinutes;
            
            int iDuration = 0;
            switch (DayTime)
            {
                case DayTime.MORNING:
                    iDuration = 14*60 - dayMinutes;
                    break;
                case DayTime.AFTERNOON:
                    iDuration = 19*60 - dayMinutes;
                    break;
                case DayTime.EVENING:
                    iDuration = 24*60 - dayMinutes;
                    break;
                case DayTime.NIGHT:
                    iDuration = 9*60 - dayMinutes;
                    break;
            }
            m_pTimeline.AddRecord(new CEventSheduleFinished(DayTime, iDuration));
        }

        public NewEventInfo PassTime()
        {
            int iMinutesPassed = m_pTimeline.PassTime();
            
            m_iMinutes += iMinutesPassed;
            while (m_iMinutes > 59)
            {
                m_iMinutes -= 60;
                m_iHours++;
                while (m_iHours > 23)
                {
                    m_iHours -= 24;
                    m_iDaysPassed++;
                }
            }

            if (m_pTimeline.LeadRecord != null)
                return new NewEventInfo(m_pTimeline.LeadRecord, iMinutesPassed);
            else
                return null;
        }

        public class NewEventInfo
        {
            private CTimelineRecord m_pRecord;
            public CTimelineRecord Record
            {
                get { return m_pRecord; }
            }

            private int m_iTimePassed;
            public int TimePassed
            {
                get { return m_iTimePassed; }
            }

            public NewEventInfo(CTimelineRecord pRecord, int iTimePassed)
            {
                m_pRecord = pRecord;
                m_iTimePassed = iTimePassed;
            }
        }
    }
}
