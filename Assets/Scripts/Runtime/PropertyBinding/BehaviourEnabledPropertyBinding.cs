using UnityEngine;

namespace VBM {
    [System.Serializable]
    public class BehaviourEnabledPropertyBinding : PropertyBinding {
        public MonoBehaviour component;

        public override void OnPropertyChange(object value) {
            component.enabled = (bool)value;
        }
    }
}