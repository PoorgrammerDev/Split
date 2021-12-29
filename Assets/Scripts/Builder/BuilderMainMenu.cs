using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Split.LevelLoading;

namespace Split.Builder {
    public class BuilderMainMenu : MonoBehaviour {
        [Header("UI References")]
        [SerializeField] private GameObject createMenu;
        [SerializeField] private GameObject openingMenu;
        [SerializeField] private GameObject levelsContent;
        [SerializeField] private GameObject levelListPrefab;

        private LevelSerializer levelSerializer;
        private List<LevelData> levels;

        private void Awake() {
            this.levelSerializer = new LevelSerializer();
            this.levels = new List<LevelData>();

            //Check if directory exists
            if (Directory.Exists(levelSerializer.GetDefaultDirectoryPath())) {
                foreach (string filePath in Directory.GetFiles(levelSerializer.GetDefaultDirectoryPath())) {
                    LevelData data;
                    if (levelSerializer.Load(out data, filePath)) {
                        levels.Add(data);
                        //TODO: Instantiate the UI prefab
                    }
                }
            }
        }



    }

}