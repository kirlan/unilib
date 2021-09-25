using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NameGen;

namespace RB.Socium.Languages
{
    class Elven: Language
    {
        private Confluxer m_pNations;
        
        public Elven()
            : base(NameGenerator.Language.Elven)
        { 
            string sNation = "alvi alfari asrai avari eldari elvi elfi elveni elgari falatimi fairi falmari lindari laendi minyari mitrimi mitali noldori nandori nelyari sidhi sindari sylvani solinari teleri vistani vanyari verani";
            m_pNations = new Confluxer(sNation, 2);
        }

        #region ILanguage Members

        protected override string GetNationName()
        {
            return m_pNations.Generate();
        }

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
