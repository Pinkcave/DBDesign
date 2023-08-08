using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Repair.Models
{
    public class Rating_System
    {
        public Rating_System() { }
        private static string TABLE_NAME = "RATING_SYSTEM";
        public static string GetName { get { return TABLE_NAME; } }
        #region Model
        private string userid;
        private string engineerid;
        private int rate;
        private string user_comment;

        public string UserID
        {
            get { return userid; }
            set { userid = value; }
        }

        public string EngineerID
        {
            get { return engineerid; }
            set { engineerid = value; }
        }

        public int Rate
        {
            get { return rate; }
            set { rate = value; }
        }

        public string User_Comment
        {
            get { return user_comment; }
            set { user_comment = value; }
        }

        #endregion
    }
}
