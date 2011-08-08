using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace Socium.Settlements
{
    public class BuildingInfo
    {
        public string m_sName;
        public string m_sOwnerM;
        public string m_sOwnerF;
        public int m_iRank;

        public BuildingInfo(string sName, string sOwnerM, string sOwnerF, int iRank)
        {
            m_sName = sName;
            m_sOwnerM = sOwnerM;
            m_sOwnerF = sOwnerF;
            m_iRank = iRank;
        }
    }

    public class Building
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
        public Building(Settlement pSettlement)
        {
            m_pSettlement = pSettlement;

            //AddPopulation(1 + Rnd.Get(5), Settlement.Info[pSettlement.m_eSize].m_iMaxProfessionRank, pSettlement.m_pLand.m_pState.m_iTier * pSettlement.m_pState.m_iTier);

            do
            {
                int iPlace = Rnd.Get(m_aInfo.Length);
                m_pInfo = m_aInfo[iPlace];
            }
            while (m_pInfo.m_iRank > pSettlement.m_pInfo.m_iMaxProfessionRank);

            /*
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
             */
        }

        public override string ToString()
        {
            return m_pInfo.m_sName;
        }
    }
}
