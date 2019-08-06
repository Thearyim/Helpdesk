using System;
using System.Text;
using NUnit.Framework;

namespace HelpDesk.Api.Data
{
    [TestFixture]
    [Category("Unit")]
    public class UserAccountTests
    {
        [Test]
        public void UserAccountEncryptsPasswordsToExpectedValuesGivenAnEncryptionKeyAndInitializationVector()
        {
            // Encryption key must be EXACTLY 16 characters in length.
            string encryptionKey = "anyEncryptionKey";
            Guid initializationVector = Guid.Parse("9202A877-3E0E-4D95-AF52-D7C8D20C9C9E");

            string originalPassword = "passW@rd1234";
            string encryptedPassword = UserAccount.Encrypt(originalPassword, encryptionKey, initializationVector);

            byte[] actualBytes = Encoding.Unicode.GetBytes(encryptedPassword);
            byte[] expectedBytes = new byte[] { 207, 85, 65, 163, 143, 164, 28, 85, 216, 116, 53, 204, 96, 115, 141, 211 };

            CollectionAssert.AreEqual(expectedBytes, actualBytes);
        }

        [Test]
        public void UserAccountCanEncryptAndDecryptAGivenPasswordToTheExpectedOriginalValue()
        {
            // Encryption key must be EXACTLY 16 characters in length.
            string encryptionKey = "anyEncryptionKey";
            Guid initializationVector = Guid.Parse("9202A877-3E0E-4D95-AF52-D7C8D20C9C9E");

            string originalPassword = "passW@rd1234";
            string encryptedPassword = UserAccount.Encrypt(originalPassword, encryptionKey, initializationVector);
            string decryptedPassword = UserAccount.Decrypt(encryptedPassword, encryptionKey, initializationVector);

            Assert.AreEqual(originalPassword, decryptedPassword);
        }
    }
}
