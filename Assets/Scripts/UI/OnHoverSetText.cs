using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class OnHoverSetText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI textRef;
    [SerializeField] string myText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        textRef.text = myText;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        textRef.text = "";
        
    }
}
