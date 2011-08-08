using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MinCostFlow;
using Random;

namespace MinCostFlowTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            ActualMap = new Bitmap(panel1.Width, panel1.Height);
        }

        CFlowsGrid m_pGrid = new CFlowsGrid();

        CNode s;
        CNode t;

        private List<CVertex<Point>> m_cVertexes = new List<CVertex<Point>>();

        public void DrawMap(bool showEmpty)
        {
            Graphics gr = Graphics.FromImage(ActualMap);

            Brush fill = Brushes.Black;
            gr.FillRectangle(fill, 0, 0, ClientRectangle.Width, ClientRectangle.Height);

            if (showEmpty)
            {
                //foreach (CVertex<Point> pv in m_cVertexes)
                //{
                //    Point pt = pv.Object;

                //    foreach (CVertex<Point> pv2 in pv.Links.Keys)
                //    {
                //        if (pv2.Exit != t)
                //        {
                //            Point pt2 = pv2.Object;

                //            gr.DrawLine(new Pen(Brushes.LightSteelBlue, 2 + pv.Links[pv2].CurrentFlow / 2), pt.x, pt.y,
                //                pt2.x, pt2.y);
                //            gr.DrawLine(new Pen(Brushes.AntiqueWhite, pv.Links[pv2].CurrentFlow / 2), pt.x, pt.y,
                //                pt2.x, pt2.y);
                //        }
                //    }

                //}
            }

            foreach (CVertex<Point> pv in m_cVertexes)
            {
                Point pt = pv.Object;

                foreach (CVertex<Point> pv2 in pv.Links.Keys)
                {
                    if (pv2.Exit != t)
                    {
                        Point pt2 = pv2.Object;
                        if (pv.Links[pv2].CurrentFlow > 0)
                        {
                            gr.DrawLine(new Pen(pt.color, pv.Links[pv2].CurrentFlow / 2), pt.x, pt.y,
                                pt2.x, pt2.y);
                        }
                    }
                }

            }

            foreach (CVertex<Point> pv in m_cVertexes)
            {
                Point pt = pv.Object;
                long r = pv.Capacity / 2;
                if (r <= 0)
                    r = 1;
                gr.FillEllipse(showEmpty ? Brushes.Red : new SolidBrush(pt.color), pt.x - r, pt.y - r, 2 * r, 2 * r);

                if (pv.Flow > 0)
                {
                    r = pv.Flow / 2;
                    if (r <= 0)
                        r = 1;
                    gr.FillEllipse(new SolidBrush(pt.color), pt.x - r, pt.y - r, 2 * r, 2 * r);
                }
            }

            panel1.Refresh();
        }

        Bitmap ActualMap;

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(ActualMap, 0, 0);
        }

        private void Resolve(object sender, EventArgs e)
        {
            m_pGrid.MinCostFlow(s, t);

            DrawMap(true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private int[] Distributor(int total, int max)
        {
            List<int> result = new List<int>();

            int basis = (int)Math.Sqrt(max);
            if (basis <= 0)
                basis = 1;

            while(total > 0)
            {
                int next = Math.Min(1 + Rnd.Get(basis) * Rnd.Get(basis), total);
                result.Add(next);
                total -= next;
            }

            return result.ToArray();
        }

        private void BuildDistributed(object sender, EventArgs e)
        {
            m_pGrid = new CFlowsGrid();
            m_cVertexes.Clear();

            Point pts = new Point();
            pts.x = panel1.Width / 2;
            pts.y = panel1.Height / 20;
            pts.Name = "start";

            CVertex<Point> vs = new CVertex<Point>(pts, 50);
            m_pGrid.AddVertex(vs);
            s = vs.Entrance;

            Point ptt = new Point();
            ptt.x = panel1.Width / 2;
            ptt.y = 5 * panel1.Height / 6;
            ptt.Name = "finish";

            CVertex<Point> vt = new CVertex<Point>(ptt, 50);
            m_pGrid.AddVertex(vt);
            t = vt.Exit;

            List<CVertex<Point>> cMines = new List<CVertex<Point>>();

            int[] distr = Distributor(50, 9);

            for (int i = 0; i < distr.Length; i++)
            {
                Point pt = new Point();
                //pt.x = (panel1.Width - panel1.Width / 10) * i / count + panel1.Width / (2 * count);
                //pt.y = panel1.Height / 6;
                pt.x = Rnd.Get(panel1.Width);
                pt.y = Rnd.Get(panel1.Height);
                pt.color = Color.Green;
                pt.Name = string.Format("Mine {0}", i);

                CVertex<Point> vp = new CVertex<Point>(pt, distr[i]);
                m_pGrid.AddVertex(vp);
                m_cVertexes.Add(vp);

                vs.AddLink(vp, 0);

                cMines.Add(vp);
            }

            List<CVertex<Point>> cFactories1 = new List<CVertex<Point>>();

            distr = Distributor(50, 16);

            for (int i = 0; i < distr.Length; i++)
            {
                Point pt = new Point();
                //pt.x = (panel1.Width - panel1.Width / 10) * i / count + panel1.Width / (2 * count);
                //pt.y = 2*panel1.Height / 6;
                pt.x = Rnd.Get(panel1.Width);
                pt.y = Rnd.Get(panel1.Height);
                pt.color = Color.Aqua;
                pt.Name = string.Format("F1-{0}", i);

                CVertex<Point> vp = new CVertex<Point>(pt, distr[i]);
                m_pGrid.AddVertex(vp);
                m_cVertexes.Add(vp);

                foreach (CVertex<Point> vp0 in cMines)
                    vp0.AddLink(vp, Distance(vp.Object, vp0.Object));

                cFactories1.Add(vp);
            }

            List<CVertex<Point>> cFactories2 = new List<CVertex<Point>>();

            distr = Distributor(50, 25);

            for (int i = 0; i < distr.Length; i++)
            {
                Point pt = new Point();
                //pt.x = (panel1.Width - panel1.Width / 10) * i / count + panel1.Width / (2 * count);
                //pt.y = 3*panel1.Height / 6;
                pt.x = Rnd.Get(panel1.Width);
                pt.y = Rnd.Get(panel1.Height);
                pt.color = Color.Blue;
                pt.Name = string.Format("F2-{0}", i);

                CVertex<Point> vp = new CVertex<Point>(pt, distr[i]);
                m_pGrid.AddVertex(vp);
                m_cVertexes.Add(vp);

                foreach (CVertex<Point> vp0 in cFactories1)
                    vp0.AddLink(vp, Distance(vp.Object, vp0.Object));

                cFactories2.Add(vp);
            }

            List<CVertex<Point>> cSettlements = new List<CVertex<Point>>();

            distr = Distributor(50, 9);

            for (int i = 0; i < distr.Length; i++)
            {
                Point pt = new Point();
                //pt.x = (panel1.Width - panel1.Width / 10) * i / count + panel1.Width / (2 * count);
                //pt.y = 4*panel1.Height / 6;
                pt.x = Rnd.Get(panel1.Width);
                pt.y = Rnd.Get(panel1.Height);
                pt.color = Color.Violet;
                pt.Name = string.Format("S-{0}", i);

                CVertex<Point> vp = new CVertex<Point>(pt, distr[i]);
                m_pGrid.AddVertex(vp);
                m_cVertexes.Add(vp);

                foreach (CVertex<Point> vp0 in cFactories2)
                    vp0.AddLink(vp, Distance(vp.Object, vp0.Object));

                vp.AddLink(vt, 0);

                cSettlements.Add(vp);
            }

            DrawMap(false);
        }

        private void BuildEqual(object sender, EventArgs e)
        {
            m_pGrid = new CFlowsGrid();
            m_cVertexes.Clear();

            List<CVertex<Point>> cSettlements = GenerateSettlements();

            List<CVertex<Point>> freeWorkers = new List<CVertex<Point>>();
            List<CVertex<Point>> workingWorkers = new List<CVertex<Point>>();

            pop = 0;
            prod = 0;

            foreach (CVertex<Point> vp in cSettlements)
            {
                pop += vp.Capacity;
                prod += vp.Capacity * vp.Capacity;
                freeWorkers.Add(vp);
            }

            k = (prod - 3 * pop) / 2;

            Point pts = new Point();
            pts.x = 0;
            pts.y = 0;
            pts.Name = "start";

            CVertex<Point> vs = new CVertex<Point>(pts, (int)pop);
            m_pGrid.AddVertex(vs);
            s = vs.Entrance;

            Point ptt = new Point();
            ptt.x = panel1.Width;
            ptt.y = panel1.Height;
            ptt.Name = "finish";

            CVertex<Point> vt = new CVertex<Point>(ptt, (int)pop);
            m_pGrid.AddVertex(vt);
            t = vt.Exit;

            int j = 0;

            List<CVertex<Point>> cFactories2 = new List<CVertex<Point>>();

            prod = pop;
            foreach (CVertex<Point> worker in freeWorkers)
            {
                if (prod >= worker.Capacity * worker.Capacity)
                {
                    Point pt = new Point();
                    pt.x = worker.Object.x;
                    pt.y = worker.Object.y;
                    pt.color = Color.Blue;
                    pt.Name = string.Format("F2-{0}", j);

                    long local = worker.Capacity * worker.Capacity;
                    CVertex<Point> vp = new CVertex<Point>(pt, (int)local);
                    m_pGrid.AddVertex(vp);
                    m_cVertexes.Add(vp);

                    foreach (CVertex<Point> vp0 in cSettlements)
                        vp.AddLink(vp0, Distance(vp.Object, vp0.Object));

                    prod -= local;

                    cFactories2.Add(vp);
                    workingWorkers.Add(worker);
                }

                if (prod == 0)
                    break;
            }

            foreach (CVertex<Point> worker in workingWorkers)
                freeWorkers.Remove(worker);
            workingWorkers.Clear();

            List<CVertex<Point>> cFactories1 = new List<CVertex<Point>>();

            prod = pop;
            foreach (CVertex<Point> worker in freeWorkers)
            {
                if (prod >= worker.Capacity * worker.Capacity)
                {
                    Point pt = new Point();
                    pt.x = worker.Object.x;
                    pt.y = worker.Object.y;
                    pt.color = Color.Aqua;
                    pt.Name = string.Format("F1-{0}", j);

                    long local = worker.Capacity * worker.Capacity;
                    CVertex<Point> vp = new CVertex<Point>(pt, (int)local);
                    m_pGrid.AddVertex(vp);
                    m_cVertexes.Add(vp);

                    foreach (CVertex<Point> vp0 in cFactories2)
                        vp.AddLink(vp0, Distance(vp.Object, vp0.Object));

                    prod -= local;

                    cFactories1.Add(vp);
                    workingWorkers.Add(worker);
                }

                if (prod == 0)
                    break;
            }

            foreach (CVertex<Point> worker in workingWorkers)
                freeWorkers.Remove(worker);
            workingWorkers.Clear();

            List<CVertex<Point>> cMines = new List<CVertex<Point>>();

            prod = pop;
            foreach (CVertex<Point> worker in freeWorkers)
            {
                if (prod >= worker.Capacity * worker.Capacity)
                {
                    Point pt = new Point();
                    pt.x = worker.Object.x;
                    pt.y = worker.Object.y;
                    pt.color = Color.Green;
                    pt.Name = string.Format("Mine {0}", j);

                    long local = worker.Capacity * worker.Capacity;
                    CVertex<Point> vp = new CVertex<Point>(pt, (int)local);
                    m_pGrid.AddVertex(vp);
                    m_cVertexes.Add(vp);

                    vs.AddLink(vp, 0);
                    foreach (CVertex<Point> vp0 in cFactories1)
                        vp.AddLink(vp0, Distance(vp.Object, vp0.Object));

                    prod += local;

                    cMines.Add(vp);
                    workingWorkers.Add(worker);
                }

                if (prod == 0)
                    break;
            }

            foreach (CVertex<Point> worker in workingWorkers)
                freeWorkers.Remove(worker);
            workingWorkers.Clear();

            foreach(CVertex<Point> vp in cSettlements)
            {
                Point pt = vp.Object;
                pt.color = Color.Violet;
                pt.Name = string.Format("S-{0}", j);

                vp.AddLink(vt, 0);

                m_pGrid.AddVertex(vp);
                m_cVertexes.Add(vp);
            }

            Dictionary<Color, long> check = new Dictionary<Color, long>();
            check[Color.Violet] = 0;
            check[Color.Green] = 0;
            check[Color.Aqua] = 0;
            check[Color.Blue] = 0;

            foreach (CVertex<Point> vp in m_cVertexes)
            {
                check[vp.Object.color] += vp.Capacity;
            }

            DrawMap(false);
        }

        private void GetMaxFlow(object sender, EventArgs e)
        {
            m_pGrid.MaxFlowFF(s, t);

            DrawMap(true);
        }

        private List<CVertex<Point>> GenerateSettlements()
        {
            List<CVertex<Point>> cSettlements = new List<CVertex<Point>>();

            for (int i = 0; i < 16; i++)
            {
                Point pt = new Point();
                pt.x = Rnd.Get(panel1.Width);
                pt.y = Rnd.Get(panel1.Height);

                int eff = 4 + Rnd.Get(3);

                pt.Name = eff.ToString();

                CVertex<Point> vp = new CVertex<Point>(pt, eff);
                cSettlements.Add(vp);
            }

            for (int i = 0; i < 25; i++)
            {
                Point pt = new Point();
                pt.x = Rnd.Get(panel1.Width);
                pt.y = Rnd.Get(panel1.Height);

                int eff = 3 + Rnd.Get(2);

                pt.Name = eff.ToString();

                CVertex<Point> vp = new CVertex<Point>(pt, eff);
                cSettlements.Add(vp);
            }

            long pop = 0;
            long prod = 0;

            foreach (CVertex<Point> vp in cSettlements)
            {
                pop += vp.Capacity;
                prod += vp.Capacity * vp.Capacity;
            }

            long k = (prod - 3 * pop) / 2;

            for (int i = 0; i < k; i++)
            {
                {
                    Point pt = new Point();
                    pt.x = Rnd.Get(panel1.Width);
                    pt.y = Rnd.Get(panel1.Height);

                    int eff = 1 + Rnd.Get(2);

                    pt.Name = eff.ToString();

                    CVertex<Point> vp = new CVertex<Point>(pt, eff);
                    cSettlements.Add(vp);
                }
            }

            return cSettlements;
        }

        private void BuildComplex(object sender, EventArgs e)
        {
            m_pGrid = new CFlowsGrid();
            m_cVertexes.Clear();

            List<CVertex<Point>> cSettlements = GenerateSettlements();

            List<CVertex<Point>> freeWorkers = new List<CVertex<Point>>();
            List<CVertex<Point>> workingWorkers = new List<CVertex<Point>>();

            int pop = 0;
            int prod = 0;

            foreach (CVertex<Point> vp in cSettlements)
            {
                pop += vp.Capacity;
                prod += vp.Capacity * vp.Capacity;
                freeWorkers.Add(vp);
            }

            //ТУТ

            int j = 0;

            List<CVertex<Point>> cFactories2 = new List<CVertex<Point>>();

            prod = pop;
            foreach (CVertex<Point> worker in freeWorkers)
            {
                if (prod >= worker.Capacity * worker.Capacity)
                {
                    Point pt = new Point();
                    pt.x = worker.Object.x;
                    pt.y = worker.Object.y;
                    pt.color = Color.Blue;
                    pt.Name = string.Format("F2-{0}", j);

                    long local = worker.Capacity * worker.Capacity;
                    CVertex<Point> vp = new CVertex<Point>(pt, (int)local);
                    m_pGrid.AddVertex(vp);
                    m_cVertexes.Add(vp);

                    foreach (CVertex<Point> vp0 in cSettlements)
                        vp.AddLink(vp0, Distance(vp.Object, vp0.Object));

                    prod -= local;

                    cFactories2.Add(vp);
                    workingWorkers.Add(worker);
                }

                if (prod == 0)
                    break;
            }

            foreach (CVertex<Point> worker in workingWorkers)
                freeWorkers.Remove(worker);
            workingWorkers.Clear();

            List<CVertex<Point>> cFactories1 = new List<CVertex<Point>>();

            prod = pop;
            foreach (CVertex<Point> worker in freeWorkers)
            {
                if (prod >= worker.Capacity * worker.Capacity)
                {
                    Point pt = new Point();
                    pt.x = worker.Object.x;
                    pt.y = worker.Object.y;
                    pt.color = Color.Aqua;
                    pt.Name = string.Format("F1-{0}", j);

                    long local = worker.Capacity * worker.Capacity;
                    CVertex<Point> vp = new CVertex<Point>(pt, (int)local);
                    m_pGrid.AddVertex(vp);
                    m_cVertexes.Add(vp);

                    foreach (CVertex<Point> vp0 in cFactories2)
                        vp.AddLink(vp0, Distance(vp.Object, vp0.Object));

                    prod -= local;

                    cFactories1.Add(vp);
                    workingWorkers.Add(worker);
                }

                if (prod == 0)
                    break;
            }

            foreach (CVertex<Point> worker in workingWorkers)
                freeWorkers.Remove(worker);
            workingWorkers.Clear();

            List<CVertex<Point>> cMines = new List<CVertex<Point>>();

            prod = pop;
            foreach (CVertex<Point> worker in freeWorkers)
            {
                if (prod >= worker.Capacity * worker.Capacity)
                {
                    Point pt = new Point();
                    pt.x = worker.Object.x;
                    pt.y = worker.Object.y;
                    pt.color = Color.Green;
                    pt.Name = string.Format("Mine {0}", j);

                    long local = worker.Capacity * worker.Capacity;
                    CVertex<Point> vp = new CVertex<Point>(pt, (int)local);
                    m_pGrid.AddVertex(vp);
                    m_cVertexes.Add(vp);

                    vs.AddLink(vp, 0);
                    foreach (CVertex<Point> vp0 in cFactories1)
                        vp.AddLink(vp0, Distance(vp.Object, vp0.Object));

                    prod += local;

                    cMines.Add(vp);
                    workingWorkers.Add(worker);
                }

                if (prod == 0)
                    break;
            }

            foreach (CVertex<Point> worker in workingWorkers)
                freeWorkers.Remove(worker);
            workingWorkers.Clear();

            foreach (CVertex<Point> vp in cSettlements)
            {
                Point pt = vp.Object;
                pt.color = Color.Violet;
                pt.Name = string.Format("S-{0}", j);

                vp.AddLink(vt, 0);

                m_pGrid.AddVertex(vp);
                m_cVertexes.Add(vp);
            }

            Dictionary<Color, long> check = new Dictionary<Color, long>();
            check[Color.Violet] = 0;
            check[Color.Green] = 0;
            check[Color.Aqua] = 0;
            check[Color.Blue] = 0;

            foreach (CVertex<Point> vp in m_cVertexes)
            {
                check[vp.Object.color] += vp.Capacity;
            }

            DrawMap(false);
        }

    }
}
