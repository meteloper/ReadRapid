using System;
using System.Security.Cryptography;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;


namespace Metelab.CommonManagers
{
    public class LocalPropertiesManager : MeteSingleton<LocalPropertiesManager>
    {
        private const string TRUE = "T";
        private const string FALSE = "F";
        private const string HASH_OFFSET = "WordMemorizer";
        private MD5 mMD5;


        private static string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }


        public override void EarlyInit()
        {
            base.EarlyInit();
            mMD5 = MD5.Create();
        }

        public void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        public void DeleteSafeKey(string key)
        {
            PlayerPrefs.DeleteKey(key + "_hash");
            PlayerPrefs.DeleteKey(key);
        }

        #region Integer

        public void SetInt(string key, int x)
        {
            Metelab.Log(this, $"[{key}]: {x}");
            PlayerPrefs.SetInt(key,x);
        }

        public void SetIntSafe(string key, int value)
        {
            string hash = GetMd5Hash(mMD5, key + value + HASH_OFFSET);
            PlayerPrefs.SetString(key + "_hash", hash);
            PlayerPrefs.SetInt(key, value);
        }

        public int GetInt(string key, int defaultValue)
        {
            Metelab.Log(this, key);
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public int GetIntSafe(string key, int defaultValue)
        {
            string oldHash = PlayerPrefs.GetString(key + "_hash", string.Empty);
            int value = PlayerPrefs.GetInt(key, defaultValue);
            string hash = GetMd5Hash(mMD5, key + value + HASH_OFFSET);

            if (hash == oldHash)
                return value;
            else
                return defaultValue;
        }

        #endregion


        #region String

        public void SetString(string key, string x)
        {
            Metelab.Log(this, $"[{key}]: {x}");
            PlayerPrefs.SetString(key, x);
        }

        public void SetStringSafe(string key, string value)
        {
            string hash = GetMd5Hash(mMD5, key + value + HASH_OFFSET);
            PlayerPrefs.SetString(key + "_hash", hash);
            PlayerPrefs.SetString(key, value);
        }

        public string GetString(string key, string defaultValue)
        {
            Metelab.Log(this,key);
            return PlayerPrefs.GetString(key, defaultValue);
        }

        public string GetStringSafe(string key, string defaultValue)
        {
            string oldHash = PlayerPrefs.GetString(key + "_hash", string.Empty);
            string value = PlayerPrefs.GetString(key, defaultValue);
            string hash = GetMd5Hash(mMD5, key + value + HASH_OFFSET);

            if (hash == oldHash)
                return value;
            else
                return defaultValue;
        }

        #endregion


        #region Boolean

        public void SetBool(string key, bool value)
        {
            Metelab.Log(this, $"[{key}]: {value}");
            PlayerPrefs.SetString(key, value ? TRUE : FALSE);
        }

        public void SetBoolSafe(string key, bool value)
        {
            string hash = GetMd5Hash(mMD5, key + value + HASH_OFFSET);
            PlayerPrefs.SetString(key + "_hash", hash);
            PlayerPrefs.SetString(key, value ? TRUE : FALSE);
        }

        public bool GetBool(string key, bool defaultValue)
        {
            Metelab.Log(this, key);
            return PlayerPrefs.GetString(key, defaultValue? TRUE: FALSE) == TRUE;
        }

        public bool GetBoolSafe(string key, bool defaultValue)
        {
            string oldHash = PlayerPrefs.GetString(key + "_hash", string.Empty);
            string value = PlayerPrefs.GetString(key, defaultValue ? TRUE : FALSE);
            string hash = GetMd5Hash(mMD5, key + value + HASH_OFFSET);

            if (hash == oldHash)
                return value == TRUE;
            else
                return (defaultValue ? TRUE : FALSE) == TRUE;
        }

        #endregion
    }
}