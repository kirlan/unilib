using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NameGen;

namespace Socium.Languages
{
    class European: Language
    {
        private Confluxer m_pNations;

        public European()
            : base(NameGenerator.Language.European)
        { 
            string sNation = "italian faliscan latinish brython british french breton dalmatish corsican iberish spanish galician portugalean ligurish venetish catalan burgundishgerman franconish cimbrish austrish bavarish octish albion belgish beron cantabrish hobitish arnorish gondor elenian";
            m_pNations = new Confluxer(sNation, 2);
        }

        #region ILanguage Members
        
        protected override string GetNationName()
        {
            return m_pNations.Generate();
        }

        protected override string GetCountryName()
        {
            return NameGenerator2.GetCountryName();
        }

        protected override string GetTownName()
        {
            return NameGenerator2.GetTownName();
        }

        protected override string GetVillageName()
        {
            return NameGenerator2.GetTownName();
        }

        #endregion
    }
}
