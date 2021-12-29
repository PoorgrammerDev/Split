using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Split.Tiles;

namespace Split.Builder {
    public class CreateLevelManager : MonoBehaviour {
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
        [SerializeField] private Image background;
        [SerializeField] private TextMeshProUGUI createButtonText;

        [Header("Settings")]
        [SerializeField] private Color buttonRegular;
        [SerializeField] private Color buttonError;

        private WaitForSecondsRealtime wait3sRT = new WaitForSecondsRealtime(3);

        public void OnClick() {
            BuilderLevelData data = new BuilderLevelData();
            data.levelName = levelName.text;
            data.levelDescription = levelDescription.text;
            
            int x, y;
            if (int.TryParse(gridX.text, out x) & int.TryParse(gridY.text, out y)) {
                //TODO: maybe add a warning for grid sizes over 100

                data.gridData = new List<List<TileType>>(x);
                for (int i = 0; i < x; ++i) {
                    data.gridData[i] = new List<TileType>(y);
                }


            }
            else {
                StartCoroutine(DisplayError("ERROR: Grid Size Invalid"));
            }

        }

        private IEnumerator DisplayError(string msg) {
            float t = 0.0f;
            float progVal = progressBar.value;

            while (t <= 1) {
                progressBar.value = Mathf.Lerp(progVal, 0, t);
                background.color = Color.Lerp(buttonRegular, buttonError, t);
                t += 0.0375f;
                yield return null;
            }

            createButtonText.text = msg;
            yield return wait3sRT;

            t = 0.0f;
            while (t <= 1) {
                background.color = Color.Lerp(buttonError, buttonRegular, t);
                t += 0.025f;
                yield return null;
            }
            createButtonText.text = BUTTON_TEXT;
        }
    }
}

