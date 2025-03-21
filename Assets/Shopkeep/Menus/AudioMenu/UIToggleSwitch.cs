using PixelCrushers.DialogueSystem.Articy.Articy_4_0;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIToggle : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// https://www.youtube.com/watch?v=E9AWlbPGi_4
    /// </summary>
    [Header ("Sclider Vlaue")]
    [SerializeField, Range(0f, 1f)] private float sliderValue;
    // Start is called before the first frame update

    public bool CurrentValue{ get; private set; }
    public Slider slider;

    [Header ("Animation Speed")]
    [SerializeField, Range(0f,1f)] private float animVal = 0.5f;

    [Header("Event")]
    [SerializeField] private UnityEvent onToggleOn;
    [SerializeField] private UnityEvent onToggleOff;

    private Coroutine animationCoroutine;
    private


    void Start()
    {
        slider.value = sliderValue;
        CurrentValue = sliderValue > 0.5f;

        if (CurrentValue)
        {
            if (onToggleOn != null)
            {
                onToggleOn.Invoke();
            }
        }
        else {
            if (onToggleOff != null) { 
                onToggleOff.Invoke();
            }
        }

    }


    public void OnPointerClick(PointerEventData eventData) {
        CurrentValue = !CurrentValue;

        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }

        float targetValue;
        if (CurrentValue)
        {
            targetValue = 1f;
        }
        else
        {
            targetValue = 0f;
        }
        animationCoroutine = StartCoroutine(AnimateSlider(targetValue));
    }

    private IEnumerator AnimateSlider(float targetValue) {
        float startValue = slider.value;
        float duration = 1f - animVal; // Lower animSpeed = faster animation
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float normalizedTime = time / duration;
            slider.value = Mathf.Lerp(startValue, targetValue, normalizedTime);
            yield return null;
        }

        // Ensure the final value is set precisely
        slider.value = targetValue;
        sliderValue = targetValue;
    }

    public void SetToggle(bool value)
    {
        if (CurrentValue != value)
        {
            CurrentValue = value;

            // Stop any existing animation
            if (animationCoroutine != null)
                StopCoroutine(animationCoroutine);

            // Start animation to target value
            float targetValue = CurrentValue ? 1f : 0f;
            animationCoroutine = StartCoroutine(AnimateSlider(targetValue));

            // Invoke the appropriate event
            if (CurrentValue)
            {
                if (onToggleOn != null)
                {
                    onToggleOn.Invoke();
                }
            }
            else
            {
                if (onToggleOff != null)
                {
                    onToggleOff.Invoke();
                }
            }
        }
    }
}
