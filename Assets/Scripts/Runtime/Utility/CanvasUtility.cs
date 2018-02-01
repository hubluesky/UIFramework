using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VBM {
    public static class CanvasUtility {
        public static Canvas CreateRootCanvas(int width, int height) {
            // Create uiroot transform
            GameObject uiRoot = new GameObject("UIRoot");
            uiRoot.layer = LayerMask.NameToLayer("UI");
            // uiRoot.AddComponent<Transform>();
            // Create ui camera
            GameObject uiCamera = new GameObject("UICamera");
            uiCamera.layer = LayerMask.NameToLayer("UI");
            uiCamera.transform.SetParent(uiRoot.transform, false);
            uiCamera.transform.localPosition = new Vector3(0, 0, -10f);
            Camera camera = uiCamera.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.Depth;
            camera.orthographic = true;
            camera.nearClipPlane = 1f;
            camera.farClipPlane = 100f;
            camera.cullingMask = 1 << 5;
            uiCamera.AddComponent<GUILayer>();
            // Create screen space camera canvas
            GameObject uiCanvas = new GameObject("RootCanvas");
            uiCanvas.layer = LayerMask.NameToLayer("UI");
            uiCanvas.transform.SetParent(uiRoot.transform, false);
            Canvas canvas = uiCanvas.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.pixelPerfect = true;
            canvas.worldCamera = camera;
            uiCanvas.AddComponent<GraphicRaycaster>();
            CanvasScaler canvasScaler = uiCanvas.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(width, height);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            canvasScaler.matchWidthOrHeight = 1.0f;
            // Create event system
            GameObject uiEventSystem = new GameObject("EventSystem");
            uiEventSystem.layer = LayerMask.NameToLayer("UI");
            uiEventSystem.transform.SetParent(uiRoot.transform, false);
            uiEventSystem.AddComponent<EventSystem>();
            uiEventSystem.AddComponent<StandaloneInputModule>();
            return canvas;
        }

        public static GameObject CreateLayer(Transform parent, string name) {
            GameObject uiLayer = new GameObject(name);
            uiLayer.layer = LayerMask.NameToLayer("UI");
            uiLayer.transform.SetParent(parent, false);

            RectTransform rectTransform = uiLayer.AddComponent<RectTransform>();
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            return uiLayer;
        }
    }
}