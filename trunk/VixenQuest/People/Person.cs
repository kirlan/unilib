using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nsUniLibXML;
using System.Xml;
using VixenQuest.World;

namespace VixenQuest
{
    public enum Gender
    {
        Male,
        Female,
        Shemale
    }

    public enum Orientation
    {
        Stright,
        Bi,
        Homo
    }

    public enum Stat
    {
        Force,
        Beauty,
        Luck,
        Potency
    }

    public enum VixenSkill
    {
        Anal,
        Oral,
        Traditional,
        SM
    }

    public abstract class Person
    {
        protected Gender m_eGender = Gender.Female;

        public Gender Gender
        {
            get { return m_eGender; }
        }

        public string GenderString
        {
            get
            {
                switch (m_eGender)
                {
                    case Gender.Male:
                        return "Male";
                    case Gender.Female:
                        return "Female";
                    case Gender.Shemale:
                        return "Shemale";
                    default:
                        return "Unknown";
                }
            }
        }

        protected Orientation m_eOrientation = Orientation.Bi;

        public Orientation Orientation
        {
            get { return m_eOrientation; }
        }

        public string OrientationString
        {
            get
            {
                switch (m_eOrientation)
                {
                    case Orientation.Stright:
                        return "Stright";
                    case Orientation.Bi:
                        return "Bi";
                    case Orientation.Homo:
                        if (m_eGender == Gender.Male)
                            return "Gay";
                        else
                            return "Lesbian";
                    default:
                        return "Unknown";
                }
            }
        }

        public virtual bool WannaFuck(Person pOpponent)
        {
            switch (m_eOrientation)
            {
                case Orientation.Stright:
                    if ((m_eGender == Gender.Male && pOpponent.Gender == Gender.Male) ||
                        (m_eGender != Gender.Male && pOpponent.Gender != Gender.Male))
                        return false;
                    break;
                case Orientation.Homo:
                    if ((m_eGender == Gender.Male && pOpponent.Gender != Gender.Male) ||
                        (m_eGender != Gender.Male && pOpponent.Gender == Gender.Male))
                        return false;
                    break;
            }
            return true;
        }

        protected int m_iLevel = 1;

        public int Level
        {
            get { return m_iLevel; }
        }

        protected Dictionary<Stat, int> m_cStats = new Dictionary<Stat, int>();

        /// <summary>
        /// Статсы оппонента - уже с учётом численности группы
        /// </summary>
        public Dictionary<Stat, int> Stats
        {
            get { return m_cStats; }
        }

        protected Dictionary<VixenSkill, int> m_cSkills = new Dictionary<VixenSkill, int>();

        public Dictionary<VixenSkill, int> Skills
        {
            get { return m_cSkills; }
        }

        internal void Write2XML(UniLibXML pXml, XmlNode pNode)
        {
            pXml.AddAttribute(pNode, "gender", m_eGender);
            pXml.AddAttribute(pNode, "orientation", m_eOrientation);
            pXml.AddAttribute(pNode, "level", m_iLevel);

            XmlNode pStatsNode = pXml.CreateNode(pNode, "Stats");
            foreach (Stat eStat in m_cStats.Keys)
            {
                XmlNode pStatNode = pXml.CreateNode(pStatsNode, "Stat");
                pXml.AddAttribute(pStatNode, "name", eStat);
                pXml.AddAttribute(pStatNode, "value", m_cStats[eStat]);
            }

            XmlNode pSkillsNode = pXml.CreateNode(pNode, "Skills");
            foreach (VixenSkill eSkill in m_cSkills.Keys)
            {
                XmlNode pSkillNode = pXml.CreateNode(pSkillsNode, "Skill");
                pXml.AddAttribute(pSkillNode, "name", eSkill);
                pXml.AddAttribute(pSkillNode, "value", m_cSkills[eSkill]);
            }
        }

        public Location m_pHome = null;
    }
}
