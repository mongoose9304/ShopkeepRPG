using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class AdrielsDebugCircus : MonoBehaviour
{
    [Tooltip("Serialize a dictionary")]
    public bool SerializeDictionary = false;

    [Tooltip("GEt hascode")]
    public bool HashAString = false;


    void SerializeDictionaryFunction() {

        LocalizationDatabase LD = new LocalizationDatabase();
        //LD.database.Add("Please", "Work");
        string json = JsonUtility.ToJson(LD);
        Debug.Log(json);

        var LDOne = JsonUtility.FromJson<LocalizationDatabase>(json);
        Debug.Log(LDOne);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update() {
        if(SerializeDictionary) { SerializeDictionaryFunction(); SerializeDictionary = false; }
    }
}
