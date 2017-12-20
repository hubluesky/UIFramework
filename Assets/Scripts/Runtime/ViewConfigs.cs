using UnityEngine;

namespace VBM {
    public class ViewConfigs : ScriptableObject {
        [SerializeField]
        private ViewConfig[] viewConfigs;
        public ViewConfig[] configs { get { return viewConfigs; } }
    }
}