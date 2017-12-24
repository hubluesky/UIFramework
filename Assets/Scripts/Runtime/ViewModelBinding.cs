using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace VBM {
    public class ViewModelBinding : MonoBehaviour, ISerializationCallbackReceiver {
        [SerializeField]
        private ViewModelBinding parentBinding;
        [SerializeField]
        private string modelUniqueId;
        [SerializeField, HideInInspector]
        private byte[] bindingData;
        public List<PropertyBinding> bindingList { get; private set; }
        public Model model { get; protected set; }
        public View view { get; set; }

        void Awake() {
            if (parentBinding == null) {
                model = ModelManager.Instance.GetModel(modelUniqueId);
            } else {
                if (parentBinding.model == null) {
                    Debug.LogWarningFormat("Get model {1} failed! {1} parent binding model is null.", modelUniqueId, parentBinding.name);
                    return;
                }
                model = parentBinding.model.GetProperty<Model>(modelUniqueId);
            }
            if (model == null) {
                Debug.LogWarningFormat("Get model {0} falied! {1} view bind model failed", modelUniqueId, name);
                return;
            }
            model.propertyChanged += PropertyChanged;
            if (bindingList != null) {
                foreach (PropertyBinding binding in bindingList)
                    binding.refresh = true;
            }
        }

        void OnEnable() {
            if (bindingList == null) return;
            foreach (PropertyBinding binding in bindingList) {
                if (!binding.refresh) continue;
                binding.SetProperty(model.GetProperty(binding.propertyName));
            }
        }

        private void PropertyChanged(string propertyName, object value) {
            foreach (PropertyBinding binding in bindingList) {
                if (binding.propertyName == propertyName) {
                    if (isActiveAndEnabled)
                        binding.SetProperty(value);
                    else
                        binding.refresh = true;
                }
            }
        }

        void OnDestroy() {
            if (model != null)
                model.propertyChanged -= PropertyChanged;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() {
            if (bindingList == null) return;
            using(MemoryStream stream = new MemoryStream()) {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, bindingList);
                bindingData = stream.ToArray();
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize() {
            bindingList = new List<PropertyBinding>();
            if(bindingData == null) return;
            using(MemoryStream stream = new MemoryStream(bindingData)) {
                BinaryFormatter formatter = new BinaryFormatter();
                bindingList = formatter.Deserialize(stream) as List<PropertyBinding>;
            }
        }
    }
}