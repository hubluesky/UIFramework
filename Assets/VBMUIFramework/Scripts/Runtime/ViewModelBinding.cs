using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace VBM {
    public class ViewModelBinding : MonoBehaviour {
        [System.Serializable]
        class PropertiesBinding {
            public GraphicColorPropertyBinding[] graphicColorArray = null;
            public ImagePropertyBinding[] imageSpriteArray = null;
            public ImageFillAmountPropertyBinding[] imageFillAmountArray = null;
            public SliderValuePropertyBinding[] sliderValueArray = null;
            public TextPropertyBinding[] textLabelArray = null;
            public InputFieldPropertyBinding[] inputFieldArray = null;
            public TogglePropertyBinding[] toggleIsOnArray = null;
            public SelectableInteractableBinding[] selectableInteractableArray = null;
            public BehaviourEnabledPropertyBinding[] behaviourEnabledArray = null;
            public GameObjectActivePropertyBinding[] gameObjectActiveArray = null;
            public AnimatorPropertyBinding[] animatorParametersArray = null;
            public ReflectPropertyBinding[] reflectPropertyArray = null;
            public ListPropertyBinding[] listPropertyArray = null;

            public List<PropertyBinding> InitBindingList() {
                List<PropertyBinding> list = new List<PropertyBinding>();
                foreach (PropertyBinding binding in listPropertyArray)
                    list.Add(binding);
                foreach (PropertyBinding binding in graphicColorArray)
                    list.Add(binding);
                foreach (PropertyBinding binding in imageSpriteArray)
                    list.Add(binding);
                foreach (PropertyBinding binding in imageFillAmountArray)
                    list.Add(binding);
                foreach (PropertyBinding binding in sliderValueArray)
                    list.Add(binding);
                foreach (PropertyBinding binding in textLabelArray)
                    list.Add(binding);
                foreach (PropertyBinding binding in inputFieldArray)
                    list.Add(binding);
                foreach (PropertyBinding binding in toggleIsOnArray)
                    list.Add(binding);
                foreach (PropertyBinding binding in selectableInteractableArray)
                    list.Add(binding);
                foreach (PropertyBinding binding in behaviourEnabledArray)
                    list.Add(binding);
                foreach (PropertyBinding binding in gameObjectActiveArray)
                    list.Add(binding);
                foreach (PropertyBinding binding in animatorParametersArray)
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
        public IModel model { get; protected set; }
        public View view { get; set; }
        public string modelId { get { return modelUniqueId; } }
        public event System.Action<IModel> BindedModelEvent;

        void Awake() {
            if (propertiesBinding != null)
                bindingList = propertiesBinding.InitBindingList();
            foreach (PropertyBinding binding in bindingList)
                binding.Initialized();
            InitModel();
        }

        void InitModel() {
            if (model == null) {
                if (parentBinding == null) {
                    model = ModelManager.Instance.GetModel(modelUniqueId);
                } else {
                    if (parentBinding.model == null) {
                        Debug.LogWarningFormat("Get model {1} failed! {1} parent binding model is null.", modelUniqueId, parentBinding.name);
                        return;
                    }
                    model = parentBinding.model.GetProperty<IModel>(modelUniqueId);
                }

                if (model != null)
                    SetModel(model);
            }
        }

        void Start() {
            if (model == null) {
                Debug.LogWarningFormat("Get model {0} falied! {1} view bind model failed", modelUniqueId, name);
            }
        }

        public void SetModel(IModel model) {
            if (this.model != null)
                this.model.propertyChanged -= PropertyChanged;
            model.propertyChanged += PropertyChanged;
            this.model = model;

            foreach (PropertyBinding binding in bindingList) {
                binding.SetModel(model);
                binding.refresh = true;
            }

            if (isActiveAndEnabled)
                RefreshBindingPropertys();

            if (BindedModelEvent != null)
                BindedModelEvent(model);
        }

        public void HideView() {
            view.Hide();
        }

        void OnEnable() {
            RefreshBindingPropertys();
        }

        void RefreshBindingPropertys() {
            if (bindingList == null || model == null) return;
            foreach (PropertyBinding binding in bindingList) {
                if (!binding.refresh) continue;
                binding.SetProperty(model.GetProperty(binding.propertyName));
                binding.refresh = false;
            }
        }

        private void PropertyChanged(string propertyName, object value) {
            foreach (PropertyBinding binding in bindingList) {
                if (binding.propertyName == propertyName) {
                    if (isActiveAndEnabled) {
                        binding.SetProperty(value);
                        binding.refresh = false;
                    } else {
                        binding.refresh = true;
                    }
                }
            }
        }

        void OnDestroy() {
            foreach (PropertyBinding binding in bindingList)
                binding.Finalized();
            if (model != null)
                model.propertyChanged -= PropertyChanged;
            bindingList = null;
        }
    }
}