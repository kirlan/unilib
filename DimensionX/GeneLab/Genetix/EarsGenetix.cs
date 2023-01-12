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

    public class EarsGenetix : IGenetix
    {
        /// <summary>
        /// long pointy ears at the sides
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            string sEars = "?";
            switch (EarsType)
            {
                case EarsType.None:
                    sEars = "no visible ears";
                    break;
                case EarsType.Round:
                    sEars = "small round ears";
                    break;
                case EarsType.Pointy:
                    sEars = "small pointy ears";
                    break;
                case EarsType.Long:
                    sEars = "long pointy ears";
                    break;
                case EarsType.BigRound:
                    sEars = "big round ears";
                    break;
                case EarsType.Feelers:
                    sEars = "thin, long feelers";
                    break;
            }

            return sEars + " at the " + (EarsPlacement == EarsPlacement.Top ? "top" : "sides");
        }

        /// <summary>
        /// no ears
        /// </summary>
        public static EarsGenetix None
        {
            get { return new EarsGenetix(EarsType.None, EarsPlacement.Side); }
        }

        /// <summary>
        /// round, at sides
        /// </summary>
        public static EarsGenetix Human
        {
            get { return new EarsGenetix(EarsType.Round, EarsPlacement.Side); }
        }

        /// <summary>
        /// round, at top
        /// </summary>
        public static EarsGenetix Herbivore
        {
            get { return new EarsGenetix(EarsType.Round, EarsPlacement.Top); }
        }

        /// <summary>
        /// pointy, at top
        /// </summary>
        public static EarsGenetix Carnivore
        {
            get { return new EarsGenetix(EarsType.Pointy, EarsPlacement.Top); }
        }

        /// <summary>
        /// pointy, at sides
        /// </summary>
        public static EarsGenetix Elf
        {
            get { return new EarsGenetix(EarsType.Pointy, EarsPlacement.Side); }
        }

        /// <summary>
        /// feelers, at top
        /// </summary>
        public static EarsGenetix Insect
        {
            get { return new EarsGenetix(EarsType.Feelers, EarsPlacement.Top); }
        }

        public EarsType EarsType { get; private set; } = EarsType.Round;

        public EarsPlacement EarsPlacement { get; } = EarsPlacement.Side;

        public bool IsIdentical(IGenetix pOther)
        {
            if (!(pOther is EarsGenetix pAnother))
                return false;

            return EarsType == pAnother.EarsType &&
                EarsPlacement == pAnother.EarsPlacement;
        }

        public EarsGenetix()
        { }

        public EarsGenetix(EarsGenetix pPredcessor)
        {
            EarsType = pPredcessor.EarsType;
            EarsPlacement = pPredcessor.EarsPlacement;
        }

        public EarsGenetix(EarsType eEarsType, EarsPlacement eEarsPlacement)
        {
            EarsType = eEarsType;
            EarsPlacement = eEarsPlacement;
        }

        public IGenetix MutateRace()
        {
            if (Rnd.OneChanceFrom(10))
            {
                EarsGenetix pMutant = new EarsGenetix(this);

                if (Rnd.OneChanceFrom(2))
                    pMutant.EarsType = (EarsType)Rnd.Get(typeof(EarsType));

                //if (Rnd.OneChanceFrom(2))
                //    pMutant.m_eEarsPlacement = (EarsPlacement)Rnd.Get(typeof(EarsPlacement));

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public IGenetix MutateGender()
        {
            if (Rnd.OneChanceFrom(50))
            {
                EarsGenetix pMutant = new EarsGenetix(this);

                if (Rnd.OneChanceFrom(2))
                    pMutant.EarsType = (EarsType)Rnd.Get(typeof(EarsType));

                //if (Rnd.OneChanceFrom(2))
                //    pMutant.m_eEarsPlacement = (EarsPlacement)Rnd.Get(typeof(EarsPlacement));

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public IGenetix MutateNation()
        {
            return this;
        }

        public IGenetix MutateFamily()
        {
            return this;
        }

        public IGenetix MutateIndividual()
        {
            return this;
        }
    }
}
