using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ExpatManager.Helper
{
    static class XMLSerializationHelper
    {
        public static string XmlSerialize(object obj)
        {
            if (null != obj)
            {
                XmlSerializer ser = new XmlSerializer(obj.GetType());
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                System.IO.StreamWriter writer = new System.IO.StreamWriter(sb.ToString());
                ser.Serialize(writer, obj);
                return sb.ToString();
            }
            return string.Empty;
        
        }

        public static object XmlDeserialize(Type objtype, string xmlDoc)
        {
            if (xmlDoc != null && objtype != null)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlDoc);

                XmlNodeReader reader = new XmlNodeReader(doc.DocumentElement);
                XmlSerializer ser = new XmlSerializer(objtype);
                return ser.Deserialize(reader);

            }
            return null;
        }
    }
}