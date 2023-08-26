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
    public class CustomerServiceServer
    {
        public static List<CustomerService> Query(string? id = null)
        {
            string sql;
            if (id != null)
            {
                sql = "select * from " + CustomerService.GetName + " where id=" + "\'" + id + "\'";
            }
            else
            {
                sql = "select * from " + CustomerService.GetName;
            }
            List<CustomerService> list = new List<CustomerService>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    CustomerService Object = new CustomerService();
                    Object.ID = reader.GetString(0);
                    Object.Question = reader.GetString(1);
                    Object.Answer = reader.GetString(2);
                    list.Add(Object);
                }
                reader.Close();
            }
            return list;
        }

        public static List<CustomerService> QueryByAttribute(string attribute, string value)
        {
            string sql = string.Format("select * from {0} where {1} = {2}", CustomerService.GetName, attribute, value);
            List<CustomerService> list = new List<CustomerService>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    CustomerService Object = new CustomerService();
                    Object.ID = reader.GetString(0);
                    Object.Question = reader.GetString(1);
                    Object.Answer = reader.GetString(2);
                    list.Add(Object);
                }
            }
            return list;
        }

        public static int Insert(CustomerService customerService)
        {
            //CreditCard? card = JsonSerializer.Deserialize<CreditCard>(JsonInfo);
            if (customerService == null)
                return -1;
            string sql = "insert into " + CustomerService.GetName + " values(" 
                       + "\'" + customerService.ID + "\'," 
                       + "\'" + customerService.Question + "\'," 
                       + "\'" + customerService.Answer + "\')";

            int row = DBHelper.RunExecNonQuery(sql, null);
            return row;
        }

        public static int Delete(string id)
        {
            string sql = "delete from " + CustomerService.GetName + " where id=:del_ID";
            OracleParameter[] param =
            {
                new OracleParameter(":del_ID", OracleDbType.Varchar2, id, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }

        public static int Update(CustomerService customerService, string old_id)
        {
            //CreditCard? card = JsonSerializer.Deserialize<CreditCard>(JsonInfo);
            if (customerService == null)
                return -1;
            string sql = "update " + CustomerService.GetName + " set "
                         + "id=:new_ID, question=:new_Question, answer=:new_Answer" + " where id=\'" + old_id + "\'";
            OracleParameter[] param =
            {
                new OracleParameter(":new_ID", OracleDbType.Varchar2, customerService.ID, ParameterDirection.Input),
                new OracleParameter(":new_Question", OracleDbType.Varchar2, customerService.Question, ParameterDirection.Input),
                new OracleParameter(":new_Answer", OracleDbType.Varchar2, customerService.Answer, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }
    }
}
