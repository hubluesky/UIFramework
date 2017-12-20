using UnityEngine;

namespace VBM {
    [System.Serializable]
    public class PropertyBinding {
        public string propertyName;
        public PropertyConverter converter;
        public bool refresh { get; set; }

        public void SetProperty(object value) {
            OnPropertyChange(converter != null ? converter.Convert(value) : value);
        }

        public virtual void OnPropertyChange(object value) { }
    }
}