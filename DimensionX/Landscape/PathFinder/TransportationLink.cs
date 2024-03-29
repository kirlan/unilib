﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using LandscapeGeneration.PlanetBuilder;

namespace LandscapeGeneration.PathFind
{
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
        private float m_fBaseCost;

        private float m_fFinalCost;

        //private float m_fMoveCostModifer = 1;

        //public float MoveCostModifer
        //{
        //    get { return m_fMoveCostModifer; }
        //}

        private RoadQuality m_eRoadLevel = RoadQuality.None;

        /// <summary>
        /// Уровень построенной дороги. Используется только при отрисовке карты.
        /// </summary>
        public RoadQuality RoadLevel
        {
            get 
            {
                return m_eRoadLevel;
                //switch (m_eRoadLevel)
                //{ 
                //    case 1:
                //        return RoadQuality.Country;
                //    case 2:
                //        return RoadQuality.Normal;
                //    case 3:
                //        return RoadQuality.Good;
                //    default:
                //        return RoadQuality.None;
                //}
            }
        }

        /// <summary>
        /// Построить дорогу на этом участке
        /// </summary>
        /// <param name="eLevel">Уровень дороги: 1 - просёлок, 2 - обычная дорога, 3 - имперская дорога</param>
        public void BuildRoad(RoadQuality eLevel)
        {
            if (eLevel <= m_eRoadLevel)
                return;

            m_eRoadLevel = eLevel;
            //switch (iLevel)
            //{
            //    case 1:
            //        m_fMoveCostModifer = 0.25f;
            //        break;
            //    case 2:
            //        m_fMoveCostModifer = 0.10f;
            //        break;
            //    case 3:
            //        m_fMoveCostModifer = 0.01f;
            //        break;
            //}

            //if (m_bSea)
            //    m_fMoveCostModifer *= m_fMoveCostModifer;

            RecalcFinalCost();
        }

        public void ClearRoad()
        {
            m_eRoadLevel = 0;
            //m_fMoveCostModifer = 1;
            RecalcFinalCost();
        }

        private void RecalcFinalCost()
        {
            if (m_bEmbark)
//                m_fFinalCost = (float)Math.Pow(m_fBaseCost + 20000, 1.0 / (m_eRoadLevel + 1));
                switch(m_eRoadLevel)
                {
                    case RoadQuality.None:
                        m_fFinalCost = m_fBaseCost + 20000;
                        break;
                    case RoadQuality.Country:
                        m_fFinalCost = (float)((m_fBaseCost + 20000) * 0.8f);
                        break;
                    case RoadQuality.Normal:
                        m_fFinalCost = (float)((m_fBaseCost + 20000) * 0.5f);
                        break;
                    case RoadQuality.Good:
                        m_fFinalCost = (float)((m_fBaseCost + 20000) * 0.2f);
                        break;
                }
            else
                if(m_bSea)
                    m_fFinalCost = m_eRoadLevel > 0 ? m_fBaseCost * 0.8f : m_fBaseCost;
                else
                    switch (m_eRoadLevel)
                    {
                        case RoadQuality.None:
                            m_fFinalCost = m_fBaseCost;
                            break;
                        case RoadQuality.Country:
                            m_fFinalCost = (float)(m_fBaseCost * 0.8f);
                            break;
                        case RoadQuality.Normal:
                            m_fFinalCost = (float)(m_fBaseCost * 0.5f);
                            break;
                        case RoadQuality.Good:
                            m_fFinalCost = (float)(m_fBaseCost * 0.2f);
                            break;
                    }

            if (m_bRuins)
                m_fFinalCost *= 10;
        }

        public float MovementCost
        {
            get
            {
                return m_fFinalCost;
            }
        }

        /// <summary>
        /// Дорога перекрыта. Нужно ТОЛЬКО для постройки внутригосударственных дорог.
        /// </summary>
        public bool m_bClosed = false;

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

        public IPointF[] m_aPoints = new IPointF[3];

        private static float GetDist(IPointF pPoint1, IPointF pPoint2)
        {
            return (float)Math.Sqrt((pPoint1.X - pPoint2.X) * (pPoint1.X - pPoint2.X) + (pPoint1.Y - pPoint2.Y) * (pPoint1.Y - pPoint2.Y) + (pPoint1.Z - pPoint2.Z) * (pPoint1.Z - pPoint2.Z));
        }

        public TransportationLinkBase(Location pLoc1, Location pLoc2)
        {
            m_aPoints = new IPointF[5]; 
            
            m_aPoints[0] = pLoc1;

            Location.Edge pLine = pLoc1.BorderWith[pLoc2][0];

            m_aPoints[1] = pLine.m_pInnerPoint;
            m_aPoints[2] = pLine.m_pMidPoint;

            //float fDist1 = GetDist(m_aPoints[0], m_aPoints[1]);
            //float fDist2 = GetDist(m_aPoints[2], m_aPoints[1]);

            //float fDist11 = GetDist(m_aPoints[0], pLine.m_pPoint1);
            //float fDist21 = GetDist(m_aPoints[2], pLine.m_pPoint1);

            //float fDist12 = GetDist(m_aPoints[0], pLine.m_pPoint2);
            //float fDist22 = GetDist(m_aPoints[2], pLine.m_pPoint2);

            pLine = pLoc2.BorderWith[pLoc1][0];
            m_aPoints[3] = pLine.m_pInnerPoint;

            m_aPoints[4] = pLoc2;

            //if (fDist1 + fDist2 < fDist11 + fDist21 &&
            //    fDist1 + fDist2 < fDist12 + fDist22)
            //{
            //    {
            //        m_aPoints[1].X = (m_aPoints[0].X + m_aPoints[2].X) / 2;
            //        m_aPoints[1].Y = (m_aPoints[0].Y + m_aPoints[2].Y) / 2;
            //        m_aPoints[1].Z = (m_aPoints[0].Z + m_aPoints[2].Z) / 2;
            //    }
            //}
            //else
            //{
            //    if (fDist11 + fDist21 < fDist12 + fDist22)
            //    {
            //        m_aPoints[1].X = (m_aPoints[1].X + 2 * pLine.m_pPoint1.X) / 3;
            //        m_aPoints[1].Y = (m_aPoints[1].Y + 2 * pLine.m_pPoint1.Y) / 3;
            //        m_aPoints[1].Z = (m_aPoints[1].Z + 2 * pLine.m_pPoint1.Z) / 3;
            //    }
            //    else
            //    {
            //        m_aPoints[1].X = (m_aPoints[1].X + 2 * pLine.m_pPoint2.X) / 3;
            //        m_aPoints[1].Y = (m_aPoints[1].Y + 2 * pLine.m_pPoint2.Y) / 3;
            //        m_aPoints[1].Z = (m_aPoints[1].Z + 2 * pLine.m_pPoint2.Z) / 3;
            //    }
            //}

            float fDist1final = GetDist(m_aPoints[0], m_aPoints[1]);
            float fDist2final = GetDist(m_aPoints[2], m_aPoints[1]);

            m_fBaseCost = fDist1final * pLoc1.GetMovementCost() + fDist2final * pLoc2.GetMovementCost();
            RecalcFinalCost();
        }

        public TransportationLinkBase(ILand pLand1, ILand pLand2)
        {
            m_aPoints[0] = pLand1;
            m_aPoints[2] = pLand2;

            Location.Edge pBestLine = null;
            float fShortest = float.MaxValue;
            Location.Edge[] cLines = pLand1.BorderWith[pLand2].ToArray();
            foreach (var pLine in cLines)
            {
                m_aPoints[1] = pLine.m_pMidPoint;

                float fDist1 = GetDist(m_aPoints[0], m_aPoints[1]);
                float fDist2 = GetDist(m_aPoints[2], m_aPoints[1]);

                if (fDist1 + fDist2 < fShortest)
                {
                    fShortest = fDist1 + fDist2;
                    pBestLine = pLine;
                }
            }
            m_aPoints[1] = pBestLine.m_pMidPoint;

            float fDist1final = GetDist(m_aPoints[0], m_aPoints[1]);
            float fDist2final = GetDist(m_aPoints[2], m_aPoints[1]); 

            m_fBaseCost = fDist1final * pLand1.GetMovementCost() + fDist2final * pLand2.GetMovementCost();
            RecalcFinalCost();
        }

        public TransportationLinkBase(ILandMass pLandMass1, ILandMass pLandMass2)
        {
            m_aPoints[0] = pLandMass1;
            m_aPoints[2] = pLandMass2;

            Location.Edge pBestLine = null;
            float fShortest = float.MaxValue;
            Location.Edge[] cLines = pLandMass1.BorderWith[pLandMass2].ToArray();
            foreach (var pLine in cLines)
            {
                m_aPoints[1] = pLine.m_pMidPoint;

                float fDist1 = GetDist(m_aPoints[0], m_aPoints[1]);
                float fDist2 = GetDist(m_aPoints[2], m_aPoints[1]); 

                if (fDist1 + fDist2 < fShortest)
                {
                    fShortest = fDist1 + fDist2;
                    pBestLine = pLine;
                }
            }
            m_aPoints[1] = pBestLine.m_pMidPoint;

            float fDist1final = GetDist(m_aPoints[0], m_aPoints[1]);
            float fDist2final = GetDist(m_aPoints[2], m_aPoints[1]);

            m_fBaseCost = fDist1final * pLandMass1.GetMovementCost() + fDist2final * pLandMass2.GetMovementCost();
            RecalcFinalCost();
        }

        public TransportationLinkBase(TransportationNode[] aPath)
        {
            List<IPointF> cPoints = new List<IPointF>();
            cPoints.Add(aPath[0]);

            m_fBaseCost = 0;

            TransportationNode pLastNode = null;
            foreach (TransportationNode pNode in aPath)
            {
                if (pLastNode != null)
                {
                    TransportationLinkBase pLink = pLastNode.m_cLinks[pNode];
                    if (pLink.m_aPoints[0] == pLastNode)
                    {
                        for (int i = 1; i < pLink.m_aPoints.Length; i++)
                            cPoints.Add(pLink.m_aPoints[i]);
                    }
                    else
                    {
                        for (int i = pLink.m_aPoints.Length - 2; i >= 0; i--)
                            cPoints.Add(pLink.m_aPoints[i]);
                    }

                    m_fBaseCost += pLink.m_fBaseCost;
                }

                pLastNode = pNode;
            }

            m_aPoints = cPoints.ToArray();
            RecalcFinalCost();
        }

    }

}
