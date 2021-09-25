using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persona.Parameters;
using Persona.Conditions;
using nsUniLibXML;
using System.Xml;
using Persona.Consequences;
using Persona.Collections;

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

            public Consequence m_pConsequence = null;

            public bool Check()
            {
                foreach (Condition pCond in m_cConditions)
                {
                    if (!pCond.Check())
                        return false;
                }

                return true;
            }

            public void Apply(Parameter pParam, List<StringParameter> cStringParams, List<NumericParameter> cNumericParams, List<ObjectsCollection> cCollections)
            {
                if (m_pConsequence == null)
                    return;

                if (m_pConsequence is ParameterSet)
                {
                    if(pParam is StringParameter)
                        pParam.SetValue(Situation.ComposeMessage((m_pConsequence as ParameterSet).m_sNewValue, cStringParams, cNumericParams, cCollections));
                    else
                        pParam.SetValue((m_pConsequence as ParameterSet).m_sNewValue);
                    return;
                }
                if (m_pConsequence is ParameterChangeVariable)
                {
                    ParameterChangeVariable pFormula = m_pConsequence as ParameterChangeVariable;

                    NumericParameter pParam1 = pFormula.m_pParam1 as NumericParameter;
                    NumericParameter pParam2 = pFormula.m_pParam2 as NumericParameter;

                    if (pParam1 == null || pParam2 == null)
                        return;

                    float fValue = NumericParameter.GetOperationResult(pParam1.m_fValue, pParam2.m_fValue, pFormula.m_eOperation);
                    (pParam as NumericParameter).SetValue(fValue);
                    return;
                }
            }

            public Rule(Parameter pParam)
            {
                m_pConsequence = new ParameterSet(pParam, "");
            }

            public Rule(Consequence pCons)
            {
                m_pConsequence = pCons.Clone();
            }

            public Rule(Rule pOrigin)
            {
                m_pConsequence = pOrigin.m_pConsequence.Clone();
            
                foreach (var pCondition in pOrigin.m_cConditions)
                    m_cConditions.Add(pCondition.Clone());            
            }

            public Rule(UniLibXML pXml, XmlNode pParamNode, List<Parameter> cParams, List<ObjectsCollection> cCollections, Parameter pParam)
            {
                foreach (XmlNode pSubNode in pParamNode.ChildNodes)
                {
                    if (pSubNode.Name == "Consequences")
                    {
                        foreach (XmlNode pConsequenceNode in pSubNode.ChildNodes)
                        {
                            //if (pConsequenceNode.Name == "ParameterChange")
                            //{
                            //    m_pConsequence = new ParameterChange(pXml, pConsequenceNode, cParams);
                            //}
                            if (pConsequenceNode.Name == "ParameterChangeVariable")
                            {
                                m_pConsequence = new ParameterChangeVariable(pXml, pConsequenceNode, cParams);
                            }
                            if (pConsequenceNode.Name == "ParameterSet")
                            {
                                m_pConsequence = new ParameterSet(pXml, pConsequenceNode, cParams);
                            }
                            //if (pConsequenceNode.Name == "CollectionShuffle")
                            //{
                            //    m_pConsequence = new CollectionShuffle(pXml, pConsequenceNode, cCollections);
                            //}
                            //if (pConsequenceNode.Name == "SystemCommand")
                            //{
                            //    m_pConsequence = new SystemCommand(pXml, pConsequenceNode, cParams);
                            //}
                        }
                    }
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
                }
                if(m_pConsequence == null)
                {
                    string sNewValue = "";
                    pXml.GetStringAttribute(pParamNode, "value", ref sNewValue);
                    m_pConsequence = new ParameterSet(pParam, sNewValue);
                }
            }

            internal void WriteXML(UniLibXML pXml, XmlNode pReactionNode)
            {
                XmlNode pConsequencesNode = pXml.CreateNode(pReactionNode, "Consequences");
                //if (m_pConsequence is ParameterChange)
                //{
                //    XmlNode pConditionNode = pXml.CreateNode(pConsequencesNode, "ParameterChange");
                //    m_pConsequence.WriteXML(pXml, pConditionNode);
                //}
                if (m_pConsequence is ParameterChangeVariable)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pConsequencesNode, "ParameterChangeVariable");
                    m_pConsequence.WriteXML(pXml, pConditionNode);
                }
                if (m_pConsequence is ParameterSet)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pConsequencesNode, "ParameterSet");
                    m_pConsequence.WriteXML(pXml, pConditionNode);
                }
                //if (m_pConsequence is CollectionShuffle)
                //{
                //    XmlNode pConditionNode = pXml.CreateNode(pConsequencesNode, "CollectionShuffle");
                //    m_pConsequence.WriteXML(pXml, pConditionNode);
                //}
                //if (m_pConsequence is SystemCommand)
                //{
                //    XmlNode pConditionNode = pXml.CreateNode(pConsequencesNode, "SystemCommand");
                //    m_pConsequence.WriteXML(pXml, pConditionNode);
                //}

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
            }
        }

        public Parameter m_pParam;

        public List<Rule> m_cRules = new List<Rule>();

        public void Evaluate(List<StringParameter> cStringParams, List<NumericParameter> cNumericParams, List<ObjectsCollection> cCollections)
        {
            foreach (Rule pRule in m_cRules)
            {
                if (pRule.Check())
                {
                    pRule.Apply(m_pParam, cStringParams, cNumericParams, cCollections);
                    return;
                }
            }
        }

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

        public Function(UniLibXML pXml, XmlNode pParamNode, List<Parameter> cParams, List<ObjectsCollection> cCollections)
        {
            string sParam = "";
            pXml.GetStringAttribute(pParamNode, "param", ref sParam);
            foreach (Parameter pParam in cParams)
                if (pParam.FullName == sParam)
                {
                    m_pParam = pParam;
                    break;
                }

            m_pParam.m_pFunction = this;
            
            foreach (XmlNode pSubNode in pParamNode.ChildNodes)
            {
                if (pSubNode.Name == "Rule")
                {
                    Rule pRule = new Rule(pXml, pSubNode, cParams, cCollections, m_pParam);
                    m_cRules.Add(pRule);
                }
            }
        }


        internal void WriteXML(UniLibXML pXml, XmlNode pFunctionNode)
        {
            pXml.AddAttribute(pFunctionNode, "param", m_pParam.FullName);

            foreach (Rule pRule in m_cRules)
            {
                XmlNode pRuleNode = pXml.CreateNode(pFunctionNode, "Rule");
                pRule.WriteXML(pXml, pRuleNode);
            }
        }

        public override string ToString()
        {
            return m_pParam.FullName;
        }
    }
}
