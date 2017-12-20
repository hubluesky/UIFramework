using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VBM {
    public static class CanvasUtility {
        public static Canvas CreateRootCanvas() {
            // Create uiroot transform
            GameObject uiRoot = new GameObject("UIRoot");
            uiRoot.layer = LayerMask.NameToLayer("UI");
            uiRoot.AddComponent<RectTransform>();
            // Create ui camera
            GameObject uiCamera = new GameObject("UICamera");
            uiCamera.layer = LayerMask.NameToLayer("UI");
            uiCamera.transform.SetParent(uiRoot.transform, false);
            uiCamera.transform.localPosition = new Vector3(0, 0, -10f);
            Camera camera = uiCamera.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.Depth;
            camera.orthographic = true;
            camera.farClipPlane = 200f;
            camera.cullingMask = 1 << 5;
            camera.nearClipPlane = -50f;
            camera.farClipPlane = 50f;
            uiCamera.AddComponent<AudioListener>();
            uiCamera.AddComponent<GUILayer>();
            // Create screen space camera canvas
            GameObject uiCanvas = new GameObject("RootCanvas");
            uiCanvas.transform.SetParent(uiRoot.transform, false);
            Canvas canvas = uiCanvas.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.pixelPerfect = true;
            canvas.worldCamera = camera;
            uiCanvas.AddComponent<GraphicRaycaster>();
            CanvasScaler canvasScaler = uiCanvas.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1280, 720);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            canvasScaler.matchWidthOrHeight = 1.0f;
            // Create event system
            GameObject uiEventSystem = new GameObject("EventSystem");
            uiEventSystem.layer = LayerMask.NameToLayer("UI");
            uiEventSystem.transform.SetParent(uiRoot.transform, false);
            uiEventSystem.AddComponent<EventSystem>();
            uiEventSystem.AddComponent<StandaloneInputModule>();

            CreateCavnasLayers(canvas);
            return canvas;
        }

        public static void CreateCavnasLayers(Canvas canvas) {
            foreach (Layer layer in System.Enum.GetValues(typeof(Layer))) {
                CreateLayer(canvas.transform, layer.ToString());
            }
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