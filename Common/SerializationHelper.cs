using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Common
{
    public class SerializationHelper
    {
        /// <summary>
        /// 将对象序列化为xml字符串
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string SerializerObj2XmlString(object o)
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(o.GetType());
                System.IO.MemoryStream mem = new MemoryStream();
                XmlTextWriter writer = new XmlTextWriter(mem, Encoding.UTF8);
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                ser.Serialize(writer, o, ns);
                writer.Close();
                return Encoding.UTF8.GetString(mem.ToArray());
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex, "SerializerObj2XmlString");
                return string.Empty;
            }
        }

        /// <summary>
        /// 将xml字符串反序列化为实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T DeserializeXmlString2Obj<T>(string s)
        {
            XmlDocument xdoc = new XmlDocument();
            try
            {
                xdoc.LoadXml(s);
                XmlNodeReader reader = new XmlNodeReader(xdoc.DocumentElement);
                XmlSerializer ser = new XmlSerializer(typeof(T));
                object obj = ser.Deserialize(reader);
                reader.Close();
                return (T)obj;
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex, "DeserializeXmlString2Obj");
                return default(T);
            }
        }

        /// <summary>
        /// 将实体对象序列化为Xml文件
        /// </summary>
        /// <param name="o"></param>
        /// <param name="fileName">Xml文件名称</param>
        public static void SerializerObj2XmlFile(object o, string fileName)
        {
            try
            {
                using (FileStream stream = new FileStream(fileName, FileMode.Create))
                {
                    XmlSerializer serializer = new XmlSerializer(o.GetType());
                    serializer.Serialize(stream, o);
                }
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex, "SerializerObj2XmlFile");
            }
        }

        /// <summary>
        /// 将指定Xml文件序列化为实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static T DeserializeXmlFile2Obj<T>(string fileName)
        {
            T obj = Activator.CreateInstance<T>();
            try
            {
                using (FileStream stream = new FileStream(fileName, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex, "DeserializeXmlFile2Obj");
                return obj;
            }
        }


        /// <summary>
        /// 将实体对象序列化为Json
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string SerializeObj2Json(object o)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    new DataContractJsonSerializer(o.GetType()).WriteObject(ms, o);
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex, "SerializeObj2Json");
                return string.Empty;
            }
        }

        /// <summary>
        /// 将Json反序列化为实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T DeserializeJson2Obj<T>(string s)
        {
            T obj = Activator.CreateInstance<T>();
            try
            {
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(s)))
                {
                    return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(ms);
                }
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex, "DeserializeJson2Obj");
                return obj;
            }
        }
    }
}
