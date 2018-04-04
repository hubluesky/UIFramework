using UnityEngine;
using UnityEngine.UI;

namespace VBM {
    [System.Serializable]
    public class GraphicColorPropertyBinding : PropertyBinding {
        public Graphic component;

        public override void OnPropertyChange(object value) {
            component.color = (Color)value;
        }
    }
}