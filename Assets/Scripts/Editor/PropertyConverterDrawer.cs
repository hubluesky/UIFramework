using System.Collections.Generic;
using GeneralEditor;
using UnityEditor;
using UnityEngine;
using VBM;
using VBM.Reflection;

namespace VBMEditor {
    [GeneralEditor.CustomDrawer(typeof(PropertyConverter), true)]
    public class PropertyConverterDrawer : GeneralEditor.PropertyDrawer {

        public static void SelectedConverterMene(GeneralEditor.SerializedProperty property) {
            GenericMenu menu = new GenericMenu();
            List<System.Type> list = ReflectionUtility.GetClassTypeFromAssembly(typeof(PropertyConverter));
            foreach (System.Type type in list) {
                menu.AddItem(new GUIContent(type.FullName), false, () => {
                    property.PropertyValue = System.Activator.CreateInstance(type);
                });
            }
            menu.ShowAsContext();
        }

        public override void OnGUI(GeneralEditor.SerializedProperty property, GUIContent label, params GUILayoutOption[] options) {
            string typeName = property.PropertyType.Name;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(label);
            if (GUILayout.Button(typeName, EditorStyles.popup)) {
                SelectedConverterMene(property);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}