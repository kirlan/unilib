using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestCubePlanet
{
    public class CubeFace
    {
        public int Size;

        public Chunk[,] m_cChunk;

        public CubeFace(int iSize, ref VertexCH[] locations, ref CellCH[] vertices, float fSize, Cube.Face3D eFace)
        {
            Size = iSize;

            m_cChunk = new Chunk[Size, Size];

            for (int x = 0; x < Size; x++)
                for (int y = 0; y < Size; y++ )
                    m_cChunk[x, y] = new Chunk(ref locations, ref vertices, fSize * x, fSize * y, fSize * Size / 2, eFace);

            for (int x = 0; x < Size; x++)
                for (int y = 0; y < Size; y++)
                {
                    if (x > 0 && y > 0)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.UpLeft] = new Chunk.NeighbourInfo(m_cChunk[x - 1, y - 1], VertexCH.Transformation.Stright);
                    if (y > 0)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.Up] = new Chunk.NeighbourInfo(m_cChunk[x, y - 1], VertexCH.Transformation.Stright);
                    if (x < Size - 1 && y > 0)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.UpRight] = new Chunk.NeighbourInfo(m_cChunk[x + 1, y - 1], VertexCH.Transformation.Stright);
                    if (x < Size - 1)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.Right] = new Chunk.NeighbourInfo(m_cChunk[x + 1, y], VertexCH.Transformation.Stright);
                    if (x < Size - 1 && y < Size - 1)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.DownRight] = new Chunk.NeighbourInfo(m_cChunk[x + 1, y + 1], VertexCH.Transformation.Stright);
                    if (y < Size - 1)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.Down] = new Chunk.NeighbourInfo(m_cChunk[x, y + 1], VertexCH.Transformation.Stright);
                    if (x > 0 && y < Size - 1)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.DownLeft] = new Chunk.NeighbourInfo(m_cChunk[x - 1, y + 1], VertexCH.Transformation.Stright);
                    if (x > 0)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.Left] = new Chunk.NeighbourInfo(m_cChunk[x - 1, y], VertexCH.Transformation.Stright);
                }

            foreach (var pChunk in m_cChunk)
                pChunk.Ghostbusters();

            //foreach (var pChunk in m_cChunk)
            //    pChunk.FixEdges();
        }
    }
}
