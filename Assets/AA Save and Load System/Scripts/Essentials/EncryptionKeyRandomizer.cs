// This code has been written by AHMET ALP for the Unity Asset "AA Save and Load System".
// Link to the asset store page: https://u3d.as/2TxY
// Publisher contact: ahmetalp.business@gmail.com

using UnityEngine;

namespace AASave
{
    public static class EncryptionKeyRandomizer
    {
        private static readonly char[] lettersOnly = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        private static readonly char[] allowedChars = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private static readonly int keyLength = 32;

        public static string GetRandomKey()
        {
            string key = lettersOnly[Random.Range(0, lettersOnly.Length)].ToString();

            for (int i = 0; i < keyLength - 1; i++)
            {
                key += allowedChars[Random.Range(0, allowedChars.Length)];
            }

            return key;
        }
    }
}
