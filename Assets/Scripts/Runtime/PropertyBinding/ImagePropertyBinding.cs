using UnityEngine;
using UnityEngine.UI;

namespace VBM {
    [System.Serializable]
    public class ImagePropertyBinding : PropertyBinding {
        public Image component;

        public override void OnPropertyChange(object value) {
            component.sprite = value as Sprite;
        }
    }
}