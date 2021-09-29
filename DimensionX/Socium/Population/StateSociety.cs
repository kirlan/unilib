using GeneLab.Genetix;
using Random;
using Socium.Nations;
using Socium.Psichology;
using Socium.Settlements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Socium.State;
using Socium.Languages;

namespace Socium.Population
{
    public class StateSociety: Society
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
        }

        internal static StateModel[] s_aModels =
        {
            new StateModel("Land", 1,
                new SettlementInfo(SettlementSize.Hamlet, "Hamlet", 5, 10, 2, new BuildingInfo("Elder's hut", ProfessionInfo.RulerPrimitive, ProfessionInfo.HeirPrimitive, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Hamlet, "Hamlet", 5, 10, 2, new BuildingInfo("Elder's hut", ProfessionInfo.GovernorPrimitive, ProfessionInfo.GovernorPrimitive, BuildingSize.Unique)),
                true, 0, 0, false, SocialOrder.Primitive, null),
            new StateModel("Lands", 1,
                new SettlementInfo(SettlementSize.Hamlet, "Hamlet", 5, 10, 2, new BuildingInfo("Elder's hut", ProfessionInfo.RulerPrimitive, ProfessionInfo.HeirPrimitive, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Hamlet, "Hamlet", 5, 10, 2, new BuildingInfo("Elder's hut", ProfessionInfo.GovernorPrimitive, ProfessionInfo.GovernorPrimitive, BuildingSize.Unique)),
                true, 0, 0, true, SocialOrder.Primitive, null),
            new StateModel("Tribes", 1,
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, 3, new BuildingInfo("Clans hall", ProfessionInfo.RulerPrimitive, ProfessionInfo.HeirPrimitive, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, 3, new BuildingInfo("Village hall", ProfessionInfo.GovernorPrimitive, ProfessionInfo.GovernorPrimitive, BuildingSize.Unique)),
                true, 1, 1, false, SocialOrder.Primitive, null),
            new StateModel("Clans", 1,
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, 3, new BuildingInfo("Clans hall", ProfessionInfo.RulerPrimitive, ProfessionInfo.HeirPrimitive, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, 3, new BuildingInfo("Village hall", ProfessionInfo.GovernorPrimitive, ProfessionInfo.GovernorPrimitive, BuildingSize.Unique)),
                true, 1, 1, true, SocialOrder.Primitive, null),
            new StateModel("Kingdom", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.KingEuro, ProfessionInfo.KingHeirEuro, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorEuro, ProfessionInfo.GovernorEuro, BuildingSize.Unique)),
                true, 2, 6, false, SocialOrder.MedievalEurope, new Language[] {Language.Dwarwen, Language.European, Language.Highlander}),
            new StateModel("Kingdom", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.KingEuro, ProfessionInfo.KingHeirEuro, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorEuro2, ProfessionInfo.GovernorEuro2, BuildingSize.Unique)),
                true, 2, 6, false, SocialOrder.MedievalEurope2, new Language[] {Language.Drow, Language.Elven, Language.European, Language.Highlander}),
            new StateModel("Empire", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.EmperorEuro, ProfessionInfo.KingHeirEuro, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorEuro3, ProfessionInfo.GovernorEuro3, BuildingSize.Unique)),
                true, 2, 6, true, SocialOrder.MedievalEurope2, new Language[] {Language.Drow, Language.Elven, Language.European, Language.Dwarwen}),
            new StateModel("Kingdom", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.KingEuro, ProfessionInfo.KingHeirEuro, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorEuro4, ProfessionInfo.GovernorEuro4, BuildingSize.Unique)),
                true, 1, 6, false, SocialOrder.MedievalEurope3, new Language[] {Language.Dwarwen, Language.European, Language.Highlander}),
            new StateModel("Reich", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.KingNorth, ProfessionInfo.KingHeirNorth, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorNorth, ProfessionInfo.GovernorNorth, BuildingSize.Unique)),
                true, 1, 6, false, SocialOrder.MedievalNorth, new Language[] {Language.Dwarwen, Language.Northman}),
            new StateModel("Kaiserreich", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.EmperorNorth, ProfessionInfo.EmperorHeirNorth, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorNorth2, ProfessionInfo.GovernorNorth2, BuildingSize.Unique)),
                true, 2, 6, true, SocialOrder.MedievalNorth, new Language[] {Language.Dwarwen, Language.Northman}),
            new StateModel("Regnum", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.KingLatin, ProfessionInfo.KingHeirLatin, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorLatin, ProfessionInfo.GovernorLatin, BuildingSize.Unique)),
                true, 1, 6, false, SocialOrder.MedievalLatin, new Language[] {Language.Latin}),
            new StateModel("Imperium", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.EmperorLatin, ProfessionInfo.KingHeirLatin, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.KingLatin, ProfessionInfo.KingHeirLatin, BuildingSize.Small)),
                true, 2, 6, true, SocialOrder.MedievalLatin, new Language[] {Language.Latin}),
            new StateModel("Shogunate", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.KingAsian, ProfessionInfo.KingHeirAsian, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorAsian, ProfessionInfo.GovernorAsian, BuildingSize.Unique)),
                true, 1, 6, false, SocialOrder.MedievalAsian, new Language[] {Language.Asian}),
            new StateModel("Empire", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.EmperorAsian, ProfessionInfo.EmperorHeirAsian, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorAsian2, ProfessionInfo.GovernorAsian2, BuildingSize.Unique)),
                true, 2, 6, true, SocialOrder.MedievalAsian, new Language[] {Language.Asian}),
            new StateModel("Shahdom", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.KingArabian, ProfessionInfo.KingHeirArabian, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorArabian, ProfessionInfo.GovernorArabian, BuildingSize.Unique)),
                true, 1, 6, false, SocialOrder.MedievalArabian, new Language[] {Language.Arabian}),
            new StateModel("Sultanate", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.KingArabian2, ProfessionInfo.KingHeirArabian2, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorArabian, ProfessionInfo.GovernorArabian, BuildingSize.Unique)),
                true, 1, 6, false, SocialOrder.MedievalArabian, new Language[] {Language.Arabian, Language.African}),
            new StateModel("Caliphate", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.EmperorArabian, ProfessionInfo.KingHeirEuro, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorArabian, ProfessionInfo.GovernorArabian, BuildingSize.Unique)),
                true, 2, 6, true, SocialOrder.MedievalArabian, new Language[] {Language.Arabian, Language.African}),
            new StateModel("Khanate", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.KingMongol, ProfessionInfo.KingHeirEuro, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorMongol, ProfessionInfo.GovernorMongol, BuildingSize.Unique)),
                true, 1, 6, false, SocialOrder.MedievalMongol, new Language[] {Language.Orkish, Language.Eskimoid}),
            new StateModel("Khaganate", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.EmperorMongol, ProfessionInfo.KingHeirEuro, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.KingMongol, ProfessionInfo.KingHeirEuro, BuildingSize.Small)),
                true, 2, 6, true, SocialOrder.MedievalMongol, new Language[] {Language.Orkish, Language.Eskimoid}),
            new StateModel("Knyazdom", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.KingSlavic, ProfessionInfo.KingHeirSlavic, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorSlavic, ProfessionInfo.GovernorSlavic, BuildingSize.Unique)),
                true, 1, 6, false, SocialOrder.MedievalSlavic, new Language[] {Language.Slavic}),
            new StateModel("Tsardom", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.EmperorSlavic, ProfessionInfo.EmperorHeirSlavic, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.KingSlavic, ProfessionInfo.KingHeirSlavic, BuildingSize.Small)),
                true, 2, 6, true, SocialOrder.MedievalSlavic, new Language[] {Language.Slavic}),
            new StateModel("Basileia", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.KingGreek, ProfessionInfo.KingHeirGreek, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorGreek, ProfessionInfo.GovernorGreek, BuildingSize.Unique)),
                true, 1, 6, false, SocialOrder.MedievalGreek, new Language[] {Language.Greek}),
            new StateModel("Autokratoria", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.EmperorGreek, ProfessionInfo.KingHeirGreek, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorGreek2, ProfessionInfo.GovernorGreek2, BuildingSize.Unique)),
                true, 2, 6, true, SocialOrder.MedievalGreek, new Language[] {Language.Greek}),
            new StateModel("Raj", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.KingHindu, ProfessionInfo.KingHeirHindu, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorHindu, ProfessionInfo.GovernorHindu, BuildingSize.Unique)),
                true, 1, 6, false, SocialOrder.MedievalHindu, new Language[] {Language.Hindu}),
            new StateModel("Empire", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.EmperorHindu, ProfessionInfo.EmperorHeirHindu, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.KingHindu, ProfessionInfo.KingHeirHindu, BuildingSize.Small)),
                true, 2, 6, true, SocialOrder.MedievalHindu, new Language[] {Language.Hindu}),
            new StateModel("Altepetl", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.KingAztec, ProfessionInfo.KingHeirAztec, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorAztec, ProfessionInfo.GovernorAztec, BuildingSize.Unique)),
                true, 1, 6, false, SocialOrder.MedievalAztec, new Language[] {Language.Aztec}),
            new StateModel("Empire", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.EmperorEuro, ProfessionInfo.KingHeirAztec, BuildingSize.Small, FamilyOwnership.Full)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorAztec, ProfessionInfo.GovernorAztec, BuildingSize.Unique)),
                true, 2, 6, true, SocialOrder.MedievalAztec, new Language[] {Language.Aztec}),
            new StateModel("Republic", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Statehouse", ProfessionInfo.RulerModern, ProfessionInfo.AdvisorModern, BuildingSize.Small)),
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Town hall", ProfessionInfo.GovernorModern, ProfessionInfo.GovernorModern, BuildingSize.Unique)),
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
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 16, new BuildingInfo("Statehouse", ProfessionInfo.RulerModern2, ProfessionInfo.AdvisorModern2, BuildingSize.Unique)),
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 15, new BuildingInfo("Town hall", ProfessionInfo.GovernorModern2, ProfessionInfo.GovernorModern2, BuildingSize.Unique)),
                false, 4, 7, false, SocialOrder.Modern, null),
            new StateModel("League", 3,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 16, new BuildingInfo("Palace", ProfessionInfo.RulerModern3, ProfessionInfo.AdvisorModern3, BuildingSize.Unique)),
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 15, new BuildingInfo("Palace", ProfessionInfo.GovernorModern, ProfessionInfo.GovernorModern, BuildingSize.Unique)),
                false, 4, 7, true, SocialOrder.Modern, null),
            new StateModel("Union", 3,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 16, new BuildingInfo("Statehouse", ProfessionInfo.RulerModern2, ProfessionInfo.AdvisorModern2, BuildingSize.Unique)),
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 15, new BuildingInfo("Town hall", ProfessionInfo.GovernorModern2, ProfessionInfo.GovernorModern2, BuildingSize.Unique)),
                false, 5, 7, true, SocialOrder.Modern, null),
            new StateModel("Alliance", 3,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 16, new BuildingInfo("Palace", ProfessionInfo.RulerModern4, ProfessionInfo.AdvisorModern, BuildingSize.Unique)),
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 15, new BuildingInfo("Town hall", ProfessionInfo.GovernorModern2, ProfessionInfo.GovernorModern2, BuildingSize.Unique)),
                false, 5, 7, true, SocialOrder.Modern, null),
            new StateModel("Coalition", 3,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 16, new BuildingInfo("Statehouse", ProfessionInfo.RulerModern3, ProfessionInfo.AdvisorModern, BuildingSize.Unique)),
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 15, new BuildingInfo("Town hall", ProfessionInfo.GovernorModern2, ProfessionInfo.GovernorModern2, BuildingSize.Unique)),
                false, 5, 7, true, SocialOrder.Modern, null),
            new StateModel("Association", 3,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 16, new BuildingInfo("Statehouse", ProfessionInfo.RulerModern2, ProfessionInfo.AdvisorModern4, BuildingSize.Unique)),
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 15, new BuildingInfo("Town hall", ProfessionInfo.GovernorModern, ProfessionInfo.GovernorModern, BuildingSize.Unique)),
                false, 5, 7, true, SocialOrder.Modern, null),
            //new StateInfo("Realm", 17,
            //    new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 16, 5, 10, new BuildingInfo("Citadel", "God-King", "Goddess-Queen", 17)), 
            //    new SettlementInfo(SettlementSize.City, "City", 40, 80, 15, 5, 10, new BuildingInfo("Palace", "Father", "Mother", 15)), 
            //    "Brother", "Sister", false, 5, 8, null),
            new StateModel("Commonwealth", 1,
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 16, new BuildingInfo("Town hall", ProfessionInfo.RulerModern3, ProfessionInfo.AdvisorModern5, BuildingSize.Unique)),
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 15, new BuildingInfo("Town hall", ProfessionInfo.GovernorModern3, ProfessionInfo.GovernorModern3, BuildingSize.Unique)),
                false, 7, 8, false, SocialOrder.Future, null),
            new StateModel("Society", 1,
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, 16, null),
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, 15, null),
                false, 7, 8, false, SocialOrder.Future, null),
            new StateModel("Collective", 2,
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, 16, null),
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, 15, null),
                false, 7, 8, true, SocialOrder.Future, null),
        };
        #endregion

        /// <summary>
        /// Доступный жителям уровень жизни.
        /// Зависит от технического и магического развития, определяет доступные формы государственного правления
        /// </summary>
        public int m_iInfrastructureLevel = 0;

        public Nation m_pTitularNation = null;

        public StateModel m_pStateModel = null;

        private State m_pState = null;

        public StateSociety(State pState)
        {
            m_pState = pState;

            m_iTechLevel = 0;
            m_iInfrastructureLevel = 0;
            m_pCulture = new Culture(pState.m_pMethropoly.m_pCulture);
            m_pCustoms = new Customs(pState.m_pMethropoly.m_pCustoms, Customs.Mutation.Possible);

            m_pTitularNation = pState.m_pMethropoly.m_pNation;
        }

        public void CalculateTitularNation()
        { 
            Dictionary<Nation, int> cNationsCount = new Dictionary<Nation, int>();

            int iMaxPop = 0;
            Nation pMostCommonNation = null;

            foreach (Province pProvince in m_pState.m_cContents)
            {
                int iCount = 0;

                if (!cNationsCount.TryGetValue(pProvince.m_pNation, out iCount))
                    cNationsCount[pProvince.m_pNation] = 0;
                cNationsCount[pProvince.m_pNation] = iCount + pProvince.m_iPopulation;
                if (cNationsCount[pProvince.m_pNation] > iMaxPop)
                {
                    iMaxPop = cNationsCount[pProvince.m_pNation];
                    pMostCommonNation = pProvince.m_pNation;
                }

                if (pProvince.m_iTechLevel > m_iTechLevel)
                    m_iTechLevel = pProvince.m_iTechLevel;

                if (pProvince.m_iInfrastructureLevel > m_iInfrastructureLevel)
                    m_iInfrastructureLevel = pProvince.m_iInfrastructureLevel;
                //m_iInfrastructureLevel += pProvince.m_iInfrastructureLevel;
                //fInfrastructureLevel += 1.0f/(pProvince.m_iInfrastructureLevel+1);
            }

            m_pTitularNation = pMostCommonNation;
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


        public override string GetEstateName(Estate.Position ePosition)
        {
            return m_pStateModel.m_pSocial.m_cEstates[ePosition][Rnd.Get(m_pStateModel.m_pSocial.m_cEstates[ePosition].Length)];
        }

        public override int GetImportedTech()
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

        public override string GetImportedTechString()
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

            return GetTechString(pExporter.m_pSociety.m_iTechLevel, pExporter.m_pSociety.m_pCustoms.m_eProgress);
        }
        public override string ToString()
        {
            return string.Format("{1} (C{0}T{2}M{4}) - {3}", m_iCultureLevel, m_pTitularNation, m_iTechLevel, m_pStateModel.m_sName, m_iMagicLimit);
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
                                cChances[BuildingInfo.WarriorsHutSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Agression][iInfrastructureLevel] / 2;//(float)iControl / 4;

                            if (iInfrastructureLevel < 2)
                                cChances[BuildingInfo.ShamansHutSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Piety][iInfrastructureLevel] / 2;

                            if (m_iSocialEquality == 0)
                                cChances[BuildingInfo.SlavePensMedium] = (float)cChances.Count / 2 + 1;

                            cChances[BuildingInfo.RaidersHutSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Treachery][iInfrastructureLevel] / 2;
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

                            cChances[pGuard] = (float)iControl * m_pCulture.MentalityValues[Psichology.Mentality.Agression][iInfrastructureLevel] / 4;

                            BuildingInfo pChurch;
                            if (iInfrastructureLevel < 2)
                                pChurch = BuildingInfo.ShamansHutSmall;
                            else
                                pChurch = BuildingInfo.ChurchSmall;

                            cChances[pChurch] = m_pCulture.MentalityValues[Psichology.Mentality.Piety][iInfrastructureLevel];

                            if (iInfrastructureLevel >= 2)
                            {
                                if (iInfrastructureLevel < 4)
                                {
                                    cChances[m_iSocialEquality == 0 ? BuildingInfo.InnSlvSmall : BuildingInfo.InnSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] / 4;
                                    cChances[m_iSocialEquality == 0 ? BuildingInfo.TavernSlvSmall : BuildingInfo.TavernSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel];
                                }
                                else
                                {
                                    cChances[m_iSocialEquality == 0 ? BuildingInfo.HotelSlvSmall : BuildingInfo.HotelSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] / 4;
                                    cChances[m_iSocialEquality == 0 ? BuildingInfo.BarSlvSmall : BuildingInfo.BarSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel];
                                }
                            }

                            cChances[BuildingInfo.RoguesDenSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Treachery][iInfrastructureLevel] / 2;

                            if (iInfrastructureLevel >= 3)
                            {
                                float fScience = 0.05f;
                                if (m_pCustoms.m_eMindSet == Customs.MindSet.Balanced_mind)
                                    fScience = 0.25f;
                                if (m_pCustoms.m_eMindSet == Customs.MindSet.Logic)
                                    fScience = 0.5f;

                                if (m_pTitularNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Sapient)
                                    fScience *= 2;
                                if (m_pTitularNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Ingenious)
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

                            cChances[pGuard] = (float)iControl * m_pCulture.MentalityValues[Psichology.Mentality.Agression][iInfrastructureLevel] / 4;

                            BuildingInfo pPrison;
                            if (iInfrastructureLevel < 4)
                                pPrison = BuildingInfo.PrisonSmall;
                            else
                                if (iInfrastructureLevel == 8)
                                pPrison = BuildingInfo.HoldingSmall;
                            else
                                pPrison = BuildingInfo.PrisonPoliceSmall;

                            cChances[pPrison] = (float)iControl / 4;

                            cChances[BuildingInfo.ChurchMedium] = m_pCulture.MentalityValues[Psichology.Mentality.Piety][iInfrastructureLevel] / 5;
                            cChances[BuildingInfo.ChurchSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Piety][iInfrastructureLevel];

                            BuildingInfo pBrothelProfile = m_iSocialEquality == 0 ? BuildingInfo.BrothelSlvMedium : BuildingInfo.BrothelMedium;
                            BuildingInfo pStripClubProfile = m_iSocialEquality == 0 ? BuildingInfo.StripClubSlvSmall : BuildingInfo.StripClubSmall;

                            switch (m_pCustoms.m_eSexuality)
                            {
                                case Psichology.Customs.Sexuality.Moderate_sexuality:
                                    cChances[pBrothelProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] / 4;
                                    if (iInfrastructureLevel >= 4)
                                        cChances[pStripClubProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] / 2;
                                    break;
                                case Psichology.Customs.Sexuality.Lecherous:
                                    cChances[pBrothelProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] / 2;
                                    if (iInfrastructureLevel >= 4)
                                        cChances[pStripClubProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] / 2;
                                    break;
                            }

                            if (iInfrastructureLevel < 4)
                            {
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.InnSlvSmall : BuildingInfo.InnSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] / 4;
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.TavernSlvSmall : BuildingInfo.TavernSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel];
                            }
                            else
                            {
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.HotelSlvMedium : BuildingInfo.HotelMedium] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] / 4;
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.BarSlvSmall : BuildingInfo.BarSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel];
                            }

                            cChances[BuildingInfo.TheatreMedium] = 1f - m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] / 2;

                            cChances[BuildingInfo.RoguesDenMedium] = m_pCulture.MentalityValues[Psichology.Mentality.Treachery][iInfrastructureLevel] / 2;
                            cChances[BuildingInfo.GamblingSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Treachery][iInfrastructureLevel] / 2;

                            if (iInfrastructureLevel < 3)
                                cChances[BuildingInfo.MedicineSmall] = (float)cChances.Count / 12;
                            else
                                cChances[BuildingInfo.MedicineMedium] = (float)cChances.Count / 12;

                            float fBureaucracy = 0.05f;
                            if (m_pCustoms.m_eMindSet == Customs.MindSet.Balanced_mind)
                                fBureaucracy = 0.25f;
                            if (m_pCustoms.m_eMindSet == Customs.MindSet.Logic)
                                fBureaucracy = 0.5f;

                            if (m_pTitularNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Sapient)
                                fBureaucracy *= 2;
                            if (m_pTitularNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Primitive)
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
                                if (m_pCustoms.m_eMindSet == Customs.MindSet.Balanced_mind)
                                    fScience = 0.25f;
                                if (m_pCustoms.m_eMindSet == Customs.MindSet.Logic)
                                    fScience = 0.5f;

                                if (m_pTitularNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Sapient)
                                    fScience *= 2;
                                if (m_pTitularNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Ingenious)
                                    fScience *= 4;

                                cChances[BuildingInfo.ScienceSmall] = fScience / 4;

                                cChances[BuildingInfo.SchoolSmall] = fScience;
                                cChances[BuildingInfo.SchoolMedium] = fScience / 2;
                            }

                            if (m_iSocialEquality == 0)
                            {
                                cChances[BuildingInfo.SlaveMarketMedium] = (float)cChances.Count / 6 + 1;
                                switch (m_pCustoms.m_eSexuality)
                                {
                                    case Psichology.Customs.Sexuality.Moderate_sexuality:
                                        cChances[BuildingInfo.SlaveMarketMedium2] = (float)cChances.Count / 6 + 1;
                                        break;
                                    case Psichology.Customs.Sexuality.Lecherous:
                                        cChances[BuildingInfo.SlaveMarketMedium2] = (float)cChances.Count / 6 + 1;
                                        break;
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

                            cChances[pGuard] = (float)iControl * m_pCulture.MentalityValues[Psichology.Mentality.Agression][iInfrastructureLevel] / 4;

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

                            cChances[BuildingInfo.ChurchMedium] = m_pCulture.MentalityValues[Psichology.Mentality.Piety][iInfrastructureLevel] / 5;
                            cChances[BuildingInfo.ChurchSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Piety][iInfrastructureLevel];

                            BuildingInfo pBrothelProfile = m_iSocialEquality == 0 ? BuildingInfo.BrothelSlvMedium : BuildingInfo.BrothelMedium;
                            BuildingInfo pStripClubProfile = m_iSocialEquality == 0 ? BuildingInfo.StripClubSlvSmall : BuildingInfo.StripClubSmall;

                            switch (m_pCustoms.m_eSexuality)
                            {
                                case Psichology.Customs.Sexuality.Moderate_sexuality:
                                    cChances[pBrothelProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] / 4;
                                    if (iInfrastructureLevel >= 4)
                                        cChances[pStripClubProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] / 2;
                                    break;
                                case Psichology.Customs.Sexuality.Lecherous:
                                    cChances[pBrothelProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] / 2;
                                    if (iInfrastructureLevel >= 4)
                                        cChances[pStripClubProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] / 2;
                                    break;
                            }

                            if (iInfrastructureLevel < 4)
                            {
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.InnSlvSmall : BuildingInfo.InnSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] / 4;
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.TavernSlvSmall : BuildingInfo.TavernSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel];
                                cChances[BuildingInfo.CircusMedium] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] *
                                                                      m_pCulture.MentalityValues[Psichology.Mentality.Agression][iInfrastructureLevel] / 2;
                            }
                            else
                            {
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.HotelSlvMedium : BuildingInfo.HotelMedium] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] / 4;
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.BarSlvSmall : BuildingInfo.BarSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel];
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.NightClubSlvMedium : BuildingInfo.NightClubMedium] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel];
                            }

                            cChances[BuildingInfo.TheatreMedium] = 1f - m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] / 2;

                            cChances[BuildingInfo.RoguesDenMedium] = m_pCulture.MentalityValues[Psichology.Mentality.Treachery][iInfrastructureLevel] / 2;
                            cChances[BuildingInfo.GamblingSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Treachery][iInfrastructureLevel] / 2;

                            if (iInfrastructureLevel < 3)
                                cChances[BuildingInfo.MedicineSmall] = (float)cChances.Count / 12;
                            else
                            {
                                cChances[BuildingInfo.MedicineMedium] = (float)cChances.Count / 12;
                                cChances[BuildingInfo.MedicineLarge] = (float)cChances.Count / 24;
                            }

                            float fBureaucracy = 0.05f;
                            if (m_pCustoms.m_eMindSet == Psichology.Customs.MindSet.Balanced_mind)
                                fBureaucracy = 0.25f;
                            if (m_pCustoms.m_eMindSet == Psichology.Customs.MindSet.Logic)
                                fBureaucracy = 0.5f;

                            if (m_pTitularNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Sapient)
                                fBureaucracy *= 2;
                            if (m_pTitularNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Primitive)
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
                                if (m_pCustoms.m_eMindSet == Customs.MindSet.Balanced_mind)
                                    fScience = 0.25f;
                                if (m_pCustoms.m_eMindSet == Customs.MindSet.Logic)
                                    fScience = 0.5f;

                                if (m_pTitularNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Sapient)
                                    fScience *= 2;
                                if (m_pTitularNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Ingenious)
                                    fScience *= 4;

                                cChances[BuildingInfo.ScienceSmall] = fScience / 4;
                                cChances[BuildingInfo.ScienceMedium] = fScience / 8;

                                cChances[BuildingInfo.SchoolSmall] = fScience;
                                cChances[BuildingInfo.SchoolMedium] = fScience / 2;
                            }

                            if (m_iSocialEquality == 0)
                            {
                                cChances[BuildingInfo.SlaveMarketMedium] = (float)cChances.Count / 6 + 1;
                                switch (m_pCustoms.m_eSexuality)
                                {
                                    case Psichology.Customs.Sexuality.Moderate_sexuality:
                                        cChances[BuildingInfo.SlaveMarketMedium2] = (float)cChances.Count / 6 + 1;
                                        break;
                                    case Psichology.Customs.Sexuality.Lecherous:
                                        cChances[BuildingInfo.SlaveMarketMedium2] = (float)cChances.Count / 4 + 1;
                                        break;
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
                                pProfile = iInfrastructureLevel < 4 ? (m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] > Rnd.Get(2f) ? BuildingInfo.CircusMedium : BuildingInfo.TheatreMedium) : BuildingInfo.CinemaLarge;
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

                            cChances[pGuard] = (float)iControl * m_pCulture.MentalityValues[Psichology.Mentality.Agression][iInfrastructureLevel] / 4;

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

                            cChances[BuildingInfo.ChurchMedium] = m_pCulture.MentalityValues[Psichology.Mentality.Piety][iInfrastructureLevel] / 5;
                            cChances[BuildingInfo.ChurchSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Piety][iInfrastructureLevel];

                            BuildingInfo pBrothelProfile = m_iSocialEquality == 0 ? BuildingInfo.BrothelSlvMedium : BuildingInfo.BrothelMedium;
                            BuildingInfo pStripClubProfile = m_iSocialEquality == 0 ? BuildingInfo.StripClubSlvSmall : BuildingInfo.StripClubSmall;

                            switch (m_pCustoms.m_eSexuality)
                            {
                                case Psichology.Customs.Sexuality.Moderate_sexuality:
                                    cChances[pBrothelProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] / 4;
                                    if (iInfrastructureLevel >= 4)
                                        cChances[pStripClubProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] / 2;
                                    break;
                                case Psichology.Customs.Sexuality.Lecherous:
                                    cChances[pBrothelProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] / 2;
                                    if (iInfrastructureLevel >= 4)
                                        cChances[pStripClubProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] / 2;
                                    break;
                            }

                            if (iInfrastructureLevel < 4)
                            {
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.InnSlvSmall : BuildingInfo.InnSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] / 4;
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.TavernSlvSmall : BuildingInfo.TavernSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel];
                                cChances[BuildingInfo.CircusMedium] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] *
                                                                      m_pCulture.MentalityValues[Psichology.Mentality.Agression][iInfrastructureLevel] / 2;
                            }
                            else
                            {
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.HotelSlvMedium : BuildingInfo.HotelMedium] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] / 4;
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.BarSlvSmall : BuildingInfo.BarSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel];
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.NightClubSlvMedium : BuildingInfo.NightClubMedium] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel];
                            }

                            cChances[BuildingInfo.TheatreMedium] = 1f - m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] / 2;

                            cChances[BuildingInfo.RoguesDenMedium] = m_pCulture.MentalityValues[Psichology.Mentality.Treachery][iInfrastructureLevel] / 2;
                            cChances[BuildingInfo.GamblingSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Treachery][iInfrastructureLevel] / 2;

                            if (iInfrastructureLevel < 3)
                                cChances[BuildingInfo.MedicineSmall] = (float)cChances.Count / 12;
                            else
                            {
                                cChances[BuildingInfo.MedicineMedium] = (float)cChances.Count / 12;
                                cChances[BuildingInfo.MedicineLarge] = (float)cChances.Count / 24;
                            }

                            float fBureaucracy = 0.05f;
                            if (m_pCustoms.m_eMindSet == Psichology.Customs.MindSet.Balanced_mind)
                                fBureaucracy = 0.25f;
                            if (m_pCustoms.m_eMindSet == Psichology.Customs.MindSet.Logic)
                                fBureaucracy = 0.5f;

                            if (m_pTitularNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Sapient)
                                fBureaucracy *= 2;
                            if (m_pTitularNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Primitive)
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
                                if (m_pCustoms.m_eMindSet == Customs.MindSet.Balanced_mind)
                                    fScience = 0.25f;
                                if (m_pCustoms.m_eMindSet == Customs.MindSet.Logic)
                                    fScience = 0.5f;

                                if (m_pTitularNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Sapient)
                                    fScience *= 2;
                                if (m_pTitularNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Ingenious)
                                    fScience *= 4;

                                cChances[BuildingInfo.ScienceSmall] = fScience / 4;
                                cChances[BuildingInfo.ScienceMedium] = fScience / 8;

                                cChances[BuildingInfo.SchoolSmall] = fScience;
                                cChances[BuildingInfo.SchoolMedium] = fScience / 2;
                            }

                            if (m_iSocialEquality == 0)
                            {
                                cChances[BuildingInfo.SlaveMarketMedium] = (float)cChances.Count / 6 + 1;
                                switch (m_pCustoms.m_eSexuality)
                                {
                                    case Psichology.Customs.Sexuality.Moderate_sexuality:
                                        cChances[BuildingInfo.SlaveMarketMedium2] = (float)cChances.Count / 6 + 1;
                                        break;
                                    case Psichology.Customs.Sexuality.Lecherous:
                                        cChances[BuildingInfo.SlaveMarketMedium2] = (float)cChances.Count / 4 + 1;
                                        break;
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
                                pProfile = iInfrastructureLevel < 4 ? (m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] > Rnd.Get(2f) ? BuildingInfo.CircusMedium : BuildingInfo.TheatreMedium) : BuildingInfo.CinemaLarge;
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
                            cChances[BuildingInfo.ChurchMedium] = m_pCulture.MentalityValues[Psichology.Mentality.Piety][iInfrastructureLevel] / 5;
                            cChances[BuildingInfo.ChurchSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Piety][iInfrastructureLevel];

                            BuildingInfo pBrothelProfile = m_iSocialEquality == 0 ? BuildingInfo.BrothelSlvMedium : BuildingInfo.BrothelMedium;
                            BuildingInfo pStripClubProfile = m_iSocialEquality == 0 ? BuildingInfo.StripClubSlvSmall : BuildingInfo.StripClubSmall;

                            switch (m_pCustoms.m_eSexuality)
                            {
                                case Psichology.Customs.Sexuality.Moderate_sexuality:
                                    cChances[pBrothelProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] / 4;
                                    if (iInfrastructureLevel >= 4)
                                        cChances[pStripClubProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] / 2;
                                    break;
                                case Psichology.Customs.Sexuality.Lecherous:
                                    cChances[pBrothelProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] / 2;
                                    if (iInfrastructureLevel >= 4)
                                        cChances[pStripClubProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] / 2;
                                    break;
                            }

                            if (iInfrastructureLevel < 4)
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.TavernSlvSmall : BuildingInfo.TavernSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] / 2;
                            else
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.BarSlvSmall : BuildingInfo.BarSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Simplicity][iInfrastructureLevel] / 2;

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
            CheckResources();

            // Set available infrastructure level according TL and food availability
            CheckFood();

            // Choose state system
            SelectGovernmentSystem(iEmpireTreshold);

            // Set social equality level
            SetSocialEquality();

            // Set state control level
            SetStateControl();

            Person.GetSkillPreferences(m_pCulture, m_iCultureLevel, m_pCustoms, ref m_eMostRespectedSkill, ref m_eLeastRespectedSkill);
        }

        private void CheckResources()
        {
            if (m_pState.m_iWood * 2 < Rnd.Get(m_pState.m_iPopulation) && m_pState.m_iOre * 2 < Rnd.Get(m_pState.m_iPopulation))// && Rnd.OneChanceFrom(2))
                m_iTechLevel -= 2;
            else if (m_pState.m_iWood + m_pState.m_iOre < Rnd.Get(m_pState.m_iPopulation))// && Rnd.OneChanceFrom(2))
                m_iTechLevel--;
            else if ((m_pState.m_iWood > Rnd.Get(m_pState.m_iPopulation) * 2 && m_pState.m_iOre > Rnd.Get(m_pState.m_iPopulation) * 2))// || Rnd.OneChanceFrom(4))
                m_iTechLevel++;

            if (m_pTitularNation.m_bInvader)
            {
                if (m_iTechLevel < m_pTitularNation.m_pEpoch.m_iInvadersMinTechLevel)
                    m_iTechLevel = m_pTitularNation.m_pEpoch.m_iInvadersMinTechLevel;
                if (m_iTechLevel > m_pTitularNation.m_pEpoch.m_iInvadersMaxTechLevel)
                    m_iTechLevel = m_pTitularNation.m_pEpoch.m_iInvadersMaxTechLevel;
            }
            else
            {
                if (m_iTechLevel < m_pTitularNation.m_pEpoch.m_iNativesMinTechLevel)
                    m_iTechLevel = m_pTitularNation.m_pEpoch.m_iNativesMinTechLevel;
                if (m_iTechLevel > m_pTitularNation.m_pEpoch.m_iNativesMaxTechLevel)
                    m_iTechLevel = m_pTitularNation.m_pEpoch.m_iNativesMaxTechLevel;
            }
        }

        private void CheckFood()
        {
            //m_iInfrastructureLevel = 4 - (int)(m_pCulture.GetDifference(Culture.IdealSociety, m_iTechLevel, m_iTechLevel) * 4);
            m_iInfrastructureLevel = m_iTechLevel;// -(int)(m_iTechLevel * Math.Pow(Rnd.Get(1f), 3));

            if (m_pState.m_cContents.Count == 1 && m_iInfrastructureLevel > 4)
                m_iInfrastructureLevel /= 2;

            if (m_pState.m_pSociety.m_iTechLevel == 0 && m_pTitularNation.m_iMagicLimit == 0)
                m_iInfrastructureLevel = 0;


            if (m_pState.m_iFood * 2 < m_pState.m_iPopulation)
                m_iInfrastructureLevel--;// = Rnd.Get(m_iCultureLevel);            
            if (m_pState.m_iFood < m_pState.m_iPopulation || Rnd.OneChanceFrom(10))
                m_iInfrastructureLevel--;// = Rnd.Get(m_iCultureLevel);
            if (m_pState.m_iFood > m_pState.m_iPopulation * 2 && Rnd.OneChanceFrom(10))
                m_iInfrastructureLevel++;

            if (m_iInfrastructureLevel < 0)//Math.Max(m_iTechLevel + 1, iAverageMagicLimit) / 2)
                m_iInfrastructureLevel = 0;//Math.Max(m_iTechLevel + 1, iAverageMagicLimit) / 2;
            if (m_iInfrastructureLevel > m_iTechLevel + 1)//Math.Max(m_iTechLevel + 1, iAverageMagicLimit - 1))
                m_iInfrastructureLevel = m_iTechLevel + 1;// Math.Max(m_iTechLevel + 1, iAverageMagicLimit - 1);
            if (m_iInfrastructureLevel > 8)
                m_iInfrastructureLevel = 8;

            // Adjusting TL due to infrastructure level
            while (GetEffectiveTech() > m_iInfrastructureLevel * 2)
                m_iTechLevel--;

            if (m_iTechLevel < 0)
                m_iTechLevel = 0;

            if (m_pTitularNation.m_bInvader)
            {
                if (m_iTechLevel > m_pTitularNation.m_pEpoch.m_iInvadersMaxTechLevel)
                    m_iTechLevel = m_pTitularNation.m_pEpoch.m_iInvadersMaxTechLevel;
            }
            else
            {
                if (m_iTechLevel > m_pTitularNation.m_pEpoch.m_iNativesMaxTechLevel)
                    m_iTechLevel = m_pTitularNation.m_pEpoch.m_iNativesMaxTechLevel;
            }
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

            m_iCultureLevel = (m_iInfrastructureLevel + m_pStateModel.m_iMinGovernmentLevel) / 2;
        }

        private void SetSocialEquality()
        {
            m_iSocialEquality = 2;

            if (m_pCulture.MentalityValues[Mentality.Agression][m_iCultureLevel] > 1.66)
                m_iSocialEquality--;
            //if (m_pCulture.MentalityValues[Mentality.Agression][m_iCultureLevel] > 1.33)
            //    m_iSocialEquality--;
            //if (m_pCulture.MentalityValues[Mentality.Agression][m_iCultureLevel] > 1)
            //    m_iSocialEquality--;

            if (m_pCulture.MentalityValues[Mentality.Fanaticism][m_iCultureLevel] > 1.66)
                m_iSocialEquality--;
            if (m_pCulture.MentalityValues[Mentality.Fanaticism][m_iCultureLevel] > 1.33)
                m_iSocialEquality--;
            //if (m_pCulture.MentalityValues[Mentality.Fanaticism][m_iTechLevel] > 1)
            //    m_iSocialEquality--;

            if (m_pCulture.MentalityValues[Mentality.Selfishness][m_iCultureLevel] > 1.66)
                m_iSocialEquality--;
            if (m_pCulture.MentalityValues[Mentality.Selfishness][m_iCultureLevel] > 1.33)
                m_iSocialEquality--;
            //if (m_pCulture.MentalityValues[Mentality.Selfishness][m_iCultureLevel] > 1)
            //    m_iSocialEquality--;

            if (m_pCulture.MentalityValues[Mentality.Treachery][m_iCultureLevel] > 1.66)
                m_iSocialEquality--;
            if (m_pCulture.MentalityValues[Mentality.Treachery][m_iCultureLevel] > 1.33)
                m_iSocialEquality--;
            //if (m_pCulture.MentalityValues[Mentality.Treachery][m_iCultureLevel] > 1)
            //    m_iSocialEquality--;

            if (m_pStateModel.m_bDinasty)
                m_iSocialEquality--;

            if (m_iSocialEquality < 0)
                m_iSocialEquality = 0;

            if (m_pCulture.MentalityValues[Mentality.Agression][m_iCultureLevel] < 1)
                m_iSocialEquality++;

            if (m_pCulture.MentalityValues[Mentality.Fanaticism][m_iCultureLevel] < 1)
                m_iSocialEquality++;

            if (m_iSocialEquality > 0 && m_pState.m_iFood < m_pState.m_iPopulation)
                m_iSocialEquality--;
            if (m_pState.m_iFood > m_pState.m_iPopulation && m_pState.m_iOre > m_pState.m_iPopulation && m_pState.m_iWood > m_pState.m_iPopulation)
                m_iSocialEquality++;

            //в либеральном обществе (фанатизм < 2/3) не может быть рабства (0) или крепостного права (1), т.е. только 2 и выше
            if (m_pCulture.MentalityValues[Mentality.Fanaticism][m_iCultureLevel] < 0.66)
                m_iSocialEquality = Math.Max(2, m_iSocialEquality);
            //в обществе абсолютных пацифистов (агрессивность < 1/3) не может быть даже капитализма (2), т.е. только 3 и выше
            if (m_pCulture.MentalityValues[Mentality.Agression][m_iCultureLevel] < 0.33)
                m_iSocialEquality = Math.Max(3, m_iSocialEquality);
            //в обществе абсолютного самоотречения (эгоизм < 1/3) не может быть капитализма (2) - только или социализм, или феодализм
            if (m_pCulture.MentalityValues[Mentality.Selfishness][m_iCultureLevel] < 0.33)
                if (m_pStateModel.m_bDinasty)
                    m_iSocialEquality = Math.Min(1, m_iSocialEquality);
                else
                    m_iSocialEquality = Math.Max(3, m_iSocialEquality);
            //эгоизм и коммунизм не совместимы
            if (m_pCulture.MentalityValues[Mentality.Selfishness][m_iCultureLevel] > 1)
                m_iSocialEquality = Math.Min(3, m_iSocialEquality);
            //преступный склад ума и социализм не совместимы
            if (m_pCulture.MentalityValues[Mentality.Treachery][m_iCultureLevel] > 0.66)
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
            if (m_pCulture.MentalityValues[Mentality.Fanaticism][m_iCultureLevel] > 1.33)
                m_iControl++;
            if (m_pCulture.MentalityValues[Mentality.Fanaticism][m_iCultureLevel] > 1.66)
                m_iControl++;
            if (m_pCulture.MentalityValues[Mentality.Fanaticism][m_iCultureLevel] < 0.33)
                m_iControl--;

            if (m_pCulture.MentalityValues[Mentality.Selfishness][m_iCultureLevel] > 1.66)
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

            float fMagesCount = 0;
            float[] aDistribution = new float[10];

            foreach (Province pProvince in m_pState.m_cContents)
            {
                if (pProvince.m_pNation.m_iMagicLimit > m_iMagicLimit)
                    m_iMagicLimit = pProvince.m_pNation.m_iMagicLimit;

                float fPrevalence = 1;
                switch (pProvince.m_pNation.m_pCustoms.m_eMagic)
                {
                    case Customs.Magic.Magic_Feared:
                        fPrevalence = 0.1f;
                        break;
                    case Customs.Magic.Magic_Allowed:
                        fPrevalence = 0.5f;
                        break;
                    case Customs.Magic.Magic_Praised:
                        fPrevalence = 0.9f;
                        break;
                }

                float fProvinceMagesCount = 0;
                foreach (LandX pLand in pProvince.m_cContents)
                {
                    fProvinceMagesCount += pLand.m_cContents.Count * fPrevalence;
                }

                switch (pProvince.m_pNation.m_eMagicAbilityDistribution)
                {
                    case MagicAbilityDistribution.mostly_weak:
                        aDistribution[(1 + pProvince.m_pNation.m_iMagicLimit) / 2] += fProvinceMagesCount;
                        break;
                    case MagicAbilityDistribution.mostly_average:
                        aDistribution[(1 + pProvince.m_pNation.m_iMagicLimit) / 2] += fProvinceMagesCount / 2;
                        aDistribution[1 + pProvince.m_pNation.m_iMagicLimit] += fProvinceMagesCount / 2;
                        break;
                    case MagicAbilityDistribution.mostly_powerful:
                        aDistribution[1 + pProvince.m_pNation.m_iMagicLimit] += fProvinceMagesCount;
                        break;
                }
                fMagesCount += fProvinceMagesCount;
            }
            fMagesCount /= m_pState.m_iPopulation;

            //m_eMagicAbilityPrevalence = MagicAbilityPrevalence.AlmostEveryone;

            //if (fMagesCount <= 0.75)
            //    m_eMagicAbilityPrevalence = MagicAbilityPrevalence.Common;

            //if (fMagesCount <= 0.25)
            //    m_eMagicAbilityPrevalence = MagicAbilityPrevalence.Rare;

            float fWeakMagesCount = 0;
            float fPowerfulMagesCount = 0;
            for (int i = 0; i < 10; i++)
            {
                if (i <= (m_iMagicLimit + 1) / 2)
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

            iHostility += m_pCustoms.GetDifference(pOpponent.m_pSociety.m_pCustoms, ref sPositiveReasons, ref sNegativeReasons);

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

            float iCultureDifference = m_pCulture.GetDifference(pOpponent.m_pSociety.m_pCulture, m_iCultureLevel, pOpponent.m_pSociety.m_iCultureLevel);
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
                iHostility = (int)(m_pCulture.MentalityValues[Mentality.Fanaticism][m_iCultureLevel] * iHostility + 0.25);
                sReasons += string.Format("Fanaticism \t(x{0}%)\n", (int)(m_pCulture.MentalityValues[Mentality.Fanaticism][m_iCultureLevel] * 100));

                iHostility = (int)(m_pCulture.MentalityValues[Mentality.Agression][m_iCultureLevel] * iHostility + 0.25);
                sReasons += string.Format("Agression \t(x{0}%)\n", (int)(m_pCulture.MentalityValues[Mentality.Agression][m_iCultureLevel] * 100));

                if (iHostility == 0)
                    iHostility = 1;
            }
            else
            {
                if (iHostility < 0)
                {
                    iHostility = (int)((2.0f - m_pCulture.MentalityValues[Mentality.Fanaticism][m_iCultureLevel]) * iHostility - 0.25);
                    sReasons += string.Format("Tolerance \t(x{0}%)\n", (int)((2.0f - m_pCulture.MentalityValues[Mentality.Fanaticism][m_iCultureLevel]) * 100));

                    iHostility = (int)((2.0f - m_pCulture.MentalityValues[Mentality.Agression][m_iCultureLevel]) * iHostility - 0.25);
                    sReasons += string.Format("Amiability \t(x{0}%)\n", (int)((2.0f - m_pCulture.MentalityValues[Mentality.Agression][m_iCultureLevel]) * 100));

                    if (iHostility == 0)
                        iHostility = -1;
                }
            }

            //if (fContact < fBorder / 2)
            //    iHostility = iHostility / 2;

            sReasons += string.Format("----\nTotal \t({0:+#;-#;0})\n", -iHostility);
            return iHostility;
        }
        
        internal override Customs.GenderPriority FixGenderPriority(Customs.GenderPriority ePriority)
        {
            switch (ePriority)
            {
                case Customs.GenderPriority.Matriarchy:
                    if (m_pTitularNation.m_pFenotype.m_pLifeCycle.m_eGendersDistribution == GendersDistribution.OnlyFemales ||
                        m_pTitularNation.m_pFenotype.m_pLifeCycle.m_eGendersDistribution == GendersDistribution.MostlyFemales)
                        return Customs.GenderPriority.Genders_equality;
                    else
                        return Customs.GenderPriority.Patriarchy;
                case Customs.GenderPriority.Patriarchy:
                    if (m_pTitularNation.m_pFenotype.m_pLifeCycle.m_eGendersDistribution == GendersDistribution.OnlyMales ||
                        m_pTitularNation.m_pFenotype.m_pLifeCycle.m_eGendersDistribution == GendersDistribution.MostlyMales)
                        return Customs.GenderPriority.Genders_equality;
                    else
                        return Customs.GenderPriority.Matriarchy;
            }

            return base.FixGenderPriority(ePriority);
        }
    }
}
