using UnityEngine;

namespace VBM {
    public class View {
        public ViewConfig config { get; internal set; }
        public Transform transform { get; protected set; }

        public void SetViewAsset(GameObject gameObject) {
            this.transform = gameObject.transform;
            ViewModelBinding binding = gameObject.GetComponent<ViewModelBinding>();
            binding.view = this;
            GameObjectEvent objectEvent = gameObject.AddComponent<GameObjectEvent>();
            objectEvent.onStartEvent += OnCreated;
            objectEvent.onEnableEvent += OnShow;
            objectEvent.onDisableEvent += OnHide;
            objectEvent.onDestroyEvent += OnDestroyed;
        }

        public virtual void Show() {
            ViewManager.Instance.ShowView(this);
        }

        public virtual void Hide() {
            ViewManager.Instance.HideView(this);
        }

        public virtual void OnCreated() { }
        public virtual void OnShow() { }
        public virtual void OnHide() { }
        public virtual void OnDestroyed() { }
    }
}