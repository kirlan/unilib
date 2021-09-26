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
        /// <summary>
        /// 1 owner, 0 workers
        /// </summary>
        Unique,
        /// <summary>
        /// 1 owner, 3 workers
        /// </summary>
        Small,
        /// <summary>
        /// 3 owners, 15 workers
        /// </summary>
        Medium,
        /// <summary>
        /// 5 owners, 50 workers
        /// </summary>
        Large,
        /// <summary>
        /// 10 owners, 200 workers
        /// </summary>
        Huge
    }

    public enum FamilyOwnership
    {
        /// <summary>
        /// Никаких требований к семейным узам
        /// </summary>
        None,
        /// <summary>
        /// Все владельцы должны быть из одной семьи
        /// </summary>
        Owners,
        /// <summary>
        /// Все обитатели должны быть из одной семьи
        /// </summary>
        Full
    }
    
    public class BuildingInfo
    {
        #region statics
        public static readonly BuildingInfo LoafersHut = new BuildingInfo("Hut", ProfessionInfo.Loafer, ProfessionInfo.Loafer, BuildingSize.Small);

        //public static readonly BuildingInfo SlavePensSmall = new BuildingInfo("Slave Pens", ProfessionInfo.Slaver, ProfessionInfo.WorkingSlave, BuildingSize.Small);
        public static readonly BuildingInfo SlavePensMedium = new BuildingInfo("Slave Pens", ProfessionInfo.Slaver, ProfessionInfo.WorkingSlave, BuildingSize.Medium);
        //public static readonly BuildingInfo SlavePensLarge = new BuildingInfo("Slave Pens", ProfessionInfo.Slaver, ProfessionInfo.WorkingSlave, BuildingSize.Large);
        //public static readonly BuildingInfo SlavePensHuge = new BuildingInfo("Slave Pens", ProfessionInfo.Slaver, ProfessionInfo.WorkingSlave, BuildingSize.Huge);

        public static readonly BuildingInfo SlaveMarketMedium = new BuildingInfo("Slave Market", ProfessionInfo.Slaver, ProfessionInfo.WorkingSlave, BuildingSize.Medium);
        public static readonly BuildingInfo SlaveMarketMedium2 = new BuildingInfo("Slave Market", ProfessionInfo.Slaver, ProfessionInfo.PleasureSlave, BuildingSize.Medium);
        
        public static readonly BuildingInfo WarriorsHutSmall    = new BuildingInfo("Warriors Hut", ProfessionInfo.Warrior, ProfessionInfo.Warrior, BuildingSize.Small);

        public static readonly BuildingInfo RaidersHutSmall = new BuildingInfo("Raiders Hut", ProfessionInfo.Raider, ProfessionInfo.Raider, BuildingSize.Small);

        public static readonly BuildingInfo GuardTowerSmall = new BuildingInfo("Guard Tower", ProfessionInfo.Guard, ProfessionInfo.Guard, BuildingSize.Small);
        public static readonly BuildingInfo GuardBarracksSmall = new BuildingInfo("Guard Barracks", ProfessionInfo.Officer, ProfessionInfo.Guard, BuildingSize.Small);
        public static readonly BuildingInfo GuardBarracksMedium = new BuildingInfo("Guard Barracks", ProfessionInfo.Officer, ProfessionInfo.Guard, BuildingSize.Medium);
        public static readonly BuildingInfo PrisonSmall = new BuildingInfo("Prison", ProfessionInfo.Guard, ProfessionInfo.Guard, BuildingSize.Small);
        public static readonly BuildingInfo PrisonMedium = new BuildingInfo("Prison", ProfessionInfo.Guard, ProfessionInfo.Guard, BuildingSize.Medium);

        public static readonly BuildingInfo EmergencyPostSmall = new BuildingInfo("Emergency Post", ProfessionInfo.Watcher, ProfessionInfo.Watcher, BuildingSize.Small);
        public static readonly BuildingInfo HoldingSmall = new BuildingInfo("Holding", ProfessionInfo.Watcher, ProfessionInfo.Watcher, BuildingSize.Small);
        public static readonly BuildingInfo HoldingMedium = new BuildingInfo("Holding", ProfessionInfo.Watcher, ProfessionInfo.Watcher, BuildingSize.Medium);

        public static readonly BuildingInfo PoliceStationSmall = new BuildingInfo("Police Station", ProfessionInfo.Officer, ProfessionInfo.Policeman, BuildingSize.Small);
        public static readonly BuildingInfo PoliceStationMedium = new BuildingInfo("Police Department", ProfessionInfo.Officer, ProfessionInfo.Policeman, BuildingSize.Medium);
        public static readonly BuildingInfo PrisonPoliceSmall = new BuildingInfo("Prison", ProfessionInfo.Officer, ProfessionInfo.Policeman, BuildingSize.Small);
        public static readonly BuildingInfo PrisonPoliceMedium = new BuildingInfo("Prison", ProfessionInfo.Officer, ProfessionInfo.Policeman, BuildingSize.Medium);

        public static readonly BuildingInfo FishingBoatMedium = new BuildingInfo("Fishing boat", ProfessionInfo.Fisher, ProfessionInfo.Fisher, BuildingSize.Medium);
        public static readonly BuildingInfo FishingBoatLarge = new BuildingInfo("Fishing Trawler", ProfessionInfo.Fisher, ProfessionInfo.Fisher, BuildingSize.Large);

        public static readonly BuildingInfo FishingBoatSlvMedium = new BuildingInfo("Fishing boat", ProfessionInfo.Fisher, ProfessionInfo.WorkingSlave, BuildingSize.Medium);
        public static readonly BuildingInfo FishingBoatSlvLarge = new BuildingInfo("Fishing Trawler", ProfessionInfo.Fisher, ProfessionInfo.WorkingSlave, BuildingSize.Large);

        public static readonly BuildingInfo FarmMedium = new BuildingInfo("Farm", ProfessionInfo.Farmer, ProfessionInfo.Farmer, BuildingSize.Medium, FamilyOwnership.Owners);
        public static readonly BuildingInfo FarmLarge = new BuildingInfo("Ranch", ProfessionInfo.Farmer, ProfessionInfo.Farmer, BuildingSize.Large, FamilyOwnership.Owners);

        public static readonly BuildingInfo FarmSlvMedium = new BuildingInfo("Farm", ProfessionInfo.Farmer, ProfessionInfo.WorkingSlave, BuildingSize.Medium, FamilyOwnership.Owners);
        public static readonly BuildingInfo FarmSlvLarge = new BuildingInfo("Ranch", ProfessionInfo.Farmer, ProfessionInfo.WorkingSlave, BuildingSize.Large, FamilyOwnership.Owners);

        public static readonly BuildingInfo PeasantsHutMedium = new BuildingInfo("Peasants' Hut", ProfessionInfo.Peasant, ProfessionInfo.Peasant, BuildingSize.Medium, FamilyOwnership.Owners);
        public static readonly BuildingInfo PeasantsHutSlvMedium = new BuildingInfo("Peasants' Hut", ProfessionInfo.Peasant, ProfessionInfo.WorkingSlave, BuildingSize.Medium, FamilyOwnership.Owners);
        public static readonly BuildingInfo PeasantsHutLarge = new BuildingInfo("Plantation", ProfessionInfo.Peasant, ProfessionInfo.Peasant, BuildingSize.Large, FamilyOwnership.Owners);
        public static readonly BuildingInfo PeasantsHutSlvLarge = new BuildingInfo("Plantation", ProfessionInfo.Peasant, ProfessionInfo.WorkingSlave, BuildingSize.Large, FamilyOwnership.Owners);

        public static readonly BuildingInfo HuntersHutMedium = new BuildingInfo("Hunters Hut", ProfessionInfo.Hunter, ProfessionInfo.Hunter, BuildingSize.Medium);
        public static readonly BuildingInfo HuntersHutLarge = new BuildingInfo("Hunters Lodge", ProfessionInfo.Hunter, ProfessionInfo.Hunter, BuildingSize.Large);

        public static readonly BuildingInfo HuntersHutSlvMedium = new BuildingInfo("Hunters Hut", ProfessionInfo.Hunter, ProfessionInfo.WorkingSlave, BuildingSize.Medium);
        public static readonly BuildingInfo HuntersHutSlvLarge = new BuildingInfo("Hunters Lodge", ProfessionInfo.Hunter, ProfessionInfo.WorkingSlave, BuildingSize.Large);

        public static readonly BuildingInfo MineMedium = new BuildingInfo("Quarry", ProfessionInfo.Miner, ProfessionInfo.Miner, BuildingSize.Medium);
        public static readonly BuildingInfo MineLarge = new BuildingInfo("Mine", ProfessionInfo.Miner, ProfessionInfo.Miner, BuildingSize.Large);

        public static readonly BuildingInfo MineSlvMedium = new BuildingInfo("Quarry", ProfessionInfo.Miner, ProfessionInfo.WorkingSlave, BuildingSize.Medium);
        public static readonly BuildingInfo MineSlvLarge = new BuildingInfo("Mine", ProfessionInfo.Miner, ProfessionInfo.WorkingSlave, BuildingSize.Large);

        public static readonly BuildingInfo LumberjacksHutMedium = new BuildingInfo("Lumberjacks Hut", ProfessionInfo.Lumberjack, ProfessionInfo.Lumberjack, BuildingSize.Medium);
        public static readonly BuildingInfo LumberjacksHutLarge = new BuildingInfo("Sawmill", ProfessionInfo.Lumberjack, ProfessionInfo.Lumberjack, BuildingSize.Large);

        public static readonly BuildingInfo LumberjacksHutSlvMedium = new BuildingInfo("Lumberjacks Hut", ProfessionInfo.Lumberjack, ProfessionInfo.WorkingSlave, BuildingSize.Medium);
        public static readonly BuildingInfo LumberjacksHutSlvLarge = new BuildingInfo("Sawmill", ProfessionInfo.Lumberjack, ProfessionInfo.WorkingSlave, BuildingSize.Large);

        public static readonly BuildingInfo ShamansHutSmall = new BuildingInfo("Shamans Hut", ProfessionInfo.Shaman, ProfessionInfo.Shaman, BuildingSize.Small);

        public static readonly BuildingInfo ChurchSmall = new BuildingInfo("Church", ProfessionInfo.Priest, ProfessionInfo.Priest, BuildingSize.Small);
        public static readonly BuildingInfo ChurchMedium = new BuildingInfo("Temple", ProfessionInfo.Priest, ProfessionInfo.Priest, BuildingSize.Medium);
        public static readonly BuildingInfo ChurchLarge = new BuildingInfo("Cathedral", ProfessionInfo.Priest, ProfessionInfo.Priest, BuildingSize.Large);

        public static readonly BuildingInfo TavernSmall = new BuildingInfo("Tavern", ProfessionInfo.Barman, ProfessionInfo.Barman, BuildingSize.Small);
        public static readonly BuildingInfo TavernSlvSmall = new BuildingInfo("Tavern", ProfessionInfo.Barman, ProfessionInfo.PleasureSlave, BuildingSize.Small);
        public static readonly BuildingInfo BarSmall = new BuildingInfo("Bar", ProfessionInfo.Barman, ProfessionInfo.Barman, BuildingSize.Small);
        public static readonly BuildingInfo BarSlvSmall = new BuildingInfo("Bar", ProfessionInfo.Barman, ProfessionInfo.PleasureSlave, BuildingSize.Small);
        public static readonly BuildingInfo NightClubMedium = new BuildingInfo("Night Club", ProfessionInfo.Barman, ProfessionInfo.Barman, BuildingSize.Medium);
        public static readonly BuildingInfo NightClubSlvMedium = new BuildingInfo("Night Club", ProfessionInfo.Barman, ProfessionInfo.PleasureSlave, BuildingSize.Medium);

        public static readonly BuildingInfo RoguesDenSmall = new BuildingInfo("Rogue's Den", ProfessionInfo.RogueLeader, ProfessionInfo.Rogue, BuildingSize.Small);
        public static readonly BuildingInfo RoguesDenMedium = new BuildingInfo("Rogue's Den", ProfessionInfo.RogueLeader, ProfessionInfo.Rogue, BuildingSize.Medium);

        public static readonly BuildingInfo GamblingSmall = new BuildingInfo("Gambling Den", ProfessionInfo.Gambler, ProfessionInfo.Gambler, BuildingSize.Small);
        public static readonly BuildingInfo GamblingMedium = new BuildingInfo("Casino", ProfessionInfo.Gambler, ProfessionInfo.Gambler, BuildingSize.Medium);

        public static readonly BuildingInfo BrothelMedium = new BuildingInfo("Brothel", ProfessionInfo.Prostitute, ProfessionInfo.Prostitute, BuildingSize.Medium);
        public static readonly BuildingInfo BrothelSlvMedium = new BuildingInfo("Brothel", ProfessionInfo.Slaver, ProfessionInfo.PleasureSlave, BuildingSize.Medium);

        public static readonly BuildingInfo StripClubSmall = new BuildingInfo("Strip Club", ProfessionInfo.Stripper, ProfessionInfo.Stripper, BuildingSize.Small);
        public static readonly BuildingInfo StripClubSlvSmall = new BuildingInfo("Strip Club", ProfessionInfo.Slaver, ProfessionInfo.PleasureSlave, BuildingSize.Small);

        public static readonly BuildingInfo CultureMeduim = new BuildingInfo("Concert Hall", ProfessionInfo.Composer, ProfessionInfo.Musician, BuildingSize.Medium);
        public static readonly BuildingInfo CultureLarge = new BuildingInfo("Conservatoire", ProfessionInfo.Composer, ProfessionInfo.Musician, BuildingSize.Large);

        public static readonly BuildingInfo TheatreMedium = new BuildingInfo("Theatre", ProfessionInfo.Playwright, ProfessionInfo.Actor, BuildingSize.Medium);
        public static readonly BuildingInfo CinemaLarge = new BuildingInfo("Film Studio", ProfessionInfo.Producer, ProfessionInfo.Actor, BuildingSize.Large);

        public static readonly BuildingInfo CircusMedium        = new BuildingInfo("Circus", ProfessionInfo.Gladiator, ProfessionInfo.Gladiator, BuildingSize.Medium);

        public static readonly BuildingInfo TraderMedium = new BuildingInfo("Merchant ship", ProfessionInfo.Captain, ProfessionInfo.Sailor, BuildingSize.Medium);
        public static readonly BuildingInfo TraderLarge = new BuildingInfo("Freighter", ProfessionInfo.Captain, ProfessionInfo.Sailor, BuildingSize.Large);

        public static readonly BuildingInfo TraderSlvMedium = new BuildingInfo("Merchant ship", ProfessionInfo.Captain, ProfessionInfo.WorkingSlave, BuildingSize.Medium);
        public static readonly BuildingInfo TraderSlvLarge = new BuildingInfo("Freighter", ProfessionInfo.Captain, ProfessionInfo.WorkingSlave, BuildingSize.Large);

        public static readonly BuildingInfo NavalAcademyHuge = new BuildingInfo("Nautical School", ProfessionInfo.Admiral, ProfessionInfo.Sailor, BuildingSize.Huge);
        public static readonly BuildingInfo NavalVessel = new BuildingInfo("Naval Vessel", ProfessionInfo.Captain, ProfessionInfo.Sailor, BuildingSize.Medium);

        public static readonly BuildingInfo TailorWorkshopSmall = new BuildingInfo("Tailoring Shop", ProfessionInfo.Tailor, ProfessionInfo.Tailor, BuildingSize.Small);
        public static readonly BuildingInfo TailorWorkshopSlvSmall = new BuildingInfo("Tailoring Shop", ProfessionInfo.Tailor, ProfessionInfo.WorkingSlave, BuildingSize.Small);

        public static readonly BuildingInfo JevellerWorkshopSmall = new BuildingInfo("Jewellery Workshop", ProfessionInfo.Jeveller, ProfessionInfo.Jeveller, BuildingSize.Small);
        public static readonly BuildingInfo JevellerWorkshopSlvSmall = new BuildingInfo("Jewellery Workshop", ProfessionInfo.Jeveller, ProfessionInfo.WorkingSlave, BuildingSize.Small);

        public static readonly BuildingInfo SmithySmall = new BuildingInfo("Smithy", ProfessionInfo.Blacksmith, ProfessionInfo.Blacksmith, BuildingSize.Small);
        public static readonly BuildingInfo SmithySlvSmall = new BuildingInfo("Smithy", ProfessionInfo.Blacksmith, ProfessionInfo.WorkingSlave, BuildingSize.Small);

        public static readonly BuildingInfo CarpentrySmall = new BuildingInfo("Carpentry Workshop", ProfessionInfo.Carpenter, ProfessionInfo.Carpenter, BuildingSize.Small);
        public static readonly BuildingInfo CarpentrySlvSmall = new BuildingInfo("Carpentry Workshop", ProfessionInfo.Carpenter, ProfessionInfo.WorkingSlave, BuildingSize.Small);

        public static readonly BuildingInfo IronworksSmall = new BuildingInfo("Ironworks", ProfessionInfo.Master, ProfessionInfo.Worker, BuildingSize.Medium);
        public static readonly BuildingInfo IronworksMedium = new BuildingInfo("Metallurgical Plant", ProfessionInfo.Master, ProfessionInfo.Worker, BuildingSize.Large);
        public static readonly BuildingInfo FurnitureSmall = new BuildingInfo("Woodworking Factory", ProfessionInfo.Master, ProfessionInfo.Worker, BuildingSize.Medium);
        public static readonly BuildingInfo FurnitureMedium = new BuildingInfo("Woodworking Factory", ProfessionInfo.Master, ProfessionInfo.Worker, BuildingSize.Large);
        public static readonly BuildingInfo ClothesFactorySmall = new BuildingInfo("Sewing Workshop", ProfessionInfo.Master, ProfessionInfo.Worker, BuildingSize.Medium);
        public static readonly BuildingInfo ClothesFactoryMedium = new BuildingInfo("Textile Factory", ProfessionInfo.Master, ProfessionInfo.Worker, BuildingSize.Large);
        public static readonly BuildingInfo JevellerWorkshopMedium = new BuildingInfo("Jewellery Factory", ProfessionInfo.Master, ProfessionInfo.Worker, BuildingSize.Large);

        public static readonly BuildingInfo IronworksSlvSmall = new BuildingInfo("Ironworks", ProfessionInfo.Slaver, ProfessionInfo.WorkingSlave, BuildingSize.Medium);
        public static readonly BuildingInfo IronworksSlvMedium = new BuildingInfo("Metallurgical Plant", ProfessionInfo.Slaver, ProfessionInfo.WorkingSlave, BuildingSize.Large);
        public static readonly BuildingInfo FurnitureSlvSmall = new BuildingInfo("Woodworking Factory", ProfessionInfo.Slaver, ProfessionInfo.WorkingSlave, BuildingSize.Medium);
        public static readonly BuildingInfo FurnitureSlvMedium = new BuildingInfo("Woodworking Factory", ProfessionInfo.Slaver, ProfessionInfo.WorkingSlave, BuildingSize.Large);
        public static readonly BuildingInfo ClothesFactorySlvSmall = new BuildingInfo("Sewing Workshop", ProfessionInfo.Slaver, ProfessionInfo.WorkingSlave, BuildingSize.Medium);
        public static readonly BuildingInfo ClothesFactorySlvMedium = new BuildingInfo("Textile Factory", ProfessionInfo.Slaver, ProfessionInfo.WorkingSlave, BuildingSize.Large);
        public static readonly BuildingInfo JevellerWorkshopSlvMedium = new BuildingInfo("Jewellery Factory", ProfessionInfo.Slaver, ProfessionInfo.WorkingSlave, BuildingSize.Large);

        public static readonly BuildingInfo PirateShip = new BuildingInfo("Pirate ship", ProfessionInfo.PirateLeader, ProfessionInfo.Pirate, BuildingSize.Small);

        public static readonly BuildingInfo BanditsBarracks     = new BuildingInfo("Barracks", ProfessionInfo.BanditLeader, ProfessionInfo.Bandit, BuildingSize.Small);

        public static readonly BuildingInfo BarracksSmall       = new BuildingInfo("Barracks", ProfessionInfo.Officer, ProfessionInfo.Soldier, BuildingSize.Small);
        public static readonly BuildingInfo BarracksHuge        = new BuildingInfo("Barracks", ProfessionInfo.General, ProfessionInfo.Soldier, BuildingSize.Huge);

        public static readonly BuildingInfo ScienceSmall        = new BuildingInfo("Laboratory", ProfessionInfo.Scientiest, ProfessionInfo.Scientiest, BuildingSize.Small);
        public static readonly BuildingInfo ScienceMedium       = new BuildingInfo("University", ProfessionInfo.Scientiest, ProfessionInfo.Scientiest, BuildingSize.Medium);
        public static readonly BuildingInfo ScienceLarge        = new BuildingInfo("Academy", ProfessionInfo.Scientiest, ProfessionInfo.Scientiest, BuildingSize.Large);

        public static readonly BuildingInfo SchoolSmall         = new BuildingInfo("School", ProfessionInfo.Teacher, ProfessionInfo.Teacher, BuildingSize.Small);
        public static readonly BuildingInfo SchoolMedium        = new BuildingInfo("College", ProfessionInfo.Teacher, ProfessionInfo.Teacher, BuildingSize.Medium);

        public static readonly BuildingInfo InnSmall = new BuildingInfo("Inn", ProfessionInfo.Innkeeper, ProfessionInfo.Innkeeper, BuildingSize.Small);
        public static readonly BuildingInfo InnSlvSmall = new BuildingInfo("Inn", ProfessionInfo.Innkeeper, ProfessionInfo.PleasureSlave, BuildingSize.Small);

        public static readonly BuildingInfo MarketSmall         = new BuildingInfo("Marketplace", ProfessionInfo.Merchant, ProfessionInfo.Merchant, BuildingSize.Small);
        public static readonly BuildingInfo MarketMedium        = new BuildingInfo("Bazaar", ProfessionInfo.Merchant, ProfessionInfo.Merchant, BuildingSize.Medium);

        public static readonly BuildingInfo CourtSmall          = new BuildingInfo("Courthouse", ProfessionInfo.Administrator, ProfessionInfo.Scribe, BuildingSize.Small);
        public static readonly BuildingInfo CourtMedium = new BuildingInfo("Administration", ProfessionInfo.Administrator, ProfessionInfo.Scribe, BuildingSize.Medium);

        public static readonly BuildingInfo AdministrationSmall = new BuildingInfo("Courthouse", ProfessionInfo.Administrator, ProfessionInfo.Clerk, BuildingSize.Small);
        public static readonly BuildingInfo AdministrationMedium = new BuildingInfo("Administration", ProfessionInfo.Administrator, ProfessionInfo.Clerk, BuildingSize.Medium);

        public static readonly BuildingInfo HotelSmall = new BuildingInfo("Motel", ProfessionInfo.Manager, ProfessionInfo.Servant, BuildingSize.Small);
        public static readonly BuildingInfo HotelMedium = new BuildingInfo("Hotel", ProfessionInfo.Manager, ProfessionInfo.Servant, BuildingSize.Medium);
        public static readonly BuildingInfo HotelLarge = new BuildingInfo("Resort", ProfessionInfo.Manager, ProfessionInfo.Servant, BuildingSize.Large);

        public static readonly BuildingInfo HotelSlvSmall = new BuildingInfo("Motel", ProfessionInfo.Manager, ProfessionInfo.Servant, BuildingSize.Small);
        public static readonly BuildingInfo HotelSlvMedium = new BuildingInfo("Hotel", ProfessionInfo.Manager, ProfessionInfo.Servant, BuildingSize.Medium);
        public static readonly BuildingInfo HotelSlvLarge = new BuildingInfo("Resort", ProfessionInfo.Manager, ProfessionInfo.Servant, BuildingSize.Large);

        public static readonly BuildingInfo OfficeSmall = new BuildingInfo("Company", ProfessionInfo.Manager, ProfessionInfo.Clerk, BuildingSize.Small);
        public static readonly BuildingInfo OfficeMedium = new BuildingInfo("Agency", ProfessionInfo.Manager, ProfessionInfo.Clerk, BuildingSize.Medium);
        public static readonly BuildingInfo OfficeLarge = new BuildingInfo("Corporation", ProfessionInfo.Manager, ProfessionInfo.Clerk, BuildingSize.Medium);

        public static readonly BuildingInfo CastleSmall = new BuildingInfo("Keep", ProfessionInfo.Noble, ProfessionInfo.Servant, BuildingSize.Medium, FamilyOwnership.Owners);
        public static readonly BuildingInfo EstateSmall = new BuildingInfo("Manor", ProfessionInfo.Noble, ProfessionInfo.Servant, BuildingSize.Medium, FamilyOwnership.Owners);
        public static readonly BuildingInfo MansionSmall = new BuildingInfo("Mansion", ProfessionInfo.Noble, ProfessionInfo.Servant, BuildingSize.Small, FamilyOwnership.Owners);
        public static readonly BuildingInfo MansionMedium = new BuildingInfo("Palace", ProfessionInfo.Noble, ProfessionInfo.Servant, BuildingSize.Medium, FamilyOwnership.Owners);

        public static readonly BuildingInfo CastleSlvSmall = new BuildingInfo("Keep", ProfessionInfo.Noble, ProfessionInfo.PleasureSlave, BuildingSize.Medium, FamilyOwnership.Owners);
        public static readonly BuildingInfo EstateSlvSmall = new BuildingInfo("Manor", ProfessionInfo.Noble, ProfessionInfo.PleasureSlave, BuildingSize.Medium, FamilyOwnership.Owners);
        public static readonly BuildingInfo MansionSlvSmall = new BuildingInfo("Mansion", ProfessionInfo.Noble, ProfessionInfo.PleasureSlave, BuildingSize.Small, FamilyOwnership.Owners);
        public static readonly BuildingInfo MansionSlvMedium = new BuildingInfo("Palace", ProfessionInfo.Noble, ProfessionInfo.PleasureSlave, BuildingSize.Medium, FamilyOwnership.Owners);

        public static readonly BuildingInfo MedicineSmall = new BuildingInfo("Chemist Shop", ProfessionInfo.Chemist, ProfessionInfo.Chemist, BuildingSize.Small);
        public static readonly BuildingInfo MedicineMedium      = new BuildingInfo("Clinic", ProfessionInfo.Doctor, ProfessionInfo.Nurce, BuildingSize.Medium);
        public static readonly BuildingInfo MedicineLarge       = new BuildingInfo("Hospital", ProfessionInfo.Doctor, ProfessionInfo.Nurce, BuildingSize.Large);

        public static readonly BuildingInfo AlchemyShopSmall    = new BuildingInfo("Alchemy Shop", ProfessionInfo.Alchemist, ProfessionInfo.Alchemist, BuildingSize.Small);
      
        public static readonly BuildingInfo MagicSmall          = new BuildingInfo("Mage Tower", ProfessionInfo.Mage, ProfessionInfo.Mage, BuildingSize.Small);
        public static readonly BuildingInfo MagicMedium         = new BuildingInfo("Magic University", ProfessionInfo.Mage, ProfessionInfo.Mage, BuildingSize.Medium);
        public static readonly BuildingInfo MagicLarge          = new BuildingInfo("Magic Academy", ProfessionInfo.Mage, ProfessionInfo.Mage, BuildingSize.Large);

        #endregion

        public string m_sName;
        public ProfessionInfo m_pOwner;
        public ProfessionInfo m_pWorkers;

        public BuildingSize m_eSize;

        public FamilyOwnership m_eOwnership;

        public BuildingInfo(string sName, ProfessionInfo pOwner, ProfessionInfo pWorkers, BuildingSize eSize)
            :this(sName, pOwner, pWorkers, eSize, FamilyOwnership.None)
        { }

        public BuildingInfo(string sName, ProfessionInfo pOwner, ProfessionInfo pWorkers, BuildingSize eSize, FamilyOwnership eOwnership)
        {
            m_sName = sName;
            m_pOwner = pOwner;
            m_pWorkers = pWorkers;

            m_eSize = eSize;

            m_eOwnership = eOwnership;
        }

        public int OwnersCount
        {
            get
            {
                int iOwnersCount = 1;
                switch (m_eSize)
                {
                    case BuildingSize.Small:
                        iOwnersCount = 1;
                        break;
                    case BuildingSize.Medium:
                        iOwnersCount = 3;
                        break;
                    case BuildingSize.Large:
                        iOwnersCount = 5;
                        break;
                    case BuildingSize.Huge:
                        iOwnersCount = 10;
                        break;
                }

                return iOwnersCount;
            }
        }

        public int WorkersCount
        {
            get
            {
                int iWorkersCount = 0;
                switch (m_eSize)
                {
                    case BuildingSize.Small:
                        iWorkersCount = 3;
                        break;
                    case BuildingSize.Medium:
                        iWorkersCount = 15;
                        break;
                    case BuildingSize.Large:
                        iWorkersCount = 50;
                        break;
                    case BuildingSize.Huge:
                        iWorkersCount = 200;
                        break;
                }

                return iWorkersCount;
            }
        }

        public override string ToString()
        {
            return m_sName + " (" + m_eSize.ToString() + ")";
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
        public Building(Settlement pSettlement, BuildingInfo pInfo, bool bCapital = false)
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
            get
            {
                ValuedString[] profM = { new ValuedString(pInfo.m_sOwnerM, pInfo.m_iRank) };
                ValuedString[] profF = { new ValuedString(pInfo.m_sOwnerF, pInfo.m_iRank) };
                Opponent pFounder = new Opponent(this, m_pState.GetRandomRace(State.DwellersCathegory.MajorRaces),
                    profM, profF, true, null);
                m_cDwellers.Add(pFounder);

                int iPop = Rnd.Get(5);
                for (int i = 0; i < iPop; i++)
                int iOwnersCount = 1;
                switch (m_eSize)
                {
                    Opponent pPretendent = new Opponent(this, m_pState.GetRandomRace(State.DwellersCathegory.MajorRaces),
                        profM, profF, true, pFounder);
                    m_cDwellers.Add(pPretendent);
                    case BuildingSize.Small:
                        iOwnersCount = 1;
                        break;
                    case BuildingSize.Medium:
                        iOwnersCount = 3;
                        break;
                    case BuildingSize.Large:
                        iOwnersCount = 5;
                        break;
                    case BuildingSize.Huge:
                        iOwnersCount = 10;
                        break;
                }

                return iOwnersCount;
            }
             */
        }

        public override string ToString()
        {
            return m_pInfo.m_sName;
        }
    }
}
