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
	bool isQuitting;
	public void OnSelect(BaseEventData eventData)
	{
		selectEvent.Invoke();
	}
	void OnMouseOver()
	{
		selectEvent.Invoke();
	}
	void OnMouseExit()
	{
		deselectEvent.Invoke();
	}
	public void OnDeselect(BaseEventData data)
	{
		deselectEvent.Invoke();
	}

    private void OnDisable()
    {
		if (!isQuitting)
			deselectEvent.Invoke();
    }
    private void OnEnable()
    {
		if (EventSystem.current == null)
			return;
		if (EventSystem.current.currentSelectedGameObject==gameObject)
        {
			selectEvent.Invoke();
		}
	}
    private void OnApplicationQuit()
	{
		isQuitting = true;
	}
}
