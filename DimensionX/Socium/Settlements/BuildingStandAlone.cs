using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Socium.Settlements
{
    public enum BuildingType
    {
        None,
        Farm,
        HuntingFields,
        Lair,
        Hideout
    }

    public class BuildingStandAlone
    {
        public BuildingType Type { get; }

        public BuildingStandAlone(BuildingType eType)
        {
            Type = eType;
        }

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}
