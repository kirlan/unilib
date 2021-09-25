using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NameGen;

namespace RB.Socium.Languages
{
    public abstract class Language
    {
        #region Static Members
        private enum Lang
        { 
            Arabian,
            Asian,
            Dwarwen,
            Elven,
            Eskimoid,
            European,
            Northman,
            Orkish,
            Slavic,
            African,
            Aztec,
            Greek,
            Highlander,
            Drow,
            Hindu,
            Latin
        }

        private static Dictionary<Lang, Language> s_cLanguages = new Dictionary<Lang,Language>();

        public static void ResetUsedLists()
        {
            foreach(Language pLang in s_cLanguages.Values)
                pLang.Reset();
        }

        public static Language Arabian
        {
            get
            {
                if (!s_cLanguages.ContainsKey(Lang.Arabian))
                    s_cLanguages[Lang.Arabian] = new Arabian();

                return s_cLanguages[Lang.Arabian];
            }
        }

        public static Language Asian
        {
            get
            {
                if (!s_cLanguages.ContainsKey(Lang.Asian))
                    s_cLanguages[Lang.Asian] = new Asian();

                return s_cLanguages[Lang.Asian];
            }
        }

        public static Language Dwarwen
        {
            get
            {
                if (!s_cLanguages.ContainsKey(Lang.Dwarwen))
                    s_cLanguages[Lang.Dwarwen] = new Dwarwen();

                return s_cLanguages[Lang.Dwarwen];
            }
        }

        public static Language Elven
        {
            get
            {
                if (!s_cLanguages.ContainsKey(Lang.Elven))
                    s_cLanguages[Lang.Elven] = new Elven();

                return s_cLanguages[Lang.Elven];
            }
        }

        public static Language Eskimoid
        {
            get
            {
                if (!s_cLanguages.ContainsKey(Lang.Eskimoid))
                    s_cLanguages[Lang.Eskimoid] = new Eskimoid();

                return s_cLanguages[Lang.Eskimoid];
            }
        }

        public static Language European
        {
            get
            {
                if (!s_cLanguages.ContainsKey(Lang.European))
                    s_cLanguages[Lang.European] = new European();

                return s_cLanguages[Lang.European];
            }
        }

        public static Language Northman
        {
            get
            {
                if (!s_cLanguages.ContainsKey(Lang.Northman))
                    s_cLanguages[Lang.Northman] = new Northman();

                return s_cLanguages[Lang.Northman];
            }
        }

        public static Language Orkish
        {
            get
            {
                if (!s_cLanguages.ContainsKey(Lang.Orkish))
                    s_cLanguages[Lang.Orkish] = new Orkish();

                return s_cLanguages[Lang.Orkish];
            }
        }

        public static Language Slavic
        {
            get
            {
                if (!s_cLanguages.ContainsKey(Lang.Slavic))
                    s_cLanguages[Lang.Slavic] = new Slavic();

                return s_cLanguages[Lang.Slavic];
            }
        }

        public static Language African
        {
            get
            {
                if (!s_cLanguages.ContainsKey(Lang.African))
                    s_cLanguages[Lang.African] = new African();

                return s_cLanguages[Lang.African];
            }
        }

        public static Language Aztec
        {
            get
            {
                if (!s_cLanguages.ContainsKey(Lang.Aztec))
                    s_cLanguages[Lang.Aztec] = new Aztec();

                return s_cLanguages[Lang.Aztec];
            }
        }

        public static Language Greek
        {
            get
            {
                if (!s_cLanguages.ContainsKey(Lang.Greek))
                    s_cLanguages[Lang.Greek] = new Greek();

                return s_cLanguages[Lang.Greek];
            }
        }

        public static Language Latin
        {
            get
            {
                if (!s_cLanguages.ContainsKey(Lang.Latin))
                    s_cLanguages[Lang.Latin] = new Latin();

                return s_cLanguages[Lang.Latin];
            }
        }

        public static Language Highlander
        {
            get
            {
                if (!s_cLanguages.ContainsKey(Lang.Highlander))
                    s_cLanguages[Lang.Highlander] = new Highlander();

                return s_cLanguages[Lang.Highlander];
            }
        }

        public static Language Drow
        {
            get
            {
                if (!s_cLanguages.ContainsKey(Lang.Drow))
                    s_cLanguages[Lang.Drow] = new Drow();

                return s_cLanguages[Lang.Drow];
            }
        }

        public static Language Hindu
        {
            get
            {
                if (!s_cLanguages.ContainsKey(Lang.Hindu))
                    s_cLanguages[Lang.Hindu] = new Hindu();

                return s_cLanguages[Lang.Hindu];
            }
        }

        private static List<string> UsedNames = new List<string>();

        #endregion

        private NameGenerator.Language m_eLanguage;

        public Language(NameGenerator.Language eLanguage)
        {
            m_eLanguage = eLanguage;
        }

        public void Reset()
        {
            UsedNames.Clear();
        }

        public string RandomNationName()
        {
            string sName;
            int iCounter = 0;
            do
            {
                sName = GetNationName();
                iCounter++;
            }
            while (UsedNames.Contains(sName) && iCounter < 10);

            UsedNames.Add(sName);
            return sName;
        }

        public string RandomCountryName()
        { 
            string sName;
            int iCounter = 0;
            do
            {
                sName = GetCountryName();
                iCounter++;
            }
            while (UsedNames.Contains(sName) && iCounter < 10);

            UsedNames.Add(sName);
            return sName;
        }

        public string RandomTownName()
        { 
            string sName;
            int iCounter = 0;
            do
            {
                sName = GetTownName();
                iCounter++;
            }
            while (UsedNames.Contains(sName) && iCounter < 10);

            UsedNames.Add(sName);
            return sName;
        }

        public string RandomVillageName()
        {
            string sName;
            int iCounter = 0;
            do
            {
                sName = GetVillageName();
                iCounter++;
            }
            while (UsedNames.Contains(sName) && iCounter < 10);

            //UsedNames.Add(sName);
            return sName;
        }

        public string RandomSurname()
        {
            string sName;
            int iCounter = 0;
            do
            {
                sName = GetFamily();
                iCounter++;
            }
            while (UsedNames.Contains(sName) && iCounter < 10);

            UsedNames.Add(sName);
            return sName;
        }

        protected virtual string GetNationName()
        {
            return NameGenerator.GetEthnicName(m_eLanguage);
        }

        protected virtual string GetCountryName()
        {
            return NameGenerator.GetEthnicName(m_eLanguage);
        }

        protected virtual string GetTownName()
        {
            return NameGenerator.GetEthnicName(m_eLanguage);
        }

        protected virtual string GetVillageName()
        {
            return NameGenerator.GetEthnicName(m_eLanguage);
        }

        protected virtual string GetFamily()
        {
            return NameGenerator.GetEthnicName(m_eLanguage);
        }

        public virtual string RandomFemaleName()
        {
            return NameGenerator.GetHeroName(false);
        }

        public virtual string RandomMaleName()
        {
            return NameGenerator.GetHeroName(true);
        }
    }
}
