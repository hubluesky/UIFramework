using UnityEngine.UI;

namespace VBM {
    [System.Serializable]
    public class TogglePropertyBinding : PropertyBinding {
        public Toggle component;

        public override void OnPropertyChange(object value) {
            component.isOn = (bool)value;
        }
    }
}