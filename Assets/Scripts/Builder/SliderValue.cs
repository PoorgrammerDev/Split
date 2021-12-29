using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class SliderValue : MonoBehaviour
{
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
