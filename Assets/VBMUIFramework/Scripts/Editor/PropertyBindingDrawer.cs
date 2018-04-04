using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VBM;
using VBM.Reflection;

namespace VBMEditor {
    [CustomPropertyDrawer(typeof(PropertyBinding), true)]
    public class PropertyBindingDrawer : PropertyDrawer {
        private static string[] converterTypeNames;
        private static string[] converterAssemblyQualifiedNames;
        private float height;

        static PropertyBindingDrawer() {
            List<System.Type> converterList = ReflectionUtility.GetClassTypeFromAssembly(typeof(PropertyConverter));
            converterList.Insert(0, null);
            converterTypeNames = new string[converterList.Count];
            converterAssemblyQualifiedNames = new string[converterList.Count];
            for (int i = 1; i < converterList.Count; i++) {
                converterTypeNames[i] = converterList[i].Name;
                converterAssemblyQualifiedNames[i] = converterList[i].AssemblyQualifiedName;
            }
            converterTypeNames[0] = "None";
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return height;
        }

        public void DrawPropertyName(Rect rect, SerializedProperty property) {
            float switchButtonWidth = rect.height;
            Rect rectContent = new Rect(rect.x, rect.y, rect.width - switchButtonWidth, rect.height);
            Rect rectButton = new Rect(rect.x + rectContent.width, rect.y, switchButtonWidth, rect.height);
            if (property.isExpanded) {
                EditorGUI.PropertyField(rectContent, property);
            } else {
                SerializedProperty modelUniqueId = property.serializedObject.FindProperty("modelUniqueId");
                int selected = ViewModelBindingEditor.Instance.IndexOfModelProperty(modelUniqueId.stringValue);
                if (selected == -1) {
                    Color oldColor = GUI.color;
                    GUI.color = Color.red;
                    EditorGUI.LabelField(rectContent, property.displayName, "Not Propertys", EditorStyles.popup);
                    GUI.color = oldColor;
                } else {
                    List<string> propertyList = ViewModelBindingEditor.Instance.GetModelPropertyList(selected);
                    string[] fieldNames = propertyList.ToArray();
                    selected = propertyList.FindIndex((element) => { return element == property.stringValue; });
                    int newSelected = EditorGUI.Popup(rectContent, property.displayName, selected, fieldNames);
                    if (selected != newSelected) {
                        property.stringValue = fieldNames[newSelected];
                    }
                }
            }
            property.isExpanded = EditorGUI.Toggle(rectButton, property.isExpanded, EditorStyles.radioButton);
        }

        protected void DrawConverterType(Rect rect, SerializedProperty property, string[] stringValues, string[] displayNames) {
            int index = ArrayUtility.FindIndex(stringValues, (item) => { return item == property.stringValue; });
            int newIndex = EditorGUI.Popup(rect, property.displayName, index, displayNames);
            if (newIndex != index)
                property.stringValue = stringValues[newIndex];
        }

        protected virtual bool DrawProperty(Rect rect, SerializedProperty childProperty) {
            if (childProperty.propertyPath.EndsWith("propertyName")) {
                DrawPropertyName(rect, childProperty);
            } else if (childProperty.propertyPath.EndsWith("converterType")) {
                DrawConverterType(rect, childProperty, converterAssemblyQualifiedNames, converterTypeNames);
            } else {
                EditorGUI.PropertyField(rect, childProperty);
            }
            return true;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
            height = 0.0f;
            foreach (SerializedProperty childProperty in property) {
                Rect rect = new Rect(position.x, position.y + height, position.width, EditorGUIUtility.singleLineHeight);
                if (DrawProperty(rect, childProperty))
                    height += EditorGUIUtility.singleLineHeight;
            }
            EditorGUI.EndProperty();
        }
    }
}