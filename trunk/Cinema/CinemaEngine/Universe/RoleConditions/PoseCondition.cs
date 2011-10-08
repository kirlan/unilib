using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nsUniLibXML;
using System.Xml;

namespace CinemaEngine.RoleConditions
{
    /// <summary>
    /// Условие, основанное на позе персонажа.
    /// </summary>
    public class PoseCondition : RoleCondition
    {
        private bool m_bForbidden = false;
        /// <summary>
        /// Признак того, является ли указанная поза требуемой или наоборот - недопустимой
        /// </summary>
        public bool Forbidden
        {
            get { return m_bForbidden; }
            set { m_bForbidden = value; }
        }

        private CharacterState.Pose m_ePose = CharacterState.Pose.Undefined;
        /// <summary>
        /// Поза персонажа
        /// </summary>
        public CharacterState.Pose Pose
        {
            get { return m_ePose; }
            set { m_ePose = value; }
        }

        public PoseCondition()
            : base()
        { }

        public PoseCondition(PoseCondition pCondition)
            : base(pCondition)
        {
            m_ePose = pCondition.m_ePose;
            m_bForbidden = pCondition.m_bForbidden;
        }

        internal override void Write(UniLibXML pXml, XmlNode pConditionNode)
        {
            base.Write(pXml, pConditionNode);

            pXml.AddAttribute(pConditionNode, "pose", m_ePose);
            pXml.AddAttribute(pConditionNode, "forbidden", m_bForbidden);
        }

        public PoseCondition(UniLibXML pXml, XmlNode pConditionNode)
            : base(pXml, pConditionNode)
        {
            m_ePose = (CharacterState.Pose)pXml.GetEnumAttribute(pConditionNode, "pose", m_ePose.GetType());
            pXml.GetBoolAttribute(pConditionNode, "forbidden", ref m_bForbidden);
        }

        public override RoleCondition Duplicate()
        {
            return new PoseCondition(this);
        }
    }
}
