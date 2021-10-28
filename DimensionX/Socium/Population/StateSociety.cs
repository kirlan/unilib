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

namespace Socium.Population
{
    public class StateSociety: NationalSociety
    {
        #region State Models Array
        public class StateModel
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

            public bool m_bBig;

            public List<Language> m_cLanguages = new List<Language>();

            public SocialOrder m_pSocial = null;

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
            public StateModel(string sName, int iRank, SettlementInfo pStateCapital, SettlementInfo pProvinceCapital, bool bDinasty, int iMinGovernmentLevel, int iMaxGovernmentLevel, bool bBig, SocialOrder pSocial, Language[] cLanguages)
            {
                m_sName = sName;
                m_iRank = iRank;

                m_bDinasty = bDinasty;

                m_pStateCapital = pStateCapital;
                m_pProvinceCapital = pProvinceCapital;

                m_pSocial = pSocial;

                m_iMinGovernmentLevel = iMinGovernmentLevel;
                m_iMaxGovernmentLevel = iMaxGovernmentLevel;

                m_bBig = bBig;

                if (cLanguages != null)
                    m_cLanguages.AddRange(cLanguages);
            }

            public override string ToString()
            {
                return m_sName;
            }
        }

        internal static StateModel[] s_aModels =
        {
            new StateModel("Land", 1,
                new SettlementInfo(SettlementSize.Hamlet, "Hamlet", 5, 10, new BuildingInfo("Elder's hut", ProfessionInfo.RulerPrimitive, ProfessionInfo.HeirPrimitive, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Hamlet, "Hamlet", 5, 10, new BuildingInfo("Elder's hut", ProfessionInfo.GovernorPrimitive, ProfessionInfo.GovernorPrimitive, BuildingSize.Unique)),
                true, 0, 0, false, SocialOrder.Primitive, null),
            new StateModel("Lands", 1,
                new SettlementInfo(SettlementSize.Hamlet, "Hamlet", 5, 10, new BuildingInfo("Elder's hut", ProfessionInfo.RulerPrimitive, ProfessionInfo.HeirPrimitive, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Hamlet, "Hamlet", 5, 10, new BuildingInfo("Elder's hut", ProfessionInfo.GovernorPrimitive, ProfessionInfo.GovernorPrimitive, BuildingSize.Unique)),
                true, 0, 0, true, SocialOrder.Primitive, null),
            new StateModel("Tribes", 1,
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, new BuildingInfo("Clans hall", ProfessionInfo.RulerPrimitive, ProfessionInfo.HeirPrimitive, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, new BuildingInfo("Village hall", ProfessionInfo.GovernorPrimitive, ProfessionInfo.GovernorPrimitive, BuildingSize.Unique)),
                true, 1, 1, false, SocialOrder.Primitive, null),
            new StateModel("Clans", 1,
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, new BuildingInfo("Clans hall", ProfessionInfo.RulerPrimitive, ProfessionInfo.HeirPrimitive, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, new BuildingInfo("Village hall", ProfessionInfo.GovernorPrimitive, ProfessionInfo.GovernorPrimitive, BuildingSize.Unique)),
                true, 1, 1, true, SocialOrder.Primitive, null),
            new StateModel("Kingdom", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.KingEuro, ProfessionInfo.KingHeirEuro, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorEuro, ProfessionInfo.GovernorEuro, BuildingSize.Unique)),
                true, 2, 6, false, SocialOrder.MedievalEurope, new Language[] {Language.Dwarwen, Language.European, Language.Highlander}),
            new StateModel("Kingdom", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.KingEuro, ProfessionInfo.KingHeirEuro, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorEuro2, ProfessionInfo.GovernorEuro2, BuildingSize.Unique)),
                true, 2, 6, false, SocialOrder.MedievalEurope2, new Language[] {Language.Drow, Language.Elven, Language.European, Language.Highlander}),
            new StateModel("Empire", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.EmperorEuro, ProfessionInfo.KingHeirEuro, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorEuro3, ProfessionInfo.GovernorEuro3, BuildingSize.Unique)),
                true, 2, 6, true, SocialOrder.MedievalEurope2, new Language[] {Language.Drow, Language.Elven, Language.European, Language.Dwarwen}),
            new StateModel("Kingdom", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.KingEuro, ProfessionInfo.KingHeirEuro, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorEuro4, ProfessionInfo.GovernorEuro4, BuildingSize.Unique)),
                true, 1, 6, false, SocialOrder.MedievalEurope3, new Language[] {Language.Dwarwen, Language.European, Language.Highlander}),
            new StateModel("Reich", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.KingNorth, ProfessionInfo.KingHeirNorth, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorNorth, ProfessionInfo.GovernorNorth, BuildingSize.Unique)),
                true, 1, 6, false, SocialOrder.MedievalNorth, new Language[] {Language.Dwarwen, Language.Northman}),
            new StateModel("Kaiserreich", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.EmperorNorth, ProfessionInfo.EmperorHeirNorth, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorNorth2, ProfessionInfo.GovernorNorth2, BuildingSize.Unique)),
                true, 2, 6, true, SocialOrder.MedievalNorth, new Language[] {Language.Dwarwen, Language.Northman}),
            new StateModel("Regnum", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.KingLatin, ProfessionInfo.KingHeirLatin, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorLatin, ProfessionInfo.GovernorLatin, BuildingSize.Unique)),
                true, 1, 6, false, SocialOrder.MedievalLatin, new Language[] {Language.Latin}),
            new StateModel("Imperium", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.EmperorLatin, ProfessionInfo.KingHeirLatin, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.KingLatin, ProfessionInfo.KingHeirLatin, BuildingSize.Small)),
                true, 2, 6, true, SocialOrder.MedievalLatin, new Language[] {Language.Latin}),
            new StateModel("Shogunate", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.KingAsian, ProfessionInfo.KingHeirAsian, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorAsian, ProfessionInfo.GovernorAsian, BuildingSize.Unique)),
                true, 1, 6, false, SocialOrder.MedievalAsian, new Language[] {Language.Asian}),
            new StateModel("Empire", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.EmperorAsian, ProfessionInfo.EmperorHeirAsian, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorAsian2, ProfessionInfo.GovernorAsian2, BuildingSize.Unique)),
                true, 2, 6, true, SocialOrder.MedievalAsian, new Language[] {Language.Asian}),
            new StateModel("Shahdom", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.KingArabian, ProfessionInfo.KingHeirArabian, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorArabian, ProfessionInfo.GovernorArabian, BuildingSize.Unique)),
                true, 1, 6, false, SocialOrder.MedievalArabian, new Language[] {Language.Arabian}),
            new StateModel("Sultanate", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.KingArabian2, ProfessionInfo.KingHeirArabian2, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorArabian, ProfessionInfo.GovernorArabian, BuildingSize.Unique)),
                true, 1, 6, false, SocialOrder.MedievalArabian, new Language[] {Language.Arabian, Language.African}),
            new StateModel("Caliphate", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.EmperorArabian, ProfessionInfo.KingHeirEuro, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorArabian, ProfessionInfo.GovernorArabian, BuildingSize.Unique)),
                true, 2, 6, true, SocialOrder.MedievalArabian, new Language[] {Language.Arabian, Language.African}),
            new StateModel("Khanate", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.KingMongol, ProfessionInfo.KingHeirEuro, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorMongol, ProfessionInfo.GovernorMongol, BuildingSize.Unique)),
                true, 1, 6, false, SocialOrder.MedievalMongol, new Language[] {Language.Orkish, Language.Eskimoid}),
            new StateModel("Khaganate", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.EmperorMongol, ProfessionInfo.KingHeirEuro, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.KingMongol, ProfessionInfo.KingHeirEuro, BuildingSize.Small)),
                true, 2, 6, true, SocialOrder.MedievalMongol, new Language[] {Language.Orkish, Language.Eskimoid}),
            new StateModel("Knyazdom", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.KingSlavic, ProfessionInfo.KingHeirSlavic, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorSlavic, ProfessionInfo.GovernorSlavic, BuildingSize.Unique)),
                true, 1, 6, false, SocialOrder.MedievalSlavic, new Language[] {Language.Slavic}),
            new StateModel("Tsardom", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.EmperorSlavic, ProfessionInfo.EmperorHeirSlavic, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.KingSlavic, ProfessionInfo.KingHeirSlavic, BuildingSize.Small)),
                true, 2, 6, true, SocialOrder.MedievalSlavic, new Language[] {Language.Slavic}),
            new StateModel("Basileia", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.KingGreek, ProfessionInfo.KingHeirGreek, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorGreek, ProfessionInfo.GovernorGreek, BuildingSize.Unique)),
                true, 1, 6, false, SocialOrder.MedievalGreek, new Language[] {Language.Greek}),
            new StateModel("Autokratoria", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.EmperorGreek, ProfessionInfo.KingHeirGreek, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorGreek2, ProfessionInfo.GovernorGreek2, BuildingSize.Unique)),
                true, 2, 6, true, SocialOrder.MedievalGreek, new Language[] {Language.Greek}),
            new StateModel("Raj", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.KingHindu, ProfessionInfo.KingHeirHindu, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorHindu, ProfessionInfo.GovernorHindu, BuildingSize.Unique)),
                true, 1, 6, false, SocialOrder.MedievalHindu, new Language[] {Language.Hindu}),
            new StateModel("Empire", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.EmperorHindu, ProfessionInfo.EmperorHeirHindu, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.KingHindu, ProfessionInfo.KingHeirHindu, BuildingSize.Small)),
                true, 2, 6, true, SocialOrder.MedievalHindu, new Language[] {Language.Hindu}),
            new StateModel("Altepetl", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.KingAztec, ProfessionInfo.KingHeirAztec, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorAztec, ProfessionInfo.GovernorAztec, BuildingSize.Unique)),
                true, 1, 6, false, SocialOrder.MedievalAztec, new Language[] {Language.Aztec}),
            new StateModel("Empire", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Castle", ProfessionInfo.EmperorEuro, ProfessionInfo.KingHeirAztec, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Castle", ProfessionInfo.GovernorAztec, ProfessionInfo.GovernorAztec, BuildingSize.Unique)),
                true, 2, 6, true, SocialOrder.MedievalAztec, new Language[] {Language.Aztec}),
            new StateModel("Republic", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Statehouse", ProfessionInfo.RulerModern, ProfessionInfo.AdvisorModern, BuildingSize.Small)),
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Town hall", ProfessionInfo.GovernorModern, ProfessionInfo.GovernorModern, BuildingSize.Unique)),
                false, 3, 7, false, SocialOrder.Modern, null),
            //new StateInfo("Republic", 16,
            //    new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 15, 5, 10, new BuildingInfo("Palace", "Dictator", "Dictator", 16)), 
            //    new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, 5, 7, new BuildingInfo("Palace", "Governor", "Governor", 14)), 
            //    "Counsellor", "Counsellor", false, 2, 6, 3, 4),
            //new StateInfo("Republic", 16,
            //    new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 15, 5, 10, new BuildingInfo("Palace", "General", "General", 16)), 
            //    new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, 5, 7, new BuildingInfo("Palace", "Colonel", "Colonel", 14)), 
            //    "Officer", "Officer", false, 2, 6, 3, 4),
            new StateModel("Federation", 2,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, new BuildingInfo("Statehouse", ProfessionInfo.RulerModern2, ProfessionInfo.AdvisorModern2, BuildingSize.Unique)),
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Town hall", ProfessionInfo.GovernorModern2, ProfessionInfo.GovernorModern2, BuildingSize.Unique)),
                false, 4, 7, false, SocialOrder.Modern, null),
            new StateModel("League", 3,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, new BuildingInfo("Palace", ProfessionInfo.RulerModern3, ProfessionInfo.AdvisorModern3, BuildingSize.Unique)),
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Palace", ProfessionInfo.GovernorModern, ProfessionInfo.GovernorModern, BuildingSize.Unique)),
                false, 4, 7, true, SocialOrder.Modern, null),
            new StateModel("Union", 3,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, new BuildingInfo("Statehouse", ProfessionInfo.RulerModern2, ProfessionInfo.AdvisorModern2, BuildingSize.Unique)),
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Town hall", ProfessionInfo.GovernorModern2, ProfessionInfo.GovernorModern2, BuildingSize.Unique)),
                false, 5, 7, true, SocialOrder.Modern, null),
            new StateModel("Alliance", 3,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, new BuildingInfo("Palace", ProfessionInfo.RulerModern4, ProfessionInfo.AdvisorModern, BuildingSize.Unique)),
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Town hall", ProfessionInfo.GovernorModern2, ProfessionInfo.GovernorModern2, BuildingSize.Unique)),
                false, 5, 7, true, SocialOrder.Modern, null),
            new StateModel("Coalition", 3,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, new BuildingInfo("Statehouse", ProfessionInfo.RulerModern3, ProfessionInfo.AdvisorModern, BuildingSize.Unique)),
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Town hall", ProfessionInfo.GovernorModern2, ProfessionInfo.GovernorModern2, BuildingSize.Unique)),
                false, 5, 7, true, SocialOrder.Modern, null),
            new StateModel("Association", 3,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, new BuildingInfo("Statehouse", ProfessionInfo.RulerModern2, ProfessionInfo.AdvisorModern4, BuildingSize.Unique)),
                new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("Town hall", ProfessionInfo.GovernorModern, ProfessionInfo.GovernorModern, BuildingSize.Unique)),
                false, 5, 7, true, SocialOrder.Modern, null),
            //new StateInfo("Realm", 17,
            //    new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 16, 5, 10, new BuildingInfo("Citadel", "God-King", "Goddess-Queen", 17)), 
            //    new SettlementInfo(SettlementSize.City, "City", 40, 80, 15, 5, 10, new BuildingInfo("Palace", "Father", "Mother", 15)), 
            //    "Brother", "Sister", false, 5, 8, null),
            new StateModel("Commonwealth", 1,
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Town hall", ProfessionInfo.RulerModern3, ProfessionInfo.AdvisorModern5, BuildingSize.Unique)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Town hall", ProfessionInfo.GovernorModern3, ProfessionInfo.GovernorModern3, BuildingSize.Unique)),
                false, 7, 8, false, SocialOrder.Future, null),
            new StateModel("Society", 1,
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, null),
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, null),
                false, 7, 8, false, SocialOrder.Future, null),
            new StateModel("Collective", 2,
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, null),
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, null),
                false, 7, 8, true, SocialOrder.Future, null),
        };
        #endregion

        /// <summary>
        /// 0 - Анархия. Есть номинальная власть, но она не занимается охраной правопорядка.
        /// 1 - Власти занимаются только самыми вопиющими преступлениями.
        /// 2 - Есть законы, их надо соблюдать, кто не соблюдает - тот преступник, а вор должен сидеть в тюрьме.
        /// 3 - Законы крайне строги, широко используется смертная казнь.
        /// 4 - Тоталитарная диктатура. Все граждане, кроме правящей верхушки, попадают под презумпцию виновности.
        /// </summary>
        public int m_iControl = 0;
        /// <summary>
        /// Уровень социального (не)равенства.
        /// 0 - рабство
        /// 1 - крепостное право
        /// 2 - капитализм
        /// 3 - социализм
        /// 4 - коммунизм
        /// </summary>
        public int m_iSocialEquality = 0;

        public StateModel m_pStateModel = null;

        private State m_pState = null;

        public Dictionary<Estate.Position, Estate> m_cEstates = new Dictionary<Estate.Position, Estate>();

        public Nation m_pHostNation = null;
        
        public Nation m_pSlavesNation = null;

        public StateSociety(State pState)
            : base(pState.m_pMethropoly.m_pLocalSociety.m_pTitularNation)
        {
            m_pState = pState;

            m_iTechLevel = 0;
            m_iInfrastructureLevel = 0;

            m_cCulture[Gender.Male] = new Culture(pState.m_pMethropoly.m_pLocalSociety.m_cCulture[Gender.Male], Customs.Mutation.Possible);
            m_cCulture[Gender.Female] = new Culture(pState.m_pMethropoly.m_pLocalSociety.m_cCulture[Gender.Female], Customs.Mutation.Possible);

            FixSexCustoms();
        }

        public void CalculateTitularNation()
        { 
            Dictionary<Nation, int> cNationsCount = new Dictionary<Nation, int>();

            foreach (Province pProvince in m_pState.m_cContents)
            {
                int iCount = 0;
                if (!cNationsCount.TryGetValue(pProvince.m_pLocalSociety.m_pTitularNation, out iCount))
                    cNationsCount[pProvince.m_pLocalSociety.m_pTitularNation] = 0;
                cNationsCount[pProvince.m_pLocalSociety.m_pTitularNation] = iCount + pProvince.m_iPopulation;

                if (pProvince.m_pLocalSociety.m_iTechLevel > m_iTechLevel)
                    m_iTechLevel = pProvince.m_pLocalSociety.m_iTechLevel;

                if (pProvince.m_pLocalSociety.m_iInfrastructureLevel > m_iInfrastructureLevel)
                    m_iInfrastructureLevel = pProvince.m_pLocalSociety.m_iInfrastructureLevel;
                //m_iInfrastructureLevel += pProvince.m_iInfrastructureLevel;
                //fInfrastructureLevel += 1.0f/(pProvince.m_iInfrastructureLevel+1);
            }

            Nation pMostCommonNation = cNationsCount.Keys.ToArray()[Rnd.ChooseBest(cNationsCount.Values)];
            if (m_pTitularNation.DominantPhenotype.m_pValues.Get<NutritionGenetix>().IsParasite() || m_pTitularNation.m_bInvader)
            {
                cNationsCount.Remove(m_pTitularNation);
                if (cNationsCount.Count > 0)
                    pMostCommonNation = cNationsCount.Keys.ToArray()[Rnd.ChooseBest(cNationsCount.Values)];
            }
            else
            {
                m_pTitularNation = pMostCommonNation;
                cNationsCount.Remove(m_pTitularNation);
            }
            m_pHostNation = pMostCommonNation;
            if (cNationsCount.ContainsKey(pMostCommonNation))
                cNationsCount.Remove(pMostCommonNation);
            if (cNationsCount.Count > 0)
                pMostCommonNation = cNationsCount.Keys.ToArray()[Rnd.ChooseOne(cNationsCount.Values, 1)];

            m_pSlavesNation = pMostCommonNation;

            Nation getAccessableNation(bool bCanBeDying, bool bCanBeParasite, bool bOnlyLocals, Nation pDefault)
            {
                List<Nation> cAccessableNations = new List<Nation>();
                ContinentX pContinent = m_pState.Owner as ContinentX;
                foreach (var pNations in pContinent.m_cLocalNations)
                {
                    cAccessableNations.AddRange(pNations.Value);
                }
                if (!bOnlyLocals)
                {
                    if (InfrastructureLevels[m_iInfrastructureLevel].m_eMaxNavalPath == RoadQuality.Good ||
                        InfrastructureLevels[m_iInfrastructureLevel].m_iAerialAvailability > 1)
                    {
                        var pWorld = pContinent.Owner as World;
                        foreach (var pOtherContinent in pWorld.m_aContinents)
                        {
                            if (pOtherContinent == pContinent)
                                continue;

                            foreach (var pNations in pOtherContinent.m_cLocalNations)
                            {
                                cAccessableNations.AddRange(pNations.Value);
                            }
                        }
                    }
                }

                Dictionary<Nation, int> cChances = new Dictionary<Nation, int>();
                foreach (var pNation in cAccessableNations)
                {
                    if (pNation.m_bDying && !bCanBeDying)
                        continue;

                    if (pNation.DominantPhenotype.m_pValues.Get<NutritionGenetix>().IsParasite() && !bCanBeParasite)
                        continue;

                    if (!cChances.ContainsKey(pNation))
                        cChances[pNation] = 0;
                    cChances[pNation]++;

                    if (pNation.m_pRace.m_pLanguage == m_pTitularNation.m_pRace.m_pLanguage)
                        cChances[pNation]++;

                    foreach (var pNeighbour in m_pState.BorderWith)
                    {
                        var pOtherState = pNeighbour.Key as State;
                        if (pOtherState.Forbidden)
                            continue;

                        if (pOtherState.m_pSociety.m_pTitularNation == pNation)
                            cChances[pNation]++;
                    }

                    int iHostility = m_pTitularNation.DominantPhenotype.GetFenotypeDifference(pNation.DominantPhenotype);
                    if (iHostility > 0)
                        cChances[pNation]++;
                    if (iHostility > 3)
                        cChances[pNation]++;

                    if (pNation == pDefault)
                        cChances[pNation] = 1;
                }

                int iChoice = Rnd.ChooseOne(cChances.Values, 2);
                if (iChoice == -1)
                    throw new Exception("Can't find host nation for a parasite!");

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

            if (m_pTitularNation == m_pHostNation && (m_pTitularNation.DominantPhenotype.m_pValues.Get<NutritionGenetix>().IsParasite() || (m_pTitularNation.m_bInvader /*&& m_iControl >= 3*/)))
            {
                m_pHostNation = getAccessableNation(false, false, true, m_pHostNation);
            }
            if (m_pTitularNation == m_pSlavesNation && Rnd.OneChanceFrom(3))
            {
                m_pSlavesNation = getAccessableNation(true, false, false, m_pSlavesNation);
            }
        }

        public override void AddBuildings(Settlement pSettlement)
        {
            pSettlement.m_cBuildings.Clear();

            int iBuildingsCount = pSettlement.m_pInfo.m_iMinBuildingsCount + Rnd.Get(pSettlement.m_pInfo.m_iDeltaBuildingsCount + 1);
            for (int i = 0; i < iBuildingsCount; i++)
            {
                Building pNewBuilding = new Building(pSettlement, ChooseNewBuilding(pSettlement));
                pSettlement.m_cBuildings.Add(pNewBuilding);
            }

            if (pSettlement.m_pInfo.m_pMainBuilding != null)
            {
                Building pNewBuilding = new Building(pSettlement, pSettlement.m_pInfo.m_pMainBuilding);
                pSettlement.m_cBuildings.Add(pNewBuilding);
            }
        }


        public Dictionary<ProfessionInfo, int> m_cProfessions = new Dictionary<ProfessionInfo, int>();
        public Dictionary<Estate.Position, int> m_cEstatesCounts = new Dictionary<Estate.Position, int>();

        /// <summary>
        /// Распределяет население государства по сословиям. 
        /// Вызывается в CWorld.PopulateMap после того, как созданы и застроены все поселения в государстве - т.е. после того, как сформировано собственно население.
        /// </summary>
        public void SetEstates()
        {
            // Базовые настройки культуры и обычаев для сообщества
            Estate pBase = new Estate(this, Estate.Position.Commoners, m_pStateModel.m_pSocial.GetEstateName(Estate.Position.Commoners), true);

            // Элита может немного отличаться от среднего класса
            m_cEstates[Estate.Position.Elite] = new Estate(pBase, Estate.Position.Elite, m_pStateModel.m_pSocial.GetEstateName(Estate.Position.Elite), false);

            // средний класс - это и есть основа всего сообщества
            m_cEstates[Estate.Position.Commoners] = pBase;
            m_cEstates[Estate.Position.Commoners].m_pTitularNation = m_pHostNation;
            // TODO: насколько сильно родная культура порабощённой нации должна влиять на культуру образованного ей сословия?
            // Сейчас она не влияет вообще, порабощённые расы полностью ассимилируются. Но правильно ли это?

            // низший класс - его обычаи должны отличаться не только от среднего класса, но и от элиты
            do
            {
                m_cEstates[Estate.Position.Lowlifes] = new Estate(pBase, Estate.Position.Lowlifes, m_pStateModel.m_pSocial.GetEstateName(Estate.Position.Lowlifes), false);
            }
            while (m_cEstates[Estate.Position.Elite].m_cCulture[Gender.Male].Equals(m_cEstates[Estate.Position.Lowlifes].m_cCulture[Gender.Male]) ||
                   m_cEstates[Estate.Position.Elite].m_cCulture[Gender.Female].Equals(m_cEstates[Estate.Position.Lowlifes].m_cCulture[Gender.Female]));
            m_cEstates[Estate.Position.Lowlifes].m_pTitularNation = m_pSlavesNation;


            // аутсайдеры - строим либо на базе среднего класса, либо на базе низшего - и следим, чтобы тоже отличалось от всех 3 других сословий
            do
            {
                if (!Rnd.OneChanceFrom(3) && m_iSocialEquality != 0)
                {
                    m_cEstates[Estate.Position.Outlaws] = new Estate(m_cEstates[Estate.Position.Lowlifes], Estate.Position.Outlaws, m_pStateModel.m_pSocial.GetEstateName(Estate.Position.Outlaws), false);
                }
                else
                {
                    m_cEstates[Estate.Position.Outlaws] = new Estate(pBase, Estate.Position.Outlaws, m_pStateModel.m_pSocial.GetEstateName(Estate.Position.Outlaws), false);
                }
            }
            while (m_cEstates[Estate.Position.Elite].m_cCulture[Gender.Male].Equals(m_cEstates[Estate.Position.Outlaws].m_cCulture[Gender.Male]) ||
                   m_cEstates[Estate.Position.Elite].m_cCulture[Gender.Female].Equals(m_cEstates[Estate.Position.Outlaws].m_cCulture[Gender.Female]) ||
                   m_cEstates[Estate.Position.Lowlifes].m_cCulture[Gender.Male].Equals(m_cEstates[Estate.Position.Outlaws].m_cCulture[Gender.Male]) ||
                   m_cEstates[Estate.Position.Lowlifes].m_cCulture[Gender.Female].Equals(m_cEstates[Estate.Position.Outlaws].m_cCulture[Gender.Female]) ||
                   pBase.m_cCulture[Gender.Male].Equals(m_cEstates[Estate.Position.Outlaws].m_cCulture[Gender.Male]) ||
                   pBase.m_cCulture[Gender.Female].Equals(m_cEstates[Estate.Position.Outlaws].m_cCulture[Gender.Female]));
            m_cEstates[Estate.Position.Outlaws].m_pTitularNation = Rnd.OneChanceFrom(3) ? m_pTitularNation : Rnd.OneChanceFrom(2) ? m_pHostNation : m_pSlavesNation;

            // перебираем все поселения, где присутсвует сообщество
            foreach (LocationX pLocation in Settlements)
            {
                if (pLocation.m_pSettlement == null)
                    continue;

                if (pLocation.m_pSettlement.m_cBuildings.Count > 0)
                {
                    // перебираем все строения в поселениях
                    foreach (Building pBuilding in pLocation.m_pSettlement.m_cBuildings)
                    {
                        int iCount = 0;
                        int iOwnersCount = pBuilding.m_pInfo.OwnersCount;
                        int iWorkersCount = pBuilding.m_pInfo.WorkersCount;

                        var pOwner = pBuilding.m_pInfo.m_pOwnerProfession;
                        m_cProfessions.TryGetValue(pOwner, out iCount);
                        m_cProfessions[pOwner] = iCount + iOwnersCount;

                        var pWorkers = pBuilding.m_pInfo.m_pWorkersProfession;
                        m_cProfessions.TryGetValue(pWorkers, out iCount);
                        m_cProfessions[pWorkers] = iCount + iWorkersCount;
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
            foreach (var pProfession in m_cProfessions)
            {
                iTotalPopulation += pProfession.Value;

                int iPreference = GetProfessionSkillPreference(pProfession.Key);

                if (pProfession.Key.m_bMaster)
                    iPreference += 4;

                int iDiscrimination = (int)(DominantCulture.GetTrait(Trait.Fanaticism) + 0.5);

                var eProfessionGenderPriority = GetProfessionGenderPriority(pProfession.Key);

                if (DominantCulture.m_pCustoms.Has(Customs.GenderPriority.Patriarchy) &&
                    eProfessionGenderPriority == Customs.GenderPriority.Matriarchy)
                    iPreference -= iDiscrimination;
                if (DominantCulture.m_pCustoms.Has(Customs.GenderPriority.Matriarchy) &&
                    eProfessionGenderPriority == Customs.GenderPriority.Patriarchy)
                    iPreference -= iDiscrimination;

                if (!cProfessionPreference.ContainsKey(iPreference))
                    cProfessionPreference[iPreference] = new List<ProfessionInfo>();

                cProfessionPreference[iPreference].Add(pProfession.Key);
            }

            //Численность сословий - в зависимости от m_iSocialEquality.
            //При рабовладельческом строе (0) элита должна составлять порядка 1% населения, низшее сословие - 90-95%
            //При коммунизме - численность сословий равна. Или может наоборот, должна быть элита 90+%

            int iLowEstateCount = iTotalPopulation * (92 - m_iSocialEquality * 23) / 100;
            int iEliteEstateCount = iTotalPopulation * (10 + m_iSocialEquality * 5) / 100;

            void addCasteRestrictedProfessionToEstate(ProfessionInfo pProfession)
            {
                if (pProfession.m_eCasteRestriction.HasValue)
                {
                    var pEstate = m_cEstates[Estate.Position.Elite];
                    var iCount = 0;
                    m_cProfessions.TryGetValue(pProfession, out iCount);
                    switch (pProfession.m_eCasteRestriction)
                    {
                        case ProfessionInfo.Caste.Elite:
                            pEstate = m_cEstates[Estate.Position.Elite];
                            iEliteEstateCount -= iCount;
                            break;
                        case ProfessionInfo.Caste.Low:
                            pEstate = m_cEstates[Estate.Position.Lowlifes];
                            iLowEstateCount -= iCount;
                            break;
                        case ProfessionInfo.Caste.Outlaw:
                            pEstate = m_cEstates[Estate.Position.Outlaws];
                            break;
                    }
                    pEstate.m_cGenderProfessionPreferences[pProfession] = GetProfessionGenderPriority(pProfession);
                    removeProfessionPreference(pProfession);
                }
            }

            addCasteRestrictedProfessionToEstate(m_pStateModel.m_pStateCapital.m_pMainBuilding.m_pOwnerProfession);
            addCasteRestrictedProfessionToEstate(m_pStateModel.m_pStateCapital.m_pMainBuilding.m_pWorkersProfession);
            addCasteRestrictedProfessionToEstate(m_pStateModel.m_pProvinceCapital.m_pMainBuilding.m_pOwnerProfession);

            foreach (var pProfession in m_cProfessions)
            {
                addCasteRestrictedProfessionToEstate(pProfession.Key);
            }

            int iMinCommonersProfessionsCount = cProfessionPreference.Count - 2 * cProfessionPreference.Count / 3;
            if (m_iSocialEquality != 0 || m_cEstates[Estate.Position.Lowlifes].m_cGenderProfessionPreferences.Count == 0)
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
                        if (pProfession.m_eCasteRestriction != ProfessionInfo.Caste.MiddleOrUp &&
                            m_cProfessions[pProfession] < iLowEstateCount && m_cProfessions[pProfession] > iBestFit)
                        {
                            pBestFit = pProfession;
                            iBestFit = m_cProfessions[pProfession];
                        }
                    }
                    if (pBestFit == null)
                    {
                        iBestFit = int.MaxValue;
                        foreach (ProfessionInfo pProfession in cProfessionPreference[iLowestPreference])
                        {
                            if (pProfession.m_eCasteRestriction != ProfessionInfo.Caste.MiddleOrUp &&
                                m_cProfessions[pProfession] < iBestFit)
                            {
                                pBestFit = pProfession;
                                iBestFit = m_cProfessions[pProfession];
                            }
                        }
                    }
                    if (pBestFit != null)
                    {
                        m_cEstates[Estate.Position.Lowlifes].m_cGenderProfessionPreferences[pBestFit] = GetProfessionGenderPriority(pBestFit);
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
                    if (m_cProfessions[pStrata] < iBestFit)
                    {
                        pBestFit = pStrata;
                        iBestFit = m_cProfessions[pStrata];
                    }
                }
                if (pBestFit != null)
                {
                    m_cEstates[Estate.Position.Elite].m_cGenderProfessionPreferences[pBestFit] = GetProfessionGenderPriority(pBestFit);
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
                    m_cEstates[Estate.Position.Commoners].m_cGenderProfessionPreferences[pProfession] = GetProfessionGenderPriority(pProfession);
                }
            }

            foreach (var pEstate in m_cEstates)
            {
                m_cEstatesCounts[pEstate.Key] = 0;
                foreach (var pProfession in pEstate.Value.m_cGenderProfessionPreferences)
                {
                    int iCount = 0;
                    if (m_cProfessions.TryGetValue(pProfession.Key, out iCount))
                        m_cEstatesCounts[pEstate.Key] += iCount;
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
            string sEquality = "Slavery";
            switch (iEquality)
            {
                case 1:
                    sEquality = "Serfdom";
                    break;
                case 2:
                    sEquality = "Hired labour";
                    break;
                case 3:
                    sEquality = "Social equality";
                    break;
                case 4:
                    sEquality = "Utopia";
                    break;
            }
            return sEquality;
        }

        public int GetImportedTech()
        {
            if (m_pTitularNation.m_bDying)
                return -1;

            int iMaxTech = GetEffectiveTech();
            foreach (State pState in m_pState.m_aBorderWith)
            {
                if (pState.Forbidden)
                    continue;

                if (pState.m_pSociety.GetEffectiveTech() > iMaxTech)
                    iMaxTech = pState.m_pSociety.GetEffectiveTech();
            }

            if (iMaxTech <= GetEffectiveTech())
                iMaxTech = -1;

            return iMaxTech;
        }

        public string GetImportedTechString()
        {
            if (m_pTitularNation.m_bDying)
                return "";

            int iMaxTech = GetEffectiveTech();
            State pExporter = null;
            foreach (State pState in m_pState.m_aBorderWith)
            {
                if (pState.Forbidden)
                    continue;

                if (pState.m_pSociety.GetEffectiveTech() > iMaxTech)
                {
                    iMaxTech = pState.m_pSociety.GetEffectiveTech();
                    pExporter = pState;
                }
            }

            if (pExporter == null)
                return "";

            return GetTechString(pExporter.m_pSociety.m_iTechLevel, pExporter.m_pSociety.DominantCulture.m_pCustoms.ValueOf<Customs.Science>());
        }
        public override string ToString()
        {
            return string.Format("{2} (C{1}T{3}M{5}) - {0} {4}", m_sName, DominantCulture.m_iProgressLevel, m_pTitularNation, m_iTechLevel, m_pStateModel.m_sName, m_iMagicLimit);
        }

        protected override BuildingInfo ChooseNewBuilding(Settlement pSettlement)
        {
            int iInfrastructureLevel = m_iInfrastructureLevel;
            int iControl = m_iControl * 2;

            Dictionary<BuildingInfo, float> cChances = new Dictionary<BuildingInfo, float>();
            switch (pSettlement.m_pInfo.m_eSize)
            {
                case SettlementSize.Hamlet:
                    {
                        if (pSettlement.m_cBuildings.Count > 0)
                        {
                            if (iInfrastructureLevel < 2)
                                cChances[BuildingInfo.WarriorsHutSmall] = DominantCulture.GetTrait(Trait.Agression) / 2;//(float)iControl / 4;

                            if (iInfrastructureLevel < 2)
                                cChances[BuildingInfo.ShamansHutSmall] = DominantCulture.GetTrait(Trait.Piety) / 2;

                            if (m_iSocialEquality == 0)
                                cChances[BuildingInfo.SlavePensMedium] = (float)cChances.Count / 2 + 1;

                            cChances[BuildingInfo.RaidersHutSmall] = DominantCulture.GetTrait(Trait.Treachery) / 2;
                        }

                        BuildingInfo pProfile;
                        switch (pSettlement.m_eSpeciality)
                        {
                            case SettlementSpeciality.Fishers:
                                pProfile = m_iSocialEquality == 0 ? BuildingInfo.FishingBoatSlvMedium : BuildingInfo.FishingBoatMedium;
                                break;
                            case SettlementSpeciality.Farmers:
                                pProfile = m_iSocialEquality == 0 ? BuildingInfo.FarmSlvMedium : BuildingInfo.FarmMedium;
                                break;
                            case SettlementSpeciality.Peasants:
                                pProfile = m_iSocialEquality == 0 ? BuildingInfo.PeasantsHutSlvMedium : BuildingInfo.PeasantsHutMedium;
                                break;
                            case SettlementSpeciality.Hunters:
                                pProfile = m_iSocialEquality == 0 ? BuildingInfo.HuntersHutSlvMedium : BuildingInfo.HuntersHutMedium;
                                break;
                            case SettlementSpeciality.Miners:
                                pProfile = m_iSocialEquality == 0 ? BuildingInfo.MineSlvMedium : BuildingInfo.MineMedium;
                                break;
                            case SettlementSpeciality.Lumberjacks:
                                pProfile = m_iSocialEquality == 0 ? BuildingInfo.LumberjacksHutSlvMedium : BuildingInfo.LumberjacksHutMedium;
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
                        if (pSettlement.m_cBuildings.Count > 0)// && Rnd.OneChanceFrom(2))
                        {
                            BuildingInfo pGuard;
                            if (iInfrastructureLevel < 2)
                                pGuard = BuildingInfo.WarriorsHutSmall;
                            else
                                if (iInfrastructureLevel < 4)
                                pGuard = BuildingInfo.GuardTowerSmall;
                            else
                                    if (iInfrastructureLevel == 8)
                                pGuard = BuildingInfo.EmergencyPostSmall;
                            else
                                pGuard = BuildingInfo.PoliceStationSmall;

                            cChances[pGuard] = (float)iControl * DominantCulture.GetTrait(Trait.Agression) / 4;

                            BuildingInfo pChurch;
                            if (iInfrastructureLevel < 2)
                                pChurch = BuildingInfo.ShamansHutSmall;
                            else
                                pChurch = BuildingInfo.ChurchSmall;

                            cChances[pChurch] = DominantCulture.GetTrait(Trait.Piety);

                            if (iInfrastructureLevel >= 2)
                            {
                                if (iInfrastructureLevel < 4)
                                {
                                    cChances[m_iSocialEquality == 0 ? BuildingInfo.InnSlvSmall : BuildingInfo.InnSmall] = DominantCulture.GetTrait(Trait.Simplicity) / 4;
                                    cChances[m_iSocialEquality == 0 ? BuildingInfo.TavernSlvSmall : BuildingInfo.TavernSmall] = DominantCulture.GetTrait(Trait.Simplicity);
                                }
                                else
                                {
                                    cChances[m_iSocialEquality == 0 ? BuildingInfo.HotelSlvSmall : BuildingInfo.HotelSmall] = DominantCulture.GetTrait(Trait.Simplicity) / 4;
                                    cChances[m_iSocialEquality == 0 ? BuildingInfo.BarSlvSmall : BuildingInfo.BarSmall] = DominantCulture.GetTrait(Trait.Simplicity);
                                }
                            }

                            cChances[BuildingInfo.RoguesDenSmall] = DominantCulture.GetTrait(Trait.Treachery) / 2;

                            if (iInfrastructureLevel >= 3)
                            {
                                float fScience = 0.05f;
                                if (DominantCulture.m_pCustoms.Has(Customs.MindSet.Balanced_mind))
                                    fScience = 0.25f;
                                if (DominantCulture.m_pCustoms.Has(Customs.MindSet.Logic))
                                    fScience = 0.5f;

                                if (m_pTitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Sapient)
                                    fScience *= 2;
                                if (m_pTitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Ingenious)
                                    fScience *= 4;

                                cChances[BuildingInfo.SchoolSmall] = fScience / 4;
                            }

                            //if (pState.m_iSocialEquality == 0)
                            //{
                            //    cChances[BuildingInfo.SlavePensMedium] = (float)cChances.Count;// / 2 + 1;
                            //}

                            cChances[BuildingInfo.MarketSmall] = (float)cChances.Count / 2;// / 2 + 1;

                            if (m_pStateModel.m_bDinasty)
                            {
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.EstateSlvSmall : BuildingInfo.EstateSmall] = (float)cChances.Count / 4;
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.CastleSlvSmall : BuildingInfo.CastleSmall] = (float)cChances.Count / 12;
                            }
                        }

                        BuildingInfo pProfile;
                        switch (pSettlement.m_eSpeciality)
                        {
                            case SettlementSpeciality.Fishers:
                                pProfile = m_iSocialEquality == 0 ? BuildingInfo.FishingBoatSlvMedium : BuildingInfo.FishingBoatMedium;
                                break;
                            case SettlementSpeciality.Farmers:
                                pProfile = Rnd.OneChanceFrom(2) ? (m_iSocialEquality == 0 ? BuildingInfo.FarmSlvMedium : BuildingInfo.FarmMedium) : (m_iSocialEquality == 0 ? BuildingInfo.FarmSlvLarge : BuildingInfo.FarmLarge);
                                break;
                            case SettlementSpeciality.Peasants:
                                pProfile = Rnd.OneChanceFrom(2) ? (m_iSocialEquality == 0 ? BuildingInfo.PeasantsHutSlvMedium : BuildingInfo.PeasantsHutMedium) : (m_iSocialEquality == 0 ? BuildingInfo.PeasantsHutSlvLarge : BuildingInfo.PeasantsHutLarge);
                                break;
                            case SettlementSpeciality.Hunters:
                                pProfile = Rnd.OneChanceFrom(2) ? (m_iSocialEquality == 0 ? BuildingInfo.HuntersHutSlvMedium : BuildingInfo.HuntersHutMedium) : (m_iSocialEquality == 0 ? BuildingInfo.HuntersHutSlvLarge : BuildingInfo.HuntersHutLarge);
                                break;
                            case SettlementSpeciality.Miners:
                                pProfile = Rnd.OneChanceFrom(2) ? (m_iSocialEquality == 0 ? BuildingInfo.MineSlvMedium : BuildingInfo.MineMedium) : (m_iSocialEquality == 0 ? BuildingInfo.MineSlvLarge : BuildingInfo.MineLarge);
                                break;
                            case SettlementSpeciality.Lumberjacks:
                                pProfile = Rnd.OneChanceFrom(2) ? (m_iSocialEquality == 0 ? BuildingInfo.LumberjacksHutSlvMedium : BuildingInfo.LumberjacksHutMedium) : (m_iSocialEquality == 0 ? BuildingInfo.LumberjacksHutSlvLarge : BuildingInfo.LumberjacksHutLarge);
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
                        if (pSettlement.m_cBuildings.Count > 0)// && Rnd.OneChanceFrom(2))
                        {
                            BuildingInfo pGuard;
                            if (iInfrastructureLevel < 4)
                                pGuard = BuildingInfo.GuardBarracksSmall;
                            else
                                if (iInfrastructureLevel == 8)
                                pGuard = BuildingInfo.EmergencyPostSmall;
                            else
                                pGuard = BuildingInfo.PoliceStationSmall;

                            cChances[pGuard] = (float)iControl * DominantCulture.GetTrait(Trait.Agression) / 4;

                            BuildingInfo pPrison;
                            if (iInfrastructureLevel < 4)
                                pPrison = BuildingInfo.PrisonSmall;
                            else
                                if (iInfrastructureLevel == 8)
                                pPrison = BuildingInfo.HoldingSmall;
                            else
                                pPrison = BuildingInfo.PrisonPoliceSmall;

                            cChances[pPrison] = (float)iControl / 4;

                            cChances[BuildingInfo.ChurchMedium] = DominantCulture.GetTrait(Trait.Piety) / 5;
                            cChances[BuildingInfo.ChurchSmall] = DominantCulture.GetTrait(Trait.Piety);

                            BuildingInfo pBrothelProfile = m_iSocialEquality == 0 ? BuildingInfo.BrothelSlvMedium : BuildingInfo.BrothelMedium;
                            BuildingInfo pStripClubProfile = m_iSocialEquality == 0 ? BuildingInfo.StripClubSlvSmall : BuildingInfo.StripClubSmall;

                            if (DominantCulture.m_pCustoms.Has(Customs.Sexuality.Moderate_sexuality))
                            {
                                cChances[pBrothelProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 4;
                                if (iInfrastructureLevel >= 4)
                                    cChances[pStripClubProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 2;
                            }
                            else if (DominantCulture.m_pCustoms.Has(Customs.Sexuality.Lecherous))
                            { 
                                cChances[pBrothelProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 2;
                                if (iInfrastructureLevel >= 4)
                                    cChances[pStripClubProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 2;
                            }

                            if (iInfrastructureLevel < 4)
                            {
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.InnSlvSmall : BuildingInfo.InnSmall] = DominantCulture.GetTrait(Trait.Simplicity) / 4;
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.TavernSlvSmall : BuildingInfo.TavernSmall] = DominantCulture.GetTrait(Trait.Simplicity);
                            }
                            else
                            {
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.HotelSlvMedium : BuildingInfo.HotelMedium] = DominantCulture.GetTrait(Trait.Simplicity) / 4;
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.BarSlvSmall : BuildingInfo.BarSmall] = DominantCulture.GetTrait(Trait.Simplicity);
                            }

                            cChances[BuildingInfo.TheatreMedium] = 1f - DominantCulture.GetTrait(Trait.Simplicity) / 2;

                            cChances[BuildingInfo.RoguesDenMedium] = DominantCulture.GetTrait(Trait.Treachery) / 2;
                            cChances[BuildingInfo.GamblingSmall] = DominantCulture.GetTrait(Trait.Treachery) / 2;

                            if (iInfrastructureLevel < 3)
                                cChances[BuildingInfo.MedicineSmall] = (float)cChances.Count / 12;
                            else
                                cChances[BuildingInfo.MedicineMedium] = (float)cChances.Count / 12;

                            float fBureaucracy = 0.05f;
                            if (DominantCulture.m_pCustoms.Has(Customs.MindSet.Balanced_mind))
                                fBureaucracy = 0.25f;
                            else if (DominantCulture.m_pCustoms.Has(Customs.MindSet.Logic))
                                fBureaucracy = 0.5f;

                            if (m_pTitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Sapient)
                                fBureaucracy *= 2;
                            if (m_pTitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Primitive)
                                fBureaucracy *= 4;

                            if (pSettlement.m_bCapital)
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
                                if (DominantCulture.m_pCustoms.Has(Customs.MindSet.Balanced_mind))
                                    fScience = 0.25f;
                                else if (DominantCulture.m_pCustoms.Has(Customs.MindSet.Logic))
                                    fScience = 0.5f;

                                if (m_pTitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Sapient)
                                    fScience *= 2;
                                if (m_pTitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Ingenious)
                                    fScience *= 4;

                                cChances[BuildingInfo.ScienceSmall] = fScience / 4;

                                cChances[BuildingInfo.SchoolSmall] = fScience;
                                cChances[BuildingInfo.SchoolMedium] = fScience / 2;
                            }

                            if (m_iSocialEquality == 0)
                            {
                                cChances[BuildingInfo.SlaveMarketMedium] = (float)cChances.Count / 6 + 1;
                                if (DominantCulture.m_pCustoms.Has(Customs.Sexuality.Moderate_sexuality))
                                {
                                    cChances[BuildingInfo.SlaveMarketMedium2] = (float)cChances.Count / 6 + 1;
                                }
                                else if (DominantCulture.m_pCustoms.Has(Customs.Sexuality.Lecherous))
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
                                cChances[BuildingInfo.OfficeMedium] = (float)cChances.Count / 4;// / 2 + 1;
                            }

                            if (m_pStateModel.m_bDinasty)
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.MansionSlvSmall : BuildingInfo.MansionSmall] = pSettlement.m_bCapital ? (float)cChances.Count / 2 : (float)cChances.Count / 4;
                        }

                        BuildingInfo pProfile;
                        switch (pSettlement.m_eSpeciality)
                        {
                            case SettlementSpeciality.Fishers:
                                pProfile = m_iSocialEquality == 0 ? BuildingInfo.FishingBoatSlvLarge : BuildingInfo.FishingBoatLarge;
                                break;
                            case SettlementSpeciality.Naval:
                                pProfile = m_iSocialEquality == 0 ? BuildingInfo.TraderSlvMedium : BuildingInfo.TraderMedium;
                                break;
                            case SettlementSpeciality.Tailors:
                                if (iInfrastructureLevel < 4)
                                    pProfile = m_iSocialEquality == 0 ? BuildingInfo.TailorWorkshopSlvSmall : BuildingInfo.TailorWorkshopSmall;
                                else
                                    pProfile = m_iSocialEquality == 0 ? BuildingInfo.ClothesFactorySlvSmall : BuildingInfo.ClothesFactorySmall;
                                break;
                            case SettlementSpeciality.Jevellers:
                                pProfile = m_iSocialEquality == 0 ? BuildingInfo.JevellerWorkshopSlvSmall : BuildingInfo.JevellerWorkshopSmall;
                                break;
                            case SettlementSpeciality.Factory:
                                if (iInfrastructureLevel < 4)
                                    pProfile = m_iSocialEquality == 0 ? BuildingInfo.SmithySlvSmall : BuildingInfo.SmithySmall;
                                else
                                    pProfile = m_iSocialEquality == 0 ? BuildingInfo.IronworksSlvSmall : BuildingInfo.IronworksSmall;
                                break;
                            case SettlementSpeciality.Artisans:
                                if (iInfrastructureLevel < 4)
                                    pProfile = m_iSocialEquality == 0 ? BuildingInfo.CarpentrySlvSmall : BuildingInfo.CarpentrySmall;
                                else
                                    pProfile = m_iSocialEquality == 0 ? BuildingInfo.FurnitureSlvSmall : BuildingInfo.FurnitureSmall;
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
                        if (pSettlement.m_cBuildings.Count > 0)// && Rnd.OneChanceFrom(2))
                        {
                            BuildingInfo pGuard;
                            if (iInfrastructureLevel < 4)
                                pGuard = BuildingInfo.GuardBarracksMedium;
                            else
                                if (iInfrastructureLevel == 8)
                                pGuard = BuildingInfo.EmergencyPostSmall;
                            else
                                pGuard = BuildingInfo.PoliceStationSmall;

                            cChances[pGuard] = (float)iControl * DominantCulture.GetTrait(Trait.Agression) / 4;

                            if (iInfrastructureLevel >= 4 && iInfrastructureLevel < 8)
                                cChances[BuildingInfo.PoliceStationMedium] = (float)iControl / 8;

                            BuildingInfo pPrison;
                            if (iInfrastructureLevel < 4)
                                pPrison = BuildingInfo.PrisonSmall;
                            else
                                if (iInfrastructureLevel == 8)
                                pPrison = BuildingInfo.HoldingSmall;
                            else
                                pPrison = BuildingInfo.PrisonPoliceSmall;

                            cChances[pPrison] = (float)iControl / 4;

                            cChances[BuildingInfo.ChurchMedium] = DominantCulture.GetTrait(Trait.Piety) / 5;
                            cChances[BuildingInfo.ChurchSmall] = DominantCulture.GetTrait(Trait.Piety);

                            BuildingInfo pBrothelProfile = m_iSocialEquality == 0 ? BuildingInfo.BrothelSlvMedium : BuildingInfo.BrothelMedium;
                            BuildingInfo pStripClubProfile = m_iSocialEquality == 0 ? BuildingInfo.StripClubSlvSmall : BuildingInfo.StripClubSmall;

                            if (DominantCulture.m_pCustoms.Has(Customs.Sexuality.Moderate_sexuality))
                            {
                                cChances[pBrothelProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 4;
                                if (iInfrastructureLevel >= 4)
                                    cChances[pStripClubProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 2;
                            }
                            else if (DominantCulture.m_pCustoms.Has(Customs.Sexuality.Lecherous))
                            { 
                                cChances[pBrothelProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 2;
                                if (iInfrastructureLevel >= 4)
                                    cChances[pStripClubProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 2;
                            }

                            if (iInfrastructureLevel < 4)
                            {
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.InnSlvSmall : BuildingInfo.InnSmall] = DominantCulture.GetTrait(Trait.Simplicity) / 4;
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.TavernSlvSmall : BuildingInfo.TavernSmall] = DominantCulture.GetTrait(Trait.Simplicity);
                                cChances[BuildingInfo.CircusMedium] = DominantCulture.GetTrait(Trait.Simplicity) * DominantCulture.GetTrait(Trait.Agression) / 2;
                            }
                            else
                            {
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.HotelSlvMedium : BuildingInfo.HotelMedium] = DominantCulture.GetTrait(Trait.Simplicity) / 4;
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.BarSlvSmall : BuildingInfo.BarSmall] = DominantCulture.GetTrait(Trait.Simplicity);
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.NightClubSlvMedium : BuildingInfo.NightClubMedium] = DominantCulture.GetTrait(Trait.Simplicity);
                            }

                            cChances[BuildingInfo.TheatreMedium] = 1f - DominantCulture.GetTrait(Trait.Simplicity) / 2;

                            cChances[BuildingInfo.RoguesDenMedium] = DominantCulture.GetTrait(Trait.Treachery) / 2;
                            cChances[BuildingInfo.GamblingSmall] = DominantCulture.GetTrait(Trait.Treachery) / 2;

                            if (iInfrastructureLevel < 3)
                                cChances[BuildingInfo.MedicineSmall] = (float)cChances.Count / 12;
                            else
                            {
                                cChances[BuildingInfo.MedicineMedium] = (float)cChances.Count / 12;
                                cChances[BuildingInfo.MedicineLarge] = (float)cChances.Count / 24;
                            }

                            float fBureaucracy = 0.05f;
                            if (DominantCulture.m_pCustoms.Has(Customs.MindSet.Balanced_mind))
                                fBureaucracy = 0.25f;
                            else if (DominantCulture.m_pCustoms.Has(Customs.MindSet.Logic))
                                fBureaucracy = 0.5f;

                            if (m_pTitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Sapient)
                                fBureaucracy *= 2;
                            if (m_pTitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Primitive)
                                fBureaucracy *= 4;

                            if (pSettlement.m_bCapital)
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
                                if (DominantCulture.m_pCustoms.Has(Customs.MindSet.Balanced_mind))
                                    fScience = 0.25f;
                                else if (DominantCulture.m_pCustoms.Has(Customs.MindSet.Logic))
                                    fScience = 0.5f;

                                if (m_pTitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Sapient)
                                    fScience *= 2;
                                if (m_pTitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Ingenious)
                                    fScience *= 4;

                                cChances[BuildingInfo.ScienceSmall] = fScience / 4;
                                cChances[BuildingInfo.ScienceMedium] = fScience / 8;

                                cChances[BuildingInfo.SchoolSmall] = fScience;
                                cChances[BuildingInfo.SchoolMedium] = fScience / 2;
                            }

                            if (m_iSocialEquality == 0)
                            {
                                cChances[BuildingInfo.SlaveMarketMedium] = (float)cChances.Count / 6 + 1;
                                if (DominantCulture.m_pCustoms.Has(Customs.Sexuality.Moderate_sexuality))
                                {
                                    cChances[BuildingInfo.SlaveMarketMedium2] = (float)cChances.Count / 6 + 1;
                                }
                                else if (DominantCulture.m_pCustoms.Has(Customs.Sexuality.Lecherous))
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

                            if (m_pStateModel.m_bDinasty)
                            {
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.MansionSlvSmall : BuildingInfo.MansionSmall] = pSettlement.m_bCapital ? (float)cChances.Count / 2 : (float)cChances.Count / 4;
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.MansionSlvMedium : BuildingInfo.MansionMedium] = pSettlement.m_bCapital ? (float)cChances.Count / 2 : (float)cChances.Count / 4;
                            }
                        }

                        BuildingInfo pProfile;
                        switch (pSettlement.m_eSpeciality)
                        {
                            case SettlementSpeciality.NavalAcademy:
                                pProfile = Rnd.OneChanceFrom(2) ? BuildingInfo.NavalAcademyHuge : (m_iSocialEquality == 0 ? BuildingInfo.TraderSlvLarge : BuildingInfo.TraderLarge);
                                break;
                            case SettlementSpeciality.Naval:
                                pProfile = m_iSocialEquality == 0 ? BuildingInfo.TraderSlvLarge : BuildingInfo.TraderLarge;
                                break;
                            case SettlementSpeciality.Resort:
                                pProfile = Rnd.OneChanceFrom(2) ? (m_iSocialEquality == 0 ? BuildingInfo.HotelSlvMedium : BuildingInfo.HotelMedium) : (m_iSocialEquality == 0 ? BuildingInfo.HotelSlvLarge : BuildingInfo.HotelLarge);
                                break;
                            case SettlementSpeciality.Cultural:
                                pProfile = Rnd.OneChanceFrom(2) ? BuildingInfo.CultureMeduim : BuildingInfo.CultureLarge;
                                break;
                            case SettlementSpeciality.ArtsAcademy:
                                pProfile = iInfrastructureLevel < 4 ? (DominantCulture.GetTrait(Trait.Simplicity) > Rnd.Get(2f) ? BuildingInfo.CircusMedium : BuildingInfo.TheatreMedium) : BuildingInfo.CinemaLarge;
                                break;
                            case SettlementSpeciality.Religious:
                                pProfile = BuildingInfo.ChurchLarge;
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
                                pProfile = m_iSocialEquality == 0 ? BuildingInfo.ClothesFactorySlvMedium : BuildingInfo.ClothesFactoryMedium;
                                break;
                            case SettlementSpeciality.Jevellers:
                                pProfile = m_iSocialEquality == 0 ? BuildingInfo.JevellerWorkshopSlvMedium : BuildingInfo.JevellerWorkshopMedium;
                                break;
                            case SettlementSpeciality.Factory:
                                pProfile = m_iSocialEquality == 0 ? BuildingInfo.IronworksSlvMedium : BuildingInfo.IronworksMedium;
                                break;
                            case SettlementSpeciality.Artisans:
                                pProfile = m_iSocialEquality == 0 ? BuildingInfo.FurnitureSlvMedium : BuildingInfo.FurnitureMedium;
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
                        if (pSettlement.m_cBuildings.Count > 0)// && Rnd.OneChanceFrom(2))
                        {
                            BuildingInfo pGuard;
                            if (iInfrastructureLevel < 4)
                                pGuard = BuildingInfo.GuardBarracksMedium;
                            else
                                if (iInfrastructureLevel == 8)
                                pGuard = BuildingInfo.EmergencyPostSmall;
                            else
                                pGuard = BuildingInfo.PoliceStationSmall;

                            cChances[pGuard] = (float)iControl * DominantCulture.GetTrait(Trait.Agression) / 4;

                            if (iInfrastructureLevel >= 4 && iInfrastructureLevel < 8)
                                cChances[BuildingInfo.PoliceStationMedium] = (float)iControl / 8;

                            BuildingInfo pPrison;
                            if (iInfrastructureLevel < 4)
                                pPrison = BuildingInfo.PrisonMedium;
                            else
                                if (iInfrastructureLevel == 8)
                                pPrison = BuildingInfo.HoldingMedium;
                            else
                                pPrison = BuildingInfo.PrisonPoliceMedium;

                            cChances[pPrison] = (float)iControl / 4;

                            cChances[BuildingInfo.ChurchMedium] = DominantCulture.GetTrait(Trait.Piety) / 5;
                            cChances[BuildingInfo.ChurchSmall] = DominantCulture.GetTrait(Trait.Piety);

                            BuildingInfo pBrothelProfile = m_iSocialEquality == 0 ? BuildingInfo.BrothelSlvMedium : BuildingInfo.BrothelMedium;
                            BuildingInfo pStripClubProfile = m_iSocialEquality == 0 ? BuildingInfo.StripClubSlvSmall : BuildingInfo.StripClubSmall;

                            if (DominantCulture.m_pCustoms.Has(Customs.Sexuality.Moderate_sexuality))
                            {
                                cChances[pBrothelProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 4;
                                if (iInfrastructureLevel >= 4)
                                    cChances[pStripClubProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 2;
                            }
                            else if (DominantCulture.m_pCustoms.Has(Customs.Sexuality.Lecherous))
                            {
                                cChances[pBrothelProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 2;
                                if (iInfrastructureLevel >= 4)
                                    cChances[pStripClubProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 2;
                            }

                            if (iInfrastructureLevel < 4)
                            {
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.InnSlvSmall : BuildingInfo.InnSmall] = DominantCulture.GetTrait(Trait.Simplicity) / 4;
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.TavernSlvSmall : BuildingInfo.TavernSmall] = DominantCulture.GetTrait(Trait.Simplicity);
                                cChances[BuildingInfo.CircusMedium] = DominantCulture.GetTrait(Trait.Simplicity) * DominantCulture.GetTrait(Trait.Agression) / 2;
                            }
                            else
                            {
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.HotelSlvMedium : BuildingInfo.HotelMedium] = DominantCulture.GetTrait(Trait.Simplicity) / 4;
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.BarSlvSmall : BuildingInfo.BarSmall] = DominantCulture.GetTrait(Trait.Simplicity);
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.NightClubSlvMedium : BuildingInfo.NightClubMedium] = DominantCulture.GetTrait(Trait.Simplicity);
                            }

                            cChances[BuildingInfo.TheatreMedium] = 1f - DominantCulture.GetTrait(Trait.Simplicity) / 2;

                            cChances[BuildingInfo.RoguesDenMedium] = DominantCulture.GetTrait(Trait.Treachery) / 2;
                            cChances[BuildingInfo.GamblingSmall] = DominantCulture.GetTrait(Trait.Treachery) / 2;

                            if (iInfrastructureLevel < 3)
                                cChances[BuildingInfo.MedicineSmall] = (float)cChances.Count / 12;
                            else
                            {
                                cChances[BuildingInfo.MedicineMedium] = (float)cChances.Count / 12;
                                cChances[BuildingInfo.MedicineLarge] = (float)cChances.Count / 24;
                            }

                            float fBureaucracy = 0.05f;
                            if (DominantCulture.m_pCustoms.Has(Customs.MindSet.Balanced_mind))
                                fBureaucracy = 0.25f;
                            if (DominantCulture.m_pCustoms.Has(Customs.MindSet.Logic))
                                fBureaucracy = 0.5f;

                            if (m_pTitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Sapient)
                                fBureaucracy *= 2;
                            if (m_pTitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Primitive)
                                fBureaucracy *= 4;

                            if (pSettlement.m_bCapital)
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
                                if (DominantCulture.m_pCustoms.Has(Customs.MindSet.Balanced_mind))
                                    fScience = 0.25f;
                                else if (DominantCulture.m_pCustoms.Has(Customs.MindSet.Logic))
                                    fScience = 0.5f;

                                if (m_pTitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Sapient)
                                    fScience *= 2;
                                if (m_pTitularNation.DominantPhenotype.m_pValues.Get<BrainGenetix>().Intelligence == Intelligence.Ingenious)
                                    fScience *= 4;

                                cChances[BuildingInfo.ScienceSmall] = fScience / 4;
                                cChances[BuildingInfo.ScienceMedium] = fScience / 8;

                                cChances[BuildingInfo.SchoolSmall] = fScience;
                                cChances[BuildingInfo.SchoolMedium] = fScience / 2;
                            }

                            if (m_iSocialEquality == 0)
                            {
                                cChances[BuildingInfo.SlaveMarketMedium] = (float)cChances.Count / 6 + 1;
                                if (DominantCulture.m_pCustoms.Has(Customs.Sexuality.Moderate_sexuality))
                                {
                                    cChances[BuildingInfo.SlaveMarketMedium2] = (float)cChances.Count / 6 + 1;
                                }
                                if (DominantCulture.m_pCustoms.Has(Customs.Sexuality.Lecherous))
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

                            if (m_pStateModel.m_bDinasty)
                            {
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.MansionSlvSmall : BuildingInfo.MansionSmall] = pSettlement.m_bCapital ? (float)cChances.Count / 2 : (float)cChances.Count / 4;
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.MansionSlvMedium : BuildingInfo.MansionMedium] = pSettlement.m_bCapital ? (float)cChances.Count / 2 : (float)cChances.Count / 4;
                            }
                        }

                        BuildingInfo pProfile;
                        switch (pSettlement.m_eSpeciality)
                        {
                            case SettlementSpeciality.NavalAcademy:
                                pProfile = Rnd.OneChanceFrom(2) ? BuildingInfo.NavalAcademyHuge : (m_iSocialEquality == 0 ? BuildingInfo.TraderSlvLarge : BuildingInfo.TraderLarge);
                                break;
                            case SettlementSpeciality.Naval:
                                pProfile = m_iSocialEquality == 0 ? BuildingInfo.TraderSlvLarge : BuildingInfo.TraderLarge;
                                break;
                            case SettlementSpeciality.Resort:
                                pProfile = Rnd.OneChanceFrom(2) ? (m_iSocialEquality == 0 ? BuildingInfo.HotelSlvMedium : BuildingInfo.HotelMedium) : (m_iSocialEquality == 0 ? BuildingInfo.HotelSlvLarge : BuildingInfo.HotelLarge);
                                break;
                            case SettlementSpeciality.Cultural:
                                pProfile = Rnd.OneChanceFrom(2) ? BuildingInfo.CultureMeduim : BuildingInfo.CultureLarge;
                                break;
                            case SettlementSpeciality.ArtsAcademy:
                                pProfile = iInfrastructureLevel < 4 ? (DominantCulture.GetTrait(Trait.Simplicity) > Rnd.Get(2f) ? BuildingInfo.CircusMedium : BuildingInfo.TheatreMedium) : BuildingInfo.CinemaLarge;
                                break;
                            case SettlementSpeciality.Religious:
                                pProfile = BuildingInfo.ChurchLarge;
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
                                pProfile = m_iSocialEquality == 0 ? BuildingInfo.ClothesFactorySlvMedium : BuildingInfo.ClothesFactoryMedium;
                                break;
                            case SettlementSpeciality.Jevellers:
                                pProfile = m_iSocialEquality == 0 ? BuildingInfo.JevellerWorkshopSlvMedium : BuildingInfo.JevellerWorkshopMedium;
                                break;
                            case SettlementSpeciality.Factory:
                                pProfile = m_iSocialEquality == 0 ? BuildingInfo.IronworksSlvSmall : BuildingInfo.IronworksMedium;
                                break;
                            case SettlementSpeciality.Artisans:
                                pProfile = m_iSocialEquality == 0 ? BuildingInfo.FurnitureSlvMedium : BuildingInfo.FurnitureMedium;
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
                        if (pSettlement.m_cBuildings.Count > 0)// && Rnd.OneChanceFrom(2))
                        {
                            cChances[BuildingInfo.ChurchMedium] = DominantCulture.GetTrait(Trait.Piety) / 5;
                            cChances[BuildingInfo.ChurchSmall] = DominantCulture.GetTrait(Trait.Piety);

                            BuildingInfo pBrothelProfile = m_iSocialEquality == 0 ? BuildingInfo.BrothelSlvMedium : BuildingInfo.BrothelMedium;
                            BuildingInfo pStripClubProfile = m_iSocialEquality == 0 ? BuildingInfo.StripClubSlvSmall : BuildingInfo.StripClubSmall;

                            if (DominantCulture.m_pCustoms.Has(Customs.Sexuality.Moderate_sexuality))
                            {
                                cChances[pBrothelProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 4;
                                if (iInfrastructureLevel >= 4)
                                    cChances[pStripClubProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 2;
                            }
                            if (DominantCulture.m_pCustoms.Has(Customs.Sexuality.Lecherous))
                            {
                                cChances[pBrothelProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 2;
                                if (iInfrastructureLevel >= 4)
                                    cChances[pStripClubProfile] = DominantCulture.GetTrait(Trait.Simplicity) / 2;
                            }

                            if (iInfrastructureLevel < 4)
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.TavernSlvSmall : BuildingInfo.TavernSmall] = DominantCulture.GetTrait(Trait.Simplicity) / 2;
                            else
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.BarSlvSmall : BuildingInfo.BarSmall] = DominantCulture.GetTrait(Trait.Simplicity) / 2;

                            //if (pState.m_iSocialEquality == 0)
                            //{
                            //    cChances[BuildingInfo.SlavePensMedium] = (float)cChances.Count / 2;
                            //}
                        }

                        BuildingInfo pProfile;
                        switch (pSettlement.m_eSpeciality)
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

            foreach (Building pBuilding in pSettlement.m_cBuildings)
                if (cChances.ContainsKey(pBuilding.m_pInfo))
                {
                    float fKoeff = 1;
                    switch (pBuilding.m_pInfo.m_eSize)
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
                    cChances[pBuilding.m_pInfo] *= fKoeff;
                }

            int iChance = Rnd.ChooseOne(cChances.Values);
            return cChances.ElementAt(iChance).Key;
        }

        internal void CalculateSocietyFeatures(int iEmpireTreshold)
        {
            // Adjustiong TL due to lack or abundance of resouces
            CheckResources(m_pState.m_iWood, m_pState.m_iOre, m_pState.m_iFood, m_pState.m_iPopulation, m_pState.m_cContents.Count);

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
            List<StateModel> cInfos = new List<StateModel>();

            foreach (StateModel pInfo in s_aModels)
            {
                if (m_iInfrastructureLevel >= pInfo.m_iMinGovernmentLevel &&
                    m_iInfrastructureLevel <= pInfo.m_iMaxGovernmentLevel &&
                    (pInfo.m_cLanguages.Count == 0 ||
                     pInfo.m_cLanguages.Contains(m_pTitularNation.m_pRace.m_pLanguage)) &&
                    (m_pState.m_iPopulation > iEmpireTreshold * 80) == pInfo.m_bBig)
                {
                    for (int i = 0; i < pInfo.m_iRank; i++)
                        cInfos.Add(pInfo);
                }
            }

            if (cInfos.Count == 0)
            {
                foreach (StateModel pInfo in s_aModels)
                {
                    if (m_iInfrastructureLevel >= pInfo.m_iMinGovernmentLevel &&
                        m_iInfrastructureLevel <= pInfo.m_iMaxGovernmentLevel &&
                        (m_pState.m_iPopulation > iEmpireTreshold * 80) == pInfo.m_bBig)
                    {
                        for (int i = 0; i < pInfo.m_iRank; i++)
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

            m_pStateModel = cInfos[Rnd.Get(cInfos.Count)];

            m_cCulture[Gender.Male].m_iProgressLevel = (m_iInfrastructureLevel + m_pStateModel.m_iMinGovernmentLevel) / 2;
            m_cCulture[Gender.Female].m_iProgressLevel = (m_iInfrastructureLevel + m_pStateModel.m_iMinGovernmentLevel) / 2;
        }

        private void SetSocialEquality()
        {
            m_iSocialEquality = 2;

            if (DominantCulture.GetTrait(Trait.Agression) > 1.66)
                m_iSocialEquality--;
            //if (m_pCulture.MentalityValues[Mentality.Agression][m_iCultureLevel] > 1.33)
            //    m_iSocialEquality--;
            //if (m_pCulture.MentalityValues[Mentality.Agression][m_iCultureLevel] > 1)
            //    m_iSocialEquality--;

            if (DominantCulture.GetTrait(Trait.Fanaticism) > 1.66)
                m_iSocialEquality--;
            if (DominantCulture.GetTrait(Trait.Fanaticism) > 1.33)
                m_iSocialEquality--;
            //if (m_pCulture.MentalityValues[Mentality.Fanaticism][m_iTechLevel] > 1)
            //    m_iSocialEquality--;

            if (DominantCulture.GetTrait(Trait.Selfishness) > 1.66)
                m_iSocialEquality--;
            if (DominantCulture.GetTrait(Trait.Selfishness) > 1.33)
                m_iSocialEquality--;
            //if (m_pCulture.MentalityValues[Mentality.Selfishness][m_iCultureLevel] > 1)
            //    m_iSocialEquality--;

            if (DominantCulture.GetTrait(Trait.Treachery) > 1.66)
                m_iSocialEquality--;
            if (DominantCulture.GetTrait(Trait.Treachery) > 1.33)
                m_iSocialEquality--;
            //if (m_pCulture.MentalityValues[Mentality.Treachery][m_iCultureLevel] > 1)
            //    m_iSocialEquality--;

            if (m_pStateModel.m_bDinasty)
                m_iSocialEquality--;

            if (m_iSocialEquality < 0)
                m_iSocialEquality = 0;

            if (DominantCulture.GetTrait(Trait.Agression) < 1)
                m_iSocialEquality++;

            if (DominantCulture.GetTrait(Trait.Fanaticism) < 1)
                m_iSocialEquality++;

            if (m_iSocialEquality > 0 && m_pState.m_iFood < m_pState.m_iPopulation)
                m_iSocialEquality--;
            if (m_pState.m_iFood > m_pState.m_iPopulation && m_pState.m_iOre > m_pState.m_iPopulation && m_pState.m_iWood > m_pState.m_iPopulation)
                m_iSocialEquality++;

            //в либеральном обществе (фанатизм < 2/3) не может быть рабства (0) или крепостного права (1), т.е. только 2 и выше
            if (DominantCulture.GetTrait(Trait.Fanaticism) < 0.66)
                m_iSocialEquality = Math.Max(2, m_iSocialEquality);
            //в обществе абсолютных пацифистов (агрессивность < 1/3) не может быть даже капитализма (2), т.е. только 3 и выше
            if (DominantCulture.GetTrait(Trait.Agression) < 0.33)
                m_iSocialEquality = Math.Max(3, m_iSocialEquality);
            //в обществе абсолютного самоотречения (эгоизм < 1/3) не может быть капитализма (2) - только или социализм, или феодализм
            if (DominantCulture.GetTrait(Trait.Selfishness) < 0.33)
                if (m_pStateModel.m_bDinasty)
                    m_iSocialEquality = Math.Min(1, m_iSocialEquality);
                else
                    m_iSocialEquality = Math.Max(3, m_iSocialEquality);
            //эгоизм и коммунизм не совместимы
            if (DominantCulture.GetTrait(Trait.Selfishness) > 1)
                m_iSocialEquality = Math.Min(3, m_iSocialEquality);
            //преступный склад ума и социализм не совместимы
            if (DominantCulture.GetTrait(Trait.Treachery) > 0.66)
                m_iSocialEquality = Math.Min(2, m_iSocialEquality);

            //коммунизм возможен только в условиях изобилия ресурсов
            if (m_pState.m_iFood < m_pState.m_iPopulation * 2 || m_pState.m_iOre < m_pState.m_iPopulation * 2 || m_pState.m_iWood < m_pState.m_iPopulation * 2)
                m_iSocialEquality = Math.Min(3, m_iSocialEquality);

            //при всём уважении - какой нафиг социализм/коммунизм при наследственной власти???
            if (m_pStateModel.m_bDinasty)
                m_iSocialEquality = Math.Min(2, m_iSocialEquality);

            if (m_iSocialEquality > 4)
                m_iSocialEquality = 4;
        }

        private void SetStateControl()
        {
            m_iControl = 2;

            //if (m_pCulture.Moral[Culture.Morale.Agression] > 1)
            //    m_iControl++;

            if (m_pStateModel.m_bDinasty)
                m_iControl++;
            if (DominantCulture.GetTrait(Trait.Fanaticism) > 1.33)
                m_iControl++;
            if (DominantCulture.GetTrait(Trait.Fanaticism) > 1.66)
                m_iControl++;
            if (DominantCulture.GetTrait(Trait.Fanaticism) < 0.33)
                m_iControl--;

            if (DominantCulture.GetTrait(Trait.Selfishness) > 1.66)
                m_iControl--;

            if (m_iInfrastructureLevel == 0)
                m_iControl = 0;
            if (m_iControl == 0 && m_iInfrastructureLevel >= 1 && m_iInfrastructureLevel <= 6)
                m_iControl = 1;

            if (m_iControl < 0)
                m_iControl = 0;
            if (m_iControl > 4)
                m_iControl = 4;
        }
        
        public void CalculateMagic()
        {
            m_iMagicLimit = 0;

            Dictionary<Gender, float[]> aDistribution = new Dictionary<Gender, float[]>
            {
                {Gender.Male, new float[10] },
                {Gender.Female, new float[10] }
            };

            foreach (Province pProvince in m_pState.m_cContents)
            {
                if (pProvince.m_pLocalSociety.m_iMagicLimit > m_iMagicLimit)
                    m_iMagicLimit = pProvince.m_pLocalSociety.m_iMagicLimit;

                float fPrevalence = 1;
                if (pProvince.m_pLocalSociety.DominantCulture.m_pCustoms.Has(Customs.Magic.Magic_Feared))
                {
                    fPrevalence = 0.1f;
                }
                else if (pProvince.m_pLocalSociety.DominantCulture.m_pCustoms.Has(Customs.Magic.Magic_Allowed))
                {
                    fPrevalence = 0.5f;
                }
                else if (pProvince.m_pLocalSociety.DominantCulture.m_pCustoms.Has(Customs.Magic.Magic_Praised))
                {
                    fPrevalence = 0.9f;
                }

                Dictionary<Gender, float> cProvinceMagesCount = new Dictionary<Gender, float>()
                {
                    { Gender.Male, 0 },
                    { Gender.Female, 0 }
                };
                foreach (LandX pLand in pProvince.m_cContents)
                {
                    cProvinceMagesCount[Gender.Male] += pLand.m_cContents.Count * fPrevalence;
                    cProvinceMagesCount[Gender.Female] += pLand.m_cContents.Count * fPrevalence;
                }

                switch (pProvince.m_pLocalSociety.m_pTitularNation.m_pPhenotypeM.m_pValues.Get<LifeCycleGenetix>().BirthRate)
                {
                    case BirthRate.Low:
                        cProvinceMagesCount[Gender.Male] *= 0.1f;
                        break;
                    case BirthRate.Moderate:
                        cProvinceMagesCount[Gender.Male] *= 0.25f;
                        break;
                }

                switch (pProvince.m_pLocalSociety.m_pTitularNation.m_pPhenotypeF.m_pValues.Get<LifeCycleGenetix>().BirthRate)
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
                    switch (pProvince.m_pLocalSociety.m_cCulture[distribution.Key].m_eMagicAbilityDistribution)
                    {
                        case MagicAbilityDistribution.mostly_weak:
                            distribution.Value[(1 + pProvince.m_pLocalSociety.m_iMagicLimit) / 2] += cProvinceMagesCount[distribution.Key];
                            break;
                        case MagicAbilityDistribution.mostly_average:
                            distribution.Value[(1 + pProvince.m_pLocalSociety.m_iMagicLimit) / 2] += cProvinceMagesCount[distribution.Key] / 2;
                            distribution.Value[1 + pProvince.m_pLocalSociety.m_iMagicLimit] += cProvinceMagesCount[distribution.Key] / 2;
                            break;
                        case MagicAbilityDistribution.mostly_powerful:
                            distribution.Value[1 + pProvince.m_pLocalSociety.m_iMagicLimit] += cProvinceMagesCount[distribution.Key];
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
                    if (i <= (m_iMagicLimit + 1) / 2)
                        fWeakMagesCount += distribution.Value[i];
                    else
                        fPowerfulMagesCount += distribution.Value[i];
                }

                m_cCulture[distribution.Key].m_eMagicAbilityDistribution = MagicAbilityDistribution.mostly_average;

                if (fWeakMagesCount > fPowerfulMagesCount * 2)
                    m_cCulture[distribution.Key].m_eMagicAbilityDistribution = MagicAbilityDistribution.mostly_weak;

                if (fPowerfulMagesCount > fWeakMagesCount * 2)
                    m_cCulture[distribution.Key].m_eMagicAbilityDistribution = MagicAbilityDistribution.mostly_powerful;
            }
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

            if (m_pTitularNation != pOpponent.m_pSociety.m_pTitularNation)
            {
                iHostility++;
                sNegativeReasons += " (-1) " + pOpponent.m_pSociety.m_pTitularNation.ToString() + "\n";

                if (m_pTitularNation.m_pRace.m_pLanguage != pOpponent.m_pSociety.m_pTitularNation.m_pRace.m_pLanguage)
                {
                    iHostility++;
                    sNegativeReasons += " (-1) Different language\n";
                }
            }
            else
            {
                iHostility--;
                sPositiveReasons += " (+1) " + pOpponent.m_pSociety.m_pTitularNation.ToString() + "\n";
            }

            iHostility += DominantCulture.m_pCustoms.GetDifference(pOpponent.m_pSociety.DominantCulture.m_pCustoms, ref sPositiveReasons, ref sNegativeReasons);

            if (m_pState.m_iFood < m_pState.m_iPopulation && pOpponent.m_iFood > pOpponent.m_iPopulation * 2)
            {
                iHostility++;
                sNegativeReasons += " (-1) Envy for food\n";
            }
            if (m_pState.m_iWood < m_pState.m_iPopulation && pOpponent.m_iWood > pOpponent.m_iPopulation * 2)
            {
                iHostility++;
                sNegativeReasons += " (-1) Envy for wood\n";
            }
            if (m_pState.m_iOre < m_pState.m_iPopulation && pOpponent.m_iOre > pOpponent.m_iPopulation * 2)
            {
                iHostility++;
                sNegativeReasons += " (-1) Envy for ore\n";
            }

            int iControlDifference = Math.Abs(pOpponent.m_pSociety.m_iControl - m_iControl);
            if (iControlDifference != 1)
            {
                iHostility += iControlDifference - 1;
                if (iControlDifference > 1)
                    sNegativeReasons += string.Format(" (-{1}) {0}\n", GetControlString(pOpponent.m_pSociety.m_iControl), iControlDifference - 1);
                else
                    sPositiveReasons += string.Format(" (+{1}) {0}\n", GetControlString(pOpponent.m_pSociety.m_iControl), 1);
            }

            if (pOpponent.m_pSociety.m_iInfrastructureLevel > m_iInfrastructureLevel + 1)
            {
                iHostility++;//= pOpponent.m_iLifeLevel - m_iLifeLevel;
                sNegativeReasons += string.Format(" (-{0}) Envy for civilization\n", 1);//pOpponent.m_iLifeLevel - m_iLifeLevel);
            }
            else
            {
                if (pOpponent.m_pSociety.m_iInfrastructureLevel < m_iInfrastructureLevel - 1)
                {
                    iHostility++;//= m_iLifeLevel - pOpponent.m_iLifeLevel;
                    sNegativeReasons += string.Format(" (-{0}) Scorn for savagery\n", 1);//m_iLifeLevel - pOpponent.m_iLifeLevel);
                }
            }

            int iEqualityDifference = Math.Abs(pOpponent.m_pSociety.m_iSocialEquality - m_iSocialEquality);
            if (iEqualityDifference != 1)
            {
                iHostility += iEqualityDifference - 1;
                if (iEqualityDifference > 1)
                    sNegativeReasons += string.Format(" (-{1}) {0}\n", GetEqualityString(pOpponent.m_pSociety.m_iSocialEquality), iEqualityDifference - 1);
                else
                    sPositiveReasons += string.Format(" (+{1}) {0}\n", GetEqualityString(pOpponent.m_pSociety.m_iSocialEquality), 1);
            }

            float iCultureDifference = DominantCulture.m_pMentality.GetDifference(pOpponent.m_pSociety.DominantCulture.m_pMentality, DominantCulture.m_iProgressLevel, pOpponent.m_pSociety.DominantCulture.m_iProgressLevel);
            if (iCultureDifference < -0.75)
            {
                iHostility -= 2;
                sPositiveReasons += " (+2) Very close culture\n";
            }
            else
                if (iCultureDifference < -0.5)
            {
                iHostility--;
                sPositiveReasons += " (+1) Close culture\n";
            }
            else
                    if (iCultureDifference > 0.5)
            {
                iHostility += 2;
                sNegativeReasons += " (-2) Very different culture\n";
            }
            else
                        if (iCultureDifference > 0)
            {
                iHostility++;
                sNegativeReasons += " (-1) Different culture\n";
            }

            sReasons = "Good:\n" + sPositiveReasons + "Bad:\n" + sNegativeReasons + "----\n";

            if (iHostility > 0)
            {
                iHostility = (int)(DominantCulture.GetTrait(Trait.Fanaticism) * iHostility + 0.25);
                sReasons += string.Format("Fanaticism \t(x{0}%)\n", (int)(DominantCulture.GetTrait(Trait.Fanaticism) * 100));

                iHostility = (int)(DominantCulture.GetTrait(Trait.Agression) * iHostility + 0.25);
                sReasons += string.Format("Agression \t(x{0}%)\n", (int)(DominantCulture.GetTrait(Trait.Agression) * 100));

                if (iHostility == 0)
                    iHostility = 1;
            }
            else
            {
                if (iHostility < 0)
                {
                    iHostility = (int)((2.0f - DominantCulture.GetTrait(Trait.Fanaticism)) * iHostility - 0.25);
                    sReasons += string.Format("Tolerance \t(x{0}%)\n", (int)((2.0f - DominantCulture.GetTrait(Trait.Fanaticism)) * 100));

                    iHostility = (int)((2.0f - DominantCulture.GetTrait(Trait.Agression)) * iHostility - 0.25);
                    sReasons += string.Format("Amiability \t(x{0}%)\n", (int)((2.0f - DominantCulture.GetTrait(Trait.Agression)) * 100));

                    if (iHostility == 0)
                        iHostility = -1;
                }
            }

            //if (fContact < fBorder / 2)
            //    iHostility = iHostility / 2;

            sReasons += string.Format("----\nTotal \t({0:+#;-#;0})\n", -iHostility);
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
            if (m_cCulture[Gender.Male].m_pCustoms.Has(Customs.GenderPriority.Matriarchy))
            {
                if (m_pTitularNation.m_pPhenotypeF.m_pValues.Get<LifeCycleGenetix>().BirthRate >
                    m_pTitularNation.m_pPhenotypeM.m_pValues.Get<LifeCycleGenetix>().BirthRate)
                    return Customs.GenderPriority.Genders_equality;
                else
                    return Customs.GenderPriority.Patriarchy;
            }
            if (m_cCulture[Gender.Male].m_pCustoms.Has(Customs.GenderPriority.Patriarchy))
            {
                if (m_pTitularNation.m_pPhenotypeM.m_pValues.Get<LifeCycleGenetix>().BirthRate >
                    m_pTitularNation.m_pPhenotypeF.m_pValues.Get<LifeCycleGenetix>().BirthRate)
                    return Customs.GenderPriority.Genders_equality;
                else
                    return Customs.GenderPriority.Matriarchy;
            }

            return base.GetMinorGender();
        }
    }
}
