using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EconomyModelFlows
{
    public class CMap
    {
        List<CBuilding> m_cBuildings = new List<CBuilding>();

        /// <summary>
        /// Находим строение, из которого ведут маршруты, более выгодные чем уже проложенные
        /// </summary>
        /// <returns>строение</returns>
        private CBuilding CheckCycles()
        {
            foreach (CBuilding pFrom in m_cBuildings)
                foreach (CBuilding pTo in pFrom.Routes.Keys)
                    if (pTo.Cost > pFrom.Cost + pFrom.GetRoutePrice(pTo))
                        return pFrom;
            
            return null;
        }

        private bool BreadthFirstSearch(CBuilding s, CBuilding t)
        {
            Queue<CBuilding> Q;
            s.Push(s, int.MaxValue);
            Q.Enqueue(s);

            while (!t.Marked && Q.Count > 0)
            {
                CBuilding pFrom = Q.Dequeue();
                foreach (CBuilding pTo in pFrom.Routes.Keys)
                {
                    if (pTo.Push(pFrom, pFrom.Routes[pTo].Capacity > pFrom.Routes[pTo].CurrentFlow))
                        Q.Enqueue(pTo);
                }
            }

            return t.Marked;
        }

        private CBuilding BellmanForde(CBuilding s)
        {
            Queue<CBuilding> Q;
            s.SetRoute(s);

            Q.Enqueue(s);

            CBuilding MAX_N = new CBuilding();
            Q.Enqueue(MAX_N);

            int series = 0;
            while (Q.Count > 0)
            {
                while (Q.First() == MAX_N)
                {
                    Q.Dequeue();
                    if (++series > m_cBuildings.Count)
                        return CheckCycles();
                    else
                        Q.Enqueue(MAX_N);
                }

                CBuilding pFrom = Q.Dequeue();
                foreach (CBuilding pTo in pFrom.Routes.Keys)
                {
                    if (pTo.SetRoute(pFrom))
                        Q.Enqueue(pTo);
                }
            }
        }

        private int MaxFlowFF(CBuilding s, CBuilding t)
        {
            int flow = 0;

            while (BreadthFirstSearch(s, t))
            {
                int add = t.Pushed;

                CBuilding pTo = t;
                CBuilding pFrom = t.From;
                while (pTo != s)
                {
                    pFrom.Routes[pTo].CurrentFlow += add;
                    pTo.Routes[pFrom].CurrentFlow -= add;

                    pTo = pFrom;
                    pFrom = pTo.From;
                }
                flow += add;
            }

            return flow;
        }

        public void MinCostFlow(CBuilding s, CBuilding t)
        {
            int maxFlow = MaxFlowFF(s, t);

            int flow = 0;
            int add = int.MaxValue;
            
            CBuilding negCycle = BellmanForde(t);

            while (negCycle != null)
            {
                CBuilding pTo = negCycle;
                CBuilding pFrom = pTo.From;
                do
                {
                    add = Math.Min(add, pFrom.Routes[pTo].Capacity - pFrom.Routes[pTo].CurrentFlow);
                    pTo = pFrom;
                    pFrom = pTo.From;
                }
                while(pTo != negCycle);

                pTo = negCycle;
                pFrom = pTo.From;
                do
                {
                    pFrom.Routes[pTo].CurrentFlow += add;
                    pTo.Routes[pFrom].CurrentFlow -= add;

                    pTo = pFrom;
                    pFrom = pTo.From;
                }
                while (pTo != negCycle);

                negCycle = BellmanForde(t);
            }

            int minCost = 0;

            foreach (CBuilding pFrom in m_cBuildings)
                foreach (CBuilding pTo in pFrom.Routes.Keys)
                    if (pFrom.Routes[pTo].CurrentFlow > 0)
                        minCost += pFrom.Routes[pTo].CurrentFlow * pFrom.Routes[pTo].Price;
        }
    }
}
