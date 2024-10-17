using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutGemRock : MineableObject
{
    [SerializeField] bool randomGem;
    [SerializeField] GameObject[] possibleGems;
    [SerializeField] GameObject myGem;
    [SerializeField] UnityEvent mineevent;
    void SpawnGem()
    {
        if (randomGem)
        {
            myGem = possibleGems[Random.Range(0, possibleGems.Length)];
        }
        GameObject temp = GameObject.Instantiate(myGem, transform.position, transform.rotation);
        temp.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0, 0.25f), 4, Random.Range(0, 0.25f)), ForceMode.VelocityChange);
    }

    public override void MineInteraction()
    {
        SpawnGem();
        mineevent.Invoke();
        transform.parent.gameObject.SetActive(false);
    }
}
