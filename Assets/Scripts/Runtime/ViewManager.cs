using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VBM {
    public class ViewManager : Singleton<ViewManager> {
        private Stack<View> viewStack = new Stack<View>();
        private Dictionary<int, Transform> layerMap = new Dictionary<int, Transform>();
        public Canvas rootCanvas { get; set; }

        public T CreateView<T>(ViewConfig config) where T : View,
        new() {
            T view = new T( /*config*/ );
            config.CreateAsset((gameObject) => { OnAssetCreate(view, gameObject); });
            return view;
        }

        private void OnAssetCreate(View view, GameObject viewAsset) {
            ViewModelBinding binding = viewAsset.GetComponent<ViewModelBinding>();
            binding.view = view;
            GameObjectEvent objectEvent = viewAsset.AddComponent<GameObjectEvent>();
            objectEvent.onStartEvent += view.OnCreated;
            objectEvent.onEnableEvent += view.OnShow;
            objectEvent.onDisableEvent += view.OnHide;
            objectEvent.onDestroyEvent += view.OnDestroyed;
            view.SetViewAsset(viewAsset.transform);
        }

        public void InitCanvasLayers(Canvas canvas, System.Type layerEnumType) {
            foreach (var layer in System.Enum.GetValues(layerEnumType)) {
                GameObject layerObject = CanvasUtility.CreateLayer(canvas.transform, layer.ToString());
                layerMap.Add((int) layer, layerObject.transform);
            }
        }

        internal void PushStack(View view) {
            viewStack.Push(view);
        }

        public void ShowView(View view) {
            int layer = (int) view.config.layer;
            if (!layerMap.ContainsKey(layer)) {
                Debug.LogWarning("Show view failed! Have not include layer " + layer);
                return;
            }

            Transform transform = layerMap[layer];
            switch (view.config.showRule) {
                case ViewShowRule.HideLayer:
                    HideAllChild(transform);
                    break;
                case ViewShowRule.HideLowLayers:
                    foreach (var entry in layerMap) {
                        if (entry.Key <= layer)
                            HideAllChild(entry.Value);
                    }
                    break;
            }
            view.transform.SetParent(transform, false);
            view.transform.gameObject.SetActive(true);
        }

        private void HideAllChild(Transform parent) {
            for (int i = 0; i < parent.childCount; i++) {
                Transform child = parent.GetChild(i);
                child.gameObject.SetActive(false);
            }
        }

        public void HideView(View view) {
            int layer = (int) view.config.layer;
            if (!layerMap.ContainsKey(layer)) {
                Debug.LogWarning("Hide view failed! Have not include layer " + layer);
                return;
            }
        }
    }
}