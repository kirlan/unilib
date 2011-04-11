using System;
namespace MinCostFlow
{
    public interface IVertex
    {
        CNode Entrance { get; }
        CNode Exit { get; }
    }
}
