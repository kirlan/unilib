﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using BenTools.Mathematics;
using System.Drawing;
using LandscapeGeneration;
using LandscapeGeneration.PathFind;
using Socium.Languages;
using System.Diagnostics;
using Socium.Nations;
using Socium.Settlements;
using Socium.Psychology;
using Socium.Population;
using GeneLab.Genetix;

namespace Socium
{
    public class World : Landscape
    {
        public Province[] m_aProvinces = null;

        private int m_iProvincesCount = 300;

        public State[] m_aStates = null;

        private int m_iStatesCount = 30;

        public Nation[] m_aLocalNations = null;

        public List<Person> m_cPersons = new List<Person>();

        private void AddRaces(Epoch pEpoch)
        {
            List<Nation> cNations = new List<Nation>();

            int iDyingRaces = 0;
            //Все расы, оставшиеся с прошлых исторических циклов, адаптируем к новым условиям.
            if(m_aLocalNations != null)
                foreach (Nation pNation in m_aLocalNations)
                {
                    pNation.Accommodate(pEpoch);
                    cNations.Add(pNation);

                    if (pNation.IsAncient)
                        iDyingRaces++;
                }

            //Рассчитываем шансы новых рас попасть в новый мир - учитывая, его параметры
            //Dictionary<RaceTemplate, float> cRaceChances = new Dictionary<RaceTemplate, float>();
            //foreach (RaceTemplate pRaceTemplate in pEpoch.m_cNativesRaceTemplates)
            //{
            //    bool bAlreadyHave = false;
            //    if(m_aLocalRaces != null)
            //        foreach (Race pRace in m_aLocalRaces)
            //            if (pRace.m_pTemplate == pRaceTemplate && !pRace.m_bDying)
            //                bAlreadyHave = true;
                
            //    cRaceChances[pRaceTemplate] = bAlreadyHave ? 0 : 100.0f;// / pRaceTemplate.m_iRank;
            //}
            //cRaceChances[pRace] = (m_aLocalRaces != null && m_aLocalRaces.Contains(pRace)) ? 0 : Math.Max(1, 10 + m_iMagicLimit * 10 - pRace.m_iRank);// : 100.0f / pRace.m_iRank;

            //Добавляем необходимое количесто новых рас.
            while (cNations.Count - iDyingRaces < pEpoch.m_iNativesCount)
            {
                int iChance = Rnd.Get(pEpoch.m_cNatives.Count);
                //foreach (RaceTemplate pRaceTemplate in cRaceChances.Keys)
                //{
                    //iChance--;
                    //if (iChance < 0)
                    //{
                        Nation pNation = new Nation(pEpoch.m_cNatives[iChance], pEpoch);//new Race(pRaceTemplate, pEpoch);
                        pNation.Accommodate(pEpoch);

                        cNations.Add(pNation);
                        //cRaceChances[pRaceTemplate] = 0;
                        //break;
                    //}
                //}
            }

            m_aLocalNations = cNations.ToArray();
        }

        private void AddInvadersRaces(Epoch pEpoch)
        {
            if (pEpoch.m_iInvadersCount == 0)
                return;

            List<Nation> cNations = new List<Nation>(m_aLocalNations);

            //Рассчитываем шансы новых рас попасть в новый мир - учитывая, его параметры
            Dictionary<Race, float> cInvadersRaceChances = new Dictionary<Race, float>();
            foreach (Race pRace in Race.m_cAllRaces)
            {
                if (!pEpoch.m_cInvaders.Contains(pRace))
                    continue;

                bool bAlreadyHave = false;
                if (m_aLocalNations != null)
                    foreach (Nation pNation in m_aLocalNations)
                        if (pNation.m_pRace == pRace && !pNation.IsAncient)
                            bAlreadyHave = true;

                cInvadersRaceChances[pRace] = bAlreadyHave ? 10 : 100;
            }

            for (int i = 0; i < pEpoch.m_iInvadersCount; i++)
            {
                int iChance = Rnd.ChooseOne(cInvadersRaceChances.Values, 1);
                foreach (Race pRaceTemplate in cInvadersRaceChances.Keys)
                {
                    iChance--;
                    if (iChance < 0)
                    {
                        Nation pNation = new Nation(pRaceTemplate, pEpoch, true);
                        pNation.Accommodate(pEpoch);

                        cNations.Add(pNation);
                        cInvadersRaceChances[pRaceTemplate] = 0;
                        break;
                    }
                }
            }

            m_aLocalNations = cNations.ToArray();
        }

        private void DistributeRacesToLandMasses()
        {
            //Dictionary<Language, List<Race>> cLanguages = new Dictionary<Language, List<Race>>();

            ////разобъём расы по языковым группам
            //foreach (Race pRace in m_aLocalRaces)
            //{
            //    List<Race> cRaces;
            //    if (!cLanguages.TryGetValue(pRace.m_pTemplate.m_pLanguage, out cRaces))
            //    {
            //        cRaces = new List<Race>();
            //        cLanguages[pRace.m_pTemplate.m_pLanguage] = cRaces;
            //    }
            //    cRaces.Add(pRace);
            //}


            //Распределим расы по наиболее подходящим им тектоническим плитам (чтобы все расы хоть где-то да жили)
            foreach (Nation pNation in m_aLocalNations)
            {
                //убедимся, что эта раса ещё нигде не живёт
                bool bHomeless = true;
                foreach (Continent pConti in Contents)
                {
                    ContinentX pContiX = pConti.As<ContinentX>();
                    if (pContiX != null)
                    {
                        foreach (LandMass pLandMass in pConti.Contents)
                            if (pContiX.m_cLocalNations.ContainsKey(pLandMass) && pContiX.m_cLocalNations[pLandMass].Contains(pNation))
                            {
                                bHomeless = false;
                                break;
                            }
                        if (!bHomeless)
                            break;
                    }
                }

                //если уже где-то живёт и не гегемон - пропускаем её
                if (!bHomeless)// && !pRace.m_bHegemon)
                    continue;

                //рассчитаем вероятности для всех регионов стать прародиной этой расы
                Dictionary<Region, float> cRegionChances = new Dictionary<Region, float>();
                foreach (Continent pContinent in Contents)
                {
                    ContinentX pContiX = pContinent.As<ContinentX>();
                    if (pContiX != null)
                    {
                        foreach (Region pRegion in pContiX.m_cRegions)
                        {
                            //отсеиваем уже занятые и непригодные для заселения регионы
                            if (pRegion.Forbidden ||
                                pRegion.HasOwner() ||
                                pRegion.IsWater)
                                continue;

                            cRegionChances[pRegion] = 1.0f;

                            //рассчитываем шансы, исходя из предпочтений и антипатий расы
                            foreach (LandTypeInfo pType in pNation.m_aPreferredLands)
                                if (pRegion.m_pType == pType)
                                    cRegionChances[pRegion] *= 100;

                            foreach (LandTypeInfo pType in pNation.m_aHatedLands)
                                if (pRegion.m_pType == pType)
                                    cRegionChances[pRegion] /= 1000;

                            //смотрим, сколько ещё других рас уже живут на этом же континенте и говорят ли они на нашем языке
                            int iPop = 0;
                            bool bSameLanguage = false;
                            foreach (var pRaces in pRegion.Continent.As<ContinentX>().m_cLocalNations)
                            {
                                foreach (var pRce in pRaces.Value)
                                {
                                    if (!pRce.IsAncient)
                                        iPop++;

                                    if (pRce.m_pRace.m_pLanguage == pNation.m_pRace.m_pLanguage)
                                        bSameLanguage = true;
                                }
                            }

                            if (iPop > 0)
                            {
                                //снижаем привлекательность земли в зависимости от количества конкурентов.
                                //гегемоны и вторженцы игнорируют эту проверку.
                                if (!pNation.IsHegemon && !pNation.IsInvader && !bSameLanguage)
                                    cRegionChances[pRegion] = (float)Math.Pow(cRegionChances[pRegion], 1.0 / (1 + iPop));

                                //смотрим, на каких языках говорят другие уже живущие здесь расы
                                //и снижаем либо повышаем привлекательность земли в зависимости от совпдения языка
                                if (bSameLanguage)
                                    cRegionChances[pRegion] *= 100;
                                else
                                    cRegionChances[pRegion] /= 100;
                            }
                        }
                    }
                }

                int iChance = Rnd.ChooseOne(cRegionChances.Values, 2);
                Region pCradle = cRegionChances.ElementAt(iChance).Key;

                if (pCradle != null)
                {
                    pCradle.m_pNatives = pNation;
                    pCradle.m_sName = pNation.m_pRace.m_pLanguage.RandomCountryName();
                    foreach (LandX pLand in pCradle.Contents)
                    {
                        pLand.Populate(pCradle.m_pNatives, pCradle.m_sName);
                    }

                    LandMass pLandMass = pCradle.LandMass;
                    if (pLandMass != null)
                    {
                        if (!pCradle.Continent.As<ContinentX>().m_cLocalNations.ContainsKey(pLandMass))
                            pCradle.Continent.As<ContinentX>().m_cLocalNations[pLandMass] = new List<Nation>();
                        
                        pCradle.Continent.As<ContinentX>().m_cLocalNations[pLandMass].Add(pNation);
                    }
                }
            }

            //Позаботимся о том, чтобы на каждом континенте жила бы хоть одна раса
            foreach (Continent pConti in Contents)
            {
                //если континент уже обитаем, пропускаем его
                int iPop = 0;
                foreach (var pNations in pConti.As<ContinentX>().m_cLocalNations)
                {
                    foreach (var pNation in pNations.Value)
                        if (!pNation.IsAncient && !pNation.DominantPhenotype.m_pValues.Get<NutritionGenetix>().IsParasite())
                            iPop++;
                }

                if (iPop > 0)
                    continue;

                LandMass pLandMass = pConti.Contents.ElementAt(Rnd.Get(pConti.Contents.Count));

                pConti.As<ContinentX>().m_cLocalNations[pLandMass] = new List<Nation>();

                Dictionary<LandTypeInfo, int> cLandTypesCount = new Dictionary<LandTypeInfo, int>();
                foreach (Land pLand in pLandMass.Contents)
                {
                    if (pLand.IsWater)
                        continue;

                    if (!cLandTypesCount.ContainsKey(pLand.LandType))
                        cLandTypesCount[pLand.LandType] = 0;

                    cLandTypesCount[pLand.LandType] += pLand.Contents.Count;
                }

                Dictionary<Nation, float> cNationChances = new Dictionary<Nation, float>();
                foreach (Nation pNation in m_aLocalNations)
                {
                    if (pNation.IsAncient || pNation.DominantPhenotype.m_pValues.Get<NutritionGenetix>().IsParasite())
                        continue;

                    cNationChances[pNation] = 1.0f;

                    foreach (LandTypeInfo pType in pNation.m_aPreferredLands)
                        if (cLandTypesCount.ContainsKey(pType))
                            cNationChances[pNation] *= cLandTypesCount[pType];

                    foreach (LandTypeInfo pType in pNation.m_aHatedLands)
                        if (cLandTypesCount.ContainsKey(pType))
                            cNationChances[pNation] /= cLandTypesCount[pType];
                }

                int iChance = Rnd.ChooseOne(cNationChances.Values, 2);
                Nation pChoosenNation = null;
                foreach (Nation pNation in cNationChances.Keys)
                {
                    iChance--;
                    if (iChance < 0)
                    {
                        pChoosenNation = pNation;
                        break;
                    }
                }

                if (pChoosenNation != null)
                {
                    pConti.As<ContinentX>().m_cLocalNations[pLandMass].Add(pChoosenNation);
                    if (pConti.Contents.Count == 1)
                        pConti.As<ContinentX>().m_sName = pChoosenNation.m_pRace.m_pLanguage.RandomCountryName();
                }
            }
        }

        private void PopulateAreas()
        {
            foreach (Continent pConti in Contents)
            {
                List<Nation> cHegemonNations = new List<Nation>();
                //список рас, способных свободно расселяться по континенту
                List<Nation> cAvailableContinentNations = new List<Nation>();
                foreach (LandMass pLandMass in pConti.As<ContinentX>().m_cLocalNations.Keys)
                {
                    foreach (Nation pNation in pConti.As<ContinentX>().m_cLocalNations[pLandMass])
                    {
                        if (!pNation.IsAncient && !cAvailableContinentNations.Contains(pNation))
                            cAvailableContinentNations.Add(pNation);
                        if (pNation.IsHegemon && !cHegemonNations.Contains(pNation))
                            cHegemonNations.Add(pNation);
                    }
                }

                //List<Race> cSettledDyingRaces = new List<Race>();
                //переберём все пригодные для заселения и незаселённые территории
                foreach (Region pRegion in pConti.As<ContinentX>().m_cRegions)
                {
                    if (!pRegion.IsWater && pRegion.m_pNatives == null)
                    {
                        List<LandMass> cLandMasses = new List<LandMass>();
                        cLandMasses.Add(pRegion.LandMass);

                        //составим список рас, которые могут претендовать на эту территорию
                        List<Nation> cAvailableNations = GetAvailableNations(cLandMasses);

                        while (cAvailableNations.Count == 0)
                        {
                            List<LandMass> cLandMassesExtended = new List<LandMass>(cLandMasses);

                            foreach (LandMass pLandMass in cLandMasses)
                                foreach (LandMass pLinkedLandMass in pLandMass.m_aBorderWith)
                                {
                                    if (pLinkedLandMass.Forbidden)
                                        continue;
                                    if (!pLinkedLandMass.IsWater && !cLandMassesExtended.Contains(pLinkedLandMass))
                                        cLandMassesExtended.Add(pLinkedLandMass);
                                }

                            cLandMasses = cLandMassesExtended;
                            cAvailableNations = GetAvailableNations(cLandMasses);
                        }

                        //если эту территория можно отнести к одной или нескольким тектоническим плитам с коренным населением, населяем её соответствующими расами,
                        //иначе берём список рас по континенту в целом
                        if (cAvailableNations.Count > 0)
                        {
                            //cAvailableRaces.AddRange(cDyingRaces);
                            //cAvailableRaces.AddRange(cHegemonRaces);
                            pRegion.SetRace(cAvailableNations);

                            //if (cDyingRaces.Contains(pArea.m_pRace))
                            //    cSettledDyingRaces.Add(pArea.m_pRace);
                        }
                        else
                            pRegion.SetRace(cAvailableContinentNations);
                    }
                }
            }
        }

        private List<Nation> GetAvailableNations(List<LandMass> cLandMasses)
        {
            List<Nation> cAvailableNations = new List<Nation>();
            //List<Race> cDyingRaces = new List<Race>();
            foreach (LandMass pLandMass in cLandMasses)
            {
                ContinentX pConti = pLandMass.GetOwner().As<ContinentX>();

                if (!pConti.m_cLocalNations.ContainsKey(pLandMass))
                    continue;

                foreach (Nation pNation in pConti.m_cLocalNations[pLandMass])
                    if (!pNation.IsAncient)
                    {
                        if (!cAvailableNations.Contains(pNation))
                        {
                            cAvailableNations.Add(pNation);
                            if (!m_aLocalNations.Contains(pNation))
                                throw new InvalidOperationException();
                        }
                    }
                //else
                //{
                //    if (!cSettledDyingRaces.Contains(pRace) && !cDyingRaces.Contains(pRace))
                //    {
                //        cDyingRaces.Add(pRace);
                //        if (!m_aLocalRaces.Contains(pRace))
                //            throw new Exception();
                //    }
                //}
            }

            return cAvailableNations;
        }

        /// <summary>
        /// Генерация мира
        /// </summary>
        /// <param name="cLocations">Сетка локаций, на которой будет строиться мир.</param>
        /// <param name="iContinents">Общее количество континентов - обособленных, разделённых водой участков суши.</param>
        /// <param name="bGreatOcean">Если true, то карта со всех сторон окружена водой.</param>
        /// <param name="iLandsDiversity">Общее количество `земель` - групп соседних локаций с одинаковым типом территории</param>
        /// <param name="iProvinces">Общее количество провинций. Каждая провинция объединяет несколько сопредельных земель.</param>
        /// <param name="iStates">Общее количество государств. Каждое государство объединяет несколько сопредельных провинций.</param>
        /// <param name="iLandMassesDiversity">Общее количество тектонических плит, являющихся строительными блоками при составлении континентов.</param>
        /// <param name="iOcean">Процент тектонических плит, лежащих на дне океана - от 10 до 90.</param>
        /// <param name="iEquator">Положение экватора на карте в процентах по вертикали. 50 - середина карты, 0 - верхний край, 100 - нижний край</param>
        /// <param name="iPole">Расстояние от экватора до полюсов в процентах по вертикали. Если экватор расположен посередине карты, то значение 50 даст для полюсов верхний и нижний края карты соответственно.</param>
        /// <param name="aEpoches">Информация об эпохах развития мира (расы, техническое развитие, магические способности, пришельцы и т.д....)</param>
        public World(int iLocations,
                     int iResolution,
                     int iContinents,
                     bool bGreatOcean,
                     int iLandsDiversity,
                     int iProvinces,
                     int iStates,
                     int iLandMassesDiversity,
                     int iOcean,
                     int iEquator,
                     int iPole,
                     Epoch[] aEpoches,
                     BeginStepDelegate BeginStep,
                     ProgressStepDelegate ProgressStep)
            : base(iLocations, iResolution, iContinents, bGreatOcean, iLandsDiversity, iLandMassesDiversity, iOcean, iEquator, iPole, BeginStep, ProgressStep)
        {
            Create(iProvinces, iStates, aEpoches, BeginStep, ProgressStep);
        }

        public Epoch[] m_aEpoches;

        private void Create(int iProvinces, 
                            int iStates,
                            Epoch[] aEpoches,
                            BeginStepDelegate BeginStep,
                            ProgressStepDelegate ProgressStep)
        {
            foreach (Location pLoc in m_pLocationsGrid.Locations)
                pLoc.AddLayer(new LocationX(pLoc));

            foreach (Location pLoc in m_pLocationsGrid.Locations)
                pLoc.As<LocationX>().FillBorderWithKeys();

            foreach (Land pLand in m_aLands)
                pLand.AddLayer(new LandX(pLand));

            foreach (Land pLand in m_aLands)
                pLand.As<LandX>().FillBorderWithKeys();

            foreach (Continent pConti in Contents)
                pConti.AddLayer(new ContinentX(pConti));

            foreach (Continent pConti in Contents)
                pConti.As<ContinentX>().FillBorderWithKeys();

            BuildRegions(BeginStep, ProgressStep);

            foreach (Race pRace in Race.m_cAllRaces)
                pRace.ResetNations();
            Language.ResetUsedLists();
            Epoch.s_cUsedNames.Clear();

            m_iProvincesCount = iProvinces;
            m_iStatesCount = iStates;

            m_aEpoches = aEpoches;

            int iCounter = m_aEpoches.Length;
            foreach (Epoch pEpoch in m_aEpoches)
            {
                Reset(pEpoch);

                for (int i = 0; i < pEpoch.m_iLength - 1; i++)
                {
                    BeginStep("Simulating history - " + pEpoch.m_sName + " (" + (i+1).ToString() + ")...", 8);
                    PopulateWorld(pEpoch, false, BeginStep, ProgressStep);
                    Reset(pEpoch);
                }

                iCounter--;

                BeginStep("Simulating history - " + pEpoch.m_sName + " (" + pEpoch.m_iLength.ToString() + ")...", 8);
                PopulateWorld(pEpoch, iCounter == 0, BeginStep, ProgressStep);
            }
        }

        public void BuildRegions(BeginStepDelegate BeginStep,
                            ProgressStepDelegate ProgressStep)
        {
            BeginStep("Building regions...", Contents.Length);
            //Gathering areas of the same land type
            foreach (Continent pContinent in Contents)
            {
                pContinent.As<ContinentX>().BuildRegions(m_pLocationsGrid.CycleShift, m_aLands.Length / 100);
                ProgressStep();
            }
        }

        private void PopulateWorld(Epoch pEpoch, bool bFinalize,
                            BeginStepDelegate BeginStep,
                            ProgressStepDelegate ProgressStep)
        {
            BeginStep("Growing states...", 6);

            AddRaces(pEpoch);
            if (bFinalize)
                AddInvadersRaces(pEpoch);
            ProgressStep();

            //BeginStep("Distributing races...", 1);
            DistributeRacesToLandMasses();
            ProgressStep();
            
            //BeginStep("Populating lands...", 1);
            PopulateAreas();
            ProgressStep();

            //BeginStep("Building provinces...", 1);
            BuildProvinces();
            ProgressStep();

            //BeginStep("Building cities...", 1);
            BuildCities(m_pLocationsGrid.CycleShift, !bFinalize, BeginStep, ProgressStep);
            ProgressStep();

            //BeginStep("Building states...", 1);
            BuildStates(m_pLocationsGrid.CycleShift, !bFinalize, BeginStep, ProgressStep);
            ProgressStep();

            //if (bFinalize)
            //    BuildSeaRoutes(m_pGrid.CycleShift);

            BeginStep("Specializing settlements...", m_aProvinces.Length);
            foreach (State pState in m_aStates)
            {
                if (!pState.Forbidden)
                    pState.SpecializeSettlements();
                ProgressStep();
            }

            BeginStep("Adding buildings...", m_aProvinces.Length);
            foreach (State pState in m_aStates)
            {
                if (!pState.Forbidden)
                {
                    foreach (LocationX pLoc in pState.m_pSociety.Settlements)
                    {
                        pState.m_pSociety.AddBuildings(pLoc.m_pSettlement);
                    }
                }
                pState.m_pSociety.SetEstates();
                ProgressStep();
            }

            BeginStep("Renaming provinces...", m_aStates.Length);
            //ProgressStep();
            foreach (Continent pConti in Contents)
            {
                foreach (State pState in pConti.As<ContinentX>().Contents)
                {
                    //если какой-то регион занимает больше половины провинции - переименовываем провинцию по его имени.
                    foreach (Province pProvince in pState.Contents)
                    {
                        int iBiggestSize = 0;
                        int iTotalSize = 0;
                        Region pBiggestRegion = null;
                        foreach(Region pRegion in pProvince.Contents)
                        {
                            iTotalSize += pRegion.Contents.Count;
                            if(pRegion.Contents.Count > iBiggestSize)
                            {
                                iBiggestSize = pRegion.Contents.Count;
                                pBiggestRegion = pRegion;
                            }
                        }
                        
                        if(pBiggestRegion != null && iBiggestSize > iTotalSize/2)
                        {
                            pProvince.m_pLocalSociety.m_sName = pBiggestRegion.m_sName;
                        }
                    }

                    //если государство состоит из единственной провинции, то переименовываем государство по имени провинции
                    if (pState.Contents.Count == 1)
                        pState.m_pSociety.m_sName = pState.Contents.First().m_pLocalSociety.m_sName;

                    pState.m_pSociety.CalculateMagic();
                    
                    ProgressStep();
                }

                ////если на континенте единственное государство, то переименовываем континент по имени этого государства
                //if (pConti.m_cStates.Count == 1)
                //{
                //    //если на континенте одно государство и один регион, то государство должно называться по имени этого региона,
                //    //независимо от того, из скольки провинций оно состоит
                //    if (pConti.m_cRegions.Count == 1)
                //        pConti.m_cStates[0].m_pSociety.m_sName = pConti.m_cRegions[0].m_sName;

                //    pConti.m_sName = pConti.m_cStates[0].m_pSociety.m_sName;
                //}
                //else
                //    //если государств несколько, но территория одна - переименовываем континент по имени этой территории
                //    if (pConti.m_cRegions.Count == 1)
                //        pConti.m_sName = pConti.m_cRegions[0].m_sName;

            }

            //ProgressStep();
        }

        private void BuildProvinces()
        {
            List<Continent> cUsed = new List<Continent>();

            List<Province> cProvinces = new List<Province>();

            //Заново стартуем провинции, выжившие с прошлой эпохи
            if(m_aProvinces != null)
                foreach (Province pProvince in m_aProvinces)
                {
                    if (pProvince.m_pCenter.Continent != null && !cUsed.Contains(pProvince.m_pCenter.Continent))
                        cUsed.Add(pProvince.m_pCenter.Continent);

                    pProvince.Start(pProvince.m_pCenter);
                    if (!m_aLocalNations.Contains(pProvince.m_pLocalSociety.m_pTitularNation))
                        throw new Exception();

                    cProvinces.Add(pProvince);
                }
            
            List<Province> cFinished = new List<Province>();
            //Позаботимся о том, чтобы на каждом континенте была хотя бы одна провинция
            foreach (Continent pConti in Contents)
            {
                int iUsed = 0;

                ContinentX pContiX = pConti.As<ContinentX>();

                //Для каждого анклава древней цивилизации создаём собственную провинцию
                foreach (Region pRegion in pContiX.m_cRegions)
                {
                    if (pRegion.m_pNatives != null && pRegion.m_pNatives.IsAncient)
                    {
                        if (!pRegion.HasOwner())
                        {
                            Province pLostProvince = new Province();
                            pLostProvince.Start(pRegion);
                            cProvinces.Add(pLostProvince);
                            while (pLostProvince.Grow(pRegion.Contents.Count) != null) { }
                            cFinished.Add(pLostProvince);
                        }
                        iUsed++;
                    }
                }

                //если на континенте есть хоть одна провинция - дальше делать нечего
                if (cUsed.Contains(pConti) || pContiX.m_cRegions.Count == iUsed)
                    continue;

                //иначе ищем подходящее место дня новой провинции
                Region pSeed = null;
                do
                {
                    int iRegionIndex = Rnd.Get(pContiX.m_cRegions.Count);
                    if (!pContiX.m_cRegions[iRegionIndex].IsWater)
                    {
                        pSeed = pContiX.m_cRegions[iRegionIndex];

                        bool bBorder = false;
                        foreach (LandX pLand in pSeed.Contents)
                            foreach (Location pLoc in pLand.As<Land>().Contents)
                                if (pLoc.m_bBorder)
                                    bBorder = true;

                        if (pSeed.m_pNatives.IsAncient || (bBorder && !Rnd.OneChanceFrom(25)))
                            pSeed = null;
                    }
                }
                while (pSeed == null);

                //создаём новую провинцию в найденном месте
                Province pProvince = new Province();
                pProvince.Start(pSeed);
                cProvinces.Add(pProvince);

                if (!m_aLocalNations.Contains(pProvince.m_pLocalSociety.m_pTitularNation))
                    throw new InvalidOperationException();

                cUsed.Add(pConti);
            }

            //теперь догоним общее число провинций до заданного
            while (cProvinces.Count < m_iProvincesCount)
            {
                //ищем подходящее место дня новой провинции
                Region pSeed = null;
                int iCounter = 0;
                do
                {
                    int iIndex = Rnd.Get(m_aLands.Length);
                    pSeed = m_aLands[iIndex].As<LandX>().GetOwner();

                    if (pSeed != null)
                    {
                        if (pSeed.Forbidden ||
                            pSeed.HasOwner() ||
                            pSeed.IsWater ||
                            pSeed.LandMass.IsWater ||
                            (pSeed.m_pNatives != null && pSeed.m_pNatives.IsAncient))
                        {
                            pSeed = null;
                        }
                        else
                        {
                            bool bBorder = false;
                            foreach (LandX pLand in pSeed.Contents)
                                foreach (Location pLoc in pLand.As<Land>().Contents)
                                    if (pLoc.m_bBorder)
                                        bBorder = true;

                            if (bBorder)
                                pSeed = null;
                        }
                    }

                    if (pSeed == null)
                        iCounter++;
                    else
                        iCounter = 0;
                }
                while (pSeed == null && iCounter < m_aLands.Length * 2);//ищем подходящее место дня новой провинции

                if (pSeed == null)
                    break;

                //создаём новую провинцию в найденном месте
                Province pProvince = new Province();
                pProvince.Start(pSeed);
                cProvinces.Add(pProvince);

                if (!m_aLocalNations.Contains(pProvince.m_pLocalSociety.m_pTitularNation))
                    throw new InvalidOperationException();
            }

            //наращиваем территорию всем созданным провинциям до вычисленного максимума
            int iMaxProvinceSize = (m_aLands.Length * (100 - m_iOceansPercentage) / 100) / m_iProvincesCount;
            do
            {
                foreach (Province pProvince in cProvinces)
                {
                    if (pProvince.m_bFullyGrown)
                        continue;

                    if (pProvince.Grow(iMaxProvinceSize) == null)
                        cFinished.Add(pProvince);
                }
            }
            while (cFinished.Count < cProvinces.Count);

            //убедимся, что провинции заняли весь континент.
            //если остались ничейные земли - будем принудительно наращивать все провинции, пока пустых мест не останется.
            bool bAlreadyFinished = true;
            int iCnt = 0;
            foreach (Province pProvince in cProvinces)
                pProvince.m_bFullyGrown = false;

            do
            {
                bAlreadyFinished = true;
                foreach (Continent pConti in Contents)
                {
                    foreach (Region pRegion in pConti.As<ContinentX>().m_cRegions)
                    {
                        if (!pRegion.Forbidden && !pRegion.IsWater && !pRegion.HasOwner())
                        {
                            bAlreadyFinished = false;
                            break;
                        }
                    }
                }

                if (bAlreadyFinished)
                    break;

                foreach (Province pProvince in cProvinces)
                {
                    if (pProvince.m_bFullyGrown)
                        continue;

                    if (!pProvince.ForcedGrow())
                        cFinished.Add(pProvince);
                }
            }
            while (iCnt++ < m_aLands.Length);

            //если всё равно остались пыстые земли (это возможно, т.к. провинции вымирающих рас принудительно не приращиваются), создадим там новые провинции
            foreach (Continent pConti in Contents)
            {
                foreach (Region pRegion in pConti.As<ContinentX>().m_cRegions)
                {
                    if (!pRegion.Forbidden && !pRegion.IsWater && !pRegion.HasOwner())
                    {
                        Province pProvince = new Province();
                        pProvince.Start(pRegion);
                        cProvinces.Add(pProvince);
                        while (pProvince.Grow(iMaxProvinceSize) != null) { }
                    }
                }
            }

            m_aProvinces = cProvinces.ToArray();

            foreach (Province pProvince in m_aProvinces)
                pProvince.Finish(m_pLocationsGrid.CycleShift);
        }

        /// <summary>
        /// Распределяем провинции, строим столицы, налаживаем дипломатию и строим форты.
        /// </summary>
        /// <param name="fCycleShift"></param>
        /// <param name="bFast"></param>
        /// <param name="BeginStep"></param>
        /// <param name="ProgressStep"></param>
        private void BuildStates(float fCycleShift, bool bFast,
                            BeginStepDelegate BeginStep,
                            ProgressStepDelegate ProgressStep)
        {
            BeginStep("Building states...", 10);

            List<ContinentX> cUsed = new List<ContinentX>();

            List<State> cStates = new List<State>();

            //Заново стартуем государства, выжившие с прошлой эпохи
            if(m_aStates != null)
                foreach (State pState in m_aStates)
                {
                    pState.Start(pState.m_pMethropoly);
                    if(!m_aLocalNations.Contains(pState.m_pSociety.m_pTitularNation))
                        throw new Exception();
            
                    ContinentX pConti = pState.GetOwner();
                    if (!cUsed.Contains(pConti))
                        cUsed.Add(pConti);

                    cStates.Add(pState);
                }

            ProgressStep();
            
            //Позаботимся о том, чтобы у каждой расы пришельцев было хотя бы одно своё государство
            foreach(Nation pInvader in m_aLocalNations)
                if (pInvader.IsInvader)
                {
                    List<Province> cSeeds = new List<Province>();
                    foreach (Province pProvince in m_aProvinces)
                        if (pProvince.m_pLocalSociety.m_pTitularNation == pInvader && !pProvince.HasOwner())
                            cSeeds.Add(pProvince);

                    if(cSeeds.Count > 0)
                    {
                        Province pSeed = cSeeds[Rnd.Get(cSeeds.Count)];
                        State pState = new State();
                        pState.Start(pSeed);
                        cStates.Add(pState);

                        if (!m_aLocalNations.Contains(pState.m_pSociety.m_pTitularNation))
                            throw new Exception();
                        cUsed.Add(pSeed.m_pCenter.Continent.As<ContinentX>());
                    }
                }

            ProgressStep();

            //Позаботимся о том, чтобы на каждом континенте было хотя бы одно государство
            foreach (Continent pConti in Contents)
            {
                ContinentX pContiX = pConti.As<ContinentX>();

                if (cUsed.Contains(pContiX))
                    continue;

                Province pSeed = null;
                do
                {
                    int iIndex = Rnd.Get(pContiX.m_cRegions.Count);
                    if(!pContiX.m_cRegions[iIndex].IsWater)
                    {
                        pSeed = pContiX.m_cRegions[iIndex].GetOwner();

                        if (pSeed.IsBorder() || pSeed.HasOwner())
                            pSeed = null;
                    }
                }
                while (pSeed == null); 
                
                State pState = new State();
                pState.Start(pSeed);
                cStates.Add(pState);

                if (!m_aLocalNations.Contains(pState.m_pSociety.m_pTitularNation))
                    throw new Exception();
                cUsed.Add(pContiX);
            }

            ProgressStep();

            //догоняем количество государств до заданного числа
            while(cStates.Count < m_iStatesCount)
            {
                Province pSeed = null;
                do
                {
                    int iIndex = Rnd.Get(m_aProvinces.Length);
                    pSeed = m_aProvinces[iIndex];

                    if (pSeed.Forbidden ||
                        pSeed.m_pCenter.IsWater ||
                        pSeed.HasOwner() ||
                        pSeed.IsBorder())
                    {
                        pSeed = null;
                    }
                }
                while (pSeed == null); 

                State pState = new State();
                pState.Start(pSeed);
                cStates.Add(pState);
                if (!m_aLocalNations.Contains(pState.m_pSociety.m_pTitularNation))
                    throw new Exception();
            }

            ProgressStep();

            //приращиваем территории всех государств до заданного лимита
            int iMaxStateSize = m_aProvinces.Length / cStates.Count;
            bool bContinue = false;
            do
            {
                bContinue = false;
                foreach (State pState in cStates)
                    if (pState.Grow(iMaxStateSize) != null)
                        bContinue = true;
            }
            while (bContinue);

            ProgressStep();

            //убедимся, что государства заняли весь континент.
            //если остались ничейные провинции - будем принудительно наращивать все государства, пока пустых мест не останется.
            bool bAlreadyFinished = true;
            int iCnt = 0;

            do
            {
                bAlreadyFinished = true;
                foreach (Province pProvince in m_aProvinces)
                    if (!pProvince.Forbidden && !pProvince.HasOwner())
                    {
                        bAlreadyFinished = false;
                        break;
                    }

                if (bAlreadyFinished)
                    break;

                foreach (State pState in cStates)
                    pState.ForcedGrow();
            }
            while (iCnt++ < m_aProvinces.Length);

            ProgressStep();

            //если остались незанятые провинции - стартуем там дополнительные государства и приращиваем их территории до заданного лимита
            foreach(Province pProvince in m_aProvinces)
            {
                if (!pProvince.Forbidden && !pProvince.HasOwner())
                {
                    State pState = new State();
                    pState.Start(pProvince);
                    cStates.Add(pState);
                    while (pState.Grow(iMaxStateSize) != null) { }
                }
            }

            m_aStates = cStates.ToArray();

            ProgressStep();

            //распределяем государства по континентам
            foreach (State pState in m_aStates)
            {
                ContinentX pConti = pState.GetOwner();

                if (pConti == null)
                {
                    foreach (Province pProvince in pState.Contents)
                    {
                        foreach (Region pRegion in pProvince.Contents)
                        {
                            pConti = pRegion.Continent.As<ContinentX>();
                            break;
                        }
                        if (pConti != null)
                            break;
                    }

                    pState.SetOwner(pConti);
                }
                pConti.Contents.Add(pState);
            }
            //строим столицы, налаживаем дипломатические связи
            foreach (State pState in m_aStates)
            {
                pState.BuildCapital(m_aProvinces.Length / (2 * m_iStatesCount), m_aProvinces.Length / m_iStatesCount, bFast);
                pState.Finish(m_pLocationsGrid.CycleShift);
            }

            ProgressStep();

            //сглаживаем культурные различия соседних стран
            //высокоразвитые страны "подтягивают" более отсталые, но не наоборот
            foreach (State pState in m_aStates)
            {
                int iCounter = 0;
                int iSum = 0;

                foreach (State pLinkedState in pState.m_aBorderWith)
                {
                    if (pLinkedState.Forbidden)
                        continue;

                    iSum += Math.Max(pLinkedState.m_pSociety.m_iSocialEquality, pState.m_pSociety.m_iSocialEquality);

                    iCounter++;
                }

                pState.m_pSociety.m_iSocialEquality = (pState.m_pSociety.m_iSocialEquality + iSum) / (iCounter + 1);
            }

            ProgressStep();

            //строим форты на границах враждующих государств и фиксим дороги
            Dictionary<State, Dictionary<State, int>> cHostility = new Dictionary<State, Dictionary<State, int>>();
            foreach (State pState in m_aStates)
            {
                if (!pState.Forbidden)
                {
                    pState.BuildForts(cHostility, bFast);

                    if (!bFast)
                        pState.FixRoads(fCycleShift);
                }
            }
        }

        private void BuildCities(float fCycleShift, bool bFast,
                            BeginStepDelegate BeginStep,
                            ProgressStepDelegate ProgressStep)
        {
            BeginStep("Building cities...", m_aProvinces.Length * 2 + 1);

            foreach (Province pProvince in m_aProvinces)
            {
                if (!pProvince.Forbidden)
                {
                    pProvince.BuildCapital(bFast);

                    pProvince.BuildSettlements(SettlementSize.City, bFast);
                    if (!bFast)
                        pProvince.BuildRoads(RoadQuality.Good, fCycleShift);

                    pProvince.BuildSettlements(SettlementSize.Town, bFast);

                    if (!bFast)
                        pProvince.BuildRoads(RoadQuality.Normal, fCycleShift);
                }

                ProgressStep();
            }

            if (!bFast)
                BuildInterstateRoads(fCycleShift);

            ProgressStep();

            foreach (Province pProvince in m_aProvinces)
            {
                if (!pProvince.Forbidden)
                {
                    pProvince.BuildSettlements(SettlementSize.Village, bFast);

                    if (!bFast)
                        pProvince.BuildRoads(RoadQuality.Country, fCycleShift);

                    pProvince.BuildSettlements(SettlementSize.Hamlet, bFast);

                    pProvince.BuildLairs(m_aLands.Length / 125);
                }

                ProgressStep();
            }
        }

        public override void PresetLandTypesInfo()
        {
            base.PresetLandTypesInfo();

            LandTypes.Coastral.AddLayer(new ResourcesInfo(0, 0, 0, 0));
            LandTypes.Coastral.AddLayer(new StandAloneBuildingsInfo(0, 0, 1));
            LandTypes.Coastral.AddLayer(new SettlementsInfo(0, 0, 0));

            LandTypes.Ocean.AddLayer(new ResourcesInfo(0, 0, 0, 0));
            LandTypes.Ocean.AddLayer(new StandAloneBuildingsInfo(0, 0, 1));
            LandTypes.Ocean.AddLayer(new SettlementsInfo(0, 0, 0));

            LandTypes.Tundra.AddLayer(new ResourcesInfo(0, 0.1f, 0, 0));
            LandTypes.Tundra.AddLayer(new StandAloneBuildingsInfo(1, 0, 10));
            LandTypes.Tundra.AddLayer(new SettlementsInfo(0.004f, 0.02f, 0.05f));
//            LandTypes.Tundra.SetSettlementsDensity(0.004f, 0.01f, 0.003f);
            
            LandTypes.Plains.AddLayer(new ResourcesInfo(3, 0, 0, 0));
            LandTypes.Plains.AddLayer(new StandAloneBuildingsInfo(1, 3, 30));
            LandTypes.Plains.AddLayer(new SettlementsInfo(0.02f, 0.1f, 0.3f));
//            LandTypes.Plains.SetSettlementsDensity(0.01f, 0.026f, 0.1f);
            
            LandTypes.Savanna.AddLayer(new ResourcesInfo(0.1f, 0.2f, 0, 0));
            LandTypes.Savanna.AddLayer(new StandAloneBuildingsInfo(1, 3, 20));
            LandTypes.Savanna.AddLayer(new SettlementsInfo(0.01f, 0.05f, 0.2f));
//            LandTypes.Savanna.SetSettlementsDensity(0.01f, 0.023f, 0.02f);
            
            LandTypes.Desert.AddLayer(new ResourcesInfo(0, 0.1f, 0, 0));
            LandTypes.Desert.AddLayer(new StandAloneBuildingsInfo(1, 2, 30));
            LandTypes.Desert.AddLayer(new SettlementsInfo(0.01f, 0.03f, 0.025f));
//            LandTypes.Desert.SetSettlementsDensity(0.006f, 0.01f, 0.003f);
            
            LandTypes.Forest.AddLayer(new ResourcesInfo(0, 1, 5, 0));
            LandTypes.Forest.AddLayer(new StandAloneBuildingsInfo(10, 5, 10));
            LandTypes.Forest.AddLayer(new SettlementsInfo(0.01f, 0.05f, 0.05f));
//            LandTypes.Forest.SetSettlementsDensity(0.008f, 0.01f, 0.01f);
            
            LandTypes.Taiga.AddLayer(new ResourcesInfo(0, 1, 5, 0));
            LandTypes.Taiga.AddLayer(new StandAloneBuildingsInfo(10, 5, 10));
            LandTypes.Taiga.AddLayer(new SettlementsInfo(0.01f, 0.05f, 0.05f));
//            LandTypes.Taiga.SetSettlementsDensity(0.008f, 0.01f, 0.01f);
            
            LandTypes.Jungle.AddLayer(new ResourcesInfo(0, 0.5f, 2, 0));
            LandTypes.Jungle.AddLayer(new StandAloneBuildingsInfo(10, 5, 10));
            LandTypes.Jungle.AddLayer(new SettlementsInfo(0.002f, 0.01f, 0.05f));
//            LandTypes.Jungle.SetSettlementsDensity(0.008f, 0.006f, 0.006f);
            
            LandTypes.Swamp.AddLayer(new ResourcesInfo(0, 0.2f, 1, 0));
            LandTypes.Swamp.AddLayer(new StandAloneBuildingsInfo(10, 8, 0));
            LandTypes.Swamp.AddLayer(new SettlementsInfo(0.002f, 0.01f, 0.04f));
//            LandTypes.Swamp.SetSettlementsDensity(0.003f, 0.005f, 0.026f);
            
            LandTypes.Mountains.AddLayer(new ResourcesInfo(0, 0.5f, 0, 10));
            LandTypes.Mountains.AddLayer(new StandAloneBuildingsInfo(1, 1, 7));
            LandTypes.Mountains.AddLayer(new SettlementsInfo(0.01f, 0.075f, 0.06f));
//            LandTypes.Mountains.SetSettlementsDensity(0.004f, 0.005f, 0.006f);
        }

        #region Roads stuff
        private void BuildInterstateRoads(float fCycleShift)
        {
            foreach (Province pProvince in m_aProvinces)
                pProvince.m_cConnectionString.Clear();

            foreach (Province pProvince in m_aProvinces)
            {
                if (!pProvince.Forbidden)// && !pProvince.m_pRace.m_bDying)
                {
                    foreach (Province pLinkedProvince in pProvince.m_aBorderWith)
                    {
                        //pProvince.m_cConnectionString[pLinkedProvince] = "null";

                        if (!pLinkedProvince.Forbidden && pLinkedProvince != pProvince)// && (!pLinkedProvince.m_pRace.m_bDying || pLinkedProvince.m_iTechLevel > pProvince.m_iTechLevel))
                        {
                            //int iHostility = pProvince.CalcHostility(pLinkedProvince);
                            //if (iHostility > 2)
                            //{
                            //    pProvince.m_cConnectionString[pLinkedProvince] = string.Format("no road due to high hostility ({0})", iHostility);
                            //    pLinkedProvince.m_cConnectionString[pProvince] = string.Format("no road due to high hostility ({0})", iHostility);
                            //    continue;
                            //}

                            float fMinLength = float.MaxValue;
                            LocationX pBestTown1 = null;
                            LocationX pBestTown2 = null;
                            foreach (LocationX pTown in pProvince.m_pLocalSociety.Settlements)
                            {
                                foreach (LocationX pOtherTown in pLinkedProvince.m_pLocalSociety.Settlements)
                                {
                                    if (pTown != pOtherTown && !pTown.m_cHaveRoadTo.ContainsKey(pOtherTown))
                                    {
                                        float fDist = pTown.DistanceTo(pOtherTown, fCycleShift);// (float)Math.Sqrt((pTown.X - pOtherTown.X) * (pTown.X - pOtherTown.X) + (pTown.Y - pOtherTown.Y) * (pTown.Y - pOtherTown.Y));

                                        if (fDist < fMinLength)
                                        {
                                            fMinLength = fDist;

                                            pBestTown1 = pTown;
                                            pBestTown2 = pOtherTown;
                                        }
                                    }
                                }
                            }
                            if (pBestTown1 != null && 
                                (State.InfrastructureLevels[pProvince.m_pLocalSociety.m_iInfrastructureLevel].m_eMaxGroundRoad > 0 || 
                                 State.InfrastructureLevels[pLinkedProvince.m_pLocalSociety.m_iInfrastructureLevel].m_eMaxGroundRoad > 0))
                            {
                                RoadQuality eMaxRoadLevel = RoadQuality.Country;
                                foreach (var pRoad in pBestTown1.m_cRoads)
                                    if (pRoad.Value.Count > 0 && pRoad.Key > eMaxRoadLevel)
                                        eMaxRoadLevel = pRoad.Key;
                                foreach (var pRoad in pBestTown2.m_cRoads)
                                    if (pRoad.Value.Count > 0 && pRoad.Key > eMaxRoadLevel)
                                        eMaxRoadLevel = pRoad.Key;
                                //int iRoadLevel = 2;
                                //if (State.LifeLevels[pState.m_iLifeLevel].m_iMaxGroundRoad <= 1 && State.LifeLevels[pBorderState.m_iLifeLevel].m_iMaxGroundRoad <= 1)
                                //    iRoadLevel = 1;
                                //if (State.LifeLevels[pState.m_iLifeLevel].m_iMaxGroundRoad > 2 && State.LifeLevels[pBorderState.m_iLifeLevel].m_iMaxGroundRoad > 2)
                                //    iRoadLevel = 3;

                                if (State.InfrastructureLevels[pProvince.m_pLocalSociety.m_iInfrastructureLevel].m_eMaxGroundRoad == 0 || 
                                    State.InfrastructureLevels[pLinkedProvince.m_pLocalSociety.m_iInfrastructureLevel].m_eMaxGroundRoad == 0)
                                    eMaxRoadLevel = RoadQuality.Country;

                                BuildRoad(pBestTown1, pBestTown2, eMaxRoadLevel, fCycleShift);
                                pProvince.m_cConnectionString[pLinkedProvince] = "ok";
                                pLinkedProvince.m_cConnectionString[pProvince] = "ok";
                            }
                            else
                            {
                                pProvince.m_cConnectionString[pLinkedProvince] = "no road due to low infrastructure level or inaccessibility";
                                pLinkedProvince.m_cConnectionString[pProvince] = "no road due to low infrastructure level or inaccessibility";
                            }
                        }
                    }
                }
            }
        }

        private void BuildSeaRoutesFront(LocationX pHarbor, int iMaxDistance, float fCycleShift)
        {
            List<LocationX> cWaveFront = new List<LocationX>();

            foreach (Location pLinkedTerr in pHarbor.Origin.m_aBorderWith)
            {
                if (pLinkedTerr.Forbidden)
                    continue;

                LocationX pLinkedLoc = pLinkedTerr.As<LocationX>();

                if (pLinkedLoc.Origin.GetOwner().IsWater)
                {
                    SeaRouteBuilderInfo pInfo = null;
                    if (!pLinkedLoc.m_cSeaRouteBuildInfo.TryGetValue(pHarbor, out pInfo))
                    {
                        pInfo = new SeaRouteBuilderInfo();
                        pLinkedLoc.m_cSeaRouteBuildInfo[pHarbor] = pInfo;
                    }

                    pInfo.m_pFrom = pHarbor;
                    pInfo.m_fCost = pHarbor.DistanceTo(pLinkedLoc, fCycleShift) * pHarbor.GetMovementCost();

                    cWaveFront.Add(pLinkedLoc);
                }
            }

            float fMinDistance;

            do
            {
                fMinDistance = iMaxDistance;
                List<LocationX> cFutureWaveFront = new List<LocationX>();
                foreach (LocationX pLoc in cWaveFront)
                {
                    foreach (var pLinkedNode in pLoc.Links)
                    {
                        LocationX pLinkedLoc = pLinkedNode.Key as LocationX;

                        if (pLinkedLoc == null)
                            continue;

                        if (pLinkedLoc.Origin.GetOwner().IsWater || pLinkedLoc.IsHarbor)
                        {
                            float fDist = pLoc.m_cSeaRouteBuildInfo[pHarbor].m_fCost + pLinkedNode.Value.MovementCost;// pLoc.DistanceTo(pLinkedLoc, fCycleShift) * pLoc.GetMovementCost();

                            if (fDist > iMaxDistance)
                                continue;

                            SeaRouteBuilderInfo pInfo = null;
                            if (pLinkedLoc.m_cSeaRouteBuildInfo.TryGetValue(pHarbor, out pInfo))
                            {
                                if (pInfo.m_fCost <= fDist)
                                    continue;
                            }
                            else
                            {
                                pInfo = new SeaRouteBuilderInfo();
                                pLinkedLoc.m_cSeaRouteBuildInfo[pHarbor] = pInfo;
                            }

                            pInfo.m_pFrom = pLoc;
                            pInfo.m_fCost = fDist;

                            if (pLinkedLoc.Origin.GetOwner().IsWater)
                            {
                                cFutureWaveFront.Add(pLinkedLoc);

                                if (fDist < fMinDistance)
                                    fMinDistance = fDist;
                            }
                        }
                    }
                }
                cWaveFront.Clear();
                cWaveFront.AddRange(cFutureWaveFront);
            }
            while (fMinDistance < iMaxDistance);
        }

        private void BuildSeaRoutes(float fCycleShift)
        {
            foreach (Land pLand in m_aLands)
                pLand.IsHarbor = false;

            foreach (LandMass pLandMass in m_aLandMasses)
                pLandMass.IsHarbor = false;

            List<LocationX> cHarbors = new List<LocationX>();
            foreach (Province pProvince in m_aProvinces)
            {
                if (!pProvince.Forbidden && !pProvince.m_pLocalSociety.m_pTitularNation.IsAncient)
                {
                    foreach (LocationX pTown in pProvince.m_pLocalSociety.Settlements)
                    {
                        pTown.IsHarbor = false;

                        if (pTown.m_pSettlement.m_iRuinsAge > 0)
                            continue;

                        if (pTown.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Fort ||
                            //pTown.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Village ||
                            pTown.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Hamlet)
                            continue;

                        bool bCoastral = false;
                        foreach (Location pTerr in pTown.Origin.m_aBorderWith)
                        {
                            if (pTerr.Forbidden || !pTerr.HasOwner())
                                continue;

                            if (pTerr.GetOwner().IsWater)
                            {
                                bCoastral = true;
                                //SetLink(pTown.m_cBorderWith[pTerr][0].m_pPoint1, pTown);
                                //SetLink(pTown.m_cBorderWith[pTerr][0].m_pPoint2, pTown);
                            }
                        }
                        if (bCoastral)
                        {
                            cHarbors.Add(pTown);
                            pTown.IsHarbor = true;
                            pTown.Origin.GetOwner().IsHarbor = true;
                            pTown.Origin.GetOwner().GetOwner().IsHarbor = true;
                        }
                    }
                }
            }

            LocationX[] aHarbors = cHarbors.ToArray();
            foreach (LocationX pHarbor in aHarbors)
            {
                //if (pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Village)// ||
                //pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Town)
                //continue;

                Province pProvince = pHarbor.Origin.GetOwner().As<LandX>().GetOwner().GetOwner();
                RoadQuality eMaxNavalPath = State.InfrastructureLevels[pProvince.m_pLocalSociety.m_iInfrastructureLevel].m_eMaxNavalPath;
                if (eMaxNavalPath == 0)
                    continue;

                int iMaxLength = m_pLocationsGrid.RX * 10;
                if (eMaxNavalPath == RoadQuality.Country)
                    iMaxLength /= 10;
                if (eMaxNavalPath == RoadQuality.Normal)
                    iMaxLength /= 4;

                if(pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Village)
                    iMaxLength /= 10;

                BuildSeaRoutesFront(pHarbor, iMaxLength, fCycleShift);
            //}

            //foreach (LocationX pHarbor in aHarbors)
            //{
                foreach (var pOtherHarbor in pHarbor.m_cSeaRouteBuildInfo)
                {
                    if (pOtherHarbor.Key == pHarbor || pHarbor.m_cHaveSeaRouteTo.Contains(pOtherHarbor.Key))
                        continue;

                    State pState = pHarbor.OwnerState;
                    State pOtherState = pOtherHarbor.Key.OwnerState;
                    RoadQuality ePathLevel = RoadQuality.Country;

//                    int iMaxHostility = (int)Math.Sqrt(Math.Max(pState.CalcHostility(pOtherState), pOtherState.CalcHostility(pState)));
                    int iMaxHostility = (int)Math.Max(pState.m_pSociety.CalcHostility(pOtherState), pOtherState.m_pSociety.CalcHostility(pState));

                    if (pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Village &&
                        pOtherHarbor.Key.m_pSettlement.m_pInfo.m_eSize != SettlementSize.Village)
                        continue;

                    if (pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Town &&
                        (pOtherHarbor.Key.m_pSettlement.m_pInfo.m_eSize == SettlementSize.City ||
                         pOtherHarbor.Key.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Capital))
                        continue;

                    if (iMaxHostility <= 0 ||
                        Rnd.OneChanceFrom(iMaxHostility))
                    {
                        if (pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Town &&
                            pOtherHarbor.Key.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Town)
                            ePathLevel = RoadQuality.Normal;

                        if ((pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.City ||
                             pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Capital) &&
                            pOtherHarbor.Key.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Town)
                            ePathLevel = RoadQuality.Normal;

                        if ((pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.City ||
                             pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Capital) &&
                            (pOtherHarbor.Key.m_pSettlement.m_pInfo.m_eSize == SettlementSize.City ||
                             pOtherHarbor.Key.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Capital))
                            ePathLevel = RoadQuality.Good;
                    }

                    eMaxNavalPath = State.InfrastructureLevels[pOtherState.m_pSociety.m_iInfrastructureLevel].m_eMaxNavalPath;
                    iMaxLength = m_pLocationsGrid.RX * 10;
                    if (eMaxNavalPath == RoadQuality.Country)
                        iMaxLength /= 10;
                    if (eMaxNavalPath == RoadQuality.Normal)
                        iMaxLength /= 4;

                    //if (pOtherHarbor.Key.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Village)
                    //    iMaxLength /= 10;

                    //if (iPathLevel > iMaxNavalPath)
                    //    iPathLevel = iMaxNavalPath;

                    if (ePathLevel == RoadQuality.Country)
                        iMaxLength /= 10;
                    if (ePathLevel == RoadQuality.Normal)
                        iMaxLength /= 4; 
                    
                    if (pOtherHarbor.Value.m_fCost > iMaxLength)
                        continue;
                    
                    List<TransportationNode> pPath = new List<TransportationNode>();
                    pPath.Add(pHarbor);

                    LocationX pPosition = pHarbor;
                    while (pPosition != pOtherHarbor.Key)
                    {
                        pPosition = pPosition.m_cSeaRouteBuildInfo[pOtherHarbor.Key].m_pFrom;
                        pPath.Add(pPosition);
                    }

                    //if (pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Town ||
                    //    pOtherHarbor.Key.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Town)
                    //    iPathLevel = 2;
                    
                    //if (pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Village ||
                    //    pOtherHarbor.Key.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Village)
                    //    iPathLevel = 1;

                    SetSeaRoute(pPath.ToArray(), ePathLevel);

                    pHarbor.m_cHaveSeaRouteTo.Add(pOtherHarbor.Key);
                    pOtherHarbor.Key.m_cHaveSeaRouteTo.Add(pHarbor);
                }

                //float fMinDist = float.MaxValue;
                //LocationX pClosestHarbor1 = null;
                //LocationX pClosestHarbor2 = null;
                //LocationX pClosestHarbor3 = null;
                //foreach (LocationX pOtherHarbor in aHarbors)
                //{
                //    if (pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Village &&
                //        (pOtherHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.City ||
                //         pOtherHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Capital))
                //        continue;

                //    if (pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Town &&
                //        pOtherHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Village)
                //        continue;

                //    if (pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.City &&
                //        pOtherHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Village)
                //        continue;

                //    if (pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Capital &&
                //        pOtherHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Village)
                //        continue;

                //    if (pHarbor != pOtherHarbor && !pHarbor.m_cHaveSeaRouteTo.Contains(pOtherHarbor))
                //    {
                //        float fDist = pHarbor.DistanceTo(pOtherHarbor, fCycleShift);
                //        if (fDist < fMinDist)
                //        {
                //            fMinDist = fDist;

                //            if (pClosestHarbor1 != null)
                //            {
                //                if (pClosestHarbor2 != null)
                //                    pClosestHarbor3 = pClosestHarbor2;
                //                pClosestHarbor2 = pClosestHarbor1;
                //            }
                //            pClosestHarbor1 = pOtherHarbor;
                //        }
                //    }
                //}

                //int iPathLevel = 3;

                //if (pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Village)
                //{
                //    iPathLevel = 1;
                //    iMaxLength = m_pGrid.RX * 10 / 10;
                //}
                //if (pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Town)
                //    iPathLevel = 2;

                //if (pClosestHarbor1 != null)
                //    BuildSeaRoute(pHarbor, pClosestHarbor1, iPathLevel, fCycleShift, iMaxLength);
                //if (pClosestHarbor2 != null && iPathLevel >= 1)
                //    BuildSeaRoute(pHarbor, pClosestHarbor2, iPathLevel, fCycleShift, iMaxLength);
                //if (pClosestHarbor3 != null && iPathLevel >= 3)
                //    BuildSeaRoute(pHarbor, pClosestHarbor3, iPathLevel, fCycleShift, iMaxLength);
            }

            //FinishSeaRoutes(fCycleShift);
        }

        private Dictionary<ShortestPath, int> m_cSeaRoutesProjects = new Dictionary<ShortestPath,int>();

        //private void BuildSeaRoute(LocationX pTown1, LocationX pTown2, int iPathLevel, float fCycleShift, int iMaxLength)
        //{
        //    //TransportationNode[] aBestPath = FindBestPath(pTown1, pTown2, fCycleShift, true).m_aNodes;
        //    //if (aBestPath == null || aBestPath.Length == 0)
        //    //    aBestPath = FindBestPath(pTown2, pTown1, fCycleShift, true).m_aNodes;
        //    ShortestPath pBestPath = FindReallyBestPath(pTown1, pTown2, fCycleShift, true);

        //    if (pBestPath.m_aNodes != null && pBestPath.m_aNodes.Length > 1 && pBestPath.m_fLength < iMaxLength)
        //    //if (aBestPath != null && aBestPath.Length > 1)
        //    {
        //        m_cSeaRoutesProjects[pBestPath] = iPathLevel;

        //        pTown1.m_cHaveSeaRouteTo.Add(pTown2);
        //        pTown2.m_cHaveSeaRouteTo.Add(pTown1);
        //    }
        //}

        //private void FinishSeaRoutes(float fCycleShift)
        //{
        //    foreach (var pPath in m_cSeaRoutesProjects)
        //    {
        //        ShortestPath pBestPath = FindReallyBestPath(pPath.Key.m_aNodes[0] as LocationX, 
        //                                                    pPath.Key.m_aNodes[pPath.Key.m_aNodes.Length - 1] as LocationX, 
        //                                                    fCycleShift, false);

        //        TransportationLink pNewLink = new TransportationLink(pPath.Key.m_aNodes);
        //        pNewLink.m_bEmbark = true;
        //        pNewLink.m_bSea = true;
        //        pNewLink.BuildRoad(pPath.Value);

        //        if (pBestPath.m_aNodes != null &&
        //            pBestPath.m_aNodes.Length > 1 &&
        //            pBestPath.m_fLength < pNewLink.MovementCost)
        //            continue; 
                
        //        SetSeaRoute(pBestPath.m_aNodes, pPath.Value);
        //    }

        //    m_cSeaRoutesProjects.Clear();
        //}

        private void SetSeaRoute(TransportationNode[] aNodes, RoadQuality eLevel)
        {
            TransportationNode pLastNode = null;
            foreach (TransportationNode pNode in aNodes)
            {
                if (pLastNode != null)
                {
                    pLastNode.Links[pNode].BuildRoad(RoadQuality.Country);
                    //pNode.m_cLinks[pLastNode].BuildRoad(iRoadLevel);

                    //if (pLastNode.Owner != pNode.Owner)
                    //{
                    //    try
                    //    {
                    //        (pLastNode.Owner as LandX).m_cLinks[pNode.Owner as LandX].BuildRoad(iRoadLevel);

                    //        if ((pLastNode.Owner as LandX).Owner != (pNode.Owner as LandX).Owner)
                    //        {
                    //            ((pLastNode.Owner as LandX).Owner as LandMass<LandX>).m_cLinks[(pNode.Owner as LandX).Owner as LandMass<LandX>].BuildRoad(iRoadLevel);
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Trace.WriteLine(ex.Message);
                    //    }
                    //}
                }

                pLastNode = pNode;
            }

            TransportationLinkBase pNewLink = new TransportationLinkBase(aNodes);
            pNewLink.Embark = true;
            pNewLink.Sea = true;
            pNewLink.BuildRoad(eLevel);

            SetLink(aNodes[0], aNodes[aNodes.Length - 1], pNewLink);//aSeaRoute);

            LocationX pHarbor1 = aNodes[0] as LocationX;
            LocationX pHarbor2 = aNodes[aNodes.Length - 1] as LocationX;

            Province pProvince1 = pHarbor1.Origin.GetOwner().As<LandX>().GetOwner().GetOwner();
            Province pProvince2 = pHarbor2.Origin.GetOwner().As<LandX>().GetOwner().GetOwner();
            if (pProvince1.GetOwner() != pProvince2.GetOwner())
            {

                List<Province> pProvince1Neighbours = new List<Province>(pProvince1.m_aBorderWith);
                if (!pProvince1Neighbours.Contains(pProvince2))
                    pProvince1Neighbours.Add(pProvince2);
                pProvince1.m_aBorderWith = pProvince1Neighbours.ToArray();
                pProvince1.m_cConnectionString[pProvince2] = "ok";

                List<Province> pProvince2Neighbours = new List<Province>(pProvince2.m_aBorderWith);
                if (!pProvince2Neighbours.Contains(pProvince1))
                    pProvince2Neighbours.Add(pProvince1);
                pProvince2.m_aBorderWith = pProvince2Neighbours.ToArray();
                pProvince2.m_cConnectionString[pProvince1] = "ok";
            }
        }

        private static void DestroyRoad(Road pOldRoad)
        {
            LocationX pTown1 = pOldRoad.Locations[0];
            LocationX pTown2 = pOldRoad.Locations[pOldRoad.Locations.Length - 1];

            //удаляем старую дорогу
            foreach (LocationX pLoc in pOldRoad.Locations)
            {
                if (pLoc != pTown2)
                    pLoc.m_cHaveRoadTo.Remove(pTown2);
                if (pLoc != pTown1)
                    pLoc.m_cHaveRoadTo.Remove(pTown1);

                pLoc.m_cRoads[pOldRoad.m_eLevel].Remove(pOldRoad);
            }
        }

        /// <summary>
        /// Подготовка к строительству новой дороги:
        /// проверяем, нет ли уже между городами старой дороги, и если есть, то не лучше ли она чем та, которую хотим построоить.
        /// если хуже, то удаляем старую дорогу.
        /// Возвращаемое значение: true - можно строить новую дорогу, false - нельзя (уже есть такого же уровня или лучше)
        /// </summary>
        /// <param name="pTown1">первый город</param>
        /// <param name="pTown2">второй город</param>
        /// <param name="eRoadLevel">уровень новой дороги</param>
        /// <returns>true - можно строить новую дорогу, false - нельзя</returns>
        private static bool CheckOldRoad(LocationX pTown1, LocationX pTown2, RoadQuality eRoadLevel)
        {
            //проверим, а нет ли уже между этими городами дороги
            while (pTown1.m_cHaveRoadTo.ContainsKey(pTown2) && pTown2.m_cHaveRoadTo.ContainsKey(pTown1))
            {
                Road pOldRoad = pTown1.m_cHaveRoadTo[pTown2];

                //если существующая дорога лучше или такая же, как та, которую мы собираемся строить, то нафиг строить?
                if (pOldRoad.m_eLevel >= eRoadLevel)
                    return false;

                //удаляем старую дорогу
                DestroyRoad(pOldRoad);
            }

            return true;
        }

        /// <summary>
        /// Отстраивает заданный участок дороги.
        /// Предполагается, что на этом участке дорога не проходит ни через какие населённые пункты.
        /// </summary>
        /// <param name="pRoad">участок дороги</param>
        /// <param name="eRoadLevel">уровень дороги</param>
        private static void BuildRoad(Road pRoad, RoadQuality eRoadLevel)
        {
            LocationX pTown1 = pRoad.Locations[0];
            LocationX pTown2 = pRoad.Locations[pRoad.Locations.Length - 1];

            //если между этими двумя городами уже есть дорога не хуже - новую не строим
            if (!CheckOldRoad(pTown1, pTown2, eRoadLevel))
                return;

            int passed = 0;

            LocationX pLastNode = null;
            foreach (LocationX pNode in pRoad.Locations)
            {
                if (pNode != pTown1 && !pNode.m_cHaveRoadTo.ContainsKey(pTown1))
                    pNode.m_cHaveRoadTo[pTown1] = pRoad;
                if (pNode != pTown2 && !pNode.m_cHaveRoadTo.ContainsKey(pTown2))
                    pNode.m_cHaveRoadTo[pTown2] = pRoad;

                pNode.m_cRoads[eRoadLevel].Add(pRoad);

                var approxH = pTown1.H + (pTown2.H - pTown1.H) * (float)passed / (float)pRoad.Locations.Length;
                float fRoadWight = 0;
                switch (eRoadLevel)
                {
                    case RoadQuality.Country:
                        fRoadWight = 0.5f;
                        break;
                    case RoadQuality.Normal:
                        fRoadWight = 1f;
                        break;
                    case RoadQuality.Good:
                        fRoadWight = 2f;
                        break;
                }
                pNode.H = (pNode.H + approxH * fRoadWight) / (1f + fRoadWight);

                if (pLastNode != null)
                {
                    pLastNode.Links[pNode.Origin].BuildRoad(eRoadLevel);
                    //pNode.m_cLinks[pLastNode].BuildRoad(iRoadLevel);

                    if (pLastNode.Origin.GetOwner() != pNode.Origin.GetOwner())
                    {
                        try
                        {
                            pLastNode.Origin.GetOwner().Links[pNode.Origin.GetOwner()].BuildRoad(eRoadLevel);

                            if (pLastNode.Origin.GetOwner().GetOwner() != pNode.Origin.GetOwner().GetOwner())
                            {
                                pLastNode.Origin.GetOwner().GetOwner().Links[pNode.Origin.GetOwner().GetOwner()].BuildRoad(eRoadLevel);
                            }
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(ex.Message);
                        }
                    }
                }

                pLastNode = pNode;

                passed++;
            }
        }

        /// <summary>
        /// "обновляем" дорогу.
        /// если она проходит через какие-то населённые пункты, то разбиваем её на отдельный сегменты 
        /// от одного поселения до другого.
        /// </summary>
        /// <param name="pRoad">дорога</param>
        public static void RenewRoad(Road pOldRoad)
        {
            LocationX pTown1 = pOldRoad.Locations[0];
            LocationX pTown2 = pOldRoad.Locations[pOldRoad.Locations.Length - 1];

            //разобьем найденный путь на участки от одного населённого пункта до другого
            List<Road> cRoadsChain = new List<Road>();
            Road pNewRoad = null;
            foreach (LocationX pNode in pOldRoad.Locations)
            {
                if (pNewRoad == null)
                    pNewRoad = new Road(pNode, pOldRoad.m_eLevel);
                else
                {
                    pNewRoad.BuidTo(pNode);

                    if (pNode.m_pSettlement != null &&
                        pNode.m_pSettlement.m_iRuinsAge == 0 &&
                        (pNode.m_pSettlement.m_pInfo.m_eSize > SettlementSize.Village ||
                         pTown1.m_pSettlement.m_pInfo.m_eSize <= SettlementSize.Village ||
                         pTown2.m_pSettlement.m_pInfo.m_eSize <= SettlementSize.Village))
                    {
                        cRoadsChain.Add(pNewRoad);
                        pNewRoad = new Road(pNode, pOldRoad.m_eLevel);
                    }
                }
            }
            if (cRoadsChain.Count == 1)
                return;

            DestroyRoad(pOldRoad);
        
            Road[] aRoadsChain = cRoadsChain.ToArray();

            foreach (Road pRoad in aRoadsChain)
                BuildRoad(pRoad, pOldRoad.m_eLevel);
        }

        /// <summary>
        /// Строит дорогу из одного города в другой. Если дорога проходит через другие населённые пункты, она разбивается на 
        /// отдельные участки от одного поселения до другого.
        /// </summary>
        /// <param name="pTown1">первый город</param>
        /// <param name="pTown2">второй город</param>
        /// <param name="eRoadLevel">уровень строящейся дороги</param>
        /// <param name="fCycleShift">циклический сдвиг координат по горизонтали для закольцованных карт</param>
        public static void BuildRoad(LocationX pTown1, LocationX pTown2, RoadQuality eRoadLevel, float fCycleShift)
        {
            if (!CheckOldRoad(pTown1, pTown2, eRoadLevel))
                return;

            //PathFinder pBestPath = new PathFinder(pTown1, pTown2, fCycleShift, -1);
            ShortestPath pBestPath = FindReallyBestPath(pTown1.Origin, pTown2.Origin, fCycleShift, false);

            if (pBestPath.m_aNodes != null && pBestPath.m_aNodes.Length > 1)
            {
                //разобьем найденный путь на участки от одного населённого пункта до другого
                List<Road> cRoadsChain = new List<Road>();
                Road pNewRoad = null;
                foreach (Location pNode in pBestPath.m_aNodes)
                {
                    LocationX pLocX = pNode.As<LocationX>();

                    if (pNewRoad == null)
                        pNewRoad = new Road(pLocX, eRoadLevel);
                    else
                    {
                        pNewRoad.BuidTo(pLocX);

                        if (pLocX.m_pSettlement != null &&
                            pLocX.m_pSettlement.m_iRuinsAge == 0 &&
                            (pLocX.m_pSettlement.m_pInfo.m_eSize > SettlementSize.Village ||
                            pTown1.m_pSettlement.m_pInfo.m_eSize <= SettlementSize.Village ||
                            pTown2.m_pSettlement.m_pInfo.m_eSize <= SettlementSize.Village))
                        {
                            cRoadsChain.Add(pNewRoad);
                            pNewRoad = new Road(pLocX, eRoadLevel);
                        }
                    }
                }

                Road[] aRoadsChain = cRoadsChain.ToArray();

                foreach (Road pRoad in aRoadsChain)
                    BuildRoad(pRoad, eRoadLevel);
            }
        }

        #endregion Roads stuff
        
        /// <summary>
        /// "Оборот" мира в начале каждой эпохи
        /// расы стареют, города обращаются в руины, границы государств и дороги стираются, чтобы быть проложенными вновь - по-новому
        /// </summary>
        /// <param name="pNewEpoch">новая эпоха</param>
        public void Reset(Epoch pNewEpoch)
        {
            if (m_aLocalNations == null || m_aLocalNations.Length == 0)
                return;

            //Ассимилируем коренное население
            foreach (Province pProvince in m_aProvinces)
            {
                foreach (Region pRegion in pProvince.Contents)
                {
                    foreach (LandX pLand in pRegion.Contents)
                    {
                        //если коренное население не является доминирующим, есть вероятность, что доминирующая раса вытеснит коренную.
                        if (pLand.m_pDominantNation != pProvince.m_pLocalSociety.m_pTitularNation)
                        {
                            if (pProvince.m_pLocalSociety.m_pTitularNation.DominantPhenotype.m_pValues.Get<NutritionGenetix>().IsParasite())
                                continue;

                            bool bHated = false;
                            foreach (LandTypeInfo pLTI in pProvince.m_pLocalSociety.m_pTitularNation.m_aHatedLands)
                                if (pLand.Origin.LandType == pLTI)
                                    bHated = true;

                            if (bHated)
                                continue;

                            bool bPreferred = false;
                            foreach (LandTypeInfo pLTI in pProvince.m_pLocalSociety.m_pTitularNation.m_aPreferredLands)
                                if (pLand.Origin.LandType == pLTI)
                                    bPreferred = true;

                            if (pProvince.m_pLocalSociety.m_pTitularNation.IsHegemon || bPreferred || Rnd.OneChanceFrom(2))
                            {
                                pLand.m_pDominantNation = pProvince.m_pLocalSociety.m_pTitularNation;
                                pLand.GetOwner().m_pNatives = pProvince.m_pLocalSociety.m_pTitularNation;
                            }
                        }
                        //иначе, если коренная раса и доминирующая совпадают, но земля не самая подходящая - есть вероятность того, что её покинут.
                        else
                        {
                            if (pProvince.m_pLocalSociety.m_pTitularNation.IsHegemon)
                                continue;

                            bool bHated = false;
                            foreach (LandTypeInfo pLTI in pProvince.m_pLocalSociety.m_pTitularNation.m_aHatedLands)
                                if (pLand.Origin.LandType == pLTI)
                                    bHated = true;

                            if (bHated && Rnd.OneChanceFrom(2))
                            {
                                pLand.m_pDominantNation = null;
                                pLand.GetOwner().m_pNatives = null;
                            }
                        }
                    }
                }
            }

            //List<Race> cEraseRace = new List<Race>();
            foreach (Nation pNation in m_aLocalNations)
            {
                if (!pNation.IsAncient)
                {
                    if (pNation.m_pEpoch != pNewEpoch)
                    {
                        pNation.Age();
                    }
                    else
                        //if (Rnd.OneChanceFrom(3))
                        //    cEraseRace.Add(pRace);
                        //else
                        if (Rnd.OneChanceFrom(m_aLocalNations.Length))
                            pNation.Grow();
                }
                //else
                //    if (Rnd.OneChanceFrom(10))
                //        cEraseRace.Add(pRace);
            }

            //List<Race> cRaces = new List<Race>(m_aLocalRaces);

            //foreach (Race pRace in cEraseRace)
            //    cRaces.Remove(pRace);

            foreach (Continent pConti in Contents)
            {
                //foreach (var pLandMass in pConti.m_cLocalRaces)
                //{
                //    foreach (Race pRace in cEraseRace)
                //        if (pLandMass.Value.Contains(pRace))
                //            pLandMass.Value.Remove(pRace);

                //    //foreach (Race pRce in pLandMass.Value)
                //    //    if (pRce == pRace)
                //    //    {
                //    //        if (pLandMass.Value.Contains(pRace))
                //    //            pLandMass.Value.Remove(pRace);
                //    //        throw new Exception();
                //    //    }
                //}

                foreach (Region pRegion in pConti.As<ContinentX>().m_cRegions)
                {
                    if (pRegion.m_pNatives == null)
                        continue;

                    if (//cEraseRace.Contains(pArea.m_pRace) ||
                        (pRegion.m_pNatives.IsAncient && !Rnd.OneChanceFrom(100 / (pRegion.m_pType.m_iMovementCost * pRegion.m_pType.m_iMovementCost))))
                        pRegion.m_pNatives = null;

                    foreach (LandX pLand in pRegion.Contents)
                    {
                        if (//cEraseRace.Contains(pLand.m_pRace) || 
                            pLand.m_pDominantNation.IsAncient)
                            pLand.m_pDominantNation = pRegion.m_pNatives;
                    }
                }
            }

            //m_aLocalRaces = cRaces.ToArray();

            //foreach (ContinentX pConti in m_cContinents)
            //{
            //    foreach (LandMass<LandX> pLandMass in pConti.m_cLocalRaces.Keys)
            //    {
            //        foreach (Race pRace in pConti.m_cLocalRaces[pLandMass])
            //            if (!m_cLocalRaces.Contains(pRace))
            //                throw new Exception();
            //    }
            //    foreach (AreaX pArea in pConti.m_cAreas)
            //        foreach (LandX pLand in pArea.m_cContents)
            //            if(pLand.m_pRace != null && !m_cLocalRaces.Contains(pLand.m_pRace))
            //                throw new Exception();
            //}

            List<State> cEraseState = new List<State>();
            //все государства
            foreach (State pState in m_aStates)
            {
                if (pState.Forbidden)
                    continue;

                //все провинции в каждом государстве
                foreach (Province pProvince in pState.Contents)
                {
                    //ни одна провинция не принадлежит ни какому государству
                    pProvince.ClearOwner();
                    //все регионы в каждой провинции
                    foreach (Region pRegion in pProvince.Contents)
                    {
                        //ни один регион не принадлежит никакой провинции
                        pRegion.ClearOwner();
                        pRegion.m_iProvincePresence = 0;
                        //все земли в каждом регионе
                        foreach (LandX pLand in pRegion.Contents)
                        {
                            //все локации в каждой земле
                            foreach (Location pLoc in pLand.Origin.Contents)
                            {
                                LocationX pLocX = pLoc.As<LocationX>();
                                //если есть поселение
                                if (pLocX.m_pSettlement != null)
                                {
                                    //если это уже руины
                                    if (pLocX.m_pSettlement.m_iRuinsAge > 0)
                                    {
                                        //в зависимости от проходимости местности
                                        //либо наращиваем возраст руин - в труднодоступных местах
                                        if (Rnd.OneChanceFrom((int)pLand.Origin.MovementCost) || pLocX.m_pSettlement.Ruin())
                                        {
                                            //либо окончательно уничтожаем все следы цивилизации - в легкодоступных местах
                                            pLocX.m_pSettlement = null;
                                            //снимаем флаг "руины" со всех путей, ведущих в эту локацию
                                            foreach (var pLink in pLoc.Links)
                                                pLink.Value.Ruins = false;
                                            //и убираем локацию из списка поселений в провинции
                                            //if (pProvince.m_cSettlements.Contains(pLoc))
                                            //    pProvince.m_cSettlements.Remove(pLoc);
                                        }
                                    }
                                    //иначе, т.е. если это ещё не руины
                                    else
                                    {
                                        //в зависимости от размера поселения и проходимости местности даём ему шанс стать руинами
                                        switch (pLocX.m_pSettlement.m_pInfo.m_eSize)
                                        {
                                            case SettlementSize.Village:
                                                break;
                                            case SettlementSize.Town:
                                                if (Rnd.OneChanceFrom(1 + (int)(24 / pLoc.GetMovementCost())))
                                                    pLocX.m_pSettlement.Ruin();
                                                break;
                                            case SettlementSize.City:
                                                if (Rnd.OneChanceFrom(1 + (int)(12 / pLoc.GetMovementCost())))
                                                    pLocX.m_pSettlement.Ruin();
                                                break;
                                            case SettlementSize.Capital:
                                                if (Rnd.OneChanceFrom(1 + (int)(6 / pLoc.GetMovementCost())))
                                                    pLocX.m_pSettlement.Ruin();
                                                break;
                                            case SettlementSize.Fort:
                                                if (Rnd.OneChanceFrom(1 + (int)(9 / pLoc.GetMovementCost())))
                                                    pLocX.m_pSettlement.Ruin();
                                                break;
                                        }

                                        //административные центры всегда становятся руинами - если только они крупнее деревни
                                        if (pLocX == pState.m_pMethropoly.m_pAdministrativeCenter &&
                                            pLocX.m_pSettlement.m_pInfo.m_eSize > SettlementSize.Village &&
                                            pLocX.m_pSettlement.m_iRuinsAge == 0)
                                            pLocX.m_pSettlement.Ruin();

                                        //если поселение не превратилось в руины - значит оно полностью погибло и не оставило никаких следов
                                        if (pLocX.m_pSettlement.m_iRuinsAge == 0 || pLocX.m_pSettlement.m_cBuildings.Count == 0)
                                            pLocX.m_pSettlement = null;
                                        else
                                            //если же оно таки стало руинами - помечаем соответсвенно все пути ведущие в эту локацию, чтобы дороги обходили её стороной
                                            foreach (var pLink in pLoc.Links)
                                                pLink.Value.Ruins = true;

                                    }
                                }

                                //так же снимаем пометку "порт" и сносим все отдельно стоящие строения
                                pLocX.IsHarbor = false;
                                pLocX.m_pBuilding = null;
                            }//все локации в каждой земле
                        }//все земли в каждом регионе
                    }//все регионы в каждой провинции

                    //очищаем список поселений в провинции
                    pProvince.m_pLocalSociety.Settlements.Clear();
                }//все провинции в каждом государстве

                //pState.ClearOwner();

                //некоторые государства вообще исчезают с лица земли
                if (!m_aLocalNations.Contains(pState.m_pSociety.m_pTitularNation) || Rnd.OneChanceFrom(2))
                    cEraseState.Add(pState);

            }//все государства

            //удаляем из общего списка государства, которые не выжили
            List<State> cStates = new List<State>(m_aStates);
            foreach (State pState in cEraseState)
                cStates.Remove(pState);
            m_aStates = cStates.ToArray();

            //оставляем провинции - центры выживших государств, остальные удаляем из общего списка
            List<Province> cProvinces = new List<Province>();
            foreach (State pState in m_aStates)
                cProvinces.Add(pState.m_pMethropoly);
            m_aProvinces = cProvinces.ToArray();

            //очищаем поконтинентные списки государств
            foreach (Continent pConti in Contents)
                pConti.As<ContinentX>().Contents.Clear();

            //уничтожаем все дороги
            foreach (TransportationLinkBase pRoad in m_cTransportGrid)
                pRoad.ClearRoad();
            foreach (TransportationLinkBase pRoad in m_cLandsTransportGrid)
                pRoad.ClearRoad();
            foreach (TransportationLinkBase pRoad in m_cLMTransportGrid)
                pRoad.ClearRoad();

            //все локации в мире
            foreach (Location pLoc in m_pLocationsGrid.Locations)
            {
                LocationX pLocX = pLoc.As<LocationX>();

                //очищаем информацию о дорогах, проходивших через локацию
                pLocX.m_cRoads.Clear();
                pLocX.m_cRoads[RoadQuality.Country] = new List<Road>();
                pLocX.m_cRoads[RoadQuality.Normal] = new List<Road>();
                pLocX.m_cRoads[RoadQuality.Good] = new List<Road>();
                pLocX.m_cHaveRoadTo.Clear();
                pLocX.m_cHaveSeaRouteTo.Clear();

                //удаляем все пути с суши на море (выходить в море можно только в портах, а раз портов нет, то и путей нет)
                if (pLoc.HasOwner() && !pLoc.GetOwner().IsWater)
                {
                    List<TransportationNode> cErase = new List<TransportationNode>();
                    foreach (TransportationNode pNode in pLoc.Links.Keys)
                    {
                        if (pLoc.Links[pNode].Sea)
                            cErase.Add(pNode);
                    }
                    foreach (TransportationNode pNode in cErase)
                    {
                        pLoc.Links.Remove(pNode);
                        pNode.Links.Remove(pLoc);
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает локацию с поселением, близкую к заданной, в которой есть жители относящиеся к заданному сословию.
        /// Если такие жители есть прямо в заданной локации - возвращает её же.
        /// Если в ней нет - ищет по другим поселениям в том же государстве.
        /// Если и там не находит - ищет по всему миру.
        /// Если и там не находит - возвращает null.
        /// </summary>
        /// <param name="pPreferredLoc"></param>
        /// <param name="eEstate"></param>
        /// <returns></returns>
        public LocationX GetRandomSettlement(LocationX pPreferredLoc, Estate.Position eEstate)
        {
            if (pPreferredLoc != null && pPreferredLoc.HaveEstate(eEstate))
                return pPreferredLoc;

            List<LocationX> pPossibleSettlements = new List<LocationX>();

            State pState = m_aStates[Rnd.Get(m_aStates.Length)];
            if (pPreferredLoc != null)
                pState = pPreferredLoc.OwnerState;

            foreach (LocationX pLocation in pState.m_pSociety.Settlements)
            {
                if (pLocation.HaveEstate(eEstate))
                    pPossibleSettlements.Add(pLocation);
            }

            if (pPossibleSettlements.Count == 0)
            {
                pState = m_aStates[Rnd.Get(m_aStates.Length)];

                foreach (LocationX pLocation in pState.m_pSociety.Settlements)
                {
                    if (pLocation.HaveEstate(eEstate))
                            pPossibleSettlements.Add(pLocation);
                }

                if (pPossibleSettlements.Count == 0)
                {
                    foreach (Province pProvince in m_aProvinces)
                    {
                        foreach (LocationX pLocation in pProvince.m_pLocalSociety.Settlements)
                        {
                            if (pLocation.HaveEstate(eEstate))
                                pPossibleSettlements.Add(pLocation);
                        }
                    }

                    if (pPossibleSettlements.Count == 0)
                        return null;
                }
            }

            return pPossibleSettlements[Rnd.Get(pPossibleSettlements.Count)];
        }

        /// <summary>
        /// Возвращает случайного персонажа в заданной локации.
        /// Если локация не задана (null) - выбирает случайную локацию из числа наименее густо заселённых поселений.
        /// </summary>
        /// <param name="pPreferredHome"></param>
        /// <returns></returns>
        public Person GetPossibleRelative(LocationX pPreferredHome)
        {
            return GetPossibleRelative(pPreferredHome, null);
        }

        /// <summary>
        /// Возвращает случайного персонажа в заданной локации или государстве.
        /// Если локация не задана (null) - выбирает случайную локацию из числа наименее густо заселённых поселений в указанном государстве.
        /// Если государство тоже не задано (null) - выбирает случайную локацию из числа наименее густо заселённых во всём мире.
        /// </summary>
        /// <param name="pPreferredHome"></param>
        /// <param name="pPreferredState"></param>
        /// <returns></returns>
        private Person GetPossibleRelative(LocationX pPreferredHome, State pPreferredState)
        {
            if (m_cPersons.Count == 0)
                return null;

            Dictionary<Person, float> cRelatives = new Dictionary<Person, float>();

            foreach (Person pPerson in m_cPersons)
            {
                if (pPreferredHome != null && pPerson.m_pHomeLocation != pPreferredHome)
                    continue;

                if (pPreferredState != null && pPerson.m_pHomeLocation.Origin.GetOwner().As<LandX>().GetOwner().GetOwner().GetOwner() != pPreferredState)
                    continue;

                //if (!pPerson.CouldInviteNewDwellers())
                //    continue;

                float fRelativesCount = pPerson.GetFamilySize();// m_cRelations.Count();

                if (fRelativesCount == 0)
                    fRelativesCount = 1;

                if (pPerson.m_pNation.DominantPhenotype.m_pValues.Get<LifeCycleGenetix>().BirthRate == BirthRate.Moderate)
                    fRelativesCount *= 2;

                if (pPerson.m_pNation.DominantPhenotype.m_pValues.Get<LifeCycleGenetix>().BirthRate == BirthRate.Low)
                    fRelativesCount *= 5;

                if (fRelativesCount > 10)
                    continue;

                int iNeighbours = 1;
                foreach (Building pBuilding in pPerson.m_pHomeLocation.m_pSettlement.m_cBuildings)
                    iNeighbours += pBuilding.m_cPersons.Count();

                switch (pPerson.m_pHomeLocation.m_pSettlement.m_pInfo.m_eSize)
                {
                    case SettlementSize.Hamlet:
                        fRelativesCount *= (float)iNeighbours / 2;
                        break;
                    case SettlementSize.Fort:
                        fRelativesCount *= (float)iNeighbours / 3;
                        break;
                    case SettlementSize.Village:
                        fRelativesCount *= (float)iNeighbours / 3;
                        break;
                    case SettlementSize.Town:
                        fRelativesCount *= (float)iNeighbours / 4;
                        break;
                    case SettlementSize.City:
                        fRelativesCount *= (float)iNeighbours / 5;
                        break;
                    case SettlementSize.Capital:
                        fRelativesCount *= (float)iNeighbours / 6;
                        break;
                }

                cRelatives[pPerson] = 1.0f / fRelativesCount;
            }

            if (cRelatives.Count == 0)
                return null;// GetPossibleRelative(null, pPreferredHome != null ? pPreferredHome.Owner : null);

            int iChances = Rnd.ChooseOne(cRelatives.Values);

            return cRelatives.Keys.ElementAt(iChances);
        }

        /// <summary>
        /// Возвращает случайного персонажа заданной нации или расы.
        /// Если нация не задана (null) - выбирает случайного персонажа той же расы, к которой принадлежит указанная нация.
        /// Но - из государства с цивилизованностью не выше заданной, чтобы дикари не вписывались в родню с цивилизованным людям.
        /// </summary>
        /// <param name="pPreferredNation"></param>
        /// <returns></returns>
        public Person GetPossibleRelative(Nation pPreferredNation, int iMaxCivilizationLevel)
        {
            return GetPossibleRelative(pPreferredNation, null, iMaxCivilizationLevel);
        }

        /// <summary>
        /// Возвращает случайного персонажа заданной нации или расы.
        /// Если нация не задана (null) - выбирает случайного персонажа указанной расы.
        /// Если раса тоже не задана (null) - возвращает null.
        /// </summary>
        /// <param name="pPreferredHome"></param>
        /// <param name="pPreferredState"></param>
        /// <returns></returns>
        private Person GetPossibleRelative(Nation pPreferredNation, Race pPreferredRace, int iMaxCivilizationLevel)
        {
            if (m_cPersons.Count == 0)
                return null;

            Dictionary<Person, float> cRelatives = new Dictionary<Person, float>();

            foreach (Person pPerson in m_cPersons)
            {
                if (pPreferredRace != null && pPerson.m_pNation.m_pRace != pPreferredRace)
                    continue;

                if (pPreferredNation != null && pPerson.m_pNation != pPreferredNation)
                    continue;

                if (pPerson.m_pState.m_iInfrastructureLevel > iMaxCivilizationLevel)
                    continue;

                //if (!pPerson.CouldInviteNewDwellers())
                //    continue;

                float fRelativesCount = pPerson.GetFamilySize();// m_cRelations.Count();

                if (fRelativesCount == 0)
                    fRelativesCount = 1;

                if (pPerson.m_pNation.DominantPhenotype.m_pValues.Get<LifeCycleGenetix>().BirthRate == BirthRate.Moderate)
                    fRelativesCount *= 2;

                if (pPerson.m_pNation.DominantPhenotype.m_pValues.Get<LifeCycleGenetix>().BirthRate == BirthRate.Low)
                    fRelativesCount *= 5;

                if (fRelativesCount > 10)
                    continue;

                int iNeighbours = 1;
                foreach (Building pBuilding in pPerson.m_pHomeLocation.m_pSettlement.m_cBuildings)
                    iNeighbours += pBuilding.m_cPersons.Count();

                switch (pPerson.m_pHomeLocation.m_pSettlement.m_pInfo.m_eSize)
                {
                    case SettlementSize.Hamlet:
                        fRelativesCount *= (float)iNeighbours / 2;
                        break;
                    case SettlementSize.Fort:
                        fRelativesCount *= (float)iNeighbours / 3;
                        break;
                    case SettlementSize.Village:
                        fRelativesCount *= (float)iNeighbours / 3;
                        break;
                    case SettlementSize.Town:
                        fRelativesCount *= (float)iNeighbours / 4;
                        break;
                    case SettlementSize.City:
                        fRelativesCount *= (float)iNeighbours / 5;
                        break;
                    case SettlementSize.Capital:
                        fRelativesCount *= (float)iNeighbours / 6;
                        break;
                }

                cRelatives[pPerson] = 1.0f / fRelativesCount;
            }

            if (cRelatives.Count == 0)
                return pPreferredNation != null ? GetPossibleRelative(null, pPreferredNation.m_pRace, iMaxCivilizationLevel) : null;

            int iChances = Rnd.ChooseOne(cRelatives.Values);

            return cRelatives.Keys.ElementAt(iChances);
        }
    }
}
