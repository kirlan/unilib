using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleVectors;
using Random;

namespace StarMap
{
    public class CWorld: CStar
    {
        public enum WorldType
        {
            AdvancedWorld,
            Colony,
            AdvancingWorld,
            ClosedWorld,
            PrimitiveWorld,
            EmptyWorld
        }

        public enum WorldProduction
        {
            Academy,
            Resort,
            Smith,
            Resource,
            Nothing
        }

        public enum Resources
        { 
            Gems,
            Ore,
            Grain
        }

        public CUniverse m_pMap;

        public CRace m_pMajorRace;
        public CRace m_pLesserRace;
        //public CWorld m_pTradingRoute;
        public WorldType m_eWorldType = WorldType.EmptyWorld;
        public WorldProduction m_eProduction = WorldProduction.Nothing;

        public List<CRoute> m_cIncoming = new List<CRoute>();
        public List<CRoute> m_cOutcoming = new List<CRoute>();

        public Dictionary<Resources, int> m_cResources = new Dictionary<Resources, int>();

        public int m_iPotential;
        public double m_fWealth = 1;

        public CWorld(CUniverse pMap, CStar pStar)
            : base(pStar.m_iX, pStar.m_iY, pStar.m_bHaveHabitablePlanets)
        {
            if (pMap == null)
                throw new ArgumentNullException("pMap");

            if (pStar == null)
                throw new ArgumentNullException("pStar");

            m_iPotential = 1 + Rnd.Get(10);

            if (!m_bHaveHabitablePlanets)
                m_iPotential = 0;

            m_cResources[Resources.Gems] = 0;
            m_cResources[Resources.Grain] = 0;
            m_cResources[Resources.Ore] = 0;

            //ФФВЫА

            for (int i = 0; i < m_iPotential; i++)
            {
                switch (Rnd.Get(3))
                {
                    case 0:
                        m_cResources[Resources.Gems]++;
                        break;
                    case 1:
                        m_cResources[Resources.Grain]++;
                        break;
                    case 2:
                        m_cResources[Resources.Ore]++;
                        break;
                }
            }

            m_pMap = pMap;
            m_sName = string.Format("[{0}:{1}] ({2})", m_iX, m_iY, m_iPotential);
        }

        public override string ToString()
        {
            return m_sName;
        }

        public string m_sName;

        public void ClaimAsHomeWorld(CRace pRace)
        {
            if (pRace == null)
                throw new ArgumentNullException();

            m_pMajorRace = pRace;

            if (pRace.m_eLevel == CRace.TechLevel.lv0_Barbarian ||
                pRace.m_eLevel == CRace.TechLevel.lv1_Medieval ||
                pRace.m_eLevel == CRace.TechLevel.lv2_Industrial)
            {
                m_eWorldType = WorldType.PrimitiveWorld;
            }
            else
            {
                if (pRace.m_eSociety == CRace.Extraversion.lv0_Closed)
                    m_eWorldType = WorldType.ClosedWorld;
                else
                    m_eWorldType = WorldType.AdvancedWorld;
            }

            m_sName = string.Format("[{0}:{1}] ({3}) Homeworld of {2}", m_iX, m_iY, pRace.ToString(), m_iPotential);
        }

        public void ClaimAsColony(CRace pRace)
        {
            if (pRace == null)
                throw new ArgumentNullException();

            if (m_pMajorRace == pRace)
                throw new ArgumentException("World is already colonized");

            if (m_pMajorRace != null)
                m_pLesserRace = m_pMajorRace;
            m_pMajorRace = pRace;

            m_eWorldType = WorldType.Colony;

            m_sName = string.Format("[{0}:{1}] ({3}) Colony of {2}", m_iX, m_iY, pRace.ToString(), m_iPotential);
        }

        public void ClaimAsBorderWorld(CWorld pBaseWorld)
        {
            if (pBaseWorld == null)
                throw new ArgumentNullException();

            //if (m_pNearestCivilizedWorld != null)
            //    return;

            if (m_eWorldType == WorldType.PrimitiveWorld)
                m_eWorldType = WorldType.AdvancingWorld;
        }

        /// <summary>
        /// Returns a value, representing the time needed to this world to became a colony of pBaseWorld.
        /// </summary>
        /// <param name="pBaseWorld">World, that makes expansion</param>
        /// <returns>int.MaxValue if colonization not possible</returns>
        public int ColonyzationTime(CWorld pBaseWorld)
        {
            if (!m_bHaveHabitablePlanets)
                return int.MaxValue;

            if (m_eWorldType != WorldType.PrimitiveWorld &&
                m_eWorldType != WorldType.EmptyWorld)
                return int.MaxValue;

            if (pBaseWorld == null)
                throw new ArgumentNullException();

            if (pBaseWorld.m_eWorldType == WorldType.PrimitiveWorld ||
                pBaseWorld.m_eWorldType == WorldType.EmptyWorld)
                return int.MaxValue;

            if (pBaseWorld.m_pMajorRace == null)
                throw new ArgumentException("World can't be uninhabitant and not empty at the same time!");

            SimpleVector3d pBaseVector = new SimpleVector3d(pBaseWorld.m_iX, pBaseWorld.m_iY, 0);
            SimpleVector3d pWorldVector = new SimpleVector3d(m_iX, m_iY, 0);
            double fDist = (int)(!(pBaseVector - pWorldVector));

            double fSpeed = pBaseWorld.m_pMajorRace.ExpansionSpeed * pBaseWorld.m_fWealth * 0.001 * m_iPotential;

            if (fSpeed <= 0)
                return int.MaxValue;

            if ((m_eWorldType == WorldType.PrimitiveWorld ||
                m_eWorldType == WorldType.AdvancingWorld) &&
                pBaseWorld.m_pMajorRace.m_eSociety != CRace.Extraversion.lv2_Agressive)
                fSpeed /= 2;

            int iChances = (int)(fDist / fSpeed);

            if(iChances == 0)
                iChances = 1;

            return iChances;
        }

        public void Advance()
        {
            if (m_pMajorRace == null)
            {
                foreach (CRoute pRoute in m_cIncoming)
                {
                    pRoute.m_pFrom.m_fWealth += m_iPotential * pRoute.m_pFrom.m_pMajorRace.ProductionRate * 0.01;
                }
            }
            else
            {
                switch (m_eWorldType)
                { 
                    case WorldType.AdvancingWorld:
                        m_fWealth += m_iPotential * m_pMajorRace.ProductionRate * 2;
                        //m_pTradingRoute.m_fWealth += m_iPotential * m_pTradingRoute.m_pMajorRace.ProductionRate * 0.05;
                        break;
                    case WorldType.Colony:
                        m_fWealth += m_iPotential * m_pMajorRace.ProductionRate * 0.50;
                        //m_pTradingRoute.m_fWealth += m_iPotential * m_pMajorRace.ProductionRate * 0.15;
                        break;
                    case WorldType.ClosedWorld:
                        m_fWealth += m_iPotential * m_pMajorRace.ProductionRate;
                        break;
                    case WorldType.AdvancedWorld:
                        m_fWealth += m_iPotential * m_pMajorRace.ProductionRate;
                        break;
                    case WorldType.PrimitiveWorld:
                        m_fWealth += m_iPotential * m_pMajorRace.ProductionRate;
                        break;
                }
            }

            if (m_eWorldType == WorldType.Colony && m_fWealth >= m_pMajorRace.ProductionRate * 50)
            {
                m_fWealth = 1;
                foreach (CRoute pRoute in m_cIncoming)
                {
                    if(pRoute.m_pFrom.m_pMajorRace == m_pMajorRace)
                        m_eWorldType = pRoute.m_pFrom.m_eWorldType;
                }

                m_pMajorRace.AddRoute(this);
            }

            if ((m_eWorldType == WorldType.AdvancedWorld ||
                 m_eWorldType == WorldType.ClosedWorld) && 
                m_fWealth >= m_pMajorRace.ProductionRate * 50)
            {
                m_fWealth = 1;

                m_pMajorRace.AddRoute(this);
            }
        }
    }
}
