using UnityEngine;

namespace VBM {
    [System.Serializable]
    public class ViewConfig {
        public string assetPath;
        public GameObject prefab;
        public ViewLayer layer;
        public ViewShowRule showRule;
        public ViewHideRule hideRule;

        public void LoadAsset(System.Action<GameObject> completed) {
            if (prefab != null)
                completed(Object.Instantiate(prefab));
        }
    }
}