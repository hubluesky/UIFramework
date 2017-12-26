using UnityEngine;

namespace VBM {
    public class TransientView : View {
        public bool hideDestroyAsset { get; set; }

        public override void Show() {
            if (transform == null)
                ViewManager.Instance.LoadViewAsset(this);
            else
                base.Show();
        }

        public override void SetViewAsset(GameObject gameObject) {
            base.SetViewAsset(gameObject);
            base.Show();
        }

        public override void Hide() {
            base.Hide();
            if (hideDestroyAsset)
                DestroyAsset();
        }
    }
}