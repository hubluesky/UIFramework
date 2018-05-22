using UnityEngine;

namespace VBM {

    [RequireComponent(typeof(RectTransform))]
    public class DelayLoadedUI : MonoBehaviour {
        public RectTransform uiPrefab;

        private void Start() {
           Instantiate(uiPrefab, transform, false);
        }
    }
}