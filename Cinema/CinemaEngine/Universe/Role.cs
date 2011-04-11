using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nsUniLibXML;
using System.Xml;
using CinemaEngine.RoleConditions;

namespace CinemaEngine
{
    /// <summary>
    /// Роль в действии. Может быть назначена только персонажу, соответсвующему заданным условиям.
    /// Так же определяет изменение состояния персонажа после окончания действия.
    /// </summary>
    public class Role
    {
        private string m_sName;

        public string Name
        {
            get { return m_sName; }
        }

        private Dictionary<RoleCondition.ConditionType, RoleCondition> m_cConditions = new Dictionary<RoleCondition.ConditionType, RoleCondition>();

        public Dictionary<RoleCondition.ConditionType, RoleCondition> Conditions
        {
            get { return m_cConditions; }
        }

        private Dictionary<RoleCondition.ConditionType, RoleCondition> m_cConsequences = new Dictionary<RoleCondition.ConditionType, RoleCondition>();

        public Dictionary<RoleCondition.ConditionType, RoleCondition> Consequences
        {
            get { return m_cConsequences; }
        }

        private void Init()
        {
            foreach (RoleCondition.ConditionType eType in Enum.GetValues(typeof(RoleCondition.ConditionType)))
            {
                switch (eType)
                {
                    case RoleCondition.ConditionType.Pose:
                        m_cConditions[eType] = new PoseCondition();
                        m_cConsequences[eType] = new PoseCondition();
                        break;
                    case RoleCondition.ConditionType.Gender:
                        m_cConditions[eType] = new GenderCondition();
                        break;
                    case RoleCondition.ConditionType.Bindings:
                        m_cConditions[eType] = new BindingsCondition();
                        m_cConsequences[eType] = new BindingsCondition();
                        break;
                }
            }
        }

        public Role(string sName)
        {
            Init();
            m_sName = sName;
        }

        public Role(UniLibXML pXml, XmlNode pRoleNode)
        {
            Init();
            pXml.GetStringAttribute(pRoleNode, "name", ref m_sName);

            foreach (XmlNode pSubNode in pRoleNode.ChildNodes)
            {
                if (pSubNode.Name == "Conditions")
                {
                    foreach (XmlNode pSubSubNode in pSubNode.ChildNodes)
                    {
                        RoleCondition.ConditionType eType = (RoleCondition.ConditionType)Enum.Parse(typeof(RoleCondition.ConditionType), pSubSubNode.Name);
                        switch (eType)
                        {
                            case RoleCondition.ConditionType.Pose:
                                m_cConditions[eType] = new PoseCondition(pXml, pSubSubNode);
                                break;
                            case RoleCondition.ConditionType.Gender:
                                m_cConditions[eType] = new GenderCondition(pXml, pSubSubNode);
                                break;
                            case RoleCondition.ConditionType.Bindings:
                                m_cConditions[eType] = new BindingsCondition(pXml, pSubSubNode);
                                break;
                        }
                    }
                }
                if (pSubNode.Name == "Consequences")
                {
                    foreach (XmlNode pSubSubNode in pSubNode.ChildNodes)
                    {
                        RoleCondition.ConditionType eType = (RoleCondition.ConditionType)Enum.Parse(typeof(RoleCondition.ConditionType), pSubSubNode.Name);
                        switch (eType)
                        {
                            case RoleCondition.ConditionType.Pose:
                                m_cConsequences[eType] = new PoseCondition(pXml, pSubSubNode);
                                break;
                            //case ConditionType.Gender:
                            //    m_cConsequences[eType] = new GenderCondition(pXml, pSubSubNode);
                            //    break;
                            case RoleCondition.ConditionType.Bindings:
                                m_cConsequences[eType] = new BindingsCondition(pXml, pSubSubNode);
                                break;
                        }
                    }
                }
            }
        }

        internal void Write(UniLibXML pXml, XmlNode pRoleNode)
        {
            pXml.AddAttribute(pRoleNode, "name", m_sName);

            XmlNode pConditionsNode = pXml.CreateNode(pRoleNode, "Conditions");

            foreach (RoleCondition.ConditionType eType in m_cConditions.Keys)
            {
                XmlNode pConditionNode = pXml.CreateNode(pConditionsNode, Enum.GetName(typeof(RoleCondition.ConditionType), eType));
                m_cConditions[eType].Write(pXml, pConditionNode);
            }

            XmlNode pConsequencesNode = pXml.CreateNode(pRoleNode, "Consequences");

            foreach (RoleCondition.ConditionType eType in m_cConsequences.Keys)
            {
                XmlNode pConsequenceNode = pXml.CreateNode(pConsequencesNode, Enum.GetName(typeof(RoleCondition.ConditionType), eType));
                m_cConsequences[eType].Write(pXml, pConsequenceNode);
            }
        }

        public Role(Role pOriginal)
        {
            Init();
            m_sName = pOriginal.m_sName;

            foreach (RoleCondition.ConditionType eType in pOriginal.m_cConditions.Keys)
            {
                m_cConditions[eType] = pOriginal.m_cConditions[eType].Duplicate();
            }
            foreach (RoleCondition.ConditionType eType in pOriginal.m_cConsequences.Keys)
            {
                m_cConsequences[eType] = pOriginal.m_cConsequences[eType].Duplicate();
            }
        }
    }
}
