using UnityEngine;

namespace VBM {
    [System.Serializable]
    public class PropertyConverter {
        public virtual object Convert(object value) { return value; }
    }
}