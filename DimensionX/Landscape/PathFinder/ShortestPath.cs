using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace LandscapeGeneration.PathFind
{
    public class ShortestPath
    {
        public TransportationNode[] m_aNodes;

        public float m_fLength = 0;

        //public HashSet<TransportationNode> visited = new HashSet<TransportationNode>();

        Path<TransportationNode> FindPath(
            TransportationNode start,
            TransportationNode destination,
            int iPassword,
            bool bNavalOnly)
        {
            var closed = new HashSet<TransportationNode>();
            //var queue = new PriorityQueue<double, Path<LOC>>();
            var queue = new BasicHeap<Path<TransportationNode>>();

            queue.Enqueue(0, new Path<TransportationNode>(start));

            while (!queue.IsEmpty)
            {
                var path = queue.Dequeue();

                if (closed.Contains(path.LastStep))
                    continue;
                
                if (path.LastStep.Equals(destination))
                    return path;
                
                closed.Add(path.LastStep);

                foreach (var pLinked in path.LastStep.m_cLinks)
                {
                    TransportationNode pLinkedNode = pLinked.Key;

                    //ограничиваем доступную территорию только теми нодами, которые ходят в уже найденный путь на более высоком уровне
                    if (iPassword != -1 && pLinkedNode.m_iPassword != iPassword)
                        continue;

                    //грузиться на корабли и высаживаться с них можно только в портах
                    if (pLinked.Value.Embark && !pLinkedNode.m_bHarbor && !path.LastStep.m_bHarbor)
                        continue;

                    //ограничиваем доступную территорию по государственному признаку
                    if (pLinked.Value.m_bClosed)
                        continue;

                    if (bNavalOnly && !pLinked.Value.Sea && !pLinked.Value.Embark)
                        continue;

                    //TransportationNode pLinkedNodeOwner = (pLinkedNode as ITerritory).Owner as TransportationNode;
                    //if (pLinkedNodeOwner != null && !visited.Contains(pLinkedNodeOwner))
                    //    visited.Add(pLinkedNodeOwner);

                    var newPath = path.AddStep(pLinkedNode, pLinked.Value.MovementCost);

                    float fStrightDistance = pLinkedNode.DistanceTo(destination);// (float)Math.Sqrt((destination.X - pLink.X) * (destination.X - pLink.X) + (destination.Y - pLink.Y) * (destination.Y - pLink.Y));

                    //if (pLink.m_pLocation.m_bRoad)// && !bInternal)
                    //    fStrightDistance /= 2;

                    queue.Enqueue(newPath.TotalCost + fStrightDistance * pLinkedNode.GetMovementCost(), newPath);
                }
            }
            return null;
        }

        public ShortestPath(TransportationNode pFrom, TransportationNode pTo, int iPassword, bool bNavalOnly)
        {
            List<TransportationNode> cResult = new List<TransportationNode>();

            if(pFrom == null || pTo == null)
            {
                m_aNodes = cResult.ToArray();
                return;
            }

            Path<TransportationNode> pPath = FindPath(pFrom, pTo, iPassword, bNavalOnly);

            if (pPath != null)
            {
                m_fLength = (float)pPath.TotalCost;

                Path<TransportationNode> pStep = pPath;
                do
                {
                    cResult.Insert(0, pStep.LastStep);
                    pStep = pStep.PreviousSteps;
                }
                while (pStep != null);
            }
            m_aNodes = cResult.ToArray();

            return;
        }
    }
}
