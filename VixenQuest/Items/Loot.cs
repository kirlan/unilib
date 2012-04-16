using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random;
using nsUniLibXML;
using System.Xml;
using VixenQuest.World;
using VixenQuest.People;
using VixenQuest.Story;

namespace VixenQuest
{
    public struct ValuedString
    {
        public string m_sName;
        public string m_sNames;
        public int m_iValue;

        public ValuedString(string sName, int iValue)
            :this(sName, sName, iValue)
        { }

        public ValuedString(string sName, string sNames, int iValue)
        {
            m_sName = sName;
            m_sNames = sNames;
            m_iValue = iValue;
        }
    }

    class Loot: Item
    {
        private static ValuedString[] m_aVials = 
        {
            //new ValuedString("Drop", "Drops", 1),
            new ValuedString("Vial", "Vials", 1),
            new ValuedString("Flask", "Flasks", 2),
            //new ValuedString("Small bottle", "Small bottles", 2),
            new ValuedString("Bottle", "Bottles", 3)
        };

        public Loot(Opponent pTarget)
        {
            m_sName = "";
            m_iPrice = 0;
            m_iWeight = 0;

            Fuck(pTarget);
        }

        public Loot(VQAction pAction)
        {
            m_sName = "";

            switch (pAction.m_eType)
            {
                case ActionType.Fucking:
                    Fuck(pAction.m_pTarget);
                    break;
                case ActionType.AssFucking:
                    Fuck(pAction.m_pTarget);
                    break;
                case ActionType.OralFucking:
                    Fuck(pAction.m_pTarget);
                    break;
                //case ActionType.Fucked:
                //    Fuck(pAction.m_pTarget);
                //    break;
                //case ActionType.AssFucked:
                //    Fuck(pAction.m_pTarget);
                //    break;
                //case ActionType.OralFucked:
                //    Fuck(pAction.m_pTarget);
                //    break;
                case ActionType.Sado:
                    Sado(pAction.m_pTarget);
                    break;
            }
        }

        private void Sado(Opponent pTarget)
        {
            string sNameOf = pTarget.SingleName.Trim();
            if (sNameOf.EndsWith("s"))
                sNameOf += "'";
            else
                sNameOf += "'s";

            int part1 = Rnd.Get(m_aVials.Length);
            m_sName = m_aVials[part1].m_sName;
            m_sNames = m_aVials[part1].m_sNames;
            m_iWeight = m_aVials[part1].m_iValue;
            m_iPrice = m_iWeight * pTarget.Level;

            if (Rnd.OneChanceFrom(4))
            {
                m_sName += " of " + sNameOf + " saliva";
                m_sNames += " of " + sNameOf + " saliva";
            }
            else
            {
                if (Rnd.OneChanceFrom(3))
                {
                    m_sName += " of " + sNameOf + " tears";
                    m_sNames += " of " + sNameOf + " tears";
                }
                else
                {
                    if (Rnd.OneChanceFrom(2))
                    {
                        m_sName += " of " + sNameOf + " urine";
                        m_sNames += " of " + sNameOf + " urine";
                    }
                    else
                    {
                        m_sName += " of " + sNameOf + " sweat";
                        m_sNames += " of " + sNameOf + " sweat";
                    }
                }
            }

            m_iPrice *= 5;
        }

        private void Fuck(Opponent pTarget)
        {
            string sNameOf = pTarget.GetDescription();
            if (sNameOf.EndsWith("s"))
                sNameOf += "'";
            else
                sNameOf += "'s";

            int part1 = Rnd.Get(m_aVials.Length);
            m_sName = m_aVials[part1].m_sName;
            m_sNames = m_aVials[part1].m_sNames;
            m_iWeight = m_aVials[part1].m_iValue;
            m_iPrice = m_iWeight * pTarget.Level;

            if (pTarget.Gender == Gender.Male || (pTarget.Gender == Gender.Shemale && Rnd.OneChanceFrom(2)))
            {
                m_sName += " of " + sNameOf + " sperm";
                m_sNames += " of " + sNameOf + " sperm";
            }
            else
            {
                if (Rnd.OneChanceFrom(2))
                {
                    m_sName += " of " + sNameOf + " love juice";
                    m_sNames += " of " + sNameOf + " love juice";
                }
                else
                {
                    string sMilk = " breast milk";
                    if (pTarget.m_pRace.m_eSapience == Sapience.Animal)
                        sMilk = " milk";

                    m_sName += " of " + sNameOf + sMilk;
                    m_sNames += " of " + sNameOf + sMilk;
                }
            }

            m_iPrice *= 5;
        }
    }
}
