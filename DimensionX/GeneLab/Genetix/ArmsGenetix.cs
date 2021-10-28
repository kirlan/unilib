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
        /// <summary>
        /// No arms
        /// </summary>
        public static ArmsGenetix None
        {
            get { return new ArmsGenetix(ArmsCount.None, ArmsType.Palms); }
        }

        /// <summary>
        /// 2 arms with palms
        /// </summary>
        public static ArmsGenetix Human
        {
            get { return new ArmsGenetix(ArmsCount.Bimanous, ArmsType.Palms); }
        }

        /// <summary>
        /// 2 arms with nippers
        /// </summary>
        public static ArmsGenetix Insect2
        {
            get { return new ArmsGenetix(ArmsCount.Bimanous, ArmsType.Nippers); }
        }

        /// <summary>
        /// 4 arms with nippers
        /// </summary>
        public static ArmsGenetix Insect4
        {
            get { return new ArmsGenetix(ArmsCount.Quadrumanous, ArmsType.Nippers); }
        }

        /// <summary>
        /// 4 arms with palms
        /// </summary>
        public static ArmsGenetix Human4
        {
            get { return new ArmsGenetix(ArmsCount.Quadrumanous, ArmsType.Palms); }
        }

        public ArmsCount ArmsCount { get; private set; } = ArmsCount.Bimanous;

        public ArmsType ArmsType { get; private set; } = ArmsType.Palms;

        public ArmsGenetix()
        { }

        public ArmsGenetix(ArmsGenetix pPredcessor)
        {
            ArmsCount = pPredcessor.ArmsCount;
            ArmsType = pPredcessor.ArmsType;
        }

        public ArmsGenetix(ArmsCount eArmsCount, ArmsType eArmsType)
        {
            ArmsCount = eArmsCount;
            ArmsType = eArmsType;
        }

        public GenetixBase MutateRace()
        {
            if (Rnd.OneChanceFrom(10))
            {
                ArmsGenetix pMutant = new ArmsGenetix(this);

                if (Rnd.OneChanceFrom(2))
                {
                    if (pMutant.ArmsCount == ArmsCount.Bimanous)
                        pMutant.ArmsCount = ArmsCount.Quadrumanous;
                    else if (pMutant.ArmsCount == ArmsCount.Quadrumanous)
                        pMutant.ArmsCount = ArmsCount.Bimanous;
                }

                //if (Rnd.OneChanceFrom(2))
                //    pMutant.m_eArmsType = (ArmsType)Rnd.Get(typeof(ArmsType));

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public GenetixBase MutateGender()
        {
            if (Rnd.OneChanceFrom(50))
            {
                ArmsGenetix pMutant = new ArmsGenetix(this);

                if (Rnd.OneChanceFrom(2))
                {
                    if (pMutant.ArmsCount == ArmsCount.Bimanous)
                        pMutant.ArmsCount = ArmsCount.Quadrumanous;
                    else if (pMutant.ArmsCount == ArmsCount.Quadrumanous)
                        pMutant.ArmsCount = ArmsCount.Bimanous;
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

            return ArmsCount == pAnother.ArmsCount && ArmsType == pAnother.ArmsType;
        }

        /// <summary>
        /// 4 arms with 5 fingers on each
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            if (ArmsCount == ArmsCount.None)
                return "";

            return (ArmsCount == ArmsCount.Bimanous ? "2" : "4") + " arms with " + (ArmsType == ArmsType.Palms ? "5 fingers" : "mighty nippers") + " on each";
        }
    }
}
