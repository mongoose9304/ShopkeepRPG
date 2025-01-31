using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;



public class StorageUIScript : MonoBehaviour
{
    public CanvasGroup canvas;
    public GameObject uiFish;
    public Texture fishTexture;
    private List<Fish> allFish = new List<Fish>();
    public int maxFish = 10;


    // Start is called before the first frame update
    void Start()
    {

        Deactivate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool AddFish(Fish _fish)
    {
        if (allFish.Count >= maxFish)
        {
            return false;
        }

        allFish.Add(_fish);
        return true;
    }

    public void Activate()
    {
        // Make canvas visible
        canvas.alpha = 1.0f;
        canvas.blocksRaycasts = true;

        allFish.Add(new Fish(FishType.Pike, 3.4f));

        // Add all current fish to the canvas
        for (int i = 0; i < allFish.Count; ++i)
        {
            TextMeshPro text = canvas.AddComponent<TextMeshPro>();
            text.transform.position = new Vector3(i * 50.0f, -75.0f, 0.0f);
            text.text = "Hello World!";
        }
    }

    public void Deactivate()
    {
        canvas.alpha = 0.0f;
        canvas.blocksRaycasts = false;
    }
}
