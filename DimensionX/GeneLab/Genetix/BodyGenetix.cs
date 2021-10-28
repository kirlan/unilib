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
        /// small and agile bodies 
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            return GetDescription(null, true);
        }

        /// <summary>
        /// 'tall agile bodies'
        /// or, if gender specified - just
        /// 'tall agile woman'
        /// </summary>
        /// <returns></returns>
        public string GetDescription(Gender? eGender, bool bFull)
        {
            string sSize = "";
            switch (BodySize)
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
            switch (BodyBuild)
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
                    return sResult + " bodies";
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
                        return sResult + "bodies";
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

        /// <summary>
        /// big size, fat
        /// </summary>
        public static BodyGenetix InsectQueen
        {
            get { return new BodyGenetix(BodySize.Big, BodyBuild.Fat); }
        }

        public BodySize BodySize { get; private set; } = BodySize.Normal;

        public BodyBuild BodyBuild { get; private set; } = BodyBuild.Slim;

        public bool IsIdentical(GenetixBase pOther)
        {
            BodyGenetix pAnother = pOther as BodyGenetix;

            if (pAnother == null)
                return false;

            return BodySize == pAnother.BodySize && 
                BodyBuild == pAnother.BodyBuild;
        }

        public BodyGenetix()
        { }

        public BodyGenetix(BodyGenetix pPredcessor)
        {
            BodySize = pPredcessor.BodySize;
            BodyBuild = pPredcessor.BodyBuild;
        }

        public BodyGenetix(BodySize eBodySize, BodyBuild eBodyBuild)
        {
            BodySize = eBodySize;
            BodyBuild = eBodyBuild;
        }

        #region GenetixBase Members

        public GenetixBase MutateRace()
        {
            if (Rnd.OneChanceFrom(10))
            {
                BodyGenetix pMutant = new BodyGenetix(this);

                if (Rnd.OneChanceFrom(2))
                {
                    switch (BodySize)
                    {
                        case BodySize.Tini:
                            pMutant.BodySize = BodySize.Small;
                            break;
                        case BodySize.Small:
                            pMutant.BodySize = Rnd.OneChanceFrom(2) ? BodySize.Tini : BodySize.Normal;
                            break;
                        case BodySize.Normal:
                            pMutant.BodySize = Rnd.OneChanceFrom(2) ? BodySize.Small : BodySize.Big;
                            break;
                        case BodySize.Big:
                            pMutant.BodySize = Rnd.OneChanceFrom(2) ? BodySize.Normal : BodySize.Giant;
                            break;
                        case BodySize.Giant:
                            pMutant.BodySize = BodySize.Big;
                            break;
                    }
                }

                if (Rnd.OneChanceFrom(2))
                {
                    switch (BodyBuild)
                    {
                        case BodyBuild.Skinny:
                            pMutant.BodyBuild = BodyBuild.Slim;
                            break;
                        case BodyBuild.Slim:
                            pMutant.BodyBuild = (BodyBuild)Rnd.Get(typeof(BodyBuild));
                            break;
                        case BodyBuild.Muscular:
                            pMutant.BodyBuild = Rnd.OneChanceFrom(2) ? BodyBuild.Slim : BodyBuild.Fat;
                            break;
                        case BodyBuild.Fat:
                            pMutant.BodyBuild = Rnd.OneChanceFrom(2) ? BodyBuild.Slim : BodyBuild.Muscular;
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
                    switch (pMutant.BodySize)
                    {
                        case BodySize.Tini:
                            pMutant.BodySize = BodySize.Small;
                            break;
                        case BodySize.Small:
                            pMutant.BodySize = Rnd.OneChanceFrom(2) ? BodySize.Tini : BodySize.Normal;
                            break;
                        case BodySize.Normal:
                            pMutant.BodySize = Rnd.OneChanceFrom(2) ? BodySize.Small : BodySize.Big;
                            break;
                        case BodySize.Big:
                            pMutant.BodySize = Rnd.OneChanceFrom(2) ? BodySize.Normal : BodySize.Giant;
                            break;
                        case BodySize.Giant:
                            pMutant.BodySize = BodySize.Big;
                            break;
                    }
                }

                switch (pMutant.BodyBuild)
                {
                    case BodyBuild.Skinny:
                        pMutant.BodyBuild = BodyBuild.Slim;
                        break;
                    case BodyBuild.Slim:
                        pMutant.BodyBuild = (BodyBuild)Rnd.Get(typeof(BodyBuild));
                        break;
                    case BodyBuild.Muscular:
                        pMutant.BodyBuild = Rnd.OneChanceFrom(3) ? BodyBuild.Fat : BodyBuild.Slim;
                        break;
                    case BodyBuild.Fat:
                        pMutant.BodyBuild = Rnd.OneChanceFrom(3) ? BodyBuild.Muscular : BodyBuild.Slim;
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

                switch (pMutant.BodyBuild)
                {
                    case BodyBuild.Skinny:
                        pMutant.BodyBuild = BodyBuild.Slim;
                        break;
                    case BodyBuild.Slim:
                        pMutant.BodyBuild = (BodyBuild)Rnd.Get(typeof(BodyBuild));
                        break;
                    case BodyBuild.Muscular:
                        pMutant.BodyBuild = Rnd.OneChanceFrom(2) ? BodyBuild.Slim : BodyBuild.Fat;
                        break;
                    case BodyBuild.Fat:
                        pMutant.BodyBuild = Rnd.OneChanceFrom(2) ? BodyBuild.Slim : BodyBuild.Muscular;
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

                switch (pMutant.BodyBuild)
                {
                    case BodyBuild.Skinny:
                        pMutant.BodyBuild = BodyBuild.Slim;
                        break;
                    case BodyBuild.Slim:
                        pMutant.BodyBuild = (BodyBuild)Rnd.Get(typeof(BodyBuild));
                        break;
                    case BodyBuild.Muscular:
                        pMutant.BodyBuild = BodyBuild.Fat;
                        break;
                    case BodyBuild.Fat:
                        pMutant.BodyBuild = Rnd.OneChanceFrom(2) ? BodyBuild.Slim : BodyBuild.Muscular;
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

                switch (pMutant.BodyBuild)
                {
                    case BodyBuild.Skinny:
                        pMutant.BodyBuild = BodyBuild.Slim;
                        break;
                    case BodyBuild.Slim:
                        pMutant.BodyBuild = (BodyBuild)Rnd.Get(typeof(BodyBuild));
                        break;
                    case BodyBuild.Muscular:
                        pMutant.BodyBuild = BodyBuild.Fat;
                        break;
                    case BodyBuild.Fat:
                        pMutant.BodyBuild = Rnd.OneChanceFrom(2) ? BodyBuild.Slim : BodyBuild.Muscular;
                        break;
                }

                return pMutant;
            }

            return this;
        }

        #endregion
    }
}
