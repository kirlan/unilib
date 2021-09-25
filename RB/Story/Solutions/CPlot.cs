using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RB.Story.Adventures;

namespace RB.Story.Solutions
{
    public abstract class CPlot : CStoryBase
    {
        protected CAdventure m_pGoal = null;
        /// <summary>
        /// Последовательность разнообразных приключений, собственно и составляющих сюжет
        /// </summary>
        public CAdventure Goal
        {
            get { return m_pGoal; }
        }

        public CPlot()
        {
        }
    }
}
