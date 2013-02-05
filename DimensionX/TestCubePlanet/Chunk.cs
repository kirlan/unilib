using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestCubePlanet
{
    public class Chunk
    {
        public Dictionary<VertexCH.Direction, Chunk> m_cNeighbour = new Dictionary<VertexCH.Direction, Chunk>();

        public List<Location> m_cLocations = new List<Location>();
        public List<Vertex> m_cVertexes = new List<Vertex>();

        public Chunk(List<VertexCH> locations, List<CellCH> vertices, float fDX, float fDY, float fR, Cube.Face3D eFace)
        {
            Dictionary<CellCH, Vertex> cTempVertexes = new Dictionary<CellCH, Vertex>();
            foreach (var vertex in vertices)
            {
                Vertex myVertex = new Vertex((float)vertex.Circumcenter.X + fDX - fR, (float)vertex.Circumcenter.Y + fDY - fR, fR, eFace);
                m_cVertexes.Add(myVertex);
                cTempVertexes[vertex] = myVertex;
            }

            Dictionary<VertexCH, Location> cTempLocations = new Dictionary<VertexCH, Location>();
            foreach (var loc in locations)
            {
                Location myLocation = new Location((float)loc.Position[0] + fDX - fR, (float)loc.Position[1] + fDY - fR, fR, eFace, loc.m_eGhost != VertexCH.Direction.CenterNone);
                m_cLocations.Add(myLocation);
                cTempLocations[loc] = myLocation;
            }

            foreach (var loc in locations)
            {
                Location myLocation = cTempLocations[loc];
                foreach (var edge in loc.m_cEdges)
                {
                    myLocation.m_cEdges[cTempLocations[edge.Key]] = new Location.Edge(cTempVertexes[edge.Value.m_pFrom], cTempVertexes[edge.Value.m_pTo]);
                }
            }
        }
    }
}
