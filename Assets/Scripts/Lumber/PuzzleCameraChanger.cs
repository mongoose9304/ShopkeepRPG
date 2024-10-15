using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCameraChanger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<LumberPlayer>().EnterPuzzle();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<LumberPlayer>().ExitPuzzle();
        }
    }
}
