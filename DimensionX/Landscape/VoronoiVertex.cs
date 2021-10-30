using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using BenTools.Mathematics;
using System.IO;
using LandscapeGeneration.PathFind;

namespace LandscapeGeneration
{
    public class VoronoiVertex : IPointF
    {
        public float m_fX = 0;
        public float m_fY = 0;
        
        public float m_fH = float.NaN;

        public float X
        {
            get { return m_fX; }
            set { m_fX = value; }
        }

        public float Y
        {
            get { return m_fY; }
            set { m_fY = value; }
        }

        public float H
        {
            get { return m_fH; }
            set { m_fH = value; }
        }

        private static long s_iCounter = 0;

        public long m_iVertexID = s_iCounter++;

        public List<VoronoiVertex> m_cVertexes = new List<VoronoiVertex>();

        public List<long> m_cLinksTmp = new List<long>();

        /// <summary>
        /// Список локаций, соприкасающихся с вершиной.
        /// Используется при настройке связей между квадратами (в Vertex::Replace())
        /// </summary>
        public List<Location> m_cLocations = new List<Location>();

        internal List<Location> m_cLocationsBuild = new List<Location>();

        public List<long> m_cLocationsTmp = new List<long>();

        /// <summary>
        /// Чанк, в который входит данная вершина. 
        /// Используется в Chunk::RebuildVertexArray() при просмотре соседей как маркер того, что эта вершина уже была обработана.
        /// </summary>
        public object m_pChunkMarker = null;

        /// <summary>
        /// Заменяет ссылку на текущую вершину во всех связанных локациях.
        /// Вызывается из Chunk::ReplaceVertexes()
        /// </summary>
        /// <param name="pGood">"правильная" вершина</param>
        public void Replace(VoronoiVertex pGood)
        {
            List<Location> cTemp = new List<Location>(m_cLocations);
            //проходим по всем локациям, связанным к "неправильной" вершиной
            foreach (Location pLinkedLoc in cTemp)
                pLinkedLoc.ReplaceVertex(this, pGood);
        }

        public VoronoiVertex()
        {
            m_fX = 0f;
            m_fY = 0f;
        }

        public VoronoiVertex(float fX, float fY)
        {
            m_fX = fX;
            m_fY = fY;
        }

        public VoronoiVertex(BTVector pVector)
        {
            m_fX = (float)pVector.data[0];
            m_fY = (float)pVector.data[1];
        }

        public VoronoiVertex(VoronoiVertex pVector)
        {
            m_fX = pVector.X;
            m_fY = pVector.Y;
        }

        public VoronoiVertex(BinaryReader binReader)
        {
            m_iVertexID = binReader.ReadInt64();

            m_fX = (float)binReader.ReadDouble();
            m_fY = (float)binReader.ReadDouble();


            int iLinksCount = binReader.ReadInt32();
            for (int i = 0; i < iLinksCount; i++)
                m_cLinksTmp.Add(binReader.ReadInt64());

            int iLocationsCount = binReader.ReadInt32();
            for (int i = 0; i < iLocationsCount; i++)
                m_cLocationsTmp.Add(binReader.ReadInt64());
        }

        public void Save(BinaryWriter binWriter)
        {
            binWriter.Write(m_iVertexID);

            binWriter.Write((double)m_fX);
            binWriter.Write((double)m_fY);

            binWriter.Write(m_cVertexes.Count);
            foreach (VoronoiVertex pVertex in m_cVertexes)
                binWriter.Write(pVertex.m_iVertexID);

            binWriter.Write(m_cLocationsBuild.Count);
            foreach (Location pLocation in m_cLocationsBuild)
                binWriter.Write(pLocation.m_iID);
        }

        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}, H: {2}", X, Y, H);
        }

        //public override float GetMovementCost()
        //{
        //    return 100;
        //}

        internal void PointOnCurve(VoronoiVertex p0, VoronoiVertex p1, VoronoiVertex p2, VoronoiVertex p3, float t, float fCycle, float smoothRate)
        {
            for (int i = 0; i < m_cLocations.Count; i++)
                if (m_cLocations[i].Forbidden || m_cLocations[i].Owner == null)
                    return;

            if (smoothRate > 1.0f)
                smoothRate = 1.0f;
            if (smoothRate < 0.0f)
                smoothRate = 0.0f;

            float t2 = t * t; 
            float t3 = t2 * t;

            float p0X = p0.X;
            float p0Y = p0.Y;
            float p1X = p1.X;
            float p1Y = p1.Y;
            float p2X = p2.X;
            float p2Y = p2.Y;
            float p3X = p3.X;
            float p3Y = p3.Y;

            if (fCycle != 0.0f)
            {
                if (p0X + fCycle / 2 < m_fX)
                    p0X += fCycle;
                if (p0X - fCycle / 2 > m_fX)
                    p0X -= fCycle;

                if (p1X + fCycle / 2 < m_fX)
                    p1X += fCycle;
                if (p1X - fCycle / 2 > m_fX)
                    p1X -= fCycle;

                if (p2X + fCycle / 2 < m_fX)
                    p2X += fCycle;
                if (p2X - fCycle / 2 > m_fX)
                    p2X -= fCycle;

                if (p3X + fCycle / 2 < m_fX)
                    p3X += fCycle;
                if (p3X - fCycle / 2 > m_fX)
                    p3X -= fCycle;
            }

            float newX = 0.5f * ((2.0f * p1X) + (-p0X + p2X) * t + 
                (2.0f * p0X - 5.0f * p1X + 4 * p2X - p3X) * t2 + 
                (-p0X + 3.0f * p1X - 3.0f * p2X + p3X) * t3); 

            float newY = 0.5f * ((2.0f * p1Y) + (-p0Y + p2Y) * t + 
                (2.0f * p0Y - 5.0f * p1Y + 4 * p2Y - p3Y) * t2 + 
                (-p0Y + 3.0f * p1Y - 3.0f * p2Y + p3Y) * t3);

            m_fX = m_fX * (1.0f - smoothRate) + newX * smoothRate;
            m_fY = m_fY * (1.0f - smoothRate) + newY * smoothRate;

            if (fCycle != 0.0f)
            {
                while (m_fX > fCycle / 2)
                    m_fX -= fCycle;
                while (m_fX < -fCycle / 2)
                    m_fX += fCycle;
            }

            if (m_fX < -100000) //TODO: здесь должно быть RX*2 !!!
                throw new Exception();
        }
    }
}
