// This code has been written by AHMET ALP for the Unity Asset "AA Save and Load System".
// Link to the asset store page: https://u3d.as/2TxY
// Publisher contact: ahmetalp.business@gmail.com

using UnityEngine;

namespace AASave
{
    public class ExtensionRandomizer
    {

        [Tooltip("This string is used while generating a random file extension.")] private static readonly string extensionChars = "abcdefghijklmnopqrstuvwxyz0123456789";

        /// <summary>
        /// Generates a new random save file extension with the given length.
        /// </summary>
        /// <param name="length">Length of the save file extension.</param>
        /// <returns>Generated save file extension.</returns>
        public static string GenerateRandomExtension(int length)
        {
            string temp = extensionChars[Random.Range(0, 25)].ToString();

            for (int i = 0; i < length - 1; i++)
            {
                temp += extensionChars[Random.Range(0, extensionChars.Length)].ToString();
            }

            return temp;
        }
    }
}
