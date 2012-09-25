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
        public Vertex m_pMidPoint;
        public Vertex m_pInnerPoint;

        public Line(Vertex pPoint1, Vertex pPoint2, Vertex pMidPoint, Vertex pInnerPoint)
        {
            m_pPoint1 = pPoint1;
            m_pPoint2 = pPoint2;
            m_pMidPoint = pMidPoint;
            m_pInnerPoint = pInnerPoint;

            m_fLength = (float)Math.Sqrt((pPoint1.m_fX - pPoint2.m_fX)*(pPoint1.m_fX - pPoint2.m_fX) + (pPoint1.m_fY - pPoint2.m_fY)*(pPoint1.m_fY - pPoint2.m_fY));
        }

        public Line(Line pOriginal)
        {
            m_pPoint1 = pOriginal.m_pPoint1;
            m_pPoint2 = pOriginal.m_pPoint2;
            m_pMidPoint = pOriginal.m_pMidPoint;
            m_pInnerPoint = pOriginal.m_pInnerPoint;

            m_fLength = pOriginal.m_fLength;
        }

        public Line(BinaryReader binReader, Dictionary<long, Vertex> cVertexes)
        {
            m_pPoint1 = cVertexes[binReader.ReadInt64()];
            m_pPoint2 = cVertexes[binReader.ReadInt64()];
            m_pMidPoint = cVertexes[binReader.ReadInt64()];
            m_pInnerPoint = cVertexes[binReader.ReadInt64()];

            m_fLength = (float)Math.Sqrt((m_pPoint1.m_fX - m_pPoint2.m_fX) * (m_pPoint1.m_fX - m_pPoint2.m_fX) + (m_pPoint1.m_fY - m_pPoint2.m_fY) * (m_pPoint1.m_fY - m_pPoint2.m_fY));
        }

        public void Save(BinaryWriter binWriter)
        {
            binWriter.Write(m_pPoint1.m_iID);
            binWriter.Write(m_pPoint2.m_iID);
            binWriter.Write(m_pMidPoint.m_iID);
            binWriter.Write(m_pInnerPoint.m_iID);
        }

        public void Merge(Vertex pFrom, Vertex pTo)
        {
            if (m_pPoint1 == pFrom)
                m_pPoint1 = pTo;
            if (m_pPoint2 == pFrom)
                m_pPoint2 = pTo;
            if (m_pMidPoint == pFrom)
                m_pMidPoint = pTo;
            if (m_pInnerPoint == pFrom)
                m_pInnerPoint = pTo;
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

        public RegionType m_eType = RegionType.Empty;

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
        /// <summary>
        /// Координаты центра локации нельзя смещать при вычислении сетки, т.к. она используется для формирования ровного края плоской карты
        /// </summary>
        public bool m_bFixed = false;

        public long m_iID = 0;

        public int m_iGridX = -1;
        public int m_iGridY = -1;

        public void Create(long iID, double x, double y, double z)
        {
            Create(iID, (float)x, (float)y, (float)z);
        }

        public void Create(long iID, float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
            m_iID = iID;
        }

        public void Create(long iID, float x, float y, float z, int iGridX, int iGridY)
        {
            X = x;
            Y = y;
            Z = z;
            m_iID = iID;
            m_iGridX = iGridX;
            m_iGridY = iGridY;
        }

        public void Create(long iID, float x, float y, float z, Location pOrigin)
        {
            X = x;
            Y = y;
            Z = z;
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
        public void BuildBorder()
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
                    if (pLine.m_pPoint1 == pCurrentLine.m_pPoint2)
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

            float fX = 0, fY = 0, fZ = 0, fLength = 0;

            Line pLine = m_pFirstLine;
            do
            {
                fX += pLine.m_fLength * (pLine.m_pPoint1.X + pLine.m_pPoint2.X) / 2;
                fY += pLine.m_fLength * (pLine.m_pPoint1.Y + pLine.m_pPoint2.Y) / 2;
                fZ += pLine.m_fLength * (pLine.m_pPoint1.Z + pLine.m_pPoint2.Z) / 2;
                fLength += pLine.m_fLength;

                pLine = pLine.m_pNext;
            }
            while (pLine != m_pFirstLine);

            X = fX / fLength;
            Y = fY / fLength;
            Z = fZ / fLength;

            List<Line> cTotalBorder = new List<Line>();

            foreach (var cLines in m_cBorderWith)
                cTotalBorder.AddRange(cLines.Value);

            Line[] aTotalBorder = cTotalBorder.ToArray();
            foreach (Line pLne in aTotalBorder)
            {
                pLne.m_pMidPoint.X = (pLne.m_pPoint1.X + pLne.m_pPoint2.X) / 2;
                pLne.m_pMidPoint.Y = (pLne.m_pPoint1.Y + pLne.m_pPoint2.Y) / 2;
                pLne.m_pMidPoint.Z = (pLne.m_pPoint1.Z + pLne.m_pPoint2.Z) / 2;

                pLne.m_pInnerPoint.X = (pLne.m_pMidPoint.X + X) / 2;
                pLne.m_pInnerPoint.Y = (pLne.m_pMidPoint.Y + Y) / 2;
                pLne.m_pInnerPoint.Z = (pLne.m_pMidPoint.Z + Z) / 2;
            }
        }

        public string GetStringID()
        {
            string sID = m_iID.ToString();
            if(m_iGridX != -1 || m_iGridY != -1)
                sID = string.Format("{0}, {1}", m_iGridX, m_iGridY);

            return string.Format("{1}[{0}]", sID, m_bBorder || m_bUnclosed ? "x" : " ");
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

            binWriter.Write((double)X);
            binWriter.Write((double)Y);
            binWriter.Write((double)Z);

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

            X = (float)binReader.ReadDouble();
            Y = (float)binReader.ReadDouble();
            Z = (float)binReader.ReadDouble();

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
