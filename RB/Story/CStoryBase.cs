using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RB.Story
{
    public interface IStoryBase
    {
        string Description { get; set; }
        string Name { get; set; }
    }


    public class CStoryBase : IStoryBase
    {
        protected string m_sName;
        /// <summary>
        /// Короткое имя
        /// </summary>
        public virtual string Name
        {
            get { return m_sName; }
            set { m_sName = value; }
        }

        protected string m_sDescription;
        /// <summary>
        /// Литературное описание начала истории
        /// </summary>
       public string Description
        {
            get { return m_sDescription; }
            set { m_sDescription = value; }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
