using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persona.Flows;
using nsUniLibXML;
using System.Xml;

namespace Persona.Consequences
{
    public class FlowProgress : Consequence
    {
        public Flow m_pFlow;

        public float m_fProgress = 0;

        public bool m_bMajor = false;

        public FlowProgress()
        { }

        public FlowProgress(Flow pFlow, float fProgress)
        {
            m_pFlow = pFlow;
            m_fProgress = fProgress;
        }

        public FlowProgress(UniLibXML pXml, XmlNode pCollNode, List<Flow> cFlows)
        {
            pXml.GetFloatAttribute(pCollNode, "progress", ref m_fProgress);
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
            pXml.AddAttribute(pConsequenceNode, "progress", m_fProgress);
            pXml.AddAttribute(pConsequenceNode, "major", m_bMajor);
            pXml.AddAttribute(pConsequenceNode, "flow", m_pFlow.m_sName);
        }

        public override string ToString()
        {
            return m_pFlow.m_sName + ".Прибавить(" + m_fProgress.ToString() + (m_bMajor ? (" x " + m_pFlow.m_fMajorProgress.ToString()) : "") + ")";
        }

        public override Consequence Clone()
        {
            FlowProgress pNew = new FlowProgress(m_pFlow, m_fProgress);
            pNew.m_bMajor = m_bMajor;

            return pNew;
        }

        internal override void Apply(Module pModule)
        {
            pModule.m_sLog.AppendLine("\tDO " + this.ToString());
            if (m_bMajor)
                m_pFlow.ProgressMajor(m_fProgress, pModule);
            else
                m_pFlow.Progress(m_fProgress, pModule);
        }
    }
}
