using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RB.Story.Troubles;
using RB.Story.Solutions;
using Random;
using RB.Story.Troubles.Harm;
using RB.Genetix;
using RB.Geography;
using RB.Socium;

namespace RB.Story
{
    /// <summary>
    /// Сюжетная линия. Включает в себя ознакомление ГГ с проблемой, серию связанных с решением проблемы
    /// приключений и само решение проблемы.
    /// </summary>
    public class CStoryLine : CStoryBase
    {
        private CTrouble m_pBeginning;
        /// <summary>
        /// Завязка - ознакомление героя с проблемой
        /// </summary>
        public CTrouble Trouble
        {
            get { return m_pBeginning; }
        }

        private CLocation GetHabitableLocation(CLocation[] aLocations)
        {
            CLocation pHabitable;

            do
            {
                pHabitable = aLocations[Rnd.Get(aLocations.Length)];
            }
            while (!pHabitable.Territory.LandProperties.HasFlag(LandscapeProperty.Habitable));

            return pHabitable;
        }

        private CLocation GetSettlement(CLocation[] aLocations)
        {
            CLocation pSettlement;

            do
            {
                pSettlement = aLocations[Rnd.Get(aLocations.Length)];
            }
            while (pSettlement.Settlement == null);

            return pSettlement;
        }

        public CStoryLine(CLocation[] aLocations)
        {
            CTrouble pTrouble = null;

            //if (Rnd.Chances(1, 2))
            {
                //int iHarm = Rnd.Get(7);
                //switch (iHarm)
                //{
                //    case 0:
                //        {
                //            CLocation pFarLands = GetHabitableLocation(aLocations);
                //            CLocation pHome = GetSettlement(aLocations);
                //            CPerson pVictim = new CPerson();
                //            CPerson pVillain = new CPerson();
                //            pTrouble = new CForcedTransportation(pVictim, pVillain, pFarLands, pHome);
                //        }
                //        break;
                //    case 1:
                //        {
                //            CPerson pVictim = new CPerson();
                //            CPerson pVillain = new CPerson();
                //            pTrouble = new CImprisonment(pVictim, pVillain);
                //        }
                //        break;
                //}
            }
            //else
            //{
            //}

            m_pBeginning = pTrouble;

            m_pPlot = pTrouble.GetSolution();
        }

        private CPlot m_pPlot = null;
        /// <summary>
        /// Сюжет - решение задачи, поставленной в завязке
        /// </summary>
        public CPlot Solution
        {
            get { return m_pPlot; }
        }
    }
}
