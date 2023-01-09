using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandscapeGeneration.PathFind
{
    public abstract class TransportationNode : VoronoiVertex
    {
        public int GreenLightCode { get; set; } = 0;

        public virtual Dictionary<TransportationNode, TransportationLinkBase> Links { get; } = new Dictionary<TransportationNode, TransportationLinkBase>();

        /// <summary>
        /// здесь сухопутные и морские маршруты могут пересекаться
        /// </summary>
        public virtual bool IsHarbor { get; set; } = false;

        // не нужно пытаться кешировать результаты вычислений - обращение к словарю занимает больше времени, чем повторное вычисление квадратного корня!
        public float DistanceTo(TransportationNode pOtherNode, float fCycleShift)
        {
            float fOwnX = X;
            float fOwnY = Y;
            float fOtherX = pOtherNode.X;
            float fOtherY = pOtherNode.Y;
            if (Math.Abs(fOwnX - fOtherX) > fCycleShift / 2)
            {
                if (fOwnX < 0)
                    fOtherX -= fCycleShift;
                else
                    fOtherX += fCycleShift;
            }

            float fDist = (float)Math.Sqrt((fOwnX - fOtherX) * (fOwnX - fOtherX) + (fOwnY - fOtherY) * (fOwnY - fOtherY));

            return fDist;
        }

        public abstract float GetMovementCost();
    }
}
