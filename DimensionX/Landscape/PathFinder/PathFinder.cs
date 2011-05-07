using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace LandscapeGeneration.PathFind
{
    public class PathFinder
    {
        public TransportationNode[] m_cPath;

        public float m_fLength = 0;

        public HashSet<TransportationNode> visited = new HashSet<TransportationNode>();

        Path<TransportationNode> FindPath(
            TransportationNode start,
            TransportationNode destination,
            float fCycleShift,
            int iPassword,
            bool bNaval)
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

                foreach (TransportationNode pLinked in path.LastStep.m_cLinks.Keys)
                {
                    //ограничиваем доступную территорию только теми нодами, которые ходят в уже найденный путь на более высоком уровне
                    if (iPassword != -1 && pLinked.m_iPassword != iPassword)
                        continue;

                    //грузиться на корабли и высаживаться с них можно только в портах
                    if (path.LastStep.m_cLinks[pLinked].m_bEmbark && !pLinked.m_bHarbor && !path.LastStep.m_bHarbor)
                        continue;

                    //ограничиваем доступную территорию по государственному признаку
                    if (path.LastStep.m_cLinks[pLinked].m_bClosed)
                        continue;

                    if (bNaval && !path.LastStep.m_cLinks[pLinked].m_bSea && !path.LastStep.m_cLinks[pLinked].m_bEmbark)
                        continue;

                    if ((pLinked as ITerritory).Owner != null && !visited.Contains((pLinked as ITerritory).Owner as TransportationNode))
                        visited.Add((pLinked as ITerritory).Owner as TransportationNode);

                    var newPath = path.AddStep(pLinked, path.LastStep.m_cLinks[pLinked].MovementCost);

                    float fStrightDistance = pLinked.DistanceTo(destination, fCycleShift);// (float)Math.Sqrt((destination.X - pLink.X) * (destination.X - pLink.X) + (destination.Y - pLink.Y) * (destination.Y - pLink.Y));

                    //if (pLink.m_pLocation.m_bRoad)// && !bInternal)
                    //    fStrightDistance /= 2;

                    queue.Enqueue(newPath.TotalCost + fStrightDistance * pLinked.GetMovementCost(), newPath);
                }
            }
            return null;
        }

        public PathFinder(TransportationNode pFrom, TransportationNode pTo, float fCycleShift, int iPassword, bool bNaval)
        {
            List<TransportationNode> cResult = new List<TransportationNode>();

            if(pFrom == null || pTo == null)
            {
                m_cPath = cResult.ToArray();
                return;
            }

            Path<TransportationNode> pPath = FindPath(pFrom, pTo, fCycleShift, iPassword, bNaval);

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
            m_cPath = cResult.ToArray();

            return;
        }
    }
}
