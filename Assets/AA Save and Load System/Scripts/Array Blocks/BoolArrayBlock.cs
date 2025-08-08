// This code has been written by AHMET ALP for the Unity Asset "AA Save and Load System".
// Link to the asset store page: https://u3d.as/2TxY
// Publisher contact: ahmetalp.business@gmail.com

namespace AASave
{
    [System.Serializable]
    public class BoolArrayBlock
    {
        public string dataName;
        public string[] value;

        public BoolArrayBlock(string dataName, bool[] value, bool encryptData)
        {
            if (encryptData)
            {
                this.dataName = AAEncryption.Encrypt(dataName);
            }
            else
            {
                this.dataName = dataName;
            }
            
            this.value = new string[value.Length];

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i])
                {
                    if (encryptData)
                    {
                        this.value[i] = AAEncryption.Encrypt("true");
                    }
                    else
                    {
                        this.value[i] = "true";
                    }
                }
                else
                {
                    if (encryptData)
                    {
                        this.value[i] = AAEncryption.Encrypt("false");
                    }
                    else
                    {
                        this.value[i] = "false";
                    }
                }
            }
        }
    }
}
