using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VBM;
using VBM.Reflection;

namespace VBMEditor {
    [CustomPropertyDrawer(typeof(PropertyBinding), true)]
    public class PropertyBindingDrawer : PropertyDrawer {
        private float height;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            if (height == 0 && property.objectReferenceValue != null) {
                SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
                SerializedProperty iteratorProperty = serializedObject.GetIterator();
                iteratorProperty.NextVisible(true);
                while (iteratorProperty.NextVisible(false)) {
                    height += EditorGUIUtility.singleLineHeight;
                }
            }
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
            if (property.objectReferenceValue == null)
                return;
            SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
            SerializedProperty iteratorProperty = serializedObject.GetIterator();
            iteratorProperty.NextVisible(true);
            height = 0.0f;
            while (iteratorProperty.NextVisible(false)) {
                EditorGUI.PropertyField(new Rect(position.x, position.y + height, position.width, EditorGUIUtility.singleLineHeight), iteratorProperty);
                height += EditorGUIUtility.singleLineHeight;
            }
            EditorGUI.EndProperty();
        }
    }
}