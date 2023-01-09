using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NameGen;

namespace Socium.Languages
{
    internal class Aztec: Language
    {
        private readonly Confluxer m_pNations;

        public Aztec()
            : base(NameGenerator.Language.Aztec)
        {
            const string sNation = "olmec toltec mixtec aztec maya inca moche chibcha canari totonac huron apache cheroki sioux mohegan iroques inuit huastec mexica nahua quecha aymara chavin cambeba omagua umana kambeba iguana lama muisca narino tairona manteno chimu chincha chancay chucuito qotu huaylas conchucos talan ichma tiwanaku huari quimbaya tolima nazca siches";
            m_pNations = new Confluxer(sNation, 2);
        }

        protected override string GetNationName()
        {
            return m_pNations.Generate();
        }
    }
}
