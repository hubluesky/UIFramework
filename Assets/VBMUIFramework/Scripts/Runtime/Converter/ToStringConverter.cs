using UnityEngine;

namespace VBM {
    [System.Serializable]
    public class ToStringConverter : PropertyConverter {
        public override object Convert(object value) {
            return value.ToString();
        }
    }
}