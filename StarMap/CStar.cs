using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarMap
{
    public class CStar
    {
        public int m_iX;
        public int m_iY;
        
        public bool m_bHaveHabitablePlanets;

        public CStar(int iX, int iY, bool bHabitable)
        {
            m_iX = iX;
            m_iY = iY;
            m_bHaveHabitablePlanets = bHabitable;
        }
    }
}
