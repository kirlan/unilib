using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS30Conf.Actions;
using SS30Conf.Persons;

namespace SS30Conf.Time
{
    class CEventActionFinished: CTimelineRecord
    {
        private CAction m_pAction;
        /// <summary>
        /// Действие, с которым связано событие
        /// </summary>
        public CAction Action
        {
            get { return m_pAction; }
        }

        private CPerson m_pActor;
        /// <summary>
        /// Исполнитель действия
        /// </summary>
        public CPerson Actor
        {
            get { return m_pActor; }
        }

        private CPerson m_pTarget;
        /// <summary>
        /// Цель действия
        /// </summary>
        public CPerson Target
        {
            get { return m_pTarget; }
        }

        public CEventActionFinished(CAction pAction, CPerson pActor, CPerson pTarget)
            : base(RecordType.ACTION_END)
        {
            m_pAction = pAction;
            m_pActor = pActor;
            m_pTarget = pTarget;

            m_iTimeLabel = pAction.Duration;
        }
    }
}
