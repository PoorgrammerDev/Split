using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Split.LevelLoading;

namespace Split.Builder {
    public class LevelEntry : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private TextMeshProUGUI fileName;
        [SerializeField] private Button button;
        [SerializeField] private Image icon;

        public BuilderLevelData Data {get; set;}
        public BuilderMainMenu Manager {get; set;}

        public void OnClick() {
            Manager.OpenLevel(Data);
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


