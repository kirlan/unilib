using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace GeneLab.Genetix
{
    public enum LegsCount
    {
        /// <summary>
        /// без ног, но с хвостом - как змеи/наги
        /// </summary>
        NoneTail,
        /// <summary>
        /// без ног, но на слизистой подушке - как слизняки/улитки
        /// </summary>
        NoneBlob,
        /// <summary>
        /// без ног, но может парить в воздухе - как джинны
        /// </summary>
        NoneHover,
        /// <summary>
        /// две ноги, как у людей
        /// </summary>
        Bipedal,
        /// <summary>
        /// четыре ноги, как у животных
        /// </summary>
        Quadrupedal,
        /// <summary>
        /// шесть ног, как у жуков
        /// </summary>
        Hexapod,
        /// <summary>
        /// восемь ног, как у пауков
        /// </summary>
        Octapod
    }

    public enum LegsType
    {
        /// <summary>
        /// копыта, как у травоядных или у сатиров
        /// </summary>
        Hoofs,
        /// <summary>
        /// ступня с опорной пяткой, как у человека
        /// </summary>
        Foots,
        /// <summary>
        /// лапа с опорой только на подушечку перед пальцами, как у животных
        /// </summary>
        Paws,
        /// <summary>
        /// птичья лапа - опора на несколько растопыренных пальцев с когтями на концах
        /// </summary>
        Claws,
        /// <summary>
        /// просто коготь на конце ноги, как у насекомых
        /// </summary>
        Spidery,
        /// <summary>
        /// щупальца - как у осьминога
        /// </summary>
        Tentacles
    }

    public class LegsGenetix : GenetixBase
    {
        public LegsCount m_eLegsCount = LegsCount.Bipedal;

        public LegsType m_eLegsType = LegsType.Foots;

        public LegsGenetix()
        { }

        public LegsGenetix(LegsGenetix pPredcessor)
        {
            m_eLegsCount = pPredcessor.m_eLegsCount;
            m_eLegsType = pPredcessor.m_eLegsType;
        }

        public LegsGenetix(LegsCount eLegsCount, LegsType eLegsType)
        {
            m_eLegsCount = eLegsCount;
            m_eLegsType = eLegsType;
        }

        public GenetixBase MutateRace()
        {
            if (Rnd.OneChanceFrom(10))
            {
                bool bMutation = false;

                LegsGenetix pMutant = new LegsGenetix(this);

                if (Rnd.OneChanceFrom(2))
                    pMutant.m_eLegsCount = (LegsCount)Rnd.Get(typeof(LegsCount));

                if (pMutant.m_eLegsCount != m_eLegsCount)
                    bMutation = true;

                int iChance = 0;
                if (pMutant.m_eLegsCount == LegsCount.Bipedal)
                {
                    int[] aChances = new int[] { 4, 8, 4, 4, 0, 0 };
                    iChance = Rnd.ChooseOne(aChances, 1);
                }

                if (pMutant.m_eLegsCount == LegsCount.Quadrupedal)
                {
                    int[] aChances = new int[] { 8, 0, 8, 2, 1, 1 };
                    iChance = Rnd.ChooseOne(aChances, 1);
                }

                if (pMutant.m_eLegsCount == LegsCount.Hexapod ||
                    pMutant.m_eLegsCount == LegsCount.Octapod)
                {
                    int[] aChances = new int[] { 1, 0, 1, 1, 4, 2 };
                    iChance = Rnd.ChooseOne(aChances, 1);
                }

                switch (iChance)
                {
                    case 0:
                        pMutant.m_eLegsType = LegsType.Hoofs;
                        break;
                    case 1:
                        pMutant.m_eLegsType = LegsType.Foots;
                        break;
                    case 2:
                        pMutant.m_eLegsType = LegsType.Paws;
                        break;
                    case 3:
                        pMutant.m_eLegsType = LegsType.Claws;
                        break;
                    case 4:
                        pMutant.m_eLegsType = LegsType.Spidery;
                        break;
                    case 5:
                        pMutant.m_eLegsType = LegsType.Tentacles;
                        break;
                }

                if (pMutant.m_eLegsType != m_eLegsType)
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
    }
}
