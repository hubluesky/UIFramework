using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using VBM;
using VBM.Reflection;

namespace VBMEditor {
    [CustomEditor(typeof(ViewModelBinding), true), CanEditMultipleObjects]
    public class ViewModelBindingEditor : Editor {
        public static readonly List<System.Type> modelTypeList;
        public static readonly string[] modelNames;

        private const string expandStatusText = " Click Expand";
        private const string collapseStatusText = " Click Collapse";
        private bool switchModelSelected;

        static ViewModelBindingEditor() {
            modelTypeList = ReflectionUtility.GetClassTypeFromAssembly(typeof(Model));
            modelNames = System.Array.ConvertAll(modelTypeList.ToArray(), (src) => src.FullName);
        }

        protected void DrawSelectedModel(SerializedProperty modelUniqueId) {
            int selected = System.Array.FindIndex(modelNames, (element) => { return element == modelUniqueId.stringValue; });
            int newSelected = EditorGUILayout.Popup(selected, modelNames);
            if (selected != newSelected) {
                modelUniqueId.stringValue = modelNames[newSelected];
            }
        }

        public override void OnInspectorGUI() {
            ViewModelBinding behavior = target as ViewModelBinding;
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();

            SerializedProperty parentBinding = serializedObject.FindProperty("parentBinding");
            EditorGUILayout.PropertyField(parentBinding);
            if (parentBinding.objectReferenceValue != null) {
                ViewModelBinding parent = parentBinding.objectReferenceValue as ViewModelBinding;
                if (!behavior.transform.IsChildOf(parent.transform))
                    EditorGUILayout.HelpBox("The object is not parent transform.", MessageType.Error);
            }

            EditorGUILayout.BeginHorizontal();
            SerializedProperty modelUniqueId = serializedObject.FindProperty("modelUniqueId");
            EditorGUILayout.PrefixLabel("Model");
            if (switchModelSelected) {
                EditorGUILayout.PropertyField(modelUniqueId, GUIContent.none, null);
            } else {
                DrawSelectedModel(modelUniqueId);
            }

            switchModelSelected = EditorGUILayout.Toggle(switchModelSelected, EditorStyles.radioButton, GUILayout.Width(15f));
            EditorGUILayout.EndHorizontal();

            SerializedProperty propertiesBinding = serializedObject.FindProperty("propertiesBinding");
            EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(modelUniqueId.stringValue));
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
            EditorGUI.indentLevel++;
            DrawPropertiesBinding(propertiesBinding);
            EditorGUI.indentLevel--;
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
            EditorGUI.EndDisabledGroup();

            if (EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void DrawPropertyBindingList(SerializedProperty bindingList) {
            Color backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.yellow;
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            GUI.backgroundColor = backgroundColor;
            string displayName = bindingList.displayName + (bindingList.isExpanded ? collapseStatusText : expandStatusText);
            bindingList.isExpanded = EditorGUILayout.Foldout(bindingList.isExpanded, displayName, true);
            if (GUILayout.Button(new GUIContent("Add", "Add Property Binding"), EditorStyles.miniButton, GUILayout.ExpandWidth(false))) {
                bindingList.InsertArrayElementAtIndex(bindingList.arraySize);
            }
            EditorGUILayout.EndHorizontal();
            if (!bindingList.isExpanded)
                return;

            for (int i = bindingList.arraySize - 1; i >= 0; i--) {
                SerializedProperty elementProperty = bindingList.GetArrayElementAtIndex(i);
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                Color contentColor = GUI.contentColor;
                GUI.contentColor = Color.cyan;
                EditorGUILayout.LabelField("Property Binding " + i, EditorStyles.boldLabel);
                if (GUILayout.Button(new GUIContent("X", "Remove Property Binding"), EditorStyles.miniButton, GUILayout.Width(25))) {
                    bindingList.DeleteArrayElementAtIndex(i);
                    break;
                }
                GUI.contentColor = contentColor;
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.PropertyField(elementProperty, true);
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawPropertiesBinding(SerializedProperty propertiesBinding) {
            propertiesBinding.NextVisible(true);
            do {
                DrawPropertyBindingList(propertiesBinding);
            } while (propertiesBinding.NextVisible(false));
        }
    }
}