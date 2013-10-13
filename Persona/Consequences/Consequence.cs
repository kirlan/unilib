using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nsUniLibXML;
using System.Xml;

namespace Persona.Consequences
{
    public abstract class Consequence
    {
        internal abstract void WriteXML(UniLibXML pXml, XmlNode pConsequenceNode);

        public abstract Consequence Clone();

        internal abstract void Apply(Module pModule);
    }
}
