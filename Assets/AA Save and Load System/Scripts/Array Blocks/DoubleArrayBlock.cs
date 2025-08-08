// This code has been written by AHMET ALP for the Unity Asset "AA Save and Load System".
// Link to the asset store page: https://u3d.as/2TxY
// Publisher contact: ahmetalp.business@gmail.com

namespace AASave
{
    [System.Serializable]
    public class DoubleArrayBlock
    {
        public string dataName;
        public string[] value;

        public DoubleArrayBlock(string dataName, double[] value, bool encryptData)
        {
            if (encryptData)
            {
                this.dataName = AAEncryption.Encrypt(dataName);
                this.value = new string[value.Length];

                for (int i = 0; i < value.Length; i++)
                {
                    this.value[i] = AAEncryption.Encrypt(value[i].ToString());
                }
            }
            else
            {
                this.dataName = dataName;
                this.value = new string[value.Length];

                for (int i = 0; i < value.Length; i++)
                {
                    this.value[i] = value[i].ToString();
                }
            }
        }
    }
}
