using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Swaps sprite of a banner based on the current SIn
/// </summary>
public class ArenaBanner : MonoBehaviour
{
    [SerializeField] SpriteRenderer swapableSprite;

    public void SwapSprite()
    {
      swapableSprite.sprite=DungeonManager.instance.SinSprites[(int)DungeonManager.instance.currentSin];
    }
    private void OnEnable()
    {
        SwapSprite();
    }
}
