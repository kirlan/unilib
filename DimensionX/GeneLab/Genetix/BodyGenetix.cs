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
        Slim,
        /// <summary>
        /// обычное телосложение - ничего выдяляющегося
        /// </summary>
        Normal,
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
        Сarnivorous,
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

    public class BodyGenetix: GenetixBase
    {
        public BodySize m_eBodySize = BodySize.Normal;

        public BodyBuild m_eBodyBuild = BodyBuild.Normal;

        public NutritionType m_eNutritionType = NutritionType.Organic;

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

        private bool MutateNutritionType()
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
                case NutritionType.Сarnivorous:
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
                aChances[6] = Math.Min(1, aChances[6]);//ParasitismBlood
                aChances[7] = Math.Min(1, aChances[7]);//ParasitismMeat
            }

            if (m_eBodyBuild == BodyBuild.Slim)
            {
                aChances[3] *= 2;//Vegetarian
                aChances[6] *= 2;//ParasitismBlood

                aChances[10] = Math.Min(1, aChances[10]);//Mineral
            }

            if (m_eBodySize == BodySize.Tini ||
                m_eBodySize == BodySize.Small)
            {
                aChances[5] = Math.Min(1, aChances[5]);//Сarnivorous
                aChances[7] = Math.Min(1, aChances[7]);//ParasitismMeat
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
                    m_eNutritionType = NutritionType.Сarnivorous;
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

            return m_eNutritionType != eNutritionType;
        }

        #region GenetixBase Members

        public GenetixBase MutateRace()
        {
            if (Rnd.OneChanceFrom(10))
            {
                bool bMutation = false;

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

                    bMutation = true;
                }

                if (Rnd.OneChanceFrom(2))
                {
                    pMutant.m_eBodyBuild = (BodyBuild)Rnd.Get(typeof(BodyBuild));
                    if (pMutant.m_eBodyBuild != m_eBodyBuild)
                        bMutation = true;
                }

                if (pMutant.MutateNutritionType())
                    bMutation = true;

                if(bMutation)
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
                    case BodyBuild.Slim:
                        pMutant.m_eBodyBuild = BodyBuild.Normal;
                        break;
                    case BodyBuild.Normal:
                        pMutant.m_eBodyBuild = (BodyBuild)Rnd.Get(typeof(BodyBuild));
                        break;
                    case BodyBuild.Muscular:
                        pMutant.m_eBodyBuild = Rnd.OneChanceFrom(2) ? BodyBuild.Normal : BodyBuild.Fat;
                        break;
                    case BodyBuild.Fat:
                        pMutant.m_eBodyBuild = Rnd.OneChanceFrom(2) ? BodyBuild.Normal : BodyBuild.Muscular;
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
                    case BodyBuild.Slim:
                        pMutant.m_eBodyBuild = BodyBuild.Normal;
                        break;
                    case BodyBuild.Normal:
                        pMutant.m_eBodyBuild = (BodyBuild)Rnd.Get(typeof(BodyBuild));
                        break;
                    case BodyBuild.Muscular:
                        pMutant.m_eBodyBuild = BodyBuild.Fat;
                        break;
                    case BodyBuild.Fat:
                        pMutant.m_eBodyBuild = Rnd.OneChanceFrom(2) ? BodyBuild.Normal : BodyBuild.Muscular;
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
                    case BodyBuild.Slim:
                        pMutant.m_eBodyBuild = BodyBuild.Normal;
                        break;
                    case BodyBuild.Normal:
                        pMutant.m_eBodyBuild = (BodyBuild)Rnd.Get(typeof(BodyBuild));
                        break;
                    case BodyBuild.Muscular:
                        pMutant.m_eBodyBuild = BodyBuild.Fat;
                        break;
                    case BodyBuild.Fat:
                        pMutant.m_eBodyBuild = Rnd.OneChanceFrom(2) ? BodyBuild.Normal : BodyBuild.Muscular;
                        break;
                }

                return pMutant;
            }

            return this;
        }

        #endregion
    }
}
