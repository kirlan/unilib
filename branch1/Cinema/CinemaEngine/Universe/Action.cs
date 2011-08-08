using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using nsUniLibXML;

namespace CinemaEngine
{
    /// <summary>
    /// Действие - несколько человек в одной и той же локации что-то совместно делают
    /// </summary>
    public class Action
    {
        private string m_sName;

        public string Name
        {
            get { return m_sName; }
            set { m_sName = value; }
        }

        private string m_sDescription;

        public string Description
        {
            get { return m_sDescription; }
            set { m_sDescription = value; }
        }

        public int GetRating(GenreTag.Genre eGenre)
        {
            int iRating = 0;
            foreach (GenreTag pTag in m_cTags)
            {
                iRating += pTag.Rating[eGenre];
            }

            if (iRating > GenreTag.MaxRating)
                iRating = GenreTag.MaxRating;

            return iRating;
        }

        public int GetFullRating()
        {
            int iRating = 0;
            foreach (GenreTag pTag in m_cTags)
            {
                iRating += pTag.FullRating;
            }

            return iRating;
        }

        private List<GenreTag> m_cTags = new List<GenreTag>();

        public List<GenreTag> Tags
        {
            get { return m_cTags; }
        }

        private List<Role> m_cRoles = new List<Role>();

        public List<Role> Roles
        {
            get { return m_cRoles; }
        }

        public Action(string[] cTags)
        {
            m_sName = "New Action";
            m_sDescription = "No description yet.";

            foreach (string sTag in cTags)
            {
                if (Universe.Instance.Tags.ContainsKey(sTag))
                    m_cTags.Add(Universe.Instance.Tags[sTag]);
            }

            Role pNewRole = new Role("Actor");
            m_cRoles.Add(pNewRole);
        }

        public Action(Action pOriginal)
        {
            m_sName = pOriginal.m_sName;
            m_sDescription = pOriginal.m_sDescription;

            foreach (GenreTag pTag in pOriginal.m_cTags)
            {
                m_cTags.Add(pTag);
            }

            foreach (Role pRole in pOriginal.m_cRoles)
            {
                m_cRoles.Add(new Role(pRole));
            }
        }

        public void Assign(Action pUpdate)
        {
            m_sName = pUpdate.m_sName;
            m_sDescription = pUpdate.m_sDescription;

            m_cTags.Clear();
            foreach (GenreTag pTag in pUpdate.m_cTags)
            {
                m_cTags.Add(pTag);
            }

            m_cRoles.Clear();
            foreach (Role pRole in pUpdate.m_cRoles)
            {
                m_cRoles.Add(pRole);
            }
        }

        public Action(UniLibXML pXml, XmlNode pActionNode)
        {
            pXml.GetStringAttribute(pActionNode, "name", ref m_sName);
            pXml.GetStringAttribute(pActionNode, "description", ref m_sDescription);

            foreach (XmlNode pSubNode in pActionNode.ChildNodes)
            {
                if (pSubNode.Name == "Tag")
                {
                    string sTag = "";
                    pXml.GetStringAttribute(pSubNode, "name", ref sTag);
                    if(Universe.Instance.Tags.ContainsKey(sTag))
                        m_cTags.Add(Universe.Instance.Tags[sTag]);
                }
                if (pSubNode.Name == "Role")
                {
                    Role pNewRole = new Role(pXml, pSubNode);
                    m_cRoles.Add(pNewRole);
                }
            }
        }

        public void Write(UniLibXML pXml, XmlNode pActionNode)
        {
            pXml.AddAttribute(pActionNode, "name", m_sName);
            pXml.AddAttribute(pActionNode, "description", m_sDescription);

            foreach (GenreTag pTag in m_cTags)
            {
                XmlNode pSubGenreNode = pXml.CreateNode(pActionNode, "Tag");
                pXml.AddAttribute(pSubGenreNode, "name", pTag.Name);
            }

            foreach (Role pRole in m_cRoles)
            {
                XmlNode pRoleNode = pXml.CreateNode(pActionNode, "Role");

                pRole.Write(pXml, pRoleNode);
            }
        }
    }
}
