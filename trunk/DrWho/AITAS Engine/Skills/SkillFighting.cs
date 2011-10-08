using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITAS_Engine.Skills
{
    public class SkillFighting: Skill
    {
        private static string[] s_AllExpertises =
        {
            "Unarmed",
            "Parry",
            "Block",
            "Sword",
            "Club"
        };

        public override string[] AllExpertises
        {
            get { return s_AllExpertises; }
        }
    }
}
