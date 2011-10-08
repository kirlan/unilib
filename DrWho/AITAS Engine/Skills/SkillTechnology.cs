using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITAS_Engine.Skills
{
    public class SkillTechnology: Skill
    {
        private static string[] s_AllExpertises =
        {
            "Computers",
            "Electronics",
            "Gadgetry",
            "Hacking",
            "Repair",
            "TARDIS"
        };

        public override string[] AllExpertises
        {
            get { return s_AllExpertises; }
        }
    }
}
