using System;
using System.Collections.Generic;
using UnityEngine;

public class StringDictionary {
}

public class LocalizationDatabase : ScriptableObject
{
    [SerializeField]
    public StringDictionary database = new StringDictionary();

}
