using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Split.LevelLoading;

namespace Split.Builder {
    /// <summary>
    /// Handles functionality in the main page of the Builder opening menu
    /// </summary>
    public class BuilderMainMenu : MonoBehaviour {
        [Header("UI References")]
        [SerializeField] private GameObject createMenu;
        [SerializeField] private GameObject openingMenu;
        [SerializeField] private GameObject levelsContent;
        [SerializeField] private LevelEntry levelListPrefab;

        [Header("Other References")]
        [SerializeField] private BuilderManager sceneManager;

        private LevelSerializer levelSerializer;
        private List<LevelEntry> levelEntries;

        private void Awake() {
            this.levelSerializer = new LevelSerializer();
            this.levelEntries = new List<LevelEntry>();

            PopulateLevelEntries();
            openingMenu.SetActive(true);
        }

        /// <summary>
        /// Adds entries for the list of levels on the left side of the main page
        /// </summary>
        private void PopulateLevelEntries() {
            //Check if directory exists
            if (Directory.Exists(levelSerializer.GetDefaultDirectoryPath())) {
                foreach (string filePath in Directory.GetFiles(levelSerializer.GetDefaultDirectoryPath(), "*.json")) {
                    LevelEntry entry = null;
                    try {
                        LevelData data;
                        
                        //Load the .json through the serializer into a level data object
                        if (levelSerializer.Load(out data, filePath)) {
                            //Instantiate a new Level Entry UI object based on the prefab
                            entry = Instantiate(levelListPrefab.gameObject, Vector3.zero, Quaternion.identity, levelsContent.transform).GetComponent<LevelEntry>();

                            //Set data accordingly
                            entry.SetFileName(Path.GetFileName(filePath));
                            entry.SetTitle(
                                !string.IsNullOrEmpty(data.levelName) ? 
                                data.levelName : 
                                Path.GetFileNameWithoutExtension(filePath)
                            );

                            entry.SetDescription(data.levelDescription);
                            entry.Data = new BuilderLevelData(data, Path.GetFileName(filePath));
                            entry.Manager = this.sceneManager;

                            //Add entry to the list
                            levelEntries.Add(entry);
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
        }

        //TODO: Works on Mac and Windows, haven't tested Linux yet
        public void OpenFolder() {
            Application.OpenURL("file://" + levelSerializer.GetDefaultDirectoryPath());
        }

        /// <summary>
        /// Closes main page and opens Create page
        /// </summary>
        public void CreateMenu() {
            openingMenu.SetActive(false);
            createMenu.SetActive(true);
        }

        /// <summary>
        /// Closes Create page and opens main page
        /// </summary>
        public void CloseCreateMenu() {
            openingMenu.SetActive(true);
            createMenu.SetActive(false);
        }

        /// <summary>
        /// Closes all menus in the opening menus
        /// </summary>
        public void CloseAllMenus() {
            openingMenu.SetActive(false);
            createMenu.SetActive(false);
        }


    }

}