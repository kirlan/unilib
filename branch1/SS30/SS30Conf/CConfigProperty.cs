using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS30Conf
{
    public enum ModifyState
    {
        Added,
        Erased,
        Modified,
        Unmodified
    }

    public class CConfigProperty
    {
        protected object m_Value;

        public object Value
        {
            get { return m_Value; }
            set
            {
                if (m_Value != value)
                {
                    m_Value = value;
                    Modify();
                }
            }
        }

        protected ModifyState m_eState = ModifyState.Unmodified;
        public ModifyState State
        {
            get { return m_eState; }
        }

        public void Modify()
        {
            if (m_eState == ModifyState.Unmodified)
                m_eState = ModifyState.Modified;

            if (m_pOwner != null)
                m_pOwner.Modify();
        }

        private CConfigObject m_pOwner;

        public CConfigProperty()
        {
            m_pOwner = null;
        }

        public CConfigProperty(CConfigObject pOwner, string sName)
        {
            m_pOwner = pOwner;
            m_pOwner.Properties[sName] = this;
        }
    }

    public class CConfigProperty<T> : CConfigProperty where T:IComparable
    {
        public new virtual T Value
        {
            get { return (T)m_Value; }
            set 
            {
                if (!value.Equals(m_Value))
                {
                    m_Value = value;
                    Modify();
                }
            }
        }

        public CConfigProperty(T defaultValue)
            : base()
        {
            m_Value = defaultValue;
        }

        public CConfigProperty(CConfigObject pOwner, string sName, T defaultValue)
            : base(pOwner, sName)
        {
            m_Value = defaultValue;
        }
    }

    public class CBindingConfigProperty<T> : CConfigProperty<string> where T:CConfigObject
    {
        private bool m_bFirstTime = true;

        public override string Value
        {
            get { return base.Value; }
            set
            {
                if (Object != null)
                {
                    Object.IdChanged -= ObjectIdChangedHandler;
                }
                base.Value = value;
                m_bFirstTime = true;
            }
        }

        protected StringCategory m_eCategory;

        public void UpdateCategory(StringCategory eCategory)
        {
            m_eCategory = eCategory;
        }

        public T Object
        {
            get 
            {
                if (Value == "nil")
                    return null;
                if (CConfigRepository.Instance.Strings.ContainsKey(m_eCategory) &&
                    CConfigRepository.Instance.Strings[m_eCategory].ContainsKey(Value))
                {
                    T pObject = CConfigRepository.Instance.Strings[m_eCategory][Value] as T;
                    if (m_bFirstTime)
                    {
                        pObject.IdChanged += ObjectIdChangedHandler;
                        m_bFirstTime = false;
                    }
                    return CConfigRepository.Instance.Strings[m_eCategory][Value] as T;
                }
                else
                    return null;
            }
            set 
            {
                if (Object != null)
                {
                    Object.IdChanged -= ObjectIdChangedHandler;
                }
                base.Value = value.Value;
                Object.IdChanged += ObjectIdChangedHandler;
            }
        }

        void ObjectIdChangedHandler(object sender, CConfigObject.IdChangedArgs e)
        {
            if (base.Value == e.OldId)
                base.Value = e.Sender.Value;
        }

        public CBindingConfigProperty(StringCategory eCategory)
            : base("nil")
        {
            m_eCategory = eCategory;
        }

        public CBindingConfigProperty(CConfigObject pOwner, string sName, StringCategory eCategory)
            : base(pOwner, sName, "nil")
        {
            m_eCategory = eCategory;
        }
    }
}
