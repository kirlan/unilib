using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NameGen;

namespace Socium.Languages
{
    class Arabian:Language
    {
        private Confluxer m_pNations;

        private Confluxer m_pFamily;

        private Confluxer m_pFemale;

        private Confluxer m_pMale;

        public Arabian()
            :base(NameGenerator.Language.Arabian)
        {
            string sNation = "syri arabi akadi sumeri babiloni aramai amhari semiti harari mehri najadi omani hejazi shihi dhofari yemeni nubi saidi magrebi sudani kuwati turki safai thamudi hasai sahari bedawi sanani dhofari soqotri shehri jibali bathari sabai madhabi dahli shirvani bahrani selti";
            m_pNations = new Confluxer(sNation, 3);

            string sFamily = "Abaz Abo Hamed Abu Shakr Akil Akwal Amr Dalharn Dossad Dossar Dwairan Farran Habash Harad Jaber Jaber Jahn Mawalhad Mehalel Mowaled Mub Mukhtar Mutad Muwalid Nub Owairan Sabah Sahaf Shahran Shammar Alam Asmar Dosar Fayoum Algosaub Jaber Jahan Jur Karach Alkhaiwan Masaar Mehalel Raz Sahah Sharan Temiyat Thynniyan Zeid Zesh Amer Amin Andon Anwar Asir Atef Bahamdan Bakahasab Baran Billah Doka Elouahab Faran Farakhan Hijaz Ismail Kanasan Madan Madar Mar Raboud Rostom Saleh Siham Solaiman Somayl Suliman Surur Zebraman Zuab Zubromaw Hafez Kardar Qadir Abid Afaq Husain Aftab Amir Elah Arif Asad Khan Asif Ahmed Asif Masud Rahman Azmat Ran Dilawar Husain Fakir Syed Aizaz Fasih Mahmud Fid Husain Ghazal Ghulam Abas Mahomed Hafiz Hanif Mohamad Hasib Ahsan Ijaz Ahmed Ijaz But Ikram Elah Imran Khan Imtiaz Ahmed Inshan Intikhab Inzamam Jahangir Khan Javed Akhtar Javed Miandad Karim Ibadul Khalid Hasan Khalid Wazir Khan Mohamad Mahmod Husain Mahomed Nisar Majid Jahangir Khan Maqsud Faruq Mohsin Kamal Mudasar Nazar Mul Mushtaq Naushad Niaz Ahmed Perv Sajad Raz Husain Nasir Nasir Sadiq Saed Salah Salim Altaf Salim Malik Sarfraz Nawaz Shafquat Husain Shafquat Ran Shahid Shakur Waqar Ahmed Waqar Hasan Waqar Youn Wasim Akram Bar Wazir Abas Zulfikar Abedzadeh Abelzad Afnan Ahrar Akhlaq Alam Amer Amin Amir Amirsadegh Amin Amouzegar Amuzgar Ansar Ansar Anvar Ardabil Arf Armanjan Asgapur Ashraf Avarigan Aziz Bahonar Bakhtavar Bakhtiar Baraher Bazargan Bihmard Dalvand Dastghayb Dihmubid Dihqan Dulab Ebteh Eftekhar Estil Estilid Etehad Fahandiz Fatem Forrug Gafar Garous Ghatar Ghuran Habib Ham Haqiqat Hejaz Hisam Hoveyd Hushmand Imam Iravan Ishraq Izad Jafar Jahanpur Jalil Javad Jayhun Kar Kazem Kazim Keshuapad Khadim Khakpour Khamen Khandil Khan Khanlar Khatam Khomein Khuzayn Madar Mahmud Majid Mansur Marad Mazlum Mehan Mehr Meshkat Minavand Misb Moham Mosad Moul Mualim Muhamad Mumtaz Mumtaz Muqim Musav Mutlaq Najaf Nakis Nasir Nazar Nemaz Nezam Nirumand Norouz Nour Pahlav Pakravan Pash Qaz Raban Rad Rafig Rahim Rawhan Razmar Sabah Sabet Sabir Sadiq Salimpour Sanjab Shafaq Shahriar Shah Shariat Sharud Shiraz Sot Sumech Shah Talaqan Talav Taleb Taleqan Vafar Vahdat Yaldar Zahed Zanjan Zarincheh";
            m_pFamily = new Confluxer(sFamily, 2);

            string sFemale = "Abia Abida Abla Abra Adara Adiba Adila Adiva Aisha Alima Alia Amala Amina Amira Asima Asiya Atifa Bilqis Bushra Cala Cantara Fadila Faiza Fara Fatima Fatina Habiba Haifa Halima Hamida Hanifa Jala Jamila Johara Jumana Kalila Karima Latifa Layla Leila Madiha Maha Malika Munira Nabiha Nabila Nadida Nadira Nafisa Naila Naima Najiba Nashida Nashita Nasiha Nasira Nathifa Rafa Rahima Rihana Sabira Safa Safiya Sakina Salima Samira Sharifa Sumaiya Tahira Taliba Wahiba Yafia Yamha Yamina Yasira Yasmina Yusra Zafira Zahira Zahra Zainaba Zaina Zubaida";
            m_pFemale = new Confluxer(sFemale, 2);

            string sMale = "Abdul Hakam Hakim Naser Rahman Rashid Salam Samad Wahid Wahab Abbas Abed Ashraf Ahmad Akram Ali Amjad Azhar Abu Burhan Fadil Fahad Farid Muhamad Fuad Harun Hasan Hashim Husam Imad Imran Irfan Jabir Jafar Jalal Jamal Jihad Jawad Kamal Khalid Mahir Mahmoud Marwan Muayid Munir Muntasir Musad Mustafa Nabhan Nabih Nabil Nadim Nadir Najeb Naser Nazih Omar Omran Qasim Rabah Rajab Rashad Rashid Ratib Sabir Sahir Saed Said Salman Samir Sulayman Tahir Tamir Umar Walid Yaman Yasar Yasin Yasir Yazid Yonus Yosuf Zafir Zahid Zakariya Ziyad Zahir";
            m_pMale = new Confluxer(sMale, 2);
        }

        #region ILanguage Members

        protected override string GetNationName()
        {
            return m_pNations.Generate() + "an";
        }

        protected override string GetFamily()
        {
            return "al-" + m_pFamily.Generate();
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
