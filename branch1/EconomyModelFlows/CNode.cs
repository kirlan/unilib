using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinCostFlow
{
    public class CNode
    {
        static private int s_iCounter;

        public static void Reset()
        {
            s_iCounter = 0;
        }

        private int m_iId;
        public int Id
        {
            get { return m_iId; }
        }

        private object m_pObject;
        public object Object
        {
            get { return m_pObject; }
        }

        public CNode(object pObj)
        {
            m_pObject = pObj;
            m_iId = s_iCounter++;

            Init();
        }

        public void Init()
        {
            m_bMarked = false;
            m_iPushed = 0;
            m_pFrom = null;
            Cost = int.MaxValue;
        }

        public override string ToString()
        {
            return string.Format("{3}{0} ({1}x{2}$)", m_pObject.ToString(), m_iPushed, Cost, m_bMarked ? "*" : "");
        }

        Dictionary<CNode, CLink> m_cLinks = new Dictionary<CNode, CLink>();

        public Dictionary<CNode, CLink> Links
        {
            get { return m_cLinks; }
        }

        private CLink[] m_cFastAccess;

        public void BuildFastAccess(int iCount)
        {
            m_cFastAccess = new CLink[iCount];

            m_cFastAccess[m_iId] = new CLink(int.MaxValue, 0);

            Mates = m_cLinks.Keys.ToArray();
        }

        public CNode[] Mates;

        public void AddFastAccess(CNode pNode)
        {
            if (m_cFastAccess[pNode.m_iId] == null)
                m_cLinks.TryGetValue(pNode, out m_cFastAccess[pNode.m_iId]);
        }

        public long GetLinkCost(int id)
        {
            if (id == m_iId)
                return 0;

            if (m_cFastAccess[id] == null)
                return int.MaxValue;

            return m_cFastAccess[id].Cost;
        }

        long m_iPushed;
        public long Pushed
        {
            get { return m_iPushed; }
        }

        bool m_bMarked;
        public bool Marked
        {
            get { return m_bMarked; }
        }

        public long Cost;

        CNode m_pFrom;
        internal CNode From
        {
            get { return m_pFrom; }
        }

        internal void SetLink()
        {
            Cost = 0;
            m_pFrom = this;
        }

        internal bool SetLink(CNode pFrom)
        {
            long iNewCost = pFrom.Cost + pFrom.m_cFastAccess[m_iId].Cost;// GetLinkCost(this);

            if (Cost > iNewCost)
            {
                Cost = iNewCost;
                m_pFrom = pFrom;
                return true;
            }
            else
                return false;
        }

        internal bool Push(CNode pFrom)
        {
            if (m_bMarked)
                return false;

            long iCount = int.MaxValue;
            if (pFrom != this && pFrom != null)
                iCount = Math.Min(pFrom.Pushed, pFrom.Links[this].Capacity - pFrom.Links[this].CurrentFlow);

            if (iCount <= 0)
                return false;

            m_bMarked = true;
            m_pFrom = pFrom;
            m_iPushed = iCount;

            return true;
        }

        private CLink AddLink(CNode pTo, int iCapacity, int iCost)
        {
            if (m_cLinks.Keys.Contains(pTo) || pTo.Links.Keys.Contains(this))
                return null;

            m_cLinks[pTo] = new CLink(iCapacity, iCost);
            if (pTo != this)
                pTo.m_cLinks[this] = new CLink(0, -iCost);

            return m_cLinks[pTo];
        }

        public CLink AddCostLink(CNode pTo, int iCost)
        {
            return AddLink(pTo, int.MaxValue, iCost);
        }

        public CLink AddCapacityLink(CNode pTo, int iCapacity)
        {
            return AddLink(pTo, iCapacity, 0);
        }
    }
}
