using UnityEngine;

namespace VBM {
    public class ViewProxy : View {
        public System.Action<GameObject> SetViewAssetFunc;
        public System.Action ShowFunc;
        public System.Action HideFunc;
        public System.Action OnShowFunc;
        public System.Action OnHideFunc;
        public System.Action OnCreatedFunc;
        public System.Action OnDestroyedFunc;

        public ViewProxy() {
            SetViewAssetFunc = SetViewAsset;
            ShowFunc = Show;
            HideFunc = Hide;
            OnShowFunc = OnShow;
            OnHideFunc = OnHide;
            OnCreatedFunc = OnCreated;
            OnDestroyedFunc = OnDestroyed;
        }

        public void BaseSetViewAsset(GameObject gameObject) {
            base.SetViewAsset(gameObject);
        }

        public void BaseShow() {
            base.Show();
        }

        public void BaseHide() {
            base.Hide();
        }

        public override void SetViewAsset(GameObject gameObject) {
            SetViewAssetFunc(gameObject);
        }

        public override void Show() {
            ShowFunc();
        }

        public override void Hide() {
            HideFunc();
        }

        public override void OnCreated() {
            OnCreatedFunc();
        }
        public override void OnShow() {
            OnShowFunc();
        }
        public override void OnHide() {
            OnHideFunc();
        }
        public override void OnDestroyed() {
            OnDestroyedFunc();
        }
    }
}