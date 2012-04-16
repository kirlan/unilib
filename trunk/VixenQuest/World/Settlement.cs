using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using NameGen;
using VixenQuest.People;

namespace VixenQuest.World
{
    class SettlementInfo
    {
        public string m_sName;
        public int m_iMinPop;
        public int m_iDeltaPop;
        public int m_iMaxProfessionRank;
        public int m_iMinBuildingsCount;
        public int m_iDeltaBuildingsCount;
        public BuildingInfo m_pMainBuilding;

        public SettlementInfo(string sName, int iMinPop, int iDeltaPop, int iMaxProfessionRank, int iMinBuildingsCount, int iDeltaBuildingsCount, BuildingInfo pMainBuilding)
        {
            m_sName = sName;
            m_iMinPop = iMinPop;
            m_iDeltaPop = iDeltaPop;
            m_iMaxProfessionRank = iMaxProfessionRank;
            m_iMinBuildingsCount = iMinBuildingsCount;
            m_iDeltaBuildingsCount = iDeltaBuildingsCount;
            m_pMainBuilding = pMainBuilding;
        }
    }

    public class Settlement : Location
    {
        private static ValuedString[] m_aProfessionM = 
        {
            //lowest ranks - everybody could command them
            new ValuedString("slave", 0),
            new ValuedString("servant", 1),
            new ValuedString("peasant", 1),
            //still low, but they have mo masters
            new ValuedString("stranger", 2),
            new ValuedString("traveller", 2),
            //small, but own buisiness
            new ValuedString("shopkeeper", 3),
            new ValuedString("hunter", 3),
            new ValuedString("ranger", 3),
            new ValuedString("butcher", 3),
            //crime grunts
            new ValuedString("rogue", 4),
            new ValuedString("bandit", 4),
            new ValuedString("pirate", 4),
            new ValuedString("burglar", 4),
            //city crime
            new ValuedString("thief", 5),
            //well-paid workers
            new ValuedString("masturbator", 6),
            //common adventurers
            new ValuedString("fighter", 7),
            new ValuedString("monk", 7),
            new ValuedString("warden", 7),
            new ValuedString("mage apprentice", 7),
            new ValuedString("monk", 7),
            new ValuedString("bruiser", 7),
            new ValuedString("guardian", 7),
            new ValuedString("warrior", 7),
            //professional adventurers
            new ValuedString("ninja", 8),
            new ValuedString("spy", 8),
            new ValuedString("sentinel", 8),
            new ValuedString("slut-hunter", 8),
            new ValuedString("head-hunter", 8),
            new ValuedString("bitch-hunter", 8),
            //mages, clerics, etc.
            new ValuedString("witch-hunter", 9),
            new ValuedString("demon-hunter", 9),
            new ValuedString("necromancer", 9),
            new ValuedString("eromancer", 9),
            new ValuedString("summoner", 9),
            new ValuedString("sage", 9),
            new ValuedString("shaman", 9),
            new ValuedString("mage", 9),
            new ValuedString("prophet", 9),
            //nobile & rich
            new ValuedString("knight", 10),
            //officials
            new ValuedString("vizier", 11),
            new ValuedString("noble", 12),
        };

        private static ValuedString[] m_aProfessionF = 
        {
            //lowest ranks - everybody could command them
            new ValuedString("slavegirl", 0),
            new ValuedString("peasant maid", 1),
            //still low, but they have mo masters
            new ValuedString("maid", 2),
            new ValuedString("schoolgirl", 2),
            new ValuedString("housemaid", 2),
            new ValuedString("housewife", 2),
            new ValuedString("chick", 2),
            //small, but own buisiness
            new ValuedString("huntress", 3),
            //crime grunts
            new ValuedString("whore", 4),
            new ValuedString("slut", 4),
            //new ValuedString("rogue", 4),
            new ValuedString("burglaress", 4),
            //city crime
            new ValuedString("thief", 5),
            new ValuedString("prostitute", 5),
            new ValuedString("bitch", 5),
            //well-paid workers
            new ValuedString("dancer", 6),
            new ValuedString("bride", 6),
            //common adventurers
            new ValuedString("masturbatrix", 7),
            new ValuedString("wardeness", 7),
            new ValuedString("wench", 7),
            //professional adventurers
            //new ValuedString("sentinel", 8),
            new ValuedString("head-huntress", 8),
            new ValuedString("bitch-huntress", 8),
            //mages, clerics, etc.
            new ValuedString("witch-huntress", 9),
            new ValuedString("demon-huntress", 9),
            new ValuedString("witch", 9),
            new ValuedString("sorceress", 9),
            new ValuedString("prophetess", 9),
            new ValuedString("necromantress", 9),
            new ValuedString("enchantress", 9),
            new ValuedString("shamaness", 9),
            //nobile & rich
            new ValuedString("lady knight", 10),
            //officials
            new ValuedString("noble lady", 12),
        };
        
        public enum Size
        { 
            Hamlet,
            Village,
            Town,
            City,
            Capital,
            Methropoly
        }

        private static Dictionary<Size, SettlementInfo> m_cInfo = new Dictionary<Size, SettlementInfo>();

        internal static Dictionary<Size, SettlementInfo> Info
        {
            get 
            {
                if (m_cInfo.Count == 0)
                {
                    m_cInfo[Size.Hamlet] = new SettlementInfo("Hamlet", 5, 10, 2, 0, 1, null);
                    m_cInfo[Size.Village] = new SettlementInfo("Village", 10, 20, 3, 0, 3, new BuildingInfo("Village hall", "Elder", "Elder", 3));
                    m_cInfo[Size.Town] = new SettlementInfo("Town", 20, 40, 7, 2, 5, new BuildingInfo("Fort", "Baron", "Baroness", 7));
                    m_cInfo[Size.City] = new SettlementInfo("City", 40, 80, 14, 5, 7, new BuildingInfo("Castle", "Count", "Countess", 14));
                    m_cInfo[Size.Capital] = new SettlementInfo("City", 40, 80, 15, 5, 10, new BuildingInfo("Palace", "Lord", "Lady", 15));
                    m_cInfo[Size.Methropoly] = new SettlementInfo("City", 40, 80, 16, 5, 10, new BuildingInfo("Citadel", "Lord", "Lady", 16));
                }
                return Settlement.m_cInfo; 
            }
        }

        public Land m_pLand;

        public List<Building> m_cBuildings = new List<Building>();

        public Size m_eSize;

        public Settlement(Land pLand, Size eSize, bool bCapital)
            : base(pLand.m_pWorld)
        {
            m_pState = pLand.m_pState;
            m_eSize = eSize;
            m_pLand = pLand;

            AddPopulation(Info[eSize].m_iMinPop + Rnd.Get(Info[eSize].m_iDeltaPop), Settlement.Info[eSize].m_iMaxProfessionRank, m_pLand.m_pState.m_iTier * m_pState.m_iTier);

            m_sName = Info[eSize].m_sName + " " + NameGenerator.GetAbstractName();

            int iBuildingsCount = Info[eSize].m_iMinBuildingsCount + Rnd.Get(Info[eSize].m_iDeltaBuildingsCount);
            for (int i = 0; i < iBuildingsCount; i++)
            {
                Building pNewBuilding = new Building(this);
                m_cBuildings.Add(pNewBuilding);
            }

            if (Info[eSize].m_pMainBuilding != null && (bCapital || Rnd.OneChanceFrom(2)))
            {
                Building pNewBuilding = new Building(this, Info[eSize].m_pMainBuilding, bCapital);
                m_cBuildings.Add(pNewBuilding);
            }
        }

        private static Dictionary<State.DwellersCathegory, int> m_cRaces = new Dictionary<State.DwellersCathegory, int>();

        public override Dictionary<State.DwellersCathegory, int> Races
        {
            get
            {
                if (m_cRaces.Count == 0)
                {
                    m_cRaces[State.DwellersCathegory.WildAnimals] = 0;
                    m_cRaces[State.DwellersCathegory.DomesticAnimals] = 1;
                    m_cRaces[State.DwellersCathegory.SacredAnimals] = 0;
                    m_cRaces[State.DwellersCathegory.LesserRaces] = 1;
                    m_cRaces[State.DwellersCathegory.MajorRaces] = 2;
                }
                return m_cRaces;
            }
        }

        public override ValuedString[] ProfessionM
        {
            get { return m_aProfessionM; }
        }

        public override ValuedString[] ProfessionF
        {
            get { return m_aProfessionF; }
        }

        List<Opponent> m_cAvailableOpponents = new List<Opponent>();

        public override Opponent[] AvailableOpponents()
        {
            if (m_cAvailableOpponents.Count == 0)
            {
                m_cAvailableOpponents.AddRange(m_cDwellers);
                foreach (Building bld in m_cBuildings)
                    m_cAvailableOpponents.AddRange(bld.m_cDwellers);
            }

            return m_cAvailableOpponents.ToArray();
        }

        public override Location NextStepTo(Location pTarget)
        {
            if (this == pTarget)
                return null;

            if (pTarget is Building)
            {
                Location pNextStep = NextStepTo((pTarget as Building).m_pSettlement);
                if (pNextStep == null)
                    return pTarget;

                return pNextStep; 
            }

            return m_pLand;
        }
    }
}
