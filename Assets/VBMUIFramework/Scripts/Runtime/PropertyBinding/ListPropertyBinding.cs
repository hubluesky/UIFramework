using UnityEngine;

namespace VBM {
    [System.Serializable]
    public class ListPropertyBinding : PropertyBinding {
        public ListBindingContent listContent;
        
        public override void OnPropertyChange(object value) {
            listContent.SetListModel(value as ListModel);
        }

        public override void Initialized() {
            listContent.Initialize();
        }

        public override void Finalized() {
            listContent.Finalized();
        }
    }
}