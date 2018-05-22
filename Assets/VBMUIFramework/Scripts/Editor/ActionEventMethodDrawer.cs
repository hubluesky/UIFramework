using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using VBM;
using VBM.Reflection;

namespace VBMEditor {
    [CustomPropertyDrawer(typeof(ActionEventMethod), true)]
    public class ActionEventMethodDrawer : PropertyDrawer {
        struct MenuContext {
            public MenuContext(SerializedProperty property, MemberInfo memberInfo, System.Type parameterType = null) {
                this.property = property;
                this.memberInfo = memberInfo;
                this.parameterType = parameterType;
            }

            public readonly SerializedProperty property;
            public readonly MemberInfo memberInfo;
            public readonly System.Type parameterType;
        }

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {
            SerializedProperty methodNameProperty = property.FindPropertyRelative("methodName");
            SerializedProperty parameterTypeProperty = property.FindPropertyRelative("parameterTypeName");

            Rect methodRect = rect;
            if (!string.IsNullOrEmpty(parameterTypeProperty.stringValue)) {
                System.Type parameterType = System.Type.GetType(parameterTypeProperty.stringValue);
                if (parameterType != null) {
                    SerializedProperty parameterProperty = property.FindPropertyRelative("parameter");
                    float space = 5;
                    methodRect = new Rect(rect.x, rect.y, rect.width * 0.6f, rect.height);
                    Rect paramRect = new Rect(rect.x + methodRect.width + space, rect.y, rect.width - methodRect.width - space, rect.height);
                    if (parameterType == typeof(bool)) {
                        SerializedProperty paramChildProperty = parameterProperty.FindPropertyRelative("boolParam");
                        EditorGUI.PropertyField(paramRect, paramChildProperty, GUIContent.none);
                    } else if (parameterType == typeof(int)) {
                        SerializedProperty paramChildProperty = parameterProperty.FindPropertyRelative("intParam");
                        EditorGUI.PropertyField(paramRect, paramChildProperty, GUIContent.none);
                    } else if (parameterType == typeof(float)) {
                        SerializedProperty paramChildProperty = parameterProperty.FindPropertyRelative("floatParam");
                        EditorGUI.PropertyField(paramRect, paramChildProperty, GUIContent.none);
                    } else if (parameterType == typeof(string)) {
                        SerializedProperty paramChildProperty = parameterProperty.FindPropertyRelative("stringParam");
                        EditorGUI.PropertyField(paramRect, paramChildProperty, GUIContent.none);
                    } else if (typeof(System.Enum).IsAssignableFrom(parameterType)) {
                        EditorGUI.BeginChangeCheck();
                        SerializedProperty paramChildProperty = parameterProperty.FindPropertyRelative("intParam");
                        System.Enum value = System.Enum.ToObject(parameterType, paramChildProperty.intValue) as System.Enum;
                        if (value == null)
                            value = System.Enum.GetValues(parameterType).GetValue(0) as System.Enum;
                        value = EditorGUI.EnumPopup(paramRect, value);
                        if (EditorGUI.EndChangeCheck())
                            paramChildProperty.intValue = System.Convert.ToInt32(value);
                    } else if (typeof(Object).IsAssignableFrom(parameterType)) {
                        SerializedProperty paramChildProperty = parameterProperty.FindPropertyRelative("objectParam");
                        paramChildProperty.objectReferenceValue = EditorGUI.ObjectField(paramRect, paramChildProperty.objectReferenceValue, parameterType, true);
                    } else {
                        EditorGUI.HelpBox(paramRect, "Unkonwn Parameter Type", MessageType.Warning);
                    }
                }
            }

            if (GUI.Button(methodRect, methodNameProperty.stringValue, EditorStyles.popup)) {
                ActionEvent actionEvent = property.serializedObject.targetObject as ActionEvent;
                SerializedProperty bindingProperty = property.serializedObject.FindProperty("viewModelBinding");
                ShowAddMemberMenu(property, methodRect, bindingProperty.objectReferenceValue as ViewModelBinding, actionEvent.ParameterType);
            }

        }

        void ShowAddMemberMenu(SerializedProperty property, Rect rect, ViewModelBinding viewmodelBinding, System.Type paramType) {
            GenericMenu menu = new GenericMenu();
            AddMembers(property, menu, viewmodelBinding.modelId, paramType);
            menu.DropDown(rect);
        }

        protected virtual void AddMembers(SerializedProperty property, GenericMenu menu, string modelId, System.Type paramType) {
            List<System.Type> modelTypeList = ReflectionUtility.GetModelTypeList();
            System.Type type = modelTypeList.Find((t) => t.FullName == modelId);
            if (type == null) return;

            List<MemberInfo> memberInfoList = ReflectionMemberUtility.GetMembers(type);
            foreach (MemberInfo memberInfo in memberInfoList) {
                AddMemberMenu(property, menu, memberInfo, modelId, paramType);
            }
        }

        protected void AddMemberMenu(SerializedProperty property, GenericMenu menu, MemberInfo memberInfo, string modelId, System.Type paramType) {
            GUIContent content = new GUIContent(ReflectionMemberUtility.FormatMemberName(memberInfo));
            if (paramType == null) {
                if (memberInfo.MemberType == MemberTypes.Method) {
                    MethodInfo methodInfo = memberInfo as MethodInfo;
                    if (methodInfo.GetParameters().Length == 0) {
                        menu.AddItem(content, false, OnAddMemberBinding, new MenuContext(property, memberInfo));
                        return;
                    } else if (methodInfo.GetParameters().Length == 1) {
                        menu.AddItem(content, false, OnAddMemberBinding, new MenuContext(property, memberInfo, methodInfo.GetParameters()[0].ParameterType));
                        return;
                    }
                }
            } else {
                switch (memberInfo.MemberType) {
                    case MemberTypes.Field:
                        FieldInfo fieldInfo = memberInfo as FieldInfo;
                        if (paramType.IsAssignableFrom(fieldInfo.FieldType)) {
                            menu.AddItem(content, false, OnAddMemberBinding, new MenuContext(property, memberInfo));
                            return;
                        }
                        break;
                    case MemberTypes.Property:
                        PropertyInfo propertyInfo = memberInfo as PropertyInfo;
                        if (paramType.IsAssignableFrom(propertyInfo.PropertyType)) {
                            menu.AddItem(content, false, OnAddMemberBinding, new MenuContext(property, memberInfo));
                            return;
                        }
                        break;
                    case MemberTypes.Method:
                        MethodInfo methodInfo = memberInfo as MethodInfo;
                        ParameterInfo[] parameters = methodInfo.GetParameters();
                        if (parameters.Length == 0) {
                            menu.AddItem(content, false, OnAddMemberBinding, new MenuContext(property, memberInfo));
                            return;
                        } else if (parameters.Length == 1) {
                            if (paramType.IsAssignableFrom(parameters[0].ParameterType)) {
                                menu.AddItem(new GUIContent(content.text + " - TP"), false, OnAddMemberBinding, new MenuContext(property, memberInfo));
                                menu.AddItem(content, false, OnAddMemberBinding, new MenuContext(property, memberInfo, parameters[0].ParameterType));
                                return;
                            } else {
                                menu.AddItem(content, false, OnAddMemberBinding, new MenuContext(property, memberInfo, parameters[0].ParameterType));
                                return;
                            }
                        }
                        break;
                }

            }
            menu.AddDisabledItem(content);
        }

        void OnAddMemberBinding(object value) {
            MenuContext context = (MenuContext)value;
            SerializedProperty methodName = context.property.FindPropertyRelative("methodName");
            SerializedProperty parameterType = context.property.FindPropertyRelative("parameterTypeName");
            methodName.stringValue = context.memberInfo.Name;
            parameterType.stringValue = context.parameterType != null ? context.parameterType.AssemblyQualifiedName : null;
            context.property.serializedObject.ApplyModifiedProperties();
        }
    }
}