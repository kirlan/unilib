using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneLab
{
    public interface GenetixBase
    {
        GenetixBase MutateRace();
        GenetixBase MutateGender();
        GenetixBase MutateNation();
        GenetixBase MutateFamily();
        GenetixBase MutateIndividual();

        bool IsIdentical(GenetixBase pOther);

        string GetDescription();
    }
}
