using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MapDrawEngine.Signs
{
    public struct SignRuin : ILandMark
    {
        float x, y, r1;
        string name;
        PointF[] points;

        //internal static Font s_pFont = new Font("Arial", 10);
        //internal static Pen s_pBlack1Pen = new Pen(Color.Black, 1);

        public SignRuin(float iX, float iY, string sName)
        {
            x = iX;
            y = iY;

            r1 = 120;

            List<PointF> cPoints = new List<PointF>();
            cPoints.Add(new PointF(x, y - r1));
            cPoints.Add(new PointF(x + r1, y));
            cPoints.Add(new PointF(x, y + r1));
            cPoints.Add(new PointF(x - r1, y));
            points = cPoints.ToArray();

            name = sName;

            //s_pBlack1Pen.DashStyle = DashStyle.Dot;
        }

        public void Draw(Graphics gr, float fScaleMultiplier, float iDX, float iDY, float fScale)
        {
            if (fScaleMultiplier <= 2)
                return;

            PointF[] pts = (PointF[])points.Clone();
            for (int i = 0; i < pts.Length; i++)
            {
                pts[i].X *= fScale;
                pts[i].Y *= fScale;

                pts[i].X += iDX;
                pts[i].Y += iDY;
            }

            gr.FillPolygon(Brushes.Silver, pts);
            gr.DrawPolygon(MapDraw.s_pBlack2Pen, pts);

            //if (name != null)
            //    gr.DrawString(name, s_pFont, Brushes.Black, x + r, y);
        }

        public void Translate(float fDX, float fDY)
        {
            x += fDX;
            y += fDY;

            for (int i = 0; i < points.Length; i++)
            {
                points[i].X += fDX;
                points[i].Y += fDY;
            }
        }
    }
}
