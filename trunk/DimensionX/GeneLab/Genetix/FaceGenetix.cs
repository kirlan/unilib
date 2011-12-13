using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace GeneLab.Genetix
{
    public enum NoseType
    {
        /// <summary>
        /// носа нет, только две ноздри
        /// </summary>
        None,
        /// <summary>
        /// обычный человеческий нос
        /// </summary>
        Normal,
        /// <summary>
        /// обычный звериный нос
        /// </summary>
        Snout,
        /// <summary>
        /// хобот, как у слона
        /// </summary>
        Proboscis
    }

    public enum MouthType
    {
        /// <summary>
        /// обычный рот, как у человека или другого млекопитающего животного (зубы, язык, губы)
        /// </summary>
        Normal,
        /// <summary>
        /// клюв как у птиц
        /// </summary>
        Beak,
        /// <summary>
        /// жвалы, как у насекомых
        /// </summary>
        Mandibles,
        /// <summary>
        /// щупальца, как у Ктулху
        /// </summary>
        Tentackles
    }
    
    class FaceGenetix : GenetixBase
    {
        public NoseType m_eNoseType = NoseType.Normal;

        public MouthType m_eMouthType = MouthType.Normal;
        
        public FaceGenetix()
        { }

        public FaceGenetix(FaceGenetix pPredcessor)
        {
            m_eNoseType = pPredcessor.m_eNoseType;
            m_eMouthType = pPredcessor.m_eMouthType;
        }

        public FaceGenetix(NoseType eNoseType, MouthType eMouthType)
        {
            m_eNoseType = eNoseType;
            m_eMouthType = eMouthType;
        }
        
        public GenetixBase MutateRace()
        {
            if (Rnd.OneChanceFrom(10))
            {
                bool bMutation = false;

                FaceGenetix pMutant = new FaceGenetix(this);

                if (Rnd.OneChanceFrom(3))
                {
                    pMutant.m_eNoseType = (NoseType)Rnd.Get(typeof(NoseType));
                    if (pMutant.m_eNoseType != m_eNoseType)
                        bMutation = true;
                }

                if (Rnd.OneChanceFrom(3))
                {
                    pMutant.m_eMouthType = (MouthType)Rnd.Get(typeof(MouthType));
                    if (pMutant.m_eMouthType != m_eMouthType)
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
