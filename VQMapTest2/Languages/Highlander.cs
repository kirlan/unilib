using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NameGen;

namespace VQMapTest2.Languages
{
    class Highlander:Language
    {
        public Highlander()
            : base(NameGenerator.Language.Scotish)
        { }
    }
}
