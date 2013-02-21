using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Modules.Problem
{
    public class ProblemDao
    {
        private SqlConnectionStringBuilder csBuilder;

        public ProblemDao()
        {
            csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["AzureConnection"].ConnectionString);
        }
    }
}
