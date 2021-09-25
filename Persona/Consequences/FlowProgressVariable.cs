using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persona.Flows;
using Persona.Parameters;
using nsUniLibXML;
using System.Xml;

namespace Persona.Consequences
{
    public class FlowProgressVariable : Consequence
    {
        public Flow m_pFlow;

        public Parameter m_pParameter = null;

        public bool m_bMajor = false;

        public FlowProgressVariable()
        { }

        public FlowProgressVariable(Flow pFlow, Parameter pParameter)
        {
            m_pFlow = pFlow;
            m_pParameter = pParameter;
        }

        public FlowProgressVariable(UniLibXML pXml, XmlNode pCollNode, List<Parameter> cParams, List<Flow> cFlows)
        {
            string sParam = "";
            pXml.GetStringAttribute(pCollNode, "param", ref sParam);
            foreach (Parameter pParam in cParams)
                if (pParam.FullName == sParam)
                {
                    m_pParameter = pParam;
                    break;
                }

            pXml.GetBoolAttribute(pCollNode, "major", ref m_bMajor);

            string sFlow = "";
            pXml.GetStringAttribute(pCollNode, "flow", ref sFlow);
            foreach (Flow pFlow in cFlows)
                if (pFlow.m_sName == sFlow)
                {
                    m_pFlow = pFlow;
                    break;
                }
        }

        internal override void WriteXML(UniLibXML pXml, XmlNode pConsequenceNode)
        {
            pXml.AddAttribute(pConsequenceNode, "param", m_pParameter.FullName);
            pXml.AddAttribute(pConsequenceNode, "major", m_bMajor);
            pXml.AddAttribute(pConsequenceNode, "flow", m_pFlow.m_sName);
        }

        public override string ToString()
        {
            return m_pFlow.m_sName + ".Прибавить([" + (m_pParameter != null ? m_pParameter.FullName : "НЕВЕРНЫЙ ПАРАМЕТР") + (m_bMajor ? ("] x " + m_pFlow.m_fMajorProgress.ToString()) : "]") + ")";
        }

        public override Consequence Clone()
        {
            FlowProgressVariable pNew = new FlowProgressVariable(m_pFlow, m_pParameter);
            pNew.m_bMajor = m_bMajor;

            return pNew;
        }

        internal override void Apply(Module pModule)
        {
            NumericParameter pParam = m_pParameter as NumericParameter;

            if (pParam == null)
                return;

            pModule.m_sLog.AppendLine("\tDO " + this.ToString());

            if (m_bMajor)
                m_pFlow.ProgressMajor(pParam.m_fValue, pModule);
            else
                m_pFlow.Progress(pParam.m_fValue, pModule);
        }
    }
}
