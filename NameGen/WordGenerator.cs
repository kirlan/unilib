using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using System.Globalization;

namespace NameGen
{
    public static class WordGenerator
    {
        public enum Language
        {
            English,
            Eskimoid,
            French,
            Hawaiian,
            Japanese,
            Latin,
            VictorianEnglish
        };

        private static string Capitalize(string value)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value);
        }

        private static Dictionary<Language, Dictionary<char, string[]>> s_cRules = null;

        internal static Dictionary<Language, Dictionary<char, string[]>> Rules
        {
            get
            {
                if (s_cRules == null)
                {
                    s_cRules = new Dictionary<Language, Dictionary<char, string[]>>();

                    s_cRules[Language.English] = new Dictionary<char, string[]>();
                    //Words
                    s_cRules[Language.English]['W'] = new[]{ "CT", "CT", "CX", "CDF", "CVFT", "CDFU", "CTU", "IT", "ICT", "A" };
                    //Latinate words
                    s_cRules[Language.English]['A'] = new[]{ "KVKVtion" };
                    s_cRules[Language.English]['K'] = new[]{ "b", "c", "d", "f", "g", "j", "l", "m", "n", "p", "qu", "r", "s", "t", "v", "sP" };
                    //# Prefixes
                    s_cRules[Language.English]['I'] = new[]{ "ex", "in", "un", "re", "de" };
                    //# Ends of Words
                    s_cRules[Language.English]['T'] = new[]{ "VF", "VEe" };
                    //# Suffixes
                    s_cRules[Language.English]['U'] = new[]{ "er", "ish", "ly", "en", "ing", "ness", "ment", "able", "ive" };
                    //# Consonants
                    s_cRules[Language.English]['C'] = new[]{ "b", "c", "ch", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "qu", "r", "s", "sh", "t", "th", "v", "w", "y", "sP", "Rr", "Ll" };
                    //# Occurring before silent "e"
                    s_cRules[Language.English]['E'] = new[]{ "b", "c", "ch", "d", "f", "g", "dg", "l", "m", "n", "p", "r", "s", "t", "th", "v", "z" };
                    //# Final letters
                    s_cRules[Language.English]['F'] = new[]{ "b", "tch", "d", "ff", "g", "gh", "ck", "ll", "m", "n", "n", "ng", "p", "r", "ss", "sh", "t", "tt", "th", "x", "y", "zz", "rR", "sP", "lL" };
                    //# Voiceless stops
                    s_cRules[Language.English]['P'] = new[]{ "p", "t", "k", "c" };
                    //# Voiced stops
                    s_cRules[Language.English]['Q'] = new[]{ "b", "d", "g" };
                    //# Can be next to "l"
                    s_cRules[Language.English]['L'] = new[]{ "b", "f", "k", "p", "s" };
                    //# Can be next to "r"
                    s_cRules[Language.English]['R'] = new[]{ "P", "Q", "f", "th", "sh" };
                    //# Simple vowels
                    s_cRules[Language.English]['V'] = new[]{ "a", "e", "i", "o", "u" };
                    //# Dipthongs
                    s_cRules[Language.English]['D'] = new[]{ "aw", "ei", "ow", "ou", "ie", "ea", "ai", "oy" };
                    //# Final vowels/dipthongs
                    s_cRules[Language.English]['X'] = new[]{ "e", "i", "o", "aw", "ow", "oy" };

                    s_cRules[Language.Eskimoid] = new Dictionary<char, string[]>();
                    s_cRules[Language.Eskimoid]['W'] = new[]{ "IVMF", "IVF", "XMVC", "IVVnZ", "IVMZ", "XX", "ZX", "XZF" };
                    s_cRules[Language.Eskimoid]['I'] = new[]{ "p", "py", "t", "ty", "k", "ky", "ch", "chy", "s", "sy" };
                    s_cRules[Language.Eskimoid]['V'] = new[]{ "a", "i", "u" };
                    s_cRules[Language.Eskimoid]['M'] = new[]{ "p", "t", "k", "pp", "tt", "kk", "tch", "ss" };
                    s_cRules[Language.Eskimoid]['F'] = new[]{ "akya", "it", "in", "ikta", "ukta" };
                    s_cRules[Language.Eskimoid]['Z'] = new[]{ "ippik", "issik", "innik", "utta", "ani", "achka" };
                    s_cRules[Language.Eskimoid]['X'] = new[]{ "VC", "VCVC", "VCV" };
                    s_cRules[Language.Eskimoid]['C'] = new[]{ "p", "t", "k", "n", "ch" };

                    s_cRules[Language.French] = new Dictionary<char, string[]>();
                    //# Basic word shapes
                    s_cRules[Language.French]['W'] = new[]{ "CDF", "CVnAX", "CVnAU", "CVnAT", "CVMT", "CDMU", "CVMU", "IVMT", "ECT", "CVZX" };
                    //# Prefixes
                    s_cRules[Language.French]['I'] = new[]{ "in", "ad", "con", "des", "mal", "pour", "sous" };
                    //# V Prefixes
                    s_cRules[Language.French]['E'] = new[]{ "entre", "re" };
                    //# Ends of words
                    s_cRules[Language.French]['T'] = new[]{ "VF" };
                    //# Medial consonants
                    s_cRules[Language.French]['M'] = new[]{ "l", "ll", "t", "ss", "n", "m" };
                    //# Affixes
                    s_cRules[Language.French]['U'] = new[]{ "eur", "ien", "ant", "esse", "ent", "able", "oir", "eau", "aire", "erie", "e", "er", "ir", "ain", "age", "ule", "on", "ade" };
                    //# Consonants
                    s_cRules[Language.French]['C'] = new[]{ "b", "c", "ch", "d", "f", "g", "j", "l", "m", "n", "p", "qu", "r", "s", "t", "v", "sP", "Rr", "Ll" };
                    //# Finals
                    s_cRules[Language.French]['F'] = new[]{ "c", "f", "gne", "m", "n", "nt", "p", "r", "sse", "t", "s", "l" };
                    //# Other finals
                    s_cRules[Language.French]['Z'] = new[]{ "c", "f", "gn", "m", "n", "nt", "p", "r", "t", "s", "l" };
                    //# Finals after nasals
                    s_cRules[Language.French]['A'] = new[]{ "c", "p", "s", "t" };
                    //# Voiceless stops
                    s_cRules[Language.French]['P'] = new[]{ "p", "t" };
                    //# Voiced stops
                    s_cRules[Language.French]['Q'] = new[]{ "b", "d", "g" };
                    //# Can be next to L
                    s_cRules[Language.French]['L'] = new[]{ "b", "f", "p", "c" };
                    //# Can be next to R
                    s_cRules[Language.French]['R'] = new[]{ "P", "Q", "f" };
                    //# Simple vowels
                    s_cRules[Language.French]['V'] = new[]{ "a", "e", "i", "o", "u" };
                    //# Diphthongs
                    s_cRules[Language.French]['D'] = new[]{ "au", "ai", "oi", "ou", "ie", "eau", "oeu" };
                    //# Final vowels or diphthongs
                    s_cRules[Language.French]['X'] = new[]{ "ee", "e", "ou", "ie", "eau", "oi" };

                    s_cRules[Language.Hawaiian] = new Dictionary<char, string[]>();
                    s_cRules[Language.Hawaiian]['W'] = new[]{ "S", "SS", "SCV", "CVS" };
                    s_cRules[Language.Hawaiian]['S'] = new[]{ "CVCD", "CDCV", "CVCV" };
                    s_cRules[Language.Hawaiian]['C'] = new[]{ "h", "k", "l", "m", "p", "n", "'", "w" };
                    s_cRules[Language.Hawaiian]['V'] = new[]{ "a", "e", "i", "o", "u" };
                    s_cRules[Language.Hawaiian]['D'] = new[]{ "ai", "ae", "au", "ao", "ei", "eu", "oi", "ou", "iu" };

                    s_cRules[Language.Japanese] = new Dictionary<char, string[]>();
                    s_cRules[Language.Japanese]['W'] = new[]{ "SSV", "SSV", "VSS", "VSSS", "SnSS", "SVSS", "VnVSS", "A", "A", "A", "A", "A", "B", "B", "B", "B", "B", "C", "C", "C", "C", "C", "F", "F", "F", "F", "F", "G", "G", "G", "G", "G", "H", "H", "H", "H", "H", "I", "I", "I", "I", "I" };
                    s_cRules[Language.Japanese]['A'] = new[]{ "akiO", "haruO", "natsuO", "fuyuO", "kazuO", "yoshiO", "tomoO", "tatsuO", "tetsuO", "takaO", "noriO" };
                    s_cRules[Language.Japanese]['B'] = new[]{ "mitsuO", "yasuO", "yukiO", "itsuO", "masaO", "hiroO", "nobuO", "takeO", "asaO", "kuniO", "kiyoO" };
                    s_cRules[Language.Japanese]['C'] = new[]{ "shizuO", "takiO", "hisaO", "mikiO", "yasuO", "shinO" };
                    s_cRules[Language.Japanese]['F'] = new[]{ "aiP", "akiP", "asaP", "ayaP", "ayuP", "ikuP", "itsuP", "utaP", "emiP", "kazuP", "katsuP", "kiP", "kimiP", "kikuP" };
                    s_cRules[Language.Japanese]['G'] = new[]{ "kiyoP", "kumiP", "kotoP", "sanaP", "shizuP", "suzuP", "tamiP", "takiP", "tatsuP", "takaP", "tomoP", "chinaP" };
                    s_cRules[Language.Japanese]['H'] = new[]{ "chikaP", "nanaP", "noriP", "haruP", "hanaP", "hisaP", "fumiP", "maiP", "masaP", "miyaP", "mamiP", "makiP" };
                    s_cRules[Language.Japanese]['I'] = new[]{ "minaP", "mikiP", "mitsuP", "yasuP", "yukiP", "yoshiP", "riP", "rikaP", "rinaP", "ritsuP", "reiP", "wakaP" };
                    s_cRules[Language.Japanese]['O'] = new[]{ "o", "hiko", "taro", "yoshi", "ro", "aki", "toshi", "fumi", "kazu", "ji", "shi", "ma", "ya" };
                    s_cRules[Language.Japanese]['P'] = new[]{ "ko", "ko", "ko", "ko", "e", "mi", "yo", "ka", "ri" };
                    s_cRules[Language.Japanese]['V'] = new[]{ "a", "i", "u", "e", "o" };
                    s_cRules[Language.Japanese]['S'] = new[]{ "kV", "nV", "hV", "mV", "rV", "gV", "pV", "bV", "tT", "wa", "U", "U", "U", "zZ", "zZ" };
                    s_cRules[Language.Japanese]['T'] = new[]{ "a", "su", "e", "o" };
                    s_cRules[Language.Japanese]['Y'] = new[]{ "a", "u", "o" };
                    s_cRules[Language.Japanese]['Z'] = new[]{ "u", "e", "o" };
                    s_cRules[Language.Japanese]['U'] = new[]{ "yY", "ji", "dD", "vu" };
                    s_cRules[Language.Japanese]['D'] = new[]{ "a", "e", "o" };

                    s_cRules[Language.Latin] = new Dictionary<char, string[]>();
                    s_cRules[Language.Latin]['W'] = new[]{ "Y", "VF", "YF", "YF", "YXF", "YXF", "YXYF", "B" };
                    s_cRules[Language.Latin]['B'] = new[]{ "PSB", "YNF", "YNXF", "YNXF", "YNXYF", "YXYNF" };
                    s_cRules[Language.Latin]['P'] = new[]{ "pre", "re", "con", "in", "anti", "de", "ad", "bi", "ambi", "dis", "a", "e", "i", "o", "u" };
                    s_cRules[Language.Latin]['Y'] = new[]{ "Ca", "Ce", "Ci", "Co", "Cu", "Cy", "Da", "De", "Di", "Do", "Dy" };
                    s_cRules[Language.Latin]['X'] = new[]{ "Ce", "Ci", "Co", "o" };
                    s_cRules[Language.Latin]['V'] = new[]{ "a", "e", "i", "o", "u", "y" };
                    s_cRules[Language.Latin]['F'] = new[]{ "Ca", "Ce", "Cy", "Cic", "Cous", "Cate", "Cal", "Cion", "Cent", "Cist", "Cian", "Cium" };
                    s_cRules[Language.Latin]['C'] = new[]{ "b", "c", "ch", "d", "g", "l", "m", "n", "p", "ph", "ps", "r", "s", "t", "th", "v", "x", "z", "Rr", "sH", "Ll" };
                    s_cRules[Language.Latin]['D'] = new[]{ "b", "c", "ch", "d", "g", "l", "m", "n", "p", "ph", "ps", "r", "s", "t", "th", "v", "x", "z", "Rr", "sH", "Ll", "h", "qu", "gn", "pt" };
                    s_cRules[Language.Latin]['R'] = new[]{ "b", "c", "ch", "g", "p", "ph", "t", "th" };
                    s_cRules[Language.Latin]['L'] = new[]{ "c", "ch", "p", "g", "f" };
                    s_cRules[Language.Latin]['H'] = new[]{ "c", "ch", "cl", "m", "n", "p", "ph", "pl", "pr", "qu", "t", "th" };
                    s_cRules[Language.Latin]['N'] = new[]{ "n", "c", "l", "r" };

                    s_cRules[Language.VictorianEnglish] = new Dictionary<char, string[]>();
                    s_cRules[Language.VictorianEnglish]['W'] = new[]{ "BVMIVF", "BVME", "OE", "BVMIVF", "BVME", "OE", "BVMIVF", "BVME" };
                    s_cRules[Language.VictorianEnglish]['B'] = new[]{ "b", "b", "c", "d", "d", "f", "f", "g", "g", "h", "h", "m", "n", "p", "p", "s", "s", "w", "ch", "br", "cr", "dr", "bl", "cl", "s" };
                    s_cRules[Language.VictorianEnglish]['I'] = new[]{ "b", "d", "f", "h", "k", "l", "m", "n", "p", "s", "t", "w", "ch", "st" };
                    s_cRules[Language.VictorianEnglish]['V'] = new[]{ "a", "e", "i", "o", "u" };
                    s_cRules[Language.VictorianEnglish]['M'] = new[]{ "ving", "zzle", "ndle", "ddle", "ller", "rring", "tting", "nning", "ssle", "mmer", "bber", "bble", "nger", "nner", "sh", "ffing", "nder", "pper", "mmle", "lly", "bling", "nkin", "dge", "ckle", "ggle", "mble", "ckle", "rry" };
                    s_cRules[Language.VictorianEnglish]['F'] = new[]{ "t", "ck", "tch", "d", "g", "n", "t", "t", "ck", "tch", "dge", "re", "rk", "dge", "re", "ne", "dging" };
                    s_cRules[Language.VictorianEnglish]['O'] = new[]{ "small", "snod", "bard", "billing", "black", "shake", "tilling", "good", "worthing", "blythe", "green", "duck", "pitt", "grand", "brook", "blather", "bun", "buzz", "clay", "fan", "dart", "grim", "honey", "light", "murd", "nickle", "pick", "pock", "trot", "toot", "turvey" };
                    s_cRules[Language.VictorianEnglish]['E'] = new[]{ "shaw", "man", "stone", "son", "ham", "gold", "banks", "foot", "worth", "way", "hall", "dock", "ford", "well", "bury", "stock", "field", "lock", "dale", "water", "hood", "ridge", "ville", "spear", "forth", "will" };
                    s_cRules[Language.VictorianEnglish]['Z'] = new[]{ "hanson", "winner", "smythe", "carlton", "adams", "foster", "case", "burton", "bates", "beck", "baker", "burdick", "dudley", "duncan", "goodwin", "hope", "lewis", "turner", "macaulay", "bennett", "harris" };
                }
                return s_cRules;
            }
        }

        public static string GetWord(Language eLanguage)
        {
            string sWord = Rules[eLanguage]['W'][Rnd.Get(Rules[eLanguage]['W'].Length)];

            bool bFinished = false;

            do
            {
                bFinished = true;
                for (int i = sWord.Length - 1; i >= 0; i--)
                {
                    if (Rules[eLanguage].ContainsKey(sWord[i]))
                    {
                        string sReplacement = Rules[eLanguage][sWord[i]][Rnd.Get(Rules[eLanguage][sWord[i]].Length)];
                        sWord = sWord.Remove(i, 1);
                        sWord = sWord.Insert(i, sReplacement);
                        bFinished = false;
                    }
                }
            }
            while (!bFinished);

            return Capitalize(sWord);
        }
    }
}
