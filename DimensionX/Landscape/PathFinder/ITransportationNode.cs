using System;
namespace LandscapeGeneration.PathFind
{
    public interface ITransportationNode : IPointF
    {
        float DistanceTo(ITransportationNode pOtherNode, float fCycleShift);
        float GetMovementCost();
    }
}
