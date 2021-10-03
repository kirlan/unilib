using BenTools.Mathematics;
using Socium.Nations;
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

        /// <summary>
        /// Процент реально крутых магов среди всех носителей магических способностей
        /// </summary>
        public MagicAbilityDistribution m_eMagicAbilityDistribution;

        public Culture(Mentality pMentality, int iProgressLevel, Customs pCustoms)
        {
            m_pMentality = pMentality;
            m_iProgressLevel = iProgressLevel;
            m_pCustoms = pCustoms;
        }


        public Culture(Culture pOrigin, Customs.Mutation eCustomsMutation)
        {
            m_pMentality = new Mentality(pOrigin.m_pMentality, eCustomsMutation != Customs.Mutation.None);
            m_iProgressLevel = pOrigin.m_iProgressLevel;
            m_pCustoms = new Customs(pOrigin.m_pCustoms, eCustomsMutation);
        }

        public float GetTrait(Trait eTrait)
        {
            return m_pMentality.Traits[eTrait][m_iProgressLevel];
        }

        /// <summary>
        /// Различие между культурами от +1 (полная противоположность) до -1 (полное совпадение)
        /// </summary>
        /// <param name="pOther">другая культура</param>
        /// <returns>скалярное произведение нормализованных векторов культуры / (количество моральных качеств)</returns>
        public float GetMentalityDifference(Culture pOther)
        {
            return m_pMentality.GetDifference(pOther.m_pMentality, m_iProgressLevel, pOther.m_iProgressLevel);
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
