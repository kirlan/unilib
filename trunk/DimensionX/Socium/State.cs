using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using NameGen;
using LandscapeGeneration;
using LandscapeGeneration.PathFind;
using Socium.Languages;

namespace Socium
{
    public class StateInfo
    {
        public string m_sName;
        public int m_iRank;

        public string m_sHeirM;
        public string m_sHeirF;

        public SettlementInfo m_pStateCapital;
        public SettlementInfo m_pProvinceCapital;

        public int m_iMinGovernmentLevel;

        public int m_iMaxGovernmentLevel;

        public bool m_bDinasty;

        public List<Language> m_cLanguages = new List<Language>();

        /// <summary>
        /// Информация о государственном устройстве
        /// </summary>
        /// <param name="sName">Тип государственного устройства</param>
        /// <param name="iRank">Социальный ранг правителя</param>
        /// <param name="pStateCapital">Информация о поселении - столице государства</param>
        /// <param name="pProvinceCapital">Информация о поселениях - центрах провинций</param>
        /// <param name="sHeirM">Титул наследника</param>
        /// <param name="sHeirF">Титул наследницы</param>
        /// <param name="bDinasty">Должны ли наследники являться родственниками текущего правителя</param>
        /// <param name="iMinGovernmentLevel">Минимальный возможный уровень государственности</param>
        /// <param name="iMaxGovernmentLevel">Максимальный возможный уровень государственности</param>
        /// <param name="cLanguages">Языки, носители которых могут иметь государство такого типа</param>
        public StateInfo(string sName, int iRank, SettlementInfo pStateCapital, SettlementInfo pProvinceCapital, string sHeirM, string sHeirF, bool bDinasty, int iMinGovernmentLevel, int iMaxGovernmentLevel, Language[] cLanguages)
        {
            m_sName = sName;
            m_iRank = iRank;
            
            m_sHeirM = sHeirM;
            m_sHeirF = sHeirF;
            m_bDinasty = bDinasty;
            
            m_pStateCapital = pStateCapital;
            m_pProvinceCapital = pProvinceCapital;

            m_iMinGovernmentLevel = iMinGovernmentLevel;
            m_iMaxGovernmentLevel = iMaxGovernmentLevel;

            if(cLanguages != null)
                m_cLanguages.AddRange(cLanguages);
        }
    }

    public class State : BorderBuilder<Province>, ITerritory
    {
        #region States Info Array
        private static StateInfo[] s_aInfo = 
        {
            new StateInfo("Tribes", 14,
                new SettlementInfo(SettlementSize.Hamlet, "Hamlet", 5, 10, 2, 0, 1, new BuildingInfo("Elder's hut", "Patriarch", "Matriarch", 14)), 
                new SettlementInfo(SettlementSize.Hamlet, "Hamlet", 5, 10, 2, 0, 1, new BuildingInfo("Elder's hut", "Elder", "Elder", 3)), 
                "Warlord", "Princess", true, 0, 0, null),
            new StateInfo("Clans", 15,
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, 3, 0, 3, new BuildingInfo("Clans hall", "Patriarch", "Matriarch", 15)), 
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, 3, 0, 3, new BuildingInfo("Village hall", "Elder", "Elder", 3)), 
                "Warlord", "Princess", true, 1, 1, null),
            new StateInfo("Kingdom", 16,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, 5, 7, new BuildingInfo("Castle", "King", "Queen", 16)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, 2, 5, new BuildingInfo("Castle", "Baron", "Baroness", 9)), 
                "Prince", "Princess", true, 2, 6, new Language[] {Language.Dwarwen, Language.European, Language.Highlander, Language.Northman}),
            new StateInfo("Kingdom", 16,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, 5, 7, new BuildingInfo("Castle", "King", "Queen", 16)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, 2, 5, new BuildingInfo("Castle", "Count", "Countess", 9)), 
                "Prince", "Princess", true, 2, 6, new Language[] {Language.Drow, Language.Elven, Language.European, Language.Highlander}),
            new StateInfo("Kingdom", 16,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, 5, 7, new BuildingInfo("Castle", "King", "Queen", 16)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, 2, 5, new BuildingInfo("Castle", "Duke", "Duchess", 9)), 
                "Prince", "Princess", true, 2, 6, new Language[] {Language.Drow, Language.Elven, Language.European, Language.Dwarwen}),
            new StateInfo("Shogunate", 16,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, 5, 7, new BuildingInfo("Castle", "Shogun", "Midaidokoro", 16)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, 2, 5, new BuildingInfo("Castle", "Gensui", "Gensui", 9)), 
                "Prince", "Hime", true, 2, 6, new Language[] {Language.Asian}),
            new StateInfo("Kingdom", 16,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, 5, 7, new BuildingInfo("Castle", "Tenno", "Chugu", 16)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, 2, 5, new BuildingInfo("Castle", "Daimyo", "Lady", 9)), 
                "Shinno", "Naishinno", true, 2, 6, new Language[] {Language.Asian}),
            new StateInfo("Caliphate", 16,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, 5, 7, new BuildingInfo("Castle", "Caliph", "Calipha", 16)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, 2, 5, new BuildingInfo("Castle", "Emir", "Emira", 9)), 
                "Prince", "Princess", true, 2, 6, new Language[] {Language.Arabian}),
            new StateInfo("Sultanate", 16,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, 5, 7, new BuildingInfo("Castle", "Sultan", "Sultana", 16)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, 2, 5, new BuildingInfo("Castle", "Shah", "Shahbanu", 9)), 
                "Prince", "Princess", true, 2, 6, new Language[] {Language.Arabian, Language.African}),
            new StateInfo("Khanate", 16,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, 5, 7, new BuildingInfo("Castle", "Khan", "Khatun", 16)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, 2, 5, new BuildingInfo("Castle", "Bey", "Lady", 9)), 
                "Prince", "Princess", true, 2, 6, new Language[] {Language.Orkish, Language.Eskimoid}),
            new StateInfo("Tsardom", 16,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, 5, 7, new BuildingInfo("Castle", "Tsar", "Tsaritsa", 16)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, 2, 5, new BuildingInfo("Castle", "Boyar", "Boyarynia", 9)), 
                "Tsarevich", "Tsarevna", true, 2, 6, new Language[] {Language.Slavic, Language.Greek}),
            new StateInfo("Raj", 16,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, 5, 7, new BuildingInfo("Castle", "Maharaja", "Maharani", 16)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, 2, 5, new BuildingInfo("Castle", "Raja", "Rani", 9)), 
                "Maharajkumar", "Maharajkumari", true, 2, 6, new Language[] {Language.Hindu}),
            //new StateInfo("Empire", 16,
            //    new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, 5, 7, new BuildingInfo("Palace", "Emperor", "Empress", 16)), 
            //    new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, 5, 7, new BuildingInfo("Castle", "Governor", "Governor", 14)), 
            //    "Prince", "Princess", true, 2, 6, 2, 4),
            new StateInfo("Republic", 16,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, 5, 7, new BuildingInfo("Statehouse", "President", "President", 16)), 
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, 5, 7, new BuildingInfo("Town hall", "Governor", "Governor", 14)), 
                "Minister", "Minister", false, 3, 6, null),
            //new StateInfo("Republic", 16,
            //    new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 15, 5, 10, new BuildingInfo("Palace", "Dictator", "Dictator", 16)), 
            //    new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, 5, 7, new BuildingInfo("Palace", "Governor", "Governor", 14)), 
            //    "Counsellor", "Counsellor", false, 2, 6, 3, 4),
            //new StateInfo("Republic", 16,
            //    new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 15, 5, 10, new BuildingInfo("Palace", "General", "General", 16)), 
            //    new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, 5, 7, new BuildingInfo("Palace", "Colonel", "Colonel", 14)), 
            //    "Officer", "Officer", false, 2, 6, 3, 4),
            new StateInfo("Federation", 17,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 16, 5, 10, new BuildingInfo("Statehouse", "Chairman", "Chairman", 17)), 
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 15, 5, 10, new BuildingInfo("Town hall", "Mayor", "Mayor", 15)), 
                "Deputy", "Deputy", false, 4, 7, null),
            new StateInfo("League", 17,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 16, 5, 10, new BuildingInfo("Palace", "Speaker", "Speaker", 17)), 
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 15, 5, 10, new BuildingInfo("Palace", "Governor", "Governor", 15)), 
                "Counsellor", "Counsellor", false, 4, 6, null),
            new StateInfo("Union", 17,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 16, 5, 10, new BuildingInfo("Statehouse", "Chairman", "Chairman", 17)), 
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 15, 5, 10, new BuildingInfo("Town hall", "Mayor", "Mayor", 15)), 
                "Deputy", "Deputy", false, 5, 7, null),
            new StateInfo("Alliance", 17,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 16, 5, 10, new BuildingInfo("Palace", "Ruler", "Ruler", 17)), 
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 15, 5, 10, new BuildingInfo("Town hall", "Mayor", "Mayor", 15)), 
                "Minister", "Ministress", false, 5, 7, null),
            new StateInfo("Realm", 17,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 16, 5, 10, new BuildingInfo("Citadel", "God-King", "Goddess-Queen", 17)), 
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 15, 5, 10, new BuildingInfo("Palace", "Father", "Mother", 15)), 
                "Brother", "Sister", false, 5, 8, null),
            new StateInfo("Commonwealth", 17,
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 16, 2, 5, new BuildingInfo("Town hall", "Speaker", "Speaker", 17)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 15, 2, 5, new BuildingInfo("Town hall", "Manager", "Manager", 15)), 
                "Advisor", "Advisor", false, 7, 8, null),
            new StateInfo("Society", 17,
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, 16, 0, 3, new BuildingInfo("Village hall", "", "", 17)), 
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, 15, 0, 3, new BuildingInfo("Village hall", "", "", 15)), 
                "", "", false, 7, 8, null),
        };
        #endregion

        public class LifeLevel
        {
            public int m_iMaxGroundRoad;
            public int m_iMaxNavalPath;
            public int m_iAerialAvailability;
            public List<SettlementSize> m_cAvailableSettlements;

            public LifeLevel(int iMaxRoad, int iMaxNaval, int iAerial, SettlementSize[] cSettlements)
            {
                m_iMaxGroundRoad = iMaxRoad;
                m_iMaxNavalPath = iMaxNaval;
                m_iAerialAvailability = iAerial;
                m_cAvailableSettlements = new List<SettlementSize>(cSettlements);
            }
        }

        public static LifeLevel[] InfrastructureLevels = 
        {
            // 0 - только не соединённые дорогами поселения
            new LifeLevel(0, 0, 0, new SettlementSize[]{SettlementSize.Hamlet}),
            // 1 - можно строить деревни, просёлочные дороги и короткие морские маршруты
            new LifeLevel(1, 1, 0, new SettlementSize[]{SettlementSize.Hamlet, SettlementSize.Village}),
            // 2 - можно строить городки, обычные дороги и морские маршруты средней дальности
            new LifeLevel(2, 2, 0, new SettlementSize[]{SettlementSize.Hamlet, SettlementSize.Village, SettlementSize.Town, SettlementSize.Fort}),
            // 3 - большие города, имперские дороги, неограниченные морские маршруты
            new LifeLevel(3, 3, 0, new SettlementSize[]{SettlementSize.Hamlet, SettlementSize.Village, SettlementSize.Town, /*SettlementSize.City, */SettlementSize.Fort}),
            // 4 - большие города, имперские дороги, неограниченные морские маршруты, доступ к воздушному сообщению только в столице
            new LifeLevel(3, 3, 1, new SettlementSize[]{SettlementSize.Hamlet, SettlementSize.Village, SettlementSize.Town, SettlementSize.City, SettlementSize.Fort}),
            // 5 - большие города, имперские дороги, неограниченные морские маршруты, доступ к воздушному сообщению только в столице и центрах провинций
            new LifeLevel(3, 3, 2, new SettlementSize[]{SettlementSize.Village, SettlementSize.Town, SettlementSize.City, SettlementSize.Fort}),
            // 6 - большие города, имперские дороги, неограниченные морские маршруты, доступ к воздушному сообщению во всех поселениях
            new LifeLevel(3, 3, 3, new SettlementSize[]{SettlementSize.Town, SettlementSize.City, SettlementSize.Fort}),
            // 7 - небольшие города, обычные дороги, морской транспорт не используется, доступ к воздушному сообщению во всех поселениях
            new LifeLevel(2, 0, 3, new SettlementSize[]{SettlementSize.Hamlet, SettlementSize.Village, SettlementSize.Town, SettlementSize.Fort}),
            // 8 - деревни, просёлочные дороги, морской транспорт не используется, доступ к воздушному сообщению во всех поселениях
            new LifeLevel(1, 0, 3, new SettlementSize[]{SettlementSize.Hamlet, SettlementSize.Village, SettlementSize.Fort}),
        }; 
        
        public string m_sName;

        private static State m_pForbidden = new State();

        public bool Forbidden
        {
            get { return this == State.m_pForbidden; }
        }

        public List<Province> m_cContents = new List<Province>();

        private Dictionary<object, List<Line>> m_cBorderWith = new Dictionary<object, List<Line>>();

        private object m_pOwner = null;

        public object Owner
        {
            get { return m_pOwner; }
            set { m_pOwner = value; }
        }

        public Dictionary<object, List<Line>> BorderWith
        {
            get { return m_cBorderWith; }
        }

        public object[] m_aBorderWith = null;

        internal void FillBorderWithKeys()
        {
            m_aBorderWith = new List<object>(m_cBorderWith.Keys).ToArray();
        }

        public Province m_pMethropoly = null;

        public override void Start(Province pSeed)
        {
            m_cBorderWith.Clear();
            m_cContents.Clear();

            base.Start(pSeed);

            m_pMethropoly = pSeed;

            m_pRace = m_pMethropoly.m_pRace;

            m_cContents.Add(pSeed);
            pSeed.Owner = this;
        }

        /// <summary>
        /// Присоединяет к стране сопредельную нечейную провинцию.
        /// Чем длиннее общая граница с землёй - тем выше вероятность того, что выбрана будет именно она.
        /// </summary>
        /// <returns></returns>
        public bool Grow(int iMaxStateSize)
        {
            if (m_cContents.Count > iMaxStateSize)
                return false;

            Dictionary<Province, float> cBorderLength = new Dictionary<Province, float>();

            foreach (ITerritory pTerr in m_cBorder.Keys)
            {
                if (pTerr.Forbidden)
                    continue;

                Province pProvince = pTerr as Province;

                if (pProvince != null && pProvince.Owner == null && !pProvince.m_pCenter.IsWater)
                {
                    float fWholeLength = 0;
                    foreach (Line pLine in m_cBorder[pProvince])
                        fWholeLength += pLine.m_fLength;

                    //граница этой земли с окружающими землями
                    float fTotalLength = 0;
                    foreach (var pLinkTerr in pProvince.BorderWith)
                    {
                        if ((pLinkTerr.Key as ITerritory).Forbidden)
                            continue;

                        Line[] cLines = pLinkTerr.Value.ToArray();
                        foreach (Line pLine in cLines)
                            fTotalLength += pLine.m_fLength;
                    }

                    fWholeLength /= fTotalLength;

                    if (fWholeLength < 0.15f)
                        continue;

                    float fCultureDifference = m_pMethropoly.m_pCulture.GetDifference(pProvince.m_pCulture) * 5;

                    string sPosReasons = "";
                    string sNegReasons = "";
                    int iCustomsDifference = m_pMethropoly.m_pCustoms.GetDifference(pProvince.m_pCustoms, ref sPosReasons, ref sNegReasons);

                    int iRelation = (int)(fCultureDifference - iCustomsDifference);

                    if (m_pMethropoly.m_pRace != pProvince.m_pRace)
                        iRelation--;
                        //fWholeLength /= 2;

                    if (m_pMethropoly.m_pRace.m_pTemplate.m_pLanguage != pProvince.m_pRace.m_pTemplate.m_pLanguage)
                        //iRelation--;
                        continue;
                        //fWholeLength /= 2;
                    
                    //положительное отношение
                    if (iRelation > 1)
                        fWholeLength *= iRelation;

                    //отрицательное отношение
                    if (iRelation < 0)
                        continue;
                        //fWholeLength /= iCustomsDifference - fCultureDifference;

                    cBorderLength[pProvince] = fWholeLength;
                }
            }

            Province pAddon = null;

            int iChoice = Rnd.ChooseOne(cBorderLength.Values, 2);
            foreach (Province pProvince in cBorderLength.Keys)
            {
                iChoice--;
                if (iChoice < 0)
                {
                    pAddon = pProvince;
                    break;
                }
            }

            if (pAddon == null)
                return false;

            //if (!Rnd.OneChanceFrom(1 + m_iPower * m_iPower))
            //if (Rnd.OneChanceFrom(1 + pAddon.m_pCenter.Type.m_iMovementCost * pAddon.m_pCenter.Type.m_iMovementCost))
            //    return true;

            //foreach (LandTypeInfoX pType in m_pRace.m_cHatedLands)
            //    if (pType == pAddon.m_pCenter.Type && !Rnd.OneChanceFrom(5))
            //        return true;

            m_cContents.Add(pAddon);
            pAddon.Owner = this;

            m_cBorder[pAddon].Clear();
            m_cBorder.Remove(pAddon);

            foreach (var pLand in pAddon.BorderWith)
            {
                ITerritory pL = pLand.Key as ITerritory;

                if (!pL.Forbidden && m_cContents.Contains(pL))
                    continue;

                if (!m_cBorder.ContainsKey(pL))
                    m_cBorder[pL] = new List<Line>();
                Line[] cLines = pLand.Value.ToArray();
                foreach (Line pLine in cLines)
                    m_cBorder[pL].Add(new Line(pLine));
            }

            //ChainBorder();

            return true;
        }

        /// <summary>
        /// Заполняет словарь границ с другими странами и гарантирует принадлежность государства той расе, которая доминирует на его территории.
        /// </summary>
        public void Finish(float fCycleShift)
        {
            ChainBorder(fCycleShift);

            foreach (ITerritory pProvince in m_cBorder.Keys)
            {
                State pState;
                if (pProvince.Forbidden || (pProvince as Province).Owner == null)
                    pState = State.m_pForbidden;
                else
                    pState = (pProvince as Province).Owner as State;

                if (!m_cBorderWith.ContainsKey(pState))
                    m_cBorderWith[pState] = new List<Line>();
                m_cBorderWith[pState].AddRange(m_cBorder[pProvince]);
            }
            FillBorderWithKeys();

            Dictionary<Race, int> cRacesCount = new Dictionary<Race, int>();

            int iMaxPop = 0;
            Race pMaxRace = null;

            foreach (Province pProvince in m_cContents)
            {
                //bool bRestricted = true;
                //foreach (LocationX pLoc in pProvince.m_cContents)
                //    if (!pLoc.Forbidden && !pLoc.m_bBorder)
                //        bRestricted = false;

                //if (bRestricted)
                //    continue;

                int iCount = 0;
                if (!cRacesCount.TryGetValue(pProvince.m_pRace, out iCount))
                    cRacesCount[pProvince.m_pRace] = 0;
                cRacesCount[pProvince.m_pRace] = iCount + pProvince.m_cContents.Count;
                if (cRacesCount[pProvince.m_pRace] > iMaxPop)
                {
                    iMaxPop = cRacesCount[pProvince.m_pRace];
                    pMaxRace = pProvince.m_pRace;
                }
            }

            if (pMaxRace != null)
            {
                m_pRace = pMaxRace;

                m_pMethropoly.m_pRace = pMaxRace;
                foreach (LandX pLand in m_pMethropoly.m_cContents)
                    pLand.m_pRace = m_pRace;
            }
        }

        public void CalculateMagic()
        {
            m_iMagicLimit = 0;

            float fMagesCount = 0;
            float[] aDistribution = new float[10];

            bool bOneRace = true;

            foreach (Province pProvince in m_cContents)
            {
                if (pProvince.m_pRace != m_pRace)
                    bOneRace = false;

                if (pProvince.m_pRace.m_iMagicLimit > m_iMagicLimit)
                    m_iMagicLimit = pProvince.m_pRace.m_iMagicLimit;

                float fPrevalence = 1;
                switch (pProvince.m_pRace.m_eMagicAbilityPrevalence)
                {
                    case MagicAbilityPrevalence.rare:
                        fPrevalence = 0.1f;
                        break;
                    case MagicAbilityPrevalence.common:
                        fPrevalence = 0.5f;
                        break;
                    case MagicAbilityPrevalence.almost_everyone:
                        fPrevalence = 0.9f;
                        break;
                }

                float fProvinceMagesCount = 0;
                foreach (LandX pLand in pProvince.m_cContents)
                {
                    fProvinceMagesCount += pLand.m_cContents.Count * fPrevalence;
                }

                switch (pProvince.m_pRace.m_eMagicAbilityDistribution)
                {
                    case MagicAbilityDistribution.mostly_weak:
                        aDistribution[(1 + pProvince.m_pRace.m_iMagicLimit) / 2] += fProvinceMagesCount;
                        break;
                    case MagicAbilityDistribution.mostly_average:
                        aDistribution[(1 + pProvince.m_pRace.m_iMagicLimit) / 2] += fProvinceMagesCount / 2;
                        aDistribution[1 + pProvince.m_pRace.m_iMagicLimit] += fProvinceMagesCount / 2;
                        break;
                    case MagicAbilityDistribution.mostly_powerful:
                        aDistribution[1 + pProvince.m_pRace.m_iMagicLimit] += fProvinceMagesCount;
                        break;
                }
                fMagesCount += fProvinceMagesCount;
            }
            fMagesCount /= m_iPopulation;

            m_eMagicAbilityPrevalence = MagicAbilityPrevalence.almost_everyone;

            if (fMagesCount <= 0.75)
                m_eMagicAbilityPrevalence = MagicAbilityPrevalence.common;

            if (fMagesCount <= 0.25)
                m_eMagicAbilityPrevalence = MagicAbilityPrevalence.rare;

            float fWeakMagesCount = 0;
            float fPowerfulMagesCount = 0;
            for (int i = 0; i < 10; i++)
            {
                if (i <= (m_iMagicLimit+1) / 2)
                    fWeakMagesCount += aDistribution[i];
                else
                    fPowerfulMagesCount += aDistribution[i];
            }

            m_eMagicAbilityDistribution = MagicAbilityDistribution.mostly_average;

            if (fWeakMagesCount > fPowerfulMagesCount * 2)
                m_eMagicAbilityDistribution = MagicAbilityDistribution.mostly_weak;

            if (fPowerfulMagesCount > fWeakMagesCount * 2)
                m_eMagicAbilityDistribution = MagicAbilityDistribution.mostly_powerful;
        }

        public Race m_pRace = null;

        /// <summary>
        /// 1 - Бронзовый век. В ходу бронзовые кирасы и кожаные доспехи, бронзовое холодное оружие, из стрелкового оружия – луки и дротики.
        /// 2 - Железный век. Стальное холодное оружие, кольчуги, рыцарские доспехи. Из стрелкового оружия - луки и арбалеты.
        /// 3 - Эпоха пороха. Примитивное однозарядное огнестрельное оружие, лёгкие сабли и шпаги, облегчённая броня (кирасы, кожаные куртки).
        /// 4 - Индустриальная эра. Нарезное огнестрельное оружие, примитивная бронетехника и авиация, химическое оружие массового поражения.
        /// 5 - Атомная эра. Автоматическое огнестрельное оружие, бронежилеты, развитая бронетехника и авиация, ядерные ракеты и бомбы.
        /// 6 - Энергетическая эра. Силовые поля, лучевое оружие.
        /// 7 - Квантовая эра. Ограниченная телепортация, материализация, дезинтеграция.
        /// 8 - Переход. Полная и неограниченная власть человека над пространственно-временным континуумом.
        /// </summary>
        public int m_iTechLevel = 0;
        /// <summary>
        /// 1 - Мистика. Ритуальная магия, требующая длительной предварительной подготовки. Используя психотропные вещества, гипноз и йогические практики, мистики могут общаться с миром духов, получая из него информацию или заключая сделки с его обитателями.
        /// 2 - Псионика. Познание окружающего мира силой собственной мысли. Эмпатия, телепатия, ясновиденье, дальновиденье.
        /// 3 - Сверхспособности. Управление материальными объектами без физического контакта: телекинез, пирокинез, левитация и т.д.... Как правило, один отдельно взятый персонаж может развить у себя только одну-две сверхспособности.
        /// 4 - Традиционная фэнтезийная магия. То же самое, что и на предыдущем уровне, но гораздо доступнее. Один маг может владеть десятками заклинаний на разные случаи жизни.
        /// 5 - Джинны. Способность произвольно менять облик, массу и физические свойства собственного тела. Использование магии без заклинаний - просто усилием мысли.
        /// 6 - Титаны. Уровень личного могущества, обычно приписываемый языческим богам. Телепортация, материализация, управление течением времени.
        /// 7 - Трансцендентность. Отсутствие привязки к физическом телу. Разум способен существовать  в нематериальной форме, при этом сохраняя все свои возможности воспринимать окружающую среду и воздействать на неё.
        /// 8 - Единое. Границы между индивидуальностями стираются, фактически всё сообщество является единым разумным существом, неизмеримо более могущественным, чем составляющие его отдельные личности сами по себе.
        /// </summary>
        public int m_iMagicLimit = 0;
        /// <summary>
        /// Доступный жителям уровень жизни.
        /// Зависит от технического и магического развития, определяет доступные формы государственного правления
        /// </summary>
        public int m_iInfrastructureLevel = 0;
        /// <summary>
        /// 0 - На преступников и диссидентов власти никакого внимания не обращают, спасение утопающих - дело рук самих утопающих.
        /// 1 - Власти занимаются только самыми вопиющими преступлениями.
        /// 2 - Есть законы, их надо соблюдать, кто не соблюдает - тот преступник, а вор должен сидеть в тюрьме.
        /// 3 - Законы крайне строги, широко используется смертная казнь.
        /// 4 - Все граждане, кроме правящей верхушки, попадают под презумпцию виновности.
        /// </summary>
        public int m_iControl = 0;

        public Culture m_pCulture = null;
        public Customs m_pCustoms = null;

        /// <summary>
        /// Как часто встрачаются носители магических способностей
        /// </summary>
        public MagicAbilityPrevalence m_eMagicAbilityPrevalence = MagicAbilityPrevalence.rare;
        /// <summary>
        /// Процент реально крутых магов среди всех носителей магических способностей
        /// </summary>
        public MagicAbilityDistribution m_eMagicAbilityDistribution = MagicAbilityDistribution.mostly_weak;

        public int m_iFood = 0;
        public int m_iOre = 0;
        public int m_iWood = 0;
        public int m_iPopulation = 0;

        public StateInfo m_pInfo = null;

        public LocationX BuildCapital(int iMinSize, int iMaxSize, bool bFast, int iMinTech, int iMaxTech)
        {
            m_iTechLevel = 0;
            m_iInfrastructureLevel = 0;
            m_pCulture = new Culture(m_pMethropoly.m_pCulture);
            m_pCustoms = new Customs(m_pMethropoly.m_pCustoms);

            int iAverageMagicLimit = 0;

            m_iFood = 0;
            m_iWood = 0;
            m_iOre = 0;
            m_iPopulation = 0;

            foreach (Province pProvince in m_cContents)
            {
                m_iFood += pProvince.m_iFood;
                m_iWood += pProvince.m_iWood;
                m_iOre += pProvince.m_iOre;

                m_iPopulation += pProvince.m_iPopulation;

                if (pProvince.m_iTechLevel > m_iTechLevel)
                    m_iTechLevel = pProvince.m_iTechLevel;

                if (pProvince.m_iInfrastructureLevel > m_iInfrastructureLevel)
                    m_iInfrastructureLevel = pProvince.m_iInfrastructureLevel;

                foreach (LandX pLand in pProvince.m_cContents)
                {
                    iAverageMagicLimit += pProvince.m_pRace.m_iMagicLimit * pLand.m_cContents.Count;
                }
            }

            iAverageMagicLimit = iAverageMagicLimit / m_iPopulation;

            int iOldLevel = m_pMethropoly.m_iTechLevel;// Math.Max(m_pRace.m_iTechLevel, m_pRace.m_iMagicLimit / 2);
            int iNewLevel = m_iTechLevel;// Math.Max(m_iTechLevel, iAverageMagicLimit / 2);
            if (iNewLevel > iOldLevel)
                for (int i = 0; i < iNewLevel - iOldLevel; i++)
                {
                    m_pCulture.Evolve();
                    m_pCustoms.Evolve();
                }
            else
                for (int i = 0; i < iOldLevel - iNewLevel; i++)
                {
                    m_pCulture.Degrade();
                    m_pCustoms.Degrade();
                }

            //m_iInfrastructureLevel = 4 + (int)(m_pCulture.GetDifference(Culture.IdealSociety) * 4);
            //m_iGovernmentLevel = 1 + Math.Max(m_iTechLevel, m_iMagicLimit) / 2 + Rnd.Get(Math.Max(m_iTechLevel, m_iMagicLimit) / 2);
            //if (m_iTechLevel == 0)
            //    m_iInfrastructureLevel = 0;

            //if (m_iFood < m_iPopulation || Rnd.OneChanceFrom(10))
            //    m_iInfrastructureLevel--;// = Rnd.Get(m_iCultureLevel);
            //if (m_iFood > m_iPopulation * 2 && Rnd.OneChanceFrom(10))
            //    m_iInfrastructureLevel++;

            //if (m_cContents.Count > iMaxSize)
            //    m_iInfrastructureLevel++;

            //if (m_cContents.Count < iMinSize)
            //    m_iInfrastructureLevel = Rnd.Get(Math.Min(3, m_iInfrastructureLevel + 1));

            //if (m_iInfrastructureLevel < (m_iTechLevel + 1) / 2)//Math.Max(m_iTechLevel + 1, iAverageMagicLimit) / 2)
            //    m_iInfrastructureLevel = (m_iTechLevel + 1) / 2;//Math.Max(m_iTechLevel + 1, iAverageMagicLimit) / 2;
            //if (m_iInfrastructureLevel > m_iTechLevel+1)//Math.Max(m_iTechLevel + 1, iAverageMagicLimit - 1))
            //    m_iInfrastructureLevel = m_iTechLevel+1;// Math.Max(m_iTechLevel + 1, iAverageMagicLimit - 1);
            //if (m_iInfrastructureLevel > 8)
            //    m_iInfrastructureLevel = 8;

            if (m_iTechLevel > m_iInfrastructureLevel * 2)
                m_iTechLevel = m_iInfrastructureLevel * 2;
            if (m_iTechLevel > iMaxTech)
                m_iTechLevel = iMaxTech;

            List<StateInfo> cInfos = new List<StateInfo>();

            foreach (StateInfo pInfo in s_aInfo)
            {
                if (m_iInfrastructureLevel >= pInfo.m_iMinGovernmentLevel &&
                    m_iInfrastructureLevel <= pInfo.m_iMaxGovernmentLevel &&
                    (pInfo.m_cLanguages.Count == 0 ||
                     pInfo.m_cLanguages.Contains(m_pRace.m_pTemplate.m_pLanguage)))
                    cInfos.Add(pInfo);
            }

            if (cInfos.Count == 0)
            {
                foreach (StateInfo pInfo in s_aInfo)
                {
                    if (m_iInfrastructureLevel >= pInfo.m_iMinGovernmentLevel &&
                        m_iInfrastructureLevel <= pInfo.m_iMaxGovernmentLevel)
                        cInfos.Add(pInfo);
                }
            }

            m_pInfo = cInfos[Rnd.Get(cInfos.Count)];
            
            m_iControl = 2;

            //if (m_pCulture.Moral[Culture.Morale.Agression] > 1)
            //    m_iControl++;

            if (m_pCulture.MentalityValues[Culture.Mentality.Exploitation] > 1.2)
                m_iControl++;
            if (m_pCulture.MentalityValues[Culture.Mentality.Exploitation] > 1.8)
                m_iControl++;
            if (m_pCulture.MentalityValues[Culture.Mentality.Exploitation] < 0.4)
                m_iControl--;

            if (m_pCulture.MentalityValues[Culture.Mentality.Fanaticism] > 1.2)
                m_iControl++;
            if (m_pCulture.MentalityValues[Culture.Mentality.Fanaticism] > 1.8)
                m_iControl++;
            if (m_pCulture.MentalityValues[Culture.Mentality.Fanaticism] < 0.4)
                m_iControl--;

            if (m_pCulture.MentalityValues[Culture.Mentality.Selfishness] > 1.6)
                m_iControl--;

            if (m_iInfrastructureLevel == 0)
                m_iControl = 0;
            if (m_iControl == 0 && m_iInfrastructureLevel >= 1 && m_iInfrastructureLevel <= 6)
                m_iControl = 1;

            if (m_iControl < 0)
                m_iControl = 0;
            if (m_iControl > 4)
                m_iControl = 4;

            m_pMethropoly.m_pAdministrativeCenter.m_pSettlement = new Settlement(m_pInfo.m_pStateCapital, m_pMethropoly.m_pRace, m_iTechLevel, m_iMagicLimit, true, bFast);

            foreach (Province pProvince in m_cContents)
            {
                if (pProvince == m_pMethropoly)
                    continue;

                pProvince.m_pAdministrativeCenter.m_pSettlement = new Settlement(m_pInfo.m_pProvinceCapital, m_pMethropoly.m_pRace, m_iTechLevel, m_iMagicLimit, false, bFast);
            }

            if (m_pMethropoly.m_pCenter.Area != null)
                m_sName = m_pRace.m_pTemplate.m_pLanguage.RandomCountryName();

            return m_pMethropoly.m_pAdministrativeCenter;
        }

        /// <summary>
        /// Негативное отношение к другому государству. От 2 (ненависть) до -2 (дружба)
        /// </summary>
        /// <param name="pOpponent"></param>
        /// <returns></returns>
        public int CalcHostility(State pOpponent)
        {
            string s;
            return CalcHostility(pOpponent, out s);
        }

        /// <summary>
        /// Негативное отношение к другому государству. От 2 (ненависть) до -2 (дружба)
        /// </summary>
        /// <param name="pOpponent"></param>
        /// <returns></returns>
        public int CalcHostility(State pOpponent, out string sReasons)
        {
            int iHostility = 0;

            string sPositiveReasons = "";
            string sNegativeReasons = "";

            //sReasons = "";

            //float fContact = 0;
            //float fBorder = 0;
            //foreach (ITerritory pTerr in BorderWith.Keys)
            //{ 
            //    if(pTerr.Forbidden)
            //        continue;

            //    State pState = pTerr as State;
            //    foreach (Line pLine in BorderWith[pState])
            //    {
            //        fBorder += pLine.m_fLength;
            //        if (pState == pOpponent)
            //            fContact += pLine.m_fLength;
            //    }
            //}

            if (m_pRace != pOpponent.m_pRace)
            {
                iHostility++;
                sNegativeReasons += " (-1) " + pOpponent.m_pRace.ToString() + "\n";

                if (m_pRace.m_pTemplate.m_pLanguage != pOpponent.m_pRace.m_pTemplate.m_pLanguage)
                {
                    iHostility++;
                    sNegativeReasons += " (-1) Different language\n";
                }
            }
            else
            {
                iHostility--;
                sPositiveReasons += " (+1) " + pOpponent.m_pRace.ToString() + "\n";
            }

            iHostility += m_pCustoms.GetDifference(pOpponent.m_pCustoms, ref sPositiveReasons, ref sNegativeReasons);

            if (m_iFood < m_iPopulation && pOpponent.m_iFood > pOpponent.m_iPopulation * 2)
            {
                iHostility++;
                sNegativeReasons += " (-1) Envy for food\n";
            }
            if (m_iWood < m_iPopulation && pOpponent.m_iWood > pOpponent.m_iPopulation * 2)
            {
                iHostility++;
                sNegativeReasons += " (-1) Envy for wood\n";
            }
            if (m_iOre < m_iPopulation && pOpponent.m_iOre > pOpponent.m_iPopulation * 2)
            {
                iHostility++;
                sNegativeReasons += " (-1) Envy for ore\n";
            }

            int iControlDifference = Math.Abs(pOpponent.m_iControl - m_iControl);
            if (iControlDifference != 1)
            {
                iHostility += iControlDifference - 1;
                if (iControlDifference > 1)
                    sNegativeReasons += string.Format(" (-{1}) {0}\n", State.GetControlString(pOpponent.m_iControl), iControlDifference - 1);
                else
                    sPositiveReasons += string.Format(" (+{1}) {0}\n", State.GetControlString(pOpponent.m_iControl), 1);
            }

            if (pOpponent.m_iInfrastructureLevel > m_iInfrastructureLevel)
            {
                iHostility++;//= pOpponent.m_iLifeLevel - m_iLifeLevel;
                sNegativeReasons += string.Format(" (-{0}) Envy for civilization\n", 1);//pOpponent.m_iLifeLevel - m_iLifeLevel);
            }
            else
            {
                if (pOpponent.m_iInfrastructureLevel < m_iInfrastructureLevel)
                {
                    iHostility++;//= m_iLifeLevel - pOpponent.m_iLifeLevel;
                    sNegativeReasons += string.Format(" (-{0}) Scorn for savagery\n", 1);//m_iLifeLevel - pOpponent.m_iLifeLevel);
                }
            }

            float iCultureDifference = m_pCulture.GetDifference(pOpponent.m_pCulture);
            if (iCultureDifference > 0.75)
            {
                iHostility -= 2;
                sPositiveReasons += " (+2) Very close culture\n";
            }
            else
                if (iCultureDifference > 0.5)
                {
                    iHostility--;
                    sPositiveReasons += " (+1) Close culture\n";
                }
                else
                    if (iCultureDifference < -0.5)
                    {
                        iHostility += 2;
                        sNegativeReasons += " (-2) Very different culture\n";
                    }
                    else
                        if (iCultureDifference < 0)
                        {
                            iHostility++;
                            sNegativeReasons += " (-1) Different culture\n";
                        }

            sReasons = "Good:\n" + sPositiveReasons + "Bad:\n" + sNegativeReasons + "----\n";

            if (iHostility > 0)
            {
                iHostility = (int)(m_pCulture.MentalityValues[Culture.Mentality.Fanaticism] * iHostility + 0.25);
                sReasons += string.Format("Fanaticism \t(x{0}%)\n", (int)(m_pCulture.MentalityValues[Culture.Mentality.Fanaticism] * 100));

                iHostility = (int)(m_pCulture.MentalityValues[Culture.Mentality.Agression] * iHostility + 0.25);
                sReasons += string.Format("Agression \t(x{0}%)\n", (int)(m_pCulture.MentalityValues[Culture.Mentality.Agression] * 100));

                if (iHostility == 0)
                    iHostility = 1;
            }
            else
            {
                if (iHostility < 0)
                {
                    iHostility = (int)((2.0f - m_pCulture.MentalityValues[Culture.Mentality.Fanaticism]) * iHostility - 0.25);
                    sReasons += string.Format("Tolerance \t(x{0}%)\n", (int)((2.0f - m_pCulture.MentalityValues[Culture.Mentality.Fanaticism]) * 100));

                    iHostility = (int)((2.0f - m_pCulture.MentalityValues[Culture.Mentality.Agression]) * iHostility - 0.25);
                    sReasons += string.Format("Amiability \t(x{0}%)\n", (int)((2.0f - m_pCulture.MentalityValues[Culture.Mentality.Agression]) * 100));

                    if (iHostility == 0)
                        iHostility = -1;
                }
            }

            //if (fContact < fBorder / 2)
            //    iHostility = iHostility / 2;

            sReasons += string.Format("----\nTotal \t({0:+#;-#;0})\n", -iHostility);
            return iHostility;
        }

        public State[] GetEnemiesList()
        {
            List<State> cList = new List<State>();
 
            foreach (ITerritory pTerr in m_aBorderWith)
            {
                if (pTerr.Forbidden)
                    continue;

                State pState = pTerr as State;
                int iHostility = CalcHostility(pState);
                int iHostility2 = pState.CalcHostility(this);

                if (iHostility2 > iHostility)
                    iHostility = iHostility2;

                if (iHostility > 0)
                {
                    cList.Add(pState);
                }
            }

            return cList.ToArray();
        }

        public State[] GetAlliesList()
        {
            List<State> cList = new List<State>();

            foreach (ITerritory pTerr in m_aBorderWith)
            {
                if (pTerr.Forbidden)
                    continue;

                State pState = pTerr as State;
                int iHostility = CalcHostility(pState);
                int iHostility2 = pState.CalcHostility(this);

                if (iHostility2 > iHostility)
                    iHostility = iHostility2;

                if (iHostility <= 0)
                {
                    cList.Add(pState);
                }
            }

            return cList.ToArray();
        }

        public void BuildForts(Dictionary<State, Dictionary<State, int>> cHostility, bool bFast)
        {
            if (!InfrastructureLevels[m_iInfrastructureLevel].m_cAvailableSettlements.Contains(SettlementSize.Fort))
                return;

            foreach (Province pProvince in m_cContents)
                foreach (LandX pLand in pProvince.m_cContents)
                {
                    float fTreat = 0;
                    float fBorder = 0;
                    foreach (var pTerr in pLand.BorderWith)
                    {
                        if((pTerr.Key as ITerritory).Forbidden)
                            continue;

                        LandX pLink = pTerr.Key as LandX;
                        Line[] cLines = pTerr.Value.ToArray();
                        foreach (Line pLine in cLines)
                        {
                            fBorder += pLine.m_fLength / pLink.MovementCost;
                            if (pLink.m_pProvince == null)
                                fTreat += pLine.m_fLength / pLink.MovementCost;
                            else
                            {
                                if (!cHostility.ContainsKey(pLink.m_pProvince.Owner as State))
                                    cHostility[pLink.m_pProvince.Owner as State] = new Dictionary<State, int>();

                                if(!cHostility[pLink.m_pProvince.Owner as State].ContainsKey(this))
                                    cHostility[pLink.m_pProvince.Owner as State][this] = (pLink.m_pProvince.Owner as State).CalcHostility(this);

                                int iHostility = cHostility[pLink.m_pProvince.Owner as State][this];

                                if (pLink.m_pProvince.Owner != this && iHostility > 0)
                                    fTreat += pLine.m_fLength * (float)Math.Sqrt(iHostility) / pLink.MovementCost;
                            }
                        }
                    }

                    if (fTreat == 0)
                        continue;

                    if (Rnd.ChooseOne(fTreat, fBorder - fTreat))
                        //if (m_iSize > 1 || Rnd.OneChanceFrom(2))
                        {
                            LocationX pFort = pLand.BuildFort(bFast);
                            if (pFort != null)
                            {
                                pLand.m_pProvince.m_cSettlements.Add(pFort);
                                //bHaveOne = true;
                            }
                        }
                }
            }

        public static string GetTechString(int iLevel)
        {
            string sTech = "prehistoric";
            switch (iLevel)
            {
                case 1:
                    sTech = "bronze weapons";
                    break;
                case 2:
                    sTech = "steel weapons";
                    break;
                case 3:
                    sTech = "muskets";
                    break;
                case 4:
                    sTech = "rifles";//railroads
                    break;
                case 5:
                    sTech = "machineguns";//aviation
                    break;
                case 6:
                    sTech = "beam guns";
                    break;
                case 7:
                    sTech = "desintegrators";//limited teleportation
                    break;
                case 8:
                    sTech = "reality destructors";//unlimited teleportation
                    break;
                //case 8:
                //    sTech = "mind net";
                //    break;
            }

            return sTech;
        }

        public static string GetMagicString(int iLevel)
        {
            string sMagic = "none";
            switch (iLevel)
            {
                case 1:
                    sMagic = "mystics";
                    break;
                case 2:
                    sMagic = "seers";
                    break;
                case 3:
                    sMagic = "jedi";//animal empower?
                    break;
                case 4:
                    sMagic = "wizards";//portals
                    break;
                case 5:
                    sMagic = "jinnes";
                    break;
                case 6:
                    sMagic = "titans";//limited teleportation
                    break;
                case 7:
                    sMagic = "gods";//unlimited teleportation
                    break;
                case 8:
                    sMagic = "the One";
                    break;
            }

            return sMagic;
        }

        public static string GetControlString(int iControl)
        {
            string sControl = "Anarchic";
            switch (iControl)
            {
                case 1:
                    sControl = "Liberal";
                    break;
                case 2:
                    sControl = "Lawful";
                    break;
                case 3:
                    sControl = "Autocratic";
                    break;
                case 4:
                    sControl = "Despotic";
                    break;
            }
            return sControl;
        }

        public int GetImportedTech()
        {
            int iMaxTech = m_iTechLevel;
            foreach (State pState in m_aBorderWith)
            {
                if (pState.m_iTechLevel > iMaxTech)
                    iMaxTech = pState.m_iTechLevel;
            }

            if (iMaxTech <= this.m_iTechLevel)
                iMaxTech = -1;

            return iMaxTech;
        }

        public override string ToString()
        {
            return string.Format("{2} (C{1}T{3}M{5}) - {0} {4}", m_sName, m_iInfrastructureLevel, m_pRace, m_iTechLevel, m_pInfo.m_sName, m_iMagicLimit);
        }

        public override float GetMovementCost()
        {
            throw new NotImplementedException();
        }
    }
}
