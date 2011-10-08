using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITAS_Engine.Skills
{
    public class SkillTransport: Skill
    {
        private static string[] s_AllExpertises =
        {
            "Cars",
            "Trucks",
            "Helicopters",
            "Aircraft",
            "Spaceships",
            "Temporal Ships",
            "Motorcycles"
        };

        public override string[] AllExpertises
        {
            get { return s_AllExpertises; }
        }
    }
}
