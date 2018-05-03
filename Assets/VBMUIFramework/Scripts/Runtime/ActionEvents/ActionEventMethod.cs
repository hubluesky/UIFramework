using UnityEngine;

namespace VBM {
    [System.Serializable]
    public class ActionEventMethod : ISerializationCallbackReceiver {
        public string methodName;
        public string parameterTypeName;
        public MethodParameter parameter;
        public System.Type parameterType { get; private set; }
        public object parameterValue { get; private set; }

        void ISerializationCallbackReceiver.OnAfterDeserialize() {
            if (parameter != null && !string.IsNullOrEmpty(parameterTypeName)) {
                parameterType = System.Type.GetType(parameterTypeName);
                parameterValue = parameter.GetParameterValue(parameterType);
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }
    }
}