using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITAS_Engine.Skills
{
    public class SkillAthletics: Skill
    {
        private static string[] s_AllExpertises =
        {
            "Running",
            "Jumping",
            "Riding",
            "Climbing",
            "Parachuting",
            "Scuba",
            "Swimming"
        };

        public override string[] AllExpertises
        {
            get { return s_AllExpertises; }
        }
    }
}
