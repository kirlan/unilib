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
        internal abstract void SaveXML(UniLibXML pXml, XmlNode pConsequenceNode);
    }
}
