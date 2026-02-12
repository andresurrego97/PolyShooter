using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider)), ExecuteInEditMode]
public class SliderColor : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Graphic fill;

    [SerializeField] private Gradient gradient = new();

    public void OnValueChanged()
    {
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}