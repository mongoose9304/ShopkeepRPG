using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariousTextSpawner : MonoBehaviour
{
    [SerializeField] MMF_Player textSpawner;
    [SerializeField] List<string> texts=new List<string>();
    public MMF_FloatingText floatingText;
    public float maxTimeBetweenSpawns;
    float currentTimeBetweenSpawns;
    // Start is called before the first frame update
    void Start()
    {
        floatingText = textSpawner.GetFeedbackOfType<MMF_FloatingText>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTimeBetweenSpawns -= Time.deltaTime;
        if(currentTimeBetweenSpawns<=0)
        {
            currentTimeBetweenSpawns = maxTimeBetweenSpawns;
            floatingText.Value = texts[Random.Range(0, texts.Count)];
            textSpawner.PlayFeedbacks();
        }
    }
}
