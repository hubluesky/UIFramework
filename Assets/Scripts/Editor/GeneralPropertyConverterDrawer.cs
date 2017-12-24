using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VBM;

namespace VBMEditor {
    [CustomPropertyDrawer(typeof(GeneralPropertyConverter))]
    public class GeneralPropertyConverterDrawer  {
        private System.Type[] generalTypeList;
        private string[] generalStringList;

        public GeneralPropertyConverterDrawer() {
            generalTypeList = new System.Type[] {
                typeof(string),
                typeof(decimal),
                typeof(bool),
                typeof(char),
                typeof(byte),
                typeof(sbyte),
                typeof(short),
                typeof(ushort),
                typeof(int),
                typeof(uint),
                typeof(long),
                typeof(ulong),
                typeof(float),
                typeof(double),
            };
            generalStringList = new string[generalTypeList.Length];
            for (int i = 0; i < generalStringList.Length; i++) {
                generalStringList[i] = generalTypeList[i].Name;
            }
        }

        // public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {
        //     float labelWidth = EditorGUIUtility.labelWidth;
        //     EditorGUI.PrefixLabel(new Rect(rect.x, rect.y, labelWidth, rect.height), new GUIContent(property.displayName));
        //     string typeName = property.objectReferenceValue.GetType().Name;
        //     if (GUI.Button(new Rect(rect.x + labelWidth, rect.y, (rect.width - labelWidth) * 0.6f, rect.height), typeName, EditorStyles.popup)) {
        //         PropertyConverterDrawer.SelectedConverterMene(property);
        //     }

        //     SerializedObject serializedObject = new SerializedObject(property.objectReferenceValue);
        //     SerializedProperty typeNameProperty = serializedObject.FindProperty("typeName");
        //     int selected = System.Array.FindIndex(generalTypeList, (element) => { return element.AssemblyQualifiedName == typeNameProperty.stringValue; });
        //     rect = new Rect(rect.x + labelWidth + (rect.width - labelWidth) * 0.6f, rect.y, (rect.width - labelWidth) * 0.4f, rect.height);
        //     int newSelected = EditorGUI.Popup(rect, selected, generalStringList);
        //     if (newSelected != selected) {
        //         typeNameProperty.stringValue = generalTypeList[newSelected].AssemblyQualifiedName;
        //         serializedObject.ApplyModifiedProperties();
        //     }
        // }
    }
}