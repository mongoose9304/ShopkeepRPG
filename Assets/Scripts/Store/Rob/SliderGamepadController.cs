using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderGamepadController : MonoBehaviour
{
    [SerializeField] private Slider mySlider;
    [SerializeField]  private GameObject buttonObject;
    private float sliderChange;
    private float sliderRange;
    public float SliderMovement;
    public string SliderInputString = "SliderHorizontal";
    private void Awake()
    {
        sliderRange = mySlider.maxValue - mySlider.minValue;
    }
    private void Update()
    {
        if (buttonObject == EventSystem.current.currentSelectedGameObject)
        {
            sliderChange = Input.GetAxis(SliderInputString) * sliderRange / SliderMovement;
            float sliderValue = mySlider.value;
            float tempValue = sliderValue + sliderChange;
            Mathf.Clamp(tempValue, mySlider.minValue, mySlider.maxValue);
            mySlider.value = tempValue;
        }
        
    }
}
