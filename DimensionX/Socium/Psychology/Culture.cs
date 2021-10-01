using Socium.Psychology;
using Socium.Settlements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socium.Psychology
{
    /// <summary>
    /// Культура - сочетание менальности и обычаев конкретного человека или сообщества
    /// </summary>
    public class Culture
    {
        public int m_iProgressLevel = 0;
        public Mentality m_pMentality = null;
        public Customs m_pCustoms = null;

        public Culture(Mentality pMentality, int iProgressLevel, Customs pCustoms)
        {
            m_pMentality = pMentality;
            m_iProgressLevel = iProgressLevel;
            m_pCustoms = pCustoms;
        }


        public Culture(Culture pOrigin)
        {
            m_pMentality = new Mentality(pOrigin.m_pMentality);
            m_iProgressLevel = pOrigin.m_iProgressLevel;
            m_pCustoms = new Customs(pOrigin.m_pCustoms, Customs.Mutation.Mandatory);
        }

        public float GetTrait(Trait eTrait)
        {
            return m_pMentality.Traits[eTrait][m_iProgressLevel];
        }

        public override bool Equals(object obj)
        {
            Culture pOther = obj as Culture;
            if (pOther == null)
                return false;

            return m_iProgressLevel == pOther.m_iProgressLevel &&
                m_pMentality.Equals(pOther.m_pMentality) &&
                m_pCustoms.Equals(pOther.m_pCustoms);
        }
    }
}
