using System;
namespace GridBuilderTest
{
    public interface ITransportationNode
    {
        float DistanceTo(ITransportationNode pOtherNode, float fCycleShift);
       // float GetMovementCost();
        float X { get; }
        float Y { get; }
    }
}
