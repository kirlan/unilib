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
        /// Имя коллекции, к которой принадлежит параметр (если принадлежит).
        /// Нужно для однозначной идентификации параметра, если в нескольких коллекциях есть одинаковые параметры.
        /// </summary>
        public string m_sCollection = "";

        public string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(m_sCollection))
                    return m_sName;
                else
                    return m_sCollection + "." + m_sName;
            }
        }

        /// <summary>
        /// Если true. то параметр будет скрыт от игрока.
        /// </summary>
        public bool m_bHidden = false;

        public bool m_bChanged = false;

        /// <summary>
        /// Комментарий к параметру - используется только при разработке модуля,
        /// может содержать, например, расшифровку числовых значений...
        /// </summary>
        public string m_sComment;

        /// <summary>
        /// Ссылка на функцию для автоматического вычисления значения параметра.
        /// </summary>
        public Function m_pFunction = null;

        public virtual string DisplayValue
        {
            get 
            {
                return "значение не определено";
            }
        }

        public virtual string GetDisplayValue(object value)
        {
            return "значение не определено";
        }

        public abstract void SetValue(string sValue);

        protected Parameter()
        { 
        }

        protected Parameter(Parameter pOrigin, bool bClone)
        {
            m_sName = pOrigin.m_sName;
            if(bClone)
                m_sName += " (копия)";
            m_sGroup = pOrigin.m_sGroup;
            m_sCollection = pOrigin.m_sCollection;
            m_sComment = pOrigin.m_sComment;
            m_bHidden = pOrigin.m_bHidden;
        }

        protected Parameter(UniLibXML pXml, XmlNode pParamNode, string sCollection)
        {
            pXml.GetStringAttribute(pParamNode, "name", ref m_sName);
            pXml.GetStringAttribute(pParamNode, "group", ref m_sGroup);
            pXml.GetStringAttribute(pParamNode, "comment", ref m_sComment);
            pXml.GetBoolAttribute(pParamNode, "hidden", ref m_bHidden);
            m_sCollection = sCollection;
        }

        internal virtual void WriteXML(UniLibXML pXml, XmlNode pParamNode)
        {
            pXml.AddAttribute(pParamNode, "name", m_sName);
            pXml.AddAttribute(pParamNode, "group", m_sGroup);
            pXml.AddAttribute(pParamNode, "comment", m_sComment);
            pXml.AddAttribute(pParamNode, "hidden", m_bHidden);
        }

        public override string ToString()
        {
            return m_sGroup + ": " + m_sName;
        }

        public abstract void Init();
    }
}
