using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NameGen;

namespace Socium.Languages
{
    internal class Dwarwen: Language
    {
        private readonly Confluxer m_pNations;

        #region ILanguage Members

        public Dwarwen()
            : base(NameGenerator.Language.Dwarven)
        {
            const string sNation = "grumdek kajulg ugun lamak tangit azkag ilraz khaz tarag baraz zahag tarak karar khavit varak marag dornat hringr jordukat isakaz vanag brizagul kurzr drakhaz gedag nomak khuzd dekhaz taud talenag varlosag khazat brizag kheled gabild dekhum hunkaz kestaz rasuh dornat duzkak tarkaz varak gedat danaz zarag karak zongag marnag keluz khuz";
            m_pNations = new Confluxer(sNation, 2);
        }
        
        protected override string GetNationName()
        {
            return m_pNations.Generate();
        }

        public override string RandomMaleName()
        {
            return NameGenerator2.GetHeroName(NameGenerator2.Language.Dwarf);
        }

        #endregion
    }
}
