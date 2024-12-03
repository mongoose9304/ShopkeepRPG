using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderGamepadController : MonoBehaviour
{
    public bool isHaggleSlider;
    public bool isPlayer2;
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
            if (!isHaggleSlider)
            {
                sliderChange = ShopManager.instance.GetSliderInput() * sliderRange / SliderMovement;
            }
            else
            {
                if(!isPlayer2)
                {
                    sliderChange = ShopManager.instance.GetPlayerSliderInputDirectly(0) * sliderRange / SliderMovement;
                }
                else
                {
                    sliderChange = ShopManager.instance.GetPlayerSliderInputDirectly(1) * sliderRange / SliderMovement;
                }
            }
            float sliderValue = mySlider.value;
            float tempValue = sliderValue + sliderChange*Time.deltaTime;
            Mathf.Clamp(tempValue, mySlider.minValue, mySlider.maxValue);
            mySlider.value = tempValue;
        }
        
    }
}
