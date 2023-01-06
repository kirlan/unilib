using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BenTools.Mathematics;
using Random;
using System.IO;
using Zipp;
using SimpleVectors;

namespace LandscapeGeneration
{
    public enum GridType
    {
        Square,
        Hex
    }

    public enum WorldShape
    {
        /// <summary>
        /// обычная плоская прямоугольная карта с непереходимым краем
        /// </summary>
        Plain,
        /// <summary>
        /// Мир-кольцо - внутренняя поверхность цилиндра, боковые грани непереходимые
        /// </summary>
        Ringworld,
        /// <summary>
        /// Панцирь черепахи - круглый плоский мир с горами в центре, со всех сторон окружённый бесконечным океаном
        /// </summary>
        Shell,
        /// <summary>
        /// Чаша - круглый плоский мир с центральным морем, со всех сторон окружённый непроходимыми горами
        /// </summary>
        Chalice
    }

    public class LocationsGrid : IGrid 
    {
        /// <summary>
        /// Расстояние от экватора до полюса. 
        /// </summary>
        private const int m_iRY = 50000;

        public int RY
        {
            get { return m_iRY; }
        }

        private int m_iRX = 62500;

        /// <summary>
        /// Половина длины экватора
        /// </summary>
        public int RX
        {
            get { return m_iRX; }
        }
        public int FrameWidth { get; } = 0;

        public float CycleShift
        {
            get { return m_eShape == WorldShape.Ringworld ? m_iRX * 2 : 0; }
        }

        public Location[] Locations { get; private set; } = null;

        public string m_sDescription = "";

        /// <summary>
        /// Общее количество `локаций` - минимальных "кирпичиков" мира.
        /// Обычно меньше реального числа элементов в m_cLocations, т.к. там присутсвуют ещё "бордюрные"
        /// локации, недоступные для посещения.
        /// </summary>
        public int m_iLocationsCount = 5000;//25000;

        public VoronoiVertex[] m_aVertexes = null;

        public WorldShape m_eShape = WorldShape.Plain;


        /// <summary>
        /// Создание сетки из случайных точек
        /// </summary>
        /// <param name="iLocations">количество точек</param>
        /// <param name="iWidth">пропорции карты - ширина</param>
        /// <param name="iHeight">пропорции карты - высота)</param>
        public LocationsGrid(int iLocations, int iWidth, int iHeight, string sDescription, WorldShape eShape)
        {
            m_iRX = m_iRY * iWidth / iHeight;
            m_iLocationsCount = iLocations;
            m_eShape = eShape;

            bool bOK = true;
            do
            {
                BuildRandomGrid();
                bOK = CalculateVoronoi();
                if (bOK)
                {
                    //для всех ячеек связываем разрозненные рёбра в замкнутую ломаную границу
                    foreach (Location pLoc in Locations)
                    {
                        pLoc.BuildBorder(m_iRX * 2);
                        pLoc.CorrectCenter();
                    }
                }
            }
            while (!bOK);

            m_sDescription = sDescription;
            m_bLoaded = true;
        }

        /// <summary>
        /// Создание упорядоченной сетки
        /// </summary>
        /// <param name="iWidth">ширина</param>
        /// <param name="iHeight">высота</param>
        /// <param name="eGridType">тип сетки</param>
        public LocationsGrid(int iWidth, int iHeight, GridType eGridType, string sDescription, WorldShape eShape)
        {
            m_eShape = eShape;

            if (eGridType == GridType.Hex)
            {
                float fProportion = 2.0f * iWidth * (float)Math.Sqrt(0.75) / (iHeight + 1);
                m_iRX = (int)(m_iRY * fProportion);

                m_iLocationsCount = iWidth * iHeight / 2;

                bool bOK = true;
                do
                {
                    BuildHexGrid(iWidth, iHeight);
                    bOK = CalculateVoronoi();
                }
                while (!bOK);

                //для всех ячеек связываем разрозненные рёбра в замкнутую ломаную границу
                foreach (Location pLoc in Locations)
                {
                    pLoc.BuildBorder(m_iRX * 2);
                    pLoc.CorrectCenter();
                }

                m_sDescription = sDescription;
                m_bLoaded = true;
            }
            else
                throw new ArgumentException(string.Format("Grid type '{0}' is not implemented yet!", eGridType));
        }

        /// <summary>
        /// <summary>
        /// Строит сетку/диаграмму Вороного НА ПЛОСКОСТИ.
        /// </summary>
        /// <returns></returns>
        private bool CalculateVoronoi()
        {
            Dictionary<BTVector, Location> cData = new Dictionary<BTVector, Location>();
            foreach (Location pLoc in Locations)
                cData[new BTVector(pLoc.X, pLoc.Y)] = pLoc;

            //Строим диаграмму вороного - определяем границы локаций
            VoronoiGraph graph = Fortune.ComputeVoronoiGraph(cData.Keys);
            Dictionary<BTVector, VoronoiVertex> cVertexes = new Dictionary<BTVector, VoronoiVertex>();

            //Переводим данные из диаграммы Вороного в наш формат
            try
            {
                foreach (BenTools.Mathematics.VoronoiEdge pEdge in graph.Edges)
                    AddEdge(cData, cVertexes, pEdge);
            }
            catch (Exception ex)
            {
                //бывает, алгоритм выдаёт данные, которые мы не можем корректно перевести (нулевые рёбра, etc.)
                //в этом случае всё приходится начинать заново
                return false;
            }

            m_aVertexes = new List<VoronoiVertex>(cVertexes.Values).ToArray();

            foreach (Location pLoc in Locations)
                pLoc.FillBorderWithKeys();

            return true;
        }

        private bool AddEdge(Dictionary<BTVector, Location> cData, Dictionary<BTVector, VoronoiVertex> cVertexes, BenTools.Mathematics.VoronoiEdge pEdge)
        {
            Location pLoc1 = null;
            Location pLoc2 = null;

            if (pEdge.VVertexA == pEdge.VVertexB)
                return false;

            if (pEdge.VVertexA.data[0] == pEdge.VVertexB.data[0] && pEdge.VVertexA.data[1] == pEdge.VVertexB.data[1])
                return false;

            //получаем ссылки на реальные локации, с которыми связаны BT-вектора
            if (cData.ContainsKey(pEdge.LeftData))
                pLoc1 = cData[pEdge.LeftData];
            if (cData.ContainsKey(pEdge.RightData))
                pLoc2 = cData[pEdge.RightData];

            //если обе локации "призрачные" - нас это не интересует
            if (pLoc1.m_pOrigin != null && pLoc2.m_pOrigin != null)
                return false;

            //если одна из вершин грани оказывается в бесконечности - игнорируем такую грань
            if (pEdge.VVertexA == Fortune.VVUnkown || pEdge.VVertexB == Fortune.VVUnkown)
            {
                pLoc1.m_bUnclosed = true;
                pLoc2.m_bUnclosed = true;
                return false;
            }
            //если одна из вершин грани оказывается хоть и не в бесконечности, но всё-равно достаточно далеко за краем карты - тоже игнорируем
            if (pEdge.VVertexA.data[0] < -RX * 2 ||
                pEdge.VVertexA.data[0] > RX * 2 ||
                pEdge.VVertexA.data[1] < -RY * 2 ||
                pEdge.VVertexA.data[1] > RY * 2 ||
                pEdge.VVertexB.data[0] < -RX * 2 ||
                pEdge.VVertexB.data[0] > RX * 2 ||
                pEdge.VVertexB.data[1] < -RY * 2 ||
                pEdge.VVertexB.data[1] > RY * 2)
            {
                pLoc1.m_bUnclosed = true;
                pLoc2.m_bUnclosed = true;
                return false;
            }

            //при необходимости, создадим новые вертексы для вершин добавляемой грани
            if (!cVertexes.ContainsKey(pEdge.VVertexA))
                cVertexes[pEdge.VVertexA] = new VoronoiVertex(pEdge.VVertexA);

            VoronoiVertex pVertexA = cVertexes[pEdge.VVertexA];

            if (!cVertexes.ContainsKey(pEdge.VVertexB))
                cVertexes[pEdge.VVertexB] = new VoronoiVertex(pEdge.VVertexB);

            VoronoiVertex pVertexB = cVertexes[pEdge.VVertexB];

            if (pVertexA == pVertexB)
                throw new Exception("Vertexes too close!");

            //Для определения того, лежат ли точки A и B относительно центра локации L1 по часовой стрелке
            //или против, рассчитаем z-координату векторного произведения векторов (L1, A) и (L1, B).
            //Если она будет положительна, то все три вектора (направления на вершины из центра локации и 
            //их векторное произведение) составляют "правую тройку", т.е. точки A и B лежат против часовой 
            //стрелки относительно центра.
            //Мы же, для определённости будем все вектора границы локации приводить к направлению "по часовой стрелке".

            float fAx = pVertexA.X - pLoc1.X;
            float fAy = pVertexA.Y - pLoc1.Y;
            float fBx = pVertexB.X - pLoc1.X;
            float fBy = pVertexB.Y - pLoc1.Y;

            bool bSwap = false;
            if (fAx * fBy > fAy * fBx)
                bSwap = true;

            if (bSwap)
            {
                VoronoiVertex pVertexC = pVertexA;
                pVertexA = pVertexB;
                pVertexB = pVertexC;
            }

            //добавим вертексам ссылки друг на друга
            if (!pVertexA.m_cVertexes.Contains(pVertexB))
                pVertexA.m_cVertexes.Add(pVertexB);
            if (!pVertexB.m_cVertexes.Contains(pVertexA))
                pVertexB.m_cVertexes.Add(pVertexA);

            if (pLoc1 != null && pLoc1.m_pOrigin == null)
            {
                if (!pVertexA.m_cLocationsBuild.Contains(pLoc1))
                    pVertexA.m_cLocationsBuild.Add(pLoc1);
                if (!pVertexB.m_cLocationsBuild.Contains(pLoc1))
                    pVertexB.m_cLocationsBuild.Add(pLoc1);

                if (pLoc2 != null)
                {
                    VoronoiEdge pLine = new VoronoiEdge(pVertexA, pVertexB);
                    if (pLine.Length > 0)
                    {
                        foreach (List<VoronoiEdge> cLines in pLoc1.BorderWith.Values)
                            if (cLines[0].m_pPoint1 == pVertexA)
                                throw new Exception("Wrong edge!");
                        //if(!bTwin)
                        if (pLoc2.m_pOrigin == null)
                        {
                            pLoc1.BorderWith[pLoc2] = new List<VoronoiEdge>();
                            pLoc1.BorderWith[pLoc2].Add(pLine);
                        }
                        else
                        {
                            pLoc1.BorderWith[pLoc2.m_pOrigin] = new List<VoronoiEdge>();
                            pLoc1.BorderWith[pLoc2.m_pOrigin].Add(pLine);
                        }
                    }
                }
            }

            if (pLoc2 != null && pLoc2.m_pOrigin == null)
            {
                if (!pVertexA.m_cLocationsBuild.Contains(pLoc2))
                    pVertexA.m_cLocationsBuild.Add(pLoc2);
                if (!pVertexB.m_cLocationsBuild.Contains(pLoc2))
                    pVertexB.m_cLocationsBuild.Add(pLoc2);

                if (pLoc1 != null)
                {
                    VoronoiEdge pLine = new VoronoiEdge(pVertexB, pVertexA);
                    if (pLine.Length > 0)
                    {
                        foreach (List<VoronoiEdge> cLines in pLoc2.BorderWith.Values)
                            if (cLines[0].m_pPoint1 == pVertexB)
                                throw new Exception("Wrong edge!");
                        //if (!bTwin)
                        if (pLoc1.m_pOrigin == null)
                        {
                            pLoc2.BorderWith[pLoc1] = new List<VoronoiEdge>();
                            pLoc2.BorderWith[pLoc1].Add(pLine);
                        }
                        else
                        {
                            pLoc2.BorderWith[pLoc1.m_pOrigin] = new List<VoronoiEdge>();
                            pLoc2.BorderWith[pLoc1.m_pOrigin].Add(pLine);
                        }
                    }
                }
            }

            return true;
        }

        private void BuildHexGrid(int iWidth, int iHeight)
        {
            List<Location> cLocations = new List<Location>();

            //float stepX = 2.0f * RX / (iWidth);
            //float stepY = stepX / (float)Math.Sqrt(0.75);

            float stepY = 4.0f * RY / (iHeight + 1);
            float stepX = stepY * (float)Math.Sqrt(0.75);

            //List<long> cUsedIDs = new List<long>();

            int iWidthReserv = m_eShape == WorldShape.Ringworld ? 0 : 2;
            int iHeightReserv = 2;

            for (int yy = -iHeightReserv; yy < iHeight / 2 + iHeightReserv; yy++)
                for (int xx = -iWidthReserv; xx < iWidth + iWidthReserv; xx++)
                {
                    Location pLocation = new Location();

                    float fx = xx * stepX;

                    float fy = yy * stepY;
                    if ((xx + 2) % 2 == 1)
                        fy += stepY / 2;

                    bool bBorder = yy < 0 || yy >= iHeight / 2 || xx < 0 || xx >= iWidth;

                    long iID = xx + yy * iWidth;
                    if (bBorder)
                        iID = xx + iWidthReserv + (yy + iHeightReserv) * (iWidth + 2 * iWidthReserv) + iWidth * iHeight * 2;

                    //if(cUsedIDs.Contains(iID))
                    //    iID += iWidth * iHeight * 2;

                    pLocation.Create(iID, -RX + stepX / 2 + fx, -RY + stepY / 2 + fy, xx, yy);
                    pLocation.m_bBorder = bBorder;

                    cLocations.Add(pLocation);

                    if (m_eShape == WorldShape.Ringworld)
                    {
                        Location pGhostLocation = new Location();

                        if (stepX / 2 + fx > RX)
                            pGhostLocation.Create(iID + iWidth * iHeight * 4, -RX * 3 + stepX / 2 + fx, -RY + stepY / 2 + fy, pLocation);
                        else
                            pGhostLocation.Create(iID + iWidth * iHeight * 4, RX + stepX / 2 + fx, -RY + stepY / 2 + fy, pLocation);

                        pGhostLocation.m_bBorder = true;

                        cLocations.Add(pGhostLocation);
                    }

                    //cUsedIDs.Add(iID);

                }

            Locations = cLocations.ToArray();
        }

        private List<Location> BuildRandomGridFrame()
        {
            List<Location> cLocations = new List<Location>();

            float kx = (int)(Math.Sqrt((float)RX * m_iLocationsCount / RY));
            float ky = m_iLocationsCount / kx;

            float dkx = RX / (kx * 2);
            float dky = RY / (ky * 2);

            //Создаём периметр карты из "запретных" локаций, для того чтобы получить ровную кромку.
            for (int ii = 0; ii <= kx; ii++)
            {
                int xx = (int)(ii * 2 * RX / kx);

                Location pLocation11 = new Location();
                pLocation11.Create(cLocations.Count, RX - xx, RY - dky);
                pLocation11.m_bBorder = xx == 0 || xx == 2 * RX;//false;
                pLocation11.m_bFixed = true;
                cLocations.Add(pLocation11);

                Location pLocation12 = new Location();
                pLocation12.Create(cLocations.Count, RX - xx, RY + dky);
                pLocation12.m_bBorder = true;
                pLocation12.m_bFixed = true;
                cLocations.Add(pLocation12);

                Location pLocation21 = new Location();
                pLocation21.Create(cLocations.Count, RX - xx, -RY - dky);
                pLocation21.m_bBorder = true;
                pLocation21.m_bFixed = true;
                cLocations.Add(pLocation21);

                Location pLocation22 = new Location();
                pLocation22.Create(cLocations.Count, RX - xx, -RY + dky);
                pLocation22.m_bBorder = xx == 0 || xx == 2 * RX;//false;
                pLocation22.m_bFixed = true;
                cLocations.Add(pLocation22);

                if (m_eShape == WorldShape.Ringworld)
                {
                    Location pLocation11Ghost = new Location();
                    if (pLocation11.X > 0)
                        pLocation11Ghost.Create(cLocations.Count, pLocation11.X - RX * 2, pLocation11.Y, pLocation11);
                    else
                        pLocation11Ghost.Create(cLocations.Count, pLocation11.X + RX * 2, pLocation11.Y, pLocation11);
                    pLocation11Ghost.m_bBorder = true;
                    pLocation11Ghost.m_bFixed = true;
                    cLocations.Add(pLocation11Ghost);

                    Location pLocation12Ghost = new Location();
                    if (pLocation12.X > 0)
                        pLocation12Ghost.Create(cLocations.Count, pLocation12.X - RX * 2, pLocation12.Y, pLocation12);
                    else
                        pLocation12Ghost.Create(cLocations.Count, pLocation12.X + RX * 2, pLocation12.Y, pLocation12);
                    pLocation12Ghost.m_bBorder = true;
                    pLocation12Ghost.m_bFixed = true;
                    cLocations.Add(pLocation12Ghost);

                    Location pLocation21Ghost = new Location();
                    if (pLocation21.X > 0)
                        pLocation21Ghost.Create(cLocations.Count, pLocation21.X - RX * 2, pLocation21.Y, pLocation21);
                    else
                        pLocation21Ghost.Create(cLocations.Count, pLocation21.X + RX * 2, pLocation21.Y, pLocation21);
                    pLocation21Ghost.m_bBorder = true;
                    pLocation21Ghost.m_bFixed = true;
                    cLocations.Add(pLocation21Ghost);

                    Location pLocation22Ghost = new Location();
                    if (pLocation22.X > 0)
                        pLocation22Ghost.Create(cLocations.Count, pLocation22.X - RX * 2, pLocation22.Y, pLocation22);
                    else
                        pLocation22Ghost.Create(cLocations.Count, pLocation22.X + RX * 2, pLocation22.Y, pLocation22);
                    pLocation22Ghost.m_bBorder = true;
                    pLocation22Ghost.m_bFixed = true;
                    cLocations.Add(pLocation22Ghost);
                }
            }

            if (m_eShape != WorldShape.Ringworld)
            {
                for (int jj = 0; jj <= ky; jj++)
                {
                    int yy = (int)(jj * 2 * RY / ky);

                    Location pLocation11 = new Location();
                    pLocation11.Create(cLocations.Count, RX - dkx, RY - yy);
                    pLocation11.m_bBorder = yy == 0 || yy == 2 * RY;//false;
                    pLocation11.m_bFixed = true;
                    cLocations.Add(pLocation11);

                    Location pLocation12 = new Location();
                    pLocation12.Create(cLocations.Count, RX + dkx, RY - yy);
                    pLocation12.m_bBorder = true;
                    pLocation12.m_bFixed = true;
                    cLocations.Add(pLocation12);

                    Location pLocation21 = new Location();
                    pLocation21.Create(cLocations.Count, -RX - dkx, RY - yy);
                    pLocation21.m_bBorder = true;
                    pLocation21.m_bFixed = true;
                    cLocations.Add(pLocation21);

                    Location pLocation22 = new Location();
                    pLocation22.Create(cLocations.Count, -RX + dkx, RY - yy);
                    pLocation22.m_bBorder = yy == 0 || yy == 2 * RY;//false;
                    pLocation22.m_bFixed = true;
                    cLocations.Add(pLocation22);
                }
            }

            return cLocations;
        }

        private void BuildRandomGrid()
        {
            List<Location> cLocations = BuildRandomGridFrame();

            float kx = (int)(Math.Sqrt((float)RX * m_iLocationsCount / RY));
            float ky = m_iLocationsCount / kx;

            float dkx = RX / (kx * 2);
            float dky = RY / (ky * 2);

            float Sbig = (float)RX * (float)RY * 4;
            var S1 = Sbig / m_iLocationsCount;
            var h = Math.Sqrt(S1 / (4 * Math.Sqrt(3)));
            var R = Math.Sqrt(S1 / (3 * Math.Sqrt(3)));

            R = (R + h) / 2;
            R *= 0.96;

            int c = 0;
            List<SimpleVector3d> cPoints = new List<SimpleVector3d>();
            do
            {
                cPoints = UniformPoissonDiskSampler.SampleRectangle(new SimpleVector3d(-RX + dkx * 2, -RY + dky * 2, 0),
                                                    new SimpleVector3d(RX - dkx * 2, RY - dky * 2, 0),
                                                        (float)R * 2);
                R *= 0.99f;
                c++;
            }
            while (cPoints.Count < m_iLocationsCount);

            m_iLocationsCount = cPoints.Count;

            //Добавляем центры остальных локаций в случайные позиции внутри периметра.
            for (int i = 0; i < m_iLocationsCount; i++)
            {
                Location pLocation = new Location();
                //                pLocation.Create(cLocations.Count, RX - dkx * 2 - Rnd.Get(RX * 2 - 4 * dkx), RY - dky * 2 - Rnd.Get(RY * 2 - 4 * dky), 0);
                pLocation.Create(cLocations.Count, cPoints[i].X, cPoints[i].Y);
                cLocations.Add(pLocation);

                if (m_eShape == WorldShape.Ringworld)
                {
                    Location pGhostLocation = new Location();

                    if (pLocation.X > 0)
                        pGhostLocation.Create(cLocations.Count, pLocation.X - RX * 2, pLocation.Y, pLocation);
                    else
                        pGhostLocation.Create(cLocations.Count, pLocation.X + RX * 2, pLocation.Y, pLocation);

                    pGhostLocation.m_bBorder = true;

                    cLocations.Add(pGhostLocation);
                }
            }

            Locations = cLocations.ToArray();
        }

        private static int s_iVersion = 30;
        private static string s_sHeader = "DimensionX World Generator Grid File.";

        public void Save(string sFilename)
        {
            var fil = new FileStream(sFilename, FileMode.Create);
            m_sFilename = sFilename;

            using (var arc = ZipArchive.OpenOnStream(fil))
            {
                var fs = arc.AddFile("grid", ZipArchive.CompressionMethodEnum.Deflated, ZipArchive.DeflateOptionEnum.Maximum);

                using (BinaryWriter binWriter =
                    new BinaryWriter(fs.GetStream(FileMode.OpenOrCreate, FileAccess.Write)))
                {
                    binWriter.Write(m_aVertexes.Length);
                    foreach (VoronoiVertex pVertex in m_aVertexes)
                    {
                        pVertex.Save(binWriter);
                    }

                    binWriter.Write(Locations.Length);
                    foreach (Location pLoc in Locations)
                    {
                        pLoc.Save(binWriter);
                    }
                }

                var fs2 = arc.AddFile("header", ZipArchive.CompressionMethodEnum.Deflated, ZipArchive.DeflateOptionEnum.Maximum);

                using (BinaryWriter binWriter =
                    new BinaryWriter(fs2.GetStream(FileMode.OpenOrCreate, FileAccess.Write)))
                {
                    binWriter.Write(s_sHeader);
                    binWriter.Write(s_iVersion);
                    binWriter.Write(m_sDescription);
                    binWriter.Write(m_iLocationsCount);
                    binWriter.Write(m_iRX);
                    binWriter.Write((int)m_eShape);
                }
            }
            fil.Dispose();

            m_sFilename = sFilename;
        }

        public static bool CheckFile(string sFilename, out string sDescription, out int iLocationsCount, out WorldShape eShape)
        {
            sDescription = "";
            eShape = WorldShape.Plain;
            iLocationsCount = 0;

            if (!File.Exists(sFilename))
                return false;

            using (var fil = new FileStream(sFilename, FileMode.Open))
            {
                using (var arc = ZipArchive.OpenOnStream(fil))
                {
                    BinaryReader binReader = null;

                    try
                    {
                        ZipArchive.ZipFileInfo? pInfo = arc.GetFile("header");
                        if (!pInfo.HasValue)
                        {
                            return false;
                        }

                        binReader = new BinaryReader(pInfo.Value.GetStream());

                        // If the file is not empty,
                        // read the application settings.
                        // First read 4 bytes into a buffer to
                        // determine if the file is empty.
                        byte[] testArray = new byte[3];
                        int count = binReader.Read(testArray, 0, 3);

                        if (count != 0)
                        {
                            // Reset the position in the stream to zero.
                            binReader.BaseStream.Seek(0, SeekOrigin.Begin);

                            string sHeader = binReader.ReadString();
                            if (sHeader != s_sHeader)
                                return false;

                            int iVersion = binReader.ReadInt32();
                            if (iVersion != s_iVersion)
                                return false;

                            sDescription = binReader.ReadString();
                            iLocationsCount = binReader.ReadInt32();
                            int iRX = binReader.ReadInt32();
                            eShape = (WorldShape)Enum.GetValues(typeof(WorldShape)).GetValue(binReader.ReadInt32());
                        }
                    }
                    catch (EndOfStreamException e)
                    {
                        return false;
                    }
                    finally
                    {
                        if (binReader != null)
                            binReader.Close();
                    }
                }
            }
            //fil.Dispose();
            return true;
        }

        public bool m_bLoaded = false;

        public string m_sFilename;

        public LocationsGrid(string sFilename)
        {
            if (!CheckFile(sFilename, out m_sDescription, out m_iLocationsCount, out m_eShape))
                throw new ArgumentException("File not exists or have a wrong format!");

            m_sFilename = sFilename;
        }

        public void Unload()
        {
            Reset();

            Locations = null;
            m_aVertexes = null;

            m_bLoaded = false;
        }

        public void Load(BeginStepDelegate BeginStep, ProgressStepDelegate ProgressStep)
        {
            if (m_bLoaded)
                return;

            if (!File.Exists(m_sFilename))
                throw new Exception("File '" + m_sFilename + "' not exists!");

            var fil = new FileStream(m_sFilename, FileMode.Open);
            using (var arc = ZipArchive.OpenOnStream(fil))
            {
                BinaryReader binReader = null;

                try
                {
                    ZipArchive.ZipFileInfo? pInfo = arc.GetFile("header");
                    if (!pInfo.HasValue)
                        throw new Exception("No header!");

                    binReader = new BinaryReader(pInfo.Value.GetStream());

                    // If the file is not empty,
                    // read the application settings.
                    // First read 4 bytes into a buffer to
                    // determine if the file is empty.
                    byte[] testArray = new byte[3];
                    int count = binReader.Read(testArray, 0, 3);

                    if (count != 0)
                    {
                        if (BeginStep != null)
                            BeginStep("Loading grid...", 1);

                        // Reset the position in the stream to zero.
                        binReader.BaseStream.Seek(0, SeekOrigin.Begin);

                        string sHeader = binReader.ReadString();
                        if (sHeader != s_sHeader)
                            throw new Exception("Header mismatch!");

                        int iVersion = binReader.ReadInt32();
                        if (iVersion != s_iVersion)
                            throw new Exception("Version mismatch!");

                        m_sDescription = binReader.ReadString();
                        m_iLocationsCount = binReader.ReadInt32();
                        m_iRX = binReader.ReadInt32();
                        m_eShape = (WorldShape)Enum.GetValues(typeof(WorldShape)).GetValue(binReader.ReadInt32());

                        if (ProgressStep != null)
                            ProgressStep();
                    }
                }
                catch (EndOfStreamException e)
                {
                    throw new Exception("Wrong file format!", e);
                }
                finally
                {
                    if (binReader != null)
                        binReader.Close();
                }

                try
                {
                    ZipArchive.ZipFileInfo? pInfo = arc.GetFile("grid");
                    if (!pInfo.HasValue)
                        throw new Exception("No data!");

                    binReader = new BinaryReader(pInfo.Value.GetStream());

                    // If the file is not empty,
                    // read the application settings.
                    // First read 4 bytes into a buffer to
                    // determine if the file is empty.
                    byte[] testArray = new byte[3];
                    int count = binReader.Read(testArray, 0, 3);

                    if (count != 0)
                    {
                        // Reset the position in the stream to zero.
                        binReader.BaseStream.Seek(0, SeekOrigin.Begin);

                        Dictionary<long, VoronoiVertex> cTempDicVertex = new Dictionary<long, VoronoiVertex>();
                        int iVertexesCount = binReader.ReadInt32();
                        if (BeginStep != null)
                            BeginStep("Loading vertexes...", iVertexesCount * 2);
                        for (int i = 0; i < iVertexesCount; i++)
                        {
                            VoronoiVertex pVertexLoc = new VoronoiVertex(binReader);

                            cTempDicVertex[pVertexLoc.m_iVertexID] = pVertexLoc;

                            if (ProgressStep != null)
                                ProgressStep();
                        }

                        m_aVertexes = new List<VoronoiVertex>(cTempDicVertex.Values).ToArray();

                        //Восстанавливаем словарь соседей
                        foreach (VoronoiVertex pVertex in m_aVertexes)
                        {
                            foreach (long iID in pVertex.m_cLinksTmp)
                            {
                                pVertex.m_cVertexes.Add(cTempDicVertex[iID]);
                            }
                            pVertex.m_cLinksTmp.Clear();

                            if (ProgressStep != null)
                                ProgressStep();
                        }

                        Dictionary<long, Location> cTempDic = new Dictionary<long, Location>();
                        int iLocationsCount = binReader.ReadInt32();
                        if (BeginStep != null)
                            BeginStep("Loading locations...", iLocationsCount * 2);
                        for (int i = 0; i < iLocationsCount; i++)
                        {
                            Location pLoc = new Location();
                            pLoc.Load(binReader, cTempDicVertex);

                            cTempDic[pLoc.m_iID] = pLoc;

                            if (ProgressStep != null)
                                ProgressStep();
                        }

                        Locations = new List<Location>(cTempDic.Values).ToArray();

                        //Восстанавливаем словарь соседей
                        foreach (Location pLoc in Locations)
                        {
                            foreach (var ID in pLoc.m_cBorderWithID)
                            {
                                pLoc.BorderWith[cTempDic[ID.Key]] = ID.Value;
                            }
                            pLoc.m_cBorderWithID.Clear();
                            pLoc.FillBorderWithKeys();

                            if (ProgressStep != null)
                                ProgressStep();
                        }

                        //Восстанавливаем словарь соседей для вертексов
                        foreach (VoronoiVertex pVertex in m_aVertexes)
                        {
                            pVertex.m_cLocations.Clear();// = new Location[pVertex.m_cLocationsTmp.Count];
                            //int iIndex = 0;
                            foreach (long iID in pVertex.m_cLocationsTmp)
                            {
                                //pVertex.m_cLocations[iIndex++] = cTempDic[iID];
                                pVertex.m_cLocations.Add(cTempDic[iID]);
                            }
                            pVertex.m_cLocationsTmp.Clear();
                        }

                        cTempDicVertex.Clear();
                        cTempDic.Clear();

                        if (BeginStep != null)
                            BeginStep("Recalculating grid edges...", Locations.Length);
                        //для всех ячеек связываем разрозненные рёбра в замкнутую ломаную границу
                        foreach (Location pLoc in Locations)
                        {
                            if (pLoc.Forbidden)
                                continue;

                            pLoc.BuildBorder(CycleShift);
                            if (ProgressStep != null)
                                ProgressStep();
                        }

                        m_bLoaded = true;
                    }
                }
                catch (EndOfStreamException e)
                {
                    throw new Exception("Wrong file format!", e);
                }
                finally
                {
                    if (binReader != null)
                        binReader.Close();
                }
            }
            fil.Dispose();
        }

        public void Reset()
        {
            if (!m_bLoaded)
                return;

            foreach (Location pLoc in Locations)
                pLoc.Reset();
        }

        public override string ToString()
        {
            FileInfo pInfo = new FileInfo(m_sFilename);
            return m_sDescription + " (" + pInfo.Name + ")";
        }
    }
}
