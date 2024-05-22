using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox {
    public Text nameText;
    public Text dialogueText;
}

public class Dialogue : MonoBehaviour
{
    public DialogueBox dialogueBox;
    public string[] dialogueLines;
    private int currentLine;

    public GameObject dialoguePanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
