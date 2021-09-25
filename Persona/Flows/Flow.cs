using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nsUniLibXML;
using System.Xml;
using Persona.Consequences;

namespace Persona.Flows
{
    public class Flow
    {
        public override string ToString()
        {
            return m_sName;
        }

        /// <summary>
        /// Короткое имя параметра, например - "Здоровье", "$$$", "Встретил лешего"
        /// </summary>
        public string m_sName;

        /// <summary>
        /// Комментарий к параметру - используется только при разработке модуля,
        /// может содержать, например, расшифровку числовых значений...
        /// </summary>
        public string m_sComment;

        public List<Milestone> m_cMilestones = new List<Milestone>();

        /// <summary>
        /// Comparer for comparing two keys, handling equality as beeing greater
        /// Use this Comparer e.g. with SortedLists or SortedDictionaries, that don't allow duplicate keys
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        public class DuplicateKeyComparer<TKey>
            :IComparer<TKey> where TKey : IComparable
        {
            #region IComparer<TKey> Members

            public int Compare(TKey x, TKey y)
            {
                int result = x.CompareTo(y);

                if (result == 0)
                    return 1;   // Handle equality as beeing greater
                else
                    return result;
            }

            #endregion
        }

        public SortedList<float, Milestone> m_cFutureMilestones = new SortedList<float, Milestone>(new DuplicateKeyComparer<float>());

        /// <summary>
        /// Значение в начале игры.
        /// </summary>
        public float m_fStartPosition = 0;

        /// <summary>
        /// Значение в течение игры.
        /// </summary>
        public float m_fCurrentPosition = 0;

        /// <summary>
        /// Большой шаг прогресса.
        /// </summary>
        public float m_fMajorProgress = 60;

        public void Init(Module pModule)
        {
            m_fCurrentPosition = m_fStartPosition;

            m_cFutureMilestones.Clear();
            foreach (Milestone pMile in m_cMilestones)
            {
                pModule.m_sLog.AppendLine("ADD MILESTONE " + pMile.m_sName + "[" + pMile.m_fPosition.ToString() + "]");
                m_cFutureMilestones.Add(pMile.m_fPosition, pMile);
            }
        }

        public Flow()
        { 
        }

        public Flow(UniLibXML pXml, XmlNode pParamNode)
        {
            pXml.GetStringAttribute(pParamNode, "name", ref m_sName);
            pXml.GetStringAttribute(pParamNode, "comment", ref m_sComment);
            pXml.GetFloatAttribute(pParamNode, "start", ref m_fStartPosition);
            pXml.GetFloatAttribute(pParamNode, "major", ref m_fMajorProgress);
        }

        internal virtual void WriteXML(UniLibXML pXml, XmlNode pParamNode)
        {
            pXml.AddAttribute(pParamNode, "name", m_sName);
            pXml.AddAttribute(pParamNode, "comment", m_sComment);
            pXml.AddAttribute(pParamNode, "start", m_fStartPosition);
            pXml.AddAttribute(pParamNode, "major", m_fMajorProgress);
        }

        public void ProgressMajor(float fProgress, Module pModule)
        {
            Progress(fProgress * m_fMajorProgress, pModule);
        }

        public void Progress(float fProgress, Module pModule)
        {
            float fNewPosition = m_fCurrentPosition + fProgress;

            if(m_cFutureMilestones.Count == 0)
            {
                m_fCurrentPosition = fNewPosition;
                return;
            }

            float fFirstPosition = m_cFutureMilestones.First().Key;
            int iCounter = 0;
            while (m_cFutureMilestones.Count > 0 && fFirstPosition <= fNewPosition)
            {
                Milestone pActiveStone = m_cFutureMilestones.First().Value;

                pModule.m_sLog.AppendLine("--DEBUG-- fFirstPosition =" + fFirstPosition.ToString() + ", fNewPosition =" + fNewPosition.ToString());

                bool bPossible = true;
                foreach (var pCondition in pActiveStone.m_cConditions)
                {
                    if (!pCondition.Check())
                    {
                        bPossible = false;
                        break;
                    }
                }

                if (bPossible)
                {
                    pModule.m_sLog.AppendLine("MILESTONE " + pActiveStone.m_sName + "[" + fFirstPosition.ToString() + "]");
                    foreach (Consequence pConsequence in pActiveStone.m_cConsequences)
                        pConsequence.Apply(pModule);
                }

                if (pActiveStone.m_bRepeatable)
                {
                    pModule.m_sLog.AppendLine("ADD MILESTONE " + pActiveStone.m_sName + "[" + (fFirstPosition + pActiveStone.m_fPosition).ToString() + "]");
                    m_cFutureMilestones.Add(fFirstPosition + pActiveStone.m_fPosition, pActiveStone);
                }

                m_cFutureMilestones.RemoveAt(0);
                if (m_cFutureMilestones.Count > 0)
                    fFirstPosition = m_cFutureMilestones.First().Key;

                iCounter++;
                pModule.m_sLog.AppendLine("--DEBUG-- iCounter =" + iCounter.ToString());
            }

            m_fCurrentPosition = fNewPosition;
        }
    }
}
