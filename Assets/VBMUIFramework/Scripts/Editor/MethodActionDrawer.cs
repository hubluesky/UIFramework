using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using VBM;
using VBM.Reflection;

namespace VBMEditor {
    public class MethodListOption {
        public List<MethodInfo> methodInfoList;
        public string[] methodInfoName;
    }

    [CustomPropertyDrawer(typeof(MethodActionAttribute), true)]
    public class MethodActionDrawer : PropertyDrawer {
        public const float MethodGenericWidth = 60.0f;
        public static Dictionary<Attribute, MethodListOption> methodOptionMap = new Dictionary<Attribute, MethodListOption>();
        public static List<Type> methodGenericTypeList = new List<Type>();
        public static string[] methodGenericNames;

        static MethodActionDrawer() {
            methodGenericTypeList.Add(MethodParameter.BoolType);
            methodGenericTypeList.Add(MethodParameter.IntType);
            methodGenericTypeList.Add(MethodParameter.FloatType);
            methodGenericTypeList.Add(MethodParameter.StringType);
            methodGenericTypeList.Add(MethodParameter.EnumType);
            methodGenericTypeList.Add(MethodParameter.UnityObjectType);
            methodGenericTypeList.Add(MethodParameter.ColorType);
            methodGenericNames = new string[methodGenericTypeList.Count];
            for (int i = 0; i < methodGenericNames.Length; i++)
                methodGenericNames[i] = methodGenericTypeList[i].Name;
        }

        public static int FindMethodInfoIndex(MethodListOption option, string targetType, string methodName) {
            for (int i = 1; i < option.methodInfoList.Count; i++) {
                MethodInfo methodInfo = option.methodInfoList[i];
                if (methodInfo.ReflectedType.AssemblyQualifiedName == targetType && methodInfo.Name == methodName)
                    return i;
            }
            return 0;
        }

        public static MethodListOption CreateMethodListOption(Type attributeType) {
            MethodListOption methodListOption = new MethodListOption();
            methodListOption.methodInfoList = new List<MethodInfo>();
            methodListOption.methodInfoList.Add(null);
            ReflectionUtility.ForeachClassTypeFromAssembly((type) => {
                MethodInfo[] methodInfos = type.GetMethods(BindingFlags.Static | BindingFlags.Public);
                foreach (MethodInfo methodInfo in methodInfos) {
                    if (methodInfo.IsDefined(attributeType, false) && !methodInfo.IsGenericMethod && methodInfo.GetParameters().Length <= MethodAction.MaxParameterCount)
                        methodListOption.methodInfoList.Add(methodInfo);
                }
                return true;
            });

            methodListOption.methodInfoName = new string[methodListOption.methodInfoList.Count];
            methodListOption.methodInfoName[0] = "None";
            for (int i = 1; i < methodListOption.methodInfoList.Count; i++)
                methodListOption.methodInfoName[i] = FormatMethodName(methodListOption.methodInfoList[i]);
            return methodListOption;
        }

        public static MethodListOption GetOrCreateOptionFromCache(Attribute attribute, Type defineType) {
            MethodListOption option;
            if (!methodOptionMap.TryGetValue(attribute, out option)) {
                option = CreateMethodListOption(defineType);
                methodOptionMap.Add(attribute, option);
            }
            return option;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            SerializedProperty parametersProperty = property.FindPropertyRelative("parameters");
            float lineNumber = parametersProperty.arraySize + 1;
            return lineNumber * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            MethodActionAttribute methodActionAttribute = attribute as MethodActionAttribute;
            if (methodActionAttribute == null) {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            EditorGUI.BeginProperty(position, label, property);

            MethodListOption option = GetOrCreateOptionFromCache(methodActionAttribute, methodActionAttribute.attributeType);

            SerializedProperty targetTypeProperty = property.FindPropertyRelative("targetType");
            SerializedProperty methodNameProperty = property.FindPropertyRelative("methodName");
            SerializedProperty parametersProperty = property.FindPropertyRelative("parameters");
            SerializedProperty bindingFlagsProperty = property.FindPropertyRelative("bindingFlags");

            if (bindingFlagsProperty.intValue != (int) methodActionAttribute.bindingFlags)
                bindingFlagsProperty.intValue = (int) methodActionAttribute.bindingFlags;

            if (!string.IsNullOrEmpty(methodNameProperty.stringValue))
                GUI.Box(position, "");

            int index = FindMethodInfoIndex(option, targetTypeProperty.stringValue, methodNameProperty.stringValue);
            EditorGUI.BeginChangeCheck();

            Rect rectMethod = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            index = EditorGUI.Popup(rectMethod, label.text, index, option.methodInfoName);

            if (EditorGUI.EndChangeCheck()) {
                if (index == 0) {
                    targetTypeProperty.stringValue = null;
                    methodNameProperty.stringValue = null;
                    parametersProperty.arraySize = 0;
                    parametersProperty.serializedObject.ApplyModifiedProperties();
                } else {
                    targetTypeProperty.stringValue = option.methodInfoList[index].ReflectedType.AssemblyQualifiedName;
                    methodNameProperty.stringValue = option.methodInfoList[index].Name;
                    parametersProperty.ClearArray();
                    parametersProperty.arraySize = option.methodInfoList[index].GetParameters().Length;
                    parametersProperty.serializedObject.ApplyModifiedProperties();
                }
            }

            ParameterInfo[] parameterInfos = index > 0 ? option.methodInfoList[index].GetParameters() : null;
            EditorGUI.indentLevel++;
            if (parameterInfos != null && parameterInfos.Length == parametersProperty.arraySize) {
                for (int i = 0; i < parametersProperty.arraySize; i++) {
                    SerializedProperty elementProperty = parametersProperty.GetArrayElementAtIndex(i);
                    position = NextLieRect(position);
                    PropertyBindingNameDrawer.DrawMethodParameterPassing(position, elementProperty, new GUIContent("Parameter " + (i + 1)), parameterInfos[i].ParameterType);
                }
            }
            EditorGUI.indentLevel--;

            EditorGUI.EndProperty();
        }

        public static Rect NextLieRect(Rect rect) {
            return new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing, rect.width, EditorGUIUtility.singleLineHeight);
        }

        public static string FormatMethodName(MethodInfo methodInfo) {
            StringBuilder builder = new StringBuilder();
            builder.Append(methodInfo.ReturnType.Name + ' ');
            builder.Append(methodInfo.ReflectedType.Name);
            builder.Append('.');
            builder.Append(methodInfo.Name);
            builder.Append('(');
            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
            for (int i = 0; i < parameterInfos.Length; i++) {
                if (i > 0)
                    builder.Append(", ");
                builder.Append(parameterInfos[i].ParameterType.Name);
            }
            builder.Append(')');
            return builder.ToString();
        }
    }
}