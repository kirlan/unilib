using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace LandscapeGeneration.PlanetBuilder
{
    public class CubeFace<LOC>
        where LOC : Location, new()
    {
        public int Resolution;

        public Chunk<LOC>[,] m_cChunk;

        public CubeFace(int iResolution, Rect pBounds2D, ref VertexCH[] locations, ref CellCH[] vertices, float fSize)
        {
            Resolution = iResolution;

            m_cChunk = new Chunk<LOC>[Resolution, Resolution];

            for (int x = 0; x < Resolution; x++)
                for (int y = 0; y < Resolution; y++)
                    m_cChunk[x, y] = new Chunk<LOC>(ref locations, pBounds2D, ref vertices, fSize * x, fSize * y, fSize * Resolution);
        }

        public Chunk<LOC> GetChunk(int x, int y)
        {
            Chunk<LOC> pChunk = m_cChunk[x, y];

            return pChunk;
        }
    }
}
