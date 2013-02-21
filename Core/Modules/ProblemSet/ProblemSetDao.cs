using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Modules.ProblemSet
{
    public class ProblemSetDao
    {
        private SqlConnectionStringBuilder csBuilder;

        public ProblemSetDao()
        {
            csBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["AzureConnection"].ConnectionString);
        }
    }
}
