using UnityEngine;

namespace VBM {
    [System.Serializable]
    public class ListPropertyBinding : PropertyBinding {
        public Transform componentList;
        public ViewModelBinding componentElement;
        protected ListModel listModel;

        public ListPropertyBinding() {
            ElementCleared();
        }

        public override void OnPropertyChange(object value) {
            if (listModel != null) {
                ElementCleared();
                UnbindList(listModel);
            } else {
                listModel = value as ListModel;
                if (listModel != null)
                    BindList(listModel);
            }
        }

        protected virtual void BindList(object value) {
            listModel.elementAdded += ElementAdded;
            listModel.elementInserted += ElementInserted;
            listModel.elementRemoved += ElementRemoved;
            listModel.elementRemovRanged += ElementRemovRanged;
            listModel.elementCleared += ElementCleared;
        }

        protected virtual void UnbindList(object value) {
            listModel.elementAdded -= ElementAdded;
            listModel.elementInserted -= ElementInserted;
            listModel.elementRemoved -= ElementRemoved;
            listModel.elementRemovRanged += ElementRemovRanged;
            listModel.elementCleared -= ElementCleared;
        }

        protected Transform CreateChild(Model model) {
            Transform child;
            if (listModel.Count <= componentList.childCount) {
                child = componentList.GetChild(listModel.Count - 1);
                child.gameObject.SetActive(true);
            } else {
                child = Object.Instantiate(componentElement.transform, Vector3.zero, Quaternion.identity, componentList);
            }
            ViewModelBinding binding = child.GetComponent<ViewModelBinding>();
            binding.SetModel(model);
            return child;
        }

        protected void ElementAdded(Model model) {
            CreateChild(model);
        }

        protected void ElementInserted(int index, Model model) {
            Transform child = CreateChild(model);
            child.SetSiblingIndex(index);
        }

        protected void ElementRemoved(int index) {
            Transform child = componentList.GetChild(index); ;
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
                transform.gameObject.SetActive(false);
            }
        }
    }
}