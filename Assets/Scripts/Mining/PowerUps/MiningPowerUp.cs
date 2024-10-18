using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MiningPowerType
{
    Heal,
    ExtraBomb,
}
public class MiningPowerUp : MonoBehaviour
{
    public float amount;
    public MiningPowerType myType;
    public bool isThrown;
    private void OnPickUp()
    {
        if (isThrown)
        {
            transform.parent.gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<MiningPlayer>().ApplyPowerUp(amount,myType);
            OnPickUp();
        }
    }
}

