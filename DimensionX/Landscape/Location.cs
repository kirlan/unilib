using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using LandscapeGeneration.PathFind;

namespace LandscapeGeneration
{
    public class Line
    {
        public Vertex m_pPoint1;
        public Vertex m_pPoint2;

        public Line(Vertex pPoint1, Vertex pPoint2)
        {
            m_pPoint1 = pPoint1;
            m_pPoint2 = pPoint2;

            m_fLength = (float)Math.Sqrt((pPoint1.m_fX - pPoint2.m_fX)*(pPoint1.m_fX - pPoint2.m_fX) + (pPoint1.m_fY - pPoint2.m_fY)*(pPoint1.m_fY - pPoint2.m_fY));
        }

        private void CalcLength(float fCycle)
        {
            float fPoint1X = m_pPoint1.m_fX;
            float fPoint1Y = m_pPoint1.m_fY;

            float fPoint2X = m_pPoint2.m_fX;
            float fPoint2Y = m_pPoint2.m_fY;

            if (fPoint2X + fCycle / 2 < fPoint1X)
                fPoint2X += fCycle;
            if (fPoint2X - fCycle / 2 > fPoint1X)
                fPoint2X -= fCycle;

            m_fLength = (float)Math.Sqrt((fPoint1X - fPoint2X) * (fPoint1X - fPoint2X) + (fPoint1Y - fPoint2Y) * (fPoint1Y - fPoint2Y));
        }

        public Line(Line pOriginal)
        {
            m_pPoint1 = pOriginal.m_pPoint1;
            m_pPoint2 = pOriginal.m_pPoint2;

            m_fLength = pOriginal.m_fLength;
        }

        public Line(BinaryReader binReader, Dictionary<long, Vertex> cVertexes)
        {
            m_pPoint1 = cVertexes[binReader.ReadInt64()];
            m_pPoint2 = cVertexes[binReader.ReadInt64()];

            m_fLength = (float)Math.Sqrt((m_pPoint1.m_fX - m_pPoint2.m_fX) * (m_pPoint1.m_fX - m_pPoint2.m_fX) + (m_pPoint1.m_fY - m_pPoint2.m_fY) * (m_pPoint1.m_fY - m_pPoint2.m_fY));
        }

        public void Save(BinaryWriter binWriter)
        {
            binWriter.Write(m_pPoint1.m_iID);
            binWriter.Write(m_pPoint2.m_iID);
        }

        public Line m_pPrevious = null;
        public Line m_pNext = null;

        public float m_fLength;

        public override string ToString()
        {
            return string.Format("({0}) - ({1}), Length {2}", m_pPoint1, m_pPoint2, m_fLength);
        }

    }

    public enum RegionType
    {
        Empty,
        Peak,
        Volcano
    }

    public class Location : TransportationNode, ITerritory
    {
        public Dictionary<object, List<Line>> m_cBorderWith = new Dictionary<object, List<Line>>();

        public PointF m_pCenter = new PointF(0,0);

        public RegionType m_eType = RegionType.Empty;

        #region IXY Members

        public override float X
        {
            get { return m_pCenter.X; }
        }

        public override float Y
        {
            get { return m_pCenter.Y; }
        }

        #endregion

        #region ITerritory Members

        /// <summary>
        /// Границы с другими такими же объектами
        /// </summary>
        public Dictionary<object, List<Line>> BorderWith
        {
            get { return m_cBorderWith; }
        }
        
        public object[] m_aBorderWith = null;

        internal void FillBorderWithKeys()
        {
            m_aBorderWith = new List<object>(m_cBorderWith.Keys).ToArray();
        }

        public bool m_bUnclosed = false;

        public bool Forbidden
        {
            get { return m_bUnclosed || m_bBorder; }
        }

        private object m_pOwner = null;

        public object Owner
        {
            get { return m_pOwner; }
            set { m_pOwner = value; }
        }

        #endregion

        /// <summary>
        /// Для "призрачных" локаций - ссылка на оригинал.
        /// </summary>
        internal Location m_pOrigin = null;

        /// <summary>
        /// Локация расположена за краем карты, здесь нельзя размещать постройки или прокладывать дороги.
        /// </summary>
        public bool m_bBorder = false;
        public bool m_bGhost = false;

        public long m_iID = 0;

        public int m_iGridX = -1;
        public int m_iGridY = -1;

        public void Create(long iID, double x, double y)
        {
            Create(iID, (float)x, (float)y);
        }

        public void Create(long iID, float x, float y)
        {
            m_pCenter = new PointF(x, y);
            m_iID = iID;
        }

        public void Create(long iID, float x, float y, int iGridX, int iGridY)
        {
            m_pCenter = new PointF(x, y);
            m_iID = iID;
            m_iGridX = iGridX;
            m_iGridY = iGridY;
        }

        public void Create(long iID, float x, float y, Location pOrigin)
        {
            m_pCenter = new PointF(x, y);
            m_iID = iID;

            m_pOrigin = pOrigin;
        }

        public Line m_pFirstLine = null;

        private float m_fPerimeter = 0;

        public float PerimeterLength
        {
            get { return m_fPerimeter; }
        }

        /// <summary>
        /// Настраивает связи "следующая"-"предыдущая" среди граней, уже хранящихся в словаре границ с другими локациями.
        /// </summary>
        public void BuildBorder(float fCycleShift)
        {
            if (m_bUnclosed || m_bBorder || m_cBorderWith.Count == 0)
                return;

            m_pFirstLine = m_cBorderWith[m_aBorderWith[0]][0];

            Line pCurrentLine = m_pFirstLine;
            List<Line> cTotalBorder = new List<Line>();

            m_fPerimeter = 0;
            foreach (var cLines in m_cBorderWith)
            {
                cTotalBorder.AddRange(cLines.Value);
                foreach (Line pLine in cLines.Value)
                    m_fPerimeter += pLine.m_fLength;
            }

            Line[] aTotalBorder = cTotalBorder.ToArray();

            int iLength = 0;
            do
            {
                bool bFound = false;
                foreach (Line pLine in aTotalBorder)
                {
                    if (pLine.m_pPoint1 == pCurrentLine.m_pPoint2 ||
                        (pLine.m_pPoint1.m_fY == pCurrentLine.m_pPoint2.m_fY &&
                         (pLine.m_pPoint1.m_fX == pCurrentLine.m_pPoint2.m_fX ||
                          Math.Abs(pLine.m_pPoint1.m_fX - pCurrentLine.m_pPoint2.m_fX) == fCycleShift)))
                    {
                        pCurrentLine.m_pNext = pLine;
                        pLine.m_pPrevious = pCurrentLine;

                        pCurrentLine = pLine;

                        iLength++;

                        bFound = true;

                        break;
                    }
                }
                if (!bFound)
                {
                    m_bUnclosed = true;
                    return;
                }
            }
            while (pCurrentLine != m_pFirstLine && iLength < m_cBorderWith.Count);
        }

        /// <summary>
        /// Смещает центр локации в реальный геометрический центр многоугольника
        /// </summary>
        public void CorrectCenter()
        {
            if (m_bUnclosed || m_bBorder)
                return;

            float fX = 0, fY = 0, fLength = 0;

            Line pLine = m_pFirstLine;
            do
            {
                fX += pLine.m_fLength * (pLine.m_pPoint1.X + pLine.m_pPoint2.X) / 2;
                fY += pLine.m_fLength * (pLine.m_pPoint1.Y + pLine.m_pPoint2.Y) / 2;
                fLength += pLine.m_fLength;

                pLine = pLine.m_pNext;
            }
            while (pLine != m_pFirstLine);

            m_pCenter.X = fX / fLength;
            m_pCenter.Y = fY / fLength;
        }

        public string GetStringID()
        {
            string sID = m_iID.ToString();
            if(m_iGridX != -1 || m_iGridY != -1)
                sID = string.Format("{0}, {1}", m_iGridX, m_iGridY);

            return string.Format("{1}[{0}]", sID, m_bBorder || m_bUnclosed ? "x" : " ");
        }

        public override string ToString()
        {
            return m_pCenter.ToString();
        }

        public override float GetMovementCost()
        {
            if (m_pOwner == null || Forbidden || m_bBorder)
                return 100;

            ILand pLand = m_pOwner as ILand;
            return pLand.MovementCost * (m_eType == RegionType.Empty ? 1 : 10);
        }

        public void Save(BinaryWriter binWriter)
        {
            binWriter.Write(m_iID);

            binWriter.Write((double)m_pCenter.X);
            binWriter.Write((double)m_pCenter.Y);

            binWriter.Write(m_bBorder ? 1 : 0);
            binWriter.Write(m_bUnclosed ? 1 : 0);

            if (m_pOwner != null)
                throw new Exception("Oops...");

            binWriter.Write(m_cBorderWith.Count);
            foreach (var pLoc in m_cBorderWith)
            {
                binWriter.Write((pLoc.Key as Location).m_iID);
                binWriter.Write(pLoc.Value.Count);
                foreach (Line pLine in pLoc.Value)
                {
                    pLine.Save(binWriter);
                }
            }
        }

        /// <summary>
        /// Временный список соседей с границами. Используется ТОЛЬКО при считывании списка локаций из файла.
        /// После синхронизации с m_cBorderWith может быть очищен.
        /// </summary>
        public Dictionary<long, List<Line>> m_cBorderWithID = new Dictionary<long, List<Line>>();

        /// <summary>
        /// Считывает локацию из файла.
        /// ВНИМАНИЕ: сразу после считывания m_cBorderWith остаётся пустой, но заполняется m_cBorderWithID, 
        /// в котором адресация выполняется не по ссылке на саму соседнюю локацию, а по её ID.
        /// После считывания всех локаций необходимо перевести информацию из m_cBorderWithID в m_cBorderWith.
        /// </summary>
        /// <param name="binReader"></param>
        public void Load(BinaryReader binReader, Dictionary<long, Vertex> cVertexes)
        {
            m_cLinks.Clear();
            m_cBorderWith.Clear();
            m_eType = RegionType.Empty;
            m_pOwner = null;

            m_iID = binReader.ReadInt64();

            m_pCenter.X = (float)binReader.ReadDouble();
            m_pCenter.Y = (float)binReader.ReadDouble();

            m_bBorder = binReader.ReadInt32() == 1;
            m_bUnclosed = binReader.ReadInt32() == 1;

            int iBorderCount = binReader.ReadInt32();
            for (int i = 0; i < iBorderCount; i++)
            {
                long iID = binReader.ReadInt64();
                m_cBorderWithID[iID] = new List<Line>();
                int iLinesCount = binReader.ReadInt32();
                for (int j = 0; j < iLinesCount; j++)
                {
                    Line pLine = new Line(binReader, cVertexes);
                    m_cBorderWithID[iID].Add(pLine);
                }
            }
        }

        public virtual void Reset()
        {
            m_cLinks.Clear();
            m_eType = RegionType.Empty;
            m_pOwner = null;
        }
    }
}
