using UnityEngine;
using UnityEngine.UI;

namespace VBM {
    [RequireComponent(typeof(InputField)), AddComponentMenu("VBMUIFramework/ActionEvents/InputFieldEndEvent")]
    public class InputFieldEndEvent : ActionEvent {
        private InputField inputField;
        public override System.Type ParameterType { get { return typeof(string); } }

        void Start() {
            if (CheckViewModelBinding()) {
                inputField = GetComponent<InputField>();
                inputField.onEndEdit.AddListener(OnEventChanged);
            }
        }

        void OnDestroy() {
            if (inputField != null)
                inputField.onEndEdit.RemoveListener(OnEventChanged);
        }

        void OnEventChanged(string value) {
            CallMemberFunctions(value);
        }
    }
}