using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VBM;
using VBM.Reflection;

namespace VBMEditor {
    [CustomPropertyDrawer(typeof(SerializableEnum))]
    public class SerializableEnumDrawer : PropertyDrawer {
        private static readonly List<System.Type> enumTypeList = new List<System.Type>();

        static SerializableEnumDrawer() {
            ReflectionUtility.ForeachClassTypeFromAssembly((type) => {
                if (type.IsEnum && type.IsPublic) {
                    enumTypeList.Add(type);
                }
                return true;
            });

        }

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {
            SerializedProperty enumTypeProperty = property.FindPropertyRelative("enumType");
            SerializedProperty enumValueProperty = property.FindPropertyRelative("enumValue");

            float switchButtonWidth = rect.height;
            Rect rectContent = new Rect(rect.x, rect.y, rect.width - switchButtonWidth, rect.height);
            Rect rectButton = new Rect(rect.x + rectContent.width, rect.y, switchButtonWidth, rect.height);

            System.Type enumType = System.Type.GetType(enumTypeProperty.stringValue);
            if (string.IsNullOrEmpty(enumTypeProperty.stringValue) || property.isExpanded) {
                EditorGUI.BeginChangeCheck();
                rectContent = EditorGUI.PrefixLabel(rectContent, label);
                if(GUI.Button(rectContent, enumType != null ? enumType.Name : null)) {
                    ShowEnumMenu(rectContent, enumTypeProperty);
                }
            } else {
                EditorGUI.BeginChangeCheck();
                System.Enum enumValue;
                try {
                    enumValue = System.Enum.Parse(enumType, enumValueProperty.stringValue, true) as System.Enum;
                } catch(System.Exception) {
                    enumValue = System.Enum.GetValues(enumType).GetValue(0) as System.Enum;
                    enumValueProperty.stringValue = System.Enum.GetName(enumType, enumValue);
                    enumTypeProperty.serializedObject.ApplyModifiedProperties();
                }
                enumValue = EditorGUI.EnumPopup(rectContent, label, enumValue);
                if(EditorGUI.EndChangeCheck()) {
                    enumValueProperty.stringValue = System.Enum.GetName(enumType, enumValue);
                    enumTypeProperty.serializedObject.ApplyModifiedProperties();
                }
            }

            EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(enumTypeProperty.stringValue));
            property.isExpanded = EditorGUI.Toggle(rectButton, property.isExpanded, EditorStyles.radioButton);
            EditorGUI.EndDisabledGroup();
        }

        private void ShowEnumMenu(Rect rect, SerializedProperty enumTypeProperty) {
            GenericMenu enumTypeMenu = new GenericMenu();
            foreach(System.Type enumType in enumTypeList) {
                enumTypeMenu.AddItem(new GUIContent(enumType.FullName.Replace('.', '/')), false, ()=>{
                    enumTypeProperty.stringValue = enumType.AssemblyQualifiedName;
                    enumTypeProperty.serializedObject.ApplyModifiedProperties();
                });   
            }
            enumTypeMenu.DropDown(rect);
        }
    }
}