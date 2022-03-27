using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Split.Builder;

namespace Split.LevelLoading {
    /// <summary>
    /// Represents a loaded Level inside of the Builder Main Menu UI
    /// </summary>
    public class LevelEntry : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private TextMeshProUGUI fileName;
        [SerializeField] private Button button;
        [SerializeField] private Image icon;

        public BuilderLevelData Data {get; set;}

        public void OnClick() {
            //fires a level entry press event
            MenuEvents.current.OnLevelEntryPress(this);
        }

        public void SetTitle(string str) {
            this.title.text = str;
        }

        public void SetDescription(string str) {
            this.description.text = str;
        }

        public void SetFileName(string str) {
            this.fileName.text = str;
        }

        public void SetIconColor(Color color) {
            icon.color = color;
        }
    }
}


