// This code has been written by AHMET ALP for the Unity Asset "AA Save and Load System".
// Link to the asset store page: https://u3d.as/2TxY
// Publisher contact: ahmetalp.business@gmail.com

namespace AASave
{
    [System.Serializable]
    public class BoolBlock
    {
        public string dataName;
        public string value;

        public BoolBlock(string dataName, bool value, bool encryptData)
        {
            if (encryptData)
            {
                this.dataName = AAEncryption.Encrypt(dataName);

                if (value)
                {
                    this.value = AAEncryption.Encrypt("true");
                }
                else
                {
                    this.value = AAEncryption.Encrypt("false");
                }
            }
            else
            {
                this.dataName = dataName;

                if (value)
                {
                    this.value = "true";
                }
                else
                {
                    this.value = "false";
                }
            }
        }
    }
}
