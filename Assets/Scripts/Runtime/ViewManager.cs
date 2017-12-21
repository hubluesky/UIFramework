using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VBM {
    public class ViewManager : Singleton<ViewManager> {
        struct LayerTransform {
            public ViewLayer layer;
            public Transform transform;
        }

        private Dictionary<ViewLayer, Stack<View>> viewStackMap = new Dictionary<ViewLayer, Stack<View>>();
        private List<LayerTransform> layerList = new List<LayerTransform>();
        public Canvas rootCanvas { get; private set; }

        public T CreateView<T>(ViewConfig config) where T : View, new() {
            T view = new T();
            view.config = config;
            return view;
        }

        public void LoadViewAsset(View view) {
            view.config.LoadAsset(view.SetViewAsset);
        }

        public void InitCanvasLayers(Canvas canvas) {
            rootCanvas = canvas;
            foreach (ViewLayer layer in System.Enum.GetValues(typeof(ViewLayer))) {
                GameObject layerObject = CanvasUtility.CreateLayer(canvas.transform, layer.ToString());
                layerList.Add(new LayerTransform() { layer = layer, transform = layerObject.transform });
            }
        }

        internal void ShowView(View view) {
            int index = layerList.FindIndex((item) => item.layer == view.config.layer);
            if (index == -1) {
                Debug.LogWarning("Show view failed! Have not include layer " + view.config.layer);
                return;
            }

            Transform layerTransform = layerList[index].transform;
            switch (view.config.showRule) {
                case ViewShowRule.HideLayerView:
                    for (int i = 0; i < layerTransform.childCount; i++) {
                        Transform child = layerTransform.GetChild(i);
                        if (!child.gameObject.activeSelf)
                            continue;
                        child.gameObject.SetActive(false);
                        View childView = child.GetComponent<ViewModelBinding>().view;
                        if (childView.config.hideRule == ViewHideRule.SaveToStack) {
                            Stack<View> stack;
                            if (!viewStackMap.TryGetValue(childView.config.layer, out stack))
                                stack = new Stack<View>();
                            stack.Push(childView);
                        }
                    }
                    break;
            }
            if (view.transform.parent == layerTransform)
                view.transform.SetAsLastSibling();
            else
                view.transform.SetParent(layerTransform, false);
            view.transform.gameObject.SetActive(true);
        }

        internal void HideView(View view) {
            int index = layerList.FindIndex((item) => item.layer == view.config.layer);
            if (index == -1) {
                Debug.LogWarning("Show view failed! Have not include layer " + view.config.layer);
                return;
            }

            view.transform.gameObject.SetActive(false);
            if (view.config.showRule == ViewShowRule.HideLayerView) {
                Stack<View> stack;
                if (viewStackMap.TryGetValue(view.config.layer, out stack)) {
                    while (stack.Count > 0) {
                        View stackView = stack.Pop();
                        stackView.Show();
                        if (stackView.config.showRule == ViewShowRule.HideLayerView)
                            break;
                    }
                }
            }
        }
    }
}