using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persona.Conditions;
using Persona.Consequences;
using Persona.Parameters;
using System.Xml;
using nsUniLibXML;
using Persona.Collections;
using Persona.Flows;

namespace Persona
{
    /// <summary>
    /// Триггер - событие, происходящее не в ответ на какое-то действие игрока, а как реакция игрового мира на изменение
    /// каких-то параметров. Например - если время перевалило через 24 часа, то сбросить счётчик часов и увеличить счётчик дней.
    /// </summary>
    public class Trigger
    {
        /// <summary>
        /// <para>Повторяемость события. </para>
        /// <para>Если true, то событие может происходить неограниченное количество раз, 
        /// иначе - только 1 раз за игровую сессию. </para>
        /// </summary>
        public bool m_bRepeatable = true;

        /// <summary>
        /// Идентификатор события. Используется только при редактировании модуля, для навигации по списку событий.
        /// </summary>
        public string m_sID = "Trigger " + Guid.NewGuid().ToString();

        /// <summary>
        /// Список условий, при которых эта реакция доступна.
        /// Условия в списке связываются друг с другом логическим И.
        /// </summary>
        public List<Condition> m_cConditions = new List<Condition>();

        /// <summary>
        /// Список последствий выбора этой реакции, применяется к текущему состоянию системы после выбора
        /// реакции пользователем.
        /// </summary>
        public List<Consequence> m_cConsequences = new List<Consequence>();

        public Trigger()
        {
        }

        public Trigger(Trigger pOrigin)
        {
            m_sID = pOrigin.m_sID;
            m_bRepeatable = pOrigin.m_bRepeatable;

            foreach (var pCondition in pOrigin.m_cConditions)
                m_cConditions.Add(pCondition.Clone());

            foreach (var pConsequence in pOrigin.m_cConsequences)
                m_cConsequences.Add(pConsequence.Clone());
        }

        public Trigger(UniLibXML pXml, XmlNode pParamNode, List<Parameter> cParams, List<ObjectsCollection> cCollections, List<Flow> cFlows)
        {
            pXml.GetStringAttribute(pParamNode, "id", ref m_sID);
            pXml.GetBoolAttribute(pParamNode, "repeat", ref m_bRepeatable);

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


        internal void WriteXML(UniLibXML pXml, XmlNode pReactionNode)
        {
            pXml.AddAttribute(pReactionNode, "id", m_sID);
            pXml.AddAttribute(pReactionNode, "repeat", m_bRepeatable);

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
    }
}
