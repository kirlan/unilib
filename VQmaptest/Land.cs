using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;

namespace VQmaptest
{
    enum LandType
    {
        Undefined,
        Forbidden
    }

    public class Land
    {
        public State m_pState = null;

        public World m_pWorld;

        protected static string[] m_aEpithet = 
        {
            "Drunken ",
            "Silent ",
            "Fucked ",
            "Lustful ",
            "Sinful ",
            "Naked ",
            "Cursed ",
            "Posessed ",
            "Black ",
            "Pink ",
            "Horny ",
            "Diseased ",
            //"Dead ",
            "Hot ",
            "Wet ",
            "Happy ",
            "Extasy ",
            "Blessed ",
            "Dragon's ",
            "Siren's ",
            "Maid's ",
            "Lady's ",
            "Sexy ",
            "Cumming ",
            "Unreal ",
            "Burned ",
            //"Bloody ",
            "Burning ",
            "Haunted ",
            "Corrupted ",
            "Warped ",
            "Desired ",
            "Deep ",
            "Muddy ",
            "Orgazming ",
            "Singing ",
            "Screaming ",
            "Dangerous ",
            "Forgotten ",
            "Secret ",
            "Mysterious ",
            "Mystical ",
            "Beloved ",
            "Summer ",
            "Winter ",
            "Northern ",
            "Southern ",
            "Western ",
            "Eastern ",
            "Sweet ",
        };

        //private static string[] m_aPlace = 
        //{
        //    "Forest",
        //    "Woods",
        //    "Grove",
        //    "Sands",
        //    "Desert",
        //    "Wastes",
        //    "Lake",
        //    "River",
        //    "Mansion",
        //    "Castle",
        //    "Palace",
        //    "Cave",
        //    "Country",
        //    "Kingdom",
        //    "Cathedral",
        //    "Plains",
        //    "Shore",
        //    "Swamp",
        //    "Spot",
        //    "Gates",
        //    "Valley",
        //    "Grotto",
        //    "Peak",
        //    "Mountain",
        //    "Weil",
        //    "Wall",
        //    "Cemetary",
        //    "Graveyard",
        //    "Ruins",
        //    "Realm",
        //    "Plane",
        //    "Dimension",
        //    "Hole",
        //    "Hills",
        //    "Hill",
        //    "Mountains",
        //    "Hideout",
        //    "Nest",
        //    "Garden",
        //    "Lair",
        //};

        protected static string[] m_aDescription = 
        {
            " of Lust",
            " of Love",
            " of Eros",
            " of Sex",
            " of Erotic Dreams",
            " of Wet Dreams",
            " of Hot Dreams",
            " of Incredible Dreams",
            " of Forbidden Dreams",
            " of Fucking Good",
            " of Flying Dildos",
            " of Singing Dildos",
            " of Screaming Dildos",
            " of Jumping Dildos",
            " of Flying Cocks",
            " of Singing Cocks",
            " of Screaming Cocks",
            " of Jumping Cocks",
            " of Flying Whores",
            " of Screaming Whores",
            " of Singing Whores",
            " of Whores",
            " of Lustful Whores",
            " of Virgins",
            " of Nudity",
            " of Desire",
            " of Earthly Delights",
            " of Extasy",
            " of Fertylity"
        };
        
        private static string[] m_aPlace = 
        {
            "Forest",
            "Woods",
            "Sands",
            "Desert",
            "Wastes",
            "Country",
            "Plains",
            "Shore",
            "Swamp",
            "Valley",
            "Mountains",
            "Hills",
        };

        private List<Land> m_cLinks = new List<Land>();

        internal List<Land> Links
        {
            get { return m_cLinks; }
        }

        private const int m_iMaxLinks = 8;

        public int MaxLinks
        {
            get { return m_iMaxLinks; }
        }

        public string m_sName;

        public void Assign(State pState, bool bCapital)
        {
            if (pState != null && m_pWorld != pState.m_pWorld)
                throw new ArgumentException("Given state not belongs to this world!");

            if (m_eType == LandType.Forbidden && pState != null)
                throw new ArgumentException("Forbidden lands can't belong to any state!");

            m_pState = pState;

            int iPlace = Rnd.Get(m_aPlace.Length);
            int iEpithet = Rnd.Get(m_aEpithet.Length);
            int iDescription = Rnd.Get(m_aDescription.Length);

            int variant = Rnd.Get(3);

            switch (variant)
            {
                case 0:
                    {
                        m_sName = m_aEpithet[iEpithet];
                        m_sName += m_aPlace[iPlace];
                    }
                    break;
                case 1:
                    {
                        m_sName = m_aPlace[iPlace];
                        m_sName += m_aDescription[iDescription];
                    }
                    break;
                default:
                    {
                        m_sName = m_aEpithet[iEpithet];
                        m_sName += m_aPlace[iPlace];
                        m_sName += m_aDescription[iDescription];
                    }
                    break;
            }
        }

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

        public Land(World pWorld, int iX, int iY)
        {
            m_pWorld = pWorld;
            m_iX = iX;
            m_iY = iY;
        }

        private LandType m_eType = LandType.Undefined;

        internal LandType Type
        {
            get { return m_eType; }
            set { m_eType = value; }
        }

        public Land NextStepTo(Land pTarget)
        {
            if (this == pTarget)
                return null;

            if (m_pWorld == pTarget.m_pWorld)
            {
                Land[] pPath = m_pWorld.ShortestWay(this, pTarget);
                return pPath[1];
            }

            return null;
        }
    }
}
