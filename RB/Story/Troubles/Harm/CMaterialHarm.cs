using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RB.Story.Solutions;
using Random;
using RB.Story.Solutions.Harm;
using RB.Socium;

namespace RB.Story.Troubles.Harm
{
    public abstract class CMaterialHarm : CHarm
    {
        public CMaterialHarm(CPerson pVictim, CPerson pVillain)
            : base(pVictim, pVillain)
        {
        }

        protected override CHarmLiquidation GetLiquidation()
        {
            List<HarmLiquidationType> cLiquidation = new List<HarmLiquidationType>();

            if (CanBeRestored)
                cLiquidation.Add(HarmLiquidationType.Direct);
            if (CanBeReplaced)
                cLiquidation.Add(HarmLiquidationType.Alternative);

            if (cLiquidation.Count == 0)
                cLiquidation.Add(HarmLiquidationType.Impossible);

            int iChoice = Rnd.Get(cLiquidation.Count);

            switch (cLiquidation[iChoice])
            {
                case HarmLiquidationType.Direct:
                    return new CRestoreObject(this);
                case HarmLiquidationType.Alternative:
                    return new CReplaceObject(this);
                case HarmLiquidationType.Impossible:
                    return new CImpossibleLiquidation(this);
            }

            return new CImpossibleLiquidation(this);
        }
    }
}
