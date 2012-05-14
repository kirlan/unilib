using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GridBuilderTest
{
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

            public override string ToString()
            {
                return string.Format("{0} [{1}]", priority, value);
            }
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

            if (InnerList.Count == 0)
                return result.value;

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
            } 
            while (true);
            
            return result.value;
        }
    }
}
