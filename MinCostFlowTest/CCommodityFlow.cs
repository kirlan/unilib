using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MinCostFlow;
using System.Drawing;

namespace MinCostFlowTest
{
    internal class Point
    {
        public int x;
        public int y;
        public Color color;
        public string Name;

        public override string ToString()
        {
            return Name;
        }

        public int DistanceTo(Point pt2)
        {
            return (int)Math.Sqrt((x - pt2.x) * (x - pt2.x) + (y - pt2.y) * (y - pt2.y));
        }
    }

    class CCommodityFlow
    {
        private CFlowsGrid m_pGrid = new CFlowsGrid();

        private List<CVertex<Point>> m_cOffer = new List<CVertex<Point>>();
        private List<CVertex<Point>> m_cDemand = new List<CVertex<Point>>();

        public CVertex<Point> AddOffer(Point pt, int power)
        {
            CVertex<Point> vp = new CVertex<Point>(pt, power);

            m_pGrid.AddVertex(vp);
            m_cOffer.Add(vp);

            foreach (CVertex<Point> dem in m_cDemand)
                vp.AddLink(dem, pt.DistanceTo(dem.Object));
        }

        public CVertex<Point> AddDemand(Point pt, int power)
        {
            CVertex<Point> vp = new CVertex<Point>(pt, power);

            m_pGrid.AddVertex(vp);
            m_cDemand.Add(vp);

            foreach (CVertex<Point> off in m_cOffer)
                off.AddLink(vp, off.Object.DistanceTo(pt));
        }

        public long GetOffer()
        {
            long offer = 0;

            foreach (CVertex<Point> off in m_cOffer)
                offer += off.Capacity;

            return offer;
        }

        public long GetDemand()
        {
            long demand = 0;

            foreach (CVertex<Point> dem in m_cDemand)
                demand += dem.Capacity;

            return demand;
        }

        public bool Resolve()
        {
            long offer = GetOffer();
            
            if (offer != GetDemand())
                return false;

            Point pts = new Point();
            pts.Name = "start";
            CVertex<Point> vs = new CVertex<Point>(pts, (int)offer);
            m_pGrid.AddVertex(vs);

            Point ptt = new Point();
            ptt.Name = "finish";
            CVertex<Point> vt = new CVertex<Point>(ptt, (int)offer);
            m_pGrid.AddVertex(vt);

            foreach (CVertex<Point> off in m_cOffer)
                vs.AddLink(off, 0);

            foreach (CVertex<Point> dem in m_cDemand)
                dem.AddLink(vt, 0);

            m_pGrid.MinCostFlow(vs.Entrance, vt.Exit);

            return true;
        }
    }
}
