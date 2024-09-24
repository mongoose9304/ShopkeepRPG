using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// The portals that connect levels, these will be set up by the dungeon manager
/// </summary>
public class NextLevelObject : MonoBehaviour
{
    [SerializeField] SinType mySin;
    List<SinType> sinsAlreadyUsed=new List<SinType>();
    [Tooltip("The time you must hold the interact button before the tunnel will teleport a player")]
    [SerializeField] float maxHoldDuration;
    float currentHoldDuration;
    [Header("References")]
    [Tooltip("REFERNCE to the UI bar that fills up as held")]
    public MMProgressBar myUIBar;
    [Tooltip("REFERNCE to the UI image ")]
    [SerializeField] Image sinImage;
    [Tooltip("REFERNCE to the UI name test")]
    [SerializeField] TextMeshProUGUI sinText;
    [Tooltip("REFERNCE to the other portals that are beside me")]
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
    /// <summary>
    /// Set up the portal using the provided Sin
    /// </summary>
    public void ChangeSin(SinType sinType_)
    {
        mySin = sinType_;
        sinText.text = mySin.ToString();
        sinImage.sprite = DungeonManager.instance.SinSprites[(int)mySin];
    }
    /// <summary>
    /// Randomize the sin selection and try to exlude ones used before
    /// </summary>
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
