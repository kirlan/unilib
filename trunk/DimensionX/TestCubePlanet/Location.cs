using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestCubePlanet
{
    public class Location: Vertex
    {
        public bool m_bGhost = false;

        public Location(float fX, float fY, float fR, Cube.Face3D eFace, bool bGhost)
            : base(fX, fY, fR, eFace)
        {
            m_bGhost = bGhost;
        }

        public class Edge
        {
            public Vertex m_pFrom;
            public Vertex m_pTo;

            public Edge(Vertex pFrom, Vertex pTo)
            {
                m_pFrom = pFrom;
                m_pTo = pTo;
            }
        }

        public Dictionary<Location, Edge> m_cEdges = new Dictionary<Location, Edge>();
    }
}
