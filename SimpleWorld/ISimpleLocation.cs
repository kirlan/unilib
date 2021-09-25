using System.Windows;

namespace SimpleWorld.Geography
{
    public interface ISimpleLocation
    {
        int X { get; }
        int Y { get; }

        int MovementCost { get; }
    }
}