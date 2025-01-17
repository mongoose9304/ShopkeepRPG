using UnityEngine;

using UnityEngine.UI;

public class CastingPowerBar : MonoBehaviour
{
    private Slider healthBar;
    private GameObject playerRef;
    private FishingPlayer castRef;
    private float totalTime;

        // Start is called before the first frame update
        void Start()
    {
        playerRef = null;
        healthBar = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRef == null)
        {
            playerRef = GameObject.Find("FishingPlayer");
            if (playerRef != null)
            {
                castRef = playerRef.GetComponent<FishingPlayer>();
            }
            return;
        }

        healthBar.value = castRef.castPower;
    }
}
