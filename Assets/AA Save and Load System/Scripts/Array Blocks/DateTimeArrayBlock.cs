// This code has been written by AHMET ALP for the Unity Asset "AA Save and Load System".
// Link to the asset store page: https://u3d.as/2TxY
// Publisher contact: ahmetalp.business@gmail.com

using System;

namespace AASave
{
    [System.Serializable]
    public class DateTimeArrayBlock
    {
        public string dataName;
        public string[] value;

        public DateTimeArrayBlock(string dataName, DateTime[] value, bool encryptData)
        {
            if (encryptData)
            {
                this.dataName = AAEncryption.Encrypt(dataName);
                this.value = new string[value.Length];

                for (int i = 0; i < value.Length; i++)
                {
                    this.value[i] = "";
                    this.value[i] += value[i].Year.ToString() + "#";
                    this.value[i] += value[i].Month.ToString() + "#";
                    this.value[i] += value[i].Day.ToString() + "#";
                    this.value[i] += value[i].Hour.ToString() + "#";
                    this.value[i] += value[i].Minute.ToString() + "#";
                    this.value[i] += value[i].Second.ToString() + "#";
                    this.value[i] += value[i].Millisecond.ToString() + "#";
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
                    this.value[i] += value[i].Year.ToString() + "#";
                    this.value[i] += value[i].Month.ToString() + "#";
                    this.value[i] += value[i].Day.ToString() + "#";
                    this.value[i] += value[i].Hour.ToString() + "#";
                    this.value[i] += value[i].Minute.ToString() + "#";
                    this.value[i] += value[i].Second.ToString() + "#";
                    this.value[i] += value[i].Millisecond.ToString() + "#";
                }
            }
        }
    }
}
