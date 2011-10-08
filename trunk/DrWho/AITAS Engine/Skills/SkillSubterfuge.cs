using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITAS_Engine.Skills
{
    public class SkillSubterfuge: Skill
    {
        private static string[] s_AllExpertises =
        {
            "Sneaking",
            "Lockpicking",
            "Sleight of Hand",
            "Pickpocketing",
            "Safecracking",
            "Camouflage"
        };

        public override string[] AllExpertises
        {
            get { return s_AllExpertises; }
        }
    }
}
