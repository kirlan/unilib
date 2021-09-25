using System;
using System.Collections.Generic;
using System.Text;
using nsUniLibXML;
using System.Linq;
using System.Xml;
using Random;
using System.Threading.Tasks;

namespace RandomStory
{
    public class Strings
    {
        private List<string> m_cStrings = new List<string>();

        public Strings()
        {
        }

        public Strings(IEnumerable<string> cBase)
        {
            m_cStrings.AddRange(cBase);
        }

        public Strings(Strings cBase)
        {
            m_cStrings.AddRange(cBase.m_cStrings);
        }

        public Strings(Strings cBase1, Strings cBase2)
            : this(cBase1)
        {
            Merge(cBase2);
        }

        public override string ToString()
        {
            return ToString(", ");
        }

        public string ToString(string sDivider)
        {
            return string.Join(sDivider, m_cStrings.ToArray());
        }

        public void Add(string sStr)
        {
            if (!m_cStrings.Contains(sStr))
                m_cStrings.Add(sStr);
        }

        public int Count
        {
            get { return m_cStrings.Count; }
        }

        public string this[int key]
        {
            get
            {
                return m_cStrings[key];
            }
            set
            {
                m_cStrings[key] = value;
            }
        }


        public Strings(UniLibXML pXml, XmlNode pSubNode)
        {
            foreach (XmlNode pItemNode in pSubNode.ChildNodes)
            {
                if (pItemNode.Name == "Item")
                {
                    string sName = "";
                    pXml.GetStringAttribute(pItemNode, "name", ref sName);

                    sName = sName.Trim();

                    if (string.IsNullOrWhiteSpace(sName))
                        continue;

                    if (m_cStrings.Contains(sName))
                        continue;

                    m_cStrings.Add(sName);
                }
            }
        }

        public void WriteXML(UniLibXML pXml, XmlNode pNode, string sName)
        {
            XmlNode pStringsNode = pXml.CreateNode(pNode, sName);

            foreach (string sString in m_cStrings)
            {
                XmlNode pRaceNode = pXml.CreateNode(pStringsNode, "Item");
                pXml.AddAttribute(pRaceNode, "name", sString);
            }
        }

        /// <summary>
        /// Равенство двух массивов строк - без учёта порядка следования
        /// </summary>
        /// <param name="pOther"></param>
        /// <returns></returns>
        public bool Equals(Strings pOther)
        {
            if (m_cStrings.Count != pOther.m_cStrings.Count)
                return false;

            foreach (string sString in m_cStrings)
            {
                if (!pOther.m_cStrings.Contains(sString))
                    return false;
            }

            return true;
        }

        public static char[] GetFlags(ref string sStr)
        {
            StringBuilder sFlags = new StringBuilder();

            bool bFound;
            do
            {
                bFound = false;
                int iPos1 = sStr.IndexOf('[');
                if (iPos1 != -1)
                {
                    int iPos2 = sStr.IndexOf(']', iPos1);
                    if (iPos2 != -1)
                    {
                        sFlags.Append(sStr.Substring(iPos1 + 1, iPos2 - iPos1 - 1));
                        sStr = sStr.Remove(iPos1, iPos2 - iPos1 + 1);
                        bFound = true;
                    }
                }
            }
            while (bFound);

            return sFlags.ToString().ToCharArray();
        }

        public string GetRandom(ref char[] aFlags)
        {
            return GetRandom(new Strings(), string.Empty, ref aFlags);
        }

        public string GetRandom(string sDefault, ref char[] aFlags)
        {
            return GetRandom(new Strings(), sDefault, ref aFlags);
        }

        public string GetRandom(Strings cExceptions, ref char[] aFlags)
        {
            return GetRandom(cExceptions, string.Empty, ref aFlags);
        }

        /// <summary>
        /// Парсит заданную строку на токены, в каждом токене выделяет флаги, если они есть, 
        /// затем возвращает список токенов с флагами, совпадающими с заданной комбинацией
        /// </summary>
        /// <param name="sItem">строка для парса</param>
        /// <param name="aFlags">требуемая комбинация флагов</param>
        /// <returns></returns>
        private SortedList<string, char[]> CheckFlags(string sItem, char[] aFlags)
        {
            string[] aItems = sItem.Split(new char[] { '\\', '/', '|' });
        
            if (aFlags != null)
            {
                SortedList<string, char[]> cTrueItems = new SortedList<string, char[]>();
                SortedList<string, char[]> cPossibleItems = new SortedList<string, char[]>();
                for (int i = 0; i < aItems.Length; i++)
                {
                    string sItm = aItems[i];
                    char[] aItmFlags = GetFlags(ref sItm);
                    if (aItmFlags.Length == 0)
                        cPossibleItems[sItm] = aItmFlags;

                    if (aFlags.Length == aItmFlags.Length)
                    {
                        if (aFlags.Length == 0)
                            cTrueItems[sItm] = aItmFlags;
                        else
                        {
                            bool bOK = true;
                            foreach (char chFlag1 in aFlags)
                                if (!aItmFlags.Contains(chFlag1))
                                    bOK = false;

                            if (bOK)
                                cTrueItems[sItm] = aItmFlags;
                        }
                    }
                }

                if (cTrueItems.Count > 0)
                    return cTrueItems;
                else
                    return cPossibleItems;
            }

            SortedList<string, char[]> cItems = new SortedList<string, char[]>();
            for (int i = 0; i < aItems.Length; i++)
            {
                string sItm = aItems[i];
                char[] aItmFlags = GetFlags(ref sItm);
                cItems[sItm] = aItmFlags;
            }

            return cItems;
        }

        public string GetRandom(Strings cExceptions, string sDefault, ref char[] aFlags)
        {
            if (m_cStrings.Count > cExceptions.m_cStrings.Count)
            {
                int iCounter = 1000;
                do
                {
                    iCounter--;
                    string sItem = m_cStrings[Rnd.Get(m_cStrings.Count)];
                    SortedList<string, char[]> aItems = CheckFlags(sItem, aFlags);

                    if (aItems.Count > 0)
                    {
                        bool bException = false;
                        foreach (string sItm in aItems.Keys)
                            if (cExceptions.m_cStrings.Contains(sItm))
                                bException = true;

                        if (bException)
                            continue;

                        sItem = aItems.Keys[Rnd.Get(aItems.Count)];

                        if (aFlags == null)
                            aFlags = aItems[sItem];
                        return sItem;
                    }
                }
                while (iCounter > 0);
            }

            return sDefault;
        }

        public string GetRelative(string sRelative, ref char[] aFlags)
        {
            foreach (string sItems in m_cStrings)
            {
                SortedList<string, char[]> aItems = CheckFlags(sItems, aFlags);

                if (!aItems.ContainsKey(sRelative))
                    continue;

                if (aItems.Count > 1)
                {
                    int iCounter = 1000;
                    do
                    {
                        iCounter--;
                        string sItem = aItems.Keys[Rnd.Get(aItems.Count)];

                        if (sItem.Equals(sRelative))
                            continue;

                        if (aFlags == null)
                            aFlags = aItems[sItem];
                        return sItem;
                    }
                    while (iCounter > 0);
                }
            }

            return sRelative;
        }

        private static readonly char[] s_aSeparators = new char[] { '\\', '/', '|' };

        public string[] Merge(Strings cBase2)
        {
            List<string> cResult = new List<string>();

            foreach (string sString2 in cBase2.m_cStrings)
            {
                if (m_cStrings.Contains(sString2))
                    continue;

                string[] aItems2 = sString2.Split(s_aSeparators);
                bool bMerged = false;
                for (int i = 0; i < m_cStrings.Count; i++)
                {
                    bool bNeedMerge = false;
                    foreach (string sItem2 in aItems2)
                    {
                        if (m_cStrings[i].Contains(sItem2))
                        {
                            bNeedMerge = true;
                            break;
                        }
                    }

                    if (bNeedMerge)
                    {
                        foreach(string sItem2 in aItems2)
                        {
                            if (!m_cStrings[i].Contains(sItem2))
                                m_cStrings[i] += string.Format("\\{0}", sItem2);
                        }
                        bMerged = true;
                    }
                }

                if (!bMerged)
                    m_cStrings.Add(sString2);

                cResult.Add(sString2);
            }

            return cResult.ToArray();
        }

        public void KeepCommon(Strings cStrings2)
        {
            List<string> cCommon = new List<string>();

            foreach (string sStr in m_cStrings)
                if (cStrings2.m_cStrings.Contains(sStr))
                    cCommon.Add(sStr);

            m_cStrings.Clear();
            m_cStrings.AddRange(cCommon);
        }

        public string[] RemoveCommon(Strings cCommon)
        {
            List<string> cRemoved = new List<string>();
            foreach (string sStr in cCommon.m_cStrings)
            {
                if (m_cStrings.Contains(sStr))
                {
                    m_cStrings.Remove(sStr);
                    cRemoved.Add(sStr);
                }
            }

            return cRemoved.ToArray();
        }

        internal string[] ToArray()
        {
            return m_cStrings.ToArray();
        }
    }
}
