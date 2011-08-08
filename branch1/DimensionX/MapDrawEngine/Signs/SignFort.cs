using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MapDrawEngine.Signs
{
    public struct SignFort : ILandMark
    {
        float x, y, r1, r2;
        string name;
        PointF[] points;

        internal static Font s_pFont = new Font("Arial", 10);

        public SignFort(float iX, float iY, string sName)
        {
            x = iX;
            y = iY;

            r1 = 133;
            r2 = 200;

            List<PointF> cPoints = new List<PointF>();
            cPoints.Add(new PointF(x, y + r1));
            cPoints.Add(new PointF(x + r2, y));
            cPoints.Add(new PointF(x + r1, y - r2));
            cPoints.Add(new PointF(x - r1, y - r2));
            cPoints.Add(new PointF(x - r2, y));

            points = cPoints.ToArray();

            name = sName;
        }

        public void Draw(Graphics gr, float fScaleMultiplier, float iDX, float iDY, float fScale)
        {
            if (fScaleMultiplier <= 2)
                return;

            float xx = x * fScale;
            float yy = y * fScale;

            float rr1 = r1 * fScale;
            if (rr1 < 1)
                rr1 = 1;
            float rr2 = r2 * fScale;
            if (rr2 <= rr1)
                rr2 = rr1 + 1;

            PointF[] pts = (PointF[])points.Clone();
            for (int i = 0; i < pts.Length; i++)
            {
                pts[i].X *= fScale;
                pts[i].Y *= fScale;

                pts[i].X += iDX;
                pts[i].Y += iDY;
            }

            gr.FillPolygon(Brushes.Gray, pts);
            gr.DrawPolygon(MapDraw.s_pBlack2Pen, pts);

            if (fScaleMultiplier > 4)
                gr.DrawString(name, s_pFont, Brushes.Black, xx + iDX + rr1, yy + iDY);
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
