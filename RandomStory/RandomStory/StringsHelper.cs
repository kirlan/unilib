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

        public static string GetRandom(List<string> cStrings, ref char[] aFlags)
        {
            return GetRandom(cStrings, new List<string>(), string.Empty, ref aFlags);
        }

        public static string GetRandom(List<string> cStrings, string sDefault, ref char[] aFlags)
        {
            return GetRandom(cStrings, new List<string>(), sDefault, ref aFlags);
        }

        public static string GetRandom(List<string> cStrings, List<string> cExceptions, ref char[] aFlags)
        {
            return GetRandom(cStrings, cExceptions, string.Empty, ref aFlags);
        }

        public static string GetRandom(List<string> cStrings, List<string> cExceptions, string sDefault, ref char[] aFlags)
        {
            if (cStrings.Count > cExceptions.Count)
            {
                int iCounter = 1000;
                do
                {
                    iCounter--;
                    string sItem = cStrings[Rnd.Get(cStrings.Count)];
                    string[] aItems = sItem.Split(new char[] { '\\', '/', '|' });

                    bool bException = false;
                    foreach (string sItm in aItems)
                        if (cExceptions.Contains(sItm))
                            bException = true;

                    if (bException)
                        continue;

                    if (aFlags != null)
                    {
                        List<string> cTrueItems = new List<string>();
                        List<string> cPossibleItems = new List<string>();
                        for (int i = 0; i < aItems.Length; i++)
                        {
                            string sItm = aItems[i];
                            char[] aItmFlags = StringsHelper.GetFlags(ref sItm);
                            if (aItmFlags.Length == 0)
                                cPossibleItems.Add(sItm);

                            if(aFlags.Length == aItmFlags.Length)
                            {
                                if (aFlags.Length == 0)
                                    cTrueItems.Add(sItm);
                                else
                                { 
                                    bool bOK = true;
                                    foreach (char chFlag1 in aFlags)
                                        if (!aItmFlags.Contains(chFlag1))
                                            bOK = false;
                                    if(bOK)
                                        cTrueItems.Add(sItm);
                                }
                            }
                        }

                        if (cTrueItems.Count > 0)
                            aItems = cTrueItems.ToArray();
                        else
                            aItems = cPossibleItems.ToArray();
                    }

                    if (aItems.Length > 0)
                    {
                        sItem = aItems[Rnd.Get(aItems.Length)];

                        char[] aItemFlags = StringsHelper.GetFlags(ref sItem);

                        if (!cExceptions.Contains(sItem))
                        {
                            if (aFlags == null)
                                aFlags = aItemFlags;
                            return sItem;
                        }
                    }
                }
                while (iCounter>0);
            }

            return sDefault;
        }

        public static string GetRelative(List<string> cStrings, string sRelative, ref char[] aFlags)
        {
            foreach (string sItem in cStrings)
            {
                string[] aItems = sItem.Split(new char[] { '\\', '/', '|' });

                if (aFlags != null)
                {
                    List<string> cTrueItems = new List<string>();
                    List<string> cPossibleItems = new List<string>();
                    for (int i = 0; i < aItems.Length; i++)
                    {
                        string sItm = aItems[i];
                        char[] aItmFlags = StringsHelper.GetFlags(ref sItm);
                        if (aItmFlags.Length == 0)
                            cPossibleItems.Add(sItm);

                        if (aFlags.Length == aItmFlags.Length)
                        {
                            if (aFlags.Length == 0)
                                cTrueItems.Add(sItm);
                            else
                            {
                                bool bOK = true;
                                foreach (char chFlag1 in aFlags)
                                    if (!aItmFlags.Contains(chFlag1))
                                        bOK = false;
                                if (bOK)
                                    cTrueItems.Add(sItm);
                            }
                        }
                    }

                    if (cTrueItems.Count > 0)
                        aItems = cTrueItems.ToArray();
                    else
                        aItems = cPossibleItems.ToArray();
                }

                for (int i = 0; i < aItems.Length; i++)
                {
                    string sItm = aItems[i];
                    char[] aItmFlags = StringsHelper.GetFlags(ref sItm);
                    if (sItm.Equals(sRelative))
                    {
                        if (aFlags == null)
                            aFlags = aItmFlags;

                        return sItm;
                    }
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
