using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace VBM {
    public static class ReflectionMemberUtility {
        public const BindingFlags bindingAttr = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public;
        private static object[] parameters = new object[1];
        private static Dictionary<System.Type, List<MemberInfo>> memberInfoMap = new Dictionary<System.Type, List<MemberInfo>>();

        public static string FormatMemberName(MemberInfo memberInfo) {
            switch (memberInfo.MemberType) {
                case MemberTypes.Field:
                    FieldInfo fieldInfo = memberInfo as FieldInfo;
                    return string.Format("{0}{1} {2}", fieldInfo.IsStatic ? "static " : "", fieldInfo.FieldType.Name, memberInfo.Name);
                case MemberTypes.Property:
                    PropertyInfo propertyInfo = memberInfo as PropertyInfo;
                    return string.Format("{0} {1}", propertyInfo.PropertyType.Name, memberInfo.Name);
                case MemberTypes.Method:
                    MethodInfo methodInfo = memberInfo as MethodInfo;
                    ParameterInfo[] parameters = methodInfo.GetParameters();
                    return string.Format("{0}{1}({2})", methodInfo.IsStatic ? "static " : "", memberInfo.Name, parameters.Length > 0 ? parameters[0].ParameterType.Name : "");
                default:
                    return null;
            }
        }

        public static List<MemberInfo> GetMembers(System.Type type) {
            List<MemberInfo> memberList;
            if (!memberInfoMap.TryGetValue(type, out memberList)) {
                memberList = ReflectionMembers(type);
                memberInfoMap.Add(type, memberList);
            }
            return memberList;
        }

        public static List<MemberInfo> ReflectionMembers(System.Type type) {
            List<MemberInfo> memberList = new List<MemberInfo>();
            MemberInfo[] memberInfos = type.GetMembers(bindingAttr);
            foreach (MemberInfo memberInfo in memberInfos) {
                switch (memberInfo.MemberType) {
                    case MemberTypes.Field:
                        FieldInfo fieldInfo = memberInfo as FieldInfo;
                        if (!fieldInfo.IsInitOnly)
                            memberList.Add(memberInfo);
                        break;
                    case MemberTypes.Property:
                        PropertyInfo propertyInfo = memberInfo as PropertyInfo;
                        if (propertyInfo.CanWrite)
                            memberList.Add(memberInfo);
                        break;
                    case MemberTypes.Method:
                        MethodInfo methodInfo = memberInfo as MethodInfo;
                        if (!methodInfo.IsSpecialName && !methodInfo.IsGenericMethod && !methodInfo.IsConstructor && methodInfo.ReturnType == typeof(void)) {
                            System.Reflection.ParameterInfo[] parameters = methodInfo.GetParameters();
                            if (parameters.Length > 1) continue;
                            if (parameters.Length == 1 && (parameters[0].IsIn || parameters[0].IsOut)) continue;
                            if (!methodInfo.IsSpecialName && methodInfo.GetParameters().Length <= 1)
                                memberList.Add(memberInfo);
                        }
                        break;
                }
            }
            return memberList;
        }
        public static void CallMemberFunction(object obj, string memberName) {
            System.Type type = obj.GetType();
            MethodInfo methodInfo = type.GetMethod(memberName, bindingAttr);
            if (methodInfo == null) {
                Debug.LogWarning("Action event get method failed!" + memberName);
            } else {
                methodInfo.Invoke(obj, null);
            }
        }

        public static void CallMemberFunction<T>(object obj, string memberName, T value) {
            System.Type type = obj.GetType();
            MemberInfo[] memberInfo = type.GetMember(memberName, bindingAttr);
            if (memberInfo == null || memberInfo.Length == 0) {
                Debug.LogWarning("Action event get member failed!" + memberName);
                return;
            }
            switch (memberInfo[0].MemberType) {
                case MemberTypes.Field:
                    FieldInfo fieldInfo = memberInfo[0] as FieldInfo;
                    if (fieldInfo == null) {
                        Debug.LogWarning("Action event get field failed!" + memberName);
                    } else {
                        fieldInfo.SetValue(obj, value);
                    }
                    break;
                case MemberTypes.Property:
                    PropertyInfo propertyInfo = memberInfo[0] as PropertyInfo;
                    if (propertyInfo == null) {
                        Debug.LogWarning("Action event get property failed!" + memberName);
                    } else {
                        propertyInfo.SetValue(obj, value, null);
                    }
                    break;
                case MemberTypes.Method:
                    MethodInfo methodInfo = memberInfo[0] as MethodInfo;
                    if (methodInfo == null) {
                        Debug.LogWarning("Action event get method failed!" + memberName);
                    } else {
                        if (methodInfo.GetParameters().Length == 0) {
                            methodInfo.Invoke(obj, null);
                        } else {
                            parameters[0] = value;
                            methodInfo.Invoke(obj, parameters);
                        }
                    }
                    break;
                default:
                    Debug.LogWarningFormat("Action event call member function failed! unknown member type {0} {1}", memberInfo[0].MemberType, memberName);
                    break;
            }
        }
    }
}