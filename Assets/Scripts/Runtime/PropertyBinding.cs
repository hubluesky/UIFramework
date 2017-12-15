using UnityEngine;

namespace VBM {
    [System.Serializable]
    public class PropertyBinding {
        public string modelPropertyName;
        public Component component;
        public string propertyName;
        public PropertyConverter converter = new PropertyConverter();
        private System.Action<object> bindFunction;
        public bool refresh { get; set; }

        public void SetProperty(object value) {
            bindFunction(converter.Conver(value));
        }
    }
}