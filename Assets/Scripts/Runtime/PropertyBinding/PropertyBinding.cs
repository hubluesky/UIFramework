using UnityEngine;

namespace VBM {
    public abstract class PropertyBinding : ISerializationCallbackReceiver {
        public string propertyName;
        public string converterType;
        public PropertyConverter converter { get; set; }
        public bool isClean { get; set; }

        public void SetProperty(object value) {
            OnPropertyChange(value != null && converter != null ? converter.Convert(value) : value);
        }

        public abstract void OnPropertyChange(object value);

        void ISerializationCallbackReceiver.OnBeforeSerialize() {
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize() {
            if (!string.IsNullOrEmpty(converterType)) {
                System.Type type = System.Type.GetType(converterType);
                if (type == null)
                    Debug.LogWarning("Deserialize convertype type failed! " + converterType);
                else
                    converter = System.Activator.CreateInstance(type) as PropertyConverter;
            }
        }
    }
}