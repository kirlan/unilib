using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CinemaEngine
{
    /// <summary>
    /// "Мгновенный снимок" состояния действующего лица в определённый момент времени 
    /// (обычно - начало эпизода или сцены)
    /// </summary>
    public class CharacterState
    {
        public enum Pose
        {
            Undefined,
            StandNear,
            SitOn,
            LieOn,
            StandOn,
            LieUnder,
            StayInside,
            SitInside,
            LieInside
        }

        public static string GetPoseString(Pose ePose, bool bFuture)
        {
            switch (ePose)
            {
                case Pose.Undefined:
                    return bFuture ? "Not changed" : "Any";
                case Pose.StandNear:
                    return "Standing near";
                case Pose.LieOn:
                    return "Lying on";
                case Pose.LieInside:
                    return "Lying inside";
                case Pose.LieUnder:
                    return "Lying under";
                case Pose.SitOn:
                    return "Sitting on";
                case Pose.SitInside:
                    return "Sitting inside";
                case Pose.StandOn:
                    return "Standing on";
                case Pose.StayInside:
                    return "Standing inside";
                default:
                    return "Impossible!";
            }
        }

        private Character m_pCharacter;

        public Character Character
        {
            get { return m_pCharacter; }
        }

        private List<Item> m_cInventory = new List<Item>();

        public List<Item> Inventory
        {
            get { return m_cInventory; }
        }

        private Dictionary<EquipmentSlot, Item> m_cEquipment = new Dictionary<EquipmentSlot, Item>();

        public Dictionary<EquipmentSlot, Item> Equipment
        {
            get { return m_cEquipment; }
        }

        private Dictionary<string, List<Character>> m_cBindings = new Dictionary<string, List<Character>>();

        public Dictionary<string, List<Character>> Bindings
        {
            get { return m_cBindings; }
        }

        private Pose m_eCurrentPose;

        public Pose CurrentPose
        {
            get { return m_eCurrentPose; }
            set { m_eCurrentPose = value; }
        }

        private Furniture m_pAnchor;

        public Furniture Anchor
        {
            get { return m_pAnchor; }
            set { m_pAnchor = value; }
        }

        public CharacterState(Character pChar, Furniture pAnchor)
        {
            m_pCharacter = pChar;
            m_pAnchor = pAnchor;
            foreach(Pose pPose in pAnchor.Template.Anchors[0].PossiblePoses.Keys)
            {
                m_eCurrentPose = pPose;
                break;
            }
        }
    }
}
