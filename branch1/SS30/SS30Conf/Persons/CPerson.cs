using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml;
using nsUniLibXML;
using SS30Conf.Actions;
using SS30Conf.Time;
using Random;

namespace SS30Conf.Persons
{
    public enum Gender
    { 
        MALE,
        FEMALE
    }

    public abstract class CPerson : CConfigObject
    {
        private CConfigProperty<StringSubCategory> m_pSubCategory;

        public StringSubCategory SubCategory
        {
            get { return m_pSubCategory.Value; }
        }

        private CConfigProperty<Gender> m_pGender;

        public Gender Gender
        {
            get { return m_pGender.Value; }
            set { m_pGender.Value = value; }
        }

        private Dictionary<string, int> m_eStats = new Dictionary<string, int>();

        [ImmutableObject(true)]
        public Dictionary<string, int> Stats
        {
            get { return m_eStats; }
        }

        private Dictionary<string, int> m_eSkills = new Dictionary<string, int>();

        [ImmutableObject(true)]
        public Dictionary<string, int> Skills
        {
            get { return m_eSkills; }
        }

        private List<string> m_cStatuses = new List<string>();

        public List<string> Statuses
        {
            get { return m_cStatuses; }
        }

        private Dictionary<DayTime, CAction> m_cSheduledActions = new Dictionary<DayTime, CAction>();

        public Dictionary<DayTime, CAction> SheduledActions
        {
            get { return m_cSheduledActions; }
        }

        private CRoom m_pRoom = null;

        public CRoom Room
        {
            get { return m_pRoom; }
        }

        public void GoTo(CRoom pRoom)
        {
            if (m_pRoom != null)
                m_pRoom.Persons.Remove(this);

            pRoom.Persons.Add(this);
            m_pRoom = pRoom;
        }

        public void GoTo(CPerson pPerson)
        {
            if (pPerson.Room == null)
                return;

            if (m_pRoom != null)
                m_pRoom.Persons.Remove(this);

            pPerson.Room.Persons.Add(this);
            m_pRoom = pPerson.Room;
        }

        public CPerson(StringSubCategory eSubCategory, Gender eDefaultGender)
            : base(StringCategory.PERSON)
        {
            m_pSubCategory.Value = eSubCategory;

            Name = "New Person";
            Gender = eDefaultGender;
        }

        public CPerson(StringSubCategory eSubCategory, Gender eGender, string sId)
            : base(StringCategory.PERSON, sId)
        {
            m_pSubCategory.Value = eSubCategory;

            Name = "New Person";
            Gender = eGender;
        }

        public CPerson(UniLibXML xml, XmlNode pNode, CWorld pWorld)
            : base(xml, pNode)
        {
            pWorld.Persons.Add(this);
        }

        protected override void InitProperties()
        {
            base.InitProperties();

            m_pSubCategory = new CConfigProperty<StringSubCategory>(this, "SubCategory", StringSubCategory.NULL);

            m_pGender = new CConfigProperty<Gender>(this, "Gender", Gender.FEMALE);
        }

        public int GetStat(PersonStats stat)
        { 
            if(!CConfigRepository.Instance.StatsNames.ContainsKey(stat))
                return 0;

            string statName = CConfigRepository.Instance.StatsNames[stat];

            if (!Stats.ContainsKey(statName))
                return 0;

            return Stats[statName];
        }

        private void SetStat(PersonStats stat, int value)
        {
            if(!CConfigRepository.Instance.StatsNames.ContainsKey(stat))
                return;

            if (value < 0)
                value = 0;
            if (value > 100)
                value = 100;
            
            Stats[CConfigRepository.Instance.StatsNames[stat]] = value;
        }

        private void ModifyStat(PersonStats stat, int value)
        {
            if (!CConfigRepository.Instance.StatsNames.ContainsKey(stat))
                return;

            string statName = CConfigRepository.Instance.StatsNames[stat];

            int oldValue = 0;
            if (Stats.ContainsKey(statName))
                oldValue = Stats[statName];

            SetStat(stat, oldValue + value);
        }

        private void ModifyStatPercent(PersonStats stat, int percent)
        {
            if (!CConfigRepository.Instance.StatsNames.ContainsKey(stat))
                return;

            string statName = CConfigRepository.Instance.StatsNames[stat];

            int oldValue = 0;
            if (Stats.ContainsKey(statName))
                oldValue = Stats[statName];

            SetStat(stat, oldValue + oldValue * percent / 100);
        }

        public void Rest(bool bNight)
        {
            int iCount = 2;
            if (bNight)
                iCount = 10;

            ModifyStatPercent(PersonStats.fatique, -GetStat(PersonStats.health) * iCount / 20);
            //Room.RecordEvent(string.Format("{0} усталость    ={1} (--)", Name, GetStat(PersonStats.fatique)));
            //Если здоровье выше бодрости, т.е. персонаж здоров, но устал
            if (GetStat(PersonStats.health) > 100 - GetStat(PersonStats.fatique))
            {
                //то снижаем усталость пропорционально здоровью и расходуем единицу здоровья
                ModifyStatPercent(PersonStats.health, -iCount);
                //Room.RecordEvent(string.Format("{0} здоровье     ={1} (-)", Name, GetStat(PersonStats.health)));
            }
            else
            {
                //иначе, т.е. если персонаж болен, но хорошо отдохнул,
                //то повышаем усталость пропорционально недостатку здоровья и восстанавливаем единицу здоровья
                for (int i = 0; i < iCount - 1; i++)
                {
                    if (Rnd.Get(100) > GetStat(PersonStats.stress)/2 + GetStat(PersonStats.fatique))
                        ModifyStat(PersonStats.health, 1);
                }
                //Room.RecordEvent(string.Format("{0} здоровье     ={1} (+)", Name, GetStat(PersonStats.health)));
            }

            ModifyStatPercent(PersonStats.stress, -GetStat(PersonStats.happiness) * iCount / 20);
            //Room.RecordEvent(string.Format("{0} стресс       ={1} (--)", Name, GetStat(PersonStats.stress)));
            //Если счастье выше радости, т.е. персонаж в общем доволен жизнью, но конкретно сейчас испытывает стресс
            if (GetStat(PersonStats.happiness) > 100 - GetStat(PersonStats.stress))
            {
                //то снижаем стресс пропорционально счастью и расходуем единицу счастья
                ModifyStatPercent(PersonStats.happiness, -iCount);
                //Room.RecordEvent(string.Format("{0} счастье      ={1} (-)", Name, GetStat(PersonStats.happiness)));
            }
            else
            {
                //иначе, т.е. если персонаж в общем несчастен, но конкретно сейчас у него всё хорошо,
                //то повышаем стресс пропорционально недостатку счастья, но счастье повышаем на 1
                for (int i = 0; i < iCount-1; i++)
                {
                    if (Rnd.Get(100) > GetStat(PersonStats.stress) + GetStat(PersonStats.fatique)/2)
                        ModifyStat(PersonStats.happiness, 1);
                }
                //Room.RecordEvent(string.Format("{0} счастье      ={1} (+)", Name, GetStat(PersonStats.happiness)));
            }

            ModifyStat(PersonStats.arousal, GetStat(PersonStats.sexuality) * iCount / 100);
            //Room.RecordEvent(string.Format("{0} возбуждение  ={1} (++)", Name, GetStat(PersonStats.arousal)));
            //Если сексуальность (aka потребность в сексе) выше удовлетворённости, 
            //т.е. персонаж получает меньше секса, чем хочет
            if (GetStat(PersonStats.sexuality) > 100 - GetStat(PersonStats.arousal))
            {
                //то повышаем возбуждение пропорционально сексуальности, но сексуальность снижаем на 1
                ModifyStatPercent(PersonStats.sexuality, -iCount);
                //Room.RecordEvent(string.Format("{0} сексуальность={1} (-)", Name, GetStat(PersonStats.sexuality)));
            }
            else
            {
                //иначе, т.е. если персонаж сам по себе не слишком озабочен сексом, но вынужден вести активную сексуальную жизнь,
                //то всё равно повышаем возбуждение (т.е. снижаем удовлетворённость) пропорционально сексуальности, 
                //но сексуальность при этом так же повышаем на 1
                ModifyStat(PersonStats.sexuality, iCount/2);
                //Room.RecordEvent(string.Format("{0} сексуальность={1} (+)", Name, GetStat(PersonStats.sexuality)));
            }
        }

        internal void Tire()
        {
            ModifyStat(PersonStats.fatique, (100 - GetStat(PersonStats.endurance)) / 10);
            //Room.RecordEvent(string.Format("{0} усталость    ={1} (+)", Name, GetStat(PersonStats.fatique)));
            if (Rnd.Get(100) > GetStat(PersonStats.endurance))
            {
                ModifyStat(PersonStats.endurance, 1);
                //Room.RecordEvent(string.Format("{0} выносливость ={1} (+)", Name, GetStat(PersonStats.endurance)));
            }
        }

        internal void Punish()
        {
            ModifyStat(PersonStats.stress, (100 - GetStat(PersonStats.obedience)) / 10);
            //Room.RecordEvent(string.Format("{0} стресс       ={1} (+)", Name, GetStat(PersonStats.stress)));
            if (Rnd.Get(100) > GetStat(PersonStats.obedience))
            {
                ModifyStat(PersonStats.obedience, 1);
                //Room.RecordEvent(string.Format("{0} покорность   ={1} (+)", Name, GetStat(PersonStats.obedience)));
            }
        }

        internal void Cum()
        {
            ModifyStatPercent(PersonStats.arousal, -GetStat(PersonStats.sensuality));
            //Room.RecordEvent(string.Format("{0} возбуждение  ={1} (-)", Name, GetStat(PersonStats.arousal)));
            if (Rnd.Get(100) > GetStat(PersonStats.sensuality))
            {
                ModifyStat(PersonStats.sensuality, 1);
                //Room.RecordEvent(string.Format("{0} чувственность={1} (+)", Name, GetStat(PersonStats.sensuality)));
            }
        }
    }
}
