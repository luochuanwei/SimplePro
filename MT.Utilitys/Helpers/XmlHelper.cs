using System;
using System.Xml;

namespace MT.LQQ.Utilitys.Helpers
{
    public class XmlHelper
    {
        #region filed
        /// <summary>
        /// XML string
        /// </summary>
        private string _XmlString = string.Empty;
        /// <summary>
        /// Xml document 
        /// </summary>
        private XmlDocument _XmlDocument;
        /// <summary>
        /// XML node
        /// </summary>
        private XmlElement _XmlElement;

        #endregion

        #region Attribute

        public XmlNode GetNode(string xPath)
        {
            return _XmlElement.SelectSingleNode(xPath);
        }

        public XmlElement RootNode
        {
            get
            {
                return _XmlElement;
            }
        }

        public XmlNodeList ChildNodes
        {
            get
            {
                return _XmlElement.ChildNodes;
            }
        }

        #endregion

        #region constructer
        public XmlHelper(string xmlString)
        {
            if (string.IsNullOrEmpty(xmlString))
                throw new ArgumentNullException("xmlString", "input no xml string");
            this._XmlString = xmlString;
            this.CreateXMLElement();
        }

        #endregion

        #region function

        private void CreateXMLElement()
        {
            this._XmlDocument = new XmlDocument();
            _XmlDocument.LoadXml(this._XmlString);
            _XmlElement = _XmlDocument.DocumentElement;
        }

        public string GetValue(string xPath)
        {
            return _XmlElement.SelectSingleNode(xPath).InnerText;
        }

        public string GetValue(XmlNode node, string path)
        {
            return node.SelectSingleNode(path).InnerText;
        }

        public string GetAttributeValue(string xPath, string attributeName)
        {
            return _XmlElement.SelectSingleNode(xPath).Attributes[attributeName].Value;
        }

        public string GetAttributeValue(XmlNode node, string attributeName)
        {
            if (node?.Attributes == null)
            {
                return string.Empty;
            }
            var attribute = node.Attributes[attributeName];
            return attribute == null ? string.Empty : attribute.Value;

        }

        public void AppendNode(XmlNode xmlNode)
        {
            XmlNode node = _XmlDocument.ImportNode(xmlNode, true);

            _XmlElement.AppendChild(node);
        }

        public void RemoveNode(string xPath)
        {
            XmlNode node = _XmlDocument.SelectSingleNode(xPath);

            _XmlElement.RemoveChild(node);
        }

        #endregion

        #region static function

        private static XmlElement CreateRootElement(string xmlString)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlString);

            return xmlDocument.DocumentElement;
        }

        public static string GetValue(string xmlFilePath, string xPath)
        {
            XmlElement rootElement = CreateRootElement(xmlFilePath);

            return rootElement.SelectSingleNode(xPath).InnerText;
        }

        public static string GetAttributeValue(string xmlFilePath, string xPath, string attributeName)
        {
            XmlElement rootElement = CreateRootElement(xmlFilePath);

            return rootElement.SelectSingleNode(xPath).Attributes[attributeName].Value;
        }

        #endregion
    }
}