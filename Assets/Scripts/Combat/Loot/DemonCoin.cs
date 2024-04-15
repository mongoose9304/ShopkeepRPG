using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonCoin : MonoBehaviour
{
    public int value;
    [SerializeField] List<Material> myMaterials=new List<Material>();
    [SerializeField] MeshRenderer materialToSwitch;
    public void SetUpCoin(int Value_)
    {
        value = Value_;

        switch(Value_)
        {
            case 1:
                materialToSwitch.material = myMaterials[0];
                break;
            case 5:
                materialToSwitch.material = myMaterials[1];
                break;
            case 10:
                materialToSwitch.material = myMaterials[2];
                break;
            case 50:
                materialToSwitch.material = myMaterials[3];
                break;
            case 100:
                materialToSwitch.material = myMaterials[4];
                break;
            case 200:
                materialToSwitch.material = myMaterials[5];
                break;
            case 500:
                materialToSwitch.material = myMaterials[6];
                break;
            case 1000:
                materialToSwitch.material = myMaterials[7];
                break;
        }
    }
}
