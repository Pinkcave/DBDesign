using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repair.Models
{
    public class CustomerService
    {
        public CustomerService() { }
        private static string TABLE_NAME = "CUSTOMER_SERVICE";
        public static string GetName { get { return TABLE_NAME; } }
        #region Model
        private string id;
        private string question;
        private string answer;
        
        public string ID
        {
            get { return id; }
            set { id = value; }
        }
        public string Question
        {
            get { return question; }
            set { question = value; }
        }
        public string Answer
        {
            get { return answer; }
            set { answer = value; }
        }
        #endregion
    }
}
