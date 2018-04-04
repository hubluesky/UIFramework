using UnityEngine.UI;

namespace VBM {
    [System.Serializable]
    public class TextPropertyBinding : PropertyBinding {
        public Text component;

        public override void OnPropertyChange(object value) {
            component.text = value as string;
        }
    }
}