using UnityEngine;

namespace VBM {
    public class TransientView : View {
        public override void Show() {
            ViewManager.Instance.LoadViewAsset(this);
        }

        public override void SetViewAsset(GameObject gameObject) {
            base.SetViewAsset(gameObject);
            base.Show();
        }

        public override void Hide() {
            base.Hide();
            DestroyAsset();
        }
    }
}