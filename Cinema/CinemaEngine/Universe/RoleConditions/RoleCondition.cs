using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nsUniLibXML;
using System.Xml;

namespace CinemaEngine.RoleConditions
{
    /// <summary>
    /// Описание условия, которому должен соответсвовать персонаж, чтобы принять участие в действии.
    /// также
    /// Описание измения в состоянии персонажа после выполнения действия.
    /// </summary>
    public abstract class RoleCondition
    {
        public enum ConditionType
        {
            Pose,
            Gender,
            Bindings,
        }

        //public bool Check()
        //{
        //    return m_bInverse ? !CheckCondition() : CheckCondition();
        //}

        //protected abstract bool CheckCondition();

        protected RoleCondition()
        { }

        protected RoleCondition(RoleCondition pCondition)
        {
        }

        internal virtual void Write(UniLibXML pXml, XmlNode pConditionNode)
        {
        }

        protected RoleCondition(UniLibXML pXml, XmlNode pConditionNode)
        {
        }

        public abstract RoleCondition Duplicate();
    }
}
