using System.Collections;
using UnityEngine;

public class UniversalLocalizationManager : MonoBehaviour
{
    public Stack databases { get; private set; } = new Stack();
    public void AddDatabase(LocalizationDatabase database) {
        //Check if the database is already in the stack
        if (databases.Contains(database)) {
            //Push the database to the top
            return;
        };
        databases.Push(database);
    }
    public void LookupKey(string key) {
        
    }

    public string DatabaseSearch(string key) {
        //Copy the database to a new temp one
        //check if the count is less than 0
        //call database search
        return "";
    }

    public string DatabaseSearch(ref Stack databases, ref string key) {
        if(databases.Count <=0) {
            return "no key found";
        }
        return "";
    }
}
