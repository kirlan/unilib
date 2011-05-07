using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NameGen;

namespace Socium.Languages
{
    class European: Language
    {
        public European()
            : base(NameGenerator.Language.European)
        { }

        #region ILanguage Members

        protected override string GetCountryName()
        {
            return NameGenerator2.GetCountryName();
        }

        protected override string GetTownName()
        {
            return NameGenerator2.GetTownName();
        }

        protected override string GetVillageName()
        {
            return NameGenerator2.GetTownName();
        }

        #endregion
    }
}
