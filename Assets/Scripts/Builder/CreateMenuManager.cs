using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Split.Tiles;
using Split.LevelLoading;

namespace Split.Builder {
    /// <summary>
    /// Handles functionality of the Create page of the opening menu 
    /// </summary>
    public class CreateMenuManager : MonoBehaviour {
        const string BUTTON_TEXT = "Create";

        [Header("UI Inputs")]
        [SerializeField] private TMP_InputField gridX;
        [SerializeField] private TMP_InputField gridY;
        [SerializeField] private Slider maxPlayers;
        [SerializeField] private TMP_InputField levelName;
        [SerializeField] private TMP_InputField fileName;
        [SerializeField] private TMP_InputField levelDescription;

        [Header("Other UI References")]
        [SerializeField] private Slider progressBar;
        [SerializeField] private Image createButtonBackground;
        [SerializeField] private TextMeshProUGUI createButtonText;

        [Header("Other References")]
        [SerializeField] private BuilderManager sceneManager;

        [Header("Settings")]
        [SerializeField] private Color buttonRegular;
        [SerializeField] private Color buttonError;

        private WaitForSecondsRealtime wait;
        private LevelSerializer serializer;

        private void Awake() {
            this.wait = new WaitForSecondsRealtime(1.5f);
            this.serializer = new LevelSerializer();
        }

        /// <summary>
        /// Handles all preliminary checks when the Create button is clicked.
        /// If inputs are valid, passes control to BeginCreatingStage.
        /// If not, displays an error message.
        /// </summary>
        public void OnClick() {
            int x, y;
            if (int.TryParse(gridX.text, out x) & int.TryParse(gridY.text, out y)) {
                //Check if file name is valid

                string path = Path.Combine(serializer.GetDefaultDirectoryPath(), fileName.text);
                FileStream stream = null;
                bool passedFileCheck = false;

                //File already exists
                if (File.Exists(path)) {
                    StartCoroutine(DisplayError("File Already Exists"));
                }

                //File does not exist
                else {
                    //Create the file to see if it's legal to exist
                    try {
                        stream = File.Create(path);
                    }
                    catch {
                        //If it fails to create then show error
                        StartCoroutine(DisplayError("Invalid File Name"));
                    }
                    finally {
                        //If it creates successfully then delete the file
                        if (File.Exists(path) && stream != null) {
                            stream.Close();
                            File.Delete(path);
                            passedFileCheck = true;
                        }
                    }
                    
                    //Passed file check
                    if (passedFileCheck) {
                        BeginCreatingStage(x, y);
                    }
                    
                }
            }
            else {
                StartCoroutine(DisplayError("Grid Size Invalid"));
            }
        }

        /// <summary>
        /// Creates a new BuilderLevelData with input info and calls scene manager to open the level
        /// </summary>
        /// <param name="x">Size of Grid: X</param>
        /// <param name="y">Size of Grid: Y</param>
        private void BeginCreatingStage(int x, int y) {
            BuilderLevelData data = new BuilderLevelData();
            data.levelName = levelName.text;
            data.levelDescription = levelDescription.text;
            data.fileName = fileName.text;
            data.maxPlayers = (int) maxPlayers.value;

            //Create the list
            data.gridData.Capacity = x;
            for (int i = 0; i < x; ++i) {
                data.gridData.Add(new List<TileType>(y));

                for (int j = 0; j < y; ++j) {
                    data.gridData[i].Add(TileType.EMPTY);
                }
            }

            sceneManager.OpenLevel(data);
        }

        /// <summary>
        /// Coroutine that changes the "Create" button to display an error message, and then reverts it.
        /// </summary>
        /// <param name="msg">Error message to display</param>
        private IEnumerator DisplayError(string msg) {
            float t = 0.0f;
            float progVal = progressBar.value;

            //To error color
            while (t <= 1) {
                progressBar.value = Mathf.Lerp(progVal, 0, t);
                createButtonBackground.color = Color.Lerp(buttonRegular, buttonError, t);
                t += 0.0375f; //Time-scale independent
                yield return null;
            }

            createButtonText.text = msg;
            yield return wait;

            //Back to regular color
            t = 0.0f;
            while (t <= 1) {
                createButtonBackground.color = Color.Lerp(buttonError, buttonRegular, t);
                t += 0.025f; //Time-scale independent
                yield return null;
            }  

            createButtonText.text = BUTTON_TEXT;
        }
    }
}

