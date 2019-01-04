using System;
using System.Security.Cryptography;
using System.Text;

namespace RegistryLibrary.Infrastructure
{
    public static class EncryptionData
    {
        /// <summary>
        /// Encrypts the data you want to encrypt
        /// </summary>
        /// <param name="valueToEncrypt">The value you want to encrypt</param>  
        public static string EncryptData(string valueToEncrypt)
        {
            //Generates a salted value for the password/value you wish to encrypt
            string GenerateSalt()
            {
                RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
                byte[] salt = new byte[32];
                crypto.GetBytes(salt);
                return Convert.ToBase64String(salt);
            }

            //Finally encrypts the value with the salted value.
            string EncryptValue(string strvalue)
            {
                string saltValue = GenerateSalt();
                byte[] saltedPassword = Encoding.UTF8.GetBytes(saltValue + strvalue);
                SHA256Managed hashStr = new SHA256Managed();
                byte[] hash = hashStr.ComputeHash(saltedPassword);
                return $"{Convert.ToBase64String(hash)}:{saltValue}";

            }

            return EncryptValue(valueToEncrypt);
        }

        /// <summary>
        /// Validates the value you want to validate in the database
        /// </summary>
        /// <param name="valueToValidate">The value you want to validate</param>
        /// <param name="valueFromDatabase">The encrypted value in the database</param>       
        public static bool ValidateEncryptedData(string valueToValidate, string valueFromDatabase)
        {
            string[] arrValues = valueFromDatabase.Split(':');
            string encryptedDbValue = arrValues[0];
            string saltValue = arrValues[1];

            byte[] saltedValue = Encoding.UTF8.GetBytes(saltValue + valueToValidate);
            SHA256Managed hashStr = new SHA256Managed();
            byte[] hash = hashStr.ComputeHash(saltedValue);
    
            return encryptedDbValue.Equals(Convert.ToBase64String(hash));
        }
    }
}
