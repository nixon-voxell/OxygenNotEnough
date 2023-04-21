using UnityEngine;
using UnityEngine.UI;

public class OxygenUI : MonoBehaviour
{
    public Slider Slider;
    public Gradient Gradient;
    public Image Fill;

    public void SetMaxO2(float health)
    {
        Slider.maxValue = health;
        Slider.value = health;

        Fill.color = Gradient.Evaluate(1f);
    }

    public void SetOxygen(float oxygen)
    {
        Slider.value = oxygen;
        Fill.color = Gradient.Evaluate(Slider.normalizedValue);
    }

    private void Start()
    {
        UIManager.Instance.OxygenUI = this;
    }
}
