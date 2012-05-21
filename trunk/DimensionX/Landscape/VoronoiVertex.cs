using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using BenTools.Mathematics;
using System.IO;
using LandscapeGeneration.PathFind;

namespace LandscapeGeneration
{
    public class Vertex : IPointF
    {
        public float m_fX = 0;
        public float m_fY = 0;
        public float m_fZ = 0;
        
        public float m_fHeight = float.NaN;

        public float X
        {
            get { return m_fX; }
            set { m_fX = value; }
        }

        public float Y
        {
            get { return m_fY; }
            set { m_fY = value; }
        }

        public float Z
        {
            get { return m_fZ; }
            set { m_fZ = value; }
        }

        private static long s_iCounter = 0;

        public long m_iID = s_iCounter++;

        public List<Vertex> m_cVertexes = new List<Vertex>();

        public List<long> m_cLinksTmp = new List<long>();

        public List<Location> m_cLocations = new List<Location>();

        public List<long> m_cLocationsTmp = new List<long>();

        public Vertex(float fX, float fY, float fZ)
        {
            m_fX = fX;
            m_fY = fY;
            m_fZ = fZ;
        }

        public Vertex(BTVector pVector)
        {
            m_fX = (float)pVector.data[0];
            m_fY = (float)pVector.data[1];
            m_fZ = 0;
        }

        public Vertex(BinaryReader binReader)
        {
            m_iID = binReader.ReadInt64();

            m_fX = (float)binReader.ReadDouble();
            m_fY = (float)binReader.ReadDouble();
            m_fZ = (float)binReader.ReadDouble();


            int iLinksCount = binReader.ReadInt32();
            for (int i = 0; i < iLinksCount; i++)
                m_cLinksTmp.Add(binReader.ReadInt64());

            int iLocationsCount = binReader.ReadInt32();
            for (int i = 0; i < iLocationsCount; i++)
                m_cLocationsTmp.Add(binReader.ReadInt64());
        }

        public void Save(BinaryWriter binWriter)
        {
            binWriter.Write(m_iID);

            binWriter.Write((double)m_fX);
            binWriter.Write((double)m_fY);
            binWriter.Write((double)m_fZ);

            binWriter.Write(m_cVertexes.Count);
            foreach (Vertex pVertex in m_cVertexes)
                binWriter.Write(pVertex.m_iID);

            binWriter.Write(m_cLocations.Count);
            foreach (Location pLocation in m_cLocations)
                binWriter.Write(pLocation.m_iID);
        }

        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}, Z: {2}, H: {3}", X, Y, Z, m_fHeight);
        }

        //TODO: для замкнутых (цилиндрических, сферических) миров - вычислять угловые координаты и работать с ними
        internal void PointOnCurve(Vertex p0, Vertex p1, Vertex p2, Vertex p3, float t)
        {
            float t2 = t * t; 
            float t3 = t2 * t;

            float p0X = p0.X;
            float p0Y = p0.Y;
            float p1X = p1.X;
            float p1Y = p1.Y;
            float p2X = p2.X;
            float p2Y = p2.Y;
            float p3X = p3.X;
            float p3Y = p3.Y;

            m_fX = 0.5f * ((2.0f * p1X) + (-p0X + p2X) * t + 
                (2.0f * p0X - 5.0f * p1X + 4 * p2X - p3X) * t2 + 
                (-p0X + 3.0f * p1X - 3.0f * p2X + p3X) * t3); 

            m_fY = 0.5f * ((2.0f * p1Y) + (-p0Y + p2Y) * t + 
                (2.0f * p0Y - 5.0f * p1Y + 4 * p2Y - p3Y) * t2 + 
                (-p0Y + 3.0f * p1Y - 3.0f * p2Y + p3Y) * t3);
        }
    }
}
