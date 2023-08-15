using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repair.Models
{
    public class Engineer
    {
        public Engineer() { }
        private static string TABLE_NAME = "ENGINEER";
        public static string GetName { get { return TABLE_NAME; } }

        #region Model
        private string id;
        private string name;
        private string sex;
        private int workingyears;
        private float salary;
        private int rate;
        private string pwd;
        private string working_center;

        
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Password
        {
            get { return pwd; }
            set { pwd = value; }
        }

        

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Sex
        {
            get { return sex; }
            set { sex = value; }
        }

        public int WorkingYears
        {
            get { return workingyears; }
            set { workingyears = value; }
        }

        public float Salary
        {
            get { return salary; }
            set { salary = value; }
        }

        public int Rate
        {
            get { return rate; }
            set { rate = value; }
        }
        public string WorkingCenter
        {
            get { return working_center; }
            set { working_center = value; }
        }
        #endregion
    }
}