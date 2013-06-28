using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nsUniLibXML;
using System.Xml;

namespace Persona
{
    /// <summary>
    /// Категория событий. Во время хода, игроку предлагается выбрать категорию из числа тех, 
    /// в которых есть хотя бы одно возможное в настоящий момент событие. После выбора категории 
    /// автоматически случайным образом выбирается и разыгрывается одно из относящихся к этой категории
    /// возможных событий.
    /// </summary>
    public class Domain
    {
        /// <summary>
        /// Краткое описание категории, например - "Секс", "Работа", "Учёба"...
        /// </summary>
        public string m_sName = "Новая категория";

        public Domain(string sName)
        {
            m_sName = sName;
        }

        public Domain(UniLibXML pXml, XmlNode pParamNode)
        {
            pXml.GetStringAttribute(pParamNode, "name", ref m_sName);
        }

        public override string ToString()
        {
            return m_sName;
        }

        internal void WriteXML(UniLibXML pXml, XmlNode pDomainNode)
        {
            pXml.AddAttribute(pDomainNode, "name", m_sName);
        }
    }
}
