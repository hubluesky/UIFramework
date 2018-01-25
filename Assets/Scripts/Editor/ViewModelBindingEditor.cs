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
        protected List<System.Type> modelTypeList;

        private const string expandStatusText = " Click Expand";
        private const string collapseStatusText = " Click Collapse";
        private bool switchModelSelected;

        public static ViewModelBindingEditor Instance { get; protected set; }

        protected void OnEnable() {
            Instance = this;
            modelTypeList = ReflectionUtility.GetModelTypeList();
        }

        public virtual int IndexOfModelProperty(string name) {
            return modelTypeList.FindIndex((element) => { return element.FullName == name; });
        }

        public virtual List<string> GetModelPropertyList(int selected) {
            List<string> propertyList = new List<string>();
            ReflectionUtility.ForeachGetClassProperty(modelTypeList[selected], (propertyInfo) => {
                if (propertyInfo.CanRead)
                    propertyList.Add(propertyInfo.Name);
            });
            return propertyList;
        }

        protected void DrawSelectedModel(SerializedProperty modelUniqueId) {
            if (GUILayout.Button(modelUniqueId.stringValue)) {
                ShowAddMemberMenu(modelUniqueId);
            }
        }

        protected void AddModelTypeMenus(SerializedProperty property, GenericMenu menu) {
            foreach (System.Type type in modelTypeList) {
                GUIContent content = new GUIContent(type.FullName);
                menu.AddItem(content, false, () => {
                    property.stringValue = type.FullName;
                    serializedObject.ApplyModifiedProperties();
                });
            }
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
                EditorGUILayout.PropertyField(modelUniqueId, GUIContent.none, null);
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
            if (GUILayout.Button(new GUIContent("Clear", "Remove all Property Binding"), EditorStyles.miniButton, GUILayout.ExpandWidth(false))) {
                bindingList.ClearArray();
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
                EditorGUILayout.PropertyField(elementProperty);
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
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
            propertiesBinding.Next(true);
            do {
                if (propertiesBinding.isArray && propertiesBinding.arraySize > 0)
                    DrawPropertyBindingList(propertiesBinding);
            } while (propertiesBinding.NextVisible(false));

            //
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add New Properties Binding Type", GUILayout.ExpandWidth(false))) {
                GenericMenu menu = new GenericMenu();
                propertiesBinding = serializedObject.FindProperty("propertiesBinding");
                propertiesBinding.Next(true);
                do {
                    if (propertiesBinding.isArray)
                        AddMenuItem(menu, propertiesBinding);
                } while (propertiesBinding.NextVisible(false));

                menu.ShowAsContext();
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
    }
}