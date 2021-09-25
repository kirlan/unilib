using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nsUniLibXML;
using System.Xml;

namespace Persona
{
    /// <summary>
    /// Действие. Во время хода, игроку предлагается выбрать действие из числа тех, 
    /// которые ведут хотя бы к одному возможному в настоящий момент событию. После выбора действия 
    /// автоматически случайным образом выбирается и разыгрывается одно из следующих из него
    /// возможных событий.
    /// </summary>
    public class Action
    {
        /// <summary>
        /// Краткое описание действия, например - "Секс", "Работа", "Учёба"...
        /// </summary>
        public string m_sName = "Новое действие";

        public Action(string sName)
        {
            m_sName = sName;
        }

        public Action(UniLibXML pXml, XmlNode pActionNode)
        {
            pXml.GetStringAttribute(pActionNode, "name", ref m_sName);
        }

        public override string ToString()
        {
            return m_sName;
        }

        internal void WriteXML(UniLibXML pXml, XmlNode pActionNode)
        {
            pXml.AddAttribute(pActionNode, "name", m_sName);
        }
    }
}
