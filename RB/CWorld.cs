using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace RB
{
    enum GenderPriority
    { 
        OnlyFemale,
        MostlyFemale,
        Equal,
        MostlyMale,
        OnlyMale
    }

    /// <summary>
    /// Каждый мир представляется совокупностью сообществ и локаций различного размера и 
    /// характеризуется тремя уровнями развития: техногенного, биогенного и культурного. 
    /// </summary>
    class CWorld
    {
        GenderPriority m_eGenderPriority = GenderPriority.Equal;

        internal GenderPriority GenderPriority
        {
            get { return m_eGenderPriority; }
        }

        int m_iTechLevel;

        public int TechLevel
        {
            get { return m_iTechLevel; }
        }
        int m_iBioLevel;

        public int BioLevel
        {
            get { return m_iBioLevel; }
        }
        int m_iCultureLevel;

        public int CultureLevel
        {
            get { return m_iCultureLevel; }
        }

        private List<CSociety> m_cSocieties = new List<CSociety>();

        internal List<CSociety> Societies
        {
            get { return m_cSocieties; }
        }

        private List<CLocation> m_cLocations = new List<CLocation>();

        internal List<CLocation> Locations
        {
            get { return m_cLocations; }
        }

        private int m_iWorldScale = 150;

        public int WorldScale
        {
            get { return m_iWorldScale; }
        } 

        private const int m_iMinDist = 25;
        private const int m_iMaxDist = 35;

        public CWorld(int iScale, int iCountries)
        {
            int iGender = Rnd.Get(3);
            switch (iGender)
            {
                case 0:
                    m_eGenderPriority = GenderPriority.MostlyFemale;
                    break;
                case 1:
                    m_eGenderPriority = GenderPriority.Equal;
                    break;
                case 2:
                    m_eGenderPriority = GenderPriority.MostlyMale;
                    break;
            }

            m_iTechLevel = (int)(Math.Pow(Rnd.Get(30), 2) / 100);
            m_iBioLevel = (int)(Math.Pow(Rnd.Get(30), 2) / 100);
            m_iCultureLevel = (int)(Math.Pow(Rnd.Get(30), 2) / 100);

            m_iWorldScale = iScale;

            for (int i = 0; i < iCountries; i++)
                AddSociety();

            GenerateMap();

            PopulateMap();
        }

        private void PopulateMap()
        {
            for (int i = 0; i < m_cSocieties.Count; i++)
            {
                int iLoc = -1;
                do
                {
                    iLoc = Rnd.Get(m_cLocations.Count);
                }
                while (m_cLocations[iLoc].Owner != null || m_cLocations[iLoc].Type == LocationType.Forbidden);

                m_cLocations[iLoc].Owner = m_cSocieties[i];
                m_cSocieties[i].Lands.Add(m_cLocations[iLoc]);
            }

            bool bExpanding = false;
            do
            {
                bExpanding = false;
                for (int i = 0; i < m_cLocations.Count; i++)
                {
                    if (m_cLocations[i].Owner != null)
                    {
                        List<CLocation> cPossibleDirections = new List<CLocation>();
                        for (int j = 0; j < m_cLocations[i].Links.Count; j++)
                        {
                            if (m_cLocations[i].Links[j].Owner == null && m_cLocations[i].Links[j].Type != LocationType.Forbidden)
                                cPossibleDirections.Add(m_cLocations[i].Links[j]);
                        }
                        if (cPossibleDirections.Count > 0)
                        {
                            int iLoc = Rnd.Get(cPossibleDirections.Count);
                            cPossibleDirections[iLoc].Owner = m_cLocations[i].Owner;
                            m_cLocations[i].Owner.Lands.Add(cPossibleDirections[iLoc]);
                            bExpanding = true;
                        }
                    }
                }
            }
            while(bExpanding);

            for (int i = 0; i < m_cSocieties.Count; i++)
            {
                int tryings = 0;
                do
                {
                    int iLoc = Rnd.Get(m_cSocieties[i].Lands.Count);
                    if (m_cSocieties[i].Lands[iLoc].Type == LocationType.Undefined)
                    {
                        m_cSocieties[i].Lands[iLoc].Type = LocationType.Settlement;
                        m_cSocieties[i].Settlements.Add(m_cSocieties[i].Lands[iLoc]);
                        foreach (CLocation pLoc in m_cSocieties[i].Lands[iLoc].Links)
                        {
                            if (pLoc.Type == LocationType.Undefined)
                            {
                                pLoc.Type = LocationType.Field;
                                foreach (CLocation pLoc2 in pLoc.Links)
                                {
                                    if (pLoc2.Type == LocationType.Undefined && !Rnd.OneChanceFrom(3))
                                    {
                                        pLoc2.Type = LocationType.Field;
                                        foreach (CLocation pLoc3 in pLoc2.Links)
                                        {
                                            if (pLoc3.Type == LocationType.Undefined && Rnd.OneChanceFrom(2))
                                            {
                                                pLoc3.Type = LocationType.Field;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        tryings = 0;
                    }
                }
                while (tryings++ < 10);
            }
        }

        private void GenerateMap()
        {
            int iX, iY;

            double fAngle = 0;
            do
            {
                iX = (int)(m_iWorldScale * Math.Cos(fAngle));
                iY = (int)(m_iWorldScale * Math.Sin(fAngle));
                AddLocation(iX, iY).Type = LocationType.Forbidden;

                int iDelta = m_iMinDist + Rnd.Get(m_iMaxDist - m_iMinDist);
                fAngle += (double)iDelta / m_iWorldScale;
            }
            while(fAngle < 2*Math.PI);

            int iLocationsCount = (int)(Math.Pow(m_iWorldScale, 2) * Math.PI / 1000);
            while (m_cLocations.Count < iLocationsCount)
            {
                do
                {
                    iX = m_iWorldScale - Rnd.Get(m_iWorldScale * 2);
                    iY = m_iWorldScale - Rnd.Get(m_iWorldScale * 2);
                }
                while (!PossibleLocation(iX, iY));

                AddLocation(iX, iY);
            }

            List<CLocation> cEmpty = new List<CLocation>();
            for (int i = 0; i < m_cLocations.Count; i++)
            {
                if (m_cLocations[i].Links.Count == 0)
                    cEmpty.Add(m_cLocations[i]);
            }

            foreach (CLocation pLoc in cEmpty)
                m_cLocations.Remove(pLoc);
        }

        private CLocation AddLocation(int iX, int iY)
        {
            CLocation pNewLocation = new CLocation(iX, iY);
            m_cLocations.Add(pNewLocation);

            int k;
            int try_count = -1;
            do
            {
                k = MakeLink(pNewLocation);
                try_count++;
                //if (k == -1)
                //    k = pNewLocation.MaxLinks + 1;
            }
            while (try_count < 2 * pNewLocation.MaxLinks);
            //while (Rnd.Get(pNewLocation.MaxLinks * pNewLocation.MaxLinks) <= k * k && try_count < 2 * pNewLocation.MaxLinks) ;

            return pNewLocation;
        }

        private int MakeLink(CLocation pNewLocation)
        {
            float fDist;
            int k;
            for (int i = 0; i < m_cLocations.Count; i++)
            {
                fDist = (m_cLocations[i].X - pNewLocation.X) * (m_cLocations[i].X - pNewLocation.X) +
                    (m_cLocations[i].Y - pNewLocation.Y) * (m_cLocations[i].Y - pNewLocation.Y);
                if (m_cLocations[i] != pNewLocation && fDist < 2 * m_iMaxDist * m_iMaxDist)
                {
                    k = CreateLink(m_cLocations[i], pNewLocation);
                    if (k != -1)
                        return k;
                }
            }
            return -1;
        }

        private int CreateLink(CLocation pLocation1, CLocation pLocation2)
        {
	        if(pLocation1.Links.Contains(pLocation2))
		        return -1;

            if (pLocation1.Links.Count >= pLocation1.MaxLinks || pLocation2.Links.Count >= pLocation2.MaxLinks)
                return -1;

            double k1, b1;
            //						коеффициент наклона потенциальной связи
            if (pLocation1.X - pLocation2.X != 0)
                k1 = (double)(pLocation1.Y - pLocation2.Y) / (pLocation1.X - pLocation2.X);
            else
                k1 = (double)(pLocation1.Y - pLocation2.Y) / (pLocation1.X - pLocation2.X + 0.00000001);

            b1 = (double)pLocation2.Y - pLocation2.X * k1;
            
            
            //	Проверка перекрёстков
	        for(int i=0; i<m_cLocations.Count; i++)
	        {
                CLocation pLocation3 = m_cLocations[i];
                if (pLocation3 != pLocation1 &&
                    pLocation3 != pLocation2)
                {
                    for (int j = 0; j < pLocation3.Links.Count; j++)
                    {
                        CLocation pLocation4 = pLocation3.Links[j];
                        if (pLocation4 != pLocation1 &&
                            pLocation4 != pLocation2)
                        {
                            //	Нашли пару соединённых пещер, не входящих в рассматриваемую пару.
                            double k2, b2;
                            //						коеффициент наклона существующей связи
                            if (pLocation3.X - pLocation4.X != 0)
                                k2 = (double)(pLocation3.Y - pLocation4.Y) / (pLocation3.X - pLocation4.X);
                            else
                                k2 = (double)(pLocation3.Y - pLocation4.Y) / (pLocation3.X - pLocation4.X + 0.00000001);

                            b2 = (double)pLocation4.Y - pLocation4.X * k2;

                            if (k1 != k2)
                            {
                                double x1;
                                x1 = (b2 - b1) / (k1 - k2);

                                //						ЕСЛИ точка пересечения где-то здесь
                                if (((x1 > pLocation1.X && x1 < pLocation2.X) ||
                                     (x1 < pLocation1.X && x1 > pLocation2.X)) &&
                                    ((x1 > pLocation3.X && x1 < pLocation4.X) ||
                                     (x1 < pLocation3.X && x1 > pLocation4.X)))
                                {
                                    return -1;
                                }
                            }
                        }
                    }
                }
	        }

            pLocation1.Links.Add(pLocation2);
            pLocation2.Links.Add(pLocation1);

	        return pLocation1.Links.Count;
        }

        private bool PossibleLocation(int iX, int iY)
        {
            float fDist = iX * iX + iY * iY;
            if (fDist > m_iWorldScale * m_iWorldScale)
                return false;

            bool result = false;

            for (int i = 0; i < m_cLocations.Count; i++)
            {
                fDist = (m_cLocations[i].X - iX) * (m_cLocations[i].X - iX) + (m_cLocations[i].Y - iY) * (m_cLocations[i].Y - iY);
                if (fDist < m_iMinDist * m_iMinDist)
                    return false;
                if (fDist < m_iMaxDist * m_iMaxDist && m_cLocations[i].Links .Count < m_cLocations[i].MaxLinks)
                    result = true;
            }

            if (m_cLocations.Count < 1)
                return true;

            return result;
        }

        public CSociety AddSociety()
        {
            CSociety pNewSociety = new CSociety(this);
            m_cSocieties.Add(pNewSociety);
            return pNewSociety;
        }
    }
}
