using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITAS_Engine.Attributes
{
    public enum Attr
    { 
        Awareness,
        Coordination,
        Ingenuity,
        Presence,
        Resolve,
        Strength
    }

    public abstract class CharAttribute
    {
        public int m_iBaseValue = 3;

        public int m_iPermanentDamage = 0;

        public int m_iTemporalDamage = 0;

        public int GetActualValue()
        {
            return Math.Max(0, m_iBaseValue - m_iPermanentDamage - m_iTemporalDamage);
        }

        public abstract string GetDescription(int iValue);
    }
}
