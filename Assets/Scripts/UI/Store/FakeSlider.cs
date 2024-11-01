
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
public class FakeSlider : Slider
{
   [SerializeField] public GameObject objectToSelectOnDown;
    public override void OnMove(AxisEventData eventData)
    {

        if(eventData.moveDir==MoveDirection.Left)
        {
            return;
        }
        if (eventData.moveDir == MoveDirection.Right)
        {
            return;
        }
        if (eventData.moveDir == MoveDirection.Down)
        {
            if(navigation.selectOnDown.gameObject)
            EventSystem.current.SetSelectedGameObject(navigation.selectOnDown.gameObject);
            return;
        }
    }
   
}
