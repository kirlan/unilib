using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using NameGen;
using LandscapeGeneration;
using LandscapeGeneration.PathFind;
using VQMapTest2.Languages;

namespace VQMapTest2
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
        private static StateInfo[] m_aInfo = 
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
                "Minister", "Ministress", false, 3, 6, null),
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
                "Deputy", "Deputy", false, 3, 7, null),
            new StateInfo("Union", 17,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 16, 5, 10, new BuildingInfo("Statehouse", "Chairman", "Chairman", 17)), 
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 15, 5, 10, new BuildingInfo("Town hall", "Mayor", "Mayor", 15)), 
                "Deputy", "Deputy", false, 3, 7, null),
            new StateInfo("League", 17,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 16, 5, 10, new BuildingInfo("Palace", "Speaker", "Speaker", 17)), 
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 15, 5, 10, new BuildingInfo("Palace", "Governor", "Governor", 15)), 
                "Counsellor", "Counsellor", false, 3, 6, null),
            new StateInfo("Alliance", 17,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 16, 5, 10, new BuildingInfo("Palace", "Ruler", "Ruler", 17)), 
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 15, 5, 10, new BuildingInfo("Town hall", "Mayor", "Mayor", 15)), 
                "Minister", "Ministress", false, 3, 7, null),
            new StateInfo("Realm", 17,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 16, 5, 10, new BuildingInfo("Citadel", "God-King", "Goddess-Queen", 17)), 
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 15, 5, 10, new BuildingInfo("Palace", "Father", "Mother", 15)), 
                "Brother", "Sister", false, 4, 8, null),
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
            new LifeLevel(3, 3, 0, new SettlementSize[]{SettlementSize.Hamlet, SettlementSize.Village, SettlementSize.Town, SettlementSize.City, SettlementSize.Fort}),
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
        
        public LocationX m_pCapital = null;

        public List<LocationX> m_cSettlements = new List<LocationX>();

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
            m_cSettlements.Clear();
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
        public bool Grow()
        {
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

                    if (fWholeLength < 0.25f && m_cContents.Count > 1)
                        continue; 
                    
                    foreach (LandTypeInfoX pType in m_pRace.m_cPrefferedLands)
                        if (pType == pProvince.m_pCenter.Type)
                            fWholeLength *= 10;

                    foreach (LandTypeInfoX pType in m_pRace.m_cHatedLands)
                        if (pType == pProvince.m_pCenter.Type)
                            fWholeLength /= 10;

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
            if (Rnd.OneChanceFrom(1 + pAddon.m_pCenter.Type.m_iMovementCost * pAddon.m_pCenter.Type.m_iMovementCost))
                return true;

            foreach (LandTypeInfoX pType in m_pRace.m_cHatedLands)
                if (pType == pAddon.m_pCenter.Type && !Rnd.OneChanceFrom(5))
                    return true;

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
        /// Заполняет словарь границ с другими странами.
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
        }

        public Race m_pRace = null;

        public GenderPriority m_eGenderPriority = GenderPriority.GendersEquality;

        /// <summary>
        /// 1 - Допороховая эра. В ходу металлические и кожаные доспехи, холодное оружие, из стрелкового оружия – луки, дротики и арбалеты.
        /// 2 - Эпоха пороха. Примитивное однозарядное огнестрельное оружие, лёгкие сабли и шпаги, облегчённая броня (кирасы, кожаные куртки).
        /// 3 - Индустриальная эра. Автоматическое огнестрельное оружие, бронежилеты, развитая бронетехника, оружие массового поражения.
        /// 4 - Кибернетическая эра. Боевые имплантаты могут превратить любого человека в почти неуязвимую машину для убийства. Активно используются вспомогательные боевые механизмы (дроны, дроиды), управляемые искусственным интеллектом.
        /// 5 - Энергетическая эра. Силовые поля, лучевое оружие.
        /// 6 - Квантовая эра. Доступна телепортация, материализация, управление скоростью времени.
        /// 7 - Эра чистого разума. Полный контакт между человеческим сознанием и машиной. Возможна передача и копирование человеческой личности. Больше нет разницы между человеческим разумом и искусственным интеллектом.
        /// 8 - Переход. Люди давно отказались от физических тел и используют их только для развлечения – как экзотические костюмы. Основное время человеческое сознание проводит внутри мегасети, образованной миллиардами разнообразных машин, которые являются их глазами и руками.
        /// </summary>
        public int m_iTechLevel = 0;
        /// <summary>
        /// 1 - Мистика. Использование психотропных веществ, психология, гипноз, йогические практики, акупунктура.
        /// 2 - Псионика. Познание окружающего мира силой мысли. Эмпатия, телепатия, ясновиденье, дальновиденье.
        /// 3 - Сверхспособности. Управление материальными объектами без физического контакта: телекинез, пирокинез, левитация и т.д.... Как правило, один отдельно взятый персонаж может развить у себя только одну-две сверхспособности.
        /// 4 - Магия. То же самое, что и на предыдущем уровне, но гораздо доступнее. Один маг может владеть десятками заклинаний на разные случаи жизни.
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
        /// 4 - Все граждане, кроме правящей верхушки, попадают по презумпцию виновности.
        /// </summary>
        public int m_iControl = 0;

        public Culture m_pCulture = null;

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

        public LocationX BuildCapital(int iMinSize, int iMaxSize, bool bFast)
        {
            m_eGenderPriority = m_pRace.m_eGenderPriority;
            m_iTechLevel = m_pRace.m_iTechLevel;
            m_iMagicLimit = m_pRace.m_iMagicLimit;

            if (Rnd.OneChanceFrom(3))
                m_eGenderPriority = (GenderPriority)Rnd.Get(typeof(GenderPriority));

            m_eMagicAbilityPrevalence = m_pRace.m_eMagicAbilityPrevalence;
            m_eMagicAbilityDistribution = m_pRace.m_eMagicAbilityDistribution;
            m_pCulture = new Culture(m_pRace.m_pCulture);

            if (Rnd.OneChanceFrom(3))
                m_eMagicAbilityPrevalence = (MagicAbilityPrevalence)Rnd.Get(typeof(MagicAbilityPrevalence));
            if (Rnd.OneChanceFrom(3))
                m_eMagicAbilityDistribution = (MagicAbilityDistribution)Rnd.Get(typeof(MagicAbilityDistribution));

            foreach (Province pProvince in m_cContents)
                foreach (LandX pLand in pProvince.m_cContents)
                {
                    int iCoast = 0;
                    int iBorder = 0;
                    foreach (LocationX pLoc in pLand.m_cContents)
                    {
                        foreach (LocationX pLink in pLoc.m_aBorderWith)
                        {
                            if (pLink.Owner != pLoc.Owner)
                                iBorder++;
                            if (pLink.Owner != null && (pLink.Owner as LandX).IsWater)
                                iCoast++;
                        }
                    }

                    m_iFood += (int)(pLand.m_cContents.Count * pLand.Type.m_fFood) + iCoast * 3;
                    m_iWood += (int)(pLand.m_cContents.Count * pLand.Type.m_fWood);
                    m_iOre += (int)(pLand.m_cContents.Count * pLand.Type.m_fOre);

                    m_iPopulation += pLand.m_cContents.Count;
                }

            if (m_iWood < m_iPopulation && m_iOre < m_iPopulation)
                m_iTechLevel -= 2;
            else if (m_iWood + m_iOre < 2 * m_iPopulation)
                m_iTechLevel--;
            else if (m_iWood > m_iPopulation && m_iOre > m_iPopulation && Rnd.OneChanceFrom(4))
                m_iTechLevel++;

            if (m_iTechLevel < 0)
                m_iTechLevel = 0;
            if (m_iTechLevel > 8)
                m_iTechLevel = 8;

            int iBioLevel = (int)(Math.Pow(Rnd.Get(14), 3) / 1000);
            if (Rnd.OneChanceFrom(2))
                m_iMagicLimit += iBioLevel;
            else
                m_iMagicLimit -= iBioLevel;

            if (m_iMagicLimit < 0)
                m_iMagicLimit = 0;
            if (m_iMagicLimit > 8)
                m_iMagicLimit = 8;

            int iOldLevel = Math.Max(m_pRace.m_iTechLevel, m_pRace.m_iMagicLimit);
            int iNewLevel = Math.Max(m_iTechLevel, m_iMagicLimit);
            if (iNewLevel > iOldLevel)
                for (int i = 0; i < iNewLevel - iOldLevel; i++)
                    m_pCulture.Evolve();
            else
                for (int i = 0; i < iOldLevel - iNewLevel; i++)
                    m_pCulture.Degrade();

            m_iInfrastructureLevel = 4 + (int)(m_pCulture.GetDifference(Culture.IdealSociety) * 4);
            //m_iGovernmentLevel = 1 + Math.Max(m_iTechLevel, m_iMagicLimit) / 2 + Rnd.Get(Math.Max(m_iTechLevel, m_iMagicLimit) / 2);
            if (m_iTechLevel == 0 && m_iMagicLimit == 0)
                m_iInfrastructureLevel = 0;

            if (m_iFood < m_iPopulation || Rnd.OneChanceFrom(10))
                m_iInfrastructureLevel--;// = Rnd.Get(m_iCultureLevel);
            if (m_iFood > m_iPopulation * 2 && Rnd.OneChanceFrom(10))
                m_iInfrastructureLevel++;

            if (m_cContents.Count > iMaxSize)
                m_iInfrastructureLevel++;

            if (m_cContents.Count < iMinSize)
                m_iInfrastructureLevel = Rnd.Get(Math.Min(3, m_iInfrastructureLevel + 1));

            if (m_iInfrastructureLevel < Math.Max(m_iTechLevel + 1, m_iMagicLimit) / 2)
                m_iInfrastructureLevel = Math.Max(m_iTechLevel + 1, m_iMagicLimit) / 2;
            if (m_iInfrastructureLevel > Math.Max(m_iTechLevel + 1, m_iMagicLimit - 1))
                m_iInfrastructureLevel = Math.Max(m_iTechLevel + 1, m_iMagicLimit - 1);
            if (m_iInfrastructureLevel > 8)
                m_iInfrastructureLevel = 8;

            if (m_iTechLevel > m_iInfrastructureLevel * 2)
                m_iTechLevel = m_iInfrastructureLevel * 2;

            //if (m_iGovernmentLevel >= 7)
            //    m_eMagicUsingAttitude = MagicUsingAttitude.allowed;

            //int iAlternateGovernmentLevel = 4 + (int)(m_pCulture.GetDifference(Culture.IdealSociety) * 4);

            List<StateInfo> cInfos = new List<StateInfo>();

            foreach (StateInfo pInfo in m_aInfo)
            {
                if (m_iInfrastructureLevel >= pInfo.m_iMinGovernmentLevel &&
                    m_iInfrastructureLevel <= pInfo.m_iMaxGovernmentLevel &&
                    (pInfo.m_cLanguages.Count == 0 ||
                     pInfo.m_cLanguages.Contains(m_pRace.m_pLanguage)))
                    cInfos.Add(pInfo);
            }

            if (cInfos.Count == 0)
            {
                foreach (StateInfo pInfo in m_aInfo)
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

            //if (m_iGovernmentLevel == 1)
            //    m_iControl = 3 + Rnd.Get(2);
            //if (m_iGovernmentLevel == 2)
            //    m_iControl = 1 + Rnd.Get(4);
            //if (m_iGovernmentLevel == 3 || m_iGovernmentLevel == 4)
            //    m_iControl = 1 + Rnd.Get(3);
            //if (m_iGovernmentLevel == 5 || m_iGovernmentLevel == 6)
            //    m_iControl = 2 + Rnd.Get(2);
            //if (m_iGovernmentLevel == 7)
            //    m_iControl = Rnd.Get(2);
            //if (m_iGovernmentLevel == 8)
            //    m_iControl = 0;

            if (m_iControl < 0)
                m_iControl = 0;
            if (m_iControl > 4)
                m_iControl = 4;

            //m_pCapital = m_cContents[iMethropoly].BuildSettlement(eSize, true);
            m_pCapital = m_pMethropoly.BuildAdministrativeCenter(m_pInfo.m_pStateCapital, bFast);
            if (m_pCapital != null)
                m_cSettlements.Add(m_pCapital);
            else
                throw new Exception("Can't build capital!");

            foreach (Province pProvince in m_cContents)
            {
                if (pProvince == m_pMethropoly)
                    continue;

                LocationX pLoc = pProvince.BuildAdministrativeCenter(m_pInfo.m_pProvinceCapital, bFast);
                if (pLoc != null)
                    m_cSettlements.Add(pLoc);
            }

            if (m_pMethropoly.m_pCenter.Area != null)
                m_sName = m_pRace.m_pLanguage.RandomCountryName();

            return m_pCapital;
        }

        public void BuildLairs(int iScale)
        {
            foreach (Province pProvince in m_cContents)
                foreach (LandX pLand in pProvince.m_cContents)
                    pLand.BuildLair();
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

            sReasons = "";

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
                sReasons += pOpponent.m_pRace.m_sName + " \t(-1)\n";
            }
            else
            {
                iHostility--;
                sReasons += pOpponent.m_pRace.m_sName + " \t(+1)\n";
            }

            if (m_eGenderPriority == pOpponent.m_eGenderPriority)
            {
                iHostility--;
                sReasons += pOpponent.m_eGenderPriority.ToString() + " \t(+1)\n";
            }
            else
                if (m_eGenderPriority != GenderPriority.GendersEquality &&
                    pOpponent.m_eGenderPriority != GenderPriority.GendersEquality)
                {
                    iHostility++;
                    sReasons += pOpponent.m_eGenderPriority.ToString() + " \t(-1)\n";
                }

            if (m_iFood < m_iPopulation && pOpponent.m_iFood > pOpponent.m_iPopulation * 2)
            {
                iHostility++;
                sReasons += "Envy for food \t(-1)\n";
            }
            if (m_iWood < m_iPopulation && pOpponent.m_iWood > pOpponent.m_iPopulation * 2)
            {
                iHostility++;
                sReasons += "Envy for wood \t(-1)\n";
            }
            if (m_iOre < m_iPopulation && pOpponent.m_iOre > pOpponent.m_iPopulation * 2)
            {
                iHostility++;
                sReasons += "Envy for ore \t(-1)\n";
            }

            if (Math.Abs(pOpponent.m_iControl - m_iControl) - 1 != 0)
            {
                iHostility += Math.Abs(pOpponent.m_iControl - m_iControl) - 1;
                sReasons += string.Format("{0} \t({1:+#;-#})\n", State.GetControlString(pOpponent.m_iControl), 1 - Math.Abs(pOpponent.m_iControl - m_iControl));
            }

            if (pOpponent.m_iInfrastructureLevel > m_iInfrastructureLevel)
            {
                iHostility ++;//= pOpponent.m_iLifeLevel - m_iLifeLevel;
                sReasons += string.Format("Envy for civilization \t(-{0})\n", 1);//pOpponent.m_iLifeLevel - m_iLifeLevel);
            }
            else
            {
                if (pOpponent.m_iInfrastructureLevel < m_iInfrastructureLevel)
                {
                    iHostility ++;//= m_iLifeLevel - pOpponent.m_iLifeLevel;
                    sReasons += string.Format("Scorn for savagery \t(-{0})\n", 1);//m_iLifeLevel - pOpponent.m_iLifeLevel);
                }
            }

            float iCultureDifference = m_pCulture.GetDifference(pOpponent.m_pCulture);
            if (iCultureDifference > 0.75)
            {
                iHostility -= 2;
                sReasons += "Very close culture \t(+2)\n";
            }
            else
                if (iCultureDifference > 0.5)
                {
                    iHostility--;
                    sReasons += "Close culture \t(+1)\n";
                }
                else
                    if (iCultureDifference < -0.5)
                    {
                        iHostility += 2;
                        sReasons += "Very different culture \t(-2)\n";
                    }
                    else
                        if (iCultureDifference < 0)
                        {
                            iHostility++;
                            sReasons += "Different culture \t(-1)\n";
                        }

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
                                m_cSettlements.Add(pFort);
                                //bHaveOne = true;
                            }
                        }
                }
            }

        public void BuildSettlements(SettlementSize eSize, bool bFast)
        {
            if (!InfrastructureLevels[m_iInfrastructureLevel].m_cAvailableSettlements.Contains(eSize))
                return;

            int iMinCount = m_cContents.Count / 3;
            switch (eSize)
            {
                case SettlementSize.City:
                    iMinCount = m_cContents.Count / 9;
                    break;
                case SettlementSize.Town:
                    iMinCount = m_cContents.Count / 6;
                    break;
            }

            Dictionary<LandX, float> cLandsChances = new Dictionary<LandX, float>();
            foreach (Province pProvince in m_cContents)
                foreach (LandX pLand in pProvince.m_cContents)
                    cLandsChances[pLand] = (float)pLand.m_cContents.Count * pLand.Type.m_cSettlementsDensity[eSize];

            for (int i = 0; i < iMinCount; i++)
            {
                int iChance = Rnd.ChooseOne(cLandsChances.Values, 1);

                foreach (LandX pLand in cLandsChances.Keys)
                {
                    iChance--;
                    if (iChance < 0)
                    {
                        LocationX pSettlement = pLand.BuildSettlement(Settlement.Info[eSize], false, bFast);
                        if (pSettlement != null)
                        {
                            m_cSettlements.Add(pSettlement);
                            //bHaveOne = true;
                        }
                        cLandsChances[pLand] = 0;
                        break;
                    }
                }
            }

            foreach (Province pProvince in m_cContents)
                foreach (LandX pLand in pProvince.m_cContents)
                {
                    int iSettlementsCount = (int)(pLand.m_cContents.Count * pLand.Type.m_cSettlementsDensity[eSize]);
                    if (iSettlementsCount == 0)
                    {
                        int iSettlementChance = (int)(1 / (pLand.m_cContents.Count * pLand.Type.m_cSettlementsDensity[eSize]));
                        if (Rnd.OneChanceFrom(iSettlementChance))
                            iSettlementsCount = 1;
                    }

                    //bool bHaveOne = false;
                    for (int i = 0; i < iSettlementsCount; i++)
                    {
                        //if (bHaveOne && !Rnd.OneChanceFrom(3))
                        //    continue;

                        LocationX pSettlement = pLand.BuildSettlement(Settlement.Info[eSize], false, bFast);
                        if (pSettlement != null)
                        {
                            m_cSettlements.Add(pSettlement);
                            //bHaveOne = true;
                        }
                    }
                }
        }

        /// <summary>
        /// Присоединяет в общую транспортную сеть ещё не присоединённые города государства.
        /// </summary>
        /// <param name="iRoadLevel">Уровень новых дорог: 1 - просёлок, 2 - обычная дорога, 3 - имперская дорога</param>
        public void BuildRoads(int iRoadLevel, float fCycleShift)
        {
            if (iRoadLevel > InfrastructureLevels[m_iInfrastructureLevel].m_iMaxGroundRoad)
                iRoadLevel = InfrastructureLevels[m_iInfrastructureLevel].m_iMaxGroundRoad;

            if (iRoadLevel == 0)
                return;

            foreach (Province pProvince in m_cContents)
                foreach (LandX pLand in pProvince.m_cContents)
                    foreach (LocationX pLoc in pLand.m_cContents)
                        foreach (TransportationNode pLinked in pLoc.m_cLinks.Keys)
                        {
                            if (pLinked is LocationX)
                            {
                                LandX pLinkedOwner = (pLinked as LocationX).Owner as LandX;
                                if (pLinkedOwner.m_pProvince == null || pLinkedOwner.m_pProvince.Owner != this)
                                    pLoc.m_cLinks[pLinked].m_bClosed = true;
                            }
                            else
                                pLoc.m_cLinks[pLinked].m_bClosed = true;
                        }

            List<LocationX> cConnected = new List<LocationX>();
            cConnected.Add(m_pCapital);

            LocationX[] aSettlements = m_cSettlements.ToArray();

            foreach (LocationX pTown in aSettlements)
            {
                if (!cConnected.Contains(pTown) && pTown.m_iRoad > 1)
                    cConnected.Add(pTown);
            }

            while (cConnected.Count < aSettlements.Length)
            {
                //Road pBestRoad = null;
                LocationX pBestTown1 = null;
                LocationX pBestTown2 = null;
                float fMinLength = float.MaxValue;

                foreach (LocationX pTown in aSettlements)
                {
                    if (cConnected.Contains(pTown))
                        continue;

                    foreach (LocationX pOtherTown in cConnected)
                    {
                        float fDist = pTown.DistanceTo(pOtherTown, fCycleShift);// (float)Math.Sqrt((pTown.X - pOtherTown.X) * (pTown.X - pOtherTown.X) + (pTown.Y - pOtherTown.Y) * (pTown.Y - pOtherTown.Y));

                        if (fDist < fMinLength && 
                            (fMinLength == float.MaxValue || 
                             Rnd.OneChanceFrom(2)))
                        {
                            fMinLength = fDist;
                            //pBestRoad = pRoad;

                            pBestTown1 = pTown;
                            pBestTown2 = pOtherTown;
                        }
                    }
                }
                if (pBestTown2 != null)
                {
                    World.BuildRoad(pBestTown1, pBestTown2, iRoadLevel, fCycleShift);
                    
                    fMinLength = float.MaxValue;
                    LocationX pBestTown3 = null;
                    foreach (LocationX pOtherTown in cConnected)
                    {
                        float fDist = pBestTown1.DistanceTo(pOtherTown, fCycleShift);// (float)Math.Sqrt((pBestTown1.X - pOtherTown.X) * (pBestTown1.X - pOtherTown.X) + (pBestTown1.Y - pOtherTown.Y) * (pBestTown1.Y - pOtherTown.Y));

                        if (pOtherTown != pBestTown2 && 
                            fDist < fMinLength && 
                            (fMinLength == float.MaxValue || 
                             Rnd.OneChanceFrom(2)))
                        {
                            fMinLength = fDist;
                            //pBestRoad = pRoad;

                            pBestTown3 = pOtherTown;
                        }
                    }

                    if (pBestTown3 != null)
                        World.BuildRoad(pBestTown1, pBestTown3, iRoadLevel, fCycleShift);

                    cConnected.Add(pBestTown1);
                }
            }

            foreach (Province pProvince in m_cContents)
                foreach (LandX pLand in pProvince.m_cContents)
                    foreach (LocationX pLoc in pLand.m_cContents)
                        foreach (TransportationNode pLink in pLoc.m_cLinks.Keys)
                            pLoc.m_cLinks[pLink].m_bClosed = false;
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
                    sTech = "intellectual weapons";
                    break;
                case 7:
                    sTech = "beam guns";//limited teleportation
                    break;
                case 8:
                    sTech = "desintegrators";//unlimited teleportation
                    break;
                //case 8:
                //    sTech = "mind net";
                //    break;
            }

            return sTech;
        }

        public static string GetMagicString(int iLevel)
        {
            string sMagic = "sages";
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
                    sMagic = "mages";//portals
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
                    sMagic = "One";
                    break;
            }

            return sMagic;
        }

        ///// <summary>
        ///// 0 - Выживание. Закон джунглей — или ешь ты, или едят тебя. Вместо имен используются клички — имена зверей (Пёс, Ворон) или описательные эпитеты (Хромой, Одноглазый).
        ///// 1 - Власть. Есть разделение на своих и чужих. В почёте культ силы. В ходу сочетания имени и клички.
        ///// 2 - Честь. Существует неписаный кодекс поведения, предписывающий благородное отношение даже к врагу, хотя им и не всегда руководствуются. Клички в основном несут прославляющий (Доблестный, Славный) или очерняющий (Жестокий, Безжалостный) характер.
        ///// 3 - Закон. Закон один для всех — по крайней мере так провозглашается. Подлости и озлобленности ещё хватает, но страх наказания многих останавливает. Клички не используются.
        ///// 4 - Корпоративный дух. Превалирование заботы о благе своего сообщества над заботой о собственном благе является необходимым условием выживания личности.
        ///// 5 - Общее Благо. Высокая мера личной ответственности и самосознания. Индивиидум реально ощущает себя частью своего сообщества и искренне заботится о его благе.
        ///// 6 - Великая Миссия. Ответственность за весь мир и стремление сделать его лучше и чище.
        ///// 7 - Свобода. Непротивление злу насилием. Каждый волен выбирать свой путь, кем бы он ни был и куда бы ни вёл его путь.
        ///// 8 - Любовь. Общество безграничной любви и сострадания к ближнему — кем бы он ни был. Полное самоотречение и абсолютная готовность к самопожертвованию.
        ///// </summary>
        public static string GetIdeologyString(int iLevel)
        {
            string sCulture = "law of the jungle";
            switch (iLevel)
            {
                case 1:
                    sCulture = "sword law";
                    break;
                case 2:
                    sCulture = "code of honour";
                    break;
                case 3:
                    sCulture = "social equality";
                    break;
                case 4:
                    sCulture = "utilitarianism";
                    break;
                case 5:
                    sCulture = "servants of greater good";
                    break;
                case 6:
                    sCulture = "keepers of the world";
                    break;
                case 7:
                    sCulture = "freedom for all";
                    break;
                case 8:
                    sCulture = "universal love";
                    break;
            }

            return sCulture;
        }

        public static string GetGendersPariahString(GenderPriority eGenderPriority)
        {
            switch (eGenderPriority)
            {
                case GenderPriority.GendersEquality:
                    return "";
                case GenderPriority.Matriarchy:
                    return "males";
                case GenderPriority.Patriarchy:
                    return "females";
                default:
                    return "error";
            }
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
            foreach (State pState in m_cBorderWith.Keys)
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
            return string.Format("{2}(C{1}T{3}B{4}) - {0} {5}", m_sName, m_iInfrastructureLevel, m_pRace.m_sName, m_iTechLevel, m_iMagicLimit, m_pInfo.m_sName);
        }

        public override float GetMovementCost()
        {
            throw new NotImplementedException();
        }
    }
}
