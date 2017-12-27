using System.Reflection;
using UnityEngine;

namespace VBM {
    public abstract class ActionEvent : MonoBehaviour {
        [System.Serializable]
        public class MemberData {
            public string memberName;
            public MemberTypes memberType;
        }

        public const BindingFlags bindingAttr = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public;
        public object[] parameters = new object[1];
        public ViewModelBinding viewModelBinding;
        public MemberData[] memberDataArray = new MemberData[0];

        public abstract System.Type ParameterType { get; }

        public void CallMemberFunctions() {
            foreach (MemberData memberData in memberDataArray) {
                CallMemberFunction(memberData.memberType, memberData.memberName);
            }
        }

        public void CallMemberFunctions<T>(T value) {
            foreach (MemberData memberData in memberDataArray) {
                CallMemberFunction(memberData.memberType, memberData.memberName, value);
            }
        }

        private void CallMemberFunction(MemberTypes memberType, string memberName) {
            if (memberType != MemberTypes.Method) {
                Debug.LogWarningFormat("Action event call member function failed! the memberType {0} is no method", memberType);
            } else {
                System.Type type = viewModelBinding.model.GetType();
                MethodInfo methodInfo = type.GetMethod(memberName, bindingAttr);
                if (methodInfo == null) {
                    Debug.LogWarning("Action event get method failed!" + memberName);
                } else {
                    methodInfo.Invoke(viewModelBinding.model, null);
                }
            }
        }

        private void CallMemberFunction<T>(MemberTypes memberType, string memberName, T value) {
            System.Type type = viewModelBinding.model.GetType();
            switch (memberType) {
                case MemberTypes.Field:
                    FieldInfo fieldInfo = type.GetField(memberName, bindingAttr);
                    if (fieldInfo == null) {
                        Debug.LogWarning("Action event get field failed!" + memberName);
                    } else {
                        fieldInfo.SetValue(viewModelBinding.model, value);
                    }
                    break;
                case MemberTypes.Property:
                    PropertyInfo propertyInfo = type.GetProperty(memberName, bindingAttr);
                    if (propertyInfo == null) {
                        Debug.LogWarning("Action event get property failed!" + memberName);
                    } else {
                        propertyInfo.SetValue(viewModelBinding.model, value, null);
                    }
                    break;
                case MemberTypes.Method:
                    MethodInfo methodInfo = type.GetMethod(memberName, bindingAttr);
                    if (methodInfo == null) {
                        Debug.LogWarning("Action event get method failed!" + memberName);
                    } else {
                        if (methodInfo.GetParameters().Length == 0) {
                            methodInfo.Invoke(viewModelBinding.model, null);
                        } else {
                            parameters[0] = value;
                            methodInfo.Invoke(viewModelBinding.model, parameters);
                        }
                    }
                    break;
                default:
                    Debug.LogWarningFormat("Action event call member function failed! unknown member type {0} {1}", memberType, memberName);
                    break;
            }
        }
    }
}