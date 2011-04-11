using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nsUniLibXML;
using System.Xml;

namespace SS30Conf
{
    public enum StringCategory
    { 
        ACTION,
        COMMAND,
        CONDITION,
        REACTION,
        FETISH,
        LEISURE,
        STAT,
        ROOM,
        SKILL,
        CONFIGURATION,
        ITEM,
        PERSON,
        COMMODITY
    }

    public enum StringSubCategory
    { 
        NULL,
        ROOM,
        SKILL,
        SKILL_COMP,
        STAT,
        STATUS,
        RND,
        APPEAL,
        ITEM,
        ITEM_GIFT,
        ITEM_HHM,
        ITEM_SEX_TOY,
        ITEM_UNIFORM,
        PERSON_MAID,
        PERSON_GUEST,
        PERSON_SPECIAL,
        SHOP,
        REST
    }

    public abstract class CConfigObject : CConfigProperty<string>
    {
        protected CConfigProperty<string> m_pName;
        public string Name
        {
            get { return m_pName.Value; }
            set { m_pName.Value = value; }
        }

        protected CConfigProperty<string> m_pDescription;
        public string Description
        {
            get { return m_pDescription.Value; }
            set { m_pDescription.Value = value; }
        }

        private StringCategory m_eCategory;

        public StringCategory Category
        {
            get { return m_eCategory; }
        }


        public class IdChangedArgs : EventArgs
        {
            private string m_sOldId;
            public string OldId
            {
                get { return m_sOldId; }
            }

            private CConfigObject m_pSender;
            public CConfigObject Sender
            {
                get { return m_pSender; }
            }

            public IdChangedArgs(string sOldId, CConfigObject pSender)
            {
                m_sOldId = sOldId;
                m_pSender = pSender;
            }
        }

        public event EventHandler<IdChangedArgs> IdChanged;

        private void RiseEvent(IdChangedArgs arg)
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<IdChangedArgs> temp = IdChanged;
            if (temp != null)
                temp(this, arg);
        }

        public override string Value
        {
            get { return base.Value; }
            set 
            {
                if (base.Value != value)
                {
                    string oldValue = base.Value;
                    base.Value = value;
                    if (CConfigRepository.Instance.Strings[m_eCategory].ContainsKey(oldValue))
                        CConfigRepository.Instance.Strings[m_eCategory].Remove(oldValue);
                    CConfigRepository.Instance.Strings[m_eCategory][base.Value] = this;
                    RiseEvent(new IdChangedArgs(oldValue, this));
                }
            }
        }

        private string MakeID(string newID)
        {
            if (newID.StartsWith(CConfigRepository.Instance.CategoryNames[m_eCategory]))
                return newID;
            else
                return CConfigRepository.Instance.CategoryNames[m_eCategory] + newID;
        }

        public virtual bool Delete()
        {
            if (m_eState != ModifyState.Added)
            {
                m_eState = ModifyState.Erased;
                return false;
            }
            CConfigRepository.Instance.Strings[m_eCategory].Remove(Value);
            return true;
        }

        private Dictionary<string, CConfigProperty> m_cProperties = new Dictionary<string, CConfigProperty>();

        public Dictionary<string, CConfigProperty> Properties
        {
            get { return m_cProperties; }
        }

        protected virtual void InitProperties()
        {
            m_pName = new CConfigProperty<string>(this, "Name", "name");

            m_pDescription = new CConfigProperty<string>(this, "Description", "description");
        }

        public CConfigObject(StringCategory eCategory)
            : base("nil")
        {
            InitProperties();

            m_eState = ModifyState.Added;

            m_eCategory = eCategory;
            Value = MakeID(DateTime.Now.Ticks.ToString("x").Substring(3,7));

            CConfigRepository.Instance.Strings[eCategory][Value] = this;
        }

        public CConfigObject(StringCategory eCategory, string sID)
            : base("nil")
        {
            InitProperties();

            m_eCategory = eCategory;
            Value = MakeID(sID);

            m_eState = ModifyState.Unmodified;

            CConfigRepository.Instance.Strings[eCategory][Value] = this;
        }

        public CConfigObject(UniLibXML xml, XmlNode pNode)
            : base("nil")
        {
            m_eCategory = (StringCategory)Enum.Parse(typeof(StringCategory), pNode.ParentNode.Name);
            InitProperties();

            Parse(xml, pNode);
            m_eState = ModifyState.Unmodified;

            CConfigRepository.Instance.Strings[m_eCategory][Value] = this;
        }

        /// <summary>
        /// Записывает в строку ID, имя и описание элемента
        /// </summary>
        /// <returns></returns>
        public void Write2XML(UniLibXML xml, XmlNode categoryNode)
        {
            if (State == ModifyState.Erased)
                return;

            XmlNode newNode = xml.CreateNode(categoryNode, Value);
            foreach (string propName in m_cProperties.Keys)
                xml.AddAttribute(newNode, propName, m_cProperties[propName].Value);
        }

        /// <summary>
        /// Считывает из трёх первых токенов ID, имя и описание элемента
        /// </summary>
        /// <param name="tokens"></param>
        public void Parse(UniLibXML xml, XmlNode pNode)
        {
            Value = pNode.Name;

            foreach (string propName in m_cProperties.Keys)
            {
                object prop = m_cProperties[propName].Value;
                xml.GetAttribute(pNode, propName, ref prop);
                m_cProperties[propName].Value = prop;
            }
        }

        /// <summary>
        /// Вызывается после того, как все строки считаны из файла -
        /// для того, чтобы восстановить перекрёстные ссылки (если они есть)
        /// </summary>
        public virtual void PostParse()
        { 
        }
    }
}
