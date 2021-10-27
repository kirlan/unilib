using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace GeneLab.Genetix
{
    public enum BodySize
    {
        /// <summary>
        /// очень маленький - не меньше мыши, не больше домашнего кота
        /// </summary>
        Tini,
        /// <summary>
        /// маленький - человеческий ребёнок 5-15 лет (а так же гном, хоббит или средняя собака)
        /// </summary>
        Small,
        /// <summary>
        /// средний - средний человеческий рост
        /// </summary>
        Normal,
        /// <summary>
        /// высокий - высокий человек (не больше 2,5 м)
        /// </summary>
        Big,
        /// <summary>
        /// гигантский - в несколько раз больше самого высокого человека.
        /// </summary>
        Giant
    }

    public enum BodyBuild
    {
        /// <summary>
        /// стройный, худой, ловкий
        /// </summary>
        Skinny,
        /// <summary>
        /// обычное телосложение - ничего выдяляющегося
        /// </summary>
        Slim,
        /// <summary>
        /// мускулистый, сильный, быстрый
        /// </summary>
        Muscular,
        /// <summary>
        /// тучный, слабый, неповоротливый
        /// </summary>
        Fat
    }

    public enum Gender
    {
        Male,
        Female
    }


    public class BodyGenetix: GenetixBase
    {
        /// <summary>
        /// have small and agile bodies 
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            return GetDescription(null, true);
        }

        /// <summary>
        /// 'have tall agile bodies'
        /// or, if gender specified - just
        /// 'tall agile woman'
        /// </summary>
        /// <returns></returns>
        public string GetDescription(Gender? eGender, bool bFull)
        {
            string sSize = "";
            switch (m_eBodySize)
            {
                case BodySize.Tini:
                    sSize = "tiny";
                    break;
                case BodySize.Small:
                    sSize = "short";
                    break;
                case BodySize.Normal:
                    if (bFull)
                        sSize = "average-sized";
                    break;
                case BodySize.Big:
                    sSize = "tall";
                    break;
                case BodySize.Giant:
                    sSize = "giant";
                    break;
            }

            string sComplexion = "";
            switch (m_eBodyBuild)
            {
                case BodyBuild.Skinny:
                    sComplexion = "nimble";
                    break;
                case BodyBuild.Slim:
                    if (bFull)
                        sComplexion = "proportional";
                    break;
                case BodyBuild.Muscular:
                    sComplexion = "muscular";
                    break;
                case BodyBuild.Fat:
                    sComplexion = "fat";
                    break;
            }

            if (sSize != "" && sComplexion != "")
                sSize += " ";

            string sResult = sSize + sComplexion;
            if (bFull)
            {
                if (eGender.HasValue)
                    return sResult + (eGender == Gender.Male ? " man" : " woman") + ".";
                else
                    return "have " + sResult + " bodies";
            }
            else
            {
                if (eGender.HasValue)
                {
                    if (sResult != "")
                        sResult += " ";
                    return sResult + (eGender == Gender.Male ? "man" : "woman");
                }
                else
                {
                    if (sResult != "")
                        sResult += " ";

                    if (sResult != "")
                        return "have " + sResult + "bodies";
                    else
                        return "";
                }
            }
        }

        /// <summary>
        /// average size, slim
        /// </summary>
        public static BodyGenetix Human
        {
            get { return new BodyGenetix(BodySize.Normal, BodyBuild.Slim); }
        }

        /// <summary>
        /// average size, muscular
        /// </summary>
        public static BodyGenetix Barbarian
        {
            get { return new BodyGenetix(BodySize.Normal, BodyBuild.Muscular); }
        }

        /// <summary>
        /// small size, skinny
        /// </summary>
        public static BodyGenetix Goblin
        {
            get { return new BodyGenetix(BodySize.Small, BodyBuild.Skinny); }
        }

        /// <summary>
        /// small size, muscular
        /// </summary>
        public static BodyGenetix Dwarf
        {
            get { return new BodyGenetix(BodySize.Small, BodyBuild.Muscular); }
        }

        /// <summary>
        /// small size, fat
        /// </summary>
        public static BodyGenetix Hobbit
        {
            get { return new BodyGenetix(BodySize.Small, BodyBuild.Fat); }
        }

        /// <summary>
        /// average size, skinny
        /// </summary>
        public static BodyGenetix Elf
        {
            get { return new BodyGenetix(BodySize.Normal, BodyBuild.Skinny); }
        }

        /// <summary>
        /// tiny size, skinny
        /// </summary>
        public static BodyGenetix Pixie
        {
            get { return new BodyGenetix(BodySize.Tini, BodyBuild.Skinny); }
        }

        /// <summary>
        /// giant size, muscular
        /// </summary>
        public static BodyGenetix Giant
        {
            get { return new BodyGenetix(BodySize.Giant, BodyBuild.Muscular); }
        }


        public BodySize m_eBodySize = BodySize.Normal;

        public BodyBuild m_eBodyBuild = BodyBuild.Slim;

        public bool IsIdentical(GenetixBase pOther)
        {
            BodyGenetix pAnother = pOther as BodyGenetix;

            if (pAnother == null)
                return false;

            return m_eBodySize == pAnother.m_eBodySize && 
                m_eBodyBuild == pAnother.m_eBodyBuild;
        }

        public BodyGenetix()
        { }

        public BodyGenetix(BodyGenetix pPredcessor)
        {
            m_eBodySize = pPredcessor.m_eBodySize;
            m_eBodyBuild = pPredcessor.m_eBodyBuild;
        }

        public BodyGenetix(BodySize eBodySize, BodyBuild eBodyBuild)
        {
            m_eBodySize = eBodySize;
            m_eBodyBuild = eBodyBuild;
        }

        #region GenetixBase Members

        public GenetixBase MutateRace()
        {
            if (Rnd.OneChanceFrom(10))
            {
                BodyGenetix pMutant = new BodyGenetix(this);

                if (Rnd.OneChanceFrom(2))
                {
                    switch (m_eBodySize)
                    {
                        case BodySize.Tini:
                            pMutant.m_eBodySize = BodySize.Small;
                            break;
                        case BodySize.Small:
                            pMutant.m_eBodySize = Rnd.OneChanceFrom(2) ? BodySize.Tini : BodySize.Normal;
                            break;
                        case BodySize.Normal:
                            pMutant.m_eBodySize = Rnd.OneChanceFrom(2) ? BodySize.Small : BodySize.Big;
                            break;
                        case BodySize.Big:
                            pMutant.m_eBodySize = Rnd.OneChanceFrom(2) ? BodySize.Normal : BodySize.Giant;
                            break;
                        case BodySize.Giant:
                            pMutant.m_eBodySize = BodySize.Big;
                            break;
                    }
                }

                if (Rnd.OneChanceFrom(2))
                {
                    switch (m_eBodyBuild)
                    {
                        case BodyBuild.Skinny:
                            pMutant.m_eBodyBuild = BodyBuild.Slim;
                            break;
                        case BodyBuild.Slim:
                            pMutant.m_eBodyBuild = (BodyBuild)Rnd.Get(typeof(BodyBuild));
                            break;
                        case BodyBuild.Muscular:
                            pMutant.m_eBodyBuild = Rnd.OneChanceFrom(2) ? BodyBuild.Slim : BodyBuild.Fat;
                            break;
                        case BodyBuild.Fat:
                            pMutant.m_eBodyBuild = Rnd.OneChanceFrom(2) ? BodyBuild.Slim : BodyBuild.Muscular;
                            break;
                    }
                }

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public GenetixBase MutateGender()
        {
            if (Rnd.OneChanceFrom(2))
            {
                BodyGenetix pMutant = new BodyGenetix(this);

                if (Rnd.OneChanceFrom(50))
                {
                    switch (pMutant.m_eBodySize)
                    {
                        case BodySize.Tini:
                            pMutant.m_eBodySize = BodySize.Small;
                            break;
                        case BodySize.Small:
                            pMutant.m_eBodySize = Rnd.OneChanceFrom(2) ? BodySize.Tini : BodySize.Normal;
                            break;
                        case BodySize.Normal:
                            pMutant.m_eBodySize = Rnd.OneChanceFrom(2) ? BodySize.Small : BodySize.Big;
                            break;
                        case BodySize.Big:
                            pMutant.m_eBodySize = Rnd.OneChanceFrom(2) ? BodySize.Normal : BodySize.Giant;
                            break;
                        case BodySize.Giant:
                            pMutant.m_eBodySize = BodySize.Big;
                            break;
                    }
                }

                switch (pMutant.m_eBodyBuild)
                {
                    case BodyBuild.Skinny:
                        pMutant.m_eBodyBuild = BodyBuild.Slim;
                        break;
                    case BodyBuild.Slim:
                        pMutant.m_eBodyBuild = (BodyBuild)Rnd.Get(typeof(BodyBuild));
                        break;
                    case BodyBuild.Muscular:
                        pMutant.m_eBodyBuild = Rnd.OneChanceFrom(3) ? BodyBuild.Fat : BodyBuild.Slim;
                        break;
                    case BodyBuild.Fat:
                        pMutant.m_eBodyBuild = Rnd.OneChanceFrom(3) ? BodyBuild.Muscular : BodyBuild.Slim;
                        break;
                }

                if (!pMutant.IsIdentical(this))
                    return pMutant;
            }

            return this;
        }

        public GenetixBase MutateNation()
        {
            if (Rnd.OneChanceFrom(20))
            {
                BodyGenetix pMutant = new BodyGenetix(this);

                switch (pMutant.m_eBodyBuild)
                {
                    case BodyBuild.Skinny:
                        pMutant.m_eBodyBuild = BodyBuild.Slim;
                        break;
                    case BodyBuild.Slim:
                        pMutant.m_eBodyBuild = (BodyBuild)Rnd.Get(typeof(BodyBuild));
                        break;
                    case BodyBuild.Muscular:
                        pMutant.m_eBodyBuild = Rnd.OneChanceFrom(2) ? BodyBuild.Slim : BodyBuild.Fat;
                        break;
                    case BodyBuild.Fat:
                        pMutant.m_eBodyBuild = Rnd.OneChanceFrom(2) ? BodyBuild.Slim : BodyBuild.Muscular;
                        break;
                }

                return pMutant;
            }

            return this;
        }

        public GenetixBase MutateFamily()
        {
            if (Rnd.OneChanceFrom(20))
            {
                BodyGenetix pMutant = new BodyGenetix(this);

                switch (pMutant.m_eBodyBuild)
                {
                    case BodyBuild.Skinny:
                        pMutant.m_eBodyBuild = BodyBuild.Slim;
                        break;
                    case BodyBuild.Slim:
                        pMutant.m_eBodyBuild = (BodyBuild)Rnd.Get(typeof(BodyBuild));
                        break;
                    case BodyBuild.Muscular:
                        pMutant.m_eBodyBuild = BodyBuild.Fat;
                        break;
                    case BodyBuild.Fat:
                        pMutant.m_eBodyBuild = Rnd.OneChanceFrom(2) ? BodyBuild.Slim : BodyBuild.Muscular;
                        break;
                }

                return pMutant;
            }

            return this;
        }

        public GenetixBase MutateIndividual()
        {
            if (Rnd.OneChanceFrom(3))
            {
                BodyGenetix pMutant = new BodyGenetix(this);

                switch (pMutant.m_eBodyBuild)
                {
                    case BodyBuild.Skinny:
                        pMutant.m_eBodyBuild = BodyBuild.Slim;
                        break;
                    case BodyBuild.Slim:
                        pMutant.m_eBodyBuild = (BodyBuild)Rnd.Get(typeof(BodyBuild));
                        break;
                    case BodyBuild.Muscular:
                        pMutant.m_eBodyBuild = BodyBuild.Fat;
                        break;
                    case BodyBuild.Fat:
                        pMutant.m_eBodyBuild = Rnd.OneChanceFrom(2) ? BodyBuild.Slim : BodyBuild.Muscular;
                        break;
                }

                return pMutant;
            }

            return this;
        }

        #endregion
    }
}
