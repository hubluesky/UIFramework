using System.Collections.Generic;
using UnityEngine;

namespace VBM {
    public class ViewModelBinding : MonoBehaviour {
        [SerializeField]
        private string modelUniqueId;
        [SerializeField]
        private List<PropertyBinding> propertyBindingList = new List<PropertyBinding>();

        public Model model { get; protected set; }

        void Awake() {
            model = ModelManager.Instance.GetModel(modelUniqueId);
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
                if(!binding.refresh) continue;
                binding.SetProperty(model.GetProperty(binding.modelPropertyName));
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