using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NameGen;

namespace VQMapTest2.Languages
{
    class Elven: Language
    {
        public Elven()
            : base(NameGenerator.Language.Elven)
        { }

        #region ILanguage Members

        protected override string GetTownName()
        {
            return NameGenerator.GetEthnicName(NameGenerator.Language.Elfish);
        }

        protected override string GetVillageName()
        {
            return NameGenerator.GetEthnicName(NameGenerator.Language.Elfish);
        }

        public override string RandomMaleName()
        {
            return NameGenerator2.GetHeroName(NameGenerator2.Language.Elf);
        }

        #endregion
    }
}
