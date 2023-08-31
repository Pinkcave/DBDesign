using Oracle.ManagedDataAccess.Client;
using Repair.Helper;
using Repair.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repair.Server
{
    public class WebCounterServer
    {
        public static WebCounter Query()
        {
            string sql = string.Format("select * from {0}",WebCounter.GetName);
            WebCounter web = new WebCounter();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                if(reader.Read())
                {
                    web.RequestTimes = reader.GetInt64(0);
                    web.TransactionAmount = reader.GetDouble(1);
                    web.TotalUser = reader.GetInt64(2);
                }
            }
            return web;
        }

        public static int UpdateRequest(int num) 
        {
            string sql = string.Format("update {0} set request=request+{1}", WebCounter.GetName, num);
            return DBHelper.RunExecNonQuery(sql, null);
        }

        public static int UpdateTransaction(Double num)
        {
            string sql = string.Format("update {0} set trans_amount=trans_amount+{1}", WebCounter.GetName, num);
            return DBHelper.RunExecNonQuery(sql, null);
        }

        public static int UpdateTotalUser()
        {
            string sql = string.Format("update {0} set totaluser=(select count(*) from {1})", WebCounter.GetName, UserInfo.GetName);
            return DBHelper.RunExecNonQuery(sql, null);
        }
    }
}
