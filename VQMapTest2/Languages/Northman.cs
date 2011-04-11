using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NameGen;

namespace VQMapTest2.Languages
{
    class Northman: Language
    {
        private Confluxer m_pFemale;

        private Confluxer m_pMale;

        public Northman()
            :base(NameGenerator.Language.Viking)
        {
            string sFemale = "alfdis arnora asgerd asleif asta astrid bera bergljot bergthora dotta freydis gjaflaug grima grimhild groa gudrid gudrun gunnhild gyda halldis hallfrid hallgerd hallveig helga herdis hild hildigunn hlif hrefna hrodny ingibjorg ingigerd ingirid ingunn jorunn katla ragna ragnhild rannveig saeunn sigrid svala thjodhild thora thorbjorg thordis thorfinna thorgerd thorgunna thorhalla thorhild thorkatla thorunn thurid thyra unn valgerd vigdis";
            m_pFemale = new Confluxer(sFemale, 2);

            string sMale = "aki alf alfgeir amundi ari armod arnfinn arnlaug arnor aslak bardi bergthor bersi bjarni bjorn bodvar bork botolf brand bui egil einar eindridi eirik eldgrim erlend eyjolf eystein eyvind finn finnbogi fridgeir gardi geir geirmund geirstein gest gizur glum grani grim gudmund gunnar gunnbjorn gunnlaug hafgrim hakon halfdan hall halldor hallfred harald harek hastein hauk havard hedin helgi herjolf hjalti hogni hord hrafn hring hroald hrut illugi ingi ingjald ingolf isleif ivar kalf kari karlsefni ketil knut kol kolbein lambi leif ljot ljotolf lodin mord odd ofeig ogmund olaf olvir onund orm otkel otrygg ottar ozur ragnar rognvald runolf sam sighvat sigmund sigtrygg sigulf sigurd sigwulf skapti snorri solmund solvi starkad stein steinkel steinthor sturla styrkar sumarlidi svein thjodolf thjostolf thorarin thorbjorn thorbrand thord thorfinn thorgeir thorgest thorgils thorgrim thorhall thorir thorkel thormod thorstein thorvald thrain thrand tosti ulf uni vagn valgard vandrad vermund vestein vigfus yngvar";
            m_pMale = new Confluxer(sMale, 2);
        }

        #region ILanguage Members

        public override string RandomFemaleName()
        {
            return m_pFemale.Generate();
        }

        public override string RandomMaleName()
        {
            return m_pMale.Generate();
        }

        #endregion
    }
}
