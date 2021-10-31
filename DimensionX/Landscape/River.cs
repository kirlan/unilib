using Random;
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

    public class River<LOC, LAND, LTI> : IRiver
        where LOC : Location, new()
        where LAND : Land<LOC, LTI>, new()
        where LTI : LandTypeInfo, new()
    {
        private string m_sName;

        /// <summary>
        /// имя реки
        /// </summary>
        public string Name { get => m_sName; set => m_sName = value; }

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
        private float m_fLength = 0f;

        public float Length { get => m_fLength; }

        /// <summary>
        /// локация в море, куда впадает река
        /// </summary>
        private LOC m_pMouth = null;

        public River(LOC pMouth)
        {
            m_pMouth = pMouth;

            Dictionary<VoronoiVertex, float> cNextRiverVertexes = new Dictionary<VoronoiVertex, float>();
            // составим список возможных вертексов начала реки на базе списка крайних вертексов стартовой локации
            var pEdge = pMouth.m_pFirstLine;
            do
            {
                float fChances = GetRiverVertexChance(pEdge.m_pPoint1);
                if (pEdge.m_pPoint1.H >= 0f && fChances > 0f)
                    cNextRiverVertexes[pEdge.m_pPoint1] = fChances;

                pEdge = pEdge.m_pNext;
            }
            while (pEdge != pMouth.m_pFirstLine);

            // пока список возможных вертексов не оустеет - наращиваем реку
            while (cNextRiverVertexes.Count > 0)
            {
                int iChances = Rnd.ChooseOne(cNextRiverVertexes.Values);
                VoronoiVertex pNextRiverVertex = cNextRiverVertexes.Keys.ElementAt(iChances);
                if (!AddVertex(pNextRiverVertex))
                    break;

                // составим список возможных вертексов продолжения реки на базе списка соседних вертексов последнего найденного вертекса
                cNextRiverVertexes.Clear();
                foreach (var pVertex in pNextRiverVertex.m_cVertexes)
                {
                    float fChances = GetRiverVertexChance(pVertex);
                    if (pVertex.H > pNextRiverVertex.H && fChances > 0f)
                        cNextRiverVertexes[pVertex] = fChances;
                }
            }
        }

        /// <summary>
        /// Вычисляет шансы указанного вертекса стать частью реки. 
        /// Шансы прямо пропорциональны средней влажности, но только если вертекс не касается воды - сроме самого первого, ему можно.
        /// </summary>
        /// <param name="pVertex"></param>
        /// <param name="pThisLocation"></param>
        /// <returns></returns>
        private float GetRiverVertexChance(VoronoiVertex pVertex)
        {
            bool bForbidden = false;
            float fAverageHumidity = 0f;
            foreach (LOC pLoc in pVertex.m_cLocations)
            {
                if (m_cVertexes.Count == 0 && pLoc == m_pMouth)
                    continue;

                if (pLoc.Forbidden || pLoc.Owner == null || (pLoc.Owner as LAND).Type.m_eEnvironment.HasFlag(Environment.Liquid))
                {
                    bForbidden = true;
                    break;
                }

                fAverageHumidity += (pLoc.Owner as LAND).Humidity;
            }
            fAverageHumidity /= pVertex.m_cLocations.Count;

            if (bForbidden)
                fAverageHumidity = 0f;

            return fAverageHumidity;
        }

        internal bool AddVertex(VoronoiVertex pNextRiverVertex)
        {
            if (m_cVertexes.Count > 0)
            {
                Location.Edge pEdge1 = null;
                Location.Edge pEdge2 = null;

                foreach (LOC pLoc in pNextRiverVertex.m_cLocations)
                {
                    foreach (var pLink in pLoc.BorderWith)
                    {
                        if (pLink.Value[0].m_pPoint1 == m_cVertexes.Last() &&
                            pLink.Value[0].m_pPoint2 == pNextRiverVertex)
                            pEdge1 = pLink.Value[0];
                        else if (pLink.Value[0].m_pPoint2 == m_cVertexes.Last() &&
                            pLink.Value[0].m_pPoint1 == pNextRiverVertex)
                            pEdge2 = pLink.Value[0];
                    }
                }

                if (pEdge1 == null || pEdge2 == null)
                    throw new Exception("No valid edge found!");

                m_fLength += pEdge1.Length;

                pEdge1.m_pRiver = this;
                pEdge2.m_pRiver = this;
            }
            m_cVertexes.Add(pNextRiverVertex);

            if (pNextRiverVertex.Depth > 0)
                return false;

            pNextRiverVertex.Depth = 1;
            return true;
        }
    }
}
