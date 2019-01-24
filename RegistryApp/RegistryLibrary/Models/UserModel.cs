namespace RegistryLibrary.Models
{
    public class UserModel
    {
        /// <summary>
        /// The unique identifier of the user
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The user name of the user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The user full name
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// The password of the user
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The security question of the user
        /// </summary>
        public int Question { get; set; }

        /// <summary>
        /// The answer of the security question
        /// </summary>
        public string  Answer { get; set; }

        /// <summary>
        /// The access type of the question
        /// </summary>
        public string AccessType { get; set; }
    }
}
