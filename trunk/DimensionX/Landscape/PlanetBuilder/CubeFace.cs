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

        public bool m_bHighRes;

        public CubeFace(int iResolution, Rect pBounds2D, ref VertexCH[] locations, ref CellCH[] vertices, float fSize, int iR, CubeFace3D eFace, bool bHighRes)
        {
            Resolution = iResolution;

            m_bHighRes = bHighRes;

            m_cChunk = new Chunk<LOC>[Resolution, Resolution];

            for (int x = 0; x < Resolution; x++)
                for (int y = 0; y < Resolution; y++)
                    m_cChunk[x, y] = new Chunk<LOC>(ref locations, pBounds2D, ref vertices, fSize * x, fSize * y, fSize * Resolution, iR, eFace, bHighRes);
        }

        public Chunk<LOC> GetChunk(int x, int y, VertexCH.Transformation eTransform)
        {
            Chunk<LOC> pChunk = m_cChunk[x, y]; ;
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

        public void LinkNeighbours(CubeFace<LOC> pTopLeft, VertexCH.Transformation eTopLeft,
                                    CubeFace<LOC> pTop, VertexCH.Transformation eTop,
                                    CubeFace<LOC> pTopRight, VertexCH.Transformation eTopRight,
                                    CubeFace<LOC> pRight, VertexCH.Transformation eRight,
                                    CubeFace<LOC> pBottomRight, VertexCH.Transformation eBottomRight,
                                    CubeFace<LOC> pBottom, VertexCH.Transformation eBottom,
                                    CubeFace<LOC> pBottomLeft, VertexCH.Transformation eBottomLeft,
                                    CubeFace<LOC> pLeft, VertexCH.Transformation eLeft)
        {
            for (int x = 0; x < Resolution; x++)
                for (int y = 0; y < Resolution; y++)
                {
                    if (x > 0 && y > 0)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.DownLeft] = new Chunk<LOC>.NeighbourInfo(m_cChunk[x - 1, y - 1], VertexCH.Transformation.Stright);
                    else
                    {
                        if (pBottomLeft != null)
                        {
                            Chunk<LOC> pChunk = pBottomLeft.GetChunk(pBottomLeft.Resolution - 1, pBottomLeft.Resolution - 1, eBottomLeft);
                            m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.DownLeft] = new Chunk<LOC>.NeighbourInfo(pChunk, eBottomLeft);
                        }
                    }

                    if (y > 0)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.Down] = new Chunk<LOC>.NeighbourInfo(m_cChunk[x, y - 1], VertexCH.Transformation.Stright);
                    else
                    {
                        if (pBottom != null)
                        {
                            Chunk<LOC> pChunk = pBottom.GetChunk(x, pBottom.Resolution - 1, eBottom);
                            m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.Down] = new Chunk<LOC>.NeighbourInfo(pChunk, eBottom);
                        }
                    }

                    if (x < Resolution - 1 && y > 0)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.DownRight] = new Chunk<LOC>.NeighbourInfo(m_cChunk[x + 1, y - 1], VertexCH.Transformation.Stright);
                    else
                    {
                        if (pBottomRight != null)
                        {
                            Chunk<LOC> pChunk = pBottomRight.GetChunk(0, pBottomRight.Resolution - 1, eBottomRight);
                            m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.DownRight] = new Chunk<LOC>.NeighbourInfo(pChunk, eBottomRight);
                        }
                    }

                    if (x < Resolution - 1)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.Right] = new Chunk<LOC>.NeighbourInfo(m_cChunk[x + 1, y], VertexCH.Transformation.Stright);
                    else
                    {
                        if (pRight != null)
                        {
                            Chunk<LOC> pChunk = pRight.GetChunk(0, y, eRight);
                            m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.Right] = new Chunk<LOC>.NeighbourInfo(pChunk, eRight);
                        }
                    }

                    if (x < Resolution - 1 && y < Resolution - 1)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.UpRight] = new Chunk<LOC>.NeighbourInfo(m_cChunk[x + 1, y + 1], VertexCH.Transformation.Stright);
                    else
                    {
                        if (pTopRight != null)
                        {
                            Chunk<LOC> pChunk = pTopRight.GetChunk(0, 0, eTopRight);
                            m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.UpRight] = new Chunk<LOC>.NeighbourInfo(pChunk, eTopRight);
                        }
                    }

                    if (y < Resolution - 1)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.Up] = new Chunk<LOC>.NeighbourInfo(m_cChunk[x, y + 1], VertexCH.Transformation.Stright);
                    else
                    {
                        if (pTop != null)
                        {
                            Chunk<LOC> pChunk = pTop.GetChunk(x, 0, eTop);
                            m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.Up] = new Chunk<LOC>.NeighbourInfo(pChunk, eTop);
                        }
                    }

                    if (x > 0 && y < Resolution - 1)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.UpLeft] = new Chunk<LOC>.NeighbourInfo(m_cChunk[x - 1, y + 1], VertexCH.Transformation.Stright);
                    else
                    {
                        if (pTopLeft != null)
                        {
                            Chunk<LOC> pChunk = pTopLeft.GetChunk(pTopLeft.Resolution - 1, 0, eTopLeft);
                            m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.UpLeft] = new Chunk<LOC>.NeighbourInfo(pChunk, eTopLeft);
                        }
                    }

                    if (x > 0)
                        m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.Left] = new Chunk<LOC>.NeighbourInfo(m_cChunk[x - 1, y], VertexCH.Transformation.Stright);
                    else
                    {
                        if (pLeft != null)
                        {
                            Chunk<LOC> pChunk = pLeft.GetChunk(pLeft.Resolution - 1, y, eLeft);
                            m_cChunk[x, y].m_cNeighbours[VertexCH.Direction.Left] = new Chunk<LOC>.NeighbourInfo(pChunk, eLeft);
                        }
                    }
                }
        }
    }
}
