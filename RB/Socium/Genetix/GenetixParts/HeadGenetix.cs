using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace RB.Genetix.GenetixParts
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
                    sNeck = "without neck";
                    break;
                case NeckLength.Short:
                    sNeck = "on short neck";
                    break;
                case NeckLength.ShortRotary:
                    sNeck = "on short pivoted neck";
                    break;
                case NeckLength.Long:
                    sNeck = "on long neck";
                    break;
                case NeckLength.ExtraLong:
                    sNeck = "on long, flexible neck";
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
                    sForm = "";
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

            return m_iHeadsCount.ToString() + " " + sForm + (m_iHeadsCount == 1 ? "head" : "heads") + " " + sNeck;
        }
        
        public static HeadGenetix Human
        {
            get { return new HeadGenetix(1, NeckLength.Short, HeadForm.Human); }
        }

        public static HeadGenetix Sectoid
        {
            get { return new HeadGenetix(1, NeckLength.Short, HeadForm.IncreasedForehead); }
        }

        public static HeadGenetix Pitecantrop
        {
            get { return new HeadGenetix(1, NeckLength.Short, HeadForm.Flat); }
        }

        public static HeadGenetix Insect
        {
            get { return new HeadGenetix(1, NeckLength.ShortRotary, HeadForm.Hammer); }
        }
        

        public int m_iHeadsCount = 1;

        public NeckLength m_eNeckLength = NeckLength.Short;

        public HeadForm m_eHeadForm = HeadForm.Human;

        public bool IsIdentical(GenetixBase pOther)
        {
            HeadGenetix pAnother = pOther as HeadGenetix;

            if (pAnother == null)
                return false;

            return m_iHeadsCount == pAnother.m_iHeadsCount &&
                m_eNeckLength == pAnother.m_eNeckLength &&
                m_eHeadForm == pAnother.m_eHeadForm;
        }
        
        public HeadGenetix()
        { }

        public HeadGenetix(HeadGenetix pPredcessor)
        {
            m_iHeadsCount = pPredcessor.m_iHeadsCount;
            m_eNeckLength = pPredcessor.m_eNeckLength;
            m_eHeadForm = pPredcessor.m_eHeadForm;
        }

        public HeadGenetix(int iHeadsCount, NeckLength eNeckLength, HeadForm eHeadForm)
        {
            m_iHeadsCount = iHeadsCount;
            m_eNeckLength = eNeckLength;
            m_eHeadForm = eHeadForm;
        }
        
        #region GenetixBase Members

        public GenetixBase MutateRace()
        {
            if (Rnd.OneChanceFrom(10))
            {
                HeadGenetix pMutant = new HeadGenetix(this);

                //pMutant.m_iHeadsCount = 1 + (int)Math.Pow(Rnd.Get(14), 3) / 1000;
                    
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

                //pMutant.m_eHeadForm = (HeadForm)Rnd.Get(typeof(HeadForm));

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

        #endregion
    }
}
