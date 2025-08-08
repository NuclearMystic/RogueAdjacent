// This code has been written by AHMET ALP for the Unity Asset "AA Save and Load System".
// Link to the asset store page: https://u3d.as/2TxY
// Publisher contact: ahmetalp.business@gmail.com

using System.Linq;
using System;
using UnityEngine;

namespace AASave
{
    public static class Converter
    {
        private static readonly char dividerChar = '#';

        private static float[] GetDividedFloats(string value)
        {
            int charAmount = value.Count(f => f == dividerChar);

            float[] indexes = new float[charAmount];

            int foundParts = 0;
            string currentPart = "";

            for (int i = 0; i < value.Length; i++)
            {
                if (!value[i].Equals(dividerChar))
                {
                    currentPart += value[i];
                }
                else
                {
                    indexes[foundParts] = Convert.ToSingle(currentPart);
                    foundParts += 1;
                    currentPart = "";
                }
            }

            return indexes;
        }

        private static int[] GetDividedInts(string value)
        {
            int charAmount = value.Count(f => f == dividerChar);

            int[] indexes = new int[charAmount];

            int foundParts = 0;
            string currentPart = "";

            for (int i = 0; i < value.Length; i++)
            {
                if (!value[i].Equals(dividerChar))
                {
                    currentPart += value[i];
                }
                else
                {
                    indexes[foundParts] = Convert.ToInt32(currentPart);
                    foundParts += 1;
                    currentPart = "";
                }
            }

            return indexes;
        }

        /// <summary>
        /// Converts the value of the game data to a boolean. Use this method after the Load method.
        /// </summary>
        /// <param name="value">Value of the game data as a string. Load method returns the data value as a string.</param>
        /// <returns>Value of the game data as a boolean.</returns>
        public static bool AsBool(this string value)
        {
            return value.Equals("true");
        }

        /// <summary>
        /// Converts the value of the game data to a byte. Use this method after the Load method.
        /// </summary>
        /// <param name="value">Value of the game data as a string because the Load method returns the game data value as a string.</param>
        /// <returns>Value of the game data as a byte.</returns>
        public static byte AsByte(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return 0;
            }
            else
            {
                return Convert.ToByte(value);
            }
        }

        /// <summary>
        /// Converts the value of the game data to a char. Use this method after the Load method.
        /// </summary>
        /// <param name="value">Value of the game data as a string because the Load method returns the game data value as a string.</param>
        /// <returns>Value of the game data as a char.</returns>
        public static char AsChar(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return ' ';
            }
            else
            {
                return value[0];
            }
        }

        /// <summary>
        /// Converts the value of the game data to a Color. Use this method after the Load method.
        /// </summary>
        /// <param name="value">Value of the game data as a string because the Load method returns the game data value as a string.</param>
        /// <returns>Value of the game data as a Color.</returns>
        public static Color AsColor(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return Color.clear;
            }
            else
            {
                float[] colorChannels = GetDividedFloats(value);
                return new Color(colorChannels[0], colorChannels[1], colorChannels[2], colorChannels[3]);
            }
        }

        /// <summary>
        /// Converts the value of the game data to a DateTime. Use this method after the Load method.
        /// </summary>
        /// <param name="value">Value of the game data as a string. Load method returns the data value as a string.</param>
        /// <returns>Value of the game data as a DateTime.</returns>
        public static DateTime AsDateTime(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return DateTime.MinValue;
            }
            else
            {
                float[] dateTimeParts = GetDividedFloats(value);
                return new DateTime((int)dateTimeParts[0], (int)dateTimeParts[1], (int)dateTimeParts[2], (int)dateTimeParts[3], (int)dateTimeParts[4], (int)dateTimeParts[5], (int)dateTimeParts[6]);
            }
        }

        /// <summary>
        /// Converts the value of the game data to a decimal. Use this method after the Load method.
        /// </summary>
        /// <param name="value">Value of the game data as a string because the Load method returns the game data value as a string.</param>
        /// <returns>Value of the game data as a decimal.</returns>
        public static decimal AsDecimal(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return 0;
            }
            else
            {
                return Convert.ToDecimal(value);
            }
        }

        /// <summary>
        /// Converts the value of the game data to a double. Use this method after the Load method.
        /// </summary>
        /// <param name="value">Value of the game data as a string because the Load method returns the game data value as a string.</param>
        /// <returns>Value of the game data as a double.</returns>
        public static double AsDouble(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return 0D;
            }
            else
            {
                return Convert.ToDouble(value);
            }
        }

        /// <summary>
        /// Converts the value of the game data to a float. Use this method after the Load method.
        /// </summary>
        /// <param name="value">Value of the game data as a string because the Load method returns the game data value as a string.</param>
        /// <returns>Value of the game data as a float.</returns>
        public static float AsFloat(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return 0F;
            }
            else
            {
                return Convert.ToSingle(value);
            }
        }

        /// <summary>
        /// Converts the value of the game data to an integer. Use this method after the Load method.
        /// </summary>
        /// <param name="value">Value of the game data as a string because the Load method returns the game data value as a string.</param>
        /// <returns>Value of the game data as an integer.</returns>
        public static int AsInt(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(value);
            }
        }

        /// <summary>
        /// Converts the value of the game data to a long. Use this method after the Load method.
        /// </summary>
        /// <param name="value">Value of the game data as a string because the Load method returns the game data value as a string.</param>
        /// <returns>Value of the game data as a long.</returns>
        public static long AsLong(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return 0;
            }
            else
            {
                return Convert.ToInt64(value);
            }
        }

        /// <summary>
        /// Converts the value of the game data to a Quaternion. Use this method after the Load method.
        /// </summary>
        /// <param name="value">Value of the game data as a string because the Load method returns the game data value as a string.</param>
        /// <returns>Value of the game data as a Quaternion.</returns>
        public static Quaternion AsQuaternion(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return new Quaternion(0F, 0F, 0F, 0F);
            }
            else
            {
                float[] quaternionParts = GetDividedFloats(value);
                return new Quaternion(quaternionParts[0], quaternionParts[1], quaternionParts[2], quaternionParts[3]);
            }
        }

        /// <summary>
        /// Converts the value of the game data to a sbyte. Use this method after the Load method.
        /// </summary>
        /// <param name="value">Value of the game data as a string because the Load method returns the game data value as a string.</param>
        /// <returns>Value of the game data as a sbyte.</returns>
        public static sbyte AsSbyte(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return 0;
            }
            else
            {
                return Convert.ToSByte(value);
            }
        }

        /// <summary>
        /// Converts the value of the game data to a short. Use this method after the Load method.
        /// </summary>
        /// <param name="value">Value of the game data as a string because the Load method returns the game data value as a string.</param>
        /// <returns>Value of the game data as a short.</returns>
        public static short AsShort(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return 0;
            }
            else
            {
                return Convert.ToInt16(value);
            }
        }

        /// <summary>
        /// Converts the value of the game data to a string. Use this method after the Load method.
        /// </summary>
        /// <param name="value">Value of the game data as a string because the Load method returns the game data value as a string.</param>
        /// <returns>Value of the game data as a string.</returns>
        public static string AsString(this string value)
        {
            return value;
        }

        /// <summary>
        /// Converts the value of the game data to a TimeSpan. Use this method after the Load method.
        /// </summary>
        /// <param name="value">Value of the game data as a string. Load method returns the data value as a string.</param>
        /// <returns>Value of the game data as a TimeSpan.</returns>
        public static TimeSpan AsTimeSpan(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return TimeSpan.Zero;
            }
            else
            {
                return new TimeSpan(Convert.ToInt64(value));
            }
        }

        /// <summary>
        /// Converts the value of the game data to an uint. Use this method after the Load method.
        /// </summary>
        /// <param name="value">Value of the game data as a string because the Load method returns the game data value as a string.</param>
        /// <returns>Value of the game data as an uint.</returns>
        public static uint AsUint(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return 0;
            }
            else
            {
                return Convert.ToUInt32(value);
            }
        }

        /// <summary>
        /// Converts the value of the game data to an ulong. Use this method after the Load method.
        /// </summary>
        /// <param name="value">Value of the game data as a string because the Load method returns the game data value as a string.</param>
        /// <returns>Value of the game data as an ulong.</returns>
        public static ulong AsUlong(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return 0;
            }
            else
            {
                return Convert.ToUInt64(value);
            }
        }

        /// <summary>
        /// Converts the value of the game data to an ushort. Use this method after the Load method.
        /// </summary>
        /// <param name="value">Value of the game data as a string because the Load method returns the game data value as a string.</param>
        /// <returns>Value of the game data as an ushort.</returns>
        public static ushort AsUshort(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return 0;
            }
            else
            {
                return Convert.ToUInt16(value);
            }
        }

        /// <summary>
        /// Converts the value of the game data to a Vector 2. Use this method after the Load method.
        /// </summary>
        /// <param name="value">Value of the game data as a string because the Load method returns the game data value as a string.</param>
        /// <returns>Value of the game data as a Vector 2.</returns>
        public static Vector2 AsVector2(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return Vector2.zero;
            }
            else
            {
                float[] vector2Parts = GetDividedFloats(value);
                return new Vector2(vector2Parts[0], vector2Parts[1]);
            }
        }

        /// <summary>
        /// Converts the value of the game data to a Vector 2 Int. Use this method after the Load method.
        /// </summary>
        /// <param name="value">Value of the game data as a string because the Load method returns the game data value as a string.</param>
        /// <returns>Value of the game data as a Vector 2 Int.</returns>
        public static Vector2Int AsVector2Int(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return Vector2Int.zero;
            }
            else
            {
                int[] vector2IntParts = GetDividedInts(value);
                return new Vector2Int(vector2IntParts[0], vector2IntParts[1]);
            }
        }

        /// <summary>
        /// Converts the value of the game data to a Vector 3. Use this method after the Load method.
        /// </summary>
        /// <param name="value">Value of the game data as a string because the Load method returns the game data value as a string.</param>
        /// <returns>Value of the game data as a Vector 3.</returns>
        public static Vector3 AsVector3(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return Vector3.zero;
            }
            else
            {
                float[] vector3Parts = GetDividedFloats(value);
                return new Vector3(vector3Parts[0], vector3Parts[1], vector3Parts[2]);
            }
        }

        /// <summary>
        /// Converts the value of the game data to a Vector 3 Int. Use this method after the Load method.
        /// </summary>
        /// <param name="value">Value of the game data as a string because the Load method returns the game data value as a string.</param>
        /// <returns>Value of the game data as a Vector 3 Int.</returns>
        public static Vector3Int AsVector3Int(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return Vector3Int.zero;
            }
            else
            {
                int[] vector3IntParts = GetDividedInts(value);
                return new Vector3Int(vector3IntParts[0], vector3IntParts[1], vector3IntParts[2]);
            }
        }

        /// <summary>
        /// Converts the value of the game data to a Vector 4. Use this method after the Load method.
        /// </summary>
        /// <param name="value">Value of the game data as a string because the Load method returns the game data value as a string.</param>
        /// <returns>Value of the game data as a Vector 4.</returns>
        public static Vector4 AsVector4(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return Vector4.zero;
            }
            else
            {
                float[] vector4Parts = GetDividedFloats(value);
                return new Vector4(vector4Parts[0], vector4Parts[1], vector4Parts[2], vector4Parts[3]);
            }
        }

        /// <summary>
        /// Converts the value of the game data to an array of booleans. Use this method after the LoadArray method.
        /// </summary>
        /// <param name="value">Value of the game data as a string array because the LoadArray method returns the game data value as a string array.</param>
        /// <returns>Value of the game data as an array of booleans.</returns>
        public static bool[] AsBoolArray(this string[] value)
        {
            bool[] result = new bool[value.Length];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = value[i].Equals("true");
            }

            return result;
        }

        /// <summary>
        /// Converts the value of the game data to an array of bytes. Use this method after the LoadArray method.
        /// </summary>
        /// <param name="value">Value of the game data as a string array because the LoadArray method returns the game data value as a string array.</param>
        /// <returns>Value of the game data as an array of bytes.</returns>
        public static byte[] AsByteArray(this string[] value)
        {
            byte[] result = new byte[value.Length];

            for (int i = 0; i < result.Length; i++)
            {
                if (String.IsNullOrEmpty(value[i]))
                {
                    result[i] = 0;
                }
                else
                {
                    result[i] = Convert.ToByte(value[i]);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the value of the game data to an array of chars. Use this method after the LoadArray method.
        /// </summary>
        /// <param name="value">Value of the game data as a string array because the LoadArray method returns the game data value as a string array.</param>
        /// <returns>Value of the game data as an array of chars.</returns>
        public static char[] AsCharArray(this string[] value)
        {
            char[] result = new char[value.Length];

            for (int i = 0; i < result.Length; i++)
            {
                if (String.IsNullOrEmpty(value[i]))
                {
                    result[i] = ' ';
                }
                else
                {
                    result[i] = value[i][0];
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the value of the game data to an array of colors. Use this method after the LoadArray method.
        /// </summary>
        /// <param name="value">Value of the game data as a string array because the LoadArray method returns the game data value as a string array.</param>
        /// <returns>Value of the game data as an array of colors.</returns>
        public static Color[] AsColorArray(this string[] value)
        {
            Color[] result = new Color[value.Length];
            float[] colorChannels;

            for (int i = 0; i < result.Length; i++)
            {
                if (String.IsNullOrEmpty(value[i]))
                {
                    result[i] = Color.clear;
                }
                else
                {
                    colorChannels = GetDividedFloats(value[i]);
                    result[i] = new Color(colorChannels[0], colorChannels[1], colorChannels[2], colorChannels[3]);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the value of the game data to an array of DateTime. Use this method after the LoadArray method.
        /// </summary>
        /// <param name="value">Value of the game data as a string array because the LoadArray method returns the game data value as a string array.</param>
        /// <returns>Value of the game data as an array of DateTime.</returns>
        public static DateTime[] AsDateTimeArray(this string[] value)
        {
            DateTime[] result = new DateTime[value.Length];
            float[] dateTimeParts;

            for (int i = 0; i < result.Length; i++)
            {
                if (String.IsNullOrEmpty(value[i]))
                {
                    result[i] = DateTime.MinValue;
                }
                else
                {
                    dateTimeParts = GetDividedFloats(value[i]);
                    result[i] = new DateTime((int)dateTimeParts[0], (int)dateTimeParts[1], (int)dateTimeParts[2], (int)dateTimeParts[3], (int)dateTimeParts[4], (int)dateTimeParts[5], (int)dateTimeParts[6]);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the value of the game data to an array of decimals. Use this method after the LoadArray method.
        /// </summary>
        /// <param name="value">Value of the game data as a string array because the LoadArray method returns the game data value as a string array.</param>
        /// <returns>Value of the game data as an array of decimals.</returns>
        public static decimal[] AsDecimalArray(this string[] value)
        {
            decimal[] result = new decimal[value.Length];

            for (int i = 0; i < result.Length; i++)
            {
                if (String.IsNullOrEmpty(value[i]))
                {
                    result[i] = 0;
                }
                else
                {
                    result[i] = Convert.ToDecimal(value[i]);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the value of the game data to an array of doubles. Use this method after the LoadArray method.
        /// </summary>
        /// <param name="value">Value of the game data as a string array because the LoadArray method returns the game data value as a string array.</param>
        /// <returns>Value of the game data as an array of doubles.</returns>
        public static double[] AsDoubleArray(this string[] value)
        {
            double[] result = new double[value.Length];

            for (int i = 0; i < result.Length; i++)
            {
                if (String.IsNullOrEmpty(value[i]))
                {
                    result[i] = 0D;
                }
                else
                {
                    result[i] = Convert.ToDouble(value[i]);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the value of the game data to an array of floats. Use this method after the LoadArray method.
        /// </summary>
        /// <param name="value">Value of the game data as a string array because the LoadArray method returns the game data value as a string array.</param>
        /// <returns>Value of the game data as an array of floats.</returns>
        public static float[] AsFloatArray(this string[] value)
        {
            float[] result = new float[value.Length];

            for (int i = 0; i < result.Length; i++)
            {
                if (String.IsNullOrEmpty(value[i]))
                {
                    result[i] = 0F;
                }
                else
                {
                    result[i] = Convert.ToSingle(value[i]);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the value of the game data to an array of integers. Use this method after the LoadArray method.
        /// </summary>
        /// <param name="value">Value of the game data as a string array because the LoadArray method returns the game data value as a string array.</param>
        /// <returns>Value of the game data as an array of integers.</returns>
        public static int[] AsIntArray(this string[] value)
        {
            int[] result = new int[value.Length];

            for (int i = 0; i < result.Length; i++)
            {
                if (String.IsNullOrEmpty(value[i]))
                {
                    result[i] = 0;
                }
                else
                {
                    result[i] = Convert.ToInt32(value[i]);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the value of the game data to an array of longs. Use this method after the LoadArray method.
        /// </summary>
        /// <param name="value">Value of the game data as a string array because the LoadArray method returns the game data value as a string array.</param>
        /// <returns>Value of the game data as an array of longs.</returns>
        public static long[] AsLongArray(this string[] value)
        {
            long[] result = new long[value.Length];

            for (int i = 0; i < result.Length; i++)
            {
                if (String.IsNullOrEmpty(value[i]))
                {
                    result[i] = 0;
                }
                else
                {
                    result[i] = Convert.ToInt64(value[i]);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the value of the game data to an array of Quaternions. Use this method after the LoadArray method.
        /// </summary>
        /// <param name="value">Value of the game data as a string array because the LoadArray method returns the game data value as a string array.</param>
        /// <returns>Value of the game data as an array of Quaternions.</returns>
        public static Quaternion[] AsQuaternionArray(this string[] value)
        {
            Quaternion[] result = new Quaternion[value.Length];
            float[] quaternionParts;

            for (int i = 0; i < result.Length; i++)
            {
                if (String.IsNullOrEmpty(value[i]))
                {
                    result[i] = new Quaternion(0F, 0F, 0F, 0F);
                }
                else
                {
                    quaternionParts = GetDividedFloats(value[i]);
                    result[i] = new Quaternion(quaternionParts[0], quaternionParts[1], quaternionParts[2], quaternionParts[3]);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the value of the game data to an array of sbytes. Use this method after the LoadArray method.
        /// </summary>
        /// <param name="value">Value of the game data as a string array because the LoadArray method returns the game data value as a string array.</param>
        /// <returns>Value of the game data as an array of sbytes.</returns>
        public static sbyte[] AsSbyteArray(this string[] value)
        {
            sbyte[] result = new sbyte[value.Length];

            for (int i = 0; i < result.Length; i++)
            {
                if (String.IsNullOrEmpty(value[i]))
                {
                    result[i] = 0;
                }
                else
                {
                    result[i] = Convert.ToSByte(value[i]);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the value of the game data to an array of shorts. Use this method after the LoadArray method.
        /// </summary>
        /// <param name="value">Value of the game data as a string array because the LoadArray method returns the game data value as a string array.</param>
        /// <returns>Value of the game data as an array of shorts.</returns>
        public static short[] AsShortArray(this string[] value)
        {
            short[] result = new short[value.Length];

            for (int i = 0; i < result.Length; i++)
            {
                if (String.IsNullOrEmpty(value[i]))
                {
                    result[i] = 0;
                }
                else
                {
                    result[i] = Convert.ToInt16(value[i]);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the value of the game data to an array of string. Use this method after the LoadArray method.
        /// </summary>
        /// <param name="value">Value of the game data as a string array because the LoadArray method returns the game data value as a string array.</param>
        /// <returns>Value of the game data as an array of strings.</returns>
        public static string[] AsStringArray(this string[] value)
        {
            return value;
        }

        /// <summary>
        /// Converts the value of the game data to an array of TimeSpan. Use this method after the LoadArray method.
        /// </summary>
        /// <param name="value">Value of the data as a TimeSpan array. LoadArray method returns the data value as a string array.</param>
        /// <returns>Value of the game data as an array of TimeSpans.</returns>
        public static TimeSpan[] AsTimeSpanArray(this string[] value)
        {
            TimeSpan[] result = new TimeSpan[value.Length];

            for (int i = 0; i < result.Length; i++)
            {
                if (String.IsNullOrEmpty(value[i]))
                {
                    result[i] = TimeSpan.Zero;
                }
                else
                {
                    result[i] = new TimeSpan(Convert.ToInt64(value[i]));
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the value of the game data to an array of uints. Use this method after the LoadArray method.
        /// </summary>
        /// <param name="value">Value of the game data as a string array because the LoadArray method returns the game data value as a string array.</param>
        /// <returns>Value of the game data as an array of uints.</returns>
        public static uint[] AsUintArray(this string[] value)
        {
            uint[] result = new uint[value.Length];

            for (int i = 0; i < result.Length; i++)
            {
                if (String.IsNullOrEmpty(value[i]))
                {
                    result[i] = 0;
                }
                else
                {
                    result[i] = Convert.ToUInt32(value[i]);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the value of the game data to an array of ulongs. Use this method after the LoadArray method.
        /// </summary>
        /// <param name="value">Value of the game data as a string array because the LoadArray method returns the game data value as a string array.</param>
        /// <returns>Value of the game data as an array of ulongs.</returns>
        public static ulong[] AsUlongArray(this string[] value)
        {
            ulong[] result = new ulong[value.Length];

            for (int i = 0; i < result.Length; i++)
            {
                if (String.IsNullOrEmpty(value[i]))
                {
                    result[i] = 0;
                }
                else
                {
                    result[i] = Convert.ToUInt64(value[i]);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the value of the game data to an array of ushorts. Use this method after the LoadArray method.
        /// </summary>
        /// <param name="value">Value of the game data as a string array because the LoadArray method returns the game data value as a string array.</param>
        /// <returns>Value of the game data as an array of ushorts.</returns>
        public static ushort[] AsUshortArray(this string[] value)
        {
            ushort[] result = new ushort[value.Length];

            for (int i = 0; i < result.Length; i++)
            {
                if (String.IsNullOrEmpty(value[i]))
                {
                    result[i] = 0;
                }
                else
                {
                    result[i] = Convert.ToUInt16(value[i]);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the value of the game data to an array of Vector2. Use this method after the LoadArray method.
        /// </summary>
        /// <param name="value">Value of the game data as a string array because the LoadArray method returns the game data value as a string array.</param>
        /// <returns>Value of the game data as an array of Vector2.</returns>
        public static Vector2[] AsVector2Array(this string[] value)
        {
            Vector2[] result = new Vector2[value.Length];
            float[] vector2Parts;

            for (int i = 0; i < result.Length; i++)
            {
                if (String.IsNullOrEmpty(value[i]))
                {
                    result[i] = Vector2.zero;
                }
                else
                {
                    vector2Parts = GetDividedFloats(value[i]);
                    result[i] = new Vector2(vector2Parts[0], vector2Parts[1]);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the value of the game data to an array of Vector2Int. Use this method after the LoadArray method.
        /// </summary>
        /// <param name="value">Value of the game data as a string array because the LoadArray method returns the game data value as a string array.</param>
        /// <returns>Value of the game data as an array of Vector2Int.</returns>
        public static Vector2Int[] AsVector2IntArray(this string[] value)
        {
            Vector2Int[] result = new Vector2Int[value.Length];
            int[] vector2IntParts;

            for (int i = 0; i < result.Length; i++)
            {
                if (String.IsNullOrEmpty(value[i]))
                {
                    result[i] = Vector2Int.zero;
                }
                else
                {
                    vector2IntParts = GetDividedInts(value[i]);
                    result[i] = new Vector2Int(vector2IntParts[0], vector2IntParts[1]);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the value of the game data to an array of Vector3. Use this method after the LoadArray method.
        /// </summary>
        /// <param name="value">Value of the game data as a string array because the LoadArray method returns the game data value as a string array.</param>
        /// <returns>Value of the game data as an array of Vector3.</returns>
        public static Vector3[] AsVector3Array(this string[] value)
        {
            Vector3[] result = new Vector3[value.Length];
            float[] vector3Parts;

            for (int i = 0; i < result.Length; i++)
            {
                if (String.IsNullOrEmpty(value[i]))
                {
                    result[i] = Vector3.zero;
                }
                else
                {
                    vector3Parts = GetDividedFloats(value[i]);
                    result[i] = new Vector3(vector3Parts[0], vector3Parts[1], vector3Parts[2]);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the value of the game data to an array of Vector3Int. Use this method after the LoadArray method.
        /// </summary>
        /// <param name="value">Value of the game data as a string array because the LoadArray method returns the game data value as a string array.</param>
        /// <returns>Value of the game data as an array of Vector3Int.</returns>
        public static Vector3Int[] AsVector3IntArray(this string[] value)
        {
            Vector3Int[] result = new Vector3Int[value.Length];
            int[] vector3IntParts;

            for (int i = 0; i < result.Length; i++)
            {
                if (String.IsNullOrEmpty(value[i]))
                {
                    result[i] = Vector3Int.zero;
                }
                else
                {
                    vector3IntParts = GetDividedInts(value[i]);
                    result[i] = new Vector3Int(vector3IntParts[0], vector3IntParts[1], vector3IntParts[2]);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the value of the game data to an array of Vector4. Use this method after the LoadArray method.
        /// </summary>
        /// <param name="value">Value of the game data as a string array because the LoadArray method returns the game data value as a string array.</param>
        /// <returns>Value of the game data as an array of Vector4.</returns>
        public static Vector4[] AsVector4Array(this string[] value)
        {
            Vector4[] result = new Vector4[value.Length];
            float[] vector4Parts;

            for (int i = 0; i < result.Length; i++)
            {
                if (String.IsNullOrEmpty(value[i]))
                {
                    result[i] = Vector4.zero;
                }
                else
                {
                    vector4Parts = GetDividedFloats(value[i]);
                    result[i] = new Vector4(vector4Parts[0], vector4Parts[1], vector4Parts[2], vector4Parts[3]);
                }
            }

            return result;
        }
    }
}
