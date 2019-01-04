using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistryLibrary.Models
{
    public class IncomingFileModel
    {
        /// <summary>
        /// Represents the unique id of the file
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Represents the registry number of 
        /// </summary>
        public int RegistryNumber { get; set; }

        /// <summary>
        /// Represents the Date Received from the incoming file
        /// </summary>
        public DateTime DateReceived { get; set; }

        /// <summary>
        /// Reoresents the person sent
        /// </summary>
        public string PersonSent { get; set; }

        /// <summary>
        /// Represents the date indicated on the letter
        /// </summary>
        public DateTime DateOfLetter { get; set; }

        /// <summary>
        /// Represents the reference number on the letter
        /// </summary>
        public string ReferenceNumber { get; set; }

        /// <summary>
        /// Represents the subject on the letter
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Represents the department/individual who brought the letter
        /// </summary>
        public string DepartmentSent { get; set; }

        /// <summary>
        /// Represents the pdf file name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Indicates the remarks of the letter
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// Represents the department to receive the letter
        /// </summary>
        public DepartmentModel Department { get; set; }
    }
}
