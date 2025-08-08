// This code has been written by AHMET ALP for the Unity Asset "AA Save and Load System".
// Link to the asset store page: https://u3d.as/2TxY
// Publisher contact: ahmetalp.business@gmail.com

using System;

namespace AASave
{
    [System.Serializable]
    public class TimeSpanBlock
    {
        public string dataName, value = "";

        public TimeSpanBlock(string dataName, TimeSpan value, bool encryptData)
        {
            if (encryptData)
            {
                this.dataName = AAEncryption.Encrypt(dataName);
                this.value = AAEncryption.Encrypt(value.Ticks.ToString());
            }
            else
            {
                this.dataName = dataName;
                this.value = value.Ticks.ToString();
            }
        }
    }
}
