using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinCostFlow
{
    public class CVertex<T> : IVertex
    {
        private CNode m_pEntrance;
        public CNode Entrance
        {
            get { return m_pEntrance; }
        }

        private CNode m_pExit;
        public CNode Exit
        {
            get { return m_pExit; }
        }

        private CLink m_pInnerLink;

        public long Capacity
        {
            get { return m_pInnerLink.Capacity; }
        }

        public long Flow
        {
            get { return m_pInnerLink.CurrentFlow; }
        }

        private T m_pObject;
        public T Object
        {
            get { return m_pObject; }
        }

        public CVertex(T t, int iCapacity)
        {
            m_pObject = t;

            m_pEntrance = new CNode(this);
            m_pExit = new CNode(this);

            m_pInnerLink = m_pEntrance.AddCapacityLink(m_pExit, iCapacity);
        }

        private Dictionary<CVertex<T>, CLink> m_cLinks = new Dictionary<CVertex<T>, CLink>();
        public Dictionary<CVertex<T>, CLink> Links
        {
            get { return m_cLinks; }
        }

        public CLink AddLink(CVertex<T> pVertex, int iCost)
        {
            CLink newLink = m_pExit.AddCostLink(pVertex.m_pEntrance, iCost);
            m_cLinks[pVertex] = newLink;
            return newLink;
        }

        public override string ToString()
        {
            return m_pObject.ToString();
        }
    }
}
