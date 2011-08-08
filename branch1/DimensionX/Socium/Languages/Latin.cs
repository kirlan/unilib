using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NameGen;
using Random;

namespace Socium.Languages
{
    class Latin:Language
    {
        private Confluxer m_pName;

        private Confluxer m_pFamily;

        private Confluxer m_pCountry;

        private Confluxer m_pTown;

        private Confluxer m_pVillage;

        private Confluxer m_pNations;

        public Latin()
            : base(NameGenerator.Language.NONE)
        {
            string sName = "Gai Luci Marc Publi Quint Tit Tif Tiberi Sext Aul Decim Gnae Spuri Mani Servi Api Vibi Numi";
            m_pName = new Confluxer(sName, 2);

            string sFamily = "Acil Aebut Ael Aemil Alb Amat Ambros Ane An Anton Apolon Ar Artor Asin Atil At Aurel Autron Caecil Caedic Cael Calid Calpurn Cas Claud Cloel Coce Comin Cornel Coruncan Curiat Cur Curt Dec Did Domit Duil Durm Equit Fab Fabric Fann Flav Fulv Fur Gabin Galer Gegan Gel Gemin Genuc Grat Heren Hirt Horat Hortens Hostil Iul Iun Iuvent Lael Lart Licin Liv Lucil Lucret Manl Marc Mar Mem Menen Minic Min Minuc Mod Moc Naev Naut Numer Numic Octav Ovid Papir Petron Pinar Pompe Pompil Pont Popil Porc Postum Quinctil Quinct Rubel Ruf Rutil Salust Salon Salv Scribon Se Sempron Sent Serg Sertor Servil Sext Sicin Sueton Sulpic Tarpe Tarquit Terent Titin Titur Tuc Tul Ulp Valer Ved Vele Vergil Vergin Vib Vil Vipsan Vitel Vitruv Volumni";
            m_pFamily = new Confluxer(sFamily, 2);

            string sCountry = "hibern caledon britan belg aquitan lusitan baet mauretan ital raet dalmat dac moes corsoc sardin sicil macedon acha as thrac sarmat iber armen asyr syr galat capadoc gaetul phazan roman numid panon venet ligur flamin valer umbr apul galabr lucan";
            m_pCountry = new Confluxer(sCountry, 2);

            string sTown = "eborac treveror vindelicor  tolet mediolan lauriac salonac apul durostor philipop bizant limon lutet lugdun narbon cremon buthrot iliric pont noric picen samin garianon siluri durovern cantiacor noviomag corini camulodun corstopit dubr durocobri glev halifaci hortoni vect coritanor lind londini mamuci aeli verulami durocornovi lactodor mediolan viroconi lucent singidun gesoriac posoni aquinc corial oenipont byzani lugdun parisior spalat argentorat trebecor taurin traiect eridit tigur turic";
            m_pTown = new Confluxer(sTown, 2);

            string sVillage = "aqu causen durobriv rat nice basil aec lupi stabi siracus velitr vercel vigili volater arnemeti rutupi salin vent brug mari torn raum grati vil ar flavi dresd embd hanover lipsi lubic pat tubing agri regi cirpi contra intercis matric mursel solv salin castr eblan horgan silv vien vad calisi sirad bals bracar leuc cel lori dertus egar arbel athen cans edes insul";
            m_pVillage = new Confluxer(sVillage, 2);

            string sNation = "rom tit lucer palat subur esquil aemil fab faler galer enen papir pol quir romil sabat stel ter trom vel volt vest mar nept oct trib polib pob lem";
            m_pNations = new Confluxer(sNation, 1);
        }

        #region ILanguage Members

        protected override string GetNationName()
        {
            return m_pNations.Generate() + "an";
        }

        protected override string GetCountryName()
        {
            return m_pCountry.Generate() + "ia";
            //            return WordGenerator.GetWord(WordGenerator.Language.Japanese);
        }

        protected override string GetTownName()
        {
            return m_pTown.Generate() + "um";
        }

        protected override string GetVillageName()
        {
            return m_pVillage.Generate() + (Rnd.OneChanceFrom(2) ? "ae":"a");
        }

        protected override string GetFamily()
        {
            return m_pFamily.Generate() + "ius";
        }

        public override string RandomFemaleName()
        {
            return m_pName.Generate() + "a";
        }

        public override string RandomMaleName()
        {
            return m_pName.Generate() + "us";
        }

        #endregion
    }
}
