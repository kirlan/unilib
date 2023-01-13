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
        public VoronoiVertex Point1 { get; set; }
        public VoronoiVertex Point2 { get; set; }

        public float Length { get; }

        public VoronoiEdge Next { get; set; } = null;

        public IRiver River { get; set; } = null;

        public VoronoiEdge(VoronoiVertex pPoint1, VoronoiVertex pPoint2)
        {
            Point1 = pPoint1;
            Point2 = pPoint2;

            if (!Point1.LinkedVertexes.Contains(Point2))
                Point1.LinkedVertexes.Add(Point2);

            if (!Point2.LinkedVertexes.Contains(Point1))
                Point2.LinkedVertexes.Add(Point1);

            Length = (float)Math.Sqrt((pPoint1.X - pPoint2.X) * (pPoint1.X - pPoint2.X) + (pPoint1.Y - pPoint2.Y) * (pPoint1.Y - pPoint2.Y));
        }

        public VoronoiEdge(VoronoiEdge pOriginal)
        {
            Point1 = pOriginal.Point1;
            Point2 = pOriginal.Point2;

            Length = pOriginal.Length;
        }

        public VoronoiEdge(BinaryReader binReader, Dictionary<long, VoronoiVertex> cVertexes)
        {
            Point1 = cVertexes[binReader.ReadInt64()];
            Point2 = cVertexes[binReader.ReadInt64()];

            Length = (float)Math.Sqrt((Point1.X - Point2.X) * (Point1.X - Point2.X) + (Point1.Y - Point2.Y) * (Point1.Y - Point2.Y));
        }

        public void Save(BinaryWriter binWriter)
        {
            binWriter.Write(Point1.VertexID);
            binWriter.Write(Point2.VertexID);
        }

        public override string ToString()
        {
            return string.Format("({0}) - ({1}), Length {2}", Point1, Point2, Length);
        }
    }
}
