using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RB.Story.Solutions;
using RB.Socium;

namespace RB.Story.Troubles
{
    /// <summary>
    /// Начало - постановка перед ГГ проблемы
    /// </summary>
    public abstract class CTrouble : CStoryBase
    {
        private CPerson m_pVictim;
        /// <summary>
        /// Жертва - тот персонаж, проблемы которого мы будем решать
        /// </summary>
        public CPerson Victim
        {
            get { return m_pVictim; }
        }

        public CTrouble(CPerson pVictim)
        {
            m_sDescription = "Эта история началась так...";
            m_sName = "Завязка";

            m_pVictim = pVictim;
        }

        /// <summary>
        /// Возвращает сюжет решения проблемы
        /// </summary>
        /// <returns>сюжет</returns>
        public abstract CPlot GetSolution();
    }
}
