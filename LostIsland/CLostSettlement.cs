using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LostIsland
{
    public enum SettlementSize
    { 
        Small,
        Big
    }

    public class CLostSettlement
    {
        public string Name { get; }

        public SettlementSize Size { get; }

        public CLostFaction Owner { get; }

        public CLostSettlement(CLostFaction pFaction, SettlementSize eSize, string sName)
        {
            Owner = pFaction;
            Size = eSize;
            Name = sName;
        }
    }
}
