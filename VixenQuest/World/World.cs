using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using NameGen;
using System.Drawing;
using BenTools.Mathematics;

namespace VixenQuest.World
{
    public class World : Space
    {
        private static string[] m_aPlace = 
        {
            "Realm",
            "Plane",
            "Dimension",
            "World",
        };

        public Universe m_pUniverse;

        public Land[] m_aLands = null; 

        private const int m_iWorldScale = 150;

        public int WorldScale
        {
            get { return m_iWorldScale; }
        }

        public class LandPtr
        { 
            public Land m_pLand = null;
        }

        private int m_iLocationsCount = 150;
        
        private void GenerateMap()
        {
            m_iLocationsCount = (int)(Math.Pow(m_iWorldScale, 2) * Math.PI / 1000);

            bool bOK = true;
            do
            {
                BuildRandomGrid();
                bOK = CalculateVoronoi();
            }
            while (!bOK);

            //для всех ячеек связываем разрозненные рёбра в замкнутую ломаную границу
            foreach (Land pLand in m_aLands)
            {
                pLand.BuildBorder(m_iWorldScale * 2);
                pLand.CorrectCenter();
            }
        }

        public Vertex[] m_aVertexes = null;

        private bool CalculateVoronoi()
        {
            Dictionary<BTVector, Land> cData = new Dictionary<BTVector, Land>();
            foreach (Land pLoc in m_aLands)
                cData[new BTVector(pLoc.m_pCenter.X, pLoc.m_pCenter.Y)] = pLoc;

            //Строим диаграмму вороного - определяем границы локаций
            VoronoiGraph graph = Fortune.ComputeVoronoiGraph(cData.Keys);
            Dictionary<BTVector, Vertex> cVertexes = new Dictionary<BTVector, Vertex>();

            //Переводим данные из диаграммы Вороного в наш формат
            try
            {
                foreach (VoronoiEdge pEdge in graph.Edges)
                    AddEdge(cData, cVertexes, pEdge);
            }
            catch (Exception ex)
            {
                //бывает, алгоритм выдаёт данные, которые мы не можем корректно перевести (нулевые рёбра, etc.)
                //в этом случае всё приходится начинать заново
                return false;
            }

            m_aVertexes = new List<Vertex>(cVertexes.Values).ToArray();

            foreach (Land pLoc in m_aLands)
                pLoc.FillBorderWithKeys();

            return true;
        }

        private bool AddEdge(Dictionary<BTVector, Land> cData, Dictionary<BTVector, Vertex> cVertexes, VoronoiEdge pEdge)
        {
            Land pLoc1 = null;
            Land pLoc2 = null;
            if (cData.ContainsKey(pEdge.LeftData))
                pLoc1 = cData[pEdge.LeftData];
            if (cData.ContainsKey(pEdge.RightData))
                pLoc2 = cData[pEdge.RightData];

            if (pLoc1.m_pOrigin != null && pLoc2.m_pOrigin != null)
                return false;

            if (pEdge.VVertexA == Fortune.VVUnkown || pEdge.VVertexB == Fortune.VVUnkown)
            {
                pLoc1.m_bUnclosed = true;
                pLoc2.m_bUnclosed = true;
                return false;
            }
            if (pEdge.VVertexA.data[0] < -m_iWorldScale * 2 ||
                pEdge.VVertexA.data[0] > m_iWorldScale * 2 ||
                pEdge.VVertexA.data[1] < -m_iWorldScale * 2 ||
                pEdge.VVertexA.data[1] > m_iWorldScale * 2 ||
                pEdge.VVertexB.data[0] < -m_iWorldScale * 2 ||
                pEdge.VVertexB.data[0] > m_iWorldScale * 2 ||
                pEdge.VVertexB.data[1] < -m_iWorldScale * 2 ||
                pEdge.VVertexB.data[1] > m_iWorldScale * 2)
            {
                pLoc1.m_bUnclosed = true;
                pLoc2.m_bUnclosed = true;
                return false;
            }

            if (!cVertexes.ContainsKey(pEdge.VVertexA))
                cVertexes[pEdge.VVertexA] = new Vertex(pEdge.VVertexA);

            Vertex pVertexA = cVertexes[pEdge.VVertexA];

            if (!cVertexes.ContainsKey(pEdge.VVertexB))
                cVertexes[pEdge.VVertexB] = new Vertex(pEdge.VVertexB);

            Vertex pVertexB = cVertexes[pEdge.VVertexB];

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
                Vertex pVertexC = pVertexA;
                pVertexA = pVertexB;
                pVertexB = pVertexC;
            }

            if (!pVertexA.m_cVertexes.Contains(pVertexB))
                pVertexA.m_cVertexes.Add(pVertexB);
            if (!pVertexB.m_cVertexes.Contains(pVertexA))
                pVertexB.m_cVertexes.Add(pVertexA);

            if (pLoc1 != null && pLoc1.m_pOrigin == null)
            {
                if (!pVertexA.m_cLocations.Contains(pLoc1))
                    pVertexA.m_cLocations.Add(pLoc1);
                if (!pVertexB.m_cLocations.Contains(pLoc1))
                    pVertexB.m_cLocations.Add(pLoc1);

                if (pLoc2 != null)
                {
                    Line pLine = new Line(pVertexA, pVertexB);
                    if (pLine.m_fLength > 0)
                    {
                        foreach (List<Line> cLines in pLoc1.BorderWith.Values)
                            if (cLines[0].m_pPoint1 == pVertexA)
                                throw new Exception("Wrong edge!");
                        //if(!bTwin)
                        if (pLoc2.m_pOrigin == null)
                        {
                            pLoc1.BorderWith[pLoc2] = new List<Line>();
                            pLoc1.BorderWith[pLoc2].Add(pLine);
                        }
                        else
                        {
                            pLoc1.BorderWith[pLoc2.m_pOrigin] = new List<Line>();
                            pLoc1.BorderWith[pLoc2.m_pOrigin].Add(pLine);
                        }
                        //else
                        //    pLoc1.m_cBorderWith[pLoc2] = new Line(pVertexB, pVertexA);
                    }
                }
            }

            if (pLoc2 != null && pLoc2.m_pOrigin == null)
            {
                if (!pVertexA.m_cLocations.Contains(pLoc2))
                    pVertexA.m_cLocations.Add(pLoc2);
                if (!pVertexB.m_cLocations.Contains(pLoc2))
                    pVertexB.m_cLocations.Add(pLoc2);

                if (pLoc1 != null)
                {
                    Line pLine = new Line(pVertexB, pVertexA);
                    if (pLine.m_fLength > 0)
                    {
                        foreach (List<Line> cLines in pLoc2.m_cBorderWith.Values)
                            if (cLines[0].m_pPoint1 == pVertexB)
                                throw new Exception("Wrong edge!");
                        //if (!bTwin)
                        if (pLoc1.m_pOrigin == null)
                        {
                            pLoc2.BorderWith[pLoc1] = new List<Line>();
                            pLoc2.BorderWith[pLoc1].Add(pLine);
                        }
                        else
                        {
                            pLoc2.BorderWith[pLoc1.m_pOrigin] = new List<Line>();
                            pLoc2.BorderWith[pLoc1.m_pOrigin].Add(pLine);
                        }
                        //else
                        //    pLoc2.m_cBorderWith[pLoc1] = new Line(pVertexA, pVertexB);
                    }
                }
            }

            return true;
        }

        private void BuildRandomGrid()
        {
            List<Land> cLocations = new List<Land>();

            float kr = (int)(Math.Sqrt((float)m_iLocationsCount));

            float dkr = m_iWorldScale / (kr * 2);

            //Создаём периметр карты из "запретных" локаций, для того чтобы получить ровную кромку.
            for (int ii = 0; ii <= kr; ii++)
            {
                int xx = (int)(ii * 2 * m_iWorldScale / kr);

                Land pLocation11 = new Land(this);
                pLocation11.Create(cLocations.Count, m_iWorldScale - xx, m_iWorldScale - dkr);
                pLocation11.m_bBorder = xx == 0 || xx == 2 * m_iWorldScale;//false;
                cLocations.Add(pLocation11);

                Land pLocation12 = new Land(this);
                pLocation12.Create(cLocations.Count, m_iWorldScale - xx, m_iWorldScale + dkr);
                pLocation12.m_bBorder = true;
                cLocations.Add(pLocation12);

                Land pLocation21 = new Land(this);
                pLocation21.Create(cLocations.Count, m_iWorldScale - xx, -m_iWorldScale - dkr);
                pLocation21.m_bBorder = true;
                cLocations.Add(pLocation21);

                Land pLocation22 = new Land(this);
                pLocation22.Create(cLocations.Count, m_iWorldScale - xx, -m_iWorldScale + dkr);
                pLocation22.m_bBorder = xx == 0 || xx == 2 * m_iWorldScale;//false;
                cLocations.Add(pLocation22);
            }

            for (int jj = 0; jj <= kr; jj++)
            {
                int yy = (int)(jj * 2 * m_iWorldScale / kr);

                Land pLocation11 = new Land(this);
                pLocation11.Create(cLocations.Count, m_iWorldScale - dkr, m_iWorldScale - yy);
                pLocation11.m_bBorder = yy == 0 || yy == 2 * m_iWorldScale;//false;
                cLocations.Add(pLocation11);

                Land pLocation12 = new Land(this);
                pLocation12.Create(cLocations.Count, m_iWorldScale + dkr, m_iWorldScale - yy);
                pLocation12.m_bBorder = true;
                cLocations.Add(pLocation12);

                Land pLocation21 = new Land(this);
                pLocation21.Create(cLocations.Count, -m_iWorldScale - dkr, m_iWorldScale - yy);
                pLocation21.m_bBorder = true;
                cLocations.Add(pLocation21);

                Land pLocation22 = new Land(this);
                pLocation22.Create(cLocations.Count, -m_iWorldScale + dkr, m_iWorldScale - yy);
                pLocation22.m_bBorder = yy == 0 || yy == 2 * m_iWorldScale;//false;
                cLocations.Add(pLocation22);
            }

            //Добавляем центры остальных локаций в случайные позиции внутри периметра.
            for (int i = 0; i < m_iLocationsCount; i++)
            {
                Land pLocation = new Land(this);
                pLocation.Create(cLocations.Count, m_iWorldScale - dkr * 2 - Rnd.Get(m_iWorldScale * 2 - 4 * dkr), m_iWorldScale - dkr * 2 - Rnd.Get(m_iWorldScale * 2 - 4 * dkr));
                cLocations.Add(pLocation);
            }

            m_aLands = cLocations.ToArray();
        }

        private void PopulateMap()
        {
            foreach (State pState in m_cStates)
            {
                int iLoc = -1;
                do
                {
                    iLoc = Rnd.Get(m_aLands.Length);
                }
                while (m_aLands[iLoc].m_pState != null || m_aLands[iLoc].Type == LandType.Forbidden);

                m_aLands[iLoc].Assign(pState, true);
            }

            bool bExpanding = false;
            do
            {
                bExpanding = false;
                foreach (Land pLand in m_aLands)
                {
                    if (pLand.m_pState != null)
                    {
                        List<Land> cPossibleDirections = new List<Land>();
                        foreach (Land pLink in pLand.m_aBorderWith)
                        {
                            if (pLink.m_pState == null && pLink.Type != LandType.Forbidden)
                                cPossibleDirections.Add(pLink);
                        }
                        if (cPossibleDirections.Count > 0)
                        {
                            int iLoc = Rnd.Get(cPossibleDirections.Count);
                            cPossibleDirections[iLoc].Assign(pLand.m_pState, false);
                            bExpanding = true;
                        }
                    }
                }
            }
            while (bExpanding);
        }

        public List<Race> m_cLocalRaces = new List<Race>();
        public List<Race> m_cLocalAnimals = new List<Race>();

        public List<State> m_cStates = new List<State>();

        private World()
            : base()
        {
        }

        /// <summary>
        /// Заказной мир с единственной локацией
        /// </summary>
        /// <param name="sName">имя локации</param>
        /// <param name="pUniverse">ссылка на вселенную</param>
        public World(string sName, Universe pUniverse) :
            this()
        {
            m_pUniverse = pUniverse;

            m_sName = sName;

            m_cStates.Add(new State(this, sName));

            Land pLand = new Land(this, sName);
            //pLand.Assign(m_cStates[0], false);
            m_aLands = new Land[1];
            m_aLands[0] = pLand;
        }

        /// <summary>
        /// Обычный мир
        /// </summary>
        /// <param name="iTier">тир мира</param>
        /// <param name="pUniverse">ссылка на вселенную</param>
        public World(int iTier, Universe pUniverse) :
            this()
        {
            m_iTier = iTier;

            m_pUniverse = pUniverse;

            int variant = Rnd.Get(2);

            switch (variant)
            {
                case 0:
                    {
                        int iEpithet = Rnd.Get(m_aEpithet.Length);
                        m_sName = m_aEpithet[iEpithet];

                        int iPlace = Rnd.Get(m_aPlace.Length);
                        m_sName += m_aPlace[iPlace];
                    }
                    break;
                case 1:
                    {
                        int iPlace = Rnd.Get(m_aPlace.Length);
                        m_sName = m_aPlace[iPlace];

                        int iDescription = Rnd.Get(m_aDescription.Length);
                        m_sName += m_aDescription[iDescription];
                    }
                    break;
                default:
                    {
                        int iEpithet = Rnd.Get(m_aEpithet.Length);
                        m_sName = m_aEpithet[iEpithet];

                        int iPlace = Rnd.Get(m_aPlace.Length);
                        m_sName += m_aPlace[iPlace];

                        int iDescription = Rnd.Get(m_aDescription.Length);
                        m_sName += m_aDescription[iDescription];
                    }
                    break;
            }

            m_sName = NameGenerator.GetAbstractName() + ", " + m_sName;

            //m_sName += " (T" + m_iTier.ToString();// +")";

            int iDiversity = 4 + Rnd.Get(4);
            for (int i = 0; i < iDiversity; i++)
            {
                Race pLocalAnimal;
                do
                {
                    pLocalAnimal = GetRandomRace(Sapience.Animal);
                }
                while (m_cLocalAnimals.Contains(pLocalAnimal));

                m_cLocalAnimals.Add(pLocalAnimal);
                //m_sName += ", " + pLocalAnimals.ToString();
            }

            iDiversity = 4 + Rnd.Get(4);
            for (int i = 0; i < iDiversity; i++)
            {
                Race pNewRace;
                do
                {
                    pNewRace = GetRandomRace(Sapience.Human);
                }
                while (m_cLocalRaces.Contains(pNewRace));
                    
                m_cLocalRaces.Add(pNewRace);
                //m_sName += ", " + pNewRace.ToString();
            }
            //m_sName += ")";

            GenerateMap();

            iDiversity = 4 + Rnd.Get(4);
            for (int i = 0; i < iDiversity; i++)
            {
                State pNewState = new State(this);
                m_cStates.Add(pNewState);
            }

            PopulateMap();
        }

        private Race GetRandomRace(Sapience eSapience)
        {
            Race pPretendent;

            Race pBestPretendent = null;
            int iBestDifference = int.MaxValue;

            int iCounter = 0;
            do
            {
                int index = Rnd.Get(m_aAllRaces.Length);
                pPretendent = m_aAllRaces[index];

                int iDifference = Math.Abs(pPretendent.m_iRank - (m_iTier - 1) * 10);

                if (pPretendent.m_iRank < m_iTier * 10)
                    iDifference = 2 * iDifference;

                if (pPretendent.m_eSapience != eSapience)
                    iDifference = int.MaxValue;

                if (m_cLocalRaces.Contains(pPretendent))
                    iDifference = int.MaxValue;

                if (pBestPretendent == null || iBestDifference > iDifference)
                {
                    pBestPretendent = pPretendent;
                    iBestDifference = iDifference;
                }

                if (Rnd.Gauss(iDifference, 3))
                    return pPretendent;

                if (iCounter++ > 100)
                    return pBestPretendent;
            }
            while (true);
        }

        List<Opponent> m_cAvailableOpponents = new List<Opponent>();

        public override Opponent[] AvailableOpponents()
        {
            if (m_cAvailableOpponents.Count == 0)
                foreach (Land land in m_aLands)
                    m_cAvailableOpponents.AddRange(land.AvailableOpponents());

            return m_cAvailableOpponents.ToArray();
        }
    }
}
