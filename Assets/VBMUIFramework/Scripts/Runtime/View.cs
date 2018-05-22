using UnityEngine;

namespace VBM {
    public class View {
        public event System.Action OnLoadAsset;
        public event System.Action OnDestroyAsset;

        public ViewConfig config { get; internal set; }
        public Transform transform { get; protected set; }
        protected IModel model;
        public bool isLoadingAsset { get; internal set; }
        protected bool delayShow;

        public virtual void SetViewAsset(GameObject gameObject) {
            gameObject.name = config.viewName;
            this.transform = gameObject.transform;
            ViewModelBinding binding = gameObject.GetComponent<ViewModelBinding>();
            if (binding != null) {
                binding.view = this;
                if (model != null)
                    binding.SetModel(model);
            }
            MonoBehaviourEvent objectEvent = gameObject.AddComponent<MonoBehaviourEvent>();
            objectEvent.onStartEvent += OnCreated;
            objectEvent.onEnableEvent += OnShow;
            objectEvent.onDisableEvent += OnHide;
            objectEvent.onDestroyEvent += OnDestroyed;
            isLoadingAsset = false;
            if (OnLoadAsset != null)
                OnLoadAsset();
            if (delayShow)
                Show();
            delayShow = false;
        }

        public void DestroyAsset() {
            if (transform != null) {
                if (OnDestroyAsset != null)
                    OnDestroyAsset();
                Object.Destroy(transform.gameObject);
                transform = null;
            }
        }

        public void SetModel(IModel model) {
            this.model = model;
            if (transform != null)
                transform.GetComponent<ViewModelBinding>().SetModel(model);
        }

        public bool IsShowing() {
            return transform != null && transform.gameObject.activeSelf;
        }

        public virtual void Show() {
            if (transform == null) {
                delayShow = true;
                if (!isLoadingAsset)
                    ViewManager.Instance.LoadViewAsset(this);
            } else {
                ViewManager.Instance.ShowView(this);
            }
        }

        public virtual void Hide() {
            if (delayShow) {
                delayShow = false;
            } else {
                ViewManager.Instance.HideView(this);
            }
        }

        public virtual void OnCreated() { }
        public virtual void OnShow() { }
        public virtual void OnHide() { }
        public virtual void OnDestroyed() { }
    }
}