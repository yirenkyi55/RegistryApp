using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistryLibrary.Models
{
    public class DepartmentModel
    {
        /// <summary>
        /// Represents the unique id of the department
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Represents the name of the department
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// Represents the address of the department
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Represents the email of the department
        /// </summary>
        public string Email { get; set; }
    }
}
