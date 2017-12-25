using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GeneralEditor {
    public class ArrayPropertyDrawer : GeneralEditor.PropertyDrawer {

        public override void OnGUI(GeneralEditor.SerializedProperty property, GUIContent label, params GUILayoutOption[] options) {
            if (property.PropertyValue == null)
                property.CreatePropertyValue();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal(options);
            property.IsExpanded = EditorGUILayout.Foldout(property.IsExpanded, label);
            if (GUILayout.Button(new GUIContent("Add", "Add Element"), EditorStyles.miniButton, GUILayout.ExpandWidth(false))) {
                OnAddButton(property);
            }

            EditorGUI.BeginDisabledGroup(property.ArraySize == 0);
            if (GUILayout.Button(new GUIContent("Remove", "Remove Last Element"), EditorStyles.miniButton, GUILayout.ExpandWidth(false))) {
                OnAddButton(property);
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            if (property.IsExpanded) {
                for (int i = 0; i < property.ArraySize; i++) {
                    SerializedProperty elementProperty = property.GetArrayElementAtIndex(i);
                    EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                    GUILayout.Space(10.0f);
                    PropertyDrawerMgr.PropertyField(elementProperty, new GUIContent("Element " + i));
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
        }

        protected virtual void OnAddButton(GeneralEditor.SerializedProperty property) {
            property.CreateArrayElementAtIndex(property.ArraySize, null);
        }

        protected virtual void OnRemoveButton(GeneralEditor.SerializedProperty property) {
            property.DeleteArrayElementAtIndex(property.ArraySize - 1);
        }
    }
}