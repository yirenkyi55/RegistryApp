using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistryLibrary.Infrastructure
{
    public class LoggerModel
    {
      
        public string LoggerName { get; set; }
        public string Event { get; set; }
        public string Date { get; set; } = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
    }
}
