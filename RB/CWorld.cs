using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using MIConvexHull;
using System.Windows;
using RB.Socium;
using RB.Socium.Languages;
using RB.Genetix;
using RB.Geography;
using RB.Genetix.GenetixParts;
using RB.Socium.Psichology;

namespace RB
{
    /// <summary>
    /// Каждый мир представляется совокупностью сообществ и локаций различного размера и 
    /// характеризуется тремя уровнями развития: техногенного, биогенного и культурного. 
    /// </summary>
    public class CWorld
    {
        private List<CSociety> m_cSocieties = new List<CSociety>();

        internal List<CSociety> Societies
        {
            get { return m_cSocieties; }
        }

        private List<CState> m_cStates = new List<CState>();

        internal List<CState> States
        {
            get { return m_cStates; }
        }

        private CLocation[] m_aLocations = new CLocation[] {};

        internal CLocation[] Locations
        {
            get { return m_aLocations; }
        }

        private List<CLink> m_cLinks = new List<CLink>();

        internal CLink[] Links
        {
            get { return m_cLinks.ToArray(); }
        }

        private int m_iWorldScale = 150;

        public int WorldScale
        {
            get { return m_iWorldScale; }
        }

        public const int m_iMinDist = 25;
        public const int m_iMaxDist = 35;

        //private List<int> m_cRandomTests = new List<int>();

        public CWorld(int iScale, int iCountries)
        {
            //чтобы конструторы всех рас (обращающиеся к Rnd) вызвались до остальных работ по генерации мира.
            //нужно для того, чтобы вызовы с однаковым семенем генератора случайных чисел всегда выдавали одну и ту же последовательность.
            CRace.Init();

            m_iWorldScale = iScale;

            //m_cRandomTests.Add(Rnd.Get(1000));

            GenerateMap();

            //m_cRandomTests.Add(Rnd.Get(1000));

            MarkForbiddenSubTypes();
            MarkSubTypes();

            //m_cRandomTests.Add(Rnd.Get(1000));

            RandomizeLinks();

            //m_cRandomTests.Add(Rnd.Get(1000));

            PopulateMap(iCountries);

            //m_cRandomTests.Add(Rnd.Get(1000));

            //Rnd.SetDebug(false);

            CreateFactions();

            //m_cRandomTests.Add(Rnd.Get(1000));
        }

        private void RandomizeLinks()
        {
            for (int i = 0; i < m_aLocations.Length*2; i++)
            {
                CLocation pLoc = m_aLocations[Rnd.Get(m_aLocations.Length)];
                if (pLoc.Links.Count > 1)
                {
                    CLocation pLoc2 = pLoc.Links.ElementAt(Rnd.Get(pLoc.Links.Count)).Key;

                    //if (pLoc.Type == LocationType.Settlement && pLoc2.Type == LocationType.Settlement)
                    //    continue;

                    bool bCanDelete = false;
                    foreach (var pLoc3 in pLoc.Links)
                    {
                        if (pLoc3.Key.Links.ContainsKey(pLoc2))
                            bCanDelete = true;
                    }

                    if (bCanDelete)
                    {
                        m_cLinks.Remove(pLoc.Links[pLoc2]);
                        pLoc.Links.Remove(pLoc2);
                        pLoc2.Links.Remove(pLoc);
                    }
                }
            }
        }

        private void MarkSubTypes()
        {
            List<CLocation> cFront = new List<CLocation>();
            for (int i = 0; i < m_aLocations.Length; i++)
            {
                if (m_aLocations[i].Type == LocationType.Forbidden && m_aLocations[i].Territory != Territory.Undefined)
                {
                    List<CLocation> cPossibleDirections = new List<CLocation>();
                    foreach (var pEdge in m_aLocations[i].m_cEdges)
                    {
                        if (pEdge.Key.Type != LocationType.Forbidden && pEdge.Key.Territory == Territory.Undefined)
                            cPossibleDirections.Add(pEdge.Key);
                    }
                    if (cPossibleDirections.Count > 0 && Rnd.OneChanceFrom(5))
                    {
                        int iLoc = Rnd.Get(cPossibleDirections.Count);
                        switch (m_aLocations[i].Territory.Type)
                        {
                            case Territory.TerritoryType.DeadDesert:
                                cPossibleDirections[iLoc].Territory = Territory.Wastes;
                                break;
                            case Territory.TerritoryType.DeadMountains:
                                cPossibleDirections[iLoc].Territory = Territory.Hills;
                                break;
                            case Territory.TerritoryType.DeadSea:
                                cPossibleDirections[iLoc].Territory = Territory.Plains;
                                break;
                            case Territory.TerritoryType.DeadSwamp:
                                cPossibleDirections[iLoc].Territory = Territory.Forest;
                                break;
                        }
                        cFront.Add(cPossibleDirections[iLoc]);
                    }
                }
            }
            for (int i = 0; i < 10; i++)
            {
                int iLoc = -1;
                do
                {
                    iLoc = Rnd.Get(m_aLocations.Length);
                }
                while (m_aLocations[iLoc].Type == LocationType.Forbidden || m_aLocations[iLoc].Territory != Territory.Undefined);

                int iSubType = Rnd.Get(6);
                switch (iSubType)
                {
                    case 0: m_aLocations[iLoc].Territory = Territory.Wastes;
                        break;
                    case 1: m_aLocations[iLoc].Territory = Territory.Hills;
                        break;
                    case 2: m_aLocations[iLoc].Territory = Territory.Plains;
                        break;
                    case 3: m_aLocations[iLoc].Territory = Territory.Plains;
                        break;
                    case 4: m_aLocations[iLoc].Territory = Territory.Plains;
                        break;
                    case 5: m_aLocations[iLoc].Territory = Territory.Forest;
                        break;
                }
                cFront.Add(m_aLocations[iLoc]);
            }

            bool bExpanding = false;
            do
            {
                List<CLocation> cNewFront = new List<CLocation>();
                bExpanding = false;
                for (int i = 0; i < cFront.Count; i++)
                {
                    if (cFront[i].Type != LocationType.Forbidden && cFront[i].Territory != Territory.Undefined)
                    {
                        List<CLocation> cPossibleDirections = new List<CLocation>();
                        foreach (var pEdge in cFront[i].m_cEdges)
                        {
                            if (pEdge.Key.Type != LocationType.Forbidden && pEdge.Key.Territory == Territory.Undefined)
                                cPossibleDirections.Add(pEdge.Key);
                        }
                        if (cPossibleDirections.Count > 0)
                        {
                            int iLoc = Rnd.Get(cPossibleDirections.Count);
                            cPossibleDirections[iLoc].Territory = cFront[i].Territory;
                            bExpanding = true;
                            cNewFront.Add(cPossibleDirections[iLoc]);
                        }
                    }
                }
                cFront.AddRange(cNewFront);
            }
            while (bExpanding);
        }

        private void MarkForbiddenSubTypes()
        {
            List<CLocation> cFront = new List<CLocation>();

            for (int i = 0; i < 20; i++)
            {
                int iLoc = -1;
                do
                {
                    iLoc = Rnd.Get(m_aLocations.Length);
                }
                while (m_aLocations[iLoc].Type != LocationType.Forbidden || m_aLocations[iLoc].Territory != Territory.Undefined);

                int iSubType = Rnd.Get(6);
                switch (iSubType)
                {
                    case 0: m_aLocations[iLoc].Territory = Territory.DeadDesert;
                        break;
                    case 1: m_aLocations[iLoc].Territory = Territory.DeadMountains;
                        break;
                    case 2: m_aLocations[iLoc].Territory = Territory.DeadSea;
                        break;
                    case 3: m_aLocations[iLoc].Territory = Territory.DeadSea;
                        break;
                    case 4: m_aLocations[iLoc].Territory = Territory.DeadSea;
                        break;
                    case 5: m_aLocations[iLoc].Territory = Territory.DeadSwamp;
                        break;
                }
                if (m_aLocations[iLoc].Type != LocationType.Forbidden)
                    throw new Exception();

                cFront.Add(m_aLocations[iLoc]);
            }

            bool bExpanding = false;
            do
            {
                List<CLocation> cNewFront = new List<CLocation>();
                bExpanding = false;
                for (int i = 0; i < cFront.Count; i++)
                {
                    if (cFront[i].Type == LocationType.Forbidden && cFront[i].Territory != Territory.Undefined)
                    {
                        List<CLocation> cPossibleDirections = new List<CLocation>();
                        foreach (var pEdge in cFront[i].m_cEdges)
                        {
                            if (pEdge.Key.Type == LocationType.Forbidden && pEdge.Key.Territory == Territory.Undefined)
                                cPossibleDirections.Add(pEdge.Key);
                        }
                        if (cPossibleDirections.Count > 0)
                        {
                            int iLoc = Rnd.Get(cPossibleDirections.Count);
                            cPossibleDirections[iLoc].Territory = cFront[i].Territory;
                            if (cPossibleDirections[iLoc].Type != LocationType.Forbidden)
                                throw new Exception();
                            bExpanding = true;
                            cNewFront.Add(cPossibleDirections[iLoc]);
                        }
                    }
                }
                cFront.AddRange(cNewFront);
            }
            while (bExpanding);

            for (int i = 0; i < m_aLocations.Length; i++)
            {
                if (m_aLocations[i].Type == LocationType.Forbidden && m_aLocations[i].Territory == Territory.Undefined)
                {
                    int iSubType = Rnd.Get(4);
                    switch (iSubType)
                    {
                        case 0: m_aLocations[i].Territory = Territory.DeadDesert;
                            break;
                        case 1: m_aLocations[i].Territory = Territory.DeadMountains;
                            break;
                        case 2: m_aLocations[i].Territory = Territory.DeadSea;
                            break;
                        case 3: m_aLocations[i].Territory = Territory.DeadSwamp;
                            break;
                    }
                    if (m_aLocations[i].Type != LocationType.Forbidden)
                        throw new Exception();

                    foreach (var pEdge in m_aLocations[i].m_cEdges)
                    {
                        if (pEdge.Key.Type == LocationType.Forbidden && pEdge.Key.Territory != Territory.Undefined)
                        {
                            m_aLocations[i].Territory = pEdge.Key.Territory;
                            if (m_aLocations[i].Type != LocationType.Forbidden)
                                throw new Exception();
                        }
                        if (pEdge.Key.Type == LocationType.Forbidden && pEdge.Key.Territory == Territory.Undefined)
                        {
                            pEdge.Key.Territory = m_aLocations[i].Territory;
                            if (pEdge.Key.Type != LocationType.Forbidden)
                                throw new Exception();
                        }
                    }
                }
            }

        }


        public CNation[] m_aLocalNations = null;

        public List<CPerson> m_cPersons = new List<CPerson>();

        public CPerson GetPossibleBoss()
        {
            float fMaxInfluence = float.MinValue;
            CPerson pBigBoss = null;
            foreach (CPerson pPerson in m_cPersons)
            {
                if (pPerson.m_pPatron != null)
                    continue;

                float fInfluence = pPerson.GetInfluence(true);
                if (fInfluence > fMaxInfluence)
                {
                    fMaxInfluence = fInfluence;
                    pBigBoss = pPerson;
                }
            }

            return pBigBoss;
        }

        #region PopulateMap() and helper functions
        private void PopulateMap(int iCountries)
        {
            //m_cRandomTests.Add(1000 + Rnd.Get(1000));

            #region Creating races and nations
            foreach (CRace pRace in CRace.m_cAllRaces)
            {
                pRace.ResetNations();
                pRace.m_pFenotype.m_pHairs.CheckHairColors();
            }
            Language.ResetUsedLists();
            CEpoch.s_cUsedNames.Clear();

            //m_cRandomTests.Add(1000 + Rnd.Get(1000));

            int iRacesCount = 1 + Rnd.Get(3);
            CRace[] aRaces = new CRace[iRacesCount];
            for (int i = 0; i < iRacesCount; i++)
            {
                int iRace = Rnd.Get(CRace.Preset.Humans.Races.Length);
                aRaces[i] = CRace.Preset.Humans.Races[iRace];
            }

            //Rnd.SetDebug(true);

            //m_cRandomTests.Add(1000 + Rnd.Get(1000));

            CEpoch pEpoch = new CEpoch(CEpoch.ProgressPreset.HistoricalMedieval, aRaces, iCountries);

            List<CNation> cNations = new List<CNation>();
            while (cNations.Count < pEpoch.m_iNativesCount)
            {
                int iChance = Rnd.Get(pEpoch.m_aNatives.Length);

                CNation pNation = new CNation(pEpoch.m_aNatives[iChance], pEpoch);//new Race(pRaceTemplate, pEpoch);
                pNation.Accommodate(pEpoch);

                cNations.Add(pNation);
            }

            m_aLocalNations = cNations.ToArray();
            #endregion

            //m_cRandomTests.Add(1000 + Rnd.Get(1000));

            #region Creating states
            for (int i = 0; i < iCountries; i++)
            {
                CNation pNation = m_aLocalNations[i];

                Dictionary<int, float> cChances = new Dictionary<int, float>();
                for (int j = 0; j < 10; j++)
                {
                    int iLoc = -1;
                    do
                    {
                        iLoc = Rnd.Get(m_aLocations.Length);
                    }
                    while (m_aLocations[iLoc].Owner != null || m_aLocations[iLoc].Type == LocationType.Forbidden || cChances.ContainsKey(iLoc));

                    int iCost = m_aLocations[iLoc].GetClaimingCost(pNation);
                    cChances[iLoc] = (float)1000.0 / iCost;
                }

                int iChoice = Rnd.ChooseOne(cChances.Values);
                CState pNewState = new CState(m_aLocations[cChances.ElementAt(iChoice).Key], pNation);
                m_cStates.Add(pNewState);
            }
            #endregion

            //m_cRandomTests.Add(1000 + Rnd.Get(1000));

            #region Expanding states
            bool bExpanding = false;
            do
            {
                bExpanding = false;
                for (int i = 0; i < m_aLocations.Length; i++)
                {
                    if (m_aLocations[i].Owner != null)
                    {
                        Dictionary<CLocation, float> cPossibleDirections = new Dictionary<CLocation, float>();
                        foreach (var pLink in m_aLocations[i].Links)
                        {
                            if (pLink.Key.Owner == null && pLink.Key.Type != LocationType.Forbidden)
                            {
                                int iCost = pLink.Key.GetClaimingCost(m_aLocations[i].Owner.m_pNation);
                                cPossibleDirections[pLink.Key] = (float)1000.0 / iCost;
                            }
                        }
                        if (cPossibleDirections.Count > 0)
                        {
                            int iChoice = Rnd.ChooseOne(cPossibleDirections.Values);
                            CLocation pNewProvince = cPossibleDirections.ElementAt(iChoice).Key;

                            pNewProvince.Owner = m_aLocations[i].Owner;
                            m_aLocations[i].Owner.Lands.Add(pNewProvince);
                            bExpanding = true;
                        }
                    }
                }
            }
            while(bExpanding);
            #endregion

            //m_cRandomTests.Add(1000 + Rnd.Get(1000));

            #region Creating settlements and estates
            int iPopulatedLocations = 0;
            for (int i = 0; i < m_aLocations.Length; i++)
                if (m_aLocations[i].Owner != null)
                    iPopulatedLocations++;

            
            for (int i = 0; i < m_cStates.Count; i++)
                m_cStates[i].BuildUp(iPopulatedLocations / m_cStates.Count);
            #endregion

            //m_cRandomTests.Add(1000 + Rnd.Get(1000));

            #region Add states' and cities' rulers
            for (int i = 0; i < m_cStates.Count; i++)
            {
                foreach (CLocation pSettlement in m_cStates[i].Settlements)
                {
                    foreach (Building pBuilding in pSettlement.Settlement.m_cBuildings)
                    {
                        if (pBuilding.m_pInfo == m_cStates[i].m_pInfo.m_pStateCapital.m_pMainBuilding)
                        {
                            CPerson pRuler = new CPerson(this, pSettlement, pBuilding, true);
                            pRuler.FixReferences();
                            m_cPersons.Add(pRuler);
                            if (pRuler.m_pCustoms.m_eMarriage != Customs.MarriageType.Polyamory && pRuler.m_pCustoms.m_eSexRelations != Customs.SexualOrientation.Homosexual)
                            {
                                CPerson pSpouse = new CPerson(this, pSettlement, pBuilding, true);
                                pSpouse.FixReferences();
                                m_cPersons.Add(pSpouse);
                            }
                            CPerson pHeir = new CPerson(this, pSettlement, pBuilding, false);
                            pHeir.FixReferences();
                            m_cPersons.Add(pHeir);
                            if (Rnd.Chances(1, 2))
                            {
                                CPerson pMinorHeir = new CPerson(this, pSettlement, pBuilding, false);
                                pMinorHeir.FixReferences();
                                m_cPersons.Add(pMinorHeir);
                            }
                        }
                    }
                }
            }
            //Сначала создаём всех государственных правителей, затем только региональных - для того, чтобы у царей были шансы породниться.
            for (int i = 0; i < m_cStates.Count; i++)
            {
                foreach (CLocation pSettlement in m_cStates[i].Settlements)
                {
                    foreach (Building pBuilding in pSettlement.Settlement.m_cBuildings)
                    {
                        if (pBuilding.m_pInfo == m_cStates[i].m_pInfo.m_pProvinceCapital.m_pMainBuilding)
                        {
                            CPerson pRuler = new CPerson(this, pSettlement, pBuilding, true);
                            pRuler.FixReferences();
                            m_cPersons.Add(pRuler);
                            if (pRuler.m_pCustoms.m_eMarriage != Customs.MarriageType.Polyamory && pRuler.m_pCustoms.m_eSexRelations != Customs.SexualOrientation.Homosexual && Rnd.Chances(1, 2))
                            {
                                CPerson pSpouse = new CPerson(this, pSettlement, pBuilding, true);
                                pSpouse.FixReferences();
                                m_cPersons.Add(pSpouse);
                            }
                            //CPerson pHeir = new CPerson(pSettlement, pBuilding, false);
                            //pHeir.FixReferences();
                            //m_cPersons.Add(pHeir);
                        }
                    }
                }
            }
            #endregion

            //m_cRandomTests.Add(1000 + Rnd.Get(1000));

            #region Add other random characters
            int iCharactersCount = m_cPersons.Count;// *2;
            for (int i = 0; i < iCharactersCount; i++)
            {
                CPerson pPerson = null;
                do
                {
                    try
                    {
                        pPerson = new CPerson(this, null);
                    }
                    catch(Exception ex)
                    {
                        pPerson = null;
                    }
                }
                while(pPerson == null);

                pPerson.FixReferences();
                m_cPersons.Add(pPerson);
            }
            #endregion

            //m_cRandomTests.Add(1000 + Rnd.Get(1000));

            #region Fix missed family relations and age of all characters
            bool bFixed;
            do
            {
                bFixed = false;
                foreach (CPerson pPerson in m_cPersons)
                    if (pPerson.FixKinship())
                        bFixed = true;
            }
            while (bFixed);

            do
            {
                bFixed = false;
                foreach (CPerson pPerson in m_cPersons)
                    if (pPerson.FixAge())
                        bFixed = true;
            }
            while (bFixed);

            foreach (CPerson pPerson in m_cPersons)
                pPerson.ApplyInjuries();
            #endregion

            foreach (CPerson pPerson in m_cPersons)
            {
                pPerson.FixSubordination(this);
                pPerson.FixHealth();
            }

            //m_cRandomTests.Add(1000 + Rnd.Get(1000));

            //foreach (CPerson pPerson in m_cPersons)
            //{
            //    pPerson.RemoveTrashAssociates();
            //}

            //*
            #region Search for isolated social groups ("islands"), members of wich aren't connected to anyone out of their "island"
            List<CPerson> cProcessed = new List<CPerson>();
            List<List<CPerson>> cIslands = new List<List<CPerson>>();
            foreach (CPerson pPerson in m_cPersons)
            {
                if (cProcessed.Contains(pPerson))
                    continue;

                List<CPerson> cIsland = new List<CPerson>();
                cIsland.Add(pPerson);
                cProcessed.Add(pPerson);

                List<CPerson> cCurrentWave = new List<CPerson>();
                List<CPerson> cNewWave = new List<CPerson>();
                cCurrentWave.Add(pPerson);
                do
                {
                    cNewWave.Clear();
                    foreach (CPerson pActive in cCurrentWave)
                    {
                        foreach (var pRelative in pActive.m_cRelations)
                        {
                            if (cProcessed.Contains(pRelative.Key))
                                continue;

                            cNewWave.Add(pRelative.Key);
                            cProcessed.Add(pRelative.Key);
                        }
                    }
                    cIsland.AddRange(cNewWave);
                    cCurrentWave.Clear();
                    cCurrentWave.AddRange(cNewWave);
                }
                while (cCurrentWave.Count > 0);

                cIslands.Add(cIsland);
            }
            #endregion

            //m_cRandomTests.Add(1000 + Rnd.Get(1000));

            float fIdealSimilarity;
            KeyValuePair<List<CPerson>, List<CPerson>>? pMostSimilarPair;
            do
            {
                #region Searching for a most similar pair of islands and stores it in pMostSimilarPair
                fIdealSimilarity = 0;
                pMostSimilarPair = null;
                foreach (var cIsland1 in cIslands)
                {
                    if (cIsland1.Count == 0)
                        continue;

                    float fBestProximity = 0;
                    List<CPerson> cMostSimilarIsland = null;

                    foreach (var cIsland2 in cIslands)
                    {
                        if (cIsland2.Count == 0)
                            continue;

                        if (cIsland1 == cIsland2)
                            continue;

                        float fProximity = 0;
                        foreach (CPerson pPerson1 in cIsland1)
                        {
                            foreach (CPerson pPerson2 in cIsland2)
                            {
                                float fProx = (float)Math.Pow(100*CPerson.GetProximity(pPerson1, pPerson2), 3);
                                if (fProx > fProximity)
                                    fProximity = fProx;
                            }
                        }
                        //fSimilarity /= cIsland2.Count;
                        //fSimilarity /= cIsland1.Count;

                        if (fProximity > fBestProximity)
                        {
                            fBestProximity = fProximity;
                            cMostSimilarIsland = cIsland2;
                            
                            //For DEBUG only!
                            //fSimilarity = 0;
                            //foreach (CPerson pPerson1 in cIsland1)
                            //{
                            //    foreach (CPerson pPerson2 in cIsland2)
                            //    {
                            //        fSimilarity += (float)Math.Pow(CPerson.GetSimilarity(pPerson1, pPerson2), 3);
                            //    }
                            //}
                            //For DEBUG only!
                        }
                    }

                    if (cMostSimilarIsland != null)
                    {
                        if (fBestProximity > fIdealSimilarity)
                        {
                            fIdealSimilarity = fBestProximity;
                            pMostSimilarPair = new KeyValuePair<List<CPerson>, List<CPerson>>(cIsland1, cMostSimilarIsland);
                        }
                    }
                }
                #endregion

                #region Creating new relations between persons from found pair of islands and merges islands
                if (pMostSimilarPair.HasValue)
                {
                    List<CPerson> cIsland = pMostSimilarPair.Value.Key;
                    List<CPerson> cOtherIsland = pMostSimilarPair.Value.Value;
                    //cIsland - всегда меньший из двух
                    if (cIsland.Count > cOtherIsland.Count)
                    {
                        cOtherIsland = pMostSimilarPair.Value.Key;
                        cIsland = pMostSimilarPair.Value.Value;
                    }
                    int iLinks = cIsland.Count > 1 ? (int)Math.Sqrt(cIsland.Count - 1) : 1;

                    bool bOK = false;
                    for (int i = 0; i < iLinks; i++)
                    {
                        Dictionary<CPerson, float> cPretenders = new Dictionary<CPerson, float>();
                        foreach (CPerson pPerson in cIsland)
                        {
                            cPretenders[pPerson] = 1.0f / (pPerson.m_cRelations.Count + 1);
                        }
                        CPerson pP1 = cPretenders.Keys.ElementAt(Rnd.ChooseOne(cPretenders.Values, 3));

                        Dictionary<CPerson, float> cChances = new Dictionary<CPerson, float>();
                        foreach (CPerson pOtherPerson in cOtherIsland)
                        {
                            if (pP1.m_cRelations.ContainsKey(pOtherPerson))
                                continue;

                            cChances[pOtherPerson] = CPerson.GetProximity(pP1, pOtherPerson);
                        }

                        if (cChances.Count > 0)
                        {
                            CPerson pP2 = cChances.Keys.ElementAt(Rnd.ChooseBest(cChances.Values));

                            pP1.AddRelation(pP2);

                            //For DEBUG only!
                            float fProx = CPerson.GetProximity(pP1, pP2);
                            bOK = true;
                        }
                    }

                    if (bOK)
                    {
                        cIsland.AddRange(cOtherIsland);
                        cOtherIsland.Clear();
                    }
                }
                #endregion
            }
            while (pMostSimilarPair.HasValue);
            //*/

            //m_cRandomTests.Add(1000 + Rnd.Get(1000));

            foreach (CPerson pPerson in m_cPersons)
                pPerson.UpdateRelations();

            //m_cRandomTests.Add(1000 + Rnd.Get(1000));

            foreach (CPerson pPerson in m_cPersons)
                pPerson.AddDrama();

            //m_cRandomTests.Add(1000 + Rnd.Get(1000));

            foreach (CPerson pPerson in m_cPersons)
                pPerson.SetFullName();

            //m_cRandomTests.Add(1000 + Rnd.Get(1000));
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
        public CLocation GetRandomSettlement(CLocation pPreferredLoc, CEstate.Position eEstate)
        {
            if (pPreferredLoc != null && pPreferredLoc.HaveEstate(eEstate))
                return pPreferredLoc;

            List<CLocation> pPossibleSettlements = new List<CLocation>();

            CState pState = m_cStates[Rnd.Get(m_cStates.Count)];
            if (pPreferredLoc != null)
                pState = pPreferredLoc.Owner;

            foreach (CLocation pLocation in pState.Settlements)
            {
                if (pLocation.HaveEstate(eEstate))
                    pPossibleSettlements.Add(pLocation);
            }

            if (pPossibleSettlements.Count == 0)
            {
                pState = m_cStates[Rnd.Get(m_cStates.Count)];

                foreach (CLocation pLocation in pState.Settlements)
                {
                    if (pLocation.HaveEstate(eEstate))
                        pPossibleSettlements.Add(pLocation);
                }

                if (pPossibleSettlements.Count == 0)
                {
                    foreach (CLocation pLocation in m_aLocations)
                    {
                        if (pLocation.HaveEstate(eEstate))
                            pPossibleSettlements.Add(pLocation);
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
        public CPerson GetPossibleRelative(CLocation pPreferredHome)
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
        private CPerson GetPossibleRelative(CLocation pPreferredHome, CState pPreferredState)
        {
            if (m_cPersons.Count == 0)
                return null;

            Dictionary<CPerson, float> cRelatives = new Dictionary<CPerson, float>();

            foreach (CPerson pPerson in m_cPersons)
            {
                if (pPreferredHome != null && pPerson.m_pHomeLocation != pPreferredHome)
                    continue;

                if (pPreferredState != null && pPerson.m_pHomeLocation.Owner != pPreferredState)
                    continue;

                //if (!pPerson.CouldInviteNewDwellers())
                //    continue;

                float fRelativesCount = pPerson.GetFamilySize();// m_cRelations.Count();

                if (fRelativesCount == 0)
                    fRelativesCount = 1;

                if (pPerson.m_pNation.m_pFenotype.m_pLifeCycle.m_eBirthRate == BirthRate.Moderate)
                    fRelativesCount *= 2;

                if (pPerson.m_pNation.m_pFenotype.m_pLifeCycle.m_eBirthRate == BirthRate.Low)
                    fRelativesCount *= 5;

                if (fRelativesCount > 10)
                    continue;

                int iNeighbours = 1;
                foreach (Building pBuilding in pPerson.m_pHomeLocation.Settlement.m_cBuildings)
                    iNeighbours += pBuilding.m_cPersons.Count();

                switch (pPerson.m_pHomeLocation.Settlement.m_pInfo.m_eSize)
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
        public CPerson GetPossibleRelative(CNation pPreferredNation, int iMaxCivilizationLevel)
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
        private CPerson GetPossibleRelative(CNation pPreferredNation, CRace pPreferredRace, int iMaxCivilizationLevel)
        {
            if (m_cPersons.Count == 0)
                return null;

            Dictionary<CPerson, float> cRelatives = new Dictionary<CPerson, float>();

            foreach (CPerson pPerson in m_cPersons)
            {
                if (pPreferredRace != null && pPerson.m_pNation.m_pRace != pPreferredRace)
                    continue;

                if (pPreferredNation != null && pPerson.m_pNation != pPreferredNation)
                    continue;

                if (pPerson.m_pHomeLocation.Owner.m_iInfrastructureLevel > iMaxCivilizationLevel)
                    continue;

                //if (!pPerson.CouldInviteNewDwellers())
                //    continue;

                float fRelativesCount = pPerson.GetFamilySize();// m_cRelations.Count();

                if (fRelativesCount == 0)
                    fRelativesCount = 1;

                if (pPerson.m_pNation.m_pFenotype.m_pLifeCycle.m_eBirthRate == BirthRate.Moderate)
                    fRelativesCount *= 2;

                if (pPerson.m_pNation.m_pFenotype.m_pLifeCycle.m_eBirthRate == BirthRate.Low)
                    fRelativesCount *= 5;

                if (fRelativesCount > 10)
                    continue;

                int iNeighbours = 1;
                foreach (Building pBuilding in pPerson.m_pHomeLocation.Settlement.m_cBuildings)
                    iNeighbours += pBuilding.m_cPersons.Count();

                switch (pPerson.m_pHomeLocation.Settlement.m_pInfo.m_eSize)
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
        #endregion

        #region GenerateMap() and helper functions
        private void GenerateMap()
        {
            int iX, iY;

            List<CLocation> cLocations = new List<CLocation>();

            // Creating 3 rings of outer forbidden locations
            // inner ring
            double fAngle = (double)(m_iMinDist + Rnd.Get(m_iMaxDist - m_iMinDist)) / m_iWorldScale;
            do
            {
                int iR = m_iWorldScale - (m_iMaxDist - m_iMinDist)/2 + Rnd.Get(m_iMaxDist - m_iMinDist);
                iX = (int)(iR * Math.Cos(fAngle));
                iY = (int)(iR * Math.Sin(fAngle));
                AddLocation(ref cLocations, iX, iY, true);

                int iDelta = m_iMinDist + Rnd.Get(m_iMaxDist - m_iMinDist);
                fAngle += (double)iDelta / m_iWorldScale;
            }
            while (fAngle < 2 * Math.PI);

            // middle ring
            fAngle -= 2 * Math.PI;
            do
            {
                int iR = m_iWorldScale + m_iMinDist + Rnd.Get(m_iMaxDist - m_iMinDist);
                iX = (int)(iR * Math.Cos(fAngle));
                iY = (int)(iR * Math.Sin(fAngle));
                AddLocation(ref cLocations, iX, iY, true);

                int iDelta = m_iMinDist + Rnd.Get(m_iMaxDist - m_iMinDist);
                fAngle += (double)iDelta / m_iWorldScale;
            }
            while (fAngle < 2 * Math.PI);

            // outer ring
            fAngle -= 2 * Math.PI;
            do
            {
                iX = (int)(m_iWorldScale * 2 * Math.Cos(fAngle));
                iY = (int)(m_iWorldScale * 2 * Math.Sin(fAngle));
                AddLocation(ref cLocations, iX, iY, true);

                int iDelta = m_iMinDist + Rnd.Get(m_iMaxDist - m_iMinDist);
                fAngle += (double)iDelta * 0.5 / m_iWorldScale;
            }
            while (fAngle < 2 * Math.PI);

            // add common locations
            int iLocationsCount = cLocations.Count + (int)(Math.Pow(m_iWorldScale, 2) * Math.PI / 1500);
            int iTriesCount = 0;
            while (cLocations.Count < iLocationsCount && iTriesCount++ < iLocationsCount * 1000)
            {
                iX = m_iWorldScale - Rnd.Get(m_iWorldScale * 2);
                iY = m_iWorldScale - Rnd.Get(m_iWorldScale * 2);

                if (PossibleLocation(ref cLocations, iX, iY, false))
                    AddLocation(ref cLocations, iX, iY, false);
            }

            // fill remained gaps with impassable locations
            iTriesCount = 0;
            while (iTriesCount++ < iLocationsCount*1000)
            {
                iX = m_iWorldScale - Rnd.Get(m_iWorldScale * 2);
                iY = m_iWorldScale - Rnd.Get(m_iWorldScale * 2);

                if(PossibleLocation(ref cLocations, iX, iY, true))
                    AddLocation(ref cLocations, iX, iY, true);
            }

            //List<CLocation> cEmpty = new List<CLocation>();
            //for (int i = 0; i < m_cLocations.Count; i++)
            //{
            //    if (m_cLocations[i].Links.Count == 0)
            //        cEmpty.Add(m_cLocations[i]);
            //}

            //foreach (CLocation pLoc in cEmpty)
            //    m_cLocations.Remove(pLoc);

            m_aLocations = cLocations.ToArray();

            //Наконец, строим диаграмму Вороного.
            VoronoiMesh<CLocation, CLocation.VoronoiCell, VoronoiEdge<CLocation, CLocation.VoronoiCell>> voronoiMesh = VoronoiMesh.Create<CLocation, CLocation.VoronoiCell>(m_aLocations);

            //Переведём результат в удобный нам формат.
            //Для каждого найденного ребра диаграммы Вороного найдём локации, которые оно разделяет
            RebuildEdges(voronoiMesh.Edges);
        }

        /// <summary>
        /// Восстанавливаем информацию о смежных гранях по выходным данным MIConvexHull.
        /// Возвращает минимальный Rect, полностью включающий в себя все вершины.
        /// </summary>
        /// <param name="locations"></param>
        /// <param name="cEdges"></param>
        /// <returns></returns>
        private void RebuildEdges(IEnumerable<VoronoiEdge<CLocation, CLocation.VoronoiCell>> cEdges)
        {
            foreach (var edge in cEdges)
            {
                var from = edge.Source;
                var to = edge.Target;

                CLocation pLeft = null;
                CLocation pRight = null;

                //для этого просканируем все локации, имеющие связь с начальной точкой ребра
                foreach (var n in from.Vertices)
                {
                    //и с конечной точкой ребра
                    foreach (var nnn in to.Vertices)
                    {
                        //нас интересуют те 2 локации, которые будут в обеих списках
                        if (n == nnn)
                        {
                            if (pLeft == null)
                                pLeft = n;
                            else
                            {
                                if (pRight == null)
                                    pRight = n;
                                else
                                    throw new Exception("Нашли больше 2 локаций с общей границей!");
                            }
                        }
                    }
                }

                if (pLeft == null || pRight == null)
                    throw new Exception("У границы меньше 2 сопредельных локаций!");

                //для порядка, определим порядок обхода вершин из локации - по часовой стрелке
                if (IsLeft(pLeft.ToPoint(), from.Circumcenter, to.Circumcenter) < 0)
                {
                    CLocation pSwap = pLeft;
                    pLeft = pRight;
                    pRight = pSwap;
                }

                CLocation.VoronoiCell pMiddle = new CLocation.VoronoiCell(from, to);
                //CLocation.Cell pInnerLeft = new CLocation.Cell(pMiddle, pLeft);
                //CLocation.Cell pInnerRight = new CLocation.Cell(pMiddle, pRight);

                //пропишем ссылки на ребро в найденных локациях
                pLeft.m_cEdges[pRight] = new CLocation.VoronoiEdge(from, to, pMiddle);//, pInnerLeft);
                pRight.m_cEdges[pLeft] = new CLocation.VoronoiEdge(to, from, pMiddle);//, pInnerRight);
            }
        }

        static int IsLeft(Point a, Point b, Point c)
        {
            decimal ax = (decimal)a.X;
            decimal bx = (decimal)b.X;
            decimal cx = (decimal)c.X;
            decimal ay = (decimal)a.Y;
            decimal by = (decimal)b.Y;
            decimal cy = (decimal)c.Y;
            return ((bx - ax) * (cy - ay) - (by - ay) * (cx - ax)) > 0 ? 1 : -1;
        }

        private CLocation AddLocation(ref List<CLocation> cLocations, int iX, int iY, bool bForbidden)
        {
            CLocation pNewLocation = new CLocation(iX, iY);
            if (bForbidden)
                pNewLocation.Type = LocationType.Forbidden;
            cLocations.Add(pNewLocation);

            if (!bForbidden)
            {
                int k;
                int try_count = -1;
                do
                {
                    k = MakeLink(ref cLocations, pNewLocation);
                    try_count++;
                    //if (k == -1)
                    //    k = pNewLocation.MaxLinks + 1;
                }
                while (try_count < 2 * pNewLocation.MaxLinks);
                //while (Rnd.Get(pNewLocation.MaxLinks * pNewLocation.MaxLinks) <= k * k && try_count < 2 * pNewLocation.MaxLinks) ;
            }

            return pNewLocation;
        }

        private int MakeLink(ref List<CLocation> cLocations, CLocation pNewLocation)
        {
            float fDist;
            int k;
            for (int i = 0; i < cLocations.Count; i++)
            {
                if (cLocations[i].Type == LocationType.Forbidden)
                    continue;

                fDist = (cLocations[i].X - pNewLocation.X) * (cLocations[i].X - pNewLocation.X) +
                    (cLocations[i].Y - pNewLocation.Y) * (cLocations[i].Y - pNewLocation.Y);
                if (cLocations[i] != pNewLocation && fDist < 2 * m_iMaxDist * m_iMaxDist)
                {
                    k = CreateLink(ref cLocations, cLocations[i], pNewLocation);
                    if (k != -1)
                        return k;
                }
            }
            return -1;
        }

        private int CreateLink(ref List<CLocation> cLocations, CLocation pLocation1, CLocation pLocation2)
        {
	        if(pLocation1.Links.ContainsKey(pLocation2))
		        return -1;

            if (pLocation1.Links.Count >= pLocation1.MaxLinks || pLocation2.Links.Count >= pLocation2.MaxLinks)
                return -1;

            double k1, b1;
            //						коеффициент наклона потенциальной связи
            if (pLocation1.X - pLocation2.X != 0)
                k1 = (double)(pLocation1.Y - pLocation2.Y) / (pLocation1.X - pLocation2.X);
            else
                k1 = (double)(pLocation1.Y - pLocation2.Y) / (pLocation1.X - pLocation2.X + 0.00000001);

            b1 = (double)pLocation2.Y - pLocation2.X * k1;
            
            
            //	Проверка перекрёстков
	        for(int i=0; i<cLocations.Count; i++)
	        {
                CLocation pLocation3 = cLocations[i];
                if (pLocation3 != pLocation1 &&
                    pLocation3 != pLocation2)
                {
                    foreach (var pLink4 in pLocation3.Links)
                    {
                        if (pLink4.Key != pLocation1 &&
                            pLink4.Key != pLocation2)
                        {
                            //	Нашли пару соединённых пещер, не входящих в рассматриваемую пару.
                            double k2, b2;
                            //						коеффициент наклона существующей связи
                            if (pLocation3.X - pLink4.Key.X != 0)
                                k2 = (double)(pLocation3.Y - pLink4.Key.Y) / (pLocation3.X - pLink4.Key.X);
                            else
                                k2 = (double)(pLocation3.Y - pLink4.Key.Y) / (pLocation3.X - pLink4.Key.X + 0.00000001);

                            b2 = (double)pLink4.Key.Y - pLink4.Key.X * k2;

                            if (k1 != k2)
                            {
                                double x1;
                                x1 = (b2 - b1) / (k1 - k2);

                                //						ЕСЛИ точка пересечения где-то здесь
                                if (((x1 > pLocation1.X && x1 < pLocation2.X) ||
                                     (x1 < pLocation1.X && x1 > pLocation2.X)) &&
                                    ((x1 > pLocation3.X && x1 < pLink4.Key.X) ||
                                     (x1 < pLocation3.X && x1 > pLink4.Key.X)))
                                {
                                    return -1;
                                }
                            }
                        }
                    }
                }
	        }

            CLink pNewLink = new CLink(pLocation1, pLocation2);

            pLocation1.Links[pLocation2] = pNewLink;
            pLocation2.Links[pLocation1] = pNewLink;
            m_cLinks.Add(pNewLink);

	        return pLocation1.Links.Count;
        }

        private bool PossibleLocation(ref List<CLocation> cLocations, int iX, int iY, bool bForbidden)
        {
            float fDist = iX * iX + iY * iY;
            if (fDist > m_iWorldScale * m_iWorldScale)
                return false;

            bool result = false;

            int iPossibleAnchors = 0;
            for (int i = 0; i < cLocations.Count; i++)
            {
                fDist = (cLocations[i].X - iX) * (cLocations[i].X - iX) + (cLocations[i].Y - iY) * (cLocations[i].Y - iY);
                if (fDist < m_iMinDist * m_iMinDist)
                    return false;

                if (cLocations[i].Type == LocationType.Forbidden)
                    continue;

                if (cLocations[i].Type != LocationType.Forbidden)
                    iPossibleAnchors++;

                if (fDist < m_iMaxDist * m_iMaxDist && cLocations[i].Links .Count < cLocations[i].MaxLinks)
                    result = true;
            }

            if (bForbidden || iPossibleAnchors < 10)
                return true;

            return result;
        }
        #endregion

        #region CreateFactions() and helper functions

        public List<CPerson> m_cFactions = new List<CPerson>();

        public List<List<CPerson>> m_cCoalitions = new List<List<CPerson>>();

        private void CreateFactions()
        {
            CPerson pBigBoss = GetPossibleBoss();

            m_cFactions.Add(pBigBoss);
            m_cFactions.Add(pBigBoss.GetNemezis());

            bool bGrowing;
            do
            {
                bGrowing = false;
                ResetMinions();

                foreach (CPerson pBoss in m_cFactions)
                    pBoss.m_pPatron = pBoss;

                List<CPerson> cPatrons = new List<CPerson>();
                cPatrons.AddRange(m_cFactions);

                do
                {
                    cPatrons = AssignMinions(cPatrons);
                }
                while (cPatrons.Count > 0);

                CPerson pMinorBoss = GetPossibleBoss();
                if (pMinorBoss != null)
                {
                    m_cFactions.Add(pMinorBoss);
                    CPerson pMinorNemesis = pMinorBoss.GetNemezis();
                    if (pMinorNemesis != null && !m_cFactions.Contains(pMinorNemesis))
                        m_cFactions.Add(pMinorNemesis);

                    bGrowing = true;
                }
            }
            while (bGrowing);

            List<CPerson> cIdiots = new List<CPerson>();
            foreach (CPerson pBoss in m_cFactions)
                if (pBoss.m_cMinions.Count == 0)
                    cIdiots.Add(pBoss);

            foreach(CPerson pIdiot in cIdiots)
            {
                pIdiot.m_pPatron = null;
                m_cFactions.Remove(pIdiot);
            }

            List<CPerson> cInCoalitions = new List<CPerson>();
            do
            {
                List<CPerson> c1 = new List<CPerson>();
                List<CPerson> c2 = new List<CPerson>();
                m_cCoalitions.Add(c1);
                m_cCoalitions.Add(c2);

                CPerson pLeader = null;
                foreach (CPerson pBoss in m_cFactions)
                {
                    if (!cInCoalitions.Contains(pBoss))
                    {
                        pLeader = pBoss;
                        break;
                    }
                }

                if (pLeader == null)
                    break;
                c1.Add(pLeader);
                cInCoalitions.Add(pLeader);

                foreach (CPerson pBoss in m_cFactions)
                {
                    if (cInCoalitions.Contains(pBoss))
                        continue;

                    bool bCouldBeInC1 = true;
                    foreach (CPerson pP1 in c1)
                    {
                        float fAttraction = CPerson.GetFactionAttraction(pP1, pBoss);
                        if (fAttraction < 0)
                            bCouldBeInC1 = false;
                    }
                    if (bCouldBeInC1)
                    {
                        c1.Add(pBoss);
                        cInCoalitions.Add(pBoss);
                    }
                    else
                    {
                        bool bCouldBeInC2 = true;
                        foreach (CPerson pP2 in c2)
                        {
                            float fAttraction = CPerson.GetFactionAttraction(pP2, pBoss);
                            if (fAttraction < 0)
                                bCouldBeInC2 = false;
                        }
                        if (bCouldBeInC2)
                        {
                            c2.Add(pBoss);
                            cInCoalitions.Add(pBoss);
                        }
                    }
                }
            }
            while (true);
        }

        private void ResetMinions()
        {
            foreach (CPerson pPerson in m_cPersons)
            {
                pPerson.m_pPatron = null;
                pPerson.m_cMinions.Clear();
            } 
        }

        private List<CPerson> AssignMinions(List<CPerson> cPatrons)
        {
            Dictionary<CPerson, Dictionary<CPerson, float>> cMinions = new Dictionary<CPerson, Dictionary<CPerson, float>>();
            
            foreach (CPerson pPatron in cPatrons)
            {
                int iPatronInfluence = pPatron.GetInfluence(true);
                CPerson pFaction = pPatron.GetFaction();
                foreach (var pRelation in pPatron.m_cRelations)
                {
                    if (pRelation.Value == CPerson.Relation.FormerLifeOf ||
                        pRelation.Value == CPerson.Relation.PresentLifeOf ||
                        pRelation.Value == CPerson.Relation.AlterEgo)
                        continue;

                    if (pRelation.Key.GetFaction() != null && pRelation.Key.GetFaction() != pFaction)
                        continue;

                    if (m_cFactions.Contains(pRelation.Key))
                        continue;

                    int iMinionInfluence = pRelation.Key.GetInfluence(true);

                    if (iMinionInfluence >= iPatronInfluence)
                        continue;

                    float fAttraction = CPerson.GetAttraction(pPatron, pRelation.Key);
                    float fFactionAttraction = CPerson.GetAttraction(pFaction, pRelation.Key);
                    if (fFactionAttraction < 0)
                        fAttraction /= -fFactionAttraction;
                    else
                        fAttraction *= fFactionAttraction;

                    if (fAttraction < 0)
                        continue;
                    //    fAttraction = -1f / fAttraction;
                    //else
                    //    fAttraction += 1;

                    Dictionary<CPerson, float> cPossiblePatrons;
                    if (!cMinions.TryGetValue(pRelation.Key, out cPossiblePatrons))
                    {
                        cPossiblePatrons = new Dictionary<CPerson, float>();
                        cMinions[pRelation.Key] = cPossiblePatrons;
                    }

                    cPossiblePatrons[pPatron] = fAttraction;
                }
            }

            foreach (var pMinion in cMinions)
            {
                CPerson pPatron = pMinion.Value.Keys.ElementAt(Rnd.ChooseOne(pMinion.Value.Values));

                if (pMinion.Key.m_pPatron != null)
                    pMinion.Key.m_pPatron.m_cMinions.Remove(pMinion.Key);

                pMinion.Key.m_pPatron = pPatron;
                pPatron.m_cMinions.Add(pMinion.Key);
            }

            return cMinions.Keys.ToList();
        }

        #endregion

        //public CSociety AddSociety()
        //{
        //    CSociety pNewSociety = new CSociety(this);
        //    m_cSocieties.Add(pNewSociety);
        //    return pNewSociety;
        //}
    }
}
