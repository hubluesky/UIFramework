using UnityEngine;

namespace VBM {
    public class GeneralPropertyConverter : PropertyConverter {
        public System.Type targetType { get; protected set; }

        public GeneralPropertyConverter(System.Type targetType) {
            this.targetType = targetType;
        }

        public override object Convert(object value) {
            return System.Convert.ChangeType(value, targetType);
        }
    }
}