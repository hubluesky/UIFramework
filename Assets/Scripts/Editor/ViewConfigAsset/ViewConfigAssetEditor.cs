using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using VBM;

namespace VBMEditor {
    [CustomEditor(typeof(ViewConfigAsset), true), CanEditMultipleObjects]
    public class ViewConfigAssetEditor : Editor {
        private ReorderableList list;

        void OnEnable() {
            SerializedProperty configs = serializedObject.FindProperty("viewConfigs");
            list = new ReorderableList(serializedObject, configs, true, true, true, true);

            list.drawElementCallback += DrawElement;
            list.elementHeightCallback += ElementHeight;
        }

        private float ElementHeight(int index) {
            SerializedProperty childProperty = list.serializedProperty.GetArrayElementAtIndex(index);
            return EditorGUI.GetPropertyHeight(childProperty, null);
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused) {
            EditorGUI.indentLevel++;
            SerializedProperty childProperty = list.serializedProperty.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, childProperty, true);
            EditorGUI.indentLevel--;
        }

        public override void OnInspectorGUI() {
            list.DoLayoutList();
        }
    }
}