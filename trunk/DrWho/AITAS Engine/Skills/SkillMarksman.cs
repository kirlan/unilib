using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITAS_Engine.Skills
{
    public class SkillMarksman: Skill
    {
        private static string[] s_AllExpertises =
        {
            "Bow",
            "Pistol",
            "Rifle",
            "Machinegun",
            "Thrown Weapons",
            "Automatic Weapon Systems",
            "Cannon",
            "Plasma weapons",
            "Desintegrators"
        };

        public override string[] AllExpertises
        {
            get { return s_AllExpertises; }
        }
    }
}
