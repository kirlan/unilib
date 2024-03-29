﻿using System;
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
        /// <summary>
        /// 6 insectoid legs
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            bool bExtended = false;

            string sLegs = "?";
            switch (m_eLegsCount)
            {
                case LegsCount.NoneHover:
                    sLegs = "no legs (but able to hover above earth)";
                    break;
                case LegsCount.NoneBlob:
                    sLegs = "no legs (but able to grow pseudopods)";
                    break;
                case LegsCount.NoneTail:
                    sLegs = "no legs (and crawls on own belly)";
                    break;
                case LegsCount.Bipedal:
                    sLegs = "2 ";
                    bExtended = true;
                    break;
                case LegsCount.Quadrupedal:
                    sLegs = "4 ";
                    bExtended = true;
                    break;
                case LegsCount.Hexapod:
                    sLegs = "6 ";
                    bExtended = true;
                    break;
                case LegsCount.Octapod:
                    sLegs = "8 ";
                    bExtended = true;
                    break;
            }

            if (bExtended)
            {
                switch (m_eLegsType)
                {
                    case LegsType.Foots:
                        sLegs += "foots";
                        break;
                    case LegsType.Paws:
                        sLegs += "paws";
                        break;
                    case LegsType.Hoofs:
                        sLegs += "hoofed legs";
                        break;
                    case LegsType.Claws:
                        sLegs += "sharp-clawed foots";
                        break;
                    case LegsType.Spidery:
                        sLegs += "insectoid legs";
                        break;
                    case LegsType.Tentacles:
                        sLegs += "muscular tentacles instead of legs";
                        break;
                }
            }

            return sLegs;
        }
        
        public static LegsGenetix Human
        {
            get { return new LegsGenetix(LegsCount.Bipedal, LegsType.Foots); }
        }

        public static LegsGenetix Demon
        {
            get { return new LegsGenetix(LegsCount.Bipedal, LegsType.Hoofs); }
        }

        public static LegsGenetix Bird
        {
            get { return new LegsGenetix(LegsCount.Bipedal, LegsType.Claws); }
        }

        public static LegsGenetix Furry
        {
            get { return new LegsGenetix(LegsCount.Bipedal, LegsType.Paws); }
        }

        public static LegsGenetix Ktulhu
        {
            get { return new LegsGenetix(LegsCount.Octapod, LegsType.Tentacles); }
        }

        public static LegsGenetix Horse
        {
            get { return new LegsGenetix(LegsCount.Quadrupedal, LegsType.Hoofs); }
        }

        public static LegsGenetix Beast
        {
            get { return new LegsGenetix(LegsCount.Quadrupedal, LegsType.Paws); }
        }

        public static LegsGenetix Insect6
        {
            get { return new LegsGenetix(LegsCount.Hexapod, LegsType.Spidery); }
        }

        public static LegsGenetix Insect8
        {
            get { return new LegsGenetix(LegsCount.Octapod, LegsType.Spidery); }
        }

        public static LegsGenetix Ifrit
        {
            get { return new LegsGenetix(LegsCount.NoneHover, LegsType.Tentacles); }
        }

        public static LegsGenetix Snake
        {
            get { return new LegsGenetix(LegsCount.NoneTail, LegsType.Tentacles); }
        }


        public LegsCount m_eLegsCount = LegsCount.Bipedal;

        public LegsType m_eLegsType = LegsType.Foots;

        public bool IsIdentical(GenetixBase pOther)
        {
            LegsGenetix pAnother = pOther as LegsGenetix;

            if (pAnother == null)
                return false;

            return m_eLegsCount == pAnother.m_eLegsCount &&
                m_eLegsType == pAnother.m_eLegsType;
        }
        
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

                int iChance = 0;

                switch (pMutant.m_eLegsCount)
                {
                    case LegsCount.NoneHover:
                        {
                            int[] aChances = new int[] { 8, 1, 4, 0, 0, 0, 0 };
                            iChance = Rnd.ChooseOne(aChances, 1);
                        }
                        break;
                    case LegsCount.NoneBlob:
                        {
                            int[] aChances = new int[] { 2, 8, 4, 0, 0, 0, 0 };
                            iChance = Rnd.ChooseOne(aChances, 1);
                        }
                        break;
                    case LegsCount.NoneTail:
                        {
                            int[] aChances = new int[] { 2, 2, 8, 0, 0, 0, 0 };
                            iChance = Rnd.ChooseOne(aChances, 1);
                        }
                        break;
                    case LegsCount.Bipedal:
                        {
                            int[] aChances = new int[] { 0, 0, 0, 8, 0, 0, 0 };
                            iChance = Rnd.ChooseOne(aChances, 1);
                        }
                        break;
                    case LegsCount.Quadrupedal:
                        {
                            int[] aChances = new int[] { 0, 0, 0, 0, 8, 2, 2 };
                            iChance = Rnd.ChooseOne(aChances, 1);
                        }
                        break;
                    case LegsCount.Hexapod:
                        {
                            int[] aChances = new int[] { 0, 0, 0, 0, 0, 8, 4 };
                            iChance = Rnd.ChooseOne(aChances, 1);
                        }
                        break;
                    case LegsCount.Octapod:
                        {
                            int[] aChances = new int[] { 0, 0, 0, 0, 0, 4, 8 };
                            iChance = Rnd.ChooseOne(aChances, 1);
                        }
                        break;
                }
                
                switch (iChance)
                {
                    case 0:
                        pMutant.m_eLegsCount = LegsCount.NoneHover;
                        break;
                    case 1:
                        pMutant.m_eLegsCount = LegsCount.NoneBlob;
                        break;
                    case 2:
                        pMutant.m_eLegsCount = LegsCount.NoneTail;
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

                //iChance = -1;

                //if (pMutant.m_eLegsCount == LegsCount.Bipedal)
                //{
                //    int[] aChances = new int[] { 4, 8, 2, 2, 0, 0 };
                //    if (pMutant.m_eLegsType == LegsType.Claws)
                //        aChances = new int[] { 0, 0, 2, 8, 0, 0 };
                //    iChance = Rnd.ChooseOne(aChances, 1);
                //}

                //if (pMutant.m_eLegsCount == LegsCount.Quadrupedal)
                //{
                //    int[] aChances = new int[] { 8, 0, 8, 2, 1, 1 };
                //    if (pMutant.m_eLegsType == LegsType.Claws)
                //        aChances = new int[] { 0, 0, 4, 8, 1, 0 };
                //    iChance = Rnd.ChooseOne(aChances, 1);
                //}

                //if (pMutant.m_eLegsCount == LegsCount.Hexapod ||
                //    pMutant.m_eLegsCount == LegsCount.Octapod)
                //{
                //    int[] aChances = new int[] { 1, 0, 1, 1, 4, 2 };
                //    if (pMutant.m_eLegsType == LegsType.Spidery)
                //        aChances = new int[] { 0, 0, 0, 0, 4, 0 };
                //    iChance = Rnd.ChooseOne(aChances, 1);
                //}

                ////если это безногая раса, то iChance останется = 0, но и чёрт с ним - ног-то всё-равно нет

                //switch (iChance)
                //{
                //    case 0:
                //        pMutant.m_eLegsType = LegsType.Hoofs;
                //        break;
                //    case 1:
                //        pMutant.m_eLegsType = LegsType.Foots;
                //        break;
                //    case 2:
                //        pMutant.m_eLegsType = LegsType.Paws;
                //        break;
                //    case 3:
                //        pMutant.m_eLegsType = LegsType.Claws;
                //        break;
                //    case 4:
                //        pMutant.m_eLegsType = LegsType.Spidery;
                //        break;
                //    case 5:
                //        pMutant.m_eLegsType = LegsType.Tentacles;
                //        break;
                //}

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
