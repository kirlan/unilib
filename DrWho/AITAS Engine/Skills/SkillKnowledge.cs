using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITAS_Engine.Skills
{
    public class SkillKnowledge: Skill
    {
        private static string[] s_AllExpertises =
        {
            "Earth History",
            "Earth Law",
            "Psychology",
            "Earth Languages",
            "Earth Literature",
            "Sociology",
            "Alien Cultures",
            "Earth Economics",
            "Skaro History",
            "Gallifrey",
            "The Dark Times"
        };

        public override string[] AllExpertises
        {
            get { return s_AllExpertises; }
        }
    }
}
