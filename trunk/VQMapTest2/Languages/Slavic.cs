using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NameGen;

namespace VQMapTest2.Languages
{
    class Slavic: Language
    {
        private Confluxer m_pCountries;

        private Confluxer m_pTowns;

        private Confluxer m_pVillages;

        private Confluxer m_pFamily;

        private Confluxer m_pFemale;

        private Confluxer m_pMale;

        public Slavic()
            :base(NameGenerator.Language.NONE)
        {
            string sCountries = "Alimia Alepia Amuria Arania Artania Batania Belarusia Bolgaria Borania Bosnia Bulgaria Burania Chania Chehia Chonia Gorelia Goria Grunia Gvidonia Ikaria Ilonia Itonia Izboria Izhovia Izovia Hania Hazaria Horvatia Horutania Kania Karantia Karelia Kuavia Malia Moravia Moria Moskovia Mouravia Nevia Nosia Novia Olgia Onegia Orelia Orenia Ostogia Panonia Patonia Polonia Putovia Radonia Rogozia Rostovia Runia Rusa Ruta Rusia Serbia Slavia Slovakia Slovenia Syberia Ukrainia Ulusia Uralia";
            m_pCountries = new Confluxer(sCountries, 2);

            string sTowns = "Aleksin Andreev Arkhangelsk Astrakhan Belgorod Berezov Bilgorod Bobruisk Bogoliubov Boguslavl Bolshev Borisov Borovsk Bratislavl Briansk Bykoven Chernigov Chichersk Debriansk Dedoslavl Deviagorsk Dmitrov Donetsk Eniseisk Gdov Glebov Glukhov Goroshin Gubin Yaroslavl Iskorosten Yurichev Yuryev Ivangorod Izborsk Iziaslavl Karachev Kashin Kasimov Kirillov Klechesk Komov Koponov Korchesk Korchev Kozelsk Krichev Kursk Listven Liubachev Liubutsk Lobynsk Logozhsk Lublin Lutsk Mchenesk Menesk Mensk Mezchesk Mikhailov Mikulin Minsk Mogilev Mosalsk Mozhaisk Murom Murovinsk Nerinsk Nezhatin Nezhegorod Nosov Novgorod Obdorsk Obolensk Obrov Ochakov Odoev Olgov Orlov Orekhov Orelsk Oskol Ozhsk Pereiaslavl Perevitsk Pinsk Plesensk Polatsk Polotsk Pskov Rogachev Rogov Romanov Ropesk Roslavl Rostislavl Rostov Rzhev Rzhevsk Saratov Semenov Sepukhov Serensk Serpeisk Sevsk Shatsk Shestakov Shumsk Sluchesk Slutsk Smolensk Strezhev Sugrov Suteisk Sviatoslavl Temnikov Teshilov Tesov Tmutarakhan Tobolsk Torchev Tsargorod Turilsk Turov Vasilev Verkhovsk Vernev Vladimir Volodarev Volokolamsk Vorobin Voronezh Vyshegrad Zubtsov Zvenigorod";
            m_pTowns = new Confluxer(sTowns, 2);

            string sVillages = "Aleksino Andreevka Beloe Berezovka Berezovo Bogoliubovo Bolshevka Borisovo Borodino Borovichi Bykovo Chernigovo Debriansk Demianovo Deviatovo Dmitrovka Elisevka Glebovo Glukhovka Goroshino Gubino Iskorostenevka Yurichevo Yuryevo Ivanovo Ivanovka Karachevo Kashino Kasimovo Kirillovka Komovo Koponovka Korchevo Krichevo Listvenka Liubachevo Lobno Logozhi Lublino Luki Mikhailovka Mikulino Mogilka Muromka Nezhatino Nosovo Obrovo Ochakovo Odoevka Olgovo Orlovo Orekhovka Rogachevo Rogovka Romanovka Rostovo Saratovka Semenovo Sepukhovka Shestakovo Shumka Slutovo Smolenka Strezhevo Sugrovka Temnikovo Teshilovka Tesovo Tobolka Torchevo Turovo Vasilevka Verkhovo Vernevo Vladimirka Volodarevka Vorobino Zubtsovo";
            m_pVillages = new Confluxer(sVillages, 2);

            string sFamily = "Abelev Abramov Alenichev Andropov Aniskin Antipov Afanasiev Apraxin Artamonov Astafiev Baikov Bakhtin Bakunin Balakirev Baranov Barishnikov Barsukov Baryatinskiy Batalov Batkin Baturin Beketov Belanov Beletskiy Belikov Belov Belyakov Berezhnoy Bestemianov Bezborodov Bezuhov Bobrikov Bobylev Bocharkov Bolkonskiy Borisov Borodin Borshevskiy Brezhnev Brilev Budarin Bukharin Bukin Bukovskiy Butkovskiy Bykov Bykovskiy Chaikovskiy Charkov Chausov Cherniavin Chernov Chernyaev Chesnokov Chicherin Chistakov Chuikov Daletskiy Dashkov Davydov Demichev Dolgonosov Dolgorukiy Dolohov Donkin Dubkov Dubnikov Dubrovskiy Fedotov Fomin Gagarin Gavrilov Gerasimov Glebov Godunov Golitsin Golovanov Golovastov Golubev Gorbunov Govorov Grachev Groshev Gubin Gudelin Gusev Ignatov Ipatyev Ivanov Kaledin Kalinin Kapustin Kazakov Komarov Kondretov Konovalov Konev Kornev Kozlov Koshkin Krylov Kubarev Kudrov Kulikov Larin Lebedev Lobov Lobanov Loginov Luzhkov Makarov Malinin Medvedev Mishin Molotov Morozov Naumov Nazarov Nemov Nikitin Neverov Nikonov Nosov Novikov Nevolin Obolenskiy Obukhov Orlov Ochakov Pakhomov Pankratov Panov Petrov Popov Peskov Pletnev Potemkin Primakov Prokhorov Privalov Pronin Pushkin Putin Rakov Rasputin Rezanov Rodionov Rodin Rogachev Romanov Romashin Rostov Rudakov Ryabov Ryabinin Sazhin Seleznev Semenov Sharov Sharikov Shishkin Shubin Shulgin Sidorov Skobelev Sokolov Sotnikov Starodumov Strogov Sviridov Tabakov Tarasov Titov Tokarev Tolstov Toporov Trubachev Tumanov Tratyakov Tupolev Turov Udalov Udovin Ulanov Uvarov Vdovin Vasiliev Voronin Voronov Yakovlev Yeltsin Zakharov Zbruev Zolotov Zubov";
            m_pFamily = new Confluxer(sFamily, 2);

            string sFemale = "Maria Tatiana Natalia Marfa Matriona Nadezhda Lada Larisa Tamara Fekla Yulia Aliona Valentina Valeria Galina Daria Evdokia Elena Ekaterina Evgenia Zinaida Inga Irina Klavdia Katya Katka Marina Nastasia Anastasia Olga Svetlana Sveta Sonia Antonina";
            m_pFemale = new Confluxer(sFemale, 2);

            string sMale = "Beloslav Berimir Berislav Blagoslav Bogdan Boleslav Borimir Borislav Bratislav Bronislav Bryacheslav Budimir Velimir Velislav Vladimir Vladislav Vsevolod Vseslav Vyacheslav Gorislav Gostemil Gostomisl Gradimir Gradislav Granislav Dobromil Dobromir Dobromisl Dragomir Zvenislav Zlatomir Izyaslav Istislav Ladislav Lubomir Lubomisl Mechislav Milorad Miloslav Miroslav Mstislav Nevzor Ostromir Peresvet Putimir Putislav Radimir Radislav Ratibor Rodislav Rostislav Svetovid Svetozar Svyatogor Svyatopolk Svyatoslav Stanimir Stanislav Sudimir Sudislav Tverdimir Tverdislav Tihomir Yaromir Yaropolk Yaroslav";
            m_pMale = new Confluxer(sMale, 2);
        }

        #region ILanguage Members

        protected override string GetCountryName()
        {
            return m_pCountries.Generate();
        }

        protected override string GetTownName()
        {
            return m_pTowns.Generate();
        }

        protected override string GetVillageName()
        {
            return m_pVillages.Generate();
        }

        protected override string GetFamily()
        {
            return m_pFamily.Generate();
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
