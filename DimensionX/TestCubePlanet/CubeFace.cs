using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestCubePlanet
{
    public class CubeFace
    {
        public static int Size = 3;

        public Chunk[,] m_cChunk = new Chunk[Size, Size];

        public CubeFace(List<VertexCH> locations, List<CellCH> vertices, float fSize, Cube.Face3D eFace)
        {
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++ )
                    m_cChunk[i, j] = new Chunk(locations, vertices, fSize * i, fSize * j, fSize * Size / 2, eFace);
        }
    }
}
