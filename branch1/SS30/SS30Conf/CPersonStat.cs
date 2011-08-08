using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SS30Conf
{
    public enum PersonStats
    {
        health,
        fatique,
        endurance,
        happiness,
        stress,
        obedience,
        sexuality,
        arousal,
        sensuality,
        service_satisfaction,
        service_need,
        leisure_setisfaction,
        leisure_need,
        sex_satisfaction,
        sex_need
    }

    public class CPersonStat: CConfigObject
    {
        private CConfigProperty<PersonStats> m_pType;

        public PersonStats Type
        {
            get { return m_pType.Value; }
        }

        private Dictionary<StringSubCategory, bool> m_cAvailability = new Dictionary<StringSubCategory, bool>();

        [ImmutableObject(true)]
        public Dictionary<StringSubCategory, bool> Availability
        {
            get { return m_cAvailability; }
        }

        public CPersonStat(PersonStats eType, string sName)
            : base(StringCategory.STAT, Enum.GetName(typeof(PersonStats), eType))
        {
            m_pType.Value = eType;
            Name = sName;

            m_cAvailability[StringSubCategory.PERSON_GUEST] = false;
            m_cAvailability[StringSubCategory.PERSON_MAID] = false;
            m_cAvailability[StringSubCategory.PERSON_SPECIAL] = false;

            CConfigRepository.Instance.StatsNames[eType] = Value;
        }

        public void SetAvailable(StringSubCategory ePersonCategory)
        {
            if (m_cAvailability.ContainsKey(ePersonCategory))
                m_cAvailability[ePersonCategory] = true;
        }

        protected override void InitProperties()
        {
            base.InitProperties();

            m_pType = new CConfigProperty<PersonStats>(this, "Type", PersonStats.health);
        }
    }
}
