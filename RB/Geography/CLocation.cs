using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MIConvexHull;
using System.Windows;
using Random;
using RB.Genetix;
using RB.Socium;

namespace RB.Geography
{
    enum LocationType
    {
        Undefined,
        Settlement,
        Field,
        Forbidden
    }

    /// <summary>
    /// Локация — это точка на карте, в которой могут происходить какие-либо события. 
    /// Это может быть город, деревня, горный перевал или поляна в лесу... Локации соединяются 
    /// между собой переходами,  определяющими возможность и затраты времени на перемещение 
    /// из одной локации в другую. В любой локации в каждый момент игрового времени могет 
    /// находиться неограниченное количество людей, невзирая на их принадлежность к сообществам. 
    /// 
    /// Любое взаимодействие между персонажами возможно тогда и только тогда, когда они 
    /// находятся в одной локации.
    /// </summary>
    public class CLocation : IVertex
    {
        public class VoronoiCell : TriangulationCell<CLocation, VoronoiCell>
        {
//            static System.Random rnd = new System.Random();

//            public object m_pTag = null;

            Point GetCircumcenter()
            {
                // From MathWorld: http://mathworld.wolfram.com/Circumcircle.html

                var points = Vertices;

                double[,] m = new double[3, 3];

                // x, y, 1
                for (int i = 0; i < 3; i++)
                {
                    m[i, 0] = points[i].Position[0];
                    m[i, 1] = points[i].Position[1];
                    m[i, 2] = 1;
                }
                var a = StarMath.determinant(m);

                // size, y, 1
                for (int i = 0; i < 3; i++)
                {
                    //                m[i, 0] = StarMath.norm2(points[i].Position, 2, true);
                    m[i, 0] = StarMath.norm2(points[i].Position, true);
                }
                var dx = -StarMath.determinant(m);

                // size, x, 1
                for (int i = 0; i < 3; i++)
                {
                    m[i, 1] = points[i].Position[0];
                }
                var dy = StarMath.determinant(m);

                // size, x, y
                for (int i = 0; i < 3; i++)
                {
                    m[i, 2] = points[i].Position[1];
                }
                var c = -StarMath.determinant(m);

                var s = -1.0 / (2.0 * a);
                var r = System.Math.Abs(s) * System.Math.Sqrt(dx * dx + dy * dy - 4 * a * c);
                return new Point(s * dx, s * dy);
            }

            Point GetCentroid()
            {
                return new Point(Vertices.Select(v => v.Position[0]).Average(), Vertices.Select(v => v.Position[1]).Average());
            }

            Point? circumCenter;
            public Point Circumcenter
            {
                get
                {
                    circumCenter = circumCenter ?? GetCircumcenter();
                    return circumCenter.Value;
                }
            }

            Point? centroid;
            public Point Centroid
            {
                get
                {
                    centroid = centroid ?? GetCentroid();
                    return centroid.Value;
                }
            }

            public VoronoiCell()
            {
            }

            public VoronoiCell(VoronoiCell pOrigin1, VoronoiCell pOrigin2)
            {
                //double fLength = Math.Sqrt((pOrigin1.Circumcenter.X - pOrigin2.Circumcenter.X) * (pOrigin1.Circumcenter.X - pOrigin2.Circumcenter.X) + (pOrigin1.Circumcenter.Y - pOrigin2.Circumcenter.Y) * (pOrigin1.Circumcenter.Y - pOrigin2.Circumcenter.Y));


                double fAngle = Rnd.Get(2 * Math.PI);
                double fRX = Rnd.Get(Math.Abs(pOrigin1.Circumcenter.X - pOrigin2.Circumcenter.X) / 4);
                double fRY = Rnd.Get(Math.Abs(pOrigin1.Circumcenter.Y - pOrigin2.Circumcenter.Y) / 4);

                circumCenter = new Point((pOrigin1.Circumcenter.X + pOrigin2.Circumcenter.X) / 2 + fRX * Math.Cos(fAngle), (pOrigin1.Circumcenter.Y + pOrigin2.Circumcenter.Y) / 2 + fRY * Math.Sin(fAngle));
            }

            public VoronoiCell(VoronoiCell pOrigin1, CLocation pOrigin2)
            {
                circumCenter = new Point((pOrigin1.Circumcenter.X + pOrigin2.Position[0]) / 2, (pOrigin1.Circumcenter.Y + pOrigin2.Position[1]) / 2);
            }

            public override string ToString()
            {
                return string.Format("[{0}, {1}]", Circumcenter.X, Circumcenter.Y);
            }
        }

        public class VoronoiEdge
        {
            public VoronoiCell m_pFrom;
            public VoronoiCell m_pTo;
            public VoronoiCell m_pMidPoint;
            //public Cell m_pInnerPoint;

            public VoronoiEdge(VoronoiCell pFrom, VoronoiCell pTo, VoronoiCell pMidPoint)//, CLocation pInnerPoint)
            {
                m_pFrom = pFrom;
                m_pTo = pTo;
                m_pMidPoint = pMidPoint;
                //m_pInnerPoint = pInnerPoint;
            }

            public override string ToString()
            {
                return string.Format("{0} - {1}", m_pFrom, m_pTo);
            }
        }

        /// <summary>
        /// Gets or sets the coordinates.
        /// </summary>
        /// <value>The coordinates.</value>
        public double[] Position { get; set; }

        public int X
        {
            get { return (int)Position[0]; }
        }

        public int Y
        {
            get { return (int)Position[1]; }
        }

        public Point ToPoint()
        {
            return new Point(Position[0], Position[1]);
        }

        private Dictionary<CLocation, CLink> m_cLinks = new Dictionary<CLocation, CLink>();

        internal Dictionary<CLocation, CLink> Links
        {
            get { return m_cLinks; }
        }

        private const int m_iMaxLinks = 8;

        public int MaxLinks
        {
            get { return m_iMaxLinks; }
        }

        private CState m_pOwner = null;

        internal CState Owner
        {
            get { return m_pOwner; }
            set { m_pOwner = value; }
        }

        private LocationType m_eType = LocationType.Undefined;

        internal LocationType Type
        {
            get { return m_eType; }
            set { m_eType = value; }
        }

        private Territory m_pTerritory = Territory.Undefined;

        internal Territory Territory
        {
            get { return m_pTerritory; }
            set { m_pTerritory = value; }
        }

        private CSettlement m_pSettlement = null;

        public CSettlement Settlement
        {
            get { return m_pSettlement; }
            set { m_pSettlement = value; }
        }

        public CLocation(int iX, int iY)
        {
            Position = new double[] { iX, iY };
        }

        public bool HaveEstate(CEstate.Position eEstate)
        {
            if (m_pSettlement != null && m_pOwner.m_cEstates.ContainsKey(eEstate))
            {
                foreach (Building pBuilding in m_pSettlement.m_cBuildings)
                {
                    CStratum pOwner = m_pOwner.GetStratum(pBuilding.m_pInfo.m_pOwner);
                    if (m_pOwner.m_cEstates[eEstate].m_cStratums.Contains(pOwner))
                    {
                        List<CPerson> cOwners = new List<CPerson>();
                        foreach (CPerson pDweller in pBuilding.m_cPersons)
                            if (pDweller.m_pProfession == pBuilding.m_pInfo.m_pOwner)
                                cOwners.Add(pDweller);

                        if (cOwners.Count < pBuilding.m_pInfo.OwnersCount)
                            return true;
                    }
                    CStratum pWorkers = m_pOwner.GetStratum(pBuilding.m_pInfo.m_pWorkers);
                    if (m_pOwner.m_cEstates[eEstate].m_cStratums.Contains(pWorkers))
                    {
                        List<CPerson> cWorkers = new List<CPerson>();
                        foreach (CPerson pDweller in pBuilding.m_cPersons)
                            if (pDweller.m_pProfession == pBuilding.m_pInfo.m_pWorkers)
                                cWorkers.Add(pDweller);

                        if (cWorkers.Count < pBuilding.m_pInfo.WorkersCount)
                            return true;
                    }
                }
            }

            return false;

        }

        /// <summary>
        /// Заполняется в Cube::RebuildEdges()
        /// </summary>
        public Dictionary<CLocation, VoronoiEdge> m_cEdges = new Dictionary<CLocation, VoronoiEdge>();

        public override string ToString()
        {
            return string.Format("{2} ({0}, {1}, {3}) F:{4}/{5} W:{6} O:{7} POP:{8}", Position[0], Position[1],
                m_eType, m_pTerritory.m_sName, 
                GetResource(Territory.Resource.Grain), GetResource(Territory.Resource.Game), 
                GetResource(Territory.Resource.Wood), GetResource(Territory.Resource.Ore),
                m_pOwner == null ? 0 : 100 / GetClaimingCost(m_pOwner.m_pNation));
        }

        /// <summary>
        /// Считает стоимость заселения локации указанной расой с учётом ландшафта локации и её соседей.
        /// Возвращает значение в диапазоне 1-100.
        /// 1 - любая территория, идеально подходящая указанной расе, окружённая так же идеально подходящими.
        /// 5 - идеально подходящая, окружённая простыми для заселения, но совсем не подходящими. Или наоборот.
        /// 10 - простая, но не подходящая, окружённая другими простыми, но не подходящими.
        /// 50 - подходящая, окружённая сложными и не подходящими, или наоборот.
        /// 55 - простая и не подходящая, окружённая сложными и не подходящими, или наоборот.
        /// 100 - сложная и не подходящая, окружённая сложными и не подходящими.
        /// </summary>
        /// <param name="pNation"></param>
        /// <returns></returns>
        public int GetClaimingCost(CNation pNation)
        {
            double fCost = m_pTerritory.GetClaimingCost(pNation);

            foreach (var pLink in m_cEdges)
            {
                if (pLink.Key.Owner == null && pLink.Key.Territory.LandProperties.HasFlag(LandscapeProperty.Habitable))
                    fCost += (double)pLink.Key.Territory.GetClaimingCost(pNation) / m_cEdges.Count;
            }

            if (fCost < 2)
                fCost = 2;
            return (int)(fCost/2);
        }

        public float GetResource(Territory.Resource eResource)
        {
            float fAmount = m_pTerritory.Resources[eResource];

            foreach (var pLink in m_cEdges)
            {
                if (pLink.Key.Owner == null || pLink.Key.Owner == m_pOwner)
                    fAmount += pLink.Key.Territory.Resources[eResource] / m_cEdges.Count;
            }

            return fAmount/2;
        }

        public void ClaimSettlement()
        {
            m_eType = LocationType.Settlement;
            m_pTerritory = Territory.Plains;
            foreach (var pLink in m_cEdges)
            {
                if (pLink.Key.Type == LocationType.Undefined)
                {
                    //if (pLoc.SubType != LocationSubType.Undefined)
                    //    throw new Exception();
                    pLink.Key.Type = LocationType.Field;
                    foreach (var pLink2 in pLink.Key.m_cEdges)
                    {
                        if (pLink2.Key.Type == LocationType.Undefined && !Rnd.OneChanceFrom(3))
                        {
                            //if (pLoc2.SubType != LocationSubType.Undefined)
                            //    throw new Exception();
                            pLink2.Key.Type = LocationType.Field;
                            foreach (var pLink3 in pLink2.Key.m_cEdges)
                            {
                                if (pLink3.Key.Type == LocationType.Undefined && Rnd.OneChanceFrom(2))
                                {
                                    //if (pLoc3.SubType != LocationSubType.Undefined)
                                    //    throw new Exception();
                                    pLink3.Key.Type = LocationType.Field;
                                }
                            }
                        }
                    }
                }
            }
        }

        public string ShortName
        {
            get
            {
                if (m_pSettlement != null)
                    return string.Format("{0} {1} {2}", m_pOwner.m_sName,
                        m_pOwner.m_pInfo.m_sName, m_pSettlement.m_pInfo.m_sName.ToLower());
                else
                    return string.Format("{0} {1} {2}", m_pOwner.m_sName,
                        m_pOwner.m_pInfo.m_sName, m_pTerritory.m_sName);
            }
        }

        public string FullName
        {
            get
            {
                if (m_pSettlement != null)
                    return string.Format("{0} {1} {2} {3}", m_pOwner.m_sName,
                        m_pOwner.m_pInfo.m_sName, m_pSettlement.m_pInfo.m_sName.ToLower(), m_pSettlement.m_sName);
                else
                    return string.Format("{0} {1} {2}", m_pOwner.m_sName,
                        m_pOwner.m_pInfo.m_sName, m_pTerritory.m_sName);
            }
        }
    }
}
