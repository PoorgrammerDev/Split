using System.IO;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_InputField))]
public class FileNameMirror : MonoBehaviour
{
    [SerializeField] private TMP_InputField levelName;
    private TMP_InputField fileName;

    private bool edited;
    private string invalids;

    public void Start() {
        this.fileName = this.GetComponent<TMP_InputField>();
        this.edited = false;
        this.invalids = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

        MirrorNameField();
    }

    public bool IsEdited() {
        return edited;
    }

    public void SetEdited(bool edited) {
        this.edited = edited;
    }

    public void MirrorNameField() {
        if (this.edited) return;

        string name = levelName.text;

        if (string.IsNullOrEmpty(name)) {
            name = GetNextDefault();
        }
        else {
            foreach (char c in this.invalids) {
                name = name.Replace(c, '-');
            }
        }

        name += ".json";
        fileName.text = name;
    }

    private string GetNextDefault() {
        int num = 0;
        while (File.Exists("New Level " + num + ".json")) {
            num++;
        }

        return ("New Level " + num);
    }
    
}
