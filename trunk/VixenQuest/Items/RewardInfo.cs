using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VixenQuest
{
    public class RewardInfo
    {
        public string m_sName;
        public string m_sNames;
        public int m_iRank;

        public RewardInfo(string sName, string sNames, int iValue)
        {
            m_sName = sName;
            m_sNames = sNames;
            m_iRank = iValue;
        }    
    }

    public enum ClothesBodyPart
    {
        Top,
        Bottom,
        Foot,
    }

    public enum ClothesGender
    {
        Male,
        Female,
        Unisex
    }

    public class ClothesInfo
        : RewardInfo
    {
        public ClothesBodyPart m_eBodyPart = ClothesBodyPart.Top;
        public ClothesGender m_eGender = ClothesGender.Unisex;

        public ClothesInfo(ClothesBodyPart eBodyPart, ClothesGender eGender, string sName, int iValue)
            : this(eBodyPart, eGender, sName, sName, iValue)
        { }

        public ClothesInfo(ClothesBodyPart eBodyPart, ClothesGender eGender, string sName, string sNames, int iValue)
            : base(sName, sNames, iValue)
        {
            m_eBodyPart = eBodyPart;
            m_eGender = eGender;
        }
    }

    public enum JevelryType
    { 
        Neck,
        Finger,
        Bracelet,
        Pierceing
    }

    public class JevelryInfo
        : RewardInfo
    {
        public JevelryType m_eBodyPart = JevelryType.Neck;

        public JevelryInfo(JevelryType eBodyPart, string sName, int iValue)
            : this(eBodyPart, sName, sName, iValue)
        { }

        public JevelryInfo(JevelryType eBodyPart, string sName, string sNames, int iValue)
            : base(sName, sNames, iValue)
        {
            m_eBodyPart = eBodyPart;
        }
    }

    public class ToysInfo
        : RewardInfo
    { 
        public ToysInfo(string sName, int iValue)
            : this(sName, sName, iValue)
        { }

        public ToysInfo(string sName, string sNames, int iValue)
            : base(sName, sNames, iValue)
        { }
    }

    public class TrashInfo
        : RewardInfo
    { 
        public TrashInfo(string sName, int iValue)
            : this(sName, sName, iValue)
        { }

        public TrashInfo(string sName, string sNames, int iValue)
            : base(sName, sNames, iValue)
        { }
    }
}
