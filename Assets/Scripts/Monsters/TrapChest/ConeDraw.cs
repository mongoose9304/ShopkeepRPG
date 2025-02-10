using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ConeDraw : MonoBehaviour
{
    public Material coneMat;
    public MeshRenderer coneMesh;
    public LineRenderer coneLine1;
    public LineRenderer coneLine2;

    Color coneColor = new Color(1.0f, 1.0f, 1.0f);
    public float viewDistance = 10.0f;
    public float viewAngle = 90.0f;
    public int amountOfPointsOnArch = 8;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  
}
