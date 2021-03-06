﻿using ICSharpCode.SharpZipLib.Zip;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            GetImages(@"G:\壁纸s\TsImageDel");
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


        #region  0.mysql中获取xml内容

        //创建command对象      
        private MySqlCommand cmd = null;
        //创建connection连接对象  
        private MySqlConnection conn = null;
        //数据库连接字符串  
        String connstr = "server=localhost;port=45017;uid=root;pwd=admin;database=zwhzyq";
        //建立数据库连接  

        public void readSql()
        {
            string WhereSql = "'ZB01','ZB87','ZB03','ZB03','ZB04','GB2261','GB3304','GB2261D','GB4762','GB4762','GB4762','ZB125','ZB126','ZB130','XZ93','ZB09','ZB148','ZB134','ZB135','ZB139','ZB129','ZB122','ZB14','ZB09','ZB133','ZB14','GB8561','ZB24','ZB64','GB6864','GB16835','ZB123','ZB65','ZB67'," +
                "'ZB03','ZB09','ZB128','ZB18','GB4761','GB4762'";

            conn = new MySqlConnection(connstr);
            //从数据库读取字段
            MySqlDataReader reader = null;
            string SQLStr = string.Format("SELECT CODE_TYPE,CODE_NAME,CODE_VALUE,CODE_NAME2,CODE_NAME3 FROM code_value WHERE CODE_TYPE IN ({0})", WhereSql);
            //获取数据
            List<code_value> ListData = realsql.QueryExecute(SQLStr);
            #region  //操作excel

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null));

            //第一节点
            XmlElement xmlRoot = xmlDocument.CreateElement("root");
            xmlDocument.AppendChild(xmlRoot);


            //第一次创建的二级节点
            string Second = "";
            string Contrast = "";
            XmlElement xmlChild = null;
            foreach (var item in ListData)
            {

                Second = item.CODE_TYPE;
                if (Second != Contrast)
                {
                    //第二节点
                    xmlChild = xmlDocument.CreateElement("items");
                    XmlAttribute xmlChildKey = xmlDocument.CreateAttribute("name");
                    xmlChildKey.Value = item.CODE_TYPE;//总标题
                    xmlChild.Attributes.SetNamedItem(xmlChildKey);
                    Contrast = item.CODE_TYPE;
                }

                //第二子节点
                XmlElement xmlElementInner = xmlDocument.CreateElement("item");
                //第二子节点 key key值
                XmlAttribute node1 = xmlDocument.CreateAttribute("key");
                node1.Value = item.CODE_VALUE;
                xmlElementInner.Attributes.SetNamedItem(node1);
                xmlChild.AppendChild(xmlElementInner);

                //第二子节点 键1  value值
                XmlAttribute node2 = xmlDocument.CreateAttribute("value");
                node2.Value = item.CODE_NAME;
                xmlElementInner.Attributes.SetNamedItem(node2);
                xmlChild.AppendChild(xmlElementInner);


                //第三子节点 键2  value值
                XmlAttribute node3 = xmlDocument.CreateAttribute("value2");
                node3.Value = item.CODE_NAME2;
                xmlElementInner.Attributes.SetNamedItem(node3);
                xmlChild.AppendChild(xmlElementInner);


                //第四子节点 键3  value值
                XmlAttribute node4 = xmlDocument.CreateAttribute("value3");
                node4.Value = item.CODE_NAME3;
                xmlElementInner.Attributes.SetNamedItem(node4);
                xmlChild.AppendChild(xmlElementInner);

                xmlRoot.AppendChild(xmlChild);
            }

            xmlDocument.Save(@"K:\Getxml.xml");
            #endregion


            string ts = "测试点";
        }



        #endregion

        #region 1.读写xml

        /// <summary>
        ///1.1 读取xml文件 1.0
        /// </summary>
        public void ReadXml()
        {
            //读取xml文件
            XmlTextReader xmlRd = new XmlTextReader(@"G:\syxWork\工作使用到的文件等等\2020-1-16任务\xml导入导出\02.重要文件\按机构导出文件_C59.F19.P11_浙江省人民政府20200209102708\Table\A02.xml");
            var xml = XDocument.Load(xmlRd);
            var ts = xml.Root.Elements("data").Elements("row");//.First(x => x.Attribute("name").Value == "群众").Attribute("id").Value; 



            List<string> lts = new List<string>();
            var getList = ts.ToList();
            foreach (var item in getList)
            {
                string getStr = item.Attributes("A0272").First().Value;
                lts.Add(getStr);
            }

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


        #region 3.获取文件中的图片


        public void die(string path)
        {
            string url = "", fileName = "";
            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] files = di.GetFiles();
            fileName = files[0].Name.ToLower();//因为图片只有一张 这里就字节使用  files[0].Name.ToLower();
            if (fileName.EndsWith(".png") || fileName.EndsWith(".jpg") || fileName.EndsWith(".jpeg"))
            {
                url = path + @"\" + fileName.ToString();
            }
        }
        /// <summary>
        /// 获取多种图片
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        public void GetImages(string filePath)
        {
            // GetImages(@"G:\壁纸s\TsImageDel");
            DirectoryInfo di = new DirectoryInfo(filePath);
            FileInfo[] files = di.GetFiles();
            string fileName;
            List<string> list = new List<string>();
            for (int i = 0; i < files.Length; i++)
            {
                fileName = files[i].Name.ToLower();
                if (fileName.EndsWith(".png") || fileName.EndsWith(".jpg") || fileName.EndsWith(".jpeg"))
                {
                    list.Add(fileName);
                }
            }

        }

        public void GetImage(string path)
        {
            // GetImage(@"G:\壁纸s\TsImageDel");
            //  string path = Server.MapPath("img");//获取img文件夹的路径 

            DirectoryInfo di = new DirectoryInfo(path);
            string[] GetFile = new string[] { "*.jpg", "*.png", "*.jpeg" };
            //   di.GetFiles(["","",""]);

            var getx = di.GetFiles("*.jpg");//只获取jpg图片 

            string url = path + @"\" + getx[0].ToString(); ;
            var imagegByteArr = ChangeStreamToByteArr(url);
            string imagestr = Convert.ToBase64String(imagegByteArr);

            var get = di.GetFiles();//获取文件夹下所有的文件 

        }
        public static byte[] ChangeStreamToByteArr(string relativePath)
        {
            string filePath = GetAbsolutePath(relativePath);
            using (Stream input = GetFileStreamByPath(filePath))
            {
                return ChangeStreamToByteArr(input);
            }
        }
        public static byte[] ChangeStreamToByteArr(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 获取文件的绝对路径,针对window程序和web程序都可使用
        /// </summary>
        /// <param name="relativePath">相对路径地址</param>
        /// <returns>绝对路径地址</returns>
        public static string GetAbsolutePath(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                throw new ArgumentNullException("参数relativePath空异常！");
            }
            relativePath = relativePath.Replace("/", "\\");
            if (relativePath[0] == '\\')
            {
                relativePath = relativePath.Remove(0, 1);
            }
            //判断是Web程序还是window程序
            //if (HttpContext.Current != null)
            //{
            //    return Path.Combine(HttpRuntime.AppDomainAppPath, relativePath);
            //}
            //else
            //{
            //    return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
            //}


            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

        }


        public static Stream GetFileStreamByPath(string path)
        {
            Stream stream = System.IO.File.Open(path, FileMode.OpenOrCreate);
            return stream;
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