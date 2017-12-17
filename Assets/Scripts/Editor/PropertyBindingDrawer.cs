using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VBM;
using VBM.Reflection;

namespace VBMEditor {
    [CustomPropertyDrawer(typeof(PropertyBinding))]
    public class PropertyBindingDrawer : PropertyDrawer {
        private float height;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return height;
        }

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {
            height = 0.0f;
            EditorGUI.BeginProperty(rect, label, property);
            SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
            SerializedProperty iteratorProperty = serializedObject.GetIterator();
            EditorGUI.BeginChangeCheck();
            iteratorProperty.NextVisible(true);
            while (iteratorProperty.NextVisible(false)) {
                EditorGUI.PropertyField(new Rect(rect.x, rect.y + height, rect.width, EditorGUIUtility.singleLineHeight), iteratorProperty);
                height += EditorGUIUtility.singleLineHeight;
            }
            if (EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
            }
            EditorGUI.EndProperty();
        }
    }
}