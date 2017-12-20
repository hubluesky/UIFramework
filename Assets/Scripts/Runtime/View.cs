using UnityEngine;

namespace VBM {
    public class View {
        public readonly ViewConfig config;
        public Transform transform { get; protected set; }

        public View(ViewConfig config) {
            this.config = config;
        }

        public void SetViewAsset(Transform transform) {
            this.transform = transform;
        }

        internal void OnHideView() {
            OnHide();
            if (config.hideRule == ViewHideRule.SaveToStack)
                ViewManager.Instance.PushStack(this);
        }

        public void OnCreated() { }
        public void OnShow() { }
        public void OnHide() { }
        public void OnDestroyed() { }
    }
}