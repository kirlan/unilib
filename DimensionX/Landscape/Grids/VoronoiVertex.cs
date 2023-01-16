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
    public class VoronoiVertex
    {
        public virtual float X { get; set; } = 0;
        public virtual float Y { get; set; } = 0;
        public virtual float H { get; set; } = float.NaN;
        public virtual float Depth { get; set; } = 0;

        private static long s_iCounter = 0;

        public long VertexID { get; } = s_iCounter++;

        public List<VoronoiVertex> LinkedVertexes { get; } = new List<VoronoiVertex>();

        /// <summary>
        /// Список локаций, соприкасающихся с вершиной.
        /// Используется при настройке связей между квадратами (в Vertex::Replace())
        /// </summary>
        public List<Location> Locations { get; } = new List<Location>();

        //internal List<Location> m_cLocationsBuild = new List<Location>();

        //public List<long> m_cLocationsTmp = new List<long>();

        /// <summary>
        /// Чанк, в который входит данная вершина.
        /// Используется в Chunk::RebuildVertexArray() при просмотре соседей как маркер того, что эта вершина уже была обработана.
        /// </summary>
        public object ChunkMarker { get; set; } = null;

        /// <summary>
        /// Заменяет ссылку на текущую вершину во всех связанных локациях.
        /// Вызывается из Chunk::ReplaceVertexes()
        /// </summary>
        /// <param name="pGood">"правильная" вершина</param>
        public void Replace(VoronoiVertex pGood)
        {
            List<Location> cTemp = new List<Location>(Locations);
            //проходим по всем локациям, связанным к "неправильной" вершиной
            foreach (Location pLinkedLoc in cTemp)
                pLinkedLoc.ReplaceVertex(this, pGood);

            foreach (var pLinkedVertex in LinkedVertexes)
            {
                pLinkedVertex.LinkedVertexes.Remove(this);
                if (!pLinkedVertex.LinkedVertexes.Contains(pGood))
                    pLinkedVertex.LinkedVertexes.Add(pGood);

                if (!pGood.LinkedVertexes.Contains(pLinkedVertex))
                    pGood.LinkedVertexes.Add(pLinkedVertex);
            }
        }

        public VoronoiVertex()
        {
            X = 0f;
            Y = 0f;
        }

        public VoronoiVertex(float fX, float fY)
        {
            X = fX;
            Y = fY;
        }

        public VoronoiVertex(BTVector pVector)
        {
            X = (float)pVector.data[0];
            Y = (float)pVector.data[1];
        }

        public VoronoiVertex(VoronoiVertex pVector)
        {
            X = pVector.X;
            Y = pVector.Y;
        }

        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}, H: {2}", X, Y, H);
        }

        internal void PointOnCurve(VoronoiVertex p0, VoronoiVertex p1, VoronoiVertex p2, VoronoiVertex p3, float t, float fCycle, float smoothRate)
        {
            for (int i = 0; i < Locations.Count; i++)
            {
                if (Locations[i].Forbidden || !Locations[i].HasOwner())
                    return;
            }

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
                if (p0X + fCycle / 2 < X)
                    p0X += fCycle;
                if (p0X - fCycle / 2 > X)
                    p0X -= fCycle;

                if (p1X + fCycle / 2 < X)
                    p1X += fCycle;
                if (p1X - fCycle / 2 > X)
                    p1X -= fCycle;

                if (p2X + fCycle / 2 < X)
                    p2X += fCycle;
                if (p2X - fCycle / 2 > X)
                    p2X -= fCycle;

                if (p3X + fCycle / 2 < X)
                    p3X += fCycle;
                if (p3X - fCycle / 2 > X)
                    p3X -= fCycle;
            }

            float newX = 0.5f * ((2.0f * p1X) + (-p0X + p2X) * t +
                (2.0f * p0X - 5.0f * p1X + 4 * p2X - p3X) * t2 +
                (-p0X + 3.0f * p1X - 3.0f * p2X + p3X) * t3);

            float newY = 0.5f * ((2.0f * p1Y) + (-p0Y + p2Y) * t +
                (2.0f * p0Y - 5.0f * p1Y + 4 * p2Y - p3Y) * t2 +
                (-p0Y + 3.0f * p1Y - 3.0f * p2Y + p3Y) * t3);

            X = X * (1.0f - smoothRate) + newX * smoothRate;
            Y = Y * (1.0f - smoothRate) + newY * smoothRate;

            if (fCycle != 0.0f)
            {
                while (X > fCycle / 2)
                    X -= fCycle;
                while (X < -fCycle / 2)
                    X += fCycle;
            }

            if (X < -100000) //TODO: здесь должно быть RX*2 !!!
                throw new InvalidOperationException();
        }
    }
}
