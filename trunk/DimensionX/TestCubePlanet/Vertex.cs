using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestCubePlanet
{
    public class Vertex
    {
        public float m_fX;
        public float m_fY;
        public float m_fZ;

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

        public Microsoft.Xna.Framework.Color m_eColor = Microsoft.Xna.Framework.Color.White;

        public Vertex(Vertex pOriginal)
        {
            m_fX = pOriginal.m_fX;
            m_fY = pOriginal.m_fY;
            m_fZ = pOriginal.m_fZ;
            m_eColor = pOriginal.m_eColor;
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
        public Vertex(float fX, float fY, float fSize, Cube.Face3D eFace, float fR)
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
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1}, {2}]", m_fX, m_fY, m_fZ);
        }
    }
}
