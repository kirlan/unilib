using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace TestCubePlanet
{
    public class CubeFace
    {
        public int Resolution;

        public Chunk[,] m_cChunk;

        public CubeFace(int iResolution, Rect pBounds2D, ref VertexCH[] locations, ref CellCH[] vertices, float fSize, int iR, Cube.Face3D eFace)
        {
            Resolution = iResolution;

            m_cChunk = new Chunk[Resolution, Resolution];

            for (int x = 0; x < Resolution; x++)
                for (int y = 0; y < Resolution; y++ )
                    m_cChunk[x, y] = new Chunk(ref locations, pBounds2D, ref vertices, fSize * x, fSize * y, fSize * Resolution, iR, eFace);
        }

        public Chunk GetChunk(int x, int y, VertexCH.Transformation eTransform)
        {
            Chunk pChunk = m_cChunk[x, y]; ;
            switch (eTransform)
            {
                case VertexCH.Transformation.Rotate90CCW:
                    pChunk = m_cChunk[y, Resolution - 1 - x];
                    break;
                case VertexCH.Transformation.Rotate90CW:
                    pChunk = m_cChunk[Resolution - 1 - y, x];
                    break;
                case VertexCH.Transformation.Rotate180:
                    pChunk = m_cChunk[Resolution - 1 - x, Resolution - 1 - y];
                    break;
            }

            return pChunk;
        }

        public void LinkNeighbours(CubeFace pTopLeft, VertexCH.Transformation eTopLeft, 
                                    CubeFace pTop, VertexCH.Transformation eTop,
                                    CubeFace pTopRight, VertexCH.Transformation eTopRight, 
                                    CubeFace pRight, VertexCH.Transformation eRight,
                                    CubeFace pBottomRight, VertexCH.Transformation eBottomRight, 
                                    CubeFace pBottom, VertexCH.Transformation eBottom,
                                    CubeFace pBottomLeft, VertexCH.Transformation eBottomLeft,
                                    CubeFace pLeft, VertexCH.Transformation eLeft)
        {
            for (int x = 0; x < Resolution; x++)
                for (int y = 0; y < Resolution; y++)
                {
                    if (x > 0 && y > 0)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.DownLeft] = new Chunk.NeighbourInfo(m_cChunk[x - 1, y - 1], VertexCH.Transformation.Stright);
                    else
                    {
                        if (pBottomLeft != null)
                        {
                            Chunk pChunk = pBottomLeft.GetChunk(pBottomLeft.Resolution - 1, pBottomLeft.Resolution - 1, eBottomLeft);
                            m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.DownLeft] = new Chunk.NeighbourInfo(pChunk, eBottomLeft);
                        }
                    }

                    if (y > 0)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.Down] = new Chunk.NeighbourInfo(m_cChunk[x, y - 1], VertexCH.Transformation.Stright);
                    else
                    {
                        if (pBottom != null)
                        {
                            Chunk pChunk = pBottom.GetChunk(x, pBottom.Resolution - 1, eBottom);
                            m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.Down] = new Chunk.NeighbourInfo(pChunk, eBottom);
                        }
                    }
                    
                    if (x < Resolution - 1 && y > 0)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.DownRight] = new Chunk.NeighbourInfo(m_cChunk[x + 1, y - 1], VertexCH.Transformation.Stright);
                    else
                    {
                        if (pBottomRight != null)
                        {
                            Chunk pChunk = pBottomRight.GetChunk(0, pBottomRight.Resolution - 1, eBottomRight);
                            m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.DownRight] = new Chunk.NeighbourInfo(pChunk, eBottomRight);
                        }
                    }
                    
                    if (x < Resolution - 1)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.Right] = new Chunk.NeighbourInfo(m_cChunk[x + 1, y], VertexCH.Transformation.Stright);
                    else
                    {
                        if (pRight != null)
                        {
                            Chunk pChunk = pRight.GetChunk(0, y, eRight);
                            m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.Right] = new Chunk.NeighbourInfo(pChunk, eRight);
                        }
                    }
                    
                    if (x < Resolution - 1 && y < Resolution - 1)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.UpRight] = new Chunk.NeighbourInfo(m_cChunk[x + 1, y + 1], VertexCH.Transformation.Stright);
                    else
                    {
                        if (pTopRight != null)
                        {
                            Chunk pChunk = pTopRight.GetChunk(0, 0, eTopRight);
                            m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.UpRight] = new Chunk.NeighbourInfo(pChunk, eTopRight);
                        }
                    }
                    
                    if (y < Resolution - 1)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.Up] = new Chunk.NeighbourInfo(m_cChunk[x, y + 1], VertexCH.Transformation.Stright);
                    else
                    {
                        if (pTop != null)
                        {
                            Chunk pChunk = pTop.GetChunk(x, 0, eTop);
                            m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.Up] = new Chunk.NeighbourInfo(pChunk, eTop);
                        }
                    }
                    
                    if (x > 0 && y < Resolution - 1)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.UpLeft] = new Chunk.NeighbourInfo(m_cChunk[x - 1, y + 1], VertexCH.Transformation.Stright);
                    else
                    {
                        if (pTopLeft != null)
                        {
                            Chunk pChunk = pTopLeft.GetChunk(pTopLeft.Resolution - 1, 0, eTopLeft);
                            m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.UpLeft] = new Chunk.NeighbourInfo(pChunk, eTopLeft);
                        }
                    }
                    
                    if (x > 0)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.Left] = new Chunk.NeighbourInfo(m_cChunk[x - 1, y], VertexCH.Transformation.Stright);
                    else
                    {
                        if (pLeft != null)
                        {
                            Chunk pChunk = pLeft.GetChunk(pLeft.Resolution - 1, y, eLeft);
                            m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.Left] = new Chunk.NeighbourInfo(pChunk, eLeft);
                        }
                    }
                }

            foreach (var pChunk in m_cChunk)
                pChunk.Ghostbusters();
        }

        public void Finalize()
        {
            foreach (var pChunk in m_cChunk)
                pChunk.Finalize();
        }
    }
}
