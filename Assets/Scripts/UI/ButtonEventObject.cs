using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class ButtonEventObject : MonoBehaviour, ISelectHandler, IDeselectHandler
{
	public UnityEvent selectEvent;
	public UnityEvent deselectEvent;
	public void OnSelect(BaseEventData eventData)
	{
		Debug.Log(this.gameObject.name + " was selected");
		selectEvent.Invoke();
	}
	public void OnDeselect(BaseEventData data)
	{
		Debug.Log("Deselected");
		deselectEvent.Invoke();
	}

}
