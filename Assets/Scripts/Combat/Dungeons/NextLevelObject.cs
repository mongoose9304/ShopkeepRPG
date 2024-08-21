using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NextLevelObject : MonoBehaviour
{
    [SerializeField] SinType mySin;
    List<SinType> sinsAlreadyUsed=new List<SinType>();
    [Tooltip("The time you must hold the interact button before the tunnel will teleport a player")]
    [SerializeField] float maxHoldDuration;
    float currentHoldDuration;
    [Tooltip("REFERNCE to the UI bar that fills up as held")]
    public MMProgressBar myUIBar;
    [SerializeField] Image sinImage;
    [SerializeField] TextMeshProUGUI sinText;
    [SerializeField] List<NextLevelObject> nextLevelObjects = new List<NextLevelObject>();
    private void OnTriggerStay(Collider other)
    {
        if(other.tag=="Player")
        {
            currentHoldDuration += Time.deltaTime*2;
        }
    }
    private void Start()
    {
        RandomizeAllNextLevelObjects();
    }
    private void Update()
    {
        if (currentHoldDuration >= maxHoldDuration)
        {
            Use();
            currentHoldDuration = 0;
        }
        if (currentHoldDuration > 0)
            currentHoldDuration -= Time.deltaTime;
        if (currentHoldDuration < 0)
            currentHoldDuration = 0;

        AdjustBar();
    }

    private void Use()
    {
        DungeonManager.instance.NextLevel(mySin);
    }

    public void ChangeSin(SinType sinType_)
    {
        mySin = sinType_;
        sinText.text = mySin.ToString();
        sinImage.sprite = DungeonManager.instance.SinSprites[(int)mySin];
    }
    public void RandomizeAllNextLevelObjects()
    {
        sinsAlreadyUsed.Clear();
        sinsAlreadyUsed.Add(DungeonManager.instance.currentSin);
        SinType x;
        for(int i=0;i<nextLevelObjects.Count;i++)
        {
            x = (SinType)Random.Range(0,9);
            while (sinsAlreadyUsed.Contains(x))
            {
                x = (SinType)Random.Range(0, 9);
            }
            sinsAlreadyUsed.Add(x);
            nextLevelObjects[i].ChangeSin(x);
        }
    }

    /// <summary>
    /// Adjusts the UI bar based on how long you hold down for
    /// </summary>
    private void AdjustBar()
    {
        myUIBar.UpdateBar01(currentHoldDuration / maxHoldDuration);
    }
}
