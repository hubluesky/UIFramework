using UnityEngine;

namespace VBM {
    [System.Serializable]
    public class ListPropertyBinding : PropertyBinding {
        public Transform componentList;
        public ViewModelBinding[] componentElements;
        protected ListModel listModel;

        public override void OnPropertyChange(object value) {
            ElementCleared();
            if (listModel != null)
                UnbindList(listModel);

            listModel = value as ListModel;
            if (listModel != null) {
                InitChilds();
                BindList(listModel);
            }
        }

        public override void Finalized() {
            if (listModel != null) {
                UnbindList(listModel);
                listModel = null;
            }
        }

        protected virtual void BindList(object value) {
            listModel.elementAdded += ElementAdded;
            listModel.elementInserted += ElementInserted;
            listModel.elementSwaped += ElementSwaped;
            listModel.elementRemoved += ElementRemoved;
            listModel.elementRemovRanged += ElementRemovRanged;
            listModel.elementCleared += ElementCleared;
        }

        protected virtual void UnbindList(object value) {
            listModel.elementAdded -= ElementAdded;
            listModel.elementInserted -= ElementInserted;
            listModel.elementSwaped -= ElementSwaped;
            listModel.elementRemoved -= ElementRemoved;
            listModel.elementRemovRanged -= ElementRemovRanged;
            listModel.elementCleared -= ElementCleared;
        }

        protected void InitChilds() {
            for (int i = 0; i < listModel.Count; i++) {
                CreateChild(listModel[i], i);
            }
        }

        protected Transform CreateChild(IModel model, int index) {
            Transform child = CreateChildInCache(model);
            if (child != null) {
                child.SetSiblingIndex(index);
                child.gameObject.SetActive(true);
                ViewModelBinding binding = child.GetComponent<ViewModelBinding>();
                binding.SetModel(model);
            }
            return child;
        }

        private Transform CreateChildInCache(IModel model) {
            string modelUniqueId = model.GetType().Name;
            foreach (Transform child in componentList) {
                if (child.gameObject.activeSelf) continue;
                ViewModelBinding binding = child.GetComponent<ViewModelBinding>();
                if(binding == null) continue;
                if (binding.modelId == modelUniqueId && model.CheckElementModel(binding))
                    return child;
            }

            foreach (ViewModelBinding binding in componentElements) {
                if (binding.modelId == modelUniqueId && model.CheckElementModel(binding)) {
                    Transform child = Object.Instantiate(binding.transform, componentList);
                    child.localPosition = Vector3.zero;
                    child.localRotation = Quaternion.identity;
                    return child;
                }
            }

            Debug.LogWarningFormat("Create ComponentList {0} child {1} failed.", componentList, modelUniqueId);
            return null;
        }

        protected Transform GetElement(IModel model) {
            string modelUniqueId = model.GetType().Name;
            foreach (ViewModelBinding binding in componentElements) {
                if (binding.modelId == modelUniqueId && model.CheckElementModel(binding))
                    return binding.transform;
            }
            return null;
        }

        protected void ElementAdded(IModel model) {
            CreateChild(model, listModel.Count - 1);
        }

        protected void ElementInserted(int index, IModel model) {
            Transform child = CreateChild(model, listModel.Count - 1);
            child.SetSiblingIndex(index);
        }

        protected void ElementSwaped(int index1, int index2) {
            Transform child1 = componentList.GetChild(index1);
            Transform child2 = componentList.GetChild(index2);
            child2.SetSiblingIndex(index1);
            child1.SetSiblingIndex(index2);
        }

        protected void ElementRemoved(int index) {
            Transform child = componentList.GetChild(index);
            child.SetAsLastSibling();
            child.gameObject.SetActive(false);
        }

        protected void ElementRemovRanged(int index, int count) {
            for (int i = count - 1; i >= 0; i--) {
                ElementRemoved(index + i);
            }
        }

        protected void ElementCleared() {
            foreach (Transform transform in componentList) {
                ViewModelBinding binding = transform.GetComponent<ViewModelBinding>();
                if (binding == null) continue;
                transform.gameObject.SetActive(false);
            }
        }
    }
}