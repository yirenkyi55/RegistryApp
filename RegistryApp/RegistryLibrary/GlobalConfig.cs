using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace RegistryLibrary
{
    public class GlobalConfig
    {
        private static string conStringName = "RegistryCS";

        public static string ConnString()
        {
            return ConfigurationManager.ConnectionStrings[conStringName].ConnectionString;
        }
        

    }
}
