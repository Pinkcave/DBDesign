using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repair.Models
{
    public class UserInfo
    {
        public UserInfo() { }

        #region Model
        private string uid;
        private string uname;
        private string pwd;
        private int level;
        private string name; 
        private string identity;
        private string tel;
        private string email;
        private float balance;

        private static string TABLE_NAME= "USERINFO";
        public static string GetName { get { return TABLE_NAME; } }

        private static int usernum = 10000;
        public static int Num { get { return usernum; } set { usernum = value; } }
        public string Password
        {
            get { return pwd; }
            set { pwd = value; }
        }

        public string UserId
        {
            get { return uid; }
            set { uid = value; }
        }

        public string UserName
        {
            get { return uname; }
            set { uname = value; }
        }

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Identity
        {
            get { return identity; }
            set { identity = value; }
        }

        public string Telephone
        {
            get { return tel; }
            set { tel = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public float Balance
        {
            get { return balance; }
            set { balance = value; }
        }
        #endregion
    }
}
