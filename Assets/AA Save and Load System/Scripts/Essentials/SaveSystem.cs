// This code has been written by AHMET ALP for the Unity Asset "AA Save and Load System".
// Link to the asset store page: https://u3d.as/2TxY
// Publisher contact: ahmetalp.business@gmail.com

using UnityEngine;
using System.IO;
using System;
using System.Text;
using System.Linq;

namespace AASave
{
    public class SaveSystem : MonoBehaviour
    {
        [Tooltip("If this is checked, this component and the GameObject will not be destroyed while loading a new scene.")] public bool dontDestroyOnLoad = false;
        [Tooltip("The file path where the save files will be located.")] public FileLocations fileLocation = FileLocations.PersistentDataPath;
        [Tooltip("The custom file path where the save files will be located.")] public string customFilePath = "";
        [Tooltip("If this is checked, sub folders can be created on the target save location to store the save files.")] public bool subFolder = false;
        [Tooltip("Name of the sub folder.")] public string subFolderName = "Save Slot 1";
        [Tooltip("The extension of the save files.")] public string fileExtension = "cy548xow";
        [Tooltip("If this is true, the game data will be encrypted before it is saved on the player's device.")] public bool encryptData = true;

        private readonly string consoleLogPrefix = "<color=#3de32d>AA Save and Load System : </color>";

        private void Awake()
        {
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        /// <summary>
        /// Returns the full file path to the save file for the given game data.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="dataType">Type of the game data.</param>
        /// <returns>Full file path to the target save file.</returns>
        private string GetFullFilePath(string dataName, DataTypes dataType)
        {
            string filePath = "";

            if (fileLocation == FileLocations.PersistentDataPath)
                filePath = Application.persistentDataPath.ToString() + "/";
            else if (fileLocation == FileLocations.ApplicationDataPath)
                filePath = Application.dataPath.ToString() + "/";
            else if (fileLocation == FileLocations.TemporaryCachePath)
                filePath = Application.temporaryCachePath.ToString() + "/";
            else if (fileLocation == FileLocations.StreamingAssetsPath)
                filePath = Application.streamingAssetsPath.ToString() + "/";
            else if (fileLocation == FileLocations.CustomPath)
                filePath = customFilePath + "/";

            if (subFolder)
            {
                if (!string.IsNullOrEmpty(subFolderName) && !string.IsNullOrWhiteSpace(subFolderName))
                {
                    filePath += subFolderName + "/";

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                }
                else
                {
                    Debug.LogError(consoleLogPrefix + "Sub folder name cannot be empty or white space.\n");
                    return "";
                }
            }

            if (dataType == DataTypes.Bool)
                filePath += "aa";
            else if (dataType == DataTypes.BoolArray)
                filePath += "ba";
            else if (dataType == DataTypes.Byte)
                filePath += "ab";
            else if (dataType == DataTypes.ByteArray)
                filePath += "bb";
            else if (dataType == DataTypes.Char)
                filePath += "ac";
            else if (dataType == DataTypes.CharArray)
                filePath += "bc";
            else if (dataType == DataTypes.Color)
                filePath += "ad";
            else if (dataType == DataTypes.ColorArray)
                filePath += "bd";
            else if (dataType == DataTypes.DateTime)
                filePath += "at";
            else if (dataType == DataTypes.DateTimeArray)
                filePath += "bt";
            else if (dataType == DataTypes.Decimal)
                filePath += "ae";
            else if (dataType == DataTypes.DecimalArray)
                filePath += "be";
            else if (dataType == DataTypes.Double)
                filePath += "af";
            else if (dataType == DataTypes.DoubleArray)
                filePath += "bf";
            else if (dataType == DataTypes.Float)
                filePath += "ag";
            else if (dataType == DataTypes.FloatArray)
                filePath += "bg";
            else if (dataType == DataTypes.Int)
                filePath += "ah";
            else if (dataType == DataTypes.IntArray)
                filePath += "bh";
            else if (dataType == DataTypes.Long)
                filePath += "ai";
            else if (dataType == DataTypes.LongArray)
                filePath += "bi";
            else if (dataType == DataTypes.Quaternion)
                filePath += "aj";
            else if (dataType == DataTypes.QuaternionArray)
                filePath += "bj";
            else if (dataType == DataTypes.Sbyte)
                filePath += "ak";
            else if (dataType == DataTypes.SbyteArray)
                filePath += "bk";
            else if (dataType == DataTypes.Short)
                filePath += "al";
            else if (dataType == DataTypes.ShortArray)
                filePath += "bl";
            else if (dataType == DataTypes.String)
                filePath += "am";
            else if (dataType == DataTypes.StringArray)
                filePath += "bm";
            else if (dataType == DataTypes.TimeSpan)
                filePath += "au";
            else if (dataType == DataTypes.TimeSpanArray)
                filePath += "bu";
            else if (dataType == DataTypes.Uint)
                filePath += "an";
            else if (dataType == DataTypes.UintArray)
                filePath += "bn";
            else if (dataType == DataTypes.Ulong)
                filePath += "ao";
            else if (dataType == DataTypes.UlongArray)
                filePath += "bo";
            else if (dataType == DataTypes.Ushort)
                filePath += "ap";
            else if (dataType == DataTypes.UshortArray)
                filePath += "bp";
            else if (dataType == DataTypes.Vector2)
                filePath += "aq";
            else if (dataType == DataTypes.Vector2Array)
                filePath += "bq";
            else if (dataType == DataTypes.Vector2Int)
                filePath += "av";
            else if (dataType == DataTypes.Vector2IntArray)
                filePath += "bv";
            else if (dataType == DataTypes.Vector3)
                filePath += "ar";
            else if (dataType == DataTypes.Vector3Array)
                filePath += "br";
            else if (dataType == DataTypes.Vector3Int)
                filePath += "ax";
            else if (dataType == DataTypes.Vector3IntArray)
                filePath += "bx";
            else if (dataType == DataTypes.Vector4)
                filePath += "as";
            else if (dataType == DataTypes.Vector4Array)
                filePath += "bs";

            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));
            filePath += string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24);
            filePath += "." + fileExtension;
            return filePath;
        }

        /// <summary>
        /// Returns the general save location for all the game data who is using this Save System component.
        /// </summary>
        /// <returns>General save location.</returns>
        private string GetSaveLocation()
        {
            string saveLocation = "";

            if (fileLocation == FileLocations.PersistentDataPath)
                saveLocation = Application.persistentDataPath.ToString() + "/";
            else if (fileLocation == FileLocations.ApplicationDataPath)
                saveLocation = Application.dataPath.ToString() + "/";
            else if (fileLocation == FileLocations.TemporaryCachePath)
                saveLocation = Application.temporaryCachePath.ToString() + "/";
            else if (fileLocation == FileLocations.StreamingAssetsPath)
                saveLocation = Application.streamingAssetsPath.ToString() + "/";
            else if (fileLocation == FileLocations.CustomPath)
                saveLocation = customFilePath + "/";

            if (subFolder)
            {
                if (!string.IsNullOrEmpty(subFolderName) && !string.IsNullOrWhiteSpace(subFolderName))
                {
                    saveLocation += subFolderName + "/";

                    if (!Directory.Exists(saveLocation))
                    {
                        Directory.CreateDirectory(saveLocation);
                    }
                }
                else
                {
                    Debug.LogError(consoleLogPrefix + "Sub folder name cannot be empty or white space.\n");
                    return "";
                }
            }

            return saveLocation;
        }

        /// <summary>
        /// Finds out if the game data with the given name exists in the save file location or not.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <returns>Returns true if the game data exists, returns false is the game data doesn't exists.</returns>
        public bool DoesDataExists(string dataName)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Finds out if the name of the game data is appropriate to use or not.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <returns>Returns true if the game data name can be used, returns false is the game data name can't be used.</returns>
        public bool IsDataNameAppropriate(string dataName)
        {
            if (String.IsNullOrEmpty(dataName) || String.IsNullOrWhiteSpace(dataName))
            {
                Debug.LogWarning(consoleLogPrefix + "Name of the game data cannot not be blank.\n");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public bool Save(string dataName, bool value)
        {
            string tempFilePath = GetFullFilePath(dataName, DataTypes.Bool);
            AASaver.SaveBool(dataName, value, tempFilePath, encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public byte Save(string dataName, byte value)
        {
            string tempFilePath = GetFullFilePath(dataName, DataTypes.Byte);
            AASaver.SaveByte(dataName, value, tempFilePath, encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public char Save(string dataName, char value)
        {
            string tempFilePath = GetFullFilePath(dataName, DataTypes.Char);
            AASaver.SaveChar(dataName, value, tempFilePath, encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public Color Save(string dataName, Color value)
        {
            string tempFilePath = GetFullFilePath(dataName, DataTypes.Color);
            AASaver.SaveColor(dataName, value.r, value.g, value.b, value.a, tempFilePath, encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">The name of the game data.</param>
        /// <param name="value">The value of the game data.</param>
        /// <returns>The value of the game data.</returns>
        public DateTime Save(string dataName, DateTime value)
        {
            string tempFilePath = GetFullFilePath(dataName, DataTypes.DateTime);
            AASaver.SaveDateTime(dataName, value, tempFilePath, encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public decimal Save(string dataName, decimal value)
        {
            string tempFilePath = GetFullFilePath(dataName, DataTypes.Decimal);
            AASaver.SaveDecimal(dataName, value, tempFilePath, encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public double Save(string dataName, double value)
        {
            string tempFilePath = GetFullFilePath(dataName, DataTypes.Double);
            AASaver.SaveDouble(dataName, value, tempFilePath, encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public float Save(string dataName, float value)
        {
            string tempFilePath = GetFullFilePath(dataName, DataTypes.Float);
            AASaver.SaveFloat(dataName, value, tempFilePath, encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public int Save(string dataName, int value)
        {
            string tempFilePath = GetFullFilePath(dataName, DataTypes.Int);
            AASaver.SaveInt(dataName, value, tempFilePath, encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public long Save(string dataName, long value)
        {
            string tempFilePath = GetFullFilePath(dataName, DataTypes.Long);
            AASaver.SaveLong(dataName, value, tempFilePath, encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public Quaternion Save(string dataName, Quaternion value)
        {
            string tempFilePath = GetFullFilePath(dataName, DataTypes.Quaternion);
            AASaver.SaveQuaternion(dataName, value.x, value.y, value.z, value.w, tempFilePath, encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public sbyte Save(string dataName, sbyte value)
        {
            string tempFilePath = GetFullFilePath(dataName, DataTypes.Sbyte);
            AASaver.SaveSbyte(dataName, value, tempFilePath, encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public short Save(string dataName, short value)
        {
            string tempFilePath = GetFullFilePath(dataName, DataTypes.Short);
            AASaver.SaveShort(dataName, value, tempFilePath, encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public string Save(string dataName, string value)
        {
            string tempFilePath = GetFullFilePath(dataName, DataTypes.String);
            AASaver.SaveString(dataName, value, tempFilePath, encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">The name of the game data.</param>
        /// <param name="value">The value of the game data.</param>
        /// <returns>The value of the game data.</returns>
        public TimeSpan Save(string dataName, TimeSpan value)
        {
            string tempFilePath = GetFullFilePath(dataName, DataTypes.TimeSpan);
            AASaver.SaveTimeSpan(dataName, value, tempFilePath, encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public uint Save(string dataName, uint value)
        {
            string tempFilePath = GetFullFilePath(dataName, DataTypes.Uint);
            AASaver.SaveUint(dataName, value, tempFilePath, encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public ulong Save(string dataName, ulong value)
        {
            string tempFilePath = GetFullFilePath(dataName, DataTypes.Ulong);
            AASaver.SaveUlong(dataName, value, tempFilePath, encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public ushort Save(string dataName, ushort value)
        {
            string tempFilePath = GetFullFilePath(dataName, DataTypes.Ushort);
            AASaver.SaveUshort(dataName, value, tempFilePath, encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public Vector2 Save(string dataName, Vector2 value)
        {
            string tempFilePath = GetFullFilePath(dataName, DataTypes.Vector2);
            AASaver.SaveVector2(dataName, value.x, value.y, tempFilePath, encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public Vector2Int Save(string dataName, Vector2Int value)
        {
            string tempFilePath = GetFullFilePath(dataName, DataTypes.Vector2Int);
            AASaver.SaveVector2Int(dataName, value.x, value.y, tempFilePath, encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public Vector3 Save(string dataName, Vector3 value)
        {
            string tempFilePath = GetFullFilePath(dataName, DataTypes.Vector3);
            AASaver.SaveVector3(dataName, value.x, value.y, value.z, tempFilePath, encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public Vector3Int Save(string dataName, Vector3Int value)
        {
            string tempFilePath = GetFullFilePath(dataName, DataTypes.Vector3Int);
            AASaver.SaveVector3Int(dataName, value.x, value.y, value.z, tempFilePath, encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public Vector4 Save(string dataName, Vector4 value)
        {
            string tempFilePath = GetFullFilePath(dataName, DataTypes.Vector4);
            AASaver.SaveVector4(dataName, value.x, value.y, value.z, value.w, tempFilePath, encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public bool[] Save(string dataName, bool[] value)
        {
            AASaver.SaveBoolArray(dataName, value, GetFullFilePath(dataName, DataTypes.BoolArray), encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public byte[] Save(string dataName, byte[] value)
        {
            AASaver.SaveByteArray(dataName, value, GetFullFilePath(dataName, DataTypes.ByteArray), encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public char[] Save(string dataName, char[] value)
        {
            AASaver.SaveCharArray(dataName, value, GetFullFilePath(dataName, DataTypes.CharArray), encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public Color[] Save(string dataName, Color[] value)
        {
            AASaver.SaveColorArray(dataName, value, GetFullFilePath(dataName, DataTypes.ColorArray), encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public DateTime[] Save(string dataName, DateTime[] value)
        {
            AASaver.SaveDateTimeArray(dataName, value, GetFullFilePath(dataName, DataTypes.DateTimeArray), encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public decimal[] Save(string dataName, decimal[] value)
        {
            AASaver.SaveDecimalArray(dataName, value, GetFullFilePath(dataName, DataTypes.DecimalArray), encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public double[] Save(string dataName, double[] value)
        {
            AASaver.SaveDoubleArray(dataName, value, GetFullFilePath(dataName, DataTypes.DoubleArray), encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public float[] Save(string dataName, float[] value)
        {
            AASaver.SaveFloatArray(dataName, value, GetFullFilePath(dataName, DataTypes.FloatArray), encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public int[] Save(string dataName, int[] value)
        {
            AASaver.SaveIntArray(dataName, value, GetFullFilePath(dataName, DataTypes.IntArray), encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public long[] Save(string dataName, long[] value)
        {
            AASaver.SaveLongArray(dataName, value, GetFullFilePath(dataName, DataTypes.LongArray), encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public Quaternion[] Save(string dataName, Quaternion[] value)
        {
            AASaver.SaveQuaternionArray(dataName, value, GetFullFilePath(dataName, DataTypes.QuaternionArray), encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public sbyte[] Save(string dataName, sbyte[] value)
        {
            AASaver.SaveSbyteArray(dataName, value, GetFullFilePath(dataName, DataTypes.SbyteArray), encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public short[] Save(string dataName, short[] value)
        {
            AASaver.SaveShortArray(dataName, value, GetFullFilePath(dataName, DataTypes.ShortArray), encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public string[] Save(string dataName, string[] value)
        {
            AASaver.SaveStringArray(dataName, value, GetFullFilePath(dataName, DataTypes.StringArray), encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public TimeSpan[] Save(string dataName, TimeSpan[] value)
        {
            AASaver.SaveTimeSpanArray(dataName, value, GetFullFilePath(dataName, DataTypes.TimeSpanArray), encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public uint[] Save(string dataName, uint[] value)
        {
            AASaver.SaveUintArray(dataName, value, GetFullFilePath(dataName, DataTypes.UintArray), encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public ulong[] Save(string dataName, ulong[] value)
        {
            AASaver.SaveUlongArray(dataName, value, GetFullFilePath(dataName, DataTypes.UlongArray), encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public ushort[] Save(string dataName, ushort[] value)
        {
            AASaver.SaveUshortArray(dataName, value, GetFullFilePath(dataName, DataTypes.UshortArray), encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public Vector2[] Save(string dataName, Vector2[] value)
        {
            AASaver.SaveVector2Array(dataName, value, GetFullFilePath(dataName, DataTypes.Vector2Array), encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public Vector2Int[] Save(string dataName, Vector2Int[] value)
        {
            AASaver.SaveVector2IntArray(dataName, value, GetFullFilePath(dataName, DataTypes.Vector2IntArray), encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public Vector3[] Save(string dataName, Vector3[] value)
        {
            AASaver.SaveVector3Array(dataName, value, GetFullFilePath(dataName, DataTypes.Vector3Array), encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public Vector3Int[] Save(string dataName, Vector3Int[] value)
        {
            AASaver.SaveVector3IntArray(dataName, value, GetFullFilePath(dataName, DataTypes.Vector3IntArray), encryptData);
            return value;
        }

        /// <summary>
        /// Saves the game data to the player's device and returns the value of the game data. If the save file for the game data exists, it overrides it. If the save file for the game data does not exists, it creates a new save file and writes the data on it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <returns>Value of the game data.</returns>
        public Vector4[] Save(string dataName, Vector4[] value)
        {
            AASaver.SaveVector4Array(dataName, value, GetFullFilePath(dataName, DataTypes.Vector4Array), encryptData);
            return value;
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. This method returns the data as a string. You can use .AsX methods to get the data in the requested data type.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <returns>Value of the game data as a string. If the game data with the given name doesn't exists, it returns an empty string.</returns>
        public string Load(string dataName)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "", readValue = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                Debug.LogWarning(consoleLogPrefix + "Game data with the given name could not be found: <b>" + dataName + "</b>\n");
                return "";
            }
            else
            {
                if (fullFileName.StartsWith("aa"))
                {
                    // Boolean. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValue = AAEncryption.Decrypt(AASaver.LoadBool(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValue = AASaver.LoadBool(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("ab"))
                {
                    // Byte. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValue = AAEncryption.Decrypt(AASaver.LoadByte(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValue = AASaver.LoadByte(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("ac"))
                {
                    // Char. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValue = AAEncryption.Decrypt(AASaver.LoadChar(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValue = AASaver.LoadChar(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("ad"))
                {
                    // Color. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValue = AAEncryption.Decrypt(AASaver.LoadColor(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValue = AASaver.LoadColor(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("at"))
                {
                    // DateTime. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValue = AAEncryption.Decrypt(AASaver.LoadDateTime(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValue = AASaver.LoadDateTime(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("ae"))
                {
                    // Decimal. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValue = AAEncryption.Decrypt(AASaver.LoadDecimal(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValue = AASaver.LoadDecimal(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("af"))
                {
                    // Double. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValue = AAEncryption.Decrypt(AASaver.LoadDouble(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValue = AASaver.LoadDouble(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("ag"))
                {
                    // Float. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValue = AAEncryption.Decrypt(AASaver.LoadFloat(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValue = AASaver.LoadFloat(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("ah"))
                {
                    // Int. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValue = AAEncryption.Decrypt(AASaver.LoadInt(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValue = AASaver.LoadInt(GetSaveLocation() + fullFileName).value;
                    }
                    
                }
                else if (fullFileName.StartsWith("ai"))
                {
                    // Long. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValue = AAEncryption.Decrypt(AASaver.LoadLong(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValue = AASaver.LoadLong(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("aj"))
                {
                    // Quaternion. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValue = AAEncryption.Decrypt(AASaver.LoadQuaternion(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValue = AASaver.LoadQuaternion(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("ak"))
                {
                    // Sbyte. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValue = AAEncryption.Decrypt(AASaver.LoadSbyte(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValue = AASaver.LoadSbyte(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("al"))
                {
                    // Short. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValue = AAEncryption.Decrypt(AASaver.LoadShort(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValue = AASaver.LoadShort(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("am"))
                {
                    // String. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValue = AAEncryption.Decrypt(AASaver.LoadString(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValue = AASaver.LoadString(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("au"))
                {
                    // TimeSpan. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValue = AAEncryption.Decrypt(AASaver.LoadTimeSpan(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValue = AASaver.LoadTimeSpan(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("an"))
                {
                    // Uint. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValue = AAEncryption.Decrypt(AASaver.LoadUint(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValue = AASaver.LoadUint(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("ao"))
                {
                    // Ulong. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValue = AAEncryption.Decrypt(AASaver.LoadUlong(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValue = AASaver.LoadUlong(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("ap"))
                {
                    // Ushort. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValue = AAEncryption.Decrypt(AASaver.LoadUshort(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValue = AASaver.LoadUshort(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("aq"))
                {
                    // Vector2. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValue = AAEncryption.Decrypt(AASaver.LoadVector2(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValue = AASaver.LoadVector2(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("av"))
                {
                    // Vector2Int. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValue = AAEncryption.Decrypt(AASaver.LoadVector2Int(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValue = AASaver.LoadVector2Int(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("ar"))
                {
                    // Vector3. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValue = AAEncryption.Decrypt(AASaver.LoadVector3(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValue = AASaver.LoadVector3(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("ax"))
                {
                    // Vector3Int. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValue = AAEncryption.Decrypt(AASaver.LoadVector3Int(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValue = AASaver.LoadVector3Int(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("as"))
                {
                    // Vector4. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValue = AAEncryption.Decrypt(AASaver.LoadVector4(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValue = AASaver.LoadVector4(GetSaveLocation() + fullFileName).value;
                    }
                }

                return readValue;
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as boolean. If the game data doesn't exists, it will return the default value.</returns>
        public bool Load(string dataName, bool backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("aa*." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "", readValue = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveBool(dataName, backupValue, GetFullFilePath(dataName, DataTypes.Bool), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    readValue = AAEncryption.Decrypt(AASaver.LoadBool(GetSaveLocation() + fullFileName).value);
                else
                    readValue = AASaver.LoadBool(GetSaveLocation() + fullFileName).value;

                if (readValue.Equals("true"))
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as byte. If the game data doesn't exists, it will return the default value.</returns>
        public byte Load(string dataName, byte backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveByte(dataName, backupValue, GetFullFilePath(dataName, DataTypes.Byte), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadByte(GetSaveLocation() + fullFileName).value).AsByte();
                else
                    return AASaver.LoadByte(GetSaveLocation() + fullFileName).value.AsByte();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as char. If the game data doesn't exists, it will return the default value.</returns>
        public char Load(string dataName, char backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveChar(dataName, backupValue, GetFullFilePath(dataName, DataTypes.Char), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadChar(GetSaveLocation() + fullFileName).value).AsChar();
                else
                    return AASaver.LoadChar(GetSaveLocation() + fullFileName).value.AsChar();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as color. If the game data doesn't exists, it will return the default value.</returns>
        public Color Load(string dataName, Color backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveColor(dataName, backupValue.r, backupValue.g, backupValue.b, backupValue.a, GetFullFilePath(dataName, DataTypes.Color), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadColor(GetSaveLocation() + fullFileName).value).AsColor();
                else
                    return AASaver.LoadColor(GetSaveLocation() + fullFileName).value.AsColor();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as DateTime. If the game data doesn't exists, it will return the default value.</returns>
        public DateTime Load(string dataName, DateTime backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveDateTime(dataName, backupValue, GetFullFilePath(dataName, DataTypes.DateTime), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadDateTime(GetSaveLocation() + fullFileName).value).AsDateTime();
                else
                    return AASaver.LoadDateTime(GetSaveLocation() + fullFileName).value.AsDateTime();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as decimal. If the game data doesn't exists, it will return the default value.</returns>
        public decimal Load(string dataName, decimal backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveDecimal(dataName, backupValue, GetFullFilePath(dataName, DataTypes.Decimal), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadDecimal(GetSaveLocation() + fullFileName).value).AsDecimal();
                else
                    return AASaver.LoadDecimal(GetSaveLocation() + fullFileName).value.AsDecimal();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as double. If the game data doesn't exists, it will return the default value.</returns>
        public double Load(string dataName, double backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveDouble(dataName, backupValue, GetFullFilePath(dataName, DataTypes.Double), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadDouble(GetSaveLocation() + fullFileName).value).AsDouble();
                else
                    return AASaver.LoadDouble(GetSaveLocation() + fullFileName).value.AsDouble();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as float. If the game data doesn't exists, it will return the default value.</returns>
        public float Load(string dataName, float backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveFloat(dataName, backupValue, GetFullFilePath(dataName, DataTypes.Float), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadFloat(GetSaveLocation() + fullFileName).value).AsFloat();
                else
                    return AASaver.LoadFloat(GetSaveLocation() + fullFileName).value.AsFloat();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as int. If the game data doesn't exists, it will return the default value.</returns>
        public int Load(string dataName, int backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveInt(dataName, backupValue, GetFullFilePath(dataName, DataTypes.Int), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadInt(GetSaveLocation() + fullFileName).value).AsInt();
                else
                    return AASaver.LoadInt(GetSaveLocation() + fullFileName).value.AsInt();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as long. If the game data doesn't exists, it will return the default value.</returns>
        public long Load(string dataName, long backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveLong(dataName, backupValue, GetFullFilePath(dataName, DataTypes.Long), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadLong(GetSaveLocation() + fullFileName).value).AsLong();
                else
                    return AASaver.LoadLong(GetSaveLocation() + fullFileName).value.AsLong();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as quaternion. If the game data doesn't exists, it will return the default value.</returns>
        public Quaternion Load(string dataName, Quaternion backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveQuaternion(dataName, backupValue.x, backupValue.y, backupValue.z, backupValue.w, GetFullFilePath(dataName, DataTypes.Quaternion), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadQuaternion(GetSaveLocation() + fullFileName).value).AsQuaternion();
                else
                    return AASaver.LoadQuaternion(GetSaveLocation() + fullFileName).value.AsQuaternion();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as sbyte. If the game data doesn't exists, it will return the default value.</returns>
        public sbyte Load(string dataName, sbyte backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveSbyte(dataName, backupValue, GetFullFilePath(dataName, DataTypes.Sbyte), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadSbyte(GetSaveLocation() + fullFileName).value).AsSbyte();
                else
                    return AASaver.LoadSbyte(GetSaveLocation() + fullFileName).value.AsSbyte();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as short. If the game data doesn't exists, it will return the default value.</returns>
        public short Load(string dataName, short backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveShort(dataName, backupValue, GetFullFilePath(dataName, DataTypes.Short), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadShort(GetSaveLocation() + fullFileName).value).AsShort();
                else
                    return AASaver.LoadShort(GetSaveLocation() + fullFileName).value.AsShort();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as string. If the game data doesn't exists, it will return the default value.</returns>
        public string Load(string dataName, string backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveString(dataName, backupValue, GetFullFilePath(dataName, DataTypes.String), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadString(GetSaveLocation() + fullFileName).value).AsString();
                else
                    return AASaver.LoadString(GetSaveLocation() + fullFileName).value.AsString();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as TimeSpan. If the game data doesn't exists, it will return the default value.</returns>
        public TimeSpan Load(string dataName, TimeSpan backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveTimeSpan(dataName, backupValue, GetFullFilePath(dataName, DataTypes.TimeSpan), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadTimeSpan(GetSaveLocation() + fullFileName).value).AsTimeSpan();
                else
                    return AASaver.LoadTimeSpan(GetSaveLocation() + fullFileName).value.AsTimeSpan();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as uint. If the game data doesn't exists, it will return the default value.</returns>
        public uint Load(string dataName, uint backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveUint(dataName, backupValue, GetFullFilePath(dataName, DataTypes.Uint), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadUint(GetSaveLocation() + fullFileName).value).AsUint();
                else
                    return AASaver.LoadUint(GetSaveLocation() + fullFileName).value.AsUint();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as ulong. If the game data doesn't exists, it will return the default value.</returns>
        public ulong Load(string dataName, ulong backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveUlong(dataName, backupValue, GetFullFilePath(dataName, DataTypes.Ulong), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadUlong(GetSaveLocation() + fullFileName).value).AsUlong();
                else
                    return AASaver.LoadUlong(GetSaveLocation() + fullFileName).value.AsUlong();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as ushort. If the game data doesn't exists, it will return the default value.</returns>
        public ushort Load(string dataName, ushort backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveUshort(dataName, backupValue, GetFullFilePath(dataName, DataTypes.Ushort), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadUshort(GetSaveLocation() + fullFileName).value).AsUshort();
                else
                    return AASaver.LoadUshort(GetSaveLocation() + fullFileName).value.AsUshort();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as Vector2. If the game data doesn't exists, it will return the default value.</returns>
        public Vector2 Load(string dataName, Vector2 backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveVector2(dataName, backupValue.x, backupValue.y, GetFullFilePath(dataName, DataTypes.Vector2), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadVector2(GetSaveLocation() + fullFileName).value).AsVector2();
                else
                    return AASaver.LoadVector2(GetSaveLocation() + fullFileName).value.AsVector2();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as Vector2Int. If the game data doesn't exists, it will return the default value.</returns>
        public Vector2Int Load(string dataName, Vector2Int backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveVector2Int(dataName, backupValue.x, backupValue.y, GetFullFilePath(dataName, DataTypes.Vector2Int), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadVector2Int(GetSaveLocation() + fullFileName).value).AsVector2Int();
                else
                    return AASaver.LoadVector2Int(GetSaveLocation() + fullFileName).value.AsVector2Int();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as Vector3. If the game data doesn't exists, it will return the default value.</returns>
        public Vector3 Load(string dataName, Vector3 backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveVector3(dataName, backupValue.x, backupValue.y, backupValue.z, GetFullFilePath(dataName, DataTypes.Vector3), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadVector3(GetSaveLocation() + fullFileName).value).AsVector3();
                else
                    return AASaver.LoadVector3(GetSaveLocation() + fullFileName).value.AsVector3();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as Vector3Int. If the game data doesn't exists, it will return the default value.</returns>
        public Vector3Int Load(string dataName, Vector3Int backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveVector3Int(dataName, backupValue.x, backupValue.y, backupValue.z, GetFullFilePath(dataName, DataTypes.Vector3Int), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadVector3Int(GetSaveLocation() + fullFileName).value).AsVector3Int();
                else
                    return AASaver.LoadVector3Int(GetSaveLocation() + fullFileName).value.AsVector3Int();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as Vector4. If the game data doesn't exists, it will return the default value.</returns>
        public Vector4 Load(string dataName, Vector4 backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveVector4(dataName, backupValue.x, backupValue.y, backupValue.z, backupValue.w, GetFullFilePath(dataName, DataTypes.Vector4), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadVector4(GetSaveLocation() + fullFileName).value).AsVector4();
                else
                    return AASaver.LoadVector4(GetSaveLocation() + fullFileName).value.AsVector4();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data returns the value of it. This method returns the data as a string. You can use .AsXArray methods to get the data in the requested data type.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <returns>Value of the game data as a string array. If the game data with the given name doesn't exists, it returns an empty string array.</returns>
        public string[] LoadArray(string dataName)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            string[] readValueArray = new string[0];
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                Debug.LogWarning(consoleLogPrefix + "Game data with the given name could not be found: <b>" + dataName + "</b>\n");
                return new string[0];
            }
            else
            {
                if (fullFileName.StartsWith("ba"))
                {
                    // Boolean Array. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValueArray = AAEncryption.Decrypt(AASaver.LoadBoolArray(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValueArray = AASaver.LoadBoolArray(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("bb"))
                {
                    // Byte Array. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValueArray = AAEncryption.Decrypt(AASaver.LoadByteArray(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValueArray = AASaver.LoadByteArray(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("bc"))
                {
                    // Char Array. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValueArray = AAEncryption.Decrypt(AASaver.LoadCharArray(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValueArray = AASaver.LoadCharArray(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("bd"))
                {
                    // Color Array. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValueArray = AAEncryption.Decrypt(AASaver.LoadColorArray(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValueArray = AASaver.LoadColorArray(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("bt"))
                {
                    // DateTime Array. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValueArray = AAEncryption.Decrypt(AASaver.LoadDateTimeArray(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValueArray = AASaver.LoadDateTimeArray(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("be"))
                {
                    // Decimal Array. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValueArray = AAEncryption.Decrypt(AASaver.LoadDecimalArray(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValueArray = AASaver.LoadDecimalArray(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("bf"))
                {
                    // Double Array. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValueArray = AAEncryption.Decrypt(AASaver.LoadDoubleArray(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValueArray = AASaver.LoadDoubleArray(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("bg"))
                {
                    // Float Array. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValueArray = AAEncryption.Decrypt(AASaver.LoadFloatArray(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValueArray = AASaver.LoadFloatArray(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("bh"))
                {
                    // Int Array. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValueArray = AAEncryption.Decrypt(AASaver.LoadIntArray(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValueArray = AASaver.LoadIntArray(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("bi"))
                {
                    // Long Array. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValueArray = AAEncryption.Decrypt(AASaver.LoadLongArray(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValueArray = AASaver.LoadLongArray(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("bj"))
                {
                    // Quaternion Array. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValueArray = AAEncryption.Decrypt(AASaver.LoadQuaternionArray(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValueArray = AASaver.LoadQuaternionArray(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("bk"))
                {
                    // Sbyte Array. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValueArray = AAEncryption.Decrypt(AASaver.LoadSbyteArray(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValueArray = AASaver.LoadSbyteArray(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("bl"))
                {
                    // Short Array. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValueArray = AAEncryption.Decrypt(AASaver.LoadShortArray(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValueArray = AASaver.LoadShortArray(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("bm"))
                {
                    // String Array. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValueArray = AAEncryption.Decrypt(AASaver.LoadStringArray(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValueArray = AASaver.LoadStringArray(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("bu"))
                {
                    // TimeSpan Array. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValueArray = AAEncryption.Decrypt(AASaver.LoadTimeSpanArray(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValueArray = AASaver.LoadTimeSpanArray(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("bn"))
                {
                    // Uint Array. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValueArray = AAEncryption.Decrypt(AASaver.LoadUintArray(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValueArray = AASaver.LoadUintArray(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("bo"))
                {
                    // Ulong Array. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValueArray = AAEncryption.Decrypt(AASaver.LoadUlongArray(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValueArray = AASaver.LoadUlongArray(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("bp"))
                {
                    // Ushort Array. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValueArray = AAEncryption.Decrypt(AASaver.LoadUshortArray(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValueArray = AASaver.LoadUshortArray(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("bq"))
                {
                    // Vector2 Array. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValueArray = AAEncryption.Decrypt(AASaver.LoadVector2Array(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValueArray = AASaver.LoadVector2Array(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("bv"))
                {
                    // Vector2 Int Array. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValueArray = AAEncryption.Decrypt(AASaver.LoadVector2IntArray(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValueArray = AASaver.LoadVector2IntArray(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("br"))
                {
                    // Vector3 Array. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValueArray = AAEncryption.Decrypt(AASaver.LoadVector3Array(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValueArray = AASaver.LoadVector3Array(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("bx"))
                {
                    // Vector3Int Array. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValueArray = AAEncryption.Decrypt(AASaver.LoadVector3IntArray(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValueArray = AASaver.LoadVector3IntArray(GetSaveLocation() + fullFileName).value;
                    }
                }
                else if (fullFileName.StartsWith("bs"))
                {
                    // Vector4 Array. Reads the value of the data and decrypts it.
                    if (encryptData)
                    {
                        readValueArray = AAEncryption.Decrypt(AASaver.LoadVector4Array(GetSaveLocation() + fullFileName).value);
                    }
                    else
                    {
                        readValueArray = AASaver.LoadVector4Array(GetSaveLocation() + fullFileName).value;
                    }
                }

                return readValueArray;
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as a boolean array. If the game data doesn't exists, it will return the default value.</returns>
        public bool[] LoadArray(string dataName, bool[] backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveBoolArray(dataName, backupValue, GetFullFilePath(dataName, DataTypes.BoolArray), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadBoolArray(GetSaveLocation() + fullFileName).value).AsBoolArray();
                else
                    return AASaver.LoadBoolArray(GetSaveLocation() + fullFileName).value.AsBoolArray();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as a byte array. If the game data doesn't exists, it will return the default value.</returns>
        public byte[] LoadArray(string dataName, byte[] backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveByteArray(dataName, backupValue, GetFullFilePath(dataName, DataTypes.ByteArray), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadByteArray(GetSaveLocation() + fullFileName).value).AsByteArray();
                else
                    return AASaver.LoadByteArray(GetSaveLocation() + fullFileName).value.AsByteArray();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as a char array. If the game data doesn't exists, it will return the default value.</returns>
        public char[] LoadArray(string dataName, char[] backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveCharArray(dataName, backupValue, GetFullFilePath(dataName, DataTypes.CharArray), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadCharArray(GetSaveLocation() + fullFileName).value).AsCharArray();
                else
                    return AASaver.LoadCharArray(GetSaveLocation() + fullFileName).value.AsCharArray();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as a Color array. If the game data doesn't exists, it will return the default value.</returns>
        public Color[] LoadArray(string dataName, Color[] backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveColorArray(dataName, backupValue, GetFullFilePath(dataName, DataTypes.ColorArray), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadColorArray(GetSaveLocation() + fullFileName).value).AsColorArray();
                else
                    return AASaver.LoadColorArray(GetSaveLocation() + fullFileName).value.AsColorArray();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as a DateTime array. If the game data doesn't exists, it will return the default value.</returns>
        public DateTime[] LoadArray(string dataName, DateTime[] backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveDateTimeArray(dataName, backupValue, GetFullFilePath(dataName, DataTypes.DateTimeArray), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadDateTimeArray(GetSaveLocation() + fullFileName).value).AsDateTimeArray();
                else
                    return AASaver.LoadDateTimeArray(GetSaveLocation() + fullFileName).value.AsDateTimeArray();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as a decimal array. If the game data doesn't exists, it will return the default value.</returns>
        public decimal[] LoadArray(string dataName, decimal[] backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveDecimalArray(dataName, backupValue, GetFullFilePath(dataName, DataTypes.DecimalArray), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadDecimalArray(GetSaveLocation() + fullFileName).value).AsDecimalArray();
                else
                    return AASaver.LoadDecimalArray(GetSaveLocation() + fullFileName).value.AsDecimalArray();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as a double array. If the game data doesn't exists, it will return the default value.</returns>
        public double[] LoadArray(string dataName, double[] backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveDoubleArray(dataName, backupValue, GetFullFilePath(dataName, DataTypes.DoubleArray), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadDoubleArray(GetSaveLocation() + fullFileName).value).AsDoubleArray();
                else
                    return AASaver.LoadDoubleArray(GetSaveLocation() + fullFileName).value.AsDoubleArray();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as a float array. If the game data doesn't exists, it will return the default value.</returns>
        public float[] LoadArray(string dataName, float[] backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveFloatArray(dataName, backupValue, GetFullFilePath(dataName, DataTypes.FloatArray), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadFloatArray(GetSaveLocation() + fullFileName).value).AsFloatArray();
                else
                    return AASaver.LoadFloatArray(GetSaveLocation() + fullFileName).value.AsFloatArray();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as an integer array. If the game data doesn't exists, it will return the default value.</returns>
        public int[] LoadArray(string dataName, int[] backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveIntArray(dataName, backupValue, GetFullFilePath(dataName, DataTypes.IntArray), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadIntArray(GetSaveLocation() + fullFileName).value).AsIntArray();
                else
                    return AASaver.LoadIntArray(GetSaveLocation() + fullFileName).value.AsIntArray();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as a long array. If the game data doesn't exists, it will return the default value.</returns>
        public long[] LoadArray(string dataName, long[] backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveLongArray(dataName, backupValue, GetFullFilePath(dataName, DataTypes.LongArray), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadLongArray(GetSaveLocation() + fullFileName).value).AsLongArray();
                else
                    return AASaver.LoadLongArray(GetSaveLocation() + fullFileName).value.AsLongArray();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as a Quaternion array. If the game data doesn't exists, it will return the default value.</returns>
        public Quaternion[] LoadArray(string dataName, Quaternion[] backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveQuaternionArray(dataName, backupValue, GetFullFilePath(dataName, DataTypes.QuaternionArray), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadQuaternionArray(GetSaveLocation() + fullFileName).value).AsQuaternionArray();
                else
                    return AASaver.LoadQuaternionArray(GetSaveLocation() + fullFileName).value.AsQuaternionArray();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as an sbyte array. If the game data doesn't exists, it will return the default value.</returns>
        public sbyte[] LoadArray(string dataName, sbyte[] backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveSbyteArray(dataName, backupValue, GetFullFilePath(dataName, DataTypes.SbyteArray), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadSbyteArray(GetSaveLocation() + fullFileName).value).AsSbyteArray();
                else
                    return AASaver.LoadSbyteArray(GetSaveLocation() + fullFileName).value.AsSbyteArray();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as a short array. If the game data doesn't exists, it will return the default value.</returns>
        public short[] LoadArray(string dataName, short[] backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveShortArray(dataName, backupValue, GetFullFilePath(dataName, DataTypes.ShortArray), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadShortArray(GetSaveLocation() + fullFileName).value).AsShortArray();
                else
                    return AASaver.LoadShortArray(GetSaveLocation() + fullFileName).value.AsShortArray();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as a string array. If the game data doesn't exists, it will return the default value.</returns>
        public string[] LoadArray(string dataName, string[] backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveStringArray(dataName, backupValue, GetFullFilePath(dataName, DataTypes.StringArray), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadStringArray(GetSaveLocation() + fullFileName).value).AsStringArray();
                else
                    return AASaver.LoadStringArray(GetSaveLocation() + fullFileName).value.AsStringArray();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as a TimeSpan array. If the game data doesn't exists, it will return the default value.</returns>
        public TimeSpan[] LoadArray(string dataName, TimeSpan[] backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveTimeSpanArray(dataName, backupValue, GetFullFilePath(dataName, DataTypes.TimeSpanArray), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadTimeSpanArray(GetSaveLocation() + fullFileName).value).AsTimeSpanArray();
                else
                    return AASaver.LoadTimeSpanArray(GetSaveLocation() + fullFileName).value.AsTimeSpanArray();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as an uint array. If the game data doesn't exists, it will return the default value.</returns>
        public uint[] LoadArray(string dataName, uint[] backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveUintArray(dataName, backupValue, GetFullFilePath(dataName, DataTypes.UintArray), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadUintArray(GetSaveLocation() + fullFileName).value).AsUintArray();
                else
                    return AASaver.LoadUintArray(GetSaveLocation() + fullFileName).value.AsUintArray();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as an ulong array. If the game data doesn't exists, it will return the default value.</returns>
        public ulong[] LoadArray(string dataName, ulong[] backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveUlongArray(dataName, backupValue, GetFullFilePath(dataName, DataTypes.UlongArray), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadUlongArray(GetSaveLocation() + fullFileName).value).AsUlongArray();
                else
                    return AASaver.LoadUlongArray(GetSaveLocation() + fullFileName).value.AsUlongArray();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as an ushort array. If the game data doesn't exists, it will return the default value.</returns>
        public ushort[] LoadArray(string dataName, ushort[] backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveUshortArray(dataName, backupValue, GetFullFilePath(dataName, DataTypes.UshortArray), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadUshortArray(GetSaveLocation() + fullFileName).value).AsUshortArray();
                else
                    return AASaver.LoadUshortArray(GetSaveLocation() + fullFileName).value.AsUshortArray();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as a Vector2 array. If the game data doesn't exists, it will return the default value.</returns>
        public Vector2[] LoadArray(string dataName, Vector2[] backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveVector2Array(dataName, backupValue, GetFullFilePath(dataName, DataTypes.Vector2Array), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadVector2Array(GetSaveLocation() + fullFileName).value).AsVector2Array();
                else
                    return AASaver.LoadVector2Array(GetSaveLocation() + fullFileName).value.AsVector2Array();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as a Vector2Int array. If the game data doesn't exists, it will return the default value.</returns>
        public Vector2Int[] LoadArray(string dataName, Vector2Int[] backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveVector2IntArray(dataName, backupValue, GetFullFilePath(dataName, DataTypes.Vector2IntArray), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadVector2IntArray(GetSaveLocation() + fullFileName).value).AsVector2IntArray();
                else
                    return AASaver.LoadVector2IntArray(GetSaveLocation() + fullFileName).value.AsVector2IntArray();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as a Vector3 array. If the game data doesn't exists, it will return the default value.</returns>
        public Vector3[] LoadArray(string dataName, Vector3[] backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveVector3Array(dataName, backupValue, GetFullFilePath(dataName, DataTypes.Vector3Array), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadVector3Array(GetSaveLocation() + fullFileName).value).AsVector3Array();
                else
                    return AASaver.LoadVector3Array(GetSaveLocation() + fullFileName).value.AsVector3Array();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as a Vector3Int array. If the game data doesn't exists, it will return the default value.</returns>
        public Vector3Int[] LoadArray(string dataName, Vector3Int[] backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveVector3IntArray(dataName, backupValue, GetFullFilePath(dataName, DataTypes.Vector3IntArray), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadVector3IntArray(GetSaveLocation() + fullFileName).value).AsVector3IntArray();
                else
                    return AASaver.LoadVector3IntArray(GetSaveLocation() + fullFileName).value.AsVector3IntArray();
            }
        }

        /// <summary>
        /// Reads the save file for the given game data and returns the value of it. If the game data doesn't exists, it will save the backupValue parameter and return it.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="backupValue">Value to be saved and returned if the given game data does not exists.</param>
        /// <returns>Value of the game data as a Vector4 array. If the game data doesn't exists, it will return the default value.</returns>
        public Vector4[] LoadArray(string dataName, Vector4[] backupValue)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());
            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);
            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            string fullFileName = "";
            bool doesDataExists = false;

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    doesDataExists = true;
                    fullFileName = fileInfo.Name;
                    break;
                }
            }

            if (!doesDataExists)
            {
                AASaver.SaveVector4Array(dataName, backupValue, GetFullFilePath(dataName, DataTypes.Vector4Array), encryptData);
                return backupValue;
            }
            else
            {
                if (encryptData)
                    return AAEncryption.Decrypt(AASaver.LoadVector4Array(GetSaveLocation() + fullFileName).value).AsVector4Array();
                else
                    return AASaver.LoadVector4Array(GetSaveLocation() + fullFileName).value.AsVector4Array();
            }
        }

        /// <summary>
        /// Finds the game data with the given name and returns it's data type.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <returns>The type of the given game data.</returns>
        public DataTypes GetDataType(string dataName)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());

            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);

            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    if (fileInfo.Name.StartsWith("aa"))
                        return DataTypes.Bool;
                    else if (fileInfo.Name.StartsWith("ba"))
                        return DataTypes.BoolArray;
                    else if (fileInfo.Name.StartsWith("ab"))
                        return DataTypes.Byte;
                    else if (fileInfo.Name.StartsWith("bb"))
                        return DataTypes.ByteArray;
                    else if (fileInfo.Name.StartsWith("ac"))
                        return DataTypes.Char;
                    else if (fileInfo.Name.StartsWith("bc"))
                        return DataTypes.CharArray;
                    else if (fileInfo.Name.StartsWith("ad"))
                        return DataTypes.Color;
                    else if (fileInfo.Name.StartsWith("bd"))
                        return DataTypes.ColorArray;
                    else if (fileInfo.Name.StartsWith("at"))
                        return DataTypes.DateTime;
                    else if (fileInfo.Name.StartsWith("bt"))
                        return DataTypes.DateTimeArray;
                    else if (fileInfo.Name.StartsWith("ae"))
                        return DataTypes.Decimal;
                    else if (fileInfo.Name.StartsWith("be"))
                        return DataTypes.DecimalArray;
                    else if (fileInfo.Name.StartsWith("af"))
                        return DataTypes.Double;
                    else if (fileInfo.Name.StartsWith("bf"))
                        return DataTypes.DoubleArray;
                    else if (fileInfo.Name.StartsWith("ag"))
                        return DataTypes.Float;
                    else if (fileInfo.Name.StartsWith("bg"))
                        return DataTypes.FloatArray;
                    else if (fileInfo.Name.StartsWith("ah"))
                        return DataTypes.Int;
                    else if (fileInfo.Name.StartsWith("bh"))
                        return DataTypes.IntArray;
                    else if (fileInfo.Name.StartsWith("ai"))
                        return DataTypes.Long;
                    else if (fileInfo.Name.StartsWith("bi"))
                        return DataTypes.LongArray;
                    else if (fileInfo.Name.StartsWith("aj"))
                        return DataTypes.Quaternion;
                    else if (fileInfo.Name.StartsWith("bj"))
                        return DataTypes.QuaternionArray;
                    else if (fileInfo.Name.StartsWith("ak"))
                        return DataTypes.Sbyte;
                    else if (fileInfo.Name.StartsWith("bk"))
                        return DataTypes.SbyteArray;
                    else if (fileInfo.Name.StartsWith("al"))
                        return DataTypes.Short;
                    else if (fileInfo.Name.StartsWith("bl"))
                        return DataTypes.ShortArray;
                    else if (fileInfo.Name.StartsWith("am"))
                        return DataTypes.String;
                    else if (fileInfo.Name.StartsWith("bm"))
                        return DataTypes.StringArray;
                    else if (fileInfo.Name.StartsWith("an"))
                        return DataTypes.Uint;
                    else if (fileInfo.Name.StartsWith("bn"))
                        return DataTypes.UintArray;
                    else if (fileInfo.Name.StartsWith("ao"))
                        return DataTypes.Ulong;
                    else if (fileInfo.Name.StartsWith("bo"))
                        return DataTypes.UlongArray;
                    else if (fileInfo.Name.StartsWith("ap"))
                        return DataTypes.Ushort;
                    else if (fileInfo.Name.StartsWith("bp"))
                        return DataTypes.UshortArray;
                    else if (fileInfo.Name.StartsWith("aq"))
                        return DataTypes.Vector2;
                    else if (fileInfo.Name.StartsWith("bq"))
                        return DataTypes.Vector2Array;
                    else if (fileInfo.Name.StartsWith("av"))
                        return DataTypes.Vector2Int;
                    else if (fileInfo.Name.StartsWith("bv"))
                        return DataTypes.Vector2IntArray;
                    else if (fileInfo.Name.StartsWith("ar"))
                        return DataTypes.Vector3;
                    else if (fileInfo.Name.StartsWith("br"))
                        return DataTypes.Vector3Array;
                    else if (fileInfo.Name.StartsWith("ax"))
                        return DataTypes.Vector3Int;
                    else if (fileInfo.Name.StartsWith("bx"))
                        return DataTypes.Vector3IntArray;
                    else if (fileInfo.Name.StartsWith("as"))
                        return DataTypes.Vector4;
                    else if (fileInfo.Name.StartsWith("bs"))
                        return DataTypes.Vector4Array;
                    else if (fileInfo.Name.StartsWith("au"))
                        return DataTypes.TimeSpan;
                    else if (fileInfo.Name.StartsWith("bu"))
                        return DataTypes.TimeSpanArray;
                }
            }

            return DataTypes.None;
        }

        /// <summary>
        /// Permanently deletes the corresponding Data Block component and the save file for the given game data.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <returns>Returns true if the game data with the given name has been found and deleted. Returns false if the game data with the given name could not be found.</returns>
        public bool Delete(string dataName)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetSaveLocation());

            FileInfo[] fileInfos = directoryInfo.GetFiles("*" + "." + fileExtension);

            byte[] hash = new System.Security.Cryptography.SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(dataName));

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Name.EndsWith(string.Concat(hash.Select(b => b.ToString("x2"))).Substring(0, 24) + "." + fileExtension))
                {
                    if (File.Exists(GetSaveLocation() + fileInfo.Name))
                    {
                        File.SetAttributes(GetSaveLocation() + fileInfo.Name, FileAttributes.Normal);
                        File.Delete(GetSaveLocation() + fileInfo.Name);
                    }
                    
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// If this SaveSystem instance has been marked as Dont Destroy on Load, returns true. Otherwise, returns false.
        /// </summary>
        public bool GetDontDestroyOnLoad()
        {
            return dontDestroyOnLoad;
        }

        /// <summary>
        /// Marks this Save System instance as Dont Destroy on Load. This instance will not be destroyed while loading a new scene.
        /// </summary>
        public void SetDontDestroyOnLoad()
        {
            dontDestroyOnLoad = true;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Returns the parent file path where the save files are placed at. If you want to get the full file path, use the GetFullFilePath method.
        /// </summary>
        public FileLocations GetFileLocation()
        {
            return fileLocation;
        }

        /// <summary>
        /// Sets the parent file path where the save files are placed at.
        /// </summary>
        /// <param name="location">New parent file path.</param>
        public void SetFileLocation(FileLocations location)
        {
            if ( location == FileLocations.CustomPath && StringAdjuster.IsStringEmpty(customFilePath) )
            {
                Debug.LogError(consoleLogPrefix + "You have tried to use a custom file path for the save files but the custom file path is currently empty. Assign a full path first.\n\n");
                return;
            }

            fileLocation = location;
        }

        /// <summary>
        /// Returns the custom file path.
        /// </summary>
        public string GetCustomFilePath()
        {
            return customFilePath;
        }

        /// <summary>
        /// Sets the custom file path. Enter the full file path.
        /// </summary>
        /// <param name="path">New custom file path.</param>
        public void SetCustomFilePath(string path)
        {
            if ( StringAdjuster.IsStringEmpty(path) )
            {
                Debug.LogError(consoleLogPrefix + "You cannot assign an empty value as the custom file path.\n\n");
                return;
            }

            if ( StringAdjuster.DoesStringStartsWithNumber(path) )
            {
                Debug.LogError(consoleLogPrefix + "Custom file path cannot start with a digit.\n\n");
                return;
            }

            if ( StringAdjuster.ContainsInvalidFileNameChars(path) )
            {
                Debug.LogError(consoleLogPrefix + "Custom file path contains invalid file name chars.\n\n");
                return;
            }

            customFilePath = path;
        }

        /// <summary>
        /// Returns true if the save files are located under a custom sub folder.
        /// </summary>
        public bool GetSubFolderOption()
        {
            return subFolder;
        }

        /// <summary>
        /// Sets the custom sub folder option as true or false.
        /// </summary>
        /// <param name="option">If true, the save files will be located under a custom sub folder.</param>
        public void SetSubFolderOption(bool option)
        {
            subFolder = option;
        }

        /// <summary>
        /// Sets the custom sub folder option as true or false.
        /// </summary>
        /// <param name="option">If true, the save files will be located under a custom sub folder.</param>
        /// <param name="folderName">Name of the sub folder.</param>
        public void SetSubFolderOption(bool option, string folderName)
        {
            if (StringAdjuster.IsStringEmpty(folderName))
            {
                Debug.LogError(consoleLogPrefix + "You cannot assign an empty value as the sub folder name.\n\n");
                return;
            }

            if (StringAdjuster.DoesStringStartsWithNumber(folderName))
            {
                Debug.LogError(consoleLogPrefix + "Sub folder name cannot start with a digit.\n\n");
                return;
            }

            if (StringAdjuster.ContainsInvalidFileNameChars(folderName))
            {
                Debug.LogError(consoleLogPrefix + "Sub folder name contains invalid file name chars.\n\n");
                return;
            }

            subFolder = option;
            subFolderName = folderName;
        }

        /// <summary>
        /// Returns the name of the sub folder.
        /// </summary>
        public string GetSubFolderName()
        {
            return subFolderName;
        }

        /// <summary>
        /// Sets the name of the sub folder.
        /// </summary>
        /// <param name="folderName">New name of the sub folder.</param>
        public void SetSubFolderName(string folderName)
        {
            if (StringAdjuster.IsStringEmpty(folderName))
            {
                Debug.LogError(consoleLogPrefix + "You cannot assign an empty value as the sub folder name.\n\n");
                return;
            }

            if (StringAdjuster.DoesStringStartsWithNumber(folderName))
            {
                Debug.LogError(consoleLogPrefix + "Sub folder name cannot start with a digit.\n\n");
                return;
            }

            if (StringAdjuster.ContainsInvalidFileNameChars(folderName))
            {
                Debug.LogError(consoleLogPrefix + "Sub folder name contains invalid file name chars.\n\n");
                return;
            }

            subFolderName = folderName;
        }

        /// <summary>
        /// Returns the save file extension.
        /// </summary>
        public string GetFileExtension()
        {
            return fileExtension;
        }

        /// <summary>
        /// Sets the save file extension.
        /// </summary>
        /// <param name="extension">New name of the save file extension.</param>
        public void SetFileExtension(string extension)
        {
            if (StringAdjuster.IsStringEmpty(extension))
            {
                Debug.LogError(consoleLogPrefix + "You cannot assign an empty value as the save file extension.\n\n");
                return;
            }

            if (extension.Contains(" "))
            {
                Debug.LogError(consoleLogPrefix + "Save file extension cannot contain spaces.\n\n");
                return;
            }

            if (StringAdjuster.DoesStringStartsWithNumber(extension))
            {
                Debug.LogError(consoleLogPrefix + "Save file extension cannot start with a digit.\n\n");
                return;
            }

            if (StringAdjuster.ContainsInvalidFileNameChars(extension))
            {
                Debug.LogError(consoleLogPrefix + "Save file extension contains invalid file name chars.\n\n");
                return;
            }

            fileExtension = extension;
        }

        /// <summary>
        /// If the game data encryption is enabled, returns true. Otherwise, returns false.
        /// </summary>
        public bool GetEncryptionOption()
        {
            return encryptData;
        }

        /// <summary>
        /// Enables or disables the game data encryption.
        /// </summary>
        /// <param name="option">If true, the game data will be encrypted before saving.</param>
        public void SetEncryptionOption(bool option)
        {
            encryptData = option;
        }

        public string GetFullFilePath()
        {
            return GetSaveLocation();
        }
    }
}
