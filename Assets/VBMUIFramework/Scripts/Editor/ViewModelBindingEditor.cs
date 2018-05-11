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
        private const string expandStatusText = " - Click Expand";
        private const string collapseStatusText = " - Click Collapse";
        private bool switchModelSelected;

        protected void DrawSelectedModel(SerializedProperty modelUniqueId) {
            if (GUILayout.Button(modelUniqueId.stringValue)) {
                ShowAddMemberMenu(modelUniqueId);
            }
        }

        protected void AddModelTypeMenus(SerializedProperty property, GenericMenu menu) {
            ModelReflection.instance.ForeachModelName((name)=>{
                GUIContent content = new GUIContent(name);
                menu.AddItem(content, name == property.stringValue, () => {
                    property.stringValue = name;
                    serializedObject.ApplyModifiedProperties();
                });
            });
        }

        protected virtual void ShowAddMemberMenu(SerializedProperty property) {
            GenericMenu menu = new GenericMenu();
            AddModelTypeMenus(property, menu);
            menu.ShowAsContext();
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
                if (parentBinding.objectReferenceValue == null) {
                    EditorGUILayout.PropertyField(modelUniqueId, GUIContent.none, null);
                } else if (GUILayout.Button(modelUniqueId.stringValue, EditorStyles.popup)) {
                    GenericMenu menu = new GenericMenu();
                    menu.AddDisabledItem(new GUIContent("None"));
                    ViewModelBinding parent = parentBinding.objectReferenceValue as ViewModelBinding;
                    int index = ModelReflection.instance.IndexOfModel(parent.modelId);
                    if (index != -1) {
                        ModelReflection.instance.ForeachProperty(index, (name, type)=>{
                            if (typeof(IModel).IsAssignableFrom(type)) {
                                menu.AddItem(new GUIContent(name), name == modelUniqueId.stringValue, () => {
                                    modelUniqueId.stringValue = name;
                                    modelUniqueId.serializedObject.ApplyModifiedProperties();
                                });
                            }
                        });
                    }
                    menu.ShowAsContext();
                }
            } else {
                if (GUILayout.Button(modelUniqueId.stringValue))
                    ShowAddMemberMenu(modelUniqueId);
            }

            switchModelSelected = EditorGUILayout.Toggle(switchModelSelected, EditorStyles.radioButton, GUILayout.Width(15f));
            EditorGUILayout.EndHorizontal();

            SerializedProperty propertiesBinding = serializedObject.FindProperty("propertiesBinding");
            EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(modelUniqueId.stringValue));
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
            EditorGUI.indentLevel++;
            DrawPropertiesBinding(propertiesBinding);
            EditorGUI.indentLevel--;
            EditorGUI.EndDisabledGroup();

            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }

        private void DrawPropertyBindingList(SerializedProperty bindingList) {
            Color backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.yellow;
            EditorGUILayout.BeginHorizontal("box");
            GUI.backgroundColor = backgroundColor;

            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            string displayName = bindingList.displayName + (bindingList.isExpanded ? collapseStatusText : expandStatusText);
            bindingList.isExpanded = EditorGUILayout.Foldout(bindingList.isExpanded, displayName, true);
            if (GUILayout.Button(new GUIContent("Add", "Add Property Binding"), EditorStyles.miniButton, GUILayout.ExpandWidth(false))) {
                bindingList.InsertArrayElementAtIndex(bindingList.arraySize);
            }
            if (GUILayout.Button(new GUIContent("Clear", "Remove all Property Binding"), EditorStyles.miniButton, GUILayout.ExpandWidth(false))) {
                bindingList.ClearArray();
            }
            EditorGUILayout.EndHorizontal();

            if (bindingList.isExpanded) {
                for (int i = 0; i < bindingList.arraySize; i++) {
                    SerializedProperty elementProperty = bindingList.GetArrayElementAtIndex(i);
                    EditorGUILayout.BeginHorizontal("box");
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
                    ChildPropertyField(elementProperty);
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();
                }

            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }

        private void ChildPropertyField(SerializedProperty property) {
            int depth = property.depth + 1;
            foreach (SerializedProperty childProperty in property) {
                if (childProperty.depth > depth) continue;
                EditorGUILayout.PropertyField(childProperty, true);
            }
        }

        private void AddMenuItem(GenericMenu menu, SerializedProperty bindingList) {
            if (bindingList.arraySize == 0) {
                menu.AddItem(new GUIContent(bindingList.displayName), false, (propertyPath) => {
                    SerializedProperty property = serializedObject.FindProperty(propertyPath.ToString());
                    property.InsertArrayElementAtIndex(property.arraySize);
                    serializedObject.ApplyModifiedProperties();
                }, bindingList.propertyPath);
            } else {
                menu.AddDisabledItem(new GUIContent(bindingList.displayName));
            }
        }

        private void DrawPropertiesBinding(SerializedProperty propertiesBinding) {
            //
            int depth = propertiesBinding.depth + 1;
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add New Properties Binding Type", GUILayout.ExpandWidth(false))) {
                GenericMenu menu = new GenericMenu();

                foreach (SerializedProperty childProperty in propertiesBinding) {
                    if (childProperty.isArray && depth == childProperty.depth)
                        AddMenuItem(menu, childProperty);
                }

                menu.ShowAsContext();
                return;
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            foreach (SerializedProperty childProperty in propertiesBinding) {
                if (childProperty.isArray && childProperty.arraySize > 0 && depth == childProperty.depth)
                    DrawPropertyBindingList(childProperty);
            }
        }
    }
}