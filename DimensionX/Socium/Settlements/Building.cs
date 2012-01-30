using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using Socium.Psichology;
using GeneLab.Genetix;

namespace Socium.Settlements
{
    public enum BuildingSize
    {
        Unique,
        Small,
        Medium,
        Large,
        Huge
    }
    
    public class BuildingInfo
    {
        public static readonly BuildingInfo LoafersHut          = new BuildingInfo("Hut", "loafer", "loafer", BuildingSize.Small);
        
        public static readonly BuildingInfo SlavePensSmall      = new BuildingInfo("Slave Pens", "slave", "slave", BuildingSize.Small);
        public static readonly BuildingInfo SlavePensMedium     = new BuildingInfo("Slave Pens", "slave", "slave", BuildingSize.Medium);
        public static readonly BuildingInfo SlavePensLarge      = new BuildingInfo("Slave Pens", "slave", "slave", BuildingSize.Large);
        public static readonly BuildingInfo SlavePensHuge       = new BuildingInfo("Slave Pens", "slave", "slave", BuildingSize.Huge);
        
        public static readonly BuildingInfo SlaveMarketMedium   = new BuildingInfo("Slave Market", "slaver", "slaver", BuildingSize.Medium);
        
        public static readonly BuildingInfo WarriorsHutSmall    = new BuildingInfo("Warriors Hut", "warrior", "warrior", BuildingSize.Small);

        public static readonly BuildingInfo GuardTowerSmall     = new BuildingInfo("Guard Tower", "guard", "guard", BuildingSize.Small);
        public static readonly BuildingInfo GuardBarracksSmall  = new BuildingInfo("Guard Barracks", "guard", "guard", BuildingSize.Small);
        public static readonly BuildingInfo GuardBarracksMedium = new BuildingInfo("Guard Barracks", "guard", "guard", BuildingSize.Medium);
        public static readonly BuildingInfo PrisonSmall         = new BuildingInfo("Prison", "guard", "guard", BuildingSize.Small);
        public static readonly BuildingInfo PrisonMedium        = new BuildingInfo("Prison", "guard", "guard", BuildingSize.Medium);
        
        public static readonly BuildingInfo EmergencyPostSmall  = new BuildingInfo("Emergency Post", "watcher", "watcher", BuildingSize.Small);
        public static readonly BuildingInfo HoldingSmall        = new BuildingInfo("Holding", "watcher", "watcher", BuildingSize.Small);
        public static readonly BuildingInfo HoldingMedium       = new BuildingInfo("Holding", "watcher", "watcher", BuildingSize.Medium);
        
        public static readonly BuildingInfo PoliceStationSmall  = new BuildingInfo("Police Station", "policeman", "policeman", BuildingSize.Small);
        public static readonly BuildingInfo PoliceStationMedium = new BuildingInfo("Police Department", "policeman", "policeman", BuildingSize.Medium);
        public static readonly BuildingInfo PrisonPoliceSmall   = new BuildingInfo("Prison", "policeman", "policeman", BuildingSize.Small);
        public static readonly BuildingInfo PrisonPoliceMedium  = new BuildingInfo("Prison", "policeman", "policeman", BuildingSize.Medium);
        
        public static readonly BuildingInfo FishingBoatMedium   = new BuildingInfo("Fishing boat", "fisher", "fisher", BuildingSize.Medium);
        public static readonly BuildingInfo FishingBoatLarge    = new BuildingInfo("Fishing Trawler", "fisher", "fisher", BuildingSize.Large);
        
        public static readonly BuildingInfo FarmMedium          = new BuildingInfo("Farm", "farmer", "farmer", BuildingSize.Medium);
        public static readonly BuildingInfo FarmLarge           = new BuildingInfo("Ranch", "farmer", "farmer", BuildingSize.Large);

        public static readonly BuildingInfo PeasantsHutMedium   = new BuildingInfo("Peasants' Hut", "peasant", "peasant", BuildingSize.Medium);
        public static readonly BuildingInfo PeasantsHutLarge    = new BuildingInfo("Plantation", "peasant", "peasant", BuildingSize.Large);
        
        public static readonly BuildingInfo HuntersHutMedium    = new BuildingInfo("Hunters Hut", "hunter", "hunter", BuildingSize.Medium);
        public static readonly BuildingInfo HuntersHutLarge     = new BuildingInfo("Hunters Lodge", "hunter", "hunter", BuildingSize.Large);
        
        public static readonly BuildingInfo MineMedium          = new BuildingInfo("Quarry", "miner", "miner", BuildingSize.Medium);
        public static readonly BuildingInfo MineLarge           = new BuildingInfo("Mine", "miner", "miner", BuildingSize.Large);
        
        public static readonly BuildingInfo LumberjacksHutMedium = new BuildingInfo("Lumberjacks Hut", "lumberjack", "lumberjack", BuildingSize.Medium);
        public static readonly BuildingInfo LumberjacksHutLarge = new BuildingInfo("Sawmill", "lumberjack", "lumberjack", BuildingSize.Large);

        public static readonly BuildingInfo ShamansHutSmall     = new BuildingInfo("Shamans Hut", "shaman", "shaman", BuildingSize.Small);

        public static readonly BuildingInfo ChurchSmall         = new BuildingInfo("Church", "priest", "priest", BuildingSize.Small);
        public static readonly BuildingInfo ChurchMedium        = new BuildingInfo("Temple", "priest", "priest", BuildingSize.Medium);
        public static readonly BuildingInfo ChurchLarge         = new BuildingInfo("Cathedral", "priest", "priest", BuildingSize.Large);
        
        public static readonly BuildingInfo TavernSmall         = new BuildingInfo("Tavern", "barman", "barmaid", BuildingSize.Small);
        public static readonly BuildingInfo BarSmall            = new BuildingInfo("Bar", "barman", "barmaid", BuildingSize.Small);
        public static readonly BuildingInfo NightClubMedium     = new BuildingInfo("Night Club", "barman", "barmaid", BuildingSize.Medium);

        public static readonly BuildingInfo RoguesDenSmall      = new BuildingInfo("Rogue's Den", "rogue", "rogue", BuildingSize.Small);
        public static readonly BuildingInfo RoguesDenMedium     = new BuildingInfo("Rogue's Den", "rogue", "rogue", BuildingSize.Medium);

        public static readonly BuildingInfo GamblingSmall       = new BuildingInfo("Gambling Den", "gambler", "gambler", BuildingSize.Small);
        public static readonly BuildingInfo GamblingMedium      = new BuildingInfo("Casino", "gambler", "gambler", BuildingSize.Medium);

        public static readonly BuildingInfo BrothelMedium       = new BuildingInfo("Brothel", "prostitute", "prostitute", BuildingSize.Medium);

        public static readonly BuildingInfo StripClubSmall      = new BuildingInfo("Strip Club", "stripper", "stripper", BuildingSize.Small);

        public static readonly BuildingInfo CultureMeduim       = new BuildingInfo("Concert Hall", "musician", "musician", BuildingSize.Medium);
        public static readonly BuildingInfo CultureLarge        = new BuildingInfo("Conservatoire", "musician", "musician", BuildingSize.Large);

        public static readonly BuildingInfo TheatreMedium       = new BuildingInfo("Theatre", "actor", "actor", BuildingSize.Medium);
        public static readonly BuildingInfo CinemaLarge         = new BuildingInfo("Film Studio", "actor", "actor", BuildingSize.Large);

        public static readonly BuildingInfo CircusMedium        = new BuildingInfo("Circus", "gladiator", "gladiator", BuildingSize.Medium);

        public static readonly BuildingInfo TraderMedium        = new BuildingInfo("Merchant ship", "sailor", "sailor", BuildingSize.Medium);
        public static readonly BuildingInfo TraderLarge         = new BuildingInfo("Freighter", "sailor", "sailor", BuildingSize.Large);
        public static readonly BuildingInfo NavalAcademyHuge    = new BuildingInfo("Nautical School", "sailor", "sailor", BuildingSize.Huge);
        public static readonly BuildingInfo NavalVessel         = new BuildingInfo("Naval Vessel", "sailor", "sailor", BuildingSize.Medium);
        
        public static readonly BuildingInfo TailorWorkshopSmall = new BuildingInfo("Tailoring Shop", "tailor", "tailor", BuildingSize.Small);

        public static readonly BuildingInfo JevellerWorkshopSmall = new BuildingInfo("Jewellery Workshop", "jeveller", "jeveller", BuildingSize.Small);
        
        public static readonly BuildingInfo SmithySmall         = new BuildingInfo("Smithy", "blacksmith", "blacksmith", BuildingSize.Small);
        
        public static readonly BuildingInfo CarpentrySmall      = new BuildingInfo("Carpentry Workshop", "carpenter", "carpenter", BuildingSize.Small);

        public static readonly BuildingInfo IronworksSmall      = new BuildingInfo("Ironworks", "worker", "worker", BuildingSize.Medium);
        public static readonly BuildingInfo IronworksMedium     = new BuildingInfo("Metallurgical Plant", "worker", "worker", BuildingSize.Large);
        public static readonly BuildingInfo FurnitureSmall      = new BuildingInfo("Woodworking Factory", "worker", "worker", BuildingSize.Medium);
        public static readonly BuildingInfo FurnitureMedium     = new BuildingInfo("Woodworking Factory", "worker", "worker", BuildingSize.Large);
        public static readonly BuildingInfo ClothesFactorySmall = new BuildingInfo("Sewing Workshop", "worker", "worker", BuildingSize.Medium);
        public static readonly BuildingInfo ClothesFactoryMedium = new BuildingInfo("Textile Factory", "worker", "worker", BuildingSize.Large);
        public static readonly BuildingInfo JevellerWorkshopMedium = new BuildingInfo("Jewellery Factory", "worker", "worker", BuildingSize.Large);

        public static readonly BuildingInfo IronworksSlvSmall   = new BuildingInfo("Ironworks", "slave", "slave", BuildingSize.Medium);
        public static readonly BuildingInfo IronworksSlvMedium  = new BuildingInfo("Metallurgical Plant", "slave", "slave", BuildingSize.Large);
        public static readonly BuildingInfo FurnitureSlvSmall   = new BuildingInfo("Woodworking Factory", "slave", "slave", BuildingSize.Medium);
        public static readonly BuildingInfo FurnitureSlvMedium  = new BuildingInfo("Woodworking Factory", "slave", "slave", BuildingSize.Large);
        public static readonly BuildingInfo ClothesFactorySlvSmall = new BuildingInfo("Sewing Workshop", "slave", "slave", BuildingSize.Medium);
        public static readonly BuildingInfo ClothesFactorySlvMedium = new BuildingInfo("Textile Factory", "slave", "slave", BuildingSize.Large);
        public static readonly BuildingInfo JevellerWorkshopSlvMedium = new BuildingInfo("Jewellery Factory", "slave", "slave", BuildingSize.Large);

        public static readonly BuildingInfo PirateShip = new BuildingInfo("Pirate ship", "pirate", "pirate", BuildingSize.Small);

        public static readonly BuildingInfo BanditsBarracks     = new BuildingInfo("Barracks", "bandit", "bandit", BuildingSize.Small);

        public static readonly BuildingInfo BarracksSmall       = new BuildingInfo("Barracks", "soldier", "soldier", BuildingSize.Small);
        public static readonly BuildingInfo BarracksHuge        = new BuildingInfo("Barracks", "soldier", "soldier", BuildingSize.Huge);

        public static readonly BuildingInfo ScienceSmall        = new BuildingInfo("Laboratory", "scientiest", "scientiest", BuildingSize.Small);
        public static readonly BuildingInfo ScienceMedium       = new BuildingInfo("University", "scientiest", "scientiest", BuildingSize.Medium);
        public static readonly BuildingInfo ScienceLarge        = new BuildingInfo("Academy", "scientiest", "scientiest", BuildingSize.Large);

        public static readonly BuildingInfo SchoolSmall         = new BuildingInfo("School", "teacher", "teacher", BuildingSize.Small);
        public static readonly BuildingInfo SchoolMedium        = new BuildingInfo("College", "teacher", "teacher", BuildingSize.Medium);

        public static readonly BuildingInfo InnSmall            = new BuildingInfo("Inn", "innkeeper", "barmaid", BuildingSize.Small);

        public static readonly BuildingInfo MarketSmall         = new BuildingInfo("Marketplace", "merchant", "merchant", BuildingSize.Small);
        public static readonly BuildingInfo MarketMedium        = new BuildingInfo("Bazaar", "merchant", "merchant", BuildingSize.Large);

        public static readonly BuildingInfo CourtSmall          = new BuildingInfo("Courthouse", "scribe", "scribe", BuildingSize.Small);
        public static readonly BuildingInfo CourtMedium         = new BuildingInfo("Administration", "scribe", "scribe", BuildingSize.Medium);

        public static readonly BuildingInfo AdministrationSmall = new BuildingInfo("Courthouse", "clerk", "clerk", BuildingSize.Small);
        public static readonly BuildingInfo AdministrationMedium = new BuildingInfo("Administration", "clerk", "clerk", BuildingSize.Medium);

        public static readonly BuildingInfo HotelSmall          = new BuildingInfo("Motel", "clerk", "clerk", BuildingSize.Small);
        public static readonly BuildingInfo HotelMedium         = new BuildingInfo("Hotel", "clerk", "clerk", BuildingSize.Medium);
        public static readonly BuildingInfo HotelLarge          = new BuildingInfo("Resort", "clerk", "clerk", BuildingSize.Large);

        public static readonly BuildingInfo OfficeSmall         = new BuildingInfo("Company", "clerk", "clerk", BuildingSize.Small);
        public static readonly BuildingInfo OfficeMedium        = new BuildingInfo("Agency", "clerk", "clerk", BuildingSize.Medium);
        public static readonly BuildingInfo OfficeLarge         = new BuildingInfo("Corporation", "clerk", "clerk", BuildingSize.Medium);

        public static readonly BuildingInfo CastleSmall         = new BuildingInfo("Castle", "noble", "noble", BuildingSize.Medium);
        public static readonly BuildingInfo EstateSmall         = new BuildingInfo("Estate", "noble", "noble", BuildingSize.Medium);
        public static readonly BuildingInfo MansionSmall        = new BuildingInfo("Mansion", "noble", "noble", BuildingSize.Small);
        public static readonly BuildingInfo MansionMedium       = new BuildingInfo("Palace", "noble", "noble", BuildingSize.Medium);

        public static readonly BuildingInfo MedicineSmall       = new BuildingInfo("Chemist Shop", "chemist", "chemist", BuildingSize.Medium);
        public static readonly BuildingInfo MedicineMedium      = new BuildingInfo("Clinic", "doctor", "doctor", BuildingSize.Medium);
        public static readonly BuildingInfo MedicineLarge       = new BuildingInfo("Hospital", "doctor", "doctor", BuildingSize.Large);

        public static readonly BuildingInfo AlchemyShopSmall    = new BuildingInfo("Alchemy Shop", "alchemist", "alchemist", BuildingSize.Small);
      
        public static readonly BuildingInfo MagicSmall          = new BuildingInfo("Mage Tower", "mage", "mage", BuildingSize.Small);
        public static readonly BuildingInfo MagicMedium         = new BuildingInfo("Magic University", "mage", "mage", BuildingSize.Medium);
        public static readonly BuildingInfo MagicLarge          = new BuildingInfo("Magic Academy", "mage", "mage", BuildingSize.Large);
        
        public string m_sName;
        public string m_sOwnerM;
        public string m_sOwnerF;

        public int m_iMinPop;
        public int m_iMaxPop;

        public BuildingSize m_eSize;
        
        public BuildingInfo(string sName, string sOwnerM, string sOwnerF, BuildingSize eSize)
        {
            m_sName = sName;
            m_sOwnerM = sOwnerM;
            m_sOwnerF = sOwnerF;

            m_eSize = eSize;

            switch (m_eSize)
            {
                case BuildingSize.Unique:
                    m_iMinPop = 1;
                    m_iMaxPop = 1;
                    break;
                case BuildingSize.Small:
                    m_iMinPop = 5;
                    m_iMaxPop = 15;
                    break;
                case BuildingSize.Medium:
                    m_iMinPop = 30;//10;
                    m_iMaxPop = 30;
                    break;
                case BuildingSize.Large:
                    m_iMinPop = 150;//15;
                    m_iMaxPop = 45;
                    break;
                case BuildingSize.Huge:
                    m_iMinPop = 300;//20;
                    m_iMaxPop = 70;
                    break;
            }
        }
    }

    public class Building
    {
        public Settlement m_pSettlement;

        public BuildingInfo m_pInfo;

        /// <summary>
        /// Центральное здание
        /// </summary>
        /// <param name="pSettlement">поселение</param>
        /// <param name="pInfo">шаблон здания</param>
        /// <param name="bCapital">в этом здании живёт местный правитель (false) или глава всего государства (true)</param>
        public Building(Settlement pSettlement, BuildingInfo pInfo, bool bCapital)
        {
            m_pSettlement = pSettlement;
            m_pInfo = pInfo;

            /*
            AddPopulation(1 + Rnd.Get(5), Settlement.Info[pSettlement.m_eSize].m_iMaxProfessionRank, pSettlement.m_pLand.m_pState.m_iTier * pSettlement.m_pState.m_iTier);

            GenerateName();

            if (bCapital)
            {
                foreach (Opponent pRuler in m_pState.m_cRulers)
                {
                    m_cDwellers.Add(pRuler);
                    pRuler.m_pHome = this;
                }
                foreach (Opponent pHeir in m_pState.m_cHeirs)
                {
                    m_cDwellers.Add(pHeir);
                    pHeir.m_pHome = this;
                }
            }
            else
            {
                ValuedString[] profM = { new ValuedString(pInfo.m_sOwnerM, pInfo.m_iRank) };
                ValuedString[] profF = { new ValuedString(pInfo.m_sOwnerF, pInfo.m_iRank) };
                Opponent pFounder = new Opponent(this, m_pState.GetRandomRace(State.DwellersCathegory.MajorRaces),
                    profM, profF, true, null);
                m_cDwellers.Add(pFounder);

                int iPop = Rnd.Get(5);
                for (int i = 0; i < iPop; i++)
                {
                    Opponent pPretendent = new Opponent(this, m_pState.GetRandomRace(State.DwellersCathegory.MajorRaces),
                        profM, profF, true, pFounder);
                    m_cDwellers.Add(pPretendent);
                }
            }
             */
        }

        /// <summary>
        /// Произвольное здание в поселении
        /// </summary>
        /// <param name="pSettlement"></param>
        public Building(Settlement pSettlement, Province pProvince)
        {
            m_pSettlement = pSettlement;

            State pState = (State)pProvince.Owner;

            int iInfrastructureLevel = pState.m_iInfrastructureLevel;
            int iControl = pState.m_iControl *2;

            Dictionary<BuildingInfo, float> cChances = new Dictionary<BuildingInfo, float>();
            switch (m_pSettlement.m_pInfo.m_eSize)
            {
                case SettlementSize.Hamlet:
                    {
                        if (pSettlement.m_cBuildings.Count > 0)
                        {
                            if (iInfrastructureLevel < 2)
                            {
                                cChances[BuildingInfo.WarriorsHutSmall] = (float)iControl / 4;
                                if (iInfrastructureLevel < 2)
                                    cChances[BuildingInfo.WarriorsHutSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Piety][iInfrastructureLevel] / 2;
                            }

                            if (iInfrastructureLevel < 2)
                                cChances[BuildingInfo.ShamansHutSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Piety][iInfrastructureLevel] / 2;

                            if (pState.m_iSocialEquality == 0)
                            {
                                cChances[BuildingInfo.SlavePensMedium] = (float)cChances.Count;// / 2 + 1;
                            }
                        }

                        BuildingInfo pProfile;
                        switch (m_pSettlement.m_eSpeciality)
                        {
                            case SettlementSpeciality.Fishers:
                                pProfile = BuildingInfo.FishingBoatMedium;
                                break;
                            case SettlementSpeciality.Farmers:
                                pProfile = BuildingInfo.FarmMedium;
                                break;
                            case SettlementSpeciality.Peasants:
                                pProfile = BuildingInfo.PeasantsHutMedium;
                                break;
                            case SettlementSpeciality.Hunters:
                                pProfile = BuildingInfo.HuntersHutMedium;
                                break;
                            case SettlementSpeciality.Miners:
                                pProfile = BuildingInfo.MineMedium;
                                break;
                            case SettlementSpeciality.Lumberjacks:
                                pProfile = BuildingInfo.LumberjacksHutMedium;
                                break;
                            default:
                                pProfile = BuildingInfo.LoafersHut;
                                break;
                        }

                        cChances[pProfile] = (float)cChances.Count + 1;
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

                            cChances[pGuard] = (float)iControl / 4;

                            BuildingInfo pChurch;
                            if (iInfrastructureLevel < 2)
                                pChurch = BuildingInfo.ShamansHutSmall;
                            else
                                pChurch = BuildingInfo.ChurchSmall;

                            cChances[pChurch] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Piety][iInfrastructureLevel] / 2;

                            if (iInfrastructureLevel >= 2)
                            {
                                if (iInfrastructureLevel < 4)
                                {
                                    cChances[BuildingInfo.InnSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 8;
                                    cChances[BuildingInfo.TavernSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                                }
                                else
                                {
                                    cChances[BuildingInfo.HotelSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 8;
                                    cChances[BuildingInfo.BarSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                                }

                                cChances[BuildingInfo.RoguesDenSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Treachery][iInfrastructureLevel] / 2;
                            }

                            if (iInfrastructureLevel >= 3)
                            {
                                float fScience = 0.05f;
                                if (pState.m_pCustoms.m_eMindSet == Customs.MindSet.Balanced_mind)
                                    fScience = 0.25f;
                                if (pState.m_pCustoms.m_eMindSet == Customs.MindSet.Logic)
                                    fScience = 0.5f;

                                if (pProvince.m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Sapient)
                                    fScience *= 2;
                                if (pProvince.m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Ingenious)
                                    fScience *= 4;

                                cChances[BuildingInfo.SchoolSmall] = fScience / 4;
                            }

                            if (pState.m_iSocialEquality == 0)
                            {
                                cChances[BuildingInfo.SlavePensMedium] = (float)cChances.Count;// / 2 + 1;
                            }

                            cChances[BuildingInfo.MarketSmall] = (float)cChances.Count / 2;// / 2 + 1;

                            if (pState.m_pInfo.m_bDinasty)
                            {
                                cChances[BuildingInfo.EstateSmall] = (float)cChances.Count / 4;
                                cChances[BuildingInfo.CastleSmall] = (float)cChances.Count / 12;
                            }
                        }

                        BuildingInfo pProfile;
                        switch (m_pSettlement.m_eSpeciality)
                        {
                            case SettlementSpeciality.Fishers:
                                pProfile = BuildingInfo.FishingBoatMedium;
                                break;
                            case SettlementSpeciality.Farmers:
                                pProfile = Rnd.OneChanceFrom(2) ? BuildingInfo.FarmMedium : BuildingInfo.FarmLarge;
                                break;
                            case SettlementSpeciality.Peasants:
                                pProfile = Rnd.OneChanceFrom(2) ? BuildingInfo.PeasantsHutMedium : BuildingInfo.PeasantsHutLarge;
                                break;
                            case SettlementSpeciality.Hunters:
                                pProfile = Rnd.OneChanceFrom(2) ? BuildingInfo.HuntersHutMedium : BuildingInfo.HuntersHutLarge;
                                break;
                            case SettlementSpeciality.Miners:
                                pProfile = Rnd.OneChanceFrom(2) ? BuildingInfo.MineMedium : BuildingInfo.MineLarge;
                                break;
                            case SettlementSpeciality.Lumberjacks:
                                pProfile = Rnd.OneChanceFrom(2) ? BuildingInfo.LumberjacksHutMedium : BuildingInfo.LumberjacksHutLarge;
                                break;
                            default:
                                pProfile = BuildingInfo.LoafersHut;
                                break;
                        }

                        cChances[pProfile] = (float)cChances.Count + 1;
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

                            cChances[pGuard] = (float)iControl / 4;

                            BuildingInfo pPrison;
                            if (iInfrastructureLevel < 4)
                                pPrison = BuildingInfo.PrisonSmall;
                            else
                                if (iInfrastructureLevel == 8)
                                    pPrison = BuildingInfo.HoldingSmall;
                                else
                                    pPrison = BuildingInfo.PrisonPoliceSmall;

                            cChances[pPrison] = (float)iControl / 4;

                            cChances[BuildingInfo.ChurchMedium] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Piety][iInfrastructureLevel] / 10;
                            cChances[BuildingInfo.ChurchSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Piety][iInfrastructureLevel] / 2;

                            switch (pState.m_pCustoms.m_eSexuality)
                            {
                                case Psichology.Customs.Sexuality.Moderate_sexuality:
                                    cChances[BuildingInfo.BrothelMedium] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 4;
                                    if(iInfrastructureLevel >= 4)
                                        cChances[BuildingInfo.StripClubSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                                    break;
                                case Psichology.Customs.Sexuality.Lecherous:
                                    cChances[BuildingInfo.BrothelMedium] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                                    if(iInfrastructureLevel >= 4)
                                        cChances[BuildingInfo.StripClubSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                                    break;
                            }

                            if (iInfrastructureLevel < 4)
                            {
                                cChances[BuildingInfo.InnSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 8;
                                cChances[BuildingInfo.TavernSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                            }
                            else
                            {
                                cChances[BuildingInfo.HotelMedium] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 8;
                                cChances[BuildingInfo.BarSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                            }

                            cChances[BuildingInfo.TheatreMedium] = 1f - pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;

                            cChances[BuildingInfo.RoguesDenMedium] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Treachery][iInfrastructureLevel] / 2;
                            cChances[BuildingInfo.GamblingSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Treachery][iInfrastructureLevel] / 2;

                            if (iInfrastructureLevel < 3)
                                cChances[BuildingInfo.MedicineSmall] = (float)cChances.Count / 6;
                            else
                                cChances[BuildingInfo.MedicineMedium] = (float)cChances.Count / 6;

                            float fBureaucracy = 0.05f;
                            if (pState.m_pCustoms.m_eMindSet == Customs.MindSet.Balanced_mind)
                                fBureaucracy = 0.25f;
                            if (pState.m_pCustoms.m_eMindSet == Customs.MindSet.Logic)
                                fBureaucracy = 0.5f;

                            if (pProvince.m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == GeneLab.Genetix.Intelligence.Sapient)
                                fBureaucracy *= 2;
                            if (pProvince.m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == GeneLab.Genetix.Intelligence.Primitive)
                                fBureaucracy *= 4;

                            if (m_pSettlement.m_bCapital)
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
                                if (pState.m_pCustoms.m_eMindSet == Customs.MindSet.Balanced_mind)
                                    fScience = 0.25f;
                                if (pState.m_pCustoms.m_eMindSet == Customs.MindSet.Logic)
                                    fScience = 0.5f;

                                if (pProvince.m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Sapient)
                                    fScience *= 2;
                                if (pProvince.m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Ingenious)
                                    fScience *= 4;

                                cChances[BuildingInfo.ScienceSmall] = fScience / 4;

                                cChances[BuildingInfo.SchoolSmall] = fScience;
                                cChances[BuildingInfo.SchoolMedium] = fScience / 2;
                            }

                            if (pState.m_iSocialEquality == 0)
                            {
                                cChances[BuildingInfo.SlaveMarketMedium] = (float)cChances.Count / 3 + 1;
                                cChances[BuildingInfo.SlavePensLarge] = (float)cChances.Count;// / 2 + 1;
                            }

                            if (iInfrastructureLevel < 4)
                            {
                                cChances[BuildingInfo.MarketSmall] = (float)cChances.Count / 3 + 1;
                                cChances[BuildingInfo.MarketMedium] = (float)cChances.Count / 10 + 1;
                            }
                            else
                            {
                                cChances[BuildingInfo.OfficeSmall] = (float)cChances.Count;// / 2 + 1;
                                cChances[BuildingInfo.OfficeMedium] = (float)cChances.Count / 2;// / 2 + 1;
                            }

                            if (pState.m_pInfo.m_bDinasty)
                                cChances[BuildingInfo.MansionSmall] = m_pSettlement.m_bCapital ? (float)cChances.Count*2 : (float)cChances.Count;
                        }

                        BuildingInfo pProfile;
                        switch (m_pSettlement.m_eSpeciality)
                        {
                            case SettlementSpeciality.Fishers:
                                pProfile = BuildingInfo.FishingBoatLarge;
                                break;
                            case SettlementSpeciality.Naval:
                                pProfile = BuildingInfo.TraderMedium;
                                break;
                            case SettlementSpeciality.Tailors:
                                if (iInfrastructureLevel < 4)
                                    pProfile = BuildingInfo.TailorWorkshopSmall;
                                else
                                    pProfile = pState.m_iSocialEquality == 0 ? BuildingInfo.ClothesFactorySlvSmall : BuildingInfo.ClothesFactorySmall;
                                break;
                            case SettlementSpeciality.Jevellers:
                                pProfile = BuildingInfo.JevellerWorkshopSmall;
                                break;
                            case SettlementSpeciality.Factory:
                                if (iInfrastructureLevel < 4)
                                    pProfile = BuildingInfo.SmithySmall;
                                else
                                    pProfile = pState.m_iSocialEquality == 0 ? BuildingInfo.IronworksSlvSmall : BuildingInfo.IronworksSmall;
                                break;
                            case SettlementSpeciality.Artisans:
                                if (iInfrastructureLevel < 4)
                                    pProfile = BuildingInfo.CarpentrySmall;
                                else
                                    pProfile = pState.m_iSocialEquality == 0 ? BuildingInfo.FurnitureSlvSmall : BuildingInfo.FurnitureSmall;
                                break;
                            default:
                                pProfile = BuildingInfo.LoafersHut;
                                break;
                        }

                        cChances[pProfile] = (float)cChances.Count + 1;
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

                            cChances[pGuard] = (float)iControl / 4;

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

                            cChances[BuildingInfo.ChurchMedium] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Piety][iInfrastructureLevel] / 10;
                            cChances[BuildingInfo.ChurchSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Piety][iInfrastructureLevel] / 2;

                            switch (pState.m_pCustoms.m_eSexuality)
                            {
                                case Psichology.Customs.Sexuality.Moderate_sexuality:
                                    cChances[BuildingInfo.BrothelMedium] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 4;
                                    if (iInfrastructureLevel >= 4)
                                        cChances[BuildingInfo.StripClubSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                                    break;
                                case Psichology.Customs.Sexuality.Lecherous:
                                    cChances[BuildingInfo.BrothelMedium] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                                    if (iInfrastructureLevel >= 4)
                                        cChances[BuildingInfo.StripClubSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                                    break;
                            }

                            if (iInfrastructureLevel < 4)
                            {
                                cChances[BuildingInfo.InnSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 8;
                                cChances[BuildingInfo.TavernSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                                cChances[BuildingInfo.CircusMedium] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] *
                                                                      pState.m_pCulture.MentalityValues[Psichology.Mentality.Agression][iInfrastructureLevel] / 4;
                            }
                            else
                            {
                                cChances[BuildingInfo.HotelMedium] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 8;
                                cChances[BuildingInfo.BarSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                                cChances[BuildingInfo.NightClubMedium] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                            }

                            cChances[BuildingInfo.TheatreMedium] = 1f - pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;

                            cChances[BuildingInfo.RoguesDenMedium] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Treachery][iInfrastructureLevel] / 2;
                            cChances[BuildingInfo.GamblingSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Treachery][iInfrastructureLevel] / 2;

                            if (iInfrastructureLevel < 3)
                                cChances[BuildingInfo.MedicineSmall] = (float)cChances.Count / 6;
                            else
                            {
                                cChances[BuildingInfo.MedicineMedium] = (float)cChances.Count / 6;
                                cChances[BuildingInfo.MedicineLarge] = (float)cChances.Count / 12;
                            }

                            float fBureaucracy = 0.05f;
                            if (pState.m_pCustoms.m_eMindSet == Psichology.Customs.MindSet.Balanced_mind)
                                fBureaucracy = 0.25f;
                            if (pState.m_pCustoms.m_eMindSet == Psichology.Customs.MindSet.Logic)
                                fBureaucracy = 0.5f;

                            if (pProvince.m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == GeneLab.Genetix.Intelligence.Sapient)
                                fBureaucracy *= 2;
                            if (pProvince.m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == GeneLab.Genetix.Intelligence.Primitive)
                                fBureaucracy *= 4;

                            if (m_pSettlement.m_bCapital)
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
                                if (pState.m_pCustoms.m_eMindSet == Customs.MindSet.Balanced_mind)
                                    fScience = 0.25f;
                                if (pState.m_pCustoms.m_eMindSet == Customs.MindSet.Logic)
                                    fScience = 0.5f;

                                if (pProvince.m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Sapient)
                                    fScience *= 2;
                                if (pProvince.m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Ingenious)
                                    fScience *= 4;

                                cChances[BuildingInfo.ScienceSmall] = fScience / 4;
                                cChances[BuildingInfo.ScienceMedium] = fScience / 8;

                                cChances[BuildingInfo.SchoolSmall] = fScience;
                                cChances[BuildingInfo.SchoolMedium] = fScience / 2;
                            }

                            if (pState.m_iSocialEquality == 0)
                            {
                                cChances[BuildingInfo.SlaveMarketMedium] = (float)cChances.Count / 3 + 1;
                                cChances[BuildingInfo.SlavePensHuge] = (float)cChances.Count;// / 2 + 1;
                            }

                            if (iInfrastructureLevel < 4)
                            {
                                //cChances[BuildingInfo.MarketSmall] = cChances.Count;// / 2 + 1;
                                cChances[BuildingInfo.MarketMedium] = (float)cChances.Count / 10 + 1;
                            }
                            else
                            {
                                //cChances[BuildingInfo.OfficeSmall] = cChances.Count;// / 2 + 1;
                                cChances[BuildingInfo.OfficeMedium] = (float)cChances.Count;// / 2 + 1;
                                cChances[BuildingInfo.OfficeLarge] = (float)cChances.Count / 2;// / 2 + 1;
                            }

                            if (pState.m_pInfo.m_bDinasty)
                            {
                                cChances[BuildingInfo.MansionSmall] = m_pSettlement.m_bCapital ? (float)cChances.Count * 2 : (float)cChances.Count;
                                cChances[BuildingInfo.MansionMedium] = m_pSettlement.m_bCapital ? (float)cChances.Count * 2 : (float)cChances.Count;
                            }
                        }

                        BuildingInfo pProfile;
                        switch (m_pSettlement.m_eSpeciality)
                        {
                            case SettlementSpeciality.NavalAcademy:
                                pProfile = Rnd.OneChanceFrom(2) ? BuildingInfo.NavalAcademyHuge : BuildingInfo.TraderLarge;
                                break;
                            case SettlementSpeciality.Naval:
                                pProfile = BuildingInfo.TraderLarge;
                                break;
                            case SettlementSpeciality.Resort:
                                pProfile = Rnd.OneChanceFrom(2) ? BuildingInfo.HotelMedium : BuildingInfo.HotelLarge;
                                break;
                            case SettlementSpeciality.Cultural:
                                pProfile = Rnd.OneChanceFrom(2) ? BuildingInfo.CultureMeduim : BuildingInfo.CultureLarge;
                                break;
                            case SettlementSpeciality.ArtsAcademy:
                                pProfile = iInfrastructureLevel < 4  ? (pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] > Rnd.Get(2f) ? BuildingInfo.CircusMedium : BuildingInfo.TheatreMedium) : BuildingInfo.CinemaLarge;
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
                                pProfile = pState.m_iSocialEquality == 0 ? BuildingInfo.ClothesFactorySlvMedium : BuildingInfo.ClothesFactoryMedium;
                                break;
                            case SettlementSpeciality.Jevellers:
                                pProfile = pState.m_iSocialEquality == 0 ? BuildingInfo.JevellerWorkshopSlvMedium : BuildingInfo.JevellerWorkshopMedium;
                                break;
                            case SettlementSpeciality.Factory:
                                pProfile = pState.m_iSocialEquality == 0 ? BuildingInfo.IronworksSlvMedium : BuildingInfo.IronworksMedium;
                                break;
                            case SettlementSpeciality.Artisans:
                                pProfile = pState.m_iSocialEquality == 0 ? BuildingInfo.FurnitureSlvMedium : BuildingInfo.FurnitureMedium;
                                break;
                            default:
                                pProfile = BuildingInfo.LoafersHut;
                                break;
                        }

                        cChances[pProfile] = (float)cChances.Count + 1;
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

                            cChances[pGuard] = (float)iControl / 4;

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

                            cChances[BuildingInfo.ChurchMedium] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Piety][iInfrastructureLevel] / 10;
                            cChances[BuildingInfo.ChurchSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Piety][iInfrastructureLevel] / 2;

                            switch (pState.m_pCustoms.m_eSexuality)
                            {
                                case Psichology.Customs.Sexuality.Moderate_sexuality:
                                    cChances[BuildingInfo.BrothelMedium] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 4;
                                    if (iInfrastructureLevel >= 4)
                                        cChances[BuildingInfo.StripClubSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                                    break;
                                case Psichology.Customs.Sexuality.Lecherous:
                                    cChances[BuildingInfo.BrothelMedium] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                                    if (iInfrastructureLevel >= 4)
                                        cChances[BuildingInfo.StripClubSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                                    break;
                            }

                            if (iInfrastructureLevel < 4)
                            {
                                cChances[BuildingInfo.InnSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 8;
                                cChances[BuildingInfo.TavernSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                                cChances[BuildingInfo.CircusMedium] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] *
                                                                      pState.m_pCulture.MentalityValues[Psichology.Mentality.Agression][iInfrastructureLevel] / 4;
                            }
                            else
                            {
                                cChances[BuildingInfo.HotelMedium] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 8;
                                cChances[BuildingInfo.BarSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                                cChances[BuildingInfo.NightClubMedium] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                            }

                            cChances[BuildingInfo.TheatreMedium] = 1f - pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;

                            cChances[BuildingInfo.RoguesDenMedium] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Treachery][iInfrastructureLevel] / 2;
                            cChances[BuildingInfo.GamblingSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Treachery][iInfrastructureLevel] / 2;

                            if (iInfrastructureLevel < 3)
                                cChances[BuildingInfo.MedicineSmall] = (float)cChances.Count / 6;
                            else
                            {
                                cChances[BuildingInfo.MedicineMedium] = (float)cChances.Count / 6;
                                cChances[BuildingInfo.MedicineLarge] = (float)cChances.Count / 12;
                            }

                            float fBureaucracy = 0.05f;
                            if (pState.m_pCustoms.m_eMindSet == Psichology.Customs.MindSet.Balanced_mind)
                                fBureaucracy = 0.25f;
                            if (pState.m_pCustoms.m_eMindSet == Psichology.Customs.MindSet.Logic)
                                fBureaucracy = 0.5f;

                            if (pProvince.m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == GeneLab.Genetix.Intelligence.Sapient)
                                fBureaucracy *= 2;
                            if (pProvince.m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == GeneLab.Genetix.Intelligence.Primitive)
                                fBureaucracy *= 4;

                            if (m_pSettlement.m_bCapital)
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
                                if (pState.m_pCustoms.m_eMindSet == Customs.MindSet.Balanced_mind)
                                    fScience = 0.25f;
                                if (pState.m_pCustoms.m_eMindSet == Customs.MindSet.Logic)
                                    fScience = 0.5f;

                                if (pProvince.m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Sapient)
                                    fScience *= 2;
                                if (pProvince.m_pNation.m_pFenotype.m_pBrain.m_eIntelligence == Intelligence.Ingenious)
                                    fScience *= 4;

                                cChances[BuildingInfo.ScienceSmall] = fScience / 4;
                                cChances[BuildingInfo.ScienceMedium] = fScience / 8;

                                cChances[BuildingInfo.SchoolSmall] = fScience;
                                cChances[BuildingInfo.SchoolMedium] = fScience / 2;
                            }

                            if (pState.m_iSocialEquality == 0)
                            {
                                cChances[BuildingInfo.SlaveMarketMedium] = (float)cChances.Count / 3 + 1;
                                cChances[BuildingInfo.SlavePensHuge] = (float)cChances.Count;// / 2 + 1;
                            }

                            if (iInfrastructureLevel < 4)
                            {
                                //cChances[BuildingInfo.MarketSmall] = cChances.Count;// / 2 + 1;
                                cChances[BuildingInfo.MarketMedium] = (float)cChances.Count / 10 + 1;
                            }
                            else
                            {
                                //cChances[BuildingInfo.OfficeSmall] = cChances.Count;// / 2 + 1;
                                cChances[BuildingInfo.OfficeMedium] = (float)cChances.Count;// / 2 + 1;
                                cChances[BuildingInfo.OfficeLarge] = (float)cChances.Count / 2;// / 2 + 1;
                            }

                            if (pState.m_pInfo.m_bDinasty)
                            {
                                cChances[BuildingInfo.MansionSmall] = m_pSettlement.m_bCapital ? (float)cChances.Count * 2 : (float)cChances.Count;
                                cChances[BuildingInfo.MansionMedium] = m_pSettlement.m_bCapital ? (float)cChances.Count * 2 : (float)cChances.Count;
                            }
                        }

                        BuildingInfo pProfile;
                        switch (m_pSettlement.m_eSpeciality)
                        {
                            case SettlementSpeciality.NavalAcademy:
                                pProfile = Rnd.OneChanceFrom(2) ? BuildingInfo.NavalAcademyHuge : BuildingInfo.TraderLarge;
                                break;
                            case SettlementSpeciality.Naval:
                                pProfile = BuildingInfo.TraderLarge;
                                break;
                            case SettlementSpeciality.Resort:
                                pProfile = Rnd.OneChanceFrom(2) ? BuildingInfo.HotelMedium : BuildingInfo.HotelLarge;
                                break;
                            case SettlementSpeciality.Cultural:
                                pProfile = Rnd.OneChanceFrom(2) ? BuildingInfo.CultureMeduim : BuildingInfo.CultureLarge;
                                break;
                            case SettlementSpeciality.ArtsAcademy:
                                pProfile = iInfrastructureLevel < 4  ? (pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] > Rnd.Get(2f) ? BuildingInfo.CircusMedium : BuildingInfo.TheatreMedium) : BuildingInfo.CinemaLarge;
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
                                pProfile = pState.m_iSocialEquality == 0 ? BuildingInfo.ClothesFactorySlvMedium : BuildingInfo.ClothesFactoryMedium;
                                break;
                            case SettlementSpeciality.Jevellers:
                                pProfile = pState.m_iSocialEquality == 0 ? BuildingInfo.JevellerWorkshopSlvMedium : BuildingInfo.JevellerWorkshopMedium;
                                break;
                            case SettlementSpeciality.Factory:
                                pProfile = pState.m_iSocialEquality == 0 ? BuildingInfo.IronworksSlvSmall : BuildingInfo.IronworksMedium;
                                break;
                            case SettlementSpeciality.Artisans:
                                pProfile = pState.m_iSocialEquality == 0 ? BuildingInfo.FurnitureSlvMedium : BuildingInfo.FurnitureMedium;
                                break;
                            default:
                                pProfile = BuildingInfo.LoafersHut;
                                break;
                        }

                        cChances[pProfile] = (float)cChances.Count + 1;
                    }
                    break;
                case SettlementSize.Fort:
                    {
                        if (pSettlement.m_cBuildings.Count > 0)// && Rnd.OneChanceFrom(2))
                        {
                            cChances[BuildingInfo.ChurchMedium] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Piety][iInfrastructureLevel] / 10;
                            cChances[BuildingInfo.ChurchSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Piety][iInfrastructureLevel] / 2;

                            switch (pState.m_pCustoms.m_eSexuality)
                            {
                                case Psichology.Customs.Sexuality.Moderate_sexuality:
                                    cChances[BuildingInfo.BrothelMedium] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 4;
                                    if (iInfrastructureLevel >= 4)
                                        cChances[BuildingInfo.StripClubSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                                    break;
                                case Psichology.Customs.Sexuality.Lecherous:
                                    cChances[BuildingInfo.BrothelMedium] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                                    if (iInfrastructureLevel >= 4)
                                        cChances[BuildingInfo.StripClubSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                                    break;
                            }

                            if (iInfrastructureLevel < 4)
                                cChances[BuildingInfo.TavernSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;
                            else
                                cChances[BuildingInfo.BarSmall] = pState.m_pCulture.MentalityValues[Psichology.Mentality.Rudeness][iInfrastructureLevel] / 2;

                            if (pState.m_iSocialEquality == 0)
                            {
                                cChances[BuildingInfo.SlavePensMedium] = (float)cChances.Count / 2;
                            }
                        }

                        BuildingInfo pProfile;
                        switch (m_pSettlement.m_eSpeciality)
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

            foreach (Building pBuilding in m_pSettlement.m_cBuildings)
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
            m_pInfo = cChances.ElementAt(iChance).Key;
        }

        public override string ToString()
        {
            return m_pInfo.m_sName;
        }
    }
}
