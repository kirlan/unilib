using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinCostFlow
{
    public class CFlowsGrid
    {
        public CFlowsGrid()
        {
            CNode.Reset();
        }

        List<CNode> m_cNodes = new List<CNode>();

        public List<CNode> Nodes
        {
            get { return m_cNodes; }
        }

        /// <summary>
        /// Находим узел, из которого ведут маршруты, более выгодные чем уже проложенные
        /// </summary>
        /// <returns>строение</returns>
        private CNode CheckCycles()
        {
            foreach (CNode pFrom in m_cNodes)
                foreach (CNode pTo in pFrom.Links.Keys)
                {
                    long newCost = pFrom.Cost + pFrom.GetLinkCost(pTo.Id);
                    if (pTo.Cost > newCost)
                        return pFrom;
                }
            
            return null;
        }

        private void Init()
        {
            foreach (CNode pNode in m_cNodes)
            {
                pNode.Init();
            }
        }

        private bool BreadthFirstSearch(CNode s, CNode t)
        {
            Init();
            Queue<CNode> Q = new Queue<CNode>();
            s.Push(s);
            Q.Enqueue(s);

            while (!t.Marked && Q.Count > 0)
            {
                CNode pFrom = Q.Dequeue();
                foreach (CNode pTo in pFrom.Links.Keys)
                {
                    if (pTo.Push(pFrom))
                        Q.Enqueue(pTo);
                }
            }

            return t.Marked;
        }

        private CNode BellmanForde(CNode s)
        {
            Init();
            Queue<CNode> Q = new Queue<CNode>();
            s.SetLink();

            Q.Enqueue(s);

            Q.Enqueue(null);

            int series = 0;
            while (Q.Count > 0)
            {
                while (Q.First() == null)
                {
                    Q.Dequeue();
                    if (++series > m_cNodes.Count*2)
                        return CheckCycles();
                    else
                        Q.Enqueue(null);
                }

                CNode pFrom = Q.Dequeue();
//                foreach (CNode pTo in pFrom.Links.Keys)
                for (int i = 0; i < pFrom.Mates.Length; i++ )
                {
                    if (pFrom.Mates[i].SetLink(pFrom))
                        Q.Enqueue(pFrom.Mates[i]);
                }
            }

            return null;
        }

        public long MaxFlowFF(CNode s, CNode t)
        {
            long flow = 0;

            long outflow = 0;
            foreach (CLink link in s.Links.Values)
            {
                outflow += link.Capacity;
            }

            while (BreadthFirstSearch(s, t))
            {
                long add = t.Pushed;

                CNode pTo = t;
                CNode pFrom = t.From;
                while (pTo != s)
                {
                    pFrom.Links[pTo].CurrentFlow += add;
                    pTo.Links[pFrom].CurrentFlow -= add;

                    pTo = pFrom;
                    pFrom = pTo.From;
                }
                flow += add;
            }

            if (flow != outflow)
            {
                for (int i=m_cNodes.Count-1; i>0; i--)
                {
                    CNode pNode = m_cNodes[i];
                    if (pNode != s && pNode != t)
                    {
                        long sum = 0;
                        foreach (CLink pLink in pNode.Links.Values)
                        {
                            sum += pLink.CurrentFlow;
                            if (pLink.Capacity > 0 && pLink.Capacity != int.MaxValue)
                                if (pLink.Capacity > pLink.CurrentFlow)
                                    BreadthFirstSearch(s, t);
                        }

                        if (sum != 0)
                            BreadthFirstSearch(s, t);
                    }
                }
            }

            return flow;
        }

        public void AddVertex(IVertex pVertex)
        {
            m_cNodes.Add(pVertex.Entrance);
            m_cNodes.Add(pVertex.Exit);
        }

        public void MinCostFlow(CNode s, CNode t)
        {
            try
            {
                foreach (CNode nod in m_cNodes)
                {
                    nod.BuildFastAccess(m_cNodes.Count);
                    foreach (CNode nod2 in nod.Links.Keys)
                        nod.AddFastAccess(nod2);
                }

                long maxFlow = MaxFlowFF(s, t);

                int flow = 0;
                long add = int.MaxValue;

                CNode negCycle = BellmanForde(t);

                while (negCycle != null)
                {
                    CNode pTo = negCycle;
                    CNode pFrom = pTo.From;
                    List<CNode> cheked = new List<CNode>();
                    do
                    {
                        long newAdd = pFrom.Links[pTo].Capacity - pFrom.Links[pTo].CurrentFlow;
                        add = Math.Min(add, newAdd);

                        cheked.Add(pFrom);

                        pTo = pFrom;
                        pFrom = pTo.From;
                    }
                    while (!cheked.Contains(pFrom));
//                    while (pTo != negCycle) ;

                    negCycle = pFrom;

                    pTo = negCycle;
                    pFrom = pTo.From;
                    do
                    {
                        pFrom.Links[pTo].CurrentFlow += add;
                        pTo.Links[pFrom].CurrentFlow -= add;

                        pTo = pFrom;
                        pFrom = pTo.From;
                    }
                    while (pTo != negCycle);

                    negCycle = BellmanForde(t);
                }

                long minCost = 0;

                foreach (CNode pFrom in m_cNodes)
                    foreach (CNode pTo in pFrom.Links.Keys)
                        if (pFrom.Links[pTo].CurrentFlow > 0)
                            minCost += pFrom.Links[pTo].CurrentFlow * pFrom.Links[pTo].Cost;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
