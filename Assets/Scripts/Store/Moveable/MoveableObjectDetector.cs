using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObjectDetector : MonoBehaviour
{
    StorePlayer Player;
    private void Start()
    {
        Player = GetComponentInParent<StorePlayer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Moveable")
        {
            if (other.TryGetComponent<MoveableObjectSlot>(out MoveableObjectSlot slot_))
            {
                if (!Player.myMoveableObjectSlots.Contains(slot_))
                {
                Player.myMoveableObjectSlots.Add(slot_);
                }

            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Moveable")
        {
            if (other.TryGetComponent<MoveableObjectSlot>(out MoveableObjectSlot slot_))
            {
                if (Player.myMoveableObjectSlots.Contains(slot_))
                {
                Player.myMoveableObjectSlots.Remove(slot_);
                }

            }

        }
    }
}
