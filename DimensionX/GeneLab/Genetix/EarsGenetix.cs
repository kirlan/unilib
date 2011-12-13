using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace GeneLab.Genetix
{
    public enum EarsType
    {
        /// <summary>
        /// ушей нет
        /// </summary>
        None,
        /// <summary>
        /// круглые уши, как у человека, мышей, овец или медведя
        /// </summary>
        Round,
        /// <summary>
        /// заострённые уши, как у эльфов, кошки или волка
        /// </summary>
        Pointy,
        /// <summary>
        /// длинные уши, как у кролика или осла
        /// </summary>
        Long,
        /// <summary>
        /// большие уши, как у слона
        /// </summary>
        BigRound,
        /// <summary>
        /// усики, как у насекомых
        /// </summary>
        Feelers
    }

    public enum EarsPlacement
    { 
        /// <summary>
        /// по бокам головы, как у людей и обезьян
        /// </summary>
        Side,
        /// <summary>
        /// на макушке, как у большинства животных
        /// </summary>
        Top
    }
    
    class EarsGenetix : GenetixBase
    {
        public EarsType m_eEarsType = EarsType.Round;

        public EarsPlacement m_eEarsPlacement = EarsPlacement.Side;

        public EarsGenetix()
        { }

        public EarsGenetix(EarsGenetix pPredcessor)
        {
            m_eEarsType = pPredcessor.m_eEarsType;
            m_eEarsPlacement = pPredcessor.m_eEarsPlacement;
        }

        public EarsGenetix(EarsType eEarsType, EarsPlacement eEarsPlacement)
        {
            m_eEarsType = eEarsType;
            m_eEarsPlacement = eEarsPlacement;
        }
        
        public GenetixBase MutateRace()
        {
            if (Rnd.OneChanceFrom(10))
            {
                bool bMutation = false;

                EarsGenetix pMutant = new EarsGenetix(this);

                if (Rnd.OneChanceFrom(2))
                {
                    pMutant.m_eEarsType = (EarsType)Rnd.Get(typeof(EarsType));
                    if (pMutant.m_eEarsType != m_eEarsType)
                        bMutation = true;
                }

                if (Rnd.OneChanceFrom(2))
                {
                    pMutant.m_eEarsPlacement = (EarsPlacement)Rnd.Get(typeof(EarsPlacement));
                    if (pMutant.m_eEarsPlacement != m_eEarsPlacement)
                        bMutation = true;
                }

                if(bMutation)
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
