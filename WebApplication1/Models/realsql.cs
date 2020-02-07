

using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace WebApplication1.Models
{
    public class realsql
    {

        public static List<code_value> QueryExecute(string sql)
        {
             
            //var daset = DbHelperMySQL.Query(strSql.ToString());
            using (IDbConnection connection = OpenConnection())
            {
                // 查询
                var list = connection.Query<code_value>(sql).ToList<code_value>();
              

                return list;
            }
        }

        /// <summary>
        /// 打开连接
        /// </summary>
        /// <returns></returns>
        public static IDbConnection OpenConnection()
        {
            //IDbConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["AdventureWorks"].ConnectionString);


            IDbConnection connection = new MySqlConnection(connectionString1);
            connection.Open();

            return connection;
        }
        private static readonly string connectionString1 = System.Configuration.ConfigurationManager.ConnectionStrings["conncodefirst"].ToString();

      
    }
}