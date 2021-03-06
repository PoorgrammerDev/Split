using System.IO;
using System.Text;
using UnityEngine;

namespace Split.LevelLoading {
    /// <summary>
    /// Saves and loads LevelData objects to/from JSON files
    /// </summary>
    public static class LevelSerializer {
        public const string DEFAULT_DIRECTORY = "SplitLevels";
        
        public static bool Save(LevelData data, string fileName, bool force) {
            return Save(data, fileName, GetDefaultDirectoryPath(), force);
        }
        
        public static bool Save(LevelData data, string fileName, string directoryPath, bool force) {
            string fullPath = Path.Combine(directoryPath, fileName);

            //Makes directory if it doesn't exist
            if (!Directory.Exists(directoryPath)) {
                Directory.CreateDirectory(directoryPath);
            }

            //Level Data already exists & No FORCE -> Exit
            if (!force && File.Exists(fullPath)) return false;

            FileStream fileStream = File.Create(fullPath);
            byte[] dataJSON = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));

            fileStream.Write(dataJSON, 0, dataJSON.Length);
            
            fileStream.Close();
            return true;
        }

        public static bool Load(out LevelData data, string fileName) {
            return Load(out data, fileName, GetDefaultDirectoryPath());
        }

        public static bool Load(out LevelData data, string fileName, string directoryPath) {
            string fullPath = Path.Combine(directoryPath, fileName);
            data = null;

            if (!Directory.Exists(directoryPath) || !File.Exists(fullPath)) return false;

            byte[] dataJSON = File.ReadAllBytes(fullPath);
            data = JsonUtility.FromJson<LevelData>(Encoding.UTF8.GetString(dataJSON, 0, dataJSON.Length));

            return true;
        }

        public static bool CheckFileExists(string fileName) {
            return CheckFileExists(GetDefaultDirectoryPath(), fileName);
        }

        public static bool CheckFileExists(string directoryName, string fileName) {
            return File.Exists(Path.Combine(directoryName, fileName));
        }

        public static string GetDefaultDirectoryPath() {
            return Path.Combine(Application.dataPath, DEFAULT_DIRECTORY);
        }

    }

}