// This code has been written by AHMET ALP for the Unity Asset "AA Save and Load System".
// Link to the asset store page: https://u3d.as/2TxY
// Publisher contact: ahmetalp.business@gmail.com

using System;

namespace AASave
{
    [System.Serializable]
    public class DateTimeBlock
    {
        public string dataName, value = "";

        public DateTimeBlock(string dataName, DateTime value, bool encryptData)
        {
            if (encryptData)
            {
                this.dataName = AAEncryption.Encrypt(dataName);

                this.value = "";
                this.value += value.Year.ToString() + "#";
                this.value += value.Month.ToString() + "#";
                this.value += value.Day.ToString() + "#";
                this.value += value.Hour.ToString() + "#";
                this.value += value.Minute.ToString() + "#";
                this.value += value.Second.ToString() + "#";
                this.value += value.Millisecond.ToString() + "#";
                this.value = AAEncryption.Encrypt(this.value);
            }
            else
            {
                this.dataName = dataName;

                this.value = "";
                this.value += value.Year.ToString() + "#";
                this.value += value.Month.ToString() + "#";
                this.value += value.Day.ToString() + "#";
                this.value += value.Hour.ToString() + "#";
                this.value += value.Minute.ToString() + "#";
                this.value += value.Second.ToString() + "#";
                this.value += value.Millisecond.ToString() + "#";
            }
        }
    }
}
