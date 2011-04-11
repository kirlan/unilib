using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VixenQuest
{
    public class Item
    {
        public string m_sName;
        public string m_sNames;
        public int m_iWeight;
        public int m_iPrice;

        public override string ToString()
        {
            return m_sName + " (#:" + m_iWeight.ToString() + ", $:" + m_iPrice.ToString() + ")";
        }
    }
}
