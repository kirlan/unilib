using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITAS_Engine.Skills
{
    public class SkillCreation: Skill
    {
        private static string[] s_AllExpertises =
        {
            "Painting",
            "Sculpturing"
        };

        public override string[] AllExpertises
        {
            get { return s_AllExpertises; }
        }
    }
}
