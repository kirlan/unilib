using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS30Conf
{
    public enum MaidStats
    {
        FatiqueBase,
        Fatique,
        FatiqueModifier,
        StressBase,
        Stress,
        StressModifier,
        LibidoBase,
        Libido,
        LibidoModifier
    }

    public class CMaidStat: CConfigString
    {
        private MaidStats m_eType;

        public MaidStats Type
        {
            get { return m_eType; }
        }

        public CMaidStat(MaidStats eType, string sName)
            : base(StringCategory.STAT, Enum.GetName(typeof(MaidStats), eType))
        {
            m_eType = eType;
            m_sName = sName;
        }

        public override string GetCfgString()
        {
            return string.Format("{0}", base.GetCfgString());
        }

        public override void Parse(CConfigParser pParser)
        {
            base.Parse(pParser);
            m_eType = (MaidStats)Enum.Parse(typeof(MaidStats), ID.Remove(0, 4));
        }
    }
}
