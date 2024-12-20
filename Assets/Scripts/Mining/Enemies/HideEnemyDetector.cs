using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideEnemyDetector : MonoBehaviour
{
    // Start is called before the first frame update
    public float timeSincePlayerHasBeenInRange;
    public HideEnemy myEnemy;
    // Update is called once per frame
    void Update()
    {
        if(timeSincePlayerHasBeenInRange<=0)
        {
            myEnemy.UnHide();
        }
        else
        {
            myEnemy.Hide();
            timeSincePlayerHasBeenInRange -= Time.deltaTime;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag=="Player")
        {
            timeSincePlayerHasBeenInRange = 0.5f;
        }
    }
}
