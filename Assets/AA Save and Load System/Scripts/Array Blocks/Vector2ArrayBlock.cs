// This code has been written by AHMET ALP for the Unity Asset "AA Save and Load System".
// Link to the asset store page: https://u3d.as/2TxY
// Publisher contact: ahmetalp.business@gmail.com

using UnityEngine;

namespace AASave
{
    [System.Serializable]
    public class Vector2ArrayBlock
    {
        public string dataName;
        public string[] value;

        public Vector2ArrayBlock(string dataName, Vector2[] value, bool encryptData)
        {
            if (encryptData)
            {
                this.dataName = AAEncryption.Encrypt(dataName);
                this.value = new string[value.Length];

                for (int i = 0; i < value.Length; i++)
                {
                    this.value[i] = "";
                    this.value[i] += value[i].x.ToString() + "#";
                    this.value[i] += value[i].y.ToString() + "#";
                    this.value[i] = AAEncryption.Encrypt(this.value[i]);
                }
            }
            else
            {
                this.dataName = dataName;
                this.value = new string[value.Length];

                for (int i = 0; i < value.Length; i++)
                {
                    this.value[i] = "";
                    this.value[i] += value[i].x.ToString() + "#";
                    this.value[i] += value[i].y.ToString() + "#";
                }
            }
        }
    }
}
