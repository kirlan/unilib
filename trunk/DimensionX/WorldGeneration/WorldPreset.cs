﻿using System;
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
        public LocationsGrid<LocationX> m_pLocationsGrid;
        public int m_iContinents;
        public bool m_bGreatOcean;
        public int m_iLands;
        public int m_iStates;
        public int m_iLandMasses;
        public int m_iOcean;
        public int m_iEquator;
        public int m_iPole;
        public Epoch[] m_aEpoches;

        public WorldPreset(LocationsGrid<LocationX> pLocationsGrid,
                     int iContinents,
                     bool bGreatOcean,
                     int iLands,
                     int iStates,
                     int iLandMasses,
                     int iOcean,
                     int iEquator,
                     int iPole,
                     Epoch[] aEpoches)
        {
            m_pLocationsGrid = pLocationsGrid;
            m_iContinents = iContinents;
            m_bGreatOcean = bGreatOcean;
            m_iLands = iLands;
            m_iStates = iStates;
            m_iLandMasses = iLandMasses;
            m_iOcean = iOcean;
            m_iEquator = iEquator;
            m_iPole = iPole;
            m_aEpoches = aEpoches;
        }

        public WorldPreset(string sFileName, ComboBox.ObjectCollection aGrids)
        {
            UniLibXML pXml = new UniLibXML("DimensionX_WorldPreset");

            if (!pXml.Load(sFileName))
                throw new ArgumentException("Can't load world preset from file '" + sFileName + "'");

            m_pLocationsGrid = null;
            foreach (XmlNode pCatNode in pXml.Root.ChildNodes)
            {
                if (pCatNode.Name == "World")
                {
                    string sGridFileName = "";
                    pXml.GetStringAttribute(pCatNode, "grid", ref sGridFileName);
                    foreach(LocationsGrid<LocationX> pGrid in aGrids)
                        if (pGrid.m_sFilename.EndsWith("\\" + sGridFileName))
                        {
                            m_pLocationsGrid = pGrid;
                            break;
                        }

                    pXml.GetIntAttribute(pCatNode, "continents", ref m_iContinents);
                    pXml.GetBoolAttribute(pCatNode, "borderless", ref m_bGreatOcean);
                    pXml.GetIntAttribute(pCatNode, "lands", ref m_iLands);
                    pXml.GetIntAttribute(pCatNode, "states", ref m_iStates);
                    pXml.GetIntAttribute(pCatNode, "landmasses", ref m_iLandMasses);
                    pXml.GetIntAttribute(pCatNode, "ocean", ref m_iOcean);
                    pXml.GetIntAttribute(pCatNode, "equator", ref m_iEquator);
                    pXml.GetIntAttribute(pCatNode, "pole", ref m_iPole);
                }
                if (pCatNode.Name == "Epoches")
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
                }
            }
        }

        public bool Save(string sFileName)
        {
            UniLibXML pXml = new UniLibXML("DimensionX_WorldPreset");

            XmlNode pWorldPropertiesNode = pXml.CreateNode(pXml.Root, "World");
            
            string[] aFileName = m_pLocationsGrid.m_sFilename.Split(new char[] {'/', '\\'}, StringSplitOptions.RemoveEmptyEntries);

            pXml.AddAttribute(pWorldPropertiesNode, "grid", aFileName[aFileName.Length - 1]);
            pXml.AddAttribute(pWorldPropertiesNode, "continents", m_iContinents);
            pXml.AddAttribute(pWorldPropertiesNode, "borderless", m_bGreatOcean);
            pXml.AddAttribute(pWorldPropertiesNode, "lands", m_iLands);
            pXml.AddAttribute(pWorldPropertiesNode, "states", m_iStates);
            pXml.AddAttribute(pWorldPropertiesNode, "landmasses", m_iLandMasses);
            pXml.AddAttribute(pWorldPropertiesNode, "ocean", m_iOcean);
            pXml.AddAttribute(pWorldPropertiesNode, "equator", m_iEquator);
            pXml.AddAttribute(pWorldPropertiesNode, "pole", m_iPole);

            XmlNode pEpochesNode = pXml.CreateNode(pXml.Root, "Epoches");

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