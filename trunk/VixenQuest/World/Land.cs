using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using BenTools.Mathematics;
using System.IO;
using System.Drawing;

namespace VixenQuest.World
{
    public class Vertex
    {
        public float m_fX;
        public float m_fY;

        public float X
        {
            get { return m_fX; }
        }

        public float Y
        {
            get { return m_fY; }
        }

        private static long s_iCounter = 0;

        public long m_iID = s_iCounter++;

        public List<Vertex> m_cVertexes = new List<Vertex>();

        public List<long> m_cLinksTmp = new List<long>();

        public List<Location> m_cLocations = new List<Location>();

        public List<long> m_cLocationsTmp = new List<long>();

        public Vertex(BTVector pVector)
        {
            m_fX = (float)pVector.data[0];
            m_fY = (float)pVector.data[1];
        }

        //public Vertex(BinaryReader binReader)
        //{
        //    m_iID = binReader.ReadInt64();

        //    m_fX = (float)binReader.ReadDouble();
        //    m_fY = (float)binReader.ReadDouble();


        //    int iLinksCount = binReader.ReadInt32();
        //    for (int i = 0; i < iLinksCount; i++)
        //        m_cLinksTmp.Add(binReader.ReadInt64());

        //    int iLocationsCount = binReader.ReadInt32();
        //    for (int i = 0; i < iLocationsCount; i++)
        //        m_cLocationsTmp.Add(binReader.ReadInt64());
        //}

        //public void Save(BinaryWriter binWriter)
        //{
        //    binWriter.Write(m_iID);

        //    binWriter.Write((double)m_fX);
        //    binWriter.Write((double)m_fY);

        //    binWriter.Write(m_cVertexes.Count);
        //    foreach (Vertex pVertex in m_cVertexes)
        //        binWriter.Write(pVertex.m_iID);

        //    binWriter.Write(m_cLocations.Count);
        //    foreach (Location pLocation in m_cLocations)
        //        binWriter.Write(pLocation.m_iID);
        //}

        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}", X, Y);
        }

        //public override float GetMovementCost()
        //{
        //    return 100;
        //}
    }

    public class Line
    {
        public Vertex m_pPoint1;
        public Vertex m_pPoint2;

        public Line(Vertex pPoint1, Vertex pPoint2)
        {
            m_pPoint1 = pPoint1;
            m_pPoint2 = pPoint2;

            m_fLength = (float)Math.Sqrt((pPoint1.m_fX - pPoint2.m_fX) * (pPoint1.m_fX - pPoint2.m_fX) + (pPoint1.m_fY - pPoint2.m_fY) * (pPoint1.m_fY - pPoint2.m_fY));

            //if (m_fLength == 0.0)
            //    throw new Exception("Zero length line!");
            //if(m_fLength > Landscape.RY/2)
            //    m_fLength = (float)Math.Sqrt((pPoint1.X - pPoint2.X) * (pPoint1.X - pPoint2.X) + (pPoint1.Y - pPoint2.Y) * (pPoint1.Y - pPoint2.Y));
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

    enum LandType
    {
        Undefined,
        Forbidden
    }

    public class Land : Location
    {
        private static string[] m_aPlace = 
        {
            "Forest",
            "Woods",
            "Sands",
            "Desert",
            "Wastes",
            "Country",
            "Plains",
            "Shore",
            "Swamp",
            "Valley",
            "Mountains",
            "Hills",
        };

        public static ValuedString[] m_aProfessionM = 
        {
            //still low, but they have mo masters
            new ValuedString("stranger", 2),
            new ValuedString("traveller", 2),
            new ValuedString("hermit", 2),
            //small, but own buisiness
            new ValuedString("hunter", 3),
            new ValuedString("ranger", 3),
            new ValuedString("scavenger", 3),
            new ValuedString("priest", 3),
            //crime grunts
            new ValuedString("marauder", 4),
            new ValuedString("rogue", 4),
            new ValuedString("bandit", 4),
            new ValuedString("burglar", 4),
            //well-paid workers
            new ValuedString("slaver", 6),
            //common adventurers
            new ValuedString("fighter", 7),
            new ValuedString("monk", 7),
            new ValuedString("warden", 7),
            new ValuedString("mage apprentice", 7),
            new ValuedString("monk", 7),
            new ValuedString("bruiser", 7),
            new ValuedString("guardian", 7),
            new ValuedString("warrior", 7),
            //professional adventurers
            new ValuedString("gladiator", 8),
            new ValuedString("assasin", 8),
            new ValuedString("ninja", 8),
            new ValuedString("spy", 8),
            new ValuedString("sentinel", 8),
            new ValuedString("slut-hunter", 8),
            new ValuedString("head-hunter", 8),
            new ValuedString("bitch-hunter", 8),
            //mages, clerics, etc.
            new ValuedString("witch-hunter", 9),
            new ValuedString("demon-hunter", 9),
            new ValuedString("cleric", 9),
            new ValuedString("necromancer", 9),
            new ValuedString("eromancer", 9),
            new ValuedString("summoner", 9),
            new ValuedString("inquisitor", 9),
            new ValuedString("sage", 9),
            new ValuedString("shaman", 9),
            new ValuedString("mage", 9),
            new ValuedString("prophet", 9),
            //nobile & rich
            new ValuedString("knight", 10),
        };

        public static ValuedString[] m_aProfessionF = 
        {
            //small, but own buisiness
            new ValuedString("huntress", 3),
            new ValuedString("priestess", 3),
            //crime grunts
            new ValuedString("burglaress", 4),
            //city crime
            new ValuedString("thief", 5),
            //well-paid workers
            new ValuedString("bride", 6),
            //common adventurers
            new ValuedString("masturbatrix", 7),
            new ValuedString("wardeness", 7),
            //professional adventurers
            //new ValuedString("sentinel", 8),
            new ValuedString("head-huntress", 8),
            new ValuedString("bitch-huntress", 8),
            new ValuedString("assasiness", 8),
            new ValuedString("gladiatrix", 8),
            //mages, clerics, etc.
            new ValuedString("witch-huntress", 9),
            new ValuedString("demon-huntress", 9),
            new ValuedString("witch", 9),
            new ValuedString("sorceress", 9),
            new ValuedString("prophetess", 9),
            new ValuedString("inquisitrix", 9),
            new ValuedString("necromantress", 9),
            new ValuedString("enchantress", 9),
            new ValuedString("shamaness", 9),
            //nobile & rich
            new ValuedString("lady knight", 10),
        };

        public Dictionary<Land, List<Line>> m_cBorderWith = new Dictionary<Land, List<Line>>();

        /// <summary>
        /// Границы с другими такими же объектами
        /// </summary>
        public Dictionary<Land, List<Line>> BorderWith
        {
            get { return m_cBorderWith; }
        }

        public Land[] m_aBorderWith = null;

        internal void FillBorderWithKeys()
        {
            m_aBorderWith = new List<Land>(m_cBorderWith.Keys).ToArray();
        }

        /// <summary>
        /// Локация расположена за краем карты, здесь нельзя размещать постройки или прокладывать дороги.
        /// </summary>
        public bool m_bBorder = false;
        public bool m_bUnclosed = false;

        public bool Forbidden
        {
            get { return m_bUnclosed || m_bBorder; }
        }

        public long m_iID = 0;
        public PointF m_pCenter = new PointF(0, 0);
        
        public float X
        {
            get { return m_pCenter.X; }
        }

        public float Y
        {
            get { return m_pCenter.Y; }
        }

        public void Create(long iID, double x, double y)
        {
            Create(iID, (float)x, (float)y);
        }

        public void Create(long iID, float x, float y)
        {
            m_pCenter = new PointF(x, y);
            m_iID = iID;
        }

        internal Land m_pOrigin = null;

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
            if (m_bUnclosed || m_bBorder)
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

        public List<Location> m_cLocations = new List<Location>();

        public void Assign(State pState, bool bCapital)
        {
            if (pState != null && m_pWorld != pState.m_pWorld)
                throw new ArgumentException("Given state not belongs to this world!");

            if (m_eType == LandType.Forbidden && pState != null)
                throw new ArgumentException("Forbidden lands can't belong to any state!");

            m_iTier = Rnd.Get(6);
            m_pState = pState;

            if(m_eType != LandType.Forbidden)
                AddPopulation(50 + Rnd.Get(50), 10, pState.m_iTier * m_iTier);

            int iPlace = Rnd.Get(m_aPlace.Length);
            int iEpithet = Rnd.Get(m_aEpithet.Length);
            int iDescription = Rnd.Get(m_aDescription.Length);

            int variant = Rnd.Get(3);

            switch (variant)
            {
                case 0:
                    {
                        m_sName = m_aEpithet[iEpithet];
                        m_sName += m_aPlace[iPlace];
                    }
                    break;
                case 1:
                    {
                        m_sName = m_aPlace[iPlace];
                        m_sName += m_aDescription[iDescription];
                    }
                    break;
                default:
                    {
                        m_sName = m_aEpithet[iEpithet];
                        m_sName += m_aPlace[iPlace];
                        m_sName += m_aDescription[iDescription];
                    }
                    break;
            }

            if (m_eType != LandType.Forbidden)
            {
                Settlement.Size eSize = m_pState.m_pInfo.m_eCapital;
                if (!bCapital)
                    eSize--;

                int iCount = 1;
                do
                {
                    for (int i = 0; i < iCount * iCount; i++)
                        m_cLocations.Add(new Settlement(this, eSize, bCapital && iCount == 1));
                    iCount++;

                    if (eSize == Settlement.Size.Hamlet)
                        break;
                    else
                        eSize--;
                }
                while (true);

                int iPlacesCount = Rnd.Get(25);
                for (int i = 0; i < iPlacesCount; i++)
                    m_cLocations.Add(new Place(this));
            }
        }

        public Land(World pWorld)
            : base(pWorld)
        {
        }

        private LandType m_eType = LandType.Undefined;

        internal LandType Type
        {
            get { return m_eType; }
            set { m_eType = value; }
        }
        public Land(World pWorld, string sName)
            : base(pWorld)
        {
            m_sName = sName;
        }

        private static Dictionary<State.DwellersCathegory, int> m_cRaces = new Dictionary<State.DwellersCathegory, int>();

        public override Dictionary<State.DwellersCathegory, int> Races
        {
            get 
            {
                if (m_cRaces.Count == 0)
                {
                    m_cRaces[State.DwellersCathegory.WildAnimals] = 2;
                    m_cRaces[State.DwellersCathegory.DomesticAnimals] = 0;
                    m_cRaces[State.DwellersCathegory.SacredAnimals] = 0;
                    m_cRaces[State.DwellersCathegory.LesserRaces] = 2;
                    m_cRaces[State.DwellersCathegory.MajorRaces] = 1;
                }
                return m_cRaces; 
            }
        }

        public override ValuedString[] ProfessionM
        {
            get { return m_aProfessionM; }
        }

        public override ValuedString[] ProfessionF
        {
            get { return m_aProfessionF; }
        }

        List<Opponent> m_cAvailableOpponents = new List<Opponent>();

        public override Opponent[] AvailableOpponents()
        {
            if (m_cAvailableOpponents.Count == 0)
            {
                m_cAvailableOpponents.AddRange(m_cDwellers);
                foreach (Location place in m_cLocations)
                {
                    m_cAvailableOpponents.AddRange(place.AvailableOpponents());
                }
            }
            return m_cAvailableOpponents.ToArray();
        }

        public override Location NextStepTo(Location pTarget)
        {
            if (this == pTarget)
                return null;

            if (pTarget is Land)
            {
                if (m_pWorld == pTarget.m_pWorld)
                {
                    Land[] pPath = Universe.ShortestWay(this, pTarget as Land);
                    return pPath[1];
                }
                else
                {
                    if (Rnd.OneChanceFrom(100))
                        return pTarget;
                    else
                        return this;
                }
            }

            if(pTarget is Settlement)
            {
                Location pNextStep = NextStepTo((pTarget as Settlement).m_pLand);
                if(pNextStep == null)
                    return pTarget;

                return pNextStep;
            }

            if(pTarget is Place)
            {
                Location pNextStep = NextStepTo((pTarget as Place).m_pLand);
                if (pNextStep == null)
                    return pTarget;

                return pNextStep;
            }

            if(pTarget is Building)
            {
                return NextStepTo((pTarget as Building).m_pSettlement);
            }

            return null;
        }
    }
}
