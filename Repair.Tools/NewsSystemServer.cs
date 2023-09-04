using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Repair.Helper;
using Repair.Models;

namespace Repair.Server
{
    public class NewsSystemServer
    {
        public static List<News_System> Query(string? id = null)
        {
            string sql;
            if (id != null)
            {
                sql = "select * from " + News_System.GetName + " where id=" + "\'" + id + "\'";
            }
            else
            {
                sql = "select * from " + News_System.GetName;
            }
            List<News_System> newslist = new List<News_System>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    News_System Object = new News_System();
                    Object.ID = reader.GetString(0);
                    Object.UserID = reader.GetString(1);
                    Object.News_Title = reader.GetString(2);
                    Object.News_Date = reader.GetDateTime(3);
                    Object.News_Content = reader.GetString(4);
                    Object.IsRead = reader.GetInt32(5);
                    newslist.Add(Object);
                }
                reader.Close();
            }
            return newslist;
        }

        public static List<News_System> QueryByAttribute(string attribute,string value)
        {
            string sql = "select * from News_System where {0} = {1}";
            sql=string.Format(sql,attribute,value);
            List<News_System> newslist = new List<News_System>();
            using (OracleDataReader reader = DBHelper.GetDataReader(sql, null))
            {
                while (reader.Read())
                {
                    News_System Object = new News_System();
                    Object.ID = reader.GetString(0);
                    Object.UserID = reader.GetString(1);
                    Object.News_Title = reader.GetString(2);
                    Object.News_Date = reader.GetDateTime(3);
                    Object.News_Content = reader.GetString(4);
                    Object.IsRead = reader.GetInt32(5);
                    newslist.Add(Object);
                }
                reader.Close();
            }
            return newslist;
        }

        public static int Insert(News_System news)
        {
            //News_System? news = JsonSerializer.Deserialize<News_System>(JsonInfo);
            if (news == null)
                return -1;
            string sql = "insert into " + News_System.GetName + " values("
                            + "\'" + news.ID + "\',"
                            + "\'" + news.UserID + "\',"
                            + "\'" + news.News_Title + "\',"
                            + "to_timestamp(\'" + news.News_Date + "\',\'MM-DD-YYYY HH24:MI:SS\'),"
                            //+ "to_timestamp(\'" + news.News_Date + "\',\'YYYY-MM-DD HH24:MI:SS\'),"
                            + "\'" + news.News_Content + "\',"
                            + news.IsRead + ")";

            int row = DBHelper.RunExecNonQuery(sql, null);
            return row;
        }

        public static int Delete(string uid)
        {
            string sql = "delete from " + News_System.GetName + " where id=:del_USERID";
            OracleParameter[] param =
            {
                new OracleParameter(":del_USERID", OracleDbType.Varchar2, uid, ParameterDirection.Input)
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }

        public static int Update(News_System news, string old_id)
        {
            //News_System? news = JsonSerializer.Deserialize<News_System>(JsonInfo);
            if (news == null)
                return -1;
            string sql = "update " + News_System.GetName + " set "
                         + "id=:new_ID, userid=:new_USERID, news_title=:new_TITLE,"
                         + "news_date=:new_DATE, news_content=:new_CONTENT, isread=:new_ISREAD "
                         + "where id=\'" + old_id + "\'";
            OracleParameter[] param =
            {
                new OracleParameter(":new_ID", OracleDbType.Varchar2, news.ID, ParameterDirection.Input),
                new OracleParameter(":new_USERID", OracleDbType.Varchar2, news.UserID, ParameterDirection.Input),
                new OracleParameter(":new_TITLE", OracleDbType.Varchar2, news.News_Title, ParameterDirection.Input),
                new OracleParameter(":new_DATE", OracleDbType.TimeStamp, news.News_Date, ParameterDirection.Input),
                new OracleParameter(":new_CONTENT", OracleDbType.Varchar2, news.News_Content, ParameterDirection.Input),
                new OracleParameter(":new_ISREAD", OracleDbType.Int32, news.IsRead, ParameterDirection.Input)
                
            };

            int row = DBHelper.RunExecNonQuery(sql, param);
            return row;
        }

        public static bool AddNews(string uid, string title, string content)
        {
            News_System news = new News_System();
            DateTime current = DateTime.Now;
            news.ID = uid + '&' + current.Ticks.ToString();
            news.UserID = uid;
            news.News_Title = title;
            news.News_Content = content;
            news.News_Date = current;
            news.IsRead = 0;
            if(Insert(news)<=0)
            {
                return false;
            }
            return true;
        }
    }
}
