using UnityEngine.Events;

//Note:
//Stat is in all lowercase because 'Stats' and 'Statistics' are already taken
public struct stat {
    public float Value {
        private get; 
        set;
    }

    public UnityEvent<float> modifier; 
    public float GetValue() {
        float temp = Value;
        modifier.Invoke(temp);
        return temp;
    }

    //To get the original value without any modifications
    public float GetValueRaw() { return Value; }
}