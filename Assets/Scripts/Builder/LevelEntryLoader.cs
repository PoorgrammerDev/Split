using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Split.LevelLoading;

namespace Split.Builder {
    public class LevelEntryLoader : MonoBehaviour
    {
        [SerializeField] private GameObject layoutBox;
        [SerializeField] private LevelEntry levelListPrefab;

        public List<LevelEntry> entries {get; set;}

        private void Awake() {
            this.entries = PopulateLevelEntries();
        }

        /// <summary>
        /// Adds entries for the list of levels on the left side of the main page
        /// </summary>
        public List<LevelEntry> PopulateLevelEntries() {
            List<LevelEntry> entries = new List<LevelEntry>();

            //Check if directory exists
            if (Directory.Exists(LevelSerializer.GetDefaultDirectoryPath())) {
                foreach (string filePath in Directory.GetFiles(LevelSerializer.GetDefaultDirectoryPath(), "*.json")) {
                    LevelEntry entry = null;
                    try {
                        LevelData data;
                        
                        //Load the .json through the serializer into a level data object
                        if (LevelSerializer.Load(out data, filePath)) {
                            //Instantiate a new Level Entry UI object based on the prefab
                            entry = Instantiate(levelListPrefab.gameObject, Vector3.zero, Quaternion.identity, layoutBox.transform).GetComponent<LevelEntry>();

                            //Set data accordingly
                            entry.SetFileName(Path.GetFileName(filePath));
                            entry.SetTitle(
                                !string.IsNullOrEmpty(data.levelName) ? 
                                data.levelName : 
                                Path.GetFileNameWithoutExtension(filePath)
                            );

                            entry.SetDescription(data.levelDescription);
                            entry.Data = new BuilderLevelData(data, Path.GetFileName(filePath));

                            //Add entry to the list
                            entries.Add(entry);
                        }
                    }   
                    //If any level fails to load at any point, just delete the entry and move on to the next one
                    catch {
                        if (entry != null) {
                            Destroy(entry.gameObject);
                        }

                        continue;
                    }
                }
            }

            return entries;
        }
    }
}


