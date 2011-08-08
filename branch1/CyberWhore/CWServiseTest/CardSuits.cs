using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using nsUniLibControls;

namespace CWServiseTest
{
    public enum CardSuit
    {
        Traditional,
        Oral,
        Anal,
        SadoMaso,
        Exhibitionism
    }

    public class CardSuits : IEnumCollection
    {
        private Dictionary<CardSuit, bool> m_cSuits = new Dictionary<CardSuit, bool>();

        #region IEnumCollection Members

        public Array AllEnumValues
        {
            get { return Enum.GetValues(typeof(CardSuit)); }
        }

        public bool IsSelected(object value)
        {
            if (!(value is CardSuit))
                throw new ArgumentException();

            bool bVal = false;

            m_cSuits.TryGetValue((CardSuit)value, out bVal);

            return bVal;
        }

        public void Select(object value, bool check)
        {
            if (!(value is CardSuit))
                throw new ArgumentException();

            m_cSuits[(CardSuit)value] = check;
        }

        #endregion

        public void Clear()
        {
            m_cSuits.Clear();
        }

        public CardSuit[] Selected
        {
            get 
            {
                List<CardSuit> aSuits = new List<CardSuit>();
                foreach (var pSuit in m_cSuits)
                    if (pSuit.Value)
                        aSuits.Add(pSuit.Key);

                return aSuits.ToArray();
            }
        }

        private static string GetShortName(CardSuit eSuit)
        {
            switch (eSuit)
            { 
                case CardSuit.Anal:
                    return "A";
                case CardSuit.Exhibitionism:
                    return "Ex";
                case CardSuit.Oral:
                    return "O";
                case CardSuit.SadoMaso:
                    return "SM";
                case CardSuit.Traditional:
                    return "T";
                default:
                    return "?";
            }
        }

        public string ToShortString()
        {
            string sResult = "";

            foreach (var pSuit in m_cSuits)
            {
                if (pSuit.Value)
                {
                    if (sResult.Length > 0)
                        sResult += ", ";
                    sResult += GetShortName(pSuit.Key);
                }
            }

            if (sResult == "")
                sResult = "-";

            return sResult;
        }

        public override string ToString()
        {
            string sResult = "";

            foreach (var pSuit in m_cSuits)
            {
                if (pSuit.Value)
                {
                    if (sResult.Length > 0)
                        sResult += ", ";
                    sResult += pSuit.Key.ToString();
                }
            }

            if (sResult == "")
                sResult = "empty";

            return sResult;
        }
    }
}

