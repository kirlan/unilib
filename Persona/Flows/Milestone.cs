using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persona.Conditions;
using Persona.Consequences;
using nsUniLibXML;
using System.Xml;
using Persona.Parameters;
using Persona.Collections;

namespace Persona.Flows
{
    public class Milestone
    {
        public string m_sName;

        /// <summary>
        /// <para>Повторяемость метки.</para>
        /// <para>Если false, то метка срабатывает только единожды, 
        /// иначе - циклично повторяется с заданной периодичностью. </para>
        /// </summary>
        public bool m_bRepeatable = true;

        public float m_fPosition;

        /// <summary>
        /// Список условий, при которых эта метка может быть активирована.
        /// Условия в списке связываются друг с другом логическим И.
        /// </summary>
        public List<Condition> m_cConditions = new List<Condition>();
        
        /// <summary>
        /// Список последствий выбора этой реакции, применяется к текущему состоянию системы после выбора
        /// реакции пользователем.
        /// </summary>
        public List<Consequence> m_cConsequences = new List<Consequence>();

        public Milestone()
        { 
        }

        public Milestone(Milestone pOrigin)
        {
            m_sName = pOrigin.m_sName;
            m_fPosition = pOrigin.m_fPosition;
            m_bRepeatable = pOrigin.m_bRepeatable;

            foreach (var pCondition in pOrigin.m_cConditions)
                m_cConditions.Add(pCondition.Clone());

            foreach (var pConsequence in pOrigin.m_cConsequences)
                m_cConsequences.Add(pConsequence.Clone());
        }

        public Milestone(UniLibXML pXml, XmlNode pMilestoneNode, List<Parameter> cParams, List<ObjectsCollection> cCollections, List<Flow> cFlows, out Flow pFlow)
        {
            string sFlowName = "";
            pXml.GetStringAttribute(pMilestoneNode, "flow", ref sFlowName);

            pFlow = null;
            foreach (Flow pFlw in cFlows)
                if (pFlw.m_sName == sFlowName)
                {
                    pFlow = pFlw;
                    break;
                }

            if (pFlow == null)
                throw new Exception("No flow '" + sFlowName + "' found!");

            pXml.GetStringAttribute(pMilestoneNode, "name", ref m_sName);
            pXml.GetBoolAttribute(pMilestoneNode, "repeat", ref m_bRepeatable);
            pXml.GetFloatAttribute(pMilestoneNode, "position", ref m_fPosition);

            foreach (XmlNode pSubNode in pMilestoneNode.ChildNodes)
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
                        if (pConsequenceNode.Name == "CollectionShuffle")
                        {
                            CollectionShuffle pConsequence = new CollectionShuffle(pXml, pConsequenceNode, cCollections);
                            m_cConsequences.Add(pConsequence);
                        }
                        if (pConsequenceNode.Name == "CollectionSelect")
                        {
                            CollectionSelect pConsequence = new CollectionSelect(pXml, pConsequenceNode, cCollections);
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

        internal void WriteXML(UniLibXML pXml, XmlNode pMilestoneNode, string sFlow)
        {
            pXml.AddAttribute(pMilestoneNode, "flow", sFlow);
            pXml.AddAttribute(pMilestoneNode, "name", m_sName);
            pXml.AddAttribute(pMilestoneNode, "repeat", m_bRepeatable);
            pXml.AddAttribute(pMilestoneNode, "position", m_fPosition);

            XmlNode pConditionsNode = pXml.CreateNode(pMilestoneNode, "Conditions");
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

            XmlNode pConsequencesNode = pXml.CreateNode(pMilestoneNode, "Consequences");
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
    }
}
