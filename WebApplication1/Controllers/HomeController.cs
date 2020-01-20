using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //string TorepotFiles = @"G:\syxWork\工作使用到的文件等等\2020-1-16任务\xml导入导出\按机构导出文件_C59.F19.P11_浙江省人民政府20200119100455.zip";
            //string reportPath = @"G:\syxWork\工作使用到的文件等等\2020-1-16任务\xml导入导出\备份\新建文件夹";
            //UnzipTheFiles(TorepotFiles, reportPath);
            using (MYDBContext db = new MYDBContext())
            {

                try
                {



                    //string SQLStr = string.Format("insert into   UserTS VALUES('{0}', '{1}')", Guid.NewGuid().ToString(), DateTime.Now.ToString());


                    string SQLStr = string.Format("INSERT INTO code_value(CODE_VALUE_SEQ, CODE_TYPE, CODE_COLUMN_NAME, CODE_LEVEL, CODE_VALUE, ININO, SUB_CODE_VALUE, CODE_NAME, CODE_NAME2, CODE_NAME3, CODE_REMARK, CODE_SPELLING, ISCUSTOMIZE, CODE_ASSIST, CODE_STATUS, CODE_LEAF, START_DATE, STOP_DATE) VALUES (9999, 'ZB79', NULL, NULL, '1', NULL, '-1', '领导班子换届考察1', '1', '领导班子换届考察1', NULL, 'LDBZHJKC', '0', NULL, '1', '1', NULL, NULL)");
                    db.Database.ExecuteSqlCommand(SQLStr);
                }
                catch (Exception ex) 
                
                {
                    var ts = ex.ToString();
                    throw; }
            }

            return View();
        }

        /// <summary>
        /// 解压文件 1.01
        /// </summary>
        ///<param name = "ZipPath" > 需要被解压的文件 </ param >
        /// <param name="Path">解压后文件的路径</param>
        public string UnzipTheFiles(string TorepotFiles, string reportPath)
        {
            
            ZipInputStream s = new ZipInputStream(System.IO.File.OpenRead(TorepotFiles));

            ZipEntry theEntry;
            try
            {
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string fileName = System.IO.Path.GetFileName(theEntry.Name);
                    //生成解压目录
                    Directory.CreateDirectory(reportPath);

                    if (fileName != String.Empty)
                    {
                        //解压文件
                        FileStream streamWriter = System.IO.File.Create(reportPath + @"\测试路径无"+fileName);
                       

                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {

                                streamWriter.Close();
                                streamWriter.Dispose();
                                break;
                            }
                        }

                        streamWriter.Close();
                        streamWriter.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
              
                throw ex;
            }
            finally
            {
                s.Close();
                s.Dispose();
            }

            return reportPath;
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