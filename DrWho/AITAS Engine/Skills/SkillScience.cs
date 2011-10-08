using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITAS_Engine.Skills
{
    public class SkillScience: Skill
    {
        private static string[] s_AllExpertises =
        {
            "Mathematics",
            "Phisics",
            "Chemistry",
            "Biology",
            "Astrophisics"
        };

        public override string[] AllExpertises
        {
            get { return s_AllExpertises; }
        }
    }
}
