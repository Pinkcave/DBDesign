using System;
using System;
using System.Configuration;
using System.Data;
using System.Web;
using Repair.Models;
using Oracle.ManagedDataAccess.Client;

namespace Repair.Helper
{
    public class DBHelper
    {
        private static string connstr = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=110.42.220.245)(PORT=1521))"
                                          + "(CONNECT_DATA=(SERVICE_NAME=xe)));"
                                          + "Persist Security Info=True;User ID=jay;Password=123456;";
        /*private static string connstr = "Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521))"
                                          + "(CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = orcl)));"
                                          + "Persist Security Info=True;User ID=C##NIJIKA;Password=nijika6425317;";*/

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="pa"></param>
        /// <returns></returns>
        public static int RunExecNonQuery(string Sql, OracleParameter[]? pa)
        {
            using (OracleConnection conn = new OracleConnection(connstr))
            {
                int row = 0;
                OracleTransaction? tran = null;
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction();//开始事务
                    OracleCommand com = new OracleCommand(Sql, conn);
                    if (pa != null) com.Parameters.AddRange(pa);
                    com.Transaction = tran;
                    row = com.ExecuteNonQuery();
                    tran.Commit();//提交事务

                }
                catch (Exception ex)
                {
                    //回滚事务
                    if(tran!=null)
                        tran.Rollback();
                    Console.WriteLine(ex.Message);
                    throw;
                }
                finally { conn.Close(); }
                return row;
            }
        }

        /// <summary>
        /// 查询，返回表信息
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="Record"></param>
        /// <returns></returns>
        public DataTable GetTable(ref string Sql)
        {
            OracleConnection conn = new OracleConnection(connstr);
            OracleCommand com = new OracleCommand(Sql, conn);
            OracleDataAdapter da = new OracleDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds.Tables[0];
        }

        public static object GetScalar(string sql, OracleParameter[]? pa)
        {
            using (OracleConnection conn = new OracleConnection(connstr))
            {
                object? ob = null;
                try
                {
                    conn.Open();
                    OracleCommand com = new OracleCommand(sql, conn);
                    com.Parameters.AddRange(pa);
                    ob = com.ExecuteScalar();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
                finally { conn.Close(); }
                return ob;
            }
        }

        /// <summary>
        /// 读取表格信息
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pa"></param>
        /// <returns></returns>
        public static OracleDataReader GetDataReader(string sql, OracleParameter[]? pa)
        {
            OracleConnection conn = new OracleConnection(connstr);
            conn.Open();
            OracleCommand com = new OracleCommand(sql, conn);
            if (pa != null) com.Parameters.AddRange(pa);
            OracleDataReader reader = com.ExecuteReader(CommandBehavior.CloseConnection);
            return reader;
        }

        public static List<string> StrQuery(string Sql)
        {
            List<string> ret = new List<string>();
            using(OracleConnection conn = new OracleConnection(connstr))
            {
                try
                {
                    conn.Open();
                    OracleCommand com = conn.CreateCommand();
                    com.CommandText = Sql;
                    OracleDataReader odr = com.ExecuteReader();
                    while (odr.Read())
                    {
                        ret.Add(odr.GetOracleString(0).ToString());
                    }
                    conn.Close();
                }
                catch (Exception e)
                {
                    ret.Clear();
                    //Console.WriteLine(e.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

            return ret;
        }
    }
}