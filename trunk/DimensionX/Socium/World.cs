using System;
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

namespace Socium
{
    public enum MagicAbilityPrevalence
    {
        rare,
        common,
        almost_everyone
    }

    public enum MagicAbilityDistribution
    {
        mostly_weak,
        mostly_average,
        mostly_powerful
    }

    public class World : Landscape<LocationX, LandX, AreaX, ContinentX, LandTypeInfoX>
    {
        public int m_iTechLevel;
        public int m_iMagicLimit;

        public MagicAbilityPrevalence m_eMagicAbilityPrevalence = MagicAbilityPrevalence.rare;
        public MagicAbilityDistribution m_eMagicAbilityDistribution = MagicAbilityDistribution.mostly_weak;

        public Province[] m_aProvinces = null;

        private int m_iProvincesCount = 300;

        public State[] m_aStates = null;

        private int m_iStatesCount = 30;

        public Race[] m_aLocalRaces = null;

        private void SetWorldLevels(Epoch pEpoch)
        {
            m_eMagicAbilityPrevalence = (MagicAbilityPrevalence)Rnd.GetExp(typeof(MagicAbilityPrevalence), 4);
            m_eMagicAbilityDistribution = (MagicAbilityDistribution)Rnd.GetExp(typeof(MagicAbilityDistribution), 4);

            //int iTotalLevel = 1 + (int)(Math.Pow(Rnd.Get(20), 3) / 1000);
            //int iBalance = Rnd.Get(201);

            //if (iBalance > 100)
            //{
            //    m_iMagicLimit = Rnd.Get(iTotalLevel + 1);//iTotalLevel * iBalance / 100;
            //    //m_iTechLevel = 1 + Rnd.Get(iTotalLevel);//iTotalLevel - m_iBioLevel;
            //    m_iTechLevel = (200 - iBalance) * m_iMagicLimit / iBalance;
            //}
            //else
            //{
            //    m_iTechLevel = 1 + Rnd.Get(iTotalLevel);//iTotalLevel - m_iBioLevel;
            //    //m_iMagicLimit = Rnd.Get(iTotalLevel + 1);//iTotalLevel * iBalance / 100;
            //    m_iMagicLimit = iBalance * m_iTechLevel / (200 - iBalance);
            //}

            m_iTechLevel = Math.Min(pEpoch.m_iNativesMaxTechLevel, pEpoch.m_iNativesMinTechLevel + 1 + (int)(Math.Pow(Rnd.Get(20), 3) / 1000));
            m_iMagicLimit = Math.Min(pEpoch.m_iNativesMaxMagicLevel, pEpoch.m_iNativesMinMagicLevel + (int)(Math.Pow(Rnd.Get(21), 3) / 1000));
        }

        private void AddRaces(Epoch pEpoch)
        {
            List<Race> cRaces = new List<Race>();

            int iDyingRaces = 0;
            //Все расы, оставшиеся с прошлых исторических циклов, адаптируем к новым условиям.
            if(m_aLocalRaces != null)
                foreach (Race pRace in m_aLocalRaces)
                {
                    pRace.Accommodate(this, pEpoch);
                    cRaces.Add(pRace);

                    if (pRace.m_bDying)
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
            while (cRaces.Count - iDyingRaces < pEpoch.m_iNativesCount)
            {
                int iChance = Rnd.Get(pEpoch.m_cNativesRaceTemplates.Count);//Rnd.ChooseOne(cRaceChances.Values, 1);
                //foreach (RaceTemplate pRaceTemplate in cRaceChances.Keys)
                //{
                    //iChance--;
                    //if (iChance < 0)
                    //{
                        Race pRace = new Race(pEpoch.m_cNativesRaceTemplates[iChance], pEpoch);//new Race(pRaceTemplate, pEpoch);
                        pRace.Accommodate(this, pEpoch);

                        cRaces.Add(pRace);
                        //cRaceChances[pRaceTemplate] = 0;
                        //break;
                    //}
                //}
            }

            m_aLocalRaces = cRaces.ToArray();
        }

        private void AddInvadersRaces(Epoch pEpoch)
        {
            if (pEpoch.m_iInvadersCount == 0)
                return;

            List<Race> cRaces = new List<Race>(m_aLocalRaces);

            //Рассчитываем шансы новых рас попасть в новый мир - учитывая, его параметры
            Dictionary<RaceTemplate, float> cInvadersRaceChances = new Dictionary<RaceTemplate, float>();
            foreach (RaceTemplate pRaceTemplate in Race.m_cTemplates)
            {
                if (!pEpoch.m_cInvadersRaceTemplates.Contains(pRaceTemplate))
                    continue;

                bool bAlreadyHave = false;
                if (m_aLocalRaces != null)
                    foreach (Race pRace in m_aLocalRaces)
                        if (pRace.m_pTemplate == pRaceTemplate && !pRace.m_bDying)
                            bAlreadyHave = true;

                cInvadersRaceChances[pRaceTemplate] = bAlreadyHave ? 10 : 100;// / pRaceTemplate.m_iRank;
            }

            for (int i = 0; i < pEpoch.m_iInvadersCount; i++)
            {
                int iChance = Rnd.ChooseOne(cInvadersRaceChances.Values, 1);
                foreach (RaceTemplate pRaceTemplate in cInvadersRaceChances.Keys)
                {
                    iChance--;
                    if (iChance < 0)
                    {
                        Race pRace = new Race(pRaceTemplate, pEpoch);
                        pRace.m_bInvader = true;
                        pRace.Accommodate(this, pEpoch);

                        cRaces.Add(pRace);
                        cInvadersRaceChances[pRaceTemplate] = 0;
                        break;
                    }
                }
            }

            m_aLocalRaces = cRaces.ToArray();
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
            foreach (Race pRace in m_aLocalRaces)
            {
                //убедимся, что эта раса ещё нигде не живёт
                bool bHomeless = true;
                foreach (ContinentX pConti in m_aContinents)
                {
                    foreach (LandMass<LandX> pLandMass in pConti.m_cContents)
                        if (pConti.m_cLocalRaces.ContainsKey(pLandMass) && pConti.m_cLocalRaces[pLandMass].Contains(pRace))
                        {
                            bHomeless = false;
                            break;
                        }
                    if (!bHomeless)
                        break;
                }

                //если уже где-то живёт и не гегемон - пропускаем её
                if (!bHomeless)// && !pRace.m_bHegemon)
                    continue;

                //рассчитаем вероятности для всех земель стать прародиной этой расы
                Dictionary<LandX, float> cLandChances = new Dictionary<LandX, float>();
                foreach (LandX pLand in m_aLands)
                {
                    //отсеиваем уже занятые и непригодные для заселения земли
                    if (pLand.Forbidden ||
                        pLand.m_pProvince != null ||
                        pLand.m_pRace != null ||
                        pLand.IsWater ||
                        pLand.Owner == null ||
                        (pLand.Owner as LandMass<LandX>).IsWater)
                        continue;

                    cLandChances[pLand] = 1.0f;// / pRace.m_iRank;

                    //рассчитываем шансы, исходя из предпочтений и антипатий расы
                    foreach (LandTypeInfoX pType in pRace.m_pTemplate.m_aPrefferedLands)
                        if (pLand.Type == pType)
                            cLandChances[pLand] *= 100;

                    foreach (LandTypeInfoX pType in pRace.m_pTemplate.m_aHatedLands)
                        if (pLand.Type == pType)
                            cLandChances[pLand] /= 1000;

                    //смотрим, сколько ещё других рас уже живут на этом же континенте и говорят ли они на нашем языке
                    int iPop = 0;
                    bool bSameLanguage = false;
                    foreach (var pRaces in pLand.Continent.m_cLocalRaces)
                    {
                        foreach (var pRce in pRaces.Value)
                        {
                            if (!pRce.m_bDying)
                                iPop++;
                            
                            if (pRce.m_pTemplate.m_pLanguage == pRace.m_pTemplate.m_pLanguage)
                                bSameLanguage = true;
                        }
                    }
 
                    if (iPop > 0)
                    {
                        //снижаем привлекательность земли в зависимости от количества конкурентов.
                        //гегемоны и вторженцы игнорируют эту проверку.
                        if (!pRace.m_bHegemon && !pRace.m_bInvader && !bSameLanguage)
                            cLandChances[pLand] = (float)Math.Pow(cLandChances[pLand], 1.0 / (1 + iPop));

                        //смотрим, на каких языках говорят другие уже живущие здесь расы
                        //и снижаем либо повышаем привлекательность земли в зависимости от совпдения языка
                        if (bSameLanguage)
                            cLandChances[pLand] *= 100;
                        else
                            cLandChances[pLand] /= 100;
                    }
                }

                int iChance = Rnd.ChooseOne(cLandChances.Values, 2);
                LandX pCradle = null;
                foreach (LandX pLand in cLandChances.Keys)
                {
                    iChance--;
                    if (iChance < 0)
                    {
                        pCradle = pLand;
                        break;
                    }
                }

                if (pCradle != null && pCradle.Area != null)
                {
                    (pCradle.Area as AreaX).m_pRace = pRace;
                    (pCradle.Area as AreaX).m_sName = pRace.m_pTemplate.m_pLanguage.RandomCountryName();
                    foreach (LandX pLand in (pCradle.Area as AreaX).m_cContents)
                    {
                        pLand.m_sName = (pCradle.Area as AreaX).m_sName;
                        pLand.m_pRace = (pCradle.Area as AreaX).m_pRace;
                    }

                    LandMass<LandX> pLandMass = pCradle.Owner as LandMass<LandX>;
                    if (pLandMass != null)
                    {
                        if (!pCradle.Continent.m_cLocalRaces.ContainsKey(pLandMass))
                            pCradle.Continent.m_cLocalRaces[pLandMass] = new List<Race>();
                        
                        pCradle.Continent.m_cLocalRaces[pLandMass].Add(pRace);
                    }
                }
            }

            //Позаботимся о том, чтобы на каждом континенте жила бы хоть одна раса
            foreach (ContinentX pConti in m_aContinents)
            {
                //если континент уже обитаем, пропускаем его
                int iPop = 0;
                foreach (var pRaces in pConti.m_cLocalRaces)
                {
                    foreach (var pRce in pRaces.Value)
                        if (!pRce.m_bDying)
                            iPop++;
                }

                if (iPop > 0)
                    continue;

                LandMass<LandX> pLandMass = pConti.m_cContents[Rnd.Get(pConti.m_cContents.Count)];

                pConti.m_cLocalRaces[pLandMass] = new List<Race>();

                Dictionary<LandTypeInfoX, int> cLandTypesCount = new Dictionary<LandTypeInfoX, int>();
                foreach (LandX pLand in pLandMass.m_cContents)
                {
                    if (pLand.IsWater)
                        continue;

                    if (!cLandTypesCount.ContainsKey(pLand.Type))
                        cLandTypesCount[pLand.Type] = 0;

                    cLandTypesCount[pLand.Type] += pLand.m_cContents.Count;
                }

                Dictionary<Race, float> cRaceChances = new Dictionary<Race, float>();
                foreach (Race pRace in m_aLocalRaces)
                {
                    if (pRace.m_bDying)
                        continue;

                    cRaceChances[pRace] = 1.0f;// / pRace.m_pTemplate.m_iRank;

                    foreach (LandTypeInfoX pType in pRace.m_pTemplate.m_aPrefferedLands)
                        if (cLandTypesCount.ContainsKey(pType))
                            cRaceChances[pRace] *= cLandTypesCount[pType];

                    foreach (LandTypeInfoX pType in pRace.m_pTemplate.m_aHatedLands)
                        if (cLandTypesCount.ContainsKey(pType))
                            cRaceChances[pRace] /= cLandTypesCount[pType];
                }

                int iChance = Rnd.ChooseOne(cRaceChances.Values, 2);
                Race pChoosenRace = null;
                foreach (Race pRace in cRaceChances.Keys)
                {
                    iChance--;
                    if (iChance < 0)
                    {
                        pChoosenRace = pRace;
                        break;
                    }
                }

                if (pChoosenRace != null)
                {
                    pConti.m_cLocalRaces[pLandMass].Add(pChoosenRace);
                    if (pConti.m_cContents.Count == 1)
                        pConti.m_sName = pChoosenRace.m_pTemplate.m_pLanguage.RandomCountryName();
                }
            }
        }

        private void PopulateAreas()
        {
            foreach (ContinentX pConti in m_aContinents)
            {
                List<Race> cHegemonRaces = new List<Race>();
                //список рас, способных свободно расселяться по континенту
                List<Race> cAvailableContinentRaces = new List<Race>();
                foreach (LandMass<LandX> pLandMass in pConti.m_cLocalRaces.Keys)
                {
                    foreach (Race pRace in pConti.m_cLocalRaces[pLandMass])
                    {
                        if (!pRace.m_bDying && !cAvailableContinentRaces.Contains(pRace))
                            cAvailableContinentRaces.Add(pRace);
                        if (pRace.m_bHegemon && !cHegemonRaces.Contains(pRace))
                            cHegemonRaces.Add(pRace);
                    }
                }

                //List<Race> cSettledDyingRaces = new List<Race>();
                //переберём все пригодные для заселения и незаселённые территории
                foreach (AreaX pArea in pConti.m_cAreas)
                    if (!pArea.IsWater && pArea.m_pRace == null)
                    {
                        //составим список тектонических плит, к которым принадлежит рассматриваемая территория
                        List<LandMass<LandX>> cLandMasses = new List<LandMass<LandX>>();
                        foreach (LandX pLand in pArea.m_cContents)
                        {
                            LandMass<LandX> pLandMass = pLand.Owner as LandMass<LandX>;
                            if (pLandMass != null)
                            {
                                if (!cLandMasses.Contains(pLandMass))
                                    cLandMasses.Add(pLandMass);
                            }
                        }

                        //составим список рас, которые могут претендовать на эту территорию
                        List<Race> cAvailableRaces = GetAvailableRaces(cLandMasses);

                        while (cAvailableRaces.Count == 0)
                        {
                            List<LandMass<LandX>> cLandMasses2 = new List<LandMass<LandX>>(cLandMasses);

                            foreach (LandMass<LandX> pLandMass in cLandMasses)
                                foreach (LandMass<LandX> pLinkedLandMass in pLandMass.m_aBorderWith)
                                    if (!pLinkedLandMass.IsWater && !cLandMasses2.Contains(pLinkedLandMass))
                                        cLandMasses2.Add(pLinkedLandMass);

                            cLandMasses = cLandMasses2;
                            cAvailableRaces = GetAvailableRaces(cLandMasses);
                        }

                        //если эта территория принадлежит одной или нескольким плитам-колыбелям, населяем её соответствующими расами,
                        //иначе берём список рас по континенту в целом
                        if (cAvailableRaces.Count > 0)
                        {
                            //cAvailableRaces.AddRange(cDyingRaces);
                            //cAvailableRaces.AddRange(cHegemonRaces);
                            pArea.SetRace(cAvailableRaces);
                            
                            //if (cDyingRaces.Contains(pArea.m_pRace))
                            //    cSettledDyingRaces.Add(pArea.m_pRace);
                        }
                        else
                            pArea.SetRace(cAvailableContinentRaces);
                    }
            }
        }

        private List<Race> GetAvailableRaces(List<LandMass<LandX>> cLandMasses)
        {
            List<Race> cAvailableRaces = new List<Race>();
            //List<Race> cDyingRaces = new List<Race>();
            foreach (LandMass<LandX> pLandMass in cLandMasses)
            {
                ContinentX pConti = pLandMass.Owner as ContinentX;

                if (!pConti.m_cLocalRaces.ContainsKey(pLandMass))
                    continue;

                foreach (Race pRace in pConti.m_cLocalRaces[pLandMass])
                    if (!pRace.m_bDying)
                    {
                        if (!cAvailableRaces.Contains(pRace))
                        {
                            cAvailableRaces.Add(pRace);
                            if (!m_aLocalRaces.Contains(pRace))
                                throw new Exception();
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

            return cAvailableRaces;
        }

        /// <summary>
        /// Генерация мира
        /// </summary>
        /// <param name="cLocations">Сетка локаций, на которой будет строиться мир.</param>
        /// <param name="iContinents">Общее количество континентов - обособленных, разделённых водой участков суши.</param>
        /// <param name="bGreatOcean">Если true, то карта со всех сторон окружена водой.</param>
        /// <param name="iLands">Общее количество `земель` - групп соседних локаций с одинаковым типом территории</param>
        /// <param name="iProvinces">Общее количество провинций. Каждая провинция объединяет несколько сопредельных земель.</param>
        /// <param name="iStates">Общее количество государств. Каждое государство объединяет несколько сопредельных провинций.</param>
        /// <param name="iLandMasses">Общее количество тектонических плит, являющихся строительными блоками при составлении континентов.</param>
        /// <param name="iOcean">Процент тектонических плит, лежащих на дне океана - от 10 до 90.</param>
        /// <param name="iEquator">Положение экватора на карте в процентах по вертикали. 50 - середина карты, 0 - верхний край, 100 - нижний край</param>
        /// <param name="iPole">Расстояние от экватора до полюсов в процентах по вертикали. Если экватор расположен посередине карты, то значение 50 даст для полюсов верхний и нижний края карты соответственно.</param>
        /// <param name="iRacesCount">Количество различных рас, населяющих мир.</param>
        /// <param name="iMinTechLevel">Минимальный возможный в этом мире уровень технического развития.</param>
        /// <param name="iMaxTechLevel">Максимальный возможный в этом мире уровень технического развития.</param>
        /// <param name="iMinMagicLevel">Минимальный возможный в этом мире уровень владения магией.</param>
        /// <param name="iMaxMagicLevel">Максимальный возможный в этом мире уровень владения магией.</param>
        /// <param name="iInvasionProbability">Вероятность вторжения из более развитого мира (0-100%).</param>
        public World(LocationsGrid<LocationX> cLocations, 
                     int iContinents, 
                     bool bGreatOcean, 
                     int iLands, 
                     int iProvinces, 
                     int iStates, 
                     int iLandMasses, 
                     int iOcean, 
                     int iEquator, 
                     int iPole, 
                     Epoch[] aEpoches)
            : base(cLocations, iContinents, bGreatOcean, iLands, iLandMasses, iOcean, iEquator, iPole)
        {
            Create(iProvinces, iStates, aEpoches);
        }

        Epoch[] m_aEpoches;

        private void Create(int iProvinces, 
                            int iStates, 
                            Epoch[] aEpoches)
        {
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
                    PopulateWorld(pEpoch, false);
                    Reset(pEpoch);
                }

                iCounter--;

                PopulateWorld(pEpoch, iCounter == 0);
            }
        }

        private void PopulateWorld(Epoch pEpoch, bool bFinalize)
        {
            SetWorldLevels(pEpoch);

            AddRaces(pEpoch);
            if (bFinalize)
                AddInvadersRaces(pEpoch);
            DistributeRacesToLandMasses();
            PopulateAreas();

            BuildProvinces();

            BuildCities(m_pGrid.CycleShift, !bFinalize);

            BuildStates(m_pGrid.CycleShift, !bFinalize);

            foreach (ContinentX pConti in m_aContinents)
            {
                if (pConti.m_cAreas.Count == 1)
                    pConti.m_sName = pConti.m_cAreas[0].m_sName;

                if (pConti.m_cStates.Count == 1)
                    if (pConti.m_cAreas.Count == 1)
                        pConti.m_cStates[0].m_sName = pConti.m_cAreas[0].m_sName;
                    else
                        pConti.m_sName = pConti.m_cStates[0].m_sName;

                foreach (State pState in pConti.m_cStates)
                    pState.CalculateMagic();
            }
        }

        private void BuildProvinces()
        {
            List<ContinentX> cUsed = new List<ContinentX>();

            List<Province> cProvinces = new List<Province>();

            //Заново стартуем провинции, выжившие с прошлой эпохи
            if(m_aProvinces != null)
                foreach (Province pProvince in m_aProvinces)
                {
                    if (pProvince.m_pCenter.Continent != null && !cUsed.Contains(pProvince.m_pCenter.Continent))
                        cUsed.Add(pProvince.m_pCenter.Continent);

                    pProvince.Start(pProvince.m_pCenter);
                    if (!m_aLocalRaces.Contains(pProvince.m_pRace))
                        throw new Exception();

                    cProvinces.Add(pProvince);
                }
            
            List<Province> cFinished = new List<Province>();
            //Позаботимся о том, чтобы на каждом континенте была хотя бы одна провинция
            foreach (ContinentX pConti in m_aContinents)
            {
                int iUsed = 0;

                //Для каждого анклава древней цивилизации создаём собственную провинцию
                foreach (AreaX pArea in pConti.m_cAreas)
                {
                    if (pArea.m_pRace != null && pArea.m_pRace.m_bDying)
                    {
                        int iCounter = 0;
                        LandX pCradle = null;
                        do
                        {
                            pCradle = pArea.m_cContents[Rnd.Get(pArea.m_cContents.Count)];
                        }
                        while (pCradle.m_pProvince != null && iCounter++ < pArea.m_cContents.Count);

                        if (pCradle.m_pProvince == null)
                        {
                            Province pLostProvince = new Province();
                            pLostProvince.Start(pCradle);
                            cProvinces.Add(pLostProvince);
                            while (pLostProvince.Grow(pArea.m_cContents.Count)) { }
                            cFinished.Add(pLostProvince);
                        }
                        iUsed++;
                    }
                }

                if (cUsed.Contains(pConti) || pConti.m_cAreas.Count == iUsed)
                    continue;

                LandX pSeed = null;
                do
                {
                    int iIndex = Rnd.Get(pConti.m_cAreas.Count);
                    if (!pConti.m_cAreas[iIndex].IsWater)
                    {
                        int iIndex2 = Rnd.Get(pConti.m_cAreas[iIndex].m_cContents.Count);
                        pSeed = pConti.m_cAreas[iIndex].m_cContents[iIndex2];

                        bool bBorder = false;
                        foreach (LocationX pLoc in pSeed.m_cContents)
                            if (pLoc.m_bBorder)
                                bBorder = true;

                        if (pSeed.m_pRace.m_bDying || (bBorder && !Rnd.OneChanceFrom(25)))
                            pSeed = null;
                    }
                }
                while (pSeed == null);

                Province pProvince = new Province();
                pProvince.Start(pSeed);
                cProvinces.Add(pProvince);

                if (!m_aLocalRaces.Contains(pProvince.m_pRace))
                    throw new Exception();

                cUsed.Add(pConti);
            }

            while (cProvinces.Count < m_iProvincesCount)
            {
                LandX pSeed = null;
                int iCounter = 0;
                do
                {
                    int iIndex = Rnd.Get(m_aLands.Length);
                    pSeed = m_aLands[iIndex];

                    if (pSeed.Forbidden ||
                        pSeed.m_pProvince != null ||
                        pSeed.IsWater ||
                        pSeed.Owner == null ||
                        (pSeed.Owner as LandMass<LandX>).IsWater ||
                        (pSeed.m_pRace != null && pSeed.m_pRace.m_bDying))
                    {
                        pSeed = null;
                    }
                    else
                    {
                        bool bBorder = false;
                        foreach (LocationX pLoc in pSeed.m_cContents)
                            if (pLoc.m_bBorder)
                                bBorder = true;

                        if (bBorder)
                            pSeed = null;
                    }
                    if (pSeed == null)
                        iCounter++;
                    else
                        iCounter = 0;
                }
                while (pSeed == null && iCounter < m_aLands.Length * 2);

                if (pSeed == null)
                    break;

                Province pProvince = new Province();
                pProvince.Start(pSeed);
                cProvinces.Add(pProvince);

                if (!m_aLocalRaces.Contains(pProvince.m_pRace))
                    throw new Exception();
            }

            int iMaxProvinceSize = (m_aLands.Length * (100 - m_iOceansPercentage) / 100) / m_iProvincesCount;

            do
            {
                foreach (Province pProvince in cProvinces)
                {
                    if (pProvince.m_bFullyGrown)
                        continue;

                    if (!pProvince.Grow(iMaxProvinceSize))
                        cFinished.Add(pProvince);
                }
            }
            while (cFinished.Count < cProvinces.Count);

            bool bAlreadyFinished = true;
            int iCnt = 0;
            foreach (Province pProvince in cProvinces)
                pProvince.m_bFullyGrown = false;

            do
            {
                bAlreadyFinished = true;
                foreach (LandX pLand in m_aLands)
                    if (!pLand.Forbidden && !pLand.IsWater && pLand.m_pProvince == null)
                    {
                        bAlreadyFinished = false;
                        break;
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

            foreach (LandX pLand in m_aLands)
            {
                if (!pLand.Forbidden && !pLand.IsWater && pLand.m_pProvince == null)
                {
                    Province pProvince = new Province();
                    pProvince.Start(pLand);
                    cProvinces.Add(pProvince);
                    while (pProvince.Grow(iMaxProvinceSize)) { };
                }
            }

            m_aProvinces = cProvinces.ToArray();

            foreach (Province pProvince in m_aProvinces)
                pProvince.Finish(m_pGrid.CycleShift);
        }

        private void BuildStates(float fCycleShift, bool bFast)
        {
            List<ContinentX> cUsed = new List<ContinentX>();

            List<State> cStates = new List<State>();

            //Заново стартуем государства, выжившие с прошлой эпохи
            if(m_aStates != null)
                foreach (State pState in m_aStates)
                {
                    pState.Start(pState.m_pMethropoly);
                    if(!m_aLocalRaces.Contains(pState.m_pRace))
                        throw new Exception();
            
                    ContinentX pConti = pState.Owner as ContinentX;
                    if (!cUsed.Contains(pConti))
                        cUsed.Add(pConti);

                    cStates.Add(pState);
                }
            
            //Позаботимся о том, чтобы на каждом континенте было хотя бы одно государство
            foreach (ContinentX pConti in m_aContinents)
            {
                if (cUsed.Contains(pConti))
                    continue;

                Province pSeed = null;
                do
                {
                    int iIndex = Rnd.Get(pConti.m_cAreas.Count);
                    if(!pConti.m_cAreas[iIndex].IsWater)
                    {
                        int iIndex2 = Rnd.Get(pConti.m_cAreas[iIndex].m_cContents.Count);
                        pSeed = pConti.m_cAreas[iIndex].m_cContents[iIndex2].m_pProvince;

                        bool bBorder = true;
                        foreach (LandX pLand in pSeed.m_cContents)
                            foreach (LocationX pLoc in pLand.m_cContents)
                                if (!pLoc.m_bBorder)
                                    bBorder = false;

                        if (bBorder)
                            pSeed = null;

                        if (pSeed.Owner != null)
                            pSeed = null;
                    }
                }
                while (pSeed == null); 
                
                State pState = new State();
                pState.Start(pSeed);
                cStates.Add(pState);

                if (!m_aLocalRaces.Contains(pState.m_pRace))
                    throw new Exception();
                cUsed.Add(pConti);
            }

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
                        pSeed.Owner != null)
                    {
                        pSeed = null;
                    }
                    else
                    {
                        bool bBorder = false;
                        foreach (LandX pLand in pSeed.m_cContents)
                            foreach (LocationX pLoc in pLand.m_cContents)
                                if (pLoc.m_bBorder)
                                    bBorder = true;

                        if (bBorder)
                            pSeed = null;
                    }
                }
                while (pSeed == null); 

                State pState = new State();
                pState.Start(pSeed);
                cStates.Add(pState);
                if (!m_aLocalRaces.Contains(pState.m_pRace))
                    throw new Exception();
            }

            //приращиваем территории всех государств до заданного лимита
            int iMaxStateSize = m_aProvinces.Length / cStates.Count;
            bool bContinue = false;
            do
            {
                bContinue = false;
                foreach (State pState in cStates)
                    if (pState.Grow(iMaxStateSize))
                        bContinue = true;
            }
            while (bContinue);

            //если остались незанятые провинции - стартуем там дополнительные государства и приращиваем их территории до заданного лимита
            foreach(Province pProvince in m_aProvinces)
            {
                if (!pProvince.Forbidden && pProvince.Owner == null)
                {
                    State pState = new State();
                    pState.Start(pProvince);
                    cStates.Add(pState);
                    while (pState.Grow(iMaxStateSize)) { }
                }
            }

            m_aStates = cStates.ToArray();

            //строим столицы, налаживаем дипломатические связи и распределяем по континентам
            foreach (State pState in m_aStates)
            {
                pState.BuildCapital(m_aProvinces.Length / (2 * m_iStatesCount), m_aProvinces.Length / m_iStatesCount, bFast);
                pState.Finish(m_pGrid.CycleShift);

                ContinentX pConti = null;
                foreach (Province pProvince in pState.m_cContents)
                {
                    foreach (LandX pLand in pProvince.m_cContents)
                    {
                        pConti = (pLand.Owner as LandMass<LandX>).Owner as ContinentX;
                        break;
                    }
                    if (pConti != null)
                        break;
                }

                pState.Owner = pConti;
                pConti.m_cStates.Add(pState);
            }

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

        public override void PresetLandTypesInfo()
        {
            base.PresetLandTypesInfo();

            LandTypes<LandTypeInfoX>.Sea.SetResources(0, 0, 0);
            LandTypes<LandTypeInfoX>.Sea.SetStandAloneBuildingsProbability(0, 0, 1);
            LandTypes<LandTypeInfoX>.Sea.SetSettlementsDensity(0, 0, 0);
            LandTypes<LandTypeInfoX>.Sea.SetColor(Color.FromArgb(0x1e, 0x5e, 0x69));//(0x2a, 0x83, 0x93);//(0x36, 0xa9, 0xbd);//FromArgb(0xa2, 0xed, 0xfa);//LightSkyBlue;//LightCyan;

            LandTypes<LandTypeInfoX>.Tundra.SetResources(0.5f, 0, 0);
            LandTypes<LandTypeInfoX>.Tundra.SetStandAloneBuildingsProbability(1, 0, 10);
            LandTypes<LandTypeInfoX>.Tundra.SetSettlementsDensity(0.004f, 0.01f, 0.003f);
            LandTypes<LandTypeInfoX>.Tundra.SetColor(Color.FromArgb(0xc9, 0xff, 0xff));//(0xc9, 0xe0, 0xff);//PaleGreen;
            
            LandTypes<LandTypeInfoX>.Plains.SetResources(5, 0, 0);
            LandTypes<LandTypeInfoX>.Plains.SetStandAloneBuildingsProbability(1, 3, 30);
            LandTypes<LandTypeInfoX>.Plains.SetSettlementsDensity(0.01f, 0.026f, 0.1f);
            LandTypes<LandTypeInfoX>.Plains.SetColor(Color.FromArgb(0xd3, 0xfa, 0x5f));//(0xdc, 0xfa, 0x83);//LightGreen;
            
            LandTypes<LandTypeInfoX>.Savanna.SetResources(2, 0, 0);
            LandTypes<LandTypeInfoX>.Savanna.SetStandAloneBuildingsProbability(1, 3, 20);
            LandTypes<LandTypeInfoX>.Savanna.SetSettlementsDensity(0.01f, 0.023f, 0.02f);
            LandTypes<LandTypeInfoX>.Savanna.SetColor(Color.FromArgb(0xf0, 0xff, 0x8a));//(0xbd, 0xb0, 0x6b);//PaleGreen;
            
            LandTypes<LandTypeInfoX>.Desert.SetResources(0.5f, 0, 0);
            LandTypes<LandTypeInfoX>.Desert.SetStandAloneBuildingsProbability(1, 2, 30);
            LandTypes<LandTypeInfoX>.Desert.SetSettlementsDensity(0.006f, 0.01f, 0.003f);
            LandTypes<LandTypeInfoX>.Desert.SetColor(Color.FromArgb(0xfa, 0xdc, 0x36));//(0xf9, 0xfa, 0x8a);//LightYellow;
            
            LandTypes<LandTypeInfoX>.Forest.SetResources(1, 5, 0);
            LandTypes<LandTypeInfoX>.Forest.SetStandAloneBuildingsProbability(10, 5, 10);
            LandTypes<LandTypeInfoX>.Forest.SetSettlementsDensity(0.008f, 0.01f, 0.01f);
            LandTypes<LandTypeInfoX>.Forest.SetColor(Color.FromArgb(0x56, 0x78, 0x34));//(0x63, 0x78, 0x4e);//LightGreen;//ForestGreen;
            
            LandTypes<LandTypeInfoX>.Taiga.SetResources(1, 5, 0);
            LandTypes<LandTypeInfoX>.Taiga.SetStandAloneBuildingsProbability(10, 5, 10);
            LandTypes<LandTypeInfoX>.Taiga.SetSettlementsDensity(0.008f, 0.01f, 0.01f);
            LandTypes<LandTypeInfoX>.Taiga.SetColor(Color.FromArgb(0x63, 0x78, 0x4e));//LightGreen;//ForestGreen;
            
            LandTypes<LandTypeInfoX>.Jungle.SetResources(0.5f, 2, 0);
            LandTypes<LandTypeInfoX>.Jungle.SetStandAloneBuildingsProbability(10, 5, 10);
            LandTypes<LandTypeInfoX>.Jungle.SetSettlementsDensity(0.008f, 0.006f, 0.006f);
            LandTypes<LandTypeInfoX>.Jungle.SetColor(Color.FromArgb(0x8d, 0xb7, 0x31));//(0x72, 0x94, 0x28);//PaleGreen;
            
            LandTypes<LandTypeInfoX>.Swamp.SetResources(0.5f, 1, 0);
            LandTypes<LandTypeInfoX>.Swamp.SetStandAloneBuildingsProbability(10, 8, 0);
            LandTypes<LandTypeInfoX>.Swamp.SetSettlementsDensity(0.003f, 0.005f, 0.026f);
            LandTypes<LandTypeInfoX>.Swamp.SetColor(Color.FromArgb(0xa7, 0xbd, 0x6b));// DarkKhaki;
            
            LandTypes<LandTypeInfoX>.Mountains.SetResources(1, 0, 10);
            LandTypes<LandTypeInfoX>.Mountains.SetStandAloneBuildingsProbability(1, 1, 7);
            LandTypes<LandTypeInfoX>.Mountains.SetSettlementsDensity(0.004f, 0.005f, 0.006f);
            LandTypes<LandTypeInfoX>.Mountains.SetColor(Color.FromArgb(0xbd, 0x6d, 0x46));//Tan;
        }

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
                            int iHostility = pProvince.CalcHostility(pLinkedProvince);
                            if (iHostility > 2)
                            {
                                pProvince.m_cConnectionString[pLinkedProvince] = string.Format("no road due to high hostility ({0})", iHostility);
                                pLinkedProvince.m_cConnectionString[pProvince] = string.Format("no road due to high hostility ({0})", iHostility);
                                continue;
                            }

                            float fMinLength = float.MaxValue;
                            LocationX pBestTown1 = null;
                            LocationX pBestTown2 = null;
                            foreach (LocationX pTown in pProvince.m_cSettlements)
                            {
                                foreach (LocationX pOtherTown in pLinkedProvince.m_cSettlements)
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
                            if (pBestTown1 != null && State.InfrastructureLevels[pProvince.m_iInfrastructureLevel].m_iMaxGroundRoad > 0 && State.InfrastructureLevels[pLinkedProvince.m_iInfrastructureLevel].m_iMaxGroundRoad > 0)
                            {
                                int iMaxRoadLevel = 1;
                                foreach (var pRoad in pBestTown1.m_cRoads)
                                    if (pRoad.Value.Count > 0 && pRoad.Key > iMaxRoadLevel)
                                        iMaxRoadLevel = pRoad.Key;
                                foreach (var pRoad in pBestTown2.m_cRoads)
                                    if (pRoad.Value.Count > 0 && pRoad.Key > iMaxRoadLevel)
                                        iMaxRoadLevel = pRoad.Key;
                                //int iRoadLevel = 2;
                                //if (State.LifeLevels[pState.m_iLifeLevel].m_iMaxGroundRoad <= 1 && State.LifeLevels[pBorderState.m_iLifeLevel].m_iMaxGroundRoad <= 1)
                                //    iRoadLevel = 1;
                                //if (State.LifeLevels[pState.m_iLifeLevel].m_iMaxGroundRoad > 2 && State.LifeLevels[pBorderState.m_iLifeLevel].m_iMaxGroundRoad > 2)
                                //    iRoadLevel = 3;

                                BuildRoad(pBestTown1, pBestTown2, iMaxRoadLevel, fCycleShift);
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

        private void BuildSeaRoutes(float fCycleShift)
        {
            foreach (LandX pLand in m_aLands)
                pLand.m_bHarbor = false;

            foreach (LandMass<LandX> pLandMass in m_aLandMasses)
                pLandMass.m_bHarbor = false;

            List<LocationX> cHarbors = new List<LocationX>();
            foreach (Province pProvince in m_aProvinces)
            {
                if (!pProvince.Forbidden && !pProvince.m_pRace.m_bDying)
                {
                    foreach (LocationX pTown in pProvince.m_cSettlements)
                    {
                        pTown.m_bHarbor = false;

                        if (pTown.m_pSettlement.m_iRuinsAge > 0)
                            continue;

                        if (pTown.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Fort ||
                            //pTown.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Village ||
                            pTown.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Hamlet)
                            continue;

                        bool bCoastral = false;
                        foreach (ITerritory pTerr in pTown.m_aBorderWith)
                        {
                            if (pTerr.Forbidden || pTerr.Owner == null)
                                continue;

                            if ((pTerr.Owner as LandX).IsWater)
                            {
                                bCoastral = true;
                                //SetLink(pTown.m_cBorderWith[pTerr][0].m_pPoint1, pTown);
                                //SetLink(pTown.m_cBorderWith[pTerr][0].m_pPoint2, pTown);
                            }
                        }
                        if (bCoastral)
                        {
                            cHarbors.Add(pTown);
                            pTown.m_bHarbor = true;
                            (pTown.Owner as LandX).m_bHarbor = true;
                            ((pTown.Owner as LandX).Owner as LandMass<LandX>).m_bHarbor = true;
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

                Province pProvince = (pHarbor.Owner as LandX).m_pProvince;
                int iMaxNavalPath = State.InfrastructureLevels[pProvince.m_iInfrastructureLevel].m_iMaxNavalPath;
                if (iMaxNavalPath == 0)
                    continue;

                int iMaxLength = m_pGrid.RX * 10;
                if (iMaxNavalPath == 1)
                    iMaxLength /= 10;
                if (iMaxNavalPath == 2)
                    iMaxLength /= 4;

                float fMinDist = float.MaxValue;
                LocationX pClosestHarbor1 = null;
                LocationX pClosestHarbor2 = null;
                LocationX pClosestHarbor3 = null;
                foreach (LocationX pOtherHarbor in aHarbors)
                {
                    if (pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Village &&
                        (pOtherHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.City ||
                         pOtherHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Capital))
                        continue;

                    if (pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Town &&
                        pOtherHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Village)
                        continue;

                    if (pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.City &&
                        pOtherHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Village)
                        continue;

                    if (pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Capital &&
                        pOtherHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Village)
                        continue;

                    if (pHarbor != pOtherHarbor && !pHarbor.m_cHaveSeaRouteTo.Contains(pOtherHarbor))
                    {
                        float fDist = pHarbor.DistanceTo(pOtherHarbor, fCycleShift);
                        if (fDist < fMinDist)
                        {
                            fMinDist = fDist;

                            if (pClosestHarbor1 != null)
                            {
                                if (pClosestHarbor2 != null)
                                    pClosestHarbor3 = pClosestHarbor2;
                                pClosestHarbor2 = pClosestHarbor1;
                            }
                            pClosestHarbor1 = pOtherHarbor;
                        }
                    }
                }

                int iPathLevel = 3;

                if (pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Village)
                {
                    iPathLevel = 1;
                    iMaxLength = m_pGrid.RX * 10 / 10;
                }
                if (pHarbor.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Town)
                    iPathLevel = 2;

                if (pClosestHarbor1 != null)
                    BuildSeaRoute(pHarbor, pClosestHarbor1, iPathLevel, fCycleShift, iMaxLength);
                if (pClosestHarbor2 != null && iPathLevel >= 1)
                    BuildSeaRoute(pHarbor, pClosestHarbor2, iPathLevel, fCycleShift, iMaxLength);
                if (pClosestHarbor3 != null && iPathLevel >= 3)
                    BuildSeaRoute(pHarbor, pClosestHarbor3, iPathLevel, fCycleShift, iMaxLength);
            }

            FinishSeaRoutes(fCycleShift);
        }

        private Dictionary<ShortestPath, int> m_cSeaRoutesProjects = new Dictionary<ShortestPath,int>();

        private void BuildSeaRoute(LocationX pTown1, LocationX pTown2, int iPathLevel, float fCycleShift, int iMaxLength)
        {
            //TransportationNode[] aBestPath = FindBestPath(pTown1, pTown2, fCycleShift, true).m_aNodes;
            //if (aBestPath == null || aBestPath.Length == 0)
            //    aBestPath = FindBestPath(pTown2, pTown1, fCycleShift, true).m_aNodes;
            ShortestPath pBestPath = FindReallyBestPath(pTown1, pTown2, fCycleShift, true);

            if (pBestPath.m_aNodes != null && pBestPath.m_aNodes.Length > 1 && pBestPath.m_fLength < iMaxLength)
            //if (aBestPath != null && aBestPath.Length > 1)
            {
                m_cSeaRoutesProjects[pBestPath] = iPathLevel;

                pTown1.m_cHaveSeaRouteTo.Add(pTown2);
                pTown2.m_cHaveSeaRouteTo.Add(pTown1);
            }
        }

        private void FinishSeaRoutes(float fCycleShift)
        {
            foreach (var pPath in m_cSeaRoutesProjects)
            {
                ShortestPath pBestPath = FindReallyBestPath(pPath.Key.m_aNodes[0] as LocationX, 
                                                            pPath.Key.m_aNodes[pPath.Key.m_aNodes.Length - 1] as LocationX, 
                                                            fCycleShift, false);

                TransportationLink pNewLink = new TransportationLink(pPath.Key.m_aNodes);
                pNewLink.m_bEmbark = true;
                pNewLink.m_bSea = true;
                pNewLink.BuildRoad(pPath.Value);

                if (pBestPath.m_aNodes != null && 
                    pBestPath.m_aNodes.Length > 1 && 
                    pBestPath.m_fLength < pNewLink.MovementCost)
                    continue;

                SetLink(pPath.Key.m_aNodes[0], pPath.Key.m_aNodes[pPath.Key.m_aNodes.Length - 1], pNewLink);//aSeaRoute);

                LocationX pHarbor1 = pPath.Key.m_aNodes[0] as LocationX;
                LocationX pHarbor2 = pPath.Key.m_aNodes[pPath.Key.m_aNodes.Length - 1] as LocationX;

                if ((pHarbor1.Owner as LandX).m_pProvince.Owner != (pHarbor2.Owner as LandX).m_pProvince.Owner)
                {
                    Province pProvince1 = (pHarbor1.Owner as LandX).m_pProvince;
                    Province pProvince2 = (pHarbor2.Owner as LandX).m_pProvince;

                    List<object> pProvince1Neighbours = new List<object>(pProvince1.m_aBorderWith);
                    if (!pProvince1Neighbours.Contains(pProvince2))
                        pProvince1Neighbours.Add(pProvince2);
                    pProvince1.m_aBorderWith = pProvince1Neighbours.ToArray();
                    pProvince1.m_cConnectionString[pProvince2] = "ok";

                    List<object> pProvince2Neighbours = new List<object>(pProvince2.m_aBorderWith);
                    if (!pProvince2Neighbours.Contains(pProvince1))
                        pProvince2Neighbours.Add(pProvince1);
                    pProvince2.m_aBorderWith = pProvince2Neighbours.ToArray();
                    pProvince2.m_cConnectionString[pProvince1] = "ok";
                }
            }

            m_cSeaRoutesProjects.Clear();
        }

        private void BuildCities(float fCycleShift, bool bFast)
        {
            foreach (Province pProvince in m_aProvinces)
            {
                if (!pProvince.Forbidden)
                {
                    pProvince.BuildCapital(bFast);

                    pProvince.BuildSettlements(SettlementSize.City, bFast);
                    if (!bFast)
                        pProvince.BuildRoads(3, fCycleShift);

                    pProvince.BuildSettlements(SettlementSize.Town, bFast);

                    if (!bFast)
                        pProvince.BuildRoads(2, fCycleShift);
                }
            }

            if (!bFast)
                BuildInterstateRoads(fCycleShift);

            foreach (Province pProvince in m_aProvinces)
            {
                if (!pProvince.Forbidden)
                {
                    pProvince.BuildSettlements(SettlementSize.Village, bFast);

                    if (!bFast)
                        pProvince.BuildRoads(1, fCycleShift);

                    pProvince.BuildSettlements(SettlementSize.Hamlet, bFast);

                    pProvince.BuildLairs(m_aLands.Length / 125);
                }
            }

            if (!bFast)
                BuildSeaRoutes(fCycleShift);
        }

        public void Reset(Epoch pNewEpoch)
        {
            if (m_aLocalRaces == null || m_aLocalRaces.Length == 0)
                return;

            //Ассимилируем коренное население
            foreach (Province pProvince in m_aProvinces)
            {
                foreach (LandX pLand in pProvince.m_cContents)
                {
                    //если коренное население не является доминирующим, есть вероятность, что доминирующая раса вытеснит коренную.
                    if (pLand.m_pRace != pProvince.m_pRace)
                    {
                        bool bHated = false;
                        foreach (LandTypeInfoX pLTI in pProvince.m_pRace.m_pTemplate.m_aHatedLands)
                            if (pLand.Type == pLTI)
                                bHated = true;

                        if (bHated)
                            continue;

                        bool bPreferred = false;
                        foreach (LandTypeInfoX pLTI in pProvince.m_pRace.m_pTemplate.m_aPrefferedLands)
                            if (pLand.Type == pLTI)
                                bPreferred = true;

                        if (bPreferred || Rnd.OneChanceFrom(2))
                        {
                            pLand.m_pRace = pProvince.m_pRace;
                            (pLand.Area as AreaX).m_pRace = pProvince.m_pRace;
                        }
                    }
                    //иначе, если коренная раса и доминирующая совпадают, но земля не самая подходящая - есть вероятность того, что её покинут.
                    else
                    {
                        bool bHated = false;
                        foreach (LandTypeInfoX pLTI in pProvince.m_pRace.m_pTemplate.m_aHatedLands)
                            if (pLand.Type == pLTI)
                                bHated = true;

                        if (bHated && Rnd.OneChanceFrom(2))
                        {
                            pLand.m_pRace = null;
                            (pLand.Area as AreaX).m_pRace = null;
                        }
                    }
                }
            }

            //List<Race> cEraseRace = new List<Race>();
            foreach (Race pRace in m_aLocalRaces)
            {
                if (!pRace.m_bDying)
                {
                    if (pRace.m_pEpoch != pNewEpoch)
                    {
                        pRace.m_bDying = true;
                        pRace.m_bHegemon = false;
                    }
                    else
                        //if (Rnd.OneChanceFrom(3))
                        //    cEraseRace.Add(pRace);
                        //else
                            pRace.m_bHegemon = pRace.m_bHegemon || Rnd.OneChanceFrom(m_aLocalRaces.Length);
                }
                //else
                //    if (Rnd.OneChanceFrom(10))
                //        cEraseRace.Add(pRace);
            }

            //List<Race> cRaces = new List<Race>(m_aLocalRaces);

            //foreach (Race pRace in cEraseRace)
            //    cRaces.Remove(pRace);

            foreach (ContinentX pConti in m_aContinents)
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

                foreach (AreaX pArea in pConti.m_cAreas)
                {
                    if (pArea.m_pRace == null)
                        continue;

                    if (//cEraseRace.Contains(pArea.m_pRace) ||
                        (pArea.m_pRace.m_bDying && !Rnd.OneChanceFrom(100 / (pArea.m_pType.m_iMovementCost * pArea.m_pType.m_iMovementCost))))
                        pArea.m_pRace = null;

                    foreach (LandX pLand in pArea.m_cContents)
                    {
                        if (//cEraseRace.Contains(pLand.m_pRace) || 
                            pLand.m_pRace.m_bDying)
                            pLand.m_pRace = pArea.m_pRace;
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
                foreach (Province pProvince in pState.m_cContents)
                {
                    //ни одна провинция не принадлежит ни какому государству
                    pProvince.Owner = null;
                    //все земли в каждой провинции
                    foreach (LandX pLand in pProvince.m_cContents)
                    {
                        //ни одна земля не принадлежит никакой провинции
                        pLand.m_pProvince = null;
                        pLand.m_iProvincePresence = 0;
                        //все локации в каждой земле
                        foreach (LocationX pLoc in pLand.m_cContents)
                        {
                            //если есть поселение
                            if (pLoc.m_pSettlement != null)
                            {
                                //если это уже руины
                                if (pLoc.m_pSettlement.m_iRuinsAge > 0)
                                {
                                    //в зависимости от проходимости местности
                                    if (!Rnd.OneChanceFrom((int)pLand.MovementCost))
                                    {
                                        //либо наращиваем возраст руин - в труднодоступных местах
                                        pLoc.m_pSettlement.m_iRuinsAge++;
                                    }
                                    else
                                    {
                                        //либо окончательно уничтожаем все следы цивилизации - в легкодоступных местах
                                        pLoc.m_pSettlement = null;
                                        //снимаем флаг "руины" со всех путей, ведущих в эту локацию
                                        foreach (var pLink in pLoc.m_cLinks)
                                            pLink.Value.m_bRuins = false;
                                        //и убираем локацию из списка поселений в провинции
                                        //if (pProvince.m_cSettlements.Contains(pLoc))
                                        //    pProvince.m_cSettlements.Remove(pLoc);
                                    }
                                }
                                //иначе, т.е. если это ещё не руины
                                else
                                {
                                    //в зависимости от размера поселения и проходимости местности даём ему шанс стать руинами
                                    switch (pLoc.m_pSettlement.m_pInfo.m_eSize)
                                    {
                                        case SettlementSize.Village:
                                            break;
                                        case SettlementSize.Town:
                                            if (Rnd.OneChanceFrom(1 + (int)(24 / pLoc.GetMovementCost())))
                                                pLoc.m_pSettlement.m_iRuinsAge++;
                                            break;
                                        case SettlementSize.City:
                                            if (Rnd.OneChanceFrom(1 + (int)(12 / pLoc.GetMovementCost())))
                                                pLoc.m_pSettlement.m_iRuinsAge++;
                                            break;
                                        case SettlementSize.Capital:
                                            if (Rnd.OneChanceFrom(1 + (int)(6 / pLoc.GetMovementCost())))
                                                pLoc.m_pSettlement.m_iRuinsAge++;
                                            break;
                                        case SettlementSize.Fort:
                                            if (Rnd.OneChanceFrom(1 + (int)(6 / pLoc.GetMovementCost())))
                                                pLoc.m_pSettlement.m_iRuinsAge++;
                                            break;
                                    }

                                    //административные центры всегда становятся руинами - если только они крупнее деревни
                                    if (pLoc == pState.m_pMethropoly.m_pAdministrativeCenter &&
                                        pLoc.m_pSettlement.m_pInfo.m_eSize != SettlementSize.Village)
                                        pLoc.m_pSettlement.m_iRuinsAge = 1;

                                    //если поселение не превратилось в руины - значит оно полностью погибло и не оставило никаких следов
                                    if (pLoc.m_pSettlement.m_iRuinsAge == 0)
                                        pLoc.m_pSettlement = null;
                                    else
                                        //если же оно таки стало руинами - помечаем соответсвенно все пути ведущие в эту локацию, чтобы дороги обходили её стороной
                                        foreach (var pLink in pLoc.m_cLinks)
                                            pLink.Value.m_bRuins = true;

                                }
                            }

                            //так же снимаем пометку "порт" и сносим все отдельно стоящие строения
                            pLoc.m_bHarbor = false;
                            pLoc.m_pBuilding = null;
                        }//все локации в каждой земле
                    }//все земли в каждой провинции
                        
                    //очищаем список поселений в провинции
                    pProvince.m_cSettlements.Clear();
                }//все провинции в каждом государстве

                //некоторые государства вообще исчезают с лица земли
                if (!m_aLocalRaces.Contains(pState.m_pRace) || Rnd.OneChanceFrom(2))
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
            foreach (ContinentX pConti in m_aContinents)
                pConti.m_cStates.Clear();

            //уничтожаем все дороги
            foreach (TransportationLink pRoad in m_cTransportGrid)
                pRoad.ClearRoad();
            foreach (TransportationLink pRoad in m_cLandsTransportGrid)
                pRoad.ClearRoad();
            foreach (TransportationLink pRoad in m_cLMTransportGrid)
                pRoad.ClearRoad();

            //все локации в мире
            foreach (LocationX pLoc in m_pGrid.m_aLocations)
            {
                //очищаем информацию о дорогах, проходивших через локацию
                pLoc.m_cRoads.Clear();
                pLoc.m_cRoads[1] = new List<Road>();
                pLoc.m_cRoads[2] = new List<Road>();
                pLoc.m_cRoads[3] = new List<Road>();
                pLoc.m_cHaveRoadTo.Clear();
                pLoc.m_cHaveSeaRouteTo.Clear();

                //удаляем все пути с суши на море (выходить в море можно только в портах, а раз портов нет, то и путей нет)
                if (pLoc.Owner != null && !(pLoc.Owner as LandX).IsWater)
                {
                    List<TransportationNode> cErase = new List<TransportationNode>();
                    foreach (TransportationNode pNode in pLoc.m_cLinks.Keys)
                    {
                        if (pLoc.m_cLinks[pNode].m_bSea)
                            cErase.Add(pNode);
                    }
                    foreach (TransportationNode pNode in cErase)
                    {
                        pLoc.m_cLinks.Remove(pNode);
                        pNode.m_cLinks.Remove(pLoc);
                    }
                }
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

                pLoc.m_cRoads[pOldRoad.m_iLevel].Remove(pOldRoad);
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
        /// <param name="iRoadLevel">уровень новой дороги</param>
        /// <returns>true - можно строить новую дорогу, false - нельзя</returns>
        private static bool CheckOldRoad(LocationX pTown1, LocationX pTown2, int iRoadLevel)
        {
            //проверим, а нет ли уже между этими городами дороги
            while (pTown1.m_cHaveRoadTo.ContainsKey(pTown2) && pTown2.m_cHaveRoadTo.ContainsKey(pTown1))
            {
                Road pOldRoad = pTown1.m_cHaveRoadTo[pTown2];

                //если существующая дорога лучше или такая же, как та, которую мы собираемся строить, то нафиг строить?
                if (pOldRoad.m_iLevel >= iRoadLevel)
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
        /// <param name="iRoadLevel">уровень дороги</param>
        private static void BuildRoad(Road pRoad, int iRoadLevel)
        {
            LocationX pTown1 = pRoad.Locations[0];
            LocationX pTown2 = pRoad.Locations[pRoad.Locations.Length - 1];

            //если между этими двумя городами уже есть дорога не хуже - новую не строим
            if (!CheckOldRoad(pTown1, pTown2, iRoadLevel))
                return;

            LocationX pLastNode = null;
            foreach (LocationX pNode in pRoad.Locations)
            {
                if (pNode != pTown1 && !pNode.m_cHaveRoadTo.ContainsKey(pTown1))
                    pNode.m_cHaveRoadTo[pTown1] = pRoad;
                if (pNode != pTown2 && !pNode.m_cHaveRoadTo.ContainsKey(pTown2))
                    pNode.m_cHaveRoadTo[pTown2] = pRoad;

                pNode.m_cRoads[iRoadLevel].Add(pRoad);

                if (pLastNode != null)
                {
                    pLastNode.m_cLinks[pNode].BuildRoad(iRoadLevel);
                    //pNode.m_cLinks[pLastNode].BuildRoad(iRoadLevel);

                    if (pLastNode.Owner != pNode.Owner)
                    {
                        try
                        {
                            (pLastNode.Owner as LandX).m_cLinks[pNode.Owner as LandX].BuildRoad(iRoadLevel);

                            if ((pLastNode.Owner as LandX).Owner != (pNode.Owner as LandX).Owner)
                            {
                                ((pLastNode.Owner as LandX).Owner as LandMass<LandX>).m_cLinks[(pNode.Owner as LandX).Owner as LandMass<LandX>].BuildRoad(iRoadLevel);
                            }
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(ex.Message);
                        }
                    }
                }

                pLastNode = pNode;
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
                    pNewRoad = new Road(pNode, pOldRoad.m_iLevel);
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
                        pNewRoad = new Road(pNode, pOldRoad.m_iLevel);
                    }
                }
            }
            if (cRoadsChain.Count == 1)
                return;

            DestroyRoad(pOldRoad);
        
            Road[] aRoadsChain = cRoadsChain.ToArray();

            foreach (Road pRoad in aRoadsChain)
                BuildRoad(pRoad, pOldRoad.m_iLevel);
        }

        /// <summary>
        /// Строит дорогу из одного города в другой. Если дорога проходит через другие населённые пункты, она разбивается на 
        /// отдельные участки от одного поселения до другого.
        /// </summary>
        /// <param name="pTown1">первый город</param>
        /// <param name="pTown2">второй город</param>
        /// <param name="iRoadLevel">уровень строящейся дороги</param>
        /// <param name="fCycleShift">циклический сдвиг координат по горизонтали для закольцованных карт</param>
        public static void BuildRoad(LocationX pTown1, LocationX pTown2, int iRoadLevel, float fCycleShift)
        {
            if (!CheckOldRoad(pTown1, pTown2, iRoadLevel))
                return;

            //PathFinder pBestPath = new PathFinder(pTown1, pTown2, fCycleShift, -1);
            ShortestPath pBestPath = FindReallyBestPath(pTown1, pTown2, fCycleShift, false);

            if (pBestPath.m_aNodes != null && pBestPath.m_aNodes.Length > 1)
            {
                //разобьем найденный путь на участки от одного населённого пункта до другого
                List<Road> cRoadsChain = new List<Road>();
                Road pNewRoad = null;
                foreach (LocationX pNode in pBestPath.m_aNodes)
                {
                    if (pNewRoad == null)
                        pNewRoad = new Road(pNode, iRoadLevel);
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
                            pNewRoad = new Road(pNode, iRoadLevel);
                        }
                    }
                }

                Road[] aRoadsChain = cRoadsChain.ToArray();

                foreach (Road pRoad in aRoadsChain)
                    BuildRoad(pRoad, iRoadLevel);
            }
        }
    }
}
