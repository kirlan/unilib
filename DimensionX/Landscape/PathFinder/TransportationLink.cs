using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace LandscapeGeneration.PathFind
{
    /// <summary>
    /// None, Country, Normal, Good
    /// </summary>
    public enum RoadQuality
    {
        /// <summary>
        /// Дороги нет
        /// </summary>
        None,
        /// <summary>
        /// Просёлочная деревенская дорога, фактически просто две колеи
        /// </summary>
        Country,
        /// <summary>
        /// Обычная дорога
        /// </summary>
        Normal,
        /// <summary>
        /// Очень хорошая дорога, уровня имперской римской дороги или скоростного шоссе
        /// </summary>
        Good
    }

    //Вообще надо это делать отдельным классом, ИМХО
    public enum Transport
    {
        /// <summary>
        /// Пешком. 3,5 км/ч, 8 часов идти, итого ~25 км. в день.
        /// </summary>
        Afoot,
        /// <summary>
        /// Верхом на лошади. 8 км/ч, 8 часов = ~60 км. в день.
        /// </summary>
        Horse,
        /// <summary>
        /// Дилижанс. 5 км/ч, 16 часов = ~80 км. в сутки
        /// </summary>
        Stagecoach,
        /// <summary>
        /// На машине начала XX века. 20 км/ч, 8 часов = 160 км. в день
        /// </summary>
        RetroCar,
        /// <summary>
        /// На современной машине. 75 км/ч (в среднем), 8 часов = 600 км. в день
        /// </summary>
        ModernCar,
        /// <summary>
        /// По железной дороге начала XX века. 60 км/ч, 24 часа = ~1500 км. в сутки
        /// </summary>
        SteamRailroad,
        /// <summary>
        /// По современной железной дороге. 180 км/ч, 24 часа = ~4500 км. в сутки
        /// </summary>
        ElectricRailroad,
        /// <summary>
        /// Классический дирижабль начала XX века. 80км/ч, 24 часа = ~2000 км. в сутки
        /// </summary>
        Zeppeline,
        /// <summary>
        /// Винтовой самолёт. 500 км/ч, 10 часов (дольше пассажирские рейсы без посадки не летают) = 5000 км. в сутки
        /// </summary>
        Propellerplane,
        /// <summary>
        /// Вертолёт. 250 км/ч, 3 часа максимальный беспосадочный перелёт = 750 км. в сутки (?)
        /// </summary>
        Helicopter,
        /// <summary>
        /// Реактивный самолёт. 800 км/ч, 10 часов (дольше пассажирские рейсы без посадки не летают) = 8000 км. в сутки
        /// </summary>
        Jetplane,
        /// <summary>
        /// Летающий автомобиль. 200 км/ч, 8 часов = 1600 км. в сутки
        /// </summary>
        Hovercar,
        /// <summary>
        /// Пассажирская баллистическая ракета или гиперзвуковой лайнер. 4000 км/ч, 4 часа = 16000 км. в сутки
        /// </summary>
        Ballistic,
        /// <summary>
        /// Телепорт. Даже не знаю, что сказать о скорости... 20000 км/ч? В общем, в любую точку планеты меньше чем за 1 час.
        /// </summary>
        Teleport,
        /// <summary>
        /// Весельное судно типа триремы. 50 км. в сутки.
        /// </summary>
        Rowboat,
        /// <summary>
        /// Прибрежное парусное судно типа ладьи или драккара. 120 км. в сутки
        /// </summary>
        Sailer,
        /// <summary>
        /// Океанское парусное судно, так же первые пароходы. 30 км/ч, 24 часа = ~600 км. в сутки
        /// </summary>
        Oceanship,
        /// <summary>
        /// Современный океанский лайнер. 60 км/ч, 24 часа = ~1500 км. в сутки
        /// </summary>
        Motorship,
        /// <summary>
        /// Судно на воздушной подушке. 110 км/ч, 24 часа = ~2500 км. в сутки
        /// </summary>
        Hovercraft
    }

    /// <summary>
    /// Информация о связях между локациями - базовая версия, не поддерживает транспортные маршруты и выбор средства передвижения
    /// </summary>
    public class TransportationLinkBase
    {
        private readonly float m_fBaseCost;

        /// <summary>
        /// Уровень построенной дороги. Используется только при отрисовке карты.
        /// </summary>
        public RoadQuality RoadLevel { get; private set; } = RoadQuality.None;

        /// <summary>
        /// Построить дорогу на этом участке
        /// </summary>
        /// <param name="eLevel">Уровень дороги: 1 - просёлок, 2 - обычная дорога, 3 - имперская дорога</param>
        public void BuildRoad(RoadQuality eLevel)
        {
            if (eLevel <= RoadLevel)
                return;

            RoadLevel = eLevel;

            RecalcFinalCost();
        }

        public void ClearRoad()
        {
            RoadLevel = 0;
            RecalcFinalCost();
        }

        private void RecalcFinalCost()
        {
            if (m_bEmbark)
            {
                const float fEmbarkCost = 20000;
                switch (RoadLevel)
                {
                    case RoadQuality.None:
                        MovementCost = m_fBaseCost + fEmbarkCost;
                        break;
                    case RoadQuality.Country:
                        MovementCost = (m_fBaseCost + fEmbarkCost) * 0.8f;
                        break;
                    case RoadQuality.Normal:
                        MovementCost = (m_fBaseCost + fEmbarkCost) * 0.5f;
                        break;
                    case RoadQuality.Good:
                        MovementCost = (m_fBaseCost + fEmbarkCost) * 0.2f;
                        break;
                }
            }
            else if (m_bSea)
            {
                MovementCost = RoadLevel > 0 ? m_fBaseCost * 0.8f : m_fBaseCost;
            }
            else
            {
                switch (RoadLevel)
                {
                    case RoadQuality.None:
                        MovementCost = m_fBaseCost;
                        break;
                    case RoadQuality.Country:
                        MovementCost = m_fBaseCost * 0.8f;
                        break;
                    case RoadQuality.Normal:
                        MovementCost = m_fBaseCost * 0.5f;
                        break;
                    case RoadQuality.Good:
                        MovementCost = m_fBaseCost * 0.2f;
                        break;
                }
            }

            if (m_bRuins)
                MovementCost *= 10;
        }

        public float MovementCost { get; private set; }

        /// <summary>
        /// Дорога перекрыта. Нужно ТОЛЬКО для постройки внутригосударственных дорог.
        /// </summary>
        public bool IsClosed { get; set; } = false;

        private bool m_bSea = false;

        public bool Sea
        {
            get { return m_bSea; }
            set
            {
                m_bSea = value;
                RecalcFinalCost();
            }
        }

        private bool m_bEmbark = false;

        public bool Embark
        {
            get { return m_bEmbark; }
            set
            {
                m_bEmbark = value;
                RecalcFinalCost();
            }
        }

        private bool m_bRuins = false;

        public bool Ruins
        {
            get { return m_bRuins; }
            set
            {
                m_bRuins = value;
                RecalcFinalCost();
            }
        }

        public VoronoiVertex[] Points { get; } = new VoronoiVertex[3];

        private static float GetDist(VoronoiVertex pPoint1, VoronoiVertex pPoint2)
        {
            return (float)Math.Sqrt((pPoint1.X - pPoint2.X) * (pPoint1.X - pPoint2.X) + (pPoint1.Y - pPoint2.Y) * (pPoint1.Y - pPoint2.Y));
        }

        public TransportationLinkBase(Location pLoc1, Location pLoc2, float fCycleShift)
        {
            Points[0] = pLoc1;
            Points[2] = pLoc2;

            VoronoiVertex point2 = new VoronoiVertex(pLoc2.X, pLoc2.Y);
            if (Math.Abs(pLoc1.X - pLoc2.X) > fCycleShift / 2)
            {
                if (pLoc1.X < 0)
                    point2.X -= fCycleShift;
                else
                    point2.X += fCycleShift;
            }

            VoronoiEdge pLine = pLoc1.BorderWith[pLoc2][0];

            Points[1] = new VoronoiVertex((pLine.Point1.X + pLine.Point2.X) / 2, (pLine.Point1.Y + pLine.Point2.Y) / 2);

            float fDist1 = GetDist(Points[0], Points[1]);
            float fDist2 = GetDist(point2, Points[1]);

            float fDist11 = GetDist(Points[0], pLine.Point1);
            float fDist21 = GetDist(point2, pLine.Point1);

            float fDist12 = GetDist(Points[0], pLine.Point2);
            float fDist22 = GetDist(point2, pLine.Point2);

            if (fDist1 + fDist2 < fDist11 + fDist21 &&
                fDist1 + fDist2 < fDist12 + fDist22)
            {
                Points[1].X = (Points[0].X + point2.X) / 2;
                Points[1].Y = (Points[0].Y + point2.Y) / 2;
            }
            else
            {
                if (fDist11 + fDist21 < fDist12 + fDist22)
                {
                    Points[1].X = (Points[1].X + 2 * pLine.Point1.X) / 3;
                    Points[1].Y = (Points[1].Y + 2 * pLine.Point1.Y) / 3;
                }
                else
                {
                    Points[1].X = (Points[1].X + 2 * pLine.Point2.X) / 3;
                    Points[1].Y = (Points[1].Y + 2 * pLine.Point2.Y) / 3;
                }
            }

            float fDist1final = GetDist(Points[0], Points[1]);
            float fDist2final = GetDist(point2, Points[1]);

            m_fBaseCost = fDist1final * pLoc1.GetMovementCost() + fDist2final * pLoc2.GetMovementCost();
            RecalcFinalCost();
        }

        public TransportationLinkBase(Land pLand1, Land pLand2, float fCycleShift)
        {
            Points[0] = pLand1;
            Points[2] = pLand2;

            VoronoiVertex point2 = new VoronoiVertex(pLand2.X, pLand2.Y);
            if (Math.Abs(pLand1.X - pLand2.X) > fCycleShift / 2)
            {
                if (pLand1.X < 0)
                    point2.X -= fCycleShift;
                else
                    point2.X += fCycleShift;
            }

            float fMinDist = float.MaxValue;
            VoronoiEdge[] cLines = pLand1.BorderWith[pLand2].ToArray();
            VoronoiEdge pBestLine = cLines[0];
            foreach (var pLine in cLines)
            {
                Points[1] = new VoronoiVertex((pLine.Point1.X + pLine.Point2.X) / 2, (pLine.Point1.Y + pLine.Point2.Y) / 2);

                float fDist1 = GetDist(Points[0], Points[1]);
                float fDist2 = GetDist(point2, Points[1]);

                if (fDist1 + fDist2 < fMinDist)
                {
                    fMinDist = fDist1 + fDist2;
                    pBestLine = pLine;
                }
            }
            Points[1] = new VoronoiVertex((pBestLine.Point1.X + pBestLine.Point2.X) / 2, (pBestLine.Point1.Y + pBestLine.Point2.Y) / 2);

            float fDist1final = GetDist(Points[0], Points[1]);
            float fDist2final = GetDist(point2, Points[1]);

            m_fBaseCost = fDist1final * pLand1.GetMovementCost() + fDist2final * pLand2.GetMovementCost();
            RecalcFinalCost();
        }

        public TransportationLinkBase(LandMass pLandMass1, LandMass pLandMass2, float fCycleShift)
        {
            Points[0] = pLandMass1;
            Points[2] = pLandMass2;

            VoronoiVertex point2 = new VoronoiVertex(pLandMass2.X, pLandMass2.Y);
            if (Math.Abs(pLandMass1.X - pLandMass2.X) > fCycleShift / 2)
            {
                if (pLandMass1.X < 0)
                    point2.X -= fCycleShift;
                else
                    point2.X += fCycleShift;
            }

            float fShortest = float.MaxValue;
            VoronoiEdge[] cLines = pLandMass1.BorderWith[pLandMass2].ToArray();
            VoronoiEdge pBestLine = cLines[0];
            foreach (var pLine in cLines)
            {
                Points[1] = new VoronoiVertex((pLine.Point1.X + pLine.Point2.X) / 2, (pLine.Point1.Y + pLine.Point2.Y) / 2);

                float fDist1 = GetDist(Points[0], Points[1]);
                float fDist2 = GetDist(point2, Points[1]);

                if (fDist1 + fDist2 < fShortest)
                {
                    fShortest = fDist1 + fDist2;
                    pBestLine = pLine;
                }
            }
            Points[1] = new VoronoiVertex((pBestLine.Point1.X + pBestLine.Point2.X) / 2, (pBestLine.Point1.Y + pBestLine.Point2.Y) / 2);

            float fDist1final = GetDist(Points[0], Points[1]);
            float fDist2final = GetDist(point2, Points[1]);

            m_fBaseCost = fDist1final * pLandMass1.GetMovementCost() + fDist2final * pLandMass2.GetMovementCost();
            RecalcFinalCost();
        }

        public TransportationLinkBase(TransportationNode[] aPath)
        {
            List<VoronoiVertex> cPoints = new List<VoronoiVertex> { aPath[0] };

            m_fBaseCost = 0;

            TransportationNode pLastNode = null;
            foreach (TransportationNode pNode in aPath)
            {
                if (pLastNode != null)
                {
                    TransportationLinkBase pLink = pLastNode.Links[pNode];
                    if (pLink.Points[0] == pLastNode)
                    {
                        for (int i = 1; i < pLink.Points.Length; i++)
                            cPoints.Add(pLink.Points[i]);
                    }
                    else
                    {
                        for (int i = pLink.Points.Length - 2; i >= 0; i--)
                            cPoints.Add(pLink.Points[i]);
                    }

                    m_fBaseCost += pLink.m_fBaseCost;
                }

                pLastNode = pNode;
            }

            Points = cPoints.ToArray();
            RecalcFinalCost();
        }
    }
}
