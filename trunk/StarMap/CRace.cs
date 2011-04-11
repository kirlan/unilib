using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using SimpleVectors;

namespace StarMap
{
    public class CRace
    {
        public static int Counter = 0;

        public int m_iID;

        public override string ToString()
        {
            return string.Format("{0} {1} - {2}", Desc, Name, m_iID);
        }

        public string Desc
        {
            get
            {
                switch (m_eSociety)
                {
                    case Extraversion.lv0_Closed:
                        return "Isolationist";
                    case Extraversion.lv1_Peaceful:
                        return "Peaceful";
                    case Extraversion.lv2_Agressive:
                        return "Agressive";
                }
                return "Unknown";
            }
        }

        public string Name
        {
            get
            {
                switch (m_eLevel)
                {
                    case TechLevel.lv0_Barbarian:
                        return "Barbarians";
                    case TechLevel.lv1_Medieval:
                        return "Knights";
                    case TechLevel.lv2_Industrial:
                        return "Industrialists";
                    case TechLevel.lv3_Humans:
                        return "Humans";
                    case TechLevel.lv4_Heirs:
                        return "Heirs";
                    case TechLevel.lv5_AncientEsses:
                        return "Ancient Esses";
                    case TechLevel.lv6_Magistrate:
                        return "Magistrate";
                    case TechLevel.lv7_AncientAnnan:
                        return "Ancient Annan";
                    case TechLevel.lv8_ModernEsses:
                        return "Modern Esses";
                    case TechLevel.lv9_DarkDragons:
                        return "Dark Dragons";
                }
                return "Unknown";
            }
        }

        public CRace(CWorld pHomeWorld, TechLevel eLevel, Extraversion eSociety)
        {
            if (!pHomeWorld.m_bHaveHabitablePlanets)
            {
                throw new ArgumentException("Unhabitable world can't be HomeWorld!");
            }

            if (eSociety == Extraversion.Unknown)
            {
                switch (Rnd.Get(3))
                {
                    case 0:
                        m_eSociety = Extraversion.lv0_Closed;
                        break;
                    case 1:
                        m_eSociety = Extraversion.lv1_Peaceful;
                        break;
                    case 2:
                        m_eSociety = Extraversion.lv2_Agressive;
                        break;
                }
            }
            else
            {
                m_eSociety = eSociety;
            }

            if (eLevel == TechLevel.Unknown)
            {
                switch (Rnd.Get(9))
                {
                    case 0:
                        m_eLevel = TechLevel.lv0_Barbarian;
                        break;
                    case 1:
                        m_eLevel = TechLevel.lv1_Medieval;
                        break;
                    case 2:
                        m_eLevel = TechLevel.lv2_Industrial;
                        break;
                    case 3:
                        m_eLevel = TechLevel.lv3_Humans;
                        break;
                    case 4:
                        m_eLevel = TechLevel.lv4_Heirs;
                        break;
                    case 5:
                        m_eLevel = TechLevel.lv5_AncientEsses;
                        break;
                    case 6:
                        m_eLevel = TechLevel.lv6_Magistrate;
                        break;
                    case 7:
                        m_eLevel = TechLevel.lv7_AncientAnnan;
                        break;
                    case 8:
                        m_eLevel = TechLevel.lv8_ModernEsses;
                        break;
                    case 9:
                        m_eLevel = TechLevel.lv9_DarkDragons;
                        break;
                }
            }
            else
            {
                m_eLevel = eLevel;
            }

            m_iID = Counter++;
            ResetQuantumLeap();

            ClaimWorld(pHomeWorld);
            AddRoute(pHomeWorld);
        }

        public CRace(CWorld pHomeWorld, TechLevel eLevel)
            : this(pHomeWorld, eLevel, Extraversion.Unknown)
        {}

        public CRace(CWorld pHomeWorld)
            : this(pHomeWorld, TechLevel.Unknown, Extraversion.Unknown) 
        {}

        public enum TechLevel
        { 
            Unknown,
            lv0_Barbarian,
            lv1_Medieval,
            lv2_Industrial,
            lv3_Humans,
            lv4_Heirs,
            lv5_AncientEsses,
            lv6_Magistrate,
            lv7_AncientAnnan,
            lv8_ModernEsses,
            lv9_DarkDragons
        }

        public enum Extraversion
        { 
            Unknown,
            lv0_Closed,
            lv1_Peaceful,
            lv2_Agressive
        }

        public TechLevel m_eLevel;
        public Extraversion m_eSociety;

        public List<CWorld> m_cOwnedWorlds = new List<CWorld>();

        private CWorld FindNearestColonizableWorld(CWorld pBaseWorld)
        {
            if (!m_cOwnedWorlds.Contains(pBaseWorld))
                return pBaseWorld;

            //return pBaseWorld;

            CWorld pNearestWorld = null;

            int iMinTime = int.MaxValue;
            foreach (CWorld pWorld in pBaseWorld.m_pMap.m_cWorlds)
            {
                int iColonizationTime = pWorld.ColonyzationTime(pBaseWorld);
                int iConcurrentColonizationTime = int.MaxValue;
                foreach (CRoute pRoute in pWorld.m_cIncoming)
                {
                    int iTime = pWorld.ColonyzationTime(pRoute.m_pFrom);
                    if (iTime < iConcurrentColonizationTime)
                    {
                        iConcurrentColonizationTime = iTime;
                    }
                }

                if (pWorld != pBaseWorld && iColonizationTime < iConcurrentColonizationTime)
                {
                    if (iColonizationTime < iMinTime)
                    {
                        iMinTime = iColonizationTime;
                        pNearestWorld = pWorld;
                        if (pWorld.m_eWorldType != CWorld.WorldType.EmptyWorld &&
                            pWorld.m_eWorldType != CWorld.WorldType.PrimitiveWorld)
                        {
                            iMinTime = iColonizationTime;
                            pNearestWorld = pWorld;
                        }
                        if (!pWorld.m_bHaveHabitablePlanets)
                        {
                            iMinTime = iColonizationTime;
                            pNearestWorld = pWorld;
                        }
                    }
                }
            }

            //int iTime = 0;
            //if (pNearestWorld != null)
            //    iTime = pNearestWorld.ColonyzationTime(pBaseWorld);

            return pNearestWorld;
        }

        private void ClaimWorld(CWorld pWorld)
        {
            if (pWorld == null)
                return;

            if (m_cOwnedWorlds.Contains(pWorld))
                return;

            //CWorld pColonizer = null;
            //int iMinTime = int.MaxValue;
            //foreach (CRoute pRoute in pWorld.m_cIncoming)
            //{
            //    int iTime = pWorld.ColonyzationTime(pRoute.m_pFrom);
            //    if (iTime < iMinTime)
            //    {
            //        iMinTime = iTime;
            //        pColonizer = pRoute.m_pFrom;
            //    }
            //}
            //if (pColonizer != null && pColonizer.m_pMajorRace != this)
            //    throw new ArgumentException("World's colonizer not belongs to this race!");

            if (m_cOwnedWorlds.Count == 0)
                pWorld.ClaimAsHomeWorld(this);
            else
                pWorld.ClaimAsColony(this);

            m_cOwnedWorlds.Add(pWorld);

        //    if (pColonizer != null)
        //    {
        //        pColonizer.m_fWealth = 1;
        //        //CWorld pNearestFreeWorld = FindNearestColonizableWorld(pColonizer);
        //        //if (pNearestFreeWorld != null)
        //        //{
        //        //    m_cBorderWorlds.Add(pNearestFreeWorld);
        //        //    pNearestFreeWorld.ClaimAsBorderWorld(pColonizer);
        //        //}
        //    }
        }

        public int m_iTotalWealth = 0;
        public int m_iQuantumLeap = 0;

        public void Advance()
        {
            foreach (CWorld pWorld in m_cOwnedWorlds)
            {
                pWorld.Advance();
            }

            if (m_eLevel != TechLevel.lv0_Barbarian &&
                m_eLevel != TechLevel.lv1_Medieval &&
                m_eLevel != TechLevel.lv2_Industrial)
            { 
                List<CWorld> cNewColonies = new List<CWorld>();
                foreach (CWorld pOwnedWorld in m_cOwnedWorlds)
                {
                    foreach (CRoute pRoute in pOwnedWorld.m_cIncoming)
                    {
                        if (pRoute.m_pTo.m_pMajorRace == null)
                        {
                            int iChances = pRoute.m_pTo.ColonyzationTime(pOwnedWorld);

                            if (Rnd.OneChanceFrom(iChances) && !cNewColonies.Contains(pRoute.m_pTo))
                            {
                                cNewColonies.Add(pRoute.m_pTo);
                                pRoute.m_pTo.m_eWorldType = CWorld.WorldType.Colony;
                            }
                        }
                    }
                }

                foreach(CWorld pNewColony in cNewColonies)
                    ClaimWorld(pNewColony);
            }

            m_iTotalWealth = 0;
            foreach (CWorld pWorld in m_cOwnedWorlds)
            {
                m_iTotalWealth += (int)pWorld.m_fWealth;
            }
            if (m_iTotalWealth >= m_iQuantumLeap)
                RaiseTechLevel();
            if (m_iTotalWealth <= 0)
                LowerTechLevel();
        }

        public double ProductionRate
        {
            get
            {
                double fProd = 0;
                switch (m_eLevel)
                {
                    case CRace.TechLevel.lv0_Barbarian:
                        fProd = 0.01;//50
                        break;
                    case CRace.TechLevel.lv1_Medieval:
                        fProd = 0.1;//100
                        break;
                    case CRace.TechLevel.lv2_Industrial:
                        fProd = 0.5;//200
                        break;
                    case CRace.TechLevel.lv3_Humans:
                        fProd = 1;//400
                        break;
                    case CRace.TechLevel.lv4_Heirs:
                        fProd = 1.5;//800
                        break;
                    case CRace.TechLevel.lv5_AncientEsses:
                        fProd = 2;//1600
                        break;
                    case CRace.TechLevel.lv6_Magistrate:
                        fProd = 3;//3200
                        break;
                    case CRace.TechLevel.lv7_AncientAnnan:
                        fProd = 5;//6400
                        break;
                    case CRace.TechLevel.lv8_ModernEsses:
                        fProd = 10;//12800
                        break;
                    case CRace.TechLevel.lv9_DarkDragons:
                        fProd = 20;
                        break;
                }
                return fProd;
            }
        }

        public double ExpansionSpeed
        {
            get 
            {
                double fSpeed = 0;
                switch (m_eLevel)
                {
                    case TechLevel.lv0_Barbarian:
                        fSpeed = 0;
                        break;
                    case TechLevel.lv1_Medieval:
                        fSpeed = 0;
                        break;
                    case TechLevel.lv2_Industrial:
                        fSpeed = 0;
                        break;
                    case TechLevel.lv3_Humans:
                        fSpeed = 0.1;
                        break;
                    case TechLevel.lv4_Heirs:
                        fSpeed = 0.2; 
                        break;
                    case TechLevel.lv5_AncientEsses:
                        fSpeed = 0.4;
                        break;
                    case TechLevel.lv6_Magistrate:
                        fSpeed = 0.8;
                        break;
                    case TechLevel.lv7_AncientAnnan:
                        fSpeed = 1.6;
                        break;
                    case TechLevel.lv8_ModernEsses:
                        fSpeed = 3.2;
                        break;
                    case TechLevel.lv9_DarkDragons:
                        fSpeed = 6.4;
                        break;
                }

                if (m_eSociety == Extraversion.lv0_Closed)
                    fSpeed /= 10;

                if (m_eSociety == Extraversion.lv2_Agressive)
                {
                    fSpeed *= 5;
                }

                return fSpeed;
            }
        }

        public void RaiseTechLevel()
        {
            switch (m_eLevel)
            {
                case TechLevel.lv0_Barbarian:
                    m_eLevel = TechLevel.lv1_Medieval;
                    break;
                case TechLevel.lv1_Medieval:
                    m_eLevel = TechLevel.lv2_Industrial;
                    break;
                case TechLevel.lv2_Industrial:
                    m_eLevel = TechLevel.lv3_Humans;
                    break;
                case TechLevel.lv3_Humans:
                    m_eLevel = TechLevel.lv4_Heirs;
                    break;
                case TechLevel.lv4_Heirs:
                    m_eLevel = TechLevel.lv5_AncientEsses;
                    break;
                case TechLevel.lv5_AncientEsses:
                    m_eLevel = TechLevel.lv6_Magistrate;
                    break;
                case TechLevel.lv6_Magistrate:
                    m_eLevel = TechLevel.lv7_AncientAnnan;
                    break;
                case TechLevel.lv7_AncientAnnan:
                    m_eLevel = TechLevel.lv8_ModernEsses;
                    break;
                case TechLevel.lv8_ModernEsses:
                    m_eLevel = TechLevel.lv9_DarkDragons;
                    break;
                case TechLevel.lv9_DarkDragons:
                    m_eLevel = TechLevel.lv9_DarkDragons;
                    break;
            }
            ResetQuantumLeap();

            foreach (CWorld pWorld in m_cOwnedWorlds)
            {
                pWorld.m_fWealth = 1;
            }

            if (m_eLevel == TechLevel.lv3_Humans)
            {
                if (m_cOwnedWorlds.Count > 0)
                {
                    CWorld pHomeWorld = m_cOwnedWorlds[0];
                    pHomeWorld.m_eWorldType = CWorld.WorldType.AdvancedWorld;
                    AddRoute(pHomeWorld);
                }
            }
        }

        public void LowerTechLevel()
        {
            switch (m_eLevel)
            {
                case TechLevel.lv0_Barbarian:
                    m_eLevel = TechLevel.lv0_Barbarian;
                    break;
                case TechLevel.lv1_Medieval:
                    m_eLevel = TechLevel.lv0_Barbarian;
                    break;
                case TechLevel.lv2_Industrial:
                    m_eLevel = TechLevel.lv1_Medieval;
                    break;
                case TechLevel.lv3_Humans:
                    m_eLevel = TechLevel.lv2_Industrial;
                    break;
                case TechLevel.lv4_Heirs:
                    m_eLevel = TechLevel.lv3_Humans;
                    break;
                case TechLevel.lv5_AncientEsses:
                    m_eLevel = TechLevel.lv4_Heirs;
                    break;
                case TechLevel.lv6_Magistrate:
                    m_eLevel = TechLevel.lv5_AncientEsses;
                    break;
                case TechLevel.lv7_AncientAnnan:
                    m_eLevel = TechLevel.lv6_Magistrate;
                    break;
                case TechLevel.lv8_ModernEsses:
                    m_eLevel = TechLevel.lv7_AncientAnnan;
                    break;
                case TechLevel.lv9_DarkDragons:
                    m_eLevel = TechLevel.lv8_ModernEsses;
                    break;
            }
            ResetQuantumLeap();
        }

        private void ResetQuantumLeap()
        {
            switch (m_eLevel)
            {
                case CRace.TechLevel.lv0_Barbarian:
                    m_iQuantumLeap = 100;
                    break;
                case CRace.TechLevel.lv1_Medieval:
                    m_iQuantumLeap = 200;
                    break;
                case CRace.TechLevel.lv2_Industrial:
                    m_iQuantumLeap = 400;
                    break;
                case CRace.TechLevel.lv3_Humans:
                    m_iQuantumLeap = 800;
                    break;
                case CRace.TechLevel.lv4_Heirs:
                    m_iQuantumLeap = 1600;
                    break;
                case CRace.TechLevel.lv5_AncientEsses:
                    m_iQuantumLeap = 3200;
                    break;
                case CRace.TechLevel.lv6_Magistrate:
                    m_iQuantumLeap = 6400;
                    break;
                case CRace.TechLevel.lv7_AncientAnnan:
                    m_iQuantumLeap = 12800;
                    break;
                case CRace.TechLevel.lv8_ModernEsses:
                    m_iQuantumLeap = 25600;
                    break;
                case CRace.TechLevel.lv9_DarkDragons:
                    m_iQuantumLeap = int.MaxValue;
                    break;
            }
            if (m_eSociety == Extraversion.lv0_Closed)
                m_iQuantumLeap *= 2;

            if (m_eSociety == Extraversion.lv2_Agressive)
                m_iQuantumLeap /= 2;
        }

        internal void AddRoute(CWorld pWorld)
        {
            CWorld pNearestFreeWorld = FindNearestColonizableWorld(pWorld);
            if (pNearestFreeWorld != null)
            {
                CStarship pShip = new CStarship(1 + Rnd.Get(3));
                CRoute pRoute = new CRoute(pWorld, pNearestFreeWorld, pShip);
                
                pNearestFreeWorld.ClaimAsBorderWorld(pWorld);
            }
        }
    }
}
