using UnityEngine;
using UnityEngine.UI;

namespace VBM {
    [RequireComponent(typeof(InputField))]
    public class InputFieldValueChangeEvent : ActionEvent {
        private InputField inputField;
        public override System.Type ParameterType { get { return typeof(string); } }

        void Start() {
            if (viewModelBinding == null || viewModelBinding.model == null) {
                Debug.LogWarning("Input Field end event binding failed! the viewModelBinding model is null" + name);
                return;
            }
            inputField = GetComponent<InputField>();
            inputField.onValueChanged.AddListener(OnEventChanged);
        }

        void OnDestroy() {
            if (inputField != null)
                inputField.onValueChanged.RemoveListener(OnEventChanged);
        }

        void OnEventChanged(string value) {
            CallMemberFunctions(value);
        }
    }
}