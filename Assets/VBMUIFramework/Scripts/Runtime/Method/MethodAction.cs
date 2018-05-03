using System;
using System.Reflection;
using UnityEngine;

namespace VBM {
    [System.Serializable]
    public class MethodAction : ISerializationCallbackReceiver {
        public const int MaxParameterCount = 4;
        public string targetType;
        public string methodName;
        public MethodParameterPassing[] parameters;
        public BindingFlags bindingFlags;

        private Delegate methodDelegate;
        private MethodInfo methodInfo;
        private object[] args;

        public object Invoke(IModel model) {
            for (int i = 0; i < parameters.Length; i++)
                args[i] = parameters[i].GetParameterValue((name) => { return model.GetProperty(name); });
                
            if (methodDelegate != null)
                return methodDelegate.DynamicInvoke(args);
            else
                return methodInfo.Invoke(null, args);
        }

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize() {
            if (parameters != null)
                args = new object[parameters.Length];
            if (!string.IsNullOrEmpty(targetType)) {
                Type type = Type.GetType(targetType);
                methodInfo = type.GetMethod(methodName, bindingFlags);
                if (methodInfo != null) {
                    System.Type delegateType = GetMethodInfoDelegateType(methodInfo);
                    methodDelegate = Delegate.CreateDelegate(delegateType, null, methodInfo);
                }
            }
        }

        public static Type GetMethodInfoDelegateType(MethodInfo methodInfo) {
            ParameterInfo[] parametersInfo = methodInfo.GetParameters();
            Type[] types = new Type[parametersInfo.Length + 1];
            for (int i = 0; i < parametersInfo.Length; i++)
                types[i] = parametersInfo[i].ParameterType;
            types[parametersInfo.Length] = methodInfo.ReturnType;
            switch (parametersInfo.Length) {
                case 0:
                    return typeof(Func<>).MakeGenericType(types);
                case 1:
                    return typeof(Func<,>).MakeGenericType(types);
                case 2:
                    return typeof(Func<, ,>).MakeGenericType(types);
                case 3:
                    return typeof(Func<, , ,>).MakeGenericType(types);
                case 4:
                    return typeof(Func<, , , ,>).MakeGenericType(types);
                default:
                    Debug.LogWarning("Create method info delegate failed!");
                    return null;
            }
        }
    }
}