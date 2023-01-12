using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneLab
{
    public interface IGenetix
    {
        IGenetix MutateRace();
        IGenetix MutateGender();
        IGenetix MutateNation();
        IGenetix MutateFamily();
        IGenetix MutateIndividual();

        bool IsIdentical(IGenetix pOther);

        string GetDescription();
    }
}
