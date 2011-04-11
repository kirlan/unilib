using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NameGen;

namespace VQMapTest2.Languages
{
    class Dwarwen:Language
    {
        #region ILanguage Members

        public Dwarwen()
            : base(NameGenerator.Language.Dwarven)
        { }

        public override string RandomMaleName()
        {
            return NameGenerator2.GetHeroName(NameGenerator2.Language.Dwarf);
        }

        #endregion
    }
}
