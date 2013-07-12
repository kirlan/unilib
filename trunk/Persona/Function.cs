using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persona.Parameters;
using Persona.Conditions;
using nsUniLibXML;
using System.Xml;

namespace Persona
{
    public class Function
    {
        public class Rule
        {
            /// <summary>
            /// Список условий, при которых это правило срабатывает.
            /// Условия в списке связываются друг с другом логическим И.
            /// </summary>
            public List<Condition> m_cConditions = new List<Condition>();

            public string m_sNewValue;

            public Rule()
            {}

            public Rule(Rule pOrigin)
            {
                m_sNewValue = pOrigin.m_sNewValue;
            
                foreach (var pCondition in pOrigin.m_cConditions)
                    m_cConditions.Add(pCondition.Clone());            
            }

            public Rule(UniLibXML pXml, XmlNode pParamNode, List<Parameter> cParams)
            {
                pXml.GetStringAttribute(pParamNode, "value", ref m_sNewValue);

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
                }
            }

            internal void WriteXML(UniLibXML pXml, XmlNode pReactionNode)
            {
                pXml.AddAttribute(pReactionNode, "value", m_sNewValue);

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
            }
        }

        public Parameter m_pParam;

        public List<Rule> m_cRules = new List<Rule>();

        public Function(Parameter pParam)
        {
            m_pParam = pParam;
        }

        public Function(Function pOrigin)
        {
            m_pParam = pOrigin.m_pParam;

            foreach (var pRule in pOrigin.m_cRules)
                m_cRules.Add(new Rule(pRule));
        }

        public Function(UniLibXML pXml, XmlNode pParamNode, List<Parameter> cParams)
        {
            string sParam = "";
            pXml.GetStringAttribute(pParamNode, "param", ref sParam);
            foreach (Parameter pParam in cParams)
                if (pParam.m_sName == sParam)
                {
                    m_pParam = pParam;
                    break;
                } 
            
            foreach (XmlNode pSubNode in pParamNode.ChildNodes)
            {
                if (pSubNode.Name == "Rule")
                {
                    Rule pRule = new Rule(pXml, pSubNode, cParams);
                    m_cRules.Add(pRule);
                }
            }
        }


        internal void WriteXML(UniLibXML pXml, XmlNode pFunctionNode)
        {
            pXml.AddAttribute(pFunctionNode, "param", m_pParam.m_sName);

            foreach (Rule pRule in m_cRules)
            {
                XmlNode pRuleNode = pXml.CreateNode(pFunctionNode, "Rule");
                pRule.WriteXML(pXml, pRuleNode);
            }
        }

        public override string ToString()
        {
            return m_pParam.m_sName;
        }
    }
}
