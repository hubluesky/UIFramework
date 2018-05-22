using System;
using UnityEngine;

namespace VBM {
    [AddComponentMenu("VBMUIFramework/ActionEvents/OnEnabledEvent")]
    public class OnEnabledEvent : ActionEvent {
        public override Type ParameterType { get { return typeof(bool); } }

        void OnEnable() {
            if (gameObject.activeSelf)
                CallMemberFunctions(true);
        }

        void OnDisable() {
            if (!gameObject.activeSelf)
                CallMemberFunctions(false);
        }
    }
}