using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GemRock : MonoBehaviour
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
            GameObject.Instantiate(myGem, transform.position, transform.rotation);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Pickaxe")
        {
            SpawnGem();
            Destroy(this.gameObject);
        }
    }
}
