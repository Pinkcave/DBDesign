using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace DBDesign
{
    enum Operation
    {
        Insert,
        Delete,
        Update
    }

    internal class DBHelper
    {
        private static string Entry = "UserId=JAY;Password=123456;Data Source=xe";

        public static OracleConnection GetConnection()
        {
            OracleConnection? con = new OracleConnection(Entry);
            try
            {
                con.Open();
                Console.WriteLine("Sueccessfully Opened\n");
                return con;
            }

            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                con.Close();
            }
            con = null; 
            return con;
        }

        public static OracleConnection GetConnection(string conInfo)
        {
            OracleConnection? con = new OracleConnection(conInfo);
            try
            {
                con.Open();
                Console.WriteLine("Sueccessfully Opened\n");
                return con;
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                con.Close();
            }
            con = null;
            return con;
        }

        public static void Disconnection(OracleConnection con)
        {
            if (con == null)
                return;
            con.Close();
        }

        public static List<Dictionary<string,string>> ReadInfo(OracleConnection con,string sql)
        {
            List<Dictionary<string,string>> res = new List<Dictionary<string,string>>();
            //判断连接是否有效
            if (con == null)
                return res;
            using (OracleCommand cmd=con.CreateCommand())
            {
                cmd.CommandText = sql;
                OracleDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    if (reader.GetBoolean(1))
                    {
                        Dictionary<string,string> temp = new Dictionary<string,string>();
                        for(int i = 0; i < reader.FieldCount; i++)
                        {
                            temp.Add(reader.GetName(i), reader.GetString(i));
                        }
                        res.Add(temp);
                    }              
                }
            }
            return res;
        }

        public static int ModifyOperation(OracleConnection con,string sql)
        {
            int ret = -1;
            using (OracleCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandText = sql;
                    ret = cmd.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return ret;
        }

        public static string ToInsertSQL(Dictionary<string,string> content)
        {
            string sql = "insert into " + content["Table"]+"(";
            
            using (OracleConnection con = GetConnection(Entry))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "select column_name,datatype from user_tab_column where table_name=\'" + content["Table"] + "\'";
                    OracleDataReader reader = cmd.ExecuteReader();
                    //生成sql语句
                    if(reader.Read())
                    {
                        if (reader.GetString(1) == "VARCHAR")
                            sql += ("\'" + content[reader.GetString(0)] + "\'");
                        else
                            sql += content[reader.GetString(0)];
                    }
                    
                    while (reader.Read())
                    {
                        if (reader.GetString(1) == "VARCHAR")
                            sql += ',' + ("\'" + content[reader.GetString(0)] + "\'");
                        else
                            sql += ',' + content[reader.GetString(0)];
                    }
                }
            }
            
            return sql;
        }
    }
}
