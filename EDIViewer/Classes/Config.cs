using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manual_test_app
{
    class Config
    {
        public static readonly Config _instance = new Config();

        private string _ConnectionString = "Data Source=10.1.1.120;Initial Catalog=al60_seton_lite_developer;Persist Security Info=True;User ID=al60_seton_lite_developer_user;Password=al60_seton_lite_developer_password";


        public void Test()
        {
            // Code runs.
            Console.WriteLine(true);
        }

        public string ConnectionString
        {
            get
            {

                return _ConnectionString;
            }
            set
            {

                _ConnectionString = value;
            }
        }




        Config()
        {

          
        }

    }
}
