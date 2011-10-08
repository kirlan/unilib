using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nsUniLibXML;
using System.Xml;

namespace CinemaEngine.RoleConditions
{
    /// <summary>
    /// Условие, основанное на поле персонажа
    /// </summary>
    public class GenderCondition : RoleCondition
    {
        private List<Actor.Gender> m_cGenders = new List<Actor.Gender>();
        /// <summary>
        /// Список полов, к которым может принадлежать персонаж для соответствия роли
        /// </summary>
        public List<Actor.Gender> Genders
        {
            get { return m_cGenders; }
        }

        public GenderCondition()
            : base()
        {
            foreach (Actor.Gender eGender in Enum.GetValues(typeof(Actor.Gender)))
                m_cGenders.Add(eGender);
        }

        public GenderCondition(GenderCondition pCondition)
            : base(pCondition)
        {
            m_cGenders.AddRange(pCondition.m_cGenders);
        }

        internal override void Write(UniLibXML pXml, XmlNode pConditionNode)
        {
            base.Write(pXml, pConditionNode);

            foreach (Actor.Gender eGender in m_cGenders)
            {
                XmlNode pGenderNode = pXml.CreateNode(pConditionNode, "Allowed");
                pXml.AddAttribute(pGenderNode, "gender", eGender);
            }
        }

        public GenderCondition(UniLibXML pXml, XmlNode pConditionNode)
            : base(pXml, pConditionNode)
        {
            foreach (XmlNode pSubNode in pConditionNode.ChildNodes)
            {
                if (pSubNode.Name == "Allowed")
                {
                    Actor.Gender eGender = Actor.Gender.Male;
                    eGender = (Actor.Gender)pXml.GetEnumAttribute(pSubNode, "gender", eGender.GetType());
                    m_cGenders.Add(eGender);
                }
            }
        }

        public override RoleCondition Duplicate()
        {
            return new GenderCondition(this);
        }
    }
}
