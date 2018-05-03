using System;
using Object = UnityEngine.Object;

namespace VBM {
    [System.Serializable]
    public enum MethodParameterType {
        Bool,
        Int,
        Float,
        String,
        Enum,
        UnityObject,
    }

    [System.Serializable]
    public class MethodParameter {
        public static readonly Type BoolType = typeof(Boolean);
        public static readonly Type IntType = typeof(Int32);
        public static readonly Type FloatType = typeof(Single);
        public static readonly Type StringType = typeof(String);
        public static readonly Type EnumType = typeof(Enum);
        public static readonly Type UnityObjectType = typeof(Object);

        public bool boolParam;
        public int intParam;
        public float floatParam;
        public string stringParam;
        public SerializableEnum enumParam;
        public Object objectParam;

        public object GetParameterValue(MethodParameterType type) {
            switch (type) {
            case MethodParameterType.Bool:
            return boolParam;
            case MethodParameterType.Int:
            return intParam;
            case MethodParameterType.Float:
            return floatParam;
            case MethodParameterType.String:
            return stringParam;
            case MethodParameterType.Enum:
            return enumParam.enumObject;
            case MethodParameterType.UnityObject:
            return objectParam;
            default:
            return null;
            }
        }

        public object GetParameterValue(System.Type parameterType) {
            if (parameterType == BoolType)
                return boolParam;
            else if (parameterType == IntType)
                return intParam;
            else if (parameterType == FloatType)
                return floatParam;
            else if (parameterType == StringType)
                return stringParam;
            else if (EnumType.IsAssignableFrom(parameterType))
                return System.Enum.ToObject(parameterType, intParam);
            else if (UnityObjectType.IsAssignableFrom(parameterType))
                return objectParam;
            return null;
        }
    }
}