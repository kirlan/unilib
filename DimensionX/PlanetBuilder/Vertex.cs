using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using PlanetBuilder;
using Random;

namespace TestCubePlanet
{
    public class Vertex
    {
        public float m_fX;
        public float m_fY;
        public float m_fZ;
        public float m_fXN;
        public float m_fYN;
        public float m_fZN;
        public float m_fH = 0;

        public float m_fRndBig = 0.7f + Rnd.Get(0.6f);
        public float m_fRndSmall = Rnd.Get(0.2f);

        /// <summary>
        /// Список локаций, соприкасающихся с вершиной.
        /// Используется при настройке связей между квадратами (в Vertex::Replace())
        /// </summary>
        public List<Location> m_cLinked = new List<Location>();

        /// <summary>
        /// Чанк, в который входит данная вершина. 
        /// Используется в Chunk::RebuildVertexArray() при просмотре соседей как маркер того, что эта вершина уже была обработана.
        /// </summary>
        public Chunk m_pChunkMarker = null;

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
            List<Location> cTemp = new List<Location>(m_cLinked);
            //проходим по всем локациям, связанным к "неправильной" вершиной
            foreach (Location pLinkedLoc in cTemp)
                pLinkedLoc.ReplaceVertex(this, pGood);
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
        public Vertex(float fX, float fY, float fSize, Cube.Face3D eFace, float fR, bool bHighRes)
        {
            //Формула взята на http://mathproofs.blogspot.ru/2005/07/mapping-cube-to-sphere.html
            float x = 1;
            float y = 1;
            float z = 1;
            switch (eFace)
            {
                case Cube.Face3D.Forward:
                        x = fX / fSize;
                        y = fY / fSize;
                        z = -1;
                    break;
                case Cube.Face3D.Right:
                        x = 1;
                        y = fY / fSize;
                        z = fX / fSize;
                    break;
                case Cube.Face3D.Backward:
                        x = -fX / fSize;
                        y = fY / fSize;
                        z = 1;
                    break;
                case Cube.Face3D.Left:
                        x = -1;
                        y = fY / fSize;
                        z = -fX / fSize;
                    break;
                case Cube.Face3D.Top:
                        x = fX / fSize;
                        y = 1;
                        z = fY / fSize;
                    break;
                case Cube.Face3D.Bottom:
                        x = fX / fSize;
                        y = -1;
                        z = -fY / fSize;
                    break;
            }
            //минус - потому что XNA оперирует правой системой координат, а не левой
            m_fX = -fR * x *(float)Math.Sqrt(1 - y * y / 2 - z * z / 2 + y * y * z * z / 3);
            m_fY = fR * y * (float)Math.Sqrt(1 - x * x / 2 - z * z / 2 + x * x * z * z / 3);
            m_fZ = fR * z * (float)Math.Sqrt(1 - x * x / 2 - y * y / 2 + x * x * y * y / 3);

            if (bHighRes)
            {
                m_fH = (float)ClassicNoise.noise(m_fX / 3, m_fY / 3, m_fZ / 3) / 5 + (float)ClassicNoise.noise(m_fX / 10, m_fY / 10, m_fZ / 10) / 2 + (float)ClassicNoise.noise(m_fX / 50, m_fY / 50, m_fZ / 50) * 2;
                if (m_fH > 0.0001)
                    m_fH *= m_fH * m_fH;
                else
                    m_fH = m_fH * m_fH * m_fH - 0.1f;
            }
            else
                m_fH = -2;

            m_fH *= 2;
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1}, {2}]", m_fX, m_fY, m_fZ);
        }
    }
}
