using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persona.Consequences;
using nsUniLibXML;
using System.Xml;
using Persona.Conditions;
using Persona.Parameters;

namespace Persona
{
    /// <summary>
    /// Игровое событие
    /// </summary>
    public class Event
    {
        /// <summary>
        /// Идентификатор события. Используется только при редактировании модуля, для навигации по списку событий.
        /// </summary>
        public string m_sID = "Event " + Guid.NewGuid().ToString();

        /// <summary>
        /// Область жизни, к которой относится это событие.
        /// </summary>
        public Action m_pAction;

        /// <summary>
        /// <para>Множитель вероятности события.</para>
        /// <para>При значении 1 (по умолчанию) событие имеет шансы выпасть равные все другим событиям 
        /// этой же области жизни, у которых есть хоть одно описание с удовлетворяющимися условиями.</para>
        /// <para>При значении меньше или равно 0 - событие не случится никогда, можно использовать 
        /// при тестировании модуля для временного отключения определённых событий.</para>
        /// <para>При значении больше 1 шансы увеличиваются в соответствующее число раз. Т.е. событие как бы дублируется 
        /// в списке выбора соответствующее число раз.</para>
        /// </summary>
        public int m_iProbability = 1;

        /// <summary>
        /// <para>Повторяемость события. </para>
        /// <para>Если true, то событие может происходить неограниченное количество раз, 
        /// иначе - только 1 раз за игровую сессию. Если false и <see cref="m_iProbability"/> > 1, то событие может 
        /// произойти <see cref="m_iProbability"/> раз, при чём с каждым разом его вероятность будет снижаться.</para>
        /// </summary>
        public bool m_bRepeatable = false;

        /// <summary>
        /// <para>Приоритет события. </para>
        /// <para>Событие с более низким приоритетом может произойти только если нет 
        /// ни одного возможного события с более высоким приоритетом, независимо от категории события.</para>
        /// </summary>
        public int m_iPriority = 1;

        /// <summary>
        /// Подробное описание события и основной список условий возможности события. 
        /// Описание используется в случае, если в <see cref="m_cAlternateDescriptions"/> нет ничего более подходящего.
        /// </summary>
        public Situation m_pDescription = new Situation();

        /// <summary>
        /// Список альтернативных описаний события с условиями. 
        /// Условия из списка дополняют, а не заменяют основной список условий из <see cref="m_pDescription"/>
        /// </summary>
        public List<Situation> m_cAlternateDescriptions = new List<Situation>();

        /// <summary>
        /// Список возможных реакций пользователя на событие. Не может быть пустым.
        /// Должен содержать хотя бы одну реакцию, которая доступна всегда.
        /// </summary>
        public List<Reaction> m_cReactions = new List<Reaction>();

        /// <summary>
        /// Список последствий события, которые применяются к текущему состоянию системы после выбора пользователем реакции - 
        /// независимо от того, какая именно реакция была выбрана. Может быть пустым.
        /// </summary>
        public List<Consequence> m_cConsequences = new List<Consequence>();

        public Event(Action pAction)
        {
            m_pAction = pAction;
        }

        public Event(Event pOrigin)
        {
            m_sID = pOrigin.m_sID + " (копия)";
            m_pAction = pOrigin.m_pAction;
            m_pDescription = new Situation(pOrigin.m_pDescription);
            m_iProbability = pOrigin.m_iProbability;
            m_iPriority = pOrigin.m_iPriority;
            m_bRepeatable = pOrigin.m_bRepeatable;

            foreach (var pCondition in pOrigin.m_cAlternateDescriptions)
                m_cAlternateDescriptions.Add(new Situation(pCondition));

            foreach (var pConsequence in pOrigin.m_cConsequences)
                m_cConsequences.Add(pConsequence.Clone());

            foreach (var pReaction in pOrigin.m_cReactions)
                m_cReactions.Add(new Reaction(pReaction));
        }

        public Event(UniLibXML pXml, XmlNode pParamNode, List<Action> cActions, List<Parameter> cParams)
        {
            pXml.GetStringAttribute(pParamNode, "id", ref m_sID);

            string sAction = "";
            if(pXml.GetStringAttribute(pParamNode, "domain", ref sAction) == "")
                pXml.GetStringAttribute(pParamNode, "action", ref sAction);
            foreach (Action pAction in cActions)
                if (pAction.m_sName == sAction)
                {
                    m_pAction = pAction;
                    break;
                }

            pXml.GetStringAttribute(pParamNode, "description", ref m_pDescription.m_sText);
            pXml.GetBoolAttribute(pParamNode, "repeat", ref m_bRepeatable);
            pXml.GetIntAttribute(pParamNode, "priority", ref m_iPriority);
            pXml.GetIntAttribute(pParamNode, "probability", ref m_iProbability);

            foreach (XmlNode pSubNode in pParamNode.ChildNodes)
            {
                if (pSubNode.Name == "Conditions")
                {
                    foreach (XmlNode pConditionNode in pSubNode.ChildNodes)
                    {
                        if (pConditionNode.Name == "Range")
                        {
                            ConditionRange pCondition = new ConditionRange(pXml, pConditionNode, cParams);
                            m_pDescription.m_cConditions.Add(pCondition);
                        }
                        if (pConditionNode.Name == "Comparsion")
                        {
                            ConditionComparsion pCondition = new ConditionComparsion(pXml, pConditionNode, cParams);
                            m_pDescription.m_cConditions.Add(pCondition);
                        }
                        if (pConditionNode.Name == "Status")
                        {
                            ConditionStatus pCondition = new ConditionStatus(pXml, pConditionNode, cParams);
                            m_pDescription.m_cConditions.Add(pCondition);
                        }
                    }
                }
                if (pSubNode.Name == "Alternatives")
                {
                    foreach (XmlNode pSituationNode in pSubNode.ChildNodes)
                    {
                        if (pSituationNode.Name == "Situation")
                        {
                            Situation pSituation = new Situation(pXml, pSituationNode, cParams);
                            m_cAlternateDescriptions.Add(pSituation);
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
                if (pSubNode.Name == "Reactions")
                {
                    foreach (XmlNode pReactionNode in pSubNode.ChildNodes)
                    {
                        if (pReactionNode.Name == "Reaction")
                        {
                            Reaction pReaction = new Reaction(pXml, pReactionNode, cParams);
                            m_cReactions.Add(pReaction);
                        }
                    }
                }
            }        }

        internal void WriteXML(UniLibXML pXml, XmlNode pEventNode)
        {
            pXml.AddAttribute(pEventNode, "id", m_sID);
            pXml.AddAttribute(pEventNode, "action", m_pAction.m_sName);
            pXml.AddAttribute(pEventNode, "description", m_pDescription.m_sText);
            pXml.AddAttribute(pEventNode, "repeat", m_bRepeatable);
            pXml.AddAttribute(pEventNode, "priority", m_iPriority);
            pXml.AddAttribute(pEventNode, "probability", m_iProbability);

            XmlNode pConditionsNode = pXml.CreateNode(pEventNode, "Conditions");
            foreach (Condition pCondition in m_pDescription.m_cConditions)
            {
                if (pCondition is ConditionRange)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pConditionsNode, "Range");
                    pCondition.SaveXML(pXml, pConditionNode);
                }
                if (pCondition is ConditionComparsion)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pConditionsNode, "Comparsion");
                    pCondition.SaveXML(pXml, pConditionNode);
                }
                if (pCondition is ConditionStatus)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pConditionsNode, "Status");
                    pCondition.SaveXML(pXml, pConditionNode);
                }
            }

            XmlNode pAlternativesNode = pXml.CreateNode(pEventNode, "Alternatives");
            foreach (Situation pSituation in m_cAlternateDescriptions)
            {
                XmlNode pSituationNode = pXml.CreateNode(pAlternativesNode, "Situation");
                pSituation.SaveXML(pXml, pSituationNode);
            }

            XmlNode pConsequencesNode = pXml.CreateNode(pEventNode, "Consequences");
            foreach (Consequence pConsequence in m_cConsequences)
            {
                if (pConsequence is ParameterChange)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pConsequencesNode, "ParameterChange");
                    pConsequence.SaveXML(pXml, pConditionNode);
                }
                if (pConsequence is ParameterSet)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pConsequencesNode, "ParameterSet");
                    pConsequence.SaveXML(pXml, pConditionNode);
                }
                if (pConsequence is SystemCommand)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pConsequencesNode, "SystemCommand");
                    pConsequence.SaveXML(pXml, pConditionNode);
                }
            }

            XmlNode pReactionsNode = pXml.CreateNode(pEventNode, "Reactions");
            foreach (Reaction pReaction in m_cReactions)
            {
                XmlNode pReactionNode = pXml.CreateNode(pReactionsNode, "Reaction");
                pReaction.SaveXML(pXml, pReactionNode);
            }
        }
    }
}
