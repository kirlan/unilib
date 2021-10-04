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

    public enum NutritionType
    {
        /// <summary>
        /// не требуется вообще никакая пища
        /// </summary>
        Eternal,
        /// <summary>
        /// фотосинтез - не требуется никакая материальная пища, но нужен доступ к свету
        /// </summary>
        Photosynthesis,
        /// <summary>
        /// термосинтез - не требуется никакая материальная пища, но нужен доступ к источникам тепла
        /// </summary>
        Thermosynthesis,
        /// <summary>
        /// только растительная пища
        /// </summary>
        Vegetarian,
        /// <summary>
        /// любая органика - растения, мясо...
        /// </summary>
        Organic,
        /// <summary>
        /// только мясо
        /// </summary>
        Carnivorous,
        /// <summary>
        /// вампиризм - питается кровью других разумных существ
        /// применимо только к разумным существам - вампирам и т.п.
        /// </summary>
        ParasitismBlood,
        /// людоедство - питается мясом других разумных существ
        /// применимо только к разумным существам - вервольфам и т.п.
        /// </summary>
        ParasitismMeat,
        /// эмоциональный вампиризм - питается эмоциями других разумных существ
        /// применимо только к разумным существам - иллитидам и т.п.
        /// </summary>
        ParasitismEmote,
        /// энергетический вампиризм - питается жизненной энергией других существ
        /// применимо только к разумным существам - иллитидам и т.п.
        /// </summary>
        ParasitismEnergy,
        /// <summary>
        /// питается неорганической пищей
        /// </summary>
        Mineral
    }
    public enum Gender
    {
        Male,
        Female
    }


    public class BodyGenetix: GenetixBase
    {
        /// <summary>
        /// eats only vegetables and have small and agile bodies 
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            string sSize = "?";
            switch (m_eBodySize)
            {
                case BodySize.Tini:
                    sSize = "tiny";
                    break;
                case BodySize.Small:
                    sSize = "small";
                    break;
                case BodySize.Normal:
                    sSize = "average-sized";
                    break;
                case BodySize.Big:
                    sSize = "quite big";
                    break;
                case BodySize.Giant:
                    sSize = "enormously big";
                    break;
            }

            string sComplexion = "?";
            switch (m_eBodyBuild)
            {
                case BodyBuild.Skinny:
                    sComplexion = "agile";
                    break;
                case BodyBuild.Slim:
                    sComplexion = "proportional";
                    break;
                case BodyBuild.Muscular:
                    sComplexion = "muscular";
                    break;
                case BodyBuild.Fat:
                    sComplexion = "stout";
                    break;
            }

            string sMeals = "?";
            switch (m_eNutritionType)
            {
                case NutritionType.Eternal:
                    sMeals = "have no need in any food at all";
                    break;
                case NutritionType.Mineral:
                    sMeals = "consumes various minearals as food";
                    break;
                case NutritionType.Organic:
                    sMeals = "eats both meat and vegetables";
                    break;
                case NutritionType.ParasitismBlood:
                    sMeals = "drinks blood of their victims";
                    break;
                case NutritionType.ParasitismEmote:
                    sMeals = "feeds on emotes of their victims";
                    break;
                case NutritionType.ParasitismEnergy:
                    sMeals = "drains life energy of their victims";
                    break;
                case NutritionType.ParasitismMeat:
                    sMeals = "eats meat of their victims";
                    break;
                case NutritionType.Photosynthesis:
                    sMeals = "needs only a sunlight to gain their life energy";
                    break;
                case NutritionType.Thermosynthesis:
                    sMeals = "needs only access to heat source to gain their life energy";
                    break;
                case NutritionType.Vegetarian:
                    sMeals = "eats only vegetables";
                    break;
                case NutritionType.Carnivorous:
                    sMeals = "eats only meat";
                    break;
            }

            return sMeals + " and have " + sSize + " and " + sComplexion + " bodies";
        }

        /// <summary>
        /// tall agile woman
        /// </summary>
        /// <returns></returns>
        public string GetDescription(Gender eGender)
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
                //case BodySize.Normal:
                //    sSize = "average-sized";
                //    break;
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
                //case BodyBuild.Slim:
                //    sComplexion = "proportional";
                //    break;
                case BodyBuild.Muscular:
                    sComplexion = "muscular";
                    break;
                case BodyBuild.Fat:
                    sComplexion = "fat";
                    break;
            }

            if (sSize != "" && sComplexion != "")
                sSize += " ";

            string sMeals = "";
            switch (m_eNutritionType)
            {
                case NutritionType.Eternal:
                    sMeals = "has no need in any food at all";
                    break;
                case NutritionType.Mineral:
                    sMeals = "consumes various minearals as food";
                    break;
                //case NutritionType.Organic:
                //    sMeals = "eats both meat and vegetables";
                //    break;
                case NutritionType.ParasitismBlood:
                    sMeals = "drinks blood of " + (eGender == Gender.Male ? "his":"her") + " victims";
                    break;
                case NutritionType.ParasitismEmote:
                    sMeals = "feeds on emotes of " + (eGender == Gender.Male ? "his" : "her") + " victims";
                    break;
                case NutritionType.ParasitismEnergy:
                    sMeals = "drains life energy of " + (eGender == Gender.Male ? "his" : "her") + " victims";
                    break;
                case NutritionType.ParasitismMeat:
                    sMeals = "eats meat of " + (eGender == Gender.Male ? "his" : "her") + " victims";
                    break;
                case NutritionType.Photosynthesis:
                    sMeals = "needs only a sunlight to gain " + (eGender == Gender.Male ? "his" : "her") + " life energy";
                    break;
                case NutritionType.Thermosynthesis:
                    sMeals = "needs only access to heat source to gain " + (eGender == Gender.Male ? "his" : "her") + " life energy";
                    break;
                case NutritionType.Vegetarian:
                    sMeals = "eats only vegetables";
                    break;
                case NutritionType.Carnivorous:
                    sMeals = "eats only meat";
                    break;
            }

            return sSize + sComplexion + (eGender == Gender.Male ? " man" : " woman");// +", who " + sMeals + ".";
        }

        /// <summary>
        /// He is a vampire
        /// </summary>
        /// <param name="eGender"></param>
        /// <returns></returns>
        public string GetDescription2(Gender eGender)
        {
            string sMeals = "";
            switch (m_eNutritionType)
            {
                case NutritionType.Eternal:
                    sMeals = "has no need in any food at all";
                    break;
                case NutritionType.Mineral:
                    sMeals = "consumes various minearals as food";
                    break;
                //case NutritionType.Organic:
                //    sMeals = "eats both meat and vegetables";
                //    break;
                case NutritionType.ParasitismBlood:
                    sMeals = "is a vampire";
                    break;
                case NutritionType.ParasitismEmote:
                    sMeals = "feeds on emotes of " + (eGender == Gender.Male ? "his" : "her") + " victims";
                    break;
                case NutritionType.ParasitismEnergy:
                    sMeals = "drains life energy of " + (eGender == Gender.Male ? "his" : "her") + " victims";
                    break;
                case NutritionType.ParasitismMeat:
                    sMeals = "eats meat of " + (eGender == Gender.Male ? "his" : "her") + " victims";
                    break;
                case NutritionType.Photosynthesis:
                    sMeals = "needs only a sunlight to gain " + (eGender == Gender.Male ? "his" : "her") + " life energy";
                    break;
                case NutritionType.Thermosynthesis:
                    sMeals = "needs only access to heat source to gain " + (eGender == Gender.Male ? "his" : "her") + " life energy";
                    break;
                case NutritionType.Vegetarian:
                    sMeals = "is a herbivore";
                    break;
                case NutritionType.Carnivorous:
                    sMeals = "is a carnivore";
                    break;
            }

            if (sMeals == "")
                return "";

            return (eGender == Gender.Male ? "He " : "She ") + sMeals + ".";
        }

        public static BodyGenetix Human
        {
            get { return new BodyGenetix(BodySize.Normal, BodyBuild.Slim, NutritionType.Organic); }
        }

        public static BodyGenetix Barbarian
        {
            get { return new BodyGenetix(BodySize.Normal, BodyBuild.Muscular, NutritionType.Organic); }
        }

        public static BodyGenetix Bull
        {
            get { return new BodyGenetix(BodySize.Normal, BodyBuild.Muscular, NutritionType.Vegetarian); }
        }

        public static BodyGenetix Orc
        {
            get { return new BodyGenetix(BodySize.Normal, BodyBuild.Muscular, NutritionType.Carnivorous); }
        }

        public static BodyGenetix Goblin
        {
            get { return new BodyGenetix(BodySize.Small, BodyBuild.Skinny, NutritionType.Carnivorous); }
        }

        public static BodyGenetix Dwarf
        {
            get { return new BodyGenetix(BodySize.Small, BodyBuild.Muscular, NutritionType.Organic); }
        }

        public static BodyGenetix Hobbit
        {
            get { return new BodyGenetix(BodySize.Small, BodyBuild.Fat, NutritionType.Organic); }
        }

        public static BodyGenetix Elf
        {
            get { return new BodyGenetix(BodySize.Normal, BodyBuild.Skinny, NutritionType.Organic); }
        }

        public static BodyGenetix Furry
        {
            get { return new BodyGenetix(BodySize.Normal, BodyBuild.Skinny, NutritionType.Carnivorous); }
        }

        public static BodyGenetix Pixie
        {
            get { return new BodyGenetix(BodySize.Tini, BodyBuild.Skinny, NutritionType.Vegetarian); }
        }

        public static BodyGenetix Giant
        {
            get { return new BodyGenetix(BodySize.Giant, BodyBuild.Muscular, NutritionType.Organic); }
        }

        public static BodyGenetix Vampire
        {
            get { return new BodyGenetix(BodySize.Normal, BodyBuild.Slim, NutritionType.ParasitismBlood); }
        }

        public static BodyGenetix Werevolf
        {
            get { return new BodyGenetix(BodySize.Normal, BodyBuild.Muscular, NutritionType.ParasitismMeat); }
        }


        public BodySize m_eBodySize = BodySize.Normal;

        public BodyBuild m_eBodyBuild = BodyBuild.Slim;

        public NutritionType m_eNutritionType = NutritionType.Organic;

        public bool IsParasite()
        {
            return m_eNutritionType == NutritionType.ParasitismBlood ||
                m_eNutritionType == NutritionType.ParasitismEmote ||
                m_eNutritionType == NutritionType.ParasitismEnergy ||
                m_eNutritionType == NutritionType.ParasitismMeat;
        }

        public bool IsIdentical(GenetixBase pOther)
        {
            BodyGenetix pAnother = pOther as BodyGenetix;

            if (pAnother == null)
                return false;

            return m_eBodySize == pAnother.m_eBodySize && 
                m_eBodyBuild == pAnother.m_eBodyBuild &&
                m_eNutritionType == pAnother.m_eNutritionType;
        }

        public BodyGenetix()
        { }

        public BodyGenetix(BodyGenetix pPredcessor)
        {
            m_eBodySize = pPredcessor.m_eBodySize;
            m_eBodyBuild = pPredcessor.m_eBodyBuild;
            m_eNutritionType = pPredcessor.m_eNutritionType;
        }

        public BodyGenetix(BodySize eBodySize, BodyBuild eBodyBuild, NutritionType eNutritionType)
        {
            m_eBodySize = eBodySize;
            m_eBodyBuild = eBodyBuild;
            m_eNutritionType = eNutritionType;
        }

        private void MutateNutritionType()
        {
            int iChance = 0;
            int[] aChances = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            switch (m_eNutritionType)
            {
                case NutritionType.Eternal:
                    {
                        aChances = new int[] { 8, 4, 4, 1, 0, 0, 0, 0, 2, 4, 1 };
                    }
                    break;
                case NutritionType.Photosynthesis:
                    {
                        aChances = new int[] { 2, 8, 4, 2, 0, 0, 0, 0, 2, 2, 1 };
                    }
                    break;
                case NutritionType.Thermosynthesis:
                    {
                        aChances = new int[] { 2, 4, 8, 2, 0, 0, 0, 0, 2, 4, 1 };
                    }
                    break;
                case NutritionType.Vegetarian:
                    {
                        aChances = new int[] { 1, 1, 1, 8, 4, 0, 0, 0, 2, 2, 0 };
                    }
                    break;
                case NutritionType.Organic:
                    {
                        aChances = new int[] { 0, 0, 0, 4, 8, 4, 2, 2, 1, 1, 0 };
                    }
                    break;
                case NutritionType.Carnivorous:
                    {
                        aChances = new int[] { 0, 0, 0, 0, 4, 8, 2, 4, 0, 0, 0 };
                    }
                    break;
                case NutritionType.ParasitismBlood:
                    {
                        aChances = new int[] { 0, 0, 0, 0, 1, 4, 8, 4, 1, 1, 0 };
                    }
                    break;
                case NutritionType.ParasitismMeat:
                    {
                        aChances = new int[] { 0, 0, 0, 0, 1, 4, 4, 8, 1, 1, 0 };
                    }
                    break;
                case NutritionType.ParasitismEmote:
                    {
                        aChances = new int[] { 0, 0, 0, 0, 0, 0, 2, 2, 8, 4, 0 };
                    }
                    break;
                case NutritionType.ParasitismEnergy:
                    {
                        aChances = new int[] { 0, 0, 0, 0, 0, 0, 2, 2, 4, 8, 0 };
                    }
                    break;
                case NutritionType.Mineral:
                    {
                        aChances = new int[] { 1, 2, 4, 0, 0, 0, 0, 0, 1, 2, 8 };
                    }
                    break;
            }

            if (m_eBodySize == BodySize.Big)
            {
                aChances[5] *= 2;//Сarnivorous
                aChances[7] *= 2;//ParasitismMeat
            }

            if (m_eBodySize == BodySize.Giant)
            {
                aChances[0] *= 2;//Eternal
                aChances[1] *= 2;//Photosynthesis
                aChances[2] *= 2;//Thermosynthesis
                aChances[3] *= 2;//Vegetarian
                aChances[7] *= 2;//ParasitismMeat
                aChances[10] *= 2;//Mineral

                aChances[5] = Math.Min(1, aChances[5]);//Сarnivorous
            }

            if (m_eBodySize == BodySize.Tini ||
                m_eBodySize == BodySize.Small)
            {
                aChances[5] = Math.Min(1, aChances[5]);//Сarnivorous
                aChances[7] = Math.Min(1, aChances[7]);//ParasitismMeat
            }

            if (m_eBodyBuild == BodyBuild.Muscular)
            {
                aChances[4] *= 2;//Organic
                aChances[5] *= 2;//Сarnivorous
                aChances[7] *= 2;//ParasitismMeat
                aChances[10] *= 2;//Mineral

                aChances[8] = Math.Min(1, aChances[8]);//ParasitismEmote
                aChances[9] = Math.Min(1, aChances[9]);//ParasitismEnergy
            }

            if (m_eBodyBuild == BodyBuild.Fat)
            {
                aChances[0] *= 2;//Eternal
                aChances[1] *= 2;//Photosynthesis
                aChances[2] *= 2;//Thermosynthesis
                aChances[8] *= 2;//ParasitismEmote
                aChances[9] *= 2;//ParasitismEnergy

                aChances[5] = Math.Min(1, aChances[5]);//Сarnivorous
                aChances[6] = 0;// Math.Min(1, aChances[6]);//ParasitismBlood
                aChances[7] = 0;// Math.Min(1, aChances[7]);//ParasitismMeat
            }

            if (m_eBodyBuild == BodyBuild.Skinny)
            {
                aChances[3] *= 2;//Vegetarian
                aChances[6] *= 2;//ParasitismBlood

                aChances[7] = Math.Min(1, aChances[7]);//ParasitismMeat
                aChances[10] = Math.Min(1, aChances[10]);//Mineral
            }

            iChance = Rnd.ChooseOne(aChances, 1);

            NutritionType eNutritionType = m_eNutritionType;

            switch (iChance)
            {
                case 0:
                    m_eNutritionType = NutritionType.Eternal;
                    break;
                case 1:
                    m_eNutritionType = NutritionType.Photosynthesis;
                    break;
                case 2:
                    m_eNutritionType = NutritionType.Thermosynthesis;
                    break;
                case 3:
                    m_eNutritionType = NutritionType.Vegetarian;
                    break;
                case 4:
                    m_eNutritionType = NutritionType.Organic;
                    break;
                case 5:
                    m_eNutritionType = NutritionType.Carnivorous;
                    break;
                case 6:
                    m_eNutritionType = NutritionType.ParasitismBlood;
                    break;
                case 7:
                    m_eNutritionType = NutritionType.ParasitismMeat;
                    break;
                case 8:
                    m_eNutritionType = NutritionType.ParasitismEmote;
                    break;
                case 9:
                    m_eNutritionType = NutritionType.ParasitismEnergy;
                    break;
                case 10:
                    m_eNutritionType = NutritionType.Mineral;
                    break;
            }
        }

        #region GenetixBase Members

        public GenetixBase MutateRace()
        {
            if (Rnd.OneChanceFrom(10))
            {
                BodyGenetix pMutant = new BodyGenetix(this);

                if (Rnd.OneChanceFrom(2))
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

                if (Rnd.OneChanceFrom(2))
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

                if (Rnd.OneChanceFrom(2))
                    pMutant.MutateNutritionType();

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
