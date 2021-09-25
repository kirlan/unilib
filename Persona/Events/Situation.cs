using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persona.Conditions;
using nsUniLibXML;
using System.Xml;
using Persona.Parameters;
using Persona.Collections;

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

        public static string ComposeMessage(string sText, List<StringParameter> cStringParams, List<NumericParameter> cNumericParams, List<ObjectsCollection> cCollections)
        {
            string sOutput = sText;

            while (sOutput.IndexOf('{') != -1)
            {
                int start = sOutput.IndexOf('{');
                int finish = sOutput.IndexOf('}', start);

                if (finish == -1)
                    break;

                string sCode = sOutput.Substring(start + 1, finish - start - 1);

                bool bFound = false;
                foreach (StringParameter pParam in cStringParams)
                    if (pParam.FullName == sCode)
                    {
                        sOutput = sOutput.Replace("{" + sCode + "}", pParam.DisplayValue);
                        bFound = true;
                        break;
                    }
                if (!bFound)
                {
                    foreach (ObjectsCollection pColl in cCollections)
                    {
                        foreach (StringParameter pParam in pColl.m_cStringParameters)
                        {
                            if (pParam.FullName == sCode)
                            {
                                sOutput = sOutput.Replace("{" + sCode + "}", pParam.DisplayValue);
                                bFound = true;
                                break;
                            }
                        }
                        if (bFound)
                            break;
                    }
                }

                bFound = false;
                foreach (NumericParameter pParam in cNumericParams)
                    if (pParam.FullName == sCode)
                    {
                        sOutput = sOutput.Replace("{" + sCode + "}", pParam.ShortDisplayValue);
                        bFound = true;
                        break;
                    }
                if (!bFound)
                {
                    foreach (ObjectsCollection pColl in cCollections)
                    {
                        foreach (NumericParameter pParam in pColl.m_cNumericParameters)
                        {
                            if (pParam.FullName == sCode)
                            {
                                sOutput = sOutput.Replace("{" + sCode + "}", pParam.ShortDisplayValue);
                                bFound = true;
                                break;
                            }
                        }
                        if (bFound)
                            break;
                    }
                }
            }

            return sOutput;
        }

        public string GetDescription(List<StringParameter> cStringParams, List<NumericParameter> cNumericParams, List<ObjectsCollection> cCollections)
        {
            return ComposeMessage(m_sText, cStringParams, cNumericParams, cCollections);
        }

        /// <summary>
        /// Список условий при выполнении которых следует выводить это описание.
        /// Условия в списке связываются друг с другом логическим И.
        /// </summary>
        public List<Condition> m_cConditions = new List<Condition>();

        public Situation()
        { 
        }

        public Situation(UniLibXML pXml, XmlNode pParamNode, List<Parameter> cParams, List<ObjectsCollection> cCollections)
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
                if (pConditionNode.Name == "Selected")
                {
                    ConditionObjectSelected pCondition = new ConditionObjectSelected(pXml, pConditionNode, cCollections);
                    m_cConditions.Add(pCondition);
                }
            }
        }

        public Situation(Situation pOrigin)
        {
            m_sText = pOrigin.m_sText;

            foreach (var pCondition in pOrigin.m_cConditions)
                m_cConditions.Add(pCondition.Clone());
        }

        internal void WriteXML(UniLibXML pXml, XmlNode pSituationNode)
        {
            pXml.AddAttribute(pSituationNode, "description", m_sText);

            foreach (Condition pCondition in m_cConditions)
            {
                if (pCondition is ConditionRange)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pSituationNode, "Range");
                    pCondition.WriteXML(pXml, pConditionNode);
                }
                if (pCondition is ConditionComparsion)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pSituationNode, "Comparsion");
                    pCondition.WriteXML(pXml, pConditionNode);
                }
                if (pCondition is ConditionStatus)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pSituationNode, "Status");
                    pCondition.WriteXML(pXml, pConditionNode);
                } 
                if (pCondition is ConditionObjectSelected)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pSituationNode, "Selected");
                    pCondition.WriteXML(pXml, pConditionNode);
                }
            }
        }
    }
}
