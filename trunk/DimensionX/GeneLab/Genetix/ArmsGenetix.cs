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
                    if (pMutant.m_eArmsCount == ArmsCount.Quadrumanous)
                        pMutant.m_eArmsCount = ArmsCount.Bimanous;
                }

                if (Rnd.OneChanceFrom(2))
                    pMutant.m_eArmsType = (ArmsType)Rnd.Get(typeof(ArmsType));

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
    }
}
