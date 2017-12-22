using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VBM;
using VBM.Reflection;

namespace VBMEditor {
    [CustomPropertyDrawer(typeof(PropertyToEnumDrawerAttribute))]
    public class PropertyEnumPropertyDrawer : PropertyDrawer {
        class Content {
            public string[] enumValues;
            public Dictionary<string, string[]> propertyEnumMap;
        }

        private static Dictionary<Type, Content> enumMap = new Dictionary<Type, Content>();

        static PropertyEnumPropertyDrawer() {
            ReflectionUtility.ForeachClassTypeFromAssembly((type) => {
                if (type.IsEnum && type.IsDefined(typeof(PropertyToEnumAttribute), false)) {
                    string[] names = Enum.GetNames(type);
                    if (names.Length > 0) {
                        PropertyToEnumAttribute attribute = Attribute.GetCustomAttribute(type, typeof(PropertyToEnumAttribute)) as PropertyToEnumAttribute;

                        Content content;
                        if (!enumMap.TryGetValue(attribute.classType, out content)) {
                            content = new Content();
                            enumMap.Add(attribute.classType, content);
                        }

                        if (attribute.propertyName == null) {
                            content.enumValues = new string[names.Length];
                            Array.Copy(names, content.enumValues, content.enumValues.Length);
                        } else {
                            if (content.propertyEnumMap == null)
                                content.propertyEnumMap = new Dictionary<string, string[]>();
                            if (!content.propertyEnumMap.ContainsKey(attribute.propertyName)) {
                                string[] enumValues = new string[names.Length];
                                Array.Copy(names, enumValues, enumValues.Length);
                                content.propertyEnumMap.Add(attribute.propertyName, enumValues);
                            }
                        }
                    }
                }
                return true;
            });
        }

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {
            float switchButtonWidth = rect.height;
            Rect rectContent = new Rect(rect.x, rect.y, rect.width - switchButtonWidth, rect.height);
            Rect rectButton = new Rect(rect.x + rectContent.width, rect.y, switchButtonWidth, rect.height);

            if (property.isExpanded) {
                EditorGUI.PropertyField(rectContent, property, label);
            } else {
                if (!enumMap.ContainsKey(fieldInfo.DeclaringType))
                    EditorGUI.TextField(rectContent, label, "Not define StringAndEnumAttribute");
                else {
                    string[] enumValues;
                    Content content = enumMap[fieldInfo.DeclaringType];
                    if (content.propertyEnumMap != null && content.propertyEnumMap.ContainsKey(fieldInfo.Name)) {
                        enumValues = content.propertyEnumMap[fieldInfo.Name];
                    } else {
                        enumValues = content.enumValues;
                    }

                    if (fieldInfo.FieldType == typeof(int)) {
                        property.intValue = EditorGUI.Popup(rectContent, label.text, property.intValue, enumValues);
                    } else if (fieldInfo.FieldType == typeof(string)) {
                        int index = ArrayUtility.FindIndex(enumValues, (item) => { return item == property.stringValue; });
                        int newIndex = EditorGUI.Popup(rectContent, label.text, index, enumValues);
                        if (index != newIndex)
                            property.stringValue = enumValues[newIndex];
                    } else {
                        EditorGUI.TextField(rectContent, label, "Only suppurt int and string to enum");
                    }
                }
            }
            property.isExpanded = EditorGUI.Toggle(rectButton, property.isExpanded, EditorStyles.radioButton);
        }
    }
}