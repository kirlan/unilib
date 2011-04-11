using System;
using System.Collections.Generic;
using System.Text;
using SS30Conf.Actions;
using System.Xml;
using nsUniLibXML;
using SS30Conf.Persons;
using SS30Conf.Time;

namespace SS30Conf
{
    public enum RoomType
    { 
        LEISURE,
        GUEST,
        SERVICE
    }

    /// <summary>
    /// Комната в отеле
    /// </summary>
    public class CRoom: CConfigObject
    {
        private List<CLogRecord> m_cLog = new List<CLogRecord>();

        public List<CLogRecord> Log
        {
            get { return m_cLog; }
        }

        public void RecordEvent(string sMessage)
        {
            m_cLog.Add(new CLogRecord(sMessage));
        }
        
        public CLogRecord GetFirstRecord()
        {
            if (m_cLog.Count == 0)
            {
                return null;
            }

            CLogRecord pFirstRecord = m_cLog[0];
            foreach (CLogRecord record in m_cLog)
            {
                if (record.FullMinutes < pFirstRecord.FullMinutes)
                    pFirstRecord = record;
            }
            return pFirstRecord;
        }

        private CConfigProperty<RoomType> m_pRoomType;

        public RoomType RoomType
        {
            get { return m_pRoomType.Value; }
            //set { m_pRoomType.Value = value; }
        }

        private CConfigProperty<int> m_pCostToBuild;

        public int CostToBuild
        {
            get { return m_pCostToBuild.Value; }
            set { m_pCostToBuild.Value = value; }
        }
        private CConfigProperty<int> m_pDaysToBuild;

        public int DaysToBuild
        {
            get { return m_pDaysToBuild.Value; }
            set { m_pDaysToBuild.Value = value; }
        }

        private CConfigProperty<bool> m_pBuilt;

        public bool Built
        {
            get { return m_pBuilt.Value; }
            set { m_pBuilt.Value = value; }
        }

        private List<CPerson> m_cPersons = new List<CPerson>();

        public List<CPerson> Persons
        {
            get { return m_cPersons; }
        }

        public CRoom()
            : base(StringCategory.ROOM)
        {
            Name = "New Room";
            Description = "Enter description for a new room here...";
        }

        public CRoom(string sId, string sName, RoomType eRoomType)
            : base(StringCategory.ROOM, sId)
        {
            Name = sName;
            m_pRoomType.Value = eRoomType;
            Description = "Enter description for a new room here...";
        }

        public CRoom(UniLibXML xml, XmlNode pNode, CWorld pWorld)
            : base(xml, pNode)
        {
            pWorld.Rooms.Add(this);
        }

        protected override void InitProperties()
        {
            base.InitProperties();

            m_pCostToBuild = new CConfigProperty<int>(this, "Cost", 1000);

            m_pDaysToBuild = new CConfigProperty<int>(this, "ETA", 10);

            m_pRoomType = new CConfigProperty<RoomType>(this, "Type", RoomType.LEISURE);

            m_pBuilt = new CConfigProperty<bool>(this, "Built", false);
        }
    }
}
