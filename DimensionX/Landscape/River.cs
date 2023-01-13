﻿using Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandscapeGeneration
{
    public interface IRiver
    {
        string Name { get; set; }
        VoronoiVertex[] Vertexes { get; }
        float Length { get; }
    }

    public class River : IRiver
    {
        /// <summary>
        /// имя реки
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// список вертексов, составляющих реку
        /// </summary>
        private readonly List<VoronoiVertex> m_cVertexes = new List<VoronoiVertex>();

        public VoronoiVertex[] Vertexes
        {
            get { return m_cVertexes.ToArray(); }
        }

        /// <summary>
        /// длина реки
        /// </summary>
        public float Length { get; private set; }

        public bool Finished { get; } = false;

        /// <summary>
        /// локация в море, куда впадает река
        /// </summary>
        private Location m_pSource = null;

        public River(Location pSource)
        {
            m_pSource = pSource;

            Dictionary<VoronoiVertex, float> cNextRiverVertexes = new Dictionary<VoronoiVertex, float>();
            // составим список возможных вертексов начала реки на базе списка крайних вертексов стартовой локации
            var pEdge = pSource.FirstLine;
            do
            {
                float fAverageHum = GetRiverChances(pEdge.Point1);
                if (pEdge.Point1.H >= 0f && fAverageHum > 0.0f)
                    cNextRiverVertexes[pEdge.Point1] = fAverageHum;

                pEdge = pEdge.Next;
            }
            while (pEdge != pSource.FirstLine);

            // пока список возможных вертексов не опустеет - наращиваем реку
            while (cNextRiverVertexes.Count > 0)
            {
                int iChances = Rnd.ChooseBest(cNextRiverVertexes.Values);
                VoronoiVertex pNextRiverVertex = cNextRiverVertexes.Keys.ElementAt(iChances);
                if (!AddVertex(pNextRiverVertex))
                {
                    Finished = true;
                    break;
                }

                // составим список возможных вертексов продолжения реки на базе списка соседних вертексов последнего найденного вертекса
                cNextRiverVertexes.Clear();
                foreach (var pPossibleNextVertex in pNextRiverVertex.LinkedVertexes)
                {
                    float fAverageHum = GetRiverChances(pPossibleNextVertex);
                    if (fAverageHum > 0.0f)
                        cNextRiverVertexes[pPossibleNextVertex] = fAverageHum * (float)Math.Exp(pNextRiverVertex.H - pPossibleNextVertex.H);
                }

                if (cNextRiverVertexes.Count == 0)
                    break;
            }
        }

        /// <summary>
        /// Вычисляет шансы указанного вертекса стать частью реки. 
        /// Шансы прямо пропорциональны средней влажности, но только если вертекс не касается воды - сроме самого первого, ему можно.
        /// </summary>
        /// <param name="pVertex"></param>
        /// <param name="pThisLocation"></param>
        /// <returns></returns>
        private float GetRiverChances(VoronoiVertex pVertex)
        {
            if (pVertex.Locations.Count == 0)
                return 0.0f;

            if (pVertex.Depth > 0)
                return 0.0f;

            int iTotalRivers = 0;
            foreach (VoronoiVertex pLinkedVertex in pVertex.LinkedVertexes)
            {
                if (pLinkedVertex.Depth > 0)
                    iTotalRivers++;
            }

            if (iTotalRivers > 1)
                return 0.0f;

            float fTotalHumidity = 0f;
            iTotalRivers = 0;
            //Посчитаем среднее арифметическое влажности всех прилегающих к вертексу локаций
            foreach (Location pLoc in pVertex.Locations)
            {
                if (m_cVertexes.Count == 0 && pLoc == m_pSource)
                    continue;

                if (pLoc.Forbidden || !pLoc.HasOwner())
                    return 0.0f;

                if (pLoc.GetOwner().LandType.Environment.HasFlag(Environments.Liquid))
                    fTotalHumidity += 10000.0f;
                else
                    fTotalHumidity += pLoc.GetOwner().Humidity;

                foreach (var pLinked in pLoc.BorderWith)
                {
                    if (pLinked.Key.HasOwner() && pLinked.Key.GetOwner().LandType.Environment.HasFlag(Environments.Liquid))
                        fTotalHumidity += 5000.0f;

                    if (pLinked.Value[0].Point1.Depth > 0 || pLinked.Value[0].Point2.Depth > 0)
                        iTotalRivers++;
                }
            }

            //if (iTotalRivers > 0)
            //    return 0.0f;

            fTotalHumidity /= iTotalRivers * 10 + 1;

            float fAverageHumidity = fTotalHumidity / pVertex.Locations.Count;

            return fAverageHumidity * fAverageHumidity / 100.0f;
        }

        private readonly List<VoronoiEdge> m_cEdges = new List<VoronoiEdge>();

        internal bool AddVertex(VoronoiVertex pNextRiverVertex)
        {
            bool bMouth = false;
            if (m_cVertexes.Count > 0)
            {
                VoronoiEdge pEdge1 = null;
                VoronoiEdge pEdge2 = null;

                foreach (Location pLoc in pNextRiverVertex.Locations)
                {
                    foreach (var pLink in pLoc.BorderWith.Values)
                    {
                        if (pLink[0].Point1 == m_cVertexes.Last() &&
                            pLink[0].Point2 == pNextRiverVertex)
                        {
                            pEdge1 = pLink[0];
                        }
                        else if (pLink[0].Point2 == m_cVertexes.Last() &&
                                 pLink[0].Point1 == pNextRiverVertex)
                        {
                            pEdge2 = pLink[0];
                        }
                    }

                    if (pLoc.GetOwner().LandType.Environment.HasFlag(Environments.Liquid))
                        bMouth = true;
                }

                if (pEdge1 == null || pEdge2 == null)
                    throw new InvalidOperationException("No valid edge found!");

                Length += pEdge1.Length;

                pEdge1.River = this;
                pEdge2.River = this;

                m_cEdges.Add(pEdge1);
                m_cEdges.Add(pEdge2);
            }
            m_cVertexes.Add(pNextRiverVertex);

            if (pNextRiverVertex.Depth > 0 || bMouth)
                return false;

            pNextRiverVertex.Depth = 1;
            return true;
        }

        public void Remove()
        {
            foreach (VoronoiEdge pEdge in m_cEdges)
            {
                pEdge.River = null;
            }
            m_cEdges.Clear();

            foreach (VoronoiVertex pVertex in Vertexes)
            {
                pVertex.Depth = 0;
            }
        }
    }
}
