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
        public BuildingType m_eType;

        public BuildingStandAlone(BuildingType eType)
        {
            m_eType = eType;
        }

        public override string ToString()
        {
            return m_eType.ToString();
        }
    }
}
