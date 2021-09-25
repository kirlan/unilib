using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persona.Parameters;
using nsUniLibXML;
using System.Xml;
using Persona.Consequences;
using Persona.Conditions;

namespace Persona.Collections
{
    public class CollectedObject
    {
        /// <summary>
        /// Номер объекта в коллекции. Не может меняться пользователем.
        /// </summary>
        public int m_iID;

        public string m_sName;

        /// <summary>
        /// <para>Уникальность объекта.</para>
        /// <para>Если true, то объект может быть выбран в коллекции только 1 раз за игровую сессию, 
        /// иначе - неограниченное количество раз. </para>
        /// </summary>
        public bool m_bUnique = false;

        /// <summary>
        /// Список условий, при которых этот объект может быть выбран.
        /// Условия в списке связываются друг с другом логическим И.
        /// </summary>
        public List<Condition> m_cConditions = new List<Condition>();

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
        
        ///// <summary>
        ///// Список числовых параметров, описывающих объект коллекции.
        ///// Числовые параметры могут участвовать в условиях сравнения и попадания в диапазон.
        ///// </summary>
        //public List<NumericParameter> m_cNumericParameters = new List<NumericParameter>();

        ///// <summary>
        ///// Список логических параметров, описывающих объект коллекции.
        ///// Логические параметры могут участвовать в условиях проверки истинности.
        ///// </summary>
        //public List<BoolParameter> m_cBoolParameters = new List<BoolParameter>();

        ///// <summary>
        ///// Список строковых параметров, описывающих объект коллекции.
        ///// Строковые параметры не могут участвовать ни в каких условиях.
        ///// </summary>
        //public List<StringParameter> m_cStringParameters = new List<StringParameter>();

        /// <summary>
        /// Список команд, задающих начальные значения параметров объекта.
        /// </summary>
        public List<ParameterSet> m_cValues = new List<ParameterSet>();

        private void SinchronizeParam(Parameter pParam)
        {
            bool bFound = false;
            foreach (var pCommand in m_cValues)
            {
                if (pCommand.m_pParam.FullName == pParam.FullName)
                {
                    pCommand.m_pParam = pParam;
                    bFound = true;
                    break;
                }
            }

            if (!bFound)
            {
                string sDefaultValue = "";
                if (pParam is NumericParameter)
                    sDefaultValue = ((NumericParameter)pParam).m_fDefaultValue.ToString();
                if (pParam is BoolParameter)
                    sDefaultValue = ((BoolParameter)pParam).m_bDefaultValue.ToString();
                if (pParam is StringParameter)
                    sDefaultValue = ((StringParameter)pParam).m_sDefaultValue.ToString();
                ParameterSet pValueSet = new ParameterSet(pParam, sDefaultValue);
                m_cValues.Add(pValueSet);
            }
        }

        public void Sinchronize(List<NumericParameter> cNumericParameters, List<BoolParameter> cBoolParameters, List<StringParameter> cStringParameters)
        {
            //m_cNumericParameters.Clear();
            foreach (var pParam in cNumericParameters)
            {
                //NumericParameter pNewParam = new NumericParameter(pParam, false);
                //m_cNumericParameters.Add(pNewParam);

                //SinchronizeParam(pNewParam);
                SinchronizeParam(pParam);
            }

            //m_cBoolParameters.Clear();
            foreach (var pParam in cBoolParameters)
            {
                //BoolParameter pNewParam = new BoolParameter(pParam, false);
                //m_cBoolParameters.Add(pNewParam);

                //SinchronizeParam(pNewParam);
                SinchronizeParam(pParam);
            }

            //m_cStringParameters.Clear();
            foreach (var pParam in cStringParameters)
            {
                //StringParameter pNewParam = new StringParameter(pParam, false);
                //m_cStringParameters.Add(pNewParam);

                //SinchronizeParam(pNewParam);
                SinchronizeParam(pParam);
            }

            List<ParameterSet> cDead = new List<ParameterSet>();
            foreach (var pCommand in m_cValues)
            {
                if (pCommand.m_pParam is NumericParameter && !cNumericParameters.Contains(pCommand.m_pParam))
                    cDead.Add(pCommand);
                if (pCommand.m_pParam is BoolParameter && !cBoolParameters.Contains(pCommand.m_pParam))
                    cDead.Add(pCommand);
                if (pCommand.m_pParam is StringParameter && !cStringParameters.Contains(pCommand.m_pParam))
                    cDead.Add(pCommand);
            }

            foreach (var pCommand in cDead)
            {
                m_cValues.Remove(pCommand);
            }
        }

        public CollectedObject(int iID, CollectedObject pOriginal)
        {
            m_iID = iID;

            m_bUnique = pOriginal.m_bUnique;
            m_iProbability = pOriginal.m_iProbability;
            m_sName = pOriginal.m_sName + " (копия)";

            foreach (ParameterSet pValueSet in pOriginal.m_cValues)
                m_cValues.Add((ParameterSet)pValueSet.Clone());

            foreach (Condition pCondition in pOriginal.m_cConditions)
                m_cConditions.Add(pCondition.Clone());
        }

        public CollectedObject(int iID, ObjectsCollection pCollection)
        {
            m_iID = iID;
            m_sName = pCollection.m_sName + " " + m_iID.ToString();

            foreach (var pParam in pCollection.m_cNumericParameters)
            {
                //NumericParameter pNewParam = new NumericParameter(pParam, false);
                //m_cNumericParameters.Add(pNewParam);
                ParameterSet pValueSet = new ParameterSet(pParam, pParam.m_fDefaultValue.ToString());
                m_cValues.Add(pValueSet);
            }

            foreach (var pParam in pCollection.m_cBoolParameters)
            {
                //BoolParameter pNewParam = new BoolParameter(pParam, false);
                //m_cBoolParameters.Add(pNewParam);
                ParameterSet pValueSet = new ParameterSet(pParam, pParam.m_bDefaultValue.ToString());
                m_cValues.Add(pValueSet);
            }

            foreach (var pParam in pCollection.m_cStringParameters)
            {
                //StringParameter pNewParam = new StringParameter(pParam, false);
                //m_cStringParameters.Add(pNewParam);
                ParameterSet pValueSet = new ParameterSet(pParam, pParam.m_sDefaultValue);
                m_cValues.Add(pValueSet);
            }
        }

        /// <summary>
        /// Устарело. Используется только для сохранения совместимости!
        /// </summary>
        /// <param name="pXml"></param>
        /// <param name="pObjectNode"></param>
        /// <param name="cCollectionParams"></param>
        /// <param name="cParams"></param>
        public CollectedObject(UniLibXML pXml, XmlNode pObjectNode, List<Parameter> cCollectionParams, List<Parameter> cParams)
        {
            pXml.GetIntAttribute(pObjectNode, "id", ref m_iID);
            pXml.GetIntAttribute(pObjectNode, "probability", ref m_iProbability);
            pXml.GetBoolAttribute(pObjectNode, "unique", ref m_bUnique);
            pXml.GetStringAttribute(pObjectNode, "name", ref m_sName);

            if (string.IsNullOrEmpty(m_sName))
                m_sName = m_iID.ToString();

            //foreach (Parameter pParam in cCollectionParams)
            //{
            //    if (pParam is NumericParameter)
            //        m_cNumericParameters.Add(new NumericParameter(pParam as NumericParameter, false));
            //    if (pParam is BoolParameter)
            //        m_cBoolParameters.Add(new BoolParameter(pParam as BoolParameter, false));
            //    if (pParam is StringParameter)
            //        m_cStringParameters.Add(new StringParameter(pParam as StringParameter, false));
            //}

            //List<Parameter> cObjectParams = new List<Parameter>();
            //cObjectParams.AddRange(m_cNumericParameters);
            //cObjectParams.AddRange(m_cBoolParameters);
            //cObjectParams.AddRange(m_cStringParameters);

            foreach (XmlNode pSubNode in pObjectNode.ChildNodes)
            {
                if (pSubNode.Name == "Conditions")
                {
                    foreach (XmlNode pConditionNode in pSubNode.ChildNodes)
                    {
                        if (pConditionNode.Name == "Range")
                        {
                            ConditionRange pCondition = new ConditionRange(pXml, pConditionNode, cParams);
                            m_cConditions.Add(pCondition);
                        }
                        if (pConditionNode.Name == "Comparsion")
                        {
                            ConditionComparsion pCondition = new ConditionComparsion(pXml, pConditionNode, cParams);
                            m_cConditions.Add(pCondition);
                        }
                        if (pConditionNode.Name == "Status")
                        {
                            ConditionStatus pCondition = new ConditionStatus(pXml, pConditionNode, cParams);
                            m_cConditions.Add(pCondition);
                        }
                    }
                }
                if (pSubNode.Name == "Value")
                {
                    ParameterSet pValue = new ParameterSet(pXml, pSubNode, cCollectionParams);
                    m_cValues.Add(pValue);
                }
            }
        }

        public CollectedObject(UniLibXML pXml, XmlNode pObjectNode, List<ObjectsCollection> cCollections, List<Parameter> cParams, out ObjectsCollection pCollection)
        {
            string sCollName = "";
            pXml.GetStringAttribute(pObjectNode, "collection", ref sCollName);

            pCollection = null;
            foreach (ObjectsCollection pColl in cCollections)
                if (pColl.m_sName == sCollName)
                {
                    pCollection = pColl;
                    break;
                }

            if (pCollection == null)
                throw new Exception("No collection '" + sCollName + "' found!");

            pXml.GetIntAttribute(pObjectNode, "id", ref m_iID);
            pXml.GetIntAttribute(pObjectNode, "probability", ref m_iProbability);
            pXml.GetBoolAttribute(pObjectNode, "unique", ref m_bUnique);
            pXml.GetStringAttribute(pObjectNode, "name", ref m_sName);

            if (string.IsNullOrEmpty(m_sName))
                m_sName = m_iID.ToString();

            //foreach (Parameter pParam in cCollectionParams)
            //{
            //    if (pParam is NumericParameter)
            //        m_cNumericParameters.Add(new NumericParameter(pParam as NumericParameter, false));
            //    if (pParam is BoolParameter)
            //        m_cBoolParameters.Add(new BoolParameter(pParam as BoolParameter, false));
            //    if (pParam is StringParameter)
            //        m_cStringParameters.Add(new StringParameter(pParam as StringParameter, false));
            //}

            List<Parameter> cObjectParams = new List<Parameter>();
            cObjectParams.AddRange(pCollection.m_cNumericParameters);
            cObjectParams.AddRange(pCollection.m_cBoolParameters);
            cObjectParams.AddRange(pCollection.m_cStringParameters);

            foreach (XmlNode pSubNode in pObjectNode.ChildNodes)
            {
                if (pSubNode.Name == "Conditions")
                {
                    foreach (XmlNode pConditionNode in pSubNode.ChildNodes)
                    {
                        if (pConditionNode.Name == "Range")
                        {
                            ConditionRange pCondition = new ConditionRange(pXml, pConditionNode, cParams);
                            m_cConditions.Add(pCondition);
                        }
                        if (pConditionNode.Name == "Comparsion")
                        {
                            ConditionComparsion pCondition = new ConditionComparsion(pXml, pConditionNode, cParams);
                            m_cConditions.Add(pCondition);
                        }
                        if (pConditionNode.Name == "Status")
                        {
                            ConditionStatus pCondition = new ConditionStatus(pXml, pConditionNode, cParams);
                            m_cConditions.Add(pCondition);
                        }
                        if (pConditionNode.Name == "Selected")
                        {
                            ConditionObjectSelected pCondition = new ConditionObjectSelected(pXml, pConditionNode, cCollections);
                            m_cConditions.Add(pCondition);
                        }
                    }
                }
                if (pSubNode.Name == "Value")
                {
                    ParameterSet pValue = new ParameterSet(pXml, pSubNode, cObjectParams);
                    m_cValues.Add(pValue);
                }
            }
        }

        internal void Activate(Module pModule)
        {
            //foreach (var pParam in m_cNumericParameters)
            //    pParam.Init();
            //foreach (var pParam in m_cBoolParameters)
            //    pParam.Init();
            //foreach (var pParam in m_cStringParameters)
            //    pParam.Init();

            foreach (var pCommand in m_cValues)
                pCommand.Apply(pModule);
        }

        internal void WriteXML(UniLibXML pXml, XmlNode pObjectNode, string sColl)
        {
            pXml.AddAttribute(pObjectNode, "collection", sColl);
            WriteXML(pXml, pObjectNode);
        }

        /// <summary>
        /// Устарело! Напрямую не вызывать!
        /// </summary>
        /// <param name="pXml"></param>
        /// <param name="pObjectNode"></param>
        internal void WriteXML(UniLibXML pXml, XmlNode pObjectNode)
        {
            pXml.AddAttribute(pObjectNode, "id", m_iID);
            pXml.AddAttribute(pObjectNode, "probability", m_iProbability);
            pXml.AddAttribute(pObjectNode, "unique", m_bUnique);
            pXml.AddAttribute(pObjectNode, "name", m_sName);

            XmlNode pConditionsNode = pXml.CreateNode(pObjectNode, "Conditions");
            foreach (Condition pCondition in m_cConditions)
            {
                if (pCondition is ConditionRange)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pConditionsNode, "Range");
                    pCondition.WriteXML(pXml, pConditionNode);
                }
                if (pCondition is ConditionComparsion)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pConditionsNode, "Comparsion");
                    pCondition.WriteXML(pXml, pConditionNode);
                }
                if (pCondition is ConditionStatus)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pConditionsNode, "Status");
                    pCondition.WriteXML(pXml, pConditionNode);
                }
                if (pCondition is ConditionObjectSelected)
                {
                    XmlNode pConditionNode = pXml.CreateNode(pConditionsNode, "Selected");
                    pCondition.WriteXML(pXml, pConditionNode);
                }
            }

            foreach (ParameterSet pCommand in m_cValues)
            {
                XmlNode pValueNode = pXml.CreateNode(pObjectNode, "Value");
                pCommand.WriteXML(pXml, pValueNode);
            }
        }

        public override string ToString()
        {
            return m_sName;
        }
    }
}
