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
        public float m_fX;
        public float m_fY;

        public float X
        {
            get { return m_fX; }
        }

        public float Y
        {
            get { return m_fY; }
        }

        private static long s_iCounter = 0;

        public long m_iID = s_iCounter++;

        public List<Vertex> m_cVertexes = new List<Vertex>();

        public List<long> m_cLinksTmp = new List<long>();

        public List<Location> m_cLocations = new List<Location>();

        public List<long> m_cLocationsTmp = new List<long>();

        public Vertex(BTVector pVector)
        {
            m_fX = (float)pVector.data[0];
            m_fY = (float)pVector.data[1];
        }

        public Vertex(BinaryReader binReader)
        {
            m_iID = binReader.ReadInt64();

            m_fX = (float)binReader.ReadDouble();
            m_fY = (float)binReader.ReadDouble();


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

            binWriter.Write(m_cVertexes.Count);
            foreach (Vertex pVertex in m_cVertexes)
                binWriter.Write(pVertex.m_iID);

            binWriter.Write(m_cLocations.Count);
            foreach (Location pLocation in m_cLocations)
                binWriter.Write(pLocation.m_iID);
        }

        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}", X, Y);
        }

        //public override float GetMovementCost()
        //{
        //    return 100;
        //}
    }
}
