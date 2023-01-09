using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandscapeGeneration
{
    public class VoronoiEdge
    {
        public VoronoiVertex m_pPoint1;
        public VoronoiVertex m_pPoint2;

        public float Length { get; private set; }

        public VoronoiEdge m_pNext = null;

        public VoronoiEdge(VoronoiVertex pPoint1, VoronoiVertex pPoint2)
        {
            m_pPoint1 = pPoint1;
            m_pPoint2 = pPoint2;

            Length = (float)Math.Sqrt((pPoint1.X - pPoint2.X) * (pPoint1.X - pPoint2.X) + (pPoint1.Y - pPoint2.Y) * (pPoint1.Y - pPoint2.Y));
        }

        public VoronoiEdge(VoronoiEdge pOriginal)
        {
            m_pPoint1 = pOriginal.m_pPoint1;
            m_pPoint2 = pOriginal.m_pPoint2;

            Length = pOriginal.Length;
        }

        public VoronoiEdge(BinaryReader binReader, Dictionary<long, VoronoiVertex> cVertexes)
        {
            m_pPoint1 = cVertexes[binReader.ReadInt64()];
            m_pPoint2 = cVertexes[binReader.ReadInt64()];

            Length = (float)Math.Sqrt((m_pPoint1.X - m_pPoint2.X) * (m_pPoint1.X - m_pPoint2.X) + (m_pPoint1.Y - m_pPoint2.Y) * (m_pPoint1.Y - m_pPoint2.Y));
        }

        public void Save(BinaryWriter binWriter)
        {
            binWriter.Write(m_pPoint1.m_iVertexID);
            binWriter.Write(m_pPoint2.m_iVertexID);
        }

        public override string ToString()
        {
            return string.Format("({0}) - ({1}), Length {2}", m_pPoint1, m_pPoint2, Length);
        }
    }
}
