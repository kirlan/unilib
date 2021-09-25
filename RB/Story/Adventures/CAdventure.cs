using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RB.Story.Adventures
{
    public abstract class CAdventure : CStoryBase
    {
        public enum TriggerType
        {
            Success,
            Fault,
            Anycase,
            Disabled
        }

        private TriggerType m_eTrigger = TriggerType.Disabled;

        public TriggerType Trigger
        {
            get { return m_eTrigger; }
        }

        private CStoryLine m_pAdventure;
        /// <summary>
        /// Второстепенное приключение
        /// </summary>
        public CStoryLine SubAdventure
        {
            get { return m_pAdventure; }
        }

        /// <summary>
        /// Задаёт новую сюжетную линию второстепенного приключения.
        /// Используется при считывании из файла.
        /// </summary>
        /// <param name="_adventure">Новая сюжетная линия</param>
        public void SetTrigger(TriggerType eTrigger, CStoryLine pAdventure)
        {
            m_eTrigger = eTrigger;
            m_pAdventure = pAdventure;
        }
    }
}
