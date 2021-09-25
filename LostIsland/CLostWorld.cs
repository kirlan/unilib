using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Random;
using SimpleWorld;
using SimpleWorld.Geography;

namespace LostIsland
{
    public class CLostWorld: CSimpleWorld<CLostLocation, CLostTerritory>
    {
        public CLostFaction FactionOthers { get; } = new CLostFaction();
        public CLostFaction FactionExplorers { get; } = new CLostFaction();
        public CLostFaction FactionShadows { get; } = new CLostFaction();

        public CLostWorld(int iScale) : base(iScale)
        { 
        }

        protected override void SetupPredefinedLocations(ref List<CLostLocation> cLocations)
        {
            base.SetupPredefinedLocations(ref cLocations);

            // Central location
            AddLocation(ref cLocations, 0, 0, false);

            // First circle - hidden plateau, lands of Others and Shadows
            int iX, iY, iR;
            int iAverageR = m_iMinDist;
            double fAngle = (double)Rnd.Get(m_iMaxDist - m_iMinDist) / iAverageR;
            double fAverageDelta = (double)m_iMinDist / iAverageR;
            int iMaxR = 0;
            List<CLostLocation> cPossibleSettlements = new List<CLostLocation>();
            do
            {
                iR = Rnd.GetInRange(m_iMinDist, m_iMaxDist);
                if (iR > iMaxR)
                    iMaxR = iR;

                iX = (int)(iR * Math.Cos(fAngle));
                iY = (int)(iR * Math.Sin(fAngle));

                int iDelta = Rnd.GetInRange(m_iMinDist, m_iMaxDist);
                fAngle += (double)iDelta / iAverageR;

                if (PossibleLocation(ref cLocations, iX, iY, false))
                {
                    cPossibleSettlements.Add(AddLocation(ref cLocations, iX, iY, false));
                }
            }
            while (fAngle < 2 * Math.PI);

            // Second circle - mountain barrier
            iAverageR = m_iMaxDist + m_iMinDist;
            fAngle = (double)Rnd.Get(m_iMaxDist - m_iMinDist) / iAverageR;
            fAverageDelta = (double)m_iMinDist / iAverageR;
            int iAverageStepsCount = (int)(2 * Math.PI / fAverageDelta);

            CLostLocation pHiddenPassage = null;
            do
            {
                iR = iMaxR + Rnd.GetInRange(m_iMinDist, m_iMaxDist);
                iX = (int)(iR * Math.Cos(fAngle));
                iY = (int)(iR * Math.Sin(fAngle));

                int iDelta = Rnd.GetInRange(m_iMinDist, m_iMaxDist);
                fAngle += (double)iDelta / iAverageR;

                bool bForbidden = true;
                if (pHiddenPassage == null && (Rnd.OneChanceFrom(iAverageStepsCount) || fAngle >= 2 * Math.PI - fAverageDelta))
                {
                    bForbidden = false;
                }

                if (PossibleLocation(ref cLocations, iX, iY, true))
                {
                    var newLocation = AddLocation(ref cLocations, iX, iY, bForbidden);
                    newLocation.Territory = CLostTerritory.Mountains;
                    if (!bForbidden)
                        pHiddenPassage = newLocation;
                }
            }
            while (fAngle < 2 * Math.PI);

            if (pHiddenPassage == null)
                throw new Exception("Hidden passage wasn't created!");

            pHiddenPassage.Settlement = new CLostSettlement(FactionShadows, SettlementSize.Small, "Hidden Passage");
            foreach (var pLink in pHiddenPassage.Links)
            {
                if (cPossibleSettlements.Contains(pLink.Key))
                    cPossibleSettlements.Remove(pLink.Key as CLostLocation);
            }
            if (cPossibleSettlements.Count == 0)
                throw new Exception("Too few internal locations at hidden plateau!");
            int iSettlement = Rnd.Get(cPossibleSettlements.Count);
            var pSettlementLoc = cPossibleSettlements[iSettlement];
            pSettlementLoc.Settlement = new CLostSettlement(FactionShadows, SettlementSize.Big, "Lair of Shadows");
            cPossibleSettlements.Remove(pSettlementLoc);
            foreach (var pLink in pSettlementLoc.Links)
            {
                if (pLink.Key.Type != LocationType.Forbidden)
                    (pLink.Key as CLostLocation).Territory = CLostTerritory.Shallows;
                if (cPossibleSettlements.Contains(pLink.Key))
                    cPossibleSettlements.Remove(pLink.Key as CLostLocation);
            }
            if (cPossibleSettlements.Count == 0)
                throw new Exception("Too few internal locations at hidden plateau!");
            iSettlement = Rnd.Get(cPossibleSettlements.Count);
            cPossibleSettlements[iSettlement].Settlement = new CLostSettlement(FactionOthers, SettlementSize.Big, "Sacred City");

            // 3 radial mountain ranges, starting at barrier
            for (int i = 0; i < 3; i++)
            {
                iR = iMaxR + m_iMaxDist + m_iMinDist;
                fAngle = Math.PI * i * 2.0 / 3 + Math.PI * 2.0 / 12 - Rnd.Get(Math.PI * 2.0 / 6);
                fAverageDelta = (m_iMinDist + m_iMaxDist) / 2;
                iAverageStepsCount = (int)((WorldScale - iR) / fAverageDelta);
                bool bHiddenPassageCreated = false;
                do
                {
                    double fAngleDeviation = (double)Rnd.Get(m_iMinDist) / iR;

                    iX = (int)(iR * Math.Cos(fAngle + fAngleDeviation));
                    iY = (int)(iR * Math.Sin(fAngle + fAngleDeviation));

                    int iDelta = Rnd.GetInRange(m_iMinDist, m_iMaxDist);
                    iR += iDelta;

                    bool bForbidden = true;
                    if (!bHiddenPassageCreated && (Rnd.OneChanceFrom(iAverageStepsCount) || iR >= WorldScale - m_iMinDist))
                    {
                        bForbidden = false;
                        bHiddenPassageCreated = true;
                    }

                    var newLocation = AddLocation(ref cLocations, iX, iY, bForbidden);
                    newLocation.Territory = CLostTerritory.Mountains;
                    if (!bForbidden)
                        newLocation.Settlement = new CLostSettlement(FactionShadows, SettlementSize.Small, "Hidden Passage");
                }
                while (iR < WorldScale - m_iMinDist);
            }
        }

        protected override void MarkSubTypes()
        {
            int iUnsetLocation = Locations.Length;

            List<CLostLocation> cFront = new List<CLostLocation>();
            for (int i = 0; i < Locations.Length; i++)
            {
                if (Locations[i].Type == LocationType.Forbidden && Locations[i].Territory != null)
                {
                    iUnsetLocation--;

                    List<CLostLocation> cPossibleDirections = new List<CLostLocation>();
                    foreach (var pEdge in Locations[i].m_cEdges)
                    {
                        if (pEdge.Key.Type != LocationType.Forbidden && pEdge.Key.Territory == null)
                            cPossibleDirections.Add(pEdge.Key as CLostLocation);
                    }
                    for (int iLoc = 0; iLoc < cPossibleDirections.Count; iLoc++)
                    {
                        if (Locations[i].Territory.Type == CLostTerritory.TerritoryType.Mountains && Rnd.OneChanceFrom(10))
                        {
                            cPossibleDirections[iLoc].Territory = CLostTerritory.Hills;
                            cFront.Add(cPossibleDirections[iLoc]);
                            iUnsetLocation--;
                        }
                        else if (Locations[i].Territory.Type == CLostTerritory.TerritoryType.DeepSea)
                        { 
                            cPossibleDirections[iLoc].Territory = Rnd.OneChanceFrom(4) ? CLostTerritory.Desert : CLostTerritory.Shallows;
                            cFront.Add(cPossibleDirections[iLoc]);
                            iUnsetLocation--;
                        }
                    }
                }
            }
            for (int i = 0; i < iUnsetLocation / 4; i++)
            {
                int iLoc;
                do
                {
                    iLoc = Rnd.Get(Locations.Length);
                }
                while (Locations[iLoc].Type == LocationType.Forbidden || Locations[iLoc].Territory != null);

                int iSubType = Rnd.Get(6);
                switch (iSubType)
                {
                    case 0:
                        Locations[iLoc].Territory = CLostTerritory.Swamp;
                        break;
                    case 1:
                        Locations[iLoc].Territory = CLostTerritory.Shallows;
                        break;
                    case 2:
                        Locations[iLoc].Territory = CLostTerritory.Plains;
                        break;
                    case 3:
                        Locations[iLoc].Territory = CLostTerritory.Plains;
                        break;
                    case 4:
                        Locations[iLoc].Territory = CLostTerritory.Plains;
                        break;
                    case 5:
                        Locations[iLoc].Territory = CLostTerritory.Forest;
                        break;
                }
                cFront.Add(Locations[iLoc]);
            }

            bool bExpanding;
            do
            {
                List<CLostLocation> cNewFront = new List<CLostLocation>();
                bExpanding = false;
                for (int i = 0; i < cFront.Count; i++)
                {
                    if (cFront[i].Type != LocationType.Forbidden && cFront[i].Territory != null)
                    {
                        List<CLostLocation> cPossibleDirections = new List<CLostLocation>();
                        foreach (var pEdge in cFront[i].m_cEdges)
                        {
                            if (pEdge.Key.Type != LocationType.Forbidden && pEdge.Key.Territory == null)
                                cPossibleDirections.Add(pEdge.Key as CLostLocation);
                        }
                        if (cPossibleDirections.Count > 0)
                        {
                            int iLoc = Rnd.Get(cPossibleDirections.Count);
                            cPossibleDirections[iLoc].Territory = cFront[i].Territory;
                            bExpanding = true;
                            cNewFront.Add(cPossibleDirections[iLoc]);
                        }
                    }
                }
                cFront.AddRange(cNewFront);
            }
            while (bExpanding);
        }

        protected override void MarkForbiddenSubTypes()
        {
            /*
            List<CLostLocation> cFront = new List<CLostLocation>();

            // размещаем "затравки" для разных типов блокирующей территории
            for (int i = 0; i < 20; i++)
            {
                int iLoc;
                do
                {
                    iLoc = Rnd.Get(Locations.Length);
                }
                while (Locations[iLoc].Type != LocationType.Forbidden || Locations[iLoc].Territory != null);
                
                if (((Vector)Locations[iLoc].ToPoint()).Length > WorldScale)
                    Locations[iLoc].Territory = CLostTerritory.DeepSea;
                else
                    Locations[iLoc].Territory = CLostTerritory.Mountains;

                if (Locations[iLoc].Type != LocationType.Forbidden)
                    throw new Exception();

                cFront.Add(Locations[iLoc]);
            }

            // расширяем разные типы блокирующей территории вокруг "затравок" - работаем только по заблокированным локациям!
            bool bExpanding;
            do
            {
                List<CLostLocation> cNewFront = new List<CLostLocation>();
                bExpanding = false;
                for (int i = 0; i < cFront.Count; i++)
                {
                    if (cFront[i].Type == LocationType.Forbidden && cFront[i].Territory != null)
                    {
                        List<CLostLocation> cPossibleDirections = new List<CLostLocation>();
                        foreach (var pEdge in cFront[i].m_cEdges)
                        {
                            if (pEdge.Key.Type == LocationType.Forbidden && pEdge.Key.Territory == null)
                                cPossibleDirections.Add(pEdge.Key as CLostLocation);
                        }
                        if (cPossibleDirections.Count > 0)
                        {
                            int iLoc = Rnd.Get(cPossibleDirections.Count);
                            cPossibleDirections[iLoc].Territory = cFront[i].Territory;
                            if (cPossibleDirections[iLoc].Type != LocationType.Forbidden)
                                throw new Exception();
                            bExpanding = true;
                            cNewFront.Add(cPossibleDirections[iLoc]);
                        }
                    }
                }
                cFront.AddRange(cNewFront);
            }
            while (bExpanding);
            */

            for (int i = 0; i < Locations.Length; i++)
            {
                if (Locations[i].Type == LocationType.Forbidden && Locations[i].Territory == null)
                {
                    if (((Vector)Locations[i].ToPoint()).Length > WorldScale - m_iMinDist)
                        Locations[i].Territory = CLostTerritory.DeepSea;
                    else
                        Locations[i].Territory = CLostTerritory.Mountains;

                    if (Locations[i].Type != LocationType.Forbidden)
                        throw new Exception();

                    //foreach (var pEdge in Locations[i].m_cEdges)
                    //{
                    //    if (pEdge.Key.Type == LocationType.Forbidden && pEdge.Key.Territory != null)
                    //    {
                    //        Locations[i].Territory = pEdge.Key.Territory;
                    //        if (Locations[i].Type != LocationType.Forbidden)
                    //            throw new Exception();
                    //    }
                    //    if (pEdge.Key.Type == LocationType.Forbidden && pEdge.Key.Territory == null)
                    //    {
                    //        pEdge.Key.Territory = Locations[i].Territory;
                    //        if (pEdge.Key.Type != LocationType.Forbidden)
                    //            throw new Exception();
                    //    }
                    //}
                }
            }
        }
    }
}
