using System.Collections.Generic;
using GeneralEditor;
using UnityEditor;
using UnityEngine;
using VBM;
using VBM.Reflection;

namespace VBMEditor {
    [CustomDrawerAttribute(typeof(PropertyBinding), true)]
    public class PropertyBindingDrawer : GeneralEditor.PropertyDrawer {

        public override void OnGUI(GeneralEditor.SerializedProperty property, GUIContent label, params GUILayoutOption[] options) {
            EditorGUILayout.BeginVertical();
            var iterProperty = property.GetEnumerator();
            while (iterProperty.MoveNext()) {
                GeneralEditor.PropertyDrawerMgr.PropertyField(iterProperty.Current, new GUIContent(iterProperty.Current.DisplayName), options);
            }
            EditorGUILayout.EndVertical();
        }
    }
}