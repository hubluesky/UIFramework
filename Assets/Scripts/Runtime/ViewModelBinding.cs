using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace VBM {
    public class ViewModelBinding : MonoBehaviour {
        [System.Serializable]
        class PropertiesBinding {
            public GraphicColorPropertyBinding[] graphicColorArray;
            public ImagePropertyBinding[] imageSpriteArray;
            public TextPropertyBinding[] textLabelArray;
            public ReflectPropertyBinding[] reflectPropertyArray;

            public List<PropertyBinding> InitBindingList() {
                List<PropertyBinding> list = new List<PropertyBinding>();
                foreach (PropertyBinding binding in graphicColorArray)
                    list.Add(binding);
                foreach (PropertyBinding binding in imageSpriteArray)
                    list.Add(binding);
                foreach (PropertyBinding binding in textLabelArray)
                    list.Add(binding);
                foreach (PropertyBinding binding in reflectPropertyArray)
                    list.Add(binding);
                return list;
            }
        }

        [SerializeField]
        private ViewModelBinding parentBinding;
        [SerializeField]
        private string modelUniqueId;
        [SerializeField]
        private PropertiesBinding propertiesBinding;
        private List<PropertyBinding> bindingList;
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
            if (propertiesBinding != null)
                bindingList = propertiesBinding.InitBindingList();
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
    }
}