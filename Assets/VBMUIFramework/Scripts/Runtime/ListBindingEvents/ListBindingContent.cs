using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VBM {
    public class ListBindingContent : MonoBehaviour {
        public ListModel listModel { get; protected set; }
        public ViewModelBinding[] componentElements;
        protected List<ViewModelBinding> usedChildList = new List<ViewModelBinding>();
        protected List<ViewModelBinding> unusedChildList = new List<ViewModelBinding>();

        void Start() { }

        public virtual void Initialize() {
            foreach (ViewModelBinding binding in componentElements) {
                if (binding.transform.parent == transform)
                    BacktoCache(binding);
            }
        }

        public virtual void Finalized() {
            if (listModel != null) {
                UnbindList(listModel);
                listModel = null;
            }
        }

        public virtual void SetListModel(ListModel newListModel) {
            ElementCleared();
            if (listModel != null)
                UnbindList(listModel);

            listModel = newListModel;
            if (listModel != null) {
                InitChilds();
                BindList(listModel);
            }
        }

        protected virtual void BindList(object value) {
            listModel.elementAdded += ElementAdded;
            listModel.elementRemoved += ElementRemoved;
            listModel.elementCleared += ElementCleared;
        }

        protected virtual void UnbindList(object value) {
            listModel.elementAdded -= ElementAdded;
            listModel.elementRemoved -= ElementRemoved;
            listModel.elementCleared -= ElementCleared;
        }

        protected virtual void InitChilds() {
            foreach (IModel model in listModel)
                ElementAdded(model);
        }

        protected virtual Transform CreateChild(IModel model) {
            ViewModelBinding binding = CreateChildInCache(model);
            if (binding != null) {
                binding.transform.SetAsLastSibling();
                binding.gameObject.SetActive(true);
                binding.SetModel(model);
                usedChildList.Add(binding);
            }
            return binding.transform;
        }

        private void ElementAdded(IModel model) {
            OnElementAdded(model);
        }

        private void ElementRemoved(IModel model) {
            OnElementRemoved(model);
        }

        private void ElementCleared() {
            OnElementCleared();
        }

        protected virtual void OnElementAdded(IModel model) {
            CreateChild(model);
        }

        protected virtual void OnElementRemoved(IModel model) {
            for (int i = 0; i < usedChildList.Count; i++) {
                if (usedChildList[i].model == model) {
                    BacktoCache(usedChildList[i]);
                    usedChildList.RemoveAt(i);
                }
            }
        }

        protected virtual void OnElementCleared() {
            foreach (ViewModelBinding binding in usedChildList)
                BacktoCache(binding);
            usedChildList.Clear();
        }

        protected ViewModelBinding CreateChildInCache(IModel model) {
            string modelUniqueId = model.GetType().Name;
            for (int i = 0; i < unusedChildList.Count; i++) {
                ViewModelBinding binding = unusedChildList[i];
                if (binding.modelId == modelUniqueId && model.CheckElementModel(binding)) {
                    unusedChildList.RemoveAt(i);
                    return binding;
                }
            }

            foreach (ViewModelBinding binding in componentElements) {
                if (binding.modelId == modelUniqueId && model.CheckElementModel(binding)) {
                    Transform child = Object.Instantiate(binding.transform, transform);
                    child.localPosition = Vector3.zero;
                    child.localRotation = Quaternion.identity;
                    return child.GetComponent<ViewModelBinding>();
                }
            }

            Debug.LogWarningFormat("Create ComponentList {0} child {1} failed.", transform, modelUniqueId);
            return null;
        }

        protected void BacktoCache(ViewModelBinding binding) {
            binding.gameObject.SetActive(false);
            unusedChildList.Add(binding);
        }
    }
}