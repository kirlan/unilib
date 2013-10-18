using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persona.Parameters;
using nsUniLibXML;
using System.Xml;

namespace Persona
{
    public class ObjectsCollection
    {
        /// <summary>
        /// Название коллекции, например - "Спутники", "Оружие", "Награды"...
        /// </summary>
        public string m_sName = "Новая коллекция";
        
        /// <summary>
        /// Список числовых параметров, описывающих объект коллекции.
        /// Числовые параметры могут участвовать в условиях сравнения и попадания в диапазон.
        /// </summary>
        public List<NumericParameter> m_cNumericParameters = new List<NumericParameter>();

        /// <summary>
        /// Список логических параметров, описывающих объект коллекции.
        /// Логические параметры могут участвовать в условиях проверки истинности.
        /// </summary>
        public List<BoolParameter> m_cBoolParameters = new List<BoolParameter>();

        /// <summary>
        /// Список строковых параметров, описывающих объект коллекции.
        /// Строковые параметры не могут участвовать ни в каких условиях.
        /// </summary>
        public List<StringParameter> m_cStringParameters = new List<StringParameter>();

        /// <summary>
        /// Список объектов, составляющих коллекцию.
        /// </summary>
        public Dictionary<int, CollectedObject> m_cObjects = new Dictionary<int, CollectedObject>();

        public ObjectsCollection(UniLibXML pXml, XmlNode pCollectionNode)
        {
            pXml.GetStringAttribute(pCollectionNode, "name", ref m_sName);

            foreach (XmlNode pSection in pCollectionNode.ChildNodes)
            {
                if (pSection.Name == "Parameters")
                {
                    foreach (XmlNode pParamNode in pSection.ChildNodes)
                    {
                        if (pParamNode.Name == "Numeric")
                        {
                            NumericParameter pParam = new NumericParameter(pXml, pParamNode);
                            m_cNumericParameters.Add(pParam);
                        }
                        if (pParamNode.Name == "Bool")
                        {
                            BoolParameter pParam = new BoolParameter(pXml, pParamNode);
                            m_cBoolParameters.Add(pParam);
                        }
                        if (pParamNode.Name == "String")
                        {
                            StringParameter pParam = new StringParameter(pXml, pParamNode);
                            m_cStringParameters.Add(pParam);
                        }
                    }
                }
            }
            
            List<Parameter> cParams = new List<Parameter>();
            cParams.AddRange(m_cNumericParameters);
            cParams.AddRange(m_cBoolParameters);
            cParams.AddRange(m_cStringParameters); 
            
            foreach (XmlNode pSection in pCollectionNode.ChildNodes)
            {
                if (pSection.Name == "Object")
                {
                    CollectedObject pObject = new CollectedObject(pXml, pSection, cParams);
                    m_cObjects[pObject.m_iID] = pObject;
                }
            }
        }

        internal void Init()
        {
            foreach (var pObject in m_cObjects)
                pObject.Value.Init();
        }

        internal void WriteXML(UniLibXML pXml, XmlNode pCollectionNode)
        {
            pXml.AddAttribute(pCollectionNode, "name", m_sName);

            XmlNode pParameters = pXml.CreateNode(pCollectionNode, "Parameters");
            foreach (NumericParameter pParam in m_cNumericParameters)
            {
                XmlNode pParamNode = pXml.CreateNode(pParameters, "Numeric");
                pParam.WriteXML(pXml, pParamNode);
            }
            foreach (BoolParameter pParam in m_cBoolParameters)
            {
                XmlNode pParamNode = pXml.CreateNode(pParameters, "Bool");
                pParam.WriteXML(pXml, pParamNode);
            }
            foreach (StringParameter pParam in m_cStringParameters)
            {
                XmlNode pParamNode = pXml.CreateNode(pParameters, "String");
                pParam.WriteXML(pXml, pParamNode);
            }

            foreach (var pObject in m_cObjects)
            {
                XmlNode pObjectNode = pXml.CreateNode(pCollectionNode, "Object");
                pObject.Value.WriteXML(pXml, pObjectNode);
            }
        }
    }
}
