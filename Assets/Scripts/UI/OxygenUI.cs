using UnityEngine;
using UnityEngine.UI;

public class OxygenUI : MonoBehaviour
{
    public Slider Slider;
    public Gradient Gradient;
    public Image Fill;

    public void SetMaxOxygen(float oxygen)
    {
        Slider.maxValue = oxygen;
        this.SetOxygen(oxygen);
    }

    public void SetOxygen(float oxygen)
    {
        Slider.value = oxygen;
        Fill.color = Gradient.Evaluate(Slider.normalizedValue);
    }
}
