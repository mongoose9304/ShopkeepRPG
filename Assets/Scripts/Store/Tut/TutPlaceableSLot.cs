using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutPlaceableSLot : MoveableObjectSlot
{
    [SerializeField] bool hasBeenUsed;
    public override void PlaceObject(MoveableObject object_)
    {
        if (object_)
        {
            placedObject = object_;
            SpawnPlacedObject();
            if(!hasBeenUsed)
            {
                hasBeenUsed = true;
                ShopTutorialManager.instance.SetTutorialState(11);
            }
        }
    }
}
