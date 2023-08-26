using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Repair.Helper;
using Repair.Models;

namespace Repair.Server
{
    public class AboutUsServer
    {
        public static List<AboutUs> Query(string? id = null)
        {
            string sql;
            if (id != null)
            {
                sql = "select * from " + AboutUs.GetName + " where id=" + "\'" + id + "\'";
            }
            else
            {
                sql = "select * from " + AboutUs.GetName;
            }
            List<AboutUs> list = new List<AboutUs>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    AboutUs Object = new AboutUs();
                    Object.ID = reader.GetString(0);
                    Object.Question = reader.GetString(1);
                    Object.Answer = reader.GetString(2);
                    list.Add(Object);
                }
                reader.Close();
            }
            return list;
        }

        public static List<AboutUs> QueryByAttribute(string attribute, string value)
        {
            string sql = string.Format("select * from {0} where {1} = {2}", AboutUs.GetName, attribute, value);
            List<AboutUs> list = new List<AboutUs>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    AboutUs Object = new AboutUs();
                    Object.ID = reader.GetString(0);
                    Object.Question = reader.GetString(1);
                    Object.Answer = reader.GetString(2);
                    list.Add(Object);
                }
            }
            return list;
        }

        public static int Insert(AboutUs aboutUs)
        {
            //CreditCard? card = JsonSerializer.Deserialize<CreditCard>(JsonInfo);
            if (aboutUs == null)
                return -1;
            string sql = "insert into " + AboutUs.GetName + " values("
                       + "\'" + aboutUs.ID + "\',"
                       + "\'" + aboutUs.Question + "\',"
                       + "\'" + aboutUs.Answer + "\')";

            int row = DBHelper.RunExecNonQuery(sql, null);
            return row;
        }

        public static int Delete(string id)
        {
            string sql = "delete from " + AboutUs.GetName + " where id=:del_ID";
            OracleParameter[] param =
            {
                new OracleParameter(":del_ID", OracleDbType.Varchar2, id, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }

        public static int Update(AboutUs aboutUs, string old_id)
        {
            //CreditCard? card = JsonSerializer.Deserialize<CreditCard>(JsonInfo);
            if (aboutUs == null)
                return -1;
            string sql = "update " + AboutUs.GetName + " set "
                         + "id=:new_ID, question=:new_Question, answer=:new_Answer" + " where id=\'" + old_id + "\'";
            OracleParameter[] param =
            {
                new OracleParameter(":new_ID", OracleDbType.Varchar2, aboutUs.ID, ParameterDirection.Input),
                new OracleParameter(":new_Question", OracleDbType.Varchar2, aboutUs.Question, ParameterDirection.Input),
                new OracleParameter(":new_Answer", OracleDbType.Varchar2, aboutUs.Answer, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }
    }
}
