using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NameGen;

namespace Socium.Languages
{
    class Eskimoid: Language
    {
        public Eskimoid()
            : base(NameGenerator.Language.Escimo)
        { }

        #region ILanguage Members

        protected override string GetCountryName()
        {
            return WordGenerator.GetWord(WordGenerator.Language.Eskimoid);
        }

        protected override string GetTownName()
        {
            return WordGenerator.GetWord(WordGenerator.Language.Eskimoid);
        }

        protected override string GetVillageName()
        {
            return WordGenerator.GetWord(WordGenerator.Language.Eskimoid);
        }

        #endregion
    }
}
