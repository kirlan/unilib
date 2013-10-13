using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persona.Consequences;
using Persona.Conditions;
using nsUniLibXML;
using System.Xml;
using Persona.Parameters;

namespace Persona
{
    /// <summary>
    /// Возможная реакция персонажа на событие.
    /// </summary>
    public class Reaction
    {
        /// <summary>
        /// Если true, то реакция будет отображаться в общем списке, даже если условие не выполняется,
        /// просто её нельзя будет выбрать.
        /// </summary>
        public bool m_bAlwaysVisible;

        /// <summary>
        /// Краткое описание реакции, как оно будет выводиться в списке возможных реакций для пользователя.
        /// </summary>
        public string m_sName;
        /// <summary>
        /// Полное описание реакции и её последствий, как оно будет выводиться в игровой лог после того,
        /// как пользователь выбрал эту реакцию.
        /// </summary>
        public string m_sResult;

        /// <summary>
        /// Список условий, при которых эта реакция доступна.
        /// Условия в списке связываются друг с другом логическим И.
        /// При m_bAlwaysVisible==false недоступные реакции не будут отображаться в предоставляемом игроку списке, иначе - будут, но выбрать их всё-равно нельзя.
        /// </summary>
        public List<Condition> m_cConditions = new List<Condition>();

        /// <summary>
        /// Список последствий выбора этой реакции, применяется к текущему состоянию системы после выбора
        /// реакции пользователем.
        /// </summary>
        public List<Consequence> m_cConsequences = new List<Consequence>();

        public Reaction()
        {
        }

        public Reaction(Reaction pOrigin)
        {
            m_sName = pOrigin.m_sName;
            m_sResult = pOrigin.m_sResult;
            m_bAlwaysVisible = pOrigin.m_bAlwaysVisible;

            foreach (var pCondition in pOrigin.m_cConditions)
                m_cConditions.Add(pCondition.Clone());

            foreach (var pConsequence in pOrigin.m_cConsequences)
                m_cConsequences.Add(pConsequence.Clone());
        }

        public Reaction(UniLibXML pXml, XmlNode pParamNode, List<Parameter> cParams)
        {
            pXml.GetStringAttribute(pParamNode, "name", ref m_sName);
            pXml.GetStringAttribute(pParamNode, "result", ref m_sResult);
            pXml.GetBoolAttribute(pParamNode, "visible", ref m_bAlwaysVisible);

            foreach (XmlNode pSubNode in pParamNode.ChildNodes)
            {
                if (pSubNode.Name == "Conditions")
                {
                    foreach (XmlNode pConditionNode in pSubNode.ChildNodes)
                    {
                        if (pConditionNode.Name == "Range")
                        {
                            ConditionRange pCondition = new ConditionRange(pXml, pConditionNode, cParams);
                            m_cConditions.Add(pCondition);
                        }
                        if (pConditionNode.Name == "Comparsion")
                        {
                            ConditionComparsion pCondition = new ConditionComparsion(pXml, pConditionNode, cParams);
                            m_cConditions.Add(pCondition);
                        }
                        if (pConditionNode.Name == "Status")
                        {
                            ConditionStatus pCondition = new ConditionStatus(pXml, pConditionNode, cParams);
                            m_cConditions.Add(pCondition);
                        }
                    }
                }

                if (pSubNode.Name == "Consequences")
                {
                    foreach (XmlNode pConsequenceNode in pSubNode.ChildNodes)
                    {
                        if (pConsequenceNode.Name == "ParameterChange")
                        {
                            ParameterChange pConsequence = new ParameterChange(pXml, pConsequenceNode, cParams);
                            m_cConsequences.Add(pConsequence);
                        }
                        if (pConsequenceNode.Name == "ParameterSet")
                        {
                            ParameterSet pConsequence = new ParameterSet(pXml, pConsequenceNode, cParams);
                            m_cConsequences.Add(pConsequence);
                        }
                        if (pConsequenceNode.Name == "SystemCommand")
                        {
                            SystemCommand pConsequence = new SystemCommand(pXml, pConsequenceNode, cParams);
                            m_cConsequences.Add(pConsequence);
                        }
                    }
                }
            }
        }


        internal void WriteXML(UniLibXML pXml, XmlNode pReactionNode)
        {
            pXml.AddAttribute(pReactionNode, "name", m_sName);
            pXml.AddAttribute(pReactionNode, "result", m_sResult);
            pXml.AddAttribute(pReactionNode, "visible", m_bAlwaysVisible);

            XmlNode pConditionsNode = pXml.CreateNode(pReactionNode, "Conditions");
            foreach (Condition pCondition in m_cConditions)
            {
                if (pCondition is ConditionRange)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pConditionsNode, "Range");
                    pCondition.WriteXML(pXml, pConditionNode);
                }
                if (pCondition is ConditionComparsion)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pConditionsNode, "Comparsion");
                    pCondition.WriteXML(pXml, pConditionNode);
                }
                if (pCondition is ConditionStatus)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pConditionsNode, "Status");
                    pCondition.WriteXML(pXml, pConditionNode);
                }
            }

            XmlNode pConsequencesNode = pXml.CreateNode(pReactionNode, "Consequences");
            foreach (Consequence pConsequence in m_cConsequences)
            {
                if (pConsequence is ParameterChange)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pConsequencesNode, "ParameterChange");
                    pConsequence.WriteXML(pXml, pConditionNode);
                }
                if (pConsequence is ParameterSet)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pConsequencesNode, "ParameterSet");
                    pConsequence.WriteXML(pXml, pConditionNode);
                }
                if (pConsequence is SystemCommand)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pConsequencesNode, "SystemCommand");
                    pConsequence.WriteXML(pXml, pConditionNode);
                }
            }
        }

        internal bool Possible()
        {
            foreach (Condition pCondition in m_cConditions)
            {
                if (!pCondition.Check())
                    return false;
            }

            return true;
        }

        internal string Execute(Module pModule)
        {
            foreach (Consequence pConsequence in m_cConsequences)
                pConsequence.Apply(pModule);
                
            return m_sResult;
        }
    }
}
