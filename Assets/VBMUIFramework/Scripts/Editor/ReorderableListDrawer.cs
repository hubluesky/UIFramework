using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using VBM;

namespace VBMEditor {
    [CustomPropertyDrawer(typeof(ListDrawerAttribute), true)]
    public class ReorderableListDrawer : PropertyDrawer {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {
            ReorderableList reorderableList = new ReorderableList(property.serializedObject, property);
            reorderableList.DoList(rect);
        }
    }
}