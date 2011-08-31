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

        // не использовать! обращение к словарю занимает больше времени, чем повторное вычисление квадратного корня!
        //public Dictionary<TransportationNode, float> m_cLinksDist = new Dictionary<TransportationNode, float>();

        public float DistanceTo(ITransportationNode pOtherNode, float fCycleShift)
        {
            float fDist = 0;
            //if (m_cLinksDist.TryGetValue(pOtherNode as TransportationNode, out fDist))
            //    return fDist;

            float fOwnX = X;
            float fOwnY = Y;
            float fOtherX = pOtherNode.X;
            float fOtherY = pOtherNode.Y;
            if (Math.Abs(fOwnX - fOtherX) > fCycleShift / 2)
                if (fOwnX < 0)
                    fOtherX -= fCycleShift;
                else
                    fOtherX += fCycleShift;

            fDist = (float)Math.Sqrt((fOwnX - fOtherX) * (fOwnX - fOtherX) + (fOwnY - fOtherY) * (fOwnY - fOtherY));
            //if (m_cLinks.ContainsKey(pOtherNode as TransportationNode))
            //    m_cLinksDist[pOtherNode as TransportationNode] = fDist;

            return fDist;
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
