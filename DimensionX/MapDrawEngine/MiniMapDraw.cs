using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Socium;
using System.Drawing.Drawing2D;
using LandscapeGeneration;

namespace MapDrawEngine
{
    public partial class MiniMapDraw : UserControl
    {
        /// <summary>
        /// Отображаемый участок карты.
        /// Координаты - экранные.
        /// </summary>
        private Rectangle m_pDrawFrame;

        /// <summary>
        /// Отображаемый участок карты.
        /// Координаты - экранные.
        /// </summary>
        public Rectangle DrawFrame
        {
            get { return m_pDrawFrame; }
        }

        /// <summary>
        /// коофициент для перевода координат из абсолютной системы координат в экранную
        /// </summary>
        private float m_fActualScale = 1;

        private float m_fFrameWidth = 0f;

        /// <summary>
        /// коофициент для перевода координат из абсолютной системы координат в экранную
        /// </summary>
        public float ActualScale
        {
            get { return m_fActualScale; }
        }

        /// <summary>
        /// смещение отображения карты для центрирования по горизонтали - если ширина карты меньше ширины рабочей поверхности
        /// </summary>
        private int m_iShiftX = 0;
        /// <summary>
        /// смещение отображения карты для центрирования по вертикали - если высота карты меньше высоты рабочей поверхности
        /// </summary>
        private int m_iShiftY = 0;

        /// <summary>
        /// связанная основная карта
        /// </summary>
        private MapDraw m_pMasterMap = null;

        public MiniMapDraw()
        {
            InitializeComponent();
        }

        internal void SinchronizeDrawFrame()
        {
            if (m_pMasterMap == null)
                return;

            m_pDrawFrame.X = (int)(m_fActualScale * m_pMasterMap.DrawFrame.X / m_pMasterMap.ActualScale) - 1;
            m_pDrawFrame.Y = (int)(m_fActualScale * m_pMasterMap.DrawFrame.Y / m_pMasterMap.ActualScale) - 1;
            m_pDrawFrame.Width = (int)(m_fActualScale * m_pMasterMap.DrawFrame.Width / m_pMasterMap.ActualScale);
            m_pDrawFrame.Height = (int)(m_fActualScale * m_pMasterMap.DrawFrame.Height / m_pMasterMap.ActualScale) + 1;

            if (m_pDrawFrame.X < (int)m_fFrameWidth)
                m_pDrawFrame.X = (int)m_fFrameWidth;

            if (m_pDrawFrame.X > m_iScaledMapWidth - m_pDrawFrame.Width + (int)m_fFrameWidth)
                m_pDrawFrame.X = m_iScaledMapWidth - m_pDrawFrame.Width + (int)m_fFrameWidth;

            if (m_pDrawFrame.Y < (int)m_fFrameWidth)
                m_pDrawFrame.Y = (int)m_fFrameWidth;

            if (m_pDrawFrame.Y > m_iScaledMapHeight - m_pDrawFrame.Height + (int)m_fFrameWidth)
                m_pDrawFrame.Y = m_iScaledMapHeight - m_pDrawFrame.Height + (int)m_fFrameWidth;

            Refresh();
        }

        internal GraphicsPath m_pContinents = new GraphicsPath();
        internal Dictionary<Brush, GraphicsPath> m_cAreas = new Dictionary<Brush, GraphicsPath>();

        /// <summary>
        /// Привязать миникарту к основной карте.
        /// Строим контуры всего, что придётся рисовать в ОРИГИНАЛЬНЫХ координатах.
        /// </summary>
        /// <param name="pMasterMap">основная карта</param>
        internal void Assign(MapDraw pMasterMap)
        {
            m_pMasterMap = pMasterMap;
        }

        internal void WorldAssigned()
        {
            if (m_pMasterMap == null || m_pMasterMap.m_pWorld == null)
                return;

            m_pContinents.Reset();
            m_cAreas.Clear();

            PointF[][] aPoints;
            //вычислим контуры континентов
            foreach (Continent pContinent in m_pMasterMap.m_pWorld.Contents)
            {
                aPoints = BuildPath(pContinent.FirstLines);
                foreach (var aPts in aPoints)
                    m_pContinents.AddPolygon(aPts);

                //вычислим контуры географических регионов
                foreach (Socium.Region pArea in pContinent.As<ContinentX>().Regions)
                {
                    aPoints = BuildPath(pArea.FirstLines);
                    foreach (var aPts in aPoints)
                    {
                        //в качестве идентификатора типа региона используем цвет, которым этот регион должен рисоваться
                        if (!m_cAreas.ContainsKey(pArea.Type.Get<LandTypeDrawInfo>().m_pBrush))
                            m_cAreas[pArea.Type.Get<LandTypeDrawInfo>().m_pBrush] = new GraphicsPath();
                        m_cAreas[pArea.Type.Get<LandTypeDrawInfo>().m_pBrush].AddPolygon(aPts);
                    }
                }
            }

            CreateCanvas();
            Draw();

            SinchronizeDrawFrame();
        }

        /// <summary>
        /// Строит сложный контур в ОРИГИНАЛЬНЫХ координатах (с отражениями)
        /// Сложный контур - это когда например, у континента есть две замкнутые береговые линии -
        /// одна с внешним океаном, а другая с внутренним морем. Каждая из них - простой контур,
        /// а в совокупности - сложный.
        /// </summary>
        /// <param name="cFirstLines">Затравки контуров</param>
        /// <returns></returns>
        private PointF[][] BuildPath(List<VoronoiEdge> cFirstLines)
        {
            List<PointF[]> cPath = new List<PointF[]>();

            //пробежимся по всем затравкам
            foreach (VoronoiEdge pFirstLine in cFirstLines)
            {
                //получаем простой одиночный контур
                cPath.Add(BuildBorder(pFirstLine, 0, out bool bCross));

                //если карта зациклена по горизонтали, нужно строить отражения и 
                //контур пересекает нулевой меридиан, то строим отражение!
                if (m_pMasterMap.m_pWorld.LocationsGrid.CycleShift != 0 && bCross)
                {
                    //определяем, на западе или на востоке будем строить отражение
                    if (pFirstLine.Point1.X > 0)
                        cPath.Add(BuildBorder(pFirstLine, -m_pMasterMap.m_pWorld.LocationsGrid.RX * 2, out _));
                    else
                        cPath.Add(BuildBorder(pFirstLine, m_pMasterMap.m_pWorld.LocationsGrid.RX * 2, out _));
                }
            }

            return cPath.ToArray();
        }

        /// <summary>
        /// строит простой контур в ОРИГИНАЛЬНЫХ координатах (без отражений)
        /// </summary>
        /// <param name="pFirstLine">затравка контура</param>
        /// <param name="fShift">сдвиг по горизонтали для закольцованной карты</param>
        /// <param name="bCross">признак того, что контур пересекает нулевой меридиан</param>
        /// <returns></returns>
        private PointF[] BuildBorder(VoronoiEdge pFirstLine, float fShift, out bool bCross)
        {
            bCross = false;

            List<PointF> cBorder = new List<PointF>();
            VoronoiEdge pLine = pFirstLine;
            cBorder.Add(ShiftPoint(pLine.Point1, fShift));
            float fLastPointX = pLine.Point1.X + fShift;
            //последовательно перебирает все связанные линии, пока круг не замкнётся.
            do
            {
                //пересекает-ли линия нулевой меридиан?
                float fDX = fShift;
                if (Math.Abs(fLastPointX - pLine.Point2.X - fShift) > m_pMasterMap.m_pWorld.LocationsGrid.RX)
                {
                    //определимся, где у нас была предыдущая часть контура - на западе или на востоке?
                    //в зависимости от этого вычислим смещение для оставшейся части контура, чтобы 
                    //не было разрыва
                    fDX += fLastPointX < fShift ? -m_pMasterMap.m_pWorld.LocationsGrid.RX * 2 : m_pMasterMap.m_pWorld.LocationsGrid.RX * 2;
                    bCross = true;
                }

                if (pLine.Point2.X > m_pMasterMap.m_pWorld.LocationsGrid.RX ||
                    pLine.Point2.X < -m_pMasterMap.m_pWorld.LocationsGrid.RX)
                {
                    bCross = true;
                }

                cBorder.Add(ShiftPoint(pLine.Point2, fDX));

                //X-координата последней добавленной точки с учётом вычисленного смещения
                fLastPointX = pLine.Point2.X + fDX;

                pLine = pLine.Next;
            }
            while (pLine != pFirstLine);

            return cBorder.ToArray();
        }

        /// <summary>
        /// Смещает начало координат из пересечения экватора и нулевого меридиана в верхний левый угол карты, плюс заданное смещение по горизонтали.
        /// </summary>
        /// <param name="pPoint">исходная точка</param>
        /// <param name="fDX">заданное смещение</param>
        /// <returns>смещённая точка</returns>
        private PointF ShiftPoint(VoronoiVertex pPoint, float fDX)
        {
            return new PointF(m_pMasterMap.m_pWorld.LocationsGrid.RX + pPoint.X + fDX, m_pMasterMap.m_pWorld.LocationsGrid.RY + pPoint.Y);
        }

        /// <summary>
        /// холст, на котором мы будем рисовать
        /// </summary>
        private Bitmap m_pCanvas = null;

        /// <summary>
        /// ширина всей карты мира в экранных координатах
        /// </summary>
        private int m_iScaledMapWidth;
        /// <summary>
        /// высота всей карты мира в экранных координатах
        /// </summary>
        private int m_iScaledMapHeight;

        /// <summary>
        /// создаёт холст для рисования видимого участка карты в текущем масштабе.
        /// размеры холста зависят от размеров окна рисования и выбранного масштаба карты (холст может быть меньше окна рисования).
        /// так же здесь выполняется пересчёт всех мастабных коэффициентов
        /// </summary>
        private void CreateCanvas()
        {
            //если компонент не готов - все в сад
            if (ClientRectangle.Width == 0)
                return;

            //соотношение высоты и ширины координатной сетки мира
            float fK = 1;
            if (m_pMasterMap?.m_pWorld != null)
                fK = (float)m_pMasterMap.m_pWorld.LocationsGrid.RY / m_pMasterMap.m_pWorld.LocationsGrid.RX;

            //ширина и высота карты мира в экранных координатах
            //из расчёта того, чтобы при единичном масштабе вся карта имела такую же ширину, как окно рисования
            m_iScaledMapWidth = ClientRectangle.Width;
            m_iScaledMapHeight = (int)(m_iScaledMapWidth * fK);

            //если при этом карта вылезает за пределы области отображения по вертикали, то вычислим ширину и высоту
            //из расчёта того, чтобы при единичном масштабе вся карта влезла в область отображения по высоте
            if (m_iScaledMapHeight > ClientRectangle.Height)
            {
                m_iScaledMapHeight = ClientRectangle.Height;
                m_iScaledMapWidth = (int)(m_iScaledMapHeight / fK);
            }

            //создадим холст такого же размера, как и окно рисования, но не больше 
            //отмасштабированной карты
            m_pCanvas = new Bitmap(Math.Min(ClientRectangle.Width, m_iScaledMapWidth), Math.Min(ClientRectangle.Height, m_iScaledMapHeight));

            //коэффициент для перевода координат из абсолютной системы координат в экранную
            if (m_pMasterMap.m_pWorld != null)
                m_fActualScale = (float)(m_iScaledMapWidth) / (m_pMasterMap.m_pWorld.LocationsGrid.RX * 2 - m_pMasterMap.m_pWorld.LocationsGrid.FrameWidth * 2);

            m_fFrameWidth = (float)m_pMasterMap.m_pWorld.LocationsGrid.FrameWidth * m_fActualScale;

            //если холст уже окна рисования, вычислим смещение для центрирования холста
            m_iShiftX = (ClientRectangle.Width - m_pCanvas.Width) / 2;

            //если холст меньше окна рисования по вертикали, вычислим смещение для центрирования холста
            m_iShiftY = (ClientRectangle.Height - m_pCanvas.Height) / 2;
        }

        /// <summary>
        /// Нарисовать видимый участок карты на холсте
        /// </summary>
        public void Draw()
        {
            //если хоста нет - делать нечего
            if (m_pCanvas == null)
                return;

            DateTime pTime1 = DateTime.Now;

            Graphics gr = Graphics.FromImage(m_pCanvas);

            //грунтуем холст цветом моря
            gr.FillRectangle(new SolidBrush(LandTypes.Ocean.Get<LandTypeDrawInfo>().m_pColor), 0, 0, m_pCanvas.Width, m_pCanvas.Height);

            //если нет мира или мир вырожденный - больше рисовать нечего
            if (m_pMasterMap == null || m_pMasterMap.m_pWorld == null || m_pMasterMap.m_pWorld.LocationsGrid.Locations.Length == 0)
                return;

            //рисуем контуры континентов
            Matrix pMatrix2 = new Matrix();
            pMatrix2.Scale(m_fActualScale, m_fActualScale);
            pMatrix2.Translate(2, 2);

            GraphicsPath pPath = (GraphicsPath)m_pContinents.Clone();
            pPath.Transform(pMatrix2);
            gr.DrawPath(MapDraw.s_pBlack2Pen, pPath);

            Matrix pMatrix = new Matrix();
            pMatrix.Scale(m_fActualScale, m_fActualScale);

            //закрашиваем карту в соответствии с типами географических регионов
            foreach (var pArea in m_cAreas)
            {
                pPath = (GraphicsPath)pArea.Value.Clone();
                pPath.Transform(pMatrix);

                gr.FillPath(pArea.Key, pPath);
            }

            Refresh();

            DateTime pTime2 = DateTime.Now;
        }

        private void MiniMapDraw_Paint(object sender, PaintEventArgs e)
        {
            if (m_pCanvas == null || m_pMasterMap == null)
                return;

            e.Graphics.DrawImage(m_pCanvas, m_iShiftX, m_iShiftY);
            e.Graphics.DrawRectangle(Pens.Red, new Rectangle(m_pDrawFrame.X + m_iShiftX, m_pDrawFrame.Y + m_iShiftY, m_pDrawFrame.Width, m_pDrawFrame.Height));

            if (m_pDrawFrame.X < 0)
                e.Graphics.DrawRectangle(Pens.Red, new Rectangle(m_pDrawFrame.X + m_iShiftX + m_iScaledMapWidth, m_pDrawFrame.Y + m_iShiftY, m_pDrawFrame.Width, m_pDrawFrame.Height));

            if (m_pDrawFrame.X + m_pDrawFrame.Width > m_iScaledMapWidth)
                e.Graphics.DrawRectangle(Pens.Red, new Rectangle(m_pDrawFrame.X + m_iShiftX - m_iScaledMapWidth, m_pDrawFrame.Y + m_iShiftY, m_pDrawFrame.Width, m_pDrawFrame.Height));

            if (m_iShiftX > 0)
            {
                e.Graphics.FillRectangle(new SolidBrush(BackColor), 0, 0, m_iShiftX, ClientRectangle.Height);
                e.Graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle.Width - m_iShiftX, 0, m_iShiftX, ClientRectangle.Height);
            }
            if (m_iShiftY > 0)
            {
                e.Graphics.FillRectangle(new SolidBrush(BackColor), 0, 0, ClientRectangle.Width, m_iShiftY);
                e.Graphics.FillRectangle(new SolidBrush(BackColor), 0, ClientRectangle.Height - m_iShiftY, ClientRectangle.Width, m_iShiftY);
            }
        }

        private void MiniMapDraw_Resize(object sender, EventArgs e)
        {
            if (ClientRectangle.Width == 0 || ClientRectangle.Height == 0 || m_pCanvas == null)
                return;

            //строим холст и вычисляем все связанные с масштабированием параметры
            //холст перестраиваем при каждом масштабировании, т.к. его размеры - это
            //меньшие из размеров карты в текущем масштабе и размеров области рисования
            CreateCanvas();
        }

        private bool m_bScrolling = false;

        private Point m_pLastMouseLocation = new Point(0, 0);

        private void MiniMapDraw_MouseDown(object sender, MouseEventArgs e)
        {
            m_bScrolling = true;
            m_pLastMouseLocation = new Point(-1, -1);
        }

        private void MiniMapDraw_MouseLeave(object sender, EventArgs e)
        {
            m_bScrolling = false;
        }

        private void MiniMapDraw_MouseUp(object sender, MouseEventArgs e)
        {
            m_bScrolling = false;
        }

        private void MiniMapDraw_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_pMasterMap == null || !m_bScrolling)
                return;

            Point p = m_pDrawFrame.Location;

            if (m_pLastMouseLocation.X > 0)
            {
                p.X += e.X - m_pLastMouseLocation.X;
                p.Y += e.Y - m_pLastMouseLocation.Y;
            }
            m_pLastMouseLocation = e.Location;

            SetPan(p.X, p.Y);
        }

        /// <summary>
        /// сдвигает область просмотра в указанные координаты и перерисовывает карту
        /// </summary>
        /// <param name="iX">X-координата левого верхнего угла области просмотра</param>
        /// <param name="iY">Y-координата левого верхнего угла области просмотра</param>
        public void SetPan(int iX, int iY)
        {
            if (m_pMasterMap == null || m_pMasterMap.m_pWorld == null)
                return;

            if (m_pMasterMap.m_pWorld.LocationsGrid.CycleShift != 0)
                m_pDrawFrame.X = iX;
            else
                m_pDrawFrame.X = Math.Max(0, Math.Min(iX, m_iScaledMapWidth - m_pDrawFrame.Width + (int)m_fFrameWidth));

            while (m_pDrawFrame.X < 0)
                m_pDrawFrame.X += m_iScaledMapWidth;

            while (m_pDrawFrame.X > m_iScaledMapWidth)
                m_pDrawFrame.X -= m_iScaledMapWidth;

            m_pDrawFrame.Y = Math.Max(0, Math.Min(iY, m_iScaledMapHeight - m_pDrawFrame.Height + (int)m_fFrameWidth));

            if (m_pDrawFrame.X < (int)m_fFrameWidth)
                m_pDrawFrame.X = (int)m_fFrameWidth;

            if (m_pDrawFrame.X > m_iScaledMapWidth - m_pDrawFrame.Width - (int)m_fFrameWidth)
                m_pDrawFrame.X = m_iScaledMapWidth - m_pDrawFrame.Width - (int)m_fFrameWidth;

            if (m_pDrawFrame.Y < (int)m_fFrameWidth)
                m_pDrawFrame.Y = (int)m_fFrameWidth;

            if (m_pDrawFrame.Y > m_iScaledMapHeight - m_pDrawFrame.Height - (int)m_fFrameWidth)
                m_pDrawFrame.Y = m_iScaledMapHeight - m_pDrawFrame.Height - (int)m_fFrameWidth;

            Refresh();

            //сигнализируем основной карте о том, что область просмотра сдвинулась
            m_pMasterMap.SinchronizeDrawFrame();
        }
    }
}
