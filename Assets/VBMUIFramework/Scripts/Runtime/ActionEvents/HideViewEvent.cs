using System;
using UnityEngine;
using UnityEngine.UI;

namespace VBM {

    [RequireComponent(typeof(Button))]
    public class HideViewEvent : MonoBehaviour {
        public ViewModelBinding viewModelBinding;
        private Button button;

        void Start() {
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