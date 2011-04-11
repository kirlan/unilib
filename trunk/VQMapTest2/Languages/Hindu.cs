using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NameGen;
using Random;

namespace VQMapTest2.Languages
{
    class Hindu:Language
    {
        private Confluxer m_pCountry;

        private Confluxer m_pTowns;

        private Confluxer m_pVillages;

        public Hindu()
            : base(NameGenerator.Language.NONE)
        { 
            string sCountry = "Majipuram Rajipuram Iranistan  Indostan Penjab Kashmir Bengal Kanyakumari Mannar Bhutan Nepal Pakistan Punjab Burma Bangladesh Khasi Baltistan Kanchenjunga Pradesh Assam Bihar Chatisgarth Goa Gujarat Haryana Andha Arunachal Himachal Jamu Jharkhand Karnataka Kerala Madhya Maharashtra Manipur Meghalaya Mizoram Orissa Rajastan Sikim Tripura Utar Utarakhand Dadra Nagar Daman Afghanistan Karakoram Siachen Biafo Kush Patkai Purvanchal Mawsynram Xherrapunji Vindya Aravali Mirzapur Satpura Gujarat Haryana Shikhar Godavari Mahanadi Krishna Kaveri Yamuna Ravi Chambal Sutlej Chenab Bhabar Bangar Khadar Nagpuram Kathiawar Khatowar Gujarat Baratang Sriharikota Vindhya Satpura Sahyadri Andaman Vembanad Loktak Sambhar Sasthamkotta";
            m_pCountry = new Confluxer(sCountry, 2);

            string sTowns = "Achalpur Adipur Anantapur Anupur Burhanpur Baharampur Balrampur Barackpur Berhampur Bhagalpur Bhajanpur Bharatpur Bilaspur Burhanpur Chandrapur Chatapur Dharampur Dimapur Dourgapur Firozpur Ghatampur Ghazipur Gorakhpur Hoshiarpur Hamirpur Hastinapur Islampur Jabalpur Jagdalpur Jaipur Jamshedpur Jaunpur Jodhpur Kanchipuram Kanpur Kharagpur Kolhapur Lakhimpur Lalitpur Markapur Mirzapur Mithapur Mulanpur Muzafapur Narsimhapur Nurpur Padampur Palanpur Pandharpur Raipur Rairangpur Rajgangpur Ramapur Rampur Saharanpur Samastipur Sambalpur Shajanpur Sheopur Sholapur Shrirampur Sidhpur Sitapur Tezpur Tirupur Udaipur Udhampur Vilupur Zirakpur";
            m_pTowns = new Confluxer(sTowns, 2);

            string sVillages = "Adra Agra Akola Ambala Amakandakara Amravati Amreli Amroha Araria Arupukotai Ashtamichira Baharagora Bahra Balagha Balia Bamra Banda Bangana Banswara Bankura Bapatla Barasa Bardhama Baraka Basna Baripada Barwani Belary Belpaha Bhandara Bharthana Bhavani Bhavnaga Bhilai Bhilwara Bhimavara Bhinma Bhopa Bhusawa Bida Biha Bijna Bikana Bilara Brajrajna Cambay Chamba Champa Chamrajna Chamdanaga Chandi Chapra Chindwara Chirala Chitradurga Cudapa Dabra Damana Barbhanga Daunda Dehgama Dehraduna Dehri Delhi Dewas Dhamtari Dhari Dharwa Dholka Dhuri Durga Ganja Gudala Gudivada Gulbarga Guntura Gurga Halisaha Hisa Ichalkara Ida Impha Itanaga Jaitara Jamnaga Jhabua Jhaja Jhalawa Jorha Kalimponga Kanu Karimnaga Karunga Khaga Khama Khurai Kohima Konaga Kotdwara Krishnanaga Kuchinda Kurukshetra Madanapa Mahabubnaga Mahuva Mahwa Mandla Mangalora Mapusa Meruta Mehsana Modasa Mohana Mokama Mumbai Muradnaga Murwara Muzafarnaga Nagao Naga Namaka Narna Nandurba Nawansha Palwa Panchkula Paratwada Pata Patna Phagwara Pokara Porbanda Punalu Pushka Raichu Rajampeta Rajgurunaga Ramanagara Rameshwara Ramnagara Rishra Rohta Sanawa Sanchore Sangamneri Sangru Satara Satna Seohara Shahada Shega Shimla Shirala Shivani Sibsagara Sika Sindha Sonipata Srinagara Talwara Tindivana Tiruchirapa Udgi Utarpa Vadodara Vatakara Vasai Vira Vyara";
            m_pVillages = new Confluxer(sVillages, 2);
        }

        protected override string GetCountryName()
        {
            return m_pCountry.Generate();
        }

        protected override string  GetTownName()
        {
 	         return Rnd.OneChanceFrom(3) ? m_pVillages.Generate() : m_pTowns.Generate();
        }

        protected override string GetVillageName()
        {
            return m_pVillages.Generate();
        }

        protected override string GetFamily()
        {
            return NameGenerator2.GetHeroName(NameGenerator2.Language.Hindu);
        }

        public override string RandomMaleName()
        {
            return NameGenerator2.GetHeroName(NameGenerator2.Language.Hindu);
        }
    }
}
