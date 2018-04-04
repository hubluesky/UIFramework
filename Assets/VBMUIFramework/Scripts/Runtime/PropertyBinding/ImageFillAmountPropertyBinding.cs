using UnityEngine;
using UnityEngine.UI;

namespace VBM {
    [System.Serializable]
    public class ImageFillAmountPropertyBinding : PropertyBinding {
        public Image component;

        public override void OnPropertyChange(object value) {
            component.fillAmount = (float)value;
        }
    }
}