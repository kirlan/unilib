using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITAS_Engine.Skills
{
    public class SkillSurvival: Skill
    {
        private static string[] s_AllExpertises =
        {
            "Space",
            "Desert",
            "Swamp",
            "Mountain",
            "Icescape",
            "Underwater",
            "Wilderness"
        };

        public override string[] AllExpertises
        {
            get { return s_AllExpertises; }
        }
    }
}
