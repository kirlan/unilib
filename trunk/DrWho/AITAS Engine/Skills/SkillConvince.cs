using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITAS_Engine.Skills
{
    public class SkillConvince: Skill
    {
        private static string[] s_AllExpertises =
        {
            "Fast Talk",
            "Bluff",
            "Leadership",
            "Seduction",
            "Interrogation",
            "Charm",
            "Lie",
            "Talk Down"
        };

        public override string[] AllExpertises
        {
            get { return s_AllExpertises; }
        }
    }
}
