
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using VBM;

namespace VBMEditor {
    [CustomEditor(typeof(ActionEvent), true)]
    public class ActionEventEditor : Editor {
        SerializedProperty bindingProperty;
        SerializedProperty memberNameProperty;
        ReorderableList memberDataList;
        System.Type modelType;

        protected virtual void OnEnable() {
            bindingProperty = serializedObject.FindProperty("viewModelBinding");
            SerializedProperty memberNameProperty = serializedObject.FindProperty("memberNameArray");
            memberDataList = new ReorderableList(serializedObject, memberNameProperty, true, true, true, true);
            memberDataList.drawHeaderCallback += DrawMemberHeader;
            memberDataList.drawElementCallback += DrawMemberDataListElement;
        }

        void DrawMemberHeader(Rect rect) {
            System.Type type = (target as ActionEvent).ParameterType;
            if (type == null)
                EditorGUI.LabelField(rect, "Action Event");
            else
                EditorGUI.LabelField(rect, "Action Event (" + type.Name + ")");
        }

        void DrawMemberDataListElement(Rect rect, int index, bool isActive, bool isFocused) {
            SerializedProperty child = memberDataList.serializedProperty.GetArrayElementAtIndex(index);

            if (GUI.Button(rect, child.stringValue, EditorStyles.popup)) {
                this.memberNameProperty = child;
                ShowAddMemberMenu(rect, modelType, (target as ActionEvent).ParameterType);
            }
        }

        void AddMemberMenu(GenericMenu menu, MemberInfo memberInfo, System.Type paramType) {
            GUIContent content = new GUIContent(ReflectionMemberUtility.FormatMemberName(memberInfo));
            if (paramType == null) {
                if (memberInfo.MemberType == MemberTypes.Method) {
                    MethodInfo methodInfo = memberInfo as MethodInfo;
                    if (methodInfo.GetParameters().Length == 0) {
                        menu.AddItem(content, false, OnAddMemberBinding, memberInfo);
                        return;
                    }
                }
            } else {
                switch (memberInfo.MemberType) {
                    case MemberTypes.Field:
                        FieldInfo fieldInfo = memberInfo as FieldInfo;
                        if (paramType.IsAssignableFrom(fieldInfo.FieldType)) {
                            menu.AddItem(content, false, OnAddMemberBinding, memberInfo);
                            return;
                        }
                        break;
                    case MemberTypes.Property:
                        PropertyInfo propertyInfo = memberInfo as PropertyInfo;
                        if (paramType.IsAssignableFrom(propertyInfo.PropertyType)) {
                            menu.AddItem(content, false, OnAddMemberBinding, memberInfo);
                            return;
                        }
                        break;
                    case MemberTypes.Method:
                        MethodInfo methodInfo = memberInfo as MethodInfo;
                        ParameterInfo[] parameters = methodInfo.GetParameters();
                        if (parameters.Length == 0 || paramType.IsAssignableFrom(parameters[0].GetType())) {
                            menu.AddItem(content, false, OnAddMemberBinding, memberInfo);
                            return;
                        }
                        break;
                }

            }
            menu.AddDisabledItem(content);
        }

        void ShowAddMemberMenu(Rect rect, System.Type type, System.Type paramType) {
            List<MemberInfo> memberInfoList = ReflectionMemberUtility.GetMembers(type);
            GenericMenu menu = new GenericMenu();
            foreach (MemberInfo memberInfo in memberInfoList) {
                AddMemberMenu(menu, memberInfo, paramType);
            }
            menu.DropDown(rect);
        }

        private void OnAddMemberBinding(object value) {
            MemberInfo memberInfo = value as MemberInfo;
            memberNameProperty.stringValue = memberInfo.Name;
            memberNameProperty.serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(bindingProperty);
            if (bindingProperty.objectReferenceValue != null) {
                ViewModelBinding viewModelBinding = bindingProperty.objectReferenceValue as ViewModelBinding;
                modelType = ViewModelBindingEditor.modelTypeList.Find((element) => { return viewModelBinding.modelId == element.FullName; });
            } else {
                modelType = null;
            }
            EditorGUI.BeginDisabledGroup(modelType == null);
            GUILayout.Button(modelType != null ? modelType.FullName : "Please set view model binding");
            memberDataList.DoLayoutList();
            EditorGUI.EndDisabledGroup();
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }
    }
}