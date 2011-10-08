﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITAS_Engine.Skills
{
    public class SkillEntertainment: Skill
    {
        private static string[] s_AllExpertises =
        {
            "Singing",
            "Guitar",
            "Piano",
            "Dancing"
        };

        public override string[] AllExpertises
        {
            get { return s_AllExpertises; }
        }
    }
}
