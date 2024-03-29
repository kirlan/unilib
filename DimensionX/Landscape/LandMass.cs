﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using SimpleVectors;

namespace LandscapeGeneration
{
    public class LandMass<LAND> : Territory<LAND>, ILandMass
        where LAND: class, ILand
    {
        private bool m_bOcean = false;

        public bool IsWater
        {
            get { return m_bOcean; }
            set { m_bOcean = value; }
        }

        public SimpleVector3d m_pDrift;

        public int m_iMaxSize = -1;

        public LandMass()
            : base()
        {
            m_pDrift = new SimpleVector3d(1.0 - Rnd.Get(2.0f), 1.0 - Rnd.Get(2.0f), 0);
            m_pDrift = m_pDrift / !m_pDrift;
            //m_iElevation = (int)(Math.Tan(1.2f - Rnd.Get(2.22f)));
        }

        public override void Start(LAND pSeed)
        {
            base.Start(pSeed);

            if (Rnd.OneChanceFrom(2))
                m_iMaxSize = Rnd.Get(5) + 1;
        }

        public override object Grow()
        {
            if (m_iMaxSize > 0 && m_cContents.Count >= m_iMaxSize)
                return null;

            return base.Grow();
        }

        public bool HaveForbiddenBorders()
        {
            foreach (var pLand in m_cBorder)
                if ((pLand.Key as ITerritory).Forbidden)
                    return true;

            return false;
        }

        public string GetLandsString()
        {
            string sLands = "";

            foreach (LAND pLand in m_cContents)
                sLands += String.Format("({0}), ", pLand.GetLandsString());

            return sLands;
        }

        public override string ToString()
        {
            return GetLandsString();

            //return (m_bOcean ? "ocean": "land") + " " + m_cContents.Count.ToString(); 
        }

        public override float GetMovementCost()
        {
            return (m_bOcean ? 10 : 1);// *m_cContents.Count;
        }
    }
}
