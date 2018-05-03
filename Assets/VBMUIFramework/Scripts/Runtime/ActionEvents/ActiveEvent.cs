using System;
using UnityEngine;

namespace VBM {
    [AddComponentMenu("VBMUIFramework/ActionEvents/ActiveEvent")]
    public class ActiveEvent : ActionEvent {
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