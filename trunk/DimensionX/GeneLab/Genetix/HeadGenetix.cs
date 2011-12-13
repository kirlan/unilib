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

    public class HeadGenetix: GenetixBase
    {
        public int m_iHeadsCount = 1;

        public NeckLength m_eNeckLength = NeckLength.Short;

        public HeadForm m_eHeadForm = HeadForm.Human;

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
                bool bMutation = false;

                HeadGenetix pMutant = new HeadGenetix(this);

                pMutant.m_iHeadsCount = 1 + (int)Math.Pow(Rnd.Get(14), 3) / 1000;
                if (pMutant.m_iHeadsCount != m_iHeadsCount)
                    bMutation = true;
                    
                if (pMutant.m_iHeadsCount == 1 &&
                    pMutant.m_eNeckLength != NeckLength.Long &&
                    pMutant.m_eNeckLength != NeckLength.ExtraLong)
                {
                    int iChance = (int)Math.Pow(Rnd.Get(17), 3) / 1000;
                    switch (iChance)
                    {
                        case 0:
                            pMutant.m_eNeckLength = NeckLength.Short;
                            break;
                        case 1:
                            pMutant.m_eNeckLength = NeckLength.None;
                            break;
                        case 2:
                            pMutant.m_eNeckLength = NeckLength.ShortRotary;
                            break;
                        case 3:
                            pMutant.m_eNeckLength = NeckLength.Long;
                            break;
                        case 4:
                            pMutant.m_eNeckLength = NeckLength.ExtraLong;
                            break;
                    }
                }
                else
                {
                    int iChance = (int)Math.Pow(Rnd.Get(17), 3) / 1000;
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
                        case 4:
                            pMutant.m_eNeckLength = NeckLength.None;
                            break;
                    }
                }
                if (pMutant.m_eNeckLength != m_eNeckLength)
                    bMutation = true;

                pMutant.m_eHeadForm = (HeadForm)Rnd.Get(typeof(HeadForm));
                if (pMutant.m_eHeadForm != m_eHeadForm)
                    bMutation = true;

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

        #endregion
    }
}
