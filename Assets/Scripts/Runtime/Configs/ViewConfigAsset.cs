using UnityEngine;

namespace VBM {
    public class ViewConfigAsset : ScriptableObject {
        [SerializeField]
        private ViewConfig[] viewConfigs;
        public ViewConfig[] configs { get { return viewConfigs; } }
    }
}