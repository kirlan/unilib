using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persona.Consequences;
using Persona.Conditions;
using nsUniLibXML;
using System.Xml;
using Persona.Parameters;
using Persona.Collections;
using Persona.Flows;

namespace Persona
{
    /// <summary>
    /// Возможная реакция персонажа на событие.
    /// </summary>
    public class Reaction : Situation
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
        /// Список последствий выбора этой реакции, применяется к текущему состоянию системы после выбора
        /// реакции пользователем.
        /// </summary>
        public List<Consequence> m_cConsequences = new List<Consequence>();

        public Reaction()
        {
        }

        public Reaction(Reaction pOrigin)
            :base(pOrigin)
        {
            m_sName = pOrigin.m_sName;
            m_bAlwaysVisible = pOrigin.m_bAlwaysVisible;

            foreach (var pConsequence in pOrigin.m_cConsequences)
                m_cConsequences.Add(pConsequence.Clone());
        }

        public Reaction(UniLibXML pXml, XmlNode pParamNode, List<Parameter> cParams, List<ObjectsCollection> cCollections, List<Flow> cFlows)
        {
            pXml.GetStringAttribute(pParamNode, "name", ref m_sName);
            pXml.GetStringAttribute(pParamNode, "result", ref m_sText);
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
                        if (pConditionNode.Name == "Selected")
                        {
                            ConditionObjectSelected pCondition = new ConditionObjectSelected(pXml, pConditionNode, cCollections);
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
                        if (pConsequenceNode.Name == "ParameterChangeVariable")
                        {
                            ParameterChangeVariable pConsequence = new ParameterChangeVariable(pXml, pConsequenceNode, cParams);
                            m_cConsequences.Add(pConsequence);
                        }
                        if (pConsequenceNode.Name == "ParameterSet")
                        {
                            ParameterSet pConsequence = new ParameterSet(pXml, pConsequenceNode, cParams);
                            m_cConsequences.Add(pConsequence);
                        }
                        if (pConsequenceNode.Name == "CollectionSelect")
                        {
                            CollectionSelect pConsequence = new CollectionSelect(pXml, pConsequenceNode, cCollections);
                            m_cConsequences.Add(pConsequence);
                        }
                        if (pConsequenceNode.Name == "CollectionShuffle")
                        {
                            CollectionShuffle pConsequence = new CollectionShuffle(pXml, pConsequenceNode, cCollections);
                            m_cConsequences.Add(pConsequence);
                        }
                        if (pConsequenceNode.Name == "FlowProgress")
                        {
                            FlowProgress pConsequence = new FlowProgress(pXml, pConsequenceNode, cFlows);
                            m_cConsequences.Add(pConsequence);
                        }
                        if (pConsequenceNode.Name == "FlowProgressVariable")
                        {
                            FlowProgressVariable pConsequence = new FlowProgressVariable(pXml, pConsequenceNode, cParams, cFlows);
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


        internal new void WriteXML(UniLibXML pXml, XmlNode pReactionNode)
        {
            pXml.AddAttribute(pReactionNode, "name", m_sName);
            pXml.AddAttribute(pReactionNode, "result", m_sText);
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
                if (pCondition is ConditionObjectSelected)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pConditionsNode, "Selected");
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
                if (pConsequence is ParameterChangeVariable)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pConsequencesNode, "ParameterChangeVariable");
                    pConsequence.WriteXML(pXml, pConditionNode);
                }
                if (pConsequence is ParameterSet)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pConsequencesNode, "ParameterSet");
                    pConsequence.WriteXML(pXml, pConditionNode);
                }
                if (pConsequence is CollectionShuffle)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pConsequencesNode, "CollectionShuffle");
                    pConsequence.WriteXML(pXml, pConditionNode);
                }
                if (pConsequence is CollectionSelect)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pConsequencesNode, "CollectionSelect");
                    pConsequence.WriteXML(pXml, pConditionNode);
                }
                if (pConsequence is FlowProgress)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pConsequencesNode, "FlowProgress");
                    pConsequence.WriteXML(pXml, pConditionNode);
                }
                if (pConsequence is FlowProgressVariable)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pConsequencesNode, "FlowProgressVariable");
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
            pModule.m_sLog.AppendLine("REACTION " + m_sName);

            foreach (Consequence pConsequence in m_cConsequences)
                pConsequence.Apply(pModule);

            return GetDescription(pModule.m_cStringParameters, pModule.m_cNumericParameters, pModule.m_cCollections);
        }
    }
}
