﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using NameGen;
using LandscapeGeneration;
using LandscapeGeneration.PathFind;
using Socium.Languages;
using Socium.Settlements;
using Socium.Nations;
using Socium.Psichology;
using GeneLab.Genetix;
using LandscapeGeneration.PlanetBuilder;

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
        public StateInfo(string sName, int iRank, SettlementInfo pStateCapital, SettlementInfo pProvinceCapital, bool bDinasty, int iMinGovernmentLevel, int iMaxGovernmentLevel, bool bBig, SocialOrder pSocial, Language[] cLanguages)
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

    public class State : BorderBuilder<Province>, ITerritory
    {
        #region States Info Array
        private static StateInfo[] s_aInfo = 
        {
            new StateInfo("Land", 1,
                new SettlementInfo(SettlementSize.Hamlet, "Hamlet", 5, 10, 2, new BuildingInfo("Elder's hut", ProfessionInfo.RulerPrimitive, ProfessionInfo.HeirPrimitive, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.Hamlet, "Hamlet", 5, 10, 2, new BuildingInfo("Elder's hut", ProfessionInfo.GovernorPrimitive, ProfessionInfo.GovernorPrimitive, BuildingSize.Unique)), 
                true, 0, 0, false, SocialOrder.Primitive, null),            
            new StateInfo("Lands", 1,
                new SettlementInfo(SettlementSize.Hamlet, "Hamlet", 5, 10, 2, new BuildingInfo("Elder's hut", ProfessionInfo.RulerPrimitive, ProfessionInfo.HeirPrimitive, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.Hamlet, "Hamlet", 5, 10, 2, new BuildingInfo("Elder's hut", ProfessionInfo.GovernorPrimitive, ProfessionInfo.GovernorPrimitive, BuildingSize.Unique)), 
                true, 0, 0, true, SocialOrder.Primitive, null),
            new StateInfo("Tribes", 1,
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, 3, new BuildingInfo("Clans hall", ProfessionInfo.RulerPrimitive, ProfessionInfo.HeirPrimitive, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, 3, new BuildingInfo("Village hall", ProfessionInfo.GovernorPrimitive, ProfessionInfo.GovernorPrimitive, BuildingSize.Unique)), 
                true, 1, 1, false, SocialOrder.Primitive, null),
            new StateInfo("Clans", 1,
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, 3, new BuildingInfo("Clans hall", ProfessionInfo.RulerPrimitive, ProfessionInfo.HeirPrimitive, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, 3, new BuildingInfo("Village hall", ProfessionInfo.GovernorPrimitive, ProfessionInfo.GovernorPrimitive, BuildingSize.Unique)), 
                true, 1, 1, true, SocialOrder.Primitive, null),
            new StateInfo("Kingdom", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.KingEuro, ProfessionInfo.KingHeirEuro, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorEuro, ProfessionInfo.GovernorEuro, BuildingSize.Unique)), 
                true, 2, 6, false, SocialOrder.MedievalEurope, new Language[] {Language.Dwarwen, Language.European, Language.Highlander}),
            new StateInfo("Kingdom", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.KingEuro, ProfessionInfo.KingHeirEuro, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorEuro2, ProfessionInfo.GovernorEuro2, BuildingSize.Unique)), 
                true, 2, 6, false, SocialOrder.MedievalEurope2, new Language[] {Language.Drow, Language.Elven, Language.European, Language.Highlander}),
            new StateInfo("Empire", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.EmperorEuro, ProfessionInfo.KingHeirEuro, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorEuro3, ProfessionInfo.GovernorEuro3, BuildingSize.Unique)), 
                true, 2, 6, true, SocialOrder.MedievalEurope2, new Language[] {Language.Drow, Language.Elven, Language.European, Language.Dwarwen}),            
            new StateInfo("Reich", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.KingNorth, ProfessionInfo.KingHeirNorth, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorNorth, ProfessionInfo.GovernorNorth, BuildingSize.Unique)), 
                true, 2, 6, false, SocialOrder.MedievalNorth, new Language[] {Language.Dwarwen, Language.Northman}),
            new StateInfo("Kaiserreich", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.EmperorNorth, ProfessionInfo.EmperorHeirNorth, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorNorth2, ProfessionInfo.GovernorNorth2, BuildingSize.Unique)), 
                true, 2, 6, true, SocialOrder.MedievalNorth, new Language[] {Language.Dwarwen, Language.Northman}),
            new StateInfo("Regnum", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.KingLatin, ProfessionInfo.KingHeirLatin, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorLatin, ProfessionInfo.GovernorLatin, BuildingSize.Unique)), 
                true, 2, 6, false, SocialOrder.MedievalLatin, new Language[] {Language.Latin}),
            new StateInfo("Imperium", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.EmperorLatin, ProfessionInfo.KingHeirLatin, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.KingLatin, ProfessionInfo.KingHeirLatin, BuildingSize.Unique)), 
                true, 2, 6, true, SocialOrder.MedievalLatin, new Language[] {Language.Latin}),
            new StateInfo("Shogunate", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.KingAsian, ProfessionInfo.KingHeirAsian, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorAsian, ProfessionInfo.GovernorAsian, BuildingSize.Unique)), 
                true, 2, 6, false, SocialOrder.MedievalAsian, new Language[] {Language.Asian}),            
            new StateInfo("Empire", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.EmperorAsian, ProfessionInfo.EmperorHeirAsian, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorAsian2, ProfessionInfo.GovernorAsian2, BuildingSize.Unique)), 
                true, 2, 6, true, SocialOrder.MedievalAsian, new Language[] {Language.Asian}),
            new StateInfo("Kingdom", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.KingArabian, ProfessionInfo.KingHeirArabian, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorArabian, ProfessionInfo.GovernorArabian, BuildingSize.Unique)), 
                true, 2, 6, false, SocialOrder.MedievalArabian, new Language[] {Language.Arabian}),
            new StateInfo("Sultanate", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.KingArabian2, ProfessionInfo.KingHeirArabian2, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorArabian, ProfessionInfo.GovernorArabian, BuildingSize.Unique)), 
                true, 2, 6, false, SocialOrder.MedievalArabian, new Language[] {Language.Arabian, Language.African}),            
            new StateInfo("Caliphate", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.EmperorArabian, ProfessionInfo.KingHeirEuro, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorArabian, ProfessionInfo.GovernorArabian, BuildingSize.Unique)), 
                true, 2, 6, true, SocialOrder.MedievalArabian, new Language[] {Language.Arabian, Language.African}),
            new StateInfo("Khanate", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.KingMongol, ProfessionInfo.KingHeirEuro, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorMongol, ProfessionInfo.GovernorMongol, BuildingSize.Unique)), 
                true, 2, 6, false, SocialOrder.MedievalMongol, new Language[] {Language.Orkish, Language.Eskimoid}),
            new StateInfo("Khaganate", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.EmperorMongol, ProfessionInfo.KingHeirEuro, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.KingMongol, ProfessionInfo.KingHeirEuro, BuildingSize.Unique)), 
                true, 2, 6, true, SocialOrder.MedievalMongol, new Language[] {Language.Orkish, Language.Eskimoid}),
            new StateInfo("Knyazdom", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.KingSlavic, ProfessionInfo.KingHeirSlavic, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorSlavic, ProfessionInfo.GovernorSlavic, BuildingSize.Unique)), 
                true, 2, 6, false, SocialOrder.MedievalSlavic, new Language[] {Language.Slavic}),
            new StateInfo("Tsardom", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.EmperorSlavic, ProfessionInfo.EmperorHeirSlavic, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.KingSlavic, ProfessionInfo.KingHeirSlavic, BuildingSize.Unique)), 
                true, 2, 6, true, SocialOrder.MedievalSlavic, new Language[] {Language.Slavic}),
            new StateInfo("Basileia", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.KingGreek, ProfessionInfo.KingHeirGreek, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorGreek, ProfessionInfo.GovernorGreek, BuildingSize.Unique)), 
                true, 2, 6, false, SocialOrder.MedievalGreek, new Language[] {Language.Greek}),
            new StateInfo("Autokratoria", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.EmperorGreek, ProfessionInfo.KingHeirGreek, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorGreek2, ProfessionInfo.GovernorGreek2, BuildingSize.Unique)), 
                true, 2, 6, true, SocialOrder.MedievalGreek, new Language[] {Language.Greek}),
            new StateInfo("Raj", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.KingHindu, ProfessionInfo.KingHeirHindu, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorHindu, ProfessionInfo.GovernorHindu, BuildingSize.Unique)), 
                true, 2, 6, false, SocialOrder.MedievalHindu, new Language[] {Language.Hindu}),            
            new StateInfo("Empire", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.EmperorHindu, ProfessionInfo.EmperorHeirHindu, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.KingHindu, ProfessionInfo.KingHeirHindu, BuildingSize.Unique)), 
                true, 2, 6, true, SocialOrder.MedievalHindu, new Language[] {Language.Hindu}),
            new StateInfo("Altepetl", 1,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.KingAztec, ProfessionInfo.KingHeirAztec, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorAztec, ProfessionInfo.GovernorAztec, BuildingSize.Unique)), 
                true, 2, 6, false, SocialOrder.MedievalAztec, new Language[] {Language.Aztec}),           
            new StateInfo("Empire", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Castle", ProfessionInfo.EmperorEuro, ProfessionInfo.KingHeirAztec, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 7, new BuildingInfo("Castle", ProfessionInfo.GovernorAztec, ProfessionInfo.GovernorAztec, BuildingSize.Unique)), 
                true, 2, 6, true, SocialOrder.MedievalAztec, new Language[] {Language.Aztec}),
            new StateInfo("Republic", 2,
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 14, new BuildingInfo("Statehouse", ProfessionInfo.RulerModern, ProfessionInfo.AdvisorModern, BuildingSize.Unique)), 
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
            new StateInfo("Federation", 2,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 16, new BuildingInfo("Statehouse", ProfessionInfo.RulerModern2, ProfessionInfo.AdvisorModern2, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 15, new BuildingInfo("Town hall", ProfessionInfo.GovernorModern2, ProfessionInfo.GovernorModern2, BuildingSize.Unique)), 
                false, 4, 7, false, SocialOrder.Modern, null),
            new StateInfo("League", 3,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 16, new BuildingInfo("Palace", ProfessionInfo.RulerModern3, ProfessionInfo.AdvisorModern3, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 15, new BuildingInfo("Palace", ProfessionInfo.GovernorModern, ProfessionInfo.GovernorModern, BuildingSize.Unique)), 
                false, 4, 7, true, SocialOrder.Modern, null),
            new StateInfo("Union", 3,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 16, new BuildingInfo("Statehouse", ProfessionInfo.RulerModern2, ProfessionInfo.AdvisorModern2, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 15, new BuildingInfo("Town hall", ProfessionInfo.GovernorModern2, ProfessionInfo.GovernorModern2, BuildingSize.Unique)), 
                false, 5, 7, true, SocialOrder.Modern, null),
            new StateInfo("Alliance", 3,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 16, new BuildingInfo("Palace", ProfessionInfo.RulerModern4, ProfessionInfo.AdvisorModern, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 15, new BuildingInfo("Town hall", ProfessionInfo.GovernorModern2, ProfessionInfo.GovernorModern2, BuildingSize.Unique)), 
                false, 5, 7, true, SocialOrder.Modern, null),
            new StateInfo("Coalition", 3,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 16, new BuildingInfo("Statehouse", ProfessionInfo.RulerModern3, ProfessionInfo.AdvisorModern, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 15, new BuildingInfo("Town hall", ProfessionInfo.GovernorModern2, ProfessionInfo.GovernorModern2, BuildingSize.Unique)), 
                false, 5, 7, true, SocialOrder.Modern, null),
            new StateInfo("Association", 3,
                new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 16, new BuildingInfo("Statehouse", ProfessionInfo.RulerModern2, ProfessionInfo.AdvisorModern4, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.City, "City", 40, 80, 15, new BuildingInfo("Town hall", ProfessionInfo.GovernorModern, ProfessionInfo.GovernorModern, BuildingSize.Unique)), 
                false, 5, 7, true, SocialOrder.Modern, null),
            //new StateInfo("Realm", 17,
            //    new SettlementInfo(SettlementSize.Capital, "City", 40, 80, 16, 5, 10, new BuildingInfo("Citadel", "God-King", "Goddess-Queen", 17)), 
            //    new SettlementInfo(SettlementSize.City, "City", 40, 80, 15, 5, 10, new BuildingInfo("Palace", "Father", "Mother", 15)), 
            //    "Brother", "Sister", false, 5, 8, null),
            new StateInfo("Commonwealth", 1,
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 16, new BuildingInfo("Town hall", ProfessionInfo.RulerModern3, ProfessionInfo.AdvisorModern5, BuildingSize.Unique)), 
                new SettlementInfo(SettlementSize.Town, "Town", 20, 40, 15, new BuildingInfo("Town hall", ProfessionInfo.GovernorModern3, ProfessionInfo.GovernorModern3, BuildingSize.Unique)), 
                false, 7, 8, false, SocialOrder.Future, null),
            new StateInfo("Society", 1,
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, 16, null), 
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, 15, null), 
                false, 7, 8, false, SocialOrder.Future, null),
            new StateInfo("Collective", 2,
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, 16, null), 
                new SettlementInfo(SettlementSize.Village, "Village", 10, 20, 15, null), 
                false, 7, 8, true, SocialOrder.Future, null),
        };
        #endregion

        public class Infrastructure
        {
            public RoadQuality m_eMaxGroundRoad;
            public RoadQuality m_eMaxNavalPath;
            public int m_iAerialAvailability;
            public List<SettlementSize> m_cAvailableSettlements;

            public Infrastructure(RoadQuality eMaxRoad, RoadQuality eMaxNaval, int iAerial, SettlementSize[] cSettlements)
            {
                m_eMaxGroundRoad = eMaxRoad;
                m_eMaxNavalPath = eMaxNaval;
                m_iAerialAvailability = iAerial;
                m_cAvailableSettlements = new List<SettlementSize>(cSettlements);
            }
        }

        public static Infrastructure[] InfrastructureLevels = 
        {
            // 0 - только не соединённые дорогами поселения
            new Infrastructure(RoadQuality.None, RoadQuality.None, 0, new SettlementSize[]{SettlementSize.Hamlet}),
            // 1 - можно строить деревни, просёлочные дороги и короткие морские маршруты
            new Infrastructure(RoadQuality.Country, RoadQuality.Country, 0, new SettlementSize[]{SettlementSize.Hamlet, SettlementSize.Village}),
            // 2 - можно строить городки, обычные дороги и морские маршруты средней дальности
            new Infrastructure(RoadQuality.Normal, RoadQuality.Normal, 0, new SettlementSize[]{SettlementSize.Hamlet, SettlementSize.Village, SettlementSize.Town, SettlementSize.Fort}),
            // 3 - большие города, имперские дороги, неограниченные морские маршруты
            new Infrastructure(RoadQuality.Good, RoadQuality.Good, 0, new SettlementSize[]{SettlementSize.Hamlet, SettlementSize.Village, SettlementSize.Town, /*SettlementSize.City, */SettlementSize.Fort}),
            // 4 - большие города, имперские дороги, неограниченные морские маршруты, доступ к воздушному сообщению только в столице
            new Infrastructure(RoadQuality.Good, RoadQuality.Good, 1, new SettlementSize[]{SettlementSize.Hamlet, SettlementSize.Village, SettlementSize.Town, SettlementSize.City, SettlementSize.Fort}),
            // 5 - большие города, имперские дороги, неограниченные морские маршруты, доступ к воздушному сообщению только в столице и центрах провинций
            new Infrastructure(RoadQuality.Good, RoadQuality.Good, 2, new SettlementSize[]{SettlementSize.Village, SettlementSize.Town, SettlementSize.City, SettlementSize.Fort}),
            // 6 - большие города, имперские дороги, неограниченные морские маршруты, доступ к воздушному сообщению во всех поселениях
            new Infrastructure(RoadQuality.Good, RoadQuality.Good, 3, new SettlementSize[]{SettlementSize.Town, SettlementSize.City, SettlementSize.Fort}),
            // 7 - небольшие города, обычные дороги, морской транспорт не используется, доступ к воздушному сообщению во всех поселениях
            new Infrastructure(RoadQuality.Normal, RoadQuality.None, 3, new SettlementSize[]{SettlementSize.Hamlet, SettlementSize.Village, SettlementSize.Town, SettlementSize.Fort}),
            // 8 - деревни, просёлочные дороги, морской транспорт не используется, доступ к воздушному сообщению во всех поселениях
            new Infrastructure(RoadQuality.Country, RoadQuality.None, 3, new SettlementSize[]{SettlementSize.Hamlet, SettlementSize.Village, SettlementSize.Fort}),
        }; 
        
        public string m_sName;

        private static State m_pForbidden = new State();

        public bool Forbidden
        {
            get { return this == State.m_pForbidden; }
        }

        public List<Province> m_cContents = new List<Province>();

        /// <summary>
        /// границы с другими государствами
        /// </summary>
        private Dictionary<object, List<Location.Edge>> m_cBorderWith = new Dictionary<object, List<Location.Edge>>();

        private object m_pOwner = null;

        public object Owner
        {
            get { return m_pOwner; }
            set { m_pOwner = value; }
        }

        /// <summary>
        /// границы с другими государствами
        /// </summary>
        public Dictionary<object, List<Location.Edge>> BorderWith
        {
            get { return m_cBorderWith; }
        }

        /// <summary>
        /// соседние государствами (включая те, с которыми только морское сообщение)
        /// </summary>
        public object[] m_aBorderWith = null;

        private float m_fPerimeter = 0;

        public float PerimeterLength
        {
            get { return m_fPerimeter; }
        }

        internal void FillBorderWithKeys()
        {
            m_aBorderWith = new List<object>(m_cBorderWith.Keys).ToArray();

            m_fPerimeter = 0;
            foreach (var pBorder in m_cBorderWith)
                foreach (var pLine in pBorder.Value)
                    m_fPerimeter += pLine.m_fLength;
        }

        public Province m_pMethropoly = null;

        public override void Start(Province pSeed)
        {
            if (pSeed.Owner != null)
                throw new Exception("This province already belongs to state!!!");

            m_cBorderWith.Clear();
            m_cContents.Clear();

            base.Start(pSeed);

            //TestChain();

            m_pMethropoly = pSeed;

            m_pNation = m_pMethropoly.m_pNation;

            m_cContents.Add(pSeed);
            pSeed.Owner = this;
        }

        public bool ForcedGrow()
        {
            object[] aBorder = new List<object>(m_cBorder.Keys).ToArray();

            bool bFullyGrown = true;

            foreach (ITerritory pTerr in aBorder)
            {
                if (pTerr.Forbidden)
                    continue;

                Province pProvince = pTerr as Province;

                if (pProvince != null && pProvince.Owner == null && !pProvince.m_pCenter.IsWater && m_pMethropoly.m_pNation.m_pRace.m_pLanguage == pProvince.m_pNation.m_pRace.m_pLanguage)
                {
                    AddProvince(pProvince);
                    bFullyGrown = false;
                }
            }

            return !bFullyGrown;
        }

        /// <summary>
        /// Присоединяет к стране сопредельную ничейную провинцию.
        /// Чем большая часть периметра ничейной провинции является общей с государством - тем выше вероятность того, что выбрана будет именно она.
        /// Так же на вероятность влияют взаимоотношения населяющих провинции народов и общность языка.
        /// Возвращает false, если больше расти некуда, иначе true.
        /// </summary>
        /// <returns></returns>
        public bool Grow(int iMaxStateSize)
        {
            //если государство уже достаточно большое - сваливаем.
            if (m_cContents.Count > iMaxStateSize)
                return false;

            Dictionary<Province, float> cChances = new Dictionary<Province, float>();

            foreach (ITerritory pTerr in m_cBorder.Keys)
            {
                if (pTerr.Forbidden)
                    continue;

                Province pProvince = pTerr as Province;

                if (pProvince != null && pProvince.Owner == null && !pProvince.m_pCenter.IsWater)
                {
                    if (m_pMethropoly.m_pNation.m_pRace.m_pLanguage != pProvince.m_pNation.m_pRace.m_pLanguage)
                        continue;

                    //int iHostility = m_pMethropoly.CalcHostility(pProvince);

                    //враждебное отношение - такую провинция не присоединяем ни при каких условиях.
                    //if (iHostility > 2)
                    //    continue;

                    //bool bHaveRoad = false;
                    //foreach (var pLinkedProvince in pProvince.m_cConnectionString)
                    //    if (pLinkedProvince.Value == "ok")
                    //    {
                    //        bHaveRoad = true;
                    //        break;
                    //    }

                    //if(!bHaveRoad)
                    //    iHostility = m_pMethropoly.CalcHostility(pProvince);

                    float fSharedPerimeter = 0;
                    foreach (var pLine in m_cBorder[pProvince])
                        fSharedPerimeter += pLine.m_fLength;

                    fSharedPerimeter /= pProvince.PerimeterLength;

                    if (fSharedPerimeter < 0.15f)
                        continue;

                    //дружественное отношение - для этой провинции шансы выше.
                    //if (iHostility < -1)
                    //    fSharedPerimeter *= -iHostility;
                    if (m_pMethropoly.m_pNation == pProvince.m_pNation)
                        fSharedPerimeter *= 2;

                    cChances[pProvince] = fSharedPerimeter;
                }
            }

            Province pAddon = null;

            int iChoice = Rnd.ChooseOne(cChances.Values, 2);

            if (iChoice < 0)
                return false;

            foreach (Province pProvince in cChances.Keys)
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

            AddProvince(pAddon);

            return true;
        }

        private void AddProvince(Province pAddon)
        {
            m_cContents.Add(pAddon);
            pAddon.Owner = this;

            //List<Line> cListLine = m_cBorder[pAddon];
            m_cBorder[pAddon].Clear();
            m_cBorder.Remove(pAddon);

            //List<Line> cNewBorder = new List<Line>();
            //List<Line> cFalseBorder = new List<Line>();
            foreach (var pLand in pAddon.BorderWith)
            {
                ITerritory pL = pLand.Key as ITerritory;

                if (!pL.Forbidden && m_cContents.Contains(pL))
                {
                    //foreach (Line pLine in pLand.Value)
                    //    cFalseBorder.Add(new Line(pLine));
                    continue;
                }

                if (!m_cBorder.ContainsKey(pL))
                    m_cBorder[pL] = new List<Location.Edge>();
                Location.Edge[] cLines = pLand.Value.ToArray();
                foreach (var pLine in cLines)
                {
                    m_cBorder[pL].Add(new Location.Edge(pLine));
                    //cNewBorder.Add(new Line(pLine));
                }
            }

            //TestChain();

            //if (cListLine.Count != cFalseBorder.Count && cFalseBorder.Count > 0)
            //{
            //    Line[] aListLine = SortLines(cListLine);
            //    Line[] aListLine2 = SortLines(cNewBorder);
            //    Line[] aListLine3 = SortLines(cFalseBorder);
            //}
        }

        private Line[] SortLines(List<Line> cListLine)
        {
            Line[] aListLine = new Line[cListLine.Count];
            int iIndex = -1;
            do
            {
                foreach (Line pLine in cListLine)
                {
                    if (iIndex < 0)
                    {
                        bool bPrevious = false;
                        foreach (Line pLine2 in cListLine)
                            if (pLine2.m_pPoint2.Y == pLine.m_pPoint1.Y)
                                bPrevious = true;

                        if (!bPrevious)
                            aListLine[++iIndex] = pLine;
                    }
                    else
                    {
                        if (pLine.m_pPoint1.Y == aListLine[iIndex].m_pPoint2.Y)
                            aListLine[++iIndex] = pLine;
                    }
                }
            }
            while (iIndex < cListLine.Count - 1);

            return aListLine;
        }

        /// <summary>
        /// Заполняет словарь границ с другими странами и гарантирует принадлежность государства той расе, которая доминирует на его территории.
        /// </summary>
        public void Finish()
        {
            ChainBorder();

            m_cBorderWith.Clear();

            //добавляем в общий список контуры границ с соседними государствами
            foreach (ITerritory pProvince in m_cBorder.Keys)
            {
                State pState;
                if (pProvince.Forbidden || (pProvince as Province).Owner == null)
                    pState = State.m_pForbidden;
                else
                    pState = (pProvince as Province).Owner as State;

                if (!m_cBorderWith.ContainsKey(pState))
                    m_cBorderWith[pState] = new List<Location.Edge>();
                m_cBorderWith[pState].AddRange(m_cBorder[pProvince]);
            }

            //добавляем в общий список пустые массивы контуров границ для тех государств, с которыми у нас 
            //морское сообщение
            foreach (Province pProvince in m_cContents)
            {
                foreach (LocationX pLoc in pProvince.m_cSettlements)
                    foreach (LocationX pOtherLoc in pLoc.m_cHaveSeaRouteTo)
                    { 
                        State pState = (pOtherLoc.Owner as LandX).m_pProvince.Owner as State;
                        if(pState != this && !m_cBorderWith.ContainsKey(pState))
                            m_cBorderWith[pState] = new List<Location.Edge>();
                    }
            }

            FillBorderWithKeys();
        }

        public void CalculateMagic()
        {
            m_iMagicLimit = 0;

            float fMagesCount = 0;
            float[] aDistribution = new float[10];

            foreach (Province pProvince in m_cContents)
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
            fMagesCount /= m_iPopulation;

            //m_eMagicAbilityPrevalence = MagicAbilityPrevalence.AlmostEveryone;

            //if (fMagesCount <= 0.75)
            //    m_eMagicAbilityPrevalence = MagicAbilityPrevalence.Common;

            //if (fMagesCount <= 0.25)
            //    m_eMagicAbilityPrevalence = MagicAbilityPrevalence.Rare;

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

        public Nation m_pNation = null;

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
        /// <summary>
        /// Уровень социального (не)равенства.
        /// 0 - рабство
        /// 1 - крепостное право
        /// 2 - капитализм
        /// 3 - социализм
        /// 4 - коммунизм
        /// </summary>
        public int m_iSocialEquality = 0;
        public int m_iCultureLevel = 0;

        public Culture m_pCulture = null;
        public Customs m_pCustoms = null;

        /// <summary>
        /// Как часто встрачаются носители магических способностей
        /// </summary>
        //public MagicAbilityPrevalence m_eMagicAbilityPrevalence = MagicAbilityPrevalence.Rare;
        /// <summary>
        /// Процент реально крутых магов среди всех носителей магических способностей
        /// </summary>
        public MagicAbilityDistribution m_eMagicAbilityDistribution = MagicAbilityDistribution.mostly_weak;

        public int m_iFood = 0;
        public int m_iOre = 0;
        public int m_iWood = 0;
        public int m_iPopulation = 0;

        public StateInfo m_pInfo = null;

        /// <summary>
        /// Строит столицу государства, рассчитывает уровни технического и культурного развития, определяет форму правления...
        /// </summary>
        /// <param name="iMinSize">предел количества провинций, ниже которого государство считается карликовым</param>
        /// <param name="iMaxSize">предел количества провинций, выше которого государство считается гигантским</param>
        /// <param name="bFast">флаг быстрой (упрощённой) генерации</param>
        /// <returns>локация, в которой построена столица</returns>
        public LocationX BuildCapital(int iMinSize, int iMaxSize, bool bFast)
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

            Dictionary<Nation, int> cNationsCount = new Dictionary<Nation, int>();

            int iMaxPop = 0;
            Nation pMaxNation = null;

            m_iInfrastructureLevel = 0;
            //float fInfrastructureLevel = 0;

            int iGrain = 0;
            int iGame = 0;
            int iFish = 0;

            foreach (Province pProvince in m_cContents)
            {
                int iCount = 0;
                if (!cNationsCount.TryGetValue(pProvince.m_pNation, out iCount))
                    cNationsCount[pProvince.m_pNation] = 0;
                cNationsCount[pProvince.m_pNation] = iCount + pProvince.m_iPopulation;
                if (cNationsCount[pProvince.m_pNation] > iMaxPop)
                {
                    iMaxPop = cNationsCount[pProvince.m_pNation];
                    pMaxNation = pProvince.m_pNation;
                }

                foreach (LocationX pLoc in pProvince.m_cSettlements)
                    foreach (LocationX pOtherLoc in pLoc.m_cHaveSeaRouteTo)
                    {
                        State pState = (pOtherLoc.Owner as LandX).m_pProvince.Owner as State;
                        if (pState != this && !m_cBorderWith.ContainsKey(pState))
                            m_cBorderWith[pState] = new List<Location.Edge>();
                    } 
                
                //m_iFood += (int)(pProvince.m_fGrain + pProvince.m_fFish + pProvince.m_fGame);
                iGrain += (int)pProvince.m_fGrain;
                iGame += (int)pProvince.m_fGame;
                iFish += (int)pProvince.m_fFish; 
                m_iWood += (int)pProvince.m_fWood;
                m_iOre += (int)pProvince.m_fOre;

                m_iPopulation += pProvince.m_iPopulation;

                if (pProvince.m_iTechLevel > m_iTechLevel)
                    m_iTechLevel = pProvince.m_iTechLevel;

                if (pProvince.m_iInfrastructureLevel > m_iInfrastructureLevel)
                    m_iInfrastructureLevel = pProvince.m_iInfrastructureLevel;
                //m_iInfrastructureLevel += pProvince.m_iInfrastructureLevel;
                //fInfrastructureLevel += 1.0f/(pProvince.m_iInfrastructureLevel+1);

                foreach (LandX pLand in pProvince.m_cContents)
                {
                    iAverageMagicLimit += pProvince.m_pNation.m_iMagicLimit * pLand.m_cContents.Count;
                }
            }
            
            if (pMaxNation != null)
            {
                m_pNation = pMaxNation;

                m_pMethropoly.m_pNation = pMaxNation;
                foreach (LandX pLand in m_pMethropoly.m_cContents)
                    pLand.m_pNation = m_pNation;
            }

            switch (m_pNation.m_pFenotype.m_pBody.m_eNutritionType)
            {
                case NutritionType.Eternal:
                    m_iFood = m_iPopulation;
                    break;
                case NutritionType.Mineral:
                    m_iFood = m_iOre;
                    break;
                case NutritionType.Organic:
                    m_iFood = iFish + iGrain + iGame;
                    break;
                case NutritionType.ParasitismBlood:
                    m_iFood = m_iPopulation;
                    break;
                case NutritionType.ParasitismEmote:
                    m_iFood = m_iPopulation;
                    break;
                case NutritionType.ParasitismEnergy:
                    m_iFood = m_iPopulation;
                    break;
                case NutritionType.ParasitismMeat:
                    m_iFood = m_iPopulation;
                    break;
                case NutritionType.Photosynthesis:
                    m_iFood = m_iPopulation;
                    break;
                case NutritionType.Thermosynthesis:
                    m_iFood = m_iPopulation;
                    break;
                case NutritionType.Vegetarian:
                    m_iFood = iGrain;
                    break;
                case NutritionType.Сarnivorous:
                    m_iFood = iFish + iGame;
                    break;
            }

            iAverageMagicLimit = iAverageMagicLimit / m_iPopulation;
            //m_iInfrastructureLevel /= m_cContents.Count;
            //m_iInfrastructureLevel = (int)(m_cContents.Count / fInfrastructureLevel + 0.1) - 1;

            if (m_cContents.Count == 1 && m_iInfrastructureLevel > 4)
                m_iInfrastructureLevel /= 2;

            if (m_pNation.m_bInvader)
            {
                if (m_iTechLevel > m_pNation.m_pEpoch.m_iInvadersMaxTechLevel)
                    m_iTechLevel = m_pNation.m_pEpoch.m_iInvadersMaxTechLevel;
            }
            else
            {
                if (m_iTechLevel > m_pNation.m_pEpoch.m_iNativesMaxTechLevel)
                    m_iTechLevel = m_pNation.m_pEpoch.m_iNativesMaxTechLevel;
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

            while (GetEffectiveTech() > m_iInfrastructureLevel * 2)
                m_iTechLevel--;

            if (m_iTechLevel < 0)
                m_iTechLevel = 0;

            List<StateInfo> cInfos = new List<StateInfo>();

            foreach (StateInfo pInfo in s_aInfo)
            {
                if (m_iInfrastructureLevel >= pInfo.m_iMinGovernmentLevel &&
                    m_iInfrastructureLevel <= pInfo.m_iMaxGovernmentLevel &&
                    (pInfo.m_cLanguages.Count == 0 ||
                     pInfo.m_cLanguages.Contains(m_pNation.m_pRace.m_pLanguage) ||
                     Rnd.OneChanceFrom(20)) &&
                    (m_iPopulation > iMaxSize * 80) == pInfo.m_bBig)
                    for (int i = 0; i < pInfo.m_iRank; i++ )
                        cInfos.Add(pInfo);
            }

            if (cInfos.Count == 0)
            {
                foreach (StateInfo pInfo in s_aInfo)
                {
                    if (m_iInfrastructureLevel >= pInfo.m_iMinGovernmentLevel &&
                        m_iInfrastructureLevel <= pInfo.m_iMaxGovernmentLevel &&
                        (m_iPopulation > iMaxSize * 80) == pInfo.m_bBig)
                        for (int i = 0; i < pInfo.m_iRank; i++)
                            cInfos.Add(pInfo);
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

            m_pInfo = cInfos[Rnd.Get(cInfos.Count)];

            m_iCultureLevel = (m_iInfrastructureLevel + m_pInfo.m_iMinGovernmentLevel) / 2;

            m_iControl = 2;

            m_iSocialEquality = 2;

            //if (m_pCulture.Moral[Culture.Morale.Agression] > 1)
            //    m_iControl++;

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

            if (m_pInfo.m_bDinasty)
                m_iSocialEquality--; 
            
            if (m_iSocialEquality < 0)
                m_iSocialEquality = 0;

            if (m_pCulture.MentalityValues[Mentality.Agression][m_iCultureLevel] < 1)
                m_iSocialEquality++;

            if (m_pCulture.MentalityValues[Mentality.Fanaticism][m_iCultureLevel] < 1)
                m_iSocialEquality++;

            if (m_iSocialEquality > 0 && m_iFood < m_iPopulation)
                m_iSocialEquality--;
            if (m_iFood > m_iPopulation && m_iOre > m_iPopulation && m_iWood > m_iPopulation)
                m_iSocialEquality++;

            //в либеральном обществе (фанатизм < 2/3) не может быть рабства (0) или крепостного права (1), т.е. только 2 и выше
            if (m_pCulture.MentalityValues[Mentality.Fanaticism][m_iCultureLevel] < 0.66)
                m_iSocialEquality = Math.Max(2, m_iSocialEquality);
            //в обществе абсолютных пацифистов (агрессивность < 1/3) не может быть даже капитализма (2), т.е. только 3 и выше
            if (m_pCulture.MentalityValues[Mentality.Agression][m_iCultureLevel] < 0.33)
                m_iSocialEquality = Math.Max(3, m_iSocialEquality);            
            //в обществе абсолютного самоотречения (эгоизм < 1/3) не может быть капитализма (2) - только или социализм, или феодализм
            if (m_pCulture.MentalityValues[Mentality.Selfishness][m_iCultureLevel] < 0.33)
                if(m_pInfo.m_bDinasty)
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
            if (m_iFood < m_iPopulation*2 || m_iOre < m_iPopulation*2 || m_iWood < m_iPopulation*2)
                m_iSocialEquality = Math.Min(3, m_iSocialEquality);

            //при всём уважении - какой нафиг социализм/коммунизм при наследственной власти???
            if (m_pInfo.m_bDinasty)
                m_iSocialEquality = Math.Min(2, m_iSocialEquality);

            if (m_iSocialEquality > 4)
                m_iSocialEquality = 4;

            if (m_pInfo.m_bDinasty)
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

            m_pMethropoly.m_pAdministrativeCenter.m_pSettlement = new Settlement(m_pInfo.m_pStateCapital, m_pMethropoly.m_pNation, m_iTechLevel, m_iMagicLimit, true, bFast);

            foreach (Province pProvince in m_cContents)
            {
                if (pProvince == m_pMethropoly)
                    continue;

                pProvince.m_pAdministrativeCenter.m_pSettlement = new Settlement(m_pInfo.m_pProvinceCapital, m_pMethropoly.m_pNation, m_iTechLevel, m_iMagicLimit, false, bFast);
            }

            if (m_pMethropoly.m_pCenter.Area != null)
                m_sName = m_pNation.m_pRace.m_pLanguage.RandomCountryName();

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

            if (m_pNation != pOpponent.m_pNation)
            {
                iHostility++;
                sNegativeReasons += " (-1) " + pOpponent.m_pNation.ToString() + "\n";

                if (m_pNation.m_pRace.m_pLanguage != pOpponent.m_pNation.m_pRace.m_pLanguage)
                {
                    iHostility++;
                    sNegativeReasons += " (-1) Different language\n";
                }
            }
            else
            {
                iHostility--;
                sPositiveReasons += " (+1) " + pOpponent.m_pNation.ToString() + "\n";
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

            if (pOpponent.m_iInfrastructureLevel > m_iInfrastructureLevel+1)
            {
                iHostility++;//= pOpponent.m_iLifeLevel - m_iLifeLevel;
                sNegativeReasons += string.Format(" (-{0}) Envy for civilization\n", 1);//pOpponent.m_iLifeLevel - m_iLifeLevel);
            }
            else
            {
                if (pOpponent.m_iInfrastructureLevel < m_iInfrastructureLevel-1)
                {
                    iHostility++;//= m_iLifeLevel - pOpponent.m_iLifeLevel;
                    sNegativeReasons += string.Format(" (-{0}) Scorn for savagery\n", 1);//m_iLifeLevel - pOpponent.m_iLifeLevel);
                }
            }

            int iEqualityDifference = Math.Abs(pOpponent.m_iSocialEquality - m_iSocialEquality);
            if (iEqualityDifference != 1)
            {
                iHostility += iEqualityDifference - 1;
                if (iEqualityDifference > 1)
                    sNegativeReasons += string.Format(" (-{1}) {0}\n", State.GetEqualityString(pOpponent.m_iSocialEquality), iEqualityDifference - 1);
                else
                    sPositiveReasons += string.Format(" (+{1}) {0}\n", State.GetEqualityString(pOpponent.m_iSocialEquality), 1);
            }

            float iCultureDifference = m_pCulture.GetDifference(pOpponent.m_pCulture, m_iCultureLevel, pOpponent.m_iCultureLevel);
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

                    int iMaxHostility = 0;
                    State pMainEnemy = null;

                    foreach (var pLinkedTerr in pLand.BorderWith)
                    {
                        if((pLinkedTerr.Key as ITerritory).Forbidden)
                            continue;

                        LandX pLinkedLand = pLinkedTerr.Key as LandX;

                        if (pLinkedLand.m_pProvince != null && pLinkedLand.m_pProvince.Owner == this)
                            continue;

                        int iHostility = 0;
                        if (pLinkedLand.m_pProvince != null)
                        {
                            State pLinkedState = pLinkedLand.m_pProvince.Owner as State;

                            Dictionary<State, int> cLinkedStateHostility;
                            if (!cHostility.TryGetValue(pLinkedState, out cLinkedStateHostility))
                            {
                                cLinkedStateHostility = new Dictionary<State, int>();
                                cHostility[pLinkedState] = cLinkedStateHostility;
                            }

                            if (!cLinkedStateHostility.TryGetValue(this, out iHostility))
                            {
                                iHostility = pLinkedState.CalcHostility(this);
                                cLinkedStateHostility[this] = iHostility;
                            }

                            if (iHostility <= 0)
                                continue;

                            if (iHostility > iMaxHostility)
                            {
                                iMaxHostility = iHostility;
                                pMainEnemy = pLinkedState;
                            }
                        }

                        Location.Edge[] cLines = pLinkedTerr.Value.ToArray();
                        foreach (var pLine in cLines)
                        {
                            fBorder += pLine.m_fLength / pLinkedLand.MovementCost;
                            if (pLinkedLand.m_pProvince == null)
                                fTreat += pLine.m_fLength / pLinkedLand.MovementCost;
                            else
                                fTreat += pLine.m_fLength * (float)Math.Sqrt(iHostility) / pLinkedLand.MovementCost;
                        }
                    }

                    if (fTreat == 0)
                        continue;

                    if (Rnd.ChooseOne(fTreat, fBorder));// - fTreat))
                        //if (m_iSize > 1 || Rnd.OneChanceFrom(2))
                        {
                            LocationX pFort = pLand.BuildFort(pMainEnemy, bFast);
                            if (pFort != null)
                            {
                                pLand.m_pProvince.m_cSettlements.Add(pFort);
                                //bHaveOne = true;
                            }
                        }
                }
            }

        public static string GetTechString(int iLevel, Customs.Progressiveness eProgress)
        {
            string sTech = "stone weapons";
            switch (iLevel)
            {
                case 0:
                    sTech = eProgress == Customs.Progressiveness.Technofetishism ? "obsidian weapons" : "stone weapons";
                    break;
                case 1:
                    sTech = eProgress == Customs.Progressiveness.Technofetishism ? "bronze weapons" : eProgress == Customs.Progressiveness.Traditionalism ? "stone weapons, rare iron weapons" : "iron weapons";
                    break;
                case 2:
                    sTech = eProgress == Customs.Progressiveness.Technofetishism ? "repeating crossbows" : eProgress == Customs.Progressiveness.Traditionalism ? "iron weapons, rare steel weapons" : "steel weapons";
                    break;
                case 3:
                    sTech = eProgress == Customs.Progressiveness.Technofetishism ? "multibarrel guns" : eProgress == Customs.Progressiveness.Traditionalism ? "steel weapons, rare muskets" : "muskets";
                    break;
                case 4:
                    sTech = eProgress == Customs.Progressiveness.Technofetishism ? "lightning guns" : eProgress == Customs.Progressiveness.Traditionalism ? "muskets, rare rifles" : "rifles";//railroads
                    break;
                case 5:
                    sTech = eProgress == Customs.Progressiveness.Technofetishism ? "smartguns" : eProgress == Customs.Progressiveness.Traditionalism ? "rifles, rare submachine guns" : "submachine guns";//aviation
                    break;
                case 6:
                    sTech = eProgress == Customs.Progressiveness.Technofetishism ? "mecha suits" : eProgress == Customs.Progressiveness.Traditionalism ? "submachine guns, rare beam guns" : "beam guns";
                    break;
                case 7:
                    sTech = eProgress == Customs.Progressiveness.Technofetishism ? "nanites" : eProgress == Customs.Progressiveness.Traditionalism ? "beam guns, rare desintegrators" : "desintegrators";//limited teleportation
                    break;
                case 8:
                    sTech = eProgress == Customs.Progressiveness.Traditionalism ? "desintegrators, rare reality destructors" : "reality destructors";//unlimited teleportation
                    break;
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

        public int GetEffectiveTech()
        {
            int iMaxTech = m_iTechLevel;
            if (m_pCustoms.m_eProgress == Customs.Progressiveness.Technofetishism)
                iMaxTech++;

            if (m_pCustoms.m_eProgress == Customs.Progressiveness.Traditionalism)
                iMaxTech--;

            if (iMaxTech > 8)
                iMaxTech = 8;
            if (iMaxTech < 0)
                iMaxTech = 0;

            return iMaxTech;
        }

        public int GetImportedTech()
        {
            if (m_pNation.m_bDying)
                return -1;

            int iMaxTech = GetEffectiveTech();
            foreach (State pState in m_aBorderWith)
            {
                if (pState.Forbidden)
                    continue;

                if (pState.GetEffectiveTech() > iMaxTech)
                    iMaxTech = pState.GetEffectiveTech();
            }

            if (iMaxTech <= GetEffectiveTech())
                iMaxTech = -1;

            return iMaxTech;
        }

        public string GetImportedTechString()
        {
            if (m_pNation.m_bDying)
                return "";

            int iMaxTech = GetEffectiveTech();
            State pExporter = null;
            foreach (State pState in m_aBorderWith)
            {
                if (pState.Forbidden)
                    continue;

                if (pState.GetEffectiveTech() > iMaxTech)
                {
                    iMaxTech = pState.GetEffectiveTech();
                    pExporter = pState;
                }
            }

            if (pExporter == null)
                return "";

            return State.GetTechString(pExporter.m_iTechLevel, pExporter.m_pCustoms.m_eProgress);
        }

        public override string ToString()
        {
            return string.Format("{2} (C{1}T{3}M{5}) - {0} {4}", m_sName, m_iCultureLevel, m_pNation, m_iTechLevel, m_pInfo.m_sName, m_iMagicLimit);
        }

        public override float GetMovementCost()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Достраиваем необходимые дороги между центрами провинций, так же вкючаем в дорожную сеть форты
        /// </summary>
        internal void FixRoads()
        {
            RoadQuality eRoadLevel = State.InfrastructureLevels[m_iInfrastructureLevel].m_eMaxGroundRoad;

            if (eRoadLevel == RoadQuality.None)
                return;

            //Закрываем границы, чтобы нельзя было "срезать" путь по чужой территории
            foreach (Province pProvince in m_cContents)
                foreach (LandX pLand in pProvince.m_cContents)
                    foreach (LocationX pLoc in pLand.m_cContents)
                        foreach (var pLinked in pLoc.m_cLinks)
                        {
                            if (pLinked.Key is LocationX)
                            {
                                LandX pLinkedOwner = (pLinked.Key as LocationX).Owner as LandX;
                                if (pLinkedOwner.m_pProvince == null || pLinkedOwner.m_pProvince.Owner != this || pLinked.Value.Sea)
                                    pLoc.m_cLinks[pLinked.Key].m_bClosed = true;
                            }
                            else
                                pLoc.m_cLinks[pLinked.Key].m_bClosed = true;
                        }

            //Сначала соеденим все центры провинций
            List<LocationX> cConnected = new List<LocationX>();
            cConnected.Add(m_pMethropoly.m_pAdministrativeCenter);

            while (cConnected.Count < m_cContents.Count)
            {
                //Road pBestRoad = null;
                LocationX pBestTown1 = null;
                LocationX pBestTown2 = null;
                float fMinLength = float.MaxValue;

                foreach (Province pProvince in m_cContents)
                {
                    LocationX pTown = pProvince.m_pAdministrativeCenter;

                    if (cConnected.Contains(pTown))
                        continue;

                    foreach (LocationX pOtherTown in cConnected)
                    {
                        float fDist = pTown.DistanceTo(pOtherTown);// (float)Math.Sqrt((pTown.X - pOtherTown.X) * (pTown.X - pOtherTown.X) + (pTown.Y - pOtherTown.Y) * (pTown.Y - pOtherTown.Y));

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
                    World.BuildRoad(pBestTown1, pBestTown2, eRoadLevel);

                    fMinLength = float.MaxValue;
                    LocationX pBestTown3 = null;
                    foreach (LocationX pOtherTown in cConnected)
                    {
                        float fDist = pBestTown1.DistanceTo(pOtherTown);// (float)Math.Sqrt((pBestTown1.X - pOtherTown.X) * (pBestTown1.X - pOtherTown.X) + (pBestTown1.Y - pOtherTown.Y) * (pBestTown1.Y - pOtherTown.Y));

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
                        World.BuildRoad(pBestTown1, pBestTown3, eRoadLevel);

                    cConnected.Add(pBestTown1);
                }
            }

            //теперь займёмся фортами
            cConnected.Clear();
            cConnected.Add(m_pMethropoly.m_pAdministrativeCenter);

            List<LocationX> cSettlements = new List<LocationX>();
            foreach (Province pProvince in m_cContents)
                cSettlements.AddRange(pProvince.m_cSettlements);
            LocationX[] aSettlements = cSettlements.ToArray();

            eRoadLevel = RoadQuality.Normal;
            if (eRoadLevel > State.InfrastructureLevels[m_iInfrastructureLevel].m_eMaxGroundRoad)
                eRoadLevel = State.InfrastructureLevels[m_iInfrastructureLevel].m_eMaxGroundRoad;

            List<LocationX> cForts = new List<LocationX>();
            foreach (LocationX pTown in aSettlements)
            {
                if (!cConnected.Contains(pTown) && (pTown.m_cRoads[RoadQuality.Normal].Count > 0 || pTown.m_cRoads[RoadQuality.Good].Count > 0))
                    cConnected.Add(pTown);
                else
                    if (pTown.m_pSettlement.m_pInfo.m_eSize == SettlementSize.Fort)
                        cForts.Add(pTown);
            }
            LocationX[] aForts = cForts.ToArray();

            foreach (LocationX pFort in aForts)
            {
                LocationX pBestTown = null;
                float fMinLength = float.MaxValue;

                foreach (LocationX pOtherTown in cConnected)
                {
                    float fDist = pFort.DistanceTo(pOtherTown);// (float)Math.Sqrt((pTown.X - pOtherTown.X) * (pTown.X - pOtherTown.X) + (pTown.Y - pOtherTown.Y) * (pTown.Y - pOtherTown.Y));

                    if (fDist < fMinLength &&
                        (fMinLength == float.MaxValue ||
                            Rnd.OneChanceFrom(2)))
                    {
                        fMinLength = fDist;
                        pBestTown = pOtherTown;
                    }
                }
                if (pBestTown != null)
                {
                    World.BuildRoad(pFort, pBestTown, eRoadLevel);

                    fMinLength = float.MaxValue;
                    LocationX pBestTown2 = null;
                    foreach (LocationX pOtherTown in cConnected)
                    {
                        float fDist = pFort.DistanceTo(pOtherTown);// (float)Math.Sqrt((pBestTown1.X - pOtherTown.X) * (pBestTown1.X - pOtherTown.X) + (pBestTown1.Y - pOtherTown.Y) * (pBestTown1.Y - pOtherTown.Y));

                        if (pOtherTown != pBestTown &&
                            fDist < fMinLength &&
                            (fMinLength == float.MaxValue ||
                             Rnd.OneChanceFrom(2)))
                        {
                            fMinLength = fDist;
                            pBestTown2 = pOtherTown;
                        }
                    }

                    if (pBestTown2 != null)
                        World.BuildRoad(pFort, pBestTown2, eRoadLevel);

                    cConnected.Add(pFort);
                }
            }

            //открываем закрытые в начале функции границы
            foreach (Province pProvince in m_cContents)
                foreach (LandX pLand in pProvince.m_cContents)
                    foreach (LocationX pLoc in pLand.m_cContents)
                        foreach (TransportationNode pLink in pLoc.m_cLinks.Keys)
                            pLoc.m_cLinks[pLink].m_bClosed = false;
        }

        public void SpecializeSettlements()
        {
            foreach(Province pProvince in m_cContents)
                foreach (LocationX pLoc in pProvince.m_cSettlements)
                {
                    if (pLoc.m_pSettlement.m_eSpeciality != SettlementSpeciality.None)
                        continue;

                    bool bCoast = false;
                    foreach (LocationX pLink in pLoc.m_aBorderWith)
                    {
                        if (pLink.Owner != null && (pLink.Owner as LandX).IsWater)
                            bCoast = true;
                    }

                    LandX pLand = pLoc.Owner as LandX;

                    switch (pLoc.m_pSettlement.m_pInfo.m_eSize)
                    {
                        case SettlementSize.Hamlet:
                            if (bCoast)
                            {
                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Fishers;
                            }
                            else
                            {
                                List<float> cResources = new List<float>();
                                cResources.Add(pLand.Type.m_fGrain * pLand.m_cContents.Count);
                                cResources.Add(pLand.Type.m_fGame * pLand.m_cContents.Count);

                                //в развитом обществе охота - это уже не способ добычи пищи, а больше развлечение
                                if (m_iInfrastructureLevel > 2)
                                {
                                    cResources[0] += cResources[1];
                                    cResources[1] = 0;
                                }

                                if (m_iInfrastructureLevel > 1)
                                {
                                    cResources.Add(pLand.Type.m_fOre * pLand.m_cContents.Count);
                                    cResources.Add(pLand.Type.m_fWood * pLand.m_cContents.Count);
                                }

                                int iChoosen = Rnd.ChooseOne(cResources, 2);

                                switch (iChoosen)
                                {
                                    case 0:
                                        pLoc.m_pSettlement.m_eSpeciality = m_iInfrastructureLevel >= 4 ? SettlementSpeciality.Farmers : SettlementSpeciality.Peasants;
                                        break;
                                    case 1:
                                        pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Hunters;
                                        break;
                                    case 2:
                                        pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Miners;
                                        break;
                                    case 3:
                                        pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Lumberjacks;
                                        break;
                                }
                            }
                            break;
                        case SettlementSize.Village:
                            if (bCoast)
                            {
                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Fishers;
                            }
                            else
                            {
                                List<float> cResources = new List<float>();
                                cResources.Add(pLand.Type.m_fGrain * pLand.m_cContents.Count);
                                cResources.Add(pLand.Type.m_fGame * pLand.m_cContents.Count);
                                cResources.Add(pLand.Type.m_fOre * pLand.m_cContents.Count);
                                cResources.Add(pLand.Type.m_fWood * pLand.m_cContents.Count);

                                //в развитом обществе охота - это уже не способ добычи пищи, а больше развлечение
                                if (m_iInfrastructureLevel > 2)
                                {
                                    cResources[0] += cResources[1];
                                    cResources[1] = 0;
                                }

                                int iChoosen = Rnd.ChooseOne(cResources, 2);

                                switch (iChoosen)
                                {
                                    case 0:
                                        pLoc.m_pSettlement.m_eSpeciality = m_iInfrastructureLevel >= 4 ? SettlementSpeciality.Farmers : SettlementSpeciality.Peasants;
                                        break;
                                    case 1:
                                        pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Hunters;
                                        break;
                                    case 2:
                                        pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Miners;
                                        break;
                                    case 3:
                                        pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Lumberjacks;
                                        break;
                                }
                            }
                            break;
                        case SettlementSize.Town:
                            if (bCoast && !Rnd.OneChanceFrom(3))
                            {
                                if (Rnd.OneChanceFrom(2))
                                    pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Naval;
                                else
                                    pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Fishers;
                            }
                            else
                            {
                                if (m_pCulture.MentalityValues[Mentality.Fanaticism][m_iInfrastructureLevel] < 1 + Rnd.Get(1f))
                                {
                                    List<float> cResources = new List<float>();
                                    cResources.Add(m_iFood);
                                    cResources.Add(m_iOre);
                                    cResources.Add(m_iWood);

                                    int iChoosen = Rnd.ChooseOne(cResources, 2);

                                    switch (iChoosen)
                                    {
                                        case 0:
                                            pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Tailors;
                                            break;
                                        case 1:
                                            pLoc.m_pSettlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Jevellers : SettlementSpeciality.Factory;
                                            break;
                                        case 2:
                                            pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Artisans;
                                            break;
                                    }
                                }
                                else
                                {
                                    List<float> cResources = new List<float>();
                                    cResources.Add(pProvince.m_fGame + pProvince.m_fGrain);
                                    cResources.Add(pProvince.m_fOre);
                                    cResources.Add(pProvince.m_fWood);

                                    int iChoosen = Rnd.ChooseOne(cResources, 2);

                                    switch (iChoosen)
                                    {
                                        case 0:
                                            pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Tailors;
                                            break;
                                        case 1:
                                            pLoc.m_pSettlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Jevellers : SettlementSpeciality.Factory;
                                            break;
                                        case 2:
                                            pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Artisans;
                                            break;
                                    }
                                }
                            }
                            break;
                        case SettlementSize.City:
                            if (bCoast && Rnd.OneChanceFrom(2))
                                pLoc.m_pSettlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.NavalAcademy : SettlementSpeciality.Naval;
                            else
                            {
                                if (bCoast && m_iInfrastructureLevel > 2 && m_pCulture.MentalityValues[Mentality.Rudeness][m_iInfrastructureLevel] > 1 + Rnd.Get(1f))
                                    pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Resort;
                                else
                                {
                                    if (Rnd.OneChanceFrom(2))
                                    {
                                        List<float> cResources = new List<float>();
                                        cResources.Add(2 - m_pCulture.MentalityValues[Mentality.Rudeness][m_iInfrastructureLevel]);
                                        cResources.Add(m_pCulture.MentalityValues[Mentality.Piety][m_iInfrastructureLevel]);
                                        cResources.Add(m_pCulture.MentalityValues[Mentality.Agression][m_iInfrastructureLevel]);
                                        cResources.Add(m_pCulture.MentalityValues[Mentality.Treachery][m_iInfrastructureLevel]);

                                        float fScience = 0.05f;
                                        if (m_pCustoms.m_eMindSet == Customs.MindSet.Balanced_mind)
                                            fScience = 0.25f;
                                        if (m_pCustoms.m_eMindSet == Customs.MindSet.Logic)
                                            fScience = 0.5f;

                                        if (m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Sapient)
                                            fScience *= 2;
                                        if (m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Ingenious)
                                            fScience *= 4;

                                        cResources.Add(fScience);
                                        //                                    cResources.Add(2 - m_pCulture.MentalityValues[Mentality.Selfishness][m_iInfrastructureLevel]);

                                        int iChoosen = Rnd.ChooseOne(cResources, 2);

                                        switch (iChoosen)
                                        {
                                            case 0:
                                                pLoc.m_pSettlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Cultural : SettlementSpeciality.ArtsAcademy;
                                                break;
                                            case 1:
                                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Religious;
                                                break;
                                            case 2:
                                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.MilitaryAcademy;
                                                break;
                                            case 3:
                                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Gambling;
                                                break;
                                            case 4:
                                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.SciencesAcademy;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        if (m_pCulture.MentalityValues[Mentality.Fanaticism][m_iInfrastructureLevel] < 1 + Rnd.Get(1f))
                                        {
                                            List<float> cResources = new List<float>();
                                            cResources.Add(m_iFood);
                                            cResources.Add(m_iOre);
                                            cResources.Add(m_iWood);

                                            int iChoosen = Rnd.ChooseOne(cResources, 2);

                                            switch (iChoosen)
                                            {
                                                case 0:
                                                    pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Tailors;
                                                    break;
                                                case 1:
                                                    pLoc.m_pSettlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Jevellers : SettlementSpeciality.Factory;
                                                    break;
                                                case 2:
                                                    pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Artisans;
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            List<float> cResources = new List<float>();
                                            cResources.Add(pProvince.m_fGame + pProvince.m_fGrain);
                                            cResources.Add(pProvince.m_fOre);
                                            cResources.Add(pProvince.m_fWood);

                                            int iChoosen = Rnd.ChooseOne(cResources, 2);

                                            switch (iChoosen)
                                            {
                                                case 0:
                                                    pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Tailors;
                                                    break;
                                                case 1:
                                                    pLoc.m_pSettlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Jevellers : SettlementSpeciality.Factory;
                                                    break;
                                                case 2:
                                                    pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Artisans;
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case SettlementSize.Capital:
                            if (bCoast && Rnd.OneChanceFrom(2))
                                pLoc.m_pSettlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.NavalAcademy : SettlementSpeciality.Naval;
                            else
                            {
                                if (bCoast && m_iInfrastructureLevel > 2 && m_pCulture.MentalityValues[Mentality.Rudeness][m_iInfrastructureLevel] > 1 + Rnd.Get(1f))
                                    pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Resort;
                                else
                                {
                                    if (Rnd.OneChanceFrom(2))
                                    {
                                        List<float> cResources = new List<float>();
                                        cResources.Add(2 - m_pCulture.MentalityValues[Mentality.Rudeness][m_iInfrastructureLevel]);
                                        cResources.Add(m_pCulture.MentalityValues[Mentality.Piety][m_iInfrastructureLevel]);
                                        cResources.Add(m_pCulture.MentalityValues[Mentality.Agression][m_iInfrastructureLevel]);
                                        cResources.Add(m_pCulture.MentalityValues[Mentality.Treachery][m_iInfrastructureLevel]);

                                        float fScience = 0.05f;
                                        if (m_pCustoms.m_eMindSet == Customs.MindSet.Balanced_mind)
                                            fScience = 0.25f;
                                        if (m_pCustoms.m_eMindSet == Customs.MindSet.Logic)
                                            fScience = 0.5f;

                                        if (m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Sapient)
                                            fScience *= 2;
                                        if (m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Ingenious)
                                            fScience *= 4;

                                        cResources.Add(fScience);
                                        //                                    cResources.Add(2 - m_pCulture.MentalityValues[Mentality.Selfishness][m_iInfrastructureLevel]);

                                        int iChoosen = Rnd.ChooseOne(cResources, 2);

                                        switch (iChoosen)
                                        {
                                            case 0:
                                                pLoc.m_pSettlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Cultural : SettlementSpeciality.ArtsAcademy;
                                                break;
                                            case 1:
                                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Religious;
                                                break;
                                            case 2:
                                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.MilitaryAcademy;
                                                break;
                                            case 3:
                                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Gambling;
                                                break;
                                            case 4:
                                                pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.SciencesAcademy;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        if (m_pCulture.MentalityValues[Mentality.Fanaticism][m_iInfrastructureLevel] < 1 + Rnd.Get(1f))
                                        {
                                            List<float> cResources = new List<float>();
                                            cResources.Add(m_iFood);
                                            cResources.Add(m_iOre);
                                            cResources.Add(m_iWood);

                                            int iChoosen = Rnd.ChooseOne(cResources, 2);

                                            switch (iChoosen)
                                            {
                                                case 0:
                                                    pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Tailors;
                                                    break;
                                                case 1:
                                                    pLoc.m_pSettlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Jevellers : SettlementSpeciality.Factory;
                                                    break;
                                                case 2:
                                                    pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Artisans;
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            List<float> cResources = new List<float>();
                                            cResources.Add(pProvince.m_fGame + pProvince.m_fGrain);
                                            cResources.Add(pProvince.m_fOre);
                                            cResources.Add(pProvince.m_fWood);

                                            int iChoosen = Rnd.ChooseOne(cResources, 2);

                                            switch (iChoosen)
                                            {
                                                case 0:
                                                    pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Tailors;
                                                    break;
                                                case 1:
                                                    pLoc.m_pSettlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Jevellers : SettlementSpeciality.Factory;
                                                    break;
                                                case 2:
                                                    pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Artisans;
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case SettlementSize.Fort:
                            if (bCoast)
                                if (m_pCulture.MentalityValues[Mentality.Agression][m_iInfrastructureLevel] > 1.5 &&
                                    m_pCulture.MentalityValues[Mentality.Treachery][m_iInfrastructureLevel] > 1.5)
                                    pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Pirates;
                                else
                                    pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Naval;
                            else
                                if (m_pCulture.MentalityValues[Mentality.Agression][m_iInfrastructureLevel] > 1.5 &&
                                    m_pCulture.MentalityValues[Mentality.Treachery][m_iInfrastructureLevel] > 1.5)
                                    pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Raiders;
                                else
                                    pLoc.m_pSettlement.m_eSpeciality = SettlementSpeciality.Military;
                            break;
                    }
                }
        }
    }
}
