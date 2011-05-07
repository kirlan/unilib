using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NameGen;
using Random;

namespace Socium.Languages
{
    class Asian: Language
    {
        private Confluxer m_pFemale;

        private Confluxer m_pMale;

        public Asian()
            :base(NameGenerator.Language.NONE)
        {
            string sFemale = "akazome akiko ayame chika chizu cho fuji hamako hana hanazono hiromusi hisae hisayo imako inoe ishi izuko jun kagami kame-hime kameko kaneko kawa kawanomu keiko kenshi kiku kimiko kogin kogo komachi kozakura kumiko kusuriko machi mariko masago masako masuko matsukaze midori mineko miwa miyako miyoko mura nari ochobo oki onshi reiko renshi rin ruri sachi sadako sakura seki sen-hime senshi setsuko shikibu shina shizue shizuyo siki sugi taka takara tamako teika tokiwa tokuko tomoe towika tsukinoyo umeko umeno wakana yasuko yoshiko yukinoyo yukio";
            m_pFemale = new Confluxer(sFemale, 2);

            string sMale = "agatamori akimitsu akira arinori azumabito bakin benkei buntaro chikafusa chikayo chomei chuemon dosan emishi emon fuhito fujifusa fujitaka fususaki gekkai gennai gidayu gongoro hakatoko hamanari haruhisa hideharu hideo hidetanda hideyoshi hirohito hirotsugu hitomaru iemitsu ienobu ieyasu ieyoshi imoko issai iwao iwazumi jikkyo jozen junkei jussai kageharu kagemasa kagemusha kahei kanemitsu katsumi katsuyori kazan kazunori keisuke kintaro kiyomori kiyosuke kmako komaro koremasa koreyasu kuronushi kyuso mabuchi maro masahide masamitsu michifusa mitsukane miyamoto mochiyo morinaga munetaka murashige nagafusa nagate nakahira nambo naoshige narihiro oguromaro okimoto okura omaro otondo razan rikyu rokuemon ryokai sadakata sanehira sanetomo sanzo saru shigenobu shigeuji shingen shoetsu shozen sukemasa tadabumi tadashiro takatoshi tameyori taneo taneyoshi tensui togama tomomasa toshifusa toyonari tsunayoshi tsunetane uchimaro ujihiro umakai watamaro yakamochi yasumori yoriie yoritomo yoshiie yoshisune yoshitane yoshizumi yukihira zuiken";
            m_pMale = new Confluxer(sMale, 2);
        }

        #region ILanguage Members

        protected override string GetCountryName()
        {
            return NameGenerator2.GetHeroName(NameGenerator2.Language.Chinese);
//            return WordGenerator.GetWord(WordGenerator.Language.Japanese);
        }

        protected override string GetTownName()
        {
            return WordGenerator.GetWord(WordGenerator.Language.Japanese);
        }

        protected override string GetVillageName()
        {
            return WordGenerator.GetWord(WordGenerator.Language.Japanese);
        }

        protected override string GetFamily()
        {
            return NameGenerator2.GetHeroName(Rnd.OneChanceFrom(2) ? NameGenerator2.Language.Japaneze1 : NameGenerator2.Language.Japaneze2);
        }

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
