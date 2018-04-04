using UnityEngine;

namespace VBM {
    public class ViewConfigAsset : ScriptableObject {
        [SerializeField]
        private ViewConfig[] viewConfigs;
        public ViewConfig[] configs { get { return viewConfigs; } }

        private void OnEnable() {
            foreach (ViewConfig viewConfig in viewConfigs) {
                if (viewConfig.prefab != null) {
                    ViewModelBinding binding = viewConfig.prefab.GetComponent<ViewModelBinding>();
                    if (binding != null)
                        viewConfig.name = binding.modelId;
                }
            }
        }
    }
}