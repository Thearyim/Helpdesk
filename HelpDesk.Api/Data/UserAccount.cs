using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HelpDesk.Api.Data
{
    public class UserAccount
    {
        public UserAccount(int id, string username, string password, string role)
        {
            if (id < 0)
            {
                throw new ArgumentException("The id parameter must be defined", nameof(id));
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("The username parameter must be defined", nameof(username));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("The password parameter must be defined", nameof(password));
            }

            if (string.IsNullOrWhiteSpace(role))
            {
                throw new ArgumentException("The role parameter must be defined", nameof(role));
            }

            this.Id = id;
            this.Username = username;
            this.Password = password;
            this.Role = role;
        }

        public int Id { get; }

        public string Username { get; }

        public string Password { get; }

        public string Role { get; }

        public static string Decrypt(string encryptedText, string encryptionKey, Guid initializationVector)
        {
            // https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.aescryptoserviceprovider?view=netframework-4.8

            if (encryptedText == null)
            {
                throw new ArgumentException("The encrypted text parameter is required.", nameof(encryptedText));
            }

            if (encryptionKey == null)
            {
                throw new ArgumentException("The encryption key parameter is required.", nameof(encryptionKey));
            }

            if (encryptionKey.Length != 16)
            {
                throw new ArgumentException("The encryption key parameter must be exactly 16 characters in length.");
            }

            string originalText = null;
            byte[] encryptionKeyBytes = Encoding.Unicode.GetBytes(encryptionKey);

            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                // Requirements
                // Key size must be 16 bits exactly (8 bits/char * 32 chars = 256)
                // Initialization Vector (IV) size must be 128 bits.  8 bits/char * 16 = 128. A standard Guid will do.
                aesAlg.KeySize = 256;
                aesAlg.BlockSize = 128;

                aesAlg.Key = encryptionKeyBytes;
                aesAlg.IV = initializationVector.ToByteArray();

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Encoding.Unicode.GetBytes(encryptedText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            originalText = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return originalText;
        }

        public static string Encrypt(string text, string encryptionKey, Guid initializationVector)
        {
            // https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.aescryptoserviceprovider?view=netframework-4.8

            if (text == null)
            {
                throw new ArgumentException("The text parameter is required.", nameof(text));
            }

            if (encryptionKey == null)
            {
                throw new ArgumentException("The encryption key parameter is required.", nameof(encryptionKey));
            }

            if (encryptionKey.Length != 16)
            {
                throw new ArgumentException("The encryption key parameter must be exactly 16 characters in length.");
            }

            byte[] encryptedText;
            byte[] encryptionKeyBytes = Encoding.Unicode.GetBytes(encryptionKey);

            using (AesCryptoServiceProvider aesAlg  = new AesCryptoServiceProvider())
            {
                // Requirements
                // Key size must be 16 bits exactly (8 bits/char * 32 chars = 256)
                // Initialization Vector (IV) size must be 128 bits.  8 bits/char * 16 = 128. A standard Guid will do.
                aesAlg.KeySize = 256;
                aesAlg.BlockSize = 128;

                aesAlg.Key = encryptionKeyBytes;
                aesAlg.IV = initializationVector.ToByteArray();

                ICryptoTransform encryptor = aesAlg .CreateEncryptor(aesAlg .Key, aesAlg .IV);

                using (MemoryStream encryptionStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(encryptionStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(cryptoStream))
                        {
                            // Write all data to the stream.
                            swEncrypt.Write(text);
                        }

                        encryptedText = encryptionStream.ToArray();
                    }
                }
            }

            return Encoding.Unicode.GetString(encryptedText);
        }
    }
}
