using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repair.Models
{
    public class WebCounter
    {
        private Int64 request_times;
        private Double transaction_amount;
        private Int64 total_user;
        private static string TABLE_NAME = "WEBCOUNTER";
        public static string GetName { get { return TABLE_NAME; } }

        #region Models
        public Int64 RequestTimes
        {
            get { return request_times; }
            set { request_times = value; }
        }

        public Double TransactionAmount
        {
            get { return transaction_amount; }
            set { transaction_amount = value; }
        }

        public Int64 TotalUser
        {
            get { return total_user; }
            set { total_user = value; }
        }
        #endregion
    }
}
