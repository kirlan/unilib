using GeneLab.Genetix;
using Random;
using Socium.Nations;
using Socium.Psychology;
using Socium.Settlements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Socium.State;
using Socium.Languages;
using LandscapeGeneration.PathFind;
using LandscapeGeneration;

namespace Socium.Population
{
    public class StateSociety: NationalSociety
    {
        #region Polity Infos Array
        public class PolityInfo
        {
            public string Name { get; }
            public int Rank { get; }

            public SettlementInfo StateCapital { get; }
            public SettlementInfo ProvinceCapital { get; }

            public int MinGovernmentLevel { get; }

            public int MaxGovernmentLevel { get; }

            public bool HasDinasty { get; }

            public bool IsEmpire { get; }

            public List<Language> Languages { get; } = new List<Language>();

            public Formation Formation { get; } = null;

            /// <summary>
            /// Информация о государственном устройстве
            /// </summary>
            /// <param name="sName">Тип государственного устройства</param>
            /// <param name="iRank">Социальный ранг правителя</param>
            /// <param name="pStateCapital">Информация о поселении - столице государства</param>
            /// <param name="pProvinceCapital">Информация о поселениях - центрах провинций</param>
            /// <param name="bDinasty">Должны ли наследники являться родственниками текущего правителя</param>
            /// <param name="iMinGovernmentLevel">Минимальный возможный уровень государственности</param>
            /// <param name="iMaxGovernmentLevel">Максимальный возможный уровень государственности</param>
            /// <param name="cLanguages">Языки, носители которых могут иметь государство такого типа</param>
            public PolityInfo(string sName, int iRank, SettlementInfo pStateCapital, SettlementInfo pProvinceCapital, bool bDinasty, int iMinGovernmentLevel, int iMaxGovernmentLevel, bool bBig, Formation pSocial, Language[] cLanguages)
            {
                Name = sName;
                Rank = iRank;

                HasDinasty = bDinasty;

                StateCapital = pStateCapital;
                ProvinceCapital = pProvinceCapital;

                Formation = pSocial;

                MinGovernmentLevel = iMinGovernmentLevel;
                MaxGovernmentLevel = iMaxGovernmentLevel;

                IsEmpire = bBig;

                if (cLanguages != null)
                    Languages.AddRange(cLanguages);
            }

            public override string ToString()
            {
                return Name;
            }
        }

        private static readonly PolityInfo[] s_aPolityInfo =
        {
            new PolityInfo("Land", 1,
                new SettlementInfo(SettlementSize.Hamlet, "Hamlet", 5, 10, new BuildingInfo("Elder's hut", ProfessionInfo.RulerPrimitive, ProfessionInfo.HeirPrimitive, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Hamlet, "Hamlet", 5, 10, new BuildingInfo("Elder's hut", ProfessionInfo.GovernorPrimitive, ProfessionInfo.GovernorPrimitive, BuildingSize.Unique)),
                true, 0, 0, false, Formation.Primitive, null),
            new PolityInfo("Lands", 1,
                new SettlementInfo(SettlementSize.Hamlet, "Hamlet", 5, 10, new BuildingInfo("Elder's hut", ProfessionInfo.RulerPrimitive, ProfessionInfo.HeirPrimitive, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Hamlet, "Hamlet", 5, 10, new BuildingInfo("Elder's hut", ProfessionInfo.GovernorPrimitive, ProfessionInfo.GovernorPrimitive, BuildingSize.Unique)),
                true, 0, 0, true, Formation.Primitive, null),
            new PolityInfo("Tribes", 1,
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, new BuildingInfo("Clans hall", ProfessionInfo.RulerPrimitive, ProfessionInfo.HeirPrimitive, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, new BuildingInfo("Village hall", ProfessionInfo.GovernorPrimitive, ProfessionInfo.GovernorPrimitive, BuildingSize.Unique)),
                true, 1, 1, false, Formation.Primitive, null),
            new PolityInfo("Clans", 1,
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, new BuildingInfo("Clans hall", ProfessionInfo.RulerPrimitive, ProfessionInfo.HeirPrimitive, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, new BuildingInfo("Village hall", ProfessionInfo.GovernorPrimitive, ProfessionInfo.GovernorPrimitive, BuildingSize.Unique)),
                true, 1, 1, true, Formation.Primitive, null),
            new PolityInfo("Kingdom", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.KingEuro, ProfessionInfo.KingHeirEuro, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorEuro, ProfessionInfo.GovernorEuro, BuildingSize.Unique)),
                true, 2, 6, false, Formation.MedievalEurope, new Language[] {Language.Dwarwen, Language.European, Language.Highlander}),
            new PolityInfo("Kingdom", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.KingEuro, ProfessionInfo.KingHeirEuro, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorEuro2, ProfessionInfo.GovernorEuro2, BuildingSize.Unique)),
                true, 2, 6, false, Formation.MedievalEurope2, new Language[] {Language.Drow, Language.Elven, Language.European, Language.Highlander}),
            new PolityInfo("Empire", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.EmperorEuro, ProfessionInfo.KingHeirEuro, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorEuro3, ProfessionInfo.GovernorEuro3, BuildingSize.Unique)),
                true, 2, 6, true, Formation.MedievalEurope2, new Language[] {Language.Drow, Language.Elven, Language.European, Language.Dwarwen}),
            new PolityInfo("Kingdom", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.KingEuro, ProfessionInfo.KingHeirEuro, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorEuro4, ProfessionInfo.GovernorEuro4, BuildingSize.Unique)),
                true, 1, 6, false, Formation.MedievalEurope3, new Language[] {Language.Dwarwen, Language.European, Language.Highlander}),
            new PolityInfo("Reich", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.KingNorth, ProfessionInfo.KingHeirNorth, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorNorth, ProfessionInfo.GovernorNorth, BuildingSize.Unique)),
                true, 1, 6, false, Formation.MedievalNorth, new Language[] {Language.Dwarwen, Language.Northman}),
            new PolityInfo("Kaiserreich", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.EmperorNorth, ProfessionInfo.EmperorHeirNorth, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorNorth2, ProfessionInfo.GovernorNorth2, BuildingSize.Unique)),
                true, 2, 6, true, Formation.MedievalNorth, new Language[] {Language.Dwarwen, Language.Northman}),
            new PolityInfo("Regnum", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.KingLatin, ProfessionInfo.KingHeirLatin, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorLatin, ProfessionInfo.GovernorLatin, BuildingSize.Unique)),
                true, 1, 6, false, Formation.MedievalLatin, new Language[] {Language.Latin}),
            new PolityInfo("Imperium", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.EmperorLatin, ProfessionInfo.KingHeirLatin, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.KingLatin, ProfessionInfo.KingHeirLatin, BuildingSize.Small)),
                true, 2, 6, true, Formation.MedievalLatin, new Language[] {Language.Latin}),
            new PolityInfo("Shogunate", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.KingAsian, ProfessionInfo.KingHeirAsian, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorAsian, ProfessionInfo.GovernorAsian, BuildingSize.Unique)),
                true, 1, 6, false, Formation.MedievalAsian, new Language[] {Language.Asian}),
            new PolityInfo("Empire", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.EmperorAsian, ProfessionInfo.EmperorHeirAsian, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorAsian2, ProfessionInfo.GovernorAsian2, BuildingSize.Unique)),
                true, 2, 6, true, Formation.MedievalAsian, new Language[] {Language.Asian}),
            new PolityInfo("Shahdom", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.KingArabian, ProfessionInfo.KingHeirArabian, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorArabian, ProfessionInfo.GovernorArabian, BuildingSize.Unique)),
                true, 1, 6, false, Formation.MedievalArabian, new Language[] {Language.Arabian}),
            new PolityInfo("Sultanate", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.KingArabian2, ProfessionInfo.KingHeirArabian2, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorArabian, ProfessionInfo.GovernorArabian, BuildingSize.Unique)),
                true, 1, 6, false, Formation.MedievalArabian, new Language[] {Language.Arabian, Language.African}),
            new PolityInfo("Caliphate", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.EmperorArabian, ProfessionInfo.KingHeirEuro, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorArabian, ProfessionInfo.GovernorArabian, BuildingSize.Unique)),
                true, 2, 6, true, Formation.MedievalArabian, new Language[] {Language.Arabian, Language.African}),
            new PolityInfo("Khanate", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.KingMongol, ProfessionInfo.KingHeirEuro, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorMongol, ProfessionInfo.GovernorMongol, BuildingSize.Unique)),
                true, 1, 6, false, Formation.MedievalMongol, new Language[] {Language.Orkish, Language.Eskimoid}),
            new PolityInfo("Khaganate", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.EmperorMongol, ProfessionInfo.KingHeirEuro, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.KingMongol, ProfessionInfo.KingHeirEuro, BuildingSize.Small)),
                true, 2, 6, true, Formation.MedievalMongol, new Language[] {Language.Orkish, Language.Eskimoid}),
            new PolityInfo("Knyazdom", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.KingSlavic, ProfessionInfo.KingHeirSlavic, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorSlavic, ProfessionInfo.GovernorSlavic, BuildingSize.Unique)),
                true, 1, 6, false, Formation.MedievalSlavic, new Language[] {Language.Slavic}),
            new PolityInfo("Tsardom", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.EmperorSlavic, ProfessionInfo.EmperorHeirSlavic, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.KingSlavic, ProfessionInfo.KingHeirSlavic, BuildingSize.Small)),
                true, 2, 6, true, Formation.MedievalSlavic, new Language[] {Language.Slavic}),
            new PolityInfo("Basileia", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.KingGreek, ProfessionInfo.KingHeirGreek, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorGreek, ProfessionInfo.GovernorGreek, BuildingSize.Unique)),
                true, 1, 6, false, Formation.MedievalGreek, new Language[] {Language.Greek}),
            new PolityInfo("Autokratoria", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.EmperorGreek, ProfessionInfo.KingHeirGreek, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorGreek2, ProfessionInfo.GovernorGreek2, BuildingSize.Unique)),
                true, 2, 6, true, Formation.MedievalGreek, new Language[] {Language.Greek}),
            new PolityInfo("Raj", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.KingHindu, ProfessionInfo.KingHeirHindu, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorHindu, ProfessionInfo.GovernorHindu, BuildingSize.Unique)),
                true, 1, 6, false, Formation.MedievalHindu, new Language[] {Language.Hindu}),
            new PolityInfo("Empire", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.EmperorHindu, ProfessionInfo.EmperorHeirHindu, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.KingHindu, ProfessionInfo.KingHeirHindu, BuildingSize.Small)),
                true, 2, 6, true, Formation.MedievalHindu, new Language[] {Language.Hindu}),
            new PolityInfo("Altepetl", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.KingAztec, ProfessionInfo.KingHeirAztec, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorAztec, ProfessionInfo.GovernorAztec, BuildingSize.Unique)),
                true, 1, 6, false, Formation.MedievalAztec, new Language[] {Language.Aztec}),
            new PolityInfo("Empire", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.EmperorEuro, ProfessionInfo.KingHeirAztec, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorAztec, ProfessionInfo.GovernorAztec, BuildingSize.Unique)),
                true, 2, 6, true, Formation.MedievalAztec, new Language[] {Language.Aztec}),
            new PolityInfo("Republic", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Statehouse", ProfessionInfo.RulerModern, ProfessionInfo.AdvisorModern, BuildingSize.Small)),
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Town hall", ProfessionInfo.GovernorModern, ProfessionInfo.GovernorModern, BuildingSize.Unique)),
                false, 3, 7, false, Formation.Modern, null),
            //new StateInfo("Republic", 16,
            //    new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 15, 5, 10, new BuildingInfo("Palace", "Dictator", "Dictator", 16)), 
            //    new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, 5, 7, new BuildingInfo("Palace", "Governor", "Governor", 14)), 
            //    "Counsellor", "Counsellor", false, 2, 6, 3, 4),
            //new StateInfo("Republic", 16,
            //    new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 15, 5, 10, new BuildingInfo("Palace", "General", "General", 16)), 
            //    new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, 5, 7, new BuildingInfo("Palace", "Colonel", "Colonel", 14)), 
            //    "Officer", "Officer", false, 2, 6, 3, 4),
            new PolityInfo("Federation", 2,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, new BuildingInfo("Statehouse", ProfessionInfo.RulerModern2, ProfessionInfo.AdvisorModern2, BuildingSize.Unique)),
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Town hall", ProfessionInfo.GovernorModern2, ProfessionInfo.GovernorModern2, BuildingSize.Unique)),
                false, 4, 7, false, Formation.Modern, null),
            new PolityInfo("League", 3,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, new BuildingInfo("Palace", ProfessionInfo.RulerModern3, ProfessionInfo.AdvisorModern3, BuildingSize.Unique)),
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Palace", ProfessionInfo.GovernorModern, ProfessionInfo.GovernorModern, BuildingSize.Unique)),
                false, 4, 7, true, Formation.Modern, null),
            new PolityInfo("Union", 3,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, new BuildingInfo("Statehouse", ProfessionInfo.RulerModern2, ProfessionInfo.AdvisorModern2, BuildingSize.Unique)),
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Town hall", ProfessionInfo.GovernorModern2, ProfessionInfo.GovernorModern2, BuildingSize.Unique)),
                false, 5, 7, true, Formation.Modern, null),
            new PolityInfo("Alliance", 3,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, new BuildingInfo("Palace", ProfessionInfo.RulerModern4, ProfessionInfo.AdvisorModern, BuildingSize.Unique)),
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Town hall", ProfessionInfo.GovernorModern2, ProfessionInfo.GovernorModern2, BuildingSize.Unique)),
                false, 5, 7, true, Formation.Modern, null),
            new PolityInfo("Coalition", 3,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, new BuildingInfo("Statehouse", ProfessionInfo.RulerModern3, ProfessionInfo.AdvisorModern, BuildingSize.Unique)),
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Town hall", ProfessionInfo.GovernorModern2, ProfessionInfo.GovernorModern2, BuildingSize.Unique)),
                false, 5, 7, true, Formation.Modern, null),
            new PolityInfo("Association", 3,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, new BuildingInfo("Statehouse", ProfessionInfo.RulerModern2, ProfessionInfo.AdvisorModern4, BuildingSize.Unique)),
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Town hall", ProfessionInfo.GovernorModern, ProfessionInfo.GovernorModern, BuildingSize.Unique)),
                false, 5, 7, true, Formation.Modern, null),
            //new StateInfo("Realm", 17,
            //    new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 16, 5, 10, new BuildingInfo("Citadel", "God-King", "Goddess-Queen", 17)), 
            //    new SettlementInfo(SettlementSize.City, "City", 40, 80, 15, 5, 10, new BuildingInfo("Palace", "Father", "Mother", 15)), 
            //    "Brother", "Sister", false, 5, 8, null),
            new PolityInfo("Commonwealth", 1,
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Town hall", ProfessionInfo.RulerModern3, ProfessionInfo.AdvisorModern5, BuildingSize.Unique)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Town hall", ProfessionInfo.GovernorModern3, ProfessionInfo.GovernorModern3, BuildingSize.Unique)),
                false, 7, 8, false, Formation.Future, null),
            new PolityInfo("Society", 1,
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, null),
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, null),
                false, 7, 8, false, Formation.Future, null),
            new PolityInfo("Collective", 2,
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, null),
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, null),
                false, 7, 8, true, Formation.Future, null),
        };
        #endregion

        /// <summary>
        /// 0 - Анархия. Есть номинальная власть, но она не занимается охраной правопорядка.
        /// 1 - Власти занимаются только самыми вопиющими преступлениями.
        /// 2 - Есть законы, их надо соблюдать, кто не соблюдает - тот преступник, а вор должен сидеть в тюрьме.
        /// 3 - Законы крайне строги, широко используется смертная казнь.
        /// 4 - Тоталитарная диктатура. Все граждане, кроме правящей верхушки, попадают под презумпцию виновности.
        /// </summary>
        public int Control { get; private set; } = 0;
        /// <summary>
        /// Уровень социального (не)равенства.
        /// 0 - рабство
        /// 1 - крепостное право
        /// 2 - капитализм
        /// 3 - социализм
        /// 4 - коммунизм
        /// </summary>
        public int SocialEquality { get; set; } = 0;

        public PolityInfo Polity { get; private set; } = null;

        private readonly State m_pState = null;

        public Dictionary<Estate.SocialRank, Estate> Estates { get; } = new Dictionary<Estate.SocialRank, Estate>();

        public Nation HostNation { get; private set; } = null;

        public Nation SlavesNation { get; private set; } = null;

        public StateSociety(State pState)
            : base(pState.Methropoly.LocalSociety.TitularNation)
        {
            m_pState = pState;

            TechLevel = 0;
            InfrastructureLevel = 0;

            Culture[Gender.Male] = new Culture(pState.Methropoly.LocalSociety.Culture[Gender.Male], Customs.Mutation.Possible);
            Culture[Gender.Male].Customs.ApplyFenotype(TitularNation.PhenotypeMale);
            Culture[Gender.Female] = new Culture(pState.Methropoly.LocalSociety.Culture[Gender.Female], Customs.Mutation.Possible);
            Culture[Gender.Female].Customs.ApplyFenotype(TitularNation.PhenotypeFemale);

            FixSexCustoms();
        }

        public void CalculateTitularNation()
        {
            Dictionary<Nation, int> cNationsCount = new Dictionary<Nation, int>();

            foreach (Province pProvince in m_pState.Contents)
            {
                if (!cNationsCount.TryGetValue(pProvince.LocalSociety.TitularNation, out int iCount))
                    cNationsCount[pProvince.LocalSociety.TitularNation] = 0;
                cNationsCount[pProvince.LocalSociety.TitularNation] = iCount + pProvince.LocationsCount;

                if (pProvince.LocalSociety.TechLevel > TechLevel)
                    TechLevel = pProvince.LocalSociety.TechLevel;

                if (pProvince.LocalSociety.InfrastructureLevel > InfrastructureLevel)
                    InfrastructureLevel = pProvince.LocalSociety.InfrastructureLevel;
            }

            Nation pMostCommonNation = cNationsCount.Keys.ToArray()[Rnd.ChooseBest(cNationsCount.Values)];
            if (TitularNation.DominantPhenotype.m_pValues.Get<NutritionGenetix>().IsParasite() || TitularNation.IsInvader)
            {
                cNationsCount.Remove(TitularNation);
                if (cNationsCount.Count > 0)
                    pMostCommonNation = cNationsCount.Keys.ToArray()[Rnd.ChooseBest(cNationsCount.Values)];
            }
            else
            {
                UpdateTitularNation(pMostCommonNation);
                cNationsCount.Remove(TitularNation);
            }
            HostNation = pMostCommonNation;
            if (cNationsCount.ContainsKey(pMostCommonNation))
                cNationsCount.Remove(pMostCommonNation);
            if (cNationsCount.Count > 0)
                pMostCommonNation = cNationsCount.Keys.ToArray()[Rnd.ChooseOne(cNationsCount.Values, 1)];

            SlavesNation = pMostCommonNation;

            Nation getAccessableNation(bool bCanBeDying, bool bCanBeParasite, bool bOnlyLocals, Nation pDefault)
            {
                List<Nation> cAccessableNations = new List<Nation>();
                ContinentX pContinent = m_pState.GetOwner();
                foreach (var pNations in pContinent.LocalNations)
                {
                    cAccessableNations.AddRange(pNations.Value);
                }
                if (!bOnlyLocals)
                {
                    if (InfrastructureLevels[InfrastructureLevel].MaxNavalPath == RoadQuality.Good ||
                        InfrastructureLevels[InfrastructureLevel].AerialAvailability > 1)
                    {
                        var pWorld = pContinent.Origin.GetOwner();
                        foreach (var pOtherContinent in pWorld.Contents)
                        {
                            if (pOtherContinent.As<ContinentX>() == pContinent)
                                continue;

                            foreach (var pNations in pOtherContinent.As<ContinentX>().LocalNations)
                            {
                                cAccessableNations.AddRange(pNations.Value);
                            }
                        }
                    }
                }

                Dictionary<Nation, int> cChances = new Dictionary<Nation, int>();
                foreach (var pNation in cAccessableNations)
                {
                    if (pNation.IsAncient && !bCanBeDying)
                        continue;

                    if (pNation.DominantPhenotype.m_pValues.Get<NutritionGenetix>().IsParasite() && !bCanBeParasite)
                        continue;

                    if (!cChances.ContainsKey(pNation))
                        cChances[pNation] = 0;
                    cChances[pNation]++;

                    if (pNation.Race.Language == TitularNation.Race.Language)
                        cChances[pNation]++;

                    foreach (var pOtherState in m_pState.BorderWith.Keys)
                    {
                        if (pOtherState.Forbidden)
                            continue;

                        if (pOtherState.Society.TitularNation == pNation)
                            cChances[pNation]++;
                    }

                    int iHostility = TitularNation.DominantPhenotype.GetFenotypeDifference(pNation.DominantPhenotype);
                    if (iHostility > 0)
                        cChances[pNation]++;
                    if (iHostility > 3)
                        cChances[pNation]++;

                    if (pNation == pDefault)
                        cChances[pNation] = 1;
                }

                int iChoice = Rnd.ChooseOne(cChances.Values, 2);
                if (iChoice == -1)
                    throw new InvalidOperationException("Can't find host nation for a parasite!");

                foreach (var pNation in cChances)
                {
                    iChoice--;
                    if (iChoice < 0)
                    {
                        return pNation.Key;
                    }
                }

                return pDefault;
            }

            if (TitularNation == HostNation && (TitularNation.DominantPhenotype.m_pValues.Get<NutritionGenetix>().IsParasite() || (TitularNation.IsInvader /*&& m_iControl >= 3*/)))
            {
                HostNation = getAccessableNation(false, false, true, HostNation);
            }
            if (TitularNation == SlavesNation && Rnd.OneChanceFrom(3))
            {
                SlavesNation = getAccessableNation(true, false, false, SlavesNation);
            }
        }

        public override void AddBuildings(Settlement pSettlement)
        {
            pSettlement.Buildings.Clear();

            int iBuildingsCount = pSettlement.Profile.MinBuildingsCount + Rnd.Get(pSettlement.Profile.DeltaBuildingsCount + 1);
            for (int i = 0; i < iBuildingsCount; i++)
            {
                Building pNewBuilding = new Building(pSettlement, ChooseNewBuilding(pSettlement));
                pSettlement.Buildings.Add(pNewBuilding);
            }

            if (pSettlement.Profile.MainBuilding != null)
            {
                Building pNewBuilding = new Building(pSettlement, pSettlement.Profile.MainBuilding);
                pSettlement.Buildings.Add(pNewBuilding);
            }
        }

        public Dictionary<ProfessionInfo, int> Professions { get; } = new Dictionary<ProfessionInfo, int>();
        public Dictionary<Estate.SocialRank, int> EstatesCounts { get; } = new Dictionary<Estate.SocialRank, int>();

        /// <summary>
        /// Распределяет население государства по сословиям.
        /// Вызывается в CWorld.PopulateMap после того, как созданы и застроены все поселения в государстве - т.е. после того, как сформировано собственно население.
        /// </summary>
        public void SetEstates()
        {
            // Базовые настройки культуры и обычаев для сообщества
            Estate pBase = new Estate(this, Estate.SocialRank.Commoners, Polity.Formation.GetEstateName(Estate.SocialRank.Commoners), true);

            // Элита может немного отличаться от среднего класса
            Estates[Estate.SocialRank.Elite] = new Estate(pBase, Estate.SocialRank.Elite, Polity.Formation.GetEstateName(Estate.SocialRank.Elite), false);

            // духовенство - его обычаи должны отличаться не только от среднего класса, но и от элиты
            do
            {
                Estates[Estate.SocialRank.Clergy] = new Estate(pBase, Estate.SocialRank.Clergy, Polity.Formation.GetEstateName(Estate.SocialRank.Clergy), false);
            }
            while (Estates[Estate.SocialRank.Elite].Culture[Gender.Male].Equals(Estates[Estate.SocialRank.Clergy].Culture[Gender.Male]) ||
                   Estates[Estate.SocialRank.Elite].Culture[Gender.Female].Equals(Estates[Estate.SocialRank.Clergy].Culture[Gender.Female]));
            if (Rnd.OneChanceFrom(2))
                Estates[Estate.SocialRank.Clergy].UpdateTitularNation(HostNation);

            // средний класс - это и есть основа всего сообщества
            Estates[Estate.SocialRank.Commoners] = pBase;
            Estates[Estate.SocialRank.Commoners].UpdateTitularNation(HostNation);
            // TODO: насколько сильно родная культура порабощённой нации должна влиять на культуру образованного ей сословия?
            // Сейчас она не влияет вообще, порабощённые расы полностью ассимилируются. Но правильно ли это?

            // низший класс - его обычаи должны отличаться не только от среднего класса, но и от элиты
            do
            {
                Estates[Estate.SocialRank.Lowlifes] = new Estate(pBase, Estate.SocialRank.Lowlifes, Polity.Formation.GetEstateName(Estate.SocialRank.Lowlifes), false);
            }
            while (Estates[Estate.SocialRank.Elite].Culture[Gender.Male].Equals(Estates[Estate.SocialRank.Lowlifes].Culture[Gender.Male]) ||
                   Estates[Estate.SocialRank.Elite].Culture[Gender.Female].Equals(Estates[Estate.SocialRank.Lowlifes].Culture[Gender.Female]) ||
                   Estates[Estate.SocialRank.Clergy].Culture[Gender.Male].Equals(Estates[Estate.SocialRank.Lowlifes].Culture[Gender.Male]) ||
                   Estates[Estate.SocialRank.Clergy].Culture[Gender.Female].Equals(Estates[Estate.SocialRank.Lowlifes].Culture[Gender.Female]));

            Estates[Estate.SocialRank.Lowlifes].UpdateTitularNation(SlavesNation);

            // аутсайдеры - строим либо на базе среднего класса, либо на базе низшего - и следим, чтобы тоже отличалось от всех других сословий
            do
            {
                if (!Rnd.OneChanceFrom(3) && SocialEquality != 0)
                {
                    Estates[Estate.SocialRank.Outlaws] = new Estate(Estates[Estate.SocialRank.Lowlifes], Estate.SocialRank.Outlaws, Polity.Formation.GetEstateName(Estate.SocialRank.Outlaws), false);
                }
                else
                {
                    Estates[Estate.SocialRank.Outlaws] = new Estate(pBase, Estate.SocialRank.Outlaws, Polity.Formation.GetEstateName(Estate.SocialRank.Outlaws), false);
                }
            }
            while (Estates[Estate.SocialRank.Elite].Culture[Gender.Male].Equals(Estates[Estate.SocialRank.Outlaws].Culture[Gender.Male]) ||
                   Estates[Estate.SocialRank.Elite].Culture[Gender.Female].Equals(Estates[Estate.SocialRank.Outlaws].Culture[Gender.Female]) ||
                   Estates[Estate.SocialRank.Lowlifes].Culture[Gender.Male].Equals(Estates[Estate.SocialRank.Outlaws].Culture[Gender.Male]) ||
                   Estates[Estate.SocialRank.Lowlifes].Culture[Gender.Female].Equals(Estates[Estate.SocialRank.Outlaws].Culture[Gender.Female]) ||
                   Estates[Estate.SocialRank.Clergy].Culture[Gender.Male].Equals(Estates[Estate.SocialRank.Outlaws].Culture[Gender.Male]) ||
                   Estates[Estate.SocialRank.Clergy].Culture[Gender.Female].Equals(Estates[Estate.SocialRank.Outlaws].Culture[Gender.Female]) ||
                   pBase.Culture[Gender.Male].Equals(Estates[Estate.SocialRank.Outlaws].Culture[Gender.Male]) ||
                   pBase.Culture[Gender.Female].Equals(Estates[Estate.SocialRank.Outlaws].Culture[Gender.Female]));

            Estates[Estate.SocialRank.Outlaws].UpdateTitularNation(Rnd.OneChanceFrom(3) ? TitularNation : Rnd.OneChanceFrom(2) ? HostNation : SlavesNation);

            // перебираем все поселения, где присутсвует сообщество
            foreach (Settlement pSettlement in Settlements.Select(x => x.Settlement))
            {
                if (pSettlement == null)
                    continue;

                if (pSettlement.Buildings.Count > 0)
                {
                    // перебираем все строения в поселениях
                    foreach (BuildingInfo pBuildingInfo in pSettlement.Buildings.Select(x => x.Info))
                    {
                        int iOwnersCount = pBuildingInfo.OwnersCount;
                        int iWorkersCount = pBuildingInfo.WorkersCount;

                        var pOwner = pBuildingInfo.OwnerProfession;
                        Professions.TryGetValue(pOwner, out int iCount);
                        Professions[pOwner] = iCount + iOwnersCount;

                        var pWorkers = pBuildingInfo.WorkersProfession;
                        Professions.TryGetValue(pWorkers, out iCount);
                        Professions[pWorkers] = iCount + iWorkersCount;
                    }
                }
            }

            // таблица предпочтений в профессиях - в соответствии с необходимыми для профессии скиллами и полом
            SortedDictionary<int, List<ProfessionInfo>> cProfessionPreference = new SortedDictionary<int, List<ProfessionInfo>>();

            /// <summary>
            /// Удаляет профессию из списка преференций
            /// </summary>
            /// <param name="cProfessionPreference"></param>
            /// <param name="pProfession"></param>
            void removeProfessionPreference(ProfessionInfo pProfession)
            {
                List<int> cCemetary = new List<int>();
                foreach (var pPreference in cProfessionPreference)
                {
                    if (pPreference.Value.Contains(pProfession))
                    {
                        pPreference.Value.Remove(pProfession);
                        if (pPreference.Value.Count == 0)
                            cCemetary.Add(pPreference.Key);
                    }
                }

                foreach (int iPreference in cCemetary)
                    cProfessionPreference.Remove(iPreference);
            }

            int iTotalPopulation = 0;

            // заполняем таблицу предпочтительных профессий - с точки зрения общих культурных норм сообщества
            foreach (var pProfession in Professions)
            {
                iTotalPopulation += pProfession.Value;

                int iPreference = GetProfessionSkillPreference(pProfession.Key);

                if (pProfession.Key.IsMaster)
                    iPreference += 4;

                int iDiscrimination = (int)(DominantCulture.GetTrait(Trait.Fanaticism) + 0.5);

                var eProfessionGenderPriority = GetProfessionGenderPriority(pProfession.Key);

                if (DominantCulture.Customs.Has(Customs.GenderPriority.Patriarchy) &&
                    eProfessionGenderPriority == Customs.GenderPriority.Matriarchy)
                {
                    iPreference -= iDiscrimination;
                }

                if (DominantCulture.Customs.Has(Customs.GenderPriority.Matriarchy) &&
                    eProfessionGenderPriority == Customs.GenderPriority.Patriarchy)
                {
                    iPreference -= iDiscrimination;
                }

                if (!cProfessionPreference.ContainsKey(iPreference))
                    cProfessionPreference[iPreference] = new List<ProfessionInfo>();

                cProfessionPreference[iPreference].Add(pProfession.Key);
            }

            //Численность сословий - в зависимости от m_iSocialEquality.
            //При рабовладельческом строе (0) элита должна составлять порядка 1% населения, низшее сословие - 90-95%
            //При коммунизме - численность сословий равна. Или может наоборот, должна быть элита 90+%

            int iLowEstateCount = iTotalPopulation * (92 - SocialEquality * 23) / 100;
            int iEliteEstateCount = iTotalPopulation * (10 + SocialEquality * 5) / 100;

            void addCasteRestrictedProfessionToEstate(ProfessionInfo pProfession)
            {
                if (pProfession.CasteRestricted.HasValue)
                {
                    var pEstate = Estates[Estate.SocialRank.Elite];
                    Professions.TryGetValue(pProfession, out int iCount);
                    switch (pProfession.CasteRestricted)
                    {
                        case ProfessionInfo.Caste.Elite:
                            pEstate = Estates[Estate.SocialRank.Elite];
                            iEliteEstateCount -= iCount;
                            break;
                        case ProfessionInfo.Caste.Cleregy:
                            pEstate = Estates[Estate.SocialRank.Clergy];
                            break;
                        case ProfessionInfo.Caste.Low:
                            pEstate = Estates[Estate.SocialRank.Lowlifes];
                            iLowEstateCount -= iCount;
                            break;
                        case ProfessionInfo.Caste.Outlaw:
                            pEstate = Estates[Estate.SocialRank.Outlaws];
                            break;
                    }
                    pEstate.GenderProfessionPreferences[pProfession] = GetProfessionGenderPriority(pProfession);
                    removeProfessionPreference(pProfession);
                }
            }

            addCasteRestrictedProfessionToEstate(Polity.StateCapital.MainBuilding.OwnerProfession);
            addCasteRestrictedProfessionToEstate(Polity.StateCapital.MainBuilding.WorkersProfession);
            addCasteRestrictedProfessionToEstate(Polity.ProvinceCapital.MainBuilding.OwnerProfession);

            foreach (var pProfession in Professions)
            {
                addCasteRestrictedProfessionToEstate(pProfession.Key);
            }

            int iMinCommonersProfessionsCount = cProfessionPreference.Count - 2 * cProfessionPreference.Count / 3;
            if (SocialEquality != 0 || Estates[Estate.SocialRank.Lowlifes].GenderProfessionPreferences.Count == 0)
            {
                while (iLowEstateCount > 0)
                {
                    if (cProfessionPreference.Count <= iMinCommonersProfessionsCount)
                        break;

                    ProfessionInfo pBestFit = null;
                    int iLowestPreference = cProfessionPreference.Keys.First();

                    int iBestFit = int.MinValue;
                    foreach (ProfessionInfo pProfession in cProfessionPreference[iLowestPreference])
                    {
                        if (pProfession.CasteRestricted != ProfessionInfo.Caste.MiddleOrUp &&
                            Professions[pProfession] < iLowEstateCount && Professions[pProfession] > iBestFit)
                        {
                            pBestFit = pProfession;
                            iBestFit = Professions[pProfession];
                        }
                    }
                    if (pBestFit == null)
                    {
                        iBestFit = int.MaxValue;
                        foreach (ProfessionInfo pProfession in cProfessionPreference[iLowestPreference])
                        {
                            if (pProfession.CasteRestricted != ProfessionInfo.Caste.MiddleOrUp &&
                                Professions[pProfession] < iBestFit)
                            {
                                pBestFit = pProfession;
                                iBestFit = Professions[pProfession];
                            }
                        }
                    }
                    if (pBestFit != null)
                    {
                        Estates[Estate.SocialRank.Lowlifes].GenderProfessionPreferences[pBestFit] = GetProfessionGenderPriority(pBestFit);
                        iLowEstateCount -= iBestFit;
                        removeProfessionPreference(pBestFit);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            iMinCommonersProfessionsCount = cProfessionPreference.Count - cProfessionPreference.Count / 2;
            while (iEliteEstateCount > 0)
            {
                if (cProfessionPreference.Count <= iMinCommonersProfessionsCount)
                    break;

                ProfessionInfo pBestFit = null;
                int iHighestPreference = cProfessionPreference.Keys.Last();

                int iBestFit = iEliteEstateCount;
                foreach (ProfessionInfo pStrata in cProfessionPreference[iHighestPreference])
                {
                    if (Professions[pStrata] < iBestFit)
                    {
                        pBestFit = pStrata;
                        iBestFit = Professions[pStrata];
                    }
                }
                if (pBestFit != null)
                {
                    Estates[Estate.SocialRank.Elite].GenderProfessionPreferences[pBestFit] = GetProfessionGenderPriority(pBestFit);
                    iEliteEstateCount -= iBestFit;
                    removeProfessionPreference(pBestFit);
                }
                else
                {
                    break;
                }
            }

            foreach (var pPreference in cProfessionPreference)
            {
                foreach (ProfessionInfo pProfession in pPreference.Value)
                {
                    Estates[Estate.SocialRank.Commoners].GenderProfessionPreferences[pProfession] = GetProfessionGenderPriority(pProfession);
                }
            }

            foreach (var pEstate in Estates)
            {
                EstatesCounts[pEstate.Key] = 0;
                foreach (var pProfession in pEstate.Value.GenderProfessionPreferences)
                {
                    if (Professions.TryGetValue(pProfession.Key, out int iCount))
                        EstatesCounts[pEstate.Key] += iCount;
                }
            }
        }

        /// <summary>
        /// Slavery - Serfdom - Hired labour - Social equality - Utopia
        /// </summary>
        /// <param name="iEquality"></param>
        /// <returns></returns>
        public static string GetEqualityString(int iEquality)
        {
            switch (iEquality)
            {
                case 1:
                    return "Serfdom";
                case 2:
                    return "Hired labour";
                case 3:
                    return "Social equality";
                case 4:
                    return "Utopia";
            }
            return "Slavery";
        }

        public int GetImportedTech()
        {
            if (TitularNation.IsAncient)
                return -1;

            int iMaxTech = GetEffectiveTech();
            foreach (State pState in m_pState.BorderWithKeys)
            {
                if (pState.Forbidden)
                    continue;

                if (pState.Society.GetEffectiveTech() > iMaxTech)
                    iMaxTech = pState.Society.GetEffectiveTech();
            }

            if (iMaxTech <= GetEffectiveTech())
                iMaxTech = -1;

            return iMaxTech;
        }

        public string GetImportedTechString()
        {
            if (TitularNation.IsAncient)
                return "";

            int iMaxTech = GetEffectiveTech();
            State pExporter = null;
            foreach (State pState in m_pState.BorderWithKeys)
            {
                if (pState.Forbidden)
                    continue;

                if (pState.Society.GetEffectiveTech() > iMaxTech)
                {
                    iMaxTech = pState.Society.GetEffectiveTech();
                    pExporter = pState;
                }
            }

            if (pExporter == null)
                return "";

            return GetTechString(pExporter.Society.TechLevel, pExporter.Society.DominantCulture.Customs.ValueOf<Customs.Science>());
        }
        public override string ToString()
        {
            return string.Format("{2} (C{1}T{3}M{5}) - {0} {4}", Name, DominantCulture.ProgressLevel, TitularNation, TechLevel, Polity.Name, MagicLimit);
        }

        protected override BuildingInfo ChooseNewBuilding(Settlement pSettlement)
        {
            int iInfrastructureLevel = InfrastructureLevel;
            int iControl = Control * 2;

            Dictionary<BuildingInfo, float> cChances = new Dictionary<BuildingInfo, float>();
            switch (pSettlement.Profile.Size)
            {
                case SettlementSize.Hamlet:
                    {
                        if (pSettlement.Buildings.Count > 0)
                        {
                            if (iInfrastructureLevel < 2)
                                cChances[BuildingInfo.WarriorsHutSmall] = DominantCulture.GetTrait(Trait.Agression) / 2;

                            if (iInfrastructureLevel < 2)
                                cChances[BuildingInfo.ShamansHutSmall] = DominantCulture.GetTrait(Trait.Piety) / 2;

                            if (SocialEquality == 0)
                                cChances[BuildingInfo.SlavePensMedium] = (float)cChances.Count / 2 + 1;

                            cChances[BuildingInfo.RaidersHutSmall] = DominantCulture.GetTrait(Trait.Treachery) / 2;
                        }

                        BuildingInfo pProfile;
                        switch (pSettlement.Speciality)
                        {
                            case SettlementSpeciality.Fishers:
                                pProfile = SocialEquality == 0 ? BuildingInfo.FishingBoatSlvMedium : BuildingInfo.FishingBoatMedium;
                                break;
                            case SettlementSpeciality.Farmers:
                                pProfile = SocialEquality == 0 ? BuildingInfo.FarmSlvMedium : BuildingInfo.FarmMedium;
                                break;
                            case SettlementSpeciality.Peasants:
                                pProfile = SocialEquality == 0 ? BuildingInfo.PeasantsHutSlvMedium : BuildingInfo.PeasantsHutMedium;
                                break;
                            case SettlementSpeciality.Hunters:
                                pProfile = SocialEquality == 0 ? BuildingInfo.HuntersHutSlvMedium : BuildingInfo.HuntersHutMedium;
                                break;
                            case SettlementSpeciality.Miners:
                                pProfile = SocialEquality == 0 ? BuildingInfo.MineSlvMedium : BuildingInfo.MineMedium;
                                break;
                            case SettlementSpeciality.Lumberjacks:
                                pProfile = SocialEquality == 0 ? BuildingInfo.LumberjacksHutSlvMedium : BuildingInfo.LumberjacksHutMedium;
                                break;
                            default:
                                pProfile = BuildingInfo.LoafersHut;
                                break;
                        }

                        cChances[pProfile] = (float)cChances.Count / 2 + 1;
                    }
                    break;
                case SettlementSize.Village:
                    {
                        if (pSettlement.Buildings.Count > 0)// && Rnd.OneChanceFrom(2))
                        {
                            BuildingInfo pGuard;
                            if (iInfrastructureLevel < 2)
                                pGuard = BuildingInfo.WarriorsHutSmall;
                            else if (iInfrastructureLevel < 4)
                                pGuard = BuildingInfo.GuardTowerSmall;
                            else if (iInfrastructureLevel == 8)
                                pGuard = BuildingInfo.EmergencyPostSmall;
                            else
                                pGuard = BuildingInfo.PoliceStationSmall;

                            cChances[pGuard] = (float)iControl * DominantCulture.GetTrait(Trait.Agression) / 4;

                            BuildingInfo pChurch;
                            if (iInfrastructureLevel < 2)
                                pChurch = BuildingInfo.ShamansHutSmall;
                            else
                                pChurch = BuildingInfo.TempleSmall;

                            cChances[pChurch] = DominantCulture.GetTrait(Trait.Piety);

                            if (iInfrastructureLevel >= 2)
                            {
                                if (iInfrastructureLevel < 4)
                                {
                                    cChances[SocialEquality == 0 ? BuildingInfo.InnSlvSmall : BuildingInfo.InnSmall] = DominantCulture.GetTrait(Trait.Simplicity) / 4;
                                    cChances[SocialEquality == 0 ? BuildingInfo.TavernSlvSmall : BuildingInfo.TavernSmall] = DominantCulture.GetTrait(Trait.Simplicity);
                                }
                                else
                                {
                                    cChances[SocialEquality == 0 ? BuildingInfo.HotelSlvSmall : BuildingInfo.HotelSmall] = DominantCulture.GetTrait(Trait.Simplicity) / 4;
                                    cChances[SocialEquality == 0 ? BuildingInfo.BarSlvSmall : BuildingInfo.BarSmall] = DominantCulture.GetTrait(Trait.Simplicity);
                                }
                            }

                            cChances[BuildingInfo.RoguesDenSmall] = DominantCulture.GetTrait(Trait.Treachery) / 2;

                            if (iInfrastructureLevel >= 3)
                            {
                                float fScience = 0.05f;
                                if (DominantCulture.Customs.Has(Customs.MindSet.Balanced_mind))
                                    fScience = 0.25f;
                                if (DominantCulture.Customs.Has(Customs.MindSet.Logic))
                                    fScience = 0.5f;

                                if (TitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Sapient)
                                    fScience *= 2;
                                if (TitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Ingenious)
                                    fScience *= 4;

                                cChances[BuildingInfo.SchoolSmall] = fScience / 4;
                            }

                            //if (pState.m_iSocialEquality == 0)
                            //{
                            //    cChances[BuildingInfo.SlavePensMedium] = (float)cChances.Count;// / 2 + 1;
                            //}

                            cChances[BuildingInfo.MarketSmall] = (float)cChances.Count / 2;

                            if (Polity.HasDinasty)
                            {
                                cChances[SocialEquality == 0 ? BuildingInfo.EstateSlvSmall : BuildingInfo.EstateSmall] = (float)cChances.Count / 4;
                                cChances[SocialEquality == 0 ? BuildingInfo.CastleSlvSmall : BuildingInfo.CastleSmall] = (float)cChances.Count / 12;
                            }
                        }

                        BuildingInfo pProfile;
                        switch (pSettlement.Speciality)
                        {
                            case SettlementSpeciality.Fishers:
                                pProfile = SocialEquality == 0 ? BuildingInfo.FishingBoatSlvMedium : BuildingInfo.FishingBoatMedium;
                                break;
                            case SettlementSpeciality.Farmers:
                                pProfile = Rnd.OneChanceFrom(2) ? (SocialEquality == 0 ? BuildingInfo.FarmSlvMedium : BuildingInfo.FarmMedium) : (SocialEquality == 0 ? BuildingInfo.FarmSlvLarge : BuildingInfo.FarmLarge);
                                break;
                            case SettlementSpeciality.Peasants:
                                pProfile = Rnd.OneChanceFrom(2) ? (SocialEquality == 0 ? BuildingInfo.PeasantsHutSlvMedium : BuildingInfo.PeasantsHutMedium) : (SocialEquality == 0 ? BuildingInfo.PeasantsHutSlvLarge : BuildingInfo.PeasantsHutLarge);
                                break;
                            case SettlementSpeciality.Hunters:
                                pProfile = Rnd.OneChanceFrom(2) ? (SocialEquality == 0 ? BuildingInfo.HuntersHutSlvMedium : BuildingInfo.HuntersHutMedium) : (SocialEquality == 0 ? BuildingInfo.HuntersHutSlvLarge : BuildingInfo.HuntersHutLarge);
                                break;
                            case SettlementSpeciality.Miners:
                                pProfile = Rnd.OneChanceFrom(2) ? (SocialEquality == 0 ? BuildingInfo.MineSlvMedium : BuildingInfo.MineMedium) : (SocialEquality == 0 ? BuildingInfo.MineSlvLarge : BuildingInfo.MineLarge);
                                break;
                            case SettlementSpeciality.Lumberjacks:
                                pProfile = Rnd.OneChanceFrom(2) ? (SocialEquality == 0 ? BuildingInfo.LumberjacksHutSlvMedium : BuildingInfo.LumberjacksHutMedium) : (SocialEquality == 0 ? BuildingInfo.LumberjacksHutSlvLarge : BuildingInfo.LumberjacksHutLarge);
                                break;
                            default:
                                pProfile = BuildingInfo.LoafersHut;
                                break;
                        }

                        cChances[pProfile] = (float)cChances.Count / 2 + 2;
                    }
                    break;
                case SettlementSize.Town:
                    {
                        if (pSettlement.Buildings.Count > 0)// && Rnd.OneChanceFrom(2))
                        {
                            BuildingInfo pGuard;
                            if (iInfrastructureLevel < 4)
                                pGuard = BuildingInfo.GuardBarracksSmall;
                            else if (iInfrastructureLevel == 8)
                                pGuard = BuildingInfo.EmergencyPostSmall;
                            else
                                pGuard = BuildingInfo.PoliceStationSmall;

                            cChances[pGuard] = (float)iControl * DominantCulture.GetTrait(Trait.Agression) / 4;

                            BuildingInfo pPrison;
                            if (iInfrastructureLevel < 4)
                                pPrison = BuildingInfo.PrisonSmall;
                            else if (iInfrastructureLevel == 8)
                                pPrison = BuildingInfo.HoldingSmall;
                            else
                                pPrison = BuildingInfo.PrisonPoliceSmall;

                            cChances[pPrison] = (float)iControl / 4;

                            cChances[BuildingInfo.MonasteryMedium] = DominantCulture.GetTrait(Trait.Piety) / 5;
                            cChances[BuildingInfo.TempleMedium] = DominantCulture.GetTrait(Trait.Piety) / 5;
                            cChances[BuildingInfo.TempleSmall] = DominantCulture.GetTrait(Trait.Piety);

                            BuildingInfo pBrothelProfile = SocialEquality == 0 ? BuildingInfo.BrothelSlvMedium : BuildingInfo.BrothelMedium;
                            BuildingInfo pStripClubProfile = SocialEquality == 0 ? BuildingInfo.StripClubSlvSmall : BuildingInfo.StripClubSmall;

                            if (DominantCulture.Customs.Has(Customs.Sexuality.Moderate_sexuality))
                            {
                                cChances[pBrothelProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 4;
                                if (iInfrastructureLevel >= 4)
                                    cChances[pStripClubProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 2;
                            }
                            else if (DominantCulture.Customs.Has(Customs.Sexuality.Lecherous))
                            {
                                cChances[pBrothelProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 2;
                                if (iInfrastructureLevel >= 4)
                                    cChances[pStripClubProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 2;
                            }

                            if (iInfrastructureLevel < 4)
                            {
                                cChances[SocialEquality == 0 ? BuildingInfo.InnSlvSmall : BuildingInfo.InnSmall] = DominantCulture.GetTrait(Trait.Simplicity) / 4;
                                cChances[SocialEquality == 0 ? BuildingInfo.TavernSlvSmall : BuildingInfo.TavernSmall] = DominantCulture.GetTrait(Trait.Simplicity);
                            }
                            else
                            {
                                cChances[SocialEquality == 0 ? BuildingInfo.HotelSlvMedium : BuildingInfo.HotelMedium] = DominantCulture.GetTrait(Trait.Simplicity) / 4;
                                cChances[SocialEquality == 0 ? BuildingInfo.BarSlvSmall : BuildingInfo.BarSmall] = DominantCulture.GetTrait(Trait.Simplicity);
                            }

                            cChances[BuildingInfo.TheatreMedium] = 1f - DominantCulture.GetTrait(Trait.Simplicity) / 2;

                            cChances[BuildingInfo.RoguesDenMedium] = DominantCulture.GetTrait(Trait.Treachery) / 2;
                            cChances[BuildingInfo.GamblingSmall] = DominantCulture.GetTrait(Trait.Treachery) / 2;

                            if (iInfrastructureLevel < 3)
                                cChances[BuildingInfo.MedicineSmall] = (float)cChances.Count / 12;
                            else
                                cChances[BuildingInfo.MedicineMedium] = (float)cChances.Count / 12;

                            float fBureaucracy = 0.05f;
                            if (DominantCulture.Customs.Has(Customs.MindSet.Balanced_mind))
                                fBureaucracy = 0.25f;
                            else if (DominantCulture.Customs.Has(Customs.MindSet.Logic))
                                fBureaucracy = 0.5f;

                            if (TitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Sapient)
                                fBureaucracy *= 2;
                            if (TitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Primitive)
                                fBureaucracy *= 4;

                            if (pSettlement.Capital)
                                fBureaucracy *= 2;

                            fBureaucracy *= (float)iControl / 2;

                            if (iInfrastructureLevel < 4)
                            {
                                cChances[BuildingInfo.CourtSmall] = fBureaucracy / 2;
                                cChances[BuildingInfo.CourtMedium] = fBureaucracy / 4;
                            }
                            else
                            {
                                cChances[BuildingInfo.AdministrationSmall] = fBureaucracy / 2;
                                cChances[BuildingInfo.AdministrationMedium] = fBureaucracy / 4;
                            }

                            if (iInfrastructureLevel >= 3)
                            {
                                float fScience = 0.05f;
                                if (DominantCulture.Customs.Has(Customs.MindSet.Balanced_mind))
                                    fScience = 0.25f;
                                else if (DominantCulture.Customs.Has(Customs.MindSet.Logic))
                                    fScience = 0.5f;

                                if (TitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Sapient)
                                    fScience *= 2;
                                if (TitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Ingenious)
                                    fScience *= 4;

                                cChances[BuildingInfo.ScienceSmall] = fScience / 4;

                                cChances[BuildingInfo.SchoolSmall] = fScience;
                                cChances[BuildingInfo.SchoolMedium] = fScience / 2;
                            }

                            if (SocialEquality == 0)
                            {
                                cChances[BuildingInfo.SlaveMarketMedium] = (float)cChances.Count / 6 + 1;
                                if (DominantCulture.Customs.Has(Customs.Sexuality.Moderate_sexuality))
                                {
                                    cChances[BuildingInfo.SlaveMarketMedium2] = (float)cChances.Count / 6 + 1;
                                }
                                else if (DominantCulture.Customs.Has(Customs.Sexuality.Lecherous))
                                {
                                    cChances[BuildingInfo.SlaveMarketMedium2] = (float)cChances.Count / 6 + 1;
                                }
                                //cChances[BuildingInfo.SlavePensLarge] = (float)cChances.Count;// / 2 + 1;
                            }

                            if (iInfrastructureLevel < 4)
                            {
                                cChances[BuildingInfo.MarketSmall] = (float)cChances.Count / 6 + 1;
                                cChances[BuildingInfo.MarketMedium] = (float)cChances.Count / 20 + 1;
                            }
                            else
                            {
                                cChances[BuildingInfo.OfficeSmall] = (float)cChances.Count / 2 + 1;
                                cChances[BuildingInfo.OfficeMedium] = (float)cChances.Count / 4;
                            }

                            if (Polity.HasDinasty)
                                cChances[SocialEquality == 0 ? BuildingInfo.MansionSlvSmall : BuildingInfo.MansionSmall] = pSettlement.Capital ? (float)cChances.Count / 2 : (float)cChances.Count / 4;
                        }

                        BuildingInfo pProfile;
                        switch (pSettlement.Speciality)
                        {
                            case SettlementSpeciality.Fishers:
                                pProfile = SocialEquality == 0 ? BuildingInfo.FishingBoatSlvLarge : BuildingInfo.FishingBoatLarge;
                                break;
                            case SettlementSpeciality.Naval:
                                pProfile = SocialEquality == 0 ? BuildingInfo.TraderSlvMedium : BuildingInfo.TraderMedium;
                                break;
                            case SettlementSpeciality.Tailors:
                                if (iInfrastructureLevel < 4)
                                    pProfile = SocialEquality == 0 ? BuildingInfo.TailorWorkshopSlvSmall : BuildingInfo.TailorWorkshopSmall;
                                else
                                    pProfile = SocialEquality == 0 ? BuildingInfo.ClothesFactorySlvSmall : BuildingInfo.ClothesFactorySmall;
                                break;
                            case SettlementSpeciality.Jevellers:
                                pProfile = SocialEquality == 0 ? BuildingInfo.JevellerWorkshopSlvSmall : BuildingInfo.JevellerWorkshopSmall;
                                break;
                            case SettlementSpeciality.Factory:
                                if (iInfrastructureLevel < 4)
                                    pProfile = SocialEquality == 0 ? BuildingInfo.SmithySlvSmall : BuildingInfo.SmithySmall;
                                else
                                    pProfile = SocialEquality == 0 ? BuildingInfo.IronworksSlvSmall : BuildingInfo.IronworksSmall;
                                break;
                            case SettlementSpeciality.Artisans:
                                if (iInfrastructureLevel < 4)
                                    pProfile = SocialEquality == 0 ? BuildingInfo.CarpentrySlvSmall : BuildingInfo.CarpentrySmall;
                                else
                                    pProfile = SocialEquality == 0 ? BuildingInfo.FurnitureSlvSmall : BuildingInfo.FurnitureSmall;
                                break;
                            default:
                                pProfile = BuildingInfo.LoafersHut;
                                break;
                        }

                        cChances[pProfile] = (float)cChances.Count / 2 + 1;
                    }
                    break;
                case SettlementSize.City:
                    {
                        if (pSettlement.Buildings.Count > 0)// && Rnd.OneChanceFrom(2))
                        {
                            BuildingInfo pGuard;
                            if (iInfrastructureLevel < 4)
                                pGuard = BuildingInfo.GuardBarracksMedium;
                            else if (iInfrastructureLevel == 8)
                                pGuard = BuildingInfo.EmergencyPostSmall;
                            else
                                pGuard = BuildingInfo.PoliceStationSmall;

                            cChances[pGuard] = (float)iControl * DominantCulture.GetTrait(Trait.Agression) / 4;

                            if (iInfrastructureLevel >= 4 && iInfrastructureLevel < 8)
                                cChances[BuildingInfo.PoliceStationMedium] = (float)iControl / 8;

                            BuildingInfo pPrison;
                            if (iInfrastructureLevel < 4)
                                pPrison = BuildingInfo.PrisonSmall;
                            else if (iInfrastructureLevel == 8)
                                pPrison = BuildingInfo.HoldingSmall;
                            else
                                pPrison = BuildingInfo.PrisonPoliceSmall;

                            cChances[pPrison] = (float)iControl / 4;

                            cChances[BuildingInfo.MonasteryMedium] = DominantCulture.GetTrait(Trait.Piety) / 5;
                            cChances[BuildingInfo.MonasteryLarge] = DominantCulture.GetTrait(Trait.Piety) / 10;
                            cChances[BuildingInfo.TempleMedium] = DominantCulture.GetTrait(Trait.Piety) / 5;
                            cChances[BuildingInfo.TempleSmall] = DominantCulture.GetTrait(Trait.Piety);

                            BuildingInfo pBrothelProfile = SocialEquality == 0 ? BuildingInfo.BrothelSlvMedium : BuildingInfo.BrothelMedium;
                            BuildingInfo pStripClubProfile = SocialEquality == 0 ? BuildingInfo.StripClubSlvSmall : BuildingInfo.StripClubSmall;

                            if (DominantCulture.Customs.Has(Customs.Sexuality.Moderate_sexuality))
                            {
                                cChances[pBrothelProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 4;
                                if (iInfrastructureLevel >= 4)
                                    cChances[pStripClubProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 2;
                            }
                            else if (DominantCulture.Customs.Has(Customs.Sexuality.Lecherous))
                            {
                                cChances[pBrothelProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 2;
                                if (iInfrastructureLevel >= 4)
                                    cChances[pStripClubProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 2;
                            }

                            if (iInfrastructureLevel < 4)
                            {
                                cChances[SocialEquality == 0 ? BuildingInfo.InnSlvSmall : BuildingInfo.InnSmall] = DominantCulture.GetTrait(Trait.Simplicity) / 4;
                                cChances[SocialEquality == 0 ? BuildingInfo.TavernSlvSmall : BuildingInfo.TavernSmall] = DominantCulture.GetTrait(Trait.Simplicity);
                                cChances[BuildingInfo.CircusMedium] = DominantCulture.GetTrait(Trait.Simplicity) * DominantCulture.GetTrait(Trait.Agression) / 2;
                            }
                            else
                            {
                                cChances[SocialEquality == 0 ? BuildingInfo.HotelSlvMedium : BuildingInfo.HotelMedium] = DominantCulture.GetTrait(Trait.Simplicity) / 4;
                                cChances[SocialEquality == 0 ? BuildingInfo.BarSlvSmall : BuildingInfo.BarSmall] = DominantCulture.GetTrait(Trait.Simplicity);
                                cChances[SocialEquality == 0 ? BuildingInfo.NightClubSlvMedium : BuildingInfo.NightClubMedium] = DominantCulture.GetTrait(Trait.Simplicity);
                            }

                            cChances[BuildingInfo.TheatreMedium] = 1f - DominantCulture.GetTrait(Trait.Simplicity) / 2;

                            cChances[BuildingInfo.RoguesDenMedium] = DominantCulture.GetTrait(Trait.Treachery) / 2;
                            cChances[BuildingInfo.GamblingSmall] = DominantCulture.GetTrait(Trait.Treachery) / 2;

                            if (iInfrastructureLevel < 3)
                            {
                                cChances[BuildingInfo.MedicineSmall] = (float)cChances.Count / 12;
                            }
                            else
                            {
                                cChances[BuildingInfo.MedicineMedium] = (float)cChances.Count / 12;
                                cChances[BuildingInfo.MedicineLarge] = (float)cChances.Count / 24;
                            }

                            float fBureaucracy = 0.05f;
                            if (DominantCulture.Customs.Has(Customs.MindSet.Balanced_mind))
                                fBureaucracy = 0.25f;
                            else if (DominantCulture.Customs.Has(Customs.MindSet.Logic))
                                fBureaucracy = 0.5f;

                            if (TitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Sapient)
                                fBureaucracy *= 2;
                            if (TitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Primitive)
                                fBureaucracy *= 4;

                            if (pSettlement.Capital)
                                fBureaucracy *= 2;

                            fBureaucracy *= (float)iControl / 2;

                            if (iInfrastructureLevel < 4)
                            {
                                cChances[BuildingInfo.CourtSmall] = fBureaucracy / 2;
                                cChances[BuildingInfo.CourtMedium] = fBureaucracy / 4;
                            }
                            else
                            {
                                cChances[BuildingInfo.AdministrationSmall] = fBureaucracy / 2;
                                cChances[BuildingInfo.AdministrationMedium] = fBureaucracy / 4;
                            }

                            if (iInfrastructureLevel >= 3)
                            {
                                float fScience = 0.05f;
                                if (DominantCulture.Customs.Has(Customs.MindSet.Balanced_mind))
                                    fScience = 0.25f;
                                else if (DominantCulture.Customs.Has(Customs.MindSet.Logic))
                                    fScience = 0.5f;

                                if (TitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Sapient)
                                    fScience *= 2;
                                if (TitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Ingenious)
                                    fScience *= 4;

                                cChances[BuildingInfo.ScienceSmall] = fScience / 4;
                                cChances[BuildingInfo.ScienceMedium] = fScience / 8;

                                cChances[BuildingInfo.SchoolSmall] = fScience;
                                cChances[BuildingInfo.SchoolMedium] = fScience / 2;
                            }

                            if (SocialEquality == 0)
                            {
                                cChances[BuildingInfo.SlaveMarketMedium] = (float)cChances.Count / 6 + 1;
                                if (DominantCulture.Customs.Has(Customs.Sexuality.Moderate_sexuality))
                                {
                                    cChances[BuildingInfo.SlaveMarketMedium2] = (float)cChances.Count / 6 + 1;
                                }
                                else if (DominantCulture.Customs.Has(Customs.Sexuality.Lecherous))
                                {
                                    cChances[BuildingInfo.SlaveMarketMedium2] = (float)cChances.Count / 4 + 1;
                                }
                                //cChances[BuildingInfo.SlavePensHuge] = (float)cChances.Count;// / 2 + 1;
                            }

                            if (iInfrastructureLevel < 4)
                            {
                                //cChances[BuildingInfo.MarketSmall] = cChances.Count;// / 2 + 1;
                                cChances[BuildingInfo.MarketMedium] = (float)cChances.Count / 10 + 1;
                            }
                            else
                            {
                                //cChances[BuildingInfo.OfficeSmall] = cChances.Count;// / 2 + 1;
                                cChances[BuildingInfo.OfficeMedium] = (float)cChances.Count / 2 + 1;
                                cChances[BuildingInfo.OfficeLarge] = (float)cChances.Count / 4;// / 2 + 1;
                            }

                            if (Polity.HasDinasty)
                            {
                                cChances[SocialEquality == 0 ? BuildingInfo.MansionSlvSmall : BuildingInfo.MansionSmall] = pSettlement.Capital ? (float)cChances.Count / 2 : (float)cChances.Count / 4;
                                cChances[SocialEquality == 0 ? BuildingInfo.MansionSlvMedium : BuildingInfo.MansionMedium] = pSettlement.Capital ? (float)cChances.Count / 2 : (float)cChances.Count / 4;
                            }
                        }

                        BuildingInfo pProfile;
                        switch (pSettlement.Speciality)
                        {
                            case SettlementSpeciality.NavalAcademy:
                                pProfile = Rnd.OneChanceFrom(2) ? BuildingInfo.NavalAcademyHuge : (SocialEquality == 0 ? BuildingInfo.TraderSlvLarge : BuildingInfo.TraderLarge);
                                break;
                            case SettlementSpeciality.Naval:
                                pProfile = SocialEquality == 0 ? BuildingInfo.TraderSlvLarge : BuildingInfo.TraderLarge;
                                break;
                            case SettlementSpeciality.Resort:
                                pProfile = Rnd.OneChanceFrom(2) ? (SocialEquality == 0 ? BuildingInfo.HotelSlvMedium : BuildingInfo.HotelMedium) : (SocialEquality == 0 ? BuildingInfo.HotelSlvLarge : BuildingInfo.HotelLarge);
                                break;
                            case SettlementSpeciality.Cultural:
                                pProfile = Rnd.OneChanceFrom(2) ? BuildingInfo.CultureMeduim : BuildingInfo.CultureLarge;
                                break;
                            case SettlementSpeciality.ArtsAcademy:
                                pProfile = iInfrastructureLevel < 4 ? (DominantCulture.GetTrait(Trait.Simplicity) > Rnd.Get(2f) ? BuildingInfo.CircusMedium : BuildingInfo.TheatreMedium) : BuildingInfo.CinemaLarge;
                                break;
                            case SettlementSpeciality.Religious:
                                pProfile = BuildingInfo.TempleLarge;
                                break;
                            case SettlementSpeciality.MilitaryAcademy:
                                pProfile = BuildingInfo.BarracksHuge;
                                break;
                            case SettlementSpeciality.Gambling:
                                pProfile = BuildingInfo.GamblingMedium;
                                break;
                            case SettlementSpeciality.SciencesAcademy:
                                pProfile = Rnd.OneChanceFrom(2) ? BuildingInfo.ScienceMedium : BuildingInfo.ScienceLarge;
                                break;
                            case SettlementSpeciality.Tailors:
                                pProfile = SocialEquality == 0 ? BuildingInfo.ClothesFactorySlvMedium : BuildingInfo.ClothesFactoryMedium;
                                break;
                            case SettlementSpeciality.Jevellers:
                                pProfile = SocialEquality == 0 ? BuildingInfo.JevellerWorkshopSlvMedium : BuildingInfo.JevellerWorkshopMedium;
                                break;
                            case SettlementSpeciality.Factory:
                                pProfile = SocialEquality == 0 ? BuildingInfo.IronworksSlvMedium : BuildingInfo.IronworksMedium;
                                break;
                            case SettlementSpeciality.Artisans:
                                pProfile = SocialEquality == 0 ? BuildingInfo.FurnitureSlvMedium : BuildingInfo.FurnitureMedium;
                                break;
                            default:
                                pProfile = BuildingInfo.LoafersHut;
                                break;
                        }

                        cChances[pProfile] = (float)cChances.Count / 2 + 1;
                    }
                    break;
                case SettlementSize.Capital:
                    {
                        if (pSettlement.Buildings.Count > 0)// && Rnd.OneChanceFrom(2))
                        {
                            BuildingInfo pGuard;
                            if (iInfrastructureLevel < 4)
                                pGuard = BuildingInfo.GuardBarracksMedium;
                            else if (iInfrastructureLevel == 8)
                                pGuard = BuildingInfo.EmergencyPostSmall;
                            else
                                pGuard = BuildingInfo.PoliceStationSmall;

                            cChances[pGuard] = (float)iControl * DominantCulture.GetTrait(Trait.Agression) / 4;

                            if (iInfrastructureLevel >= 4 && iInfrastructureLevel < 8)
                                cChances[BuildingInfo.PoliceStationMedium] = (float)iControl / 8;

                            BuildingInfo pPrison;
                            if (iInfrastructureLevel < 4)
                                pPrison = BuildingInfo.PrisonMedium;
                            else if (iInfrastructureLevel == 8)
                                pPrison = BuildingInfo.HoldingMedium;
                            else
                                pPrison = BuildingInfo.PrisonPoliceMedium;

                            cChances[pPrison] = (float)iControl / 4;

                            cChances[BuildingInfo.MonasteryMedium] = DominantCulture.GetTrait(Trait.Piety) / 5;
                            cChances[BuildingInfo.MonasteryLarge] = DominantCulture.GetTrait(Trait.Piety) / 10;
                            cChances[BuildingInfo.TempleMedium] = DominantCulture.GetTrait(Trait.Piety) / 5;
                            cChances[BuildingInfo.TempleSmall] = DominantCulture.GetTrait(Trait.Piety);

                            BuildingInfo pBrothelProfile = SocialEquality == 0 ? BuildingInfo.BrothelSlvMedium : BuildingInfo.BrothelMedium;
                            BuildingInfo pStripClubProfile = SocialEquality == 0 ? BuildingInfo.StripClubSlvSmall : BuildingInfo.StripClubSmall;

                            if (DominantCulture.Customs.Has(Customs.Sexuality.Moderate_sexuality))
                            {
                                cChances[pBrothelProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 4;
                                if (iInfrastructureLevel >= 4)
                                    cChances[pStripClubProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 2;
                            }
                            else if (DominantCulture.Customs.Has(Customs.Sexuality.Lecherous))
                            {
                                cChances[pBrothelProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 2;
                                if (iInfrastructureLevel >= 4)
                                    cChances[pStripClubProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 2;
                            }

                            if (iInfrastructureLevel < 4)
                            {
                                cChances[SocialEquality == 0 ? BuildingInfo.InnSlvSmall : BuildingInfo.InnSmall] = DominantCulture.GetTrait(Trait.Simplicity) / 4;
                                cChances[SocialEquality == 0 ? BuildingInfo.TavernSlvSmall : BuildingInfo.TavernSmall] = DominantCulture.GetTrait(Trait.Simplicity);
                                cChances[BuildingInfo.CircusMedium] = DominantCulture.GetTrait(Trait.Simplicity) * DominantCulture.GetTrait(Trait.Agression) / 2;
                            }
                            else
                            {
                                cChances[SocialEquality == 0 ? BuildingInfo.HotelSlvMedium : BuildingInfo.HotelMedium] = DominantCulture.GetTrait(Trait.Simplicity) / 4;
                                cChances[SocialEquality == 0 ? BuildingInfo.BarSlvSmall : BuildingInfo.BarSmall] = DominantCulture.GetTrait(Trait.Simplicity);
                                cChances[SocialEquality == 0 ? BuildingInfo.NightClubSlvMedium : BuildingInfo.NightClubMedium] = DominantCulture.GetTrait(Trait.Simplicity);
                            }

                            cChances[BuildingInfo.TheatreMedium] = 1f - DominantCulture.GetTrait(Trait.Simplicity) / 2;

                            cChances[BuildingInfo.RoguesDenMedium] = DominantCulture.GetTrait(Trait.Treachery) / 2;
                            cChances[BuildingInfo.GamblingSmall] = DominantCulture.GetTrait(Trait.Treachery) / 2;

                            if (iInfrastructureLevel < 3)
                            {
                                cChances[BuildingInfo.MedicineSmall] = (float)cChances.Count / 12;
                            }
                            else
                            {
                                cChances[BuildingInfo.MedicineMedium] = (float)cChances.Count / 12;
                                cChances[BuildingInfo.MedicineLarge] = (float)cChances.Count / 24;
                            }

                            float fBureaucracy = 0.05f;
                            if (DominantCulture.Customs.Has(Customs.MindSet.Balanced_mind))
                                fBureaucracy = 0.25f;
                            if (DominantCulture.Customs.Has(Customs.MindSet.Logic))
                                fBureaucracy = 0.5f;

                            if (TitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Sapient)
                                fBureaucracy *= 2;
                            if (TitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Primitive)
                                fBureaucracy *= 4;

                            if (pSettlement.Capital)
                                fBureaucracy *= 2;

                            fBureaucracy *= (float)iControl / 2;

                            if (iInfrastructureLevel < 4)
                            {
                                cChances[BuildingInfo.CourtSmall] = fBureaucracy / 2;
                                cChances[BuildingInfo.CourtMedium] = fBureaucracy / 4;
                            }
                            else
                            {
                                cChances[BuildingInfo.AdministrationSmall] = fBureaucracy / 2;
                                cChances[BuildingInfo.AdministrationMedium] = fBureaucracy / 4;
                            }

                            if (iInfrastructureLevel >= 3)
                            {
                                float fScience = 0.05f;
                                if (DominantCulture.Customs.Has(Customs.MindSet.Balanced_mind))
                                    fScience = 0.25f;
                                else if (DominantCulture.Customs.Has(Customs.MindSet.Logic))
                                    fScience = 0.5f;

                                if (TitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Sapient)
                                    fScience *= 2;
                                if (TitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Ingenious)
                                    fScience *= 4;

                                cChances[BuildingInfo.ScienceSmall] = fScience / 4;
                                cChances[BuildingInfo.ScienceMedium] = fScience / 8;

                                cChances[BuildingInfo.SchoolSmall] = fScience;
                                cChances[BuildingInfo.SchoolMedium] = fScience / 2;
                            }

                            if (SocialEquality == 0)
                            {
                                cChances[BuildingInfo.SlaveMarketMedium] = (float)cChances.Count / 6 + 1;
                                if (DominantCulture.Customs.Has(Customs.Sexuality.Moderate_sexuality))
                                {
                                    cChances[BuildingInfo.SlaveMarketMedium2] = (float)cChances.Count / 6 + 1;
                                }
                                if (DominantCulture.Customs.Has(Customs.Sexuality.Lecherous))
                                {
                                    cChances[BuildingInfo.SlaveMarketMedium2] = (float)cChances.Count / 4 + 1;
                                }
                                //cChances[BuildingInfo.SlavePensHuge] = (float)cChances.Count;// / 2 + 1;
                            }

                            if (iInfrastructureLevel < 4)
                            {
                                //cChances[BuildingInfo.MarketSmall] = cChances.Count;// / 2 + 1;
                                cChances[BuildingInfo.MarketMedium] = (float)cChances.Count / 10 + 1;
                            }
                            else
                            {
                                //cChances[BuildingInfo.OfficeSmall] = cChances.Count;// / 2 + 1;
                                cChances[BuildingInfo.OfficeMedium] = (float)cChances.Count / 2 + 1;
                                cChances[BuildingInfo.OfficeLarge] = (float)cChances.Count / 4;// / 2 + 1;
                            }

                            if (Polity.HasDinasty)
                            {
                                cChances[SocialEquality == 0 ? BuildingInfo.MansionSlvSmall : BuildingInfo.MansionSmall] = pSettlement.Capital ? (float)cChances.Count / 2 : (float)cChances.Count / 4;
                                cChances[SocialEquality == 0 ? BuildingInfo.MansionSlvMedium : BuildingInfo.MansionMedium] = pSettlement.Capital ? (float)cChances.Count / 2 : (float)cChances.Count / 4;
                            }
                        }

                        BuildingInfo pProfile;
                        switch (pSettlement.Speciality)
                        {
                            case SettlementSpeciality.NavalAcademy:
                                pProfile = Rnd.OneChanceFrom(2) ? BuildingInfo.NavalAcademyHuge : (SocialEquality == 0 ? BuildingInfo.TraderSlvLarge : BuildingInfo.TraderLarge);
                                break;
                            case SettlementSpeciality.Naval:
                                pProfile = SocialEquality == 0 ? BuildingInfo.TraderSlvLarge : BuildingInfo.TraderLarge;
                                break;
                            case SettlementSpeciality.Resort:
                                pProfile = Rnd.OneChanceFrom(2) ? (SocialEquality == 0 ? BuildingInfo.HotelSlvMedium : BuildingInfo.HotelMedium) : (SocialEquality == 0 ? BuildingInfo.HotelSlvLarge : BuildingInfo.HotelLarge);
                                break;
                            case SettlementSpeciality.Cultural:
                                pProfile = Rnd.OneChanceFrom(2) ? BuildingInfo.CultureMeduim : BuildingInfo.CultureLarge;
                                break;
                            case SettlementSpeciality.ArtsAcademy:
                                pProfile = iInfrastructureLevel < 4 ? (DominantCulture.GetTrait(Trait.Simplicity) > Rnd.Get(2f) ? BuildingInfo.CircusMedium : BuildingInfo.TheatreMedium) : BuildingInfo.CinemaLarge;
                                break;
                            case SettlementSpeciality.Religious:
                                pProfile = BuildingInfo.TempleLarge;
                                break;
                            case SettlementSpeciality.MilitaryAcademy:
                                pProfile = BuildingInfo.BarracksHuge;
                                break;
                            case SettlementSpeciality.Gambling:
                                pProfile = BuildingInfo.GamblingMedium;
                                break;
                            case SettlementSpeciality.SciencesAcademy:
                                pProfile = Rnd.OneChanceFrom(2) ? BuildingInfo.ScienceMedium : BuildingInfo.ScienceLarge;
                                break;
                            case SettlementSpeciality.Tailors:
                                pProfile = SocialEquality == 0 ? BuildingInfo.ClothesFactorySlvMedium : BuildingInfo.ClothesFactoryMedium;
                                break;
                            case SettlementSpeciality.Jevellers:
                                pProfile = SocialEquality == 0 ? BuildingInfo.JevellerWorkshopSlvMedium : BuildingInfo.JevellerWorkshopMedium;
                                break;
                            case SettlementSpeciality.Factory:
                                pProfile = SocialEquality == 0 ? BuildingInfo.IronworksSlvSmall : BuildingInfo.IronworksMedium;
                                break;
                            case SettlementSpeciality.Artisans:
                                pProfile = SocialEquality == 0 ? BuildingInfo.FurnitureSlvMedium : BuildingInfo.FurnitureMedium;
                                break;
                            default:
                                pProfile = BuildingInfo.LoafersHut;
                                break;
                        }

                        cChances[pProfile] = (float)cChances.Count / 2 + 1;
                    }
                    break;
                case SettlementSize.Fort:
                    {
                        if (pSettlement.Buildings.Count > 0)// && Rnd.OneChanceFrom(2))
                        {
                            cChances[BuildingInfo.TempleMedium] = DominantCulture.GetTrait(Trait.Piety) / 5;
                            cChances[BuildingInfo.TempleSmall] = DominantCulture.GetTrait(Trait.Piety);

                            BuildingInfo pBrothelProfile = SocialEquality == 0 ? BuildingInfo.BrothelSlvMedium : BuildingInfo.BrothelMedium;
                            BuildingInfo pStripClubProfile = SocialEquality == 0 ? BuildingInfo.StripClubSlvSmall : BuildingInfo.StripClubSmall;

                            if (DominantCulture.Customs.Has(Customs.Sexuality.Moderate_sexuality))
                            {
                                cChances[pBrothelProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 4;
                                if (iInfrastructureLevel >= 4)
                                    cChances[pStripClubProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 2;
                            }
                            if (DominantCulture.Customs.Has(Customs.Sexuality.Lecherous))
                            {
                                cChances[pBrothelProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 2;
                                if (iInfrastructureLevel >= 4)
                                    cChances[pStripClubProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 2;
                            }

                            if (iInfrastructureLevel < 4)
                                cChances[SocialEquality == 0 ? BuildingInfo.TavernSlvSmall : BuildingInfo.TavernSmall] = DominantCulture.GetTrait(Trait.Simplicity) / 2;
                            else
                                cChances[SocialEquality == 0 ? BuildingInfo.BarSlvSmall : BuildingInfo.BarSmall] = DominantCulture.GetTrait(Trait.Simplicity) / 2;

                            //if (pState.m_iSocialEquality == 0)
                            //{
                            //    cChances[BuildingInfo.SlavePensMedium] = (float)cChances.Count / 2;
                            //}
                        }

                        BuildingInfo pProfile;
                        switch (pSettlement.Speciality)
                        {
                            case SettlementSpeciality.Pirates:
                                pProfile = BuildingInfo.PirateShip;
                                break;
                            case SettlementSpeciality.Naval:
                                pProfile = BuildingInfo.NavalVessel;
                                break;
                            case SettlementSpeciality.Raiders:
                                pProfile = BuildingInfo.BanditsBarracks;
                                break;
                            case SettlementSpeciality.Military:
                                pProfile = BuildingInfo.BarracksSmall;
                                break;
                            default:
                                pProfile = BuildingInfo.LoafersHut;
                                break;
                        }

                        cChances[pProfile] = (float)cChances.Count + 1;
                    }
                    break;
            }

            foreach (BuildingInfo pBuildingInfo in pSettlement.Buildings.Select(x => x.Info))
            {
                if (cChances.ContainsKey(pBuildingInfo))
                {
                    float fKoeff = 1;
                    switch (pBuildingInfo.Size)
                    {
                        case BuildingSize.Small:
                            fKoeff = 0.75f;
                            break;
                        case BuildingSize.Medium:
                            fKoeff = 0.5f;
                            break;
                        case BuildingSize.Large:
                            fKoeff = 0.25f;
                            break;
                        case BuildingSize.Huge:
                            fKoeff = 0.125f;
                            break;
                    }
                    cChances[pBuildingInfo] *= fKoeff;
                }
            }

            int iChance = Rnd.ChooseOne(cChances.Values);
            return cChances.ElementAt(iChance).Key;
        }

        internal void CalculateSocietyFeatures(int iEmpireTreshold)
        {
            // Adjustiong TL due to lack or abundance of resouces
            CheckResources(m_pState.Resources, m_pState.LocationsCount, m_pState.Contents.Count);

            // Choose state system
            SelectGovernmentSystem(iEmpireTreshold);

            // Set social equality level
            SetSocialEquality();

            // Set state control level
            SetStateControl();

            Person.GetSkillPreferences(DominantCulture, ref m_eMostRespectedSkill, ref m_eLeastRespectedSkill);
        }

        private void SelectGovernmentSystem(int iEmpireTreshold)
        {
            List<PolityInfo> cInfos = new List<PolityInfo>();

            foreach (PolityInfo pInfo in s_aPolityInfo)
            {
                if (InfrastructureLevel >= pInfo.MinGovernmentLevel &&
                    InfrastructureLevel <= pInfo.MaxGovernmentLevel &&
                    (pInfo.Languages.Count == 0 ||
                     pInfo.Languages.Contains(TitularNation.Race.Language)) &&
                    (m_pState.LocationsCount > iEmpireTreshold * 80) == pInfo.IsEmpire)
                {
                    for (int i = 0; i < pInfo.Rank; i++)
                        cInfos.Add(pInfo);
                }
            }

            if (cInfos.Count == 0)
            {
                foreach (PolityInfo pInfo in s_aPolityInfo)
                {
                    if (InfrastructureLevel >= pInfo.MinGovernmentLevel &&
                        InfrastructureLevel <= pInfo.MaxGovernmentLevel &&
                        (m_pState.LocationsCount > iEmpireTreshold * 80) == pInfo.IsEmpire)
                    {
                        for (int i = 0; i < pInfo.Rank; i++)
                            cInfos.Add(pInfo);
                    }
                }
            }

            //int iOldLevel = m_pMethropoly.m_iInfrastructureLevel;// Math.Max(m_pRace.m_iTechLevel, m_pRace.m_iMagicLimit / 2);
            //int iNewLevel = m_iInfrastructureLevel;// Math.Max(m_iTechLevel, iAverageMagicLimit / 2);
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

            Polity = cInfos[Rnd.Get(cInfos.Count)];

            Culture[Gender.Male].ProgressLevel = (InfrastructureLevel + Polity.MinGovernmentLevel) / 2;
            Culture[Gender.Female].ProgressLevel = (InfrastructureLevel + Polity.MinGovernmentLevel) / 2;
        }

        private void SetSocialEquality()
        {
            SocialEquality = 2;

            if (DominantCulture.GetTrait(Trait.Agression) > 1.66)
                SocialEquality--;

            if (DominantCulture.GetTrait(Trait.Fanaticism) > 1.66)
                SocialEquality--;
            if (DominantCulture.GetTrait(Trait.Fanaticism) > 1.33)
                SocialEquality--;

            if (DominantCulture.GetTrait(Trait.Selfishness) > 1.66)
                SocialEquality--;
            if (DominantCulture.GetTrait(Trait.Selfishness) > 1.33)
                SocialEquality--;

            if (DominantCulture.GetTrait(Trait.Treachery) > 1.66)
                SocialEquality--;
            if (DominantCulture.GetTrait(Trait.Treachery) > 1.33)
                SocialEquality--;

            if (Polity.HasDinasty)
                SocialEquality--;

            if (SocialEquality < 0)
                SocialEquality = 0;

            if (DominantCulture.GetTrait(Trait.Agression) < 1)
                SocialEquality++;

            if (DominantCulture.GetTrait(Trait.Fanaticism) < 1)
                SocialEquality++;

            if (SocialEquality > 0 && m_pState.FoodAvailable < m_pState.LocationsCount)
                SocialEquality--;
            if (m_pState.FoodAvailable > m_pState.LocationsCount &&
                m_pState.Resources[LandResource.Ore] > m_pState.LocationsCount &&
                m_pState.Resources[LandResource.Wood] > m_pState.LocationsCount)
            {
                SocialEquality++;
            }

            //в либеральном обществе (фанатизм < 2/3) не может быть рабства (0) или крепостного права (1), т.е. только 2 и выше
            if (DominantCulture.GetTrait(Trait.Fanaticism) < 0.66)
                SocialEquality = Math.Max(2, SocialEquality);
            //в обществе абсолютных пацифистов (агрессивность < 1/3) не может быть даже капитализма (2), т.е. только 3 и выше
            if (DominantCulture.GetTrait(Trait.Agression) < 0.33)
                SocialEquality = Math.Max(3, SocialEquality);
            //в обществе абсолютного самоотречения (эгоизм < 1/3) не может быть капитализма (2) - только или социализм, или феодализм
            if (DominantCulture.GetTrait(Trait.Selfishness) < 0.33)
            {
                if (Polity.HasDinasty)
                    SocialEquality = Math.Min(1, SocialEquality);
                else
                    SocialEquality = Math.Max(3, SocialEquality);
            }
            //эгоизм и коммунизм не совместимы
            if (DominantCulture.GetTrait(Trait.Selfishness) > 1)
                SocialEquality = Math.Min(3, SocialEquality);
            //преступный склад ума и социализм не совместимы
            if (DominantCulture.GetTrait(Trait.Treachery) > 0.66)
                SocialEquality = Math.Min(2, SocialEquality);

            //коммунизм возможен только в условиях изобилия ресурсов
            if (m_pState.FoodAvailable < m_pState.LocationsCount * 2 ||
                m_pState.Resources[LandResource.Ore] < m_pState.LocationsCount * 2 ||
                m_pState.Resources[LandResource.Wood] < m_pState.LocationsCount * 2)
            {
                SocialEquality = Math.Min(3, SocialEquality);
            }

            //при всём уважении - какой нафиг социализм/коммунизм при наследственной власти???
            if (Polity.HasDinasty)
                SocialEquality = Math.Min(2, SocialEquality);

            if (SocialEquality > 4)
                SocialEquality = 4;
        }

        private void SetStateControl()
        {
            Control = 2;

            if (Polity.HasDinasty)
                Control++;
            if (DominantCulture.GetTrait(Trait.Fanaticism) > 1.33)
                Control++;
            if (DominantCulture.GetTrait(Trait.Fanaticism) > 1.66)
                Control++;
            if (DominantCulture.GetTrait(Trait.Fanaticism) < 0.33)
                Control--;

            if (DominantCulture.GetTrait(Trait.Selfishness) > 1.66)
                Control--;

            if (InfrastructureLevel == 0)
                Control = 0;
            if (Control == 0 && InfrastructureLevel >= 1 && InfrastructureLevel <= 6)
                Control = 1;

            if (Control < 0)
                Control = 0;
            if (Control > 4)
                Control = 4;
        }

        public void CalculateMagic()
        {
            MagicLimit = 0;

            Dictionary<Gender, float[]> aDistribution = new Dictionary<Gender, float[]>
            {
                {Gender.Male, new float[10] },
                {Gender.Female, new float[10] }
            };

            foreach (Province pProvince in m_pState.Contents)
            {
                if (pProvince.LocalSociety.MagicLimit > MagicLimit)
                    MagicLimit = pProvince.LocalSociety.MagicLimit;

                float fPrevalence = 1;
                if (pProvince.LocalSociety.DominantCulture.Customs.Has(Customs.Magic.Magic_Feared))
                {
                    fPrevalence = 0.1f;
                }
                else if (pProvince.LocalSociety.DominantCulture.Customs.Has(Customs.Magic.Magic_Allowed))
                {
                    fPrevalence = 0.5f;
                }
                else if (pProvince.LocalSociety.DominantCulture.Customs.Has(Customs.Magic.Magic_Praised))
                {
                    fPrevalence = 0.9f;
                }

                Dictionary<Gender, float> cProvinceMagesCount = new Dictionary<Gender, float>()
                {
                    { Gender.Male, 0 },
                    { Gender.Female, 0 }
                };
                foreach (Region pRegion in pProvince.Contents)
                {
                    foreach (var pLocationsCount in pRegion.Contents.Select(x => x.Origin.Contents.Count))
                    {
                        cProvinceMagesCount[Gender.Male] += pLocationsCount * fPrevalence;
                        cProvinceMagesCount[Gender.Female] += pLocationsCount * fPrevalence;
                    }
                }

                switch (pProvince.LocalSociety.TitularNation.PhenotypeMale.m_pValues.Get<LifeCycleGenetix>().BirthRate)
                {
                    case BirthRate.Low:
                        cProvinceMagesCount[Gender.Male] *= 0.1f;
                        break;
                    case BirthRate.Moderate:
                        cProvinceMagesCount[Gender.Male] *= 0.25f;
                        break;
                }

                switch (pProvince.LocalSociety.TitularNation.PhenotypeFemale.m_pValues.Get<LifeCycleGenetix>().BirthRate)
                {
                    case BirthRate.Low:
                        cProvinceMagesCount[Gender.Female] *= 0.1f;
                        break;
                    case BirthRate.Moderate:
                        cProvinceMagesCount[Gender.Female] *= 0.25f;
                        break;
                }

                foreach (var distribution in aDistribution)
                {
                    switch (pProvince.LocalSociety.Culture[distribution.Key].MagicAbilityDistribution)
                    {
                        case MagicAbilityDistribution.mostly_weak:
                            distribution.Value[(1 + pProvince.LocalSociety.MagicLimit) / 2] += cProvinceMagesCount[distribution.Key];
                            break;
                        case MagicAbilityDistribution.mostly_average:
                            distribution.Value[(1 + pProvince.LocalSociety.MagicLimit) / 2] += cProvinceMagesCount[distribution.Key] / 2;
                            distribution.Value[1 + pProvince.LocalSociety.MagicLimit] += cProvinceMagesCount[distribution.Key] / 2;
                            break;
                        case MagicAbilityDistribution.mostly_powerful:
                            distribution.Value[1 + pProvince.LocalSociety.MagicLimit] += cProvinceMagesCount[distribution.Key];
                            break;
                    }
                }
            }

            foreach (var distribution in aDistribution)
            {
                float fWeakMagesCount = 0;
                float fPowerfulMagesCount = 0;
                for (int i = 0; i < 10; i++)
                {
                    if (i <= (MagicLimit + 1) / 2)
                        fWeakMagesCount += distribution.Value[i];
                    else
                        fPowerfulMagesCount += distribution.Value[i];
                }

                Culture[distribution.Key].MagicAbilityDistribution = MagicAbilityDistribution.mostly_average;

                if (fWeakMagesCount > fPowerfulMagesCount * 2)
                    Culture[distribution.Key].MagicAbilityDistribution = MagicAbilityDistribution.mostly_weak;

                if (fPowerfulMagesCount > fWeakMagesCount * 2)
                    Culture[distribution.Key].MagicAbilityDistribution = MagicAbilityDistribution.mostly_powerful;
            }
        }

        /// <summary>
        /// Негативное отношение к другому государству. От 2 (ненависть) до -2 (дружба)
        /// </summary>
        /// <param name="pOpponent"></param>
        /// <returns></returns>
        public int CalcHostility(State pOpponent)
        {
            return CalcHostility(pOpponent, out _);
        }

        /// <summary>
        /// Негативное отношение к другому государству. От 2 (ненависть) до -2 (дружба)
        /// </summary>
        /// <param name="pOpponent"></param>
        /// <returns></returns>
        public int CalcHostility(State pOpponent, out string sReasons)
        {
            int iHostility = 0;

            StringBuilder sPositiveReasons = new StringBuilder();
            StringBuilder sNegativeReasons = new StringBuilder();

            //sReasons = "";

            //float fContact = 0;
            //float fBorder = 0;
            //foreach (Territory pTerr in BorderWith.Keys)
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

            if (TitularNation != pOpponent.Society.TitularNation)
            {
                iHostility++;
                sNegativeReasons.Append(" (-1) ").AppendLine(pOpponent.Society.TitularNation.ToString());

                if (TitularNation.Race.Language != pOpponent.Society.TitularNation.Race.Language)
                {
                    iHostility++;
                    sNegativeReasons.AppendLine(" (-1) Different language");
                }
            }
            else
            {
                iHostility--;
                sPositiveReasons.Append(" (+1) ").AppendLine(pOpponent.Society.TitularNation.ToString());
            }

            iHostility += DominantCulture.Customs.GetDifference(pOpponent.Society.DominantCulture.Customs, ref sPositiveReasons, ref sNegativeReasons);

            if (m_pState.FoodAvailable < m_pState.LocationsCount && pOpponent.FoodAvailable > pOpponent.LocationsCount * 2)
            {
                iHostility++;
                sNegativeReasons.AppendLine(" (-1) Envy for food");
            }
            if (m_pState.Resources[LandResource.Wood] < m_pState.LocationsCount && pOpponent.Resources[LandResource.Wood] > pOpponent.LocationsCount * 2)
            {
                iHostility++;
                sNegativeReasons.AppendLine(" (-1) Envy for wood");
            }
            if (m_pState.Resources[LandResource.Ore] < m_pState.LocationsCount && pOpponent.Resources[LandResource.Ore] > pOpponent.LocationsCount * 2)
            {
                iHostility++;
                sNegativeReasons.AppendLine(" (-1) Envy for ore");
            }

            int iControlDifference = Math.Abs(pOpponent.Society.Control - Control);
            if (iControlDifference != 1)
            {
                iHostility += iControlDifference - 1;
                if (iControlDifference > 1)
                    sNegativeReasons.AppendFormat(" (-{1}) {0}", GetControlString(pOpponent.Society.Control), iControlDifference - 1).AppendLine();
                else
                    sPositiveReasons.AppendFormat(" (+{1}) {0}", GetControlString(pOpponent.Society.Control), 1).AppendLine();
            }

            if (pOpponent.Society.InfrastructureLevel > InfrastructureLevel + 1)
            {
                iHostility++;
                sNegativeReasons.AppendFormat(" (-{0}) Envy for civilization", 1).AppendLine();
            }
            else
            {
                if (pOpponent.Society.InfrastructureLevel < InfrastructureLevel - 1)
                {
                    iHostility++;
                    sNegativeReasons.AppendFormat(" (-{0}) Scorn for savagery", 1).AppendLine();
                }
            }

            int iEqualityDifference = Math.Abs(pOpponent.Society.SocialEquality - SocialEquality);
            if (iEqualityDifference != 1)
            {
                iHostility += iEqualityDifference - 1;
                if (iEqualityDifference > 1)
                    sNegativeReasons.AppendFormat(" (-{1}) {0}", GetEqualityString(pOpponent.Society.SocialEquality), iEqualityDifference - 1).AppendLine();
                else
                    sPositiveReasons.AppendFormat(" (+{1}) {0}", GetEqualityString(pOpponent.Society.SocialEquality), 1).AppendLine();
            }

            float iCultureDifference = DominantCulture.Mentality.GetDifference(pOpponent.Society.DominantCulture.Mentality, DominantCulture.ProgressLevel, pOpponent.Society.DominantCulture.ProgressLevel);
            if (iCultureDifference < -0.75)
            {
                iHostility -= 2;
                sPositiveReasons.AppendLine(" (+2) Very close culture");
            }
            else if (iCultureDifference < -0.5)
            {
                iHostility--;
                sPositiveReasons.AppendLine(" (+1) Close culture");
            }
            else if (iCultureDifference > 0.5)
            {
                iHostility += 2;
                sNegativeReasons.AppendLine(" (-2) Very different culture");
            }
            else if (iCultureDifference > 0)
            {
                iHostility++;
                sNegativeReasons.AppendLine(" (-1) Different culture");
            }

            StringBuilder sResult = new StringBuilder();
            sResult.AppendLine("Good:").Append(sPositiveReasons).AppendLine("Bad:").Append(sNegativeReasons).AppendLine("----");

            if (iHostility > 0)
            {
                iHostility = (int)(DominantCulture.GetTrait(Trait.Fanaticism) * iHostility + 0.25);
                sResult.AppendFormat("Fanaticism \t(x{0}%)", (int)(DominantCulture.GetTrait(Trait.Fanaticism) * 100)).AppendLine();

                iHostility = (int)(DominantCulture.GetTrait(Trait.Agression) * iHostility + 0.25);
                sResult.AppendFormat("Agression \t(x{0}%)", (int)(DominantCulture.GetTrait(Trait.Agression) * 100)).AppendLine();

                if (iHostility == 0)
                    iHostility = 1;
            }
            else
            {
                if (iHostility < 0)
                {
                    iHostility = (int)((2.0f - DominantCulture.GetTrait(Trait.Fanaticism)) * iHostility - 0.25);
                    sResult.AppendFormat("Tolerance \t(x{0}%)", (int)((2.0f - DominantCulture.GetTrait(Trait.Fanaticism)) * 100)).AppendLine();

                    iHostility = (int)((2.0f - DominantCulture.GetTrait(Trait.Agression)) * iHostility - 0.25);
                    sResult.AppendFormat("Amiability \t(x{0}%)", (int)((2.0f - DominantCulture.GetTrait(Trait.Agression)) * 100)).AppendLine();

                    if (iHostility == 0)
                        iHostility = -1;
                }
            }

            //if (fContact < fBorder / 2)
            //    iHostility = iHostility / 2;

            sResult.AppendLine("----").AppendFormat("Total \t({0:+#;-#;0})", -iHostility).AppendLine();
            sReasons = sResult.ToString();

            return iHostility;
        }

        /// <summary>
        /// Учитываем дополнительные факторы, влияющие на гендерные предпочтения - например, 
        /// определяемый фенотипом перекос в родаемости детей определённого пола...
        /// </summary>
        /// <param name="ePriority"></param>
        /// <returns></returns>
        internal override Customs.GenderPriority GetMinorGender()
        {
            if (Culture[Gender.Male].Customs.Has(Customs.GenderPriority.Matriarchy))
            {
                if (TitularNation.PhenotypeFemale.m_pValues.Get<LifeCycleGenetix>().BirthRate >
                    TitularNation.PhenotypeMale.m_pValues.Get<LifeCycleGenetix>().BirthRate)
                    return Customs.GenderPriority.Genders_equality;
                else
                    return Customs.GenderPriority.Patriarchy;
            }
            if (Culture[Gender.Male].Customs.Has(Customs.GenderPriority.Patriarchy))
            {
                if (TitularNation.PhenotypeMale.m_pValues.Get<LifeCycleGenetix>().BirthRate >
                    TitularNation.PhenotypeFemale.m_pValues.Get<LifeCycleGenetix>().BirthRate)
                    return Customs.GenderPriority.Genders_equality;
                else
                    return Customs.GenderPriority.Matriarchy;
            }

            return base.GetMinorGender();
        }
    }
}
