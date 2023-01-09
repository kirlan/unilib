using GeneLab;
using GeneLab.Genetix;
using LandscapeGeneration;
using Random;
using Socium.Population;
using Socium.Psychology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socium.Nations
{
    public enum MagicAbilityDistribution
    {
        mostly_weak,
        mostly_average,
        mostly_powerful
    }

    public class Nation
    {
        public Phenotype PhenotypeMale { get; }
        public Phenotype PhenotypeFemale { get; }

        public Society ProtoSociety { get; } = null;

        /// <summary>
        /// Фенотип "сильного" пола
        /// </summary>
        public Phenotype DominantPhenotype
        {
            get
            {
                if (ProtoSociety.DominantCulture.Customs.Has(Customs.GenderPriority.Matriarchy))
                    return PhenotypeFemale;
                else
                    return PhenotypeMale;
            }
        }

        /// <summary>
        /// Фенотип "слабого" пола
        /// </summary>
        public Phenotype InferiorFenotype
        {
            get
            {
                if (ProtoSociety.DominantCulture.Customs.Has(Customs.GenderPriority.Matriarchy))
                    return PhenotypeMale;
                else
                    return PhenotypeFemale;
            }
        }

        public Race Race { get; } = null;

        private readonly LandTypeInfo[] m_aPreferredLands;
        private readonly LandTypeInfo[] m_aHatedLands;

        public LandTypeInfo[] PreferredLands { get => m_aPreferredLands; }
        public LandTypeInfo[] HatedLands { get => m_aHatedLands; }

        public bool IsAncient { get; private set; }
        public bool IsHegemon { get; private set; }
        public bool IsInvader { get; }

        /// <summary>
        /// Пометить как "древнюю". Древние расы не могут быть гегемонами!
        /// </summary>
        public void Age()
        {
            IsAncient = true;
            IsHegemon = false;
        }

        /// <summary>
        /// Сделать гегемоном
        /// </summary>
        public void Grow()
        {
            IsHegemon = true;
        }

        public Epoch Epoch { get; } = null;

        public Nation(Race pRace, Epoch pEpoch, bool bInvader) : this(pRace, pEpoch)
        {
            IsInvader = bInvader;
        }

        public Nation(Race pRace, Epoch pEpoch)
        {
            Race = pRace;

            Epoch = pEpoch;

            bool bNew = false;
            do
            {
                PhenotypeMale = (Phenotype)Race.PhenotypeMale.MutateNation();

                var pExpectedPhenotypeF = Phenotype.Combine(PhenotypeMale, Race.GenderDiffFemale);

                PhenotypeFemale = (Phenotype)pExpectedPhenotypeF.MutateGender();

                bNew = !PhenotypeMale.IsIdentical(Race.PhenotypeMale) &&
                       !PhenotypeFemale.IsIdentical(Race.PhenotypeFemale);

                foreach (Nation pOtherNation in Race.Nations)
                {
                    if (PhenotypeMale.IsIdentical(pOtherNation.PhenotypeMale) &&
                        PhenotypeFemale.IsIdentical(pOtherNation.PhenotypeFemale))
                    {
                        bNew = false;
                    }
                }
            }
            while (!bNew);

            ProtoSociety = new NationalSociety(pRace, pEpoch, this);
            ProtoSociety.Name = Race.Language.RandomNationName();

            DominantPhenotype.GetTerritoryPreferences(out m_aPreferredLands, out m_aHatedLands);

            Race.Nations.Add(this);
        }

        public override string ToString()
        {
            //if (m_bDying)
            //    return string.Format("ancient {1} ({0})", m_sName, m_pRace).ToLower();
            //else
            //    if(m_bHegemon)
            //        return string.Format("great {1} ({0})", m_sName, m_pRace).ToLower();
            //    else

            if (ProtoSociety.Name == Race.ToString())
                return ProtoSociety.Name;

            return string.Format("{1} ({0})", ProtoSociety.Name, Race).ToLower();
        }

        /// <summary>
        /// Согласовать параметры расы с параметрами мира.
        /// Параметры мира могут немного отличаться от параметров эпохи - это нормально
        /// </summary>
        /// <param name="pWorld">мир</param>
        public void Accommodate(Epoch pEpoch)
        {
            if (IsInvader)
            {
                ProtoSociety.TechLevel = pEpoch.m_iInvadersMinTechLevel + Rnd.Get(pEpoch.m_iInvadersMaxTechLevel - pEpoch.m_iInvadersMinTechLevel + 1);
                ProtoSociety.MagicLimit = pEpoch.m_iInvadersMinMagicLevel + Rnd.Get(pEpoch.m_iInvadersMaxMagicLevel - pEpoch.m_iInvadersMinMagicLevel + 1);

                if (DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Primitive)
                    ProtoSociety.TechLevel = pEpoch.m_iInvadersMinTechLevel;

                int iMagicLimit = (int)(Math.Pow(Rnd.Get(15), 3) / 1000);
                if (Rnd.OneChanceFrom(10))
                    ProtoSociety.MagicLimit += iMagicLimit;
                else
                    ProtoSociety.MagicLimit -= iMagicLimit;

                int iOldTechLevel = ProtoSociety.TechLevel;

                int iTechLevel = (int)(Math.Pow(Rnd.Get(15), 3) / 1000);
                if (DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence != Intelligence.Primitive &&
                    (DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Ingenious || Rnd.OneChanceFrom(10)))
                    ProtoSociety.TechLevel += iTechLevel;
                else
                    ProtoSociety.TechLevel -= iTechLevel;

                if (ProtoSociety.MagicLimit < pEpoch.m_iInvadersMinMagicLevel)
                    ProtoSociety.MagicLimit = pEpoch.m_iInvadersMinMagicLevel;
                if (ProtoSociety.MagicLimit > pEpoch.m_iInvadersMaxMagicLevel)
                    ProtoSociety.MagicLimit = pEpoch.m_iInvadersMaxMagicLevel;

                if (ProtoSociety.TechLevel < pEpoch.m_iInvadersMinTechLevel)
                    ProtoSociety.TechLevel = pEpoch.m_iInvadersMinTechLevel;
                if (ProtoSociety.TechLevel > pEpoch.m_iInvadersMaxTechLevel)
                    ProtoSociety.TechLevel = pEpoch.m_iInvadersMaxTechLevel;
            }
            else
            {
                if (!IsAncient)
                {
                    int iNewTechLevel = pEpoch.m_iNativesMinTechLevel + Rnd.Get(pEpoch.m_iNativesMaxTechLevel - pEpoch.m_iNativesMinTechLevel + 1);
                    int iNewMagicLimit = pEpoch.m_iNativesMinMagicLevel + Rnd.Get(pEpoch.m_iNativesMaxMagicLevel - pEpoch.m_iNativesMinMagicLevel + 1);

                    if (ProtoSociety.MagicLimit <= pEpoch.m_iNativesMinMagicLevel)
                        ProtoSociety.MagicLimit = pEpoch.m_iNativesMinMagicLevel;
                    else
                        ProtoSociety.MagicLimit = (ProtoSociety.MagicLimit + iNewMagicLimit + 1) / 2;

                    if (ProtoSociety.TechLevel <= pEpoch.m_iNativesMinTechLevel || DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Primitive)
                        ProtoSociety.TechLevel = pEpoch.m_iNativesMinTechLevel;
                    else
                        ProtoSociety.TechLevel = (ProtoSociety.TechLevel + iNewTechLevel + 1) / 2;
                }

                int iMagicLimit = (int)(Math.Pow(Rnd.Get(13), 3) / 1000);
                if (Rnd.OneChanceFrom(10))
                    ProtoSociety.MagicLimit += iMagicLimit;
                else
                    ProtoSociety.MagicLimit -= iMagicLimit;

                int iOldTechLevel = ProtoSociety.TechLevel;

                int iTechLevel = (int)(Math.Pow(Rnd.Get(13), 3) / 1000);
                if (DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence != Intelligence.Primitive && 
                    (DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Ingenious || Rnd.OneChanceFrom(5)))
                    ProtoSociety.TechLevel += iTechLevel;
                else
                    ProtoSociety.TechLevel -= iTechLevel;

                if (!IsAncient)
                {
                    if (ProtoSociety.MagicLimit < pEpoch.m_iNativesMinMagicLevel)
                        ProtoSociety.MagicLimit = pEpoch.m_iNativesMinMagicLevel;
                    if (ProtoSociety.MagicLimit > pEpoch.m_iNativesMaxMagicLevel)
                        ProtoSociety.MagicLimit = pEpoch.m_iNativesMaxMagicLevel;

                    if (ProtoSociety.TechLevel < pEpoch.m_iNativesMinTechLevel)
                        ProtoSociety.TechLevel = pEpoch.m_iNativesMinTechLevel;
                    if (ProtoSociety.TechLevel > pEpoch.m_iNativesMaxTechLevel)
                        ProtoSociety.TechLevel = pEpoch.m_iNativesMaxTechLevel;
                }
                else
                {
                    if (ProtoSociety.MagicLimit < 0)
                        ProtoSociety.MagicLimit = 0;
                    if (ProtoSociety.MagicLimit > 8)
                        ProtoSociety.MagicLimit = 8;

                    if (ProtoSociety.TechLevel < 0)
                        ProtoSociety.TechLevel = 0;
                    if (ProtoSociety.TechLevel > 8)
                        ProtoSociety.TechLevel = 8;
                }
            }

            ProtoSociety.Culture[Gender.Male].MagicAbilityDistribution = MagicAbilityDistribution.mostly_average;
            ProtoSociety.Culture[Gender.Female].MagicAbilityDistribution = MagicAbilityDistribution.mostly_average;
            if (DominantPhenotype.m_pValues.Get<BrainGenetix>().MagicAbilityPotential > ProtoSociety.MagicLimit + 1)
                ProtoSociety.DominantCulture.MagicAbilityDistribution = MagicAbilityDistribution.mostly_powerful;
            if (DominantPhenotype.m_pValues.Get<BrainGenetix>().MagicAbilityPotential < ProtoSociety.MagicLimit - 1)
                ProtoSociety.InferiorCulture.MagicAbilityDistribution = MagicAbilityDistribution.mostly_weak;

            //int iNewLevel = Math.Max(m_iTechLevel, m_iMagicLimit);
            //if (iNewLevel > iOldLevel)
            //    for (int i = 0; i < iNewLevel * iNewLevel - iOldLevel * iOldLevel; i++)
            //    {
            //        m_pCulture.Evolve();
            //        m_pCustoms.Evolve();
            //    }
            //else
            //    for (int i = 0; i < iOldLevel - iNewLevel; i++)
            //    {
            //        m_pCulture.Degrade();
            //        m_pCustoms.Degrade();
            //    }
        }

        /// <summary>
        /// Вычисляет условную стоимость заселения территории указанной расой, в соответсвии с ландшафтом и фенотипом расы.
        /// Возвращает значение в диапазоне 1-100.
        /// 1 - любая территория, идеально подходящая указанной расе (горы для гномов). Так же - простая для заселения территория, просто подходящая указанной расе.
        /// 10 - простая для заселения территория (равнины), но совсем не подходящая указанной расе (горы для эльфов). Так же - максимально сложная для заселения территория, просто подходящая указанной расе (горы для людей).
        /// 100 - максимально сложная для заселения территория (непроходимые горы), совсем не подходящая указанной расе.
        /// </summary>
        /// <param name="pNation"></param>
        /// <returns></returns>
        public int GetClaimingCost(LandTypeInfo pLandType)
        {
            if (pLandType == null)
                return -1;

            if (!pLandType.Environment.HasFlag(LandscapeGeneration.Environment.Habitable))
                return -1;

            float fCost = pLandType.MovementCost; // 1 - 10

            if (PreferredLands.Contains(pLandType))
                fCost /= 10;

            if (HatedLands.Contains(pLandType))
                fCost *= 10;

            if (IsHegemon)
                fCost /= 2;

            if (fCost < 1)
                fCost = 1;

            if (fCost > int.MaxValue)
                fCost = int.MaxValue - 1;

            return (int)fCost;
        }

        public float GetAvailableFood(Dictionary<LandResource, float> cResources, int iPrey)
        {
            switch (DominantPhenotype.m_pValues.Get<NutritionGenetix>().NutritionType)
            {
                case NutritionType.Eternal:
                    return float.MaxValue;
                case NutritionType.Mineral:
                    return cResources[LandResource.Ore];
                case NutritionType.Organic:
                    return cResources[LandResource.Grain] + cResources[LandResource.Game] + cResources[LandResource.Fish];
                case NutritionType.ParasitismBlood:
                    return iPrey; //TODO: нyжно учитывать размеры и телосложение жертв - в гигантах и толстяках крови больше, чем в карликах и худышках
                case NutritionType.ParasitismEmote:
                    return iPrey; //TODO: нужно учитывать темперамент жертв 
                case NutritionType.ParasitismEnergy:
                    return iPrey; //TODO: нужно учитывать... хз что именно, но что-то точно нужно :)
                case NutritionType.ParasitismMeat:
                    return iPrey; //TODO: нyжно учитывать размеры и телосложение жертв - в гигантах и толстяках мяса больше, чем в карликах и худышках
                case NutritionType.Photosynthesis:
                    return float.MaxValue; //TODO: нужно учитывать климат и расстояние до экватора
                case NutritionType.Thermosynthesis:
                    return float.MaxValue; //TODO: нужно учитывать наличие вулканов и расстояние до экватора
                case NutritionType.Vegetarian:
                    return cResources[LandResource.Grain];
                case NutritionType.Carnivorous:
                    return cResources[LandResource.Game] + cResources[LandResource.Fish];
                default:
                    throw new InvalidOperationException(string.Format("Unknown Nutrition type: {0}", DominantPhenotype.m_pValues.Get<NutritionGenetix>().NutritionType));
            }
        }
    }
}
