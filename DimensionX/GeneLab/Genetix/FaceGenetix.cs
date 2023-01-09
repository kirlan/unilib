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

    public class FaceGenetix : GenetixBase
    {
        /// <summary>
        /// a muzzle with a long proboscis and a tangle of wiggling tentackles instead of mouth
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            string sFace = "?";
            switch (NoseType)
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
            switch (MouthType)
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
            return sFace + (NoseType == NoseType.Proboscis ? " and " : " with ") + sMouth;
        }

        /// <summary>
        /// normal nose, normal mouth
        /// </summary>
        public static FaceGenetix Human
        {
            get { return new FaceGenetix(NoseType.Normal, MouthType.Normal); }
        }

        /// <summary>
        /// normal nose, tentakles mouth
        /// </summary>
        public static FaceGenetix Ktulhu
        {
            get { return new FaceGenetix(NoseType.Normal, MouthType.Tentackles); }
        }

        /// <summary>
        /// snout nose, normal mouth
        /// </summary>
        public static FaceGenetix Animal
        {
            get { return new FaceGenetix(NoseType.Snout, MouthType.Normal); }
        }

        /// <summary>
        /// no nose, beak mouth
        /// </summary>
        public static FaceGenetix Bird
        {
            get { return new FaceGenetix(NoseType.None, MouthType.Beak); }
        }

        /// <summary>
        /// no nose, mandibles muth
        /// </summary>
        public static FaceGenetix Insect
        {
            get { return new FaceGenetix(NoseType.None, MouthType.Mandibles); }
        }

        public NoseType NoseType { get; private set; } = NoseType.Normal;

        public MouthType MouthType { get; } = MouthType.Normal;

        public bool IsIdentical(GenetixBase pOther)
        {
            if (!(pOther is FaceGenetix pAnother))
                return false;

            return NoseType == pAnother.NoseType &&
                MouthType == pAnother.MouthType;
        }

        public FaceGenetix()
        { }

        public FaceGenetix(FaceGenetix pPredcessor)
        {
            NoseType = pPredcessor.NoseType;
            MouthType = pPredcessor.MouthType;
        }

        public FaceGenetix(NoseType eNoseType, MouthType eMouthType)
        {
            NoseType = eNoseType;
            MouthType = eMouthType;
        }

        public GenetixBase MutateRace()
        {
            if (Rnd.OneChanceFrom(10))
            {
                FaceGenetix pMutant = new FaceGenetix(this);

                if (Rnd.OneChanceFrom(3))
                    pMutant.NoseType = (NoseType)Rnd.Get(typeof(NoseType));

                //if (Rnd.OneChanceFrom(3))
                //    pMutant.m_eMouthType = (MouthType)Rnd.Get(typeof(MouthType));

                if (pMutant.MouthType == MouthType.Beak || pMutant.MouthType == MouthType.Mandibles)
                    pMutant.NoseType = NoseType.None;

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public GenetixBase MutateGender()
        {
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
