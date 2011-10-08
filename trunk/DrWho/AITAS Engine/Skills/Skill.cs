using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AITAS_Engine.Skills
{
    public enum Skl
    {
        Athletics,
        Convince,
        Craft,
        Creation,
        Entertainment,
        Fighting,
        Knowledge,
        Marksman,
        Medicine,
        Science,
        Subterfuge,
        Survival,
        Technology,
        Transport
    }

    public abstract class Skill
    {
        public Skl m_eType;

        public int m_iLevel;

        public List<string> m_cExpertise = new List<string>();

        public abstract string[] AllExpertises
        { get; }
    }
}
