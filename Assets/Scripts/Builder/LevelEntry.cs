using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class LevelEntry : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI fileName;
    [SerializeField] private Button button;
    [SerializeField] private Image icon;

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
