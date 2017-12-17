using System.Collections.Generic;
using UnityEngine;

namespace VBM {
    public class ViewModelBinding : MonoBehaviour {
        [SerializeField]
        private ViewModelBinding parentBinding;
        [SerializeField]
        private string modelUniqueId;
        [SerializeField]
        private List<PropertyBinding> propertyBindingList = new List<PropertyBinding>();

        public Model model { get; protected set; }

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
            foreach (PropertyBinding binding in propertyBindingList) {
                binding.refresh = true;
            }
        }

        void OnEnable() {
            foreach (PropertyBinding binding in propertyBindingList) {
                if (!binding.refresh) continue;
                binding.SetProperty(model.GetProperty(binding.propertyName));
            }
        }

        private void PropertyChanged(string propertyName, object value) {
            foreach (PropertyBinding binding in propertyBindingList) {
                if (binding.propertyName == propertyName) {
                    if (isActiveAndEnabled)
                        binding.SetProperty(value);
                    else
                        binding.refresh = true;
                }
            }
        }

        void OnDestroy() {
            model.propertyChanged -= PropertyChanged;
        }
    }
}