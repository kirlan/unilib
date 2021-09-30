using Socium.Psichology;
using Socium.Settlements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socium.Population
{
    /// <summary>
    /// Кредо - сочетание культурных ценностей и обычаев конкретного человека или сообщества
    /// </summary>
    public class Creed
    {
        public int m_iCultureLevel = 0;
        public Culture m_pCulture = null;
        public Customs m_pCustoms = null;

        public Creed(Culture pCulture, int iCultureLevel, Customs pCustoms)
        {
            m_pCulture = pCulture;
            m_iCultureLevel = iCultureLevel;
            m_pCustoms = pCustoms;
        }

        public float GetMentalityValue(Mentality eMentality)
        {
            return m_pCulture.MentalityValues[eMentality][m_iCultureLevel];
        }
    }
}
