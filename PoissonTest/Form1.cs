using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LandscapeGeneration;
using SimpleVectors;

namespace PoissonTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ActualMap = new Bitmap(panel1.Width, panel1.Height);
        }

        Bitmap ActualMap;

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            float W = 50000;// panel1.Width;
            float H = 50000;// panel1.Height;

            var count = 10000;// 1000;
            var Sbig = W * H;
            var S1 = Sbig / count;
            var h = Math.Sqrt(S1/(4*Math.Sqrt(3)));
            var R = Math.Sqrt(S1/(3*Math.Sqrt(3)));

            R = (R + h) / 2;
            R *= 0.96;

            int i = 0;
            List<SimpleVector3d> cPoints = new List<SimpleVector3d>();
            do
            {
                label2.Text = "i=" + i.ToString() + " R=" + R.ToString("F1");
                cPoints = UniformPoissonDiskSampler.SampleRectangle(new SimpleVector3d(0, 0, 0),
                                                        new SimpleVector3d(W, H, 0),
                                                        (float)R*2);
                R *= 0.99f;
                i++;
            }
            while (cPoints.Count < count);

            label1.Text = cPoints.Count.ToString();
            DrawMap(cPoints);

            button1.Enabled = true;
        }

        public void DrawMap(List<SimpleVector3d> cPoints)
        {
            Graphics gr = Graphics.FromImage(ActualMap);

            Brush fill = Brushes.Black;
            gr.FillRectangle(fill, 0, 0, ClientRectangle.Width, ClientRectangle.Height);

            var fScale = (float)Math.Min(ClientRectangle.Width, ClientRectangle.Height) / 50000;
            foreach (var pPoint in cPoints)
            {
                long r = 2;
                gr.FillEllipse(Brushes.Red, (int)(pPoint.X * fScale - r), (int)(pPoint.Y * fScale - r), 2 * r, 2 * r);
            }

            panel1.Refresh();
        }
        
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(ActualMap, 0, 0);
        }
    }
}
