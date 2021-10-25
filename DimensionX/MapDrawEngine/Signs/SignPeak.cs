using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MapDrawEngine.Signs
{
    public struct SignPeak : ILandMark
    {
        float x, y, r;
        string name;
        PointF[] points;

        //internal static Font s_pFont = new Font("Arial", 10);
        //internal static Pen s_pBlack1Pen = new Pen(Color.Black, 1);

        public SignPeak(float iX, float iY, float RX, string sName)
        {
            x = iX;
            y = iY;

            r = RX / 250;
            //if (r < 1)
            //    r = 1;

            List<PointF> cPoints = new List<PointF>();
            cPoints.Add(new PointF(x - r, y));
            cPoints.Add(new PointF(x, y - 2 * r));
            cPoints.Add(new PointF(x + r, y));
            points = cPoints.ToArray();

            name = sName;

            //s_pBlack1Pen.DashStyle = DashStyle.Dot;
        }

        public void Draw(Graphics gr, float fScaleMultiplier, float iDX, float iDY, float fScale)
        {
            float xx = x * fScale;
            float yy = y * fScale;

            float rr = r * fScale;

            gr.FillEllipse(Brushes.Silver, xx + iDX - rr, yy + iDY - rr / 2, rr * 2, rr);

            PointF[] pts = (PointF[])points.Clone();
            for (int i = 0; i < pts.Length; i++)
            {
                pts[i].X *= fScale;
                pts[i].Y *= fScale;

                pts[i].X += iDX;
                pts[i].Y += iDY;
            }

            gr.FillPolygon(Brushes.Silver, pts);
            gr.DrawLines(Pens.Black, pts);

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
