#region Header and Copyright

// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) Felipe Vieira Vendramini - All rights reserved
// The copy or distribution of this file or software without the original lines of this header is extrictly
// forbidden. This code is public and free as is, and if you alter anything you can insert your name
// in the fields below.
// 
// AutoUpdater - AutoUpdaterCore - MyXml.cs
// 
// Description: <Write a description for this file>
// 
// Colaborators who worked in this file:
// Felipe Vieira Vendramini
// 
// Developed by:
// Felipe Vieira Vendramini <service@ftwmasters.com.br>
// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System.IO;
using System.Text;
using System.Xml;

namespace AutoUpdaterCore
{
    public sealed class MyXml
    {
        private XmlDocument m_xml = new XmlDocument();
        private string m_path = "";

        public MyXml(string path)
        {
            if (!File.Exists(path))
            {
                XmlTextWriter writer = new XmlTextWriter(path, Encoding.UTF8)
                {
                    Formatting = Formatting.Indented
                };
                writer.WriteStartDocument();
                writer.WriteStartElement("Config");
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
            }

            m_path = path;
            m_xml.Load(path);
        }

        public void AddNewNode(string value, string node, params string[] xpath)
        {
            if (!CheckNodeExists(xpath))
                return; // must create one by one

            if (CheckNodeExists(TransformXPath(xpath), node))
            {
                ChangeValue(value, TransformXPath(xpath), node);
                return;
            }

            XmlNode appendTo = GetNode(xpath);
            XmlNode newNode = m_xml.CreateNode(XmlNodeType.Element, node, "");
            newNode.InnerText = value;
            appendTo.AppendChild(newNode);
            m_xml.Save(m_path);
        }

        public void DeleteNode(params string[] xpath)
        {
            DeleteNode(m_xml.SelectSingleNode(TransformXPath(xpath)));
        }

        private void DeleteNode(XmlNode node)
        {
            if (node == null)
                return;

            XmlNode parent = node.ParentNode;
            parent?.RemoveChild(node);
            m_xml.Save(m_path);
        }

        public XmlNode GetNode(params string[] xpath)
        {
            return m_xml.SelectSingleNode(TransformXPath(xpath));
        }

        public bool ChangeValue(string newValue, params string[] xpath)
        {
            try
            {
                m_xml.SelectSingleNode(TransformXPath(xpath)).InnerText = newValue;
                m_xml.Save(m_path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetValue(params string[] xpath)
        {
            if (!CheckNodeExists(xpath))
                return "";
            return GetNode(xpath)?.InnerText ?? "";
        }

        public bool CheckNodeExists(params string[] xpath)
        {
            return m_xml.SelectSingleNode(TransformXPath(xpath)) != null;
        }

        private string TransformXPath(params string[] xpath)
        {
            string path = "";
            foreach (var str in xpath)
                path += $"/{str}";
            return path;
        }
    }
}