using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MapDrawEngine.Signs
{
    public struct SignVolkano : ILandMark
    {
        float x, y, r;
        string name;
        PointF[] points1;
        PointF[] points2;

        //internal static Font s_pFont = new Font("Arial", 10);
        //internal static Pen s_pBlack1Pen = new Pen(Color.Black, 1);

        public SignVolkano(float iX, float iY, float RX, string sName)
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
            points1 = cPoints.ToArray();

            cPoints.Clear();
            cPoints.Add(new PointF(x - r / 2, y - r));
            cPoints.Add(new PointF(x, y - 2 * r));
            cPoints.Add(new PointF(x + r / 2, y - r));
            cPoints.Add(new PointF(x, y - r / 2));
            points2 = cPoints.ToArray();

            name = sName;

            //s_pBlack1Pen.DashStyle = DashStyle.Dot;
        }

        public void Draw(Graphics gr, float fScaleMultiplier, float iDX, float iDY, float fScale)
        {
            float xx = x * fScale;
            float yy = y * fScale;

            float rr = r * fScale;

            gr.FillEllipse(Brushes.Silver, xx + iDX - rr, yy + iDY - rr / 2, rr * 2, rr);

            PointF[] pts1 = (PointF[])points1.Clone();
            for (int i = 0; i < pts1.Length; i++)
            {
                pts1[i].X *= fScale;
                pts1[i].Y *= fScale;

                pts1[i].X += iDX;
                pts1[i].Y += iDY;
            }
            gr.FillPolygon(Brushes.Silver, pts1);
            gr.DrawLines(Pens.Black, pts1);

            PointF[] pts2 = (PointF[])points2.Clone();
            for (int i = 0; i < pts2.Length; i++)
            {
                pts2[i].X *= fScale;
                pts2[i].Y *= fScale;

                pts2[i].X += iDX;
                pts2[i].Y += iDY;
            }
            gr.FillPolygon(Brushes.Red, pts2);
            gr.DrawLines(MapDraw.s_pRed2Pen, pts2);

            //if (name != null)
            //    gr.DrawString(name, s_pFont, Brushes.Black, x + r, y);
        }

        public void Translate(float fDX, float fDY)
        {
            x += fDX;
            y += fDY;

            for (int i = 0; i < points1.Length; i++)
            {
                points1[i].X += fDX;
                points1[i].Y += fDY;
            }

            for (int i = 0; i < points2.Length; i++)
            {
                points2[i].X += fDX;
                points2[i].Y += fDY;
            }
        }
    }
}
