namespace VBM {
    [System.Serializable]
    public class MethodParameterPassing {
        public MethodParameter parameter;
        public MethodParameterType parameterType;
        public string passingName;

        public object GetParameterValue(System.Func<string, object> GetParameterPassing) {
            if (!string.IsNullOrEmpty(passingName))
                return GetParameterPassing(passingName);
            return parameter.GetParameterValue(parameterType);
        }
    }
}