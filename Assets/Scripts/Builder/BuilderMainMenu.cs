
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

        [Header("Other References")]
        [SerializeField] private BuilderManager sceneManager;
        [SerializeField] private LevelEntryLoader entryLoader;
        private List<LevelEntry> levelEntries;

        private void Start() {
            openingMenu.SetActive(true);
        }



        //TODO: Works on Mac and Windows, haven't tested Linux yet
        public void OpenFolder() {
            Application.OpenURL("file://" + LevelSerializer.GetDefaultDirectoryPath());
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