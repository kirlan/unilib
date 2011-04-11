using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nsUniLibXML;
using System.Xml;
using ReadOnlyDictionary;

namespace CinemaEngine
{
    /// <summary>
    /// Категория действия. Указывает соответсвие действия различным жанрам.
    /// </summary>
    public class GenreTag
    {
        public enum Genre
        {
            Unrated,
            Action,
            Gore,
            Mistery,
            Fun,
            Romance,
            Horror,
            XXX
        }

        private string m_sName;

        public string Name
        {
            get { return m_sName; }
            set 
            { 
                if(Universe.Instance.RenameTag(this, value))
                    m_sName = value; 
            }
        }

        private Dictionary<Genre, int> m_cRating = new Dictionary<Genre, int>();

        public ReadOnlyDictionary<Genre, int> Rating
        {
            get { return new ReadOnlyDictionary<Genre,int>(m_cRating); }
        }

        public void SetRating(Genre eGenre, int iRating)
        {
            m_cRating[eGenre] = iRating;

            if (s_iMaxRating < iRating)
                s_iMaxRating = iRating;

            m_iFullRating = 0;
            foreach (int iRtng in m_cRating.Values)
            {
                m_iFullRating += iRtng;
            }
        }

        private int m_iFullRating = 0;

        public int FullRating
        {
            get { return m_iFullRating; }
        }

        private static int s_iMaxRating = 0;

        public static int MaxRating
        {
            get { return 5;}// s_iMaxRating; }
        }

        public GenreTag()
        {
            foreach (Genre eGenre in Enum.GetValues(typeof(Genre)))
            {
                m_cRating[eGenre] = 0;
            }
        }

        public GenreTag(UniLibXML pXml, XmlNode pTagNode)
            : this()
        {
            pXml.GetStringAttribute(pTagNode, "name", ref m_sName);

            foreach (XmlNode pSubNode in pTagNode.ChildNodes)
            {
                if (pSubNode.Name == "Rating")
                {
                    Genre eGenre = Genre.Action;
                    eGenre = (Genre)pXml.GetEnumAttribute(pSubNode, "genre", eGenre.GetType());
                    int iRating = 0;
                    pXml.GetIntAttribute(pSubNode, "rating", ref iRating);

                    SetRating(eGenre, iRating);
                }
            }
        }

        internal void Write(UniLibXML pXml, XmlNode pTagNode)
        {
            pXml.AddAttribute(pTagNode, "name", m_sName);

            foreach (Genre eGenre in m_cRating.Keys)
            {
                XmlNode pRatingNode = pXml.CreateNode(pTagNode, "Rating");
                pXml.AddAttribute(pRatingNode, "genre", eGenre);
                pXml.AddAttribute(pRatingNode, "rating", m_cRating[eGenre]);
            }
        }
    }
}
