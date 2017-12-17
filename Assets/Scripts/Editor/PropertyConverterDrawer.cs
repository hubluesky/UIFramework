using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VBM;
using VBM.Reflection;

namespace VBMEditor {
    // [CustomPropertyDrawer(typeof(PropertyConverter), true)]
    public class PropertyConverterDrawer : PropertyDrawer {

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {
            float labelWidth = EditorGUIUtility.labelWidth;
            EditorGUI.PrefixLabel(new Rect(rect.x, rect.y, labelWidth, rect.height), new GUIContent(property.displayName));
            string typeName = property.objectReferenceValue == null ? null : property.objectReferenceValue.GetType().Name;
            if (GUI.Button(new Rect(rect.x + labelWidth, rect.y, rect.width - labelWidth, rect.height), typeName, EditorStyles.popup)) {
                SelectedConverterMene(property);
            }
        }

        public static void SelectedConverterMene(SerializedProperty property) {
            GenericMenu menu = new GenericMenu();
            List<System.Type> list = ReflectionUtility.GetClassTypeFromAssembly(typeof(PropertyConverter));
            foreach (System.Type type in list) {
                menu.AddItem(new GUIContent(type.FullName), false, () => {
                    property.objectReferenceValue = ScriptableObject.CreateInstance(type);
                    property.serializedObject.ApplyModifiedProperties();
                });
            }
            menu.ShowAsContext();
        }
    }
}