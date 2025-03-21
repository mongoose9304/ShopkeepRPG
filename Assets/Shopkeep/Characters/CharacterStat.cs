using UnityEngine.Events;

//Note:
//Stat is in all lowercase because 'Stats' and 'Statistics' are already taken

namespace Shopkeeper {
    /// <summary>
    /// Wraps floats for use in UnityEvents
    /// </summary>
    public class FloatWrapper {
        public FloatWrapper(float v) { value = v; }
        public float value;
    };

    public struct Stat {
        public float value {
            private get;
            set;
        }

        public UnityEvent<FloatWrapper> modifier;
        public float GetValue() {
            FloatWrapper temp = new FloatWrapper(value);
            modifier.Invoke(temp);
            return temp.value;
        }

        //To get the original value without any modifications
        public float GetValueRaw() { return value; }

        public Stat(float v) {
            value = v;
            modifier = new UnityEvent<FloatWrapper>();
        }
    }
}