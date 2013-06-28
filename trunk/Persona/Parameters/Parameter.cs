using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nsUniLibXML;
using System.Xml;

namespace Persona.Parameters
{
    /// <summary>
    /// Хранимый параметр.
    /// </summary>
    public abstract class Parameter
    {
        /// <summary>
        /// Короткое имя параметра, например - "Здоровье", "$$$", "Встретил лешего"
        /// </summary>
        public string m_sName;

        /// <summary>
        /// Короткое название группы, в которую параметр должен быть отнесён при отображении пользователю.
        /// </summary>
        public string m_sGroup;
        
        /// <summary>
        /// Если true. то параметр будет скрыт от игрока.
        /// </summary>
        public bool m_bHidden = false;

        /// <summary>
        /// Комментарий к параметру - используется только при разработке модуля,
        /// может содержать, например, расшифровку числовых значений...
        /// </summary>
        public string m_sComment;

        public Parameter()
        { 
        }

        public Parameter(UniLibXML pXml, XmlNode pParamNode)
        {
            pXml.GetStringAttribute(pParamNode, "name", ref m_sName);
            pXml.GetStringAttribute(pParamNode, "group", ref m_sGroup);
            pXml.GetStringAttribute(pParamNode, "comment", ref m_sComment);
            pXml.GetBoolAttribute(pParamNode, "hidden", ref m_bHidden);
        }

        internal virtual void WriteXML(UniLibXML pXml, XmlNode pParamNode)
        {
            pXml.AddAttribute(pParamNode, "name", m_sName);
            pXml.AddAttribute(pParamNode, "group", m_sGroup);
            pXml.AddAttribute(pParamNode, "comment", m_sComment);
            pXml.AddAttribute(pParamNode, "hidden", m_bHidden);
        }
    }
}
