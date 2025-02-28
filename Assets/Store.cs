using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;




public class Store : MonoBehaviour
{
    [Serializable]
    public class Cost
    {
        public ItemData item;
        public int amount;
    };

    [Serializable]
    public class StoreItem
    {
        public ItemData item;
        public List<Cost> costs;
    };


    public List<StoreItem> recipes = new List<StoreItem>();
    public PlayerInventory playerInventory;
    public Canvas UICanvas;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < recipes.Count; ++i)
        {
            GameObject textGameObject = new GameObject("Recipe_" + i);
            textGameObject.transform.parent = transform;

            TextMeshProUGUI text = textGameObject.AddComponent<TextMeshProUGUI>();
            string costsStr = "";
            for (int j = 0; j < recipes[i].costs.Count; ++j)
            {
                costsStr += recipes[i].costs[j].amount + " " + recipes[i].costs[j].item.name;
                if (j != recipes[i].costs.Count - 1)
                {
                    costsStr += ", ";
                }

            }
            text.text = recipes[i].item.name + ": " + costsStr;

            text.fontSize = 24;
            text.alignment = TextAlignmentOptions.Left;

            // Configure RectTransform
            RectTransform rectTransform = textGameObject.GetComponent<RectTransform>();
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 100 + 50 * i, 50);
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 200, 100);
            rectTransform.sizeDelta = new Vector2(500, 50);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Craft(int recipeIndex)
    {
        // Loop through all costs and make sure you have enough resources
        for (int i = 0; i < recipes[recipeIndex].costs.Count; ++i)
        {
            string name = recipes[recipeIndex].costs[i].item.name;
            int cost = recipes[recipeIndex].costs[i].amount;
            if (playerInventory.GetItemAmount(name) < cost)
            {
                return;
            }
        }

        // If we made it here, we have the resources, do a second loop to subtract the used resources
        for (int i = 0; i < recipes[recipeIndex].costs.Count; ++i)
        {
            string name = recipes[recipeIndex].costs[i].item.name;
            int cost = recipes[recipeIndex].costs[i].amount;
            playerInventory.SubtractItems(name, cost);
        }

        // Then add the crafted item to your inventory
        playerInventory.AddItemToInventory(recipes[recipeIndex].item.name, 1);
    }
}
