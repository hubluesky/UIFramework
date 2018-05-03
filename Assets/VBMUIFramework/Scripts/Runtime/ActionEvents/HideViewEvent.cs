using System;
using UnityEngine;
using UnityEngine.UI;

namespace VBM {

    [RequireComponent(typeof(Button)), AddComponentMenu("VBMUIFramework/ActionEvents/HideViewEvent")]
    public class HideViewEvent : MonoBehaviour {
        private ViewModelBinding viewModelBinding;
        private Button button;

        void Start() {
            viewModelBinding = GetComponentInParent<ViewModelBinding>();
            if (viewModelBinding == null || viewModelBinding.model == null) {
                Debug.LogWarning("Hide view event binding failed! the viewModelBinding model is null" + name);
                return;
            }
            button = GetComponent<Button>();
            button.onClick.AddListener(OnEventChanged);
        }

        void OnDestroy() {
            if (button != null)
                button.onClick.RemoveListener(OnEventChanged);
        }

        void OnEventChanged() {
            viewModelBinding.view.Hide();
        }
    }
}