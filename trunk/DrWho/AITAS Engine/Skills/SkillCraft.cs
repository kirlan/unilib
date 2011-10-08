using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITAS_Engine.Skills
{
    public class SkillCraft: Skill
    {
        private static string[] s_AllExpertises =
        {
            "Building",
            "Woodwork",
            "Metalwork"
        };

        public override string[] AllExpertises
        {
            get { return s_AllExpertises; }
        }
    }
}
