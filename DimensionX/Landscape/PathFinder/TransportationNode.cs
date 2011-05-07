using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandscapeGeneration.PathFind
{
    public abstract class TransportationNode : IPointF, ITransportationNode
    {
        public int m_iPassword = 0;

        public bool m_bHarbor = false;

        public Dictionary<TransportationNode, TransportationLink> m_cLinks = new Dictionary<TransportationNode, TransportationLink>();

        public float DistanceTo(ITransportationNode pOtherNode, float fCycleShift)
        {
            float fOtherX = pOtherNode.X;
            if (Math.Abs(X - fOtherX) > fCycleShift / 2)
                if (X < 0)
                    fOtherX -= fCycleShift;
                else
                    fOtherX += fCycleShift;

            return (float)Math.Sqrt((X - fOtherX) * (X - fOtherX) + (Y - pOtherNode.Y) * (Y - pOtherNode.Y));
        }

        public abstract float GetMovementCost();

        #region IPointF Members

        public abstract float X
        {
            get;
        }

        public abstract float Y
        {
            get;
        }

        #endregion
    }
}
