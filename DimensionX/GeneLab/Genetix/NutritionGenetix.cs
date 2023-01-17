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
        /// <summary>
        /// людоедство - питается мясом других разумных существ
        /// применимо только к разумным существам - вервольфам и т.п.
        /// </summary>
        ParasitismMeat,
        /// <summary>
        /// эмоциональный вампиризм - питается эмоциями других разумных существ
        /// применимо только к разумным существам - иллитидам и т.п.
        /// </summary>
        ParasitismEmote,
        /// <summary>
        /// энергетический вампиризм - питается жизненной энергией других существ
        /// применимо только к разумным существам - иллитидам и т.п.
        /// </summary>
        ParasitismEnergy,
        /// <summary>
        /// питается неорганической пищей
        /// </summary>
        Mineral
    }

    public class NutritionGenetix : IGenetix
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

            switch (NutritionType)
            {
                case NutritionType.Eternal:
                    sMeals = "has no need in any food at all";
                    break;
                case NutritionType.Mineral:
                    sMeals = "consumes various minearals as food";
                    break;
                case NutritionType.Organic:
                    if (bFull)
                    {
                        sMeals = "eats both meat and vegetables";
                    }
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

        public NutritionType NutritionType { get; private set; } = NutritionType.Organic;

        public bool IsParasite()
        {
            return NutritionType == NutritionType.ParasitismBlood ||
                NutritionType == NutritionType.ParasitismEmote ||
                NutritionType == NutritionType.ParasitismEnergy ||
                NutritionType == NutritionType.ParasitismMeat;
        }

        public bool IsIdentical(IGenetix pOther)
        {
            if (!(pOther is NutritionGenetix pAnother))
                return false;

            return NutritionType == pAnother.NutritionType;
        }

        public NutritionGenetix()
        { }

        public NutritionGenetix(NutritionGenetix pPredcessor)
        {
            NutritionType = pPredcessor.NutritionType;
        }

        public NutritionGenetix(NutritionType eNutritionType)
        {
            NutritionType = eNutritionType;
        }

        public void MutateNutritionType(BodyGenetix pBody)
        {
            int[] aChances = new[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            switch (NutritionType)
            {
                case NutritionType.Eternal:
                    {
                        aChances = new[]{ 8, 4, 4, 1, 0, 0, 0, 0, 2, 4, 1 };
                    }
                    break;
                case NutritionType.Photosynthesis:
                    {
                        aChances = new[]{ 1, 8, 4, 2, 0, 0, 0, 0, 2, 2, 1 };
                    }
                    break;
                case NutritionType.Thermosynthesis:
                    {
                        aChances = new[]{ 1, 4, 8, 2, 0, 0, 0, 0, 2, 4, 1 };
                    }
                    break;
                case NutritionType.Vegetarian:
                    {
                        aChances = new[]{ 0, 1, 0, 8, 4, 0, 0, 0, 2, 2, 0 };
                    }
                    break;
                case NutritionType.Organic:
                    {
                        aChances = new[]{ 0, 0, 0, 4, 8, 4, 2, 2, 1, 1, 0 };
                    }
                    break;
                case NutritionType.Carnivorous:
                    {
                        aChances = new[]{ 0, 0, 0, 0, 4, 8, 2, 4, 0, 0, 0 };
                    }
                    break;
                case NutritionType.ParasitismBlood:
                    {
                        aChances = new[]{ 0, 0, 0, 0, 1, 4, 8, 4, 1, 1, 0 };
                    }
                    break;
                case NutritionType.ParasitismMeat:
                    {
                        aChances = new[]{ 0, 0, 0, 0, 1, 4, 4, 8, 1, 1, 0 };
                    }
                    break;
                case NutritionType.ParasitismEmote:
                    {
                        aChances = new[]{ 0, 0, 0, 0, 0, 0, 2, 2, 8, 4, 0 };
                    }
                    break;
                case NutritionType.ParasitismEnergy:
                    {
                        aChances = new[]{ 0, 0, 0, 0, 0, 0, 2, 2, 4, 8, 0 };
                    }
                    break;
                case NutritionType.Mineral:
                    {
                        aChances = new[]{ 1, 2, 4, 0, 0, 0, 0, 0, 1, 2, 8 };
                    }
                    break;
            }

            if (pBody.BodySize == BodySize.Big)
            {
                aChances[5] *= 2;//Сarnivorous
                aChances[7] *= 2;//ParasitismMeat
            }

            if (pBody.BodySize == BodySize.Giant)
            {
                aChances[0] *= 2;//Eternal
                aChances[1] *= 2;//Photosynthesis
                aChances[2] *= 2;//Thermosynthesis
                aChances[3] *= 2;//Vegetarian
                aChances[7] *= 2;//ParasitismMeat
                aChances[10] *= 2;//Mineral

                aChances[5] = Math.Min(1, aChances[5]);//Сarnivorous
            }

            if (pBody.BodySize == BodySize.Tini ||
                pBody.BodySize == BodySize.Small)
            {
                aChances[5] = Math.Min(1, aChances[5]);//Сarnivorous
                aChances[7] = Math.Min(1, aChances[7]);//ParasitismMeat
            }

            if (pBody.BodyBuild == BodyBuild.Muscular)
            {
                aChances[4] *= 2;//Organic
                aChances[5] *= 2;//Сarnivorous
                aChances[7] *= 2;//ParasitismMeat
                aChances[10] *= 2;//Mineral

                aChances[8] = Math.Min(1, aChances[8]);//ParasitismEmote
                aChances[9] = Math.Min(1, aChances[9]);//ParasitismEnergy
            }

            if (pBody.BodyBuild == BodyBuild.Fat)
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

            if (pBody.BodyBuild == BodyBuild.Skinny)
            {
                aChances[3] *= 2;//Vegetarian
                aChances[6] *= 2;//ParasitismBlood

                aChances[7] = Math.Min(1, aChances[7]);//ParasitismMeat
                aChances[10] = Math.Min(1, aChances[10]);//Mineral
            }

            int iChance = Rnd.ChooseOne(aChances, 1);

            switch (iChance)
            {
                case 0:
                    NutritionType = NutritionType.Eternal;
                    break;
                case 1:
                    NutritionType = NutritionType.Photosynthesis;
                    break;
                case 2:
                    NutritionType = NutritionType.Thermosynthesis;
                    break;
                case 3:
                    NutritionType = NutritionType.Vegetarian;
                    break;
                case 4:
                    NutritionType = NutritionType.Organic;
                    break;
                case 5:
                    NutritionType = NutritionType.Carnivorous;
                    break;
                case 6:
                    NutritionType = NutritionType.ParasitismBlood;
                    break;
                case 7:
                    NutritionType = NutritionType.ParasitismMeat;
                    break;
                case 8:
                    NutritionType = NutritionType.ParasitismEmote;
                    break;
                case 9:
                    NutritionType = NutritionType.ParasitismEnergy;
                    break;
                case 10:
                    NutritionType = NutritionType.Mineral;
                    break;
            }
        }

        #region GenetixBase Members

        /// <summary>
        /// Do nothing, use MutateNutritionType instead
        /// </summary>
        /// <returns></returns>
        public IGenetix MutateRace()
        {
            return this;
        }

        /// <summary>
        /// Do nothing, use MutateNutritionType instead
        /// </summary>
        /// <returns></returns>
        public IGenetix MutateGender()
        {
            return this;
        }

        public IGenetix MutateNation()
        {
            return this;
        }

        public IGenetix MutateFamily()
        {
            return this;
        }

        public IGenetix MutateIndividual()
        {
            return this;
        }

        #endregion
    }
}
