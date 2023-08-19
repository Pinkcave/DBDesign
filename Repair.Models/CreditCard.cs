using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repair.Models
{
    public class CreditCard
    {
        private static string TABLE_NAME = "CREDIT_CARD";
        public static string GetName 
        {
            get { return TABLE_NAME; }
        }
        public CreditCard() { }
        #region Models
        private string cardid;
        private string userid;
        private string bank;
        private int isdefault;
        private float balance;

        public string CardID
        {
            get { return cardid; }
            set { cardid = value; }
        }

        public string UserID
        {
            get { return userid; }
            set { userid = value; }
        }

        public string Bank
        {
            get { return bank; }
            set { bank = value; }
        }

        public int IsDefault
        {
            get { return isdefault; }
            set { isdefault = value; }
        }
        public float Balance
        {
            get { return balance; }
            set { balance = value; }
        }
        #endregion
    }
}
