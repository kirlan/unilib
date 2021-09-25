using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NameGen;

namespace RB.Socium.Languages
{
    class Orkish:Language
    {
        public Orkish()
            : base(NameGenerator.Language.Orcish)
        { }

        #region ILanguage Members

        public override string RandomMaleName()
        {
            return NameGenerator2.GetHeroName(NameGenerator2.Language.Orc);
        }

        #endregion
    }
}
