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
        public int ProgressLevel { get; set; } = 0;
        public Mentality Mentality { get; } = null;
        public Customs Customs { get; } = null;

        /// <summary>
        /// Процент реально крутых магов среди всех носителей магических способностей
        /// </summary>
        public MagicAbilityDistribution MagicAbilityDistribution { get; set; }

        public Culture(Mentality pMentality, int iProgressLevel, Customs pCustoms)
        {
            Mentality = pMentality;
            ProgressLevel = iProgressLevel;
            Customs = pCustoms;
        }

        public Culture(Culture pOrigin, Customs.Mutation eCustomsMutation)
        {
            Mentality = new Mentality(pOrigin.Mentality, eCustomsMutation != Customs.Mutation.None);
            ProgressLevel = pOrigin.ProgressLevel;
            Customs = new Customs(pOrigin.Customs, eCustomsMutation);
        }

        public float GetTrait(Trait eTrait)
        {
            return Mentality.Traits[eTrait][ProgressLevel];
        }

        /// <summary>
        /// Различие между культурами от +1 (полная противоположность) до -1 (полное совпадение)
        /// </summary>
        /// <param name="pOther">другая культура</param>
        /// <returns>скалярное произведение нормализованных векторов культуры / (количество моральных качеств)</returns>
        public float GetMentalityDifference(Culture pOther)
        {
            return Mentality.GetDifference(pOther.Mentality, ProgressLevel, pOther.ProgressLevel);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Culture pOther))
                return false;

            return ProgressLevel == pOther.ProgressLevel &&
                Mentality.Equals(pOther.Mentality) &&
                Customs.Equals(pOther.Customs);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Customs, Mentality, ProgressLevel);
        }
    }
}
