using UnityEngine;

namespace VBM {
    [System.Serializable]
    public abstract class PropertyBinding {
        public string propertyName;
        public PropertyConverter converter = new PropertyConverter();
        public bool refresh { get; set; }

        public void SetProperty(object value) {
            OnPropertyChange(converter.Conver(value));
        }

        public abstract void OnPropertyChange(object value);
    }
}