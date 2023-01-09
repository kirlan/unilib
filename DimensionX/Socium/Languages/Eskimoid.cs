using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NameGen;

namespace Socium.Languages
{
    internal class Eskimoid: Language
    {
        private readonly Confluxer m_pNations;

        public Eskimoid()
            : base(NameGenerator.Language.Escimo)
        {
            const string sNation = "aleut alyutor chelkat chukch chuvat dolgat enet eskimo inuit evenk even itelmet karek ket koryak kumand negidal nenet nganasat tavg nivkh orok oroch samo selkup soyot telengit teleut tofalar tubalar tuvan udege ulch shor yukaghir";
            m_pNations = new Confluxer(sNation, 2);
       }

        #region ILanguage Members

        protected override string GetNationName()
        {
            return m_pNations.Generate();
        }

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
