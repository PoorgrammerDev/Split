using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Split.Util {
    /// <summary>
    /// Attached to a UI TMP object. Copies the slider's value (through a Unity Event on the Slider)
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class SliderValue : MonoBehaviour {
        [SerializeField] private Slider slider;
        private TextMeshProUGUI text;

        private void Start() {
            text = this.GetComponent<TextMeshProUGUI>();
            UpdateValue();
        }

        public void UpdateValue() {
            text.text = slider.value.ToString();
        }
    }
}


