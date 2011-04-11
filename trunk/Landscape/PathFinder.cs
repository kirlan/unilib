using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace LandscapeGeneration
{
    public class PathFinder<LOC>
        where LOC: Location
    {
        public LOC[] m_cPath;

        public float m_fLength = 0;

        class BasicHeap<V>
        {
            public struct HeapNode
            {
                public HeapNode(V val, double prio)
                {
                    value = val;
                    priority = prio;

                }
                public V value;
                public double priority;
            }

            private List<HeapNode> InnerList = new List<HeapNode>();

            public BasicHeap() { }

            public bool IsEmpty
            {
                get { return InnerList.Count == 0; }
            }
            //public int Count
            //{
            //    get
            //    {
            //        return InnerList.Count;
            //    }
            //}

            public void Enqueue(double priority, V value)
            {
                int p = InnerList.Count, p2;
                InnerList.Add(new HeapNode(value, priority)); // E[p] = O
                do
                {
                    if (p == 0)
                        break;

                    p2 = (p - 1) >> 1;
                    if (InnerList[p].priority < InnerList[p2].priority)
                    {
                        HeapNode h = InnerList[p];
                        InnerList[p] = InnerList[p2];
                        InnerList[p2] = h;

                        p = p2;
                    }
                    else
                        break;
                } while (true);
            }

            public V Dequeue()
            {
                HeapNode result = InnerList[0];
                int p = 0, p1, p2, pn;
                InnerList[0] = InnerList[InnerList.Count - 1];
                InnerList.RemoveAt(InnerList.Count - 1);
                do
                {
                    pn = p;
                    p1 = (p << 1) + 1;
                    p2 = (p << 1) + 2;
                    if (InnerList.Count > p1 && InnerList[p].priority > InnerList[p1].priority)
                        p = p1;
                    if (InnerList.Count > p2 && InnerList[p].priority > InnerList[p2].priority)
                        p = p2;

                    if (p == pn)
                        break;

                    HeapNode h = InnerList[p];
                    InnerList[p] = InnerList[pn];
                    InnerList[pn] = h;

                } while (true);
                return result.value;
            }
        }

        class PriorityQueue<P, V>
        {
            private SortedDictionary<P, Queue<V>> list = new SortedDictionary<P, Queue<V>>();
            public void Enqueue(P priority, V value)
            {
                Queue<V> q;
                if (!list.TryGetValue(priority, out q))
                {
                    q = new Queue<V>();
                    list.Add(priority, q);
                }
                q.Enqueue(value);
            }
            public V Dequeue()
            {
                // will throw if there isn’t any first element!
                var pair = list.First();
                var v = pair.Value.Dequeue();
                if (pair.Value.Count == 0) // nothing left of the top priority.
                    list.Remove(pair.Key);
                return v;
            }
            public bool IsEmpty
            {
                get { return !list.Any(); }
            }
        }

        class Path<Node> : IEnumerable<Node>
        {
            public Node LastStep { get; private set; }
            public Path<Node> PreviousSteps { get; private set; }
            public double TotalCost { get; private set; }
            private Path(Node lastStep, Path<Node> previousSteps, double totalCost)
            {
                LastStep = lastStep;
                PreviousSteps = previousSteps;
                TotalCost = totalCost;
            }
            public Path(Node start) : this(start, null, 0) { }
            public Path<Node> AddStep(Node step, double stepCost)
            {
                return new Path<Node>(step, this, TotalCost + stepCost);
            }
            public IEnumerator<Node> GetEnumerator()
            {
                for (Path<Node> p = this; p != null; p = p.PreviousSteps)
                    yield return p.LastStep;
            }
            //IEnumerator IEnumerable.GetEnumerator()
            //{
            //    return this.GetEnumerator();
            //}

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            #endregion
        }

        Path<LOC> FindPath(
            LOC start,
            LOC destination)
        {
            var closed = new HashSet<LOC>();
            //var queue = new PriorityQueue<double, Path<LOC>>();
            var queue = new BasicHeap<Path<LOC>>();
            
            queue.Enqueue(0, new Path<LOC>(start));
            
            while (!queue.IsEmpty)
            {
                var path = queue.Dequeue();
                
                if (closed.Contains(path.LastStep))
                    continue;
                
                if (path.LastStep.Equals(destination))
                    return path;
                
                closed.Add(path.LastStep);

                foreach (LOC pLink in path.LastStep.m_pLinks.Keys)
                {
                    if (path.LastStep.m_pLinks[pLink].m_bClosed)
                        continue;

                    var newPath = path.AddStep(pLink, path.LastStep.m_pLinks[pLink].MovementCost);

                    float fStrightDistance = (float)Math.Sqrt((destination.X - pLink.X) * (destination.X - pLink.X) + (destination.Y - pLink.Y) * (destination.Y - pLink.Y));

                    //if (pLink.m_pLocation.m_bRoad)// && !bInternal)
                    //    fStrightDistance /= 2;

                    queue.Enqueue(newPath.TotalCost + fStrightDistance * pLink.GetMovementCost(), newPath);
                }
            }
            return null;
        }

        public PathFinder(LOC pFrom, LOC pTo)
        {
            List<LOC> cResult = new List<LOC>();

            if(pFrom == null || pTo == null)
            {
                m_cPath = cResult.ToArray();
                return;
            }

            Path<LOC> pPath = FindPath(pFrom, pTo);

            if (pPath != null)
            {
                m_fLength = (float)pPath.TotalCost;

                Path<LOC> pStep = pPath;
                do
                {
                    cResult.Insert(0, pStep.LastStep);
                    pStep = pStep.PreviousSteps;
                }
                while (pStep != null);
            }
            m_cPath = cResult.ToArray();

            return;
        }
    }
}
