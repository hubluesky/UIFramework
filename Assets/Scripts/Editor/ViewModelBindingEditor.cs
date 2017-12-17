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
        private bool switchModelSelected;

        protected void DrawSelectedModel(SerializedProperty modelUniqueId, SerializedProperty propertyBindingList) {
            List<System.Type> list = ReflectionUtility.GetClassTypeFromAssembly(typeof(Model));
            string[] array = System.Array.ConvertAll(list.ToArray(), (src) => src.FullName);
            int selected = System.Array.FindIndex(array, (element) => { return element == modelUniqueId.stringValue; });
            int newSelected = EditorGUILayout.Popup(selected, array);
            if (selected != newSelected) {
                modelUniqueId.stringValue = array[newSelected];
                propertyBindingList.ClearArray();
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
            SerializedProperty propertyList = serializedObject.FindProperty("propertyBindingList");
            EditorGUILayout.PrefixLabel("Model");
            if (switchModelSelected) {
                EditorGUILayout.PropertyField(modelUniqueId, GUIContent.none, null);
            } else {
                DrawSelectedModel(modelUniqueId, propertyList);
            }

            switchModelSelected = EditorGUILayout.Toggle(switchModelSelected, EditorStyles.radioButton, GUILayout.Width(15f));
            EditorGUILayout.EndHorizontal();

            EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(modelUniqueId.stringValue));
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
            EditorGUILayout.LabelField("Model Property Binding List", EditorStyles.toolbarButton);
            DrawPropertyBindingList(propertyList);
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
            EditorGUI.EndDisabledGroup();

            if (EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void DrawPropertyBindingList(SerializedProperty serializedPropertyList) {
            int removeIndex = -1;
            for (int i = 0; i < serializedPropertyList.arraySize; i++) {
                SerializedProperty childProperty = serializedPropertyList.GetArrayElementAtIndex(i);
                string name = childProperty.objectReferenceValue != null ? childProperty.objectReferenceValue.GetType().Name : "Unknown";
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.BeginHorizontal();
                Color oldColor = GUI.backgroundColor;
                GUI.backgroundColor = Color.green;
                childProperty.isExpanded = EditorGUILayout.Foldout(childProperty.isExpanded, name, true);
                GUI.backgroundColor = oldColor;

                GUI.color = Color.green;
                EditorGUI.BeginDisabledGroup(i <= 0);
                if (GUILayout.Button(new GUIContent("↑", "Move up"), EditorStyles.miniButtonLeft, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false))) {
                    serializedPropertyList.MoveArrayElement(i, i - 1);
                }
                EditorGUI.EndDisabledGroup();

                EditorGUI.BeginDisabledGroup(i + 1 >= serializedPropertyList.arraySize);
                if (GUILayout.Button(new GUIContent("↓", "Move down"), EditorStyles.miniButtonMid, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false))) {
                    serializedPropertyList.MoveArrayElement(i, i + 1);
                }
                EditorGUI.EndDisabledGroup();

                GUI.color = Color.red;
                if (GUILayout.Button(new GUIContent("×", "Delete"), EditorStyles.miniButtonRight, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false))) {
                    removeIndex = i;
                }
                GUI.color = Color.white;
                EditorGUILayout.EndHorizontal();
                if (childProperty.isExpanded) {
                    EditorGUILayout.PropertyField(childProperty);
                }
                EditorGUILayout.EndVertical();
                EditorGUI.indentLevel--;
            }

            if (removeIndex != -1) {
                serializedPropertyList.DeleteArrayElementAtIndex(removeIndex);
                serializedPropertyList.serializedObject.ApplyModifiedProperties();
            }

            if (GUILayout.Button("Add Property Binding")) {
                GenericMenu menu = new GenericMenu();
                List<System.Type> list = ReflectionUtility.GetClassTypeFromAssembly(typeof(PropertyBinding));
                foreach (System.Type type in list) {
                    menu.AddItem(new GUIContent(type.FullName), false, () => {
                        serializedPropertyList.InsertArrayElementAtIndex(serializedPropertyList.arraySize);
                        SerializedProperty childProperty = serializedPropertyList.GetArrayElementAtIndex(serializedPropertyList.arraySize - 1);
                        childProperty.objectReferenceValue = ScriptableObject.CreateInstance(type);
                        serializedObject.ApplyModifiedProperties();
                    });
                }
                menu.ShowAsContext();
            }
        }
    }
}