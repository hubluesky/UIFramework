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
        private List<ViewModelBinding> childModelBinding = new List<ViewModelBinding>();

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
                    parentBinding.AddChildViewModelBinding(this);
                    if (parentBinding.model == null) {
                        // Debug.LogWarningFormat("Get model {1} failed! {1} parent binding model is null.", modelUniqueId, parentBinding.name);
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

            if (model == null) {
                this.model = null;
                return;
            }

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

        public ViewModelBinding GetRoot() {
            if (parentBinding == null)
                return this;
            return parentBinding.GetRoot();
        }

        public void ForceRefreshBindingPropertys() {
            if (bindingList == null || model == null) return;
            foreach (PropertyBinding binding in bindingList) {
                binding.SetProperty(model.GetProperty(binding.propertyName));
                binding.refresh = false;
            }
        }

        void RefreshBindingPropertys() {
            if (bindingList == null || model == null) return;
            foreach (PropertyBinding binding in bindingList) {
                if (!binding.refresh) continue;
                binding.SetProperty(model.GetProperty(binding.propertyName));
                binding.refresh = false;
            }

            foreach (ViewModelBinding binding in childModelBinding) {
                binding.SetModel(model.GetProperty(binding.modelId) as IModel);
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

            foreach (ViewModelBinding binding in childModelBinding) {
                if (binding.modelId == propertyName) {
                    binding.SetModel(value as IModel);
                }
            }
        }

        void AddChildViewModelBinding(ViewModelBinding binding) {
            childModelBinding.Add(binding);
        }

        void RemoveChildViewmodelBinding(ViewModelBinding binding) {
            childModelBinding.Remove(binding);
        }

        void OnDestroy() {
            foreach (PropertyBinding binding in bindingList)
                binding.Finalized();
            if (model != null)
                model.propertyChanged -= PropertyChanged;
            if (parentBinding != null)
                parentBinding.RemoveChildViewmodelBinding(this);
            bindingList = null;
        }
    }
}