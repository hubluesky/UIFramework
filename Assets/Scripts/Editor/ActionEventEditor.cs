using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using VBM;
using VBM.Reflection;

namespace VBMEditor {
    [CustomEditor(typeof(ActionEvent), true)]
    public class ActionEventEditor : Editor {
        protected SerializedProperty bindingProperty;
        protected SerializedProperty memberNameProperty;
        protected ReorderableList memberDataList;

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
                ShowAddMemberMenu(rect, bindingProperty.objectReferenceValue as ViewModelBinding, (target as ActionEvent).ParameterType);
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

        protected virtual void AddMembers(GenericMenu menu, string modelId, System.Type paramType) {
            List<System.Type> modelTypeList = modelTypeList = ReflectionUtility.GetModelTypeList();
            System.Type type = modelTypeList.Find((t) => t.FullName == modelId);
            if (type == null) return;
            
            List<MemberInfo> memberInfoList = ReflectionMemberUtility.GetMembers(type);
            foreach (MemberInfo memberInfo in memberInfoList) {
                AddMemberMenu(menu, memberInfo, paramType);
            }
        }

        void ShowAddMemberMenu(Rect rect, ViewModelBinding viewmodelBinding, System.Type paramType) {
            GenericMenu menu = new GenericMenu();
            AddMembers(menu, viewmodelBinding.modelId, paramType);
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
            EditorGUI.BeginDisabledGroup(bindingProperty.objectReferenceValue == null);
            GUILayout.Button(bindingProperty.objectReferenceValue != null ? bindingProperty.displayName : "Please set view model binding");
            memberDataList.DoLayoutList();
            EditorGUI.EndDisabledGroup();
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }
    }
}