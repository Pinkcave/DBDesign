using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repair.Models
{
    public class News_System
    {
        public News_System() { }
        private static string TABLE_NAME = "NEWS_SYSTEM";
        public static string GetName { get { return TABLE_NAME; } }

        #region Model
        private string id;
        private string userid;
        private string title;
        private DateTime date;
        private string content;
        private int isread;

        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        public string UserID
        {
            get { return userid; }
            set { userid = value; }
        }

        public string News_Title
        {
            get { return title; }
            set { title = value; }
        }

        public DateTime News_Date
        {
            get { return date; }
            set { date = value; }
        }

        public string News_Content
        {
            get { return content; }
            set { content = value; }
        }

        public int IsRead
        {
            get { return isread; }
            set { isread = value; }
        }
        #endregion
    }
}
