using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Random;

namespace LandscapeGeneration
{
    public class Land<LOC, LTI> : Territory<LOC>, IPointF, ITypedLand<LTI>
        where LOC:Location
        where LTI:LandTypeInfo
    {
        private object m_pArea = null;

        public object Area
        {
            get { return m_pArea; }
            set { m_pArea = value; }
        }

        private LTI m_pType = null;

        public LTI Type
        {
            get { return m_pType; }
            set { m_pType = value; }
        }

        public bool IsWater
        {
            get { return m_pType != null && m_pType.m_eType == EnvironmentType.Water; }
        }

        public float MovementCost
        {
            get { return m_pType == null ? 100 : m_pType.m_iMovementCost; }
        }

        public override float GetMovementCost()
        {
            return MovementCost;
        } 

        private int m_iHumidity = 0;

        /// <summary>
        /// Влажность, в процентах 0-100
        /// </summary>
        public int Humidity
        {
            get { return m_iHumidity; }
            set 
            { 
                m_iHumidity = value;

                if (Humidity > 100)
                    Humidity = 100;
                if (Humidity < 0)
                    Humidity = 0;
            }
        }

        public string GetLandsString()
        {
            string sLands = "";

            foreach (LOC pLoc in m_cContents)
                sLands += pLoc.GetStringID() + ", ";

            return sLands;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", m_pType.m_sName, m_cContents.Count);
        }
    }
}
