using UnityEngine;

namespace VBM {
    public abstract class PropertyConverter : ScriptableObject {
        public abstract object Convert(object value);
    }
}