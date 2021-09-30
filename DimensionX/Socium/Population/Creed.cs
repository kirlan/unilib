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


        public Creed(Creed pOrigin)
        {
            m_pCulture = new Culture(pOrigin.m_pCulture);
            m_iCultureLevel = pOrigin.m_iCultureLevel;
            m_pCustoms = new Customs(pOrigin.m_pCustoms, Customs.Mutation.Mandatory);
        }

        public float GetMentalityValue(Mentality eMentality)
        {
            return m_pCulture.MentalityValues[eMentality][m_iCultureLevel];
        }

        public override bool Equals(object obj)
        {
            Creed pOther = obj as Creed;
            if (pOther == null)
                return false;

            return m_iCultureLevel == pOther.m_iCultureLevel &&
                m_pCulture.Equals(pOther.m_pCulture) &&
                m_pCustoms.Equals(pOther.m_pCustoms);
        }
    }
}
