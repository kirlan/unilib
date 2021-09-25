using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RB.Socium.Languages;
using RB.Socium.Psichology;
using RB.Genetix;
using Random;
using RB.Genetix.GenetixParts;
using RB.Geography;

namespace RB.Socium
{
    public class CState: CSociety
    {
        #region States Info Array
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

            public CSocialOrder m_pSocial = null;

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
            public StateInfo(string sName, int iRank, SettlementInfo pStateCapital, SettlementInfo pProvinceCapital, bool bDinasty, int iMinGovernmentLevel, int iMaxGovernmentLevel, bool bBig, CSocialOrder pSocial, Language[] cLanguages)
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

        private static StateInfo[] s_aInfo = 
        {
            new StateInfo("Land", 1,
                new SettlementInfo(SettlementSize.Hamlet, "Hamlet", new BuildingInfo("Elder's hut", ProfessionInfo.RulerPrimitive, ProfessionInfo.HeirPrimitive, BuildingSize.Small, FamilyOwnership.Full)), 
                new SettlementInfo(SettlementSize.Hamlet, "Hamlet", new BuildingInfo("Elder's hut", ProfessionInfo.GovernorPrimitive, ProfessionInfo.GovernorPrimitive, BuildingSize.Unique)), 
                true, 0, 0, false, CSocialOrder.Primitive, null),            
            new StateInfo("Lands", 1,
                new SettlementInfo(SettlementSize.Hamlet, "Hamlet", new BuildingInfo("Elder's hut", ProfessionInfo.RulerPrimitive, ProfessionInfo.HeirPrimitive, BuildingSize.Small, FamilyOwnership.Full)), 
                new SettlementInfo(SettlementSize.Hamlet, "Hamlet", new BuildingInfo("Elder's hut", ProfessionInfo.GovernorPrimitive, ProfessionInfo.GovernorPrimitive, BuildingSize.Unique)), 
                true, 0, 0, true, CSocialOrder.Primitive, null),
            new StateInfo("Tribes", 1,
                new SettlementInfo(SettlementSize.Village, "Village", new BuildingInfo("Clans hall", ProfessionInfo.RulerPrimitive, ProfessionInfo.HeirPrimitive, BuildingSize.Small, FamilyOwnership.Full)), 
                new SettlementInfo(SettlementSize.Village, "Village", new BuildingInfo("Village hall", ProfessionInfo.GovernorPrimitive, ProfessionInfo.GovernorPrimitive, BuildingSize.Unique)), 
                true, 1, 1, false, CSocialOrder.Primitive, null),
            new StateInfo("Clans", 1,
                new SettlementInfo(SettlementSize.Village, "Village", new BuildingInfo("Clans hall", ProfessionInfo.RulerPrimitive, ProfessionInfo.HeirPrimitive, BuildingSize.Small, FamilyOwnership.Full)), 
                new SettlementInfo(SettlementSize.Village, "Village", new BuildingInfo("Village hall", ProfessionInfo.GovernorPrimitive, ProfessionInfo.GovernorPrimitive, BuildingSize.Unique)), 
                true, 1, 1, true, CSocialOrder.Primitive, null),
            new StateInfo("Kingdom", 1,
                new SettlementInfo(SettlementSize.City, "City", new BuildingInfo("Castle", ProfessionInfo.KingEuro, ProfessionInfo.KingHeirEuro, BuildingSize.Small, FamilyOwnership.Full)), 
                new SettlementInfo(SettlementSize.Town, "Town", new BuildingInfo("Castle", ProfessionInfo.GovernorEuro, ProfessionInfo.GovernorEuro, BuildingSize.Unique)), 
                true, 2, 6, false, CSocialOrder.MedievalEurope, new Language[] {Language.Dwarwen, Language.European, Language.Highlander}),
            new StateInfo("Kingdom", 1,
                new SettlementInfo(SettlementSize.City, "City", new BuildingInfo("Castle", ProfessionInfo.KingEuro, ProfessionInfo.KingHeirEuro, BuildingSize.Small, FamilyOwnership.Full)), 
                new SettlementInfo(SettlementSize.Town, "Town", new BuildingInfo("Castle", ProfessionInfo.GovernorEuro2, ProfessionInfo.GovernorEuro2, BuildingSize.Unique)), 
                true, 2, 6, false, CSocialOrder.MedievalEurope2, new Language[] {Language.Drow, Language.Elven, Language.European, Language.Highlander}),
            new StateInfo("Empire", 2,
                new SettlementInfo(SettlementSize.City, "City", new BuildingInfo("Castle", ProfessionInfo.EmperorEuro, ProfessionInfo.KingHeirEuro, BuildingSize.Small, FamilyOwnership.Full)), 
                new SettlementInfo(SettlementSize.Town, "Town", new BuildingInfo("Castle", ProfessionInfo.GovernorEuro3, ProfessionInfo.GovernorEuro3, BuildingSize.Unique)), 
                true, 2, 6, true, CSocialOrder.MedievalEurope2, new Language[] {Language.Drow, Language.Elven, Language.European, Language.Dwarwen}),            
            new StateInfo("Kingdom", 1,
                new SettlementInfo(SettlementSize.City, "City", new BuildingInfo("Castle", ProfessionInfo.KingEuro, ProfessionInfo.KingHeirEuro, BuildingSize.Small, FamilyOwnership.Full)), 
                new SettlementInfo(SettlementSize.Town, "Town", new BuildingInfo("Castle", ProfessionInfo.GovernorEuro4, ProfessionInfo.GovernorEuro4, BuildingSize.Unique)), 
                true, 1, 6, false, CSocialOrder.MedievalEurope3, new Language[] {Language.Dwarwen, Language.European, Language.Highlander}),
            new StateInfo("Reich", 1,
                new SettlementInfo(SettlementSize.City, "City", new BuildingInfo("Castle", ProfessionInfo.KingNorth, ProfessionInfo.KingHeirNorth, BuildingSize.Small, FamilyOwnership.Full)), 
                new SettlementInfo(SettlementSize.Town, "Town", new BuildingInfo("Castle", ProfessionInfo.GovernorNorth, ProfessionInfo.GovernorNorth, BuildingSize.Unique)), 
                true, 1, 6, false, CSocialOrder.MedievalNorth, new Language[] {Language.Dwarwen, Language.Northman}),
            new StateInfo("Kaiserreich", 1,
                new SettlementInfo(SettlementSize.City, "City", new BuildingInfo("Castle", ProfessionInfo.EmperorNorth, ProfessionInfo.EmperorHeirNorth, BuildingSize.Small, FamilyOwnership.Full)), 
                new SettlementInfo(SettlementSize.Town, "Town", new BuildingInfo("Castle", ProfessionInfo.GovernorNorth2, ProfessionInfo.GovernorNorth2, BuildingSize.Unique)), 
                true, 2, 6, true, CSocialOrder.MedievalNorth, new Language[] {Language.Dwarwen, Language.Northman}),
            new StateInfo("Regnum", 1,
                new SettlementInfo(SettlementSize.City, "City", new BuildingInfo("Castle", ProfessionInfo.KingLatin, ProfessionInfo.KingHeirLatin, BuildingSize.Small, FamilyOwnership.Full)), 
                new SettlementInfo(SettlementSize.Town, "Town", new BuildingInfo("Castle", ProfessionInfo.GovernorLatin, ProfessionInfo.GovernorLatin, BuildingSize.Unique)), 
                true, 1, 6, false, CSocialOrder.MedievalLatin, new Language[] {Language.Latin}),
            new StateInfo("Imperium", 2,
                new SettlementInfo(SettlementSize.City, "City", new BuildingInfo("Castle", ProfessionInfo.EmperorLatin, ProfessionInfo.KingHeirLatin, BuildingSize.Small, FamilyOwnership.Full)), 
                new SettlementInfo(SettlementSize.Town, "Town", new BuildingInfo("Castle", ProfessionInfo.KingLatin, ProfessionInfo.KingHeirLatin, BuildingSize.Small)), 
                true, 2, 6, true, CSocialOrder.MedievalLatin, new Language[] {Language.Latin}),
            new StateInfo("Shogunate", 1,
                new SettlementInfo(SettlementSize.City, "City", new BuildingInfo("Castle", ProfessionInfo.KingAsian, ProfessionInfo.KingHeirAsian, BuildingSize.Small, FamilyOwnership.Full)), 
                new SettlementInfo(SettlementSize.Town, "Town", new BuildingInfo("Castle", ProfessionInfo.GovernorAsian, ProfessionInfo.GovernorAsian, BuildingSize.Unique)), 
                true, 1, 6, false, CSocialOrder.MedievalAsian, new Language[] {Language.Asian}),            
            new StateInfo("Empire", 2,
                new SettlementInfo(SettlementSize.City, "City", new BuildingInfo("Castle", ProfessionInfo.EmperorAsian, ProfessionInfo.EmperorHeirAsian, BuildingSize.Small, FamilyOwnership.Full)), 
                new SettlementInfo(SettlementSize.Town, "Town", new BuildingInfo("Castle", ProfessionInfo.GovernorAsian2, ProfessionInfo.GovernorAsian2, BuildingSize.Unique)), 
                true, 2, 6, true, CSocialOrder.MedievalAsian, new Language[] {Language.Asian}),
            new StateInfo("Shahdom", 1,
                new SettlementInfo(SettlementSize.City, "City", new BuildingInfo("Castle", ProfessionInfo.KingArabian, ProfessionInfo.KingHeirArabian, BuildingSize.Small, FamilyOwnership.Full)), 
                new SettlementInfo(SettlementSize.Town, "Town", new BuildingInfo("Castle", ProfessionInfo.GovernorArabian, ProfessionInfo.GovernorArabian, BuildingSize.Unique)), 
                true, 1, 6, false, CSocialOrder.MedievalArabian, new Language[] {Language.Arabian}),
            new StateInfo("Sultanate", 1,
                new SettlementInfo(SettlementSize.City, "City", new BuildingInfo("Castle", ProfessionInfo.KingArabian2, ProfessionInfo.KingHeirArabian2, BuildingSize.Small, FamilyOwnership.Full)), 
                new SettlementInfo(SettlementSize.Town, "Town", new BuildingInfo("Castle", ProfessionInfo.GovernorArabian, ProfessionInfo.GovernorArabian, BuildingSize.Unique)), 
                true, 1, 6, false, CSocialOrder.MedievalArabian, new Language[] {Language.Arabian, Language.African}),            
            new StateInfo("Caliphate", 2,
                new SettlementInfo(SettlementSize.City, "City", new BuildingInfo("Castle", ProfessionInfo.EmperorArabian, ProfessionInfo.KingHeirEuro, BuildingSize.Small, FamilyOwnership.Full)), 
                new SettlementInfo(SettlementSize.Town, "Town", new BuildingInfo("Castle", ProfessionInfo.GovernorArabian, ProfessionInfo.GovernorArabian, BuildingSize.Unique)), 
                true, 2, 6, true, CSocialOrder.MedievalArabian, new Language[] {Language.Arabian, Language.African}),
            new StateInfo("Khanate", 1,
                new SettlementInfo(SettlementSize.City, "City", new BuildingInfo("Castle", ProfessionInfo.KingMongol, ProfessionInfo.KingHeirEuro, BuildingSize.Small, FamilyOwnership.Full)), 
                new SettlementInfo(SettlementSize.Town, "Town", new BuildingInfo("Castle", ProfessionInfo.GovernorMongol, ProfessionInfo.GovernorMongol, BuildingSize.Unique)), 
                true, 1, 6, false, CSocialOrder.MedievalMongol, new Language[] {Language.Orkish, Language.Eskimoid}),
            new StateInfo("Khaganate", 2,
                new SettlementInfo(SettlementSize.City, "City", new BuildingInfo("Castle", ProfessionInfo.EmperorMongol, ProfessionInfo.KingHeirEuro, BuildingSize.Small, FamilyOwnership.Full)), 
                new SettlementInfo(SettlementSize.Town, "Town", new BuildingInfo("Castle", ProfessionInfo.KingMongol, ProfessionInfo.KingHeirEuro, BuildingSize.Small)), 
                true, 2, 6, true, CSocialOrder.MedievalMongol, new Language[] {Language.Orkish, Language.Eskimoid}),
            new StateInfo("Knyazdom", 1,
                new SettlementInfo(SettlementSize.City, "City", new BuildingInfo("Castle", ProfessionInfo.KingSlavic, ProfessionInfo.KingHeirSlavic, BuildingSize.Small, FamilyOwnership.Full)), 
                new SettlementInfo(SettlementSize.Town, "Town", new BuildingInfo("Castle", ProfessionInfo.GovernorSlavic, ProfessionInfo.GovernorSlavic, BuildingSize.Unique)), 
                true, 1, 6, false, CSocialOrder.MedievalSlavic, new Language[] {Language.Slavic}),
            new StateInfo("Tsardom", 2,
                new SettlementInfo(SettlementSize.City, "City", new BuildingInfo("Castle", ProfessionInfo.EmperorSlavic, ProfessionInfo.EmperorHeirSlavic, BuildingSize.Small, FamilyOwnership.Full)), 
                new SettlementInfo(SettlementSize.Town, "Town", new BuildingInfo("Castle", ProfessionInfo.KingSlavic, ProfessionInfo.KingHeirSlavic, BuildingSize.Small)), 
                true, 2, 6, true, CSocialOrder.MedievalSlavic, new Language[] {Language.Slavic}),
            new StateInfo("Basileia", 1,
                new SettlementInfo(SettlementSize.City, "City", new BuildingInfo("Castle", ProfessionInfo.KingGreek, ProfessionInfo.KingHeirGreek, BuildingSize.Small, FamilyOwnership.Full)), 
                new SettlementInfo(SettlementSize.Town, "Town", new BuildingInfo("Castle", ProfessionInfo.GovernorGreek, ProfessionInfo.GovernorGreek, BuildingSize.Unique)), 
                true, 1, 6, false, CSocialOrder.MedievalGreek, new Language[] {Language.Greek}),
            new StateInfo("Autokratoria", 2,
                new SettlementInfo(SettlementSize.City, "City", new BuildingInfo("Castle", ProfessionInfo.EmperorGreek, ProfessionInfo.KingHeirGreek, BuildingSize.Small, FamilyOwnership.Full)), 
                new SettlementInfo(SettlementSize.Town, "Town", new BuildingInfo("Castle", ProfessionInfo.GovernorGreek2, ProfessionInfo.GovernorGreek2, BuildingSize.Unique)), 
                true, 2, 6, true, CSocialOrder.MedievalGreek, new Language[] {Language.Greek}),
            new StateInfo("Raj", 1,
                new SettlementInfo(SettlementSize.City, "City", new BuildingInfo("Castle", ProfessionInfo.KingHindu, ProfessionInfo.KingHeirHindu, BuildingSize.Small, FamilyOwnership.Full)), 
                new SettlementInfo(SettlementSize.Town, "Town", new BuildingInfo("Castle", ProfessionInfo.GovernorHindu, ProfessionInfo.GovernorHindu, BuildingSize.Unique)), 
                true, 1, 6, false, CSocialOrder.MedievalHindu, new Language[] {Language.Hindu}),            
            new StateInfo("Empire", 2,
                new SettlementInfo(SettlementSize.City, "City", new BuildingInfo("Castle", ProfessionInfo.EmperorHindu, ProfessionInfo.EmperorHeirHindu, BuildingSize.Small, FamilyOwnership.Full)), 
                new SettlementInfo(SettlementSize.Town, "Town", new BuildingInfo("Castle", ProfessionInfo.KingHindu, ProfessionInfo.KingHeirHindu, BuildingSize.Small)), 
                true, 2, 6, true, CSocialOrder.MedievalHindu, new Language[] {Language.Hindu}),
            new StateInfo("Altepetl", 1,
                new SettlementInfo(SettlementSize.City, "City", new BuildingInfo("Castle", ProfessionInfo.KingAztec, ProfessionInfo.KingHeirAztec, BuildingSize.Small, FamilyOwnership.Full)), 
                new SettlementInfo(SettlementSize.Town, "Town", new BuildingInfo("Castle", ProfessionInfo.GovernorAztec, ProfessionInfo.GovernorAztec, BuildingSize.Unique)), 
                true, 1, 6, false, CSocialOrder.MedievalAztec, new Language[] {Language.Aztec}),           
            new StateInfo("Empire", 2,
                new SettlementInfo(SettlementSize.City, "City", new BuildingInfo("Castle", ProfessionInfo.EmperorEuro, ProfessionInfo.KingHeirAztec, BuildingSize.Small, FamilyOwnership.Full)), 
                new SettlementInfo(SettlementSize.Town, "Town", new BuildingInfo("Castle", ProfessionInfo.GovernorAztec, ProfessionInfo.GovernorAztec, BuildingSize.Unique)), 
                true, 2, 6, true, CSocialOrder.MedievalAztec, new Language[] {Language.Aztec}),
            new StateInfo("Republic", 2,
                new SettlementInfo(SettlementSize.City, "City", new BuildingInfo("Statehouse", ProfessionInfo.RulerModern, ProfessionInfo.AdvisorModern, BuildingSize.Small)), 
                new SettlementInfo(SettlementSize.City, "City", new BuildingInfo("Town hall", ProfessionInfo.GovernorModern, ProfessionInfo.GovernorModern, BuildingSize.Unique)), 
                false, 3, 7, false, CSocialOrder.Modern, null),
            new StateInfo("Federation", 2,
                new SettlementInfo(SettlementSize.Capital, "City", new BuildingInfo("Statehouse", ProfessionInfo.RulerModern2, ProfessionInfo.AdvisorModern2, BuildingSize.Small)), 
                new SettlementInfo(SettlementSize.City, "City", new BuildingInfo("Town hall", ProfessionInfo.GovernorModern2, ProfessionInfo.GovernorModern2, BuildingSize.Unique)), 
                false, 4, 7, false, CSocialOrder.Modern, null),
            new StateInfo("League", 3,
                new SettlementInfo(SettlementSize.Capital, "City", new BuildingInfo("Palace", ProfessionInfo.RulerModern3, ProfessionInfo.AdvisorModern3, BuildingSize.Small)), 
                new SettlementInfo(SettlementSize.City, "City", new BuildingInfo("Palace", ProfessionInfo.GovernorModern, ProfessionInfo.GovernorModern, BuildingSize.Unique)), 
                false, 4, 7, true, CSocialOrder.Modern, null),
            new StateInfo("Union", 3,
                new SettlementInfo(SettlementSize.Capital, "City", new BuildingInfo("Statehouse", ProfessionInfo.RulerModern2, ProfessionInfo.AdvisorModern2, BuildingSize.Small)), 
                new SettlementInfo(SettlementSize.City, "City", new BuildingInfo("Town hall", ProfessionInfo.GovernorModern2, ProfessionInfo.GovernorModern2, BuildingSize.Unique)), 
                false, 5, 7, true, CSocialOrder.Modern, null),
            new StateInfo("Alliance", 3,
                new SettlementInfo(SettlementSize.Capital, "City", new BuildingInfo("Palace", ProfessionInfo.RulerModern4, ProfessionInfo.AdvisorModern, BuildingSize.Small)), 
                new SettlementInfo(SettlementSize.City, "City", new BuildingInfo("Town hall", ProfessionInfo.GovernorModern2, ProfessionInfo.GovernorModern2, BuildingSize.Unique)), 
                false, 5, 7, true, CSocialOrder.Modern, null),
            new StateInfo("Coalition", 3,
                new SettlementInfo(SettlementSize.Capital, "City", new BuildingInfo("Statehouse", ProfessionInfo.RulerModern3, ProfessionInfo.AdvisorModern, BuildingSize.Small)), 
                new SettlementInfo(SettlementSize.City, "City", new BuildingInfo("Town hall", ProfessionInfo.GovernorModern2, ProfessionInfo.GovernorModern2, BuildingSize.Unique)), 
                false, 5, 7, true, CSocialOrder.Modern, null),
            new StateInfo("Association", 3,
                new SettlementInfo(SettlementSize.Capital, "City", new BuildingInfo("Statehouse", ProfessionInfo.RulerModern2, ProfessionInfo.AdvisorModern4, BuildingSize.Small)), 
                new SettlementInfo(SettlementSize.City, "City", new BuildingInfo("Town hall", ProfessionInfo.GovernorModern, ProfessionInfo.GovernorModern, BuildingSize.Unique)), 
                false, 5, 7, true, CSocialOrder.Modern, null),
            new StateInfo("Commonwealth", 1,
                new SettlementInfo(SettlementSize.Town, "Town", new BuildingInfo("Town hall", ProfessionInfo.RulerModern3, ProfessionInfo.AdvisorModern5, BuildingSize.Small)), 
                new SettlementInfo(SettlementSize.Town, "Town", new BuildingInfo("Town hall", ProfessionInfo.GovernorModern3, ProfessionInfo.GovernorModern3, BuildingSize.Unique)), 
                false, 7, 8, false, CSocialOrder.Future, null),
            new StateInfo("Society", 1,
                new SettlementInfo(SettlementSize.Village, "Village", null), 
                new SettlementInfo(SettlementSize.Village, "Village", null), 
                false, 7, 8, false, CSocialOrder.Future, null),
            new StateInfo("Collective", 2,
                new SettlementInfo(SettlementSize.Village, "Village", null), 
                new SettlementInfo(SettlementSize.Village, "Village", null), 
                false, 7, 8, true, CSocialOrder.Future, null),
        };
        #endregion

        public CNation m_pNation = null;

        /// <summary>
        /// Доступный жителям уровень жизни.
        /// Зависит от технического и магического развития, определяет доступные формы государственного правления
        /// </summary>
        public int m_iInfrastructureLevel = 0;

        public float m_fGrain = 0;
        public float m_fGame = 0;
        public float m_fOre = 0;
        public float m_fWood = 0;
        public float m_fFood = 0;
        public int m_iPopulation = 0;

        public StateInfo m_pInfo = null;

        public CState(CLocation pSeed, CNation pNation)
        {
            m_pNation = pNation;
            pSeed.Owner = this;
            m_cLands.Add(pSeed);
        }

        /// <summary>
        /// Low TL states could have access to higher TL weapons from other countries.
        /// Returns higher TL if possible or -1 is there is no available higher TL.
        /// Currently not using, so always -1.
        /// </summary>
        /// <returns></returns>
        public override int GetImportedTech()
        {
            return -1;

            //if (m_pNation.m_bDying)
            //    return -1;

            //int iMaxTech = GetEffectiveTech();
            //foreach (CState pState in m_aBorderWith)
            //{
            //    if (pState.Forbidden)
            //        continue;

            //    if (pState.GetEffectiveTech() > iMaxTech)
            //        iMaxTech = pState.GetEffectiveTech();
            //}

            //if (iMaxTech <= GetEffectiveTech())
            //    iMaxTech = -1;

            //return iMaxTech;
        }

        /// <summary>
        /// Return most common imported weapon (see GetImportedTech() for details)
        /// Currently not using, so always empty string.
        /// </summary>
        /// <returns></returns>
        public override string GetImportedTechString()
        {
            return "";

            //if (m_pNation.m_bDying)
            //    return "";

            //int iMaxTech = GetEffectiveTech();
            //CState pExporter = null;
            //foreach (CState pState in m_aBorderWith)
            //{
            //    if (pState.Forbidden)
            //        continue;

            //    if (pState.GetEffectiveTech() > iMaxTech)
            //    {
            //        iMaxTech = pState.GetEffectiveTech();
            //        pExporter = pState;
            //    }
            //}

            //if (pExporter == null)
            //    return "";

            //return CState.GetTechString(pExporter.m_iTechLevel, pExporter.m_pCustoms.m_eProgress);
        }

        public override string ToString()
        {
            return string.Format("{2} (C{1}T{3}M{5}) - {0} {4}", m_sName, m_iCultureLevel, m_pNation, m_iTechLevel, m_pInfo.m_sName, m_iMagicLimit);
        }

        /// <summary>
        /// Cоздаёт государство - вычисляет уровень технического развития, культуру, обычаи, обеспеченность ресурсами, политический и экономический строй, определяет отношение в обществе к различным группам навыков, выбирает самоназвание, строит города, формирует сословия...
        /// </summary>
        /// <param name="iEmpireTreshold">Порог численности населения, при превышении которого государство считается империей</param>
        /// <returns></returns>
        public void BuildUp(int iEmpireTreshold)
        {
            m_iTechLevel = m_pNation.m_iTechLevel;
            //m_iMagicLimit = m_pRace.m_iMagicLimit;

            //m_eMagicAbilityPrevalence = m_pRace.m_eMagicAbilityPrevalence;
            //m_eMagicAbilityDistribution = m_pRace.m_eMagicAbilityDistribution;
            m_pCulture = new Culture(m_pNation.m_pCulture);
            m_pCustoms = new Customs(m_pNation.m_pCustoms, Customs.Mutation.Mandatory);

            //if (Rnd.OneChanceFrom(3))
            //    m_eMagicAbilityPrevalence = (MagicAbilityPrevalence)Rnd.Get(typeof(MagicAbilityPrevalence));
            //if (Rnd.OneChanceFrom(3))
            //    m_eMagicAbilityDistribution = (MagicAbilityDistribution)Rnd.Get(typeof(MagicAbilityDistribution));

            #region Counting resources production and consumption
            m_fGrain = 0;
            m_fGame = 0;
            m_fWood = 0;
            m_fOre = 0;
            m_iPopulation = 0;

            foreach (CLocation pLand in m_cLands)
            {
                int iPop = 100 / pLand.GetClaimingCost(m_pNation);

                m_fGrain += pLand.GetResource(Territory.Resource.Grain) * iPop;
                m_fGame += pLand.GetResource(Territory.Resource.Game) * iPop;
                m_fWood += pLand.GetResource(Territory.Resource.Wood) * iPop;
                m_fOre += pLand.GetResource(Territory.Resource.Ore) * iPop;

                m_iPopulation += iPop;
            }
            #endregion Counting resources production and consumption

            #region Adjustiong TL due to lack or abundance of resouces
            if (m_fWood * 2 < Rnd.Get(m_iPopulation) && m_fOre * 2 < Rnd.Get(m_iPopulation))// && Rnd.OneChanceFrom(2))
                m_iTechLevel -= 2;
            else if (m_fWood + m_fOre < Rnd.Get(m_iPopulation))// && Rnd.OneChanceFrom(2))
                m_iTechLevel--;
            else if ((m_fWood > Rnd.Get(m_iPopulation) * 2 && m_fOre > Rnd.Get(m_iPopulation) * 2))// || Rnd.OneChanceFrom(4))
                m_iTechLevel++;

            if (m_pNation.m_bInvader)
            {
                if (m_iTechLevel < m_pNation.m_pEpoch.m_iInvadersMinTechLevel)
                    m_iTechLevel = m_pNation.m_pEpoch.m_iInvadersMinTechLevel;
                if (m_iTechLevel > m_pNation.m_pEpoch.m_iInvadersMaxTechLevel)
                    m_iTechLevel = m_pNation.m_pEpoch.m_iInvadersMaxTechLevel;
            }
            else
            {
                if (m_iTechLevel < m_pNation.m_pEpoch.m_iNativesMinTechLevel)
                    m_iTechLevel = m_pNation.m_pEpoch.m_iNativesMinTechLevel;
                if (m_iTechLevel > m_pNation.m_pEpoch.m_iNativesMaxTechLevel)
                    m_iTechLevel = m_pNation.m_pEpoch.m_iNativesMaxTechLevel;
            }
            #endregion Adjustiong TL due to lack or abundance of resouces

            #region Set available infrastructure level according TL and food availability
            //m_iInfrastructureLevel = 4 - (int)(m_pCulture.GetDifference(Culture.IdealSociety, m_iTechLevel, m_iTechLevel) * 4);
            m_iInfrastructureLevel = m_iTechLevel;// -(int)(m_iTechLevel * Math.Pow(Rnd.Get(1f), 3));

            if (m_cLands.Count == 1)
                m_iInfrastructureLevel /= 2;

            if (m_iTechLevel == 0 && m_pNation.m_iMagicLimit == 0)
                m_iInfrastructureLevel = 0;

            switch (m_pNation.m_pFenotype.m_pBody.m_eNutritionType)
            {
                case NutritionType.Eternal:
                    m_fFood = m_iPopulation*10;
                    break;
                case NutritionType.Mineral:
                    m_fFood = m_fOre;
                    break;
                case NutritionType.Organic:
                    m_fFood = m_fGrain + m_fGame;
                    break;
                case NutritionType.ParasitismBlood:
                    m_fFood = m_iPopulation;
                    break;
                case NutritionType.ParasitismEmote:
                    m_fFood = m_iPopulation;
                    break;
                case NutritionType.ParasitismEnergy:
                    m_fFood = m_iPopulation;
                    break;
                case NutritionType.ParasitismMeat:
                    m_fFood = m_iPopulation;
                    break;
                case NutritionType.Photosynthesis:
                    m_fFood = m_iPopulation*10;
                    break;
                case NutritionType.Thermosynthesis:
                    m_fFood = m_iPopulation*10;
                    break;
                case NutritionType.Vegetarian:
                    m_fFood = m_fGrain;
                    break;
                case NutritionType.Carnivorous:
                    m_fFood = m_fGame;
                    break;
                default:
                    throw new Exception(string.Format("Unknown Nutrition type: {0}", m_pNation.m_pFenotype.m_pBody.m_eNutritionType));
                    break;
            }


            if (m_fFood * 2 < m_iPopulation)
                m_iInfrastructureLevel--;// = Rnd.Get(m_iCultureLevel);            
            if (m_fFood < m_iPopulation || Rnd.OneChanceFrom(10))
                m_iInfrastructureLevel--;// = Rnd.Get(m_iCultureLevel);
            if (m_fFood > m_iPopulation * 2 && Rnd.OneChanceFrom(10))
                m_iInfrastructureLevel++;

            if (m_iInfrastructureLevel < 0)//Math.Max(m_iTechLevel + 1, iAverageMagicLimit) / 2)
                m_iInfrastructureLevel = 0;//Math.Max(m_iTechLevel + 1, iAverageMagicLimit) / 2;
            if (m_iInfrastructureLevel > m_iTechLevel + 1)//Math.Max(m_iTechLevel + 1, iAverageMagicLimit - 1))
                m_iInfrastructureLevel = m_iTechLevel + 1;// Math.Max(m_iTechLevel + 1, iAverageMagicLimit - 1);
            if (m_iInfrastructureLevel > 8)
                m_iInfrastructureLevel = 8;
            #endregion Set available infrastructure level according TL and food availability

            #region Adjusting TL due to infrastructure level
            if (m_iTechLevel > m_iInfrastructureLevel * 2)
                m_iTechLevel = m_iInfrastructureLevel * 2;
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
            #endregion Adjusting TL due to infrastructure level

            #region Choose state system
            List<StateInfo> cInfos = new List<StateInfo>();

            foreach (StateInfo pInfo in s_aInfo)
            {
                if (m_iInfrastructureLevel >= pInfo.m_iMinGovernmentLevel &&
                    m_iInfrastructureLevel <= pInfo.m_iMaxGovernmentLevel &&
                    (pInfo.m_cLanguages.Count == 0 ||
                     pInfo.m_cLanguages.Contains(m_pNation.m_pRace.m_pLanguage)) &&
                    (m_iPopulation > iEmpireTreshold * 80) == pInfo.m_bBig)
                {
                    for (int i = 0; i < pInfo.m_iRank; i++)
                        cInfos.Add(pInfo);
                }
            }

            if (cInfos.Count == 0)
            {
                foreach (StateInfo pInfo in s_aInfo)
                {
                    if (m_iInfrastructureLevel >= pInfo.m_iMinGovernmentLevel &&
                        m_iInfrastructureLevel <= pInfo.m_iMaxGovernmentLevel &&
                        (m_iPopulation > iEmpireTreshold * 80) == pInfo.m_bBig)
                    {
                        for (int i = 0; i < pInfo.m_iRank; i++)
                            cInfos.Add(pInfo);
                    }
                }
            }

            m_pInfo = cInfos[Rnd.Get(cInfos.Count)];
            #endregion Choose state system

            m_iCultureLevel = (m_iInfrastructureLevel + m_pInfo.m_iMinGovernmentLevel) / 2;

            #region Set social equality level
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

            if (m_pInfo.m_bDinasty)
                m_iSocialEquality--;

            if (m_iSocialEquality < 0)
                m_iSocialEquality = 0;

            if (m_pCulture.MentalityValues[Mentality.Agression][m_iCultureLevel] < 1)
                m_iSocialEquality++;

            if (m_pCulture.MentalityValues[Mentality.Fanaticism][m_iCultureLevel] < 1)
                m_iSocialEquality++;

            if (m_iSocialEquality > 0 && m_fFood < m_iPopulation)
                m_iSocialEquality--;
            if (m_fFood > m_iPopulation && m_fOre > m_iPopulation && m_fWood > m_iPopulation)
                m_iSocialEquality++;

            //в либеральном обществе (фанатизм < 2/3) не может быть рабства (0) или крепостного права (1), т.е. только 2 и выше
            if (m_pCulture.MentalityValues[Mentality.Fanaticism][m_iCultureLevel] < 0.66)
                m_iSocialEquality = Math.Max(2, m_iSocialEquality);
            //в обществе абсолютных пацифистов (агрессивность < 1/3) не может быть даже капитализма (2), т.е. только 3 и выше
            if (m_pCulture.MentalityValues[Mentality.Agression][m_iCultureLevel] < 0.33)
                m_iSocialEquality = Math.Max(3, m_iSocialEquality);
            //в обществе абсолютного самоотречения (эгоизм < 1/3) не может быть капитализма (2) - только или социализм, или феодализм
            if (m_pCulture.MentalityValues[Mentality.Selfishness][m_iCultureLevel] < 0.33)
                if (m_pInfo.m_bDinasty)
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
            if (m_fFood < m_iPopulation * 2 || m_fOre < m_iPopulation * 2 || m_fWood < m_iPopulation * 2)
                m_iSocialEquality = Math.Min(3, m_iSocialEquality);

            //при всём уважении - какой нафиг социализм/коммунизм при наследственной власти???
            if (m_pInfo.m_bDinasty)
                m_iSocialEquality = Math.Min(2, m_iSocialEquality);

            if (m_iSocialEquality > 4)
                m_iSocialEquality = 4;

            #endregion Set social equality level

            #region Set state control level
            m_iControl = 2;

            //if (m_pCulture.Moral[Culture.Morale.Agression] > 1)
            //    m_iControl++;

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
            #endregion Set state control level

            CPerson.GetSkillPreferences(m_pCulture, m_iCultureLevel, m_pCustoms, ref m_eMostRespectedSkill, ref m_eLeastRespectedSkill);

            #region Build capital
            SettlementInfo pCapitalInfo = m_pInfo.m_pStateCapital;

            Dictionary<int, float> cChances = new Dictionary<int, float>();
            for (int iLoc = 0; iLoc < m_cLands.Count; iLoc++)
            {
                int iCost = m_cLands[iLoc].GetClaimingCost(m_pNation);
                cChances[iLoc] = (float)1000.0 / iCost;
            }

            int iChoice = Rnd.ChooseOne(cChances.Values);

            CSettlement pCapital = new CSettlement(pCapitalInfo, m_pNation, m_iTechLevel, m_iMagicLimit, true, false); 
            CLocation pCapitalLoc = m_cLands[cChances.ElementAt(iChoice).Key];

            pCapitalLoc.Settlement = pCapital;
            #endregion Build capital

            m_sName = m_pNation.m_pRace.m_pLanguage.RandomCountryName();

            pCapitalLoc.ClaimSettlement();
            m_cSettlements.Add(pCapitalLoc);

            int tryings = 0;
            do
            {
                int iLoc = Rnd.Get(m_cLands.Count);
                if (m_cLands[iLoc].Type == LocationType.Undefined)
                {
                    //if (m_cSocieties[i].Lands[iLoc].SubType != LocationSubType.Undefined)
                    //    throw new Exception();
                    m_cLands[iLoc].Settlement = new CSettlement(m_pInfo.m_pProvinceCapital, m_pNation, m_iTechLevel, m_iMagicLimit, false, false);
                    //m_cStates[i].Lands[iLoc].Settlement.AddBuildings(m_cStates[i]);

                    m_cLands[iLoc].ClaimSettlement();
                    m_cSettlements.Add(m_cLands[iLoc]);
                    tryings = 0;
                }
            }
            while (tryings++ < 10);

            SpecializeSettlements();

            foreach (CLocation pLocation in m_cLands)
            {
                if (pLocation.Settlement == null)
                    continue;

                AddBuildings(pLocation.Settlement);
            }

            SetEstates();

            return;// pCapitalLoc;
        }

        protected override BuildingInfo ChooseNewBuilding(CSettlement pSettlement)
        { 
            int iInfrastructureLevel = m_iInfrastructureLevel;
            int iControl = m_iControl *2;

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
                                    cChances[m_iSocialEquality == 0 ? BuildingInfo.InnSlvSmall : BuildingInfo.InnSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 4;
                                    cChances[m_iSocialEquality == 0 ? BuildingInfo.TavernSlvSmall : BuildingInfo.TavernSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel];
                                }
                                else
                                {
                                    cChances[m_iSocialEquality == 0 ? BuildingInfo.HotelSlvSmall : BuildingInfo.HotelSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 4;
                                    cChances[m_iSocialEquality == 0 ? BuildingInfo.BarSlvSmall : BuildingInfo.BarSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel];
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

                                if (m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Sapient)
                                    fScience *= 2;
                                if (m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Ingenious)
                                    fScience *= 4;

                                cChances[BuildingInfo.SchoolSmall] = fScience / 4;
                            }

                            //if (pState.m_iSocialEquality == 0)
                            //{
                            //    cChances[BuildingInfo.SlavePensMedium] = (float)cChances.Count;// / 2 + 1;
                            //}

                            cChances[BuildingInfo.MarketSmall] = (float)cChances.Count / 2;// / 2 + 1;

                            if (m_pInfo.m_bDinasty)
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
                                    cChances[pBrothelProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 4;
                                    if(iInfrastructureLevel >= 4)
                                        cChances[pStripClubProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                                    break;
                                case Psichology.Customs.Sexuality.Lecherous:
                                    cChances[pBrothelProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                                    if(iInfrastructureLevel >= 4)
                                        cChances[pStripClubProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                                    break;
                            }

                            if (iInfrastructureLevel < 4)
                            {
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.InnSlvSmall : BuildingInfo.InnSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 4;
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.TavernSlvSmall : BuildingInfo.TavernSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel];
                            }
                            else
                            {
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.HotelSlvMedium : BuildingInfo.HotelMedium] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 4;
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.BarSlvSmall : BuildingInfo.BarSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel];
                            }

                            cChances[BuildingInfo.TheatreMedium] = 1f - m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;

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

                            if (m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Sapient)
                                fBureaucracy *= 2;
                            if (m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Primitive)
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

                                if (m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Sapient)
                                    fScience *= 2;
                                if (m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Ingenious)
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

                            if (m_pInfo.m_bDinasty)
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

                            if(iInfrastructureLevel >= 4 && iInfrastructureLevel < 8)
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
                                    cChances[pBrothelProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 4;
                                    if (iInfrastructureLevel >= 4)
                                        cChances[pStripClubProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                                    break;
                                case Psichology.Customs.Sexuality.Lecherous:
                                    cChances[pBrothelProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                                    if (iInfrastructureLevel >= 4)
                                        cChances[pStripClubProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                                    break;
                            }

                            if (iInfrastructureLevel < 4)
                            {
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.InnSlvSmall : BuildingInfo.InnSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 4;
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.TavernSlvSmall : BuildingInfo.TavernSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel];
                                cChances[BuildingInfo.CircusMedium] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] *
                                                                      m_pCulture.MentalityValues[Psichology.Mentality.Agression][iInfrastructureLevel] / 2;
                            }
                            else
                            {
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.HotelSlvMedium : BuildingInfo.HotelMedium] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 4;
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.BarSlvSmall : BuildingInfo.BarSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel];
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.NightClubSlvMedium : BuildingInfo.NightClubMedium] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel];
                            }

                            cChances[BuildingInfo.TheatreMedium] = 1f - m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;

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

                            if (m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Sapient)
                                fBureaucracy *= 2;
                            if (m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Primitive)
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

                                if (m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Sapient)
                                    fScience *= 2;
                                if (m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Ingenious)
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

                            if (m_pInfo.m_bDinasty)
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
                                pProfile = iInfrastructureLevel < 4  ? (m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] > Rnd.Get(2f) ? BuildingInfo.CircusMedium : BuildingInfo.TheatreMedium) : BuildingInfo.CinemaLarge;
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
                                    cChances[pBrothelProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 4;
                                    if (iInfrastructureLevel >= 4)
                                        cChances[pStripClubProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                                    break;
                                case Psichology.Customs.Sexuality.Lecherous:
                                    cChances[pBrothelProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                                    if (iInfrastructureLevel >= 4)
                                        cChances[pStripClubProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                                    break;
                            }

                            if (iInfrastructureLevel < 4)
                            {
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.InnSlvSmall : BuildingInfo.InnSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 4;
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.TavernSlvSmall : BuildingInfo.TavernSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel];
                                cChances[BuildingInfo.CircusMedium] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] *
                                                                      m_pCulture.MentalityValues[Psichology.Mentality.Agression][iInfrastructureLevel] / 2;
                            }
                            else
                            {
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.HotelSlvMedium : BuildingInfo.HotelMedium] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 4;
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.BarSlvSmall : BuildingInfo.BarSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel];
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.NightClubSlvMedium : BuildingInfo.NightClubMedium] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel];
                            }

                            cChances[BuildingInfo.TheatreMedium] = 1f - m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;

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

                            if (m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Sapient)
                                fBureaucracy *= 2;
                            if (m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Primitive)
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

                                if (m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Sapient)
                                    fScience *= 2;
                                if (m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Ingenious)
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

                            if (m_pInfo.m_bDinasty)
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
                                pProfile = iInfrastructureLevel < 4  ? (m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] > Rnd.Get(2f) ? BuildingInfo.CircusMedium : BuildingInfo.TheatreMedium) : BuildingInfo.CinemaLarge;
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
                                    cChances[pBrothelProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 4;
                                    if (iInfrastructureLevel >= 4)
                                        cChances[pStripClubProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                                    break;
                                case Psichology.Customs.Sexuality.Lecherous:
                                    cChances[pBrothelProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                                    if (iInfrastructureLevel >= 4)
                                        cChances[pStripClubProfile] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                                    break;
                            }

                            if (iInfrastructureLevel < 4)
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.TavernSlvSmall : BuildingInfo.TavernSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                            else
                                cChances[m_iSocialEquality == 0 ? BuildingInfo.BarSlvSmall : BuildingInfo.BarSmall] = m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;

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

        public override void AddBuildings(CSettlement pSettlement)
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

        internal override Customs.GenderPriority FixGenderPriority(Customs.GenderPriority ePriority)
        {
            switch (ePriority)
            {
                case Customs.GenderPriority.Matriarchy:
                    if (m_pNation.m_pFenotype.m_pLifeCycle.m_eGendersDistribution == GendersDistribution.OnlyFemales ||
                        m_pNation.m_pFenotype.m_pLifeCycle.m_eGendersDistribution == GendersDistribution.MostlyFemales)
                        return Customs.GenderPriority.Genders_equality;
                    else
                        return Customs.GenderPriority.Patriarchy;
                case Customs.GenderPriority.Patriarchy:
                    if (m_pNation.m_pFenotype.m_pLifeCycle.m_eGendersDistribution == GendersDistribution.OnlyMales ||
                        m_pNation.m_pFenotype.m_pLifeCycle.m_eGendersDistribution == GendersDistribution.MostlyMales)
                        return Customs.GenderPriority.Genders_equality;
                    else
                        return Customs.GenderPriority.Matriarchy;
            }

            return base.FixGenderPriority(ePriority);
        }

        /// <summary>
        /// Задаёт специализацию поселения в зависимости от его расположения, размера, доступного уровня инфраструктуры и культурных ценностей населения
        /// </summary>
        public void SpecializeSettlements()
        {
            foreach (CLocation pLocation in m_cLands)
            {
                if (pLocation.Settlement == null)
                    continue;

                if (pLocation.Settlement.m_eSpeciality != SettlementSpeciality.None)
                    continue;

                bool bCoast = false;
                foreach (var pLink in pLocation.m_cEdges)
                {
                    if (pLink.Key.Territory.Type == Territory.TerritoryType.DeadSea)
                        bCoast = true;
                }

                switch (pLocation.Settlement.m_pInfo.m_eSize)
                {
                    #region case SettlementSize.Hamlet
                    case SettlementSize.Hamlet:
                        if (bCoast)
                        {
                            pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Fishers;
                        }
                        else
                        {
                            List<float> cResources = new List<float>();
                            cResources.Add(pLocation.GetResource(Territory.Resource.Grain));
                            cResources.Add(pLocation.GetResource(Territory.Resource.Game));

                            //в развитом обществе охота - это уже не способ добычи пищи, а больше развлечение
                            if (m_iInfrastructureLevel > 2)
                            {
                                cResources[0] += cResources[1];
                                cResources[1] = 0;
                            }

                            if (m_iInfrastructureLevel > 1)
                            {
                                cResources.Add(pLocation.GetResource(Territory.Resource.Ore));
                                cResources.Add(pLocation.GetResource(Territory.Resource.Wood));
                            }

                            int iChoosen = Rnd.ChooseOne(cResources, 2);

                            switch (iChoosen)
                            {
                                case 0:
                                    pLocation.Settlement.m_eSpeciality = m_iInfrastructureLevel >= 4 ? SettlementSpeciality.Farmers : SettlementSpeciality.Peasants;
                                    break;
                                case 1:
                                    pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Hunters;
                                    break;
                                case 2:
                                    pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Miners;
                                    break;
                                case 3:
                                    pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Lumberjacks;
                                    break;
                            }
                        }
                        break;
                    #endregion
                    #region case SettlementSize.Village
                    case SettlementSize.Village:
                        if (bCoast)
                        {
                            pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Fishers;
                        }
                        else
                        {
                            List<float> cResources = new List<float>();
                            cResources.Add(pLocation.GetResource(Territory.Resource.Grain));
                            cResources.Add(pLocation.GetResource(Territory.Resource.Game));
                            cResources.Add(pLocation.GetResource(Territory.Resource.Ore));
                            cResources.Add(pLocation.GetResource(Territory.Resource.Wood));

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
                                    pLocation.Settlement.m_eSpeciality = m_iInfrastructureLevel >= 4 ? SettlementSpeciality.Farmers : SettlementSpeciality.Peasants;
                                    break;
                                case 1:
                                    pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Hunters;
                                    break;
                                case 2:
                                    pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Miners;
                                    break;
                                case 3:
                                    pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Lumberjacks;
                                    break;
                            }
                        }
                        break;
                    #endregion
                    #region case SettlementSize.Town
                    case SettlementSize.Town:
                        if (bCoast && !Rnd.OneChanceFrom(3))
                        {
                            if (Rnd.OneChanceFrom(2))
                                pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Naval;
                            else
                                pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Fishers;
                        }
                        else
                        {
                            if (m_pCulture.MentalityValues[Mentality.Fanaticism][m_iInfrastructureLevel] < 1 + Rnd.Get(1f))
                            {
                                List<float> cResources = new List<float>();
                                cResources.Add(m_fFood);
                                cResources.Add(m_fOre);
                                cResources.Add(m_fWood);

                                int iChoosen = Rnd.ChooseOne(cResources, 2);

                                switch (iChoosen)
                                {
                                    case 0:
                                        pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Tailors;
                                        break;
                                    case 1:
                                        pLocation.Settlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Jevellers : SettlementSpeciality.Factory;
                                        break;
                                    case 2:
                                        pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Artisans;
                                        break;
                                }
                            }
                            else
                            {
                                List<float> cResources = new List<float>();
                                cResources.Add(pLocation.GetResource(Territory.Resource.Game) + pLocation.GetResource(Territory.Resource.Grain));
                                cResources.Add(pLocation.GetResource(Territory.Resource.Ore));
                                cResources.Add(pLocation.GetResource(Territory.Resource.Wood));

                                int iChoosen = Rnd.ChooseOne(cResources, 2);

                                switch (iChoosen)
                                {
                                    case 0:
                                        pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Tailors;
                                        break;
                                    case 1:
                                        pLocation.Settlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Jevellers : SettlementSpeciality.Factory;
                                        break;
                                    case 2:
                                        pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Artisans;
                                        break;
                                }
                            }
                        }
                        break;
                    #endregion
                    #region case SettlementSize.City
                    case SettlementSize.City:
                        if (bCoast && Rnd.OneChanceFrom(2))
                            pLocation.Settlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.NavalAcademy : SettlementSpeciality.Naval;
                        else
                        {
                            if (bCoast && m_iInfrastructureLevel > 2 && m_pCulture.MentalityValues[Mentality.Rudeness][m_iInfrastructureLevel] > 1 + Rnd.Get(1f))
                                pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Resort;
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
                                            pLocation.Settlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Cultural : SettlementSpeciality.ArtsAcademy;
                                            break;
                                        case 1:
                                            pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Religious;
                                            break;
                                        case 2:
                                            pLocation.Settlement.m_eSpeciality = SettlementSpeciality.MilitaryAcademy;
                                            break;
                                        case 3:
                                            pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Gambling;
                                            break;
                                        case 4:
                                            pLocation.Settlement.m_eSpeciality = SettlementSpeciality.SciencesAcademy;
                                            break;
                                    }
                                }
                                else
                                {
                                    if (m_pCulture.MentalityValues[Mentality.Fanaticism][m_iInfrastructureLevel] < 1 + Rnd.Get(1f))
                                    {
                                        List<float> cResources = new List<float>();
                                        cResources.Add(m_fFood);
                                        cResources.Add(m_fOre);
                                        cResources.Add(m_fWood);

                                        int iChoosen = Rnd.ChooseOne(cResources, 2);

                                        switch (iChoosen)
                                        {
                                            case 0:
                                                pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Tailors;
                                                break;
                                            case 1:
                                                pLocation.Settlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Jevellers : SettlementSpeciality.Factory;
                                                break;
                                            case 2:
                                                pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Artisans;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        List<float> cResources = new List<float>();
                                        cResources.Add(pLocation.GetResource(Territory.Resource.Game) + pLocation.GetResource(Territory.Resource.Grain));
                                        cResources.Add(pLocation.GetResource(Territory.Resource.Ore));
                                        cResources.Add(pLocation.GetResource(Territory.Resource.Wood));

                                        int iChoosen = Rnd.ChooseOne(cResources, 2);

                                        switch (iChoosen)
                                        {
                                            case 0:
                                                pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Tailors;
                                                break;
                                            case 1:
                                                pLocation.Settlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Jevellers : SettlementSpeciality.Factory;
                                                break;
                                            case 2:
                                                pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Artisans;
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    #endregion
                    #region case SettlementSize.Capital
                    case SettlementSize.Capital:
                        if (bCoast && Rnd.OneChanceFrom(2))
                            pLocation.Settlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.NavalAcademy : SettlementSpeciality.Naval;
                        else
                        {
                            if (bCoast && m_iInfrastructureLevel > 2 && m_pCulture.MentalityValues[Mentality.Rudeness][m_iInfrastructureLevel] > 1 + Rnd.Get(1f))
                                pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Resort;
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
                                            pLocation.Settlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Cultural : SettlementSpeciality.ArtsAcademy;
                                            break;
                                        case 1:
                                            pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Religious;
                                            break;
                                        case 2:
                                            pLocation.Settlement.m_eSpeciality = SettlementSpeciality.MilitaryAcademy;
                                            break;
                                        case 3:
                                            pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Gambling;
                                            break;
                                        case 4:
                                            pLocation.Settlement.m_eSpeciality = SettlementSpeciality.SciencesAcademy;
                                            break;
                                    }
                                }
                                else
                                {
                                    if (m_pCulture.MentalityValues[Mentality.Fanaticism][m_iInfrastructureLevel] < 1 + Rnd.Get(1f))
                                    {
                                        List<float> cResources = new List<float>();
                                        cResources.Add(m_fFood);
                                        cResources.Add(m_fOre);
                                        cResources.Add(m_fWood);

                                        int iChoosen = Rnd.ChooseOne(cResources, 2);

                                        switch (iChoosen)
                                        {
                                            case 0:
                                                pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Tailors;
                                                break;
                                            case 1:
                                                pLocation.Settlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Jevellers : SettlementSpeciality.Factory;
                                                break;
                                            case 2:
                                                pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Artisans;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        List<float> cResources = new List<float>();
                                        cResources.Add(pLocation.GetResource(Territory.Resource.Game) + pLocation.GetResource(Territory.Resource.Grain));
                                        cResources.Add(pLocation.GetResource(Territory.Resource.Ore));
                                        cResources.Add(pLocation.GetResource(Territory.Resource.Wood));

                                        int iChoosen = Rnd.ChooseOne(cResources, 2);

                                        switch (iChoosen)
                                        {
                                            case 0:
                                                pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Tailors;
                                                break;
                                            case 1:
                                                pLocation.Settlement.m_eSpeciality = Rnd.OneChanceFrom(3) ? SettlementSpeciality.Jevellers : SettlementSpeciality.Factory;
                                                break;
                                            case 2:
                                                pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Artisans;
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    #endregion
                    #region case SettlementSize.Fort
                    case SettlementSize.Fort:
                        if (bCoast)
                            if (m_pCulture.MentalityValues[Mentality.Agression][m_iInfrastructureLevel] > 1.5 &&
                                m_pCulture.MentalityValues[Mentality.Treachery][m_iInfrastructureLevel] > 1.5)
                                pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Pirates;
                            else
                                pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Naval;
                        else
                            if (m_pCulture.MentalityValues[Mentality.Agression][m_iInfrastructureLevel] > 1.5 &&
                                m_pCulture.MentalityValues[Mentality.Treachery][m_iInfrastructureLevel] > 1.5)
                                pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Raiders;
                            else
                                pLocation.Settlement.m_eSpeciality = SettlementSpeciality.Military;
                        break;
                    #endregion
                }
            }
        }

        public override string GetEstateName(CEstate.Position ePosition)
        {
            return m_pInfo.m_pSocial.m_cEstates[ePosition][Rnd.Get(m_pInfo.m_pSocial.m_cEstates[ePosition].Length)];
        }
    }
}
