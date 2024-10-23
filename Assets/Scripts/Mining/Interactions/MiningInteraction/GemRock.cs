using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GemRock : MineableObject
{
    [SerializeField] bool randomGem;
    [SerializeField] GameObject[] possibleGems;
    [SerializeField] GameObject myGem;


    void SpawnGem()
    {
        if(randomGem)
        {
            myGem = possibleGems[Random.Range(0,possibleGems.Length)];
        }
       GameObject temp = GameObject.Instantiate(myGem, transform.position, transform.rotation);
       temp.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0, 0.25f), 4, Random.Range(0, 0.25f)), ForceMode.VelocityChange);
        temp.gameObject.SetActive(true);
    }
   
    public override void MineInteraction()
    {
        SpawnGem();
        transform.parent.gameObject.SetActive(false);
    }
}
