using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using nsUniLibXML;
using SS30Conf.Persons;

namespace SS30Conf.Actions.Commands
{
    public class CCommandBuildRoom: CCommand
    {
        private CBindingConfigProperty<CRoom> m_pRoom;

        public CRoom Room
        {
            get { return m_pRoom.Object;  }
            set { m_pRoom.Object = value; }
        }

        public override void Execute(CPerson actor, CPerson target)
        {
            if(!Room.Built)
                Room.Built = true;
        }

        protected override void InitProperties()
        {
            base.InitProperties();

            m_pRoom = new CBindingConfigProperty<CRoom>(this, "Room", StringCategory.ROOM);
        }

        public CCommandBuildRoom(CReaction pParent)
            : base(StringSubCategory.ROOM, pParent)
        { 
        }

        public CCommandBuildRoom(UniLibXML xml, XmlNode pNode)
            : base(xml, pNode)
        { 
        }

        public override string ToString()
        {
            return string.Format("build {0}", Room != null ? Room.Value:"WRONG ROOM");
        }
    }
}
