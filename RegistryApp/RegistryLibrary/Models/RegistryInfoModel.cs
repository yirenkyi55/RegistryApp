namespace RegistryLibrary.Models
{
    public class RegistryInfoModel
    {
        /// <summary>
        /// The unique identifiere of the Registry
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the municipal assembly
        /// </summary>
        public string MunicipalName { get; set; }

        /// <summary>
        /// The name of the registry department
        /// </summary>
        public string RegistryName { get; set; }

        /// <summary>
        /// The address of the registry
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// The Email of the Registry
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The contact of the registry
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// The picName of the Logo
        /// </summary>
        public string PicName { get; set; }

        /// <summary>
        /// The Registry's logo data
        /// </summary>
        public byte[] PicData { get; set; }
    }
}
