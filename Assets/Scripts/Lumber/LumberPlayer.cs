using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumberPlayer : MonoBehaviour
{

    //references and inputs
    [SerializeField] GameObject interactableObjectTarget;
    [SerializeField] GameObject interactableObjectLockOnObject;
    public List<GameObject> myInteractableObjects = new List<GameObject>();
    Vector3 moveInput;
    Vector3 newInput;
    Vector3 dashStartPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void GetInput()
    {

        moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (Input.GetButtonDown("Fire3"))
        {
            
        }
        if (Input.GetButtonDown("Fire2"))
        {
           
        }
        if (Input.GetButton("Fire4"))
        {
            
        }
        if (Input.GetButtonDown("Fire1"))
        {
           
        }

    }
}
