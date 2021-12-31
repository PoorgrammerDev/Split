using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Split.LevelLoading;

namespace Split.Builder {
    public class BuilderMainMenu : MonoBehaviour {
        [Header("UI References")]
        [SerializeField] private GameObject createMenu;
        [SerializeField] private GameObject openingMenu;
        [SerializeField] private GameObject builderHUD;
        [SerializeField] private GameObject levelsContent;
        [SerializeField] private LevelEntry levelListPrefab;

        [Header("Other References")]
        [SerializeField] private BuilderLevelLoader levelLoader;
        [SerializeField] private GameObject target;

        private LevelSerializer levelSerializer;
        private List<LevelEntry> levelEntries;

        private void Awake() {
            this.levelSerializer = new LevelSerializer();
            this.levelEntries = new List<LevelEntry>();

            PopulateLevelEntries();
            openingMenu.SetActive(true);
        }

        private void PopulateLevelEntries() {
            //Check if directory exists
            if (Directory.Exists(levelSerializer.GetDefaultDirectoryPath())) {
                //TODO: do something with regex here to make .json case insensitive
                foreach (string filePath in Directory.GetFiles(levelSerializer.GetDefaultDirectoryPath(), "*.json")) {
                    LevelEntry entry = null;
                    try {
                        LevelData data;
                        if (levelSerializer.Load(out data, filePath)) {
                            entry = Instantiate(levelListPrefab.gameObject, Vector3.zero, Quaternion.identity, levelsContent.transform).GetComponent<LevelEntry>();

                            entry.SetFileName(Path.GetFileName(filePath));
                            entry.SetTitle(
                                !string.IsNullOrEmpty(data.levelName) ? 
                                data.levelName : 
                                Path.GetFileNameWithoutExtension(filePath)
                            );

                            entry.SetDescription(data.levelDescription);
                            entry.Data = new BuilderLevelData(data, Path.GetFileName(filePath));
                            entry.Manager = this;

                            levelEntries.Add(entry);
                        }
                    }
                    catch (System.Exception e) {
                        Debug.Log(e.StackTrace);

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

        public void CreateMenu() {
            openingMenu.SetActive(false);
            createMenu.SetActive(true);
        }

        public void CloseCreateMenu() {
            openingMenu.SetActive(true);
            createMenu.SetActive(false);
        }

        public void OpenLevel(BuilderLevelData data) {
            //Generate the level based on the data
            levelLoader.Generate(data);

            //Update the HUD accordingly
            openingMenu.SetActive(false);
            createMenu.SetActive(false);
            builderHUD.SetActive(true);
        }



    }

}