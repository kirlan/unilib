using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace MapGen
{
    public class CFractalMap
    {
        private int m_iWidth;
        /// <summary>
        /// ширина карты
        /// </summary>
        public int Width
        {
            get { return m_iWidth; }
            set { m_iWidth = value; }
        }

        private int m_iHeight;
        /// <summary>
        /// высота карты
        /// </summary>
        public int Height
        {
            get { return m_iHeight; }
            set { m_iHeight = value; }
        }

        /// <summary>
        /// двумерный массив регионов
        /// </summary>
        private CFRegion[][] m_pRegions;
        public CFRegion[][] Regions
        {
            get { return m_pRegions; }
        }

        private int m_iOceanLevel = 12800;
        /// <summary>
        /// уровень океана
        /// </summary>
        public int OceanLevel
        {
            get { return m_iOceanLevel; }
            set { m_iOceanLevel = value; }
        }

        private const int Uninitalized = -128;

        public CFractalMap(int iWidth, int iHeight)
        {
            m_iWidth = iWidth;
            m_iHeight = iHeight;

            m_pRegions = new CFRegion[m_iWidth][];
            int x, y;
            for (x = 0; x < m_iWidth; x++)
            {
                m_pRegions[x] = new CFRegion[m_iHeight];
                for (y = 0; y < m_iHeight; y++)
                {
                    m_pRegions[x][y] = new CFRegion(x, y, -1, Uninitalized, false);
                }
            }
        }

        private void CalculateAngularCoordinates()
        {
            int x, y;
            for (y = 0; y < m_iHeight; y++)
            {
                double currL, lStep, totalL;
                currL = 0;
                totalL = m_iWidth;

                double w;
                w = (double)(m_iHeight - 1) / 2 - y;
                if (Math.Abs(w) > m_iHeight / 6)
                {
                    totalL = (double)m_iWidth - 5.0 - (1.0 - ((int)(Math.Abs(w) + 0.5) - (double)m_iHeight / 6.0)) * ((double)m_iWidth - 10.0) / (1.0 - (double)m_iHeight / 3.0);
                }
                lStep = 2 * Math.PI / totalL;

                for (x = 0; x < m_iWidth; x++)
                {
                    if (m_pRegions[x][y].Allowed)
                    {
                        m_pRegions[x][y].Longitude = currL;
                        if (y < m_iHeight / 3)
                            m_pRegions[x][y].Longitude += lStep / 2;
                        m_pRegions[x][y].Latitude = w * Math.PI / m_iHeight;

                        if (x % 2 != y % 2)
                        {
                            m_pRegions[x][y].Latitude += (Math.PI / m_iHeight) / 6.0;
                        }
                        else
                        {
                            m_pRegions[x][y].Latitude -= (Math.PI / m_iHeight) / 6.0;
                        }

                        currL += lStep;
                    }
                }
            }
        }

        /// <summary>
        /// инициализируем карту, забиваем "снегом" вырезанные секторы проекции
        /// </summary>
        /// <param name="iHeight">высота региона</param>
        /// <param name="iContiID">континентальный идентификатор</param>
        public void Initialize(int iHeight, int iContiID)
        {
            int x, y;
            for (x = 0; x < m_iWidth; x++)
            {
                for (y = 0; y < m_iHeight; y++)
                {
                    m_pRegions[x][y].Height = iHeight;
                    m_pRegions[x][y].ContinentID = iContiID;
                    m_pRegions[x][y].Allowed = true;
                    if (y < m_iHeight / 3)
                    {
                        int xx;
                        xx = x % (m_iWidth / 5);
                        if (xx < m_iWidth / 10 - y || xx > m_iWidth / 10 + y)
                        {
                            m_pRegions[x][y].Allowed = false;
                            m_pRegions[x][y].Height = -1000;
                        }
                    }
                    if (y >= 2 * m_iHeight / 3)
                    {
                        int xx, yy;
                        xx = x % (m_iWidth / 5);
                        yy = y - 2 * m_iHeight / 3;
                        if (xx >= m_iWidth / 10 - yy && xx <= m_iWidth / 10 + yy)
                        {
                            m_pRegions[x][y].Allowed = false;
                            m_pRegions[x][y].Height = -1000;
                        }
                    }
                }
            }
            CalculateAngularCoordinates();
        }

        /// <summary>
        /// копируем регионы с одной карты на другую, совмещая размерность
        /// </summary>
        /// <param name="pOldMap">старая карта</param>
        internal void CopyFrom(CFractalMap pOldMap)
        {
            double dx, dy;
            dx = m_iWidth / pOldMap.Width;
            dy = m_iHeight / pOldMap.Height;

            int x, y;
            // переносим на новую карту регионы со старой
            for (x = 0; x < pOldMap.Width; x++)
            {
                for (y = 0; y < pOldMap.Height; y++)
                {
                    int iNewX, iNewY;
                    iNewX = (int)(x * dx);
                    iNewY = (int)(y * dy);
                    if (x % 2 == y % 2)
                    {
                        iNewY++;
                    }
                    m_pRegions[iNewX][iNewY].Height = pOldMap.m_pRegions[x][y].Height;
                    m_pRegions[iNewX][iNewY].Allowed = pOldMap.m_pRegions[x][y].Allowed;
                    m_pRegions[iNewX][iNewY].ContinentID = pOldMap.m_pRegions[x][y].ContinentID;

                    CFRegion region;
                    int iDir;
                    for (iDir = 0; iDir < 3; iDir++)
                    {
                        region = GetAdjacentRegion(iNewX, iNewY, iDir, false);
                        if (region.X != iNewX || region.Y != iNewY)
                        {
                            region.Allowed = pOldMap.m_pRegions[x][y].Allowed;
                        }
                    }
                }
            }

            CalculateAngularCoordinates();

            // заполняем пустые дыры между перенесёнными со старой карты регионами
            for (x = 0; x < m_iWidth; x++)
            {
                for (y = 0; y < m_iHeight; y++)
                {
                    if (m_pRegions[x][y].ContinentID == Uninitalized && m_pRegions[x][y].Allowed)
                    {
                        m_pRegions[x][y].Height = GetAdjacentRegion(x, y, Rnd.Get(3)).Height;
                    }
                }
            }

            // аппроксимируем пролученную поверхность, строим сплошную сетку
            Approximate();

            FillLandMasses();
        }

        // Search predicate returns true if a string ends in "saurus".
        private CLandMass FindLandMass(int id)
        {
            foreach (CLandMass land in m_pLandMass)
            {
                if (land.Id == id)
                    return land;
            }

            return null;
        }

        private void FillLandMasses()
        {
            for (int x = 0; x < m_iWidth; x++)
            {
                for (int y = 0; y < m_iHeight; y++)
                {
                    if (m_pRegions[x][y].Allowed)
                    {
                        if (m_pRegions[x][y].ContinentID != -1)
                        {
                            CLandMass land = FindLandMass(m_pRegions[x][y].ContinentID);
                            if(land == null)
                            {
                                land = new CLandMass(m_pRegions[x][y].ContinentID, 0, 0);
                                m_pLandMass.Add(land);
                            }
                            land.Regions.Add(m_pRegions[x][y]);
                        }
                    }
                }
            }
            foreach (CLandMass land in m_pLandMass)
                CalculateShoreLine(land, 0);

            foreach (CLandMass land in m_pLandMass)
                CalculateCentralPoint(land);

            //foreach (CLandMass land in m_pLandMass)
            //{
            //    VaryShoreLine(land);
            //    CalculateShoreLine(land, 0);
            //}
        }

        /// <summary>
        /// аппроксимируем полученную поверхность, строим сплошную сетку
        /// </summary>
        private void Approximate()
        {
            int x, y;
            for (x = 0; x < m_iWidth; x++)
            {
                for (y = 0; y < m_iHeight; y++)
                {
                    // если это ещё необработанный, но валидный регион
                    if (m_pRegions[x][y].ContinentID == Uninitalized && m_pRegions[x][y].Allowed)
                    {
                        // обходим всех соседей
                        for (int iDir = 0; iDir < 3; iDir++)
                        {
                            CFRegion region = GetAdjacentRegion(x, y, iDir);
                            if (region.X != x || region.Y != y)
                            {
                                // если сосед уже обработан
                                if (region.ContinentID != Uninitalized)
                                {
                                    // работать будем с двумя замерами высот
                                    int h1, h2;
                                    // первый замер - это найденный сосед
                                    h1 = region.Height;

                                    // второй замер - это сосед с противоположной стороны (с которым мы соприкасаемся вершиной, а не гранью)
                                    CFRegion counter = GetOppositeNeighbour(x, y, iDir);
                                    if (counter.ContinentID != Uninitalized)
                                    {
                                        h2 = counter.Height;
                                    }
                                    else
                                    {
                                        h2 = h1;
                                    }

                                    if (region.ContinentID == counter.ContinentID ||
                                        region.ContinentID < 0 ||
                                        counter.ContinentID < 0)
                                    {
                                        // комбинируем оба замера в соотношении 3/4, случайнам образом отдавая предпочтение первому или второну
                                        if (Rnd.Get(2) == 1)
                                            m_pRegions[x][y].Height = (3 * h2 + h1) / 4;
                                        else
                                            m_pRegions[x][y].Height = (3 * h1 + h2) / 4;

                                        // добавляем ещё немного случайных колебаний
                                        m_pRegions[x][y].Height += Rnd.Get(32) - 16;

                                        // если в итоге получается суша...
                                        if (m_pRegions[x][y].Height >= m_iOceanLevel)
                                        {
                                            // ...то присваиваем свежесформированному региону континентальный индекс соседа
                                            if (region.ContinentID > counter.ContinentID)
                                            {
                                                m_pRegions[x][y].ContinentID = region.ContinentID;
                                            }
                                            else
                                            {
                                                m_pRegions[x][y].ContinentID = counter.ContinentID;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        m_pRegions[x][y].Height = m_iOceanLevel - Rnd.Get(400) - 100;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            for (x = 0; x < m_iWidth; x++)
            {
                for (y = 0; y < m_iHeight; y++)
                {
                    if (m_pRegions[x][y].ContinentID == Uninitalized)
                        m_pRegions[x][y].ContinentID = -1;
                }
            }
        }

        private void VaryShoreLine(CLandMass land)
        {
            if (land.Border.Count == 0)
                return;

            foreach (CFRegion region in land.Border)
            {
                // вероятность тем выше, чем больше регионов суши уже граничит с этим регионом
                int chance = CountOtherLandsInRange(region.X, region.Y, -1, 2);
                if (Rnd.Get(10) < chance)
                {
                    region.Height = m_iOceanLevel - 1;
                    region.ContinentID = -1;
                }
                else
                {
                    // обходим всех соседей
                    for (int iDir = 0; iDir < 3; iDir++)
                    {
                        CFRegion adjacentRegion = GetAdjacentRegion(region.X, region.Y, iDir);
                        if (adjacentRegion.X != region.X || adjacentRegion.Y != region.Y)
                        {
                            // если мы стоим на земле, а соседний регион - вода
                            if (adjacentRegion.ContinentID == -1)
                            {
                                // вероятность тем ниже, чем больше регионов суши уже граничит с соседним регионом
                                chance = CountOtherLandsInRange(adjacentRegion.X, adjacentRegion.Y, -1, 1);
                                if (Rnd.OneChanceFrom(chance))
                                {
                                    adjacentRegion.Height = region.Height;
                                    adjacentRegion.ContinentID = region.ContinentID;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// получить соседний регион по указанному направлению
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <param name="dir">направление</param>
        /// <returns>найденный сосед</returns>
        private CFRegion GetAdjacentRegion(int x, int y, int dir)
        {
            CMapPoint result = GetAdjacentPoint(x, y, dir, true, true);
            return m_pRegions[result.X][result.Y];
        }

        /// <summary>
        /// получить соседний регион по указанному направлению
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <param name="dir">направление</param>
        /// <returns>найденный сосед</returns>
        private CFRegion GetAdjacentRegion(int x, int y, int dir, bool bAllowed)
        {
            CMapPoint result = GetAdjacentPoint(x, y, dir, bAllowed, true);
            return m_pRegions[result.X][result.Y];
        }

        /// <summary>
        /// соседние регионы для региона вершиной вниз (от начала координат)
        /// </summary>
        readonly CMapPoint[] aDir1 = {new CMapPoint(-1,0),
				               new CMapPoint(0,-1),
				               new CMapPoint(+1,0)};
        /// <summary>
        /// соседние регионы для региона вершиной вверх (к началу координат)
        /// </summary>
        readonly CMapPoint[] aDir2 = {new CMapPoint(-1,0),
							            new CMapPoint(0,+1),
							            new CMapPoint(+1,0)};

        /// <summary>
        /// получить соседний регион по указанному направлению
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <param name="dir">направление</param>
        /// <param name="onlyExists">не считать "виртуальные" регионы, находящиеся за пределами координатной сетки</param>
        /// <returns>найденный сосед</returns>
        private CMapPoint GetAdjacentPoint(int x, int y, int dir, bool bAllowed, bool onlyExists)
        {
            if (onlyExists)
            {
                if (x >= 0 && y >= 0 && x < m_iWidth && y < m_iHeight)
                {
                    if (!m_pRegions[x][y].Allowed)
                    {
                        return new CMapPoint(x, y);
                    }
                }
            }

            while (dir >= 6) dir -= 6;
            while (dir < 0) dir += 6;

            int tdir;
            if (dir > 2)
                tdir = 5 - dir;
            else
                tdir = dir;

            int iNewX, iNewY;

            if (x % 2 != y % 2)
            {
                iNewX = x + aDir1[tdir].X;
                iNewY = y + aDir1[tdir].Y;
            }
            else
            {
                iNewX = x + aDir2[tdir].X;
                iNewY = y + aDir2[tdir].Y;
            }

            while (iNewX >= m_iWidth)
            {
                iNewX -= m_iWidth;
            }
            while (iNewX < 0)
            {
                iNewX += m_iWidth;
            }

            if (onlyExists)
            {
                if (iNewY >= m_iHeight)
                {
                    iNewY = m_iHeight - 1;
                }
                if (iNewY < 0)
                {
                    iNewY = 0;
                }
            }

            if (iNewY < m_iHeight && iNewY >= 0)
            {
                while (m_pRegions[iNewX][iNewY].Allowed != bAllowed)
                {
                    if (iNewX % 2 != iNewY % 2)
                    {
                        iNewX = iNewX + aDir1[tdir].X;
                    }
                    else
                    {
                        iNewX = iNewX + aDir2[tdir].X;
                    }

                    while (iNewX >= m_iWidth)
                    {
                        iNewX -= m_iWidth;
                    }
                    while (iNewX < 0)
                    {
                        iNewX += m_iWidth;
                    }
                }
            }

            return new CMapPoint(iNewX, iNewY);
        }

        /// <summary>
        /// получить противососедний регион по указанному направлению (используется для адресации к координатной сетке фрактала предыдущего уровня глубины)
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <param name="dir">направление</param>
        /// <returns>найденный сосед</returns>
        private CFRegion GetOppositeNeighbour(int x, int y, int dir)
        {
            CMapPoint[] counter = new CMapPoint[3];

            int cnt;
            if (x % 2 != y % 2)
            {
                cnt = dir * 2 - 1;
            }
            else
            {
                cnt = 6 - dir * 2;
            }
            cnt = cnt + 2;

            counter[0] = GetAdjacentPoint(x, y, cnt, true, false);
            if ((Math.Abs(counter[0].X - x) > 1 && Math.Abs(counter[0].X - x) < m_iWidth - 1) || Math.Abs(counter[0].Y - y) > 1)
            {
                if ((counter[0].Y > m_iHeight / 2) == (counter[0].X > x))
                    cnt--;
                else
                    cnt++;
            }

            cnt++;
            counter[1] = GetAdjacentPoint(counter[0].X, counter[0].Y, cnt, true, false);
            if ((Math.Abs(counter[1].X - counter[0].X) > 1 && Math.Abs(counter[1].X - counter[0].X) < m_iWidth - 1) || Math.Abs(counter[1].Y - counter[0].Y) > 1)
            {
                if ((counter[1].Y > m_iHeight / 2) == (counter[1].X > counter[0].X))
                    cnt--;
                else
                    cnt++;
            }

            cnt++;
            counter[2] = GetAdjacentPoint(counter[1].X, counter[1].Y, cnt, true, false);
            if ((Math.Abs(counter[2].X - counter[1].X) > 1 && Math.Abs(counter[2].X - counter[1].X) < m_iWidth - 1) || Math.Abs(counter[2].Y - counter[1].Y) > 1)
            {
                if ((counter[2].Y > m_iHeight / 2) == (counter[2].X > counter[1].X))
                    cnt--;
                else
                    cnt++;
            }

            cnt--;

            return GetAdjacentRegion(counter[2].X, counter[2].Y, cnt);
        }

        public void Clear()
        {
            if (m_pRegions != null)
            {
                for (int i = 0; i < m_iWidth; i++)
                {
                    m_pRegions[i] = null;
                }
                m_pRegions = null;
            }

            m_iWidth = 0;
            m_iHeight = 0;
        }

        private List<CLandMass> m_pLandMass = new List<CLandMass>();

        public List<CLandMass> LandMass
        {
            get { return m_pLandMass; }
        }

        /// <summary>
        /// искать случайную точку на карте, пригодную для затравки нового континента
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
         private CFRegion GetRandomLonelyRegion(int range)
        {
            int iX, iY;
            int counter = 0;

            do
            {
                if (counter > m_iWidth * m_iHeight)
                {
                    range--;
                    counter = 0;
                }

                iX = Rnd.Get(m_iWidth);
                iY = Rnd.Get(m_iHeight);

                counter++;
            }
            // продолжать поиск, если найденная точка недопустима, принадлежит иному континенту либо является сопредельной с другими континентами
            while ((!m_pRegions[iX][iY].Allowed ||
                    m_pRegions[iX][iY].Height >= m_iOceanLevel ||
                    CountOtherLandsInRange(iX, iY, -1, range) > 0) &&
                   range >= 0);

            if (range > 0)
                return m_pRegions[iX][iY];
            else
                return null;
        }

        /// <summary>
         /// искать случайную точку на карте, пригодную для затравки нового острова
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        private CFRegion GetRandomShoreRegion(int range)
        {
            if(m_pLandMass.Count == 0)
                return GetRandomLonelyRegion(range);

            CLandMass land;

            do
            {
                land = m_pLandMass[Rnd.Get(m_pLandMass.Count)];
            }
            while (land.Border.Count == 0);

            int rangeStored = range;
            int counter = 0;

            int iX, iY;
            int idX, idY;
            do
            {
                range = rangeStored;
                CFRegion region = land.Border[Rnd.Get(land.Border.Count)];


                iX = region.X;
                iY = region.Y;

                if (Rnd.OneChanceFrom(2))
                {
                    idX = 0;
                    if (Rnd.OneChanceFrom(2))
                        idY = +1;
                    else
                        idY = -1;
                }
                else
                {
                    idY = 0;
                    if (Rnd.OneChanceFrom(2))
                        idX = +1;
                    else
                        idX = -1;
                }

                // продолжать поиск, если найденная точка недопустима, принадлежит иному континенту либо является сопредельной с другими континентами
                while ((!m_pRegions[iX][iY].Allowed ||
                        m_pRegions[iX][iY].Height >= m_iOceanLevel ||
                        CountOtherLandsInRange(iX, iY, -1, range) > 0/* ||
                        CountOtherLandsInRange(iX, iY, -1, range+1) == 0*/) &&
                       range >= 0)
                {
                    iX += idX;
                    iY += idY;

                    if (iX < 0 || iX >= m_iWidth ||
                        iY < 0 || iY >= m_iHeight)
                    {
                        range--;

                        iX = region.X;
                        iY = region.Y;

                        if (Rnd.OneChanceFrom(2))
                        {
                            idX = 0;
                            if (Rnd.OneChanceFrom(2))
                                idY = +1;
                            else
                                idY = -1;
                        }
                        else
                        {
                            idY = 0;
                            if (Rnd.OneChanceFrom(2))
                                idX = +1;
                            else
                                idX = -1;
                        }
                    }
                }
            }
            while (range <= 0 && counter++ < land.Border.Count);

            if (range > 0)
                return m_pRegions[iX][iY];
            else
                return GetRandomLonelyRegion(range);
        }

        internal CLandMass CreateLandMass(int iSize, bool bStandAlone)
        {
            int range = (int)Math.Sqrt(iSize); //3
            range = 1 + Rnd.Get(range) + Rnd.Get(range); //3

            if (range > m_iHeight / 2)
                range = m_iHeight / 2;

            CFRegion start;
            if (bStandAlone)
                start = GetRandomLonelyRegion(range);
            else
            {
                range = 1 + Rnd.Get(range);
                start = GetRandomShoreRegion(range);
            }

            if (start == null)
            {
                return null;
            }

            int contiID = m_pLandMass.Count;

            CLandMass land = new CLandMass(contiID, iSize, range);
            m_pLandMass.Add(land);

            start.Height = m_iOceanLevel + (10 + Rnd.Get(800)) * 16;	// случайная высота над уровнем моря
            start.ContinentID = contiID;	// идентификатор нового континента
            land.Regions.Add(start);
            land.Border.Add(start);

            return land;
        }

        /// <summary>
        /// наращиваем указанную землю на 1 регион, если возможно
        /// </summary>
        /// <param name="land">земля, которую нужно увеличить</param>
        /// <returns>удалось ли увеличить</returns>
        internal bool LandGrowUp(CLandMass land)
        {
            if (land == null)
                return false;

            if (land.Regions.Count >= land.SizeNeeded)
                return false;

            CalculateShoreLine(land, land.ShoreDistance);

            CFRegion region = land.GetBestFrontierRegion();
            //CFRegion region = continent.GetRandomBorderRegion();

            // если есть хоть один претендент
            if (region != null)
            {
                // и присоединяем его к новому континенту
                region.Height = m_iOceanLevel + (10 + Rnd.Get(800)) * 16;
                region.ContinentID = land.Id;

                land.Regions.Add(region);
                land.Border.Add(region);

                CalculateCentralPoint(land);

                return true;
            }
            return false;
        }

        /// <summary>
        /// добавить сушу заданной площади с заданным идентификатором
        /// </summary>
        /// <param name="iSize">площадь добавляемого острова/континента</param>
        /// <returns>площадь построенного континента</returns>
        internal int AddLand(int iSize, bool bStandAlone)
        {
            CLandMass land = CreateLandMass(iSize, bStandAlone);

            // до тех пор, пока не будет набрана требуемая площадь
            while (LandGrowUp(land))
            {
            }

            return land.Regions.Count;
        }

        private void CalculateShoreLine(CLandMass land, int range)
        {
            CFRegion[] seekingArray;

            if (land.Border.Count > 0)
            {
                seekingArray = new CFRegion[land.Border.Count];
                land.Border.CopyTo(seekingArray);
            }
            else
            {
                seekingArray = new CFRegion[land.Regions.Count];
                land.Regions.CopyTo(seekingArray);
            }

            land.ResetFrontier();
            CFRegion adjacentRegion = null;
            int iDir;

            // перебираем уже построенную часть континента
            foreach (CFRegion region in seekingArray)
            {
                // перебираем соседей
                for (iDir = 0; iDir < 3; iDir++)
                {
                    adjacentRegion = GetAdjacentRegion(region.X, region.Y, iDir);
                    if (adjacentRegion.X != region.X || adjacentRegion.Y != region.Y)
                    {
//                        if (adjacentRegion.Height < m_iOceanLevel)
                        if (adjacentRegion.ContinentID != land.Id || adjacentRegion.Height < m_iOceanLevel)
                        {
                            // шанс прямо пропорционален удалённости от других континентов
                            int chance = 0;

                            if (range > 0)
                            {
                                //                            chance = CountBoundsID(region.X, region.Y, contiID, range);
                                chance = CountOtherLandsInRange(adjacentRegion.X, adjacentRegion.Y, land.Id, range * 2);
                                chance = chance * chance;

                                if (chance > 0)
                                {
                                    if (CountOtherLandsInRange(adjacentRegion.X, adjacentRegion.Y, land.Id, 2) > 0)
                                        chance = -1;
                                }

                                //chance += Rnd.Get(range * 2);
                            }
                            land.AddFrontierRegion(region, adjacentRegion, chance);
                        }
                    }
                }
            }
        }

        private void CalculateCentralPoint(CLandMass land)
        {
            if (land.Regions.Count == 0)
                return;

            if (land.Border.Count == 0)
                CalculateShoreLine(land, 0);

            //List<CFRegion> body = new List<CFRegion>();
            //List<CFRegion> seekingArray = new List<CFRegion>();
            //List<CFRegion> seekingArray2 = new List<CFRegion>();

            //body.AddRange(land.Regions);
            //seekingArray.AddRange(land.Border);

            //CFRegion adjacentRegion = null;
            //int iDir;

            //do
            //{
            //    foreach (CFRegion region in seekingArray)
            //    {
            //        // перебираем соседей
            //        for (iDir = 0; iDir < 3; iDir++)
            //        {
            //            adjacentRegion = GetAdjacentRegion(region.X, region.Y, iDir);
            //            if (adjacentRegion.X != region.X || adjacentRegion.Y != region.Y)
            //            {
            //                if (body.Contains(adjacentRegion))
            //                {
            //                    if (!seekingArray2.Contains(adjacentRegion) && !seekingArray.Contains(adjacentRegion))
            //                        seekingArray2.Add(adjacentRegion);
            //                }
            //            }
            //        }
            //        body.Remove(region);

            //        if (body.Count == 1)
            //            break;
            //    }

            //    seekingArray.Clear();
            //    seekingArray.AddRange(seekingArray2);
            //    seekingArray2.Clear();
            //}
            //while (body.Count > 1);

            //land.X = body[0].X;
            //land.Y = body[0].Y;

            long sumX = 0;
            long sumY = 0;
            foreach (CFRegion region in land.Border)
            {
                sumX += region.X;
                sumY += region.Y;
            }
            land.X = (int)(sumX / land.Border.Count);
            land.Y = (int)(sumY / land.Border.Count);

            land.R = 0;
            foreach (CFRegion region in land.Border)
            {
                int iDistance = (int)GetDistance(land.X, land.Y, region.X, region.Y);
                if (iDistance > land.R)
                    land.R = iDistance;
            }
        }

        private int CountOtherLandsInRange(int x, int y, int myID, int iRange)
        {
            // если заданная дистанция не положительна, возвращаем 1
            if (iRange <= 0)
            {
                return 1;
            }

            int iCount = 0;
            foreach (CLandMass land in m_pLandMass)
            {
                if (land.Id == myID)
                    continue;

                if (GetDistance(x, y, land.X, land.Y) <= iRange + land.R)
                {
                    foreach (CFRegion region in land.Regions)//Border)
                    {
                        if (Math.Abs(region.Y - y) <= iRange)
                        {
                            if (GetDistance(x, y, region.X, region.Y) <= iRange)
                                iCount++;
                        }
                    }
                }
            }

            return iCount;
        }

        /// <summary>
        /// посчитать число соприкосновений целевого региона с другими континентами (кроме базового)
        /// </summary>
        /// <param name="x">x - координата целевого региона</param>
        /// <param name="y">y - координата целевого региона</param>
        /// <param name="contiID">идентификатор базового континента</param>
        /// <param name="iRange">радиус, в котором производить поиск</param>
        /// <returns> честно говоря, Х его уже З, что эта функция делает... :-/
        /// по идее, выдаёт некое число, и чем больше регионов, принадлежащих
        /// другим континентам, рядом - тем это число меньше.</returns>
        private int CountBoundsID(int x, int y, int contiID, int iRange)
        {
            // если точка уже принадлежит другому континенту, сразу возвращаем 0
            if (m_pRegions[x][y].Height != 0 && m_pRegions[x][y].ContinentID != contiID)
            {
                return 0;
            }

            // если заданная дистанция не положительна, возвращаем 1
            if (iRange <= 0)
            {
                return 1;
            }

            int iResult = 1 + iRange * 3;//общее число регионов в заданном радиусе
            int oldContiID = -1;

            // перебираем соседей
            int i;
            for (i = 0; i < 3; i++)
            {
                CFRegion region = GetAdjacentRegion(x, y, i);
                if (region.X != x || region.Y != x)
                {
                    //если регион уже принадлежит другому континенту
                    if (region.Height != 0 && region.ContinentID != contiID)
                    {
                        iResult -= iRange;
                        //не первый найденный другой континент
                        if (oldContiID != -1 && oldContiID != region.ContinentID)
                        {
                            iResult -= iRange;
                        }
                        else
                        {
                            oldContiID = region.ContinentID;
                        }
                    }
                    else
                    {
                        iResult -= 1 + (iRange - 1) * 3 - CountBoundsID(region.X, region.Y, contiID, iRange - 1);
                    }
                }
            }

            if (iResult < 0)
                iResult = 0;
            return iResult;
        }

        /// <summary>
        /// построить рельеф морского дна
        /// </summary>
        internal void BuildSeas()
        {
            int x, y;
            for (x = 0; x < m_iWidth; x++)
            {
                for (y = 0; y < m_iHeight; y++)
                {
                    if (m_pRegions[x][y].ContinentID == -1 && m_pRegions[x][y].Allowed)
                    {
                        // всё просто - чем дальше от ближайшей суши, тем глубже
//                        m_pRegions[x][y].Height = m_iOceanLevel - (1 + 100 * CountOtherLandsInRange(x, y, -1, 1) + Rnd.Get(100 * CountOtherLandsInRange(x, y, -1, 1))) * 16;
                        m_pRegions[x][y].Height = m_iOceanLevel - (1 + 100 * CountBoundsID(x, y, -1, 1) + Rnd.Get(100 * CountBoundsID(x, y, -1, 1))) * 16;
                        if (m_pRegions[x][y].Height < 1)
                            m_pRegions[x][y].Height = 1;
                    }
                }
            }
        }

        public void ClearLoneRegions(int iTreshold)
        { 
            int x, y;
            for (x = 0; x < m_iWidth; x++)
            {
                for (y = 0; y < m_iHeight; y++)
                {
                    if (m_pRegions[x][y].Allowed)
                    {
                        CFRegion surrounding = FindSurrounding(m_pRegions[x][y], iTreshold);
                        if (surrounding != null)
                        {
                            m_pRegions[x][y].Height = surrounding.Height;
                            m_pRegions[x][y].ContinentID = surrounding.ContinentID;
                        }
                    }
                }
            }
        }

        private CFRegion FindSurrounding(CFRegion start, int iSize)
        {
            List<CFRegion> founded = new List<CFRegion>();
            List<CFRegion> border = new List<CFRegion>();
            List<CFRegion> newBorder = new List<CFRegion>();
            founded.Add(start);
            border.Add(start);

            bool bLand = (start.Height > m_iOceanLevel);

            CFRegion surrounding = null;
            bool bSomethingNew = true;
            while (founded.Count < iSize && bSomethingNew)
            {
                bSomethingNew = false;
                foreach (CFRegion region in border)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        bool bLandToo = region.Exits[i].Height > m_iOceanLevel;
                        if (bLand == bLandToo && !founded.Contains(region.Exits[i]))
                        {
                            founded.Add(region.Exits[i]);
                            newBorder.Add(region.Exits[i]);
                            bSomethingNew = true;
                        }
                        else
                            surrounding = region.Exits[i];
                    }
                }
                border.Clear();
                border.AddRange(newBorder);
                newBorder.Clear();
            }

            return (founded.Count >= iSize) ? null : surrounding;
        }

        /// <summary>
        /// сгладить рельеф
        /// </summary>
        public void Smooth(int iTreshold)
        {
            int x, y;
            for (x = 0; x < m_iWidth; x++)
            {
                for (y = 0; y < m_iHeight; y++)
                {
                    CFRegion region = m_pRegions[x][y];
                    bool regionLand = (region.Height >= m_iOceanLevel);
                    if (region.Allowed)
                    {
                        CFRegion adjacentRegion;

                        int iDir;
                        iDir = Rnd.Get(3);

                        adjacentRegion = GetAdjacentRegion(x, y, iDir);
                        if (adjacentRegion.X != x || adjacentRegion.Y != y)
                        {
                            int h;
                            h = region.Height;

                            bool adjacentLand = (adjacentRegion.Height >= m_iOceanLevel);
                            
                            region.Height = (3 * region.Height + adjacentRegion.Height) / 4;
                            adjacentRegion.Height = (h + 3 * adjacentRegion.Height) / 4;
                            region.Height += Rnd.Get(800) - 400;
                            adjacentRegion.Height += Rnd.Get(800) - 400;

                            if (regionLand != (region.Height >= m_iOceanLevel) && FindSurrounding(region, iTreshold) == null)
                            {
                                if (regionLand)
                                    region.Height = m_iOceanLevel + 50;
                                else
                                    region.Height = m_iOceanLevel - 50;
                            }

                            if (adjacentLand != (adjacentRegion.Height >= m_iOceanLevel) && FindSurrounding(adjacentRegion, iTreshold) == null)
                            {
                                if (adjacentLand)
                                    adjacentRegion.Height = m_iOceanLevel + 50;
                                else
                                    adjacentRegion.Height = m_iOceanLevel - 50;
                            }

                            if (region.Height < 1)
                            {
                                region.Height = 1;
                            }
                            if (adjacentRegion.Height < 1)
                            {
                                adjacentRegion.Height = 1;
                            }
                        }
                    }
                }
            }
        }

        internal CParallel[] BuildAtlas()
        {
            int x, y;

            CParallel[] pAtlas = new CParallel[m_iHeight];

            for (y = 0; y < m_iHeight; y++)
            {
                double totalL = m_iWidth;

                double w;
                w = (double)(m_iHeight - 1) / 2 - y;
                if (Math.Abs(w) > m_iHeight / 6)
                {
                    totalL = (double)m_iWidth - 5.0 - (1.0 - ((int)(Math.Abs(w) + 0.5) - (double)m_iHeight / 6.0)) * ((double)m_iWidth - 10.0) / (1.0 - (double)m_iHeight / 3.0);
                }

                pAtlas[y] = new CParallel();
                pAtlas[y].Length = (int)totalL;
                pAtlas[y].MaxLatitude = (w + 0.5) * Math.PI / m_iHeight;
                pAtlas[y].MinLatitude = (w - 0.5) * Math.PI / m_iHeight;
                pAtlas[y].Regions = new CFRegion[pAtlas[y].Length];

                int k = 0;
                for (x = 0; x < m_iWidth; x++)
                {
                    if (m_pRegions[x][y].Allowed)
                    {
                        m_pRegions[x][y].Sector = k;
                        m_pRegions[x][y].Parallel = y;

                        pAtlas[y].Regions[k] = m_pRegions[x][y];
                        k++;
                    }
                }
            }

            return pAtlas;
        }

        internal void FixExits()
        {
            int x, y;
            for (y = 0; y < m_iHeight; y++)
            {
                for (x = 0; x < m_iWidth; x++)
                {
                    if (m_pRegions[x][y].Allowed)
                    {
                        // перебираем всех соседей
                        for (int i = 0; i < 3; i++)
                        {
                            CFRegion region = GetAdjacentRegion(x, y, i);

                            if (region.X != x || region.Y != y)
                            {
                                m_pRegions[x][y].Exits[i] = region;
                            }
                        }
                    }
                }
            }
        }

        private int CalculateCurrentWaterPercent()
        {
            int iTotalRegions = 0;
            int iUnderwaterRegions = 0;

            int x, y;
            for (y = 0; y < m_iHeight; y++)
            {
                for (x = 0; x < m_iWidth; x++)
                {
                    if (m_pRegions[x][y].Allowed)
                    {
                        iTotalRegions++;
                        if (m_pRegions[x][y].Height < m_iOceanLevel)
                            iUnderwaterRegions++;
                    }
                }
            }

            return (int)(100 * iUnderwaterRegions / iTotalRegions);
        }

        internal void BalanceWater(int iNeedWaterPercent)
        {
            if (m_pRegions == null)
            {
                return;
            }

            int iCurrentWaterPercent = CalculateCurrentWaterPercent();

            if (iCurrentWaterPercent > iNeedWaterPercent)
            {
                while (iCurrentWaterPercent > iNeedWaterPercent)
                {
                    m_iOceanLevel -= 10;
                    iCurrentWaterPercent = CalculateCurrentWaterPercent();
                }
                while (iCurrentWaterPercent < iNeedWaterPercent)
                {
                    m_iOceanLevel++;
                    iCurrentWaterPercent = CalculateCurrentWaterPercent();
                }
                m_iOceanLevel--;
            }
            else
            {
                while (iCurrentWaterPercent < iNeedWaterPercent)
                {
                    m_iOceanLevel += 10;
                    iCurrentWaterPercent = CalculateCurrentWaterPercent();
                }
                while (iCurrentWaterPercent > iNeedWaterPercent)
                {
                    m_iOceanLevel--;
                    iCurrentWaterPercent = CalculateCurrentWaterPercent();
                }
            }

            while (iCurrentWaterPercent != iNeedWaterPercent)
            {
                int x, y;
                x = Rnd.Get(m_iWidth);
                y = Rnd.Get(m_iHeight);

                if (iCurrentWaterPercent > iNeedWaterPercent)
                {
                    if (m_pRegions[x][y].Height < m_iOceanLevel)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (m_pRegions[x][y].Exits[i].Height >= m_iOceanLevel)
                            {
                                m_pRegions[x][y].Height = m_pRegions[x][y].Exits[i].Height;
                                iCurrentWaterPercent = CalculateCurrentWaterPercent();
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (m_pRegions[x][y].Height >= m_iOceanLevel)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (m_pRegions[x][y].Exits[i].Height < m_iOceanLevel)
                            {
                                m_pRegions[x][y].Height = m_pRegions[x][y].Exits[i].Height;
                                iCurrentWaterPercent = CalculateCurrentWaterPercent();
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// дистанция между регионами
        /// </summary>
        /// <param name="x1">х - координата первого региона</param>
        /// <param name="y1">y - координата первого региона</param>
        /// <param name="x2">x - координата второго региона</param>
        /// <param name="y2">y - координата второго региона</param>
        /// <returns>расстояние</returns>
        public double GetDistance(int x1, int y1, int x2, int y2)
        {
            double fLong1 = m_pRegions[x1][y1].Longitude;
            double fLat1 = m_pRegions[x1][y1].Latitude;
            double fLong2 = m_pRegions[x2][y2].Longitude;
            double fLat2 = m_pRegions[x2][y2].Latitude;

            // формула из "Справочника авиационного штурмана"
            double fCos = Math.Sin(fLat1) * Math.Sin(fLat2) + Math.Cos(fLat1) * Math.Cos(fLat2) * Math.Cos(fLong2 - fLong1);

            double fAngle = Math.Acos(fCos);

            return fAngle * m_iWidth / (2 * Math.PI);

            /*
            xx1 = Math.Cos(reg1.Longitude);
            yy1 = Math.Sin(reg1.Longitude);

            xx1 = xx1 * Math.Cos(reg1.Latitude);
            yy1 = yy1 * Math.Cos(reg1.Latitude);

            zz1 = Math.Sin(Math.Abs(reg1.Latitude));
            if (reg1.Latitude < 0)
                zz1 = -zz1;

            double xx2, yy2, zz2;

            xx2 = Math.Cos(reg2.Longitude);
            yy2 = Math.Sin(reg2.Longitude);

            xx2 = xx2 * Math.Cos(reg2.Latitude);
            yy2 = yy2 * Math.Cos(reg2.Latitude);

            zz2 = Math.Sin(Math.Abs(reg2.Latitude));
            if (reg2.Latitude < 0)
                zz2 = -zz2;

            double dist;
            dist = Math.Sqrt((xx2 - xx1) * (xx2 - xx1) + (yy2 - yy1) * (yy2 - yy1) + (zz2 - zz1) * (zz2 - zz1));
            
            return dist * m_iWidth / (2 * Math.PI);
            */
        }
    }
}
