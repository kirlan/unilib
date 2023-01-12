using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandscapeGeneration;
using Socium;
using System.Xml;
using nsUniLibXML;
using System.Windows.Forms;

namespace WorldGeneration
{
    public class WorldPreset
    {
        public int m_iContinents;
        public bool m_bGreatOcean;
        public int m_iLandsDiversity;
        public int m_iStates;
        public int m_iLandMassesDiversity;
        public int m_iOcean;
        public int m_iEquator;
        public int m_iPole;
        public Epoch[] m_aEpoches;

        public WorldPreset(int iContinents,
                     bool bGreatOcean,
                     int iLandsDivesity,
                     int iStates,
                     int iLandMassesDiversity,
                     int iOcean,
                     int iEquator,
                     int iPole,
                     Epoch[] aEpoches)
        {
            m_iContinents = iContinents;
            m_bGreatOcean = bGreatOcean;
            m_iLandsDiversity = iLandsDivesity;
            m_iStates = iStates;
            m_iLandMassesDiversity = iLandMassesDiversity;
            m_iOcean = iOcean;
            m_iEquator = iEquator;
            m_iPole = iPole;
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
                if (pCatNode.Name == "World")
                {
                    pXml.GetIntAttribute(pCatNode, "continents", ref m_iContinents);
                    pXml.GetBoolAttribute(pCatNode, "borderless", ref m_bGreatOcean);
                    pXml.GetIntAttribute(pCatNode, "lands", ref m_iLandsDiversity);
                    pXml.GetIntAttribute(pCatNode, "states", ref m_iStates);
                    pXml.GetIntAttribute(pCatNode, "landmasses", ref m_iLandMassesDiversity);
                    pXml.GetIntAttribute(pCatNode, "ocean", ref m_iOcean);
                    pXml.GetIntAttribute(pCatNode, "equator", ref m_iEquator);
                    pXml.GetIntAttribute(pCatNode, "pole", ref m_iPole);

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

            if (!bOK1 || !bOK2)
                throw new ArgumentException("Wrong file format '" + sFileName + "'");
        }

        public bool Save(string sFileName)
        {
            UniLibXML pXml = new UniLibXML("DimensionX_WorldPreset");

            XmlNode pWorldPropertiesNode = pXml.CreateNode(pXml.Root, "World");

            pXml.AddAttribute(pWorldPropertiesNode, "continents", m_iContinents);
            pXml.AddAttribute(pWorldPropertiesNode, "borderless", m_bGreatOcean);
            pXml.AddAttribute(pWorldPropertiesNode, "lands", m_iLandsDiversity);
            pXml.AddAttribute(pWorldPropertiesNode, "states", m_iStates);
            pXml.AddAttribute(pWorldPropertiesNode, "landmasses", m_iLandMassesDiversity);
            pXml.AddAttribute(pWorldPropertiesNode, "ocean", m_iOcean);
            pXml.AddAttribute(pWorldPropertiesNode, "equator", m_iEquator);
            pXml.AddAttribute(pWorldPropertiesNode, "pole", m_iPole);

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
