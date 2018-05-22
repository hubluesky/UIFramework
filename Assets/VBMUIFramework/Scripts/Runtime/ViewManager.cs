using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VBM {
    public sealed class ViewManager : Singleton<ViewManager> {
        struct LayerTransform {
            [PropertyToEnumDrawerAttribute]
            public int layer;
            public Transform transform;
            public List<View> viewList;
        }
        private Dictionary<string, View> viewMap = new Dictionary<string, View>();
        private Dictionary<int, Stack<View>> viewShowStackMap = new Dictionary<int, Stack<View>>();
        private List<LayerTransform> viewLayerList = new List<LayerTransform>();
        public Canvas rootCanvas { get; private set; }

        public View CreateView(ViewConfig config) {
            return CreateView<View>(config);
        }

        public View CreateView(string uniqueId, ViewConfig config) {
            return CreateView<View>(uniqueId, config);
        }

        public T CreateView<T>(ViewConfig config) where T : View, new() {
            return CreateView<T>(config.viewName, config);
        }

        public T CreateView<T>(string uniqueId, ViewConfig config) where T : View, new() {
            if (viewMap.ContainsKey(uniqueId)) {
                Debug.LogError("Create view failed! The uniqueId had contains " + uniqueId);
                return null;
            }
            T view = new T();
            view.config = config;
            viewMap.Add(uniqueId, view);
            return view;
        }

        public void LoadViewAsset(View view) {
            view.isLoadingAsset = true;
            view.config.LoadAsset(view.SetViewAsset);
        }

        public View GetView<T>() where T : IModel {
            return GetView(typeof(T).Name);
        }

        public View GetView(string uniqueId) {
            View view;
            viewMap.TryGetValue(uniqueId, out view);
            return view;
        }

        public void InitCanvasLayers(Canvas canvas, System.Type enumType) {
            rootCanvas = canvas;
            foreach (var layer in System.Enum.GetValues(enumType)) {
            GameObject layerObject = CanvasUtility.CreateLayer(canvas.transform, layer.ToString());
            viewLayerList.Add(new LayerTransform() { layer = (int) layer, transform = layerObject.transform, viewList = new List<View>() });
            }
        }

        public Transform GetLayerTransform(System.Enum layer) {
            return GetLayerTransform(System.Convert.ToInt32(layer));
        }

        public Transform GetLayerTransform(int layer) {
            foreach (LayerTransform layerTransform in viewLayerList) {
                if (layerTransform.layer == layer)
                    return layerTransform.transform;
            }
            return null;
        }

        public void ShowView<T>() where T : IModel {
            ShowView(typeof(T).Name);
        }

        public void ShowView(string viewName) {
            View view = GetView(viewName);
            if (view == null) {
                Debug.LogWarning("Show view failed! Have not view " + viewName);
                return;
            }
            view.Show();
        }

        public void HideView<T>() where T : IModel {
            HideView(typeof(T).Name);
        }

        public void HideView(string vieName) {
            View view = GetView(vieName);
            if (view == null) {
                Debug.LogWarning("Hide view failed! Have not view " + vieName);
                return;
            }
            view.Hide();
        }

        public void UnloadAllView() {
            foreach (Stack<View> stackView in viewShowStackMap.Values) {
                stackView.Clear();
            }
            foreach (LayerTransform layerTransform in viewLayerList)
                layerTransform.viewList.Clear();
            foreach (View view in viewMap.Values)
                view.DestroyAsset();
        }

        internal void ShowView(View view) {
            int index = viewLayerList.FindIndex((item) => item.layer == view.config.layer);
            if (index == -1) {
                Debug.LogWarningFormat("Show view {0} failed! Have not include layer {1}", view.config.viewName, view.config.layer);
                return;
            }

            LayerTransform layerTransform = viewLayerList[index];
            switch (view.config.showRule) {
                case ViewShowRule.HideSameLayerView:
                    HideLayerViews(layerTransform);
                    break;
                case ViewShowRule.HideLowLayerView:
                    for (int i = index; i >= 0; i--)
                        HideLayerViews(viewLayerList[i]);
                    break;
            }
            if (view.transform.parent == viewLayerList[index].transform)
                view.transform.SetAsLastSibling();
            else
                view.transform.SetParent(viewLayerList[index].transform, false);
            view.transform.gameObject.SetActive(true);
            layerTransform.viewList.Add(view);
        }

        private void HideLayerViews(LayerTransform layerTransform) {
            for (int i = layerTransform.viewList.Count - 1; i >= 0; i--) {
                View view = layerTransform.viewList[i];
                if (view.transform == null || !view.transform.gameObject.activeSelf) continue;
                view.transform.gameObject.SetActive(false);
                layerTransform.viewList.RemoveAt(i);

                if (view.config.hideRule == ViewHideRule.SaveToStack) {
                    Stack<View> stack;
                    if (!viewShowStackMap.TryGetValue(view.config.layer, out stack)) {
                        stack = new Stack<View>();
                        viewShowStackMap.Add(view.config.layer, stack);
                    }
                    stack.Push(view);
                } else if (view.config.hideRule == ViewHideRule.DestroyAsset) {
                    view.DestroyAsset();
                }
            }
        }

        internal void HideView(View view) {
            int index = viewLayerList.FindIndex((item) => item.layer == view.config.layer);
            if (index == -1) {
                Debug.LogWarning("Show view failed! Have not include layer " + view.config.layer);
                return;
            }

            viewLayerList[index].viewList.Remove(view);
            if (view.transform != null)
                view.transform.gameObject.SetActive(false);

            if (view.config.showRule == ViewShowRule.HideSameLayerView) {
                ShowLayerViews(view.config.layer);
            } else if (view.config.showRule == ViewShowRule.HideLowLayerView) {
                for (int i = view.config.layer; i >= 0; i--) {
                    if (ShowLayerViews(i))
                        break;
                }
            }
        }

        private bool ShowLayerViews(int layer) {
            Stack<View> stack;
            if (viewShowStackMap.TryGetValue(layer, out stack)) {
                while (stack.Count > 0) {
                    View stackView = stack.Pop();
                    stackView.Show();
                    viewLayerList[layer].viewList.Add(stackView);
                    if (stackView.config.showRule == ViewShowRule.HideSameLayerView || stackView.config.showRule == ViewShowRule.HideLowLayerView)
                        return true;
                }
            }
            return false;
        }
    }
}