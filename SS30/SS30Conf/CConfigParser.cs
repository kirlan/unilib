using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS30Conf
{
    public class CConfigParser
    {
        private string[] m_aTokens;

        //public string[] Tokens
        //{
        //  get { return m_aTokens; }
        //}

        private StringCategory m_eCategory;
        public StringCategory Category
        {
            get { return m_eCategory; }
        }

        private StringSubCategory m_eSubCategory;
        public StringSubCategory SubCategory
        {
            get { return m_eSubCategory; }
        }

        private int m_iPos;

        public void Parse(string line)
        {
            m_eCategory = StringCategory.CONFIGURATION;
            m_eSubCategory = StringSubCategory.NULL;
            foreach (StringCategory cat in CConfigString.CategoryNames.Keys)
            {
                if (line.StartsWith(CConfigString.CategoryNames[cat]))
                {
                    m_eCategory = cat;
                    break;
                }
            }
            m_aTokens = line.Split(new char[] { '|' });
            int subCatPos = 0;
            if (m_eCategory == StringCategory.COMMAND || m_eCategory == StringCategory.CONDITION)
                subCatPos = 4;
            if (m_eCategory == StringCategory.ITEM)
                subCatPos = 3;
            foreach (StringSubCategory subCat in CConfigString.SubCategoryNames.Keys)
            {
                if (m_aTokens.Length > subCatPos && m_aTokens[subCatPos].StartsWith(CConfigString.SubCategoryNames[subCat]))
                {
                    m_eSubCategory = subCat;
                    break;
                }
            }

            m_iPos = 0;
        }

        public string ID
        {
            get
            {
                if (m_aTokens.Length > 0)
                    return m_aTokens[0];
                else
                    return "wrong string";
            }
        }

        public void Skip()
        {
            m_iPos++;
        }

        public string ReadString()
        {
            if (m_aTokens.Length > m_iPos)
                return m_aTokens[m_iPos++];
            else
                return "";
        }

        public int ReadInt()
        {
            if (m_aTokens.Length > m_iPos)
                return int.Parse(m_aTokens[m_iPos++]);
            else
                return 0;
        }
    }
}
