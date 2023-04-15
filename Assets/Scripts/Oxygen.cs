using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Oxygen : MonoBehaviour
{
    public Slider Slider;
    public Gradient Gradient;
    public Image Fill;
    public void SetMaxO2(float health){
        Slider.maxValue = health;
        Slider.value = health;

        Fill.color = Gradient.Evaluate(1f); 
    }
    public void SetHealth(float health){
        Slider.value = health;
        Fill.color = Gradient.Evaluate(Slider.normalizedValue);
    }


}
