using UnityEngine;
using UnityEngine.UI;

namespace VBM {
    [RequireComponent(typeof(InputField))]
    public class InputFieldEndEvent : ActionEvent {
        private InputField inputField;
        public override System.Type ParameterType { get { return typeof(string); } }

        void Start() {
            if (viewModelBinding == null || viewModelBinding.model == null) {
                Debug.LogWarning("Input Field end event binding failed! the viewModelBinding model is null" + name);
                return;
            }
            inputField = GetComponent<InputField>();
            inputField.onEndEdit.AddListener(OnEventChanged);
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