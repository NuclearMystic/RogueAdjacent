// This code has been written by AHMET ALP for the Unity Asset "AA Save and Load System".
// Link to the asset store page: https://u3d.as/2TxY
// Publisher contact: ahmetalp.business@gmail.com

using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

namespace AASave
{
    public static class AASaver
    {
        /// <summary>
        /// Saves a game data in the boolean data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveBool(string dataName, bool value, string filePath, bool encryptData)
        {
            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                BoolBlock boolBlock = binaryFormatter.Deserialize(fileStream) as BoolBlock;
                fileStream.Close();

                if (encryptData)
                {
                    if (value)
                    {
                        boolBlock.value = AAEncryption.Encrypt("true");
                    }
                    else
                    {
                        boolBlock.value = AAEncryption.Encrypt("false");
                    }
                }
                else
                {
                    if (value)
                    {
                        boolBlock.value = "true";
                    }
                    else
                    {
                        boolBlock.value = "false";
                    }
                }
                
                binaryFormatter = new BinaryFormatter();
                fileStream = new FileStream(filePath, FileMode.Create);
                binaryFormatter.Serialize(fileStream, boolBlock);
                fileStream.Close();
            }
            else
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                BoolBlock boolBlock = new BoolBlock(dataName, value, encryptData);
                binaryFormatter.Serialize(fileStream, boolBlock);
                fileStream.Close();
            }
        }

        /// <summary>
        /// Loads a game data in the boolean data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>Bool Block instance that includes all the values of the game data.</returns>
        public static BoolBlock LoadBool(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BoolBlock boolBlock = binaryFormatter.Deserialize(fileStream) as BoolBlock;
            fileStream.Close();
            return boolBlock;
        }

        /// <summary>
        /// Saves a game data in the byte data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveByte(string dataName, byte value, string filePath, bool encryptData)
        {
            ByteBlock byteBlock;

            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                byteBlock = binaryFormatter.Deserialize(fileStream) as ByteBlock;
                fileStream.Close();

                if (encryptData)
                {
                    byteBlock.value = AAEncryption.Encrypt(value.ToString());
                }
                else
                {
                    byteBlock.value = value.ToString();
                }

                binaryFormatter = new BinaryFormatter();
                fileStream = new FileStream(filePath, FileMode.Create);
                binaryFormatter.Serialize(fileStream, byteBlock);
                fileStream.Close();
            }
            else
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                byteBlock = new ByteBlock(dataName, value, encryptData);
                binaryFormatter.Serialize(fileStream, byteBlock);
                fileStream.Close();
            }
        }

        /// <summary>
        /// Loads a game data in the byte data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>Byte Block instance that includes all the values of the game data.</returns>
        public static ByteBlock LoadByte(string filePath)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            ByteBlock byteBlock = binaryFormatter.Deserialize(fileStream) as ByteBlock;
            fileStream.Close();
            return byteBlock;
        }

        /// <summary>
        /// Saves a game data in the char data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveChar(string dataName, char value, string filePath, bool encryptData)
        {
            CharBlock charBlock;

            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                charBlock = binaryFormatter.Deserialize(fileStream) as CharBlock;
                fileStream.Close();

                if (encryptData)
                {
                    charBlock.value = AAEncryption.Encrypt(value.ToString());
                }
                else
                {
                    charBlock.value = value.ToString();
                }

                binaryFormatter = new BinaryFormatter();
                fileStream = new FileStream(filePath, FileMode.Create);
                binaryFormatter.Serialize(fileStream, charBlock);
                fileStream.Close();
            }
            else
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                charBlock = new CharBlock(dataName, value, encryptData);
                binaryFormatter.Serialize(fileStream, charBlock);
                fileStream.Close();
            }
        }

        /// <summary>
        /// Loads a game data in the char data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>Char Block instance that includes all the values of the game data.</returns>
        public static CharBlock LoadChar(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            CharBlock charBlock = binaryFormatter.Deserialize(fileStream) as CharBlock;
            fileStream.Close();
            return charBlock;
        }

        /// <summary>
        /// Saves a game data in the Color data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="red">Red value of the Color game data.</param>
        /// <param name="green">Green value of the Color game data.</param>
        /// <param name="blue">Blue value of the Color game data.</param>
        /// <param name="alpha">Alpha value of the Color game data.</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveColor(string dataName, float red, float green, float blue, float alpha, string filePath, bool encryptData)
        {
            ColorBlock colorBlock;

            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                colorBlock = binaryFormatter.Deserialize(fileStream) as ColorBlock;
                fileStream.Close();

                colorBlock.value = red.ToString() + "#";
                colorBlock.value += green.ToString() + "#";
                colorBlock.value += blue.ToString() + "#";
                colorBlock.value += alpha.ToString() + "#";

                if (encryptData)
                {
                    colorBlock.value = AAEncryption.Encrypt(colorBlock.value);
                }

                binaryFormatter = new BinaryFormatter();
                fileStream = new FileStream(filePath, FileMode.Create);
                binaryFormatter.Serialize(fileStream, colorBlock);
                fileStream.Close();
            }
            else
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                colorBlock = new ColorBlock(dataName, red, green, blue, alpha, encryptData);
                binaryFormatter.Serialize(fileStream, colorBlock);
                fileStream.Close();
            }
        }

        /// <summary>
        /// Loads a game data in the Color data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>Color Block instance that includes all the values of the game data.</returns>
        public static ColorBlock LoadColor(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            ColorBlock colorBlock = binaryFormatter.Deserialize(fileStream) as ColorBlock;
            fileStream.Close();
            return colorBlock;
        }

        /// <summary>
        /// Saves a game data in the DateTime data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Current value of the game data.</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveDateTime(string dataName, DateTime value, string filePath, bool encryptData)
        {
            DateTimeBlock dateTimeBlock;

            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                dateTimeBlock = binaryFormatter.Deserialize(fileStream) as DateTimeBlock;
                fileStream.Close();

                dateTimeBlock.value = value.Year.ToString() + "#";
                dateTimeBlock.value += value.Month.ToString() + "#";
                dateTimeBlock.value += value.Day.ToString() + "#";
                dateTimeBlock.value += value.Hour.ToString() + "#";
                dateTimeBlock.value += value.Minute.ToString() + "#";
                dateTimeBlock.value += value.Second.ToString() + "#";
                dateTimeBlock.value += value.Millisecond.ToString() + "#";

                if (encryptData)
                {
                    dateTimeBlock.value = AAEncryption.Encrypt(dateTimeBlock.value);
                }

                binaryFormatter = new BinaryFormatter();
                fileStream = new FileStream(filePath, FileMode.Create);
                binaryFormatter.Serialize(fileStream, dateTimeBlock);
                fileStream.Close();
            }
            else
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                dateTimeBlock = new DateTimeBlock(dataName, value, encryptData);
                binaryFormatter.Serialize(fileStream, dateTimeBlock);
                fileStream.Close();
            }
        }

        /// <summary>
        /// Loads a game data in the DateTime data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>DateTimeBlock instance that includes all the values of the game data.</returns>
        public static DateTimeBlock LoadDateTime(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            DateTimeBlock dateTimeBlock = binaryFormatter.Deserialize(fileStream) as DateTimeBlock;
            fileStream.Close();
            return dateTimeBlock;
        }

        /// <summary>
        /// Saves a game data in the decimal data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveDecimal(string dataName, decimal value, string filePath, bool encryptData)
        {
            DecimalBlock decimalBlock;

            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                decimalBlock = binaryFormatter.Deserialize(fileStream) as DecimalBlock;
                fileStream.Close();

                if (encryptData)
                {
                    decimalBlock.value = AAEncryption.Encrypt(value.ToString());
                }
                else
                {
                    decimalBlock.value = value.ToString();
                }

                binaryFormatter = new BinaryFormatter();
                fileStream = new FileStream(filePath, FileMode.Create);
                binaryFormatter.Serialize(fileStream, decimalBlock);
                fileStream.Close();
            }
            else
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                decimalBlock = new DecimalBlock(dataName, value, encryptData);
                binaryFormatter.Serialize(fileStream, decimalBlock);
                fileStream.Close();
            }
        }

        /// <summary>
        /// Loads a game data in the decimal data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>Decimal Block instance that includes all the values of the game data.</returns>
        public static DecimalBlock LoadDecimal(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            DecimalBlock decimalBlock = binaryFormatter.Deserialize(fileStream) as DecimalBlock;
            fileStream.Close();
            return decimalBlock;
        }

        /// <summary>
        /// Saves a game data in the double data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveDouble(string dataName, double value, string filePath, bool encryptData)
        {
            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                DoubleBlock doubleBlock = binaryFormatter.Deserialize(fileStream) as DoubleBlock;
                fileStream.Close();

                if (encryptData)
                {
                    doubleBlock.value = AAEncryption.Encrypt(value.ToString());
                }
                else
                {
                    doubleBlock.value = value.ToString();
                }

                binaryFormatter = new BinaryFormatter();
                fileStream = new FileStream(filePath, FileMode.Create);
                binaryFormatter.Serialize(fileStream, doubleBlock);
                fileStream.Close();
            }
            else
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                DoubleBlock doubleBlock = new DoubleBlock(dataName, value, encryptData);
                binaryFormatter.Serialize(fileStream, doubleBlock);
                fileStream.Close();
            }
        }

        /// <summary>
        /// Loads a game data in the double data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>Double Block instance that includes all the values of the game data.</returns>
        public static DoubleBlock LoadDouble(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            DoubleBlock doubleBlock = binaryFormatter.Deserialize(fileStream) as DoubleBlock;
            fileStream.Close();
            return doubleBlock;
        }

        /// <summary>
        /// Saves a game data in the float data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveFloat(string dataName, float value, string filePath, bool encryptData)
        {
            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                FloatBlock floatBlock = binaryFormatter.Deserialize(fileStream) as FloatBlock;
                fileStream.Close();

                if (encryptData)
                {
                    floatBlock.value = AAEncryption.Encrypt(value.ToString());
                }
                else
                {
                    floatBlock.value = value.ToString();
                }

                binaryFormatter = new BinaryFormatter();
                fileStream = new FileStream(filePath, FileMode.Create);
                binaryFormatter.Serialize(fileStream, floatBlock);
                fileStream.Close();
            }
            else
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                FloatBlock floatBlock = new FloatBlock(dataName, value, encryptData);
                binaryFormatter.Serialize(fileStream, floatBlock);
                fileStream.Close();
            }
        }

        /// <summary>
        /// Loads a game data in the float data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>FloatBlock instance that includes all the values of the game data.</returns>
        public static FloatBlock LoadFloat(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            FloatBlock floatBlock = binaryFormatter.Deserialize(fileStream) as FloatBlock;
            fileStream.Close();
            return floatBlock;
        }

        /// <summary>
        /// Saves a game data in the integer data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will encrypted.</param>
        public static void SaveInt(string dataName, int value, string filePath, bool encryptData)
        {
            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                IntBlock intBlock = binaryFormatter.Deserialize(fileStream) as IntBlock;
                fileStream.Close();

                if (encryptData)
                {
                    intBlock.value = AAEncryption.Encrypt(value.ToString());
                }
                else
                {
                    intBlock.value = value.ToString();
                }

                binaryFormatter = new BinaryFormatter();
                fileStream = new FileStream(filePath, FileMode.Create);
                binaryFormatter.Serialize(fileStream, intBlock);
                fileStream.Close();
            }
            else
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                IntBlock intBlock = new IntBlock(dataName, value, encryptData);
                binaryFormatter.Serialize(fileStream, intBlock);
                fileStream.Close();
            }
        }

        /// <summary>
        /// Loads a game data in the integer data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>IntBlock instance that includes all the values of the game data.</returns>
        public static IntBlock LoadInt(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            IntBlock intBlock = binaryFormatter.Deserialize(fileStream) as IntBlock;
            fileStream.Close();
            return intBlock;
        }

        /// <summary>
        /// Saves a game data in the long data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveLong(string dataName, long value, string filePath, bool encryptData)
        {
            LongBlock longBlock;

            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                longBlock = binaryFormatter.Deserialize(fileStream) as LongBlock;
                fileStream.Close();

                if (encryptData)
                {
                    longBlock.value = AAEncryption.Encrypt(value.ToString());
                }
                else
                {
                    longBlock.value = value.ToString();
                }

                binaryFormatter = new BinaryFormatter();
                fileStream = new FileStream(filePath, FileMode.Create);
                binaryFormatter.Serialize(fileStream, longBlock);
                fileStream.Close();
            }
            else
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                longBlock = new LongBlock(dataName, value, encryptData);
                binaryFormatter.Serialize(fileStream, longBlock);
                fileStream.Close();
            }
        }

        /// <summary>
        /// Loads a game data in the long data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>LongBlock instance that includes all the values of the game data.</returns>
        public static LongBlock LoadLong(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            LongBlock longBlock = binaryFormatter.Deserialize(fileStream) as LongBlock;
            fileStream.Close();
            return longBlock;
        }

        /// <summary>
        /// Saves a game data in the Quaternion data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="x">X value of the Quaternion game data.</param>
        /// <param name="y">Y value of the Quaternion game data.</param>
        /// <param name="z">Z value of the Quaternion game data.</param>
        /// <param name="w">W value of the Quaternion game data.</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveQuaternion(string dataName, float x, float y, float z, float w, string filePath, bool encryptData)
        {
            QuaternionBlock quaternionBlock;

            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                quaternionBlock = binaryFormatter.Deserialize(fileStream) as QuaternionBlock;
                fileStream.Close();

                quaternionBlock.value = x.ToString() + "#";
                quaternionBlock.value += y.ToString() + "#";
                quaternionBlock.value += z.ToString() + "#";
                quaternionBlock.value += w.ToString() + "#";

                if (encryptData)
                {
                    quaternionBlock.value = AAEncryption.Encrypt(quaternionBlock.value);
                }

                binaryFormatter = new BinaryFormatter();
                fileStream = new FileStream(filePath, FileMode.Create);
                binaryFormatter.Serialize(fileStream, quaternionBlock);
                fileStream.Close();
            }
            else
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                quaternionBlock = new QuaternionBlock(dataName, x, y, z, w, encryptData);
                binaryFormatter.Serialize(fileStream, quaternionBlock);
                fileStream.Close();
            }
        }

        /// <summary>
        /// Loads a game data in the Quaternion data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>QuaternionBlock instance that includes all the values of the game data.</returns>
        public static QuaternionBlock LoadQuaternion(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            QuaternionBlock quaternionBlock = binaryFormatter.Deserialize(fileStream) as QuaternionBlock;
            fileStream.Close();
            return quaternionBlock;
        }

        /// <summary>
        /// Saves a game data in the sbyte data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveSbyte(string dataName, sbyte value, string filePath, bool encryptData)
        {
            SbyteBlock sbyteBlock;

            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                sbyteBlock = binaryFormatter.Deserialize(fileStream) as SbyteBlock;
                fileStream.Close();

                if (encryptData)
                {
                    sbyteBlock.value = AAEncryption.Encrypt(value.ToString());
                }
                else
                {
                    sbyteBlock.value = value.ToString();
                }

                binaryFormatter = new BinaryFormatter();
                fileStream = new FileStream(filePath, FileMode.Create);
                binaryFormatter.Serialize(fileStream, sbyteBlock);
                fileStream.Close();
            }
            else
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                sbyteBlock = new SbyteBlock(dataName, value, encryptData);
                binaryFormatter.Serialize(fileStream, sbyteBlock);
                fileStream.Close();
            }
        }

        /// <summary>
        /// Loads a game data in the sbyte data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>SbyteBlock instance that includes all the values of the game data.</returns>
        public static SbyteBlock LoadSbyte(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            SbyteBlock sbyteBlock = binaryFormatter.Deserialize(fileStream) as SbyteBlock;
            fileStream.Close();
            return sbyteBlock;
        }

        /// <summary>
        /// Saves a game data in the short data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveShort(string dataName, short value, string filePath, bool encryptData)
        {
            ShortBlock shortBlock;

            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                shortBlock = binaryFormatter.Deserialize(fileStream) as ShortBlock;
                fileStream.Close();

                if (encryptData)
                {
                    shortBlock.value = AAEncryption.Encrypt(value.ToString());
                }
                else
                {
                    shortBlock.value = value.ToString();
                }

                binaryFormatter = new BinaryFormatter();
                fileStream = new FileStream(filePath, FileMode.Create);
                binaryFormatter.Serialize(fileStream, shortBlock);
                fileStream.Close();
            }
            else
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                shortBlock = new ShortBlock(dataName, value, encryptData);
                binaryFormatter.Serialize(fileStream, shortBlock);
                fileStream.Close();
            }
        }

        /// <summary>
        /// Loads a game data in the short data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>ShortBlock instance that includes all the values of the game data.</returns>
        public static ShortBlock LoadShort(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            ShortBlock shortBlock = binaryFormatter.Deserialize(fileStream) as ShortBlock;
            fileStream.Close();
            return shortBlock;
        }

        /// <summary>
        /// Saves a game data in the string data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveString(string dataName, string value, string filePath, bool encryptData)
        {
            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                StringBlock stringBlock = binaryFormatter.Deserialize(fileStream) as StringBlock;
                fileStream.Close();

                if (encryptData)
                {
                    stringBlock.value = AAEncryption.Encrypt(value);
                }
                else
                {
                    stringBlock.value = value;
                }

                binaryFormatter = new BinaryFormatter();
                fileStream = new FileStream(filePath, FileMode.Create);
                binaryFormatter.Serialize(fileStream, stringBlock);
                fileStream.Close();
            }
            else
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                StringBlock stringBlock = new StringBlock(dataName, value, encryptData);
                binaryFormatter.Serialize(fileStream, stringBlock);
                fileStream.Close();
            }
        }

        /// <summary>
        /// Loads a game data in the string data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>StringBlock instance that includes all the values of the game data.</returns>
        public static StringBlock LoadString(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            StringBlock stringBlock = binaryFormatter.Deserialize(fileStream) as StringBlock;
            fileStream.Close();
            return stringBlock;
        }

        /// <summary>
        /// Saves a game data in the TimeSpan data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveTimeSpan(string dataName, TimeSpan value, string filePath, bool encryptData)
        {
            TimeSpanBlock timeSpanBlock;

            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                timeSpanBlock = binaryFormatter.Deserialize(fileStream) as TimeSpanBlock;
                fileStream.Close();

                if (encryptData)
                {
                    timeSpanBlock.value = AAEncryption.Encrypt(value.Ticks.ToString());
                }
                else
                {
                    timeSpanBlock.value = value.Ticks.ToString();
                }

                binaryFormatter = new BinaryFormatter();
                fileStream = new FileStream(filePath, FileMode.Create);
                binaryFormatter.Serialize(fileStream, timeSpanBlock);
                fileStream.Close();
            }
            else
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                timeSpanBlock = new TimeSpanBlock(dataName, value, encryptData);
                binaryFormatter.Serialize(fileStream, timeSpanBlock);
                fileStream.Close();
            }
        }

        /// <summary>
        /// Loads a game data in the TimeSpan data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>TimeSpanBlock instance that includes all the values of the game data.</returns>
        public static TimeSpanBlock LoadTimeSpan(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            TimeSpanBlock timeSpanBlock = binaryFormatter.Deserialize(fileStream) as TimeSpanBlock;
            fileStream.Close();
            return timeSpanBlock;
        }

        /// <summary>
        /// Saves a game data in the uint data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveUint(string dataName, uint value, string filePath, bool encryptData)
        {
            UintBlock uintBlock;

            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                uintBlock = binaryFormatter.Deserialize(fileStream) as UintBlock;
                fileStream.Close();

                if (encryptData)
                {
                    uintBlock.value = AAEncryption.Encrypt(value.ToString());
                }
                else
                {
                    uintBlock.value = value.ToString();
                }

                binaryFormatter = new BinaryFormatter();
                fileStream = new FileStream(filePath, FileMode.Create);
                binaryFormatter.Serialize(fileStream, uintBlock);
                fileStream.Close();
            }
            else
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                uintBlock = new UintBlock(dataName, value, encryptData);
                binaryFormatter.Serialize(fileStream, uintBlock);
                fileStream.Close();
            }
        }

        /// <summary>
        /// Loads a game data in the uint data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>UintBlock instance that includes all the values of the game data.</returns>
        public static UintBlock LoadUint(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            UintBlock uintBlock = binaryFormatter.Deserialize(fileStream) as UintBlock;
            fileStream.Close();
            return uintBlock;
        }

        /// <summary>
        /// Saves a game data in the ulong data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveUlong(string dataName, ulong value, string filePath, bool encryptData)
        {
            UlongBlock ulongBlock;

            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                ulongBlock = binaryFormatter.Deserialize(fileStream) as UlongBlock;
                fileStream.Close();

                if (encryptData)
                {
                    ulongBlock.value = AAEncryption.Encrypt(value.ToString());
                }
                else
                {
                    ulongBlock.value = value.ToString();
                }

                binaryFormatter = new BinaryFormatter();
                fileStream = new FileStream(filePath, FileMode.Create);
                binaryFormatter.Serialize(fileStream, ulongBlock);
                fileStream.Close();
            }
            else
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                ulongBlock = new UlongBlock(dataName, value, encryptData);
                binaryFormatter.Serialize(fileStream, ulongBlock);
                fileStream.Close();
            }
        }

        /// <summary>
        /// Loads a game data in the ulong data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>UlongBlock instance that includes all the values of the game data.</returns>
        public static UlongBlock LoadUlong(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            UlongBlock ulongBlock = binaryFormatter.Deserialize(fileStream) as UlongBlock;
            fileStream.Close();
            return ulongBlock;
        }

        /// <summary>
        /// Saves a game data in the ushort data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveUshort(string dataName, ushort value, string filePath, bool encryptData)
        {
            UshortBlock ushortBlock;

            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                ushortBlock = binaryFormatter.Deserialize(fileStream) as UshortBlock;
                fileStream.Close();

                if (encryptData)
                {
                    ushortBlock.value = AAEncryption.Encrypt(value.ToString());
                }
                else
                {
                    ushortBlock.value = value.ToString();
                }

                binaryFormatter = new BinaryFormatter();
                fileStream = new FileStream(filePath, FileMode.Create);
                binaryFormatter.Serialize(fileStream, ushortBlock);
                fileStream.Close();
            }
            else
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                ushortBlock = new UshortBlock(dataName, value, encryptData);
                binaryFormatter.Serialize(fileStream, ushortBlock);
                fileStream.Close();
            }
        }

        /// <summary>
        /// Loads a game data in the ushort data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>UshortBlock instance that includes all the values of the game data.</returns>
        public static UshortBlock LoadUshort(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            UshortBlock ushortBlock = binaryFormatter.Deserialize(fileStream) as UshortBlock;
            fileStream.Close();
            return ushortBlock;
        }

        /// <summary>
        /// Saves a game data in the Vector2 data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="x">X value of the Vector2 game data.</param>
        /// <param name="y">Y value of the Vector2 game data.</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveVector2(string dataName, float x, float y, string filePath, bool encryptData)
        {
            Vector2Block vector2Block;

            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                vector2Block = binaryFormatter.Deserialize(fileStream) as Vector2Block;
                fileStream.Close();

                vector2Block.value = x.ToString() + "#";
                vector2Block.value += y.ToString() + "#";

                if (encryptData)
                {
                    vector2Block.value = AAEncryption.Encrypt(vector2Block.value);
                }

                binaryFormatter = new BinaryFormatter();
                fileStream = new FileStream(filePath, FileMode.Create);
                binaryFormatter.Serialize(fileStream, vector2Block);
                fileStream.Close();
            }
            else
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                vector2Block = new Vector2Block(dataName, x, y, encryptData);
                binaryFormatter.Serialize(fileStream, vector2Block);
                fileStream.Close();
            }
        }

        /// <summary>
        /// Loads a game data in the Vector2 data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>Vector2Block instance that includes all the values of the game data.</returns>
        public static Vector2Block LoadVector2(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            Vector2Block vector2Block = binaryFormatter.Deserialize(fileStream) as Vector2Block;
            fileStream.Close();
            return vector2Block;
        }

        /// <summary>
        /// Saves a game data in the Vector2Int data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="x">X value of the Vector2Int game data.</param>
        /// <param name="y">Y value of the Vector2Int game data.</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveVector2Int(string dataName, int x, int y, string filePath, bool encryptData)
        {
            Vector2IntBlock vector2IntBlock;

            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                vector2IntBlock = binaryFormatter.Deserialize(fileStream) as Vector2IntBlock;
                fileStream.Close();

                vector2IntBlock.value = x.ToString() + "#";
                vector2IntBlock.value += y.ToString() + "#";

                if (encryptData)
                {
                    vector2IntBlock.value = AAEncryption.Encrypt(vector2IntBlock.value);
                }

                binaryFormatter = new BinaryFormatter();
                fileStream = new FileStream(filePath, FileMode.Create);
                binaryFormatter.Serialize(fileStream, vector2IntBlock);
                fileStream.Close();
            }
            else
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                vector2IntBlock = new Vector2IntBlock(dataName, x, y, encryptData);
                binaryFormatter.Serialize(fileStream, vector2IntBlock);
                fileStream.Close();
            }
        }

        /// <summary>
        /// Loads a game data in the Vector2Int data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>Vector2IntBlock instance that includes all the values of the game data.</returns>
        public static Vector2IntBlock LoadVector2Int(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            Vector2IntBlock vector2IntBlock = binaryFormatter.Deserialize(fileStream) as Vector2IntBlock;
            fileStream.Close();
            return vector2IntBlock;
        }

        /// <summary>
        /// Saves a game data in the Vector3 data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="x">X value of the Vector3 game data.</param>
        /// <param name="y">Y value of the Vector3 game data.</param>
        /// <param name="z">Z value of the Vector3 game data.</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveVector3(string dataName, float x, float y, float z, string filePath, bool encryptData)
        {
            Vector3Block vector3Block;

            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                vector3Block = binaryFormatter.Deserialize(fileStream) as Vector3Block;
                fileStream.Close();

                vector3Block.value = x.ToString() + "#";
                vector3Block.value += y.ToString() + "#";
                vector3Block.value += z.ToString() + "#";

                if (encryptData)
                {
                    vector3Block.value = AAEncryption.Encrypt(vector3Block.value);
                }

                binaryFormatter = new BinaryFormatter();
                fileStream = new FileStream(filePath, FileMode.Create);
                binaryFormatter.Serialize(fileStream, vector3Block);
                fileStream.Close();
            }
            else
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                vector3Block = new Vector3Block(dataName, x, y, z, encryptData);
                binaryFormatter.Serialize(fileStream, vector3Block);
                fileStream.Close();
            }
        }

        /// <summary>
        /// Loads a game data in the Vector3 data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>Vector3Block instance that includes all the values of the game data.</returns>
        public static Vector3Block LoadVector3(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            Vector3Block vector3Block = binaryFormatter.Deserialize(fileStream) as Vector3Block;
            fileStream.Close();
            return vector3Block;
        }

        /// <summary>
        /// Saves a game data in the Vector3Int data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="x">X value of the Vector3Int game data.</param>
        /// <param name="y">Y value of the Vector3Int game data.</param>
        /// <param name="z">Z value of the Vector3Int game data.</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveVector3Int(string dataName, int x, int y, int z, string filePath, bool encryptData)
        {
            Vector3IntBlock vector3IntBlock;

            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                vector3IntBlock = binaryFormatter.Deserialize(fileStream) as Vector3IntBlock;
                fileStream.Close();

                vector3IntBlock.value = x.ToString() + "#";
                vector3IntBlock.value += y.ToString() + "#";
                vector3IntBlock.value += z.ToString() + "#";

                if (encryptData)
                {
                    vector3IntBlock.value = AAEncryption.Encrypt(vector3IntBlock.value);
                }
                
                binaryFormatter = new BinaryFormatter();
                fileStream = new FileStream(filePath, FileMode.Create);
                binaryFormatter.Serialize(fileStream, vector3IntBlock);
                fileStream.Close();
            }
            else
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                vector3IntBlock = new Vector3IntBlock(dataName, x, y, z, encryptData);
                binaryFormatter.Serialize(fileStream, vector3IntBlock);
                fileStream.Close();
            }
        }

        /// <summary>
        /// Loads a game data in the Vector3Int data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>Vector3IntBlock instance that includes all the values of the game data.</returns>
        public static Vector3IntBlock LoadVector3Int(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            Vector3IntBlock vector3IntBlock = binaryFormatter.Deserialize(fileStream) as Vector3IntBlock;
            fileStream.Close();
            return vector3IntBlock;
        }

        /// <summary>
        /// Save a game data in the Vector4 data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="x">X value of the Vector4 game data.</param>
        /// <param name="y">Y value of the Vector4 game data.</param>
        /// <param name="z">Z value of the Vector4 game data.</param>
        /// <param name="w">W value of the Vector4 game data.</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveVector4(string dataName, float x, float y, float z, float w, string filePath, bool encryptData)
        {
            Vector4Block vector4Block;

            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                vector4Block = binaryFormatter.Deserialize(fileStream) as Vector4Block;
                fileStream.Close();

                vector4Block.value = x.ToString() + "#";
                vector4Block.value += y.ToString() + "#";
                vector4Block.value += z.ToString() + "#";
                vector4Block.value += w.ToString() + "#";

                if (encryptData)
                {
                    vector4Block.value = AAEncryption.Encrypt(vector4Block.value);
                }

                binaryFormatter = new BinaryFormatter();
                fileStream = new FileStream(filePath, FileMode.Create);
                binaryFormatter.Serialize(fileStream, vector4Block);
                fileStream.Close();
            }
            else
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                vector4Block = new Vector4Block(dataName, x, y, z, w, encryptData);
                binaryFormatter.Serialize(fileStream, vector4Block);
                fileStream.Close();
            }
        }

        /// <summary>
        /// Loads a game data in the Vector4 data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>Vector4Block instance that includes all the values of the game data.</returns>
        public static Vector4Block LoadVector4(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            Vector4Block vector4Block = binaryFormatter.Deserialize(fileStream) as Vector4Block;
            fileStream.Close();
            return vector4Block;
        }

        /// <summary>
        /// Saves a game data in the boolean array data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveBoolArray(string dataName, bool[] value, string filePath, bool encryptData)
        {
            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            BoolArrayBlock boolArrayBlock = new BoolArrayBlock(dataName, value, encryptData);
            binaryFormatter.Serialize(fileStream, boolArrayBlock);
            fileStream.Close();
        }

        /// <summary>
        /// Loads a game data in the boolean array data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>BoolArrayBlock instance that includes all the values of the game data.</returns>
        public static BoolArrayBlock LoadBoolArray(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BoolArrayBlock boolArrayBlock = binaryFormatter.Deserialize(fileStream) as BoolArrayBlock;
            fileStream.Close();
            return boolArrayBlock;
        }

        /// <summary>
        /// Saves a game data in the byte array data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveByteArray(string dataName, byte[] value, string filePath, bool encryptData)
        {
            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            ByteArrayBlock byteArrayBlock = new ByteArrayBlock(dataName, value, encryptData);
            binaryFormatter.Serialize(fileStream, byteArrayBlock);
            fileStream.Close();
        }

        /// <summary>
        /// Loads a game data in the byte array data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>ByteArrayBlock instance that includes all the values of the game data.</returns>
        public static ByteArrayBlock LoadByteArray(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            ByteArrayBlock byteArrayBlock = binaryFormatter.Deserialize(fileStream) as ByteArrayBlock;
            fileStream.Close();
            return byteArrayBlock;
        }

        /// <summary>
        /// Saves a game data in the char array data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveCharArray(string dataName, char[] value, string filePath, bool encryptData)
        {
            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            CharArrayBlock charArrayBlock = new CharArrayBlock(dataName, value, encryptData);
            binaryFormatter.Serialize(fileStream, charArrayBlock);
            fileStream.Close();
        }

        /// <summary>
        /// Loads a game data in the char array data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>CharArrayBlock instance that includes all the values of the game data.</returns>
        public static CharArrayBlock LoadCharArray(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            CharArrayBlock charArrayBlock = binaryFormatter.Deserialize(fileStream) as CharArrayBlock;
            fileStream.Close();
            return charArrayBlock;
        }

        /// <summary>
        /// Saves a game data in the color array data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveColorArray(string dataName, Color[] value, string filePath, bool encryptData)
        {
            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            ColorArrayBlock colorArrayBlock = new ColorArrayBlock(dataName, value, encryptData);
            binaryFormatter.Serialize(fileStream, colorArrayBlock);
            fileStream.Close();
        }

        /// <summary>
        /// Loads a game data in the color array data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>ColorArrayBlock instance that includes all the values of the game data.</returns>
        public static ColorArrayBlock LoadColorArray(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            ColorArrayBlock colorArrayBlock = binaryFormatter.Deserialize(fileStream) as ColorArrayBlock;
            fileStream.Close();
            return colorArrayBlock;
        }

        /// <summary>
        /// Saves a game data in the DateTime Array data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data.</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveDateTimeArray(string dataName, DateTime[] value, string filePath, bool encryptData)
        {
            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            DateTimeArrayBlock dateTimeArrayBlock = new DateTimeArrayBlock(dataName, value, encryptData);
            binaryFormatter.Serialize(fileStream, dateTimeArrayBlock);
            fileStream.Close();
        }

        /// <summary>
        /// Loads a game data in the DateTime Array data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>DateTimeArrayBlock instance that includes all the values of the game data.</returns>
        public static DateTimeArrayBlock LoadDateTimeArray(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            DateTimeArrayBlock dateTimeArrayBlock = binaryFormatter.Deserialize(fileStream) as DateTimeArrayBlock;
            fileStream.Close();
            return dateTimeArrayBlock;
        }

        /// <summary>
        /// Saves a game data in the decimal array data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveDecimalArray(string dataName, decimal[] value, string filePath, bool encryptData)
        {
            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            DecimalArrayBlock decimalArrayBlock = new DecimalArrayBlock(dataName, value, encryptData);
            binaryFormatter.Serialize(fileStream, decimalArrayBlock);
            fileStream.Close();
        }

        /// <summary>
        /// Loads a game data in the decimal array data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>DecimalArrayBlock instance that includes all the values of the game data.</returns>
        public static DecimalArrayBlock LoadDecimalArray(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            DecimalArrayBlock decimalArrayBlock = binaryFormatter.Deserialize(fileStream) as DecimalArrayBlock;
            fileStream.Close();
            return decimalArrayBlock;
        }

        /// <summary>
        /// Saves a game data in the double array data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveDoubleArray(string dataName, double[] value, string filePath, bool encryptData)
        {
            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            DoubleArrayBlock doubleArrayBlock = new DoubleArrayBlock(dataName, value, encryptData);
            binaryFormatter.Serialize(fileStream, doubleArrayBlock);
            fileStream.Close();
        }

        /// <summary>
        /// Loads a game data in the double array data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>DoubleArrayBlock instance that includes all the values of the game data.</returns>
        public static DoubleArrayBlock LoadDoubleArray(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            DoubleArrayBlock doubleArrayBlock = binaryFormatter.Deserialize(fileStream) as DoubleArrayBlock;
            fileStream.Close();
            return doubleArrayBlock;
        }

        /// <summary>
        /// Saves a game data in the float array data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveFloatArray(string dataName, float[] value, string filePath, bool encryptData)
        {
            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            FloatArrayBlock floatArrayBlock = new FloatArrayBlock(dataName, value, encryptData);
            binaryFormatter.Serialize(fileStream, floatArrayBlock);
            fileStream.Close();
        }

        /// <summary>
        /// Loads a game data in the float array data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>FloatArrayBlock instance that includes all the values of the game data.</returns>
        public static FloatArrayBlock LoadFloatArray(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            FloatArrayBlock floatArrayBlock = binaryFormatter.Deserialize(fileStream) as FloatArrayBlock;
            fileStream.Close();
            return floatArrayBlock;
        }

        /// <summary>
        /// Saves a game data in the int array data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveIntArray(string dataName, int[] value, string filePath, bool encryptData)
        {
            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            IntArrayBlock intArrayBlock = new IntArrayBlock(dataName, value, encryptData);
            binaryFormatter.Serialize(fileStream, intArrayBlock);
            fileStream.Close();
        }

        /// <summary>
        /// Loads a game data in the int array data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>IntArrayBlock instance that includes all the values of the game data.</returns>
        public static IntArrayBlock LoadIntArray(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            IntArrayBlock intArrayBlock = binaryFormatter.Deserialize(fileStream) as IntArrayBlock;
            fileStream.Close();
            return intArrayBlock;
        }

        /// <summary>
        /// Saves a game data in the long array data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveLongArray(string dataName, long[] value, string filePath, bool encryptData)
        {
            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            LongArrayBlock longArrayBlock = new LongArrayBlock(dataName, value, encryptData);
            binaryFormatter.Serialize(fileStream, longArrayBlock);
            fileStream.Close();
        }

        /// <summary>
        /// Loads a game data in the long array data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>LongArrayBlock instance that includes all the values of the game data.</returns>
        public static LongArrayBlock LoadLongArray(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            LongArrayBlock longArrayBlock = binaryFormatter.Deserialize(fileStream) as LongArrayBlock;
            fileStream.Close();
            return longArrayBlock;
        }

        /// <summary>
        /// Saves a game data in the quaternion array data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveQuaternionArray(string dataName, Quaternion[] value, string filePath, bool encryptData)
        {
            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            QuaternionArrayBlock quaternionArrayBlock = new QuaternionArrayBlock(dataName, value, encryptData);
            binaryFormatter.Serialize(fileStream, quaternionArrayBlock);
            fileStream.Close();
        }

        /// <summary>
        /// Loads a game data in the quaternion array data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>QuaternionArrayBlock instance that includes all the values of the game data.</returns>
        public static QuaternionArrayBlock LoadQuaternionArray(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            QuaternionArrayBlock quaternionArrayBlock = binaryFormatter.Deserialize(fileStream) as QuaternionArrayBlock;
            fileStream.Close();
            return quaternionArrayBlock;
        }

        /// <summary>
        /// Saves a game data in the sbyte array data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveSbyteArray(string dataName, sbyte[] value, string filePath, bool encryptData)
        {
            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            SbyteArrayBlock sbyteArrayBlock = new SbyteArrayBlock(dataName, value, encryptData);
            binaryFormatter.Serialize(fileStream, sbyteArrayBlock);
            fileStream.Close();
        }

        /// <summary>
        /// Loads a game data in the sbyte array data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>SbyteArrayBlock instance that includes all the values of the game data.</returns>
        public static SbyteArrayBlock LoadSbyteArray(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            SbyteArrayBlock sbyteArrayBlock = binaryFormatter.Deserialize(fileStream) as SbyteArrayBlock;
            fileStream.Close();
            return sbyteArrayBlock;
        }

        /// <summary>
        /// Saves a game data in the short array data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveShortArray(string dataName, short[] value, string filePath, bool encryptData)
        {
            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            ShortArrayBlock shortArrayBlock = new ShortArrayBlock(dataName, value, encryptData);
            binaryFormatter.Serialize(fileStream, shortArrayBlock);
            fileStream.Close();
        }

        /// <summary>
        /// Loads a game data in the short array data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>ShortArrayBlock instance that includes all the values of the game data.</returns>
        public static ShortArrayBlock LoadShortArray(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            ShortArrayBlock shortArrayBlock = binaryFormatter.Deserialize(fileStream) as ShortArrayBlock;
            fileStream.Close();
            return shortArrayBlock;
        }

        /// <summary>
        /// Saves a game data in the string array data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveStringArray(string dataName, string[] value, string filePath, bool encryptData)
        {
            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            StringArrayBlock stringArrayBlock = new StringArrayBlock(dataName, value, encryptData);
            binaryFormatter.Serialize(fileStream, stringArrayBlock);
            fileStream.Close();
        }

        /// <summary>
        /// Loads a game data in the string array data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>StringArrayBlock instance that includes all the values of the game data.</returns>
        public static StringArrayBlock LoadStringArray(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            StringArrayBlock stringArrayBlock = binaryFormatter.Deserialize(fileStream) as StringArrayBlock;
            fileStream.Close();
            return stringArrayBlock;
        }

        /// <summary>
        /// Saves a game data in the TimeSpan array data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveTimeSpanArray(string dataName, TimeSpan[] value, string filePath, bool encryptData)
        {
            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            TimeSpanArrayBlock timeSpanArrayBlock = new TimeSpanArrayBlock(dataName, value, encryptData);
            binaryFormatter.Serialize(fileStream, timeSpanArrayBlock);
            fileStream.Close();
        }

        /// <summary>
        /// Loads a game data in the TimeSpan array data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>TimeSpanArrayBlock instance that includes all the values of the game data.</returns>
        public static TimeSpanArrayBlock LoadTimeSpanArray(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            TimeSpanArrayBlock timeSpanArrayBlock = binaryFormatter.Deserialize(fileStream) as TimeSpanArrayBlock;
            fileStream.Close();
            return timeSpanArrayBlock;
        }

        /// <summary>
        /// Saves a game data in the uint array data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveUintArray(string dataName, uint[] value, string filePath, bool encryptData)
        {
            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            UintArrayBlock uintArrayBlock = new UintArrayBlock(dataName, value, encryptData);
            binaryFormatter.Serialize(fileStream, uintArrayBlock);
            fileStream.Close();
        }

        /// <summary>
        /// Loads a game data in the uint array data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>UintArrayBlock instance that includes all the values of the game data.</returns>
        public static UintArrayBlock LoadUintArray(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            UintArrayBlock uintArrayBlock = binaryFormatter.Deserialize(fileStream) as UintArrayBlock;
            fileStream.Close();
            return uintArrayBlock;
        }

        /// <summary>
        /// Saves a game data in the ulong array data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="dataName">If true, the game data will be encrypted.</param>
        public static void SaveUlongArray(string dataName, ulong[] value, string filePath, bool encryptData)
        {
            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            UlongArrayBlock ulongArrayBlock = new UlongArrayBlock(dataName, value, encryptData);
            binaryFormatter.Serialize(fileStream, ulongArrayBlock);
            fileStream.Close();
        }

        /// <summary>
        /// Loads a game data in the ulong array data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>UlongArrayBlock instance that includes all the values of the game data.</returns>
        public static UlongArrayBlock LoadUlongArray(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            UlongArrayBlock ulongArrayBlock = binaryFormatter.Deserialize(fileStream) as UlongArrayBlock;
            fileStream.Close();
            return ulongArrayBlock;
        }

        /// <summary>
        /// Saves a game data in the ushort array data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveUshortArray(string dataName, ushort[] value, string filePath, bool encryptData)
        {
            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            UshortArrayBlock ushortArrayBlock = new UshortArrayBlock(dataName, value, encryptData);
            binaryFormatter.Serialize(fileStream, ushortArrayBlock);
            fileStream.Close();
        }

        /// <summary>
        /// Loads a game data in the ushort array data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>UshortArrayBlock instance that includes all the values of the game data.</returns>
        public static UshortArrayBlock LoadUshortArray(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            UshortArrayBlock ushortArrayBlock = binaryFormatter.Deserialize(fileStream) as UshortArrayBlock;
            fileStream.Close();
            return ushortArrayBlock;
        }

        /// <summary>
        /// Saves a game data in the Vector2 array data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveVector2Array(string dataName, Vector2[] value, string filePath, bool encryptData)
        {
            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            Vector2ArrayBlock vector2ArrayBlock = new Vector2ArrayBlock(dataName, value, encryptData);
            binaryFormatter.Serialize(fileStream, vector2ArrayBlock);
            fileStream.Close();
        }

        /// <summary>
        /// Loads a game data in the Vector2 array data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>Vector2ArrayBlock instance that includes all the values of the game data.</returns>
        public static Vector2ArrayBlock LoadVector2Array(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            Vector2ArrayBlock vector2ArrayBlock = binaryFormatter.Deserialize(fileStream) as Vector2ArrayBlock;
            fileStream.Close();
            return vector2ArrayBlock;
        }

        /// <summary>
        /// Saves a game data in the Vector2Int array data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveVector2IntArray(string dataName, Vector2Int[] value, string filePath, bool encryptData)
        {
            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            Vector2IntArrayBlock vector2IntArrayBlock = new Vector2IntArrayBlock(dataName, value, encryptData);
            binaryFormatter.Serialize(fileStream, vector2IntArrayBlock);
            fileStream.Close();
        }

        /// <summary>
        /// Loads a game data in the Vector2Int array data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>Vector2IntArray Block instance that includes all the values of the game data.</returns>
        public static Vector2IntArrayBlock LoadVector2IntArray(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            Vector2IntArrayBlock vector2IntArrayBlock = binaryFormatter.Deserialize(fileStream) as Vector2IntArrayBlock;
            fileStream.Close();
            return vector2IntArrayBlock;
        }

        /// <summary>
        /// Saves a game data in the Vector3 array data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveVector3Array(string dataName, Vector3[] value, string filePath, bool encryptData)
        {
            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            Vector3ArrayBlock vector3ArrayBlock = new Vector3ArrayBlock(dataName, value, encryptData);
            binaryFormatter.Serialize(fileStream, vector3ArrayBlock);
            fileStream.Close();
        }

        /// <summary>
        /// Loads a game data in the Vector3 array data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>Vector3ArrayBlock instance that includes all the values of the game data.</returns>
        public static Vector3ArrayBlock LoadVector3Array(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            Vector3ArrayBlock vector3ArrayBlock = binaryFormatter.Deserialize(fileStream) as Vector3ArrayBlock;
            fileStream.Close();
            return vector3ArrayBlock;
        }

        /// <summary>
        /// Saves a game data in the Vector3Int array data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveVector3IntArray(string dataName, Vector3Int[] value, string filePath, bool encryptData)
        {
            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            Vector3IntArrayBlock vector3IntArrayBlock = new Vector3IntArrayBlock(dataName, value, encryptData);
            binaryFormatter.Serialize(fileStream, vector3IntArrayBlock);
            fileStream.Close();
        }

        /// <summary>
        /// Loads a game data in the Vector3Int array data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>Vector3IntArray Block instance that includes all the values of the game data.</returns>
        public static Vector3IntArrayBlock LoadVector3IntArray(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            Vector3IntArrayBlock vector3IntArrayBlock = binaryFormatter.Deserialize(fileStream) as Vector3IntArrayBlock;
            fileStream.Close();
            return vector3IntArrayBlock;
        }

        /// <summary>
        /// Saves a game data in the Vector4 array data type. Do not use this method directly. Use the Save methods in the Save System class instead.
        /// </summary>
        /// <param name="dataName">Name of the game data.</param>
        /// <param name="value">Value of the game data</param>
        /// <param name="filePath">Full path to the save file.</param>
        /// <param name="encryptData">If true, the game data will be encrypted.</param>
        public static void SaveVector4Array(string dataName, Vector4[] value, string filePath, bool encryptData)
        {
            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            Vector4ArrayBlock vector4ArrayBlock = new Vector4ArrayBlock(dataName, value, encryptData);
            binaryFormatter.Serialize(fileStream, vector4ArrayBlock);
            fileStream.Close();
        }

        /// <summary>
        /// Loads a game data in the Vector4 array data type. Do not use this method directly. Use the Load method in the Save System class instead.
        /// </summary>
        /// <param name="filePath">Full path to the save file.</param>
        /// <returns>Vector4ArrayBlock instance that includes all the values of the game data.</returns>
        public static Vector4ArrayBlock LoadVector4Array(string filePath)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            Vector4ArrayBlock vector4ArrayBlock = binaryFormatter.Deserialize(fileStream) as Vector4ArrayBlock;
            fileStream.Close();
            return vector4ArrayBlock;
        }

    }
}
