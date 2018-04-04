using UnityEngine;

namespace VBM {
    [System.Serializable]
    public class GameObjectActivePropertyBinding : PropertyBinding {
        public GameObject gameObject;

        public override void OnPropertyChange(object value) {
            gameObject.SetActive((bool) value);
        }
    }
}