using Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneLab.Genetix
{
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

    public class NutritionGenetix : GenetixBase
    {
        /// <summary>
        /// eats only vegetables
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            return GetDescription(null, true);
        }

        /// <summary>
        /// 'eats only vegetables'
        /// or, if gender specified -
        /// 'He/She is a herbivore'
        /// </summary>
        /// <returns></returns>
        public string GetDescription(Gender? eGender, bool bFull)
        {
            string sMeals = "";
            string pronoun = "their";
            if (eGender.HasValue)
                pronoun = eGender == Gender.Male ? "his" : "her";

            switch (m_eNutritionType)
            {
                case NutritionType.Eternal:
                    sMeals = "has no need in any food at all";
                    break;
                case NutritionType.Mineral:
                    sMeals = "consumes various minearals as food";
                    break;
                case NutritionType.Organic:
                    if (bFull)
                        sMeals = "eats both meat and vegetables";
                    break;
                case NutritionType.ParasitismBlood:
                    sMeals = "drinks blood of " + pronoun + " victims";
                    break;
                case NutritionType.ParasitismEmote:
                    sMeals = "feeds on emotes of " + pronoun + " victims";
                    break;
                case NutritionType.ParasitismEnergy:
                    sMeals = "drains life energy of " + pronoun + " victims";
                    break;
                case NutritionType.ParasitismMeat:
                    sMeals = "eats meat of " + pronoun + " victims";
                    break;
                case NutritionType.Photosynthesis:
                    sMeals = "needs only a sunlight to gain " + pronoun + " life energy";
                    break;
                case NutritionType.Thermosynthesis:
                    sMeals = "needs only access to heat source to gain " + pronoun + " life energy";
                    break;
                case NutritionType.Vegetarian:
                    sMeals = eGender.HasValue ? "is a herbivore" : "eats only vegetables";
                    break;
                case NutritionType.Carnivorous:
                    sMeals = eGender.HasValue ? "is a carnivore" : "eats only meat";
                    break;
            }

            if (sMeals != "")
            {
                if (eGender.HasValue)
                    return (eGender == Gender.Male ? "He " : "She ") + sMeals + ".";
                else
                    return sMeals;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// organic food
        /// </summary>
        public static NutritionGenetix Human
        {
            get { return new NutritionGenetix(NutritionType.Organic); }
        }

        /// <summary>
        /// vegetarian
        /// </summary>
        public static NutritionGenetix Vegetarian
        {
            get { return new NutritionGenetix(NutritionType.Vegetarian); }
        }

        /// <summary>
        /// meat food
        /// </summary>
        public static NutritionGenetix Predator
        {
            get { return new NutritionGenetix(NutritionType.Carnivorous); }
        }

        /// <summary>
        /// drinks blood
        /// </summary>
        public static NutritionGenetix Vampire
        {
            get { return new NutritionGenetix(NutritionType.ParasitismBlood); }
        }

        /// <summary>
        /// human meat food
        /// </summary>
        public static NutritionGenetix ManEater
        {
            get { return new NutritionGenetix(NutritionType.ParasitismMeat); }
        }

        /// <summary>
        /// life force food
        /// </summary>
        public static NutritionGenetix EnergyVampire
        {
            get { return new NutritionGenetix(NutritionType.ParasitismEnergy); }
        }


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
            NutritionGenetix pAnother = pOther as NutritionGenetix;

            if (pAnother == null)
                return false;

            return m_eNutritionType == pAnother.m_eNutritionType;
        }

        public NutritionGenetix()
        { }

        public NutritionGenetix(NutritionGenetix pPredcessor)
        {
            m_eNutritionType = pPredcessor.m_eNutritionType;
        }

        public NutritionGenetix(NutritionType eNutritionType)
        {
            m_eNutritionType = eNutritionType;
        }

        public void MutateNutritionType(BodyGenetix pBody)
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
                        aChances = new int[] { 1, 8, 4, 2, 0, 0, 0, 0, 2, 2, 1 };
                    }
                    break;
                case NutritionType.Thermosynthesis:
                    {
                        aChances = new int[] { 1, 4, 8, 2, 0, 0, 0, 0, 2, 4, 1 };
                    }
                    break;
                case NutritionType.Vegetarian:
                    {
                        aChances = new int[] { 0, 1, 0, 8, 4, 0, 0, 0, 2, 2, 0 };
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

            if (pBody.m_eBodySize == BodySize.Big)
            {
                aChances[5] *= 2;//Сarnivorous
                aChances[7] *= 2;//ParasitismMeat
            }

            if (pBody.m_eBodySize == BodySize.Giant)
            {
                aChances[0] *= 2;//Eternal
                aChances[1] *= 2;//Photosynthesis
                aChances[2] *= 2;//Thermosynthesis
                aChances[3] *= 2;//Vegetarian
                aChances[7] *= 2;//ParasitismMeat
                aChances[10] *= 2;//Mineral

                aChances[5] = Math.Min(1, aChances[5]);//Сarnivorous
            }

            if (pBody.m_eBodySize == BodySize.Tini ||
                pBody.m_eBodySize == BodySize.Small)
            {
                aChances[5] = Math.Min(1, aChances[5]);//Сarnivorous
                aChances[7] = Math.Min(1, aChances[7]);//ParasitismMeat
            }

            if (pBody.m_eBodyBuild == BodyBuild.Muscular)
            {
                aChances[4] *= 2;//Organic
                aChances[5] *= 2;//Сarnivorous
                aChances[7] *= 2;//ParasitismMeat
                aChances[10] *= 2;//Mineral

                aChances[8] = Math.Min(1, aChances[8]);//ParasitismEmote
                aChances[9] = Math.Min(1, aChances[9]);//ParasitismEnergy
            }

            if (pBody.m_eBodyBuild == BodyBuild.Fat)
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

            if (pBody.m_eBodyBuild == BodyBuild.Skinny)
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

        /// <summary>
        /// Do nothing, use MutateNutritionType instead
        /// </summary>
        /// <returns></returns>
        public GenetixBase MutateRace()
        {
            return this;
        }

        /// <summary>
        /// Do nothing, use MutateNutritionType instead
        /// </summary>
        /// <returns></returns>
        public GenetixBase MutateGender()
        {
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
