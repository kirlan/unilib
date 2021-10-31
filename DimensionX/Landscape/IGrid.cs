namespace LandscapeGeneration
{
    public delegate void BeginStepDelegate(string sDescription, int iLength);
    public delegate void ProgressStepDelegate();

    public interface IGrid<LOC>
         where LOC : Location, new()
    {
        LOC[] Locations { get; set; }
        float CycleShift { get; }
        int RX { get; }
        int RY { get; }

        int FrameWidth { get; }
        VoronoiVertex[] Vertexes { get; }
    }
}