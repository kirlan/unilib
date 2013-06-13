using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandscapeGeneration;
using Socium;
using System.Xml;
using nsUniLibXML;
using System.Windows.Forms;
using LandscapeGeneration.PlanetBuilder;

namespace WorldGeneration
{
    public class WorldPreset
    {
        public WorkingArea m_eWorkingArea;
        public int m_iChunkSize;
        public int m_iChunksCount;
        public int m_iContinents;
        public int m_iLandsDiversity;
        public int m_iStates;
        public int m_iLandMassesDiversity;
        public int m_iOcean;
        public Epoch[] m_aEpoches;

        public WorldPreset(WorkingArea eWorkingArea,
                     int iChunkSize,
                     int iChunksCount,
                     int iContinents,
                     int iLandsDivesity,
                     int iStates,
                     int iLandMassesDiversity,
                     int iOcean,
                     Epoch[] aEpoches)
        {
            m_eWorkingArea = eWorkingArea;
            m_iChunkSize = iChunkSize;
            m_iChunksCount = iChunksCount;
            m_iContinents = iContinents;
            m_iLandsDiversity = iLandsDivesity;
            m_iStates = iStates;
            m_iLandMassesDiversity = iLandMassesDiversity;
            m_iOcean = iOcean;
            m_aEpoches = aEpoches;
        }

        public WorldPreset(string sFileName)
        {
            UniLibXML pXml = new UniLibXML("DimensionX_WorldPreset");

            if (!pXml.Load(sFileName))
                throw new ArgumentException("Can't load world preset from file '" + sFileName + "'");

            bool bOK1 = false;
            bool bOK2 = false;

            foreach (XmlNode pCatNode in pXml.Root.ChildNodes)
            {
                if (pCatNode.Name == "Planet")
                {
                    object temp = m_eWorkingArea;
                    pXml.GetEnumAttribute(pCatNode, "planet", typeof(WorkingArea), ref temp);
                    m_eWorkingArea = (WorkingArea)temp;
                    pXml.GetIntAttribute(pCatNode, "locations", ref m_iChunkSize);
                    pXml.GetIntAttribute(pCatNode, "chunks", ref m_iChunksCount);
                    pXml.GetIntAttribute(pCatNode, "continents", ref m_iContinents);
                    pXml.GetIntAttribute(pCatNode, "lands", ref m_iLandsDiversity);
                    pXml.GetIntAttribute(pCatNode, "states", ref m_iStates);
                    pXml.GetIntAttribute(pCatNode, "landmasses", ref m_iLandMassesDiversity);
                    pXml.GetIntAttribute(pCatNode, "ocean", ref m_iOcean);

                    bOK1 = true;
                }
                if (pCatNode.Name == "History")
                {
                    List<Epoch> cEpoches = new List<Epoch>();
                    foreach (XmlNode pEpochNode in pCatNode.ChildNodes)
                    {
                        if (pEpochNode.Name == "Epoch")
                        {
                            Epoch pNewEpoch = new Epoch(pXml, pEpochNode);
                            cEpoches.Add(pNewEpoch);
                        }
                    }
                    m_aEpoches = cEpoches.ToArray();

                    bOK2 = true;
                }
            }

            if(!bOK1 || !bOK2)
                throw new ArgumentException("Wrong file format '" + sFileName + "'");
        }

        public bool Save(string sFileName)
        {
            UniLibXML pXml = new UniLibXML("DimensionX_WorldPreset");

            XmlNode pWorldPropertiesNode = pXml.CreateNode(pXml.Root, "Planet");

            pXml.AddAttribute(pWorldPropertiesNode, "planet", m_eWorkingArea);
            pXml.AddAttribute(pWorldPropertiesNode, "locations", m_iChunkSize);
            pXml.AddAttribute(pWorldPropertiesNode, "chunks", m_iChunksCount);
            pXml.AddAttribute(pWorldPropertiesNode, "continents", m_iContinents);
            pXml.AddAttribute(pWorldPropertiesNode, "lands", m_iLandsDiversity);
            pXml.AddAttribute(pWorldPropertiesNode, "states", m_iStates);
            pXml.AddAttribute(pWorldPropertiesNode, "landmasses", m_iLandMassesDiversity);
            pXml.AddAttribute(pWorldPropertiesNode, "ocean", m_iOcean);

            XmlNode pEpochesNode = pXml.CreateNode(pXml.Root, "History");

            foreach (Epoch pEpoch in m_aEpoches)
            {
                XmlNode pEpochNode = pXml.CreateNode(pEpochesNode, "Epoch");
                pEpoch.Write(pXml, pEpochNode);
            }

            pXml.Write(sFileName);

            return true;
        }
    }
}
