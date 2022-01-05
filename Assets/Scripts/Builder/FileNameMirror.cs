using System.IO;
using UnityEngine;
using TMPro;

namespace Split.Builder {
    /// <summary>
    /// Used in the Create Menu: Mirrors text of Name field into the File field. Also filters out illegal characters.
    /// Upon clicking into the File field for manual editing, this mechanism is disabled.
    /// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    public class FileNameMirror : MonoBehaviour {
        [Header("References")]
        [SerializeField] private TMP_InputField levelName;

        [Header("Options")]
        [SerializeField] private string defaultFileName;

        private TMP_InputField fileName;
        private bool edited;
        private string invalids;

        /******************
        *  UNITY METHODS  *
        ******************/

        private void Awake() {
            this.fileName = this.GetComponent<TMP_InputField>();
            this.edited = false;
            this.invalids = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
        }

        private void Start() {
            MirrorNameField();
        }

        /**********************/

        /// <summary>
        /// Main mechanism of this class. Copies the Level Name from its text field,
        /// filters it to ensure no illegal characters or duplicate files,
        /// and then assigns it to this text field.
        /// </summary>
        public void MirrorNameField() {
            if (this.edited) return;
            string name = levelName.text;

            if (string.IsNullOrEmpty(name)) {
                name = GetUnique(defaultFileName);
            }
            else {
                foreach (char c in this.invalids) {
                    name = name.Replace(c, '-');
                }

                name = GetUnique(name);
            }

            name += ".json";
            fileName.text = name;
        }

        public bool IsEdited() {
            return edited;
        }

        //NOTE: This is required over a property because of its use in the Unity Editor
        public void SetEdited(bool edited) {
            this.edited = edited;
        }

        /// <summary>
        /// Appends numbers to a file name to ensure that the file doesn't already exist.
        /// </summary>
        /// <param name="input">File name, without the .json extension</param>
        private string GetUnique(string input) {
            int num = 0;
            while (File.Exists($"{input} {num}.json")) {
                num++;
            }

            if (num != 0) {
                input += (" " + num);
            }

            return input;
        }
    }
}

