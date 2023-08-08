using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repair.Models
{
    public class Support_Staff
    {
        public Support_Staff() { }
        #region Model
        private string id;
        private string password;
        private string realname;
        private string nickname;

        public static string GetName
        {
            get { return "SUPPORT_STUFF"; }
        }

        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string Realname
        {
            get { return realname; }
            set { realname = value; }
        }

        public string Nickname
        {
            get { return nickname; }
            set { nickname = value; }
        }
        #endregion
    }
}
