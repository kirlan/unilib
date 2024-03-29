﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using VixenQuest.People;

namespace VixenQuest.World
{
    public class BuildingInfo
    {
        public string m_sName;
        public string m_sOwnerM;
        public string m_sOwnerF;
        public int m_iRank;

        public    BuildingInfo(string sName, string sOwnerM, string sOwnerF, int iRank)
        {
            m_sName = sName;
            m_sOwnerM = sOwnerM;
            m_sOwnerF = sOwnerF;
            m_iRank = iRank;
        }
    }

    public class Building : Location
    {
        public static BuildingInfo[] m_aInfo = 
        {
            new BuildingInfo("Inn", "innkeeper", "matron", 2),
            new BuildingInfo("Bar", "barman", "barmaid", 3),
            new BuildingInfo("Church", "priest", "priestess", 3),
            new BuildingInfo("Hideout", "burglar", "burglaress", 4),
            new BuildingInfo("Hideout", "thief", "thief", 5),
            new BuildingInfo("Hideout", "rogue", "whore", 4),
            new BuildingInfo("Hideout", "bandit", "slut", 4),
            new BuildingInfo("Brothel", "pimp", "prostitute", 5),
            new BuildingInfo("Brothel", "pimp", "bitch", 5),
            new BuildingInfo("Tavern", "troubadour", "dancer", 6),
            new BuildingInfo("School", "teacher", "teacher", 6),
            new BuildingInfo("Garden", "minstrel", "geisha", 6),
            new BuildingInfo("Bazaar", "merchant", "booth babe", 6),
            new BuildingInfo("Fair", "merchant", "booth babe", 6),
            new BuildingInfo("Market", "merchant", "booth babe", 6),
            new BuildingInfo("Slave Market", "slaver", "mistress", 6),
            new BuildingInfo("Theatre", "actor", "actress", 6),
            new BuildingInfo("Prison", "executioner", "witch", 6),
            new BuildingInfo("Prison", "deflorator", "witch", 6),
            new BuildingInfo("Gates", "soldier", "wench", 7),
            new BuildingInfo("Barracks", "soldier", "wench", 7),
            new BuildingInfo("Circus", "gladiator", "gladiatrix", 8),
            new BuildingInfo("Assassins' Guild", "assasin", "assasiness", 8),
            new BuildingInfo("Temple", "prophet", "prophetess", 9),
            new BuildingInfo("Prison", "inquisitor", "inquisitrix", 9),
            new BuildingInfo("Mansion", "playboy", "courtesan", 10),
            new BuildingInfo("Cathedral", "bishop", "abbess", 11),
        };

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
            new ValuedString("pirate", 4),
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
            new ValuedString("cleric", 9),
            new ValuedString("sage", 9),
            new ValuedString("shaman", 9),
            new ValuedString("mage", 9),
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
            //new ValuedString("rogue", 4),
            //well-paid workers
            new ValuedString("bride", 6),
            //common adventurers
            new ValuedString("masturbatrix", 7),
            new ValuedString("wardeness", 7),
            //professional adventurers
            //new ValuedString("sentinel", 8),
            new ValuedString("head-huntress", 8),
            new ValuedString("bitch-huntress", 8),
            //mages, clerics, etc.
            new ValuedString("witch-huntress", 9),
            new ValuedString("demon-huntress", 9),
            new ValuedString("sorceress", 9),
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

        public Settlement m_pSettlement;

        public BuildingInfo m_pInfo;

        /// <summary>
        /// Центральное здание
        /// </summary>
        /// <param name="pSettlement">поселение</param>
        /// <param name="pInfo">шаблон здания</param>
        /// <param name="bCapital">в этом здании живёт местный правитель (false) или глава всего государства (true)</param>
        public Building(Settlement pSettlement, BuildingInfo pInfo, bool bCapital)
            : base(pSettlement.m_pWorld)
        {
            m_pState = pSettlement.m_pState;
            m_pSettlement = pSettlement;
            m_pInfo = pInfo;

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
        }

        /// <summary>
        /// Произвольное здание в поселении
        /// </summary>
        /// <param name="pSettlement"></param>
        public Building(Settlement pSettlement)
            : base(pSettlement.m_pWorld)
        {
            m_pState = pSettlement.m_pState;
            m_pSettlement = pSettlement;

            AddPopulation(1 + Rnd.Get(5), Settlement.Info[pSettlement.m_eSize].m_iMaxProfessionRank, pSettlement.m_pLand.m_pState.m_iTier * pSettlement.m_pState.m_iTier);

            do
            {
                int iPlace = Rnd.Get(m_aInfo.Length);
                m_pInfo = m_aInfo[iPlace];
            }
            while(m_pInfo.m_iRank > Settlement.Info[pSettlement.m_eSize].m_iMaxProfessionRank);

            GenerateName();

            int iPop = 1 + Rnd.Get(5);
            ValuedString[] profM = { new ValuedString(m_pInfo.m_sOwnerM, m_pInfo.m_iRank) };
            ValuedString[] profF = { new ValuedString(m_pInfo.m_sOwnerF, m_pInfo.m_iRank) };
            for (int i = 0; i < iPop; i++)
            {
                Opponent pPretendent = new Opponent(this, m_pState.GetRandomRace(State.DwellersCathegory.MajorRaces),
                    profM, profF, true, null);
                m_cDwellers.Add(pPretendent);
            }
        }

        private void GenerateName()
        {
            int iEpithet = Rnd.Get(m_aEpithet.Length);
            int iDescription = Rnd.Get(m_aDescription.Length);

            int variant = Rnd.Get(3);

            switch (variant)
            {
                case 0:
                    {
                        m_sName = m_aEpithet[iEpithet];
                        m_sName += m_pInfo.m_sName;
                    }
                    break;
                case 1:
                    {
                        m_sName = m_pInfo.m_sName;
                        m_sName += m_aDescription[iDescription];
                    }
                    break;
                default:
                    {
                        m_sName = m_aEpithet[iEpithet];
                        m_sName += m_pInfo.m_sName;
                        m_sName += m_aDescription[iDescription];
                    }
                    break;
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

        public override Location NextStepTo(Location pTarget)
        {
            if (this == pTarget)
                return null;

            return m_pSettlement;
        }

        public override Opponent[] AvailableOpponents()
        {
            return m_cDwellers.ToArray();
        }
    }
}
