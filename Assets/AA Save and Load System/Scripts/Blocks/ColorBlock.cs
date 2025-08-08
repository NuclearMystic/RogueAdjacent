// This code has been written by AHMET ALP for the Unity Asset "AA Save and Load System".
// Link to the asset store page: https://u3d.as/2TxY
// Publisher contact: ahmetalp.business@gmail.com

namespace AASave
{
    [System.Serializable]
    public class ColorBlock
    {
        public string dataName, value = "";

        public ColorBlock(string dataName, float red, float green, float blue, float alpha, bool encryptData)
        {
            if (encryptData)
            {
                this.dataName = AAEncryption.Encrypt(dataName);

                value = "";
                value += red.ToString() + "#";
                value += green.ToString() + "#";
                value += blue.ToString() + "#";
                value += alpha.ToString() + "#";
                value = AAEncryption.Encrypt(value);
            }
            else
            {
                this.dataName = dataName;

                value = "";
                value += red.ToString() + "#";
                value += green.ToString() + "#";
                value += blue.ToString() + "#";
                value += alpha.ToString() + "#";
            }
        }
    }
}
