using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITAS_Engine.Skills
{
    public class SkillMedicine: Skill
    {
        private static string[] s_AllExpertises =
        {
            "Disease",
            "Wounds",
            "Poisons",
            "Psychotherapy",
            "Surgery",
            "Forensics",
            "Veterinary",
        };

        public override string[] AllExpertises
        {
            get { return s_AllExpertises; }
        }
    }
}
