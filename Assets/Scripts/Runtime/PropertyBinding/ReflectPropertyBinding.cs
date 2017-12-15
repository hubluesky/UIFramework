using UnityEngine;

namespace VBM {
    [System.Serializable]
    public class ReflectPropertyBinding : PropertyBinding {
        public Component component;
        public string componentPropertyName;
        private System.Action<object> bindFunction;

        public override void OnPropertyChange(object value) {
            bindFunction(value);
        }
    }
}