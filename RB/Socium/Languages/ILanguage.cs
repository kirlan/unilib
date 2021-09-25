using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VQMapTest2.Languages
{
    interface ILanguage
    {
        string GetCountryName();
        string GetTownName();
        string GetVillageName();
        string GetFamily();
        string GetFemaleName();
        string GetMaleName();
    }
}
