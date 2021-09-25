using SimpleWorld.Geography;

namespace LostIsland
{
    public class CLostLocation : CSimpleLocation<CLostTerritory>
    {
        public CLostSettlement Settlement { get; internal set; } = null;

        public CLostLocation(int iX, int iY) : base(iX, iY)
        { 
        }
    }
}