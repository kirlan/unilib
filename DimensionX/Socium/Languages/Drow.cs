using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NameGen;

namespace Socium.Languages
{
    internal class Drow: Language
    {
        private readonly Confluxer m_pNations;

        public Drow()
            : base(NameGenerator.Language.Drow)
        {
            const string sNation = "druchi drachau drenai drax darthir draevu eligsar elinsrix ilinsar ilindith ilinsar indarau jhinrau linthax narkuth naudu negleth nizer rathar rikueth rinteith slurp shanau sharor sharulx sereth sulix talinth thalax tufyr tusu udosth uctax ulnhyr ustath valsharex veldrav velxund velglarth vloth xau xath xuat xuiu xiuleb xukuth xunoth xunduth xith yath zotreth";
            m_pNations = new Confluxer(sNation, 2);
        }

        protected override string GetNationName()
        {
            return m_pNations.Generate();
        }
    }
}
