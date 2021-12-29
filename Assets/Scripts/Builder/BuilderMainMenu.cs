using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Split.LevelLoading;

namespace Split.Builder {
    public class BuilderMainMenu : MonoBehaviour {
        [Header("UI References")]
        [SerializeField] private GameObject createMenu;
        [SerializeField] private GameObject openingMenu;
        [SerializeField] private GameObject levelsContent;
        [SerializeField] private LevelEntry levelListPrefab;

        private LevelSerializer levelSerializer;
        private List<LevelData> levels;
        private List<LevelEntry> levelEntries;

        private void Awake() {
            this.levelSerializer = new LevelSerializer();
            this.levels = new List<LevelData>();
            this.levelEntries = new List<LevelEntry>();

            PopulateLevelEntries();
        }

        private void PopulateLevelEntries() {
            //Check if directory exists
            if (Directory.Exists(levelSerializer.GetDefaultDirectoryPath())) {
                //TODO: do something with regex here to make .json case insensitive
                foreach (string filePath in Directory.GetFiles(levelSerializer.GetDefaultDirectoryPath(), "*.json")) {
                    LevelData data;
                    if (levelSerializer.Load(out data, filePath)) {
                        levels.Add(data);

                        LevelEntry entry = Instantiate(levelListPrefab.gameObject, Vector3.zero, Quaternion.identity, levelsContent.transform).GetComponent<LevelEntry>();
                        entry.SetFileName(Path.GetFileName(filePath));
                        entry.SetTitle(data.levelName);
                        entry.SetDescription(data.levelDescription);

                        levelEntries.Add(entry);

                    }
                }
            }
        }

        //TODO: Works on Mac, haven't tested Windowws or Linux yet
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



    }

}