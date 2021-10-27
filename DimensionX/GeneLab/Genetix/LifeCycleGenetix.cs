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

    //public enum GendersDistribution
    //{
    //    /// <summary>
    //    /// мужские особи - огромная редкость
    //    /// </summary>
    //    OnlyFemales,
    //    /// <summary>
    //    /// женщин заметно больше мужчин
    //    /// </summary>
    //    MostlyFemales,
    //    /// <summary>
    //    /// женщин и мужчин примерно поровну
    //    /// </summary>
    //    Equal,
    //    /// <summary>
    //    /// мужчин заметно больше, чем женщин
    //    /// </summary>
    //    MostlyMales,
    //    /// <summary>
    //    /// женские особи - огромная редкость
    //    /// </summary>
    //    OnlyMales
    //}

    public class LifeCycleGenetix: GenetixBase
    {
        /// <summary>
        /// lives really long and could have only a few children during whole lifetime, which are mostly females
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            string sDeath = "";
            switch (m_eDyingRate)
            {
                case DyingRate.Low:
                    sDeath = "an extremely long life span";
                    break;
                case DyingRate.Moderate:
                    sDeath = "an average life span";
                    break;
                case DyingRate.High:
                    sDeath = "a short life span";
                    break;
            }

            string sBirth = "";
            switch (m_eBirthRate)
            {
                case BirthRate.Low:
                    sBirth = "a quite low birthrate";
                    break;
                case BirthRate.Moderate:
                    sBirth = "an average birthrate";
                    break;
                case BirthRate.High:
                    sBirth = "a quite high birthrate";
                    break;
            }

            if (sDeath != "" && sBirth != "")
            {
                sDeath += " and ";
            }

            return "have " + sDeath + sBirth;
        }
        
        /// <summary>
        /// average population, moderate lifespan
        /// </summary>
        public static LifeCycleGenetix Human
        {
            get { return new LifeCycleGenetix(BirthRate.Moderate, DyingRate.Moderate); }
        }

        /// <summary>
        /// small population, long lifespan
        /// </summary>
        public static LifeCycleGenetix Elf
        {
            get { return new LifeCycleGenetix(BirthRate.Low, DyingRate.Low); }
        }

        /// <summary>
        /// average population, long lifespan
        /// </summary>
        public static LifeCycleGenetix DwarfM
        {
            get { return new LifeCycleGenetix(BirthRate.Moderate, DyingRate.Low); }
        }

        /// <summary>
        /// small population, long lifespan
        /// </summary>
        public static LifeCycleGenetix DwarfF
        {
            get { return new LifeCycleGenetix(BirthRate.Low, DyingRate.Low); }
        }

        /// <summary>
        /// large population, short lifespan
        /// </summary>
        public static LifeCycleGenetix Barbarian
        {
            get { return new LifeCycleGenetix(BirthRate.High, DyingRate.High); }
        }

        /// <summary>
        /// average population, short lifespan
        /// </summary>
        public static LifeCycleGenetix HarpyM
        {
            get { return new LifeCycleGenetix(BirthRate.Moderate, DyingRate.High); }
        }

        /// <summary>
        /// large population, short lifespan
        /// </summary>
        public static LifeCycleGenetix HarpyF
        {
            get { return new LifeCycleGenetix(BirthRate.High, DyingRate.High); }
        }

        /// <summary>
        /// large population, short lifespan
        /// </summary>
        public static LifeCycleGenetix OrcM
        {
            get { return new LifeCycleGenetix(BirthRate.High, DyingRate.High); }
        }

        /// <summary>
        /// average population, short lifespan
        /// </summary>
        public static LifeCycleGenetix OrcF
        {
            get { return new LifeCycleGenetix(BirthRate.Moderate, DyingRate.High); }
        }

        /// <summary>
        /// large population, moderate lifespan
        /// </summary>
        public static LifeCycleGenetix InsectM
        {
            get { return new LifeCycleGenetix(BirthRate.High, DyingRate.Moderate); }
        }

        /// <summary>
        /// small population, long lifespan
        /// </summary>
        public static LifeCycleGenetix InsectF
        {
            get { return new LifeCycleGenetix(BirthRate.Low, DyingRate.Low); }
        }

        public BirthRate m_eBirthRate = BirthRate.Moderate;

        public DyingRate m_eDyingRate = DyingRate.Moderate;

        public bool IsIdentical(GenetixBase pOther)
        {
            LifeCycleGenetix pAnother = pOther as LifeCycleGenetix;

            if (pAnother == null)
                return false;

            return m_eBirthRate == pAnother.m_eBirthRate &&
                m_eDyingRate == pAnother.m_eDyingRate;
        }
        
        public LifeCycleGenetix()
        { }

        public LifeCycleGenetix(LifeCycleGenetix pPredcessor)
        {
            m_eBirthRate = pPredcessor.m_eBirthRate;
            m_eDyingRate = pPredcessor.m_eDyingRate;
        }

        public LifeCycleGenetix(BirthRate eBirthRate, DyingRate eDyingRate)
        {
            m_eBirthRate = eBirthRate;
            m_eDyingRate = eDyingRate;

            //if (m_eDyingRate == DyingRate.High && m_eBirthRate != BirthRate.High)
            //    m_eBirthRate = BirthRate.High;

            //if (m_eDyingRate == DyingRate.Moderate && m_eBirthRate == BirthRate.Low)
            //    m_eBirthRate = BirthRate.Moderate;
        }
        
        #region GenetixBase Members

        public GenetixBase MutateRace()
        {
            if (Rnd.OneChanceFrom(10))
            {
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

                //if (pMutant.m_eDyingRate == DyingRate.High && pMutant.m_eBirthRate != BirthRate.High)
                //    pMutant.m_eBirthRate = BirthRate.High;

                //if (pMutant.m_eDyingRate == DyingRate.Moderate && pMutant.m_eBirthRate == BirthRate.Low)
                //    pMutant.m_eBirthRate = BirthRate.Moderate;

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public GenetixBase MutateGender()
        {
            if (Rnd.OneChanceFrom(5))
            {
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

                if (Rnd.OneChanceFrom(5))
                {
                    if (pMutant.m_eDyingRate == DyingRate.Low)
                        pMutant.m_eDyingRate = DyingRate.Moderate;
                    if (pMutant.m_eDyingRate == DyingRate.Moderate)
                        pMutant.m_eDyingRate = Rnd.OneChanceFrom(2) ? DyingRate.Low : DyingRate.High;
                    if (pMutant.m_eDyingRate == DyingRate.High)
                        pMutant.m_eDyingRate = DyingRate.Moderate;
                }

                //if (pMutant.m_eDyingRate == DyingRate.High && pMutant.m_eBirthRate != BirthRate.High)
                //    pMutant.m_eBirthRate = BirthRate.High;

                //if (pMutant.m_eDyingRate == DyingRate.Moderate && pMutant.m_eBirthRate == BirthRate.Low)
                //    pMutant.m_eBirthRate = BirthRate.Moderate;

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public GenetixBase MutateNation()
        {
            if (Rnd.OneChanceFrom(20))
            {
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

                //if (pMutant.m_eDyingRate == DyingRate.High && pMutant.m_eBirthRate != BirthRate.High)
                //    pMutant.m_eBirthRate = BirthRate.High;

                //if (pMutant.m_eDyingRate == DyingRate.Moderate && pMutant.m_eBirthRate == BirthRate.Low)
                //    pMutant.m_eBirthRate = BirthRate.Moderate;
                
                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public GenetixBase MutateFamily()
        {
            if (Rnd.OneChanceFrom(5))
            {
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

                //if (pMutant.m_eDyingRate == DyingRate.High && pMutant.m_eBirthRate != BirthRate.High)
                //    pMutant.m_eBirthRate = BirthRate.High;

                //if (pMutant.m_eDyingRate == DyingRate.Moderate && pMutant.m_eBirthRate == BirthRate.Low)
                //    pMutant.m_eBirthRate = BirthRate.Moderate;

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public GenetixBase MutateIndividual()
        {
            if (Rnd.OneChanceFrom(10))
            {
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

                //if (pMutant.m_eDyingRate == DyingRate.High && pMutant.m_eBirthRate != BirthRate.High)
                //    pMutant.m_eBirthRate = BirthRate.High;

                //if (pMutant.m_eDyingRate == DyingRate.Moderate && pMutant.m_eBirthRate == BirthRate.Low)
                //    pMutant.m_eBirthRate = BirthRate.Moderate;

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        #endregion
    }
}
