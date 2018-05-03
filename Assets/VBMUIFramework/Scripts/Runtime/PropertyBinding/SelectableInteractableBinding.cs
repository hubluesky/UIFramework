using UnityEngine.UI;

namespace VBM {
    [System.Serializable]
    public class SelectableInteractableBinding : PropertyBinding {
        public Selectable component;

        public override void OnPropertyChange(object value) {
            component.interactable = (bool) value;
        }
    }
}