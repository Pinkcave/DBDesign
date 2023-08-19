using Oracle.ManagedDataAccess.Client;
using Repair.Helper;
using Repair.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Repair.Server
{
    public class CreditCardServer
    {
        public static List<CreditCard> Query(string? id = null)
        {
            string sql;
            if (id != null)
            {
                sql = "select * from " + CreditCard.GetName + " where cardid=" + "\'" + id + "\'";
            }
            else
            {
                sql = "select * from " + CreditCard.GetName;
            }
            List<CreditCard> list = new List<CreditCard>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    CreditCard Object = new CreditCard();
                    Object.CardID = reader.GetString(0);
                    Object.UserID = reader.GetString(1);
                    Object.Bank = reader.GetString(2);
                    Object.IsDefault = reader.GetInt32(3);
                    Object.Balance = reader.GetFloat(4);
                    list.Add(Object);
                }
                reader.Close();
            }
            return list;
        }

        public static List<CreditCard> QueryByAttribute(string attribute, string value)
        {
            string sql = string.Format("select * from {0} where {1} = {2}", CreditCard.GetName,attribute, value);
            List<CreditCard> list = new List<CreditCard>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    CreditCard Object = new CreditCard();
                    Object.CardID = reader.GetString(0);
                    Object.UserID = reader.GetString(1);
                    Object.Bank = reader.GetString(2);
                    Object.IsDefault = reader.GetInt32(3);
                    Object.Balance = reader.GetFloat(4);
                    list.Add(Object);
                }
            }
            return list;
        }

        public static int Insert(CreditCard card)
        {
            //CreditCard? card = JsonSerializer.Deserialize<CreditCard>(JsonInfo);
            if (card == null)
                return -1;
            string sql = "insert into " + CreditCard.GetName + " values(" + "\'" + card.CardID + "\'," + "\'" + card.UserID + "\'," + "\'" + card.Bank + "\'," + card.IsDefault + "," + card.Balance + ")";

            int row = DBHelper.RunExecNonQuery(sql, null);
            return row;
        }

        public static int Delete(string id)
        {
            string sql = "delete from " + CreditCard.GetName + " where cardid=:del_CARDID";
            OracleParameter[] param =
            {
                new OracleParameter(":del_CARDID", OracleDbType.Varchar2, id, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }

        public static int Update(CreditCard card, string old_id)
        {
            //CreditCard? card = JsonSerializer.Deserialize<CreditCard>(JsonInfo);
            if (card == null)
                return -1;
            string sql = "update " + CreditCard.GetName + " set "
                         + "cardid=:new_CardID, userid=:new_UserID, bank=:new_Bank,isdefault=:new_Default,balance=:new_Balance" + " where cardid=\'" + old_id + "\'";
            OracleParameter[] param =
            {
                new OracleParameter(":new_CardID", OracleDbType.Varchar2, card.CardID, ParameterDirection.Input),
                new OracleParameter(":new_UserID", OracleDbType.Varchar2, card.UserID, ParameterDirection.Input),
                new OracleParameter(":new_Bank", OracleDbType.Varchar2, card.Bank, ParameterDirection.Input),
                new OracleParameter(":new_Default",OracleDbType.Int32,card.IsDefault,ParameterDirection.Input),
                new OracleParameter(":new_Balance",OracleDbType.Double,card.Balance,ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }

        public static CreditCard? GetDefaultCard(string uid)
        {
            string sql = string.Format("select * from credit_card where userid = {0} and isdefault = 1", uid);
            CreditCard? Object = null;
            using (var reader = DBHelper.GetDataReader(sql, null))
            {
                if (reader.Read())
                {
                    Object = new CreditCard();
                    Object.CardID = reader.GetString(0);
                    Object.UserID = reader.GetString(1);
                    Object.Bank = reader.GetString(2);
                    Object.IsDefault = reader.GetInt32(3);
                    Object.Balance = reader.GetFloat(4);
                }
            }
            return Object;
        }
        public static bool ChangeDefaultCard(string uid,string new_id)
        {
            string sql = string.Format("update {0} set isdefault = 0 where userid = {1}", CreditCard.GetName, uid);
            int row = DBHelper.RunExecNonQuery(sql, null);
            if (row <= 0)
                return false;
            sql = string.Format("update {0} set isdefault = 1 where cardid = {1} and userid = {2}", CreditCard.GetName, new_id, uid);
            row = DBHelper.RunExecNonQuery(sql, null);
            if (row <= 0) 
                return false;
            return true;
        }
    }
}
