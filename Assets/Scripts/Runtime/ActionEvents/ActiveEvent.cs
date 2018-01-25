using System;

namespace VBM {
    public class ActiveEvent : ActionEvent {
        public override Type ParameterType { get { return typeof(bool); } }

        void OnEnable() {
            CallMemberFunctions(true);
        }

        void OnDisable() {
            CallMemberFunctions(false);
        }
    }
}