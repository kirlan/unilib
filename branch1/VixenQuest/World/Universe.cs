using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace VixenQuest.World
{
    public class Universe: Space
    {
        public List<Opponent> m_cDeities = new List<Opponent>();

        public Dictionary<int, List<World>> m_cWorldsByTiers = new Dictionary<int, List<World>>();

        public Location m_pMarketplace;

        public Universe():
            base()
        {
            m_iTier = 0;
            World pMarketplace = new World("Superior Marketplace", this);
            m_pMarketplace = pMarketplace.m_cLands[0];

            //for (int i = 0; i < 6; i++)
            //    UnlockNewTier(i);
            UnlockNewTier(0);

            int iDiversity = 50 + Rnd.Get(50);
            for (int i = 0; i < iDiversity; i++)
            {
                Race pPretendentRace;
                Opponent pPretendent;
                do
                {
                    int index = Rnd.Get(m_aAllRaces.Length);
                    pPretendentRace = m_aAllRaces[index];

                    if (pPretendentRace.m_eSapience != Sapience.God)
                        continue;

                    pPretendent = new Opponent(pPretendentRace);

                    bool twin = false;
                    foreach (Opponent pDeity in m_cDeities)
                        if (pDeity.SingleName == pPretendent.SingleName)
                        {
                            twin = true;
                            break;
                        }

                    if (!twin)
                        break;
                }
                while (true);

                m_cDeities.Add(pPretendent);

                //m_sName += ", " + pNewRace.ToString();
            }
        }

        List<Opponent> m_cAvailableOpponents = new List<Opponent>();

        public override Opponent[] AvailableOpponents()
        {
            if (m_cAvailableOpponents.Count == 0)
            {
                foreach (int iTier in m_cWorldsByTiers.Keys)
                    foreach (World loc in m_cWorldsByTiers[iTier])
                        m_cAvailableOpponents.AddRange(loc.AvailableOpponents());
            }

            return m_cAvailableOpponents.ToArray();
        }

        public void UnlockNewTier(int iTier)
        {
            if (!m_cWorldsByTiers.ContainsKey(iTier) || m_cWorldsByTiers[iTier] == null)
                m_cWorldsByTiers[iTier] = new List<World>();

            int iNewLockCount = 1;// +Rnd.Get(2);
            for (int i = 0; i < iNewLockCount; i++)
            {
                m_cWorldsByTiers[iTier].Add(new World(iTier, this));
            }
        }

        static public Location[] GetPathTo(Person pTraveller, Person pTarget)
        {
            return GetPathTo(pTraveller, pTarget.m_pHome);
        }

        static public Location[] GetPathTo(Person pTraveller, Location pTargetLoc)
        {
            List<Location> pPath = new List<Location>();

            if (pTraveller.m_pHome == null)
            {
                if (pTargetLoc != null)
                    pPath.Add(pTargetLoc);

                return pPath.ToArray();
            }

            if (pTraveller.m_pHome is Place || pTraveller.m_pHome is Building)
            { 
                if(pTraveller.m_pHome is Place)
                    pPath.Add((pTraveller.m_pHome as Place).m_pLand);
                else
                    pPath.Add((pTraveller.m_pHome as Building).m_pSettlement);
            }
            else
                pPath.Add(pTraveller.m_pHome);

            Location pWayPoint = pPath[pPath.Count - 1];
            while (pWayPoint != pTargetLoc)
            {
                pWayPoint = pWayPoint.NextStepTo(pTargetLoc);
                pPath.Add(pWayPoint);
            }

            return pPath.ToArray();
        }

        static public Land[] ShortestWay(Land pFrom, Land pTo)
        {
            Dictionary<Land, int> pMatrix = new Dictionary<Land, int>();

            Queue<Land> Q = new Queue<Land>();
            Q.Enqueue(pFrom);

            pMatrix[pFrom] = 0;

            bool bFinished = false;
            while (Q.Count > 0 && !bFinished)
            {
                Land pLand = Q.Dequeue();
                foreach (Land pLinked in pLand.Links)
                {
                    if (!pMatrix.ContainsKey(pLinked))
                    {
                        pMatrix[pLinked] = pMatrix[pLand] + 1;
                        Q.Enqueue(pLinked);
                    }

                    if (pLinked == pTo)
                        bFinished = true;
                }
            }

            List<Land> cResult = new List<Land>();

            if (!bFinished)
                return cResult.ToArray();

            cResult.Add(pTo);
            Land pPoint = cResult[0];

            while (pPoint != pFrom)
            {
                Land pBestPretender = null;
                int iBestDistance = int.MaxValue;
                foreach (Land pLinked in pPoint.Links)
                {
                    if (pMatrix.ContainsKey(pLinked) && pMatrix[pLinked] < iBestDistance)
                    {
                        iBestDistance = pMatrix[pLinked];
                        pBestPretender = pLinked;
                    }
                }
                if (pBestPretender == null)
                    throw new Exception("Can't find path between lands!");

                pPoint = pBestPretender;
                cResult.Insert(0, pPoint);
            }

            return cResult.ToArray();
        }
    }
}
