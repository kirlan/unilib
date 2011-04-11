using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using System.Globalization;

namespace NameGen
{
    public class NameGenerator2
    {
        public enum Language
        {
            Dwarf,
            Elf,
            Gnome,
            Halfling,
            Saxon,
            Orc,
            Japaneze1,
            Japaneze2,
            Chinese,
            Hawaiian1,
            Hawaiian2,
            Hindu,
            ANY
        };

        public static string GetCountryName()
        {
            string[] syllable_1 = new string[] { "Aqu", "Bos", "Ven", "Cor", "Aghr", "Aren", "Shad", "Bel", "Dar", "Nem", "Han", "Num", "Py", "Nord", "Oph", "Fr", "I", "L", "Mec", "Ron", "Sh", "Ter", "Abb", "Akb", "An", "Asg", "Er", "Gh", "K", "Lib", "Pel", "Sab", "St", "Har", "Kh", "Neb", "Nil", "T", "Th", "Ak", "Khor", "Sam", "Vil", "Cyl", "Mik", "Gon", "R", "Mor", "Cim" };
            string[] syllable_2 = new string[] { "i", "o", "a", "e", "u", "y", "ai", "", "ae", "ea", "ia" };
            string[] syllable_3 = new string[] { "lonia", "nia", "rium", "nthium", "pur", "jun", "zar", "verus", "far", "dia", "mar", "lia", "thon", "heim", "r", "sol", "nthe", "dier", "nta", "co", "mu", "m", "son", "drah", "tania", "kia", "lum", "k", "za", "ros", "num", "shtia", "tea", "mir", "shan", "gia", "kh", "jar", "mi", "xur", "thu", "lus", "khmet", "a", "ran", "f", "sun", "ra", "hpur", "yet", "ska", "lannon", "land", "dor", "han", "ria" };
            return BuildName(syllable_1, syllable_2, syllable_3);
        }

        public static string GetTownName()
        {
            string[] syllable_1 = new string[] { "Abing", "Al", "Ald", "Aln", "Ames", "Amp", "Ash", "At", "Ave", "Aving", "Ax", "Back", "Bake", "Bamp", "Ban", "Beck", "Ber", "Berke", "Bevers", "Bi", "Bick", "Bin", "Block", "Bol", "Bos", "Bottes", "Bow", "Brad", "Brans", "Brat", "Bre", "Bree", "Bridg", "Brink", "Bris", "Brom", "Broom", "Bud", "Cad", "Caer", "Came", "Car", "Cart", "Castle", "Cavers", "Charter", "Ched", "Chew", "Chippen", "Coly", "Corn", "Cors", "Cran", "Credi", "Crick", "Crow", "Culm", "Dagger", "Dart", "Dedding", "Deer", "Din", "Ditte", "Dittis", "Dor", "Dragon", "Drif", "Dry", "Dun", "Dur", "Dwarf", "East", "Ebring", "Eding", "Elf", "Elk", "En", "Erming", "Exe", "Fair", "Faring", "Flad", "Fording", "Forth", "Framp", "From", "Gis", "Glas", "Gnome", "Goblin", "Gras", "Grey", "Guis", "Hail", "Hart", "Haver", "Helm", "Here", "Hex", "Hol", "Hop", "In", "Kelm", "Ken", "Kew", "Kil", "King", "Kirk", "Knight", "La", "Lam", "Lan", "Laner", "Laving", "Led", "Leo", "Lindis", "Lyd", "Lymp", "Mal", "Malmes", "Marsh", "Mel", "Mell", "Minchin", "Monk", "Mont", "Mow", "Muchel", "Net", "Nether", "Nev", "New", "Nib", "North", "Pen", "Per", "Pether", "Pew", "Pris", "Rad", "Rend", "Ring", "Rip", "Rock", "Rom", "Roth", "Sapper", "Sel", "Seming", "Shaftes", "Shield", "Shob", "Shrews", "Sid", "Sken", "Skip", "Somer", "South", "Spear", "Staf", "Stan", "Stan", "Staple", "Staun", "Stoke", "Sword", "Syd", "Taun", "Tavi", "Tel", "Tewkes", "Tint", "Titch", "Tiver", "Tort", "Tot", "Trout", "Uff", "Uffing", "Ulvers", "Uplea", "Urch", "Wan", "War", "Wel", "Wen", "West", "Whit", "Wide", "Wim", "Winch", "Wit", "Withing", "Wood", "Woot", "Wor", "Wot", "Wring", "Yat" };
            string[] syllable_2 = new string[] { "bane", "beck", "borne", "borough", "bourn", "bourne", "bray", "bridge", "burgh", "burn", "burton", "bury", "by", "chester", "comb", "combe", "con", "cost", "culme", "dal", "der", "dish", "don", "dor", "e", "east", "ent", "ern", "es", "farn", "fel", "field", "font", "ford", "frith", "glade", "glen", "gold", "gomery", "ham", "hampton", "house", "how", "hurst", "iard", "keep", "kirk", "lade", "land", "leigh", "leon", "ley", "lingham", "low", "meet", "mel", "mere", "minster", "moot", "mouth", "nard", "ne", "nes", "newton", "ney", "noller", "nor", "on", "pas", "peck", "rest", "ridge", "scott", "sey", "shire", "silver", "sley", "spring", "stock", "stoke", "ston", "stone", "sward", "swear", "tage", "ter", "tol", "ton", "ton", "ton", "ton", "ton", "ton", "ton", "ton", "ton", "ton", "town", "vale", "vern", "wall", "water", "well", "went", "west", "wick", "wood", "worth", "worthy", "yard" };
            return BuildName(syllable_1, syllable_2);
        }

        public static string GetTavernName()
        {
            string[] adjective_1 = new string[] { "Dancing", "Laughing", "Running", "Prancing", "Drunken", "Flying", "Sleeping", "Leaping", "Fighting", "Sleeping", "Red", "Green", "Blue", "Yellow", "White", "Black", "Rusty", "Silver", "Golden", "Shiny", "Bronze", "Iron" };
            string[] noun_1 = new string[] { "Badger", "Bear", "Beaver", "Boar", "Bull", "Cat", "Cow", "Dragon", "Dog", "Deer", "Duck", "Dwarf", "Elf", "Elk", "Eagle", "Fairy", "Ferret", "Gnome", "Goblin", "Goat", "Goose", "Hen", "Lamb", "Lion", "Orc", "Ogre", "Pig", "Pony", "Rooster", "Sheep", "Troll", "Unicorn" };
            string[] adjective_2 = new string[] { "Red", "Green", "Blue", "Yellow", "White", "Black", "Rusty", "Silver", "Golden", "Shiny", "Bronze", "Iron" };
            string[] noun_2 = new string[] { "Tree", "Bucket", "Shield", "Sword", "Spear", "Bow", "Arrow", "Axe", "Barrel", "Keg", "Tap", "Mug", "Chalice", "Helm", "Wheel", "Saw", "Plow", "Bell", "Crown", "Ship", "Sun", "Moon", "Star", "Coin", "Bottle" };
            int nameType = Rnd.Get(5);
            switch (nameType)
            {
                case 0:
                    return BuildName(adjective_1) + " " + BuildName(noun_1);
                case 1:
                    return BuildName(adjective_2) + " " + BuildName(noun_2);
                case 2:
                    return BuildName(noun_1) + "'s Head";
                case 3:
                    return BuildName(noun_1) + " and " + BuildName(noun_1);
                case 4:
                    return BuildName(noun_2) + " and " + BuildName(noun_2);
            }
            throw new Exception("Wrong name type!");
        }

        public static string GetHeroName(Language eLanguage)
        {
            while(eLanguage == Language.ANY)
                eLanguage = (Language)Rnd.Get(typeof(Language));

            switch (eLanguage)
            { 
                case Language.Dwarf:
                    return DwarfNames();
                case Language.Elf:
                    return ElfNames();
                case Language.Gnome:
                    return GnomeNames();
                case Language.Halfling:
                    return HalflingNames();
                case Language.Saxon:
                    return SaxonNames();
                case Language.Orc:
                    return OrcNames();
                case Language.Japaneze1:
                    return Japan1Names();
                case Language.Japaneze2:
                    return Japan2Names();
                case Language.Chinese:
                    return ChinaNames();
                case Language.Hawaiian1:
                    return Hawaii1Names();
                case Language.Hawaiian2:
                    return Hawaii2Names();
                case Language.Hindu:
                    return HinduNames();
                default:
                    throw new ArgumentException("Unknown language!");
            }
        }

        private static string BuildName(string[] syllable_1)
        {
            return syllable_1[Rnd.Get(syllable_1.Length)];
        }

        private static string BuildName(string[] syllable_1, string[] syllable_2)
        {
            return syllable_1[Rnd.Get(syllable_1.Length)] + syllable_2[Rnd.Get(syllable_2.Length)];
        }

        private static string BuildName(string[] syllable_1, string[] syllable_2, string[] syllable_3)
        {
            return syllable_1[Rnd.Get(syllable_1.Length)] + syllable_2[Rnd.Get(syllable_2.Length)] + syllable_3[Rnd.Get(syllable_3.Length)];
        }

        private static string Capitalize(string value)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase (value);
        }

        private static string DwarfNames()
        {
            string[] syllable_1 = new string[] {"B", "D", "F", "G", "Gl", "H", "K", "L", "M", "N", "R", "S", "T", "Th", "V"};
            string[] syllable_2 = new string[] {"a", "e", "i", "o", "oi", "u"};
            string[] syllable_3 = new string[] {"bur", "fur", "gan", "gnus", "gnar", "li", "lin", "lir", "mli", "nar", "nus", "rin", "ran", "sin", "sil", "sur"};
            return BuildName(syllable_1, syllable_2, syllable_3);
        }

        private static string ElfNames()
        {
            string[] syllable_1 = new string[] {"Al", "An", "Bal", "Bel", "Cal", "Cel", "El", "Ell", "Elr", "Elv", "Eow", "Eдr", "F", "Fal", "Fel", "Fin", "G", "Gal", "Gel", "Gl", "Is", "Lan", "Leg", "Lуm", "N", "Nal", "Nel", "S", "Sal", "Sel", "T", "Tal", "Tel", "Thr", "Tin"};
            string[] syllable_2 = new string[] {"a", "б", "adrie", "ara", "e", "й", "ebri", "ele", "ere", "i", "io", "ithra", "ilma", "il-Ga", "ili", "o", "orfi", "у", "u", "y"};
            string[] syllable_3 = new string[] {"l", "las", "lad", "ldor", "ldur", "lindл", "lith", "mir", "n", "nd", "ndel", "ndil", "ndir", "nduil", "ng", "mbor", "r", "rith", "ril", "riand", "rion", "s", "ssar", "thien", "viel", "wen", "wyn"};
            return BuildName(syllable_1, syllable_2, syllable_3);
        }

        private static string GnomeNames()
        {
            string[] syllable_1 = new string[] {"Aar", "An", "Ar", "As", "C", "H", "Han", "Har", "Hel", "Iir", "J", "Jan", "Jar", "K", "L", "M", "Mar", "N", "Nik", "Os", "Ol", "P", "R", "S", "Sam", "San", "T", "Ter", "Tom", "Ul", "V", "W", "Y"};
            string[] syllable_2 = new string[] {"a", "aa", "ai", "e", "ei", "i", "o", "uo", "u", "uu"};
            string[] syllable_3 = new string[] {"ron", "re", "la", "ki", "kseli", "ksi", "ku", "ja", "ta", "na", "namari", "neli", "nika", "nikki", "nu", "nukka", "ka", "ko", "li", "kki", "rik", "po", "to", "pekka", "rjaana", "rjatta", "rjukka", "la", "lla", "lli", "mo", "nni"};
            return BuildName(syllable_1, syllable_2, syllable_3);
        }

        private static string HalflingNames()
        {
            string[] syllable_1 = new string[] {"B", "Ber", "Br", "D", "Der", "Dr", "F", "Fr", "G", "H", "L", "Ler", "M", "Mer", "N", "P", "Pr", "Per", "R", "S", "T", "W"};
            string[] syllable_2 = new string[] {"a", "e", "i", "ia", "o", "oi", "u"};
            string[] syllable_3 = new string[] {"bo", "ck", "decan", "degar", "do", "doc", "go", "grin", "lba", "lbo", "lda", "ldo", "lla", "ll", "lo", "m", "mwise", "nac", "noc", "nwise", "p", "ppin", "pper", "sha", "tho", "to"};
            return BuildName(syllable_1, syllable_2, syllable_3);
        }

        private static string SaxonNames()
        {
            string[] syllable_1 = new string[] {"Ald", "Aeld", "Alf", "Aelf", "Alh", "Aelh", "Athel", "Aethel", "Beo", "Beor", "Berh", "Brih", "Briht", "Cad", "Cead", "Cen", "Coel", "Cuth", "Cyne", "Ed", "Ead", "El", "Eal", "Eld", "Eg", "Ecg", "Eorp", "God", "Guth", "Har", "Hwaet", "Leo", "Leof", "Oft", "Ot", "Oth", "Os", "Osw", "Peht", "Pleg", "Rad", "Raed", "Sig", "Sige", "Si", "Sihr", "Tat", "Tath", "Tost", "Ut", "Uht", "Ul", "Ulf", "Wal", "Walth", "Wer", "Wit", "Wiht", "Wil", "Wulf"};
            string[] syllable_2 = new string[] {"gar", "heah", "here", "bald", "war", "weard", "wulf", "dred", "red", "stan", "wold", "tric", "ric", "wald", "mon", "wal", "walla", "wealh", "frith", "gyth", "rum", "bert", "berht", "gar", "win", "wine", "wiu", "for", "mund", "thoef", "eof", "had", "erth", "ferth", "thin", "er", "ther", "tar", "thar", "wig", "wicg", "mer", "floed", "ith", "hild", "run", "drun", "ny"};
            return BuildName(syllable_1, syllable_2);
        }

        private static string OrcNames()
        {
            string[] syllable_1 = new string[] {"B", "Er", "G", "Gr", "H", "P", "Pr", "R", "V", "Vr", "T", "Tr", "M", "Dr"};
            string[] syllable_2 = new string[] {"a", "i", "o", "oo", "u", "ui"};
            string[] syllable_3 = new string[] {"dash", "dish", "dush", "gar", "gor", "gdush", "lo", "gdish", "k", "lg", "nak", "rag", "rbag", "rg", "rk", "ng", "nk", "rt", "ol", "urk", "shnak", "mog", "mak", "rak"};
            return BuildName(syllable_1, syllable_2, syllable_3);
        }

        private static string Japan1Names()
        {
            string[] syllable_1 = new string[] {"aka", "aki", "bashi", "gawa", "kawa", "furu", "fuku", "fuji", "hana", "hara", "haru", "hashi", "hira", "hon", "hoshi", "ichi", "iwa", "kami", "kawa", "ki", "kita", "kuchi", "kuro", "marui", "matsu", "miya", "mori", "moto", "mura", "nabe", "naka", "nishi", "no", "da", "ta", "o", "oo", "oka", "saka", "saki", "sawa", "shita", "shima", "i", "suzu", "taka", "take", "to", "toku", "toyo", "ue", "wa", "wara", "wata", "yama", "yoshi", "kei", "ko", "zawa", "zen", "sen", "ao", "gin", "kin", "ken", "shiro", "zaki", "yuki", "asa"};
            string[] syllable_2 = new string[] {"", "", "", "", "", "", "", "", "", "", "bashi", "gawa", "kawa", "furu", "fuku", "fuji", "hana", "hara", "haru", "hashi", "hira", "hon", "hoshi", "chi", "wa", "ka", "kami", "kawa", "ki", "kita", "kuchi", "kuro", "marui", "matsu", "miya", "mori", "moto", "mura", "nabe", "naka", "nishi", "no", "da", "ta", "o", "oo", "oka", "saka", "saki", "sawa", "shita", "shima", "suzu", "taka", "take", "to", "toku", "toyo", "ue", "wa", "wara", "wata", "yama", "yoshi", "kei", "ko", "zawa", "zen", "sen", "ao", "gin", "kin", "ken", "shiro", "zaki", "yuki", "sa"};
            return Capitalize(BuildName(syllable_1, syllable_2));
        }

        private static string Japan2Names()
        {
            string[] syllable_1 = new string[] {"a", "i", "u", "e", "o", "", "", "", "", ""};
            string[] syllable_2 = new string[] {"ka", "ki", "ki", "ku", "ku", "ke", "ke", "ko", "ko", "sa", "sa", "sa", "shi", "shi", "shi", "su", "su", "se", "so", "ta", "ta", "chi", "chi", "tsu", "te", "to", "na", "ni", "ni", "nu", "nu", "ne", "no", "no", "ha", "hi", "fu", "fu", "he", "ho", "ma", "ma", "ma", "mi", "mi", "mi", "mu", "mu", "mu", "mu", "me", "mo", "mo", "mo", "ya", "yu", "yu", "yu", "yo", "ra", "ra", "ra", "ri", "ru", "ru", "ru", "re", "ro", "ro", "ro", "wa", "wa", "wa", "wa", "wo", "wo"};
            string[] syllable_3 = new string[] {"", "", "", "n"};
            string sName = BuildName(syllable_1, syllable_2, syllable_2);
            int nameType = Rnd.Get(3);
            switch(nameType)
            {
                case 1:
                    sName += BuildName(syllable_2);
                    break;
                case 2:
                    if(Rnd.OneChanceFrom(2))
                        sName += BuildName(syllable_2);
                    else
                        sName += BuildName(syllable_2, syllable_2);
                    break;
            }

            return Capitalize(sName + BuildName(syllable_3));
        }

        private static string ChinaNames()
        {
            string[] syllable_1 = new string[] {"zh", "x", "q", "sh", "h"};
            string[] syllable_2 = new string[] {"ao", "ian", "uo", "ou", "ia"};
            string[] syllable_3 = new string[] {"l", "w", "c", "p", "b", "m"};
            string[] syllable_4 = new string[] {"", "n"};
            string[] syllable_5 = new string[] {"d", "j", "q", "l"};
            string[] syllable_6 = new string[] {"a", "ai", "iu", "ao", "i"};
            
            string sName = BuildName(syllable_1, syllable_2);
            int nameType = Rnd.Get(3);
            switch(nameType)
            {
                case 1:
                    sName += BuildName(syllable_3, syllable_2, syllable_4);
                    break;
                case 2:
                    sName += "-" + BuildName(syllable_3, syllable_2);
                    if(Rnd.OneChanceFrom(2))
                        sName += BuildName(syllable_5, syllable_6);
                    break;
            }

            return Capitalize(sName);
        }

        private static string Hawaii1Names()
        {
            string[] syllable_1 = new string[] {"h", "k", "l", "m", "n", "p", "w", "'"};
            string[] syllable_2 = new string[] {"a", "e", "i", "o", "u"};

            string sName = "";

            if(Rnd.OneChanceFrom(2))
                sName += BuildName(syllable_1);

            sName += BuildName(syllable_2);

            if(Rnd.OneChanceFrom(2))
                sName += BuildName(syllable_1);

            sName += BuildName(syllable_2);

            if(Rnd.OneChanceFrom(2))
            {
                if(Rnd.OneChanceFrom(2))
                    sName += BuildName(syllable_1);

                sName += BuildName(syllable_2);
            }

            if(Rnd.OneChanceFrom(2))
            {
                if(Rnd.OneChanceFrom(2))
                    sName += BuildName(syllable_1);

                sName += BuildName(syllable_2);
            }

            if(Rnd.OneChanceFrom(2))
            {
                if(Rnd.OneChanceFrom(2))
                    sName += BuildName(syllable_1);

                sName += BuildName(syllable_2);
            }

            if(Rnd.OneChanceFrom(2))
            {
                if(Rnd.OneChanceFrom(2))
                    sName += BuildName(syllable_1);

                sName += BuildName(syllable_2);
            }

            return Capitalize(sName);
        }

        private static string Hawaii2Names()
        {
            string[] syllable_1 = new string[] {"h", "k", "l", "m", "n", "p", "w", ""};
            string[] syllable_2 = new string[] {"a", "e", "i", "o", "u", "a'", "e'", "i'", "o'", "u'", "ae", "ai", "ao", "au", "oi", "ou", "eu", "ei"};
            string[] syllable_3 = new string[] {"k", "l", "m", "n", "p", ""};
            string sName = BuildName(syllable_1, syllable_2, syllable_3);
            if(Rnd.OneChanceFrom(2))
                sName += BuildName(syllable_1, syllable_2, syllable_3);
            return Capitalize(sName);
        }

        private static string HinduNames()
        {
            string[] nameParts = new string[] {"dip", "pan", "jit", "parm", "dit", "gur", "preet", "san", "pra", "mit", "harm", "deep", "nav", "ak", "am", "far", "meen", "amar", "rik", "bal", "war", "gurd", "inder", "man", "suk"};
            int pos1 = 0;
            int pos2 = 0;
            string hinduName = "";

            //make sure they're not all the same
            while (pos1 == pos2)
            {
                pos1 = Rnd.Get(nameParts.Length);
                pos2 = Rnd.Get(nameParts.Length);
            }
            hinduName = nameParts[pos1] + nameParts[pos2];

            return Capitalize(hinduName);
        }


//<select name=c> 
//<option value="<s<v|V>(tia) | s<v|V>(os) | B<v|V>c(ios) | B<v|V><c|C>v(ios|os)>"> 
//    Greek Names
//<option value="sv(nia", "lia", "cia", "sia)"> 
//    Old Latin Place Names
//<option value="<<s", "ss>", "<VC", "vC", "B", "BVs", "Vs>><v", "V", "v", "<v(l", "n", "r)", "vc>>(th)"> 
//    Dragons (Pern)
//<option value="c'<s", "cvc>"> 
//    Dragon Riders
//<option value="<i", "s>v(mon", "chu", "zard", "rtle)"> 
//    Pokemon
//<option value="(", "(<B>", "s", "h", "ty", "ph", "r))(i", "ae", "ya", "ae", "eu", "ia", "i", "eo", "ai", "a)(lo", "la", "sri", "da", "dai", "the", "sty", "lae", "due", "li", "lly", "ri", "na", "ral", "sur", "rith)(", "(su", "nu", "sti", "llo", "ria", "))(", "(n", "ra", "p", "m", "lis", "cal", "deu", "dil", "suir", "phos", "ru", "dru", "rin", "raap", "rgue))"> 
//    Fantasy (Vowels, R, etc.)
//<option value="(cham", "chan", "jisk", "lis", "frich", "isk", "lass", "mind", "sond", "sund", "ass", "chad", "lirt", "und", "mar", "lis", "il", "<BVC>)(jask", "ast", "ista", "adar", "irra", "im", "ossa", "assa", "osia", "ilsa", "<vCv>)(", "(an", "ya", "la", "sta", "sda", "sya", "st", "nya))"> 
//    Fantasy (S, A, etc.)
//<option value="(ch", "ch't", "sh", "cal", "val", "ell", "har", "shar", "shal", "rel", "laen", "ral", "jh't", "alr", "ch", "ch't", "av)(", "(is", "al", "ow", "ish", "ul", "el", "ar", "iel))(aren", "aeish", "aith", "even", "adur", "ulash", "alith", "atar", "aia", "erin", "aera", "ael", "ira", "iel", "ahur", "ishul)"> 
//    Fantasy (H, L, etc.)
//<option value="(ethr", "qil", "mal", "er", "eal", "far", "fil", "fir", "ing", "ind", "il", "lam", "quel", "quar", "quan", "qar", "pal", "mal", "yar", "um", "ard", "enn", "ey)(", "(<vc>", "on", "us", "un", "ar", "as", "en", "ir", "ur", "at", "ol", "al", "an))(uard", "wen", "arn", "on", "il", "ie", "on", "iel", "rion", "rian", "an", "ista", "rion", "rian", "cil", "mol", "yon)"> 
//    Fantasy (N, L, etc.)
//<option value="(taith", "kach", "chak", "kank", "kjar", "rak", "kan", "kaj", "tach", "rskal", "kjol", "jok", "jor", "jad", "kot", "kon", "knir", "kror", "kol", "tul", "rhaok", "rhak", "krol", "jan", "kag", "ryr)(<vc>", "in", "or", "an", "ar", "och", "un", "mar", "yk", "ja", "arn", "ir", "ros", "ror)(", "(mund", "ard", "arn", "karr", "chim", "kos", "rir", "arl", "kni", "var", "an", "in", "ir", "a", "i", "as))"> 
//    Fantasy (K, N, etc.)
//<option value="(aj", "ch", "etz", "etzl", "tz", "kal", "gahn", "kab", "aj", "izl", "ts", "jaj", "lan", "kach", "chaj", "qaq", "jol", "ix", "az", "biq", "nam)(", "(<vc>", "aw", "al", "yes", "il", "ay", "en", "tom", "", "oj", "im", "ol", "aj", "an", "as))(aj", "am", "al", "aqa", "ende", "elja", "ich", "ak", "ix", "in", "ak", "al", "il", "ek", "ij", "os", "al", "im)"> 
//    Fantasy (J, G, Z, etc.)
//<option value="(yi", "shu", "a", "be", "na", "chi", "cha", "cho", "ksa", "yi", "shu)(th", "dd", "jj", "sh", "rr", "mk", "n", "rk", "y", "jj", "th)(us", "ash", "eni", "akra", "nai", "ral", "ect", "are", "el", "urru", "aja", "al", "uz", "ict", "arja", "ichi", "ural", "iru", "aki", "esh)"> 
//    Fantasy (K, J, Y, etc.)
//<option value="(syth", "sith", "srr", "sen", "yth", "ssen", "then", "fen", "ssth", "kel", "syn", "est", "bess", "inth", "nen", "tin", "cor", "sv", "iss", "ith", "sen", "slar", "ssil", "sthen", "svis", "s", "ss", "s", "ss)(", "(tys", "eus", "yn", "of", "es", "en", "ath", "elth", "al", "ell", "ka", "ith", "yrrl", "is", "isl", "yr", "ast", "iy))(us", "yn", "en", "ens", "ra", "rg", "le", "en", "ith", "ast", "zon", "in", "yn", "ys)"> 
//    Fantasy (S, E, etc.)
//</select> 
    }
}
