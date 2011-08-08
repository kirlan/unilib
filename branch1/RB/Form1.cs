using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Random;

namespace RB
{
    public partial class Form1 : Form
    {
        CWorld pWorld = null;

        public Form1()
        {
            InitializeComponent();

            ActualMap = new Bitmap(panel1.Width, panel1.Height);
        }

        private Dictionary<CSociety, Color> m_cColors = new Dictionary<CSociety,Color>();
        Bitmap ActualMap;
        public void DrawMap()
        {
            m_cColors.Clear();
            for (int i = 0; i < pWorld.Societies.Count; i++)
            {
                switch (Rnd.Get(3))
                {
                    case 0:
                        m_cColors[pWorld.Societies[i]] = Color.FromArgb(255, Rnd.Get(168), Rnd.Get(168));
                        break;
                    case 1:
                        m_cColors[pWorld.Societies[i]] = Color.FromArgb(Rnd.Get(168), 255, Rnd.Get(168));
                        break;
                    case 2:
                        m_cColors[pWorld.Societies[i]] = Color.FromArgb(Rnd.Get(168), Rnd.Get(168), 255);
                        break;
                }
            } 
            
            Graphics gr = Graphics.FromImage(ActualMap);

            Brush fill = Brushes.Black;
            gr.FillRectangle(fill, 0, 0, panel1.Width, panel1.Height);

            if (pWorld == null)
                return;

            foreach (CLocation pLoc1 in pWorld.Locations)
            {
                foreach (CLocation pLoc2 in pLoc1.Links)
                {
                    Color pen = Color.DimGray;
                    if (pLoc1.Owner != null && pLoc1.Owner == pLoc2.Owner)
                        pen = m_cColors[pLoc1.Owner];
                    gr.DrawLine(new Pen(pen, 2), pWorld.WorldScale + pLoc1.X, pWorld.WorldScale + pLoc1.Y,
                        pWorld.WorldScale + pLoc2.X, pWorld.WorldScale + pLoc2.Y);
                }

            }

            foreach (CLocation pLoc in pWorld.Locations)
            {
                long r = 6;
                if (pLoc.Type != LocationType.Settlement)
                    r = 3;
                if (pLoc.Owner != null)
                    gr.FillEllipse(new SolidBrush(m_cColors[pLoc.Owner]), pWorld.WorldScale + pLoc.X - r, pWorld.WorldScale + pLoc.Y - r, 2 * r, 2 * r);
                else
                    gr.FillEllipse(Brushes.DimGray, pWorld.WorldScale + pLoc.X - r, pWorld.WorldScale + pLoc.Y - r, 2 * r, 2 * r);
            }

            panel1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pWorld = new CWorld(250, 3);

            DrawMap();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(ActualMap, 0, 0);
        }
    }
}
