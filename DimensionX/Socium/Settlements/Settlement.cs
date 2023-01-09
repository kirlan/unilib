using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NameGen;
using Random;
using Socium.Nations;

namespace Socium.Settlements
{
    public enum SettlementSize
    {
        Hamlet,
        Village,
        Fort,
        Town,
        City,
        Capital
    }

    public enum SettlementSpeciality
    {
        None,
        Fishers,
        Lumberjacks,
        Hunters,
        Miners,
        Peasants,
        Farmers,
        Raiders,
        Pirates,
        Military,
        Naval,
        Factory,
        Artisans,
        Jevellers,
        Tailors,
        Resort,
        Cultural,
        Religious,
        Gambling,
        MilitaryAcademy,
        NavalAcademy,
        ArtsAcademy,
        SciencesAcademy
    }

    public class SettlementInfo
    {
        public string Name { get; }
        public int MinPop { get; }
        public int DeltaPop { get; }
        public int MinBuildingsCount { get; }
        public int DeltaBuildingsCount { get; }
        public BuildingInfo MainBuilding { get; }
        public SettlementSize Size { get; }

        public SettlementInfo(SettlementSize eSize, string sName, int iMinPop, int iDeltaPop, BuildingInfo pMainBuilding)
        {
            Size = eSize;
            Name = sName;
            MinPop = iMinPop;
            DeltaPop = iDeltaPop;

            switch (Size)
            {
                case SettlementSize.Hamlet:
                    MinBuildingsCount = 5;
                    DeltaBuildingsCount = 1;
                    break;
                case SettlementSize.Village:
                    MinBuildingsCount = 7;
                    DeltaBuildingsCount = 2;
                    break;
                case SettlementSize.Town:
                    MinBuildingsCount = 15;
                    DeltaBuildingsCount = 2;
                    break;
                case SettlementSize.City:
                    MinBuildingsCount = 20;
                    DeltaBuildingsCount = 2;
                    break;
                case SettlementSize.Capital:
                    MinBuildingsCount = 30;
                    DeltaBuildingsCount = 2;
                    break;
                case SettlementSize.Fort:
                    MinBuildingsCount = 10;
                    DeltaBuildingsCount = 1;
                    break;
            }
            MainBuilding = pMainBuilding;
        }
    }

    public class Settlement
    {
        private static readonly Dictionary<SettlementSize, SettlementInfo> m_cInfo = new Dictionary<SettlementSize, SettlementInfo>();

        internal static Dictionary<SettlementSize, SettlementInfo> Info
        {
            get
            {
                if (m_cInfo.Count == 0)
                {
                    m_cInfo[SettlementSize.Hamlet] = new SettlementInfo(SettlementSize.Hamlet, "Hamlet", 5, 10, null);
                    m_cInfo[SettlementSize.Village] = new SettlementInfo(SettlementSize.Village, "Village", 10, 20, new BuildingInfo("Village hall", ProfessionInfo.Elder, ProfessionInfo.Elder, BuildingSize.Unique));
                    m_cInfo[SettlementSize.Town] = new SettlementInfo(SettlementSize.Town, "Town", 20, 40, new BuildingInfo("Town hall", ProfessionInfo.Mayor, ProfessionInfo.Mayor, BuildingSize.Unique));
                    m_cInfo[SettlementSize.City] = new SettlementInfo(SettlementSize.City, "City", 40, 80, new BuildingInfo("City hall", ProfessionInfo.Mayor, ProfessionInfo.Mayor, BuildingSize.Unique));
                    m_cInfo[SettlementSize.Capital] = new SettlementInfo(SettlementSize.Capital, "City", 40, 80, new BuildingInfo("City hall", ProfessionInfo.Mayor, ProfessionInfo.Mayor, BuildingSize.Unique));
                    m_cInfo[SettlementSize.Fort] = new SettlementInfo(SettlementSize.Fort, "Fort", 7, 5, new BuildingInfo("Headquarters", ProfessionInfo.General, ProfessionInfo.General, BuildingSize.Unique));
                }
                return m_cInfo;
            }
        }

        public SettlementInfo Profile { get; }
        public SettlementSpeciality Speciality { get; set; } = SettlementSpeciality.None;

        public string Name { get; }

        /// <summary>
        /// Возраст руин. Если 0, значит ещё не руины.
        /// </summary>
        public int RuinsAge { get; private set; } = 0;
        private Nation Nation { get; }

        public int TechLevel { get; }
        public int MagicLimit { get; }

        public bool Capital { get; }

        public List<Building> Buildings { get; } = new List<Building>();

        public Settlement(SettlementInfo pInfo, Nation pNation, int iTech, int iMagic, bool bCapital, bool bFast)
        {
            Profile = pInfo;
            Nation = pNation;

            if (bFast)
            {
                Name = "Noname";
            }
            else
            {
                if (pInfo.Size == SettlementSize.Hamlet || pInfo.Size == SettlementSize.Village)
                    Name = Nation.Race.Language.RandomVillageName();
                else
                    Name = Nation.Race.Language.RandomTownName();
            }

            TechLevel = iTech;
            MagicLimit = iMagic;

            Capital = bCapital;
        }

        public bool Ruin()
        {
            RuinsAge++;

            var aBuildings = Buildings.ToArray();
            foreach (var pBuilding in aBuildings)
            {
                if (Rnd.OneChanceFrom(2))
                    Buildings.Remove(pBuilding);
            }

            return Buildings.Count == 0;
        }

        public override string ToString()
        {
            string sRuinsAge = "";
            switch (RuinsAge)
            {
                case 0:
                    break;
                case 1:
                    sRuinsAge = "ruins of ";
                    break;
                case 2:
                    sRuinsAge = "ancient ruins of ";
                    break;
                default:
                    sRuinsAge = "prehistoric ruins of ";
                    break;
            }

            string sRuinsName = string.Format("{2} {0} {1}", Profile.Name, Name, Speciality);
            if (RuinsAge > 0)
                sRuinsName = string.Format("{2} ({0}) {1}", Nation.ProtoSociety.Name, Profile.Name, Nation.Race.Name).ToLower();

            return string.Format("{1}{0} (T{2}M{3})", sRuinsName, sRuinsAge, TechLevel, MagicLimit);
        }
    }
}
