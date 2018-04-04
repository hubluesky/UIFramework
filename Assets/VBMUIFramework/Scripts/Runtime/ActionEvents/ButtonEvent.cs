using System;
using UnityEngine;
using UnityEngine.UI;

namespace VBM {
    [RequireComponent(typeof(Button))]
    public class ButtonEvent : ActionEvent {
        private Button button;
        public override Type ParameterType { get { return null; } }

        void Start() {
            if (CheckViewModelBinding()) {
                button = GetComponent<Button>();
                button.onClick.AddListener(OnEventChanged);
            }
        }

        void OnDestroy() {
            if (button != null)
                button.onClick.RemoveListener(OnEventChanged);
        }

        void OnEventChanged() {
            CallMemberFunctions();
        }
    }
}