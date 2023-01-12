using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandscapeGeneration.PathFind
{
    internal class PriorityQueue<P, V>
    {
        private SortedDictionary<P, Queue<V>> List { get; } = new SortedDictionary<P, Queue<V>>();
        public void Enqueue(P priority, V value)
        {
            if (!List.TryGetValue(priority, out Queue<V> q))
            {
                q = new Queue<V>();
                List.Add(priority, q);
            }
            q.Enqueue(value);
        }
        public V Dequeue()
        {
            // will throw if there isn’t any first element!
            var pair = List.First();
            var v = pair.Value.Dequeue();
            if (pair.Value.Count == 0) // nothing left of the top priority.
                List.Remove(pair.Key);
            return v;
        }
        public bool IsEmpty
        {
            get { return List.Count == 0; }
        }
    }
}
