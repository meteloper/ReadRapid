using System.Collections;
using System.Collections.Generic;
using System.IO;
using Metelab;
using UnityEngine;

namespace Metelab.CommonManagers
{
    public class LocalFileManager
    {
        public static void SetJSONFile(string fileName,string jsonData)
        {
            string filePath = Path.Combine(Application.persistentDataPath, $"{fileName}.json");

            if (File.Exists(filePath))
                File.Delete(filePath);

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(jsonData);
            }
        }

        public static string GetJSONFile(string fileName,string defautString)
        {
            string filePath = Path.Combine(Application.persistentDataPath, $"{fileName}.json");

            if (!File.Exists(filePath))
                return defautString;

            using (StreamReader reader = new StreamReader(filePath))
            {
                return reader.ReadToEnd();
            }
        }

        public static void DeleteJSONFile(string fileName)
        {
            string filePath = Path.Combine(Application.persistentDataPath, $"{fileName}.json");

            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
