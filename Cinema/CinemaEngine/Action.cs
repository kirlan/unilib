using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using nsUniLibXML;

namespace CinemaEngine
{
    public enum Genre
    { 
        Porn,
        Bizzare,
        Eros,
        Action,
        Comedy,
        Romance
    }

    public class Action
    {
        private string m_sName;

        public string Name
        {
            get { return m_sName; }
            set { m_sName = value; }
        }

        private Genre m_eGenre;

        public Genre Genre
        {
            get { return m_eGenre; }
            set { m_eGenre = value; }
        }

        private List<string> m_cSubGenres = new List<string>();

        public string[] SubGenres
        {
            get { return m_cSubGenres.ToArray(); }
        }

        private List<Role> m_cRoles = new List<Role>();

        public List<Role> Roles
        {
            get { return m_cRoles; }
        }

        public Action(Genre eGenre, string[] cSubGenres)
        {
            m_sName = "New Action";
            m_eGenre = eGenre;

            foreach (string sSubGenre in cSubGenres)
                AddSubGenre(sSubGenre);

            Role pNewRole = new Role("Actor");
            m_cRoles.Add(pNewRole);
        }

        public Action(UniLibXML pXml, XmlNode pActionNode)
        {
            pXml.GetStringAttribute(pActionNode, "name", ref m_sName);
            m_eGenre = (Genre)pXml.GetEnumAttribute(pActionNode, "genre", m_eGenre.GetType());

            foreach (XmlNode pSubNode in pActionNode.ChildNodes)
            {
                if (pSubNode.Name == "SubGenre")
                {
                    string sSubGenre = "";
                    sSubGenre = pXml.GetStringAttribute(pSubNode, "name", ref sSubGenre);
                    m_cSubGenres.Add(sSubGenre);
                }
                if (pSubNode.Name == "Role")
                {
                    Role pNewRole = new Role(pXml, pSubNode);
                    m_cRoles.Add(pNewRole);
                }
            }
        }

        public void AddSubGenre(string sSubGenre)
        {
            if (m_cSubGenres.Contains(sSubGenre))
                return;

            if (!Repository.Instance.SubGenres.Contains(sSubGenre))
                Repository.Instance.SubGenres.Add(sSubGenre);

            m_cSubGenres.Add(sSubGenre);
        }

        public void RemoveSubGenre(string sSubGenre)
        {
            if (!m_cSubGenres.Contains(sSubGenre))
                return;

            m_cSubGenres.Remove(sSubGenre);
        }

        public void RemoveAllSubGenres()
        {
            m_cSubGenres.Clear();
        }

        public void Write(UniLibXML pXml, XmlNode pActionNode)
        {
            pXml.AddAttribute(pActionNode, "name", m_sName);
            pXml.AddAttribute(pActionNode, "genre", m_eGenre);

            foreach (string sSubGenre in m_cSubGenres)
            {
                XmlNode pSubGenreNode = pXml.CreateNode(pActionNode, "SubGenre");
                pXml.AddAttribute(pSubGenreNode, "name", sSubGenre);
            }

            foreach (Role pRole in m_cRoles)
            {
                XmlNode pRoleNode = pXml.CreateNode(pActionNode, "Role");

                pRole.Write(pXml, pRoleNode);
            }
        }
    }
}
