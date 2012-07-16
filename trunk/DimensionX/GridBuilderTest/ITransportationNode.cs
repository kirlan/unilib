using System;
namespace GridBuilderTest
{
    public interface ITransportationNode : IPointF
    {
        float DistanceTo(ITransportationNode pOtherNode);
        float GetMovementCost();
    }
}
