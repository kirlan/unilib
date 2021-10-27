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

    public class HeadGenetix: GenetixBase
    {
        /// <summary>
        /// 2 ugly heads on long, flexible necks
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            string sNeck = "?";
            switch (m_eNeckLength)
            {
                case NeckLength.None:
                    sNeck = "no neck";
                    break;
                case NeckLength.Short:
                    sNeck = "short neck";
                    break;
                case NeckLength.ShortRotary:
                    sNeck = "short pivoted neck";
                    break;
                case NeckLength.Long:
                    sNeck = "long neck";
                    break;
                case NeckLength.ExtraLong:
                    sNeck = "long, flexible neck";
                    break;
            }
            if (m_iHeadsCount > 1)
                sNeck += "s";

            string sForm = "?";
            switch (m_eHeadForm)
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
            switch (m_eHorns)
            {
                case Horns.None:
                    sHorns = "";
                    break;
                case Horns.Small:
                    sHorns = " with small horns";
                    break;
                case Horns.Big:
                    sHorns = " with big horns";
                    break;
            }

            return m_iHeadsCount.ToString() + " " + sNeck + " and " + sForm + (m_iHeadsCount == 1 ? "head" : "heads") + sHorns;
        }

        /// <summary>
        /// 1 human head on short neck, no horns
        /// </summary>
        public static HeadGenetix Human
        {
            get { return new HeadGenetix(1, NeckLength.Short, HeadForm.Human, Horns.None); }
        }

        /// <summary>
        /// 1 human head on short neck, small horns
        /// </summary>
        public static HeadGenetix Demon
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
        

        public int m_iHeadsCount = 1;

        public NeckLength m_eNeckLength = NeckLength.Short;

        public HeadForm m_eHeadForm = HeadForm.Human;

        public Horns m_eHorns = Horns.None; 

        public bool IsIdentical(GenetixBase pOther)
        {
            HeadGenetix pAnother = pOther as HeadGenetix;

            if (pAnother == null)
                return false;

            return m_iHeadsCount == pAnother.m_iHeadsCount &&
                m_eNeckLength == pAnother.m_eNeckLength &&
                m_eHeadForm == pAnother.m_eHeadForm &&
                m_eHorns == pAnother.m_eHorns;
        }
        
        public HeadGenetix()
        { }

        public HeadGenetix(HeadGenetix pPredcessor)
        {
            m_iHeadsCount = pPredcessor.m_iHeadsCount;
            m_eNeckLength = pPredcessor.m_eNeckLength;
            m_eHeadForm = pPredcessor.m_eHeadForm;
            m_eHorns = pPredcessor.m_eHorns;
        }

        public HeadGenetix(int iHeadsCount, NeckLength eNeckLength, HeadForm eHeadForm, Horns eHorns)
        {
            m_iHeadsCount = iHeadsCount;
            m_eNeckLength = eNeckLength;
            m_eHeadForm = eHeadForm;
            m_eHorns = eHorns;
        }

        #region GenetixBase Members

        public GenetixBase MutateRace()
        {
            if (Rnd.OneChanceFrom(5))
            {
                HeadGenetix pMutant = new HeadGenetix(this);

                //pMutant.m_iHeadsCount = 1 + (int)Math.Pow(Rnd.Get(14), 3) / 1000;

                if (Rnd.OneChanceFrom(2))
                {
                    if (pMutant.m_iHeadsCount == 1 &&
                        pMutant.m_eNeckLength != NeckLength.Long &&
                        pMutant.m_eNeckLength != NeckLength.ExtraLong)
                    {
                        int iChance = (int)Math.Pow(Rnd.Get(15), 3) / 1000;
                        switch (iChance)
                        {
                            case 0:
                                pMutant.m_eNeckLength = NeckLength.Short;
                                break;
                            case 1:
                                pMutant.m_eNeckLength = NeckLength.ShortRotary;
                                break;
                            case 2:
                                pMutant.m_eNeckLength = NeckLength.Long;
                                break;
                            case 3:
                                pMutant.m_eNeckLength = NeckLength.None;
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
                                pMutant.m_eNeckLength = NeckLength.Long;
                                break;
                            case 1:
                                pMutant.m_eNeckLength = NeckLength.ExtraLong;
                                break;
                            case 2:
                                pMutant.m_eNeckLength = NeckLength.Short;
                                break;
                            case 3:
                                pMutant.m_eNeckLength = NeckLength.ShortRotary;
                                break;
                                //case 4:
                                //    pMutant.m_eNeckLength = NeckLength.None;
                                //    break;
                        }
                    }
                }

                //pMutant.m_eHeadForm = (HeadForm)Rnd.Get(typeof(HeadForm));

                switch (m_eHorns)
                {
                    case Horns.None:
                        if (Rnd.OneChanceFrom(2))
                            m_eHorns = Rnd.OneChanceFrom(5) ? Horns.Big : Horns.Small;
                        break;
                    case Horns.Small:
                        m_eHorns = Rnd.OneChanceFrom(2) ? Horns.None : Horns.Big;
                        break;
                    case Horns.Big:
                        m_eHorns = Rnd.OneChanceFrom(5) ? Horns.None : Horns.Small;
                        break;
                }

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public GenetixBase MutateGender()
        {
            if (Rnd.OneChanceFrom(5))
            {
                HeadGenetix pMutant = new HeadGenetix(this);

                switch (m_eHorns)
                {
                    case Horns.None:
                        if (Rnd.OneChanceFrom(2))
                            pMutant.m_eHorns = Rnd.OneChanceFrom(5) ? Horns.Big : Horns.Small;
                        break;
                    case Horns.Small:
                        pMutant.m_eHorns = Rnd.OneChanceFrom(2) ? Horns.None : Horns.Big;
                        break;
                    case Horns.Big:
                        pMutant.m_eHorns = Rnd.OneChanceFrom(5) ? Horns.None : Horns.Small;
                        break;
                }

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public GenetixBase MutateNation()
        {
            if (Rnd.OneChanceFrom(10))
            {
                HeadGenetix pMutant = new HeadGenetix(this);

                switch (m_eHorns)
                {
                    case Horns.None:
                        if (Rnd.OneChanceFrom(2))
                            pMutant.m_eHorns = Rnd.OneChanceFrom(5) ? Horns.Big : Horns.Small;
                        break;
                    case Horns.Small:
                        pMutant.m_eHorns = Rnd.OneChanceFrom(2) ? Horns.None : Horns.Big;
                        break;
                    case Horns.Big:
                        pMutant.m_eHorns = Rnd.OneChanceFrom(5) ? Horns.None : Horns.Small;
                        break;
                }

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

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

        #endregion
    }
}
