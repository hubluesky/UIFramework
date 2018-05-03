using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using VBM;
using VBM.Reflection;

namespace VBMEditor {
    [CustomEditor(typeof(ActionEvent), true), CanEditMultipleObjects]
    public class ActionEventEditor : Editor {
        protected SerializedProperty bindingProperty;
        protected SerializedProperty methodProperty;
        protected ReorderableList memberDataList;

        protected virtual void OnEnable() {
            bindingProperty = serializedObject.FindProperty("viewModelBinding");
            if (bindingProperty.objectReferenceValue == null) {
                AutoFindViewModelBinding(bindingProperty);
            }

            SerializedProperty methodArrayProperty = serializedObject.FindProperty("methodArray");
            memberDataList = new ReorderableList(serializedObject, methodArrayProperty, true, true, true, true);
            memberDataList.drawHeaderCallback += DrawMemberHeader;
            memberDataList.drawElementCallback += DrawMemberDataListElement;
        }

        protected virtual void AutoFindViewModelBinding(SerializedProperty bindingProperty) {
            ActionEvent actionEvent = serializedObject.targetObject as ActionEvent;
            bindingProperty.objectReferenceValue = actionEvent.GetComponentInParent<ViewModelBinding>();
            serializedObject.ApplyModifiedProperties();
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
            EditorGUI.PropertyField(rect, child);
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