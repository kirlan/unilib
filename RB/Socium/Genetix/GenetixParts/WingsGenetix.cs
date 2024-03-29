﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace RB.Genetix.GenetixParts
{
    public enum WingsCount
    { 
        /// <summary>
        /// безкрылый
        /// </summary>
        None,
        /// <summary>
        /// двукрылый - как птицы
        /// </summary>
        Dipterous,
        /// <summary>
        /// четырёхкрылый - как стрекозы
        /// </summary>
        Quadrupterous
    }

    public enum WingsType
    { 
        /// <summary>
        /// оперённые крылья как у птиц
        /// </summary>
        Feathered,
        /// <summary>
        /// кожистые крылья, как у летучих мышей
        /// </summary>
        Leather,
        /// <summary>
        /// тонкие, жёсткие, полупрозрачные крылья, как у насекомых
        /// </summary>
        Insectoid,
        /// <summary>
        /// и не крылья вовсе, а щупальца, растущие из спины - вместо крыльев
        /// </summary>
        Tentacles
    }

    public enum WingsForce
    {
        /// <summary>
        /// никакой пользы с этих крыльев
        /// </summary>
        None,
        /// <summary>
        /// можно только планировать с высоких мест
        /// </summary>
        Gliding,
        /// <summary>
        /// способность к полноценному полёту
        /// </summary>
        Flying
    }

    public class WingsGenetix: GenetixBase
    {
        /// <summary>
        /// 2 pairs of strong leather wings
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            if (m_eWingsCount == WingsCount.None)
                return "";

            string sWings = "";
            switch (m_eWingsForce)
            {
                case WingsForce.None:
                    sWings = "small";
                    break;
                case WingsForce.Gliding:
                    sWings = "weak";
                    break;
                case WingsForce.Flying:
                    sWings = "strong";
                    break;
            }

            switch (m_eWingsType)
            {
                case WingsType.Feathered:
                    sWings += " feathered wings";
                    break;
                case WingsType.Leather:
                    sWings += " leather wings";
                    break;
                case WingsType.Insectoid:
                    sWings += " insectoid wings";
                    break;
                case WingsType.Tentacles:
                    sWings = "long tentacles on their back";
                    break;
            }

            return (m_eWingsCount == WingsCount.Dipterous ? "pair" : "2 pairs") + " of " + sWings;
        }
        
        public static WingsGenetix None
        {
            get { return new WingsGenetix(WingsCount.None, WingsType.Leather, WingsForce.None); }
        }

        public static WingsGenetix Bird
        {
            get { return new WingsGenetix(WingsCount.Dipterous, WingsType.Feathered, WingsForce.Flying); }
        }

        public static WingsGenetix Seraph
        {
            get { return new WingsGenetix(WingsCount.Quadrupterous, WingsType.Feathered, WingsForce.Flying); }
        }

        public static WingsGenetix Bat
        {
            get { return new WingsGenetix(WingsCount.Dipterous, WingsType.Leather, WingsForce.Flying); }
        }

        public static WingsGenetix Insect
        {
            get { return new WingsGenetix(WingsCount.Quadrupterous, WingsType.Insectoid, WingsForce.Flying); }
        }

        public static WingsGenetix Tentacles
        {
            get { return new WingsGenetix(WingsCount.Quadrupterous, WingsType.Tentacles, WingsForce.None); }
        }

        public WingsCount m_eWingsCount = WingsCount.Dipterous;

        public WingsType m_eWingsType = WingsType.Feathered;

        public WingsForce m_eWingsForce = WingsForce.Flying;
        
        public bool IsIdentical(GenetixBase pOther)
        {
            WingsGenetix pAnother = pOther as WingsGenetix;

            if (pAnother == null)
                return false;

            if (m_eWingsCount == WingsCount.None &&
                pAnother.m_eWingsCount == WingsCount.None)
                return true;

            return m_eWingsCount == pAnother.m_eWingsCount &&
                m_eWingsType == pAnother.m_eWingsType &&
                m_eWingsForce == pAnother.m_eWingsForce;
        }

        public WingsGenetix()
        { }

        public WingsGenetix(WingsGenetix pPredcessor)
        {
            m_eWingsCount = pPredcessor.m_eWingsCount;
            m_eWingsType = pPredcessor.m_eWingsType;
            m_eWingsForce = pPredcessor.m_eWingsForce;
        }

        public WingsGenetix(WingsCount eArmsCount, WingsType eArmsType, WingsForce eWingsForce)
        {
            m_eWingsCount = eArmsCount;
            m_eWingsType = eArmsType;
            m_eWingsForce = eWingsForce;

            if (m_eWingsType == WingsType.Tentacles || m_eWingsCount == WingsCount.None)
                m_eWingsForce = WingsForce.None;
        }
        
        public GenetixBase MutateRace()
        {
            if (Rnd.OneChanceFrom(10))
            {
                WingsGenetix pMutant = new WingsGenetix(this);

                if (Rnd.OneChanceFrom(2))
                    pMutant.m_eWingsCount = (WingsCount)Rnd.Get(typeof(WingsCount));

                int iChance = Rnd.Get(4);
                if (m_eWingsForce != WingsForce.None)
                    iChance = Rnd.Get(3);
                    
                switch (iChance)
                {
                    case 0:
                        pMutant.m_eWingsType = WingsType.Feathered;
                        break;
                    case 1:
                        pMutant.m_eWingsType = WingsType.Leather;
                        break;
                    case 2:
                        pMutant.m_eWingsType = WingsType.Insectoid;
                        break;
                    case 3:
                        pMutant.m_eWingsType = WingsType.Tentacles;
                        break;
                }

                pMutant.m_eWingsForce = (WingsForce)Rnd.Get(typeof(WingsForce));

                if (pMutant.m_eWingsType == WingsType.Tentacles || pMutant.m_eWingsCount == WingsCount.None)
                    pMutant.m_eWingsForce = WingsForce.None;

                if(!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public GenetixBase MutateNation()
        {
            if (Rnd.OneChanceFrom(20))
            {
                WingsGenetix pMutant = new WingsGenetix(this);

                if (Rnd.OneChanceFrom(2))
                {
                    if (pMutant.m_eWingsCount == WingsCount.Dipterous)
                        pMutant.m_eWingsCount = WingsCount.Quadrupterous;
                    if (pMutant.m_eWingsCount == WingsCount.Quadrupterous)
                        pMutant.m_eWingsCount = WingsCount.Dipterous;
                }

                if (Rnd.OneChanceFrom(2))
                {
                    if (pMutant.m_eWingsType == WingsType.Feathered)
                        pMutant.m_eWingsType = WingsType.Leather;
                    if (pMutant.m_eWingsType == WingsType.Leather)
                        pMutant.m_eWingsType = WingsType.Feathered;
                }

                pMutant.m_eWingsForce = (WingsForce)Rnd.Get(typeof(WingsForce));

                if (pMutant.m_eWingsType == WingsType.Tentacles || pMutant.m_eWingsCount == WingsCount.None)
                    pMutant.m_eWingsForce = WingsForce.None;

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
            if (Rnd.OneChanceFrom(50))
            {
                WingsGenetix pMutant = new WingsGenetix(this);

                pMutant.m_eWingsForce = (WingsForce)Rnd.Get(typeof(WingsForce));

                if (pMutant.m_eWingsType == WingsType.Tentacles || pMutant.m_eWingsCount == WingsCount.None)
                    pMutant.m_eWingsForce = WingsForce.None;

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }
    }
}
