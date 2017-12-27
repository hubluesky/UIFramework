using System;
using UnityEngine;
using UnityEngine.UI;

namespace VBM {
    [RequireComponent(typeof(Toggle))]
    public class ToggleEvent : ActionEvent {
        private Toggle toggle;

        public override Type ParameterType { get { return typeof(bool); } }

        void Start() {
            if (viewModelBinding.model == null) {
                Debug.LogWarning("Toggle event binding failed! the viewModelBinding model is null" + name);
                return;
            }
            toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(OnEventChanged);
        }

        void OnDestroy() {
            if (toggle != null)
                toggle.onValueChanged.RemoveListener(OnEventChanged);
        }

        void OnEventChanged(bool value) {
            CallMemberFunctions(value);
        }
    }
}