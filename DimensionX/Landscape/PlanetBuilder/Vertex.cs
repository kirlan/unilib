using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace LandscapeGeneration.PlanetBuilder
{
    public class Vertex : IPointF
    {
        public float m_fX;
        public float m_fY;
        public float m_fZ;
        public float m_fXN;
        public float m_fYN;
        public float m_fZN;
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

        public float Z
        {
            get { return m_fZ; }
            set { m_fZ = value; }
        }

        public float H
        {
            get { return m_fH; }
            set { m_fH = value; }
        }

        public float m_fRndBig = 0.7f + Rnd.Get(0.6f);
        public float m_fRndSmall = Rnd.Get(0.2f);

        /// <summary>
        /// Список локаций, соприкасающихся с вершиной.
        /// Используется при настройке связей между квадратами (в Vertex::Replace())
        /// </summary>
        public List<Location> m_cLocations = new List<Location>();

        /// <summary>
        /// Чанк, в который входит данная вершина. 
        /// Используется в Chunk::RebuildVertexArray() при просмотре соседей как маркер того, что эта вершина уже была обработана.
        /// </summary>
        public object m_pChunkMarker = null;

        public Vertex(Vertex pOriginal)
        {
            m_fX = pOriginal.m_fX;
            m_fY = pOriginal.m_fY;
            m_fZ = pOriginal.m_fZ;

            m_fXN = pOriginal.m_fXN;
            m_fYN = pOriginal.m_fYN;
            m_fZN = pOriginal.m_fZN;

            m_fH = pOriginal.m_fH;
        }

        /// <summary>
        /// Заменяет ссылку на текущую вершину во всех связанных локациях.
        /// Вызывается из Chunk::ReplaceVertexes()
        /// </summary>
        /// <param name="pGood">"правильная" вершина</param>
        public void Replace(Vertex pGood)
        {
            List<Location> cTemp = new List<Location>(m_cLocations);
            //проходим по всем локациям, связанным к "неправильной" вершиной
            foreach (Location pLinkedLoc in cTemp)
                pLinkedLoc.ReplaceVertex(this, pGood);
        }

        public Vertex()
        { }

        /// <summary>
        /// Размещает 2D-точку (fX, fY) на кубической сфере.
        /// Считаем, что (0, 0) - центр грани.
        /// </summary>
        /// <param name="fX">X координата 2D точки</param>
        /// <param name="fY">Y координата 2D точки</param>
        /// <param name="fSize">половина длины ребра куба</param>
        /// <param name="eFace">грань куба, на которой находится 2D точка</param>
        /// <param name="fR">радиус сферы</param>
        public Vertex(float fX, float fY, float fSize, CubeFace3D eFace, float fR, bool bHighRes)
        {
            Create(fX, fY, fSize, eFace, fR, bHighRes);
        }

        /// <summary>
        /// Размещает 2D-точку (fX, fY) на кубической сфере.
        /// Считаем, что (0, 0) - центр грани.
        /// </summary>
        /// <param name="fX">X координата 2D точки</param>
        /// <param name="fY">Y координата 2D точки</param>
        /// <param name="fSize">половина длины ребра куба</param>
        /// <param name="eFace">грань куба, на которой находится 2D точка</param>
        /// <param name="fR">радиус сферы</param>
        public void Create(float fX, float fY, float fSize, CubeFace3D eFace, float fR, bool bHighRes)
        {
            //Формула взята на http://mathproofs.blogspot.ru/2005/07/mapping-cube-to-sphere.html
            float x = 1;
            float y = 1;
            float z = 1;
            switch (eFace)
            {
                case CubeFace3D.Forward:
                    x = fX / fSize;
                    y = fY / fSize;
                    z = -1;
                    break;
                case CubeFace3D.Right:
                    x = 1;
                    y = fY / fSize;
                    z = fX / fSize;
                    break;
                case CubeFace3D.Backward:
                    x = -fX / fSize;
                    y = fY / fSize;
                    z = 1;
                    break;
                case CubeFace3D.Left:
                    x = -1;
                    y = fY / fSize;
                    z = -fX / fSize;
                    break;
                case CubeFace3D.Top:
                    x = fX / fSize;
                    y = 1;
                    z = fY / fSize;
                    break;
                case CubeFace3D.Bottom:
                    x = fX / fSize;
                    y = -1;
                    z = -fY / fSize;
                    break;
            }
            //минус - потому что XNA оперирует правой системой координат, а не левой
            m_fX = -fR * x * (float)Math.Sqrt(1 - y * y / 2 - z * z / 2 + y * y * z * z / 3);
            m_fY = fR * y * (float)Math.Sqrt(1 - x * x / 2 - z * z / 2 + x * x * z * z / 3);
            m_fZ = fR * z * (float)Math.Sqrt(1 - x * x / 2 - y * y / 2 + x * x * y * y / 3);

            //m_fH = 0;

            //if (bHighRes)
            //{
            //    m_fH = (float)ClassicNoise.noise(m_fX / 3, m_fY / 3, m_fZ / 3) / 5 + (float)ClassicNoise.noise(m_fX / 10, m_fY / 10, m_fZ / 10) / 2 + (float)ClassicNoise.noise(m_fX / 50, m_fY / 50, m_fZ / 50) * 2;
            //    if (m_fH > 0.0001)
            //        m_fH *= m_fH * m_fH;
            //    else
            //        m_fH = m_fH * m_fH * m_fH - 0.1f;
            //}
            //else
            //    m_fH = -2;

            //m_fH *= 2;
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1}, {2}]", m_fX, m_fY, m_fZ);
        }

        //TODO: для замкнутых (цилиндрических, сферических) миров - вычислять угловые координаты и работать с ними
        internal void PointOnCurve(Vertex p0, Vertex p1, Vertex p2, Vertex p3, float t)
        {
            for (int i = 0; i < m_cLocations.Count; i++)
                if (m_cLocations[i].Forbidden || m_cLocations[i].Owner == null)
                    return;

            float t2 = t * t;
            float t3 = t2 * t;

            float p0X = p0.X;
            float p0Y = p0.Y;
            float p0Z = p0.Z;
            float p1X = p1.X;
            float p1Y = p1.Y;
            float p1Z = p1.Z;
            float p2X = p2.X;
            float p2Y = p2.Y;
            float p2Z = p2.Z;
            float p3X = p3.X;
            float p3Y = p3.Y;
            float p3Z = p3.Z;

            m_fX = (m_fX * 4 + 0.5f * ((2.0f * p1X) + (-p0X + p2X) * t +
                (2.0f * p0X - 5.0f * p1X + 4 * p2X - p3X) * t2 +
                (-p0X + 3.0f * p1X - 3.0f * p2X + p3X) * t3)) / 5;

            m_fY = (m_fY * 4 + 0.5f * ((2.0f * p1Y) + (-p0Y + p2Y) * t +
                (2.0f * p0Y - 5.0f * p1Y + 4 * p2Y - p3Y) * t2 +
                (-p0Y + 3.0f * p1Y - 3.0f * p2Y + p3Y) * t3)) / 5;

            m_fZ = (m_fZ * 4 + 0.5f * ((2.0f * p1Z) + (-p0Z + p2Z) * t +
                (2.0f * p0Z - 5.0f * p1Z + 4 * p2Z - p3Z) * t2 +
                (-p0Z + 3.0f * p1Z - 3.0f * p2Z + p3Z) * t3)) / 5;
        }
    }
}
