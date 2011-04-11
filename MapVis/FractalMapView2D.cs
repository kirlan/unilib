using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MapGen;
using System.Drawing.Imaging;
using Random;
using SimpleVectors;

namespace MapVis
{
    public partial class FractalMapView2D : UserControl
    {
        public enum DisplayMode
        {
            Height,
            ID
        }

        public FractalMapView2D()
        {
            InitializeComponent();

            m_pMask = new CFRegion[ClientRectangle.Width][];
            for (int i = 0; i < ClientRectangle.Width; i++)
                m_pMask[i] = new CFRegion[ClientRectangle.Height];

            ActualMap = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
        }

        private bool m_bFullProection;

        public bool FullProection
        {
            get { return m_bFullProection; }
            set { m_bFullProection = value; }
        }

        private DisplayMode m_eMode = DisplayMode.Height;

        public DisplayMode Mode
        {
            get { return m_eMode; }
            set { m_eMode = value; }
        }

        private CFRegion[][] m_pMask;

        //private void ClearMask()
        //{
        //    foreach (CFRegion[] line in m_pMask)
        //    {
        //        foreach (CFRegion reg in line)
        //            reg = null;
        //    }
        //}

        Bitmap ActualMap;

        readonly private Point[] aCoor1 = {new Point(0,0),
				           new Point(+1,+1),
				           new Point(+2,0)};
        readonly private Point[] aCoor2 = {new Point(0,+1),
				           new Point(+2,+1),
				           new Point(+1,0)};

        private CFractalWorld world = null;

        public void SaveMap(string filename)
        {
            if (world == null)
                return;

            if (world.Map == null)
                return;

            if (world.Map.Width == 0)
                return;

            ActualMap.Save(filename, ImageFormat.Jpeg);
        }

        public void DrawMap(CFractalWorld pWorld)
        {
            world = pWorld;
            DrawMap();
        }

        private Color[] m_aColorsID;

        /*
        public void DrawMap()
        {
            Graphics gr = Graphics.FromImage(ActualMap);

            Brush fill = Brushes.Black;
            gr.FillRectangle(fill, 0, 0, ClientRectangle.Width, ClientRectangle.Height);

            if (m_pMask == null)
		        return;

	        if(world == null)
		        return;

	        if(world.Map == null)
		        return;

	        if(world.Map.Width == 0)
		        return;

	        //m_pFractal->m_iWater = Water->Value;

            //ClearMask();

            Bitmap Temp = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            Graphics grTemp = Graphics.FromImage(Temp);

            m_aColorsID = new Color[world.Map.LandMass.Count];
            for (int i = 0; i < world.Map.LandMass.Count; i++)
            {
                m_aColorsID[i] = Color.FromArgb(128 + Rnd.Get(128), 128 + Rnd.Get(128), 128 + Rnd.Get(128));
            }

	        float iStepX, iStepY;
	        iStepX = (float)ClientRectangle.Width/world.Map.Width;
	        iStepY = (float)ClientRectangle.Height/world.Map.Height;

	        int x,y;
	        for(y=0; y<world.Map.Height; y++)
	        {
		        for(x=0; x<world.Map.Width; x++)
		        {
			        bool border;
			        border = false;
			        Point[] points = new Point[3];
			        int i;
			        for(i=0; i<3; i++)
			        {
				        points[i] = new Point(0,0);
				        if(m_bFullProection && (y<world.Map.Height/3 || y>=2*world.Map.Height/3))
				        {
					        float iCrazyStepX = 0;
					        if(y<world.Map.Height/3)
        //					if(y+1 == m_pFractal->HEIGHT/3 && (x%2 == y%2))
					        {
						        int xx;
						        xx = x%(world.Map.Width/5);

						        int iNewX, iNewY;

						        if(x%2 != y%2)
						        {
							        iNewX = x + aCoor1[i].X;
							        iNewY = y + aCoor1[i].Y;
						        }
						        else
						        {
							        iNewX = x + aCoor2[i].X;
							        iNewY = y + aCoor2[i].Y;
						        }

						        if(xx >= world.Map.Width/10 - y && xx <= world.Map.Width/10 + y)
						        {
							        float startX;
							        startX = iStepX*(x - xx + 1);

							        if(iNewY != 0)
							        {
								        iCrazyStepX = (float)ClientRectangle.Width/(iNewY*10);
								        points[i] = new Point((int)(startX + iCrazyStepX*((float)xx -1 + (iNewX-x) - (world.Map.Width/10 - iNewY))),(int)(iStepY*iNewY));
							        }
							        else
							        {
								        points[i] = new Point((int)(startX + iCrazyStepX*((float)xx -1 + (iNewX-x) - (world.Map.Width/10 - iNewY))),-32000);
							        }
						        }
					        }
					        if(y>=2*world.Map.Height/3)
					        {
						        int xx,yy;
						        xx = (x+world.Map.Width/10)%(world.Map.Width/5);
						        yy = world.Map.Height - y-1;

						        int iNewX, iNewY;

						        if(x%2 != y%2)
						        {
							        iNewX = x + aCoor1[i].X;
							        iNewY = y + aCoor1[i].Y;
						        }
						        else
						        {
							        iNewX = x + aCoor2[i].X;
							        iNewY = y + aCoor2[i].Y;
						        }

						        if(xx >= world.Map.Width/10 - yy && xx <= world.Map.Width/10 + yy)
						        {
							        float startX;
							        startX = iStepX*(x - xx + 1);

							        if(iNewY != world.Map.Height)
							        {
								        iCrazyStepX = (float)ClientRectangle.Width/((world.Map.Height - iNewY)*10);
        //								points[i] = Point(startX + iCrazyStepX*((float)xx -1 + (iNewX-x)),iStepY*iNewY);
        //								points[i] = Point(startX + iCrazyStepX*((float)xx -1 + (iNewX-x) - (m_pFractal->WIDTH/10 - iNewY)),iStepY*iNewY);

								        points[i] = new Point((int)(startX + iCrazyStepX*((float)xx -1 + (iNewX-x) - (world.Map.Width/10 - (world.Map.Height - iNewY)))),(int)(iStepY*iNewY));
							        }
							        else
							        {
								        points[i] = new Point((int)(startX + iCrazyStepX*((float)xx -1 + (iNewX-x) - (world.Map.Width/10 - (iNewY - 2*world.Map.Height/3 +1)))),32000);
							        }
						        }
					        }
				        }
				        else
				        {
					        if(x%2 != y%2)
					        {
						        points[i] = new Point((int)(iStepX*(x + aCoor1[i].X)),(int)(iStepY*(y + aCoor1[i].Y)));
					        }
					        else
					        {
						        points[i] = new Point((int)(iStepX*(x + aCoor2[i].X)),(int)(iStepY*(y + aCoor2[i].Y)));
					        }
				        }
				        if(points[i].X > ClientRectangle.Width)
				        {
					        border = true;
				        }
			        }

                    Brush paint = Brushes.Black;
                    if (m_eMode == DisplayMode.Height)
                    {
                        int h;
                        h = world.Map.Regions[x][y].Height / 100;
                        if (h < 0)
                        {
                            h = 0;
                        }
                        if (h >= 256)
                        {
                            h = 255;
                        }

                        h = 20 * (h / 20);

                        if (world.Map.Regions[x][y].Height >= world.Map.OceanLevel)
                        {
                            paint = new SolidBrush(Color.FromArgb(h, h, 0));
                            //				Image1->Canvas->Brush->Color = 287*(m_pFractal->m_pMap[x][y].contiID*20);
                        }
                        else
                        {
                            paint = new SolidBrush(Color.FromArgb(0, 0, h));
                        }

                        if (world.Map.Regions[x][y].ContinentID == -255)
                        {
                            paint = Brushes.Blue;
                        }

                        if (!world.Map.Regions[x][y].Allowed)
                        {
                            paint = Brushes.Purple;
                        }
                    }
                    if (m_eMode == DisplayMode.ID)
                    {
                        if (world.Map.Regions[x][y].ContinentID >= 0 && world.Map.Regions[x][y].Height >= world.Map.OceanLevel)
                            paint = new SolidBrush(m_aColorsID[world.Map.Regions[x][y].ContinentID]);
                    }

                    gr.FillPolygon(paint,points);

                    Color tempColor = Color.FromArgb(y * world.Map.Width + x);
                    Brush tempBrush = new SolidBrush(tempColor);

                    grTemp.FillPolygon(tempBrush,points);

                    //int minx,miny,maxx,maxy;
                    //minx = 1024;
                    //miny = 512;
                    //maxx = 0;
                    //maxy = 0;
                    //for(int k=0; k<3; k++)
                    //{
                    //    if(points[k].X < minx)
                    //        minx = points[k].X;
                    //    if(points[k].X > maxx)
                    //        maxx = points[k].X;
                    //    if(points[k].Y < miny)
                    //        miny = points[k].Y;
                    //    if(points[k].Y > maxy)
                    //        maxy = points[k].Y;
                    //}

                    //if (minx < 0)
                    //    minx = 0;

                    //if (maxx > ClientRectangle.Width)
                    //    maxx = ClientRectangle.Width;

                    //if (miny < 0)
                    //    miny = 0;

                    //if (maxy > ClientRectangle.Height)
                    //    maxy = ClientRectangle.Height;

                    //for (int xx = minx; xx < maxx; xx++)
                    //{
                    //    for(int yy=miny; yy<maxy; yy++)
                    //    {
                    //        if (Temp.GetPixel(xx, yy) == tempColor)
                    //        {
                    //            m_pMask[xx][yy] = world.Map.Regions[x][y];
                    //        }
                    //    }
                    //}

        //			if(x == m_pFractal->WIDTH-1)
			        if(border)
			        {
				        for(i=0; i<3; i++)
				        {
					        points[i] = new Point(points[i].X - ClientRectangle.Width,points[i].Y);
				        }
			        }
		        }
	        }

            //if(courceX1 > 0 && courceX2 > 0 && courceY1 > 0 && courceY2 > 0)
            //{
            //    TWay *way;
            //    way = m_pFractal->GetCource(courceX1, courceY1, courceX2, courceY2);
            //    for(int i=0; i<way->iLength; i++)
            //    {
            //        KColor col;
            //        col.SetRGBColor(clRed);
            //        col.ToHLS();
            //        col.lightness = 0.5 + 0.5*i/way->iLength;
            //        col.ToRGB();
            //        DrawRegion(way->pWay[i].x, way->pWay[i].y, col.GetRGBColor());
            //    }
            //    delete[] way->pWay;
            //    delete way;
            //}

            //Brush whiteBrush = Brushes.White;
            //gr.FillRectangle(whiteBrush, 0, 0, ClientRectangle.Width, iStepY);
            //gr.FillRectangle(whiteBrush, 0, ClientRectangle.Height - iStepY, ClientRectangle.Width, ClientRectangle.Height);

            Refresh();
        }
        */
        private void FractalMapView_Resize(object sender, EventArgs e)
        {
            m_pMask = new CFRegion[ClientRectangle.Width][];
            for (int i = 0; i < ClientRectangle.Width; i++)
                m_pMask[i] = new CFRegion[ClientRectangle.Height];

            ActualMap = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            
            DrawMap();
        }

        private void FractalMapView_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(ActualMap, 0, 0);
        }

        private bool PointsNorth(int iSector, int iParallel)
        {
	        // если треугольник ориентирован вершиной вверх (т.е. к северу)
	        if(Math.Abs(world.Atlas[iParallel].Regions[iSector].Latitude - world.Atlas[iParallel].MinLatitude) <
               Math.Abs(world.Atlas[iParallel].Regions[iSector].Latitude - world.Atlas[iParallel].MaxLatitude))
	        {
		        return true;
	        }
	        else
	        {
		        return false;
	        }
        }

        /// <summary>
        /// возвращает координаты вершин сектора для отрисовки
        /// </summary>
        /// <param name="iSector">номер сектора в параллели</param>
        /// <param name="iParallel">номер параллели в атласе</param>
        /// <returns>массив из 3х точек</returns>
        private Point[] GetSectorTriangleArrakktur(int iSector, int iParallel)
        {
	        Point[] points = new Point[3];
	        int i;
	        for(i=0; i<3; i++)
	        {
		        points[i] = new Point(0,0);
	        }

            if (world == null)
                return points;

            if (world.Map == null || world.Atlas == null)
                return points;

            if (world.Map.Width == 0 ||world.Map.Height == 0)
                return points;

	        double fStepX, fStepY;
            fStepX = (double)ClientRectangle.Width / world.Map.Width;
            fStepY = (double)ClientRectangle.Height / world.Map.Height;

	        double fLong0, fLong1, fLong2;	// по часовой стрелке, начиная с указующей вершины
            if (world.Atlas[iParallel].Length < world.Map.Width)
	        {
		        // Это заполярье
                if (world.Atlas[iParallel].Regions[iSector].Latitude > 0)
		        {
			        // северное
			        int iNVertexCount = iParallel*5;		// число северных вершин
			        int iSVertexCount = (iParallel+1)*5;    // число южных вершин

			        if(iNVertexCount == 0)
				        iNVertexCount = 5;

			        int iLeafWidth = iParallel+1;	// число североориентированных секторов в лепестке
                    int iNumInLeaf = iSector % (world.Atlas[iParallel].Length / 5);	// номер сектора в лепестке
                    int iLeafNum = iSector / (world.Atlas[iParallel].Length / 5);	// номер лепестка

			        if(PointsNorth(iSector, iParallel))
			        {
                        fLong0 = (iLeafNum * (iLeafWidth - 1) + iNumInLeaf / 2) * 2 * Math.PI / iNVertexCount;
                        fLong1 = (iLeafNum * iLeafWidth + iNumInLeaf / 2 + 1) * 2 * Math.PI / iSVertexCount;
                        fLong2 = (iLeafNum * iLeafWidth + iNumInLeaf / 2) * 2 * Math.PI / iSVertexCount;
			        }
			        else
			        {
                        fLong0 = (iLeafNum * iLeafWidth + (iNumInLeaf + 1) / 2) * 2 * Math.PI / iSVertexCount;
                        fLong1 = (iLeafNum * (iLeafWidth - 1) + iNumInLeaf / 2) * 2 * Math.PI / iNVertexCount;
                        fLong2 = (iLeafNum * (iLeafWidth - 1) + iNumInLeaf / 2 + 1) * 2 * Math.PI / iNVertexCount;
			        }
		        }
		        else
		        {
			        // южное
			        int iNVertexCount = (world.Map.Height - iParallel) * 5;		// число северных вершин
			        int iSVertexCount = (world.Map.Height - iParallel - 1) * 5;    // число южных вершин

			        if(iSVertexCount == 0)
				        iSVertexCount = 5;

                    int iLeafWidth = world.Map.Height - iParallel;	// число южноориентированных секторов в лепестке
                    int iNumInLeaf = (iSector + world.Atlas[iParallel].Length / 10) % (world.Atlas[iParallel].Length / 5);	// номер сектора в лепестке
                    int iLeafNum = (iSector + world.Atlas[iParallel].Length / 10) / (world.Atlas[iParallel].Length / 5);	// номер лепестка

			        if(!PointsNorth(iSector, iParallel))
			        {
                        fLong0 = (iLeafNum * (iLeafWidth - 1) - iLeafWidth / 2 + iNumInLeaf / 2) * 2 * Math.PI / iSVertexCount;
                        fLong1 = (iLeafNum * iLeafWidth - iLeafWidth / 2 + iNumInLeaf / 2 + 1) * 2 * Math.PI / iNVertexCount;
                        fLong2 = (iLeafNum * iLeafWidth - iLeafWidth / 2 + iNumInLeaf / 2) * 2 * Math.PI / iNVertexCount;
			        }
			        else
			        {
                        fLong0 = (iLeafNum * iLeafWidth - iLeafWidth / 2 + (iNumInLeaf + 1) / 2) * 2 * Math.PI / iNVertexCount;
                        fLong1 = (iLeafNum * (iLeafWidth - 1) - iLeafWidth / 2 + iNumInLeaf / 2) * 2 * Math.PI / iSVertexCount;
                        fLong2 = (iLeafNum * (iLeafWidth - 1) - iLeafWidth / 2 + iNumInLeaf / 2 + 1) * 2 * Math.PI / iSVertexCount;
			        }
			        if(PointsNorth(0, iParallel))
			        {
				        if(!PointsNorth(iSector, iParallel))
				        {
                            fLong0 = fLong0 + Math.PI / iSVertexCount;
				        }
				        else
				        {
                            fLong1 = fLong1 + Math.PI / iSVertexCount;
                            fLong2 = fLong2 + Math.PI / iSVertexCount;
				        }
			        }
			        else
			        {
				        if(PointsNorth(iSector, iParallel))
				        {
                            fLong0 = fLong0 - Math.PI / iNVertexCount;
				        }
				        else
				        {
                            fLong1 = fLong1 - Math.PI / iNVertexCount;
                            fLong2 = fLong2 - Math.PI / iNVertexCount;
				        }
			        }
		        }
	        }
	        else
	        {
		        // экваториальный пояс
                int iVertexCount = world.Atlas[iParallel].Length / 2;		// число вершин

		        int iLeafWidth = iVertexCount/5;	// число вершин в лепестке
                int iNumInLeaf = iSector % (world.Atlas[iParallel].Length / 5);	// номер сектора в лепестке
                int iLeafNum = iSector / (world.Atlas[iParallel].Length / 5);	// номер лепестка (считая с 0)

		        if(PointsNorth(iSector, iParallel))
		        {
                    fLong0 = (iLeafNum * iLeafWidth + iNumInLeaf / 2) * 2 * Math.PI / iVertexCount;
                    fLong1 = (iLeafNum * iLeafWidth + iNumInLeaf / 2 + 1) * 2 * Math.PI / iVertexCount;
                    fLong2 = (iLeafNum * iLeafWidth + iNumInLeaf / 2) * 2 * Math.PI / iVertexCount;
		        }
		        else
		        {
                    fLong0 = (iLeafNum * iLeafWidth + (iNumInLeaf + 1) / 2) * 2 * Math.PI / iVertexCount;
                    fLong1 = (iLeafNum * iLeafWidth + iNumInLeaf / 2) * 2 * Math.PI / iVertexCount;
                    fLong2 = (iLeafNum * iLeafWidth + iNumInLeaf / 2 + 1) * 2 * Math.PI / iVertexCount;
		        }

		        if(PointsNorth(0, iParallel))
		        {
			        if(PointsNorth(iSector, iParallel))
			        {
                        fLong1 = fLong1 - 2 * Math.PI / world.Map.Width;
                        fLong2 = fLong2 - 2 * Math.PI / world.Map.Width;
			        }
			        else
			        {
                        fLong0 = fLong0 - 2 * Math.PI / world.Map.Width;
			        }
		        }
		        else
		        {
			        if(PointsNorth(iSector, iParallel))
			        {
                        fLong0 = fLong0 + 2 * Math.PI / world.Map.Width;
			        }
			        else
			        {
                        fLong1 = fLong1 - 2 * Math.PI / world.Map.Width;
                        fLong2 = fLong2 - 2 * Math.PI / world.Map.Width;
			        }
		        }
	        }

	        double fLat0, fLat12;
	        // если треугольник ориентирован вершиной вверх (т.е. к северу)
	        if(PointsNorth(iSector, iParallel))
	        {
		        fLat0 = world.Atlas[iParallel].MaxLatitude;
                fLat12 = world.Atlas[iParallel].MinLatitude;
	        }
	        else
	        {
                fLat0 = world.Atlas[iParallel].MinLatitude;
                fLat12 = world.Atlas[iParallel].MaxLatitude;
	        }

            points[0].X = (int)(fLong0 * ClientRectangle.Width / (2 * Math.PI));
            points[0].Y = (int)((Math.PI / 2 - fLat0) * ClientRectangle.Height / Math.PI);
	        if(iParallel == 0)
		        points[0].Y = -64000;
            if (iParallel == world.Map.Height - 1)
		        points[0].Y = 64000;

            points[1].X = (int)(fLong1 * ClientRectangle.Width / (2 * Math.PI));
            points[1].Y = (int)((Math.PI / 2 - fLat12) * ClientRectangle.Height / Math.PI);

            points[2].X = (int)(fLong2 * ClientRectangle.Width / (2 * Math.PI));
            points[2].Y = (int)((Math.PI / 2 - fLat12) * ClientRectangle.Height / Math.PI);

        //  Формулы перевода из экранных в угловые:
        //	fLong = 2.0*M_PI*X/Image1->Width;
        //	fLat = M_PI/2 - M_PI*Y/Image1->Height;

	        return points;
        }


        /// <summary>
        /// отрисовывает регион с заданными координатами вершин, если надо - обводит яркой рамкой
        /// </summary>
        /// <param name="gr">куда рисовать регион</param>
        /// <param name="points">массив с координатами вершин треугольника</param>
        /// <param name="pSector">сам регион (нужен для вычисления цвета и т.п.)</param>
        /// <param name="bBorder">нужна ли подсветка</param>
        private void DrawSector(Graphics gr, Point[] points, CFRegion pSector, bool bBorder)
        {
	        if(pSector == null)
		        return;

	        // три вершины сектора-треугольника
	        int border = 0;
	        int i;

	        for(i=0; i<3; i++)
	        {
		        if(points[i].X > ClientRectangle.Width)
		        {
			        border = 1;// регион вылезает вправо
		        }
		        if(points[i].X < 0)
		        {
			        border = 2;// регион вылезает влево
		        }
	        }// все три вершины рассчитаны

            Brush paint = Brushes.Black;
            if (m_eMode == DisplayMode.Height)
            {
                int h;
                h = pSector.Height / 100;
                if (h < 0)
                {
                    h = 0;
                }
                if (h >= 256)
                {
                    h = 255;
                }

                h = 20 * (h / 20);

                if (pSector.Height >= world.Map.OceanLevel)
                {
                    paint = new SolidBrush(Color.FromArgb(h, h, 0));
                }
                else
                {
                    paint = new SolidBrush(Color.FromArgb(0, 0, h));
                }

                if (pSector.ContinentID == -255)
                {
                    paint = Brushes.Blue;
                }

                if (!pSector.Allowed)
                {
                    paint = Brushes.Purple;
                }
            }
            if (m_eMode == DisplayMode.ID)
            {
                if (pSector.ContinentID >= 0 && pSector.Height >= world.Map.OceanLevel)
                    paint = new SolidBrush(m_aColorsID[pSector.ContinentID]);
            }
            
            if (bBorder)
                paint = new SolidBrush(Color.White);

            //Pen borderPen = new Pen(Color.White, 1);

            gr.FillPolygon(paint, points);
            //if (bBorder)
            //    gr.DrawPolygon(borderPen, points);


	        if(border == 1)
	        {
		        for(i=0; i<3; i++)
		        {
			        points[i].X = points[i].X - ClientRectangle.Width;
		        }
                gr.FillPolygon(paint, points);
                //if (bBorder)
                //    gr.DrawPolygon(borderPen, points);
            }
	        if(border == 2)
	        {
		        for(i=0; i<3; i++)
		        {
                    points[i].X = points[i].X + ClientRectangle.Width;
		        }
                gr.FillPolygon(paint, points);
                //if (bBorder)
                //    gr.DrawPolygon(borderPen, points);
            }

        /*
	        int iXX, iYY;
	        iXX = (float)Image1->Width*pSector->m_fLong/(2.0*M_PI);
	        iYY = (float)Image1->Height*(M_PI*0.5 - pSector->m_fLat)/M_PI;

	        Image1->Canvas->Pen->Color = clGreen;
	        Image1->Canvas->Ellipse(iXX-5, iYY-5, iXX+5, iYY+5);
        */
        }

        /// <summary>
        /// просто перерисовывает всю карту
        /// </summary>
         private void DrawMap()
        {
            Graphics gr = Graphics.FromImage(ActualMap);

            Brush fill = Brushes.Black;
            gr.FillRectangle(fill, 0, 0, ClientRectangle.Width, ClientRectangle.Height);

            if (world == null)
                return;

            if (world.Map == null || world.Atlas == null)
                return;

            if (world.Map.Width == 0)
                return;

            m_aColorsID = new Color[world.Map.LandMass.Count];
            for (int i = 0; i < world.Map.LandMass.Count; i++)
            {
                m_aColorsID[i] = Color.FromArgb(128 + Rnd.Get(128), 128 + Rnd.Get(128), 128 + Rnd.Get(128));
            }

            for (int iParallel = 0; iParallel < world.Map.Height; iParallel++)
            {
                for (int iSector = 0; iSector < world.Atlas[iParallel].Length; iSector++)
                {
			        Point[] points = GetSectorTriangleArrakktur(iSector, iParallel);

                    DrawSector(gr, points, world.Atlas[iParallel].Regions[iSector], false);
		        }
	        }

            Refresh();
        }
        
        CFRegion GetSector(double fLong, double fLat)
        {
	        CFRegion pResult = null;

	        double minDist = 32000;

	        // перебираем все параллели
	        for(int y=0; y<world.Map.Height; y++)
	        {
		        // если укладываемся в диапазон широт
		        if(world.Atlas[y].MinLatitude < fLat && world.Atlas[y].MaxLatitude >= fLat)
		        {
			        // считаем примерный номер сектора
                    int x = (int)(world.Atlas[y].Length * fLong / (2 * Math.PI));
			        int minx = x-1;
			        int maxx = x+1;
			        // на всякий случай захватываем на 1 сектор левее и правее
			        for(int j=minx; j<=maxx; j++)
			        {
				        int i = j;
				        if(i<0)
				        {
                            i += world.Atlas[y].Length;
				        }
                        if (i >= world.Atlas[y].Length)
				        {
                            i -= world.Atlas[y].Length;
				        }

                        SimpleVector3d p1 = new SimpleVector3d();
                        SimpleVector3d p2 = new SimpleVector3d();

				        // переводим угловые координаты в декартовы трёхмерные
                        p1.X = Math.Cos(world.Atlas[y].Regions[i].Longitude);
                        p1.X = p1.X * Math.Cos(world.Atlas[y].Regions[i].Latitude);

                        p1.Y = Math.Sin(world.Atlas[y].Regions[i].Longitude);
                        p1.Y = p1.Y * Math.Cos(world.Atlas[y].Regions[i].Latitude);

                        p1.Z = Math.Sin(Math.Abs(world.Atlas[y].Regions[i].Latitude));
                        if (world.Atlas[y].Regions[i].Latitude < 0)
					        p1.Z = -p1.Z;

                        p2.X = Math.Cos(fLong);
                        p2.X = p2.X * Math.Cos(fLat);
                        p2.Y = Math.Sin(fLong);
                        p2.Y = p2.Y * Math.Cos(fLat);

                        p2.Z = Math.Sin(Math.Abs(fLat));
				        if(fLat < 0)
					        p2.Z = -p2.Z;

				        // расстояние между полученными точками
				        double dist = !(p2-p1);

				        // ищем наименьшее расстояние
				        if(dist < minDist)
				        {
					        minDist = dist;
                            pResult = world.Atlas[y].Regions[i];
				        }
			        }
		        }
	        }

	        return pResult;
        }

        // перерисовывает отдельно взятый регион, если надо - с яркой рамкой
        private void ShowSector(double fLong, double fLat, bool bLight)
        {
	        CFRegion pSector = GetSector(fLong, fLat);

	        if(pSector == null)
		        return;

            Graphics gr = Graphics.FromImage(ActualMap);
            Point[] points = GetSectorTriangleArrakktur(pSector.Sector, pSector.Parallel);

	        DrawSector(gr, points, pSector, bLight);
        }

        private double m_fLongFocus = -1;
        private double m_fLatFocus = -1;

        private CFRegion m_pSelectedRegion = null;

        public CFRegion SelectedRegion
        {
            get { return m_pSelectedRegion; }
        }

        private void FractalMapView2D_MouseMove(object sender, MouseEventArgs e)
        {
            if (world == null)
                return;

            if (world.Map == null || world.Atlas == null)
                return;

            if (world.Map.Width == 0)
                return;

            ShowSector(m_fLongFocus, m_fLatFocus, false);

            m_fLongFocus = 2 * Math.PI * e.X / ClientRectangle.Width;
            m_fLatFocus = Math.PI / 2 - Math.PI * e.Y / ClientRectangle.Height;

            m_pSelectedRegion = GetSector(m_fLongFocus, m_fLatFocus);

            if (m_pSelectedRegion != null)
            {
                SelectRegion(m_pSelectedRegion);
                ShowSector(m_fLongFocus, m_fLatFocus, true);
                Refresh();
            }

            /*
            //	RegionLatitude->Caption = GetLatAStr(fLatFocus).Str();
            //	RegionLongitude->Caption = GetLongAStr(fLongFocus).Str();
            SectorCode->Caption = GetSectorCode(pSector->m_iParallel, pSector->m_iSector).Str();
            SectorLatitude->Caption = GetLatAStr(pSector->m_fLat, true).Str();
            SectorLongitude->Caption = GetLongAStr(pSector->m_fLong, true).Str();
            coord1->Caption = (GetLatAStr(m_fLatFocus, true) + " " + GetLongAStr(m_fLongFocus, true)).Str();
            if (pSector->m_iDepth >= 0)
            {
                SectorDepth->Caption = IntToStr(pSector->m_iDepth);
                if (pSector->m_iSettlementPop > 0)
                {
                    SectorSettlementName->Caption = pSector->m_sSettlementName.Str();
                    SectorSettlementPop->Caption = IntToStr(pSector->m_iSettlementPop);
                }
                else
                {
                    SectorSettlementName->Caption = "-";
                    SectorSettlementPop->Caption = "-";
                }
            }
            else
            {
                SectorDepth->Caption = "- ? -";
                SectorSettlementName->Caption = "- ? -";
                SectorSettlementPop->Caption = "- ? -";
            }
            */
        }

        public class RegionSelectedEventArgs : EventArgs
        {
            private CFRegion m_pRegion;
            public CFRegion Region
            {
                get { return m_pRegion; }
            }

            public RegionSelectedEventArgs(CFRegion pRegion)
            {
                m_pRegion = pRegion;
            }
        }

        public event EventHandler<RegionSelectedEventArgs> RegionSelectedEvent;

        private void SelectRegion(CFRegion pRegion)
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<RegionSelectedEventArgs> temp = RegionSelectedEvent;
            if (temp != null)
                temp(this, new RegionSelectedEventArgs(pRegion));
        }
    }
}
