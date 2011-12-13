using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace GeneLab.Genetix
{
    public enum BirthRate
    {
        /// <summary>
        /// Низкая рождаемость - среднее число детей, рождённых одним взрослым за всю жизнь - 1-2 
        /// (рождаемость ниже 1 не рассматриваем, т.к. в этом случае раса вымирает)
        /// </summary>
        Low,
        /// <summary>
        /// Средняя рождаемость - 2-3 ребёнка на одного взрослого, т.е. 4-6 на бинарную семью
        /// </summary>
        Moderate,
        /// <summary>
        /// Высокая рождаемость - 4-6 детей
        /// (рождаемость выше 6 не рассматриваем, т.к. тогда уже теряются понятия привычных родственных связей)
        /// </summary>
        High
    }

    public enum DyingRate
    {
        /// <summary>
        /// Низкая смертность - редко кто умирает иначе чем от старости, прожив весь отмеряный срок
        /// </summary>
        Low,
        /// <summary>
        /// Средняя смертность - многие не доживают до старости (несчастные случаи, болезни и пр.)
        /// Много сирот и неполных семей.
        /// В сочетании со средней рождаемостью имеем примерно стабильную численность популяции.
        /// </summary>
        Moderate,
        /// <summary>
        /// Высокая смертность - до старости доживают считанные единицы.
        /// Много сирот и неполных семей, высокая детская смертность.
        /// Компенсируется только высокой рождаемостью.
        /// </summary>
        High
    }

    public enum GendersDistribution
    {
        /// <summary>
        /// мужские особи - огромная редкость
        /// </summary>
        OnlyFemales,
        /// <summary>
        /// женщин заметно больше мужчин
        /// </summary>
        MostlyFemales,
        /// <summary>
        /// женщин и мужчин примерно поровну
        /// </summary>
        Equal,
        /// <summary>
        /// мужчин заметно больше, чем женщин
        /// </summary>
        MostlyMales,
        /// <summary>
        /// женские особи - огромная редкость
        /// </summary>
        OnlyMales
    }

    public class LifeCycleGenetix: GenetixBase
    {
        public BirthRate m_eBirthRate = BirthRate.Moderate;

        public DyingRate m_eDyingRate = DyingRate.Moderate;

        public GendersDistribution m_eGendersDistribution = GendersDistribution.Equal;

        public LifeCycleGenetix()
        { }

        public LifeCycleGenetix(LifeCycleGenetix pPredcessor)
        {
            m_eBirthRate = pPredcessor.m_eBirthRate;
            m_eDyingRate = pPredcessor.m_eDyingRate;
            m_eGendersDistribution = pPredcessor.m_eGendersDistribution;
        }

        public LifeCycleGenetix(BirthRate eBirthRate, DyingRate eDyingRate, GendersDistribution eGendersDistribution)
        {
            m_eBirthRate = eBirthRate;
            m_eDyingRate = eDyingRate;
            m_eGendersDistribution = eGendersDistribution;

            if (m_eDyingRate == DyingRate.High && m_eBirthRate != BirthRate.High)
                m_eBirthRate = BirthRate.High;

            if (m_eDyingRate == DyingRate.Moderate && m_eBirthRate == BirthRate.Low)
                m_eBirthRate = BirthRate.Moderate;
        }
        
        #region GenetixBase Members

        public GenetixBase MutateRace()
        {
            if (Rnd.OneChanceFrom(10))
            {
                bool bMutation = false;

                LifeCycleGenetix pMutant = new LifeCycleGenetix(this);

                if (Rnd.OneChanceFrom(2))
                {
                    if (pMutant.m_eBirthRate == BirthRate.Low)
                        pMutant.m_eBirthRate = BirthRate.Moderate;
                    if (pMutant.m_eBirthRate == BirthRate.Moderate)
                        pMutant.m_eBirthRate = Rnd.OneChanceFrom(2) ? BirthRate.Low : BirthRate.High;
                    if (pMutant.m_eBirthRate == BirthRate.High)
                        pMutant.m_eBirthRate = BirthRate.Moderate;
                }

                if (Rnd.OneChanceFrom(2))
                {
                    if (pMutant.m_eDyingRate == DyingRate.Low)
                        pMutant.m_eDyingRate = DyingRate.Moderate;
                    if (pMutant.m_eDyingRate == DyingRate.Moderate)
                        pMutant.m_eDyingRate = Rnd.OneChanceFrom(2) ? DyingRate.Low : DyingRate.High;
                    if (pMutant.m_eDyingRate == DyingRate.High)
                        pMutant.m_eDyingRate = DyingRate.Moderate;
                }

                if (m_eDyingRate == DyingRate.High && m_eBirthRate != BirthRate.High)
                    m_eBirthRate = BirthRate.High;

                if (m_eDyingRate == DyingRate.Moderate && m_eBirthRate == BirthRate.Low)
                    m_eBirthRate = BirthRate.Moderate;

                if (pMutant.m_eBirthRate != m_eBirthRate ||
                    pMutant.m_eDyingRate != m_eDyingRate)
                    bMutation = true;

                if (Rnd.OneChanceFrom(2))
                    pMutant.m_eGendersDistribution = (GendersDistribution)Rnd.Get(typeof(GendersDistribution));
                if (pMutant.m_eGendersDistribution != m_eGendersDistribution)
                    bMutation = true;

                if(bMutation)
                    return pMutant;
            }

            return this;
        }

        public GenetixBase MutateNation()
        {
            if (Rnd.OneChanceFrom(20))
            {
                bool bMutation = false;

                LifeCycleGenetix pMutant = new LifeCycleGenetix(this);

                if (Rnd.OneChanceFrom(2))
                {
                    if (pMutant.m_eBirthRate == BirthRate.Low)
                        pMutant.m_eBirthRate = BirthRate.Moderate;
                    if (pMutant.m_eBirthRate == BirthRate.Moderate)
                        pMutant.m_eBirthRate = Rnd.OneChanceFrom(2) ? BirthRate.Low : BirthRate.High;
                    if (pMutant.m_eBirthRate == BirthRate.High)
                        pMutant.m_eBirthRate = BirthRate.Moderate;
                }

                if (Rnd.OneChanceFrom(2))
                {
                    if (pMutant.m_eDyingRate == DyingRate.Low)
                        pMutant.m_eDyingRate = DyingRate.Moderate;
                    if (pMutant.m_eDyingRate == DyingRate.Moderate)
                        pMutant.m_eDyingRate = Rnd.OneChanceFrom(2) ? DyingRate.Low : DyingRate.High;
                    if (pMutant.m_eDyingRate == DyingRate.High)
                        pMutant.m_eDyingRate = DyingRate.Moderate;
                }

                if (m_eDyingRate == DyingRate.High && m_eBirthRate != BirthRate.High)
                    m_eBirthRate = BirthRate.High;

                if (m_eDyingRate == DyingRate.Moderate && m_eBirthRate == BirthRate.Low)
                    m_eBirthRate = BirthRate.Moderate;
                
                if (pMutant.m_eBirthRate != m_eBirthRate ||
                    pMutant.m_eDyingRate != m_eDyingRate)
                    bMutation = true;

                if (Rnd.OneChanceFrom(2))
                {
                    switch (pMutant.m_eGendersDistribution)
                    {
                        case GendersDistribution.OnlyFemales:
                            pMutant.m_eGendersDistribution = GendersDistribution.MostlyFemales;
                            break;
                        case GendersDistribution.MostlyFemales:
                            pMutant.m_eGendersDistribution = Rnd.OneChanceFrom(2) ? GendersDistribution.OnlyFemales : GendersDistribution.Equal;
                            break;
                        case GendersDistribution.Equal:
                            pMutant.m_eGendersDistribution = Rnd.OneChanceFrom(2) ? GendersDistribution.MostlyFemales : GendersDistribution.MostlyMales;
                            break;
                        case GendersDistribution.MostlyMales:
                            pMutant.m_eGendersDistribution = Rnd.OneChanceFrom(2) ? GendersDistribution.Equal : GendersDistribution.OnlyMales;
                            break;
                        case GendersDistribution.OnlyMales:
                            pMutant.m_eGendersDistribution = GendersDistribution.MostlyMales;
                            break;
                    }

                    bMutation = true;
                }

                if(bMutation)
                    return pMutant;
            }

            return this;
        }

        public GenetixBase MutateFamily()
        {
            if (Rnd.OneChanceFrom(10))
            {
                bool bMutation = false;

                LifeCycleGenetix pMutant = new LifeCycleGenetix(this);

                if (Rnd.OneChanceFrom(5))
                {
                    if (pMutant.m_eBirthRate == BirthRate.Low)
                        pMutant.m_eBirthRate = BirthRate.Moderate;
                    if (pMutant.m_eBirthRate == BirthRate.Moderate)
                        pMutant.m_eBirthRate = Rnd.OneChanceFrom(2) ? BirthRate.Low : BirthRate.High;
                    if (pMutant.m_eBirthRate == BirthRate.High)
                        pMutant.m_eBirthRate = BirthRate.Moderate;
                }

                if (Rnd.OneChanceFrom(5))
                {
                    if (pMutant.m_eDyingRate == DyingRate.Low)
                        pMutant.m_eDyingRate = DyingRate.Moderate;
                    if (pMutant.m_eDyingRate == DyingRate.Moderate)
                        pMutant.m_eDyingRate = Rnd.OneChanceFrom(2) ? DyingRate.Low : DyingRate.High;
                    if (pMutant.m_eDyingRate == DyingRate.High)
                        pMutant.m_eDyingRate = DyingRate.Moderate;
                }

                if (m_eDyingRate == DyingRate.High && m_eBirthRate != BirthRate.High)
                    m_eBirthRate = BirthRate.High;

                if (m_eDyingRate == DyingRate.Moderate && m_eBirthRate == BirthRate.Low)
                    m_eBirthRate = BirthRate.Moderate;

                if (pMutant.m_eBirthRate != m_eBirthRate ||
                    pMutant.m_eDyingRate != m_eDyingRate)
                    bMutation = true;

                if (Rnd.OneChanceFrom(2))
                    pMutant.m_eGendersDistribution = (GendersDistribution)Rnd.Get(typeof(GendersDistribution));
                if (pMutant.m_eGendersDistribution != m_eGendersDistribution)
                    bMutation = true;

                if (bMutation)
                    return pMutant;
            }

            return this;
        }

        public GenetixBase MutateIndividual()
        {
            return this;
        }

        #endregion
    }
}
