using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Controls the UI slider manually to allow 2 players to use the UI at once
/// </summary>
public class HaggleSlider : MonoBehaviour
{
    [SerializeField] private Slider mySlider;
    private float sliderChange;
    private float sliderRange;
    public float SliderMovement;
    public float sliderInput;
    private void Awake()
    {
        sliderRange = mySlider.maxValue - mySlider.minValue;
    }
    private void Update()
    {
        

            sliderChange = sliderInput * sliderRange / SliderMovement;

            float sliderValue = mySlider.value;
            float tempValue = sliderValue + sliderChange * Time.deltaTime;
            Mathf.Clamp(tempValue, mySlider.minValue, mySlider.maxValue);
            mySlider.value = tempValue;
        

    }
}
