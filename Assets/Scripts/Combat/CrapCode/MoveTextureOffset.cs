using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTextureOffset : MonoBehaviour
{
   [SerializeField] Renderer rend;
    public float scrollSpeed;
    public float offSet;
    void Start()
    {
        rend = GetComponent<Renderer>();
    }
    void Update()
    {
        offSet += Time.deltaTime * scrollSpeed;
        rend.material.mainTextureOffset = new Vector2(offSet, 0);
    }
}
