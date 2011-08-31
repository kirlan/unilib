using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace LandscapeGeneration.PathFind
{
    public class TransportationLink
    {
        private float m_fBaseCost;

        private float m_fFinalCost;

        //private float m_fMoveCostModifer = 1;

        //public float MoveCostModifer
        //{
        //    get { return m_fMoveCostModifer; }
        //}

        private int m_iRoadLevel = 0;

        public int RoadLevel
        {
            get { return m_iRoadLevel; }
        }

        /// <summary>
        /// Построить дорогу на этом участке
        /// </summary>
        /// <param name="iLevel">Уровень дороги: 1 - просёлок, 2 - обычная дорога, 3 - имперская дорога</param>
        public void BuildRoad(int iLevel)
        {
            if (iLevel <= m_iRoadLevel)
                return;

            m_iRoadLevel = iLevel;
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
            m_iRoadLevel = 0;
            //m_fMoveCostModifer = 1;
            RecalcFinalCost();
        }

        private void RecalcFinalCost()
        {
            if (m_bEmbark)
                m_fFinalCost = (float)Math.Pow(m_fBaseCost + 20000, 1.0 / (m_iRoadLevel + 1));
            else
                if(m_bSea)
                    m_fFinalCost = m_iRoadLevel > 0 ? m_fBaseCost * 0.8f : m_fBaseCost;
                else
                    m_fFinalCost = (float)Math.Pow(m_fBaseCost, 1.0 / (m_iRoadLevel + 1));

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

        public PointF[] m_aPoints = new PointF[3];

        private static float GetDist(PointF pPoint1, PointF pPoint2)
        {
            return (float)Math.Sqrt((pPoint1.X - pPoint2.X) * (pPoint1.X - pPoint2.X) + (pPoint1.Y - pPoint2.Y) * (pPoint1.Y - pPoint2.Y));
        }

        private static float GetDist(TransportationNode pPoint1, PointF pPoint2)
        {
            return (float)Math.Sqrt((pPoint1.X - pPoint2.X) * (pPoint1.X - pPoint2.X) + (pPoint1.Y - pPoint2.Y) * (pPoint1.Y - pPoint2.Y));
        }

        private static float GetDist(PointF pPoint1, Vertex pPoint2)
        {
            return (float)Math.Sqrt((pPoint1.X - pPoint2.X) * (pPoint1.X - pPoint2.X) + (pPoint1.Y - pPoint2.Y) * (pPoint1.Y - pPoint2.Y));
        }

        public TransportationLink(Location pLoc1, Location pLoc2, float fCycleShift)
        {
            m_aPoints[0] = new PointF(pLoc1.X, pLoc1.Y);
            m_aPoints[2] = new PointF(pLoc2.X, pLoc2.Y);

            if (Math.Abs(pLoc1.X - pLoc2.X) > fCycleShift / 2)
            {
                if (pLoc1.X < 0)
                    m_aPoints[2].X -= fCycleShift;
                else
                    m_aPoints[2].X += fCycleShift;
            }

            Line pLine = pLoc1.BorderWith[pLoc2][0];

            m_aPoints[1] = new PointF((pLine.m_pPoint1.X + pLine.m_pPoint2.X) / 2, (pLine.m_pPoint1.Y + pLine.m_pPoint2.Y) / 2);

            float fDist1 = GetDist(m_aPoints[0], m_aPoints[1]);
            float fDist2 = GetDist(m_aPoints[2], m_aPoints[1]);

            float fDist11 = GetDist(m_aPoints[0], pLine.m_pPoint1);
            float fDist21 = GetDist(m_aPoints[2], pLine.m_pPoint1);

            float fDist12 = GetDist(m_aPoints[0], pLine.m_pPoint2);
            float fDist22 = GetDist(m_aPoints[2], pLine.m_pPoint2);

            if (fDist1 + fDist2 < fDist11 + fDist21 &&
                fDist1 + fDist2 < fDist12 + fDist22)
            {
                {
                    m_aPoints[1].X = (m_aPoints[0].X + m_aPoints[2].X) / 2;
                    m_aPoints[1].Y = (m_aPoints[0].Y + m_aPoints[2].Y) / 2;
                }
            }
            else
            {
                if (fDist11 + fDist21 < fDist12 + fDist22)
                {
                    m_aPoints[1].X = (m_aPoints[1].X + 2 * pLine.m_pPoint1.X) / 3;
                    m_aPoints[1].Y = (m_aPoints[1].Y + 2 * pLine.m_pPoint1.Y) / 3;
                }
                else
                {
                    m_aPoints[1].X = (m_aPoints[1].X + 2 * pLine.m_pPoint2.X) / 3;
                    m_aPoints[1].Y = (m_aPoints[1].Y + 2 * pLine.m_pPoint2.Y) / 3;
                }
            }

            float fDist1final = GetDist(m_aPoints[0], m_aPoints[1]);
            float fDist2final = GetDist(m_aPoints[2], m_aPoints[1]);

            m_aPoints[2].X = pLoc2.X;

            m_fBaseCost = fDist1final * pLoc1.GetMovementCost() + fDist2final * pLoc2.GetMovementCost();
            RecalcFinalCost();
        }

        public TransportationLink(ILand pLand1, ILand pLand2, float fCycleShift)
        {
            m_aPoints[0] = new PointF(pLand1.X, pLand1.Y);
            m_aPoints[2] = new PointF(pLand2.X, pLand2.Y);

            if (Math.Abs(pLand1.X - pLand2.X) > fCycleShift / 2)
            {
                if (pLand1.X < 0)
                    m_aPoints[2].X -= fCycleShift;
                else
                    m_aPoints[2].X += fCycleShift;
            }

            Line pBestLine = null;
            float fShortest = float.MaxValue;
            Line[] cLines = pLand1.BorderWith[pLand2].ToArray();
            foreach (Line pLine in cLines)
            {
                m_aPoints[1] = new PointF((pLine.m_pPoint1.X + pLine.m_pPoint2.X) / 2, (pLine.m_pPoint1.Y + pLine.m_pPoint2.Y) / 2);

                float fDist1 = GetDist(m_aPoints[0], m_aPoints[1]);
                float fDist2 = GetDist(m_aPoints[2], m_aPoints[1]);

                if (fDist1 + fDist2 < fShortest)
                {
                    fShortest = fDist1 + fDist2;
                    pBestLine = pLine;
                }
            }
            m_aPoints[1] = new PointF((pBestLine.m_pPoint1.X + pBestLine.m_pPoint2.X) / 2, (pBestLine.m_pPoint1.Y + pBestLine.m_pPoint2.Y) / 2);

            float fDist1final = GetDist(m_aPoints[0], m_aPoints[1]);
            float fDist2final = GetDist(m_aPoints[2], m_aPoints[1]); 

            m_aPoints[2].X = pLand2.X;

            m_fBaseCost = fDist1final * pLand1.GetMovementCost() + fDist2final * pLand2.GetMovementCost();
            RecalcFinalCost();
        }

        public TransportationLink(ILandMass pLandMass1, ILandMass pLandMass2, float fCycleShift)
        {
            m_aPoints[0] = new PointF(pLandMass1.X, pLandMass1.Y);
            m_aPoints[2] = new PointF(pLandMass2.X, pLandMass2.Y);

            if (Math.Abs(pLandMass1.X - pLandMass2.X) > fCycleShift / 2)
            {
                if (pLandMass1.X < 0)
                    m_aPoints[2].X -= fCycleShift;
                else
                    m_aPoints[2].X += fCycleShift;
            }

            Line pBestLine = null;
            float fShortest = float.MaxValue;
            Line[] cLines = pLandMass1.BorderWith[pLandMass2].ToArray();
            foreach (Line pLine in cLines)
            {
                m_aPoints[1] = new PointF((pLine.m_pPoint1.X + pLine.m_pPoint2.X) / 2, (pLine.m_pPoint1.Y + pLine.m_pPoint2.Y) / 2);

                float fDist1 = GetDist(m_aPoints[0], m_aPoints[1]);
                float fDist2 = GetDist(m_aPoints[2], m_aPoints[1]); 

                if (fDist1 + fDist2 < fShortest)
                {
                    fShortest = fDist1 + fDist2;
                    pBestLine = pLine;
                }
            }
            m_aPoints[1] = new PointF((pBestLine.m_pPoint1.X + pBestLine.m_pPoint2.X) / 2, (pBestLine.m_pPoint1.Y + pBestLine.m_pPoint2.Y) / 2);

            float fDist1final = GetDist(m_aPoints[0], m_aPoints[1]);
            float fDist2final = GetDist(m_aPoints[2], m_aPoints[1]);

            m_aPoints[2].X = pLandMass2.X;

            m_fBaseCost = fDist1final * pLandMass1.GetMovementCost() + fDist2final * pLandMass2.GetMovementCost();
            RecalcFinalCost();
        }

        public TransportationLink(TransportationNode[] aPath)
        {
            List<PointF> cPoints = new List<PointF>();
            cPoints.Add(new PointF(aPath[0].X, aPath[0].Y));

            m_fBaseCost = 0;

            TransportationNode pLastNode = null;
            foreach (TransportationNode pNode in aPath)
            {
                if (pLastNode != null)
                {
                    TransportationLink pLink = pLastNode.m_cLinks[pNode];
                    if (pLink.m_aPoints[0].X == pLastNode.X &&
                        pLink.m_aPoints[0].Y == pLastNode.Y)
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
