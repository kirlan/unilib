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
                LegsGenetix pMutant = new LegsGenetix(this);

                pMutant.m_eLegsCount = (LegsCount)Rnd.Get(typeof(LegsCount));

                int iChance = 0;
                if (pMutant.m_eLegsCount == LegsCount.Bipedal)
                {
                    int[] aChances = new int[] { 4, 8, 4, 4, 0, 0 };
                    iChance = Rnd.ChooseOne(aChances, 1);
                }

                if (pMutant.m_eLegsCount == LegsCount.Quadrupedal)
                {
                    int[] aChances = new int[] { 8, 4, 8, 2, 1, 1 };
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

                return pMutant;
            }

            return this;
        }

        public GenetixBase MutateNation()
        {
            if (Rnd.OneChanceFrom(20))
            {
                LegsGenetix pMutant = new LegsGenetix(this);

                int iChance = 0;
                switch (pMutant.m_eLegsCount)
                {
                    case LegsCount.NoneTail:
                        {
                            int[] aChances = new int[] { 0, 2, 1, 4, 0, 2, 1 };
                            iChance = Rnd.ChooseOne(aChances, 1);
                        }
                        break;
                    case LegsCount.NoneBlob:
                        {
                            int[] aChances = new int[] { 2, 0, 1, 4, 0, 2, 1 };
                            iChance = Rnd.ChooseOne(aChances, 1);
                        }
                        break;
                    case LegsCount.NoneHover:
                        {
                            int[] aChances = new int[] { 4, 2, 0, 4, 0, 2, 1 };
                            iChance = Rnd.ChooseOne(aChances, 1);
                        }
                        break;
                    case LegsCount.Bipedal:
                        {
                            int[] aChances = new int[] { 4, 2, 1, 0, 8, 2, 1 };
                            iChance = Rnd.ChooseOne(aChances, 1);
                        }
                        break;
                    case LegsCount.Quadrupedal:
                        {
                            int[] aChances = new int[] { 0, 0, 0, 0, 0, 4, 2 };
                            iChance = Rnd.ChooseOne(aChances, 1);
                        }
                        break;
                    case LegsCount.Hexapod:
                        {
                            int[] aChances = new int[] { 0, 1, 1, 0, 1, 0, 2 };
                            iChance = Rnd.ChooseOne(aChances, 1);
                        }
                        break;
                    case LegsCount.Octapod:
                        {
                            int[] aChances = new int[] { 0, 1, 1, 0, 1, 2, 0 };
                            iChance = Rnd.ChooseOne(aChances, 1);
                        }
                        break;
                }
                switch (iChance)
                {
                    case 0:
                        pMutant.m_eLegsCount = LegsCount.NoneTail;
                        break;
                    case 1:
                        pMutant.m_eLegsCount = LegsCount.NoneBlob;
                        break;
                    case 2:
                        pMutant.m_eLegsCount = LegsCount.NoneHover;
                        break;
                    case 3:
                        pMutant.m_eLegsCount = LegsCount.Bipedal;
                        break;
                    case 4:
                        pMutant.m_eLegsCount = LegsCount.Quadrupedal;
                        break;
                    case 5:
                        pMutant.m_eLegsCount = LegsCount.Hexapod;
                        break;
                    case 6:
                        pMutant.m_eLegsCount = LegsCount.Octapod;
                        break;
                }

                switch (pMutant.m_eLegsType)
                {
                    case LegsType.Hoofs:
                        iChance = 0;
                        break;
                    case LegsType.Foots:
                        iChance = 1;
                        break;
                    case LegsType.Paws:
                        iChance = 2;
                        break;
                    case LegsType.Claws:
                        iChance = 3;
                        break;
                    case LegsType.Spidery:
                        iChance = 4;
                        break;
                    case LegsType.Tentacles:
                        iChance = 5;
                        break;
                }

                if (pMutant.m_eLegsCount == LegsCount.Bipedal)
                {
                    int[] aChances = new int[] { 4, 8, 4, 4, 0, 0 };
                    if(aChances[iChance] == 0)
                        iChance = Rnd.ChooseOne(aChances, 1);
                }

                if (pMutant.m_eLegsCount == LegsCount.Quadrupedal)
                {
                    int[] aChances = new int[] { 8, 4, 8, 2, 1, 1 };
                    if (aChances[iChance] == 0)
                        iChance = Rnd.ChooseOne(aChances, 1);
                }

                if (pMutant.m_eLegsCount == LegsCount.Hexapod ||
                    pMutant.m_eLegsCount == LegsCount.Octapod)
                {
                    int[] aChances = new int[] { 1, 0, 1, 1, 4, 2 };
                    if (aChances[iChance] == 0)
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
    }
}
