using UnityEngine;

namespace VBM {
    [System.Serializable]
    public class GeneralPropertyConverter : PropertyConverter {
        public System.Type targetType { get; protected set; }

        public override object Convert(object value) {
            return System.Convert.ChangeType(value, targetType);
        }
    }
}