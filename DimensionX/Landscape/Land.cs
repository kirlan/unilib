using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Random;

namespace LandscapeGeneration
{
    /// <summary>
    /// Земля - группа сопредельных локаций, имеющих один тип местности.
    /// Земли объединяются в тектонические плиты (LandMass).
    /// </summary>
    public class Land : TerritoryCluster<Land, Location>, ITypedLand<LandTypeInfo>
    {
        public LandTypeInfo LandType { get; set; } = null;

        public bool IsWater
        {
            get { return LandType != null && LandType.m_eEnvironment.HasFlag(Environment.Liquid); }
        }

        public bool IsBorder()
        {
            foreach (var pLoc in Contents)
                if (pLoc.m_bBorder)
                    return true;

            return false;
        }

        public float MovementCost
        {
            get { return LandType == null ? 100 : LandType.m_iMovementCost; }
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

        private float m_fTemperature = 0;

        public float Temperature
        {
            get { return m_fTemperature; }
            set { m_fTemperature = value; }
        }

        public override void Start(Location pSeed)
        {
            base.Start(pSeed);

            pSeed.AddLayer(this);
        }

        public string GetLandsString()
        {
            List<string> sLands = new List<string>();

            foreach (var pLoc in Contents)
                sLands.Add(pLoc.GetStringID());

            return String.Join(", ", sLands); ;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", LandType.m_sName, Contents.Count);
        }
    }
}
