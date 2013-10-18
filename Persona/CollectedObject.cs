using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persona.Parameters;
using nsUniLibXML;
using System.Xml;
using Persona.Consequences;

namespace Persona
{
    public class CollectedObject
    {
        /// <summary>
        /// Номер объекта в коллекции. Не может меняться пользователем.
        /// </summary>
        public int m_iID;

        /// <summary>
        /// <para>Множитель вероятности выборки объекта из коллекции.</para>
        /// <para>При значении 1 (по умолчанию) объект имеет шансы выпасть равные со всеми другим объектами
        /// в этой же коллекции, которые удовлетворяют условиям выборки.</para>
        /// <para>При значении меньше или равно 0 - объект не будет выбран никогда, можно использовать 
        /// при тестировании модуля для временного отключения определённых объектов.</para>
        /// <para>При значении больше 1 шансы увеличиваются в соответствующее число раз. Т.е. при выборке объект 
        /// как бы дублируется в коллекции соответствующее число раз.</para>
        /// </summary>
        public int m_iProbability = 1;
        
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
        /// Список команд, задающих начальные значения параметров объекта.
        /// </summary>
        public List<ParameterSet> m_cValues = new List<ParameterSet>();

        public CollectedObject(UniLibXML pXml, XmlNode pObjectNode, List<Parameter> cCollectionParams)
        {
            pXml.GetIntAttribute(pObjectNode, "id", ref m_iID);
            pXml.GetIntAttribute(pObjectNode, "probability", ref m_iProbability);

            foreach (Parameter pParam in cCollectionParams)
            {
                if (pParam is NumericParameter)
                    m_cNumericParameters.Add(new NumericParameter(pParam as NumericParameter));
                if (pParam is BoolParameter)
                    m_cBoolParameters.Add(new BoolParameter(pParam as BoolParameter));
                if (pParam is StringParameter)
                    m_cStringParameters.Add(new StringParameter(pParam as StringParameter));
            }
            
            List<Parameter> cParams = new List<Parameter>();
            cParams.AddRange(m_cNumericParameters);
            cParams.AddRange(m_cBoolParameters);
            cParams.AddRange(m_cStringParameters); 

            foreach (XmlNode pValueNode in pObjectNode.ChildNodes)
            {
                if (pValueNode.Name == "Value")
                {
                    ParameterSet pValue = new ParameterSet(pXml, pValueNode, cParams);
                    m_cValues.Add(pValue);
                }
            }
        }

        internal void Init()
        {
            foreach (var pParam in m_cNumericParameters)
                pParam.Init();
            foreach (var pParam in m_cBoolParameters)
                pParam.Init();
            foreach (var pParam in m_cStringParameters)
                pParam.Init();

            foreach (var pCommand in m_cValues)
                pCommand.Apply(null);
        }

        internal void WriteXML(UniLibXML pXml, XmlNode pObjectNode)
        {
            pXml.AddAttribute(pObjectNode, "id", m_iID);
            pXml.AddAttribute(pObjectNode, "probability", m_iProbability);

            foreach (ParameterSet pCommand in m_cValues)
            {
                XmlNode pValueNode = pXml.CreateNode(pObjectNode, "Value");
                pCommand.WriteXML(pXml, pValueNode);
            }
        }
    }
}
