using UnityEngine;
using UnityEngine.UI;

public class PlayerUi : MonoBehaviour
{
    [SerializeField] private NetPlayerProperties properties;

    [Space]
    [SerializeField] private Slider sliderLife;

    private void Awake()
    {
        properties.OnLifeChange += UpdateLife;
    }

    private void OnDestroy()
    {
        properties.OnLifeChange -= UpdateLife;
    }

    private void Start()
    {
        UpdateLife();
    }

    private void UpdateLife()
    {
        sliderLife.value = properties.NetLife;
    }
}