using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nsUniLibXML;
using System.Xml;
using Random;

namespace RandomStory
{
    public static class StringsHelper
    {
        public static void ReadXML(UniLibXML pXml, XmlNode pSubNode, ref List<string> cStrings)
        {
            cStrings.Clear();

            foreach (XmlNode pItemNode in pSubNode.ChildNodes)
            {
                if (pItemNode.Name == "Item")
                {
                    string sName = "";
                    pXml.GetStringAttribute(pItemNode, "name", ref sName);

                    if (!string.IsNullOrWhiteSpace(sName))
                        cStrings.Add(sName);
                }
            }
        }

        public static void WriteXML(UniLibXML pXml, XmlNode pNode, string sName, List<string> cStrings)
        {
            XmlNode pStringsNode = pXml.CreateNode(pNode, sName);

            foreach (string sString in cStrings)
            {
                XmlNode pRaceNode = pXml.CreateNode(pStringsNode, "Item");
                pXml.AddAttribute(pRaceNode, "name", sString);
            }
        }

        public static string GetRandom(List<string> cStrings)
        {
            return GetRandom(cStrings, new List<string>(), string.Empty);
        }

        public static string GetRandom(List<string> cStrings, string sDefault)
        {
            return GetRandom(cStrings, new List<string>(), sDefault);
        }

        public static string GetRandom(List<string> cStrings, List<string> cExceptions)
        {
            return GetRandom(cStrings, cExceptions, string.Empty);
        }

        public static string GetRandom(List<string> cStrings, List<string> cExceptions, string sDefault)
        {
            if (cStrings.Count > cExceptions.Count)
            {
                do
                {
                    string sItem = cStrings[Rnd.Get(cStrings.Count)];
                    string[] aItems = sItem.Split(new char[] { '\\', '/', '|' });
                    
                    bool bException = false;
                    foreach (string sItm in aItems)
                        if (cExceptions.Contains(sItm))
                            bException = true;

                    if (bException)
                        continue;

                    sItem = aItems[Rnd.Get(aItems.Length)];

                    if (!cExceptions.Contains(sItem))
                        return sItem;
                }
                while (true);
            }

            return sDefault;
        }

        public static string GetRelative(List<string> cStrings, string sRelative)
        {
            foreach (string sItem in cStrings)
            {
                string[] aItems = sItem.Split(new char[] { '\\', '/', '|' });
                if (aItems.Contains(sRelative))
                {
                    return aItems[Rnd.Get(aItems.Length)];
                }
            }

            return sRelative;
        }

        public static void Merge(ref List<string> cResult, List<string> cBase2)
        {
            foreach (string sStr in cBase2)
            {
                if (cResult.Contains(sStr))
                    continue;

                string[] aItems2 = sStr.Split(new char[] { '\\', '/', '|' });
                bool bMerged = false;
                for (int i = 0; i < cResult.Count; i++)
                {
                    string[] aItems1 = cResult[i].Split(new char[] { '\\', '/', '|' });

                    bool bNeedMerge = false;
                    foreach (string sItem2 in aItems2)
                    {
                        if (aItems1.Contains(sItem2))
                        {
                            bNeedMerge = true;
                        }
                    }

                    if (bNeedMerge)
                    {
                        foreach (string sItem2 in aItems2)
                        {
                            if (!aItems1.Contains(sItem2))
                            {
                                cResult[i] += string.Format("\\{0}", sItem2);
                            }
                        }
                        bMerged = true;
                    }
                }

                if (!bMerged)
                    cResult.Add(sStr);
            }
        }

        public static void Merge(List<string> cBase1, List<string> cBase2, ref List<string> cResult)
        {
            cResult.Clear();

            cResult.AddRange(cBase1);

            Merge(ref cResult, cBase2);
        }

        public static void KeepCommon(ref List<string> cBase, List<string> cStrings)
        {
            List<string> cCommon = new List<string>();

            foreach (string sStr in cBase)
                if (cStrings.Contains(sStr))
                    cCommon.Add(sStr);

            cBase.Clear();
            cBase.AddRange(cCommon);
        }

        public static List<string> RemoveCommon(ref List<string> cStrings, List<string> cCommon)
        {
            List<string> cRemoved = new List<string>();
            foreach (string sStr in cCommon)
            {
                if (cStrings.Contains(sStr))
                {
                    cStrings.Remove(sStr);
                    cRemoved.Add(sStr);
                }
            }

            return cRemoved;
        }
    }
}
