using ICSharpCode.SharpZipLib.Zip;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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

            readSql();
            return View();
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
                    throw;
                }
            }

            return View();
        }


        #region 修改中内容

        //创建command对象      
        private MySqlCommand cmd = null;
        //创建connection连接对象  
        private MySqlConnection conn = null;
        //数据库连接字符串  
        String connstr = "server=localhost;port=45017;uid=root;pwd=admin;database=zwhzyq";
        //建立数据库连接  

        public void readSql()
        {
            conn = new MySqlConnection(connstr);
            //需要读取的字段
            List<string> ReadField = new List<string>();

            //从数据库读取字段
            MySqlDataReader reader = null;
            string SQLStr = string.Format("SELECT CODE_TYPE,CODE_NAME,CODE_VALUE,CODE_NAME2,CODE_NAME3 FROM code_value WHERE CODE_TYPE IN ('ZB79','ZB75')");

            var tt = realsql.QueryExecute(SQLStr);
            var ts = "";

        }



        #endregion





        #region 1.读写xml

        /// <summary>
        ///1.1 读取xml文件 1.0
        /// </summary>
        public void ReadXml()
        {
            //读取xml文件
            XmlTextReader xmlRd = new XmlTextReader(@"C:\Users\14391\Downloads\按机构导出文件_C59.F19.P11_浙江省人民政府20200116113952\Table\B01.xml");
            var xml = XDocument.Load(xmlRd);
            var ts = xml.Root.Elements("data").Elements("row");//.First(x => x.Attribute("name").Value == "群众").Attribute("id").Value; 
            var ts22 = ts.Attributes("B0104").First().Value;
        }


        public void ReadXml1()
        {
            var xml = XDocument.Load(Server.MapPath("~/xml/PageIndex.xml"));
            try
            {
                //获取指定的一级节点
                var firstNode = xml.Root.Elements("items").First(x => x.Attribute("name").Value == "01");
                //获取一级节点下的所有子节点
                var nodes = new List<XElement>();
                firstNode.Elements("item").ToList().ForEach(x =>
                {
                    nodes.Add(x);
                    nodes = nodes.Union(x.Elements("item").ToList()).ToList();
                });
                int num = nodes.Count();//所有子节点的数量
                var node = nodes.ElementAt(0);//获取指定子节点
                var key = node.Attribute("Key").Value;//获取专属变量
                var value = node.Attribute("value").Value;


            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 2.1写入xml   单节点模式 1.0
        /// </summary>
        public void WriteOne()
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
            xmlDoc.Save(@"K:\write1.xml");

            //输出格式
            //        <? xml version = "1.0" encoding = "utf-8" ?>
            //< Contacts > 
            //  < Contact id = "01" value = "测试" />
            //   </ Contacts >
        }

        /// <summary>
        /// 2.2写入xml 父节点 ，子节点 第二种方式 1.0
        /// </summary>
        public void WritePs()
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

            xmlDocument.Save(@"K:\write2.xml");


            //写入格式
            //         <? xml version = "1.0" encoding = "UTF-8" ?>
            //< data1 > 
            //  < root 我是键 = "我是值" /> 
            //  < data2 > 
            //    < root 我是键 = "我是值" /> 
            //   </ data2 >
            // </ data1 >
        }


        /// <summary>
        /// 2.3写入xml第二种方式  父子节点（通过父节点判断，获取子节点数据） 1.0
        /// </summary>
        public void writeNodeP()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null));
            //第一节点
            XmlElement xmlRoot = xmlDocument.CreateElement("root");
            xmlDocument.AppendChild(xmlRoot);

            //第二节点
            XmlElement xmlChild = xmlDocument.CreateElement("items");
            XmlAttribute xmlChildKey = xmlDocument.CreateAttribute("name");
            xmlChildKey.Value = "01";
            xmlChild.Attributes.SetNamedItem(xmlChildKey);

            //第二子节点
            XmlElement xmlElementInner = xmlDocument.CreateElement("item");
            //第二子节点 键1
            XmlAttribute node1 = xmlDocument.CreateAttribute("key");
            node1.Value = "key1";
            xmlElementInner.Attributes.SetNamedItem(node1);
            xmlChild.AppendChild(xmlElementInner);

            //第二子节点 键2
            XmlAttribute node2 = xmlDocument.CreateAttribute("value");
            node2.Value = "value";
            xmlElementInner.Attributes.SetNamedItem(node2);
            xmlChild.AppendChild(xmlElementInner);

            xmlRoot.AppendChild(xmlChild);
            xmlDocument.Save(@"K:\write3.xml");


            //输出格式
            //         <? xml version = "1.0" encoding = "UTF-8" ?>
            //< root >
            //  < items name = "01" >
            //    < item key = "key1" value = "value" />
            //     </ items >
            //   </ root >

        }
        #endregion




        #region 2.对压缩文件操作
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
                        FileStream streamWriter = System.IO.File.Create(reportPath + @"\测试路径无" + fileName);


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