using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using nsUniLibXML;
using System.Xml;
using VixenQuest.People;

namespace VixenQuest
{
    public class RewardClothes
        : Reward
    {
        private static ValuedString[] m_aClothesMaterials = 
        {
            new ValuedString("", 0),
            //new ValuedString("burlap ", 1),
            new ValuedString("canvas ", 1),
            new ValuedString("linen ", 1),
            new ValuedString("cotton ", 2),
            new ValuedString("woollen ", 3),
            new ValuedString("satin ", 4),
            new ValuedString("lawn ", 5),
            new ValuedString("jean ", 6),
            new ValuedString("chainmail ", 7),
            new ValuedString("silk ", 8),
            new ValuedString("suede ", 9),
            new ValuedString("velvet ", 10),
            new ValuedString("leather ", 11),
            new ValuedString("latex ", 12),
            new ValuedString("fishnet ", 14),
            new ValuedString("illusive ", 16),
            //new ValuedString("semi-transparent ", 5),
            //new ValuedString("Transparent ", 6),
        };

        private static ValuedString[] m_aBootsMaterials = 
        {
            new ValuedString("", 0),
            new ValuedString("chitin ", 1),
            new ValuedString("coarse skin ", 2),
            new ValuedString("thick leather ", 4),
            new ValuedString("soft skin ", 6),
            new ValuedString("plain leather ", 8),
            new ValuedString("sewed leather ", 10),
            new ValuedString("studded leather ", 12),
            new ValuedString("suede ", 14),
            new ValuedString("latex ", 16),
        };

        private static ClothesInfo[] m_aClothesName = 
        {
            //common clothes
            new ClothesInfo(ClothesBodyPart.Bottom, ClothesGender.Female, "skirt", "skirts", 1),
            new ClothesInfo(ClothesBodyPart.Bottom, ClothesGender.Unisex, "pants", 1),
            new ClothesInfo(ClothesBodyPart.Bottom, ClothesGender.Male, "kilt", "kilts", 5),
            new ClothesInfo(ClothesBodyPart.Top, ClothesGender.Female, "blouse", "blouses", 1),
            new ClothesInfo(ClothesBodyPart.Top, ClothesGender.Female, "dress", "dresses", 5),
            new ClothesInfo(ClothesBodyPart.Top, ClothesGender.Unisex, "shirt", "shirts", 1),
            new ClothesInfo(ClothesBodyPart.Top, ClothesGender.Unisex, "jacket", "jackets", 5),
            new ClothesInfo(ClothesBodyPart.Top, ClothesGender.Unisex, "apron", "aprons", 1),
            new ClothesInfo(ClothesBodyPart.Top, ClothesGender.Unisex, "robe", "robes", 5),
            new ClothesInfo(ClothesBodyPart.Top, ClothesGender.Unisex, "gown", "gowns", 5),
            new ClothesInfo(ClothesBodyPart.Top, ClothesGender.Unisex, "tunic", "tunics", 1),
            new ClothesInfo(ClothesBodyPart.Top, ClothesGender.Unisex, "kimono", "kimonos", 5),
            new ClothesInfo(ClothesBodyPart.Top, ClothesGender.Unisex, "toga", "togas", 5),
            new ClothesInfo(ClothesBodyPart.Foot, ClothesGender.Unisex, "boots", 1),
            new ClothesInfo(ClothesBodyPart.Foot, ClothesGender.Unisex, "moccasins", 5),
            //common undies
            new ClothesInfo(ClothesBodyPart.Bottom, ClothesGender.Female, "g-string", "g-strings", 15),
            new ClothesInfo(ClothesBodyPart.Bottom, ClothesGender.Female, "pair of stockings", "stockings", 15),
            new ClothesInfo(ClothesBodyPart.Bottom, ClothesGender.Unisex, "panty", "panties", 10),
            new ClothesInfo(ClothesBodyPart.Bottom, ClothesGender.Unisex, "thong", "thongs", 15),
            new ClothesInfo(ClothesBodyPart.Bottom, ClothesGender.Unisex, "undies", 10),
            new ClothesInfo(ClothesBodyPart.Bottom, ClothesGender.Male, "boxers", 10),
            new ClothesInfo(ClothesBodyPart.Top, ClothesGender.Female, "bra", "bras", 10),
            new ClothesInfo(ClothesBodyPart.Top, ClothesGender.Female, "bustier", "bustiers", 15),
            new ClothesInfo(ClothesBodyPart.Top, ClothesGender.Female, "lingerie", "lingeries", 15),
            new ClothesInfo(ClothesBodyPart.Top, ClothesGender.Female, "nightie", "nighties", 15),
            new ClothesInfo(ClothesBodyPart.Top, ClothesGender.Unisex, "undershirt", "undershirts", 10),
            new ClothesInfo(ClothesBodyPart.Top, ClothesGender.Unisex, "pajama", "pajamas", 10),
            new ClothesInfo(ClothesBodyPart.Top, ClothesGender.Unisex, "leotard", "leotards", 15),
            new ClothesInfo(ClothesBodyPart.Top, ClothesGender.Male, "muscle shirt", "muscle shirts", 10),
            new ClothesInfo(ClothesBodyPart.Top, ClothesGender.Male, "tank top", "tank tops", 15),
            //new ClothesInfo(ClothesBodyPart.Top, ClothesGender.Female, "bikini", 2),
            //new ClothesInfo(ClothesBodyPart., "garter", "garters", 2),
            //new ClothesInfo(ClothesBodyPart., "suspender", "suspenders", 2),
            new ClothesInfo(ClothesBodyPart.Foot, ClothesGender.Female, "heel-strap sandals", 15),
            new ClothesInfo(ClothesBodyPart.Foot, ClothesGender.Unisex, "open-toe sandals", 15),
            new ClothesInfo(ClothesBodyPart.Foot, ClothesGender.Unisex, "sandals", 10),
            new ClothesInfo(ClothesBodyPart.Foot, ClothesGender.Unisex, "sabot shoes", 15),
            //new ClothesInfo(ClothesBodyPart.Foot, ClothesGender.Unisex, "mules", 4),
            //sexy outfit
            new ClothesInfo(ClothesBodyPart.Bottom, ClothesGender.Female, "chastity belt", "chastity belts", 25),
            new ClothesInfo(ClothesBodyPart.Bottom, ClothesGender.Female, "crotchless panty", "crotchless panties", 25),
            new ClothesInfo(ClothesBodyPart.Bottom, ClothesGender.Female, "crotchless thong", "crotchless thongs", 25),
            new ClothesInfo(ClothesBodyPart.Bottom, ClothesGender.Female, "crotchless g-string", "crotchless g-strings", 25),
            new ClothesInfo(ClothesBodyPart.Bottom, ClothesGender.Female, "mini-skirt", "mini-skirts", 20),
            new ClothesInfo(ClothesBodyPart.Bottom, ClothesGender.Unisex, "shorts", 20),
            new ClothesInfo(ClothesBodyPart.Bottom, ClothesGender.Unisex, "loincloth", "loincloths", 25),
            new ClothesInfo(ClothesBodyPart.Bottom, ClothesGender.Male, "jockstrap", "jockstraps", 25),
            new ClothesInfo(ClothesBodyPart.Bottom, ClothesGender.Male, "cock harness", "cock harnesses", 25),
            new ClothesInfo(ClothesBodyPart.Top, ClothesGender.Female, "bra harness", "bra harnesses", 25),
            new ClothesInfo(ClothesBodyPart.Top, ClothesGender.Female, "pasteases", 25),
            new ClothesInfo(ClothesBodyPart.Top, ClothesGender.Female, "nipple pasties", 25),
            new ClothesInfo(ClothesBodyPart.Top, ClothesGender.Female, "nipple tassels", 25),
            new ClothesInfo(ClothesBodyPart.Top, ClothesGender.Female, "corset", "corsets", 20),
            //new ClothesInfo(ClothesBodyPart.Top, ClothesGender.Female, "bodystocking", "bodystockings", 25),
            new ClothesInfo(ClothesBodyPart.Top, ClothesGender.Female, "catsuit", "catsuits", 20),
            new ClothesInfo(ClothesBodyPart.Top, ClothesGender.Unisex, "harness", "harnesses", 25),
            new ClothesInfo(ClothesBodyPart.Top, ClothesGender.Male, "crossbelt", "crossbelts", 25),
            new ClothesInfo(ClothesBodyPart.Top, ClothesGender.Male, "vest", "vests", 20),
            //new ClothesInfo(ClothesBodyPart.Top, ClothesGender.Male, "baldric", "baldrics", 3),
            new ClothesInfo(ClothesBodyPart.Foot, ClothesGender.Female, "high heels jackboots", 25),
            new ClothesInfo(ClothesBodyPart.Foot, ClothesGender.Female, "high heels shoes", 20),
            new ClothesInfo(ClothesBodyPart.Foot, ClothesGender.Unisex, "jackboots", 25),
        };

        public ClothesInfo m_pInfo;
        
        public RewardClothes()
        {
            m_iWeight = 1;
            m_eType = RewardType.Clothes;

            int iGarnetId = Rnd.Get(m_aClothesName.Length);

            int iQualityId = Rnd.Get(m_aQuality.Length);
            m_sName = m_aQuality[iQualityId].m_sName;
            m_sNames = m_aQuality[iQualityId].m_sName;
            m_iPrice = m_aQuality[iQualityId].m_iValue;

            if (m_aClothesName[iGarnetId].m_eBodyPart == ClothesBodyPart.Foot)
            {
                int iMaterialId = Rnd.Get(m_aBootsMaterials.Length);
                m_sName += m_aBootsMaterials[iMaterialId].m_sName;
                m_sNames += m_aBootsMaterials[iMaterialId].m_sNames;
                m_iPrice += m_aBootsMaterials[iMaterialId].m_iValue;
            }
            else
            {
                int iMaterialId = Rnd.Get(m_aClothesMaterials.Length);
                m_sName += m_aClothesMaterials[iMaterialId].m_sName;
                m_sNames += m_aClothesMaterials[iMaterialId].m_sNames;
                m_iPrice += m_aClothesMaterials[iMaterialId].m_iValue;
            }

            m_sName += m_aClothesName[iGarnetId].m_sName;
            m_sNames += m_aClothesName[iGarnetId].m_sNames;
            m_iPrice += m_aClothesName[iGarnetId].m_iRank;

            m_pInfo = m_aClothesName[iGarnetId];

            m_iPrice *= 10;
            m_iPrice += Rnd.Get(10);
        }

        public Stat m_eAffectedStat;

        public int m_iBonus;

        public override void Recognize()
        {
            if (m_bRecognized)
                return;

            m_iBonus = 1 + Rnd.Get(m_iPrice / 30);

            //if (m_iBonus > 50)
            //{
            //    m_iBonus = (m_iBonus / 10) * 10;
            //}
            //if (m_iBonus > 10)
            //{
            //    m_iBonus = (m_iBonus / 5) * 5;
            //}

            m_eAffectedStat = Stat.Potency;

            m_sName = "+" + m_iBonus.ToString() + " " + m_sName;
            m_sNames = "+" + m_iBonus.ToString() + " " + m_sNames;

            m_bRecognized = true;
        }

        internal virtual void Write2XML(UniLibXML pXml, XmlNode pRewardNode)
        {
            base.Write2XML(pXml, pRewardNode);

            pXml.AddAttribute(pRewardNode, "bonusStat", m_eAffectedStat);
            pXml.AddAttribute(pRewardNode, "bonus", m_iBonus);
            pXml.AddAttribute(pRewardNode, "subtype", m_pInfo.m_sName);
        }
    }
}
