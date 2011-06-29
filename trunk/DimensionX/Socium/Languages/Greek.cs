using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NameGen;

namespace Socium.Languages
{
    class Greek:Language
    {
        private Confluxer m_pNations;

        public Greek()
            : base(NameGenerator.Language.Greek)
        { 
            string sNation = "krita sparta mikenia pilosia eolia ionia doria gestia lemnia atica aphinia arkadia elidia etolia epiria phesalia argosia kipria ilotia aheia danaia minia";
            m_pNations = new Confluxer(sNation, 2);
        }

        protected override string GetNationName()
        {
            return m_pNations.Generate() + "n";
        }
    }
}
