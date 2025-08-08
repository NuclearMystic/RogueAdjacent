// This code has been written by AHMET ALP for the Unity Asset "AA Save and Load System".
// Link to the asset store page: https://u3d.as/2TxY
// Publisher contact: ahmetalp.business@gmail.com

using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;

namespace AASave
{
    public static class AAEncryption
    {
        // ########################################################################################
        // ########################################################################################
        // ########################################################################################
        // ###                            THIS IS THE ENCRYPTION KEY                            ###
        // ###    ALL THE SAVE SYSTEM INSTANCES IN YOUR PROJECT USES THE SAME ENCRYPTION KEY    ###
        // ###  YOU CAN USE THE "RANDOMIZE ENCRYPTION KEY" BUTTON IN THE SAVE SYSTEM INSPECTOR  ###

        private static readonly string encryptionKey = "qp14acpc3suwkplokejrdqvcjdmmkvow";

        // ########################################################################################
        // ########################################################################################
        // ########################################################################################

        /// <summary>
        /// Performs the AES encryption algorithm and returns the encrypted game data.
        /// </summary>
        /// <param name="gameData">Game data that is going to be encrypted.</param>
        /// <returns>Encrypted game data.</returns>
        public static string Encrypt(string gameData)
        {
            Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aes.IV = new byte[16];

            ICryptoTransform cryptoTransform = aes.CreateEncryptor(aes.Key, aes.IV);

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, cryptoTransform, CryptoStreamMode.Write);

            using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
            {
                streamWriter.Write(gameData);
            }

            byte[] resultArray = memoryStream.ToArray();

            cryptoStream.Close();
            memoryStream.Close();

            return Convert.ToBase64String(resultArray);
        }

        /// <summary>
        /// Performs the AES decryption algorithm and returns the decrypted game data.
        /// </summary>
        /// <param name="encryptedGameData">Encrypted game data that wants to be decrypted.</param>
        /// <returns>Decrypted game data.</returns>
        public static string Decrypt(string encryptedGameData)
        {
            byte[] buffer = Convert.FromBase64String(encryptedGameData);

            Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aes.IV = new byte[16];

            ICryptoTransform cryptoTransform = aes.CreateDecryptor(aes.Key, aes.IV);

            MemoryStream memoryStream = new MemoryStream(buffer);
            CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, cryptoTransform, CryptoStreamMode.Read);

            string tempDecrypted;

            using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
            {
                tempDecrypted = streamReader.ReadToEnd();
            }

            cryptoStream.Close();
            memoryStream.Close();

            return tempDecrypted;
        }

        /// <summary>
        /// Performs the AES decryption algorithm and returns the decrypted game data.
        /// </summary>
        /// <param name="encryptedGameData">Encrypted game data that wants to be decrypted.</param>
        /// <returns>Decrypted game data.</returns>
        public static string[] Decrypt(string[] encryptedGameData)
        {
            for (int i = 0; i < encryptedGameData.Length; i++)
            {
                encryptedGameData[i] = Decrypt(encryptedGameData[i]);
            }

            return encryptedGameData;
        }
    }
}
