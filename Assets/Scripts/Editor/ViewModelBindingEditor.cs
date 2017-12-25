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
        private const string expandStatusText = "Click Expand";
        private const string collapseStatusText = "Click Collapse";
        private bool switchModelSelected;

        protected void DrawSelectedModel(SerializedProperty modelUniqueId) {
            List<System.Type> list = ReflectionUtility.GetClassTypeFromAssembly(typeof(Model));
            string[] array = System.Array.ConvertAll(list.ToArray(), (src) => src.FullName);
            int selected = System.Array.FindIndex(array, (element) => { return element == modelUniqueId.stringValue; });
            int newSelected = EditorGUILayout.Popup(selected, array);
            if (selected != newSelected) {
                modelUniqueId.stringValue = array[newSelected];
                modelUniqueId.serializedObject.ApplyModifiedProperties();
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

        static void SwapListItem<T>(List<T> list, int index1, int index2) {
            var temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }

        private void DrawPropertyBindingList(SerializedProperty bindingList) {
            Color backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.yellow;
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            GUI.backgroundColor = backgroundColor;
            bindingList.isExpanded = EditorGUILayout.Foldout(bindingList.isExpanded, bindingList.displayName, true);
            bindingList.isExpanded = EditorGUILayout.Foldout(bindingList.isExpanded, bindingList.isExpanded ? collapseStatusText : expandStatusText, true, EditorStyles.label);
            if (GUILayout.Button(new GUIContent("Add", "Add Property Binding"), EditorStyles.miniButton, GUILayout.ExpandWidth(false))) {
                bindingList.InsertArrayElementAtIndex(bindingList.arraySize);
            }
            EditorGUILayout.EndHorizontal();
            if (!bindingList.isExpanded)
                return;

            for (int i = 0; i < bindingList.arraySize; i++) {
                SerializedProperty elementProperty = bindingList.GetArrayElementAtIndex(i);
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                Color contentColor = GUI.contentColor;
                GUI.contentColor = Color.cyan;
                string name = elementProperty.objectReferenceValue != null ? elementProperty.objectReferenceValue.GetType().Name : "Unknown";
                EditorGUILayout.LabelField(name, EditorStyles.boldLabel);
                if (GUILayout.Button(new GUIContent("X", "Remove Property Binding"), EditorStyles.miniButton, GUILayout.Width(25))) {
                    SerializedProperty childProperty = bindingList.GetArrayElementAtIndex(i);
                    Object.DestroyImmediate(childProperty.objectReferenceValue);
                    bindingList.DeleteArrayElementAtIndex(i);
                    break;
                }
                GUI.contentColor = contentColor;
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.PropertyField(elementProperty);
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawPropertiesBinding(SerializedProperty propertiesBinding) {
            while (propertiesBinding.NextVisible(true)) {
                if (propertiesBinding.isArray) {
                    DrawPropertyBindingList(propertiesBinding);
                }
            }
        }
    }
}