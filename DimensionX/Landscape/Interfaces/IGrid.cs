namespace LandscapeGeneration
{
    public delegate void BeginStepDelegate(string sDescription, int iLength);
    public delegate void ProgressStepDelegate();

    public interface IGrid
    {
        Location[] Locations { get; set; }
        float CycleShift { get; }
        int RX { get; }
        int RY { get; }

        int FrameWidth { get; }
    }
}