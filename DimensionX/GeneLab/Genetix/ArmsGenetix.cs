using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace GeneLab.Genetix
{
    public enum ArmsCount
    {
        /// <summary>
        /// без рук - как животные
        /// </summary>
        None,
        /// <summary>
        /// две руки, как у людей
        /// </summary>
        Bimanous,
        /// <summary>
        /// четыре руки, как у Мотаро
        /// </summary>
        Quadrumanous,
    }

    public enum ArmsType
    {
        /// <summary>
        /// ладони, как у людей
        /// </summary>
        Palms,
        /// <summary>
        /// клешни, как у насекомых
        /// </summary>
        Nippers,
    }

    public class ArmsGenetix : GenetixBase
    {
        public static ArmsGenetix None
        {
            get { return new ArmsGenetix(ArmsCount.None, ArmsType.Palms); }
        }

        public static ArmsGenetix Human
        {
            get { return new ArmsGenetix(ArmsCount.Bimanous, ArmsType.Palms); }
        }

        public static ArmsGenetix Insect2
        {
            get { return new ArmsGenetix(ArmsCount.Bimanous, ArmsType.Nippers); }
        }

        public static ArmsGenetix Insect4
        {
            get { return new ArmsGenetix(ArmsCount.Quadrumanous, ArmsType.Nippers); }
        }

        public static ArmsGenetix Motaro
        {
            get { return new ArmsGenetix(ArmsCount.Quadrumanous, ArmsType.Palms); }
        }

        public ArmsCount m_eArmsCount = ArmsCount.Bimanous;

        public ArmsType m_eArmsType = ArmsType.Palms;

        public ArmsGenetix()
        { }

        public ArmsGenetix(ArmsGenetix pPredcessor)
        {
            m_eArmsCount = pPredcessor.m_eArmsCount;
            m_eArmsType = pPredcessor.m_eArmsType;
        }

        public ArmsGenetix(ArmsCount eArmsCount, ArmsType eArmsType)
        {
            m_eArmsCount = eArmsCount;
            m_eArmsType = eArmsType;
        }
        
        public GenetixBase MutateRace()
        {
            if (Rnd.OneChanceFrom(10))
            {
                ArmsGenetix pMutant = new ArmsGenetix(this);

                if (Rnd.OneChanceFrom(2))
                {
                    if (pMutant.m_eArmsCount == ArmsCount.Bimanous)
                        pMutant.m_eArmsCount = ArmsCount.Quadrumanous;
                    else if (pMutant.m_eArmsCount == ArmsCount.Quadrumanous)
                        pMutant.m_eArmsCount = ArmsCount.Bimanous;
                }

                //if (Rnd.OneChanceFrom(2))
                //    pMutant.m_eArmsType = (ArmsType)Rnd.Get(typeof(ArmsType));

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public GenetixBase MutateNation()
        {
            return this;
        }

        public GenetixBase MutateFamily()
        {
            return this;
        }

        public GenetixBase MutateIndividual()
        {
            return this;
        }

        public bool IsIdentical(GenetixBase pOther)
        {
            ArmsGenetix pAnother = pOther as ArmsGenetix;

            if(pAnother == null)
                return false;

            return m_eArmsCount == pAnother.m_eArmsCount && m_eArmsType == pAnother.m_eArmsType;
        }

        /// <summary>
        /// 4 arms with 5 fingers on each
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            if (m_eArmsCount == ArmsCount.None)
                return "";

            return (m_eArmsCount == ArmsCount.Bimanous ? "2" : "4") + " arms with " + (m_eArmsType == ArmsType.Palms ? "5 fingers" : "mighty nippers") + " on each";
        }
    }
}
