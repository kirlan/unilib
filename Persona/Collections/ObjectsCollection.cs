using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persona.Parameters;
using nsUniLibXML;
using System.Xml;

namespace Persona.Collections
{
    public class ObjectsCollection
    {
        public override string ToString()
        {
            return m_sName;
        }

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

        public int GetNewID()
        {
            int iMax = 0;

            foreach (var pObject in m_cObjects)
                if (pObject.Key > iMax)
                    iMax = pObject.Key;

            return iMax+1;
        }

        public ObjectsCollection()
        { 
        }

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
                            NumericParameter pParam = new NumericParameter(pXml, pParamNode, m_sName);
                            m_cNumericParameters.Add(pParam);
                        }
                        if (pParamNode.Name == "Bool")
                        {
                            BoolParameter pParam = new BoolParameter(pXml, pParamNode, m_sName);
                            m_cBoolParameters.Add(pParam);
                        }
                        if (pParamNode.Name == "String")
                        {
                            StringParameter pParam = new StringParameter(pXml, pParamNode, m_sName);
                            m_cStringParameters.Add(pParam);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Для сохранения совместимости только! НЕ ИСПОЛЬЗОВАТЬ в новых версиях!
        /// </summary>
        public void ReadObjects(UniLibXML pXml, XmlNode pCollectionNode, List<Parameter> cParams)
        { 
            List<Parameter> cCollectionParams = new List<Parameter>();
            cCollectionParams.AddRange(m_cNumericParameters);
            cCollectionParams.AddRange(m_cBoolParameters);
            cCollectionParams.AddRange(m_cStringParameters); 
            
            foreach (XmlNode pSection in pCollectionNode.ChildNodes)
            {
                if (pSection.Name == "Object")
                {
                    CollectedObject pObject = new CollectedObject(pXml, pSection, cCollectionParams, cParams);
                    m_cObjects[pObject.m_iID] = pObject;
                }
            }
       }

        public int m_iSelectedID = -1;

        public void Select(Module pModule, int iSelection)
        {
            if (m_cObjects.ContainsKey(iSelection))
            {
                m_cObjects[iSelection].Activate(pModule);
                m_iSelectedID = iSelection;
            }
        }

        public void Shuffle(Module pModule)
        {
            List<CollectedObject> cPossible = new List<CollectedObject>();
            foreach (var pObject in m_cObjects)
            {
                bool bPossible = true;
                foreach (var pCondition in pObject.Value.m_cConditions)
                {
                    if (!pCondition.Check())
                    {
                        bPossible = false;
                        break;
                    }
                }

                if (bPossible)
                {
                    for (int i = 0; i < pObject.Value.m_iProbability; i++)
                        cPossible.Add(pObject.Value);
                    //if (iMaxPriority < pObject.Value.m_iPriority)
                    //    iMaxPriority = pObject.Value.m_iPriority;
                }
            }

            CollectedObject pTop = null;
            if (cPossible.Count > 0)
                pTop = cPossible[Random.Rnd.Get(cPossible.Count)];
            else
                pTop = new CollectedObject(-1, this);

            pTop.Activate(pModule);
            m_iSelectedID = pTop.m_iID;
        }

        internal void Init(Module pModule)
        {
            //foreach (var pObject in m_cObjects)
            //    pObject.Value.Activate();

            Shuffle(pModule);
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

            //foreach (var pObject in m_cObjects)
            //{
            //    XmlNode pObjectNode = pXml.CreateNode(pCollectionNode, "Object");
            //    pObject.Value.WriteXML(pXml, pObjectNode);
            //}
        }
    }
}
