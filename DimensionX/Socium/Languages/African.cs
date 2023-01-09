using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NameGen;

namespace Socium.Languages
{
    internal class African:Language
    {
        private readonly Confluxer m_pNations;

        public African()
            : base(NameGenerator.Language.African)
        {
            const string sNation = "bantu mbuti berber gabon kalah djemba bafana ngoro mbundu ovambo luvale kalanga kutiba kongo yombe lingala zulu basa mbati sukuma bena ganda kamba meru soga masaba gwere tonga bemba nsenga tumbuka makhuwa ronga makonde ndebele herero bwana impala jumbo mamba ubuntu simba";
            m_pNations = new Confluxer(sNation, 3);
        }

        protected override string GetNationName()
        {
            return m_pNations.Generate();
        }
    }
}
