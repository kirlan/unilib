using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persona.Conditions;
using nsUniLibXML;
using System.Xml;
using Persona.Parameters;

namespace Persona
{
    /// <summary>
    /// Привязанный к условию текст описания события - используется для того,
    /// чтобы давать различные описания одного и того же события в зависимости от условия, 
    /// сохраняя при этом все возможные реакции и последствия.
    /// Например - для различного пола персонажа или в зависимости от каких-то предыдущих действий...
    /// </summary>
    public class Situation
    {
        /// <summary>
        /// Описание события
        /// </summary>
        public string m_sText = "Описание события";

        /// <summary>
        /// Список условий при выполнении которых следует выводить это описание.
        /// Условия в списке связываются друг с другом логическим И.
        /// </summary>
        public List<Condition> m_cConditions = new List<Condition>();

        public Situation()
        { 
        }

        public Situation(UniLibXML pXml, XmlNode pParamNode, List<Parameter> cParams)
        {
            pXml.GetStringAttribute(pParamNode, "description", ref m_sText);

            foreach (XmlNode pConditionNode in pParamNode.ChildNodes)
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

        internal void SaveXML(UniLibXML pXml, XmlNode pSituationNode)
        {
            pXml.AddAttribute(pSituationNode, "description", m_sText);

            foreach (Condition pCondition in m_cConditions)
            {
                if (pCondition is ConditionRange)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pSituationNode, "Range");
                    pCondition.SaveXML(pXml, pConditionNode);
                }
                if (pCondition is ConditionComparsion)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pSituationNode, "Comparsion");
                    pCondition.SaveXML(pXml, pConditionNode);
                }
                if (pCondition is ConditionStatus)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pSituationNode, "Status");
                    pCondition.SaveXML(pXml, pConditionNode);
                }
            }
        }
    }
}
