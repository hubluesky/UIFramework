using UnityEngine.UI;

namespace VBM {
    [System.Serializable]
    public class InputFieldPropertyBinding : PropertyBinding {
        public InputField component;

        public override void OnPropertyChange(object value) {
            component.text = value as string;
        }
    }
}