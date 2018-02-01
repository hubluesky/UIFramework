using UnityEngine;
using UnityEngine.UI;

namespace VBM {
    [System.Serializable]
    public class SliderValuePropertyBinding : PropertyBinding {
        public Slider component;

        public override void OnPropertyChange(object value) {
            component.value = (float) value;
        }
    }
}