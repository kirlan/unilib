using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace StarMap
{
    public class CUniverse
    {
        public List<CWorld> m_cWorlds = new List<CWorld>();
        public int m_iWidth;
        public int m_iHeight;

        public List<CRace> m_cRaces = new List<CRace>();

        public int m_iAge = 0;

        public CUniverse()
        {
            CStarsMap pStarsMap = new CStarsMap(40, 40, 25);
            foreach (CStar pStar in pStarsMap.m_cStars)
            {
                m_cWorlds.Add(new CWorld(this, pStar));
            }
            m_iWidth = pStarsMap.m_iWidth;
            m_iHeight = pStarsMap.m_iHeight;

            CRace.Counter = 0;

            int iWorldsCount = m_iWidth * m_iHeight; //400
            int iUninhabitedWorldsCount = iWorldsCount / 4; //200
            int iUnSentientWorldsCount = iUninhabitedWorldsCount / 4;   //100

            int iTU0_WorldsCount = iUnSentientWorldsCount / 4;  //50
            //iTU0_WorldsCount += Rnd.Get(iTU0_WorldsCount);
            for (int i = 0; i < iTU0_WorldsCount; i++)
                AddRace(CRace.TechLevel.lv0_Barbarian);

            int iTU1_WorldsCount = iTU0_WorldsCount / 2;    //25
            iTU1_WorldsCount = Rnd.Get(iTU0_WorldsCount);
            for (int i = 0; i < iTU1_WorldsCount; i++)
                AddRace(CRace.TechLevel.lv1_Medieval);

            int iTU2_WorldsCount = iTU1_WorldsCount / 2;    //12
            iTU2_WorldsCount = Rnd.Get(iTU1_WorldsCount);
            for (int i = 0; i < iTU2_WorldsCount; i++)
                AddRace(CRace.TechLevel.lv2_Industrial);

            int iTU3_WorldsCount = iTU2_WorldsCount / 2;    //6
            iTU3_WorldsCount = Rnd.Get(iTU2_WorldsCount);
            for (int i = 0; i < iTU3_WorldsCount; i++)
                AddRace(CRace.TechLevel.lv3_Humans);

            int iTU4_WorldsCount = iTU3_WorldsCount / 2;    //3
            iTU4_WorldsCount = Rnd.Get(iTU3_WorldsCount);
            for (int i = 0; i < iTU4_WorldsCount; i++)
                AddRace(CRace.TechLevel.lv4_Heirs);

            int iTU5_WorldsCount = iTU4_WorldsCount / 2;    //1
            iTU5_WorldsCount = Rnd.Get(iTU4_WorldsCount);
            for (int i = 0; i < iTU5_WorldsCount; i++)
                AddRace(CRace.TechLevel.lv5_AncientEsses);

            int iTU6_WorldsCount = iTU5_WorldsCount / 2;
            iTU6_WorldsCount = Rnd.Get(iTU5_WorldsCount);
            for (int i = 0; i < iTU6_WorldsCount; i++)
                AddRace(CRace.TechLevel.lv6_Magistrate);

            int iTU7_WorldsCount = iTU6_WorldsCount / 2;
            iTU7_WorldsCount = Rnd.Get(iTU6_WorldsCount);
            for (int i = 0; i < iTU7_WorldsCount; i++)
                AddRace(CRace.TechLevel.lv7_AncientAnnan);

            int iTU8_WorldsCount = iTU7_WorldsCount / 2;
            iTU8_WorldsCount = Rnd.Get(iTU7_WorldsCount);
            for (int i = 0; i < iTU8_WorldsCount; i++)
                AddRace(CRace.TechLevel.lv8_ModernEsses);

            int iTU9_WorldsCount = iTU8_WorldsCount / 2;
            iTU9_WorldsCount = Rnd.Get(iTU8_WorldsCount);
            for (int i = 0; i < iTU9_WorldsCount; i++)
                AddRace(CRace.TechLevel.lv9_DarkDragons);
        }

        public CUniverse(CStarsMap pStarsMap)
        {
            foreach (CStar pStar in pStarsMap.m_cStars)
            {
                m_cWorlds.Add(new CWorld(this, pStar));
            }
            m_iWidth = pStarsMap.m_iWidth;
            m_iHeight = pStarsMap.m_iHeight;
        }

        private CWorld FindEmptyHabitableWorld()
        { 
            CWorld pFound = null;
            do
            {
                int index = Rnd.Get(m_cWorlds.Count);
                if (m_cWorlds[index].m_bHaveHabitablePlanets &&
                    m_cWorlds[index].m_pMajorRace == null &&
                    m_cWorlds[index].m_cIncoming.Count == 0)
                {
                    pFound = m_cWorlds[index];
                }
            }
            while (pFound == null);

            return pFound;
        }

        public void AddRace(CRace.TechLevel eLevel)
        { 
            CWorld pHomeWorld = FindEmptyHabitableWorld();
            CRace pNewRace = new CRace(pHomeWorld, eLevel);

            m_cRaces.Add(pNewRace);
        }

        public void Advance()
        {
            foreach (CRace pRace in m_cRaces)
            {
                pRace.Advance();
            }

            m_iAge += 10;
        }
    }
}
