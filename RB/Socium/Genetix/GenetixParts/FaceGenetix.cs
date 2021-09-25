using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace RB.Genetix.GenetixParts
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
    
    public class FaceGenetix : GenetixBase
    {
        /// <summary>
        /// a muzzle with a long proboscis and a tangle of wiggling tentackles instead of mouth
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            string sFace = "?";
            switch (m_eNoseType)
            {
                case NoseType.None:
                    sFace = "a flat face";
                    break;
                case NoseType.Normal:
                    sFace = "a human-like face";
                    break;
                case NoseType.Proboscis:
                    sFace = "a muzzle with a long proboscis";
                    break;
                case NoseType.Snout:
                    sFace = "an animal muzzle";
                    break;
            }

            string sMouth = "?";
            switch (m_eMouthType)
            {
                case MouthType.Normal:
                    sMouth = "small mouth";
                    break;
                case MouthType.Beak:
                    sMouth = "massive beak";
                    break;
                case MouthType.Mandibles:
                    sMouth = "multiple mandibles instead of mouth";
                    break;
                case MouthType.Tentackles:
                    sMouth = "a tangle of wiggling tentackles instead of mouth";
                    break;
            }
            return sFace + (m_eNoseType == NoseType.Proboscis ? " and " : " with ") + sMouth;
        }

        public static FaceGenetix Human
        {
            get { return new FaceGenetix(NoseType.Normal, MouthType.Normal); }
        }

        public static FaceGenetix Ktulhu
        {
            get { return new FaceGenetix(NoseType.Normal, MouthType.Tentackles); }
        }

        public static FaceGenetix Animal
        {
            get { return new FaceGenetix(NoseType.Snout, MouthType.Normal); }
        }

        public static FaceGenetix Bird
        {
            get { return new FaceGenetix(NoseType.None, MouthType.Beak); }
        }

        public static FaceGenetix Insect
        {
            get { return new FaceGenetix(NoseType.None, MouthType.Mandibles); }
        }

        public NoseType m_eNoseType = NoseType.Normal;

        public MouthType m_eMouthType = MouthType.Normal;

        public bool IsIdentical(GenetixBase pOther)
        {
            FaceGenetix pAnother = pOther as FaceGenetix;

            if (pAnother == null)
                return false;

            return m_eNoseType == pAnother.m_eNoseType &&
                m_eMouthType == pAnother.m_eMouthType;
        }
        
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
                FaceGenetix pMutant = new FaceGenetix(this);

                if (Rnd.OneChanceFrom(3))
                    pMutant.m_eNoseType = (NoseType)Rnd.Get(typeof(NoseType));

                //if (Rnd.OneChanceFrom(3))
                //    pMutant.m_eMouthType = (MouthType)Rnd.Get(typeof(MouthType));

                if (pMutant.m_eMouthType == MouthType.Beak || pMutant.m_eMouthType == MouthType.Mandibles)
                    pMutant.m_eNoseType = NoseType.None;

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
    }
}
