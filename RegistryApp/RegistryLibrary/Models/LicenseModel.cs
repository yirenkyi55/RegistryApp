using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistryLibrary.Models
{
    public class LicenseModel
    {
        public int Id { get; set; }
        public string LicenseKey { get; set; }
        public DateTime? InstalledDate { get; set; }
        public DateTime? CurrentDate { get; set; }
        public string AppliedKey { get; set; }
        public bool Licensed { get; set; }
    }
}
