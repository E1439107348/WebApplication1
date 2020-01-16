using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            write1();
            return View();
        }

        /// <summary>
        /// 读取xml文件 1.0
        /// </summary>
        public void ReadXml()
        {
            //读取xml文件
            XmlTextReader xmlRd = new XmlTextReader(@"C:\Users\14391\Downloads\按机构导出文件_C59.F19.P11_浙江省人民政府20200116113952\Table\B01.xml");
            var xml = XDocument.Load(xmlRd);
            var ts = xml.Root.Elements("data").Elements("row");//.First(x => x.Attribute("name").Value == "群众").Attribute("id").Value; 
            var ts22 = ts.Attributes("B0104").First().Value;
        }

        /// <summary>
        /// 写入xml 1.0
        /// </summary>
        public void Write()
        {
            var xmlDoc = new XmlDocument();
            //Create the xml declaration first 
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));
            //Create the root node and append into doc 
            var el = xmlDoc.CreateElement("Contacts");
            xmlDoc.AppendChild(el);
            // Contact 
            XmlElement elementContact = xmlDoc.CreateElement("Contact");
            XmlAttribute attrID = xmlDoc.CreateAttribute("id");
            attrID.Value = "01";
            elementContact.Attributes.Append(attrID);


            XmlAttribute value = xmlDoc.CreateAttribute("value");
            value.Value = "测试";
            elementContact.Attributes.Append(value);
            el.AppendChild(elementContact);
            xmlDoc.Save(@"K:\test1.xml");
        }
        #region 读写xml
        /// <summary>
        /// 写入xml第二种方式 1.0
        /// </summary>
        public void write1()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null));
            //第一节点
            XmlElement xmlRoot = xmlDocument.CreateElement("data1");
            xmlDocument.AppendChild(xmlRoot);
            ////第一子节点 
            XmlElement xmlElementInner01 = xmlDocument.CreateElement("root");
            XmlAttribute xmlAttribute01 = xmlDocument.CreateAttribute("我是键");
            xmlAttribute01.Value = "我是值";
            xmlElementInner01.Attributes.SetNamedItem(xmlAttribute01);
            xmlRoot.AppendChild(xmlElementInner01);



            //第二节点
            XmlElement xmlChild = xmlDocument.CreateElement("data2"); 
            //第二子节点
            XmlElement xmlElementInner = xmlDocument.CreateElement("root");
            XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("我是键");
            xmlAttribute.Value = "我是值";
            xmlElementInner.Attributes.SetNamedItem(xmlAttribute);
            xmlChild.AppendChild(xmlElementInner);
            xmlRoot.AppendChild(xmlChild);








            xmlDocument.Save(@"K:\XMLConfig.xml");
        }
        #endregion

        public static void xml()
        {

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}