using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RB
{
    enum LocationType
    { 
        Undefined,
        Settlement,
        Field,
        Forbidden
    }

    /// <summary>
    /// Локация — это точка на карте, в которой могут происходить какие-либо события. 
    /// Это может быть город, деревня, горный перевал или поляна в лесу... Локации соединяются 
    /// между собой переходами,  определяющими возможность и затраты времени на перемещение 
    /// из одной локации в другую. В любой локации в каждый момент игрового времени могет 
    /// находиться неограниченное количество людей, невзирая на их принадлежность к сообществам. 
    /// 
    /// Любое взаимодействие между персонажами возможно тогда и только тогда, когда они 
    /// находятся в одной локации.
    /// </summary>
    class CLocation
    {
        private int m_iX;

        public int X
        {
            get { return m_iX; }
        }
        private int m_iY;

        public int Y
        {
            get { return m_iY; }
        }

        private List<CLocation> m_cLinks = new List<CLocation>();

        internal List<CLocation> Links
        {
            get { return m_cLinks; }
        }

        private const int m_iMaxLinks = 8;

        public int MaxLinks
        {
            get { return m_iMaxLinks; }
        }

        private CSociety m_pOwner = null;

        internal CSociety Owner
        {
            get { return m_pOwner; }
            set { m_pOwner = value; }
        }

        private LocationType m_eType = LocationType.Undefined;

        internal LocationType Type
        {
            get { return m_eType; }
            set { m_eType = value; }
        }

        private string m_sName;

        public string Name
        {
            get { return m_sName; }
            set { m_sName = value; }
        }

        public CLocation(int iX, int iY)
        {
            m_iX = iX;
            m_iY = iY;
        }
    }
}
