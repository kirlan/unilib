using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace GeneLab.Genetix
{
    public enum NeckLength
    {
        /// <summary>
        /// шеи нет, голова растёт прямо из тела (и не поворачивается)
        /// </summary>
        None,
        /// <summary>
        /// короткая, ограниченно подвижная шея, как у людей
        /// </summary>
        Short,
        /// <summary>
        /// короткая, хорошо поворачивающаяся шея, как у сов
        /// </summary>
        ShortRotary,
        /// <summary>
        /// длинная, ограниченно подвижная шея, как у лошадей
        /// </summary>
        Long,
        /// <summary>
        /// очень длинная и гибкая шея, как у лебедя или диплодока
        /// </summary>
        ExtraLong
    }

    public enum HeadForm
    {
        /// <summary>
        /// голова с уменьшенной лобной частью - как у животных и питекантропов
        /// </summary>
        Flat,
        /// <summary>
        /// обычная голова, как у человека
        /// </summary>
        Human,
        /// <summary>
        /// голова с гипертрофированной лобной частью - как у сектоидов
        /// </summary>
        IncreasedForehead,
        /// <summary>
        /// голова с гипертрофированной затылочной частью - как у Чужого
        /// </summary>
        IncreasedNape,
        /// <summary>
        /// голова, похожая на головку молота - как у насекомых или рыбы-молота
        /// </summary>
        Hammer
    }

    public enum Horns
    {
        /// <summary>
        /// без рогов
        /// </summary>
        None,
        /// <summary>
        /// маленькие рожки
        /// </summary>
        Small,
        /// <summary>
        /// большие рога
        /// </summary>
        Big
    }

    public class HeadGenetix: IGenetix
    {
        /// <summary>
        /// 2 ugly heads on long, flexible necks
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            string sNeck = "?";
            switch (NeckLength)
            {
                case NeckLength.None:
                    sNeck = "no neck";
                    break;
                case NeckLength.Short:
                    sNeck = "short neck";
                    break;
                case NeckLength.ShortRotary:
                    sNeck = "short, flexible neck";
                    break;
                case NeckLength.Long:
                    sNeck = "long neck";
                    break;
                case NeckLength.ExtraLong:
                    sNeck = "long, flexible neck";
                    break;
            }
            if (HeadsCount > 1)
                sNeck += "s";

            string sForm = "?";
            switch (HeadForm)
            {
                case HeadForm.Flat:
                    sForm = "small ";
                    break;
                case HeadForm.Human:
                    sForm = "common ";
                    break;
                case HeadForm.IncreasedForehead:
                    sForm = "big ";
                    break;
                case HeadForm.IncreasedNape:
                    sForm = "prolonged ";
                    break;
                case HeadForm.Hammer:
                    sForm = "flat ";
                    break;
            }

            string sHorns = "?";
            switch (Horns)
            {
                case Horns.None:
                    sHorns = "";
                    break;
                case Horns.Small:
                    sHorns = ", with small horns";
                    break;
                case Horns.Big:
                    sHorns = ", with big horns";
                    break;
            }

            return (HeadsCount > 1 ? HeadsCount.ToString() : "single") + " " + sNeck + " with " + sForm + (HeadsCount == 1 ? "head" : "heads") + sHorns;
        }

        /// <summary>
        /// 1 human head on short neck, no horns
        /// </summary>
        public static HeadGenetix Human
        {
            get { return new HeadGenetix(1, NeckLength.Short, HeadForm.Human, Horns.None); }
        }

        /// <summary>
        /// 1 human head on short neck, big horns
        /// </summary>
        public static HeadGenetix DemonM
        {
            get { return new HeadGenetix(1, NeckLength.Short, HeadForm.Human, Horns.Big); }
        }

        /// <summary>
        /// 1 human head on short neck, small horns
        /// </summary>
        public static HeadGenetix DemonF
        {
            get { return new HeadGenetix(1, NeckLength.Short, HeadForm.Human, Horns.Small); }
        }

        /// <summary>
        /// 1 big head on short neck, no horns
        /// </summary>
        public static HeadGenetix Sectoid
        {
            get { return new HeadGenetix(1, NeckLength.Short, HeadForm.IncreasedForehead, Horns.None); }
        }

        /// <summary>
        /// 1 small head on short neck, no horns
        /// </summary>
        public static HeadGenetix Pitecantrop
        {
            get { return new HeadGenetix(1, NeckLength.Short, HeadForm.Flat, Horns.None); }
        }

        /// <summary>
        /// 1 small head on short neck, big horns
        /// </summary>
        public static HeadGenetix Minotaur
        {
            get { return new HeadGenetix(1, NeckLength.Short, HeadForm.Flat, Horns.Big); }
        }

        /// <summary>
        /// 1 hammer head on short flexible neck, no horns
        /// </summary>
        public static HeadGenetix Insect
        {
            get { return new HeadGenetix(1, NeckLength.ShortRotary, HeadForm.Hammer, Horns.None); }
        }

        public int HeadsCount { get; } = 1;

        public NeckLength NeckLength { get; private set; } = NeckLength.Short;

        public HeadForm HeadForm { get; private set; } = HeadForm.Human;

        public Horns Horns { get; private set; } = Horns.None;

        public bool IsIdentical(IGenetix pOther)
        {
            if (!(pOther is HeadGenetix pAnother))
                return false;

            return HeadsCount == pAnother.HeadsCount &&
                NeckLength == pAnother.NeckLength &&
                HeadForm == pAnother.HeadForm &&
                Horns == pAnother.Horns;
        }

        public HeadGenetix()
        { }

        public HeadGenetix(HeadGenetix pPredcessor)
        {
            HeadsCount = pPredcessor.HeadsCount;
            NeckLength = pPredcessor.NeckLength;
            HeadForm = pPredcessor.HeadForm;
            Horns = pPredcessor.Horns;
        }

        public HeadGenetix(int iHeadsCount, NeckLength eNeckLength, HeadForm eHeadForm, Horns eHorns)
        {
            HeadsCount = iHeadsCount;
            NeckLength = eNeckLength;
            HeadForm = eHeadForm;
            Horns = eHorns;
        }

        #region GenetixBase Members

        public IGenetix MutateRace()
        {
            if (Rnd.OneChanceFrom(5))
            {
                HeadGenetix pMutant = new HeadGenetix(this);

                //pMutant.m_iHeadsCount = 1 + (int)Math.Pow(Rnd.Get(14), 3) / 1000;

                if (Rnd.OneChanceFrom(2))
                {
                    if (pMutant.HeadsCount == 1 &&
                        pMutant.NeckLength != NeckLength.Long &&
                        pMutant.NeckLength != NeckLength.ExtraLong)
                    {
                        int iChance = (int)Math.Pow(Rnd.Get(15), 3) / 1000;
                        switch (iChance)
                        {
                            case 0:
                                pMutant.NeckLength = NeckLength.Short;
                                break;
                            case 1:
                                pMutant.NeckLength = NeckLength.ShortRotary;
                                break;
                            case 2:
                                pMutant.NeckLength = NeckLength.Long;
                                break;
                            case 3:
                                pMutant.NeckLength = NeckLength.None;
                                break;
                                //case 4:
                                //    pMutant.m_eNeckLength = NeckLength.ExtraLong;
                                //    break;
                        }
                    }
                    else
                    {
                        int iChance = (int)Math.Pow(Rnd.Get(15), 3) / 1000;
                        switch (iChance)
                        {
                            case 0:
                                pMutant.NeckLength = NeckLength.Long;
                                break;
                            case 1:
                                pMutant.NeckLength = NeckLength.ExtraLong;
                                break;
                            case 2:
                                pMutant.NeckLength = NeckLength.Short;
                                break;
                            case 3:
                                pMutant.NeckLength = NeckLength.ShortRotary;
                                break;
                                //case 4:
                                //    pMutant.m_eNeckLength = NeckLength.None;
                                //    break;
                        }
                    }
                }

                //pMutant.m_eHeadForm = (HeadForm)Rnd.Get(typeof(HeadForm));

                switch (Horns)
                {
                    case Horns.None:
                        if (Rnd.OneChanceFrom(2))
                            Horns = Rnd.OneChanceFrom(5) ? Horns.Big : Horns.Small;
                        break;
                    case Horns.Small:
                        Horns = Rnd.OneChanceFrom(2) ? Horns.None : Horns.Big;
                        break;
                    case Horns.Big:
                        Horns = Rnd.OneChanceFrom(5) ? Horns.None : Horns.Small;
                        break;
                }

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public IGenetix MutateGender()
        {
            if (Rnd.OneChanceFrom(5))
            {
                HeadGenetix pMutant = new HeadGenetix(this);

                switch (Horns)
                {
                    case Horns.None:
                        if (Rnd.OneChanceFrom(2))
                            pMutant.Horns = Rnd.OneChanceFrom(5) ? Horns.Big : Horns.Small;
                        break;
                    case Horns.Small:
                        pMutant.Horns = Rnd.OneChanceFrom(2) ? Horns.None : Horns.Big;
                        break;
                    case Horns.Big:
                        pMutant.Horns = Rnd.OneChanceFrom(5) ? Horns.None : Horns.Small;
                        break;
                }

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public IGenetix MutateNation()
        {
            if (Rnd.OneChanceFrom(10))
            {
                HeadGenetix pMutant = new HeadGenetix(this);

                switch (Horns)
                {
                    case Horns.None:
                        if (Rnd.OneChanceFrom(2))
                            pMutant.Horns = Rnd.OneChanceFrom(5) ? Horns.Big : Horns.Small;
                        break;
                    case Horns.Small:
                        pMutant.Horns = Rnd.OneChanceFrom(2) ? Horns.None : Horns.Big;
                        break;
                    case Horns.Big:
                        pMutant.Horns = Rnd.OneChanceFrom(5) ? Horns.None : Horns.Small;
                        break;
                }

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

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

        #endregion
    }
}
