using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace NameGen
{
    public static class NameGenerator
    {
        private static readonly string[] m_aPrefAbstract = {
        "A",
        "Ab",
        "Ach",
        "Ad",
        "Ae",
        "Ag",
        "Ai",
        "Ak",
        "Al",
        "Am",
        "An",
        "Ap",
        "Ar",
        "As",
        "Ash",
        "At",
        "Ath",
        "Au",
        "Ay",
        "Ban",
        "Bar",
        "Brel",
        "Bren",
        "Cam",
        "Cla",
        "Cler",
        "Col",
        "Con",
        "Cor",
        "Cul",
        "Cuth",
        "Cy",
        "Chal",
        "Chan",
        "Chi",
        "Chon",
        "Chul",
        "Chur",
        "Del",
        "Dur",
        "Dwar",
        "Dwur",
        "Ek",
        "El",
        "En",
        "Eth",
        "Fal",
        "Far",
        "Fel",
        "Fell",
        "Fen",
        "Flan",
        "Flar",
        "Fly",
        "Fur",
        "Fy",
        "Gal",
        "Gan",
        "Gar",
        "Gel",
        "Glan",
        "Glar",
        "Glen",
        "Glir",
        "Glyn",
        "Glyr",
        "Glyth",
        "Gogh",
        "Gor",
        "Goth",
        "Gwal",
        "Gwen",
        "Gwur",
        "Gy",
        "Gyl",
        "Gyn",
        "Ghal",
        "Ghash",
        "Ghor",
        "Ghoz",
        "Ghul",
        "Hach",
        "Haj",
        "Hal",
        "Ham",
        "Hel",
        "Hen",
        "Hil",
        "Ho",
        "Hol",
        "Hul",
        "Ice",
        "Id",
        "Ie",
        "Il",
        "Im",
        "In",
        "Ir",
        "Is",
        "Iz",
        "Ja",
        "Jak",
        "Jar",
        "Jaz",
        "Jeth",
        "Jez",
        "Ji",
        "Jul",
        "Jur",
        "Juz",
        "Kag",
        "Kai",
        "Kaj",
        "Kal",
        "Kam",
        "Ken",
        "Kor",
        "Kul",
        "Kwal",
        "Kwar",
        "Kwel",
        "Kwen",
        "Kha",
        "Khel",
        "Khor",
        "Khul",
        "Khuz",
        "Lagh",
        "Lar",
        "Lin",
        "Lir",
        "Loch",
        "Lor",
        "Lyn",
        "Lyth",
        "Mal",
        "Man",
        "Mar",
        "Me",
        "Mer",
        "Meth",
        "Mil",
        "Min",
        "Mir",
        "Nam",
        "Nar",
        "Nel",
        "Nem",
        "Nen",
        "Nor",
        "Noth",
        "Nyr",
        "Ob",
        "Oe",
        "Ok",
        "Ol",
        "On",
        "Or",
        "Ow",
        "Par",
        "Pel",
        "Por",
        "Py",
        "Pyr",
        "Pyl",
        "Ral",
        "Ra",
        "Ram",
        "Rath",
        "Re",
        "Rel",
        "Ren",
        "Ri",
        "Ril",
        "Ro",
        "Ror",
        "Ruk",
        "Ry",
        "Sen",
        "Seth",
        "Sul",
        "Shae",
        "Shal",
        "Shar",
        "Shen",
        "Shir",
        "Tal",
        "Tam",
        "Tar",
        "Tel",
        "Ten",
        "Tir",
        "Tol",
        "Tul",
        "Tur",
        "Thor",
        "Thul",
        "U",
        "Uk",
        "Un",
        "Ul",
        "Va",
        "Val",
        "Van",
        "Vel",
        "Ven",
        "Veren",
        "Vul",
        "Wal",
        "War",
        "We",
        "Wel",
        "Wil",
        "Win",
        "Y",
        "Ya",
        "Ych",
        "Ye",
        "Yg",
        "Yi",
        "Yl",
        "Yn",
        "Yo",
        "Yp",
        "Yr",
        "Yth",
        "Yu",
        "Yul",
        "Za",
        "Zar",
        "Zel",
        "Zi",
        "Zim",
        "Zir",
        "Zol",
        "Zor",
        "Zhok",
        "Zhu",
        "Zhuk",
        "Zhul"
        };

        private static readonly string[] m_aSufAbstract = {
        "a",
        "ach",
        "aech",
        "ael",
        "aem",
        "aen",
        "aer",
        "aeth",
        "ail",
        "ain",
        "air",
        "aith",
        "all",
        "an",
        "and",
        "ar",
        "ash",
        "auch",
        "aul",
        "aun",
        "aur",
        "baen",
        "bain",
        "bar",
        "bath",
        "ben",
        "byr",
        "cael",
        "caer",
        "can",
        "cen",
        "cor",
        "cynd",
        "dach",
        "dail",
        "dain",
        "dan",
        "dar",
        "dik",
        "dir",
        "dy",
        "e",
        "eal",
        "el",
        "eld",
        "eth",
        "gar",
        "gath",
        "grim",
        "i",
        "ian",
        "ield",
        "ien",
        "ieth",
        "il",
        "ior",
        "ioth",
        "ish",
        "maer",
        "mail",
        "main",
        "mar",
        "maren",
        "miel",
        "mieth",
        "nain",
        "nair",
        "naith",
        "nal",
        "nar",
        "nath",
        "nen",
        "ner",
        "niel",
        "nien",
        "nieth",
        "nor",
        "noth",
        "nul",
        "nur",
        "nyr",
        "o",
        "och",
        "or",
        "oth",
        "oum",
        "owen",
        "rach",
        "raid",
        "rail",
        "rain",
        "raish",
        "raith",
        "ran",
        "rar",
        "ras",
        "raven",
        "ren",
        "riel",
        "rien",
        "rier",
        "rik",
        "ril",
        "rish",
        "ron",
        "ror",
        "ros",
        "roth",
        "rych",
        "ryl",
        "ryr",
        "rych",
        "thach",
        "thain",
        "thak",
        "thal",
        "than",
        "thar",
        "thiel",
        "thien",
        "thor",
        "thul",
        "thur",
        "ug",
        "uild",
        "uin",
        "uith",
        "uk",
        "ul",
        "un",
        "ur",
        "uth",
        "wain",
        "waith",
        "wald",
        "war",
        "ward",
        "well",
        "wen",
        "win",
        "y",
        "yll",
        "ynd",
        "yr",
        "yth",
        "zak",
        "zel",
        "zen",
        "zokh",
        "zor",
        "zul",
        "zuth"
        };

        private static readonly string[] m_aPrefEscimo = {
"ach",
"achana",
"achka",
"achuk",
"ak",
"akip",
"akun",
"ani",
"aninnik",
"ap",
"apat",
"api",
"apucha",
"chat",
"chit",
"chua",
"chut",
"ichis",
"ichitt",
"ikissik",
"ikiutta",
"iniki",
"iniss",
"inissik",
"ippik",
"issi",
"it",
"ita",
"itani",
"kass",
"kikk",
"kiun",
"kyap",
"kyaun",
"kyu",
"kyup",
"kuk",
"kup",
"kupp",
"kut",
"pi",
"pyaun",
"pyi",
"pyu",
"pyuin",
"pyup",
"siss",
"syaun",
"syiin",
"syu",
"syuss",
"syuun",
"saun",
"suss",
"suun",
"tap",
"tikk",
"tya",
"tyaan",
"tyiun",
"tuan",
"uch",
"uchach",
"uchk",
"ucht",
"upani",
"upik",
"utin",
"unuchut"};

        private static readonly string[] m_aSufEscimo = {
"ach",
"akan",
"akya",
"ani",
"anu",
"chach",
"chat",
"chin",
"chit",
"chun",
"iakap",
"ichak",
"ikin",
"ikip",
"ikta",
"in",
"innik",
"ipa",
"ippik",
"ipuch",
"issik",
"it",
"itut",
"kan",
"kip",
"kuk",
"kukik",
"kya",
"pich",
"pin",
"pip",
"pput",
"sup",
"tait",
"tin",
"tip",
"tit",
"titut",
"uchi",
"uk",
"ukta",
"utta"};

        private static readonly string[] m_aPrefGreek = {
"aer",
"agamen",
"agor",
"aion",
"air",
"aker",
"akrogon",
"akro",
"amen",
"ametan",
"amiant",
"ampel",
"anan",
"anankamom",
"andro",
"aner",
"antano",
"aorat",
"apolytro",
"athem",
"autark",
"biast",
"byblo",
"chrono",
"dogmo",
"dokim",
"ekbalo",
"ekpipto",
"ektrom",
"entell",
"epikatar",
"ereun",
"exang",
"exod",
"gorgo",
"hekono",
"heter",
"hikan",
"hilar",
"hymno",
"hypno",
"ianno",
"ierem",
"kalym",
"katar",
"klepto",
"kreman",
"makar",
"malak",
"maran",
"metan",
"nest",
"oikonom",
"optan",
"orgil",
"otar",
"ouran",
"papyr",
"parait",
"paramen",
"parthen",
"perik",
"peril",
"perim",
"philag",
"polyl",
"poro",
"prax",
"sin",
"sken",
"smyrno",
"strato",
"thron",
"trit",
"troglo",
"zelot"};

        private static readonly string[] m_aSufGreek = {
"akos",
"alizo",
"alotos",
"arenos",
"aros",
"arotes",
"arx",
"askalos",
"atos",
"atres",
"einos",
"elos",
"eros",
"eryx",
"etes",
"imos",
"irmos",
"itos",
"okles",
"opos",
"otus",
"polis",
"us"};

        private static readonly string[] m_aPrefAztec = {
"Acayu",
"Alar",
"Apatzin",
"Ayoquez",
"Ayu",
"Cham",
"Chetu",
"Chi",
"Cho",
"Chun",
"Colo",
"Comalcal",
"Comi",
"Cuet",
"Hala",
"Huicha",
"Huimax",
"Hunuc",
"Ix",
"Ixmiquil",
"Iza",
"Jal",
"Jamil",
"Juchi",
"Kaminal",
"Kantunil",
"Maya",
"Mapas",
"Maxcan",
"Maz",
"Miahu",
"Minatit",
"Mul",
"Noch",
"Oax",
"Oco",
"Ome",
"Ozibilchal",
"Panab",
"Pet",
"Pochu",
"Popoca",
"Say",
"Sayax",
"Tehuan",
"Tenoxtit",
"Tep",
"Tik",
"Tiz",
"Tizi",
"Tlaco",
"Tom",
"Ton",
"Tul",
"Tun",
"Tux",
"Uaxac",
"Urua",
"Yaxchi",
"Zacat",
"Zana",
"Zima"};

        private static readonly string[] m_aSufAztec = {
"atlan",
"ixtlan",
"huas",
"juyu",
"poton",
"talpan",
"tepec",
"tepetl",
"titlan",
"zalan"};

        private static readonly string[] m_aPrefDrow = {
"Alean",
"Ale",
"Arab",
"Arken",
"Auvry",
"Baen",
"Barri",
"Cladd",
"Desp",
"De",
"Do’",
"Eils",
"Everh",
"Fre",
"Gode",
"Helvi",
"Hla",
"Hun",
"Ken",
"Kil",
"Mae",
"Mel",
"My",
"Noqu",
"Orly",
"Ouss",
"Rilyn",
"Teken'",
"Tor",
"Zau"};

        private static readonly string[] m_aSufDrow = {
"afin",
"ana",
"ani",
"ar",
"arn",
"ate",
"ath",
"duis",
"ervs",
"ep",
"ett",
"ghym",
"iryn",
"lyl",
"mtor",
"ndar",
"neld",
"rae",
"rahel",
"rret",
"sek",
"th",
"tlar",
"t’tar",
"tyl",
"und",
"urden",
"val",
"virr",
"zynge"};

        private static readonly string[] m_aPrefScotish = {
"Aber",
"Ar",
"As",
"At",
"Avie",
"Bal",
"Ben",
"Bran",
"Brech",
"Bro",
"Cairn",
"Can",
"Carl",
"Colon",
"Clyde",
"Craig",
"Cum",
"Dearg",
"Don",
"Dor",
"Dun",
"Dur",
"El",
"Fal",
"For",
"Fyne",
"Glas",
"Hal",
"Inver",
"Ju",
"Kil",
"Kilbran",
"Kirrie",
"Lairg",
"Lin",
"Lo",
"Loch",
"Lorn",
"Lyb",
"Ma",
"Mal",
"Mel",
"Monadh",
"Nairn",
"Nith",
"Ob",
"Oron",
"Ran",
"Scar",
"Scour",
"Spey",
"Stom",
"Strom",
"Tar",
"Tay",
"Ti",
"Tober",
"Uig",
"Ulla",
"Wick"};

        private static readonly string[] m_aSufScotish = {
"aline",
"an",
"aray",
"avon",
"ba",
"bert",
"bis",
"blane",
"bran",
"da",
"dee",
"deen",
"far",
"feldy",
"gin",
"gorm",
"ie",
"in",
"kaig",
"kirk",
"laig",
"liath",
"maol",
"mond",
"moral",
"more",
"mory",
"muir",
"na",
"nan",
"ner",
"ness",
"nhe",
"nock",
"noth",
"nure",
"ock",
"pool",
"ra",
"ran",
"ree",
"res",
"say",
"ster",
"tow"};

        private static readonly string[] m_aPrefAfrica = {
"Ag",
"Ahr",
"Ba",
"Bor",
"Dar",
"Don",
"Dor",
"Dung",
"Ga",
"Gal",
"Gam",
"Gul",
"Gur",
"Gwa",
"Gwah",
"Gwar",
"Gwul",
"Ig",
"Ja",
"Jih",
"Jug",
"Kas",
"Kesh",
"Kides",
"Kili",
"Kor",
"Kul",
"Kush",
"Lar",
"Lu",
"Ma",
"Mat",
"Mbeg",
"Mbeng",
"Min",
"Ngor",
"Ngul",
"N'Gul",
"Nyag",
"N'Yag",
"N'Zin",
"Ong",
"Rod",
"Sha",
"Sum",
"Swa",
"Ti",
"Tot",
"Ug",
"Ung",
"Wad",
"Waz",
"Wur",
"Ya",
"Za",
"Zang",
"Zar",
"Zem",
"Zik",
"Zim",
"Zu",
"Zul"};

        private static readonly string[] m_aSufAfrica = {
"a",
"ad",
"aga",
"ara",
"ai",
"al",
"alo",
"ang",
"anga",
"ani",
"bab",
"bal",
"balla",
"biba",
"bu",
"buk",
"buru",
"daja",
"dar",
"donga",
"dor",
"du",
"dul",
"duru",
"daza",
"'guba",
"'gung",
"hili",
"i",
"id",
"iji",
"ili",
"jari",
"jaro",
"juri",
"'ka",
"lah",
"lur",
"mala",
"mim",
"mu",
"munga",
"mur",
"nur",
"nuzi",
"o",
"od",
"ofo",
"oja",
"onga",
"ozi",
"ra",
"sala",
"sula",
"sunga",
"tulo",
"u",
"ula",
"ulga",
"unga",
"wa",
"wath",
"we",
"wuzi",
"zaja",
"zaza",
"zin",
"zum",
"zung",
"zur"};

        private static readonly string[] m_aPrefElven1 = {
"Ama",
"Ari",
"Aza",
"Cla",
"Cy",
"Dae",
"Dho",
"Dre",
"Fi",
"Ia",
"Ky",
"Lue",
"Ly",
"Mai",
"My",
"Na",
"Nai",
"Nu",
"Ny",
"Py",
"Ry",
"Rua",
"Sae",
"Sha",
"She",
"Si",
"Tia",
"Ty",
"Ya",
"Zy"};

        private static readonly string[] m_aSufElven1 = {
"nae",
"lae",
"dar",
"drimme",
"lath",
"lith",
"lyth",
"lan",
"lanna",
"lirr",
"lis",
"lys",
"lyn",
"llinn",
"lihn",
"nal",
"nin",
"nine",
"nyn",
"nis",
"sal",
"sel",
"tas",
"thi",
"thil",
"vain",
"vin",
"wyn",
"zair"};

        private static readonly string[] m_aPrefElven2 = {
"Aer",
"Al",
"Am",
"Ang",
"Ansr",
"Ar",
"Arn",
"Bael",
"Cael",
"Cal",
"Cas",
"Cor",
"Eil",
"Eir",
"El",
"Er",
"Ev",
"Fir",
"Fis",
"Gael",
"Gil",
"Il",
"Kan",
"Ker",
"Keth",
"Koeh",
"Kor",
"Laf",
"Lam",
"Mal",
"Nim",
"Rid",
"Rum",
"Seh",
"Sel",
"Sim",
"Syl",
"Tahl",
"Vil"};

        private static readonly string[] m_aSufElven2 = {
"ael",
"aer",
"aera",
"aias",
"aia",
"aith",
"aira",
"ala",
"ali",
"ani",
"uanna",
"ari",
"aro",
"ibrar",
"adar",
"odar",
"udrim",
"emar",
"esti",
"evar",
"afel",
"efel",
"ihal",
"ihar",
"ahel",
"ihel",
"ian",
"ianna",
"iat",
"iel",
"ila",
"inar",
"ine",
"ith",
"elis",
"ellon",
"inal",
"anis",
"aruil",
"eruil",
"isal",
"sali",
"sar",
"asar",
"isar",
"asel",
"isel",
"itas",
"ethil",
"avain",
"avin",
"azair"};

        private static readonly string[] m_aPrefDwarven = {
"agar",
"agaz",
"barak",
"baruk",
"baraz",
"bizar",
"bizul",
"bul",
"buzar",
"garak",
"gor",
"gog",
"gorog",
"gothol",
"guzib",
"ibin",
"ibiz",
"izil",
"izuk",
"kelek",
"kezan",
"kibil",
"kinil",
"kun",
"kheled",
"khelek",
"khimil",
"khuz",
"laruk",
"luz",
"moran",
"moril",
"nibin",
"nukul"};

        private static readonly string[] m_aSufDwarven = {
"akar",
"agul",
"amen",
"gib",
"gol",
"gog",
"gul",
"guluth",
"gundil",
"gundag",
"guzun",
"lib",
"lizil",
"loth",
"mab",
"mor",
"mud",
"mur",
"nazar",
"nigin",
"niz",
"nizil",
"nuz",
"nuzum",
"thibil",
"thizar",
"ulin",
"uzar",
"uzun",
"zad",
"zakar",
"zal",
"zalak",
"zam",
"zan",
"zaral",
"zarak",
"zeg",
"zerek",
"zibith",
"zikil",
"zokh",
"zukum"};

        private static readonly string[] m_aPrefOrchish = {
"Arg",
"Az",
"Bad",
"Balkh",
"Bol",
"Dreg",
"Dur",
"Durba",
"Ghash",
"Lurg",
"Luz",
"Mor",
"Nazg",
"Og",
"Tarkh",
"Urg",
"Ug",
"Vol",
"Yazh"};

        private static readonly string[] m_aSufOrchish = {
"agal",
"buz",
"dor",
"dur",
"gar",
"mog",
"narb",
"nazg",
"rod",
"shak",
"waz",
"ubal"};

        private static readonly string[] m_aPrefArabic = {
"Aaza",
"Abha",
"Ad",
"Aga",
"Ah",
"Ain",
"Ait",
"Ajda",
"Ali",
"Al",
"Arrer",
"As",
"Ash",
"Ay",
"Az",
"Bab",
"Bani",
"Bari",
"Bat",
"Birak",
"Bitam",
"Bou",
"Dakh",
"Dha",
"Dham",
"Djaz",
"Djeb",
"Fash",
"Ghad",
"Ghar",
"Ghat",
"Gra",
"Had",
"Ham",
"Har",
"Jawf",
"Jer",
"Jid",
"Jir",
"Kabir",
"Kebir",
"Ket",
"Khat",
"Khem",
"Kher",
"Khum",
"Ksar",
"Mak",
"Mara",
"Men",
"Mu",
"Qat",
"Qay",
"Sa",
"Sab",
"Sah",
"Sal",
"Sidi",
"Sma",
"Sulay",
"Tabel",
"Tar",
"Tay",
"Taza",
"Ubay",
"Wah",
"Yab",
"Yaf",
"Yous",
"Zil",
"Zou"};

        private static readonly string[] m_aSufArabic = {
"ada",
"ah",
"air",
"ama",
"amis",
"aq",
"ar",
"ash",
"at",
"bala",
"biya",
"dah",
"dir",
"el",
"faya",
"fi",
"fir",
"ha",
"hab",
"ia",
"idj",
"ir",
"is",
"ja",
"jel",
"ka",
"kah",
"kha",
"khari",
"la",
"lah",
"ma",
"na",
"nen",
"ra",
"ran",
"rar",
"rata",
"rin",
"rem",
"run",
"sef",
"sumah",
"tar",
"ya",
"yan",
"yil"};

        private static readonly string[] m_aPrefViking = {
"al",
"ber",
"drammen",
"grong",
"hag",
"hauge",
"hed",
"kinsar",
"kol",
"koper",
"lin",
"nas",
"norr",
"olof",
"os",
"Ost",
"Oster",
"skellef",
"soder",
"stal",
"stavan",
"stock",
"tons",
"trond",
"vin"};

        private static readonly string[] m_aSufViking = {
"fors",
"gard",
"heim",
"holm",
"lag",
"mar",
"marden",
"mark",
"stad",
"strom"};

        private static readonly string[] m_aPrefHumans = {
"basing",
"birming",
"black",
"bland",
"bletch",
"brack",
"brent",
"bridge",
"broms",
"bur",
"cam",
"canter",
"chelten",
"chester",
"col",
"dor",
"dun",
"glaston",
"grim",
"grin",
"harro",
"hastle",
"hels",
"hemp",
"herne",
"horn",
"hors",
"hum",
"ketter",
"lei",
"maiden",
"marble",
"mar",
"mel",
"new",
"nor",
"notting",
"oak",
"ox",
"ports",
"sher",
"stam",
"stan",
"stock",
"stroud",
"tuan",
"warring",
"wind"};

        private static readonly string[] m_aSufHumans = {
"dare",
"don",
"field",
"ford",
"grove",
"ham",
"hill",
"lock",
"mere",
"moor",
"ton",
"vil",
"wood"};

        private static readonly string[] m_aPrefInn = {
"Bent",
"Black",
"Blind",
"Blue",
"Bob's",
"Joe's",
"Broken",
"Buxom",
"Cat's",
"Crow's",
"Dirty",
"Dragon",
"Dragon's",
"Drunken",
"Diamond",
"Eagle's",
"Eastern",
"Falcon's",
"Fawning",
"Fiend's",
"Flaming",
"Frosty",
"Frozen",
"Gilded",
"Genie's",
"Golden",
"Golden",
"Gray",
"Green",
"King's",
"Licked",
"Lion's",
"Iron",
"Mended",
"Octopus",
"Old",
"Old",
"Orc's",
"Pink",
"Pot",
"Puking",
"Queen's",
"Red",
"Ruby",
"Delicate",
"Sea",
"Sexy",
"Shining",
"Silver",
"Singing",
"Steel",
"Strange",
"Thirsty",
"Violet",
"White",
"Wild",
"Yawing"};

        private static readonly string[] m_aSufInn = {
" Axe",
" Anchor",
" Barrel",
" Basilisk",
" Belly",
" Blade",
" Boar",
" Breath",
" Brew",
" Claw",
" Coin",
" Delight",
" Den",
" Dragon",
" Drum",
" Dwarf",
" Fist",
" Flower",
" Gem",
" Gryphon",
" Hand",
" Head",
" Hole",
" Inn",
" Lady",
" Maiden",
" Lantern",
" Monk",
" Mug",
" Nest",
" Orc",
" Paradise",
" Pearl",
" Pig",
" Pit",
" Place",
" Tavern",
" Portal",
" Ranger",
" Rest",
" Sailor",
" Sleep",
" Song",
" Swan",
" Swords",
" Tree",
" Unicorn",
" Whale",
" Wish",
" Wizard",
" Rain"};

        private static readonly string[] m_aPrefFort = {
"Mind",
"Iron",
"Demention ",
"Demonic ",
"Blood",
"Mistery ",
"Ancient ",
"Doom",
"Black ",
"Crimson ",
"Blue ",
"Eternal ",
"Cursed ",
"Funny ",
"Stone",
"Etherial ",
"Phantom ",
"Forgotten ",
"King's ",
"Queen's ",
"Royal ",
"Fallen",
"Lost ",
"Warrior's ",
"Sorcerer's ",
"Steel",
"Blademaster's ",
"Screaming ",
"Ice",
"Frozen ",
"Dragon",
"Glorious ",
"Infernal "};

        private static readonly string[] m_aSufFort = {
"Storm",
"Fist",
"Keep",
"Rage",
"Rose",
"Residence",
"Mansion",
"Haven",
"Gates",
"Fort",
"Tower",
"Castle",
"Citadel"
};

        private static readonly string[] m_aPrefShip = {
"Absolute",
"Adventure",
"Alisa",
"Altered",
"Amber",
"Ancient",
"Angel's",
"Animal",
"Another",
"Azure",
"Bad",
"Bad Moon",
"Betty",
"Big",
"Black",
"Blue",
"Breaking",
"Crime",
"Crimson",
"Dancing",
"Dark",
"Dawn",
"Dirty",
"Distant",
"Double",
"Dragon",
"Dream",
"Emerald",
"Empty",
"Enchanted",
"Exotic",
"Extra",
"Extreme",
"Fallen",
"Fast",
"Fatal",
"Fifth",
"Final",
"Fine",
"Fire",
"First",
"Flying",
"Foreign",
"Fortune",
"Funny",
"Gentle",
"Golden",
"Grand",
"Great",
"Green",
"Grey",
"Gypsy",
"Half",
"Happy",
"High",
"Impossible",
"Jade",
"Little",
"Lone",
"Lucky",
"Mad",
"Mermaid",
"Midnight",
"Moon",
"Morning",
"Naked",
"Naughty",
"Naval",
"New",
"Night",
"Ocean",
"Old",
"Pacific",
"Perfect",
"Pretty",
"Quick",
"Quiet",
"Red",
"Saint",
"Sea",
"Sapphire",
"Second",
"Silver",
"Southern",
"Stella",
"Sun",
"Sunset",
"Sweet",
"Third",
"Thunder",
"Treasure",
"Ultimate",
"Wave",
"Zephyr",
"Zodiac"
};

        private static readonly string[] m_aSufShip = {
" Adventure",
" Amore",
" Angel",
" Answer",
" Attraction",
" Bird",
" Boat",
" Body",
" Bound",
" Boy",
" Breaker",
" Breeze",
" Cat",
" Catcher",
" Chaser",
" Courier",
" Crusher",
" Devil",
" Diamond",
" Dog",
" Dolphin",
" Dream",
" Dreamer",
" Eagle",
" Elf",
" Fish",
" Flash",
" Flight",
" Fox",
" Girl",
" Ghost",
" Goose",
" Gull",
" Hawk",
" Huntress",
" Hunter",
" Jack",
" Jane",
" Jewel",
" Jumper",
" Karma",
" King",
" Kiss",
" Knight",
" Lady",
" Lion",
" Love",
" Lover",
" Madness",
" Magic",
" Marie",
" Minstrel",
" Mist",
" Mistake",
" Money",
" Monkey",
" Monster",
" Nest",
" Nightmare",
" Owl",
" Queen",
" Quest",
" Pig",
" Pirate",
" Plainsman",
" Phantom",
" Power",
" Presence",
" Prince",
" Princess",
" Rainbow",
" Rising",
" Rose",
" Runner",
" Scare",
" Seeker",
" Sight",
" Sirena",
" Sixteen",
" Shadow",
" Shift",
" Shine",
" Stalker",
" Stripe",
" Song",
" Spirit",
" Spice",
" Star",
" Storm",
" Swan",
" Tide",
" Tiger",
" Toy",
" Trouble",
" Turtle",
" Viking",
" Unicorn",
" Walker",
" Wind",
" Wine",
" Wish",
" Witch",
" Wizard",
" White",
" Wolf",
" Woman",
" Zebra"
};

        private static readonly string[] m_aPrefFemale = {
"Ail",
"Ara",
"Ay",
"Bren",
"Astar",
"Dae",
"Dren",
"Dwen",
"El",
"Erin",
"Eth",
"Fae",
"Fay",
"Gae",
"Gay",
"Glae",
"Gwen",
"Il",
"Jey",
"Lae",
"Lan",
"Lin",
"Mae",
"Mara",
"More",
"Mi",
"Min",
"Ne",
"Nel",
"Pae",
"Pwen",
"Rae",
"Ray",
"Re",
"Ri",
"Si",
"Sal",
"Say",
"Tae",
"Te",
"Ti",
"Tin",
"Tir",
"Vi",
"Vul"
};

        private static readonly string[] m_aSufFemale = {
"ta",
"alle",
"ann",
"arra",
"aye",
"da",
"dolen",
"ell",
"enn",
"eth",
"eya",
"fa",
"fey",
"ga",
"gwenn",
"hild",
"ill",
"ith",
"la",
"lana",
"lar",
"len",
"lwen",
"ma",
"may",
"na",
"narra",
"navia",
"nwen",
"ola",
"pera",
"pinn",
"ra",
"rann",
"rell",
"ress",
"reth",
"riss",
"sa",
"shann",
"shara",
"shea",
"shell",
"tarra",
"tey",
"ty",
"unn",
"ura",
"valia",
"vara",
"vinn",
"wen",
"weth",
"wynn",
"wyrr",
"ya",
"ye",
"yll",
"ynd",
"yrr",
"yth"
};

        private static readonly string[] m_aPrefMale = {
"ache",
"aim",
"bald",
"bear",
"cron",
"boar",
"boast",
"boil",
"boni",
"boy",
"bower",
"churl",
"corn",
"cuff",
"dark",
"dire",
"dour",
"dross",
"dupe",
"dusk",
"dwar",
"dwarf",
"ebb",
"el",
"elf",
"fag",
"fate",
"fay",
"fell",
"fly",
"fowl",
"gard",
"gay",
"gilt",
"girth",
"glut",
"goad",
"gold",
"gorge",
"grey",
"groan",
"haft",
"hale",
"hawk",
"haught",
"hiss",
"hock",
"hoof",
"hook",
"horn",
"kin",
"kith",
"lank",
"leaf",
"lewd",
"louse",
"lure",
"man",
"mars",
"meed",
"moat",
"mould",
"muff",
"muse",
"not",
"numb",
"odd",
"ooze",
"ox",
"pale",
"port",
"quid",
"rau",
"red",
"rich",
"rob",
"rod",
"rud",
"ruff",
"run",
"rush",
"scoff",
"skew",
"sky",
"sly",
"sow",
"stave",
"steed",
"swar",
"thor",
"tort",
"twig",
"twit",
"vain",
"vent",
"vile",
"wail",
"war",
"whip",
"wise",
"worm",
"yip"
};

        private static readonly string[] m_aSufMale = {
"os",
"ard",
"bald",
"ban",
"baugh",
"bert",
"brand",
"cas",
"celot",
"cent",
"cester",
"cott",
"dane",
"dard",
"doch",
"dolph",
"don",
"doric",
"dower",
"dred",
"fird",
"ford",
"fram",
"fred",
"frid",
"fried",
"gal",
"gard",
"gernon",
"gill",
"gurd",
"gus",
"ham",
"hard",
"hart",
"helm",
"horne",
"ister",
"kild",
"lan",
"lard",
"ley",
"lisle",
"loch",
"man",
"mar",
"mas",
"mon",
"mond",
"mour",
"mund",
"nald",
"nard",
"nath",
"ney",
"olas",
"pold",
"rad",
"ram",
"rard",
"red",
"rence",
"reth",
"rick",
"ridge",
"riel",
"ron",
"rone",
"roth",
"sander",
"sard",
"shall",
"shaw",
"son",
"steen",
"stone",
"ter",
"than",
"ther",
"thon",
"thur",
"ton",
"tor",
"tran",
"tus",
"ulf",
"vald",
"van",
"vard",
"ven",
"vid",
"vred",
"wald",
"wallader",
"ward",
"werth",
"wig",
"win",
"wood",
"yard"
};

        public static string GetAbstractName()
        {
            return GetAbstractName(Rnd.Get(int.MaxValue));
        }

        public static string GetAbstractName(int iSeed)
        {
            iSeed = Math.Abs(iSeed);
            string first, second;
            int w, h, mul;

            w = m_aPrefAbstract.Length;
            h = m_aSufAbstract.Length;

            mul = iSeed % (w * h);

            first = m_aPrefAbstract[mul % w];
            second = m_aSufAbstract[mul / w];

            char t = first[0];
            if (Char.IsLetter(t))
            {
                t = Char.ToUpper(t);
                first = t + first.Substring(1);
            }

            return first + second;
        }

        public enum Language
        {
            Viking,
            Scotish,
            European,
            Escimo,
            Arabian,
            African,
            Elven,
            Elfish,
            Dwarven,
            Orcish,
            Greek,
            Drow,
            Aztec,
            Any,
            NONE
        };

        public static string GetEthnicName(Language eRace)
        {
            return GetEthnicName(eRace, Rnd.Get(int.MaxValue));
        }

        public static string GetEthnicName(Language eRace, int iSeed)
        {
            string first, second;
            int w, h, mul;

            if (eRace == Language.Any)
            {
                Array _values = Enum.GetValues(typeof(Language));
                eRace = (Language)_values.GetValue(Rnd.Get(_values.Length));
            }

            switch (eRace)
            {
                case Language.Viking:
                    w = m_aPrefViking.Length;
                    h = m_aSufViking.Length;
                    mul = iSeed % (w * h);
                    first = m_aPrefViking[mul % w];
                    second = m_aSufViking[mul / w];
                    break;
                case Language.Scotish:
                    w = m_aPrefScotish.Length;
                    h = m_aSufScotish.Length;
                    mul = iSeed % (w * h);
                    first = m_aPrefScotish[mul % w];
                    second = m_aSufScotish[mul / w];
                    break;
                case Language.European:
                    w = m_aPrefHumans.Length;
                    h = m_aSufHumans.Length;
                    mul = iSeed % (w * h);
                    first = m_aPrefHumans[mul % w];
                    second = m_aSufHumans[mul / w];
                    break;
                case Language.Escimo:
                    w = m_aPrefEscimo.Length;
                    h = m_aSufEscimo.Length;
                    mul = iSeed % (w * h);
                    first = m_aPrefEscimo[mul % w];
                    second = m_aSufEscimo[mul / w];
                    break;
                case Language.Arabian:
                    w = m_aPrefArabic.Length;
                    h = m_aSufArabic.Length;
                    mul = iSeed % (w * h);
                    first = m_aPrefArabic[mul % w];
                    second = m_aSufArabic[mul / w];
                    break;
                case Language.African:
                    w = m_aPrefAfrica.Length;
                    h = m_aSufAfrica.Length;
                    mul = iSeed % (w * h);
                    first = m_aPrefAfrica[mul % w];
                    second = m_aSufAfrica[mul / w];
                    break;
                case Language.Elven:
                    w = m_aPrefElven1.Length;
                    h = m_aSufElven1.Length;
                    mul = iSeed % (w * h);
                    first = m_aPrefElven1[mul % w];
                    second = m_aSufElven1[mul / w];
                    break;
                case Language.Elfish:
                    w = m_aPrefElven2.Length;
                    h = m_aSufElven2.Length;
                    mul = iSeed % (w * h);
                    first = m_aPrefElven2[mul % w];
                    second = m_aSufElven2[mul / w];
                    break;
                case Language.Dwarven:
                    w = m_aPrefDwarven.Length;
                    h = m_aSufDwarven.Length;
                    mul = iSeed % (w * h);
                    first = m_aPrefDwarven[mul % w];
                    second = m_aSufDwarven[mul / w];
                    break;
                case Language.Orcish:
                    w = m_aPrefOrchish.Length;
                    h = m_aSufOrchish.Length;
                    mul = iSeed % (w * h);
                    first = m_aPrefOrchish[mul % w];
                    second = m_aSufOrchish[mul / w];
                    break;
                case Language.Greek:
                    w = m_aPrefGreek.Length;
                    h = m_aSufGreek.Length;
                    mul = iSeed % (w * h);
                    first = m_aPrefGreek[mul % w];
                    second = m_aSufGreek[mul / w];
                    break;
                case Language.Drow:
                    w = m_aPrefDrow.Length;
                    h = m_aSufDrow.Length;
                    mul = iSeed % (w * h);
                    first = m_aPrefDrow[mul % w];
                    second = m_aSufDrow[mul / w];
                    break;
                case Language.Aztec:
                    w = m_aPrefAztec.Length;
                    h = m_aSufAztec.Length;
                    mul = iSeed % (w * h);
                    first = m_aPrefAztec[mul % w];
                    second = m_aSufAztec[mul / w];
                    break;
                default:
                    first = "";
                    second = "";
                    break;
            }

            if (first.Length > 0 && Char.IsLetter(first[0]))
            {
                first = Char.ToUpper(first[0]) + first.Substring(1);
            }

            return first + second;
        }

        public static string GetInnName()
        {
            return GetInnName(Rnd.Get(int.MaxValue));
        }

        public static string GetInnName(int iSeed)
        {
            string first, second;
            int w, h, mul;

            w = m_aPrefInn.Length;
            h = m_aSufInn.Length;
            mul = iSeed % (w * h);
            first = m_aPrefInn[mul % w];
            second = m_aSufInn[mul / w];

            if (Char.IsLetter(first[0]))
            {
                first = Char.ToUpper(first[0]) + first.Substring(1);
            }

            return first + second;
        }

        public static string GetFortressName()
        {
            return GetFortressName(Rnd.Get(int.MaxValue));
        }

        public static string GetFortressName(int iSeed)
        {
            string first, second;
            int w, h, mul;

            w = m_aPrefFort.Length;
            h = m_aSufFort.Length;
            mul = iSeed % (w * h);
            first = m_aPrefFort[mul % w];
            second = m_aSufFort[mul / w];

            if (Char.IsLetter(first[0]))
            {
                first = Char.ToUpper(first[0]) + first.Substring(1);
            }

            return first + second;
        }

        public static string GetShipName()
        {
            return GetShipName(Rnd.Get(int.MaxValue));
        }

        public static string GetShipName(int iSeed)
        {
            string first, second;
            int w, h, mul;

            w = m_aPrefShip.Length;
            h = m_aSufShip.Length;
            mul = iSeed % (w * h);
            first = m_aPrefShip[mul % w];
            second = m_aSufShip[mul / w];

            if (Char.IsLetter(first[0]))
            {
                first = Char.ToUpper(first[0]) + first.Substring(1);
            }

            return first + second;
        }

        public static string GetHeroName(bool bMale)
        {
            return GetHeroName(Rnd.Get(int.MaxValue), bMale);
        }

        public static string GetHeroName(int iSeed, bool bMale)
        {
            string first, second;
            int w, h, mul;

            if (bMale)
            {
                w = m_aPrefMale.Length;
                h = m_aSufMale.Length;
                mul = iSeed % (w * h);
                first = m_aPrefMale[mul % w];
                second = m_aSufMale[mul / w];
            }
            else
            {
                w = m_aPrefFemale.Length;
                h = m_aSufFemale.Length;
                mul = iSeed % (w * h);
                first = m_aPrefFemale[mul % w];
                second = m_aSufFemale[mul / w];
            }

            if (Char.IsLetter(first[0]))
            {
                first = Char.ToUpper(first[0]) + first.Substring(1);
            }

            return first + second;
        }
    }
}

/* 
 * 
Barsoomian by E. R. Burroughs -- A-Kor Anatok A-Sor Astok Ay-mad Bal Tab Bal Zak Bandolian Ban-Tor Bar Comas 
Carthoris Dak Kova Dar Tarus Dejah Thoris Djor Kantos Dotar Sojat Doxus 
Dur Ajmad Dur-dan E-Mad Em-Tar E-Thas Fal Sivas Floran Fonar Gahan Gan Had 
Gan-ho Gan Hor Gantun Gur Gar Nal Ghek Ghron Gor-don Gorgum Gor Hajus Gozava 
Gur Tus Had Urtur Haglion Haja Haj Alt Haj Osis Hal Vas Hamas Han Du 
Hin Abtol Ho Ran Kim Hora San Hor Kai Lan Hortan Gur Horur Hor Vastus 
Hovan Du I-Gos Il-dur-en I-Mal Issus I-Zav Jad-han Jal Had Janai Jat Or 
Jav Kab Kadja Kal Tavan Kam Han Tor Kandus Kantos Kan Kara Vasa Kar Komak 
Komal Kor-an Kor San Kulan Tith Lakor Lan-O Lan Sohn Wen Larok Lee Um Lo 
Llana Lorquas Ptomel Lum Tar O Luud Man-lat Matai Shang Moak Mors Kajak 
Motus Multis Par Mu Tel Myr-lo Nastor Nolat Notan Nur An Nutus O Ala 
Olvia Marthis O-Mai Orm-O O-Tar O-Zar Ozara Pan Dan Chee Pandar Parthak 
Phaidor Phao Pho Lar Phor San Phor Tak Phystal Pnoxus Povak Ptang Ptantus 
Ptor Fak Rab-zov Rapas Ras Thavas Rojas Ro Tan Bim Sab Than Sag Or 
Salensus Oll Sanoma Tora San Tothis Saran Tal Sarkoja Sator Throg Sept 
Sharu Sil Vagis Sola Solan Soran Sorav Sovan Sytor Tal Hajus Talu Tan Gama 
Tan Hadron Tanus Tara Tardos Mors Tario Tar Tarkas Tasor Tavia 
Teeaytan-ov Thabis Than Kosis Thar Ban Thurid Thuvan Dihn Thuvia Tor-dur-bar 
Tor Hatan Tortih Torkar Bar Tul Axtar Tun-gan Turan Turjun U Dan U-Dor U-Kal 
Ulah Uldak Ul-to Ul Vas Umka Ur Jan Ur Raj Uthia U-Thor U-Van Vad Varo 
Vaja Val Dor Valla Dia Vandor Van-tija Vanuma Vas Kor Vobis Kan Vor Daj 
Vorion Wolak Woola Xaxa Xaxak Xodar Yamdor Yersted Yo Seno Zad Zamak 
Zanda Zan Dar Zat Arrras Zithad Zuki Zu Tith
 * 
Cthulhoid by H.P. Lovecraft -- c'thulhu azathoth nyarlathotep zoth ommog lloigor cthugha yig atlach 
nacha abhoth byakhee cyaegha daoloth shub niggurath dhole dagon hydra 
shambler tsathogua ghast ghatanothoa ghoul glaaki gnophkeh yith yugoth 
shaggai gug hastur hali carcosa tindalos ithaqua nodens nyogtha shan 
shoggoth shantak shudde m'el xiclotl y'golonac yig yog sothoth zhar
 * 
Glorantha by Greg Stafford -- acos aldrya aleshmara annilla aptanace arachne solara araganthosas 
aranea argan argar arkat artmal asrelia atyar aurelion babeester gor 
bagog bolongo brastalos cacodemon caladra chalana arroy cronisper daga 
daka fal daliath danfive xaron daruda dayzatar deezola dehore dendara 
donandar dormal drospoly ehilm eiritha ernalda etyries eurmal faranar 
flamal framanthe gagarth gark gata gbaji genert gerlant glorantha 
godunya golod gorgorma gustbran harana ilor heler himile hobimarong 
hon-eel hrestol humakt humct hwarin dalthippa hyalor hykim mikyh ikadz 
inora iphara irrippi ontor issaries jakaleel jeset jmijie kajabor 
kargan tor keraun kolat krarsht krjalk kyger litor lamsabi lanbril 
larnste lhankor mhy lodril lokarnos loon lorian lumavoxvoran lux 
magasta mahome malia malkion manthi maran gor mastakos mee vorala 
metsyla mikaday mirintha molanni mostal murthdrya natea nelat noruma 
nyanka nysalor odayla ompalam orenoar orlanth ourania pamalt paslac 
phargon pocharngo polaris ragnaglar rasout ratslaff saliligor seseine 
selarn shavaya sikkanos slor subere talor teelo norri telask thalurzni 
thanatar thed tholaina tien triolina tsankth ty kora tek tylenea uleria 
umath umbrol vadrus valind valkaro valzain vangono varchulanga vivamort 
voria vovisibor vrimak wachaza waertag waha wakboth worlath xemela 
xentha xiola umbar yamsur yanafal tarnils yanmoria yara aranis yelm 
yelmalio yelorna yinkin zaktirra zaramaka zong zorak zoran zzabur argin 
brostange forang farosh gaiseron aggar agria ajaak akasta akem alatan 
aldachur alehandro alkoth spol argrath arkat armorn arnlor arsden 
arstola aruzban asgolan azilos baka balazar ballid banajasab basim 
basmol bastis baustin oskor belathgert belstos bikhy bina bang boneric 
borin boshan godunya brithini brol can shu carantes carman celmac chang 
tsai charg chi ting cho choralinthor daran janube naskorion otkorion 
sentanos smelch col congern corflu corola corostis darmangon culonmac 
dagori inkarth dangim dangk dara happa daran darjiin darleep defin 
anostos delecti delela luatha deu dilis dital dombain dona dorastor 
dorkath doskior dranz golo drom porfain ease easval egajia egarun 
einpor elz ast erengazor erenplose erontree esrol estal estau 
estaurenic ethilrist fanza fazzur feethos felster fethlon fiesive 
filichet fornoar foyalfine fral angor fraltigern fronela frowal fuknama 
galastar galin garsting garundyer garusharp gilboch ginorth gio gonn 
orta goropheng grachamagacan graclodont grombul guhan guiching guilmarn 
gundreken h'har as jing hachuan shan halikiv handra hanjan harandash 
haranshold haraxalur harrek harstar helby hendreiki heortland hingswell 
holay holut hrelar amali hsiang wan hsin yin hsunchen damal kralorela 
rathor telmor uncoling icilius imolo wen imther istakax ivex jairn 
janan vartool jankley bore jarngror jaubon jhor jillaro jolo jonatela 
joranit jord jorri jrustel junor kaisen kallyr kanthor karasal karse 
karstall kartolin kaufan destrino kaxtorplose keanos ketha khorst 
kilwin kitor kocholang koromondol kost kostaddi kuchawn kui hui lankst 
laonan tao laufol laurmal leplain lokow loskalm lur nop garusharp 
harandros mokwaha manday manir marost matkondu mazan melib ouri 
meriatan mirin mislar modaings moirades molene moralatap morocanth 
mortasor mulliam nevs nidan nimistor noastor nochet nolos noloswal 
noran noyelle okarn oral-ta oranor orathorn oraya orgrol ormsgone 
orninior oslir ozur palamtales palbar paps pasos pavis peidin pelor 
pent perfe pithdaros pomona pomons porent porfain pralorela prax print 
puchai quinscion raibanth ralios ralzakark ramal rascius rathorela 
argan argar kalikos siglolf tolat yuthu zitro argon retrint rhis 
rindland rist bailifes blackmor colymar volsax congern ryzel safelster 
saird salisor salona saranko sartar saug segurane sentanos seshnela sha 
ming shiyang skanth smelch snodal sodal sofal sog solanth solanthos 
sor-eel spol storal suam chow surantyr surkorion syanor sylila syndics 
syran talar malaskan talastar tanier tanisor tara tarasdal tarins tarsh 
tastolar tawars telmor temertain teshnos theoblanc thoskal timms 
tinaros tink tiskos torang tork tortun trowjang tung shiu tzu lung 
ulifilas uton uzgor valant valind valsburg vanch vargatyr varnaro 
varskolin varthulwal vaska long vent vesmonstran voi volonne voshfrei 
wanzow wexten wokistan worian yao fune yolanela yolp yuthuppa zanozar 
zorakarkat 
 * 
Jorune by Andrew Leker -- tologra trarch beagre dharmee dreglamon duradon losht scragger slutch 
vintch vodra cankle busk doul hilc keether rimeen ambreh mathin 
shirm-eh thon chausis dullurgronsloth hars snoggard sharwa nals nothim 
mershwa golbin flug ransley crebb shrin frillig flornblott drind 
ragsnath grunther daclish leclur fraznt reltha morfu edwain yoplon 
morthma jorgen ullens yonia gren olion pleathers eelshon she-evid 
abahth acubon ahdis anasan ardoth arrigish auss awss aylon blosh blount 
boc-rod boccord bochigon boru bronth burdoth caji cali-shafra cashiln 
cerridus challisk chaln docha chaln-imagri chaun-tse chawgis chell 
chen-ichi chiveer cie-ebba cle-eshta cleash clep cletch coddins coditch 
coleel condrij copra coprate corastin corondon cosahmi crendor creshi 
crethin crill crith croid crondij crudge crugar cryshell cygra daij 
daijic desti dhar dhar-corondon dharlerin dharsage dichandra diyorda 
doben-al dobre drail drenn drennit du durlig dysha dytes trinnu ebba 
enclep enervor entren eris fadri farg galsh-aca gashten gauthi gawmen 
lenk giddyne giggit gilthaw gire girrig gissyne githerin gobey heridoth 
herris hishtins illidge inclep iscin isho jampers jasp jorune kadija 
rhen kayedi kee kern kesht kesktia khaun gauss khodre kim kimit klade 
kuggin lamorri launtra leesh ebeeca lelligire lerrin lih limilate 
lirjin locurian grunder lundere lystra mandare miedrinth muadra mullin 
naull ninindrue pibber querrid ramian rilij rinis ros crendor rurvi 
tchorko rusper salu sarceen scarmis scedri scrade shaharras shal shambo 
shantha sharrid shast shenters shissic sho-caudal sho-sen sho sholari 
silipus sychill talmaron tan-iricid tan-sor tarro tauch-kie tauther 
techindol pib temauntro tenter-shines thailier thantier thisting thivin 
thombo thone thriddle tlesk tlon toth tothis tra trid triddis tristy 
vinteer voligire vosule wholl whosins woffen yobre yord yordeh yordigs 
assydre reet tsulya udah doben-al salam'arine ellipie liggnie durlis 
kauthnie kaigon sobayid laindis cavris joble baysis coise lusail 
cushindell phalmre delmre suh'larvan saller s'nabla scolia sydra sodrin 
ailaudra lus'ikai gendis ayns elds syls roise callips ardis glounda 
turrus hoit allidoth monerey sutor sorcle san loo'hoss delbah bleece 
albah saybah accaptas essanja essejee polpedroth sood bord burroo d'loo 
chvans seld bopley vamdrey yucid meard elcrellia hie mairamin soitle 
koistra ellih sholis alligre esh-eye ellemin dowsen ments sommint york 
simbi korrid ellcor tahs kymay herrmoe doo'sah herbis nayine d'lang 
coomis dich sho-ecta durris dryce oiders moether dharwin draugna othen 
vintch vintra austin derrid cassadons lorgin pesade bohoo hudson kipkle 
clavis lola sharben tocour wolton arkin persis laprendell es'wother 
rappenat cryeh t'lane laysis sobrinth danes hailer ald manser t'haynian 
kinster lelsh ellelsh palzer anamber lorri setta chelly 
 * 
Klingon by Marc Okrand -- ben law'qu' yIn vIlHem-tel nap loDvam 'ej vumqu'nIS 'a batlhDaj Suq 
qaStaHvIS wa'SaD DIS lamDaq Quch ngoHchugh vaj gheghlaHtaHqu' reH 
QuchDaj rotlh law' lam rotlh puS Huch law' ghajbe' 'e' SaHbe' pIvmo' 
qorDu' 'ej Hemmo' Soj SuqmeH wIj 'ej HuD SumDaq wam wamtaHvIS tlhej 
puqloDDaj juHqo' charghta' HI' nov yoSDaq wa' HI'Hom ngeHta' HI' HI' 
woQ luvuv latlh je pe'vIl 'e' qaSmoHlaw' HI'Hom woQ luvuvmeH rewbe' 
veng botlhDaq HI' Degh lanta' HI'Hom vaj ra' Deghvam quv bojuSpa' 
'oHvaD petor SIbI' torbe'wI' luqoplu' HI'Hom SoQ Qoybe' puqloD je 
HuDDaq wammo' vengDaq cheghDI' HI' Degh lujuS vaj ghoS HI'Hom QeH jach 
HI' Degh quvvaD petor jang jItorQo' HI' nov vIvuvQo' jItlhabmo' 
puqloDwI' je lotlhwI' tlhIv Saqop jatlh HI'Hom HI'vaD chaH qem HI'Hom 
tlhob HI' nuqmo' HI'na'ra' jIHbejbogh bovuvQo' ja' matlhabmo' not 
mochqoq SoHbejbogh vuv vajna' jang HI' gharwI' jat nuch tIq jatlh 
teHHa' woQlIj 'IwlIj DalarghlaHbe' 'IwwIj jeD law' 'IwlIj jeD puS 'e' 
vItobmeH yapbe' mu' 'e' vItobmeH vI'wIj'e' vI'ang jatlh puqloD 'e' 
tobbej vavwI' vI' pov pu'beH baHchu' Sorvo' jagh ghopvo' joq naH 
bachlaH vavwI' wa' qelI'qam 'oHDI' chuq'e' wejpuH 'e' vIleghqang 
bIvItchugh ghoplIjvo' naH bachnIS vavlIj jach puqloD chotIchlaw'pu' 
vavwI' jagh vIDaQo' jay' jang HI' vaj Sor'e' yIDa yotlhDaq chaH qem HI' 
HI'Hom je ja' HI' mIw vIQIj wa' qelI'qam chuqDaq puqloD luplu' 
ghopDajvo' naH bachnIS vIlHem-tel pu'beHvam yIbaH baHbeHbogh pu'beH 
tlhap vIlHem-tel naH puStaHvIS ghaH jatlh HI' vI'qoqlIj Hajba' 
puqloDlI' jatlh nuqjatlh jatlhqa' HI' Duvoqchu'chugh vaj nujDaq naH 
lanvIpbe' puqloDlI' qar'a' puqloDvaD Qum HI'Hom nujlIjDaq naH yIlan 
jang lu' qatlh mIw choHlu' ja' raptaH SanlIj rejmorgh yIDaQo' bItlaw' 
vIlHem-tel qeqmeH Qogh mach Hop bach 'a juSqu' 'uD'a' 'ej Haw' Qogh 
Hagh HI' HI'Hom je jatlh batlh Heghbe' HaghwI' tlhob HI'Hom DaHjaj 
Hegh'a' puqloDlIj'e' QeHqu'choH vIlHem-tel nom puS 'ej baH jor naH 'ej 
pay' ghomHa' puqloD Ho' 'ej vulchoH jatlh HI' vI'lIj DaDajpu' not 
bItlhabqa' reH qama'na'ma' SoHtaH pu'beH tlhe'moH 'ej HI' tlhejbogh wej 
mang'e' bach HI'Hom rIQqu'moH je 'a Haw' HI' HI' qImHa' 'ej puqloDDaq 
qet puqloDDaq pawDI' loQ vulHa'choHpu' jatlh puqloD vavoywI' pIch 
Daghajbe'ba' 'e' vItlhoj 'ach qatlh DubHa'law'pu' vI'lIj jang QeHchugh 
qoj bItchugh matHa' vaj puSHa' jatlh jIyaj 'ej Hegh jach baQa' Heghpu' 
puqloDoywI' quv bortaS vISuq nIch ngaSbe' pu'beH 'e' tu' vaj Doch 
lI'be' woD puqloD moy'bI' Ho' je woH nuHHeyvam qengtaHvIS HI' wam 
yotlhvo' cheghlI'bogh HI' SamDI' moy'bI' puSchu' jot QeHba' 'a 
jotbejmo' HI' mong mup Ho' bortaS neHpu'bogh Suqmo' rIntaH nom 
juHqo'DajDaq cheghmeH Duj tIj Dejmo' HI'tuy batlh maqqu'lu' lop 'e' 
tIvbe' puqloD quvmoHbe'lu'mo' 
 * 
Sindarin by J.R.R. Tolkien -- adan edain adorn aduial aear aearon aer aerlinn aglar aglarni aglarond 
aiglin aiglos alfirin alph amarth amlaith ammen amon amroth 
amrun anann anborn ancalagon andaith andrast andros anduin 
anfalas ang angband angbor angerthas angmar angren angrenost anim 
annthennath annon annuminas annun anor anorien arador araglas aragorn 
aragost arahad arahael aran aranarth aranuir araphant araphor arassuil 
arathorn araval aravir aravorn araw argeleb argonath argonui arnen 
arnor arth arthedain arvedui arvegil arveleg arvenienn arwen asfaloth 
athelas athrad aur balan balchoth balrog barad barahir baran baranduin 
baranor bas bel belain belecthor beleg belegorn belegost beleriand beleth 
belfalas benadar beraid beregond beren bereth bergil berhael beruthiel beth 
bladorthin borgil boromir brandir bregalad brethil bruinen cair calad calan 
calar calembel calen calenardhon calenhad carach caradhras caran caras 
carchost cardolan carn carnen celair celduin celeb celebdil celeborn 
celebrant celebrian celebrimbor celebrindal celebrindor celepharn 
celeirdan celerdancelos cerin certh certhas cerveth chaered chebin chost 
cir cirdan ciril cirith cirth conin cor cormallen craban crebain cuio 
curunir deadelos daer daeron dagor dagorlad damrod dain dan daro daur 
degil denethor derufin dervorin din dinen dinguruthos dior dir dirhael 
diriel dol dor dorenernil doriath dorthonion dorwinion druadan druwaith 
duath duhirion duilin duin duinhir dum dun dunadan dunedain durthang 
echant echor echuir ecthelion edain edhel edhellen edhellond edhil 
edraith edro egalmoth egladil eglerio eithel elanor elbereth elenath 
elin elladan elmoth elrohir elrond elros elwing emyn enedhwaith enedwaith 
ennor ennorath ennyn enyd ephel eradan erain erebor ered eredluin eregion 
erelas erestor eriador ernil erui eryd erynesgalduin esgaroth estel ethir 
ethring ethuil falas fan fang fangorn fanghorn fanui fanuidhol fanuilos 
felagund fenn fennas fimbrethil finarfin findegil finduilas fing finglas 
fingolfin finrod firith fladrif forlindon forlond formost forochel forod 
forodwaith foron fuin galad galadh galdhon galadhremmen galadhrim galadon 
galadriel galadrim galathilion galdor galen galenas galion gaur gaurhoth 
gebir geleb gelin gil gildor gilgalad giliath gilion gilraen gilrain 
gilthoniel girion girithron glamdring glanduinglorfindel glos golasgil 
gond gondobar gondolin gondor gonui gorgor gorgoroth gorthad govannen 
guldur gundabad guruthos gwae gwaeron gwai gwaihir gwain gwanur gwath 
gwathlo gwirith hador hael hain hal halbarad haldir hallas han harad 
haradrim haradwaith harlondon harlond harnen harondor haudh hen henneth 
herion hin hir hirgon hiriath hirluin hith hithaiglin hithlain hithoel 
hithuihollen horn hoth huor huorn hurin iarwain iath iaur iavas idril 
imlad imladris imloth inglorien ioreth iorlas ithil ithildrin ithilien 
ivanneth ivorwen kelos kiril kirith laer lain lam lamedon lammen landroval 
las lasgalen lasto lebennin lebethron lefnui legolas lembas lhaw lhun lim 
linn lindir linhir linnathon linnod lith lithui lithlad lond lor lorien 
loss lossarnach lossen lossoth loth lothiriel lothlorien lothron luin 
luthien lyg mablung mae magor mal malbeth mallen mallor mallorn mellryn 
mallos malvegil marmedui megil melian mellon mellryn melui menel meneldor 
menelvagor mereth merethrond methed methedras min minas mindolluin 
minhiriath minmo minrimmon minuial mir miriel miruvor mith mitheithel 
mithlond mithrandir mithren mithrin mithril mor morannon mordor morgai 
morgoth morgul morgulduin moria morn morthond morwen muil nachaered naith 
nallon nan nanduhirion nantasarion nar narbeleth narchost nardol 
nargothrond narwain naug naugrim naur ndaedelos nef neldor neldoreth nen 
nenuial ngaurhoth nguruthos nim nimbrethil nimloth nimrais nimrodel nin 
nondalf ninui niphredil nogothrim nogrod noro norui nurn nurnen onen 
onod onodrim oraeron oranor orbelain orch orcrist orgaladh orgaladhad 
orgilion orithil ormenel orod orodnathon orodreth orodruin orophin 
orthanc osgiliath ossir ossiriand palan palandiriel pant parth pedo pel 
pelargir pelen pelennor peleth penna per peredhel perhael periainn 
periann periannath pheriain pheriannath pinn pinnath poros rais rammas 
randir rant rass rathrauros raw remm remmen remmirath ren rhiw rhosgobel 
rhovanion rhu rhudaur riel ril rim rin ring ringlo roch rochand rochann 
rodyn rant rass rath rohan roheryn rohir rohirrim rond rumil samm sammath 
sarn sarnathrad sauron serni sernui silivren sir sirannon sirith suil sul 
taith talan talf targon tarlang tasar tasarinan taur taurnaneldor teithant 
telchar tew thangorodrim tharbad thingol thiw thon thond thoniel thoron 
thorondir thorondor thorongil til tin tinuviel tir tiriel tirith tiro 
tiw tolbrandir tolfalas torech torn torog tumladen tuor tur turgon tyrn 
uchebin udun uial uilos ungol ungoliant urui vagor vedui vegil veleg vir 
vorn wain waith yrch
 * 
Tsolyani by M.A.R. Barker -- ahoggya aira ajjnai akho akhone aknallu aleya allaqi aomorh aqaa aqpu 
aridani aridzo arjasu arkhane arkuna arodai arruche arshu?u ashonu 
ashoretl ashqo atlun avanthar avanthe bakte balar barraga bednalljan 
bejjeksa belkhanu beshene beshyene bey biridlu bu?uresh burushaya 
bushu?un ch?ochi chadra chagotlekka chaigari chaimira chaka chanayaga 
chankosu chareshmu chargal charikasa charkha charukel chashkeri chayakku 
che chegarra chegeth chekkuru chengela chessa chirringga chiteng chlen 
chnelh choi cholokh choqun chosun chri chriya chulin chumaz chumetl 
chunatl churitashmu chusetl daichu dalisa dalisan dardayel dashiluna 
datsu delashai deq deshetl dhich?une dhiyalto dijaya dikkuna dilinala 
dimani dineva dirresa dlakolel dlamelish dlantu dlaqo dlekkumine dmi dna 
dnelu dormoron dra drantike drarsha dri dridakku dritlana dronu 
durritlamish durugen durumu durun dzor edluchcho ekune engsvan equnoyel 
erbule eselne eyil fashranu fasiltum fatlan fereshma?a ferruga feshenga 
firya freshshayu fressa fulat gagarsha gamalu gamra gamulu ganga 
gaqchike gashtene gatl gayel ge?en gerednya gereshma?a ghar ghaton 
girandu giriga girikkteshmu giriku giritlen gruganu guodai gupa gurusha 
gusha gyesmu gyush hagarr haida haikon haqel haringgashte harsan harsan 
hehejallu hejjeka heketh hereksa hes heshqu heshtu?atl hessa hetkolainen 
hijajai hikavuku hiketkolel hikhorsan hirilakte hirkane hitankolel 
hituritlano hiviridama hla hlaka hlakh hlakme hlash hlaya hlektis hli?ir 
hlikku hling hlutrgu hlyss hma hmakuyal hmelu hnalla hnequ hokun 
horkhunen horukel horusel hra hrga hrgash hrihayal hrishmuna hru hru?u 
hruchan hrugga huathu huketlayu hursha huru?u hutligainu hyahyu?u 
hyashra ito jaikalor jakalla jalesa jalugan jannu janule jgresh jirega 
jneksha?a jugar ka?a ka?ing kachor kaikama kaingmra kaitar kakarsha kana 
kanmi?yel kao karakan kareng karin kashi katalal kayi kazhra ke?er 
kekkeka kereng ketengku kettukal kevuk kha kheshchal kikumarsha 
kilalammu kirega kiren kirikyagga kokor kolumejalim kolumel kor korikada 
koyluga kru?om krua kruthai ksarul ksarul ktelu ku?eth kuni kureshu 
kurgha kurritlakal kurrune kurshetl kuruktashmu kuruku kurutesh kushi?il 
kutume kygashtene kyni larayn lei lelmiyani livyanu llelmuna llyan 
llyanmakchi lo?orunankh lorun lorunje lri ma?in maghz maraggu marashyalu 
marassu mash mashyan mejjai mengano mennuke meshmuyel meshqu metlunel 
mi?royel mihallu mikoyel milumanaya mirigga miriya mirkitani miruene 
mirure mirusaya mirusiya mishomuu missum miuna mnashu mnekshetra mnesun 
mnettukeng mnor morusai mriddu mridobu mriggadashu mrissa mrissutl 
mriyan mrugga mrur msinu mssa mu?agh mu?ugalavya mudallu muresh mursun 
n?len nakome nalcome nalukkan narkonaa narku nasundel nayari ndalu 
ndarka nekkuthane nenyelu neqo nerre neshkiruma ngaku ngangmorel 
ngangmuru ngayu ngharradu ngi ngoro ngrutha ngungethib nikuma nikuma 
nimune ninu?ur niritlal njenu nlyss nmartusha nrainue nriga nshe nto 
nu?unka nurgashte nurtsahlut nuru?un nyagga nyelmu nyemesel nyerebo onmu 
onumish origob oso pa?iya pachi pakala paranta pardan parunal pavar 
pavar paya pe pe?e pechani pechano pelesar penom pijena pijenani 
purdanim purdimal puru qame?el qaqtla qasu qaya qel qirgal qiyor qol qon 
qoruma qu?u qumqum quren qurruma quyove ranchaka rannalu rantike renyu 
rereshqala reru ri?isma riruchel rirune rsa ru?un ru?ungkano ruvadis 
ry?ytlanesh saa sagun saina sakbe saku?u salarvya sangar sarku se?eqel 
sea semetl serqu serudla sescha shaira shamtla shanu?u sharetlkoi shedra 
shen shichel shiringgayi shqa shra shretsaya shruka si si?is sikun 
siridlanu sirsum sirukel sne sokatis sra sra?ur sraon srigash sriyesa 
sro sruganta srukarum sruqu sryma ss ssa?atis ssamiren ssar ssdune 
ssirandir ssiyor ssormu ssorva ssra ssru ssu ssu?um ssuganar ssussu 
ssuyal su subadim subadim suchel sunraya sunum sunun sunuz suor sur 
surgeth surundano suzhan sy tamkade tane tanule targholel tariktame te 
tekai tekumel tengguren tenturen teqeqmu teretane teshkoa teshkuma 
tetkumne tetukel thayuri thendraya thenu thesun thirreqummu thomar 
thraya thri?il thu thu?usa thumis thunru?u tikasa tilekku tinaliya 
tirrikamu tiu tiyotl tka tkel tkemar tku tlakotani tlalu tlaneno tlar 
tlashte tle tleku tletlakha tlokiriqaluyal tlomitlanyal tlonu tnalum 
tnek todukai tolek tomua tontiken torunal trahlu trakonel tremunish 
trinesh tru tsa?avtulgu tsahlten tsamra tsan tsaqw tsarnu tsatsayagga 
tsaya tsemel tsi?il tsoggu tsol tsolei tsolyanu tsu?uru tsuhoridu 
tsumikel tsunu tsupil tsural tsuru?um tuleng tumissa tunkul tuor 
turshanmu ulela uletl uni unukalhai urmish urtse urunen usenanu utekh 
var varis vayuma ve vengakome verussa vgaish vimuhla vinue vipu vorodla 
vra vrasen vrayussa vrelq vres vretlish vri vriddi vridekka vridu 
vringalu vringayekmu vrishmuyel vriyagga wasuro wuru yafa yamashsha yan 
yarsur yau yeleth yetil yilrana zaftla zagu zaima zakle zalbuzhayal 
zamsher zanatl zantla zaqe zaren zetl zhabar zhagu zhahlan zhai zhaime 
zhaldai zhalmigan zhamak zhauru zhavu zhe zhitlin zhiyang zhufen zhukatl 
zhul zhune zhur zhurau zhurtla zigau ziris ziruna zirunel zisbe zish 
znayashu zrafe zramahl zrne zu?ara zu?ur zurne zuruna zushahla 
 * 
Albanian (words) -- afatgjata fillese librit nga prapambetje sidomos 
afatmesme fisnik lidhjes nga pretenduar sigurimin 
akute fizike lihet nga prire sikurse 
ardhmen forte lufterat ngjet prizrenit sone 
ardhmeria frasherit madhe ngrehim pse sot 
ardhmes fusha marre nisje pushtimet sot 
ardhmes gjashtedhje mbare nje qe sugjerojne 
ardhmes gjate mbarekombet nje qe sukses 
ardhmes gje me nje qe synimesh 
arrit gjinden me nje qe synimeve 
arsyeja gjitha me nje qe t'i 
asnje gjithsesi me nje qe t'i 
ate gurthemeli me nje qellimin t'iu 
autoreve guxim me nje qene ta 
bashkepunim hapesira me nje reja ta 
bashkimin hapesirat me nje rendesishem te 
behet hapesire me nuk reth te 
botimi hedhim me nuk rihen te 
brezave here me sa te 
c'do huaja me sami te 
c'eshte i me se te 
c'ka i me pa se te 
deshirash i mendimet pa se te 
deshmi i mendimet parashtron se te 
dhe idete mendimi pare se te 
dhe ka mendimi pareshtura se te 
dhe ka mendimin pas se te 
dhe ka mendimit pasurine se te 
dhe kane mendjet pati se te 
dhe kapercejme mendore pavaresise se te 
dhe kaq menyre per se te 
dhe kjo mira per se te 
dhe koha mirefilli per se te 
dhjete koha mirefillta per secili te 
dihet koha mirepo per sensibilizo te 
dijenite kohes mos per sepse te 
dijetareve koke mund per sfidat te 
do komb na per shekulli te 
do kombetar na per shekulli te 
do kombetar ndertese per shkencor te 
do kombit ndertimin per shoqerisht te 
duam kombit ndihet perballuar shoqeror te 
e krijime ndonjehere perfshin shpejt te 
e krye ndritura perket shpresojme te 
e kryesore ndryshme perkushtim shqiperia tejet 
e kthehu ne permbledhe shqiptar tema 
edhe ku ne perpara shqiptar thjeshte 
eshte ku ne perpara shqiptare tij 
eshte kurre ne perparoje shteroi tjeter 
eshte kurre ne perparuar shtete tona 
eshte kushtet ne perpire shteti trasheguar 
eshte kushtuar ne persa shume tre 
eshte kushtuar ne pjekur shume trojeve 
etnike ky ne plote shume u 
faqen ky nesh podiumi shume u 
faqet lejuar nevoja prandaj shumta u 
varfer vellimi vepren vetem vlen zgjidhja 
varferia vellimi vere vine vlera zgjidhjen 
vazhdimisht vemendje veshtrim vizionit vyer zhvillimit 
vellim vene veshtrimin 
 * 
Ancient Egyptian (text) -- kheri heb tep kheriu kefau-autu amu ament ahau metahu nuh-em-tuat nutchi erman-sah hotep 
kenemut heteput sebu she kherit ramses tutankhamen neter kheriset kherp sat hemt sati bat sa 
nesu repat hur hemm sa-she sataakhuet amenti sapanemma samaat het-her sasat sati-sat set ra 
sasheshem saqet satiquet sa-tathenen saut sa-en-ankh ankh ankh heru ast saiu-t saem-geb geb 
auhep abu abtu mehtt apet amu amur-ta amsu arpakh arsa hamrur akthata ateph peh khent at 
aauab turent heter heru neheb mekha uatch-nar narmeph men qebhu smer merbepa baiuneter 
batau kakau ban-neter senta nefer-ka nefertiti nefer-seker seker bebi nebka hetep sekhumui raneb 
sekhemab perenmaat perab khasekhem tcheser setches hu huni khaba ka-heru seneferu suhetes 
khufu khafra menkaura shepses userka sahu ari-ka enuser-ra pepi teta netaqerti nefres nebi maa 
khentu meren-hur efer senb annu hetep nebkau skhanra khauser nubtaui aahetep-ra aakha maab 
antef erpa menthu-hetep uah-ankh tep beb neb sankhu abkhent sehetep emhat usert kheper kau 
ka amen sebek neferu sertsen auphni mashau suatch herua tet tatu uben nehsi tata herab sekheper 
smen seba khau sesh sebkai khuaqer bebenem isis khentcher apepa pepa suser khepesh pehti ra 
nubti iamu seket shesha sekhem nebari taui penen-set khent-set heru-netch-tef netch tef shet 
her-her maat upu anteph nub senekht seqen kames aahmes nebpehti tehuti hat-shepsut shepsut 
kheperu hipa peru tcheser ramses ramessu ptah ptolemy meses meren-sa arsu nekht meri rameses 
ketep seti ankh netchem masa tchet khensu menkheper auapet nesbaneb nebtet takheperu amen 
amon osiris horus thoth apis ptah kheser-ptah anubis khemmis mesut-ra uhem nekau tem pem 
tanuat psemthek tetkau-amen psemkheser sankhuptah amenti-uhem bast basutent setut tut sem 
naiph aurut khnem psah-mut mekhapsah heteputsah herusah ptuluhem ptulsmer ptulsati ptulahau 
cleopatra patra nefer-rapat mehtt-ra tenen aakhut asut mennu ament aatuart aat ainnu au-ten 
amenit auaram auat bata batap aunaph aurm ab abhat api ap apit apturt aputnub apuqen aptu 
afttit amt amam amur amut amhur amkau amit amurt ampeh ament amen-sekher amen-kheperu 
amubes thebes memphis neferphis sebuphis kneriphis huniphis nes-ptah sebkiphis seneferu tehen 
nut pteh menpteh perab-pteh annupteh hesmener anreb rebis anbeth aneb ameb apeht arsuphet 
rut annu-rut behut setet tereht tchemeser annimrem ankenna ret reter parit arar penkha arenku 
nubenku khnemenka ammon ib ipu is imh irut ilut irep ih iha ihem irem ipensu iheshep shep uabut 
meh uatmeh upumeh shepmeh narmeh uaht uakh tetuakh masuakh uah hema ur nebtu nebamen 
asut alut amut anut anet amet kephet khesphet memphet-ra umis unast unanat nesu unes urt uheb 
uhat uhut her-maati upenpet user user usekh serhat sekhef sesh uthent utent hehtt tcheser ba bat 
bati banebtet tchet batet baam baarut bantaph bauap bau-kem baut berast bahit baht bahtet bakh 
behut bakhther bast bakem thupar huaris batapuh asutpuh batchabu baastu khufu bah assah bit 
baut buut butur er neteru kheftiu tchetiu seshiu mereniu emhatiu mesutiu enti buten bumeph buh 
beba bepset benen behen behset beht-ur behut-ut apis-ut nekht bekhen besit papena pamu pamer 
paner paneb parut paruph parbu parptah parem parekhti paherhem pasiu pasebti hehti uhemti ep 
masati kepeti shet shet patep patap pahit pomer pmer pmer-ra pmeranbet kames-pmer per perap 
asarneb asartet nemur nemuraph nemurt astis aakhepher ankhet upuatu hemt hemset perut tehuti 
khafra anebph feram maat maatiu maam maa-kheru maut mabarepu marem makha masti masaret 
mati maatiu mur muti m'ntchu m'uti m'nemu m'ankhasu m'anub m'khirpis m'kher m'shaubat 
m'keter muent munesert muneter mushant mebushem memtu memu memthu anbethu behethu 
kephethu mehem meh meh menat menasut senmut nefermem ment nenu-memt nenu meratem 
atem merit merur mermaat mernefer mehu meht mehi mehi mekhem mekhenem mesip meshet 
naat naluthu nabur nait natip naap naaph nankh narut nahrem nahsem napt hapt khapt phapt 
bapt mapt thapt uapt pertep perkhet narkhet nershem nirab nut nenut nuset nuphet nutamen nep 
nutent nut uast mestut nutmebteh patapeh seh iteh khempeh nubit nukheper nutenkham khamen 
tcheseph nuthem perthem nubti numam nuhtem nebaru nebankh nebit nebut nepi hepi heper 
nept nefer amenti amentiu uatu herakaph ereb erub nefris hesut assa sehan memt nemt nemit 
neht nehaut memaut baasaut khemaut tutamaut seshaut khepaut nebaut nehest nekhebt memept 
sesept khepept khemept nept sept ptahsept sept-ra nept-assa-ra nemher herhamept nehimiu nes 
nekhen neshen nest nest tumhenest neserer neshaut nemufu setufu khemufu hemufu psemufu 
ramsem ra-heru-hetep rankh raabti raat raam rebis rebkheper rebkhenten ra-rebnehit reper reput 
repeh remeq remaq rent hent remur-nefer renem renekhen rereber rehahti rehen rehat rehit rebit 
rebkhem rabkhem rapet rapetiu rament remufu remereb reset reshaut reshit khert hert hembufu 
hebt heb hen hentiu herneph heker heptah het hetauab hetep heteb betaah hetaap-ha hetab hetapit 
hetament hetaunt hetar akhmiu ament hamentiu heturt meturt neturt heurit heurem heurenkh 
hetbaat het-benu bepi hepi hetmut hetmes hebis nemufu hetnehem hetnes hetneshent sa-hent 
neterst ant netaph metaph rekhit hekhit hekis hekhemper hekhemt hekhemsis hesmen hestut 
henkh hekhat hetkhebit hetsmen khamemsem sebakhem sent semshaat sehetep heptah hepsis 
hap hapuant hapnebes renebes mephebes memphis sebaphis khephis hapet hamet haphet hapehit 
hapmes hapest hebkher heptah hefusu hemit hemut hemetep hensiu heturit herurem heru-maat 
heru-ament heru-semtenit hesp heserhesp khesp heser khatiu kharem kharb kharbuhem kharet 
kharankh khasu khamkhep khashebut khast khasaut khahamit emhat khameht khament khamesu 
khebit khep kheft khefu khemset khemkhem khemtut khemenu khenbat khensu kemet deshret 
akheperkare addaya ahhotpe ahmes ahmose ahmose-saneit ahmose-sipari akencheres akunosh 
amenakht amenakhte amenemhat amenemheb amenemopet amenkosh amenhirwenem amenhotpe 
amenmesse amenmose amennestawy amenope amenophis amenwahsu ameny amosis-ankh 
amunemhat amunherpanesha amunhotpe anen ankhef ankhefnamun ankhefenkhon ankhefenmut 
ankh-psamtek ankhsheshonq ankhti ankhu ankhuemhesut apophis baba baenre baen bak 
bakenkhon bakenkhonsu bakenmut bakennefi bakenptah baki besenmut butehamun deniuenkhons 
djadamankh djau djede djedhoriufankh djedi khonsue ptahefankh djehutmose djeserkare 
serkheprure sersukhons thutmose djhutmose henubat aankhef hapimen hapu hapuseneb hapimen 
haremakhet haremsat harkhebi harkhuf harmhabi harnakhte harsiese hemaka henenu henuka 
heqaemeheh heqaib neqaib kheqaib amenqaib nampen herihor hesire hor horus horemheb 
hori horpais karnak luxor horweb hunefer ibana ibe ikeni ikui imhotep inaris inebni ineni inyotef 
ipi ipuwer ishpi iu-amun iufenamun iunmin iuseneb iuwlot imeru khahma khanakht kharnefhere 
khatenen khawab khemuniu khenamun khenefer kherasher kha khemhet khemnetheru khemwaset 
khahor khakheperraseneb khensthoth kherueph kheti khemibre khumhotep khumhotpe khon 
khonsirdais khonskhu khonsuemwaset khufuhaf kuenre maakhen mahu mahempri ma'nakhtuph 
manet masahart mehi meketre mekhu menkheperraseneb menkheperre menmet-ra menna 
mentuemhat mentuherkhepshef meremptor merenamun merenkhons meren-ptah mereriu merka 
mernebptah mes min menkhat minmose minnakht mokhtar montuemhat montuhirkopshef 
montuemhet mose naga-ed-der nakhthorheb nakhtimenwast nakhtmin nakhtnebef nanefeptah 
nebamun nebankh nebemakst nebhotep nebimes nebit nebmahtre nebnefer nebseni nebwenenef 
nechoutes neferhotep neferhotpe neferkheperuhersekheper nefermaet nefermenu neferrenpet 
neferti nehasi nehi nekau nekhwemmut nendbaendankh neneferkaptah nenkhefta nes nesamun 
neshi neshorpakhered neskhons nesmont nespaherenhat nespakashut nespatawt nespherenhat 
nessuimenopet nestanebetasheru nestefnut netihur nemed nemlot niumateped phabasa phabernef 
khadiamenet khadiamenipet khadiamun khadineith naheripedjet kaair khait pakharu pakhneter 
pakhmont pamose pamu panash paneb pasebakhaenniut pasekhonsu paser pashedbast pashedu 
nediese khedihor henamun henbu henmaat hennest hentaweret fentu hepynakhte prahotpe 
psamtek psenamy psenmin ptahhemakhet ptahhemhat-ah ptahhotep ptahhukhef ptahmose 
ptahshepses rahotep rahotpe raia ramessenakhte ramessu rekhmire reuser rewer rema-ra radamun 
sabef aabni halatis samut sanehet sasobek sawesit scepter sekhemkare sekhmire seneb senebtef 
seenemut senmen seennedjem sennefer sennufer senui senwosret serapis sese setau setep sethe 
sethherwenemef sethhirkopshef sethnakhte sethnakte seth setne sethmerenptah shedsunefertu 
snafu senet shem shepenwepet hiese si-mut sinuhe sneferu semtutefnakhte sasetmer tefibi 
tenermentu teti-en tetisheri baenhebyu tahapimu tahremhent tahuemet tennaph tutun tunisba 
uhorresne uhorresnet uhi userhet usermontju sebmose wahibreh-khen wahkhent webaoner 
webensenu weni wennefer wennufer hwepmose wawetmose werdiamenniut herirenptah anhamu 
abana  ahwere amenirdis amenkhenwast amosis anhay ankhesenamun ankhesenaten 
ankhesenneferibre ankhetperure ankhnesmeryre ankhnesneferibre asenath baktre baktwerel 
bheketatent esemkhebe esemankh esam-ptah esamhenbufu esameb esameph esamrema esamhenit 
hentempet hentmehu henut henutmire hetepheres hrere hruiu inhapi ipip ipufu isetemkheb 
isetnefret isemkheb iuhetibu karpes kawit kem khedebnethireretbeneret khutenptah maatneferure 
maatkare mehareph maatare mehetweshkhet mehtetweshket mehtenweskhet meketaten meketre 
mekhare meresankh meritat merit mutemhab naneferher nani nenakhte nebefer nebnopret neferet 
neferhetetepes neferneferuat nefer kheferu kheferubit kheferuptah kheferure kheferusherit 
neskhons nestanebtishru nitetis nitqaib nutnakht nodmet nopret nubkhaes nubkhas ra rededet 
onet satehut satnebetneninesu sebtitis senebtisi senebt senet senmonthis sennu sentnah shesh 
sit-hathor-iunet sitkamose sitre sobekhemshaf sotepenre sponses tabes tabesheribet tabeshufu 
taheret tahpenes tahretdjeret tais taiheriu takhat tamin tanetnephthys taweret tayuheret tetisherit 
 * 
Ancient Greek (female names) -- Anticlea Xenoclea Meda Deianara Aegialeia Clytemnestra Periboea Hesione Leda 
Helen Xanthippe Chloe Daphne Circe Orithyia Nacippe Penthesilia Sibyl Eidyia 
Actaia Actoris Aerope Aethra Aethylla Aganippe Aglaia Alcimede Amphinome 
Arne Astynome Astyoche Autolye Callianeira Canache Chione Clytie Creusa 
Cymodece Danae Deidameia Dirce Dynamene Eriphyle Eurynome Galatea Halia 
Hiera Ianassa Iaria Leucippe Limnoraea Mante Maera Melantho Melite Metaneira 
Nacippe Nemertes Nesaea Otionia Panope Perimede Periopis Pero Pherusa 
Philomele Polymede Polymele Polypheme Polyxena Prote Protogoria Scarphe Speio 
Tecmessa Thaleia Theano Thoe
 * 
Ancient Greek (male names) -- Alastor Anthemion Antiphos Arcesilaos Archilocos Ascalaphos Astinoos Charopos 
Chromios Clytius Crethon Deicoon Democoon Diores Echemnon Echepolos 
Elephenor Epistrophes Ereuthalion Eunos Euryales Eurymedon Eurypylos Haimon 
Hicteon Hypsenor Ialmenos Idaios Iphicus Iphitos Laodice Lampos Leitos 
Leucas Mecisteus Meges Meriones Mynes Nineus Orsilochos Panthoos Peiros 
Pelasgon Peneleus Phaistos Phegeus Phereclos Podarcos Polyeides Protheoenor 
Pylaimenos Schedios Sthenelos Strophios Tecton Thepolemas Thoon Thymoites 
Ucalegon Xanthos Adrastus Aegialeus Diomedes Theseus Menestheus Oeneus Gorge 
Heracles Argus Idomeneus Anius Phyleus Acastus Laertes Odysseus Hypsipyle 
Euneus Philomeleides Alcathous Agamemnon Erginus Alcinoos Arete Admetus 
Alcestis Eumelos Peleus Nestor Antilochos Thrasymedes Telamon Periboea 
Aias Teucer Lycomedes Tyndareus Castor Polydeuces Menelau Creon Tiresias 
Manto Alcmene Thespius Megamede Eurystheus Admete Pittheus Hippolytus Priam 
Hecabe Aeolus Dido Oenopion Aeetes Chalcipe Paphos Iasus Anchises Phaidimos 
Midas Telopelemus Iolaus Megara Thoas Cocalas Daedelus Alcibiades 
Platon Socrates Aristoteles Demosthenes Diogenes Cratylus Gorgias Philemon 
Thucydides Herodotus Plotinus Heraclitus Zeno Protagoras Hermagoras Longinus 
Anaximander Pythagoras Oedipus Androcles Aristophanes Euripides Aeschylus 
 * 
Arabic (female names) -- Abia Abida Abir Abeer Ablah Abla Abra Adab Adara Adiba Adila Adilah Adeela 
Adiva Afaf Afifah Afra Ahd Ahlam Aidah Aida Ain Aini Aishah Aisha Ayishah 
Akilah Alhena Alima Alimah Aliyyah Aliah Alia Almas Aludra Alzubra Amal 
Amal Amala Amani Amatullah Amber Aminah Ameena Amineh Amirah Ameera 
Amtullah Anan Anbar Anisah Anwar Ara Areebah Arub Asima Asiya Asma Atifa 
Atiya Ayah Azhar Azizah Azeeza Azzah Badia Badra Badriyyah Bahira Baheera 
Bahiya Balqis Banan Baraka Bariah Barika Bashirah Basheera Basimah Baseema 
Basmah Batul Batool Bilqis Bushra Buthaynah Buthayna Cala Cantara Dahab 
Dalal Duha Dhuha Fadilah Fadheela Fadwa Faizah Falak Farah Faridah Fareeda 
Farihah Fareeha Fatimah Fatima Fatin Fatinah Fawziyyah Fawziya Fazia 
Fellah Firyal Firdaws Firdoos Ghadah Ghaliyah Ghaniyah Ghayda Ghusun 
Ghusoon Habibah Hadeel Hadiya Hadiyyah Hafsah Hafthah Haifa Hayfa Halah 
Halimah Haleema Hamidah Hameeda Hana Hanan Hanifah Hanifa Haneefa Haniyyah 
Hasna Hayam Hayat Hessa Hibah Hind Huda Hooda Huma Huriyyah Hooriya Husn 
Husniyah Ibtihaj Ikram Ilham Iman Imtithal Inam Inas Inaya Intisar 
Inzdihar Jala Jamilah Jameela Janan Johara Jumanah Kalila Kamilah Karida 
Karimah Kareema Kawthar Khadijah Khadeeja Khalidah Khayriyyah Khairiya 
Khulud Khulood Kulthum Kulthoom Lama Lamis Lamees Lamya Latifah Lateefa 
Layla Leila Lina Leena Lubabah Luloah Lulu Madihah Madeeha Maha Maisah 
Maizah Majidah Majeeda Makarim Malak Malika Manal Manar Maram Mariam 
Maryam Masouda Mawiyah Maymunah May Maysa Maysun Maysoon Mayyadah Mufidah 
Mufeeda Muhjah Muminah Muna Munirah Muneera Mushirah Musheera Muslimah 
Nabihah Nabeeha Nabilah Nabeela Nada Nadia Nadidah Nadeeda Nadirah Nadwah 
Nafisah Nafeesa Nahlah Nailah Naima Naimah Najah Najat Najibah Najeeba 
Najiyah Najla Najwa Najya Nashida Nashita Nashwa Nasiha Nasira Nathifa 
Nawal Nawar Nazahah Nazihah Nazirah Nibal Nida Nimah Nimat Nouf Nudar 
Nudhar Nuha Numa Nur Noor Qubilah Rabab Rabiah Radeyah Radhiya Radwa 
Radhwa Rafa Raghd Rahimah Raidah Raja Rana Rand Raniyah Rasha Rashida 
Rasheeda Rawdah Rawdha Rawiyah Rayya Rida Rihana Rim Reem Rima Reema Rukan 
Ruqayyah Ruqaya Ruwaydah Sabah Sabirah Safa Safiyyah Sahar Sahlah Saidah 
Saihah Sakinah Sakeena Salihah Salimah Saleema Salma Salwa Samah Samar 
Sameh Samihah Sameeha Samira Sameera Samiyah Sana Sawsan Shadha Shadiyah 
Shahrazad Sharifah Shareefa Siham Souad Suad Suha Suhailah Suhaylah 
Suhayla Suhaymah Suhayr Suhair Sumayyah Sumaiya Sumnah Tahirah Takiyah 
Talibah Tarub Taroob Thana Thara Thurayya Umayma Wafa Wafiqah Wafeeqa 
Wafiyyah Wafiya Wahibah Wajihah Wajeeha Walidah Warda Wardah Wordah Widad 
Wijdan Wisal Yafiah Yakootah Yamha Yaminah Yasirah Yasmine Yasmin Yasmeen 
Yumn Yusra Zafirah Zahirah Zahra Zahrah Zainab Zaynab Zakiyyah Zaynah 
Zaina Zubaidah 
 * 
Arabic (male names) -- Aleem Aalee Abdul Azeem Azeez Baari Baasit Fataah Jabaar Ghafaar Ghafoor 
Haady Haafiz Haq Hakam Hakeem Haleem Hameed Haseeb Jaleel Qaadir Kareem 
Lateef Majeed Muiz Mutaal Mujeeb Mateen Muhaimin Nasser Qudoos Qahaar 
Subduer Raafi' Raheem Rahmaan Rasheed Razaaq Salaam Raouf Khaaliq Crear 
Saboor Samad Samee' Abdullah Shakoor Tawaab Wadood Waahid Wahaab Abaan 
Abbas Al Abbas Abedin Abed Adnan Adham Ashraf Aadil Afeef Ahmad Akram Alaa 
Alaa Ali Ameen Ameer Amjad Aamir Ammar Amr Anas Anees Aarif Awad Ayoob 
Ayman Aws Azhar Azzaam Abu Bakr Abul Khayr Aasim Ataa Arsh Badr Baha 
Baahir Bahiy Bashaar Basel Basaam Baasim Smil Bilal Bishr Burhaan Dawoud 
Dhul Fiqaar Diyaa Fadi Fadl Fadl Ullah Fahad Faisal Faakhir Fakhry Fareed 
Faaris Faarooq Fateen Fawaz Muhammad Fidaa Faris Fuad Ghaalib Vicr Ghasaan 
Ghaazi Ghiyaath Haady Haamid Hamza Haani Humam Haarith Haaroon Hassan 
Haashim Hassaan Haatim Haytham Haashim Huthayfa Husaam Husaam Ihsaan 
Hussein Houd Ibraheem Idrees Ikrimah Imaad Imaad Udeen Imraan Irfaan Iesa 
Is-haaq Isaam Ismaael Izz Jaabir Jaafar Jalaal Jamaal Jamaal Jameel Jihad 
Jawad Kamal Kaamil Kareem Khairy Khair Khaldoon Khalid Khaleel Khuzaymah 
Labeeb Luqmaan Lutfi Ma'n Ma'd Mahdy Maahir Mahmoud Makeen Majd Majd Majdy 
Mamdouh Mamnoon Mansour Marwan Marzouq Masoud Maysarah Maazin Misbaah 
Muaath Ma'awiya Umayyad Khalifah Muayid Muhannad Mufeed Muhsin Muhtady 
Mujaahid Mukhtaar Munthir Cautir Muneer Muntasir Musad Mustafa Mutasim 
Mutazz Mutaa Mutee Muwafaq Nabhan Nabeeh Nabeel Naer Nadeem Naadir Naeem 
Najeeb Naajy Najm Naa'il Naasih Naseem Nasser Nawfal Nazeeh Naathim Nooh 
Nu'man Noori Noor Omar Omran Omeir Ossama Qasim Qatadah Qays Qudamah 
Qutaybah Rabah Rabee' Rafee' Ragheb Ra'ed Rajab Raakin Raamiz Rashad 
Rashid Raatib Ridha Ridhwan Riyadh Sabeeh Saabir Safiy Safwan Saahir Saad 
Saeed Saajid Salah Saalih Saleem Salman Samer Samir Saariyah Sayf Sayid 
Shaady Shafeeq Shareef Shihab Siraaj Subhy Suhayb Suhail Sulayman Suoud 
Taahir Talal Talha Tamam Taamir Tarfah Tareef Tariq Tawfeeq Tayseer 
Taymullah Thaabit Thaqib Ubaadah Ubaidah Ubay Umar Khalifah Umaarah Umair 
Uqbah Usaamah Utbah Wadee' Waahid Wafeeq Waa'il Wajeeh Waleed Waliyullah 
Waliyudeen Waseem Yahyaa Yaman Yasaar Yaaseen Yasir Yazeed Yoonus Yoosuf 
Zaafir Zaahid Zakariya Zakiy Zayd Ziyad Zuhayr Zaahir 
 * 
Arabic (surnames) -- Abaza Abou Hamed Abu Shakra Akil Akwal Al-Amri Al-Dalharni Al-Daye 
Al-Deayea Al-Dossadi Al-Dossari Al-Dwairan Al-Farran Al-Habash Al-Halou 
Al-Hardi Al-Jaber Al-Jaber Al-Jahni Al-Khlaiwi Al-Mawalhad Al-Mehalel 
Al-Mowaled Al-Mubi Al-Mukhtar Al-Mutadee Al-Muwalid Al-Nubi Al-Owairan 
Al-Sabah Al-Sahaf Al-Shahrani Al-Shammari Alam Al-Asmari Al-Dosari 
Al-Fayyoumi Algosaubi Al-Jaber Al-Jahani Al-Jurr Al-Karachi Alkhaiwani 
Al-Masaari Al-Mehalel Al-Razi al-Sahhah Al-Sharani Al-Temiyat Al-Thynniyan 
Al-Zeid Alzeshi Amer Amin Andoni Anwar Asir Atef Ba'albaki Bahamdan 
Bakahasab Baraniq Billah Bin Haji Doka Elouahabu Faraj Farrakhan Hijaz 
Ismail Kanasani Madani Madari Maro Raboud Rostom Saleh Shia-Agil Shouaa 
Siham Solaimani Somayli Sulimani Surur Ta’anari Zebramani Zuabi Zubromawi 
Abdul-Hafeez Kardar Abdul-Qadir Abid Ali Afaq Hussain Aftab Gul 
Alim-ud-Din Amir Elahi Arif Asad Khan Asif Ahmed Asif Iqbal Asif Masood 
Atta-ur-Rahman Azmat Rana Dilawar Hussain Fakir Syed Aizazuddin Fasihuddin 
Fazal-Mahmood Fida Hussain Ghazali-Ghulam Abbas-Ghulam Mahomed Gul-Mahomed 
Hafeez Hanif Mohammad Haseeb Ahsan Ijaz Ahmed Ijaz Butt Ikram Elahi Imran 
Khan Imtiaz Ahmed Inshan Ali Intikhab Ali Inzamam-al-Haq Jahangir Khan 
Javed Akhtar Javed Miandad K Salam-ud-din Karim Ibadulla Khalid Hassan 
Khalid Wazir Khan Mohammad Mahmood Hussain Mahomed Nissar Majid Jahangir 
Khan Maqsood Ahmed-Mohammad Aslam-Mohammad Farooq Mohsin Kamal Mudassar 
Nazar Mulla Mushtaq Ali Mushtaq-bin-Mohammad Nasim-ul-Ghani Naushad-Ali 
Niaz Ahmed Pervej Sajjad Raza Hussain Nasir Nassir Ali-Sadiq Saeed-Ahmed 
Salahuddin Salim Altaf Salim Malik Sarfraz Nawaz Shafquat Husain Shafquat 
Rana Al-Shahid-Mahmood Shakoor Ahmed-Shoiab Shuja-ud-Din Waqar Ahmed Waqar 
Hassan Waqar Younis Wasim Akram Wasim Bari Wazir Ahmed-Zaheer Abbas 
ZulfikarAbedzadeh Abelzada Afnan Ahrari Akhlaqi Ala Alam Al-bin-Doleh 
Ameri Amini Amir Amirsadeghi Ammini Amouzegar Amuzgar Ansari Al-Ansar 
Anvari Ardabili Arfa Armanjani Asgapur Ashrafy Avarigan Awji Azizi 
Al-bin-Bagheri Bahonar-bin-Laden Bakhtavar Bakhtiar Bani-Sadr Baraheri 
Bazargan Al-Bihmardi Daei Dalvand Dastghayb Davisadr Dhabihiyan Diba 
Dihmubidi Dihqani Dulabi Ebtehaj Eftekhari Estili Estilidaei Ettehadieh 
Fahandizh Fatemi Forrughi Gaffari Garoussi Ghatary Al-rhul-Ghias Ghurani 
Habibi Hami Haqiqat Haqiqatju Hejazi Hisami Homayoun Hoveyda Hushmand 
Imami Iravan-bin-Mahood Ishraqi Izadi Jafari Jahanpur Jalili Javadi 
Jayhoon-Ahmed Karbaschi Kazemi Kazimi Keshuapad Khadim Khakpoor-bin-Mahood 
Khakpour Khamenei Khandil Khani Khanlary Khatami Khomeini Khushkhu Khuzayn 
Kordiyeh Madari Mahallati-Al-Bin Mahdavi-Kia Mahmudi Mahmudnizhad Majidi 
Mansur Maradi Mazlum Mehani Mehr Meshkat Mihdizadih Minavand Misbahi Moham 
Mossadegh Moullai Mualimi Muhammadi Mu'ini Mumtaz Mumtazi Muqimi Musavi 
Mutlaq Na'imi Najafi Nakisa Nasiri Nassiri Nazari Nemazi Nezam Nirumand 
Norouzi Nouri Pahlavi Pakravan Pashazadeh Qashqai Qazai Qoli Rabani Radji 
Rafigdoost Rahimi Rawhani Razmara Reyahni Roohizadegan Sabbah Sabet Sabiri 
Sadiqi Salimpour Sanjabi Shafaq Shahriar Shahy Shariat-bin-Laden Shariati 
Sharudi Shirazi Siyavushi Sotoudeh Sumech Tabatabai-al-Rhul Shah Talaqani 
Al-Talavi Talebi Al-Taleqani Vafa'i Vahdat Yaldai Al-Yaldai Yaqtin 
Al-ul-Yazdi Zahed Zahedi Za'irpur Zanjani Zarincheh Ziya-al-Ghulam 
 * 
Assyrian (names and places) -- Assyria Nineveh Ashur Harran Carchemixh Kadesh Sidon Babylon 
Tigris Zab Hattush Hatti Urartu Elam Calah Dur-Sharrukin 
Jezirah Jebel Hamrin Arbela Sin Shamash Anu Adad Ishtar 
Shamshi Adad Hammurapi Mitanni Ashur-uballit Shalmaneser 
Hanagalbat Tukulti Ninurta Kar Tiglath-pileser Mushku Jebel 
Bishri Marduk Nimrud Ashur-dan Adad-nerari Ashurnasirpal Habur 
Balih Zagros Zamua Shalmaneser Urartu Shamshi-Adad Teushpa Sargon 
Merodach-baladan Dur-Sharrukin Sennacherib Elam Esarhaddon 
Ashurbanipal Shamash-shuma-ukin Turtanu Kisru Rab-kisri 
Ehursaggalkurra Arbela Ninurta Adad Harran Esarhaddon Nabu Akitu
 * 
Basque (words) -- istripu ezagun helbide egoki game arratsalde adin bizirik harritu 
harrituta azter haserre petral erantzun inurri ageri beso iritsi heldu 
igo lotsaturik galdetu for itxura elkarte risk giro itsatsi saiatu 
izeba haur txar gaizki poltsa puxika platano saski saguzahar bainutu 
komun sink bainuontzi hondartz babarrun bizar eder jarri ohe logela 
erle eskatu portatu sinestu uste gerriko txapel bizikleta handi txori 
kosk arbel basurde txalupa soin gorputz hezur liburu bota aspertu 
aspergarri aspertuta jaio sortu senargai adar ogi apurtu down gosaldu 
gosari zubi kartera ekarri together puskatuta down anaia zezen erre 
autobus sasi enpresari pinpilipauxa erosi eguntegi deitu dei lasai 
lasai ahal ezin kotxe eraman gurdi gaztelu katu harrapatu leiza mende 
kate aulki txapeldun txapelket kobratu merke gazta gerezi oilasko 
tximini kokotza eliza hiri dweller garbi garbitu zabaldu igo erreloju 
mina itxi friend itxi itxita zapi jantzi hodei traketsa ikatz txanpon 
hotza etorri jokatu kejatu osatu aitortu despistatu biltu pozik 
jarraitu galleta kortxo sakakortxo tortilla zuzen kotoi eztul herri 
lehengusina lehengusu behi karramarro kilkir kokodrilo zeharkatu 
gurutze negar negarrez armairu kiskur ohitura txikitu madarikatu dantza 
dantzaldi alaba egun after before zor jaitsi erabaki hondo saila 
basamortu gogo euskalki hil zail afaldu room afari norabide zikin 
zikindu egin sendagile txakur asto ate marraztu kajoi amets 
jantzi ariket edan edari kotxe bota bateri mozkorti legortu ipotz other 
belarri goiz belarritako lurrikara ekialde errez jan arrautza dotore 
elefante lotsaturik hutsik hustu aurkitu etsai sartu sarrera sobre 
berdin saio asterketa adibede bikain aitzeki ariket garesti azaldu 
aurpegi erori udazken asleep name urruti baserri baserritar liluratuta 
azkar lodi gizen aita up sentitu jai borrokatu burruka bete aurkitu 
isuna hatzamar amaitu bukatu su tximeneta arrantzu arrain gar ihes 
ligatu lur hegaz bitxa laino lanbro janari engainatu oin baso ahaztu 
ituri askatu of askatuta lagun ikaratu igel prejitu beteta month hileta 
barregarri haltzari mizkin partidu jolas baratzuri xamurra keinu up up 
mamu opari jirafa andragai eman notice betaurreko eskularru joan out 
out to ahuntz Jainko jainkosa urre on aitxitxa amuma mahats zelai 
matxin-salto talde hazi gorde asmatu thoughts thoughts arma tipo ijitu 
ohitura ile erdipurdiko erdipurdi urdaiazpiko esku zintzilik gertaera 
pozik alai zoriontsu gogorra working erbi gorroto idea buru sendu 
bihotz astun lagundu herentzi ezkutatu jo eutsi zulo opar ezti espero 
esperantza zaldi beroa ostatu ordu etxekoandre besarkada giza hezea 
gose analfabeto irudimen garrantzitsu ezin egonezina biztanle asmo 
interesatu interesgarri asmatu ikertu gonbidatu plantxatu burdina ale 
bidai jauzi salto oihan kanguru gorde kupel ostiko hil musu sukalde 
sink labana ezagutu ezkailera andre asken night year azkenik berandu 
barre par etzanda alfer hosto ikasi utzi porru eskuin ezker hank limoi 
eskarmentu eskutitz gezur jaso arin argi tximist arin marra lehoi 
entzun by room luze begiratu of jaun galdu galduta maite sorte bazkaldu 
bazkari txitxarro egin azoka ezkondu meza partidu larre okela haragi 
elkartu aipatu mezu errot miloi mehatz bihurri xuhur sator diru ila 
hilargi goiz ama motor sagu bibote aho mugitu buztina izen estu 
abertzale gaizto gertu hurbil beharrezko lepo behar orratz iloba urduri 
sare berri egunkari iloba gau otsa zarata eguerdi ipar sudur ohartu 
konturatu larrubizi erizain tree olagarro eskeini bulego olio zahar 
ireki ireki irekita ustez eritziz agindu arrunta zordu idi zorro min 
margotu txekmena paper loro alde pasatu oraindu pagatu bake luma arkatz 
jende of pezta argazki up txerri uso toki of of asmo hegazkin landare 
jolastu atsegin gidoi patrika kutsatu pobre quality kai zati ahalegina 
okerkeri nahiago prestatu polit apaiz aztarna babestu esaera tira 
zigortu bultzatu bultza ipini down in galdera arin untsei pala irrati 
euri aker gordin level irakurri arrazoi jaso ezagutu hotzgailu baztertu 
geratu gogoratu kendu konpondu irudi urtegi errespetu ardura itzuli 
aberats ezker eskuin txikitu ibai gela soka ustel borobil goma esames 
korrika triste gatz esan esaera bildurtu for ikusi saldu bidali 
urrundu josi itzal dardarka ardi apala artzain itsasontzi oinetako trip 
baju labur motz oihukatu oihu erakutsi dutxatu dutxa lotsati lotsati 
gaiso bazter isil isilik lelo sinplea abestu kantatu tantta of of down 
down egoera trebe larru zeru lo lo logura lirain xerra motel geldi 
poliki txiki usaindu irripar ke leun mugalari karakol suge elur xaboi 
biguna gudari sendo zerbait seme salda hego leku hitzegin mintzatu 
berezi armi-arma kirol tantta udaberri xirimiri karratu eszenario 
ezkailera seilu zutik izar hasi geratu lapurtu mother tripa harri 
gelditu geltoki zikoin lisu bitxi ezezagun marrubi erreka 
indar indartsu ikasi tonto azukre maleta uda home eguzki ainhara izerdi 
goxoki igeri ikur era mahai hartu away to berritxu altu dastatu probatu 
irakatsi burla hagin kontatu jenio kurtso lurralde eskertu lapur mehe 
argal gauza pentsatu uste egarriz oroi hari bota trumoi txartel lotu 
lotua tigre aldi denbora nekagarri nekatuta gaur bihar tresna ikutu 
herri aztarna itzulitzaile transmititu gonbidatu tratatu zuhaitz 
trintxera bidai kamioi egi into off on dordoka txotx biki itsusi aterki 
euritako osaba ulertu ale unibertsitate erabili abiadura best 
ahots itxaron itxoin up ibili buelta horm nahi epel ohar garbigailu 
zaindu ur ahul eguraldi astegune asteburu pesatu ondo behaved 
mendebalde garia gurpil txistu zabal zabaldu alardun emazte irabazi 
haize leiho negu sorgin otso egur artile har larri gurtu idatzi oker 
urte atzo gazte 
 * 
Basque (female names) -- Aes Abarne Abauntza Abelie Adonie Aduna Agate Aginaga Agirre Agurne 
Agurtzane Aiago Aiala Ainara Ainhoa Ainize Aintza Aintzane Aintzile 
Aintzine Ainuesa Aiora Aitziber Aizeti Aizkorri Aizpea Akorda Alaine 
Alaitz Alazne Albie Albizua Alduara Alduenza Alize Alkain Almika Almuza 
Aloa Alodia Altzagarate Ama Amaduena Amagoia Amaia Amalur Amane Amatza 
Amelina Amets Ametza Amilamia Amuna Anaeaxi Anaiansi Anaurra Ande 
Anderazu Andere Anderexo Anderkina Andia Andikona Andion Andolie Andone 
Andre Andregoto Andrekina Andremisa Andrezuria Ane Angelu Aniz Anoz 
Ansa Antxone Antziasko Apain Apala Araba Aragundia Araitz Arama Arana 
Arandon Arantza Arantzazu Araoz Arbeiza Arbekoa Arburua Areitio Areria 
Argi Argie Argiloain Arie Arima Ariturri Aritzaga Aritzeta Arkija Arlas 
Arluzea Armedaa Armola Arnotegi Arraitz Arrako Arrate Arrazubi Arreo 
Arriaka Arrieta Arrigorria Arriluzea Arritokieta Arrixaka Arrizabalaga 
Arrosa Artea Artederreta Artiga Artiza Artizar Artzanegi Artzeina Asa 
Asiturri Askoa Atallo Atotz Atsegie Atxarte Aurela Auria Auriola 
Aurkene Austie Axpe Ayala Azella Azitain Baano Babesne Baiakua Bakarne 
Bakene Balere Barazorda Barria Barrika Basaba Basagaitz Basalgo 
Basandre Beatasis Bedaio Begoa Belanda Belaskita Belate Beloke Beltzane 
Bengoa Bengoara Bengolarrea Beraza Berberana Berezi Berzijana Betiko 
Betisa Bibie Bidane Bihotz Bikarregi Bingene Biolarra Bioti Bittore 
Bittori Bitxi Bitxilore Bixenta Bizkaia Bizkargi Buiondo Burgondo 
Burtzea Deio Distira Dolore Doltza Domeka Dominixe Donetzine Doniantsu 
Donianzu Dorleta Dota Dulanto Dunixe Ederne Ederra Edurne Edurtzeta 
Egiarte Egilior Eguene Eguzkie Eider Ekhie Elaia Elduara Elisabete 
Elixabete Elixane Elizamendi Elizmendi Elkano Elorriaga Elurreta Eluska 
Enara Endera Enea Eneka Eneritz Erdaie Erdie Erdoitza Erdotza Erdoza 
Erguia Eriete Erika Erisenda Erkuden Erlea Ermie Ermin Ermisenda Ermua 
Ernio Erniobe Erramune Erramusa Errasti Erregina Erremulluri Erromane 
Errosali Erroz Errukine Erta Eskarne Eskolunbe Esozi Espoz Estebeni 
Estibalitz Estibaliz Etorne Etxano Etxauarren Eulari Eunate Euria Eusa 
Ezkurra Ezozia Eztegune Fede Fermina Florentxi Frantsesa Frantxa 
Frantziska Fruitutsu Gabone Gadea Gainko Garaie Garaitz Garazi Garbi 
Garbie Garbikunde Garden Gardotza Garoa Garralda Garrastazu Gartzene 
Gatzarieta Gaxi Gaxuxa Gazelu Gazeta Gaztain Geaxi Gentzane Geraxane 
Geroa Gipuzkoa Goiatz Goikiria Goikoana Goiuria Goizane Goizargi Gorane 
Goratze Gorostitza Gorria Gorritiz Gorriza Goto Gotzone Gozo Graxi Gure 
Gurutze Gurutzeta Guruzne Haize Harbona Haurramari Hegazti Helis Hiart 
Hilargi Hoki Hua Hugone Ibabe Ibane Ibernalo Ibone Idoia Idoibaltzaga 
Idurre Iera Igaratza Igone Igotz Ihintza Ikerne Ikomar Ikuska Ilazkie 
Ilia Iligardia Iloz Ines Ioar Ipuza Iragarte Iraia Irakusne Irantzu 
Irati Iratxe Iriuela Iriberri Iride Iristain Irua Irune Iruri Irutxeta 
Isasi Ismene Isurieta Itoiz Itsasne Itsaso Iturrieta Iturrisantu Itxaro 
Itziar Iurre Ixone Izar Izaro Izazkun Izorne Jaione Jasone Jauregi 
Joana Jokie Jone Josebe Josune Joxepa Jugatx Julene June Jurdana Jurre 
Kalare Karitate Karmele Katalin Katerin Katixa Kattalin Kiles Kistie 
Kizkitza Kodes Koldobike Kontxesi Kontzeziona Koru Krabelin Kupie 
Laguntzane Laida Lamia Lamiaran Lamindao Landa Landerra Larraintzar 
Larraitz Larrara Larrauri Larraza Lasagain Lasarte Latsari Latxe 
Legarda Legarra Legendika Legundia Leioar Leire Lekaretxe Leorin Lerate 
Lerden Letasu Lexuri Lezaeta Lezana Lezeta Lide Lierni Lili Lilura 
Lirain Lohitzune Loinaz Lopene Lore Loza Luixa Lukene Lur Maarrieta 
Mahats Maia Maialen Maider Maitagarri Maitane Maite Maiteder Maitena 
Makatza Maldea Malen Mantzia Mari Mariaka Marider Maritxu Martie 
Martixa Martxelie Matxalen Meaka Mendia Mendiete Mendigaa Menga Menosa 
Mentzia Mikele Milia Mina Miniain Mirari Miren Mitxoleta Molora Monlora 
Munia Muno Munondoa Muntsaratz Murgindueta Muruzabal Muskilda Muskoa 
Muxika Nabarne Nabarra Nafarroa Nagore Nahikari Naiara Naroa Nazubal 
Negu Nekane Nerea Nikole Nora Nunile Oianko Oiartza Oibar Oihana Oihane 
Oilandoi Oinaze Oitia Oka Okon Olaia Olaiz Olalla Olar Olaria Olartia 
Olatz Olite Ollano Olleta Oloriz Onditz Ondiz Oneka Onintza Opakua 
Orbaiz Ordizia Orella Oria Oriz Oro Oroitze Ororbia Orose Orrao Orreaga 
Orzuri Osabide Osakun Osane Osasune Osina Oskia Osteriz Otsana Otsanda 
Otzaurte Panpoxa Pantxike Parezi Paskalin Paternain Pauli Pelela 
Pertxenta Pilare Pizkunde Poyo Pozne Printza Pueyo Puy Sabie Sagarduia 
Sagari Sahats Saioa Sallurtegi Santllaurente Santutxo Semera Soiartze 
Sokorri Sorauren Sorkunde Sorne Soskao Soterraa Terese Tetxa Toda Toloo 
Txori Uba Ubaga Ubarriaran Udaberri Udala Udane Udara Udazken Udiarraga 
Udoz Uga Ugarte Ula Uli Untza Untzizu Uraburu Uralde Urbe Urdaiaga 
Urdie Urdina Uriarte Uribarri Uriz Urkia Uronea Urraka Urrategi Urrea 
Urreturre Urretxa Urrexola Urrialdo Urroz Ursola Urtune Urtza Urtzumu 
Usmene Usoa Usue Usune Utsune Uzuri Xabadin Xaxi Xemein Ximena Xixili 
Xoramen Zabal Zabaleta Zaballa Zaloa Zamartze Zandua Zarala Zeberiogana 
Zelai Zerran Zikuaga Zilia Ziortza Zita Zohartze Zorione Zuberoa Zubia 
Zufiaurre Zuhaitz Zumadoia Zumalburu Zurie Zuria Zuza Zuzene 
 * 
Basque (male names) -- Abarrotz Aberri Adame Adei Adon Adur Ageio Ager Agosti Agoztar Aide 
Aiert Aimar Aingeru Aintza Aioro Aire Aita Aitor Aitzol Aketza Alain 
Alaon Alar Alarabi Alatz Albi Alesander Allande Alots Altzibar Ambe 
Ametz Amuruza Anaia Anakoz Anartz Anaut Ander Andima Andoitz Andolin 
Andoni Aner Anixi Anko Anter Antso Antton Antxoka Apal Apat Arabante 
Aralar Arano Aratz Aresti Argi Argider Argina Argoitz Arixo Arnaitz 
Arnalt Arnas Arotza Arrats Arrosko Artizar Artzai Artzeiz Asentzio 
Asier Asteri Astigar Atarrabi Atarratze Atseden Atze Atzo Aurken Aurre 
Austin Auxkin Axular Azeari Azibar Aznar Aztore Azubeli Baiardo Baiarte 
Baiona Bakar Baladi Balasi Balendin Baleren Balesio Baraxil Bardol 
Barea Basajaun Batista Bazil Bazkoare Beat Behe Beila Bela Belasko 
Beltxe Beltza Benat Berart Berasko Berbiz Berdaitz Berdoi Beremundo 
Bernat Bero Berriotxoa Bertol Betadur Beti Bidari Bide Bihar Bikendi 
Bilbo Bilintx Bingen Birila Birjaio Bittor Bixintxo Bizi Bladi Bordat 
Bortzaioriz Burni Burutzagi Danel Dei Denis Deunoro Diagur Diegotxe 
Distiratsu Domeka Domiku Dominix Donostia Dunixi Eate Eder Edorta Edur 
Egoi Egoitz Eguen Eguerdi Egun Eguntsenti Eguzki Ekain Ekaitz Ekhi 
Ekialde Elazar Eleder Eli Ellande Elorri Emenon Enaut Endira Endura 
Eneko Enekoitz Eneto Enetz Erauso Ereinotz Eriz Erlantz Erramu Erramun 
Errando Errapel Errolan Erroman Error Erruki Eskuin Estebe Etor Etxahun 
Etxatxu Etxeberri Etxekopar Etxepare Eusko Ezkerra Eztebe Fermin Firmo 
Formerio Fortun Frantxizko Frantzes Frederik Froila Gabirel Gabon Gai 
Gaizka Gaizkine Gaizko Galindo Galoer Ganix Gar Garaile Garaona 
Garikoitz Garin Garoa Gartxot Gartzea Gartzen Gartzi Gaskon Gasteiz 
Gaston Gau Gauargi Gaueko Gaur Gaxan Gaztea Gentza Geraldo Gerazan 
Gergori Gero Gilen Gilesindo Giro Gizon Gogo Goi Goiz Goizeder Gomesano 
Gora Gorbea Goren Gorka Gorosti Gorri Gotzon Gurutz Gutxi Haitz Handi 
Hanni Hanot Haritz Haritzeder Harkaitz Harri Harriet Hartz Hats Hegoi 
Herauso Herensuge Hibai Hitz Hitzeder Hodei Hori Hotz Hurko Hustaz Iaki 
Iigo Iban Ibar Ibon Ieltxu Igon Igor Ihazintu Ihintza Ikatz Iker Ilazki 
Ilixo Illart Imanol Inautzi Indar Indartsu Inge Inguma Inko Intxixu 
Ioritz Ipar Iparragirre Iraitz Iratxo Iratze Iratzeder Iraunkor 
Irrintzi Iruinea Isidor Isusko Iturri Itzaina Ixaka Ixidor Ixona Izotz 
Iztal Jaizki Jakes Jakobe Jakue Janpier Jatsu Jaunti Jaunzuria Joanes 
Jokin Jon Joseba Josepe Josu JuandasalbatoreAscension Juaneizu Juango 
Juantxiki Julen Jurdan Jurgi Kaiet Karmel Kauldi Kaxen Kelemen Kemen 
Kepa Kiliz Kimetz Kismi Koldo Koldobika Kusko Lain Lander Lapurdi Larra 
Lartaun Lastur Lauaxeta Laurendi Laurentzi Laurgain Laurin Lehen 
Leheren Lehior Lehoi Leizarraga Lekubegi Leoiar Ler Lertxun Liher Lizar 
Lizardi Lohitzun Loiola Lon Lope Loramendi Lordi Lore Loren Lorenz Luar 
Luix Luken Luzaide Luzea Maiorga Maju Manex Mantzio Manu Maore Marin 
Markel Marko Martxel Martxelin Martxot Marz Matei Matia Mattin Matxin 
Maule Maurin Mazuste Meder Mederi Mendebal Mendiko Mikel Mikelar 
Mikelats Mikeldi Mikolas Milian Min Mirande Mitxaut Mitxel Mogel Montxo 
Munio Musko Nabar Nahia Nikola Nuno Nuxila Obeko Odol Odon Oidor Oier 
Oihan Oihenarte Oinatz Oinaz Olentzaro Onbera Ongile Opilano Orain 
Orixe Orkatz Oroitz Orti Ortle Ortzi Orzaize Osasun Oskarbi Oskitz 
Osoitz Ospetsu Ospin Ostadar Ostargi Ostots Otsando Otsoa Otsoko Oxarra 
Oxel Pagomari Panpili Paskal Patxi Paul Paulin Paulo Peli Pello 
Perrando Peru Peruanton Perutxo Pes Petri Petrigai Piarres Pierres 
Polentzi Poz Presebal Pudes Raitin Remir Ruisko Sabin Salbatore 
Sandaili Sanduru Santi Santikurtz Santio Santutxo Santxo Santxol Sasoin 
Satordi Seber Selatse Seme Semeno Sendoa Sengrat Sesuldo Silban Soin 
Soter Sotil Su Sugar Sugoi Sustrai Tartalo Tello Teobaldo Tibalt Tipi 
Todor Totakoxe Tristan Tuste Txanton Txaran Txartiko Txatxu Txerran 
Txeru Txilar Tximitx Txindoki Txomin Txordon Txurio Ubarna Ubelteso 
Ubendu Udalaitz Ugaitz Ugutz Uhin Umea Unai Unax Ur Urbasa Urbez 
Urdaspal Urdin Urki Urko Urre Urritz Urtats Urti Urtsin Urtsua Urtun 
Urtungo Urtzi Uzuri Xabat Xabier Xalbador Xantalen Xanti Xarles Xefe 
Ximun Xofre Xuban Xurdin Zabal Zadornin Zeledon Zernin Zeru Zeruko 
Zigor Zilar Zohiartze Zoil Zorion Zuhaitz Zumar Zunbeltz Zuri Zuriko 
Zuzen 
 * 
Bulgarian (words) -- zdravejte az ot skoro imam vazmojnost da sledja diskusiite grupata no 
zatova pak dosta vnimatelno mislja che moga da napravja njakoj izvodi 
osnovnata tema obsajdaneto na dva glavni aspekta istoricheski facti 
svarzani imeto segashnoto polojenie na republika moje da se definirat 
dve osnovni pozicii sporovete po temata poddarjashti tezata che bula 
vinagi dnes neshto zavarsheno nezavisimo otdelno ot vsichko 
obkrajavashto ja vsichki ostanali koito ne sa na tova mnenie grupata ot 
se sastoj predimno ot edna lichnost dokato grupata ot b dosta 
pomnogobrojna ponjakoga ne se sastoj samo ot balgari horata ot 
obiknovenno predstavjat njakavi istoricheski ili savremenni fakti 
avtentichni podkrepa na tjahnata teza grupata ot predimno se izkazva 
nepodgotvena pochti nikoga ne diskutira njakoj ot privedenite fakti 
dokumenti snimki ne tvardja che nabljudenijata mi sa pravilni no pone 
taka smjatam az izvod balgariaj balgarite sa oceleli poveche veka mnogo 
trudnosti preodoljal nashija narod za sajalenie nemalka chast ot tjah 
sa si po nasha vina za da prebade toja narod ne trjabva da se zabtravja 
istoriata mu ima mnogo elementi koito iskat tova da ne tochno taka za 
sajalenie ot do g mnogo ot nashata istorija beshe preinacheno mnogo 
dokumenti pogubeni mnogo neprostimi greshki bjaha napraveni osobeno 
otnosno nasheto naselenie blagodarja na gn boris dosevski blagodarja mu 
zashtoto vsichki nie trjabva da se obedinim da namerim dori najmalkoto 
ostanalo kato istorichesko svedenie za da ne umre istinata zavinagi 
umoljavam vi da ne se vprjagate mnogo na dumite na gornja gospodin samo 
prodaljavajte da namirate dokumenti svedenija gi objavjavajte po edin 
ili drug nachin smjatam che shte dojde den kogato balgarshtinata shte 
vidi pak dobri dni vsichko tova shte bade polezno njama znachenie che 
naprimer ne dotam inteligentni lichnosti sa se sabrali neshto kato omo 
ilinden ili pak njkoj se opitva da krade ot istorijata ni kato 
bezpardonno ja preinachava taja njama da mine no zatova trjabva vsichki 
da pomognem ne samo da obvinjavame politicite ni che ne varshat nishto 
po vaprosa opravdanieto najlesnata pozicija rossen dimitrov ne posnavam 
chovek ot toja kraj kojto njakoga da se identifiziral ili neshto da 
nema bugarska policija na sekoj chekor kje se izjasnat site abe bore 
shto ia mesish politsiata u tova neshto ako nekoi mozhe da duma za 
politsiata toa si ti pa nema li u rom po goleme chislo poiltsia ot 
kolko u blgaria rom ia ima ia nema miliona ima poveke politsiai na 
zaplata stho gi radite tia politsai be bore sa vsichki koito se 
narichat nie pqk se narichame blgaria ne bugari blgarite kako ti si se 
blgari koi sa tia be bore grtsite shto sa razhdani u egeiska hora kato 
mene shto sa razhdani u pirinska srbi shto sa razhdani u vardarska 
albantsi u vardarska bulgrai shto sa razhdani u site tri chasti ili 
promitite mozatsi kato tebe peto pirin bqlgarski shte si ostane takqv 
samo od so voena sila no ne za mnogu podolgo vreme kolko tova vreme be 
borka stho ti ke chinish kato umresh ot starost pa go ne dochekash toa 
vreme be bore ps ako si meraklija ela pres simata na ski bansko tam 
shte si doisjasnim predstavite ubeshdenijata kje se vidime jas ti eden 
den pirinsko ne beri gajlesamo ne veruvam deka kje ti bide mnogu do 
skijanje togash pa nema da mue on ke se chudi choveko kak da gi narani 
site tia bezhanitsi ot rom shto begaat pred srbskite shtikovi ias ne 
mislam deka ti ke stoish u vardarsko da si aprodavash kozhata za 
svoboda ako doide do toa georgi karadjov ej docevski ti osven naglo 
kopele si idiot maj tochno zaradi takiva kato teb scb pqlna prostotii 
vdigaj si shapkata se omitaj ot tuk ako ne mojesh da kajesh nishto po 
sqshtestvo ne se opitvaj pqk ako iskash ako nivoto ti na inteligencija 
pozvoljava vzemi napishi edna statija zashto spored teb pirin kakva 
tazi nacija stiga si vjal bajraka dnes veche nikoj ne se interesuva ot 
lozungi stefan preneseni izvadoci od napisot na jan pirinski vo vesnik 
narodna volja od gorna dzhumaja pirinska nova na chetvrtata starnica 
vo brojot od septemvri godinava vo velikobugarskiot 
nacionalshovinistichki vesnik organ na vmrosmd otpechatena statija so 
naslov nova odnovo lazhe od voimir asenov ova besmisleno pisanie na 
asenov odgovor na statijata apsurd no kriminal od jan pirinski koja 
bila publikuvana vo vesnikot nova no treba da znaesh deka jas ne sum 
nekojsi jan pirinski tuku chovekot shto ja kritikuvashe tvojata statija 
shto pravime nie bugarite za da ja zashtitime vtorata bugarska 
drzhavase nervirash za mojot psevdonim ne sum prviot nema da bidam nitu 
posledniot koj koristi psevdonim no toa mozhebi i poradi nashata verba 
vo golemata bugarska demokratija kako jas nikomu nema da mu dozvolam da 
me zakituva so druga tugja nacionalnost toa moe prirodno pravo obvrska 
koi kj gi chuvam po cena na sopstveniot zhivot kako taka ti chovechence 
koj republika ja narekuvash vtora bugarska drzhava da ochekuvash od 
nas da si molchime da se soglasime so tebe ako ne se soglasime te 
kritikuvame znachi prostaci sme gospodinot prashuvaneli bilo vistina 
deka vmro vekje nad eden vek srceto na narodniot krvotok da gospodine 
no ne bugarsko tuku bidejkji vasheto smd ne vmro zatoa shto vo minatoto 
ovie vrhovisti koi se vovlekle vo vnatreshnata organizacija se lugje na 
bugarskiot dvorec ne na organizacijata na narod toa se dva korenito 
razlichni poima taka shto vie ste tie koi gi tolkuvaat neshtat kako 
shto gjavolot go chita evangelieto toj anrod postoi za inaet na 
negovite dushmani toj ima slavno bogato minato toj dade kultura 
pismenost na mnogu narodi megju koi na bugarskiot namesto blagodarnost 
pochit ja dobiva vashata divjachka hunska omraza 
 * 
Celtic (female names) -- Aclitenis Aibfinna Aifric Affrica Ailbe Ailbhe Ailidh Áine Aime Aisling 
Aislinn Ashling Alannah Álmaith Almha Alva Almu Alma Anastas Anga Annábla 
Aodhamair Aodhnait Aednat Aedin Aideen Aoibh Aiobheann Aoibhinn Aoibhi 
Aoife Aoiffe Aeife Arlene Athracht Attracta Aurnia Barran Beatha Becuma 
Bhionn Béibhinn Bébhinn Bebin Befinn Bevin Bevina Bega Beirnis Bél Belocc 
Beonill Berrach Bearach Berriona Bidina Bil Bláth Bláithnait Bláthnaid 
Blathnait Bláthnat Bláithin Blanaid Blanid Bluinse Bodhbh Brenda Breanda 
Brighid Bridget Brigid Brigit Brede Breeda Brid Brighda Bridin Bedelia 
Bidelia Biddie Cacht Caoilfhionn Caoilainn Caelfind Coelfinnia Keelin 
Caoimhe Caral Cathan Cah Ceara Cera Cara Carra Cearúilin Ciannait Ciannata 
Cian Ciara Ciar Cyra Keara Kiera Ciarda Ciit Cingit Cliodhna Cliodna 
Clidna Cliona Cliona Cleena Clodagh Clothra Cochrann Cóemfind Coimell 
Colan Colleen Conandil Conchenn Coinchind Conchobarre Connora Conchobar 
Congan Creda Crida Cróeb Cron Cuach Cumman Daireen Darina Damhnait Damhnat 
Davnat Devnat Dymphna Dympna Dana Danu Danann Dar Cárthaind Dareca 
Dearbháil Derbáil Derbh Deirbhile Derval Dervila Dervla Devla Devorguilla 
Derba Forgaill Decla Declan Delbchaem Dercco Derdraigen Derdriu Deirdre 
Dierdre Devnet Doireann Dorean Dóirin Doreen Dorren Dorinnia Doirind 
Doirend Dorene Dorine Dirinn Derinn Donelle Donnfhlaidh Donnflaith 
Dunflaith Dunlaith Dunla Donla Downett Drón Éabhna Ealga Echna Edana 
Éibhliu Ébliu Éblenn Éibhleann Evle Evlin Eihrig Éile Eilgri Eimear 
Eimhear Eimer Éimhin Éimine Evin Eirnin Ernina Ernin Eithne Ethna Uaithne 
Ethne Ethenia Ethnea Etna Edna Ena Elan Elige Elva Enda bird Érennach Eri 
Erin Éirinn Eriu Ireland Étáin Éadaoin Etain Aidin Ethlinn Étromma Failend 
Faimdid Fainche Fanchea Fand Fann Faoiltiarna Whiltierna Feidhelm Fidelma 
Feidelma Fedelma Fedelim Feidlim Fedelm Delma Fenit Find Findchóem 
Findétand Findscuap Finnsech Finnseach Finsha Fiona Fina Finna Fionna 
Finnabhair Fionnabair Fionó Fennore Fionnghuala Fionnuala Finnguala 
Fenella Finella Finvola Finola Nuala Nola Flann Fodla Fodhla Fola Fuamnach 
Garb Glóir Gobnait Gobinet Gobnat Gobnata Gobnet Gubnet Gorm Gorman 
Gormflaith Gormlaith Gormla Gormley Gráinne Grania Granna Granya Grian 
Guinnear Hisolda Hya Ia Ibelide Ida Ita Ite Indécht Indiu Inis Isleen 
Keeley Keelin Keenat Kinnat Labhaoise Lára Lasairfha Lasairiona Lasairian 
Laserian Laisrian Lasairiona Lassarina Lasrina Lassar Lassi Lebarcham 
Leborcham Lethann Liadan Liadain Liban Life Liffey Lile Luigsech 
Luighseach Luigseach Luiseach Laoise Laoiseach Macha Máda Magael Mallaidh 
Méabh Meadhbh Maedbh Maedhbh Maeve Meave Medb Mave Meidhbhi Meld Mide 
Meeda Mincloth Mise Móen Moncha Monenna Moninna Moninne Blinne Bline Ninne 
Mongfhionn Mongfind Mór Moreen Morrin Móreen Móirin Muadhnait Muadnat 
Muadhnata Monat Mona Muadhnatan Mugain Múireann Muirinn Murinnia Muirenn 
Murainn Mairenn Miren Maren Muirecht Muirgen Muiri n Muirenn Miren 
Muirgheal Muirgel Muiriol Muriel Muiriath Muirne Myrna Merna Morna Moina 
Moyna Naomh Narbflaith Narvla Neasa Neassa Nessa Ness Némdaille Niamh 
Neamh Niam Ném Neave Nóinin Noleen Nolene Norlene Óchae Odharnait Odarnat 
Odhamnait Ornat Orna Ownah Óebfinn Ohnicio Oilbhe Órfhlaith Orflath 
Órlaith Orlagh Órla Orla Osmanna Paili Piala Rathnait Ranait Rónnait Ronit 
Réaltán Rigan Rioghnach Roighnach Righnach Rionach Rynagh Rinach Riomthach 
Riach Rifach Roach Sadhbh Sadbh Sabhbh Sadb Sive Sabia Sabina Saidhbhin 
Saoirse Saiorse Samhaoir Samthann Sárnat Scathach Scáthdercc Sciath Silbhe 
Sinech Sláine Slania Slanie Sodelb Sogáes Sorcha Sósaidh Stediana Taillte 
Tailltiu Tanith Tathan Teamhair Temair Tara Teath Téiti Téitl Tlachtga 
Treasa Teresa Tuilelaith Talulla Úna Ailia Ailne Ainge Airmed Aith Arminia 
Barita Beara Bechulle Betha Birog Blai Boadicea Bodicca Bonduca Boudica 
Voada Voadicia Bodhmall Brec Bri Brica Briga Brisen Brissen Brosna Caelur 
Candiedinia Cardixa Cartimandua Ceithlenn Cesair Kesair Corocca Corotica 
Cottia Crochnuit Croderg Cuillen Cunovinda Cutha Daalny Dalla Damona 
Delgnat Deoca Deoch Deorgreine Derbrenn Dianan Duineachta Eala Echgte 
Eibhir Enghi Eponina Eponnina Esa Ethernais Ethil Etterna Eurbrawst 
Failinis Fais Fianna Findabar Finndealbh Gargeolain Ghaoitha Giorsal 
Glanluadh Goneril Gruoch Guinhumara Gwyna Huctia Huna Iamicilla Igerna 
Inganiad Innogen Iorwen Kea Keelta Lakutu Lavercham Leine Lendar Lewinna 
Lia Liath Liban Locha Maer Maerica Manissa Matugena Messbuachalla 
Miluchradh Mocuxsoma Modwenna Moluag Moriath Nessan Nesta Nynia Ogarmach 
Onuava Rhyanidd Rhybrawst Rieingulid Rora Saba Sabra Sassticca Sativola 
Scathniamh Sinna Snechta Stroma Sulicena Tancorix Teleri Telta Thaneu 
Thola Tiabhal Truforna Tuag Tuiren Uaine Uchtdealb Ulidia Unna Urith Vacia 
Veldicca Verecunda Vicana Vilbia Wenna 
 * 
Celtic (male names) -- Abban Adomn Adhamh Adhamhnán Adamnan Eunan Adanodan Ailbhe Ailbe Alby 
Ailgel Ailill Ailín Aininn Ainmire Airechtach Airmedach Alabhaois Alaios 
Alastrann Alchad Alstrom Amalgaid Amergin Anluan Anlon Aodh Aodha Aoidh 
Aedus Aodhaigh Aodhán Aodán Áedán Aiden Aidan Edan Aodhagán Aogán Egan 
Aodhfin Aonghas Engus Aonas Angus Ardar Árdghal Ardal Argal Artegal 
Arthgallo Art Artan bear Artúr Artuir Arthur Baeth Baetan Balor Bairne 
Baithaus Banan Banbhan Baothghalach Barrfind Bairrfhionn Barrin Bairre 
Barre Barra Barry Beacán Beag Beairtle Beanón Bearach Berach Bearchán 
Beartlaí Becan Behellagh Behillagh Benen Beolagh Beothach Bercan Berchan 
Bercnan Bergin Blanaid Boethis Bran Brandubh Brandrub Branduff Breadán 
Bréanainn Breandán Brendan Bresal Bressal Brasil Breas Breasal Brian Brion 
Bricc Britanmael Brón Bruaidheadh Bruatur Bruddai Buadhach Buagh Buaigre 
Cadhla Caeilte Caentigern Kentigern Cainchinne Cainnech Cainneach Canice 
Cairbre Coirbre Cairpre Caircil Cearcill Caireall Cairell Coireall Kerill 
Cairthinn Caiside Calbhach Calvagh Calbach Callough Caoilte Caoimhe 
Caiomhín Caoimhín Caiomheán Caomhgain Cáemgen Coemgen Kevin Kevan Caolán 
Caolainn Kelan Caraid Carantoc Cárthach Cartach Cartagh Carthagh Cascorach 
Cassidan Cathal Cahal Cathald Catheld Cathaldus Kathel Cathan Cahan 
Cathaoir Cahir Cathfer Catharnach Cathrannach Cathasach Cathbadh Cathbad 
Cathbharr Ceallach Ceallagh Ceallachán Ceollach Cellach Kellach Kelly 
Ceall Cearbhall Cerbhall Cerball Cearul Cearull Carroll Cearbhallán Cearbh 
Cearnach Cedach Celsus Celtchair Cenn Cesarn Chattan Chulain Cian Kian 
Kean Keane Cianán Ciabh Ciárán Kieran Ciardan Ciardha Ciardubhán Ciarrai 
Cillian Cillín Cillne Killian Kealan Kilian Killan Cinneíde Cinneídigh 
Cinneíddin Cionadh Cionaodh Kenneth Cnán Coan Cobhran Cognat Colcu 
Comhghall Comhgall Comgal Comgell Congal Cowal Comhghán Comghán Comgán 
Comman Congan Conaing Conall Connell Conan Conant Conchobar Conchobhar 
Conchubhar Conchubor Conchúr Cnochúr Conquhare Conaire Connor Conor 
Congalach Conganchas Conn Connlaeth Connlaoth Connlaodh Connlaogh Connlaoi 
Conleat Conla Conley Coplait Cormac Cormacc Corb Cothric Couleth Cridan 
Crimhthann Crofinn Cromanus Crónán Cronin Crosson Cruamthain Cuán little 
hound Cuileann Cuileán Cuilén Cuilinn Cullen Cuilenn Cuimín Comyn Cuinn 
Cuirithir Cumall Cúmhaighe Cúmhaí Curoi Curran Cuthacar Daig Daigh Davin 
Dálach Daly Daley Damaen Daman Dámnh Dara Darach Daragh Darragh Darrah 
Daire Déaglán Declan Deicola Dela Demna Desle Desmond Desmumnach Deasún 
Dessi Munster Devlin Diarmait Duirmhuid Diamit Diarmaid Diarmuit Dermot 
Kermit Darby Dieul Dimnaus Disisbod Diuma Dimma Doibhilin Doireidh 
Domhnall Domnall Donál Dónall Dónall Donn Don Donnabhán Donnán Donan 
Donnchadh Donnchad Donncha Dunchad Donagh Donogh Donough Dunecan 
Donndubhán Donndubán Donovan Donngal Doran Deoradhán Dorchaidh Dubhaltach 
Dubaltach Dubultach Dualtach Dualta Duald Dubhán Duban Duane Dowan 
Dubhdara Dubhdarach Dubheidir Dubghall Dubgall Dougal Dubhghlas Dubhglas 
Dughlas Douglas Duigenan Dungal Eachann Ea Eachdhonn Éadbhard Éanna Énnae 
Enda Earna Earnán Echen Éibhear Éibhir Heber Éigneach Éigneachán Eimar 
Eiméid Éimhín Éimíne Evin Evan Eirn Ernin Ernan Eithear Elochad Emianus 
Ennae Eochaidh Echaid Eogabail Eóghan Eogan Eolus Erc Ercus Eremon Fachnan 
Fachtna Fiachna Faebhar Fáilbe Fáilbhe Failge Faolán Felan Foelan Fillan 
Farann Faughnan Feagh Fearadhach Fearadagh Farry Feardorcha Ferdorcha 
Fardoragh Fearghal Fearghall Fergal Ferghil Ferol Fearghas Fearghus Fergus 
Feichín Fehin Fechin Fiach Fiachne Feidhlim Feidhlimidh Fedelmid Feidlim 
Féilim Felim Felimy Feuillan Fillan Fiachra Ficare Fiach Findan Finegas 
Fingar Fínghín Finghin Fingin Fínín Fineen Finn Fionn Fionnán Finnian 
Finian Finnén Fionán Fianan Finnachta Finnchad Finntan Fionntán Fintan 
Fiontan Fiontán Fionnbhárr Fionnbhar Findbarr Finbar Bairre Barra Fios 
Flaithbertach Flaithrí Flurry Florry Flann Flainn Flannán Flannacán 
Fochmare Fogartach Foillas Forgael Forgall Fortchern Frainc Froichan 
Fuatach Fulan Firlan Fursa Fursey Gaithan Gall Goll Gallech Garbhán Garvan 
Garfhidh Garnard Garnat Gilian Glaisne Glassan Gnathach Gob Gobann Goban 
Gordan Gosan Gusan Gráda Guaire Herygh Hewney Huydhran Iarbonel Iarlaith 
Iarlaithe Iarfhlaith Jarlath Ior Iobhar Ibhor Ibor Abaris Ighneachán 
Imchath Incha Indract Ingnathach Ióéil Íosóg Irial Irimia Iucharba Iúd 
Iúil Joavan Kenncoh Kescog Labhcás Labraid Labhraidh Lowry Labhre Lachtna 
Laegh Leagh Laoghaire Legaire Laeghaire Leary Laistranus Laoighseach 
Laoiseach Laois Leinster Molaisse Molaise Lavren Leann Lenn Lithgean 
Lochlainn Lochlann Leachlainn Laughlin Loghlin Lodan Lomán Lonán Lorcán 
Lorcann Lorccán Lua Luchta Lugh Lughaidh Lugaidh Lughaid Lugaid Luger Lewy 
Lysagh Machar Maduta Maedoc Maidhc Maodhg Mairid Manchan Manus 
Mathghamhain Mathúin Mathghamhaim Mathgamain Mahon bear Meadhran Medran 
Meallán Mell n Mellan Mullin Medabh Mel Meldan Melkorka Melrone Meubred 
Midir Midhir Mider Mirin Mo-Bioc Mochoemoc Mochta Mochteus Mochua Mochumma 
Modomnoc Mogue Molling Moloi Molua Moluag More Morna Muchin Mughran 
Muirghean Muirgheas Muirgius Muiris Muiriartach Muicheachtach Muireadhach 
Muirchertach Muiredach Muiríoch Murtagh Murty Briartach Muirí Munnu Mura 
Muranus Murchad Murchadh Murcha Murrough Naoise Nise Nathi Nemid Nevan 
Niadh Niall Neal Neill Neil Ninian Notal Nuallán Odhrán Odran Oran Odhar 
Ógán Oisín Oissne Ossian Oisin Osheen Oscar Otteran Ounam Phelan Piran 
Rádhulbh Raghallach Reilly Riley Riaghan Riogh Rónán Ronan Rownan Rórdán 
Riordan Reardan Rearden Ríoghbhardán Ros Ró Adhán Róad Rhod Rodan Ruan 
Rowan Ruaidhrí Ruaidrí Ruairí Rudraighe Roricus Ruidhe Saebhreathach 
Sáerbrethach Saorbhreathach Saoirse Scelianus Scolaidh Scolaighe Sé 
Seachnall Séafra Seafraid Séaghdha Ségdae Seanán Senan Seanach Senach 
Séarlan Sedna Seaghán Segenus Sheary Shiel Siadhal Siaghal Siochfioldha 
Sinon Siran Siseal Sól Starn Steimhin Suibhne Suibne Sweeney Sivney Suthan 
Tadhg Tadg Tadc Tegue Teigue Teige Taig Taidgh Tiege Taidhgín Tathai 
Tiamhdha Tighearnach Tighernach Tiarnach Tiarna Tierney Tighearnán Tigern 
Tiarnán Tiernan Tiomóid Toirdhealbhach Toirdhealbharch Tairdelbach 
Toirealach Tárlach Tirloch Traolach Turlough Tomaltach Torrianus Treon 
Tóathal Tuathal Toal Uaithne Uallachán Uar Uileos Uillen Úistean Ultán 
Ultan an Ulsterman Urthaile Usliu Uthmaran Vigean Wyllow Aberth Abhartach 
Abhean Ablach Acaunus Acco Adcoprovatus Addedomarus Adgennus Adhna 
Adhnuall Adminius Adomnan Adwen Aedan Aelchinn Aer Aesico Aesk Aesubilinus 
Agh Agned Agnoman Agulus Aherne Aiel Ailbhe Ailell Ailill Aillen Aillinn 
Aincel Aindelbadh Aine Ainle Ainmire Ainsel Ainvar Airard Airetach 
Airnelach Airt Albanach Albarnaid Alcuin Allobrogicus Alpin Alston Aluin 
Alun Amalgoid Ambicatos Ambiorix Amergin Amgerit Amlaibh Amulgo Andala 
Andela Andesasus Andragius Androgius Aneroestus Anlaf Antedios Aouen 
Apullio Arbell Arcallach Archil Archu Ardan Argentocoxos Argentocoxus 
Arias Ariomardus Ariovistus Arontuis Arranen Art Artbranan Artgal Arthgal 
Arthmail Artigan Artrach Artri Arverus Arviragus Asal Ascatinius Atepacius 
Attus Audagus Auisle Aulay Aurog Autaritus Avitus Bacauda Baclan Baculo 
Badvoc Baethbarr Baiscne Baithan Baithen Baithene Balor Banquerius Banquo 
Barloc Barnoc Baroc Baruch Bathan Beag Becuma Bedwyr Belatucader Bellicia 
Bellicianus Bellovesus Belu Beolain Berchan Berec Beric Bericus Bernech 
Berngal Berric Bersa Betach Bhuice Bicelmos Bilis Biorach Bitucus 
Bitudacus Blaan Blathmac Blathmec Bleddfach Blescius Bloc Boann Boant Bobd 
Bodenius Bodh Bodhe Bodiccius Bodugenus Boduogenus Bodvoc Bodvogenus 
Bogitarus Boisel Boisil Bonoxus Borba Bothan Bov Brach Bragon Bran Brancus 
Brannoc Brath Breagan Breasal Brecbrennoch Brelade Brennus Breogan Bres 
Bresal Breward Briavel Bricriu Bricrue Brieuc Brigaco Brigantius Brigia 
Briginus Brigomaglos Brioc Britt Broccan Brockmail Broderick Brogus 
Broichan Brucetius Brude Bruide Bruidge Bruscius Brychanus Brys Bryth Buan 
Buccus Buda Budocesuganios Buic Buichet Buite Cabrach Cabriabanus 
Cacumattus Cadwan Caedmac Cael Caenneth Caibre Caichear Caier Cailcheir 
Caince Caincenn Cainnelscaith Cainte Cairbre Cairbri Cais Caisel Caitchenn 
Caittil Calgacus Calphurn Camel Camulacus Candiedo Cannaid Caoilte Capell 
Caractacus Caradig Caratach Carbery Carbh Carell Carpre Cartivellaunos 
Carvilius Cas Cascorach Cassal Cassavus Cassivellaunus Cassobellaunus 
Catavignus Cathail Cathal Cathan Cathba Cathbadh Cathlan Cathman Catigern 
Catiotuos Cattigern Catualda Cavarinus Cé Cealaigh Ceallach Ceanatis 
Ceannmhair Cearnach Cecht Ceithin Celatus Cellach Celtchar Celtillus Cenau 
Cerball Cerd Cerebig Ceretic Cermait Cerotus Cesarn Cet Cethern Cett 
Ciabhan Ciach Cian Cicht Cimarus Cinaed Cingetorix Cinhil Cintugnatus 
Cintusmus Ciotha Ciothruadh Cistumucus Cithruadh Clanova Cliach Clonard 
Cluim Cobthach Cochlan Codal Codhna Coemgen Cogidubnos Cogidubnus 
Cogitosus Coimhleathan Coinmagil Coinmail Coinneach Coirpre Colasunius 
Colban Colga Coll Colla Collamair Collbrain Colles Colm Colmkill Colpa 
Colum Comgal Comgall Comgan Comhrag Comitinus Comman Commius Compar 
Comrith Comur Comux. Con Conaing Conaire Conairy Conal Conall Conan 
Conaran Concolitanus Conconnetodumnos Concuing Condidan Conmail 
Connachtach Connell Connla Connor Conor Conory Conuall Copillus Coplait 
Coran Corann Corb Corc Corfil Corin Corio Cormiac Coron Corotiacus Corpry 
Corrgenn Cospatrick Costicus Cotuatus Covac Craftiny Credne Crega Crico 
Cridenbel Crimall Crimthan Crimthann Criomnal Crocus Crom Crotus Crovan 
Cruithne Crunnagh Crunnchu Cu Cuadan Cuailgne Cuaillemech Cualann Cuano 
Cuchulainn Cuiline Cuill Cuiran Cuirithir Culain Culcaigrie Culhwch Cullen 
Cumhaill Cumhal Cumhall Cummain Cuneda Cunedda Cuneglasus Cunetio Cunittus 
Cunlinc Cunoarda Cunobarros Cunobarrus Cunobelin Cunobelinus Cunomaglus 
Cunopectus Cunori Cunorix Cunotamus Cunoval Cunovindus Cur Curatio 
Curmissus Curoi Cushling Cuthlyn Cynloyp Cynran Cyrnan Dagobitus Daich 
Daighre Daigre Daire Dalbaech Dalbh Dall Dannicus Dathi Deaghadh Dearc 
Dearmid Debrann Decheall Dedidach Deglain Delbaith Demna Derc Derca 
Dergcroche Dergdian Dering Desa Detha Dian Dian- Cet Diarmaid Diarmait 
Diarmid Dichu Digbail Dill Dinogad Diocain Diorraing Diovicus Diviciacus 
Dobar Doccius Dogfael Doinus Dolar Dolb Doli Domhar Domingart Domnann 
Domnoellaunus Donnarthadh Donn- Ruadh Dornoch Dorus Drecan Drem Dremen 
Driccius Driumne Drochmail Drostan Druim Druimderg Drust Drustic Drystan 
Duach Duane Duartane Duatha Dubh Dubhacon Dubhan Dubhdaleithe Dubhgall 
Dubhlaing Dubnovellaunus Dubnowalos Dubnus Dudoc Dufan Dufgal Duftah 
Dugald Dumnail Dumnocoverus Dumnogenus Dumnorix Dumnove Dumnovellaunus 
Dunegal Dunegall Dunmail Dunocratis Dunod Duthac Eab Eachaidh Easal 
Eathfaigh Eber Ebicatos Eborius Eburos Echaid Ecimius Ecne Eidirsgul 
Eimhir Eine Eisu. Eithis Elagabalus Elaphius Elatha Elathan Elbodugus 
Elcmair Eldad Elitovius Elkmar Elvod Elvodug Eman Emi Emmass Enchered Enda 
Enemnogenus Enestinus Eoban Eochaid Eocho Eochy Eochymac Eogabil Eogan 
Eoganan Eoghan Eolus Eparchius Epaticcu Epaticcusepatticus Epillicus 
Eppillus Erc Eremon Erp Err Ervic Esca Espaid Esunertus Etain Etar 
Etarlaim Eterskel Etgall Ethain Ethaman Eunan Evicatos Facha- Muilleathan 
Faelinn Faltlaba Faolan Farinmagil Farinmail Febal Feclach Fedlimidh 
Feidlim Feinn Felim Ferai Fercos Ferdia Ferdiad Ferdoman Fermaise Fertai 
Fertuinne Festinien Ffion Fiacha Fiachna Fiachu Fiacuil Ficna Figel Figol 
Finan Finched Findabair Findemas Findgoll Findlaech Finegas Finn 
Finnaistucan Finnan Finnbane Finnbennach Finnian Finnleik Fintain Finvel 
Fiodhaidh Fitheal Flain Flann Flannan Flidias Fochlann Foich Foilleán 
Foiranach Fola Follamain Forannán Forannen Forne Fothaid Fotla Fuad 
Fufidius Fullon Fursa Gaible Gallgoid Gamal Garad Garbhcronan Garraidh 
Gartnait Garva Garwin Garym Gault Gavin Gebann Germocus Geron Getorix 
Gilla Gillaciaran Gillacomghain Gillechrist Gillibride Gillicolm Gillocher 
Gilloman Gingomarus Giolla- Caeimhghin Glas Glasan Glein Glentilt Glore 
Gnathach Gnobeg Gnomor Gobannitio Gobhan Godebog Goden Godfraidh Goineach 
Goitne Goll Gorm-Shuileach Gorthyn Gospatrick Gothan Gourchien Govan 
Graeme Graham Grannus Gretorix Grian Grummoch Guan Guern Gugein Guitolinus 
Guoruoe Guotepauc Guthar Guthor Gwythno Hanesa Hanlon Hanno Heber Heremon 
Huil Hunno Idanach Iduthin Iehmarc Igalram Ilar Ilaunos Ilbrec Ilbrech 
Ildathach Imhar Imidd Indech Indrechtach Indutioamrus Indutius Ingcel 
Ingnathach Ingol Innel Innsa Invomandus Iogenan Iollen Iolunn Ir Irdun 
Istolatius Istoreth Ith Iubdan Iuchar Iucharba Iunsa Ivomagus Ivonercus 
Jutus Kane Keir Kentigern Kenulphus Kian Kilian Kinan Kinemark Kineth 
Kinnear Kolbein Kuno Kylan Kyndylan Labra Labran Lachlan Laeg Laegaire 
Laeghairé Laery Laethrig Lainbhui Lairgnen Lairgren Laisren Lanuccus 
Latharne Leffius Leire Leith Leslie Lethan Levin Lewy Lia Liagan Liath 
Liathain Liathan Lifecar Lindores Liobhan Lir Litaviccus Litugenus Llif 
Llud Loarn Loarne Lobais Loban Lobharan Lobos Logiore Logotorix Lomna Lon 
Lorcain Lossio Lousius Lovernianus Luachaid Luachair Lubrin Lucco Luchta 
Luchtar Luctacus Luel Luga Lugaidh Lughaid Lugobelinus Lugotorix Lugovalos 
Luibra Luloecen Lyfing Lynch Mac Macbeathach Macbeathad Macer Machar 
Machute Macnia Madach Madan Maddan Madduin Mador Maeduin Mael Maelbeth 
Maelchwn Maeldun Maelgan Maelgwn Maelinmhain Maelmadoc Maelmichael 
Maelmuir Maelmuire Maelnibha Maelochtair Maelochtar Maelrubai Maelrubha 
Maelsechlainn Maelsechnaill Maeltine Maglocunus Maglorix Maieul Mailchon 
Maine Mal Malbride Maldred Malduin Malliacus Malone Malpedar Malpedur 
Malride Mamos Mandubracius Manducios Mannig Maol Maon Mar Marbod Maredoc 
Marobodunus Martacus Maslorius Mathgen Matuacus Matugenus Maturus Meardha 
Meargach Mechi Medraut Mellonus Melmor Menua Merddyn Mhaolain Mhichil 
Miach Michan Midac Mide Midhé Midhir Midhna Midir Mil Miled Milucra 
Miochaoin Miodac Miorog Mochrum Mochta Mochuda Modhaarn Modomnoc Moengal 
Molacus Molaise Molloy Moluag Monaid Moncuxoma Mongan Morann Morc Morgund 
Moriartak Morias Moricamulus Morirex Moritasgus Morvidd Motius Muadhan 
Muddan Muirchú Muiredach Mungo Murchadh Mutaten Muthill Nadfraech Naid 
Nantosvelta Nantua Naoise Narlos Natanleod Nathrach Natorus Neamh 
Necalimes Nechtan Nectovelius Neidhe Neit Nemanach Nemed Nemglan Nemhnain 
Nemmonius Nessa Niadhnair Nollaig Nos Novantico Nuada Nuadha Nynia 
Octrialach Octruil Odhrain Odras Ogma Ogmios Oilioll Oisin Olchobar 
Ollovico Oncus Orbissa Oren Orgetorix Orgillus Orphir Oscair Oscar Osgar 
Owain Paetus Patendinus Pesrut Pisear Potitus Potomarus Prasutagus 
Pridfirth Qodvoldeus Raighne Raigre Rascua Regol Reoda Reo-Derg Rhiada 
Riagall Rian Rianorix Riata Ribh Ringan Riommar Rivius Robartaig 
Robhartach Rogh Roth Rowan Ruadan Ruadhan Rudrach Rudraighe Ruide Ruith 
Rurio Saccius Saenius Saenu Saidhe Sal Salmhor Salorch Saltran Samtan 
Samthainn Sangus Saturio Sawan Sceolan Scrocmagil Scrocmail Seaghan 
Seanchab Searigillus Searix Sechnaill Secumos Sedullus Seghine Segine 
Segovax Sellic Semion Senaculus Sencha Senias Sennianus Senorix Senshenn 
Senuacus Sepenestus Sera Servan Sesnan Setanta Setibogius Sgoith- Gleigeil 
Sharvan Sholto Sighi Sigmal Silinus Sinell Sinill Sinnoch Sinsar Sital 
Sitric Skolawn Sligech Smertrius Solais Sollus Sorio Soulinus Sreng 
Stariat Starn Stavacus Strathairn Strowan Struan Suadnus Sualdam Sualtam 
Suanach Suavis Subhkillede Subsio Sucabus Suibhne Sulien Summacus Suriacus 
Syagris Tabarn Tadg Tailc Taileach Taistellach Talchimen Taliesin Talorcan 
Talore Tamesubugus Tammonius Tarvos Tasciovanus Tasgetius Tassach 
Taximagulus Tethra Tetrecus Tetricus Teutomatus Teyrnon Tigernach 
Tigernann Tincomarus Tincommius Tocha Togodumnus Topa Tor Torannen Toutius 
Trad Tradui Trendhorn Trenmor Trenus Treon Triathmor Trogain Troghwen 
Tuaigh Tuan Tuathal Tuirbe Tuireann Tuis Tullich Turenn Turlough Tyrnon 
Uaithnin Uar Uccus Uchtain Ueda Uepogenus Uige Uirolec Uisneach Ulchil Un 
Unthaus Urfai Urgriu Urias Urien Usna Usnach Vadrex Vainche Vallaunius 
Vassedo Vatiaucus Veda Vediacus Vellocatus Veluvius Venutius Vepogenus 
Vercassivellaunus Vercingetorix Verctissa Verecundus Verica Vernico 
Viasudrus Viducus Vindex Vindomorucius Vinnian Virdomarus Viroma 
Virssucius Volisus Vortigern Vortimax Vortimer Vortipor Vortrix Voteporix 
Vran Vron Wannard Weonard 
 * 
Chinese (syllables) -- zhang mei xue baoqin shi xiangyun jia baoyu tanchun youshi you erjie 
wang xifeng feng san xing xue zhao zhou baochai bao er baoqin baoyu xue 
pan youshi zhen shiyin jia lian zheng wang pan baochai li wan she liu 
bao chai dai feng lian lin pan tan wan xing ying zhen xia daiyu wang 
jiren yingchun fei hu junrong huan bin chang cong guang heng huan jing 
jun lan lian ling qiaojie qiong rong sijie tanchun xichun xiluan 
yuanchun zhen zheng jingui jia xing zhen da lai sheng gui qi li wan wen 
daiyu lin zhixiao xianglian loushi xia he song tian ye zhu jinxiang bao 
huan lan rong duo er lin lis qin shi tan xi xia ying yun liu zhang zhen 
liu shan xue xia you zhao zhen zhu xue youan pan beijing shui huai qian 
qiaojie qin baoqin xian sanjie xiangyun sijie jing shaozu sun tanchun 
dequan xing shanbao xin ziteng shan xindeng xiluan bingzhong xiuyan 
baochai baoqin ke youshi yu lu yuanchun yingchun yun zhang dehui hua 
guoji zhao zhen zhou rui zhuer ning guo daihua ruhai ziteng ren guang 
tiandong tianliang shiyin yinglian xindeng xiangyun qin zhong bangye 
qiaojie shiyin 
 * 
English (words) -- abnormal abode abominate absentminded accomplish acronym actress adjust 
admiration admission admit admonish admonition adolescent advertise 
affinity aflame afoul agenda agglomerate aghast agree ahem ahoy 
allotropic altruist ambuscade ameliorate anger animosity annul 
anorthosite anthracite antonym applejack approximable arcane arenaceous 
argue arise arisen armament armful arraign arroyo arteriole as aspire 
asplenium aster augur automata automobile autonomous autopsy averred 
axiomatic bag bailiff baldy bandit bangkok barley base batt battlefront 
bauble bayonet bedfast bee began belladonna bellum belong bemadden 
beneficent beverage bilinear birdseed bisect bismuth bittersweet 
bizarre blackball blest blip blow bluish boast bob boggy bonito bosonic 
boundary bovine bowline bronchiolar bronchitis bullfrog bumble 
bushmaster butane buttonhole cab cabinet calculus camelopard cannonball 
canto carboxy caret cartilage cataclysmic catastrophe cauliflower 
causal cement centerpiece centric chagrin chamfer champion chard chase 
cheerleader chide chieftain chortle chugging chump churchwomen citron 
civic clapboard clarinet clasp claw click clinic cloak clomp clonic 
close cloudy clung coagulate cocksure coffin collapsible collateral 
commando commendation committable compact compartment compressive 
compunction concave conclusion condiment conductance conferred 
confidante congeal congratulatory conservative console consternate 
consulate contaminate contention contractor contradistinct contributor 
copious cordial cornflower cornmeal corporate corpsmen cosy counterfeit 
couscous cover cranberry crap credo creekside crossbill crosswise 
crossword curlicue custodial cutaneous cutover cutset dactylic darling 
deadline dearie debunk deceive decibel deciduous decisive declivity 
decree degenerate depositor depressive derive destabilize detector 
devisee dialysis did diplomat dirty disburse dispersal dispute dissuade 
disturbance diversionary dogwood dolce doldrums downgrade drainage 
driven dryad dud due duke dungeon eavesdrop edit eigenfunction either 
elder elementary elephant elongate enunciate errata eta ethnic 
euphorbia excess excessive exemplar extendible extraneous extrapolate 
exuberant fadeout farmhouse farmland fascist fellow fidget fifteenth 
filmmake firecracker fireproof flack flagging flame flashlight fleshy 
fluffy force forsaken forthcome fortunate fortune fought freer front 
fundamental fungoid gadfly gal gangster intestinal gatekeep gather 
gavel geminate geochronology gerundive girlie glom glottis godkin 
goldenseal gorgeous grandson granule graph greylag guesswork gulf gull 
gumbo gunman guru gust gymnasium hacienda hackney handicap handicapped 
handline handwaving harmonic haulage hedge heliotrope hellfire hemp 
henbane henchmen hepatitis herb hereabout heroes high hillman hillmen 
hilt hindmost hoarfrost hobgoblin hobo hog homophobia honorary 
horseshoe hovel hover hoy hymn hysteric idol idyllic illegible 
illuminate impartation implacable improper inability inaptitude 
incommensurate incorrect indecision indefinable indelicate indicant 
indicate industry ineffective infix inflammable inflow infuriate 
ingenuity inherent inherit initiate inlay inspect inspiration instead 
intention intermit internescine intervenor invalidate involve 
irrational irrecoverable irreducible it jaguar jean jitterbugger 
jittery jock jolt junketeer jute karate kidney kindred knit kraut label 
larval larynx last latent laureate lawgive leaf lean learn lease 
leeward leftward legion lemonade library limpid lion locomotor locoweed 
logging lollipop lord lunch luscious luxe macrame magnesia malignant 
man mankind mannequin mantel marrow mater mawkish mead medic medicinal 
meiosis metric metronome mid middlemen mimicking minimum minnow 
miraculous misshapen missionary mitt mix moat modular moisture molehill 
molest molt molten monel morphology mortem motivate mulct mutter myrrh 
narcosis neck neither neural nitride nomadic non northern nuclear 
numerous numismatic nurture obduracy objectify obverse often ohm on 
onlooking oodles operate opossum orate oust pallet paramagnet parasite 
pardon parley parquet pastime paunch pauper paymaster peccary pedal 
peltry pendant penetrable penna percussive perdition periwinkle permute 
personage pestilential petroglyph pickaxe piece pilgrimage pinafore 
plaintive plantation playtime plead please pluggable polarography 
politic politician polyglot pompon poor popcorn portraiture 
postcondition preamble predatory prehistoric preponderate preservation 
principal privacy probabilist proceed prod prodigious proposition 
protoplasm proverb puff pupal purloin purposeful qualm quantitative 
quark quarrymen ragout railroad redneck registrable registration 
regulate rejecter relate repairman reparation restraint retaliatory 
retribution retrogress retrovision reversion revery revolt revolve 
rhetorician ribonucleic rightmost rode ruff ruin runneth runt sabotage 
sack sagging sailor salamander sale satan satin sawfish schist scissor 
scowl scrawny scream script seahorse seal secede seduce seen seersucker 
segmentation serine shadowy shifty shortsighted shrank shrill 
sicklewort signboard silverware singsong sinusoidal skylight slam 
slater sleepy sloganeer snowflake soften software solitaire solitude 
somebody son sophisticate southland spark spectra spectrogram 
spiderwort spiritual splash sponsor sporadic sprain springtail spurt 
squirt staff stalwart stare steelmake storeroom strawberry string 
striptease stupefy subliminal subtle sugar suggest sulk summon 
superlunary swage syndicate tall tantrum tariff tasting teamster 
telepathic tempestuous territorial thank thermostat thorium thousand 
thousandfold thymine tiger titanic tolerant tomograph tomography top 
torch tornado track traffic transact transfix transfusion transgression 
translucent transmute tread treadmill trillion tristate trisyllable 
trophic truce trump trustful turbid turgid turnout twilight twinge 
upsetting upstand uptrend uranyl urgency utensil vanadium vantage 
varistor veery vellum velocity venomous vigilante vindicate virtuous 
visa vitreous vivify waistline walkout walkway warble washbasin washout 
waspish wellington whim whimsic whittle widgeon winery wise wish 
withdraw woke wrote yank youthful zap zeta zombie 
 * 
English (surnames) -- Adams Adamson Adler Akers Akin Aleman Alexander Allen Allison Allwood 
Anderson Andreou Anthony Appelbaum Applegate Arbore Arenson Armold 
Arntzen Askew Athanas Atkinson Ausman Austin Averitt Avila-Sakar 
Badders Baer Baggerly Bailliet Baird Baker Ball Ballentine Ballew Banks 
Baptist-Nguyen Barbee Barber Barchas Barcio Bardsley Barkauskas Barnes 
Barnett Barnwell Barrera Barreto Barroso Barrow Bart Barton Bass Bates 
Bavinger Baxter Bazaldua Becker Beeghly Belforte Bellamy Bellavance 
Beltran Belusar Bennett Benoit Bensley Berger Berggren Bergman Berry 
Bertelson Bess Beusse Bickford Bierner Bird Birdwell Bixby Blackmon 
Blackwell Blair Blankinship Blanton Block Blomkalns Bloomfield Blume 
Boeckenhauer Bolding Bolt Bolton Book Boucher Boudreau Bowman Boyd 
Boyes Boyles Braby Braden Bradley Brady Bragg Brandow Brantley Brauner 
Braunhardt Bray Bredenberg Bremer Breyer Bricout Briggs Brittain 
Brockman Brockmoller Broman Brooks Brown Brubaker Bruce Brumfield 
Brumley Bruning Buck Budd Buhler Buhr Burleson Burns Burton Bush 
Butterfield Byers Byon Byrd Bzostek Cabrera Caesar Caffey Caffrey 
Calhoun Call Callahan Campbell Cano Capri Carey Carlisle Carlson 
Carmichael Carnes Carr Carreira Carroll Carson Carswell Carter 
Cartwright Cason Cates Catlett Caudle Cavallaro Cave Cazamias Chabot 
Chance Chapman Characklis Cheatham Chen Chern Cheville Chong 
Christensen Church Claibourn Clark Clasen Claude Close Coakley Coffey 
Cohen Cole Collier Conant Connell Conte Conway Cooley Cooper Copeland 
Coram Corbett Cort Cortes Cousins Cowsar Cox Coyne Crain Crankshaw 
Craven Crawford Cressman Crestani Crier Crocker Cromwell Crouse Crowder 
Crowe Culpepper Cummings Cunningham Currie Cusey Cutcher Cyprus 
D'Ascenzo Dabak Dakoulas Daly Dana Danburg Danenhauer Darley Darrouzet 
Dartt Daugherty Davila Davis Dawkins Day DeHart DeMoss DeMuth 
DeVincentis Deaton Dees Degenhardt Deggeller Deigaard Delabroy Delaney 
Demir Denison Denney Derr Deuel Devitt Diamond Dickinson Dietrich 
Dilbeck Dobson Dodds Dodson Doherty Dooley Dorsey Dortch Doughty Dove 
Dowd Dowling Drescher Drucker Dryer Dryver Duckworth Dunbar Dunham Dunn 
Duston Dettweiler Dyson Eason Eaton Ebert Eckhoff Edelman Edmonds 
Eichhorn Eisbach Elders Elias Elijah Elizabeth Elliott Elliston Elms 
Emerson Engelberg Engle Eplett Epp Erickson Estades Etezadi Evans Ewing 
Fair Farfan Fargason Farhat Farry Fawcett Faye Federle Felcher Feldman 
Ferguson Fergusson Fernandez Ferrer Fine Fineman Fisher Flanagan 
Flathmann Fleming Fletcher Folk Fortune Fossati Foster Foulston Fowler 
Fox Francis Frantom Franz Frazer Fredericks Frey Freymann Fuentes 
Fuller Fundling Furlong Gainer Galang Galeazzi Gamse Gannaway Garcia 
Gardner Garneau Gartler Garverick Garza Gatt Gattis Gayman Geiger 
Gelder George Gerbino Gerbode Gibson Gifford Gillespie Gillingham 
Gilpin Gilyot Girgis Gjertsen Glantz Glaze Glenn Glotzbach Gobble 
Gockenbach Goff Goffin Golden Goldwyn Gomez Gonzalez Good Graham Gramm 
Granlund Grant Gray Grayson Greene Greenslade Greenwood Greer Griffin 
Grinstein Grisham Gross Grove Guthrie Guyton Haas Hackney Haddock 
Hagelstein Hagen Haggard Haines Hale Haley Hall Halladay Hamill 
Hamilton Hammer Hancock Hane Hansen Harding Harless Harms Harper 
Harrigan Harris Harrison Hart Harton Hartz Harvey Hastings Hauenstein 
Haushalter Haven Hawes Hawkins Hawley Haygood Haylock Hazard Heath 
Heidel Heins Hellums Hendricks Henry Henson Herbert Herman Hernandez 
Herrera Hertzmann Hewitt Hightower Hildebrand Hill Hindman Hirasaki 
Hirsh Hochman Hocker Hoffman Hoffmann Holder Holland Holloman Holstein 
Holt Holzer Honeyman Hood Hooks Hopper Horne House Houston Howard 
Howell Howley Huang Hudgings Huffman Hughes Humphrey Hunt Hunter Hurley 
Huston Hutchinson Hyatt Irving Jacobs Jaramillo Jaranson Jarboe Jarrell 
Jenkins Johnson Johnston Jones Joy Juette Julicher Jumper Kabir 
Kamberova Kamen Kamine Kampe Kane Kang Kapetanovic Kargatis Karlin 
Karlsson Kasbekar Kasper Kastensmidt Katz Kauffman Kavanagh Kaydos 
Kearsley Keleher Kelly Kelty Kendrick Key Kicinski Kiefer Kielt Kim 
Kimmel Kincaid King Kinney Kipp Kirby Kirk Kirkland Kirkpatrick 
Klamczynski Klein Kopnicky Kotsonis Koutras Kramer Kremer Krohn Kuhlken 
Kunitz LaLonde LaValle LaWare Lacy Lam Lamb Lampkin Lane Langston 
Lanier Larsen Lassiter Latchford Lawera LeBlanc LeGrand Leatherbury 
Lebron Ledman Lee Leinenbach Leslie Levy Lewis Lichtenstein Lisowski 
Liston Litvak Llano-Restrepo Lloyd Lock Lodge Logan Lomonaco Long Lopez 
Lopez-Bassols Loren Loughridge Love Ludtke Luers Lukes Luxemburg 
MacAllister MacLeod Mackey Maddox Magee Mallinson Mann Manning Manthos 
Marie Marrow Marshall Martin Martinez Martisek Massey Mathis Matt 
Maxwell Mayer Mazurek McAdams McAfee McAlexander McBride McCarthy 
McClure McCord McCoy McCrary McCrossin McDonald McElfresh McFarland 
McGarr McGhee McGoldrick McGrath McGuire McKinley McMahan McMahon 
McMath McNally Mcdonald Meade Meador Mebane Medrano Melton Merchant 
Merwin Millam Millard Miller Mills Milstead Minard Miner Minkoff 
Minnotte Minyard Mirza Mitchell Money Monk Montgomery Monton Moore 
Moren Moreno Morris Morse Moss Moyer Mueller Mull Mullet Mullins Munn 
Murdock Murphey Murphy Murray Murry Mutchler Myers Myrick Nassar Nathan 
Nazzal Neal Nederveld Nelson Nguyen Nichols Nielsen Nockton Nolan 
Noonan Norbury Nordlander Norris Norvell Noyes Nugent Nunn O'Brien 
O'Connell O'Neill O'Steen Ober Odegard Oliver Ollmann Olson Ongley 
Ordway Ortiz Ouellette Overcash Overfelt Overley Owens Page Paige 
Pardue Parham Parker Parks Patterson Patton Paul Payne Peck Penisson 
Percer Perez Perlioni Perrino Peterman Peters Pfeiffer Phelps Philip 
Philippe Phillips Pickett Pippenger Pistole Platzek Player Poddar 
Poirier Poklepovic Polk Polking Pond Popish Porter Pound Pounds Powell 
Powers Prado Preston Price Prichep Priour Prischmann Pryor Puckett 
Raglin Ralston Rampersad Ratner Rawles Ray Read Reddy Reed Reese Reeves 
Reichenbach Reifel Rein Reiten Reiter Reitmeier Reynolds Richardson 
Rider Rhinehart Ritchie Rittenbach Roberts Robinson Rodriguez Rogers 
Roper Rosemblun Rosen Rosenberg Rosenblatt Ross Roth Rowatt Roy Royston 
Rozendal Rubble Ruhlin Rupert Russell Ruthruff Ryan Rye Sabry Sachitano 
Sachs Sammartino Sands Saunders Savely Scales Schaefer Schafer Scheer 
Schild Schlitt Schmitz Schneider Schoenberger Schoppe Scott Seay Segura 
Selesnick Self Seligmann Sewall Shami Shampine Sharp Shaw Shefelbine 
Sheldon Sherrill Shidle Shifley Shillingsburg Shisler Shopbell Shupack 
Sievert Simpson Sims Sissman Smayling Smith Snyder Solomon Solon 
Soltero Sommers Sonneborn Sorensen Southworth Spear Speight Spencer 
Spruell Spudich Stacy Staebel Steele Steinhour Steinke Stepp Stevens 
Stewart Stickel Stine Stivers Stobb Stone Stratmann Stubbers Stuckey 
Stugart Sullivan Sultan Sumrall Sunley Sunshine Sutton Swaim Swales 
Sweed Swick Swift Swindell Swint Symonds Syzdek Szafranski Takimoto 
Talbott Talwar Tanner Taslimi Tate Tatum Taylor Tchainikov Terk Thacker 
Thomas Thompson Thomson Thornton Thurman Thurow Tilley Tolle Towns 
Trafton Tran Trevas Trevino Triggs Truchard Tunison Turner Twedell 
Tyler Tyree Unger Van Vanderzanden Vanlandingham Varanasi Varela Varman 
Venier Verspoor Vick Visinsky Voltz Wagner Wake Walcott Waldron Walker 
Wallace Walters Walton Ward Wardle Warnes Warren Washington Watson 
Watters Webber Weidenfeller Weien Weimer Weiner Weinger Weinheimer 
Weirich Welch Wells Wendt West Westmoreland Wex Whitaker White Whitley 
Wiediger Wilburn Williams Williamson Willman Wilson Winger Wise Wisur 
Witt Wong Woodbury Wooten Workman Wright Wyatt Yates Yeamans Yen York 
Yotov Younan Young Zeldin Zettner Ziegler Zitterkopf Zucker 
 * 
English (female names) -- Aimee Aleksandra Alice Alicia Allison Alyssa Amy Andrea Angel Angela 
Ann Anna Anne Anne Marie Annie Ashley Barbara Beatrice Beth Betty 
Brenda Brooke Candace Cara Caren Carol Caroline Carolyn Carrie 
Cassandra Catherine Charlotte Chrissy Christen Christina Christine 
Christy Claire Claudia Courtney Crystal Cynthia Dana Danielle Deanne 
Deborah Deirdre Denise Diane Dianne Dorothy Eileen Elena Elizabeth 
Emily Erica Erin Frances Gina Giulietta Heather Helen Jane Janet Janice 
Jenna Jennifer Jessica Joanna Joyce Julia Juliana Julie Justine Kara 
Karen Katharine Katherine Kathleen Kathryn Katrina Kelly Kerry Kim 
Kimberly Kristen Kristina Kristine Laura Laurel Lauren Laurie Leah 
Linda Lisa Lori Marcia Margaret Maria Marina Marisa Martha Mary Mary 
Ann Maya Melanie Melissa Michelle Monica Nancy Natalie Nicole Nina 
Pamela Patricia Rachel Rebecca Renee Sandra Sara Sharon Sheri Shirley 
Sonia Stefanie Stephanie Susan Suzanne Sylvia Tamara Tara Tatiana Terri 
Theresa Tiffany Tracy Valerie Veronica Vicky Vivian Wendy 
 * 
English (male names) -- Aaron Adam Adrian Alan Alejandro Alex Allen Andrew Andy Anthony Art 
Arthur Barry Bart Ben Benjamin Bill Bobby Brad Bradley Brendan Brett 
Brian Bruce Bryan Carlos Chad Charles Chris Christopher Chuck Clay 
Corey Craig Dan Daniel Darren Dave David Dean Dennis Denny Derek Don 
Doug Duane Edward Eric Eugene Evan Frank Fred Gary Gene George Gordon 
Greg Harry Henry Hunter Ivan Jack James Jamie Jason Jay Jeff Jeffrey 
Jeremy Jim Joe Joel John Jonathan Joseph Justin Keith Ken Kevin Larry 
Logan Marc Mark Matt Matthew Michael Mike Nat Nathan Patrick Paul Perry 
Peter Philip Phillip Randy Raymond Ricardo Richard Rick Rob Robert Rod 
Roger Ross Ruben Russell Ryan Sam Scot Scott Sean Shaun Stephen Steve 
Steven Stewart Stuart Ted Thomas Tim Toby Todd Tom Troy Victor Wade 
Walter Wayne William 
 * 
Estonian (words) -- akti tekst 
maaparandusseadus 
vastu voetud aprillil 
i peatukk 
paragrahv seaduse ulesanne 
kaesoleva seaduse ulesandeks on reguleerida maaparandusega 
seotud oigussuhteid 
paragrahv moisted 
kaesolevas seaduses kasutatakse moisteid jargmises 
tahenduses 
maaparandus pollu ja metsamajandusliku maa 
kuivendamine niisutamine voi veereiimi kahepoolne 
reguleerimine samuti agromelioratiivsete voi kultuurtehniliste 
meetmete rakendamine 
maaparandussusteem maa kuivendamiseks niisutamiseks 
voi veereiimi kahepoolseks reguleerimiseks vajalike rajatiste 
ja hoonete kompleks 
eesvool pinnaveekogu koos sellel asuvate rajatistega 
kui sellesse suubuvad teised veejuhtmed 
polder tammidega umbritsetud kuivendatud maaala 
millelt vesi juhitakse ara veetosteseadmega 
veehoidla vooluvee tokestamisega voi muul viisil 
rajatud tehisveekogu 
niisutussusteem rajatiste kompleks vee ammutamiseks 
veeallikast ja selle jaotamiseks niisutatavale maaalale 
maaparandussusteemi omanik 
maaparandussusteem voi selle osa on maatuki oluline osa 
ja kuulub maaomanikule 
maaparandussusteemi voi selle osa omanik voib olla 
fuusiline isik voi eraoiguslik juriidiline isik samuti riik voi 
kohalik omavalitsus voi seaduses satestatud avalikoiguslik 
juriidiline isik 
riiklik maaparandusteenistus 
vabariigi valitsus maarab kindlaks riikliku 
maaparandusteenistuse struktuuri ja juhtimise korralduse 
maaparandusuhistu 
maaparandustoode tegemiseks ja maaparandushoiuks voib 
asutada maaparandusuhistu 
maaparandusuhistu liikmed 
maaparandusuhistu liikmeteks voivad olla maaomanikud 
ja valdajad omaniku volituse alusel kelle maatukile voi 
ettevottele toob uhistu tegevus kasu histu moodustanud isikud 
on uhistu asutajaliikmed 
maaparandusuhistu liikmeks olemine on sellega seotud 
maatuki voi ettevotte omanikule kohustuslik kui ta saab kasu 
maaparandussusteemi toimimisest 
kui maaparandusuhistu tegevuse kestel selgub et moni 
uhistu liige ei saa kasu uhistu tegevusest voib ta uhistust 
valja astuda uldkoosoleku nousolekul 
maaomanik kes ei saa kasu uhistu tegevusest ei ole 
kohustatud uhistu liikmeks astuma kuid peab voimaldama 
veejuhtme ehitamist labi oma maa ja juurdepaasu oma maal 
paikneva maaparandussusteemi hooldamiseks maaomanik voib nouda 
seejuures tekkiva kahju eelnevat huvitamist 
maaparandussusteemi toimimisest saadava arvestusliku 
kasu maaramise metoodika ja juhendi kinnitab 
pollumajandusminister 
maaparandusuhistu asutamine 
maaparandusuhistu asutamine toimub eesti vabariigi 
uhistuseaduse rt ja kaesoleva seadusega 
satestatud korras 
maaparandusuhistu liikme kohustused 
maaparandusuhistu liikme kohustused voib kanda 
asjaoigusena uhistu uldkoosoleku otsuse pohjal juhatuse 
uhepoolse notariaalselt toestatud avalduse alusel 
kinnistusraamatusse 
maaparandussusteemist kasu saava maatuki voi ettevotte 
omandioiguse uleminekul loetakse uus omanik uhistu liikmeks ja 
talle lahevad ule endise omaniku oigused ja kohustused uhistu 
suhtes 
maaparandusuhistu liige on kohustatud tasuma uhistule 
maaparandussusteemi rajamise ja hoiu kulutused 
proportsionaalselt arvestusliku kasuga 
maaparandusuhistu registreerimisega lahevad 
maaparandussusteemide ehituse voi hoiuga seotud oigused ja 
kohustused uhistu liikmelt ule uhistule ulatuses mis on ette 
nahtud uhistu pohikirjaga 
ii peatukk 
maaparandusssteemide rajamine ja kasutamine 
maaparandussusteemide rajamine 
maaparanduslikke ehitustoid voib teha soltumata 
maatuki kuuluvusest vaid kooskolastatud maaparandusprojekti ja 
ehitusloa alusel 
maaparandusprojekti kooskolastamisest ja ehitusloa 
valjaandmisest keeldutakse kui maaparandustoodega rikutakse 
kaitsereiime pohjendamatult kahjustatakse voi pohjendamatult 
muudetakse loodust voi tekitatakse kahju teistele maaomanikele 
voi maa ja veekasutajatele 
maaparandusprojektis sisalduvate ja tehnoloogianouetega 
nahakse ette maastikuhooldus ning tagatakse 
maaparandussusteemide rajamisel ja korrashoiul loodusvarade 
looduskeskkonna loodusobjektide muinasmalestiste ning teistele 
omanikele kuuluvate rajatiste hoid ja sailimine 
maaparandusprojektide koostamise maaparandustoode 
tehnoloogiliste nouete ja nende kinnitamise maaparandusehitiste 
ehituslubade valjastamise ning maaparandussusteemi rajamisega 
kaasneva vee erikasutusoiguse tekkimise ja loppemise korra 
kehtestab vabariigi valitsus 
maaparandussusteemi rajamine voorale maale 
maa kuivendamiseks voi niisutamiseks vajaliku veejuhtme 
rajamine voorale maale voib toimuda asjaoigusseaduse rt i 
loikes ja paragrahvis satestatud 
tingimustel 
vee juhtimiseks labi voora maa tuleb asjaosalistel 
seada servituut valitseva kinnisasja kasuks mille kohta 
kohaldatakse asjaoigusseaduse satteid kui kaesolevast seadusest 
ei tulene teisiti 
vee juhtimine peab toimuma voimalikult mooda piire voi 
sihte kus maaparandussusteem maa kasutamist koige vahem 
takistab ja maale koige vahem kahju tekitab labi oue 
puiestike viljapuu ja koogiviljaaedade voib vett juhtida 
ainult toruveejuhtmetega kui omanikuga ei lepita kokku teisiti 
loa andmine maaparandussusteemi rajamiseks 
voorale maale 
vee juhtimiseks labi voora maa voidakse anda ajutine 
voi alaline luba kui 
loa taotleja kasu on oluliselt suurem teisele omanikule 
tekitatavast kahjust 
loa taotleja kohustub rakendama ettevaatusabinousid mis 
voimaldavad kahju ara hoida voi vahendada 
loa valjaandja peab loa valjaandmise otsusest teatama 
asjaosalistele kellel on oigus otsuse peale kaevata 
kui asjaosalised ei saa kokkuleppele servituutide 
seadmisel maaparandussusteemi rajamiseks vee juhtimisega labi 
voora maa voib servituudi seada uhepoolse notariaalselt 
toestatud avalduse alusel kui on makstud tagatissumma 
deposiiti pollumajandusminister maarab kindlaks 
veejuhtimisoiguse eest makstava tasu ja tagatissumma arvutamise 
korra 
ehitusluba maaparandussusteemi rajamiseks voorale maale 
antakse parast servituudi kandmist kinnistusraamatusse 
kahjude huvitamine 
omanik kelle maast antakse oigus vett labi juhtida 
voib nouda maaparandussusteemi alla jaava maa araostmist loa 
saaja poolt samuti kogu ulejaanud maa voi selle osa araostmist 
mis maaparandussusteemi rajamise tagajarjel kasutuskolbmatuks 
muutub 
veejuhtimisoiguse eest maksab selle oiguse saaja 
omanikule tasu uhekordselt voi igaaastaste maksetena tasu 
makstakse ka maaparandussusteemi rajamise tagajarjel maatuki 
voimaliku koguvaartuse vahenemise eest 
maaparandussusteemi rajaja peab huvitama maaomanikule 
ja valdajale tekitatud kahjud maaomanik ja valdaja voivad 
nouda eelnevalt tagatise maksmist deposiiti 
kui maaparandussusteemi rajamise tagajarjel ilmnevad 
kahjud mida veejuhtimisoiguse andmisel ette ei nahtud kuuluvad 
need huvitamisele kahju tekitanud isiku poolt taies ulatuses 
maaparandustoode finantseerimine 
maaparandustood finantseerib tellija 
riik toetab maaparandustoid 
maaparandustoode finantseerimise riiklike toetusrahade 
maaramise ja krediteerimise korra kehtestab vabariigi valitsus 
maaparandussusteemi kasutamine 
maaparandussusteemi kasutamise korra kehtestab susteemi 
omanik kokkuleppel maaomaniku voi valdajaga kelle maal temale 
kuuluv susteem voi selle osad asuvad 
maaomanikul voi valdajal on oigus kasutada tema maale 
rajatud maaparandussusteemi voi selle osi omanikuga 
kokkulepitud tingimustel 
iii peatukk 
maaparandushoid 
maaparandushoiu moiste ja eesmark 
maaparandushoid on maaparandussusteemide toimimist tagav 
tegevus mis seisneb maaparandussusteemide remondis ja hooldes 
ning muus maaparandussusteemide valdamisega seotud tegevuses 
maaparandushoiu korraldus 
maaparandushoidu korraldab maaparandussusteemi omanik 
maaomanik vastutab tema maal asuvate teistele omanikele 
kuuluvate maaparandussusteemide tahtliku rikkumise eest 
voorale maale rajatud maaparandussusteemi hoiab korras 
servituuti valitseva kinnisasja omanik 
eesvoolud poldrid veehoidlad ja niisutussusteemid 
mis toovad kasu mitmele maaomanikule voi valdajale tuleb hoida 
korras maaparandusuhistute kaudu maaparandussusteemide loetelu 
mis kuuluvad korrashoidmisele maaparandusuhistute kaudu 
kehtestab pollumajandusminister kooskolastatult kohaliku 
omavalitsusega 
kuni maaparandusuhistu asutamiseni hoiab eesvoolu 
poldri veehoidla ja niisutussusteemi voi nende osa korras 
maaomanik voi valdaja kelle maal see paikneb ja kellele see 
toob kasu vajaduse korral maarab korrashoiu kohustuse 
pollumajandusminister kooskolastatult kohaliku omavalitsusega 
kaesoleva paragrahvi ja loikes nimetatud 
maaparandussusteemide korrashoidu kontrollib maaparanduse 
jarelevalve tootaja puuduste korral maaratakse tahtaeg nende 
korvaldamiseks puuduste mittekorvaldamisel tahtajaks voib 
pollumajandusministri voi kohaliku omavalitsuse korraldusel 
lasta need tood ara teha kolmandatel isikutel ja kulutused sisse 
nouda neilt kes olid kohustatud neid susteeme korras hoidma 
juhul kui kasutatava maaparandussusteemi omanikud ei 
pea vajalikuks maaparandusuhistu moodustamist voivad nad 
solmida uhise tegutsemise lepingu valja arvatud kaesoleva 
paragrahvi loikes nimetatud juhtudel 
riigi poolt korrashoitavate eesvoolude nimekirja 
kinnitab vabariigi valitsus 
kitsendused maaparandushoiul 
igasugune kunstlik veevoolu takistamine ja ummistamine 
maaparandussusteemis ning veevott maaparandussusteemist kui see 
tekitab kahju teisele maaomanikule voi maaparandussusteemile on 
keelatud 
maad ei voi harida lahemal kui uks meeter eesvoolu 
pervest kui seadusega voi vabariigi valitsuse poolt kehtestatud 
korras ei maarata kindlaks laiemat veekaitsevoondit 
maaomanik peab lubama kasutada oma maad 
maaparandussusteemide seisundi kontrollimiseks 
maaparanduslikeks uurimis ja projekteerimistoodeks 
maaparandustoodest tingitud ajutisteks labisoitudeks ja pinnase 
paigaldamiseks kui huvitatakse talle tekitatud kahju 
maaparandussusteemil paiknevate ja selle 
omanikule mittekuuluvate ehitiste hoid 
maaparandussusteeme uletavaid raudtee ja maanteesildu 
hoiab korras raudtee voi maantee omanik voi valdaja vastavalt 
teistele oigusaktidele maaparandustood mis pohjustavad 
veereiimi muutusi raudtee voi maanteemaal kooskolastatakse 
raudtee voi maantee omanikuga 
maa kruntimine 
maa kruntimisel voi umberkruntimisel kooskolas 
maakorralduslike oigusaktidega tuleb tagada maaparandussusteemi 
toimimine ja kaitse 
vastutus maaparandussusteemi hoiu kohustuste 
mittetaitmise korral 
maaparandussusteemi omanik voi valdaja on kohustatud 
huvitama teistele maaparandussusteemi omanikele voi valdajatele 
kahju mis on tekkinud tema suulise kaitumise tottu 
maaparandussusteemi hoiul kahju tekitamises suudistatu vabaneb 
selle huvitamisest kui ta toendab et see ei ole tekkinud tema 
suu labi 
maaparandussusteemi kahjustamise eest kohaldatakse 
tsiviil kriminaal voi haldusvastutust 
maaparandussusteemide kaitse ulukite 
kahjustava tegevuse eest 
omanikul on oigus kaitsta maaparandussusteemi ulukite 
kahjustava tegevuse eest 
kui maaparandussusteemi ohustavad kopratammid voib 
omanik voi valdaja need korvaldada keskkonnaministri poolt 
kehtestatud korras 
kui maaparandussusteeme ohustavaid kopratamme ei ole 
loodushoiu seisukohast voimalik korvaldada huvitatakse 
omanikule tekitatud kahju riigieelarvest 
huvituse maksmise korra kehtestab vabariigi valitsus 
iv peatukk 
seaduse rakendamine 
maaparandussusteemide uleandmine omanikele 
pollumajandusministeeriumi bilansis olevate 
maaparandussusteemide koosseisu kuuluvad rajatised nagu lahtised 
ja kinnised veejuhtmed sillad truubid paisud tammid jt 
antakse maa tagastamisel voi asendamisel maaomanikule kellele 
kuuluval maal need paiknevad aktiga ule kui 
maaparandussusteemi hooldamiseks nahakse kaesoleva seaduse 
loike jargi ette uhistu antakse 
maaparandussusteem ule koos kohustusega astuda moodustatava 
uhistu liikmeks nende maksumus kustutatakse bilansist 
maaparanduse moju maa viljelusvaartusele arvestatakse maa 
maksustamishinna maaramisel 
pollumajandusministeeriumi bilansis olevate 
maaparandussusteemide koosseisu kuuluvad hooned nagu polder ja 
niisutuspumbajaamad koos nende teenindushoonete ja teedega voib 
maaparandusuhistule tasuta ule anda jagamatu vara moodustamiseks 
juhul kui neid ei jaeta riigi voi ei anta munitsipaalomandisse 
kooskolas kaesoleva seaduse paragrahviga 
riigi voi munitsipaalomandisse jaetav 
maaparandussusteem 
riigi omandisse jaetavate voi munitsipaalomandisse antavate 
maaparandussusteemide nimistu mis on kooskolastatud kohaliku 
omavalitsusega kinnitab vabariigi valitsus 
pollumajandusministri voi keskkonnaministri ettepanekul 
maaparandussusteemi praeguse valdaja 
kohustused 
kuni maaparandussusteemi uleandmiseni uuele omanikule 
haldab ja vastutab selle hoiu eest praegune valdaja vastavalt 
kaesolevale seadusele 
haldusoiguserikkumiste seadustiku taiendamine 
haldusoiguserikkumiste seadustiku 
loiget taiendatakse punktiga jargmises sonastuses 
pollumajandusministri poolt selleks volitatud 
maaparanduse jarelevalvetootajal kaesoleva seadustiku 
paragrahvides ja loetletud haldusoiguserikkumiste 
eest kui need on toime pandud maaparandussusteemidel 
riigikogu aseesimees 
tunne kelam 
 * 
German (names, including placenames) -- aachen adalbero adalbert adalbold adaldag adela adelheid aeddila 
aethelred aethelstan agapitus agius alberada albrecht alcuin aleman 
alsleben amelung amulrada andernach anselm ansfried aquilea aribo arn 
arneburg arnolf arnulf arpads ascania asselburg athelstan augsburg azela 
babenberg bacco balderich baldwin ballenstedt bamberg bardowick bavaria 
belecke berengar bernhard berthild berthold bibra billing billung birten 
blankenburg bohemia boleslas borghorst bote brandenburg bremen brun 
brunicho bruning brunshausen burchard burckhard burgscheidungen burkhard 
b"oddekken b"orde cambrai capetia carinthia carolingia cathwulf 
charlemagne chazar christina chrobry chutizi coledizi conrad constance 
daleminz dedi derlingau dietrich dodicho dornburg dortmund drogo 
dr"ubeck dyle eberhard edgar edith eila eilenburg einhard einsiedeln 
eisdorf ekbert ekkard ekkehard ekkehart elbe elten emma emmerem engern 
erfurt erich ermintrude ernst erwin eschwege esico essen ezzo fischbeck 
folcmar francia frankfurt fraxinetum freckenhorst frederick friderun 
frohse fulda gandersheim gebhard genbloux gerberga gerbstedt gernrode 
gero gesecke giebichenstein gisela giselbert giselher godehard godesti 
godila grone gross guntram gunzelin gutenswegen g"uther hadwig hainault 
halberstadt haldensleben hamburg harzburg hatheburg hathui hathumoda hed 
heeslingen heiningen helmarshausen helmburg helmern herford heribert 
herman hermann hersfeld hessengau hessi hezilo hildesheim hildiward 
hillersleben hilwartshausen hincmar hlawitschka hodo hohenaltheim 
hohenstaufen hrotsvitha ida ifriqiya ilsenburg imma immeding ingelheim 
ippo irmgard isingrim jonas judith kalbe karl karlmann katlenburg 
kaufungen kemnade kern kiev kinugunde koledizi kunigunde k"onigslutter 
laar lammspringe lampert lappenberg lech lenzen liudgard liudger liudolf 
liudolfing liutbert liutbirg liuthar liutizi lothar lotharingia 
luidprand luitpolding lusatia lutter l"uneburg magdeburg magyar mainz 
malacin marcswith marienthal mathilda maximin mecklenburg meginzo meibom 
meinwerk meissen memleben merseburg meschede metelen miesco mistui 
monreburg morawia mulde m"ollenbeck m"unster nadda neletice neuenheerse 
niederaltaich nienburg nordgau nordgermersleben nordhausen 
nordth"uringau northein nottuln obotria obotris oda odilo ohre 
oldenstadt olvenstedt osnabr"uck otto ottonia outremer paderborn passau 
pf"afer pf"afers pippin pl"otzkau pomarius poppo pr"um p"olde 
quedlinburg querfurt radbod ragenold ratold ravenna recknitz reepsholt 
regensburg reichenau reinhard reinhilde remi rheinfelden ricbert ricdag 
richenza richer rohr rothgard rottmersleben rudolf ruodlieb ruotger saal 
saalfeld salia salz salzburg sandersleben santersleben scapendal 
schakensleben schaumburg schildesche schwabengau schweinfurt sch"oningen 
sclavain sclavania selz serimunt shilluk siegfried sigebert sigebodo 
sophia stade stavelot steterburg st"otterlingenburg suabia suitger 
s"upplingenburg s"upplingenburger tagino tegernsee thangmar theodora 
theophanu thietmar thionville thuringia tribur tundersleben udalrich udo 
utrecht verden viking vitzenberg vreden walbeck walsrode walthard 
warburg wazo welf wendhausen wergild werla werner westfalia wetter 
wettin wettins wicfried wichmann widerad wimmelburg wipo wolfhere 
wolmirstedt wulfhard w"urzburg zeitz zeven zwentibold "odingen 
 * 
Gothic (words) -- atsaihwith armaion izwara ni taujan in andwairthja manne du 
saihwan im aitthau laun ni habaith fram attin thamma in himinam than 
haurnjais faura thus swaswe thai liutans taujand in gaqumthim jah garunsim 
mannam amen qitha izwis andnemun mizdon seina ith thuk witi hleidumei 
theina hwa taihswo sijai suns ahma ustauh authida kafarnaum afletandans seinana 
zaibaidaiu galithun usfilmans habands waldufni nazorenai mikilai hropjands 
stibnai usiddja us imma lohazzen fraujinon ibnassus skalkinassus juventutis 
gudjinassus siukei laggei diupei aggwitha milditha swiknitha fastubni lathons 
bigetun thana siukan skalk hailana as hilp meinaizos ungalaubeinais fraihna 
jah ik ainis waurdis rahnjan as triggwana mik rahnida wesunuh than garaihta 
ba andwairthja guths niu saiwala mais ist fodenai jah leik wastjom swegnida 
ahmin maht wesi auk thata balsan frabugjan managizo thau thija hunda skatte 
giban unledaim andstaurraidedun tho qath letith tho duhwe izai usthriutith 
thannu goth waurhta sinteino auk frisaht habands hailaize waurde thoei at 
mis hausides galaubeinai froathwai afwandidedun ganist gatilona pawlus 
apaustaulus teimauthaiau liubin barna ansts armaio awiliudo gamaudein 
andnimands thizozei skamai hwamma winna 
 * 
Hindi (female names) -- abhilasha achit aditi akriti akuti amita amrita anika anita anjali ankita 
anu anupama anuradha anvita aparajita aparna asha ashrita askini avani 
avantika bharati bina bindiya cauvery charu chhavvi chhaya chhaya chitra 
chitrangda daksha deena deepika deepti devaki divya dristi ekta firaki 
gargi gauri gayatri girija gita gitika gitanjali harshita hem hema hina 
indira ira ila jagrati jahnavi jaya jayani juana juhi jyoti jyotsna kajal 
kalpana kamakshi kamna kanchana katyayani kavita ketaki ketika kiran kirti 
kitu komal kriti kuhuk kshama kumud lata laxmi lalima lalita lolaksi madhavi 
madhu madhur madhuri mala malati malavika malini mallika malti maitryi 
mamta manavi manisha manjari manju manasi manushi marisa maruti matangi 
maya mayuri medha meena meenakshi meera meghana mena menaka mina mira mitali 
mohini naina nalini namrata nanda nandini nandita naomi narmada natasha 
neeharika neelam neena neerja neha nidhi nidra nilima nilini nimmi nira 
niral niradhara nirguna nirmala nirupa nirupama nisha nishtha nita niti 
nitu nitya nivedita parnika parnita parul pauravi pavani pavi payal paola 
palomi pallavi pamela phutika pivari pooja poonam prabha prachi prema premila 
prerana prisha pritha priti priya priyanka puja pundari punita purandhri 
purnima purva pusti rachna radha radhika rakhi ranjana rati reena rekha 
renuka revati riddhi rima rita ritu rohini roshni ruchi ruchira rudrani 
rukmini rupa rupali sachi sahana sanjna samiksha sandhya sangita sanyogita 
sanyukta sanjukta sapna sarasvati saravati sarika sarita sarmistha saryu 
saru sashti sashi sasthi satyavati saumya savarna savita savitri seema 
shaila shailaja shaina shalini shanta shanti sharda sharmila sharmistha 
sheetal shikha shilpa shivani shobha shobhna shradhdha shreya shruti shubha 
siddhi smirti smita sneh snigdha sobha sophia somatra sonali sonia sraddha 
sruti subhadra subhaga subhangi subhuja suchi suchitra sudevi sudha sujata 
suksma sumanna sumati sumitra sunita suniti sunrita supriya surabhi suravinda 
surotama suruchi surupa surya sushma susila susumna suvrata swati sweta 
tanu tanuja tapi tapti tara tarpana taruna tasha teji tejal tina trilochana 
trishna trupti trusha tuhina tulasi tusti udaya ujjwala uma urmila urvasi 
usha uttara vaisakhi vandana vandita vanita varsha varuni vasanta vasavi 
vasudhara vasuki vasumati vibhuti vidya vikriti vimala vinata vinaya vineeta 
vinita virini visala yaksha yamini yamura yauvani kartik kunal tushar paloma 
parul charu avatika ketaki kamayani neha mitali pallavi kitu saloni juhi 
prachi paola palomi richa rachna shilpa tanu tasha trishna sonal mehul 
sonali 
 * 
Hindi (male names) -- abhay abhijit achyuta aditya ajatashatru ajay ajit akaash alok amal amar 
amit amitabh amitava amol amrit amulya anand anant anay angada anil anirudhh 
ankur anniruddha anoop anshul anshuman arjun arun arvind ashok ashutosh 
ashwin ashwini asija aseem asuman asvathama asvin ashwini atharvan atmajyoti 
atul atulya avinash balram balavan balik bharat bhaskar bhavya bhim bhishma 
bhrigu bhudev bhuvan brij chandra chapal charan dahana daruka dattatreya 
deepak devarsi devesh dhananjay dharma dharmavira dharuna dhatri dhruv 
dilip dinesh dinkar divyesh duranjaya durjaya durmada dvimidha ekachakra 
eknath phalgun gagan gajendra gautam gaurav geet girish gopal gul harish 
harsh hemal hemant hitendra hitesh iravan jaidev jatin jayant jeevan jimuta 
jivana jitendra kailash kalidas kalpanath kamadev kamal kanak kapil kartik 
kartikeya kavi keshav ketan kirit kishore kripa kulvir kunal kusagra kush 
kushan lakshman lalit lokesh madan madhav madhusudhana mahabala mahavira 
mahesh maitreya manavendra mandhatri manik manish manoj manu markandeya 
matanga mehul mihir milind mohan mohit mukul mukunda nabendu nachiketa 
nachik nakul namdev nanda nandin narayana naresh narsi nartana naveen navin 
neel neeraj nihar nikhil nimai niraj niramitra niranjan nitesh nitya-sundara 
omarjeet pallab pandya pankaj paramartha partha piyush prabhakar pradeep 
pramath pramsu pranav prasata prashant prasoon prassana pravin prayag preetish 
prem prithu privrata pulkit pundarik puranjay purujit pusan puskara rahul 
raivata raj rajan rajeev rajesh rajiv rakesh ram raman ramanuja ranjan 
rantidev ravi ravindra rishi rohit roshan rupesh ruchir sachin sagar sahadev 
samir sampath samrat samudra sanat sandeep sandy sanjay sanjeev sanjiv 
sanjog sankara sapan santosh sarasvan sarat saswata satayu satrujit satyavrat 
satyen saunak saurabh senajit shailesh shalabh shantanu shankar sharad 
shashwat shailesh shishir shiv shvetank siddharth srikant srinath srinivas 
sriram sridhar subodh sudarshan sudesha sudeva sudhansu sudhir sugriva 
sukarman sukumar sumantu sumit sundara sunil suresh surya suvrata swagat 
taksa tarun tapan tapesh tarang tej tilak trisanu tushar udit upendra urjavaha 
uttam uttanka vairaja variya varun vasant vasava vasu vasudev vasuman vedanga 
veer vidvan vijay vikas vikram vikrant vimal vinay vineet vinod vipin vipul 
viraj virasana virat vishal visvajit visvakarman visvayu viswanath vivatma 
vivek waman yash yashodhara yashovarman yashpal yogendra yudhajit zahin 
zev 
 * 
Indonesian (words) -- permintaan rombongan tanaman tandatangan tandatangi kepalai daerah 
dagang hawa letak letih cape libur pengunungan berikut depan ruang 
tanda mata oleh oleh tetangga terus toma mencari bertamasnya macam 
bermacam menginap kelihatan tukar menukar keluar permili sifat pula 
potong panggil memanggil mengulang sapu tustel kodak balasan berjanji 
berguna berteman keduanya perjanjian sari kantor besi cemra dewasa ini 
langit manusia perang perang dunia permulaan perusahaan penerbangan 
rata selama tenaga dikenal daftar kata kata perbendaharaan daftar tata 
bahasa voltage tegangan disukai genap perkataan menyat sedangkan 
karikatur gambar pikir desa tengah lebih susah kayu terang indah berita 
jenggot karangan kuku lebih dahulu cap mengundang memas menjatuhkan 
mengadakan menjalankan membesarkan lepas melepaskan menerangkan tafsir 
menyewa menyewakan belanja belanjaan denda meninggalkan angotta giat 
kedatangan pekarangan undangan menjalankan mengalahkan putus memutuskan 
menepatkan kata ganti pembina kelakuan bertanya menanyakan apakah 
menggalakkan lazim sesungguhnya maka kiranya banyak sedikitnya hubungan 
telepon jarak juah pembinaan pelacur melacur pelacuran melindungi 
logaritma lengkap perlengkapan peralatan llham setuju kampus terhadap 
pemakaian menjadi basah menjadi soal perspektip susun hormat setuju 
polusi pormulir kekenan kenakan sesungguhnya jasa kedinasaan wartawan 
pula usaha beruasaha usah terjemahpn sewakan kelaparan ketakutan 
kedingingan kesakitan kehausan kepanasan menangap tangapan kelihatan 
kedengaran ketahuan acara rapat cat diam diam perjalanan penyakit 
perdana menteri selesai sunyi tenang terbuka terkejut tersenum menemani 
teman kampung look mencari jarang nyata menyatakan dapat diterima 
menghargakan menghargai bagi wartawan utama langsung segera seketika 
maksud mampu waras berbagai terlarang bahagi bahagian rakyat pilih 
memilih memekir tepat maksud penetuan melalui rampisrampang menentang 
sedar mara bahaya aneka asalkan biar derita masak ucapan memperkuat 
tahan penjualan ilmu alam telah sehingga diperluas izin jembatan 
kebangsaan kemegkinan mutu pinggir rela terbawa terbaca seluruh 
perhatian puas berharga terbaring kehilangan akar memeparkir sekolah 
tinggi usah maju hebat insenur bulu sempuuya adalah adanya adapun 
ijazah hebat pengalaman sambutan mengaku bertindak buatan simpang empat 
terkejut untung sabar al antara lain da dengx alamat hari ulang tahun 
pd perusahan dagang propinsi memancar stasiun pemancar memang sebarnya 
tersenyum setuju seketika asal bingung dirinya sendiri kanter 
kehilangan akal kemungkinan menerangkan terangkan menyakut sahut mula 
mulanya pendapat pertama tama rapat satu sama lain sadar seharusnya 
sewajarnya keras bermenung biasa ganas impian ketinggian lebih kurang 
menaksir taksir menepatkan tepatkan mengira kira me rusak berhasil 
rumit kerugian berikut hidupnya seperti roda pedati berhari hari baju 
sehari hari bangga benda berlainan bersusah payah berupa betah buruk 
celaknya ceroboh dangkal demikian ejekan guna hak halangangan hebat 
hemat kasar kasihan kebetuan kekayaan kelak penetuan kemajuan keras 
kepala kesalahan wymist kesukaran kewajiban wajib kuasa langsing kurus 
lantang keras makin malu lukai perasaan firasat membalas guna balas hiu 
kalah ujian mem butuhkan balas kejadian peristiwa memeliki miliki 
bertahan memper kan menambah tambah meng halangi titik persoalan 
pandangan telor menyayangi sayangi otak pendirian perbedaan prinsip 
rapi seolah-olah sombong tempak terasa tertentu tujuan watak 
berpendidikan berusaha hadirin imigran keamanan sangsi mengulang 
kemiskinan kepadatan peregko kesengsaraan lurus memelih pilih mencukupi 
cukupi mengemukan kemukakan mengingatkan ingatkan menjamin jamin 
pembicara penjelasan penegasan perbaikan pertahanan satu satunya jalan 
selanjutnya serupa sama satu sama lai sederhana semenjak sejak tanah 
air terutama untunglah beruntung bersedia menganjurkan jujur dengan 
jujur kejujuran pengertian pembukaan pembersihkan mengeluarkan bekas 
terlebih dahulu dahulu dulu mendidik pendidikan penerangan menerangkan 
berjuang perbuatan pertemuan perhatian keg ran persaudara kekvatan 
kedutaan tuhan kesatuan kehidupan ke maju an keadaan keinginan catatan 
jawatan kagum larangan lawan pajak beaya pesat rupanya sisi setia 
lambat perlahan-lahan kedua kursi malas ucap meng kan upacara yaitu 
masa muda kaum muda kecemburuan rasa cemburu tanda melihat-lihat sering 
datang berkirim kirim surat lebih lebih lama lama kelamaan jangan 
jangan satu-satunya mula mula mulanya serba serbi terus-menurus 
beribu-ribu beratus ratus setuju membiarkan selama sementara biar 
selesma koperasi cita cita-cita bukti membuktikan ternyata terbukti 
layak lelucon nyata sedapat-depatnya sementara melindungi bebas jangka 
panjang kosa kata daftar kata bergama jadi mensuruh cartu pos dengki 
asese bi bihi waduhai payah rupanya kepentingan gampang kalau begitu 
mampir turun mengajak perabot apa saja jawab baru kira mahasiswa 
melawat mengajar partekelir jadl dulu belakang kerja malas menarik 
sampai lumayan tempat tidur cuma kelambu antar lewat sambil nalarwajar 
masih ada bilang cetak manijir memang simpan carikan kan sibuk lain 
kali tadi kembalikan kembalinya berganti menggantikan supaya suruh 
aerogramme repot kan dengarkan tenang kenapa sik capai ikuti masa 
begitu kebudayaan pembudaayaan habis keadaan kebetulan apa saja pagi 
pagi repot repot tanam terang nalar wajar ber kiri kanan lewat lewati 
masih pasti yuk permulaan minat upama nya mempelajari kalakian jujur 
kikir rem runtuh tergatung pada meng izin kan men meloncat meng ukur 
mengatakan bahwa harta pilihx who barangsiapa tiap orang berkedua 
terbit berawan teduh teanxg e kok licin akibat ajak naik terbang cocok 
setuju sepakat kacang tanak sendiri temu bertemu harta kemungklnkan 
pabean pasti berpesan melanggar menyembunyikan sembunyi biro perjalanan 
daripada hari raya merasa bahwa mewah pada umumnya sehingga tempat 
tujuan ker pedati oto mobil sulit susah sukar suruh keputusan pengaruh 
purba muncil menemukan menjumpai timbal balik adat istiadat aneka warna 
ketenagangan pengertian sewaktu waktu suku bangsa giat bidang mampu 
gaya hidup kegiatx kqtipx kegem sepdjdg beristiraht umpama upama 
seumpamanya samping singkat ilmu cabang maumu ada mau ada jalan maupun 
pandang benda dipecahkan segi secara suku kehabisan ciri ciri khas 
kemaupun mampu bunga urus kenapa sih utama melihat menoleh kebelakang 
berdasar terbuat dari suku erat gotong royong kawln paksa keluarga luas 
menyangkut tanggung jawab terhina penumbang duka membikin sisa nambat 
tambat melakukan dosa demikian pindah bunyi agar secara mekamar bagi 
membolehkan handai taulan menanggu menentukan merayakan nait sorga 
terlebih dahulu langsung selenggarakan katakerja berbohong sangkut 
bersangku gadis perayaan nyata terang jahat ke jahatan menonton 
kesempatan laksana melaksanakan cocok terutuama kesempatan baik 
opportunitas empuk terber gantung pada lucu sebut ditetap pihak hitung 
belah pertemuan cinta itu buta kegila gilaan sayang istilah bersatu 
menyatukan kipas angin meletakkan bahan bahan campuran jenis latihan 
senter tamat halal terpaksa keliling malang cuma anu keseratan memasak 
khusus sasaran sarjana bahasa terletak pendapat terpencil damai 
berdamai damping berdamping dengan tanggap cermat kemiripan landasan 
menarik perhatian meyangkan pada dasarnya rangka secara bertahap tahap 
mencampuran tergolong rikit busut ly ber kok daerah panas tinggal ulang 
lemari besi tas ijazah dikeluarkan berkat perak satuan surat menurat 
terlampir permohon yang paling bon kasir juro bayar nawar tawar 
bagaimana bisal buat manis ya sudah dosen sama sama semacam becak nih 
aman mudah pandai koran bahas membantah meminjam menggacau mengancam 
mengatur meng pacar tentang menilpun seadanya memeriksa memesan menemui 
mengantar menggantikan pondokan siap katasifat burung halus murah 
perpisahan pulang pergi di samping dalam hal ini dalam hal itu disia 
siakan sehingga sebetulnya usah sumpit jangan ada apa apa salahnya 
beres bilamana kenapa masehi menemukan menyedihkan pedagang serempat 
singgah membunuh pertolongan bau berkawan berteman sehingga barat 
menghafalkan menghafal mengunjung menjumpai teliti terlarang 
warganegara bisa saja nonton bioskop bangun aljabar antre kulit 
penghabisan justru kontrak sekarang juga leding air leding maksud saya 
pun rebus sumur pemandingan buat sama bungkus di kiri kanan pokoknya 
montir gaji darah suntik daftar harganya menyaki 
 * 
Italian (words) -- abbastanza acqua adige aeroplani aiuola alba alfabetico ali alla allegro allo 
allora alta alto altro altrui ama amici amico ammirazione amore analisi anche 
ancora andarci anderei andiamo andrei anello angela angelo animale anime anna 
annegherei anni annina anno annoio antonio apparecchiare apparecchiare apri 
arderei argentati aria arpa arrugginito ascolti aspre astuta attenti 
attenzione attestato auguri aure autunno avanti averne avervi avevano avrei 
bacchetta baci balli basso bastone battaglia battone bella bello bene 
benemerenza bere bevanda bevendo bianca biancamaria bimba bipolare blu bocca 
boccetta boschi bottiglia braccio bruciare brugola bugia bullone busso caca 
cacano cacare cacate cacato cacca cachi cachiamo caco cadaveri caduto 
calcolatore calore calzoni camicia camminare campo cannelloni cannoni cantare 
cantico canzoni capelloni capirai capo cara carrozza carrozzone cartone casa 
casini cattura cecco centanni cento cerca certamente certo cervello cesso 
cetrioli che chiave chissa chitarra chiusa chiuso ciclone cicoria cielo cinque 
cinquecento citta claudia clivi colli comari come compagnia compatti compriamo 
con confusione conquistarmi consegnereste continuare coraggio cornuto coro 
corona corpo corporale corporeo corrente correre corri cosa cosi crepare 
cristiani cristiano cristo cui culo dado damiano dardo davano decenza decimo 
definizione deflettente dei del delle deserto devono dice dicesti diciannove 
diciassette diciotto dico dicono dieci dio diodo dipinto diro disgraziato 
disse distanti ditta divieto dizionario dodici dolci dolore donata donne dopo 
dorate dorma dormendo dormi dormire dove dovere drago dritto due duro echeggia 
editore edotta effetto energia era eredita essere estate esterrefatto faccio 
fai famiglia fanno fare fatta fatto felice ferrari festa fidare fila 
filettatura fine fino fiordalisi firenze fiume foglie fondo forse fortuna 
forza fossi francesca franco fredda frega fresca fretta frittata fronte frose 
fuggi fuggifuggi fuggirei fulmine fumando fumare fumo fuoco furono fusilli 
fuso gabbia galleggia galleggiante galleggiare gatti gattino gatto gelosi 
generazione generazioni gente germania gia giaguari giardinetta giardiniere 
giardino gino gioco giocondo gioia gioie giornata giorno giovani giovanni giro 
giunzione gli gorilla gramo grande grano grossa guadagnarmi guai guanti guarda 
guardava guardavano guardi guccini guerre hanno imbrigherei imbucherebbe 
immaginabile imparato imperatore impiego incantato indiano indietro indirizzo 
indivia inferno infinita infinite innamorare innamorarsi inno innocente 
innocenza innovativo innovato insalata inseparabile insieme insudiciarsi 
inverno irrisolvibile laggiu lago laide lasserei lapponi lasagne lascerei 
lascio lassu latrina lattuga lavorare lazio leggerei leggiadre leonesse leoni 
levo liguria linci liquefatto lista locomotiva locomotore lombardia lontano 
lucci lui luna lungo luogo maccheroni madre maggio magico mai maiale male 
maleditemi manca mandata manderei mani maniera mantello mara marco marinella 
mario matematica matematico matrimonio mattina mela membra mentre meravigliato 
merda messi mestiere mezzo mia migliori mille miseria mistero modo molli molto 
momento mondo montagne montanara morte mozzerei napoli natale nato nei nel 
nella nessuno niente ninetta noi nome nomi non nono notte nove nuova occhi 
occhio oggi ogni olezzan ombra ora orca ordine oro orologio orto ottavo otto 
ove padre padrone pagano palazzone palimba palla paninari pantaloni pantere 
papa papaveri paradiso pare parecchie parecchio parlami parlava parola parole 
partito passa passata passate pasta pasta pelle pensiero pentirai per per per 
perche perche perse personaggi persone piatto piazza pieni pieni pioggia 
pippero pistole pitoni piu poco poi polacco pomodori popolazione porta portati 
portato porto possibile posso posta potere potrebbe poveretto pozione 
precipita precipitevolissimevolmente precisare predichi preferito premere 
prende preso primavera primo principe principessa problema problematico 
problemi profondo programma pronto protettori provaci provato provetta 
provetti pudore pulirsi pulisce punto pure puttana quadrato quando quando 
quanta quarantadue quarantaquattro quarto quattordici quattro quello quello 
questo questo questoggi quindici quinto quota ragazzi ragioniere ramazzotti 
reazione regole rendano rendita repubblicano respira resto rettangolo ribelle 
ricordo riempiamo rigatoni rione ripetizioni riposa riposa riposano riposare 
riposare riposarsi riposi riposo risveglio ritornavi ritrovarsi roberta rombo 
romeo rosa rossi rotaie ruolo ruota rupi ruscello sai sale sapra saro sassari 
sbando scappare scarola scarpe scende schermo schianto schiavo schifosi 
scimmia sciolto scivolavi scivolo scodella scontro scordare scorta scrivendo 
scriveresti secchio secondo sedici sei sembra senso sente senza senza sepolto 
serena serve sessantasei sesto sette settimana sfilavano sfuggenti siamo siamo 
silenzio silenziosa similmente singolare situazione smisteremmo societa sogna 
sognante sognare sognato sognatore sogno sola soldati sole sole sole solo 
soltanto solubile sono sono sono sono sopra sopra soprattutto sorcini sorrisi 
sotto spaghetti spara spariva sparsi specializzata speranza spode sporcarsi 
sta stadera stagionato stagioni stanco stanza stare stella stelle stessi 
stitici stivali sto storia straccerebbero strana stranamente stronzo stupido 
stupisca stupisci stupito sulla sulla sulle suoi suona svasata svegli sveglia 
svegliamo svegliano svegliare svegliarsi svegliate sveglio tanto tanto tanto 
tappato tasca tavolo tazza telefono tempesta tempesterei tempi tempo tenente 
teorema tepidi terra terzo tesi testamento tetto tondo torrei torrente 
torrente toscana tra tramontate transistore trasforma tratto tre tredici 
tremano treno trenta trentino troia troppo tulipano tuoi tutte tutti umana una 
una una undici unico uno uscendo vaffanculo vagoni vai valli vecchie vedere 
vedere vedi vedrai vedrai veduto veglia veloce velocita vendete venduto veneto 
venti venticinque ventidue ventinove ventiquattro ventisei ventisette ventitre 
vento ventotto ventuno vera verbi verga verginita vetro vetroresina via vide 
villanella vincere vincero virgola vita vite vivaldi vivesti viveva volando 
volare volavo volpe volta volte vuole vuota zingari 
 * 
Japanese (female names) -- akazome akiko ayame chika chizu cho fuji hamako hana hanazono hiromusi 
hisae hisayo imako inoe ishi izuko jun kagami kame-hime kameko kaneko 
kawa kawanomu keiko kenshi kiku kimiko kogin kogo komachi kozakura 
kumiko kusuriko machi mariko masago masako masuko matsukaze midori 
mineko miwa miyako miyoko mura nari ochobo oki onshi reiko renshi rin 
ruri sachi sadako sakura seki sen-hime senshi setsuko shikibu shina 
shizue shizuyo siki sugi taka takara tamako teika tokiwa tokuko tomoe 
towika tsukinoyo umeko umeno wakana yasuko yoshiko yukinoyo yukio 
 * 
Japanese (male names) -- agatamori akimitsu akira arinori azumabito bakin benkei buntaro 
chikafusa chikayo chomei chuemon dosan emishi emon fuhito fujifusa 
fujitaka fususaki gekkai gennai gidayu gongoro hakatoko hamanari 
haruhisa hideharu hideo hidetanda hideyoshi hirohito hirotsugu 
hitomaru iemitsu ienobu ieyasu ieyoshi imoko issai iwao iwazumi jikkyo 
jozen junkei jussai kageharu kagemasa kagemusha kahei kanemitsu 
katsumi katsuyori kazan kazunori keisuke kintaro kiyomori kiyosuke 
kmako komaro koremasa koreyasu kuronushi kyuso mabuchi maro masahide 
masamitsu michifusa mitsukane miyamoto mochiyo morinaga munetaka 
murashige nagafusa nagate nakahira nambo naoshige narihiro oguromaro 
okimoto okura omaro otondo razan rikyu rokuemon ryokai sadakata 
sanehira sanetomo sanzo saru shigenobu shigeuji shingen shoetsu shozen 
sukemasa tadabumi tadashiro takatoshi tameyori taneo taneyoshi tensui 
togama tomomasa toshifusa toyonari tsunayoshi tsunetane uchimaro 
ujihiro umakai watamaro yakamochi yasumori yoriie yoritomo yoshiie 
yoshisune yoshitane yoshizumi yukihira zuiken 
 * 
Jaqaru (words from a language related to Aymara) -- maykutxiqawanwanmnahatsknamaywaychwaqa atquqaishpawatahatskirhaatquqa 
saliwata waychawqa kumpari napsa humhamawa hats munkt"a shimingats'ukt'ma 
waychaw kumpariqa hawilli sashuqa ts'uk qallyawi may awant quqrunqa usk"aywata 
lyutap"a ukshachamna atquqa palsh munawi waychawqa waychwaqa hahptushuqa mawi 
may ahts qalsanaru atquqa walyushuqa qalsanruqa wishk"ang sawi yangqanya 
yangqanya naraykatapna shaykshushuqa matchewq"i waychaw hanatsq"anushptaki 
shimip"ats'ukt'iri waychwaqa hanatsq"ushuqa maynatsk"a ts'uk qallyawq"i ts'uk 
ushuqa sawi atuq kumparip"aqa wala akshaqanaham hatsmata ukshachamna atquqa 
qallyawi waychaw waychaw sashu ants wanwana hatskiri ishpushuqa 
p"inyaptawi tinkuwiushutxa kumparpshqa ukshachamna sawi antskaswa k"uwa 
tukki wala upasut"a haptamata ushaqa mawi imk"itxushu imk"itxushu 
 * 
Kakadu (names from a Northern Australian language) -- Kulingepu Kunamullajumbo Mukalakki Ningeniu Mupplebara Mariapaleingum 
Murrakoeri Yokorakorida Munmona Murrakumora Noroma Lala Kinmorko Karawa 
Mudanga Yerii Mindapul Kutoru Ukairi Muduarbulu Ngarukorda Unkorpu Ungara 
Arai Chilongogo Areyi Muriwallauwill Naroma Namijeya Apperakul Miniamaka 
Numerialmak Nulwoiyu Niyaudadijeri Murrapurnmini Yarrawaiika Kumerakan 
Maraonbi Wareiya Montoquialla Mangul Kumerangbukara Kudauu Ungmerierigari 
Buruwongu Umerumparengi Ulloa Nolerupungeini Tjeroboilu Muriiwapungen 
Mudingeyia Korominjil Mitchinga Alumberapa Ungara Obaiya Monmuna Murawillawill 
Mukalakki Mitjunga Numerialmak Kumbainba Miniamaka Murrapurnminni Naminjeya 
Mitjeralak Mitiunga Kopereik Oogutjali Belgramma Tjurabego Mikgeirne 
Mirowargo Minagi Mukarula 
 * 
Latin (words) -- si quid est in me ingeni judices quod sentio quam sit exiguum aut si 
qua exercitatio dicendi in qua me non infitior mediocriter esse 
versatum aut si hujusce rei ratio aliqua ab optimarum artium studiis ac 
disciplina profecta a qua ego nullum confiteor aetatis meae tempus 
abhorruisse earum rerum omnium vel in primis hic a licinius fructum a 
me repetere prope suo jure debet nam quoad longissime potest mens mea 
respicere spatium praeteriti temporis et pueritiae memoriam recordari 
ultimam inde usque repetens hunc video mihi principem et ad 
suscipiendam et ad ingrediendam rationem horum studiorum exstitisse 
quod si haec vox hujus hortatu praeceptisque conformata non nullis 
aliquando saluti fuit a quo id accepimus quo ceteris opitulari et alios 
servare possemus huic profecto ipsi quantum est situm in nobis et opem 
et salutem ferre debemus ac ne quis a nobis hoc ita dici forte miretur 
quod alia quaedam in hoc facultas sit ingeni neque haec dicendi ratio 
aut disciplina ne nos quidem huic uni studio penitus umquam dediti 
fuimus etenim omnes artes quae ad humanitatem pertinent habent quoddam 
commune vinculum et quasi cognatione quadam inter se continentur sed ne 
cui vestrum mirum esse videatur me in quaestione legitima et in judicio 
publico cum res agatur apud praetorem populi romani lectissimum virum 
et apud severissimos judices tanto conventu hominum ac frequentia hoc 
uti genere dicendi quod non modo a consuetudine judiciorum verum etiam 
a forensi sermone abhorreat quaeso a vobis ut in hac causa mihi detis 
hanc veniam adcommodatam huic reo vobis (quem ad modum spero) non 
molestam ut me pro summo poeta atque eruditissimo homine dicentem hoc 
concursu hominum literatissimorum hac vestra humanitate hoc denique 
praetore exercente judicium patiamini de studiis humanitatis ac 
litterarum paulo loqui liberius et in ejus modi persona quae propter 
otium ac studium minime in judiciis periculisque tractata est uti prope 
novo quodam et inusitato genere dicendi quod si mihi a vobis tribui 
concedique sentiam perficiam profecto ut hunc a licinium non modo non 
segregandum cum sit civis a numero civium verum etiam si non esset 
putetis asciscendum fuisse nam ut primum ex pueris excessit archias 
atque ab eis artibus quibus aetas puerilis ad humanitatem informari 
solet se ad scribendi studium contulit primum antiochiae nam ibi natus 
est loco nobili celebri quondam urbe et copiosa atque eruditissimis 
hominibus liberalissimisque studiis adfluenti celeriter antecellere 
omnibus ingeni gloria contigit post in ceteris asiae patibus cunctaeque 
graeciae sic ejus adventus celebrabantur ut famam ingeni exspectatio 
hominis exspectationem ipsius adventus admiratioque superaret erat 
italia tunc plena graecarum artium ac disciplinarum studiaque haec et 
in latio vehementius tum colebantur quam nunc eisdem in oppidis et hic 
romae propter tranquillitatem rei publicae non neglegebantur itaque 
hunc et tarentini et regini et neopolitani civitate ceterisque praemiis 
donarunt et omnes qui aliquid de ingeniis poterant judicare cognitione 
atque hospitio dignum existimarunt hac tanta celebritate famae cum 
esset jam absentibus notus romam venit mario consule et catulo nactus 
est primum consules eos quorum alter res ad scribendum maximas alter 
cum res gestas tum etiam studium atque auris adhibere posset statim 
luculli cum praetextatus etiam tum archias esset eum domum suam 
receperunt sic etiam hoc non solum ingeni ac litterarum verum etiam 
naturae atque virtutis ut domus quae hujus adulescentiae prima fuit 
eadem esset familiarissima senectuti erat temporibus illis jucundus 
metello illi numidico et ejus pio filio audiebatur a m aemilio vivebat 
cum q catulo et patre et filio a l crasso colebatur lucullos vero et 
drusum et octavios et catonem et totam hortensiorum domum devinctam 
consuetudine cum teneret adficiebatur summo honore quod eum non solum 
colebant qui aliquid percipere atque audire studebant verum etiam si 
qui forte simulabant interim satis longo intervallo cum esset cum m 
lucullo in siciliam profectus et cum ex ea provincia cum eodem lucullo 
decederet venit heracliam quae cum esset civitas aequissimo jure ac 
foedere ascribi se in eam civitatem voluit idque cum ipse per se dignus 
putaretur tum auctoritate et gratia luculli ab heracliensibus 
impetravit data est civitas silvani lege et carbonis ``si qui 
foederatis civitatibus ascripti fuissent si tum cum lex ferebatur in 
italia domicilium habuissent et si sexaginta diebus apud praetorem 
essent professi'' cum hic domicilium romae multos jam annos haberet 
professus est apud praetorem q metellum familiarissimum suum si nihil 
aliud nisi de civitate ac lege dicimus nihil dico amplius causa dicta 
est quid enim horum infirmari grati potest heracliaene esse tum 
ascriptum negabis adest vir summa auctoritate et religione et fide m 
lucullus qui se non opinari sed scire non audisse sed vidisse non 
interfuisse sed egisse dicit adsunt heraclienses legati nobilissimi 
homines hujus judici causa cum mandatis et cum publico testimonio 
venerunt qui hunc ascriptum heracliensem dicunt his tu tabulas 
desideras heracliensium publicas quas italico bello incenso tabulario 
interisse scimus omnis est ridiculum ad ea quae habemus nihil dicere 
quaerere quae habere non possumus et de hominum memoria tacere 
litterarum memoriam flagitare et cum habeas amplissimi viri religionem 
integerrimi municipi jus jurandum fidemque ea quae depravari nullo modo 
possunt repudiare tabulas quas idem dicis solere corrumpi desiderare an 
domicilium romae non habuit is qui tot annis ante civitatem datam sedem 
omnium rerum ac fortunarum suarum romae conlocavit at non est professus 
immo vero eis tabulis professus quae solae ex illa professione 
conlegioque praetorum obtinent publicarum tabularum auctoritatem nam 
cum appi tabulae neglegentius adservatae dicerentur gabini quam diu 
incolumis fuit levitas post damnationem calamitas omnem tabularum fidem 
resignasset metellus homo sanctissimus modestissimusque omnium tanta 
diligentia fuit ut ad l lentulum praetorem et ad judices venerit et 
unius nominis litura se commotum esse dixerit in his igitur tabulis 
nullam lituram in nomine a licini videtis quae cum ita sunt quid est 
quod de ejus civitate dubitetis praesertim cum aliis quoque in 
civitatibus fuerit ascriptus etenim cum mediocribus multis et aut nulla 
aut humili aliqua arte praeditis gratuito civitatem in graecia homines 
impertiebant reginos credo aut locrensis aut neapolitanos aut 
tarentinos quod scenicis artificibus largiri solebant id huic summa 
ingeni praedito gloria noluisse quid cum ceteri non modo post civitatem 
datam sed etiam post legem papiam aliquo modo in eorum municipiorum 
tabulas inrepserunt hic qui ne utitur quidem illis in quibus est 
scriptus quod semper se heracliensem esse voluit reicietur census 
nostros requiris scilicet est enim obscurum proximis censoribus hunc 
cum clarissimo imperatore l lucullo apud exercitum fuisse superioribus 
cum eodem quaestore fuisse in asia primis julio et crasso nullam populi 
partem esse censam sed quoniam census non jus civitatis confirmat ac 
tantum modo indicat eum qui sit census ita se jam tum gessisse pro cive 
eis temporibus quibus tu criminaris ne ipsius quidem judicio in civium 
romanorum jure esse versatum et testamentum saepe fecit nostris legibus 
et adiit hereditates civium romanorum et in beneficiis ad aerarium 
delatus est a l lucullo pro consule quaere argumenta si qua potes 
numquam enim his neque suo neque amicorum judicio revincetur quaeres a 
nobis grati cur tanto opere hoc homine delectemur quia suppeditat nobis 
ubi et animus ex hoc forensi strepitu reficiatur et aures convicio 
defessae conquiescant an tu existimas aut suppetere nobis posse quod 
cotidie dicamus in tanta varietate rerum nisi animos nostros doctrina 
excolamus aut ferre animos tantam posse contentionem nisi eos doctrina 
eadem relaxemus ego vero fateor me his studiis esse deditum ceteros 
pudeat si qui se ita litteris abdiderunt ut nihil possint ex eis neque 
ad communem adferre fructum neque in aspectum lucemque proferre me 
autem quid pudeat qui tot annos ita vivo judices ut a nullius umquam me 
tempore aut commodo aut otium meum abstraxerit aut voluptas avocarit 
aut denique somnus retardit qua re quis tandem me reprehendat aut quis 
mihi jure suscenseat si quantum ceteris ad suas res obeundas quantum ad 
festos dies ludorum celebrandos quantum ad alias voluptates et ad ipsam 
requiem animi et corporis conceditur temporum quantum alii tribuunt 
tempestivis conviviis quantum denique alveolo quantum pilae tantum mihi 
 * 
Latvian (female names) -- solvita spodra ilva spulga arnita rota ivanda tatjana smaida franciska 
aira felicita lidija lida agnese agnija agne grieta ksenija ilze ildze 
izolde tIna valentIna pArsla tekla violeta brigita indra spIdola aIda 
ida daila veronika agate selga silga dArta dace dora nelda aldona simona 
apologija paulIna paula laima laimdota karlIna lIna malda melita jUlija 
dZuljeta konstance kora kintija zane zuzanna smuidra eleonora Arija 
rigonda diAna dina alma evelIna mEtra lIvija lIva andra skaidrIte lavIze 
luIze laila alise auce austra aurora ella elmIra dagmAra marga margita 
silvija laimrota liliAna agita aija aiva matilde ulrika amilda amalda 
GertrUde gerda ilona adelIna made irbe una dzelme tamAra dziedra mirdza 
Zanete mAra mArIte marita eiZenija Zenija gunta ginda gunda agija nanija 
dagne irmgarde daira valda herta vija vidaga zinta zina zinaIda danute 
dana valErija anita anitra zIle strauja gudrIte aelita viviAna laura 
jadviga vEsma fanija mirta ziedIte anastasija armanda nameda lIksma 
bArbala alIna tAle raimonda gundega terEze laine lilija liAna vizbulIte 
viola henriete jete stefAnija maija milda karmena valija inAra ina elfa 
sofija taiga arita inese lita sibilla teika venta ernestIne emIlija 
leontIne lonija ligija ilvija marlEna ziedone dzidra gunita lolita alIda 
jUsma biruta mairita lIba emma inta ineta elfrIda sintija ingrIda gaida 
frIda mundra ligita gita malva nora ija uva tija saiva baNuta ZermEna 
vilija justIne juta rasma rasa maira egita ludmila lIga milija maiga 
ausma inguna malvIne kitija lauma benita everita edIte esmeralda alda 
maruta antra adele ada zaiga asne lija olIvija leonora margrieta 
margarita egija hermIne rozAlija roze jautrIte kamila digna ritma ramona 
melisa meldra marija marika marina magda magone kristIne krista 
kristiAna anna ance annija marta dita cecIlija cilda rUta ruta angelika 
sigita albIna aisma mudIte madara genoveva inuta olga zita liega vizma 
klAra elvIra velga zelma zenta dzelde astra astrIda vineta liene helEna 
melAnija imanta janIna linda rudIte ivonna natAlija broNislava auguste 
guste aiga jolanta vilma iluta elIza lizete zete berta bella dzintra 
klaudija regIna ilga telma jausma albertIne signe signija erna evita eva 
iza izabella sanita santa sanda sandra asja asnate vera vaira liesma 
elita guntra marianna vanda veneranda agrita svetlana lana elma zanda 
lAsma ilma elza modra amAlija monika zilgma aina anete elga helga monta 
tince silva kira irma vilhelmIne minna eda hedviga daiga karIna elIna 
drosma urzula Irisa irIda daina renAte modrIte beAte beatrise amanda 
kaiva lilita irita Nina ninona antoNina elva nadIna ulla vivita dzIle 
Erika dagnija Sarlote lote helma aleksandra agra nellija kaija kornElija 
jevgeNija undIne banga glorija uga doloresa elizabete liza anda andIna 
zeltIte zigrIda zigfrIda velta katrIna kate rita vita olita meta sniedze 
evija raita jogita baiba barbara barba sabIne sarma antonija anta 
dzirkstIte sarmIte guna judIte otIlija iveta lUcija veldze gaisma 
johanna jana alvIne hilda teiksma klinta lelde arta minjona saulcerIte 
viktorija balva ieva stella larisa dainuvIte gija inita inga ivita 
solveiga ilgona 
 * 
Latvian (male names) -- laimnesis indulis ivo miervaldis ringolds sImanis zintis zigmArs juliAns 
gatis kaspars aksels reinis reinholds harijs Arijs Aris roberts raitis 
fElikss tenis antons antis andulis alnis oLGerts aLGirts austris kriSs 
zigurds sigurds ansis agnis kArlis spodris aivars valErijs rihards 
valentIns alvis olafs aloizs donats vitauts haralds almants dins justs 
adrians ivars ilgvars tAlis marts vents centis Evalds konstantIns aivis 
ernests balvis guntis guntars jAzeps benedikts kazimirs gusts gustavs 
tAlrIts ilgmArs gvido atvars dagnis dairis vIlips filips helmuts edgars 
hermanis vilmArs jUlijs ainis egils nauris gastons mintauts alfs rUdolfs 
marGers armands jurGis juris georgs visvaldis sandris rUsiNS vilnis 
raimonds ziedonis zigmunds gints uvis Girts Gederts gaidis didzis 
henrijs staNislavs klAvs einArs ervIns inArs kriSjAnis edvIns edijs 
herberts dailis inesis Eriks salvis ingmArs anSlavs edvards eduards 
varis vilis vilhelms maksis raivis vitolds igors margots ardis arnis 
frIdis anatols ingus mairis vidvuds zigfrIds ainArs sentis artUrs artis 
alberts madis viktors nils emIls monvIds laimdots jAnis inguns malvis 
viesturs pEteris pAvils pauls tAlivaldis mareks imants ingars intars 
ilvars uldis sandis andZs anrijs arkAdijs svens indriKis ints oskars 
ritvars egons egmonts aleksis aleksejs kristiAns jEkabs edmunds edZus 
valters renArs albIns normunds stefans augusts romAns romualds osvalds 
arvils askolds alfrEds madars vladislavs brencis zemgus oLegs bernhards 
boriss vitAlijs ralfs bErtulis boLeslavs ludvigs ludis broNislavs Zanis 
jorens armIns vismants alvis aigars ilmArs dzintars maigonis magnuss 
bruno gunvaldis vairis verners muntis modris matIss mAris maigurs agris 
rodrigo rauls gundars kurts knuts Adolfs ilgonis sergejs miKelis mikus 
mihails francis daumants druvvaldis elgars arvIds arvis druvis valfrIds 
gaits rolands ronalds drosmis leonIds severIns dainis laimonis elvijs 
elvis valts rinalds ikars atis otomArs oto linards leons leo leonards 
lotArs teodors mArtiNS mArcis ojArs eiZens jevgeNijs fricis vikentijs 
leopolds hugo uGis aleksandrs andis alfons konrAds lauris norberts 
ignats andrievs andrejs andris arnolds klaudijs nikolajs niklAvs gunArs 
vladimirs voldemArs valdis auseklis krists kristaps sarmis toms 
saulvedis Adams elmArs dAvids dAvis silvestrs kalvis 
 * 
Malay (words) -- salam buat pemudapemudi malaysia dari jakarta indonesia mudahmudah 
melalui media ini kita dapat menjalin persahabatan yang erat di antara 
dua negara dan budaya rudi triatmono saya cuba menghubungi berita 
harian di tetapi malangnya mel saya dikembalikan jika saudara ada 
apaapa cadangan bagaimana saya dapat hubungi editor bh sila beritahu 
saya hari jumaat akan datang iaitu hari jumaat terakhir pada bulan 
adalah hari belanjawan lazimnya ucapan belanjawan oleh menteri kewangan 
akan disiarkan oleh televisyen secara langsung biasanya ucapan 
belanjawan bermula pada jam petang laporan ekonomi dan ucapan 
belanjawan boleh dibeli di blok kompleks kewangan dan percetakan 
nasional dengan harga senaskah tahun lepas saya masuk irc bercerita 
tentang belanjawan dalam channel warung kalau tak silap tahun ni kalau 
sesiapa nak tahu inti belanjawan bolehlah cubacuba masuk irc tak tahu 
lagi channel mana nak masuk kalau boleh janganlah ada bot mengganggu 
kepada pembaca yang sebangsa dengan saya saya minta janganlah kita 
bergaduh sesama sendiri sebaliknya kalau nak gaduh gaduhlah dengan 
mereka yang cuba menjatuhkan kita saya dah tak kuasa nak gaduhgaduh ni 
sekian dan selamat menunggu belanjawan sepuluh wasiat dari apabila 
mendengar azan maka bangunlah sembahyang sertamerta walaubagaimana 
keadaan sekalipun bacalah tatapilah bukubuku ilmu pergilah ke majlis 
majlis ilmu dan amalkanlah zikrullah dan janganlah membuang masa dalam 
perkara yang tiada memberi faedah berusahalah untuk bertutur dalam 
bahasa arab kerana bahasa arab yang betul itu adalah satusatunya syiar 
janganlah bertengkar dalam apaapa perkara sekalipun kerana pertengkaran 
yang kosong tiada memberi apaapa jua kebaikan janganlah banyak ketawa 
kerana hati yang sentiasa berhubung dengan itu sentiasa tenang lagi 
tenteram janganlah banyak bergurau kerana umat yang sedang berjuang itu 
tidak mengerti melainkan bersungguhsungguh dalam setiap perkara 
janganlah bercakap lebih nyaring daripada kadar yang dikehendaki oleh 
pendengar kerana percakapan yang nyaring itu adalah suatu resmi yang 
siasia malah menyakiti hati orang jauhilah daripada mengumpatumpat 
peribadi orang mengecam pertubuhanpertubuhan dan janganlah bercakap 
melainkan apa apa yang memberi kebajikan berkenalkenalanlah dengan 
setiap muslimin yang ditemui kerana asas gerakan dakwah kita ialah 
berkenalkenalan dan berkasihsayang kewajipankewajipan kita lebih banyak 
daripada masa yang ada pada kita oleh itu gunakanlah masa dengan 
sebaikbaiknya dan ringkas kanlah perlaksanaannya bacalah renungilah 
kemudian amalkanlah kebijaksanaan dan kasih sayang tunjang kejayaan 
dakwah sebelum menyangkal dakwaan yang tidak berasas oleh orientalis 
barat saya mengajak anda memerhatikan beberapa ayat pertama dalam ayat 
ini menekankan manusia agar membaca dan menjadikannya sebagai kunci 
ilmu pengetahuan berikutnya diturunkan di dalamnya bersumpah dengan 
pena yang melambangkan ilmu kemudian membantah tuduhan orang kafir yang 
tidak berasas terhadap nabi muhamad ayat dalam dua yang mula diturunkan 
ini menarik perhatian bahawa agama dan perutusan nabi adalah 
berteraskan ilmu yang betul bukannya taksub dan pemahaman yang sempit 
suatu hakikat yang diakui bahawa kesejagatan dibuktikan oleh tokoh dan 
pemikir yang ulung mereka membuktikan kebenaran dan kesejagatannya 
melalui ilmu yang benar dan alsahih pemikiran yang betul sesetengah 
pendakwah melakukan kesilapan apabila mengemukakan kepada umum 
diperlihatkan keruh kerana tidak berjaya memperkenalkannya melalui cara 
yang betul sama ada pada penghayatan dan hadis atau penyesuaiannya 
dengan suasana dan kehendak setempat perbezaan tafsiran dan pandangan 
ulama dan harus dilihat dari sudut yang positif supaya ia memberi 
kekuatan bukannya negatif yang melemahkan umat sumbangan ulama silam 
adalah besar manfaatnya jika dipelajari dengan baik tetapi sayang 
sesetengah pendakwah gemar melihat perbezaan pendapat ulama sebagai 
faktor kemudaratan di tengahtengah perbezaan pendapat dan ikhtilaf 
pendakwah harus bijak memilih dan mempertimbangkan sesuatu terutama 
ketika menangani isu yang menyentuh keterbukaan dalam dakwah dan akidah 
syeikh dalam beberapa siri ceramahnya dalam ulama malaysia di pulau 
pinang ogos lalu menyatakan beliau lebih suka merujuk setiap isu yang 
berbangkit kepada dan kemudian merujuk kepada pemikiran ulama bagaimana 
mereka menyesuaikannya dengan zaman mereka ini kerana perubahan zaman 
masa dan tempat membawa perubahan kepada pendekatan kefahaman dan 
penekanan dalam menyampaikan mesej dakwah kepada jalan sebagai contoh 
andainya saya dijemput memberi pandangan dalam satu persidangan untuk 
membincang hak asasi manusia saya pasti mengemukakan pandangan mazhab 
syafii yang menyatakan orang harus atas kesalahan membunuh orang bukan 
saya terpaksa ketepikan pendapat imam abu hanifah yang berpendapat 
tidak harus dibalas bunuh orang yang membunuh orang kafir kerana 
ketiadaan persamaan orang harus dianggap lebih tinggi nilai peribadinya 
daripada bukan kerana akidah tauhid mereka saya akan bacakan firman 
dalam ayat yang menyatakan bahawa itu hendaklah membalas mata dengan 
mata hidung dengan hidung telinga dengan telinga gigi dengan gigi dan 
luka pun ada tanpa menyebut status agama ditakdirkan dalam persidangan 
ini saya kemukakan mazhab hanafi yang mensyaratkan bunuh balas itu 
hendaklah wujud persamaan antara dua pihak pastinya awal lagi ditolak 
dengan itu zalim tidak menghormati nyawa manusia sekali gus mereka 
mendakwa bukan jalan penyelesaian imam syafii adalah ulama yang sangat 
prihatin kepada suasana dan keadaan semasa ini terbukti daripada fatwa 
beliau sewaktu menetap di bahawa taubat orang yang melakukan kesalahan 
yang membabitkan hadhudud boleh menghapuskan dosa selepas beliau 
berpindah ke mesir didapatinya orang di sana suka mempermainmainkan 
taubat dan undangundang lalu beliau mengubah fatwa sebelumnya dengan 
mengeluarkan fatwa yang baru iaitu taubat tidak boleh menggugurkan 
seseorang daripada tindakan had ke atasnya hal ini tidak harus dianggap 
kelemahan dalam fatwa dan ijtihad syafii kerana perbezaan tempat dan 
sikap manusia turut membawa perubahan kepada fahaman dan pendekatan 
yang lebih serasi dengan matlamat syarak andainya pula saya diundang ke 
persidangan hak kemanusiaan yang diadakan di china yang rakyatnya tidak 
mengakui wahyu penyelesaian hukum dan peraturan hidup hanya melihat 
kepada muslihat dan kepentingan sematamata mereka akan menuduh sebagai 
agama yang tidak menghormati hak kaum wanita andainya saya kemukakan 
pendapat imam syafii dalam persidangan itu yang menyatakan bahawa 
wanita dara boleh dipaksa wali bapanya supaya mengahwini lelaki 
walaupun tidak disukainya saya lebih berminat mengemukakan pendapat 
imam abu hanifah yang menyatakan wanita tidak boleh dikahwini tanpa 
persetujuannya saya kira pandangan ini lebih tepat untuk dikemukakan 
atas sifat saya sebagai seorang pendakwah sebagai seorang pendakwah 
yang tugasnya berusaha mengajak manusia yang selama ini menyembah 
patung dan berhala supaya mentauhidkan saya akan memberitahu mereka 
bahawa setiap insan akan ditentukan nasibnya di akhirat mengikut amalan 
yang mereka lakukan di dunia tidak memaksa sesiapapun melakukan sesuatu 
yang di luar keupayaannya dan tidak akan bertindak terhadapnya kerana 
kesalahan orang lain saya akan bacakan kepadanya wahyu yang menjelaskan 
keadilannya iaitu dalam ayat yang bermaksud tidak memberati seseorang 
melainkan sesuai dengan kesanggupannya saya akan memberitahu mereka 
bahawa dalam ayat menyatakan pada hari kiamat penyembah berhala 
dikehendaki mendapatkan bantuan berhala bagi menyelamatkan diri mereka 
daripada seksaan tetapi pada ketika itu mereka mengakui tidak pernah 
mempersekutui kemudian saya akan mengingatkan mereka firman dalam surah 
alaraf ayat yang menyatakan penyesalan mereka terhadap tindakan 
penyembahan berhala sewaktu di dunia dulu dan seterusnya mereka 
mengakui bahawa dulu mereka adalah orang yang kufur kepada saya percaya 
mungkin orang sekarang yang hidup tanpa agama menolak pendekatan ini 
pada pertemuan pertama tetapi akhirnya mereka pasti menerimanya juga 
demikian juga dengan ayat yang menjelaskan hubungan antara dengan agama 
lain dalam pertemuan awal saya akan kemukakan ayat yang menyatakan 
tiada paksaan dalam agama ini antaranya ayat alkahf yang menyatakan 
maksudnya kebenaran itu datangnya daripada tuhanmu sesiapa yang ingin 
beriman berimanlah sesiapa yang ingin kafir maka kafirlah pendakwah 
tidak seharusnya menganggap ayat ini bertentangan dengan firman dalam 
ayat yang menyatakan perangilah orang yang tidak beriman kepada dan 
tidak beriman kepada hari akhirat dan tidak mengharamkan apa yang 
diharamkan dan rasulnya dan tidak beragama dengan agama yang benar 
iaitu orang yang diberi kepadanya sehingga mereka membayar jizyah 
dengan patuh sedang mereka dalam keadaan tunduk anda mungkin 
tertanyatanya yang mana satu antara dua ayat ini harus diikuti perangi 
orang kafir atau biarkan mereka dengan agama yang mereka sukai syeikh 
menjawab setiap ayat harus dibaca dengan ayat sebelumnya dan letakkan 
setiap ayat pada tempatnya yang sesuai ayat tidak harus dianggap ada 
pertentangan antara satu dengan lain ia perlu difahami secara 
keseluruhan bukannya secara bahagian yang terputus kegagalan meletakkan 
ayat pada tempatnya yang sesuai boleh mendatangkan bahaya kepada 
perjalanan dakwah dan penyebaran ajaran kerana pastinya berlaku 
kesongsangan dalam tabiat yang mendasari ajarannya pada toleransi mudah 
dan saling hormatmenghormati dalam ayat menjelaskan hikmat diturun 
secara beransuransur iaitu supaya hati manusia berasa puas dengan 
ajaran itu kerana pada setiap waktu dan suasana ada hukum dan 
ketentuannya kemudian sila perhatikan ayat dalam imran ayat itu 
menyatakan agama yang sebenarnya di sisi ialah agama tiada agama yang 
diterima selain kemudian dalam ayat berikutnya ayat menyatakan jika aku 
serahkan diriku kepada dan juga orang yang mengikut aku dan jika kamu 
serahkan diri kamu juga kepada maka kamu juga mendapat hidayat dan 
petunjuknya dalam ayat ini jelas tidak ada paksaan dalam menjalani 
dakwah dakwah harus berjalan secara mesra dan tidak mengandungi unsur 
kekerasan dan paksaan inilah sebenarnya konsep dakwah yang diikuti nabi 
ketika baginda memperkenalkan kepada raja yang beragama seperti yang 
direkod dalam sahih bukhari iaitu dalam surat baginda bermaksud anda 
nescaya anda selamat anda pasti diberi pahala dua kali ganda wahai ahli 
kitab marilah kepada suatu ketetapan yang tidak ada perselisihan antara 
kami dan kamu bahawa kita tidak sembah kecuali dan kita tidak 
persekutukan dia dengan sesuatupun dan tidak sebahagian kita menjadikan 
sebahagian yang lain sebagai tuhan selain daripada perhatikan pula 
dalam ayat pertama melarang kita bersetia dengan orang bukan berkasih 
sayang dan bersahabat dengan mereka kerana mereka mengingkari kewujudan 
mereka mengusir nabi dan orang kemudian pada ayat berikutnya berfirman 
menyatakan tidak melarang umat berserta dengan orang kafir yang tidak 
memerangi orang dalam mengamalkan agama dan tidak mengusir dari tanah 
air mereka untuk berlaku adil terhadap orang bukan syeikh berkalikali 
menasihatkan pengulas ayat agar meneliti ayat secara keseluruhan 
bukannya dengan secara terputusputus ini untuk mengelakkan 
penyelewengan maksud misalnya ayat jika yang dibaca separuh saja iaitu 
la yang bermaksud jangan kamu hampiri sembahyang tentulah ayat yang 
tidak sempurna ini membawa maksud melarang kita bersembahyang padahal 
yang dilarang sembahyang ialah ketika mereka mabuk dan tidak waras 
fikirannya seperti yang dijelas pada bahagian akhir ayat itu menurut 
ramai orang tibatiba muncul dan bergerak aktif dalam bidang dakwah 
tetapi tidak mengetahui bagaimana harus berdakwah ia lupa peringatan 
bahawa mesej nabi ialah membawa kesejahteraan bukan kecelakaan sabdanya 
ana nabi la nabi almalhamah aku nabi pembawa rahmat bukan nabi 
penganjur peperangan andainya konsep kekerasan dilakukan tentunya bukan 
saja bertentangan kehendak tetapi mengakibatkan umat menghadapi risiko 
yang tinggi maksud ini dibayangkan dalam ayat yang bermaksud mereka 
akan melontar kamu dengan batu atau memaksa kamu kembali kepada agama 
mereka dan jika demikian kamu tidak akan beruntung buat selamalamanya 
sebagai kesimpulan perlu difahami sebaikbaiknya bagi mendapatkan ajaran 
dan teladan yang betul memahami keseluruhan memberikan kita hidayat 
yang betul bukannya satu dua ayat saja yang mendatangkan banyak 
kekeliruan 
 * 
Maori (names, including placenames) -- maarama pooneke pirongia waikaremoana whanganui kaingaroa taranaki 
apakura whakatau tuutaanekai horowhenua pani tamatekapua mere maahina 
hoturapa teheuheu teararoa tekawakawa tainui whangaehu maungawhau 
aawhitu rotorua mokoia hinemoa tiiwaiwaka maaui hinenuitepoo rehua 
tuuhourangi ngaapui hawepootki turi haakawau tehiakai taawhaki whairiri 
taranga whakaue pita wahieroa kura rata mahuika taane tuuwhakararo 
tamanuiteraa ngaatiawa hinauri tiireni maaori tinirau paania houtaaewa 
kae rewi te whaanauaapanui poihaakena rupe tama tekowha moeahu kaipara 
heremia kupe haumoe tetomo waihii tetaaite hamutana hata paeko matiu 
tongariro kahawai taukata hawaiki uenuku whakatuuria raumahora 
tuutanekai mahuika mookena paapaka hinekoorangi hamuera hoturoa 
whakaotirangi horomona kaawhia paania tereinga heta paka rongo toro 
 * 
Middle English (words) -- here bygynneth the book of the tales of caunterbury whan that aprill 
with his shoures soote the droghte of march hath perced to the roote 
and bathed every veyne in swich licour of which vertu engendred is the 
flour whan zephirus eek with his sweete breath inspired hath in every 
holt and heeth the tendre croppes and the yonge sonne hath in the ram 
his halve course yronne and smale foweles maken melodye that slepen al 
the nyght with open ye so priketh hem nature in hir corages thanne 
longen folk to goon on pilgrimages and palmeres for to seken straunge 
strondes to ferne halwes kowthe in sondry londes and specially from 
every shires ende of engelond to caunterbury they wende the hooly 
blisful martir for to seke that hem hath holpen whan that they were 
seeke bifil that in that seson on a day inb southwerk at the tabard as 
i lay redy to wenden on my pilgrimage to caunterbury with fuldevout 
corage at nyght was come into that hostelrye wel nyne and twenty in a 
compaignye of sondry folk by aventure yfalle in felaweshipe and 
pilgrimes were they alle that toward caunterbury wolden ryde the 
chambres and the stbles weren wyde and wel we weren esed atte beste and 
shortly whan the sonne was to reste so hadde i spoken with hem 
everichon that i was of hir felaweshipe anon and made forward erly for 
to ryse to take oure way ther as i yow devyse but nathelees whil i have 
tyme and space er that i ferther in this tale pace me thynketh it 
acordaunt to resoun to telle you al the condicioun of ech of hem so as 
it semed me and whiche they weren and of what degree and eek in what 
array that they were inne and at a knyght than wol i first bigynne 
allas i wepynge am constreyned to bygynnnen vers of sorwful matere that 
whilom in florysschyng studie made delitable ditees for lo randynge 
muses of poetes enditen to me thynges to ben writen and drery vers of 
wretchidnesse weten my face with verray teres at the leeste no drede ne 
myghte overcomen tho muses that thei ne were felawes and folwyden my 
wey that is to seyn whan i was exiled they that weren glorie of my 
youthe whilom weleful and grene conforten now the sorwful wyerdes of me 
olde man for eelde is comyn unwarly uppon me hasted by the harmes that 
y have and sorwe hath comandid his age to ben in me heeris hore arn 
schad overtymeliche upon myn heved and the slakke skyn trembleth of myn 
emptid body thilke deth of men is weleful that ne comyth noght in 
yeeris that be swete but cometh to wrecches often yclepid allas allas 
with how deef an ere deth cruwel turneth awey fro wrecches and nayteth 
to closen wepynge eien whil fortune unfeithful favourede me with 
lyghten goodes the sorwful houre that is to seyn the deth hadde almoost 
dreynt myn heved but now for fortune cloudy hath chaunged hir 
deceyvable chere to meward myn unpietous lif draweth along unagreable 
duellynges o ye my freended what or wherto avaunted ye me to be weleful 
for he that hath fallen stood noght in stedefast degre in the mene 
while that i stille recordede these thynges with myself and merkid my 
weply compleynte with office of poyntel i saw stondynge aboven the 
heghte of myn heved a womman of ful greet reverence by semblaunt hit 
eien brennynge and cleerseynge over the comune myghte of men with a 
lifly colour and with swich vogour and strengthe that it ne myghte nat 
ben emptid al were it so that sche was ful of so greet age that men ne 
wolden nat trowen in no manere that sche were of our elde the stature 
of hire was of a doutous jugement for sometyme it semede that sche 
touchede the hevene with the heghte of here heved and whan sche hef hir 
heved heyer sche percede the selve hevenne so that the sighte of men 
lokynge was in ydel hir clothes weren makid of right delye thredes and 
subtil craft of perdurable matere the whiche clothes sche hadde duskid 
and dirked as it is wont to dirken besmokede ymages in the nethereste 
hem of bordure of thise clothes men redden ywoven in a gekissch p that 
signifieth the lif actif and aboven that lettre in the heieste bordure 
a grekyssh t that signifieth the lif contemplatif and betwixen thise 
two lettres ther were seyn degrees nobly ywrought in manere of laddres 
by which degrees men myghten clymben fro the nethereste lettre to the 
uppereste natheles handes of some men hadden korve that cloth by 
violence or by strengthe and everich man of hem hadde boren awey swiche 
peces as he myghte geten and for sothe this forseide womman bar smale 
bokis in hir right hand and in hir left hand sche bar a ceptre and whan 
she saugh thise poetical muses aprochen aboute my bed and enditynge 
wordes to my wepynges sche was a litil amoeved and glowede with cruel 
eighen who quod sche hath suffred aprochen to this sike man thise 
comune strompettis of swich a place that men clepen the theatre the 
whiche not oonly ne asswagen noght his sorwes with none remedies but 
thei wolden fedyn and noryssen hym with sweete venym for sothe thise 
ben tho that with thornes and prikkynges of talentz of afeccions whiche 
that ne bien nothyng fructifyenge nor profitable destroyen the corn 
plentyvous of fruytes of resoun for thei holden hertes of men in usage 
but thei delyvre noght folk fro maladye but yif ye muses hadden 
withdrawen fro me with youre flateries any unkunnynge and unprofitable 
man as men ben wont to fynde comonly among the peple i wolde wene 
suffre the lasse grevosly forwhi in swych an unprofitable man myne 
ententes weren nothyng endamaged but ye withdrawen me this man that 
hath ben noryssed in the studies or scoles of eleaticis and achademycis 
in grece but goth now rather awey ye mermaydenes which that ben swete 
til it be at the laste and suffreth this man to ben cured and heeled by 
myne muses that is to seyn by noteful sciences and this the companye of 
muses iblamed casten wrothly the chere dounward to the erthe and 
schewing be rednesse hir schame thei passeden sorwfully the thesschfold 
and i of whom the sighte ploungid in teeres was dirked so that y ne 
myghte noght knowen what that womman was of so imperial auctorite i wax 
al abayssched and astoned and caste my syghte doun to the erthe and 
bygan stille for to abide what sche woolde doon aftirward tho com sche 
ner and sette her doun uppon the uttereste corner of my bed and sche 
byholfynge my chere that was cast to the erthe hevy and grevous of 
wepynge compleynde with thise wordis that i schal seyn the perturbacion 
of my thought the romaunt of the rose amant whanne i hadde herde all 
resoun seyn which hadde spilt hir speche in veyn dame seide i i dar wel 
sey of this avaunt me wel i may that from youre scole so devyaunt i am 
that never the more avaunt right nought am i thurgh youre doctrine i 
dulle under youre duscipline i wot no more that i wist er to me so 
contrarie and so fer is every thing that ye me ler and yit i can it all 
par cuer myn herte foryetith therof right nought it is so writen in my 
thought and depe greven it is so tendir that all be herte i can it 
rendre and rede it over comunely bit to mysilf lewedist am i but sith 
ye love discreven so and lak and preise it bothe twoo defyneth it into 
this letter that i may thenke on it the better for i herde never 
diffyne it er and wilfully i wolde it ler raisoun if love be serched 
wel and sought it is a syknesse of the thought annexed and knet bitwixe 
tweyne which male and female with oo cheyne so frely byndith that they 
nyll twynne whether so therof they leese or wynne the roote springith 
thurgh hoot brennyng into disordinat desiryng for to kissen and enbrace 
and at her lust then to solace of other thyng love recchith nought but 
setteth her herte and all her thought more for delectacioun than ony 
procreacioun of other fruyt by engendring which love to god is not 
plesing for of her body fruyt to get they yeve no force they are so set 
upon delit to pley infeere and somme have also this manere to feynen 
hem for love sek sich love i preise not at a lek for paramours they do 
but feyne to love truly they disdeyne they falsen ladies traitoursly 
and swern hem othes utterly with many a lesyng and many a fable and all 
they fynden deceyvable and whanne they han her lust geten the hoote 
ernes they al foryeten wymmen the harm they bien full sore but men this 
thenken evermore that lasse harm is so mote i the decyve them than 
decyved be and namely where they ne may fynde non other mene wey 
 * 
Modern Greek (female names) -- Adonia Adona Agathe Agathi Alethea Alethia Alithea Thea Alexandra Alexa 
Alexia Aleka Aleki Alex Lexa Lexi Alexine Ritsa Alfa Althaia Altheda 
Altheta Thea Theda Theta Ambrosia Ambrosina Ambrosine Brosina Anastasia 
Anastasie Anasta Stacie Stasia Andromede Andromeda Meda Angele Angela 
Angeliki Angheliki Angeline Angliki Antheia Anthea Antha Thea Apolline 
Apollina Arete Areta Ariadne Ariadna Ariana Ariane Arianne Eria Artemis 
Artemisia Aspasia Aspa Aspia Aster Asta Astra Astrea Atalante Atalanta 
Atlanta Athene Athena Basila Basilea Basilia Vasiliki Berenike Berenice 
Bernice Berna Calandra Calla Calli Charis Charissa Chloris Chloras 
Chlorise Chlorisse Cloris Christiane Christine Christa Christi Clytie 
Clyte Clytia Cosima Cosma Cressida Cresida Cressa Cyma Syma Cynara Zinara 
Cypria Cipria Cipriana Cypra Cytherea Cytheria Damalis Damala Damalas 
Damali Damalla Damaris Damara Damarra Mara Mari Maris Daphne Daphna 
Daphney Darice Daria Darise Darrice Dari Delphine Delfina Delfine Delfinia 
Delphina Delphinia Demetria Demetra Demitria Dimitra Dimitria Denise 
Denice Diantha Dianthe Dianthia Dionne Diona Dione Dionis Dionyssia Dora 
Dorinda Doris Dorea Doria Dorice Dorise Dorisse Dorris Dorrise Dorkas 
Dorca Dorcas Dorcea Dorcia Dorothe Dorthea Eirene Irene Rena Ekaterine 
Ekaterini Aikaterine Katina Eleftheria Elewteria Elewtheria Elektra 
Electra Ellice Ellise Eudora Euda Eudosia Eudocia Eudokia Eudoxia Eugenia 
Evgenia Eulalia Eulalie Eula Lelia Eunike Eunice Filia Hagne Agna Agne 
Agnes Helene Helena Lena Elaine Elena Ellena Eleni Elenitsa Elina Nitsa 
Hesper Hespera Hesperia Ianthe Iantha Iona Ione Ionia Iphigeneia Iphigenia 
Iris Irisa Isaura Isaure Aura Kali Kalli Kalyca Kalika Kalliope Kaliope 
Calliope Kallisto Calista Kassandra Casandra Cassandra Cassandre Khloe 
Chloe Cloe Kore Cora Kora Koren Korinna Corinna Corrinne Corrina Corrine 
Corinne Cori Kynthoa Cinthia Cynthia Lydia Lidia Magdelene Madeline Lena 
Margarites Gryta Melaina Melania Melanie Melodie Melody Neoma Neomah 
Nikola Nikoleta Nikki Niki Nysa Nyssa Odele Odella Olympia Olympe Lympia 
Pia Ophelia Ophelie Phelia Ourania Urania Parthenie Parhena Parthenia 
Petra Petrina Phaidra Phaedra Philana Philene Philina Phillina Philippa 
Philippe Phillipa Phillippe Pippa Phyllis Philis Phillis Philisse Phylis 
Phylisse Rhode Rhoda Selene Celena Selona Sibyl Sibella Sibyll Sibylla 
Sybil Sybilla Sophia Sofia Sophie Sofi Stefana Stefania Tecla Tekla Thecla 
Thekla Thaleia Thalia Theodora Fedora Feodora Teodora Theone Theona 
Theonie Theophania Theophano Theresa Teresa Terese Tresa Resi Xanthe 
Xantha Xenia Zena Zenia Xylia Xyla Xylina Xylona Acacia Acantha Aegea 
Aeola Agalia Alala Aldora Aleni Anatolia Andrianna Anemone Angeline 
Aphrodite Athena Athenagora Calantha Calida Cassia Cleopatra Cyra Damia 
Daphne Deianira Delia Dido Dimitra Echo Elma Elpida Emalia Erianthe 
Euphemia Euphenia Euphrosyne Evadne Evanthe Fotini Galatea Georgia 
Halcyone Helena Helia Hera Hermione Io Isadora Jocasta Kalidas Kaligenia 
Kalligenia Kallirrhoe Kalonice Kyriakoula Lalage Leandra Leda Lycoris 
Lyris Marina Medora Melantha Melina Melissa Mona Neola Neona Nerissa Nike 
Niobe Olga Orthia Pandora Pangiota Panthea Pelagia Penelope Persephone 
Philantha Philomela Philomena Philothea Phoebe Phoenix Rhea Saphhira 
Sofronia Stamata Stella Tassia Tassula Tessa Thaddea Thais Thalassa 
Tharsia Theodosia Theophilia Thetis Timothea Toula Xantippe Yana Zenaida 
Zenobia Zephyra Zoe 
 * 
Modern Greek (male names) -- Aigeus Agias Aineias Aeneas Eneas Alexander Alexandros Alekos Aleksiu 
Alexios Sander Sandros Anakletos Cletus Kletos Anastasios Anstace Angelos 
Angelo Anzioleto Anziolo Aniketos Nikita Apollo Apolo Polo Argos Argus 
Aristides Aristeides Ari Aris Aristotelis Aristoteles Aristotle Telis 
Athanasios Thanasis Thanasi Thanos Basileios Vasileios Vasilios Vasilis 
Vassili Basil Vasos Benedictos Benedict Venedictos Christiano Christian 
Kristion Kris Christos Kristos Kristo Damaskenos Damaskinos Damianos 
Damian Damon Demetrios Demetrius Dhimitrios Dimitrios Dimitris Demetri 
Mitros Dionysios Denys Dinos Dion Eleutherios Eleftherios Eleaterios 
Erasmios Eugenios Eugen Eusebios Eusebius Eustachius Eustakhios George 
Georghios Georgios Iorgos Yorgos Gregorios Gregor Grigoris Grigorius 
Heraklees Herakles Hercules Hieremias Hieronymos Isidoros Isadorios Isidor 
Isidore Jason Iason Kyprios Kypros Kyrillos Kiril Kyros Leiandros Leander 
Leandros Loukanos Lukianos Loukas Loucas Lucas Lucais Lukas Lysandros 
Lysander Sander Sandros Makarios Makar Makis Memnos Michalis Nektarios 
Nectarios Nikodemos Nicodemus Nikolaos Nicholas Nikolas Nikolos Nocolau 
Nico Nicos Niko Nikos Nilos Orestes Orest Panagiotis Panayiotis Panos Pan 
Pericles Periklis Philippos Philip Prokopios Procopius Sebastianos 
Sebastian Spiridon Spyridon Spyros Stephanos Stefanos Stephanas Staphanos 
Stamatis Stavros Theodor Theodore Theodorus Feodor Theo Zeno Zenon Acheron 
Achillios Adamantios Adonis Aetios Agapios Agathias Aiakos Aiolos Aktaion 
Ambrosios Andreas Anninos Antaios Apostolos Archimedes Aristokles 
Arkhippos Arsenios Charalambos Christophoros Dareios Dighenis Doxiadis 
Eleni Elpidios Epaminondas Euphemios Euripedes Evangelos Glafkos Gondikas 
Hali Harilaos Hermes Hesperos Hippocrates Hippolytos Homerus Kalinikos 
Kalogeros Kharilaos Kimon Kosmas Kyriako Lambros Leontius Manolis Marinos 
Methodius Metrophanes Milos Miltos Mimis Myron Nestor Nikiforos Nikomedes 
Nikostartos Odysseus Orion Parthenios Pelagios Philogathos Platon Podromos 
Polymenis Rhigas Savas Skyros Socrates Sophoklis Soterios Stelios Tanek 
Tasos Teles Telesphorus Thaddaios Themistoklis Theodosius Theofanis 
Theophilus Thrasyvoulos Timotheos Titos Traianos Tychon Tzannas Xenophon 
Zenobios Ioannes Giankos Giannes Ioannikios Ioannis Iannis Jannes 
Joannoulos Yannis Yannas Yanni Yannakis Nannos Isaakios Ioseph Iakobus 
Iakovus Baltsaros Michaelis Mikhail Michael Mikhalis Seimon Symeon 
Zachaios Zacheus Konstantinos Konstandinos Costa Costas Kastas Kostas 
Kosti Kostis Klemenis Klemens Laurentios Viktor 
 * 
Modern Greek (surnames) -- Alexopolous Alexopoulos 
Anagyrou Anastassiou Andreadis Andreou Andriopoulos Andronikos Andros 
Androuchelli Androupolos Androutsos Angelis Angelomatis Angelou 
Anistonopoulos Anninos Antikatzides Antonatos Antonious Argyra 
Athanasiadis Athanassiadi Axelos Bakirdzis Bakogianni Balanos Bazignos 
Bessarion Bizos Borbokis Botsaris Boudouris Bourgani Capodistria Carides 
Castriota Catalactus Chacoliades Chakiris Christodoulou Christos Clerides 
Constandouros Constantinis Cosmatos Costanduros Costi Cotsadis Couloumbis 
Coulouris Coulouris Damaskinos Deligiannis Deligiorgis Delivorias Dellas 
Demertzis Demosthenous Dimas Dimitrakos Dimitriades Dimitris Dimotsios 
Diomedes Dontas Doxiadis Dukakis Duvis Economakis Economou Eliades 
Elipandas Ellinas Farakos Fettas Florakis Galonopoulas Gatsioudis 
Gatzioudis Gavrielides Gennadios Genovelis George Georghiou Giorgiou 
Gislenus Gizikis Glezos Gogos Gonatas Goulandris Gounaris Gousetti Grivas 
Hagiorgiu Hajiyanni Halara Harteros Iakovakis Iakovou Iatrides Ioannidis 
Ioannou Issigonis Joakimides Kairis Kaklamanakis Kakoulli Kalergi Kallabis 
Kalvos Kanaris Kanellopoulos Kaphandaris Karahi Karaiskakis Karamanlis 
Karatossos Karoki Karyoti Kasomoulis Katsantonis Katsaris Kenteris 
Khadjikyriakos Klaras Kochalakos Koffa Kokkinos Kokotis Kolettis Kollias 
Kolokotronis Kondylis Kontoghiorghes Koraïs Korkizoglu Korres Koryzis 
Koskotas Kostanopoulos Kostopoulou Kostopulos Kotaridou Koukodimos 
Koukoudimos Koumoundouros Koundouriotis Kourniakis Kousoulas Kozani 
Kritopoulos Kyprianou Kyriacou Kyriakou Kyriazis Lambrakis Lambrianou 
Lambrinos Lathouris Lazardis Leonidis Lianis Logotheti Louca Louganis 
Louvaris Lyssarides Maganas Magos Maharis Makarezos Makrigiannis Malachias 
Maliagros Mangakis Marcoullides Mardas Marinatos Markezinis Markoulides 
Markoullides Marsolais Martis Matacena Mavridou Mavrogordatos 
Mavrokordatos Mavromikhalis Mavros Mavroudis Maximos Melas Meletoglou 
Meletoglu Melikis Melissanides Mercouri Merukides Messimeris Metaxas 
Michaelis Mikalis Mikhalakopoulos Militis Mitrou Mitsotakis Monomachus 
Moscopolis Moumoulidis Muscouri Mylonas Nafpliotis Negris Niarkhos 
Nikolaides Onassis Orphanides Oxinos Palamas Pallis Pan Panagiatopoulos 
Panagiopoulou Panagiotopou Panagou Panagoulias Panagoulis Panaotis 
Panavoglou Panayi Panayides Panayiotopoulos Panayiotou Pangalos Papadakis 
Papademetriou Papadias Papadopolos Papadopoulos Papadoupolos Papafagos 
Papageorgiou Papagos Papakostas Papamichael Papanastasiou Papandreou 
Papasotiriou Papatamelis Papathanassiou Papoulias Pappas Pappou 
Parastadidis Pattakos Pavlakakis Pentzopoulos Pesmazoglou Pheraios 
Philipousis Pierides Pietris Pipinelis Pittakis Pittakys Plastiras Polides 
Polycarpous Polychroniadis Polychroniou Polydorou Polymeropoulos Polymeru 
Polyzou Popotas Porpurogenitos Pratkanis Prevalakis Primikynos Psara 
Psaros Psaroulakis Rallis Saltsidis Samos Sampras Sanassis Sarakatsani 
Saraphis Sardzetakis Sarris Sartzetakis Sarus Savakis Savalos Savvides 
Sbokos Senteildis Siantos Sicilianos Simopoulos Sirtis Skandalis Skiotis 
Skouloudis Skouphas Solomos Sophoulis Souvaltzis Stamanakis Stasi 
Stavrakis Stavrianakis Stavrianos Stavrou Stephanopoulos Stoikos 
Strapoudopolos Stratis Stroubakos Taliadoros Tathouris Tavoularis Terzin 
Thanou Theodopoulos Theodorakis Theophanous Theothanou Theotokis Touliatos 
Tournikiotis Tragianopoulos Trikoupis Trikoupis Tsakolov Tsaldaris Tsatsos 
Tsavdaridou Tsiamita Tsigakou Tsikouna Tsiolakoudi Tsirigolis Tsirimokos 
Tsolakoglou Tsoni Tsouderos Tsougarakis Tsoutsouvas Tsyoma Tzannetakis 
Tzivas Vadeki Valaoritis Valvis Vaphiadis Vardakastanis Varvaressos 
Vasdeki Vasdekis Vasiliou Vassilides Vatatzes Veloukhiotis Venizelos 
Verdanis Vergopoulos Vlakhos Vlakhou Voulgaris Voulgaropoulos Vouris 
Xanthos Xanthou Xenakis Xydis Yannopoulos Yialouris Ypsilantis 
Zafiropoulos Zaïmis Zakhariadis Zarikos Zervakos Zervas Zikos Zindilis 
Zisimides Zographas Zoïtakis Zolotas 
 * 
Polish (words) -- zofia bastgen widac bylo na wprost wejscia zagraniczne wazono 
przenoszono czy moge isc zabawa oshmioletnia zreszta tanczyc jakish pan 
trzymac zawsze co przypomniala sobie ewa co znajdowalo sie urzedzie 
pocztowym shrodku na prawo na lewo urzedniczka spredawala rozni ludzie 
pisali wyslalas do swojego nie przyjmuja okienku ktorym polecone stalo 
kilka oshob znacki druki starszy pan kupowal jego pies stal obok niego 
czekala chwile bar barszcz bulion butelka cielecy gesh groch grochowy 
groszek indyk karta kasza kelner klusecxki kluski kotlet mieso mleczny 
napic odmiana pieczen piwo lacek postanowicz potrawa procz spis stolik 
watroba wolowina wodka zaczac ziemniaczany zupa nastepujace artykul 
bielizna brac brak bucik budynek czapka czwarty dac damski dlugo drzwi 
duzy dywan dziecinny firanka format garnek inny jechac jezdzic juz kasa 
kasjerka kawiarnia kobieta kolejka koperta koszula krawat kupowac 
kuzynka lewy marunarka meski mezczyzna mniejszy muzyka najlepszy 
niebieski numer obie ogladac oprawa parter patrzec piaty placic plyta 
przebierac przynajmniej pudelko razem rekawiczka rodzaj rozrywkowy 
rozny ruchomy rzecz schody ruchome sklep solniczka stoisko szosty 
tamten tez trzeci tylko urzad wchodzic welniany wiezc wiekszy wybierac 
zajmowac zmeczony 
 * 
Russian (placenames) -- Alatyr' 
Aleksandrova Sloboda 
Aleksin 
Andreev 
Arkhangel'sk 
Arvamas 
Astrakhan 
Azov 
Bagut 
Baia 
Bakhchisarai 
Bakota 
Baku 
Balakhna 
Bania Rodna 
Bar 
Barduev 
Baroch 
Baruch 
Barysau 
Baseia 
Bazavluk 
Belaia Vezha 
Belaia Vezhna 
Belev 
Belgorod 
Belgorod Riazanskii 
Beloberezh'e 
Beloe Selo 
Belozero 
Beloozero 
Belz 
Bel'chitsa 
Bendery 
Benitsy 
Berbend 
Berestechko 
Berest'e 
Berevoi 
Berezov 
Berezyi 
Bezhichi 
Bezhitsy 
Bezhitskii Verkh 
Bialystok 
Bilgorod 
Bilhorod 
Blestovit 
Blets 
Bleve 
Bobrovnitsy 
Bobruisk 
Bogoliubov 
Boguslavl' 
Bokhmach 
Boldyzh 
Bologoe 
Bolokhov 
Bolshev 
Boriso-Glebov 
Borisoglebskaia Sloboda 
Borisov 
Borisov-Glebov 
Borovsk 
Bortnitsy 
Bozhesk 
Bozhskii 
Braila 
Braniewo 
Bran' 
Bratslav 
Brest Litovsk 
Briagin 
Briansk 
Brody 
Bron 
Bryn 
Bulgar 
Bulich 
Bulichi 
Buzhsk 
Bykoven 
Bylev 
B'iakhan' 
Caffa 
Cembalo 
Cetatea Alba 
Cheboksary 
Chelm 
Chemerin 
Cherdyn' 
Cherkassii 
Chernechesk 
Chernesk 
Chernigoga 
Chernigov 
Chernihiv 
Chernobyl' 
Chern'chesk 
Chersoneus 
Chersonesus 
Chertoryisk 
Cherven 
Cherven' 
Chichersk 
Chiurnaev 
Chmielnik 
Chukhloma 
Churnaev 
Czerwien 
Dankov 
Daugavpils 
Davidova 
Debriansk 
Dedogostichi 
Dedoslavl' 
Demenesk 
Demon 
Derbent 
Derevich 
Dernovoi 
Deviagorsk 
Dmitrov 
Dmitrov Kievskii 
Dobriatin 
Dobrochkov 
Dobryi 
Dobryi Sot 
Dobryn 
Domagoshch 
Donets 
Dorogichin 
Dorogobuzh 
Dorostol 
Dorpat 
Driutesk 
Driutsk 
Drogichin 
Drohiczyn 
Drutsk 
Duben 
Dubintsa 
Dubno 
Dubok 
Dubrovitsa 
Dubrovitsy 
Dubrovka 
Durbe 
Dveren 
Elat'ma 
Elblag 
Elets 
Elgava 
Elna 
El'na 
Emenets 
Eniseisk 
Ez'sk 
Frombork 
Galich 
Galich Merskii 
Galych 
Gdov 
Gertsik 
Gertsike 
Glebl' 
Glebov 
Glukhov 
Gnoinitsa 
Golotichesk 
Goltav 
Gol'sk 
Gomel' 
Gomii 
Goroden 
Goroden Volynskii 
Gorodets 
Gorodets Meshcherskii 
Gorodets Radilov 
Gorodets Radilov na Volge 
Gorodishche na Sare 
Gorodno 
Gorodok 
Gorodok Volynskii 
Gorodok na Ostre 
Gorokhovets 
Goroshin 
Goshcha 
Gostinichi 
Grodna 
Gubin 
Gur'gev 
Haji-bei 
Halych 
Hapsal 
Homel' 
Hoshcha 
Hrodna 
Iam 
Iama 
Iaropolch 
Iaropolch' 
Iaroslavl' 
Iaroslavl' Galitskii 
Iaryshev 
Iasa 
Iasenskoe 
Iazhelbitsy 
Ignach'-Krest 
Igorevo Selo 
Ilomanets 
Ilukste 
Isady 
Isker 
Iskona 
Iskorosten' 
Itil' 
Iurichev 
Iur'ev 
Iur'ev-Pobol'skii 
Iur'ev Pol'skii 
Ivan 
Ivan-gorod 
Ivan-pogost 
Ivangorod 
Izborsk 
Iziaslavl' 
Iziaslavl' Galitskii 
Izmail 
Jaroslaw 
Kadam 
Kafa 
Kaffa 
Kaliningrad 
Kaluga 
Kamenets 
Kamianets-Podil's'kyi 
Kanev 
Kankor 
Kapan 
Karachev 
Karela 
Kargopol' 
Kashin 
Kashlyk 
Kasimov 
Kasplia 
Kaunas 
Kazan' 
Kekholm 
Kem' 
Kerch' 
Kergedan 
Ketrzyn 
Khalep 
Khersones 
Khliapen' 
Khlynov 
Khmil'nyk 
Khodynichi 
Kholm 
Kholmogory 
Khorobor 
Khorobor' 
Khotshin 
Kiazhta 
Khvalimichi 
Kideksha 
Kiev 
Kiliia 
Kineshma 
Kirillov 
Klaipeda 
Klechesk 
Klin 
Koenigsberg 
Kola 
Kolodiazhen 
Kolomna 
Kolomyia 
Koltesk 
Komov 
Koponov 
Kopor'e 
Kopys 
Kopys' 
Kor'dio 
Korchesk 
Korech'sk 
Korchev 
Korela 
Korsun 
Korsun' 
Korsun'-na-Rosi 
Kosh-Kar 
Kostroma 
Kotel'nich 
Kotel'nitsa 
Kotlas 
Kovno 
Kozar' 
Kozelsk 
Kozel'sk 
Krasn 
Krasnyi 
Krazhiai 
Kremenets 
Kremianets 
Krevo 
Krichev 
Krom 
Krull' 
Krupl' 
Ksniatin 
Kuchelmin 
Kudnovo Selo 
Kukenois 
Kulikovo 
Kulikovo Pole 
Kunil' 
Kurba 
Kurmysh 
Kursk 
Kwidzyn 
Kysylyn 
Ladoga 
Ldishev 
Lewartow 
Liatychiv 
Lidzbark 
Listven 
Liubachev 
Liubech 
Liubno 
Liubutsk 
Livny 
Lobynsk 
Lobyn'sk 
Lodeinitsy 
Logozhsk 
Lomza 
Lopastitsa 
Ltava 
Luben 
Lublin 
Lubno 
Luchesk 
Luchin 
Luki Velikie 
Lukoml' 
Lukoml'skoe Gorodishche 
Lutava 
Lutsk 
Luts'k 
Lybuty 
L'to 
L'viv 
L'vov 
Mahiliou 
Malbork 
Malotin 
Mangup 
Marianichi 
Matrega 
Mazhevo Selo 
Mchenesk 
Medyn' 
Memel 
Menesk 
Mensk 
Men'sk 
Mezchesk 
Mezhibozh'e 
Mezhimost'e 
Michsk 
Mich'sk 
Mikhailov 
Mikulin 
Milcovia 
Milesk 
Miliniska 
Minsk 
Mogilev 
Moklekov Galitskii 
Mologa 
Moncastro 
Moravitsa 
Moreva 
Morova 
Moroviisk 
Mosal'sk 
Moscow 
Moskva 
Mozhaiask 
Mozhaisk 
Mozyr' 
Mstislavl' 
Mtsensk 
Munarev 
Murav'in 
Muravitsa 
Murom 
Murovinsk 
Mutizhir 
Myl'sk 
Naliuch 
Narva 
Narym 
Navahrudak 
Nebl' 
Nebolchi 
Neiatin 
Nekoloch' 
Nemogardas 
Nerekhta 
Nerinsk 
Nerin'sk 
Nesvizh 
Novoe Sarai 
Nezhatin 
Nezhegorod 
Niasvizh 
Nikola-Zarazskii 
Nikulitsyn 
Nimikori 
Nizhnii Novgorod 
Nosov 
Novgorod 
Novgorod Nizhnii 
Novgorod Severskii 
Novgorod Sviatopolch 
Novgorod Velikii 
Novgorodok Litovskii 
Novogrudok 
Novosil 
Novosil' 
Novyi Torg 
Nizhyn 
Obdorsk 
Obolv' 
Obolensk 
Obrov 
Obskii Gorodok 
Obskiigorodok 
Ochakov 
Odoev 
Odrsk 
Olbia 
Olesh'e 
Olonets 
Ol'gov 
Ol'gov Novyi Gorodok 
Ol'zhitsi 
Onut 
Opochka 
Oprosh 
Orlov 
Orekhov 
Orel 
Orel'sk 
Oreshek 
Orgoshch 
Ormina 
Ornas 
Orsha 
Orzhitsa 
Oskol 
Osterskii Gorodets 
Ostrog 
Ovruch 
Ozhsk 
Paleostrovsk 
Panivtsi 
Paozero 
Patsin 
Patsyn' 
Pelym 
Pereiaslavets 
Pereiaslavl'-Riazanskii 
Pereiaslavl' Russkii 
Pereiaslavl' Zalesskii 
Perekop 
Peremid' 
Peremil' 
Peremyshl' 
Peresechen 
Peresolnitsa 
Perevitsk 
Perevolochna 
Perevoloka 
Perm 
Pesochen 
Piarnu 
Piatra Neamt 
Pinega 
Pinsk 
Piriatin 
Piten 
Plav 
Plesensk 
Plesnesk 
Pleso 
Podol 
Pogonovichi 
Pogost Iuskola 
Pogost Ivan' 
Pogost Kukueva gora 
Pogost Lipna 
Pogost Masnega 
Pogost-na-More 
Pogost Olon's' 
Pogost Pinega 
Pogost Pinera 
Pogost Poma 
Pogost Puita 
Pogost Rakul' 
Pogost Sabel' 
Pogost Svir' 
Pogost Tervinichi 
Pogost Toima 
Pogost Tudovor 
Pogost Ust'-Emets 
Pogost Ust'-Vaga 
Pogost Vel' 
Pogost Volok na Mshi 
Polatsk 
Polkosten 
Polksten' 
Polonnyi 
Polonoi 
Polotsk 
Poltesk 
Pomezania 
Pomozdin Pogost 
Popash' 
Poragopustets 
Porkhov 
Pos. Pyban'sk 
Posechen 
Posotina 
Potsin 
Poznan 
Preslav 
Preslavets 
Presnensk 
Priluk 
Pronsk 
Proposhesk 
Prozorovo 
Prupoi 
Przemysl 
Pskov 
Pudozhskoi 
Pultusk 
Pustozerskii Ostrog 
Putivl' 
Radauti 
Radogoshch 
Radonezh 
Radoshch 
Raguilov 
Rakoma 
Razdory 
Rechitsa 
Rei 
Reszel 
Reval 
Ria 
Riapolovo 
Riazan' 
Riazhsk 
Riga 
Rimov 
Roden 
Rodnia 
Rogachev 
Rogisdino 
Rogov 
Roman 
Romanov 
Romen 
Ropesk 
Roslavl 
Rostislavl' 
Rostov 
Rostovets 
Rosukha 
Rozan 
Rsha 
Rusa 
Rustovets 
Ruza 
Ryl'sk 
Rzev 
Rzhev 
Rzhevka 
S. Meltenovo 
Sakov 
Samara 
Sambir 
Sandomierz 
Sanok 
Sansin 
Sapogyn' 
Sarai 
Sarai-Berka 
Saratov 
Sarkel 
Sarskaia 
Sartakha 
Sebezh 
Sech 
Seiiazhsk 
Selpils 
Semender 
Semenov 
Semots 
Semyn' 
Semych 
Sepukhov 
Serebrianyi 
Sereger 
Serensk 
Serpeisk 
Sevsk 
Sharukan' 
Shatsk 
Shchekarev 
Sheki 
Shemakha 
Shenkursk 
Shepol' 
Shestakov 
Shiauliai 
Shiluva 
Shuia 
Shuiskaia 
Shumsk 
Siauliai 
Sibir 
Sich 
Silistria 
Sinel'sk 
Siniia Most 
Siret 
Slobodskoi 
Slonim 
Sluchesk 
Slutsk 
Smolensk 
Smotrych 
Snovsk 
Soldaia 
Solkhat 
Solodovintsy 
Sologyn' 
Solovetskii 
Sol' Kamskaia 
Sol' Velikaia 
Sol' Vychegodskaia 
Sorskii 
Sosnitsa 
Spash 
Spaso-Kamennyi 
Staraia Rusa 
Staraia Ladoga 
Staritsa 
Starodub 
Starodub Riapolovskii 
Starodub Severskii 
Stolp'e 
Strezhev 
Subak 
Suceava 
Sugrov 
Sumskii Ostrog 
Surgut 
Surozh 
Suteisk 
Suzdal' 
Sviatoslavl' 
Sviiazhsk 
Svinort 
Svinukhi 
Sviril'sk 
Syzran' 
Tabriz 
Talinn 
Tana 
Tanais 
Tannenberg 
Tara 
Tartu 
Tarusa 
Tatinets 
Tebriz 
Teliutsy 
Temnikov 
Terebovlia 
Terebovl' 
Terskii Gorodok 
Teshilov 
Tesov 
Tetiushi 
Tighina 
Tikhomel' 
Tikhoml' 
Tismianitsa 
Tiumen' 
Tlebov 
Tmutarakan' 
Tobol'sk 
Tolmach' 
Tomakivka 
Torchesk 
Torchev 
Toropets 
Torun 
Torzhok 
Totma 
Tovarov 
Trakai 
Trepol' 
Tripol' 
Trubchevsk 
Trubech 
Trubetsk 
Truso 
Tsaritsyn 
Tsar'gorod 
Tula 
Tumashch' 
Turau 
Turiisk 
Turov 
Tver' 
Tysmainitsa 
Ufa 
Ugleche Pole 
Uglich 
Ugrovesk 
Ukani 
Ukek 
Ukhtoma 
Unenezh 
Unzha 
Ushesk 
Ushitsa 
Ust-Nem 
Ustilog 
Ustiug 
Ustiug Velikii 
Ustiuzhna 
Ust'-Emtsa 
Ust'-Pashi 
Ust'-Tsil'ma 
Ust'-Vaga 
Ust'-Vym' 
Ust'e 
Usviat 
Uvetichi 
Uzmen 
Valaam 
Valaamskii 
Valuiki 
Varguza 
Varin 
Varniai 
Vasilev 
Vasilev Galitskii 
Vasilev Smolenskii 
Vasil'ev 
Vasil'sursk 
Vekshchen'ga 
Velikie-Luki 
Velikii Ustiug 
Venitsa 
Vereia 
Vereshchin 
Verkh 
Verkhne-Chusovskii 
Verkhnii Chusovskii 
Verkhoture 
Verkhotur' 
Verkhovsk 
Vernev 
Verzhavsk 
Vetskaia 
Viashnia 
Viatka 
Viazma 
Viaz'ma 
Viipuri 
Vilna 
Vilnius 
Vil'iandi 
Vinnytsia 
Vitebsk 
Vitembsk 
Vitsebsk 
Vitichev 
Vitrik 
Vizna 
Vladimir 
Vladimir-na-Kliaz'ma 
Vlodava 
Vobonitsa 
Voiadoutov Pogost 
Voin' 
Voldutovpogost 
Voldyzh 
Volkovyisk 
Volodarev 
Volodymyr 
Vologda 
Volok Lamskii 
Volok Slovinskii 
Voloklamskii 
Volokolamsk 
Volyn' 
Voni 
Vorobiin 
Vorob'in 
Voronazh' 
Voronezh 
Vorotynsk 
Vosporo 
Vosviach' 
Vovogrudok 
Vrochnitsy 
Vruchi 
Vruchii 
Vsevolozh 
Vsevolozh Chernigovskii 
Vshchizh 
Vyborg 
Vygoshev 
Vyr' 
Vyshegrad 
Vyshgorod 
Vyshnii Volochek 
Vzdvizhen' 
V'iakhan' 
Warmia 
Zapol'skii Iam 
Zaporozh'e 
Zarechesk 
Zaroi 
Zartyi 
Zarub 
Zarytyi 
Zbarazh 
Zbyrazh 
Zdvizhden' 
Zennia Velikaia 
Zhabachev 
Zhelan' 
Zheldi 
Zheldia 
Zhelian' 
Zhelni 
Zhelnii 
Zhidchichi 
Zhidichin 
Zhitomir 
Zhizhets 
Zhmuda 
Zhytomyr 
Zopish 
Zubtsov 
Zvenigorod 
Zvenigorod Chervenskii 
Zvenigorod Kievskii 
Zvizhden 
 * 
Spanish (female names) -- Agueda Aldonza Ana Angelina Antonia Barbola Bartolomeba Beatriz Belit 
Bernardina Berta Blanca Bona Catalina Clara Clarencia Columba Constanza 
Cristina Domenga Eldonza Elena Elo Elvira Enderquina Ermengarda Ermesinda 
Estefania Eva Fafila Fatima Felipa Florinda Francisca Fronilde Gelvira Gotina 
Gracia Gundisalba Guntroda Ildaria Ines Isabel Isabelica Isabella Jimena 
Juana Leonor Lucia Luisa Lupa Madelena Mafalda Mansuara Maneula Margarita 
Maria Marina Mariana Marinella Mayor Mencia Paterna Placia Sancha Scemena 
Serena Taresa Teoda Tegrida Teresa Toda Urraca Velasquita Victoria Violante 
Yolande Ysabel 
 * 
Spanish (male names) -- Adulfo Agustin Alfonso Almerique Alonso Alvaro Alvito Amentario Ambroz Amor 
Andres Angel Anselmo Ansuro Antonio Aznaro Arias Arnao Auderico Ballelo 
Baltasar Bartolome Beltran Berenguer Bernaldino Blas Borredan Castañon Cino 
Cosme Cresconio Cristobal Damian Diego Domingo Dulcido Ederoño Eliseo Emilio 
Enrique Ermegildo Escobar Esteban Estevan Fadrique Fagildo Felipe Fernán 
Fernando Flazino Froila Francisco García Garcilaso Gascon Gaspar Gelmiro 
Geraldo Godesteo Gombal Gomesindo Gómez Gonsalvo Gonzalo Gudesteo Gunsedal 
Guntrodo Guter Hernan Hernando Iago Iñigo Ignacio Isaio Isidro Jaime 
Jeronimo Jesus Jimeno Joaquin Jorge Juan Julio Ladron Lazaro Leon Lope Lorenco 
Lucas Luis Lugo Manuel Marcos Martin Mateo Melchor Mendo Menendo Mergildo Miguel 
Munio Muño Nicolao Nuño Ochoa Olivar Ordoño Osmundo Osoro Oveco Pancho 
Pascual Pedro Pelayo Pepi Rafael Raimundo Ramiro Ramón Ranemiro Rapinato 
Recaredo Recessvindo Rodrico Rodrigo Ruberte Rubin Salamo Salvadór Sancho 
Santiago Sanzo Sarracino Sebastian Sesmiro Silvestre Suero Tadio Telo 
Teodemiro Tiago Timoteo Tomas Tome Vedillo Vela Velasco Vellito Vermudo Vidal 
Vimaro Vincente Vistruario 
 * 
Spanish (surnames) -- Abarca Acarons Acosto Acuoa Adega Agassiz Agia Aguero Aguila Aguilar 
Aguilera Aguirre Aicega Alaminos Alas Albanese Albano Albarracin Albenino 
Albo Albornos Alcantara Aldama Alday Alderate Alegria Aleixandre Alemany 
Alemao Alkorta Allende Almagro Almendros Almeyda Alonso Altamirano Alvarez 
Alvaro Alvear Alzugaray Amatrian Amavisca Amendola Amor Anaya Andrés 
Angeloz Angulo Antón Antuma Aragon Aranda Arbizu Archuleta Arellano 
Arispana Aristia Aristizabal Aristzabal Armas Armendariz Armesto Arnal 
Arrese Arria Arruti Arrutti Arteaga Arzu Ascencion Aspe Asprilla Avellanos 
Ayala Azaria Aznar Badillo Bajtera Balbi Balbo Balboa Ballestero 
Ballesteros Balmaceda Banderas Barón Baradez Baraona Barba Barchitta 
Bardem Barjuan Barreiro Barrenechea Barrichello Barrigo Barrio Basadre 
Batistuta Batiz Batran Bauza Bazán Bea Becerra Becerril Belaúnde Bello 
Beloki Belsue Beltrones Benavides Benet Benitez Benítez Berganza Bergasa 
Berlanga Bermejo Bermudez Bernabé Bernal Bernaldez Beto Blanco Boedo 
Bolano Bolea Borges Borrego Bote Braga Bravo Bricenos Brizeno Brizuela 
Brugera Bueno Buey Bugeja Bunuel Burillo Burrieza Busques Bustos Caballero 
Cabellero Cabello Cabral Cabrero Cacares Cacho Cachon Cadena Cadero Cafu 
Caio Calatrava Caldern Calero Calvo Camacho Camardan Camardiel Camillo 
Caminero Campo Campos Canal Canans Canas Canellas Canesa Canete Cano 
Canseco Canura Carabali Carballo Carbajal Carbonell Carbonero Cardenas 
Carderas Carmona Carranco Carranzo Carrasco Carreno Carrera Carretero 
Carriedo Carriles Carrión Carvalho Casald liga Casamayor Casas Cascos 
Casero Caso Castanada Castaneda Castilla Castillo Castrejana Castrilli 
Castro Castulo Catalan Catano Cea Ceballos Celades Centurión Cerezo Ceron 
Cervano Cervantes Cervera Chamot Chapado Chascarillo Chaves Chavira Checa 
Chedraui Chiamuhera Chiamulera Chico Chilavert Chillida Cieza Cisneros 
Clavet Clavijero Clemente Clopes Cobo Coceres Coelho Cois Cojuango 
Concepcion Contreras Corbacho Corbelan Cordero Cordobes Cordona Cordova 
Cornejo Coronel Corral Corrales Correa Corretja Cortez Cortina Corvalan 
Coya Cresaco Crespo Criado Criville Crusellas Cruz Cubrero Cuéllar Cuello 
Cuenca Cuervo Cuesta Cúneo-Vidal Da Gama Dabino Damián Dávila Dávilo 
Davino De Alencar De Assis De Cabrea De Caxias De Jesus De La Garza De La 
Iglesia De la Madrid De La Rosa De la Vega De Lacerda De Mena De Mendoza 
De Rosas De Saldanha De Sancha De Souza De Tavira De Urquiza De Vivero 
Debarros Deferr Delmorales Delgado Diago Diaz Diego Diez Disegni Dominguez 
Dorantes Duany Duce Dunga Durán Echave Echevarria Edinho Edo Elcano 
Elixaeberna Elizondo Enciso Enrique Enriquez Erosa Escamillo Escartin 
Escriba Escriva Escuda Escudero Espejo Espinosa Espinoza Estay Estete 
Estrada Fabrega Fajardo Falcón Falla Fanez Farfán Feijoo Fejos Felipe 
Feliu Feo Fernandez Ferrado Ferrer Figo Figueira Filho Fiz Flores Foa 
Fonseca Formoselle Fraga Francia Francisquito Franco Frechilla Frois 
Fuente Fuentes Fuginato Funes Gaite Galdarres Galíndez Galindo Gallardo 
Gallego Gama Gamarra Gamboa Gamez Gangotena Garbajosa Garcia Garcilaso 
Garibay Garrido Garza Gasco Gaspar Gasquilan Gastelú Gats Gavaldon Gavilán 
Giberau Gibernau Gil Ginez Girón Gomez Gomiz Gonzaga Gonzales Gorriz 
Granados Grande Guadalcázar Guaman Guardiola Guerra Guerrero Guerro 
Guevara Guillen Guimarães Guinassi Gumy Guruceta Gutiérrez Guzmán Henarez 
Henchoz Heras Hernandez Herrera Hidalgo Hierro Hoero Holguin Hoyos Huerta 
Huertas Hurtado Ibanez Icaza Idalia Iguaran Illan Imaz Iniguez Insulza 
Isambero Isasi Ivarra Jara Jaramillo Jimenez Juarez Jura Jurada Juradu 
Keiva Kiko Lamela Lanos Lapenti Lara Laredo Larraneta Lastres Lazaro 
Ledesma Len Leon Leona Lerma Lianos Lima Lissón Lizarraga Lizaur Llano 
Llosa Lopez Lora Lorca Loredo Lorente Lorenzo Losada Lozada Lozana Lozano 
Lucero Luenga Luna Lurdes Madero Magallanes Magarino Maldonado Mamita 
Manderiaga Manjarin Manrique Mantilla Manzo Marcari Marcelo Mardomingo 
Margas Marín Marquez Márquez Marquina Marroquín MartÍ Nez Martel Martinez 
Marulanda Masoliver Masolta Massana Mattez Matutes Mauri Mayo Mayor 
Mayoral Mazinho Medina Meira Mejia Meléndez Melgarejo Meligeni Meligenis 
Mena Menchu Mendez Mendieta Mendiluce Mendiola Menem Meolans Merino Merlo 
Merry Mery Messia Methol Mezo Miguez Milian Minguet Minoza Miralles 
Miranda Molina Mondello Monentes Monreal Montalvo Montana Montaña Montanes 
Montano Montavez Monte Montejo Montero Monteros Montez Montolio Montoro 
Montoya Montt Mora Moraga Morales Moran Morayta Morcillo Morel Moreno 
Moretta Morgovejo Morientes Moscoso Motolinía Moya Muniz Munoz Murillo 
Muro Murrieta Nabarro Naca Nadal Nadol Naharro Narvaez Nascimento Navaez 
Navarrete Navarro Nino Noboa Nolasco Noseda Núnez Obrégon Ocacio Ocampo 
Ochoa Odriozola Ogarrio Olano Olazabal Oliva Olmedo Olmos Oneta Oraglio 
Ordialds Ordiales Ordonez Orellana Orgónez Orizaga Orozco Orrego Ortega 
Orteguilla Ortiz Osario Osorio Otero P¾ rez Pacheco Padilla Paez Páez 
Palacios Palencia Pancorbo Pando Pánfilo Paraguez Pardo Paredes Pareds 
Parilla Parra Parrado Parraguez Parreira Parrera Pascual Passarella 
Pastrana Patino PeÔ alves PeÔ as Pedroso Pelaez Perahia Perdiguero Perez 
Periate Pessoa Peyrera Piedrahita Pimental Pineda Pinedo Pinelo Pinero 
Pinilla Pinto Pinzon Pinzón Pires Pita Pitillas Piza Pizarro Pizzaro 
Platas Plazas Poblano Polanco Polo Porcallo Porcel Porras Portas 
Portocarrero Posados Pracatan Prado Preciado Preciosa Pretel Prieto Prieto 
Puente Puerta Pueyo Puig Puiggros Pulido Puron Quehlas Queiro Querido 
Quesada Quesado Questi Quevedo Quintero Quirot Rudenas Rabal Radondo 
Raimondi Ramiez Ramirez Ramos Rangel Rascon Rebuelta Recio Regal Rengifo 
Restrepo Reto Rey Reyes Riba Ribadeneyra Riberol Rimoldi Rincon Rionda 
Riquelme Rivas Rivera Rivero Roa Robaina Robano Robledo Roca Rocha Rodas 
Rodriguez Roig Rojas Rojo Rollero Roma Romero Romo Rosas Rosauro Rovapera 
Rozadilla Ruano Rubiera Rubio Rubios Rubruquis Rucina Rudas Rufo Ruiberriz 
Ruiz Saavedra Sáenz Saer Sagredo Sahagún Sala Salarza Salas Saldamando 
Salgada Salinas Salto Salvatella Sámarez Samper Sanchez Sanchis Sandoval 
Sanin Sanroma Santa Maria Santana Santiago Santiseban Santoro Santos 
Santoyo Sanz Sarabia Sarmiento Sarsola Seabra Sebrian Seda Sedeno Segarra 
Segurola Senra-Silva Sepúlveda Sergi Serna Serpa Serra Serrano Sert Servia 
Siculo Sieres Sierra Siliceo Silloniz Soitino Solana Solano Soldadera 
Soldan Solozano Sortani Sosa Sota Sotelo Sotomayor Spano Sporleda Suarez 
Suárez Tallez Tabuyo Tadena Taffarel Tapia Tarrega Tavares Tejada Tello 
Teofilo Terezinho Terranova Terrazos Terreros Texidor Tintorero Toldeo 
Toledano Toledo Tomas Toriano Toribio Torrens Torres Torrillas Tortosa 
Tovar Troncoso Trujillo Tuero Tuscarora Ubidia Ugarte Ulloa Unzues Urrutia 
Urteaga Ussica Vaime Valcárcel Valderrama Valencia Valente Valenzuela 
Valero Vales Valignano Vallalpando Valle Vallejo Vaquero Varela Vargas 
Vargos Vasques Vazquez Vega Vegaso Vegros Velasco Velazquez Vélez Véliz 
Velloso Vendabal Vennera Vera Verdugo Verme Veron Viana Vicario Vila Villa 
Villalobos Villar Villaroel Villaruel Villasenor Viloca Vinciguerra 
Vingade Viola Vivas Vizcaino Volonte Vrais Woriega Xalmiento Xerez Ximenéz 
Xovi Xuárez Yanez Yegros Yocemento Yuste Zabaleta Zaga Zamarilla Zamorano 
Zapata Zavarjelos Zerbino Zhili Zorita Zuaza Zubero Zubizarreta Zuidema 
Zuluaga 
 * 
Sumerian (words) -- an kia nam tartarreda lagaskie me galla sag ansze miniibil denlile en 
dningirsusze igi zi muszibar urumea nigdu pa name sza gubi namgi sza 
denlilla gubi namgi sza gubi namgi agi uru nammul imilil sza denlillake 
ididignaam duga namtum ee lugalbi gu bade eninnu mebi an kia pa muakke 
ensi lu gesztu dagalkam gesztu igaga nig galgalla szu minimumu gu du 
masz dure si imsasae sig nam tarra sag musziibil ku dude gubi musziibzi 
lugalniir une maszgika gudea en dningirsura igi muniduam eani duba 
munadu eninnu mebi galgallaam igi munanigar gudea szagani suraam inime 
minikuszu gana ganaabdu gana ganaabdu inimba hamudagub sipame namnunne 
sag maabsumsum nig maszgike maabtumaga szabi nuzu amagu mamugu ganatum 
ensi kuzu metenagu dnansze nin dingir siraratagu szabi hamapade 
magurrana giri nammigub uruni ninakisze idninakidua ma muniri idde 
hullae kurku isiile bagara idde laae imtiata ninda gisz bitag szed ide 
lugal bagarara munagin szud munasza ursag pirig ziga gabaszugar nutuku 
dningirsu abzua gal di nibrukia nirgal ursag maadu szu zi gamuraabgar 
dningirsu ezu gamuradu me szu gamuraabdu ninzu dumu eridukige tuda 
nirgal metena nin ensi dingirreneke dnansze nin dingir siraratagu 
giribi hamagaga gu deani gisz batukuam lugalani siskur razuni gudeaasz 
en dningirsuke szu basziti bagaraka eszesz iak ensi dgatumdusze kinuani 
bade ninda gisz bitag szed ide ku dgatumdura munagen siskur munabe 
ningu dumu an kuge tuda nirgal metena dingir sag zi kalamma tila nudu 
zu uruna nin ama lagaski ki garrame igi ugsze uszibarrazu szegx 
hegallaam szul zi lu igi mubarrazu namti munasu ama nutukume amagu zeme 
nutukume agu zeme agu szaga szu banidu unua itue dgatumdu mu kuzu 
dugaam gia maninu giszad galgume zagu muus negibar galla duame zisza 
muszinigal andul dagalme gissuzusze ni gamasziibte szu mahza saga 
azidabi ningu dgatumdu gara hamuuru urusze idue giskimgu hesa kur ata 
illa ninakisze udug sagazu igisze hamagen dlama sagazu giria hamudagen 
gana ganaabdu gana ganaabdu inimba hamudagub amagu mamugu ganatum ensi 
kuzu metenagu dnansze nin dingir siraratagu szabi hamapade gu deani 
gisz batukuam ninani siskur razuni gudeaasz ku dgatumduge szu basziti 
magurrana giri nammigub uruni ninakisze kar ninakinake ma bius ensike 
kisal dingir sirarataka sag ansze miniil ninda gisz bitag szed ide 
dnansze munagub szud munasza dnansze nin uru nin me ankal ankalla nin 
denlilgin nam tartarre dnanszegu dugazu zidam sagbisze eaam ensi 
dingirreneme nin kurkurrame ama inimgu uda mamuda sza mamudaka lu am 
angin ribani kigin ribani ane sagganisze dingirraam anisze 
anzumuszendam sebaanisze amarukam zida gubuna pirig inunu eani duda 
maandu szagani numuzu kiszarra matae munus am abameanu abameani sagga 
kikaradin muak gidubba kunea szu immidu dub mulan duga immigal ad 
imdabgigi kam ursaggaam mugur lium zagin szu immidu ea giszhurbi imgaga 
igigusze dusu ku igub giszuszub ku si ibsa sig namtarra giszuszubba 
maangal ildag zida igigu gubba tidigixmuszen bu lua miniibzalzale dur 
azida lugalgake ki mahurhure ensira amani dnansze munaniibgigi sipagu 
mamuzu ge gamuraburbur lu angin riba kigin ribasze sagganisze dingir 
anisze anzumuszensze sebaanisze amarusze zida gubuna pirig inunua sa 
szeszgu dningirsu ganammeam esz eninnuna duba zara maraandu kiszarra 
marataea dingirzu dningiszzida ugin kiszara maradaratae kisikil saggae 
kikaradin muak gidubba kune szu bidua dubmul duga bigallaa ad imdagia 
ningu dnisaba ganammeam ea duba mulkuba gu maraade kamma ursagam mugur 
lium zagin szu bidua dnindubkam ea giszhurba immisisige igizusze dusu 
ku gubba uszub ku si saa sig namtarra uszubba galla sig zi eninnu 
ganammeam ildag zida igizu gubba tidigixmuszen bu lua miniibzalasze 
dude igizu duga nuszikuku anszedur azida lugalzake ki marahurhurasze 
zeme eninnu mur niiskugin ki imszihure na gari narigu hedab girsuki 
esag ki lagaskisze girizu ki ibius eniggaza kiszib umikur gisz umatagar 
lugalzu giszgigir umusa anszedurur uszila giszgigirbi kune zaginna szu 
umanitag ti marurua ugin ie ankara namursagka mi umanidu szunir ki agni 
umunadim muzu umisar balag ki agni uszumgal kalamma giszgudi mu tuku 
nig ad gigini ursag nigbae ki agra lugalzu en dningirsu eninnu 
anzumuszen barbarra umunadakure tur dugazu mah dugaam szu baasziibti 
enna sza angin surani dningirsu dumu denlillaka zara marahungae giszhur 
eana marapapade ursage meni galgallaam szu maraniibmumu sipa zi gudea 
gal muzu gal igatummu inim dnanszee munadugaasz sagsig baszigar 
eniggarana kiszib bikur gisz immatagar gudea gisza mugubgub gisze mi 
ime giszmese sag bisa giszhaluubba gin bibar giszgigir zaginsze 
munaasilim dururbi pirig kase pada immaszilala szunir ki agni munadim 
muni immisar balag ki age uszumgal kalamma giszgudi mu tuku nig ad 
gigini ursag nigbae ki agra lugalni en dningirsura eninnu anzumuszen 
barbarra munadakuku ea hulla inaniku gudea esz eninnuta zalagga namtae 
kamma esze uude bidib gigi baandib dudu musiig gugar mugi ahduga girta 
imtagar szugalam ki husz ki dikune ki dningirsuke kurkurra igi 
minigallasze udui gukkal masz niga ensike aszgar gisz nuzu suba 
minidabdab ligisz usikil kurrakam izia bisisi szim erin ir 
namdingirrakam ibibi mudu lugalniir ugga munazi szud munasza 
ubszukinnaka munagub kiri szu munagal lugalgu dningirsu en ahusz gia en 
zi kur gale ria szul katar nutuku dningirsu ezu maradue giskimgu nugu 
ursag nigdue gu baade dumu denlilla en dningirsu szabi numuudazu sza 
abgin zizizu giszesigin gagazu aeagin gunun dizu amarugin uru gulgulzu 
ugin kibalsze duduzu lugalgu szazu ea nulazu ursag sza angin surazu 
dumu denlilla en dningirsu ge ana muudazu kammasze nuara nuara sagga 
munagub gir mutagtage maduna maduna ensi egu maduna gudea egu duda 
giskimbi garaabsum garzaga mulankuba gu gamuraade egu eninnu anne ki 
garra mebi me galgalmemea diriga lugalbi igi su ilil anzumuszengin sig 
giabisze an imszidubdub melam huszbi anne imus ega ni galbi kurkurra 
muri mubie anzata kurkurre gu immasisi magan meluhha kurbita immataede 
ge dningirsu ahusz gia ursag gal ki denlillaka en gabari nutuku egu 
eninnuga en kurra abdiri tukulgu szarur kur szusze gargar igi huszagu 
kurre nuumil abadagu lu labatae akugu namgal ki agda lugal amaru 
denlilla igi huszani kurda nuil dningirsu ursag denlilla musze musa me 
za minikesz giszbanszur muil szuluh si bisa szu si saagu an kuge ua 
bazige nig szuga dugaam akugu dugabi mugu an lugal dingirreneke 
dningirsu lugal iszib anna musze musa tiraasz abzugin namnunna ki 
immanigar szabia ituda usakarra me galgal ezem annagu szu gal madudu 
ehusz ki huszgu musz huszgin kiszurra bidu kibalga numiibduga szagu 
umszimiria musz ze guruagin usz maausuhe ebarbar ki aggaga ki dutugin 
dallaaga kiba distarangin di uruga si baniibsae bagara ki banszurragu 
dingir galgal lagaskiakene gu masisine egu esagkal kurkurra azida 
lagaski anzumuszen anszarra sig gigi eninnu namlugalgu sipazi gudea szu 
zi maszitumda ansze szegxe gu bade anta hegal hamuratagin uge hegalla 
szu headapesze ega usz ki garrabida hegal hedagin gana galgale szu 
maraabile pa gubi maraabzizi dudu ki nueda maraede kiengire diri mudade 
siki diri mudala temengu masigena egu szu zi maszitumda hursag ki immir 
tuszasze girigu ki ibius nita dirike immire hursag ki sikilta im si 
maraabsae uge ziszagal umasum ludili luminda kig mudaakke giana iti 
maraee enegan umadam maraee ude maradue gie maraabmumu sigta giszhaluub 
gisznehadu murataede iginimta giszerin giszszuurme giszzabalum nibia 
maraantum kur giszesiaka giszesi maranitum kur naka na gal hursagga 
lagabba marakue ubia azu izi bitag giskimgu hamuuzu gudea izi usagaam 
ihaluh mamudam inim duga dningirsukasze sag sig baszigar masz barbarra 
szu mugidde masza szu igid maszani isa gudea sza dningirsuka udam munae 
gal muzu gal igatummu ensike uruna lu diligin nari banigar ki lagaskie 
dumu ama aszagin sza munaasze gisz szu mudu giszad muzi guru mugar inim 
duga bigi szerda eba immaangi usaan barussa eme idu siki udugannakam 
szua minigargar amaa dumuda gu numadade dumuu amanira kadua numanadu ir 
agisztag tukura lugalani sag numadadub geme lu namra hul munaaka ninani 
igina nig numunanira ensi eninnu dura gudear gugarbi luu numanigar 
ensike uru muku izi immatala usugga ni gal lugian uruta batae pisan 
uszubbasze masz baszinu sig masze bipa kaalbisze igi zi baszibar sipa 
mu pada dnanszeke namnunna igar pisan uszubba gisz bihurrani kaal 
namnunna munigarrani anzumuszen szunir lugallanakam urisze bimul sze 
uru munakuge munasikile ligisz sikil kurrakam izia bisisi szim erin ir 
namdingirrakam ibibi mudu siskurra munaagal gi szudde munazale danunna 
ki lagaski dningirsuka dude gudea siskur razu mudaanszuszugeesz sipa zi 
gudea hullagin immananiibgar uba ensike kalammana ziga banigar mada 
gusag szarszarrana guedinna dningirsukaka ziga banigar uru dua adam 
garrana gu giszbarra dnanszeka ziga banigar gu husz ziga gabagi nutuku 
giszerin barbarra lugalbiir dabba imrua dningirsukaka ziga munagal 
szunir mahbi lugalkurdub sagbia mugin kiagal gabagal ata ea idmah diri 
hegalbi barabara imrua dnanszeka ziga munagal ku szunir dnanszekam 
sagbia mugin gu maszansze edinna laa niisku bir mu tuku bir dutu kiag 
imrua dinannaka ziga munagal aszme szunir dinannakam sagbia mugin 
dningirsuka dude gir szuni x gar elam elamta munagin szuszinki 
szuszinta munagin magan meluhha kurbita gu gisz munaabgal dningirsuka 
dude gudea uruni girsukisze gu munasisi dninzagada mudaag urududani sze 
mah tumagin gudea lu duara munaabuse dninsikilada mudaag giszhaluub 
galgal giszesi giszabbabi ensi eninnu dura munaabuse kur giszerinna lu 
nukukuda gudea en dningirsuke gir munanigar giszerinbi gin gale immiku 
szarur azida lagaskia tukul amaru lugallanasze gin immabar musz maham 
ae imdirigaam hursag giszerin ad giszerinna hursag giszszuurmeta ad 
giszszuurme hursag giszzabalummata ad giszzabalum giszusuh galgal 
gisztulubuum giszeranum ad galgalbi diridirigabi kar mah kasurrake 
gudea en dningirsura immanaus kur na lu nukukuda gudea en dningirsuke 
gir munagar na galgalbi minitum ma hauna ma nalua esir abaal 
esirigiesir imbarbarra hursag maadgata nigga ma szegana tumagin gudea 
en dningirsura immanaus ensi eninnu dura niggalgale szu munaabil hursag 
uruduke kimaszta nibi munaabpa urudubi gidirba munibaal lu lugalna 
dudam ensira kusig kurbita saharba munatum gudea kunea kurbita 
munataede gug girine meluhhata szu munapesze kur nuta nu munataede 
sipade kuga mudue kudim imdatusz eninnu za mudue zadim imdatusz urudu 
naggaa mudue szidsimug dnintu kalammake iginisze si imsa naszu udaam 
sig munaabgi naesi naszuke min pesz giszkinti nam szemah deagin 
munanigar u munagid gigi dugud munagid nam duda lugallanasze gianna 
nuumkuku uanbara sag numiibdue igi zi barra dnanszekam denlilla lu 
szaganakam ensi szage pada dningirsukakam gudea unu maha tuda 
dgatumdugakam dnisabake gesztuke gal munatak ea denkike giszhurbi si 
munasa melambi anne ussa mebi an kida gu laa lugalbi en igi husz ilil 
ursag dningirsu me gal zubi eninnu anzumuszen barbarsze gudea sigta 
baszigin nimsze bidu nimta baszigin sigsze bidu ganazidam esz igargar 
aba gisz bigar niteni muzu hullagin immananiibgar uteam libirraasz 
razua bagin gudea bara girnunnata sza munahungae imzal mutu aszunaga 
meteni mugi dutu hegal munatae gudea kamasz uru kuta immatae gu du masz 
dure gisz bitag ee immadu kiri szu immagal dusu ku giszuszub zi 
namtarra eninnuta muil bi mula sag il mugin dlugalkurdub igisze munagin 
digalimke gir munagaga dningiszzida dingirrani szu mudagalgal pisan 
uszubka asaga iak ensira adab siim ala munaduam kaal sigbi sag immidu 
lal inun ihinunna al immanitag szembulugszimxpi giszhia uhsze immiak 
dusu ku muil uszube immagub gudea im uszubba igar nigdu pa bie ea sigbi 
pa munigaga kurkurre mudasue erin mudasue uruni ki lagaskie selia 
mutiniibzale uszub mudub sig hadde baszub kaal imturinabasze igi zi 
baszibar szimxpi haszuur szembuluga sag immanidu sig uszubba 
munigarrani dutu imdahul agari idmahgin zigana lugal denki nam 
munanitar sig mugar uszub ea iku pisan uszubbata sig batail men ku anne 
illa sig muil uggana mudu bir ku dutu sag baledam sige esze sag illabi 
ab dnanna turba rinrindam sig mugar ea mududu ea giszhurbi imgaga 
dnisaba sza szid zuam lutur gibilbi dugin igini duga nuszikuku ab 
amarbisze igi gallagin esze tetema imszigin lu ninda tur kaa gubbagin 
dudue nuszikuszu sza lugalna udam mue gudeaar inim dningirsuka uriam 
mudu sza gu di dudakana gu nigsagaa lu maagar hullagin immananiibgar 
masza szu igid maszani isa amire sze basi igibi si ibsa gudea sagsze nu 
munu inim manatae lugalnaka dubi eninnu an kita badbi igia munaagal 
hullagin immaniibgar gumubara me szu imdudu usga kuge esz mugaga ea 
denkike temen musige dnansze dumu eridukike eszbarkigga mi banidu ama 
lagaski ku dgatumduke sigbi kurkua munitu dbau nin dumusag annake szim 
erinna banisu ee en bagub lagal bagub mee szu si immasa danunna dide 
immaszuszugeesz gudea lu duake ea dusubi men ku sagga munigal usz mugar 
agar ki immitag sa musi sigga gu bidub ea sa nam nammisi gu kurunba 
saggallaam ea sa am nammisi anzumuszen amara timuszenam ea sa nammisi 
pirig tur pirig husza guda laam ea sa am nammisi an sig sulim illaam ea 
sa am nammisi sa duga hili guram ea sa nammisi eninnu iti zalla kalam 
siam giszkana imgagane an sigga men illaam giszkanata batatusz emah 
anda gu laam mudu gisze immaru bugin dnanna sagkesz denkikam hursaggin 
immumune muruxgin dugud anszage imminiibdiridirine gugin si 
immiibililne giszgan abzugin kurkurra sag baniibilne ee hursaggin an 
kia sag ansze miniibil erin duru ki ukal muaam eninnu sig kiengiraka 
hili muniibdudu ea gisz imgaga uszum abzu teszba ededam ka anna 
immiibuudam musz mah hursagga sim akam gi gurubi musz kurra teszba nuam 
satubi erinduru haszuurra szu hetaggaam ginerin igi dibia erin barbar 
imgagane szim zi ihinunka mi baniibene imduabi hinun abzu szu taggaam 
anigkabi imsziibla esz eninnu szudag anna gu gargaraam ensike mudu mumu 
kur galgin mumu temen abzubi dim galgal kia minisisi denkida eangurraka 
sza mutiniibkuszu temen anna ursagam ee immidab kianag dingirreneka 
imnanaa eninnu dim gal mugi abgalbi mudu uruna giszasal dubi mudu 
gissubi mula giszszarurbi uri galgin lagaskida imdasi szugalam ki 
huszba imminigar suzi bidudu bara girnunna ki di kuba ua lagaski gu 
galgin bailil na galgal lagabba minituma mu dilia mutum mu dilia muak 
numadaabzal uda ta mudu kammaka ee immidab na dabi kunsze munu szimsze 
mudimdim ea miniszuszu na kisal maha miniduana narua lugal kisal si 
gudea en dningirsuke girnunta muzu naba musze immasa na kasurra bidua 
lugal amaru denlilla gabaszugar nutuku gudea en dningirsuke igi zi 
muszibar naba musze immasa na igi uea bidua lugal gu di denlilla en 
gabari nutuku gudea en dningirsuke sza kuge bipa naba musze immasa na 
igi szugalammaka bidua lugal munisze kurkukue gudea en dningirsuke 
guzani mugi naba musze immasa na igi euruagaka bidua gudea en 
dningirsuke nam du munitar naba musze immasa na aga dbauka bidua eninnu 
igi annake zu dbau ziszagal gudea naba musze immasa lugalna zideesz 
mudu sipa zi gudea an ki imdamu usakar gibilgin men biil mubi kurszasze 
pa bie gudea dningirsuka dutugin muruxta dugud batae hursag zaginnagin 
mumu hursag nu barbarragin dide bagub dublabi amgin muszuszu uszumbi 
urmahgin szuba binunu gigunbi abzugin ki sikile bimu uribi dara ku 
abzugin si bamulmul usakar gibil anna gubbagin gudea dningirsuka dide 
bagub ea dublabi szuszugabi lahama abzuda szugaam gisz garrabi agi 
ambar mah musza siggaam ka giddabi anbarbarra ni gallaam e dullabi nu 
anszage dirigaam ka ki lugal kubita huriin amsze igi ilildam giszti kae 
ussabi niranna anne ussaam giszka annabi eninnu duba gu di teszba 
gubbaam sigigi ni gurgurabi igi di dingirrenekam ea agabarbar murugune 
hursag zaginna an kia ki heussaam kingi unugal mugagane bur kusig lal 
gesztin dea anne szugaam enuda mudune kur szarda mes ku abzua kurun 
illaam mudu szu imtagarrata sza dingirrene gubi giaam sipa zi gudea gal 
muzu gal igatummu aga tukul la ka meba ursag szeg sag sagarbi 
immaabdabbe igi urukisze ki ni gurba ursag am immaabdabe szugalam ka 
melamba uszum sabi immaabdabbe igi ue ki nam tarreba szunir dutu sag 
alimma immadasige kasurra igi diba urmah ni dingirrenekam immaabdabe 
tarsirsir ki agba kulianna urudubi immaabdabe aga dbau ki sza kuszba 
magilum gu alimbida immaabdabe ursag ugga imeszakeesz kabi kianagsze 
mugar mubi muru dingirreneka gudea ensi lagaskike pa bania ig giszerin 
ea szugabi diszkur anta gu nun didaam eninnu sagkulbi bad gisznukuszbi 
urmah sigarbita muszszatur musz husz amsze eme ede gadu ige uussabi ug 
pirig banda turtur szuba turunaam ea gag giszur ku musigene uszum lusze 
szu ibgarraam igba esz ku imlane dnirah ku abzu daraam sa laabi keszki 
arattaki na rigaam sa duabi pirig huszam kalamna igi miniibgal dilidu 
igibi numadibbe eninnu nibi kurkurra tuggin imdul kunea anne ki garra 
szim zida szu tag duga szeerzi annaka itigin ea igibi kur gal ki ussa 
szabi namszub szirhamun barbi dingir emah hegalla ziga guen barrabi ki 
di ku danunnakene alalbita gu szudda szukubida hegal dingirrenekam uri 
eda sisigabi anzumuszen kurmuszada hebadraam eninnu imbi imhamun 
idedinta eda lugalbi en dningirsuke sza kuge bipa szim zigin sagga 
miniibde gudea szeerzi annaka szu tag banidu egubita ku ga ku gir 
mahbita gug gal si gal girpanabi gu gu udu gu ki szukubi usga abgaga 
nesagbi kur gesztin bibize ebappirbita ididigna uba gallaam niggabia za 
ku nagga giszgigirrabi kur kia gubba aga balagabi gu gunun di kisalbi 
szud ku siim ala kun na ea nuabi hursag ul nunneesz nuam kun nagga 
ganunsze daabi nu kursze igi su ildam giszkirimiedin esze sigabi kur 
gesztin bibize ki imnee muam na ee dabbabi nig lugalbi da sza 
kuszkuszdam nig kisiga nigsikil abzu na rigaam szimna ea szugabi gudu 
ku nusiliggedam badsianna tumuszen turunabi eriduki simhia imeam eninnu 
tumuszene imnene andul pa galgal gissu dugaam burumuszen muszene sig 
mugigi ekur denlilla ezem gallaam ea nigalbi kalamma muri katarrabi 
kurre bati eninnu nibi kurkurra tuggin imdul lugalbi hilia idu 
dningiszzidake kigalla bidu gudea ensi lagaskike temenbi musi dutugin 
kalamma ea gu galgin sahar barra gubba iti kirizalgin unkinne sia 
hursag siggagin hili gura dide gubba eninnu kibi giaba dningirsu zami 
dningirsuka dua zami murubiim 
 * 
Swahili (words) -- kwamba una kidogo au kwamba una wingi uwe radhi ukipenda usipende 
jogoo likiwika lisiwike kutakucha wakati alipokuwa akisema wabaya waanguke 
katika nyavu zao wenyewe pindi mimi ninapopita salama alisema lori ni yake 
hali sivyo usifanye hivi hali umejua imekatazwa nalikosa ikanipata iwapo 
mtu mmoja anajenga na mwenzake akabomoa je faida yao ikiwa mtu atakuja 
akakuambia kama anayo dawa ya ugonjwa huu usimsadiki mimi huwapa wanadamu 
embe zangu wakala ukimtenda mabaya akakutoroka je naweza kukupelekea barua 
ikafika kibanda kisiwe karibu hata inzi wakaweza kutoka na kufika nymbani 
linalofaa ni kumwonyesha akajua wapi yule mtu aliyekuja akamnunua mbuzi 
tulisikia jinsi alivyoomba akapata bwana alipoingia wale watu wakasimama 
wakamwamkia shauri njia hii ni ndefu kupita njia tuliyoifuata tulipokuja 
machungwa haya mazuri kushinda yale mengine ali mrefu kuliko juma fedha 
mtoto aliyesoma kitabu vitabu nisivyovisoma waliokula mikate anguka amkia 
aga potea pungua jana kesho vizuri vyema vibaya chakula shoka shamba simba 
ukanda karatasi rangi siri chuki piga kura mjumbe hesabu uchaguzi abudu 
acha achilia alasiri alfajiri alhamisi amsha arobaini asante askofu 
baadaye barabara baraka baridi biashara bidii bustani changanya cheka chemka 
chinja chukiza chumvi chungu chwa dafu dakika demani dhuru edashara eleza 
elimika endelea eneza fagia fanya farasi ficha fikiri frasils fuatisha 
fukuza fundisha furaha ganda gawanya gogota gunia habari halafu hamsini 
haribu hatimaye hesabu hiki hodari huruma huyo huzuni ibada imarisha inama 
ingawa inzi itika iva iwapo jaribu jicho joka jora jumba juzijuzi kahawa 
kaskazi kauka kesho kiarabu kibaba kidole kihindi kiingereza kijana kipofu 
kiswahili kiwanja korija kubali kusudi kwanza kwenu labda lainisha lewa 
lugha maana mahindi magharibi maisha mamlaka mamoja maskini matukano mate 
matusi mbegu mbolea mbona mchaguzi mchawi mfalme mganga mgomvi miongoni 
mjinga mkate mkristo mlinzi mnyama moshi mpaka msemaji mshipa mstari mswaki 
mtumishi mtumwa muhimu mvinyo mwafrika mwanafunzi mwenendo mzazi nanasi 
ndege nguvu njiwa nyamaza nywele nzige ogalea ondoka ovyo oza pambazuka 
patikana pendelea pendeza pengine penye pewa pima pinga punguza rafiki rudisha 
ruka sababu sabuni sadiki safisha salamu sanduku seremala serikali shilingi 
shoka shusha stahere subulkeri sukari swali tafadhati takata tangaza telemka 
tembo thelathini timiza toroka tuliza tunza twiga uchaguzi ufagio ugomvi 
ugonjwa uharibifu uingereza ulaya umri unguja usahaulifu usitawi uvumba 
uwingu uzazi vema vibaya vigumu vuka walakini wale wekundu wezesha wiki 
winda wokovu yayo yeye zabibu zamani ziba zidisha zoeza zunguka 
 * 
Tamil (words) -- poonaiyonRu kuttithanai vaayil kavvi adhanudalai muzhuvadhaiyum naavaal 
nakkum piLLaiyum koor nagaththaal thaayay keeRi anbadhanai avvaaRe 
veLiyE kaatum thirudivandha meen thuNdai thinRuvittu caththathaal en 
thookam kalaiththuvittu thayum ceyum moolaiyil aNaindhukoNdaargaL 
thoongaamal naanum unnai ninaindhukkoNdirukkiRen ammaa natpudan 
iLavarasi sindubaad kannathil aRainthu pin vizhiththukkonda sindubaad 
kuRRa uNarvil kappalai nOkki nadanthar kappalil iLavarasiai kaaNaamal 
Ethavathu nadanthu irrukumO endru thaviththaar pozhuthu maRiyum samayam 
yaazhpaaNa peNgaLin iniya kuralil vantha paadalgaL kEtkka kEttiraatha 
inimaiyil siRuvargaL pEsum mozhiyulum Ethu ithu en ooraaga 
irukkakkoodaatha ena thigaiththu ninRaar kaal pOna pOkilE manam 
pOgalaamaa manam pOna pOkilE mannidan pOgalaamaa mannidan pOna 
paadaiyai marandhu pOgalaamaa nee paarththa paarvaigaL kaanavodu pOgum 
oor paarththa unmaigaL unakkaaga vaazhum muyaryaaga vaazhvorkku yedhu 
naagareegam munnOrgaL sonnaargaL adhu naagareegam thirundhaadha 
uLLangaL irundhenna laabam irundhaalum maraindhaalum peyar solla vEndum 
ivar pOla yaar enRu oor solla vEndum kaal pOna pOkilE manam pOgalaamaa 
thagudhi enaonRu nanRE pagudhiyaal 
 * 
Thai (female names) -- Abhasra Achara Adung Anchali Apsara Ban 
Banjit Benjakalyani Boon-mee Boon-Nam Busaba Butri 
Cantana Catchada Chaiama Chalermwan Chanachai Chandra 
Chanhira Chanthara Chao-fa Charanya Chariya Charoen 
Charoenrasamee Charunee Chatmanee Chatrsuda Chatumas Chaveevan 
Chawiwan Chintana Chirawan Choi Chomechai Chomesri 
Chomsiri Chuachan Chuasiri Chulaborn Chumbot Churai 
Damni Dao Dhipyamongko Dok Dok-Rak Duan 
Duang-Prapha Hansa Jaidee Jintana Kaeo Kalaya 
Kamala Kamchana Kanchana Kanita Kannika Kanya 
Khae Khun Khunying Kimnai Klip Kohsoom 
Krijak Kultilda Kwaanfah Kwanjai Lalana Lamai 
Lamom Lek Lukden Ma-dee Mae Mae-Duna 
Mae-Khao Mae-Noi Mae-Pia Mae-Ying-Thahan Mai Malee 
Mali Malivalaya Maliwan Manee Mani Manya-Phathon 
Maprang Mekhala Mekhalaa Mekhla Monthani Naruemon 
Ngam Ngor Nim Nimnuan Nittaya Noi 
Noklek Noom Pakpao Petchra Phak-Phimonphan Phan 
Phara Phi Phim Piam Pichitra Pitsamai 
Prahong Pranee Prang Praphat Rajini Ramphoei 
Ratana Rochana Rutana Saeng Sangwan Saowapa 
Sarai Sarakit Savitree Sawat Simla Sirikit 
Sirindhorn Somawadi Songsuda Son-Klin Srinak Sri-Patana 
Srisuriyothai Sua Suchada Sugunya Sukanda Sukonta 
Sumalee Sumana Sunanda Sunatda Sunetra Sunisa 
Supaporn Sureeporn Talap Tamarine Thaksincha Thao-Ap 
Thiang Tida Tookta Tppiwan Tui Tuk 
Tukata Tulaya Tum Tuptim Ubolratana Um 
Ung Wani-Ratana-Kanya Wipa Wismita Yaowalak Yen 
Ying Yodmani 
 * 
Thai (male names) -- Alak Ananda Annan Anuia Anuman Anurak 
Badinton Baharn Bahn Bapit Baroma Bhakdi 
Choonhavon Bhumipol Boon-Mee Boon-Nam Burimas Burut Cha 
Chai Chairat Chaiyanuchit Chaiyo Chakri Chalerm 
Chalermchai Changsai Chanthara Chao-Khun-Sa Chao-Tak Charoen 
Charoensom Charong Chatchalerm Chatchom Chatri Chaturon 
Chavalit Chesda Chomanan  Chompoo Chongrak Choochai 
Choonhavan Chuachai Chuanchen Chuia Chula Chulalongkorn 
Chulamai Churai Chuthamani Daeng Darin Deng 
Dhipyamongkol Disnadda Ditaka Dithakar Dok Duchanee 
Emjaroen Erawan Fah Fufanwonich Gee Hainad 
Hanuman Intradit Ittiporn Jaidee Jao Jarunsuk 
Jatukamramthep Jayavarman Kamnan Kanda Karmatha Kasem 
Kasemchai Kasemsan Keetau Khakanang Khun Kiet 
Kit Kitti Kittibun Kittichai Kittichat Kittikchorn 
Kob Komalat Kongsampong Korn Kovit Kraisee 
Kraisingha Krarayoon Kriengsak Kris Krita Krom-Luang 
Kukrit Kusa Kwanchai Kwanjai Lamom Lamon 
Lap Leekpie Leekpai Lek Loesan Luk Maha 
Mahidol Malian Manitho Mee Mengrai Metananda 
Mok Mokkhavesa Mongkut Monyakul Muoi Nadee 
Nai-Thim Nak Nang-Klao Narai Naresuan Naris 
Narisa Net Ngam Nikom Nikon Nintau 
Niran Nit Noi Nongchai Noppadon Norachai 
Nuananong Nui Nung Nuta-Laya Obb Othong 
Pairat Paitoon Pakhdi Palat Panyarachun Paramendr 
Parnchand Pattama Pet Petchara Petchra Phaibun 
Phara Phinihan Phraisong Phrom-Borirak Phya Pichai 
Pichit Pira Pra Prachuab Pramoj Prasong 
Pravat Praves Praya  Pricha Prisna Proi 
Pu Rachotai Rak Ramkamhaeng Rangsan Ratanankorn 
Ratsami Sajja Sanouk Santichai Sanun Sap 
Sarawong Sarit Sataheep Satrud Sawat Seni 
Si Siam Sinn Sombat Somchai Somdetch 
Somdet-Ong-Yai Sompron Son Songgram Soo Sook 
Sophuk Sri Srimuang Su Suda Sudarak 
Suk Sulak Sum Sumatra Sunan Sundaravej 
Suntarankul Sunti Sup Suphatra Suphayok Supoj 
Supp Supsampantuwongse Suriwongse Suriyawong Sutep Tai 
Tak-Sin Tam Tau Tep Tham-Boon Thammaraja 
Thanarat Thanit Thawanya Thawi Thongkon Thurdchai 
Ti Tiloka Ting Tinsulaananda Tinsulanonda Ton 
Tong Totsakan Toy Ubol Udom Unakan 
Vajiralongkorn Vajiravudh Vessandan Vidura Wasi Wattana 
Wiset Witsanunat Wongsa Xuwicha Yai Yhukon 
Yindee Yod Yongchaiyudh Yongchaiyuth Yubamrung 
 * 
Turkish (words) -- bence herkesin anlina dogme koyalim kimin ne oldugu anlasilsin her 
degisik irktan gelene degisik dogme her halde bin cesit dogme yapmak 
lazim bence degisik ozerk bolgeye boluk herkesi mutlu yapalim mutluluk 
sadece kurtlere ait olmasin hatta orta asya dan gelmis hakiki bir parca 
toprak verelimhatta ve hatta sarisinlaraayri sahte sarisinlara ayri 
bire ozerk bolge burnunda kil olanlarla olmiyanlara vitesli araba 
kullananlarla kullanmiyanlarada birer parca toprak verelim verelim 
kardesim degil artik dogum gunu pasatsina donduk hatt ataturku hitlerle 
eslestirip oldukten sene sonra yargiliyip anitkabiri kafesle cevirip 
hapis yapalim belki bazi supheli sahislar o zaman tatmin olurlar evet 
gecen hafta nerde kalmistik neyse zaten o hikaye baymisti simdi yepyeni 
bi hikaye simdi olay soyle gelisti siz bilmezsiniz ressamlar aleminde 
kanli bicaklidirlar bigun dort tane tane sikistiriyolar allah ne 
verdiyse geciriyolar bunu goren kemalettin dogru toplandigi kahveye 
kosuyo kapiyi aciyo ve bagiriyo muharrem abi recebi assagi sokakta 
dovuyo cabuk yardima gelin diyo bunun uzerine kahvede kim var kim yok 
assagi sokaga hurra yalniz bi tanesi kahvede bele cay icmek icin kaliyo 
o da muhsin muhsin hemen evi ariyo cabuk buraya kahveye gelin bele cay 
var diyo evdekiler hemen kahveye gidiyolar yalniz bi tek ortaokula 
giden selami evde kaliyo selami de ne yapsin cani sikiliyo zaten bari 
bi sigara iciyim diyo yalniz sigarayi yakicam derken hali tutuuyo ev 
yanmaya baliyo bunun uzerine itfaiye yola cikiyo ama yolda tikaniyo eve 
gidemiyo soru itfaiyiye yolda neye rastliyo hani assagi sokakta biey 
oluyo ona mi takiliyo heyt be amerika sen nelere kadirsin insana kafayi 
uutturuyon valla selam giris su anda oldugunu anlatarak baslamak 
istiyorum dort sene evvel yazilari yaziliyordu turkiyenin cesitli 
kesimlerinden gelen seckin arkadaslar cok duzenli ve guzel bir sekilde 
fikirlerini ortaya sunuyorlardi kimse ayni fikirleri savunmuyordu ama 
saygi ile karsilikli tartisiyordu bu duzen ve guzellik gecen sene 
bozuldu kimlerin bu guzelligi bozdugunu biliyoruz ama sonra soruyorum 
su gectigimiz seneden bu yana kac tane kufurlu yazi okumak zorunda 
kaldiniz kufur edilir ama yerinde edilir bu kufurlu ortam tabii ki bir 
cok insani bir cok insan saydim daha yeni basligi ver yansin kufur 
amina koyayim cimbom orospular ve daha yuzlercesi ben durumdan 
utaniyorum bilmiyorum bu durumdan aranizda utanmayan var mi bu yaziyi 
okuduktan sonra hala altay sen haksizsin derseniz o zaman bu konu 
hakkinda baska bir sey yazmiyacagim cogunuzun bildigi gibi gecen hafta 
icinde denetim kurulu basligi altinda bir yazi yazmistim kisaca yaziyi 
ozetlemem gerekirse tartisma zemini acabilicegimizi savundum fikir bir 
denetim kurulu kurulacak ve bu kurul bazi kurallara uymayan arkadaslari 
uyaracak ayrica agiz dalasina donmus bazi tartismalari surdurenleri 
yazi ile uyaracak anlamiyanlar icin bir kez daha yaziyorum amac uyarmak 
sansurlemek degil neyse bir cok cevap yazildi bu konu hakkinda kimisi 
birbirine laf atti kimisi beni donmelik ile sucladi kimisi de 
olabilecek en kotu seyleri yazarak herkesi uyarmaya calisti ve de 
sadece bir ama sadece bir kisi cikipta ortaya sundugum fikirin uzerine 
kendi dusuncelerinide ekliyerek katkida bulundu ilk once sunu soylemek 
istiyorum yeni bir fikir ortaya atildiginda ilk once bu fikri didik 
didik ederek en olumsuz yonlerini veya zararli etkilerini bulmaya 
calisiyoruz soruyorum hakki arkadasima acaba yazinizi yazmadan evvel bu 
yeni fikrin pozitif yonleri ile negatif yonlerini kafanizda canlandirip 
fikri detayli olarak dusundunuz mu sakin bana cevap olarak sunu 
soylemeyin evet dusunduk ama hic pozitif bir sey bulamadik veya 
dusunduk tabii ki ama negatifler pozitiflerden cok daha fazla eger su 
ortaya sundugum fikrin bir tek ama bir tek pozitif yani varsa bile bu 
fikir tartisilmaya gelistirilmeye ve de gerekirse uygulamaya hak 
kazanir bana kalirsa bu fikir uygulanirsa okur ve yazarlarina buyuk 
yararlar sagliyacak bu fikrin bir cok pozitif yani var ama bunlardan 
sadece birini bir senaryo esliginde sizlere sunuyorum sene edirne 
lisesi ogrencilerine ayirdigi komputurleri devletin ve ozel sirketlerin 
yardimi ile yeniliyerek komputurlerini baglar amaclari ogrencilere 
komputurlerin artik hayatin bir parcasi oldugu fikrini asilamak 
odevlerini yaparken onlara sonsuz bir referans kaynagi saglamak ve de 
kafalarinda daha korpe dusunceler ile dogusen gencleri yurt disinda 
yasayan diger ogrencilerle icinde bulusturup fikir alis verisi yapmaya 
izin vererek hayatlarini pozitif bir sekilde yonlendirmeye calismaktir 
ve yasindaki cigdem komputurun basina oturur cigdem isletme okumak 
ister ve de turkiyede guclu bir firmanin basina gecerek bitmek tukenmek 
bilmeyen cabasi ve performansi ile her gun bir adim daha ileri goturmek 
istemektedir bu yuzden kendinden buyuk agabey ve ablalarindan 
tavsiyeler almak icin ileride yasayacaklarini simdiden ogrenmek icin 
yilinda hala gundemde olan girer ilk okudugu yazi basliklarini 
sunuyorum sizlere cimboma kafam girsin sen gel benim sikimi kemir ibne 
haluk seni gidi gotveren seni orospular diyarina hosgeldiniz yavsak 
kari cigdeme anani si evet senaryonunun sonunu sizlerin tamamlamanizi 
istiyorum bazi arkadaslar israrla turkiye icin bir onemi yoktur 
diyorlar bana bakin beyinzice konusucaginiza biraz kabul ediyorum 
tamami ile pozitif bir fikir ortaya atmadim ama bir kisi disinda kimse 
fikire sans bile tanimadiben size bir sey satmiyorumbunda benim 
hakkinin mana etmeye calistigi gibi bir kazancimda yok hangi 
fikirleride savunsak turkiyenin neresinden gelsekte gelelim insanlardan 
ne kadar ustunum diyerek cevremizdekilere havada satsakda 
cevremizdekileri ne kadarda asalasakta tum kullanlarin ortak bir 
noktasi varburda yazi yazan ve okuyan herkes bir grubu olusturuyor ve 
grubun adida o yuzden herkesin bu grup icinde bazi kurallara uymasi 
gerekiyor uymak istemeyen cekip gidebilir bugune kadar en az turke 
girilicegini yazilarin nasil okunacagini gosterdim hepsininde ilk 
reaksiyonu su idi kimdir bu insanlar hepsi kafayi yemis birbirine kufur 
edenler mactamiyiz kahvedemiyiz yaziklar olsun burda devletin vatanin 
parasini harcayanlara bunlari duymak uzucu ama bunlar benim her sefer 
duyduklarim eminim sizde ayni seyleri duymussunuzdur bir suru insan 
saciyor son olarak birde bugune kadar hic bir yerde sansur gormemis 
arkadaslarimiza iyice dolasmalarini tavsiye ediyorum eger size gore bu 
fikri ilk kez biz uygulayacaksak ne mutlu bize baskasinin yapmamasi 
bizim uygulamamiz icin bir sebep degildir saygilarla altay 
 * 
Ulwa (words from a Mesoamerican language) -- yaupak asang kuhkabil kalka as bik ahauka bungpai lau 
ya askana ihyawai tung lumakka puput yapa dapi kahkalu 
asung takat yau ma baka kumdai walik 
pumti talang waya wingkata puhnaka kat akatka witka kau laih 
alas pamkih ilwana dasika palka ilwang bahangh balhtang kau 
andih takat yau yawanaka yulka 
katka arungka wingkata puhnaka ya midana as yapa laih pumtasa 
kat alas kanas midanaka waltang make karak yul baunaka 
yulka ya damaska pas anakat watdi buhtang as kau yawi tuspi 
yaktang malka balna rumpai ya walti 
sutbangh as itang ukatak ya takpang dapi isamah kaupak was waya baka 
karak suhpi lawang witka kau di bubuh baka as sukpang dapi di ya 
kasna isau waltai yapa karak kasang dapi witpang kau di bubuh sukpang 
ya distang kat sangkaka ya sumaltang barangka yamka 
atnaka di dutka balna yamnaka aisau 
walang panka as kau pamkih ya sittang asna as paktang 
sau dikuh takat kau dapi katka amangpara dapak dasi palka amang dai 
al as kal kapahka sa yapa 
rauka palka yapa kal kapahka balna ya kanas yawadasa 
katka kang lawang yawi bungparang ya dapi yaka ya 
abalka pumtasa rauka kat bungpang 
sau bukdang balna kau pamkih balna kalkana wayaka daki raudang 
makdaka paptida kau asangbah atkamalh ya tingka palka ya di kal dahsa 
karak aratukuh walti akpang amangpara dapak waya kal daki 
laktai yapa dapi amangna sikka palka karak lauwi dakang 
pamkih balna ya labakat kau kanas wi awadai ati asung kau wapdi 
waya alas pamkakih kau likpang 
pamkih makamak kau uba palka as as tispang waya atak kal 
dahnaka dapi ihnaka sa yulka yapa kal daki laknaka damaska pas kau 
alas pamkakih ya talak yamka di dutka bungnaka sa atak kau di 
pumtasa yawang wayaka as bik yamtasa yawi asang midangka 
balna watya kat 
it talnaka pamkih ilwingka singka balna yapa yawi midana bang 
atak rahwah salap as takat singka kaupak nauh yapa kau alas 
pahka lau yaupak al arungka balna yapa ya turuh ilwingka balna 
yapa naukana watah 
katka singkika ya alas pan yaka upurka kaupak ya atmalh waltangka 
kanas ihirtang ya mamaka muih as luih bas yapa watah 
dapi bik balah ihyawasa ma daihka palka katka baska ya pihtang 
muhka wayaka nauh laktang siritka dapi yaka suwinka balna kau muhka 
pihtang sikka palka raudi alas kau luih taihpang 
katka kau waltangka ihirtang ya yaka muihka almuk ya 
tingka balna ya sitna atnaka dangkat kau lati dapi makamak tari yau 
muih as sitna as sitnaka karhnaka karak 
yangna ma witputingna aka kira panka kau alas andih pasdak iwang 
katka it atrang rauka palka dakat karhnaka turuh nutingka as ya 
yapa kapah aisau palka turuh ilwingka alka as ya yultang tingka 
kau wah tangka as wak kau wat sakwi nah dapi 
waya as yapa kang katang pamkih ilwingka wak as ya 
kang kanaka kau ya al as di yabasikka dapi aratukuh bik 
rumnaka bik waltai palka alas kau di bungpasa atrang bik 
ya kang lawi yawang yapa yawak kau balna yapa 
isau salap as kau bu yapa katka ampas ya saupah kau tumdana 
alas balna sangkanaka adadahka 
ya muih almuk ya sangkika subitnaka yulka alas balna paskana 
kau awanaka pumtang rauka palka alaska turuh nutingka as 
tingka kau anaka katka turuh ilwingka balna ya 
raukakana aslah bik aisau dai nauka yamnaka alas balna walik tingkana 
barangpida di nutingka pamkihka silka munka ya 
bautam pawanihni audana watah atrang mxico wisam wingka daihka wai ya 
takat kau kalka rawahpi 
ya kal kang katang nauh atnaka alas pawanihka yapa kawaranaka 
sikka as kang katang ya anaka lalahka dapi as yuhka as adahka 
bik kang katang 
alas tingka palka ya ihirtang tanaka as yapa sipah muihka 
pamkihka silka munka kau baunaka yulka katka di as dakang 
yapa dapah aratukuh as tunma kat ma ana lau ka 
turuh ilwingka arungka balna ya watdi wana suwinka as kau alasna 
balna tunakna ya aslah yapa bungna di taldasa dika balna yapa 
ya takat kau sih yawang rahwah salap yapa upurna 
yau dapi muihka balna ya atdati pah palka yaka muihka almuk karhnaka 
pas kau it yakdasa kau kau yultang kau kang 
man ya wai mankuiti dakang raudi alas kahkalu kau 
ma baka kumdi ihyawai tung dai ya it talsa dai 
miriki asangka balna aslah yamna atkamalh watdi 
apa yultang kal ahaupi barang ati al arungka balna ya ma baka ya 
talnaka yulka dapi aratukuh as tingka palka kau watah dai ka bik 
kalah di as bik yamnaka aisau muih pihka balna asangkana kaupak 
alka mining dasinika balhtang kau watah yak kanas bik yaka 
muihka balna ya di nutingka as ka turuh balna nutingka yapa 
yultang alas bik talnaka kau laih muih almuk waya yamtai 
yaupak bungnaka waltasa yapa 
yapa balna yaka dakatna karhnaka dika ka ati ya wapdang 
turuh baka as iti kasnaka ya di as dutka laih sa pumtayang yaupak 
bik iwanaka dika bik bungnaka watdi yultang raudi 
man laih raumaka waya bik aisau aka dika pas kau awanaka 
yulka yultang 
wahka sipah muihka tingka kau sitna ya dakti yaktah ati yultang 
muih pihka balna asangkana kaupak alka ya raudi turuh ilwingka 
alka ya yulka kang lawasa pi 
talnaka as pawanihka balna kau bungpang katka dapi 
hine karak lauwanakana kau dasi palka lauwana tingkana karak 
kal dasipi lauwanaka yau 
alas yamtangka kang katang ahauka palka alas di as pas kau 
awanaka pumtasa ya 
sulu balna ati wapdang anaka balna pas kau 
kau talna di karak yapa para tali kau 
ka atak yamka palka kang lawang dapi takat kau dayadang 
yakat bik tingka ya aratukuh walti akpi yamtang kau 
dapi ya asung kau di as bik pumtasa yaka arakatukuh ya 
rumnaka kau 
yaka muihka balna yapa kau alas waltasa talya 
kahkalu kau lumakka pauka as bungpang yakaupak alas 
karaskamak anakat kau wauhdi awang yapa lau dapi witka kau 
alas abukka kauh watdi lawang 
tmalh ya aratukuhka tingka kau lati talang amang palka turuh 
ilwingka wak balna kau tanaka as karak bik tatang mampa kau di 
as bungparang ya yulka dapi talnaka kau ripka dapi asung 
dasikaka bik watah 
dapi alas tingkamak ya aratukuh raudanaka kau taihpang 
manna bu as balna di as yulnaka watah manna kuiti dakang 
hine dapi kau tali nah karak 
ka witka kau aka tunak natdang diahka ati 
yapa hine kau tali dapi bik yabahna ya alas muhka kau ahauka palka 
bungpang kat alas balna pumnakana kau pumna ya al 
as sirihka palka dapi kanas ahauka mxico wisam pas kau 
dapi ya andih iwang aratukuh kanas rumpasa takat kau 
hine ya kanas pukka kau tungwingka alas pawanihka karak pamkih yaupak 
lakwang dapi alas kau kanas di as bik yultasa karak wah as muih 
almuk tingka kau sitna lau dai ka dakti yaktang 
yaupak di as bik yultasa watdi alas lauwanaka kau yawi lauwang 
yamka palka hine yultang audi karak muihka almuk ya alas tingka 
wah dakat kau watang ya yaktang kau 
yaupak alas tingka wah sitna laudai ya paukika ya alas saptang 
kawari audi karak alas iwanaka yaka kal dakang lik palka 
alas tunak kau katka yaka pumti alas sangka waya tung atnaka 
ya alas kau audanaka sikka palka atai 
ya alas yultang manna it manna pih dapi 
karak muihkana iwana balna ya anapi inaka yang bik yultaring tunak 
muihka kau kanas waya kau ati yultang atmalh tunak 
muihka ya 
hine dapi karak lakwana dapi alas balna muih iwana balna 
ya pamkih takat kau ilna katka pirihka bitah kau yaka yaka alas 
balna lauwanaka balna alas balna kanas sangka kau 
kanas bai yawasa kau ya waya takat kau wauhdai yapa yamtang 
dapi muih almuk yau tali apa yultang 
man it man turuh bakaka yaka karak midanaka 
yamka al baka di luih kau yultang muihka almuk ya alas 
bik tingka sitna paukika yau saptasa karak alas akawas midang 
ya buna pahka kau anaka yulka dadang 
turuh ilwingka bu balna ya bai bungpi yawana dapi 
karak iwana muihka balna ya duihi 
 * 
Viking (female names) -- alfdis arnora asa asgerd asleif asta astrid aud bera bergljot 
bergthora dotta freydis gjaflaug grima grimhild groa gudrid gudrun 
gunnhild gyda halldis hallfrid hallgerd hallveig helga herdis hild 
hildigunn hlif hrefna hrodny ingibjorg ingigerd ingirid ingunn jorunn 
katla ragna ragnhild rannveig saeunn sigrid svala thjodhild thora 
thorbjorg thordis thorfinna thorgerd thorgunna thorhalla thorhild 
thorkatla thorunn thurid thyra unn valgerd vigdis 
 * 
Viking (male names) -- aki alf alfgeir amundi ari armod arnfinn arnlaug arnor aslak bardi 
bergthor bersi bjarni bjorn bodvar bork botolf brand bui egil einar 
eindridi eirik eldgrim erlend eyjolf eystein eyvind finn finnbogi 
fridgeir gardi geir geirmund geirstein gest gizur glum grani grim 
gudmund gunnar gunnbjorn gunnlaug hafgrim hakon halfdan hall halldor 
hallfred harald harek hastein hauk havard hedin helgi herjolf hjalti 
hogni hord hrafn hring hroald hrut illugi ingi ingjald ingolf isleif 
ivar kalf kari karlsefni ketil knut kol kolbein lambi leif ljot 
ljotolf lodin mord odd ofeig ogmund olaf olvir onund orm otkel otrygg 
ottar ozur ragnar rognvald runolf sam sighvat sigmund sigtrygg sigulf 
sigurd sigwulf skapti snorri solmund solvi starkad stein steinkel 
steinthor sturla styrkar sumarlidi svein thjodolf thjostolf thorarin 
thorbjorn thorbrand thord thorfinn thorgeir thorgest thorgils thorgrim 
thorhall thorir thorkel thormod thorstein thorvald thrain thrand tosti 
ulf uni vagn valgard vandrad vermund vestein vigfus yngvar 
 * 
Chinese family names -- Ba
Mo
Chan
Chang
Chen
Cheng
Chiao
Chin
Cong
Dee
Fan
Fang
Fat
Fong
Gao
Gum
Ha
Han
He
Ho
Hou
Hsiao
Hu
Huang
Hui
Jianbua
Jiang
Kai
Kara
King
Kou
Kok
Kuk
Kun
Kwan
Kwok
Lai
Lam
Lan
Lang
Lau
Law
Lee
Lei
Leung
Li
Liang
Liu
Lo
Lung
Ma
Mak
Miu
Mo
Mok
Ni
Ng
Pak
Pao
Pien
Qiu
Sheng
Shih
Shing
Shiu
Sik
Song
Su
Sun
Sun
Tah
Tam
Tan
Tang
Ti
Tieh
Tong
Tsai
Tsang
Tseng
Tsien
Tsui
Wang
Wong
Wei
Woo
Wu
Xiu
Xu
Yang
Yau
Yen
Yi
Ying
Yip
Yiu
Yu
Yuan
Yuen
Yun
Yung
Zeng
Zhang
Zheng
Zhou
Zhu
 * 
Chinese female given names -- Ah Cy	lovely
Ah Kum	good as gold
Ah Lam	like an orchard
An	peace
Bik	jade
Bo	precious
Chao-xing	morning star
Chen
Chi
Ching-Hsia
Chow	summer
Chu Hua	chrysanthemum
Chun	spring
Chyou	autumn
Da Chun	long spring
Da-xia	long summer
Dai-tai	leading a boy in hopes he will follow
Ding
Eu-funh	playful phoenix
Eu-meh	especially beautiful
Fang	fragrant
Far	flower
Fung	bird
Guan-yin	goddess of mercy
Haixia
Ho-Win	a loyal swallow
Hseuh
Hu	tiger
Hui
Huifang	nice fragrance
Hwei-ru	wise, intelligent
Jiahui	nice
Jiani
Jing-wei	small bird
Juijuan
Jun	truth
Kit-Ying
Kuai Hua	mallow blossom
Kue Ching	sounds good
Kwong	broad
Kwun-Yu
Lan
Lee	plum
Li Hua	pear blossom
Lian	the graceful willow
Lien Hua	lotus flower
Lien	lotus
Lihwa	a Chinese princess
Lijuan
Lin	beautiful jade
Ling	delicate and dainty
Lingjuan
Linwei
Liping
Lixue	beautiful snow
Mao
Meh-e	beautiful posture
Meh-funh	pretty or beautiful phoenix
Mei	beautiful, plum, sister, rose
Meiying	beautiful flower
Meizhen	beautiful pearl
Meizhu
Min
Ming-hua	tomorrow's flower
Mingxing
Mu Lan	magnolia blossom
Mu Tan	tree peony blossom
Nuwa	mother goddess
Peihsi
Peijun
Peipei
Ping	duckweed
Qianru	nice smile
Qing
Qiurui
Qun
Rongfang
Rufen	nice fragrance
Sheu-fuh	elegant phoenix
Shun-Kwan
Sya	summer
Sying	star
Szu
Tao	peach
Te	special
Tse
Tu	jade
Ushi	the ox
Weihong
Xiaobo
Xiaojun
Xiaolan
Xiaoqin
Xilan
Xingjiang
Xiu Mei	beautiful plum
Xiulan
Xiumin
Xuedi
Yan
Yang	sun
Yanhong
Yanjun
Yet Kwai	beautiful as a rose
Yin	silver
Ying
Ying-Hung
Yong
Yow	feminine
Yu-jun	from Chongching
Yuefang
Yuehai	beautiful moon
Yueqin	moon-shaped lute
Yuet	moon
Yuk	moon
Yuke	jade
Yumei
Yuying	jade flower
Yuzhen	jade gem
Yuzhu
Zhen
 * 
Chinese male given names -- An	peace
Anguo	protect country
Baio
Cheh
Chen	vast, great
Cheung	good luck
Chi	the younger generation
Chi-Hei
Chi-Wai
Chia-liang
Chih
Chiu-Wai
Chin-Shek
Chu Mei
Chuen-Chun
Chung	intelligent
Dai-Muk
De-li	virtuous
De-shi	a man of virtue
Dewei	highly virtuous
Ding-bang	protect country
Fai	growth, beginning, or fly
Fan-Kei
Fei
Feng
Fu
Fui-On
Gan	dare, adventure
Gangsheng
Gong
Guoqiang
Guotin	polite, firm, strong leader
Haifeng
Hark
Hing-Hong
Hing-kai
Ho	the good
Hon-Keung
Hop	agreeable
Hou
Hu
Huang Fu	rich future
Hung	great
Jaw-long	like a dragon
Jet
Jian-jie
Ji-Dan
Jin	gold
Jun	truth
Ka
Ka-Yan
Kam-Kong
Kang
Kar-King
Kar-Ying
Keung	universe
Kien
Kit-wai
Kiu-Wai
Kong	glorious, sky
Kuang
Kuan-tai
Kuei-ling
Kung
Kungzheng
Kwok-Bun
Lap-Man
Li	strength
Liang	good, excellent
Liang-de
Lieh
Liko	protected by Buddha
Lok	happiness
Loo
Lung
Man
Man-Tat
Manchu	pure
Min
Ming-hua	brilliant, elite
Ming-tun	intelligent heavy
Ming-Yam
Ning
On	peace
Pa
Park	the cypress trees
Peng
Quon	bright
Runming
Sai Yuk
Sau-Hin
Sau-Lung
Sau-Yin
Shaoqiang	strong and profound
Shen	spirit, deep thought
Shiao-Chiang
Shilin	intellectual
Shing	victory
Shipeng
Shiyu
Shoi-ming	life of sunshine
Shu-de
Shu-sai-chong	happy all his life long
Shun
Sing-Chi
Sing-Mo
Sing-Yi
Siu-Chang
Siu-Lang
Siu-Man
Song
Sueh-yen	continuity, harmonious
Suk Wah
Sying	star
Sze
Tai
Tak-Wah
Tan
Tao
Tat-Ming
Te
Tong-ngo	Eastern Hill (good Feng Shui)
Tung	all, universal
Wai-Tak
Wang	hope or wish
Wei
Weiqian
Weiqiang
Wing	glory
Wing-heng
Wu
Xiaopeng
Xiaoxuan
Yifu
Yixiao
Yu	universe
Yuejiu
Yuwei
Zhihuan	ambitious
Zhiqiang
Zhixin	a man of ambition
Zhiyuan	ambition
Zhu
 * 
English female given names -- Adrienne
Aimee
Alexis
Annie
April
Barbie
Becky
Betty
Brigitte
Candy
Cassandra
Coral
Cynthia
Debbie
DeeDee
Delia
Emily
Eternity
Fennie
Flower
Frances
Gina
Gigi
Harmony
Jenny
Josie
Kara
Karen
Kelly
Kerry
Kim
Lilith
Mary
Melanie
Nancy
Nina
Opal
Patti
Pearl
Porphyry
Rosie
Sapphire
Shelly
Sheri
Shirley
Susie
Sylvia
Teresa
Tiffany
Vicky
Violet
Vivian
Wendy
Willow
 * 
English male given names that take adjectives -- Albert
Andy
Barry
Bennie
Bobby
Bowie
Buddy
Charlie
Chucky
Danny
David
Davy
Denny
Eddie
Frankie
Freddie
Harry
Jack
Jackie
Jerry
Jimmy
John
Johnny
Ken
Kenny
Larry
Markie
Mikey
Ollie
Ricky
Robbie
Rusty
Sammy
Scottie
Stanley
Teddy
Terry
Toby
Tommy
Tony
Victor
Willy
 * 
English male given names that get no adjs -- Albert
Andy
Bennie
Blacky
Bobby
Buddy
Burton
Charlie
Chucky
Danny
David
Davy
Denny
Duncan
Eddie
Eugene
Frankie
Freddie
Harry
Jack
Jackie
Jerry
Jimmy
John
Johnny
Ken
Kenny
Larry
Markie
Max
Mikey
Napoleon
Norman
Ollie
Perry
Philip
Ricky
Robbie
Romeo
Rusty
Sammy
Scottie
Stanley
Stephen
Teddy
Toby
Tommy
Victor
Walker
Willy
Winchester
 * 
Funny English adjectives -- Bad
Black
Cool
Cruel
Dark
Dirty
Electric
Fast
Flash
Golden
Happy
Hard
Hot
Lucky
Mad
Old
Quick
Sad
Sharp
Slick
Smilin'
Smokin'
Smooth
Stiff
Straight
Strange
Strong
Swank
Sweet
Swingin'
Thunderin'
Weird
Young
 * 
 * Aleksin 
Andreev 
Arkhangelsk 
Astrakhan 
Belgorod 
Berezov 
Bilgorod 
Bobruisk 
Bogoliubov 
Boguslavl 
Bolshev 
Borisov 
Borovsk 
Bratislavl
Briansk 
Bykoven 
Chernigov 
Chichersk 
Debriansk 
Dedoslavl
Deviagorsk 
Dmitrov 
Donetsk
Eniseisk 
Gdov 
Glebov 
Glukhov 
Goroshin 
Gubin 
Yaroslavl
Iskorosten
Yurichev 
Yuryev 
Ivangorod 
Izborsk 
Iziaslavl
Karachev 
Kashin 
Kasimov 
Kirillov 
Klechesk 
Komov 
Koponov 
Korchesk 
Korchev 
Kozelsk 
Krichev 
Kursk 
Listven 
Liubachev 
Liubutsk 
Lobynsk 
Logozhsk 
Lublin 
Lutsk 
Mchenesk 
Menesk 
Mensk 
Mezchesk 
Mikhailov 
Mikulin 
Minsk 
Mogilev 
Mosalsk 
Mozhaisk 
Murom 
Murovinsk 
Nerinsk 
Nezhatin 
Nezhegorod 
Nosov 
Novgorod 
Obdorsk 
Obolensk 
Obrov 
Ochakov 
Odoev 
Olgov 
Orlov 
Orekhov 
Orelsk 
Oskol 
Ozhsk 
Pereiaslavl
Perevitsk 
Pinsk 
Plesensk 
Polatsk 
Polotsk 
Pskov 
Rogachev 
Rogov 
Romanov 
Ropesk 
Roslavl 
Rostislavl
Rostov 
Rzhev 
Rzhevsk
Saratov 
Semenov 
Sepukhov 
Serensk 
Serpeisk 
Sevsk 
Shatsk 
Shestakov 
Shumsk 
Sluchesk 
Slutsk 
Smolensk 
Strezhev 
Sugrov 
Suteisk 
Sviatoslavl
Temnikov 
Teshilov 
Tesov 
Tmutarakhan
Tobolsk 
Torchev 
Tsargorod 
Turilsk 
Turov 
Vasilev 
Verkhovsk 
Vernev 
Vladimir 
Volodarev 
Volokolamsk 
Vorobin 
Voronezh 
Vyshegrad 
Zubtsov 
Zvenigorod 

Alimia
Alepia
Amuria
Arania 
Artania 
Batania
Belarusia 
Bolgaria
Borania
Bosnia 
Bulgaria
Burania 
Chania
Chehia 
Chonia
Gorelia
Goria
Grunia
Gvidonia
Ikaria
Ilonia
Itonia
Izboria 
Izhovia
Izovia
Hania
Hazaria 
Horvatia 
Horutania 
Kania
Karantia 
Karelia
Kuavia 
Malia
Moravia 
Moria
Moskovia
Mouravia 
Nevia
Nosia
Novia
Olgia
Onegia
Orelia
Orenia
Ostogia
Panonia
Patonia
Polonia 
Putovia 
Radonia
Rogozia 
Rostovia 
Runia 
Rusa
Ruta
Rusia 
Serbia 
Slavia 
Slovakia 
Slovenia 
Syberia 
Ukrainia 
Ulusia 
Uralia 
*/