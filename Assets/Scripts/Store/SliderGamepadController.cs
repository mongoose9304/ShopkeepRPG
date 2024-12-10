using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderGamepadController : MonoBehaviour
{
    [Tooltip("REFRERENCE to the slider we are changing")]
    [SerializeField] private Slider mySlider;
    [Tooltip("REFRERENCE to the button we are checking to see if it is currently slected to control the slider")]
    [SerializeField]  private GameObject buttonObject;
    private float sliderChange;
    private float sliderRange;
    [Tooltip("How fast the slider moves")]
    public float SliderMovement;
    private void Awake()
    {
        sliderRange = mySlider.maxValue - mySlider.minValue;
    }
    private void Update()
    {
        if (buttonObject == EventSystem.current.currentSelectedGameObject)
        {

                sliderChange = ShopManager.instance.GetSliderInput() * sliderRange / SliderMovement;

            float sliderValue = mySlider.value;
            float tempValue = sliderValue + sliderChange*Time.deltaTime;
            Mathf.Clamp(tempValue, mySlider.minValue, mySlider.maxValue);
            mySlider.value = tempValue;
        }
        
    }
}
