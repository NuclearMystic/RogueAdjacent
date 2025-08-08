// This code has been written by AHMET ALP for the Unity Asset "AA Save and Load System".
// Link to the asset store page: https://u3d.as/2TxY
// Publisher contact: ahmetalp.business@gmail.com

namespace AASave
{
    [System.Serializable]
    public class StringBlock
    {
        public string dataName, value = "";

        public StringBlock(string dataName, string value, bool encryptData)
        {
            if (encryptData)
            {
                this.dataName = AAEncryption.Encrypt(dataName);
                this.value = AAEncryption.Encrypt(value);
            }
            else
            {
                this.dataName = dataName;
                this.value = value;
            }
        }
    }
}
