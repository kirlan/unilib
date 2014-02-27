using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace nsUniLibXML
{
    public class UniLibXML
    {
        private XmlDocument xmlDoc;

        public XmlNode Root
        {
            get { return xmlDoc.DocumentElement; }
        }

        public UniLibXML(string sRootName)
        {
            xmlDoc = new XmlDocument();
            XmlNode pRoot = xmlDoc.CreateNode(XmlNodeType.Element, sRootName, null);

            xmlDoc.AppendChild(pRoot);

            // Create an XML declaration. 
            XmlDeclaration xmldecl;
            xmldecl = xmlDoc.CreateXmlDeclaration("1.0", null, null);
            xmldecl.Encoding = "UTF-8";
            xmldecl.Standalone = "yes";

            // Add the new node to the document.
            xmlDoc.InsertBefore(xmldecl, Root);
        }

        /// <summary>
        /// Считывает структуру из xml-файла.
        /// Если считать невозможно, то возвращает false.
        /// </summary>
        /// <param name="path">полное имя файла</param>
        /// <returns>признак, удалось ли считать файл</returns>
        public bool Load(string path)
        {
            if (!File.Exists(path))
            {
                return false;
            }

            FileStream file = File.OpenRead(path);
            xmlDoc.Load(file);

            file.Close();
            file.Dispose();
            return true;
        }

        /// <summary>
        /// Считывает из xml-ноды атрибут.
        /// Если такого атрибута нет, то возвращается пустая строка.
        /// </summary>
        /// <param name="xmlNode">нода, владеющая атрибутом</param>
        /// <param name="attribute">имя атрибута</param>
        /// <returns>значение атрибута</returns>
        public object GetAttribute(XmlNode xmlNode, string attribute, ref object pResult)
        {
            if (pResult is int)
            {
                int iResult = (int)pResult;
                GetIntAttribute(xmlNode, attribute, ref iResult);
                pResult = iResult;
            }
            if (pResult is float)
            {
                float fResult = (float)pResult;
                GetFloatAttribute(xmlNode, attribute, ref fResult);
                pResult = fResult;
            }
            if (pResult is bool)
            {
                bool bResult = (bool)pResult;
                GetBoolAttribute(xmlNode, attribute, ref bResult);
                pResult = bResult;
            }
            if (pResult is string)
            {
                string sResult = (string)pResult;
                GetStringAttribute(xmlNode, attribute, ref sResult);
                pResult = sResult;
            }
            if (pResult.GetType().IsEnum)
                GetEnumAttribute(xmlNode, attribute, pResult.GetType(), ref pResult);
            return pResult;
        }

        /// <summary>
        /// Считывает из xml-ноды атрибут.
        /// Если такого атрибута нет, то возвращается пустая строка.
        /// </summary>
        /// <param name="xmlNode">нода, владеющая атрибутом</param>
        /// <param name="attribute">имя атрибута</param>
        /// <returns>значение атрибута</returns>
        public string GetStringAttribute(XmlNode xmlNode, string attribute, ref string sResult)
        {
            if (xmlNode.Attributes[attribute] != null)
            {
                sResult = xmlNode.Attributes[attribute].Value;
                return sResult;
            }
            return "";
        }

        /// <summary>
        /// Считывает из xml-ноды атрибут.
        /// Если такого атрибута нет, то возвращается 0.
        /// </summary>
        /// <param name="xmlNode">нода, владеющая атрибутом</param>
        /// <param name="attribute">имя атрибута</param>
        /// <returns>значение атрибута</returns>
        public int GetIntAttribute(XmlNode xmlNode, string attribute, ref int iResult)
        {
            if (xmlNode.Attributes[attribute] != null)
            {
                int.TryParse(xmlNode.Attributes[attribute].Value, out iResult);
                return iResult;
            }
            return 0;
        }

        /// <summary>
        /// Считывает из xml-ноды атрибут.
        /// Если такого атрибута нет, то возвращается 0.
        /// </summary>
        /// <param name="xmlNode">нода, владеющая атрибутом</param>
        /// <param name="attribute">имя атрибута</param>
        /// <returns>значение атрибута</returns>
        public float GetFloatAttribute(XmlNode xmlNode, string attribute, ref float fResult)
        {
            if (xmlNode.Attributes[attribute] != null)
            {
                float.TryParse(xmlNode.Attributes[attribute].Value, out fResult);
                return fResult;
            }
            return 0;
        }

        /// <summary>
        /// Считывает из xml-ноды атрибут.
        /// Если такого атрибута нет, то возвращается false.
        /// </summary>
        /// <param name="xmlNode">нода, владеющая атрибутом</param>
        /// <param name="attribute">имя атрибута</param>
        /// <returns>значение атрибута</returns>
        public bool GetBoolAttribute(XmlNode xmlNode, string attribute, ref bool bResult)
        {
            if (xmlNode.Attributes[attribute] != null)
            {
                bResult = xmlNode.Attributes[attribute].Value == "True" || xmlNode.Attributes[attribute].Value == "true" || xmlNode.Attributes[attribute].Value == "1";
                return bResult;
            }
            return false;
        }

        /// <summary>
        /// Считывает из xml-ноды атрибут.
        /// Если такого атрибута нет, то возвращается null.
        /// </summary>
        /// <param name="xmlNode">нода, владеющая атрибутом</param>
        /// <param name="attribute">имя атрибута</param>
        /// <returns>значение атрибута</returns>
        public object GetEnumAttribute(XmlNode xmlNode, string attribute, Type enumType, ref object eResult)
        {
            if (xmlNode.Attributes[attribute] != null)
            {
                eResult = Enum.Parse(enumType, xmlNode.Attributes[attribute].Value);
                return eResult;
            }
            return null;
        }

        /// <summary>
        /// Считывает из xml-ноды атрибут.
        /// Если такого атрибута нет, то возвращается null.
        /// </summary>
        /// <param name="xmlNode">нода, владеющая атрибутом</param>
        /// <param name="attribute">имя атрибута</param>
        /// <returns>значение атрибута</returns>
        public object GetEnumAttribute(XmlNode xmlNode, string attribute, Type enumType)
        {
            if (xmlNode.Attributes[attribute] != null)
            {
                return Enum.Parse(enumType, xmlNode.Attributes[attribute].Value);
            }
            return Enum.GetValues(enumType).GetValue(0);
        }


        /// <summary>
        /// Записываем структуру в xml-файл
        /// </summary>
        /// <param name="path">полное имя файла</param>
        /// <returns></returns>
        public bool Write(string path)
        {
            FileStream file = File.Create(path);
            xmlDoc.Save(file);

            file.Close();
            file.Dispose();
            return true;
        }

        /// <summary>
        /// Добавляет новый атрибут в xml-ноду
        /// </summary>
        /// <param name="xmlNode">нода, в которую добавляем</param>
        /// <param name="attribute">имя атрибута</param>
        /// <param name="value">значение атрибута</param>
        public void AddAttribute(XmlNode xmlNode, string attribute, object value)
        {
            if (value == null)
                return;

            XmlAttribute xmlAttribute = xmlDoc.CreateAttribute(attribute);
            xmlAttribute.Value = value.ToString();
            xmlNode.Attributes.Append(xmlAttribute);
        }

        /// <summary>
        /// Создаёт новую ноду в xml-документе
        /// </summary>
        /// <param name="xmlNode">родительская нода</param>
        /// <param name="name">имя добавляемой ноды</param>
        /// <returns>созданная нода</returns>
        public XmlNode CreateNode(XmlNode xmlNode, string name)
        {
            XmlNode xmlNewNode = xmlDoc.CreateNode(XmlNodeType.Element, name, null);
            xmlNode.AppendChild(xmlNewNode);

            return xmlNewNode;
        }
    }
}
