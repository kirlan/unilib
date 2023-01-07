using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using LandscapeGeneration.PathFind;
using LandscapeGeneration.FastGrid;

namespace LandscapeGeneration
{
    public enum LandmarkType
    {
        Empty,
        Peak,
        Volcano
    }

    /// <summary>
    /// Локация - минимальная единица деления карты. Представляет собой выпуклый многоугольник, ячейку диаграммы Вороного.
    /// Локации объедняются в земли (Land)
    /// </summary>
    public class Location : Territory
    {
        public Territory[] m_aBorderWith = null;

        internal void FillBorderWithKeys()
        {
            m_aBorderWith = new List<Territory>(BorderWith.Keys).ToArray();
        }

        public bool m_bUnclosed = false;

        #region Territory Members

        public override bool Forbidden
        {
            get { return m_bUnclosed || m_bBorder; }
        }

        #endregion

        public LandmarkType m_eType = LandmarkType.Empty;

        public override float GetMovementCost()
        {
            //TODO: не гуд, что локация спрашивает что-то у земли, нужно вынести этот метод из локации
            if (GetOwner<Land>() == null || Forbidden || m_bBorder)
                return 100;

            foreach (var pEdge in BorderWith)
            {
                if (((Location)pEdge.Key).m_bBorder)
                    return 100;
            }

            Land pLand = GetOwner<Land>();
            return pLand.MovementCost * (m_eType == LandmarkType.Empty ? 1 : 10);
        }

        /// <summary>
        /// Для "призрачных" локаций - ссылка на оригинал.
        /// </summary>
        internal Location m_pOrigin = null;

        /// <summary>
        /// Для "призрачной" локации - направление, в котором следует искать "настоящую" локацию.
        /// Если локация не "призрачная", то CenterNone
        /// </summary>
        public VertexCH.Direction m_eShadowDir;

        /// <summary>
        /// Является ли локация "тенью" какой-то локации, принадлежащей на самом деле соседнему квадрату
        /// </summary>
        public bool IsShaded
        {
            get { return m_eShadowDir != VertexCH.Direction.CenterNone; }
        }

        /// <summary>
        /// Координаты центра локации нельзя смещать при вычислении сетки, т.к. она используется для формирования ровного края плоской карты
        /// </summary>
        public bool m_bFixed = false;

        /// <summary>
        /// Локация расположена за краем карты, здесь нельзя размещать постройки или прокладывать дороги.
        /// </summary>
        public bool m_bBorder = false;

        /// <summary>
        /// Уникальный номер локации. Пока нужен только для отладки, в будущем возможно ещё понадобится для чего-нибудь...
        /// </summary>
        public long m_iID = 0;

        public int m_iGridX = -1;
        public int m_iGridY = -1;

        /// <summary>
        /// Конструктор без параметров, чтобы класс можно было передавать параметром в шаблоны.
        /// После создания ОБЯЗАТЕЛЬНО вызвать Create()!
        /// </summary>
        public Location()
        {
        }

        public void Create(long iID, double x, double y)
        {
            Create(iID, (float)x, (float)y);
        }

        /// <summary>
        /// Заполняем внутренние поля локации после создания - для использования со случайно сгенерированной сеткой (диаграмма Вороного)
        /// </summary>
        /// <param name="iID">ID локации</param>
        /// <param name="x">кооринаты центра локации на плоскости</param>
        /// <param name="y">кооринаты центра локации на плоскости</param>
        public void Create(long iID, float x, float y)
        {
            X = x;
            Y = y;
            m_iID = iID;
        }

        /// <summary>
        /// Заполняем внутренние поля локации после создания - для использования с регулярной сеткой (например, гексагональной)
        /// </summary>
        /// <param name="iID">ID локации</param>
        /// <param name="x">кооринаты центра локации на плоскости</param>
        /// <param name="y">кооринаты центра локации на плоскости</param>
        /// <param name="iGridX">координаты локации на регулярной сетке</param>
        /// <param name="iGridY">координаты локации на регулярной сетке</param>
        public void Create(long iID, float x, float y, int iGridX, int iGridY)
        {
            X = x;
            Y = y;
            m_iID = iID;
            m_iGridX = iGridX;
            m_iGridY = iGridY;
        }

        /// <summary>
        /// Заполняем внутренние поля локации после создания - для "призрачных" локаций
        /// </summary>
        /// <param name="iID">ID локации</param>
        /// <param name="x">кооринаты центра локации на плоскости</param>
        /// <param name="y">кооринаты центра локации на плоскости</param>
        /// <param name="pOrigin">оригинальная локация, "призрака" которой создаём</param>
        public void Create(long iID, float x, float y, Location pOrigin)
        {
            X = x;
            Y = y;
            m_iID = iID;

            m_pOrigin = pOrigin;
        }

        /// <summary>
        /// Заполняем внутренние поля локации после создания - для "призрачных" локаций
        /// </summary>
        /// <param name="iID">ID локации</param>
        /// <param name="x">кооринаты центра локации на плоскости</param>
        /// <param name="y">кооринаты центра локации на плоскости</param>
        /// <param name="pOrigin">оригинальная локация, "призрака" которой создаём</param>
        public void Create(long iID, float x, float y, VertexCH.Direction eShadowDir)
        {
            X = x;
            Y = y;
            m_iID = iID;

            m_eShadowDir = eShadowDir;
        }

        public uint m_iShadow;

        public void SetShadow(uint stright)
        {
            m_iShadow = stright;
        }

        public VoronoiEdge m_pFirstLine = null;

        /// <summary>
        /// Во всех границах локации заменяет ссылку на "плохую" вершину ссылкой на "хорошую".
        /// Убирает связь с этой локацией у "плохой" вершины.
        /// Добавляет связь с этой локацией "хорошей" вершине
        /// </summary>
        /// <param name="pBad"></param>
        /// <param name="pGood"></param>
        public void ReplaceVertex(VoronoiVertex pBad, VoronoiVertex pGood)
        {
            foreach (var pEdge in BorderWith)
            {
                bool bGotIt = false;
                if (pEdge.Value[0].m_pPoint1 == pBad)
                {
                    pEdge.Value[0].m_pPoint1 = pGood;
                    bGotIt = true;
                }
                if (pEdge.Value[0].m_pPoint2 == pBad)
                {
                    pEdge.Value[0].m_pPoint2 = pGood;
                    bGotIt = true;
                }

                if (bGotIt)
                {
                    if (!pGood.m_cLocations.Contains(this))
                        pGood.m_cLocations.Add(this);
                    pBad.m_cLocations.Remove(this);
                }
            }
        }

        /// <summary>
        /// Настраивает связи "следующая"-"предыдущая" среди граней, уже хранящихся в словаре границ с другими локациями.
        /// </summary>
        public void BuildBorder(float fCycleShift)
        {
            if (m_bUnclosed || m_bBorder || BorderWith.Count == 0 || IsShaded)
                return;

            m_pFirstLine = BorderWith[m_aBorderWith[0]][0];

            List<VoronoiEdge> cTotalBorder = new List<VoronoiEdge>();

            PerimeterLength = 0;
            foreach (var cEdges in BorderWith)
            {
                cTotalBorder.AddRange(cEdges.Value);
                foreach (VoronoiEdge pEdge in cEdges.Value)
                    PerimeterLength += pEdge.Length;
            } 
            
            List<VoronoiEdge> cSequence = new List<VoronoiEdge>();

            VoronoiEdge pLast = BorderWith.Values.First()[0];
            cSequence.Add(pLast);
            for (int j = 0; j < BorderWith.Count; j++)
            {
                foreach (var pEdge in BorderWith)
                {
                    if (((Location)pEdge.Key).IsShaded)
                        continue;

                    if (!cSequence.Contains(pEdge.Value[0]) &&
                        (pEdge.Value[0].m_pPoint1 == pLast.m_pPoint2 ||
                         (pEdge.Value[0].m_pPoint1.Y == pLast.m_pPoint2.Y &&
                          (pEdge.Value[0].m_pPoint1.X == pLast.m_pPoint2.X ||
                           Math.Abs(pEdge.Value[0].m_pPoint1.X - pLast.m_pPoint2.X) == fCycleShift))))
                    {
                        pLast = pEdge.Value[0];
                        cSequence.Add(pLast);

                        break;
                    }
                }
            }

            if (cSequence.Count != BorderWith.Count)
            {
                throw new Exception();
            }

            pLast = cSequence.Last();
            foreach (var pEdge in cSequence)
            {
                pLast.m_pNext = pEdge;
                pLast = pEdge;

                //if (pEdge.Value.m_pMidPoint != pEdge.Key.m_cEdges[pLoc].m_pMidPoint)
                //    throw new Exception();
            }
        }

        /// <summary>
        /// Смещает центр локации в реальный геометрический центр многоугольника
        /// </summary>
        public void CorrectCenter()
        {
            if (m_pFirstLine == null)
                m_bUnclosed = true;

            if (m_bUnclosed || m_bBorder)
                return;

            float fX = 0, fY = 0, fLength = 0;

            VoronoiEdge pLine = m_pFirstLine;
            do
            {
                fX += pLine.Length * (pLine.m_pPoint1.X + pLine.m_pPoint2.X) / 2;
                fY += pLine.Length * (pLine.m_pPoint1.Y + pLine.m_pPoint2.Y) / 2;
                fLength += pLine.Length;

                pLine = pLine.m_pNext;
            }
            while (pLine != m_pFirstLine);

            X = fX / fLength;
            Y = fY / fLength;
        }

        public void Save(BinaryWriter binWriter)
        {
            binWriter.Write(m_iID);

            binWriter.Write((double)X);
            binWriter.Write((double)Y);

            binWriter.Write(m_bBorder ? 1 : 0);
            binWriter.Write(m_bUnclosed ? 1 : 0);

            //TODO: зачем здесь эта проверка???
            if (!HasOwner<Land>())
                throw new Exception("Oops...");

            binWriter.Write(BorderWith.Count);
            foreach (var pLoc in BorderWith)
            {
                binWriter.Write((pLoc.Key as Location).m_iID);
                binWriter.Write(pLoc.Value.Count);
                foreach (VoronoiEdge pLine in pLoc.Value)
                {
                    pLine.Save(binWriter);
                }
            }
        }

        /// <summary>
        /// Временный список соседей с границами. Используется ТОЛЬКО при считывании списка локаций из файла.
        /// После синхронизации с m_cBorderWith может быть очищен.
        /// </summary>
        public Dictionary<long, List<VoronoiEdge>> m_cBorderWithID = new Dictionary<long, List<VoronoiEdge>>();

        /// <summary>
        /// Считывает локацию из файла.
        /// ВНИМАНИЕ: сразу после считывания m_cBorderWith остаётся пустой, но заполняется m_cBorderWithID, 
        /// в котором адресация выполняется не по ссылке на саму соседнюю локацию, а по её ID.
        /// После считывания всех локаций необходимо перевести информацию из m_cBorderWithID в m_cBorderWith.
        /// </summary>
        /// <param name="binReader"></param>
        public void Load(BinaryReader binReader, Dictionary<long, VoronoiVertex> cVertexes)
        {
            m_cLinks.Clear();
            BorderWith.Clear();
            m_eType = LandmarkType.Empty;
            ClearOwner();

            m_iID = binReader.ReadInt64();

            X = (float)binReader.ReadDouble();
            Y = (float)binReader.ReadDouble();

            m_bBorder = binReader.ReadInt32() == 1;
            m_bUnclosed = binReader.ReadInt32() == 1;

            int iBorderCount = binReader.ReadInt32();
            for (int i = 0; i < iBorderCount; i++)
            {
                long iID = binReader.ReadInt64();
                m_cBorderWithID[iID] = new List<VoronoiEdge>();
                int iLinesCount = binReader.ReadInt32();
                for (int j = 0; j < iLinesCount; j++)
                {
                    VoronoiEdge pLine = new VoronoiEdge(binReader, cVertexes);
                    m_cBorderWithID[iID].Add(pLine);
                }
            }
        }

        //public virtual void Reset()
        //{
        //    m_cLinks.Clear();
        //    m_eType = LandmarkType.Empty;
        //    ClearOwner();
        //}
        
        public override string ToString()
        {
            return string.Format("{0}{3} ({1}, {2})", m_bBorder || m_bUnclosed ? "x" : "", X, Y, GetStringID());
        }

        public string GetStringID()
        {
            string sID = m_iID.ToString();
            if (m_iGridX != -1 || m_iGridY != -1)
                sID = string.Format("{0}, {1}", m_iGridX, m_iGridY);

            return string.Format("{1}[{0}]", sID, m_bBorder || m_bUnclosed ? "x" : " ");
        }

    }
}
