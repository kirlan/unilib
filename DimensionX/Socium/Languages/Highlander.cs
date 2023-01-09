using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NameGen;

namespace Socium.Languages
{
    internal class Highlander:Language
    {
        private readonly Confluxer m_pFamily;

        public Highlander()
            : base(NameGenerator.Language.Scotish)
        {
            const string sFamily = "maclay macbiset macboyd macboyl macbroud macbrown macburnet macdonald macruar macraig macford macdaron macdewar macdouglas macdunbar macerskin macfenton macfraser mackened maclanont maclockhart maclogan macalister macaulay macbain macdonald macdonel macfly mackay mackenzie mackinon macintosh maclaren maclean macleon macmilan macnab macneil macrae macgil macmalcolm macmaxton macmaxwel macmofat macmunro macnapier macnesbit";
            m_pFamily = new Confluxer(sFamily, 2);
        }

        protected override string GetFamily()
        {
            return m_pFamily.Generate();
        }
    }
}
